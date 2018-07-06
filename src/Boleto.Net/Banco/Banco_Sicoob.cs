using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using BoletoNet.Util;

[assembly: WebResource("BoletoNet.Imagens.756.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao Bancoob Sicoob Crédibom.
    /// Autor: Janiel Madureira Oliveira
    /// E-mail: janielbelmont@msn.com
    /// Twitter: @janiel14
    /// Data: 01/02/2012
    /// Obs: Os arquivo de remessa CNAB 400 foi implementado para cobranças com registros seguindo o padrão CBR641.
    /// 
    /// Atualização
    /// Mudança no logotipo para o SICOOB NO GERAL
    /// Criação de rotinas ausentes
    /// Autor: Adriano trentim Augusto
    /// E-mail: adriano@setrin.com.br
    /// Data: 25/02/2014
    /// </summary>
    internal sealed class Banco_Sicoob : AbstractBanco, IBanco
    {
        internal Banco_Sicoob()
        {
            Nome = "Sicoob";
            Codigo = 756;
            Digito = "0";
        }

        #region FORMATAÇÕES

        public override void FormataNossoNumero(Boleto boleto)
        {
            if (!string.IsNullOrEmpty(boleto.DigitoNossoNumero))
            {
                //boleto.NossoNumero = boleto.NossoNumero + "-" + boleto.DigitoNossoNumero;
                return;
            }

            var resultado = 0;
            var dv = 0;
            var resto = 0;
            var constante = "319731973197319731973";
            var cooperativa = boleto.Cedente.ContaBancaria.Agencia;
            var codigo = boleto.Cedente.Codigo + boleto.Cedente.DigitoCedente;
            var nossoNumero = boleto.NossoNumero;

            var seqValidacao = new StringBuilder();
            seqValidacao.Append(cooperativa.PadLeft(4, '0'));
            seqValidacao.Append(codigo.PadLeft(10, '0'));
            seqValidacao.Append(nossoNumero.PadLeft(7, '0').Truncate(7));

            //Multiplicando cada posição por sua respectiva posição na constante.
            for (var i = 0; i < 21; i++)
                resultado = resultado + (Convert.ToInt16(seqValidacao.ToString().Substring(i, 1)) * Convert.ToInt16(constante.Substring(i, 1)));
            
            //Calculando mod 11
            resto = resultado % 11;

            //Verifica resto
            if (resto == 1 || resto == 0)
                dv = 0;
            else
                dv = 11 - resto;
            
            //Montando nosso número
            //boleto.NossoNumero = boleto.NossoNumero + "-" + dv;
            boleto.DigitoNossoNumero = dv.ToString();
        }

        private void FormataCodigoCliente(Cedente cedente)
        {
            if (cedente.Codigo.Length == 7)
            {
                cedente.DigitoCedente = Convert.ToInt32(cedente.Codigo.Substring(6));
                cedente.Codigo = cedente.Codigo.Substring(0, 6).PadLeft(6, '0');
            }
            else
            {
                cedente.Codigo = cedente.Codigo.PadLeft(6, '0');
            }
        }

        private void FormataCodigoCliente(Boleto boleto)
        {
            if (boleto.Cedente.Codigo.Length == 7)
            {
                boleto.Cedente.DigitoCedente = Convert.ToInt32(boleto.Cedente.Codigo.Substring(6));
                boleto.Cedente.Codigo = boleto.Cedente.Codigo.Substring(0, 6).PadLeft(6, '0');
            }
            else
            {
                boleto.Cedente.Codigo = boleto.Cedente.Codigo.PadLeft(6, '0');
            }
        }

        private string FormataNumeroTitulo(Boleto boleto)
        {
            var novoTitulo = new StringBuilder();
            novoTitulo.Append(boleto.NossoNumero.Replace("-", "").PadLeft(8, '0'));
            return novoTitulo.ToString();
        }

        private string FormataNumeroParcela(Boleto boleto)
        {
            if (boleto.NumeroParcela <= 0)
                boleto.NumeroParcela = 1;

            return boleto.NumeroParcela.ToString().PadLeft(3, '0');
        }

        public static long FatorVencimento2000(Boleto boleto)
        {
            var dateBase = new DateTime(2000, 7, 3, 0, 0, 0);

            //Verifica se a data esta dentro do range utilizavel
            var dataAtual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var rangeUtilizavel = Utils.DateDiff(DateInterval.Day, dataAtual, boleto.DataVencimento);

            if (rangeUtilizavel > 5500 || rangeUtilizavel < -3000)
                throw new Exception("Data do vencimento fora do range de utilização proposto pela CENEGESC. Comunicado FEBRABAN de n° 082/2012 de 14/06/2012");

            while (boleto.DataVencimento > dateBase.AddDays(9999))
                dateBase = boleto.DataVencimento.AddDays(-(((Utils.DateDiff(DateInterval.Day, dateBase, boleto.DataVencimento) - 9999) - 1) + 1000));

            return Utils.DateDiff(DateInterval.Day, dateBase, boleto.DataVencimento) + 1000;
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            var peso = 2;
            var soma = 0;
            var resultado = 0;
            var dv = 0;

            var codigoValidacao = "7569" + FatorVencimento(boleto) +
                Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10) + "1" +
                boleto.Cedente.ContaBancaria.Agencia.PadLeft(4, '0') + "01" + boleto.Cedente.Codigo.PadLeft(6, '0') + 
                boleto.Cedente.DigitoCedente + boleto.NossoNumero.PadLeft(7, '0') + boleto.DigitoNossoNumero.PadLeft(1, '0') + FormataNumeroParcela(boleto);

            for (var i = codigoValidacao.Length - 1; i >= 0; i--)
            {
                var res = Convert.ToInt16(codigoValidacao.Substring(i, 1)) * peso;
                soma += res;
                peso++;

                //Verifica peso
                if (peso > 9)
                    peso = 2;
            }

            resultado = soma % 11;
            dv = 11 - resultado;

            if (dv == 0 || dv == 1 || dv > 9)
                dv = 1;

            boleto.CodigoBarra.Codigo = codigoValidacao.Substring(0, 4) + dv + codigoValidacao.Substring(4);
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função ainda não implementada.");
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            var indice = "1212121212";
            var soma = 0;
            var temp = 0;

            //Campo 1.
            //AAABC.DDDDE
            //A = Código do banco, 756
            //B = Código da moeda, 9
            //C = Carteira, 1
            //D = Código da agência, XXXX
            //E = Dígito verificador desses campos, X
            var campo1 = boleto.Banco.Codigo.ToString().PadLeft(3, '0') + "91" + boleto.Cedente.ContaBancaria.Agencia.PadLeft(4, '0');

            for (var i = 0; i < campo1.Length; i++)
            {
                temp = Convert.ToInt16(campo1.Substring(i, 1)) * Convert.ToInt16(indice.Substring(i + 1, 1));
                
                //Verifica se resultado é igual ou superior a 10.
                if (temp >= 10)
                    temp = Convert.ToInt16(temp.ToString().Substring(0, 1)) + Convert.ToInt16(temp.ToString().Substring(1, 1));

                soma = soma + temp;
            }

            var linhaDigitavel = new StringBuilder();
            linhaDigitavel.Append($"{campo1.Substring(0, 5)}.{campo1.Substring(5, 4)}{Multiplo10(soma)} ");

            soma = 0;

            //Campo 2.
            //FFGGG.GGGGHI
            //F = Código da modalidade, 01.
            //G = Código do beneficiário/cliente.
            //H = Nosso número, apenas o primeiro dígito.
            //I = Dígito verificador deste campo, calculado aqui.
            var campo2 = boleto.CodigoBarra.Codigo.Substring(24, 10);

            for (var i = 0; i < campo2.Length; i++)
            {
                temp = (Convert.ToInt16(campo2.Substring(i, 1)) * Convert.ToInt16(indice.Substring(i, 1)));
                
                //Verifica se resultado é igual ou superior a 10
                if (temp >= 10)
                    temp = Convert.ToInt16(temp.ToString().Substring(0, 1)) + Convert.ToInt16(temp.ToString().Substring(1, 1));
                
                soma = soma + temp;
            }

            linhaDigitavel.Append($"{campo2.Substring(0, 5)}.{campo2.Substring(5, 5)}{Multiplo10(soma)} ");

            soma = 0;

            //Campo 3.
            //HHHHH.HHJJJK
            //H = Nosso número do boleto.
            //J = Número da parcela, normalmente 001.
            //K = Dígito verificador, calculado aqui.
            var campo3 = boleto.CodigoBarra.Codigo.Substring(34, 10);

            for (var i = 0; i < campo3.Length; i++)
            {
                temp = (Convert.ToInt16(campo3.Substring(i, 1)) * Convert.ToInt16(indice.Substring(i, 1)));
                
                //Verifica se resultado é igual ou superior a 10
                if (temp >= 10)
                    temp = Convert.ToInt16(temp.ToString().Substring(0, 1)) + Convert.ToInt16(temp.ToString().Substring(1, 1));
                
                soma = soma + temp;
            }

            linhaDigitavel.Append(campo3.Substring(0, 5) + "." + campo3.Substring(5, 5) + Multiplo10(soma) + " ");

            //Campo 4.
            //L
            //L = Dígito verificador do código de barras.
            var campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);
            linhaDigitavel.Append(campo4 + " ");

            //Campo 5.
            //MMMMNNNNNNNNNN
            //M = Fator de vencimento.
            //N = Valor do boleto.
            var campo5 = FatorVencimento(boleto) + Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);
            linhaDigitavel.Append(campo5);
            boleto.CodigoBarra.LinhaDigitavel = linhaDigitavel.ToString();
        }

        #endregion

        public override void ValidaBoleto(Boleto boleto)
        {
            if (boleto.Instrucoes.Count(w => w.Codigo > 0) > 2)
                throw new Exception("Para o Sicoob, só é permitido selecionar 2 instruções.");

            //Atribui o nome do banco ao local de pagamento.
            boleto.LocalPagamento += Nome + "";
            
            //Verifica se data do processamento é valida.
            if (boleto.DataProcessamento == DateTime.MinValue)
                boleto.DataProcessamento = DateTime.Now;
            
            //Verifica se data do documento é valida.
            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            //Atribui o nome do banco ao local de pagamento.
            boleto.LocalPagamento = "Pagável Preferencialmente nas Cooperativas da Rede Sicoob ou Qualquer Banco até o Vencimento";
            boleto.TipoModalidade = "0" + boleto.Carteira.Trim('0');

            //Aplicando formatações.
            FormataCodigoCliente(boleto);
            FormataNossoNumero(boleto);
            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
        }
        
        #region ARQUIVO DE REMESSA

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada. Não é usada por esse banco.");
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                FormataCodigoCliente(cedente);

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                var header = " ";
                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        header = GerarHeaderRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Cnab400:
                        header = GerarHeaderRemessaCnab400(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                var header = " ";

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        header = GerarHeaderLoteRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Cnab400:
                        //Não tem no CNAB 400 header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var detalhe = " ";

                FormataNossoNumero(boleto);

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        //detalhe = GerarDetalheRemessaCNAB240(boleto, numeroRegistro, tipoArquivo);
                        //detalhe = new ArquivoRemessaCNAB240().GerarArquivoRemessa(null, boleto.Banco, boleto.Cedente, null);
                        break;
                    case TipoArquivo.Cnab400:
                        detalhe = GerarDetalheRemessaCnab400(boleto, numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do DETALHE do arquivo de REMESSA.", ex);
            }
        }
        
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                var trailer = " ";

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        //trailer = GerarTrailerRemessa240(numeroRegistro);
                        break;
                    case TipoArquivo.Cnab400:
                        trailer = GerarTrailerRemessa400(numeroRegistro);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar TRAILER do arquivo de remessa do CNAB400.", ex);
            }
        }


        private string GerarHeaderRemessaCnab400(Cedente cedente, int numeroArquivoRemessa)
        {
            var header = new StringBuilder();
            
            try
            {
                header.Append("0"); //Posição 001
                header.Append("1"); //Posição 002
                header.Append("REMESSA"); //Posição 003 a 009
                header.Append("01"); //Posição 010 a 011
                header.Append("COBRANÇA"); //Posição 012 a 019
                header.Append(new string(' ', 7)); //Posição 020 a 026
                header.Append(Utils.FitStringLength(cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true)); //Posição 027 a 030
                header.Append(Utils.FitStringLength(cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true)); //Posição 031
                header.Append(Utils.FitStringLength(cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true)); //Posição 032 a 039
                header.Append(Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true)); //Posição 40
                header.Append(new string(' ', 6)); //Posição 041 a 046
                header.Append(Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false)); //Posição 047 a 076
                header.Append(Utils.FitStringLength("756BANCOOBCED", 18, 18, ' ', 0, true, true, false)); //Posição 077 a 094
                header.Append(DateTime.Now.ToString("ddMMyy")); //Posição 095 a 100
                header.Append(Utils.FitStringLength(Convert.ToString(numeroArquivoRemessa), 7, 7, '0', 0, true, true, true)); //Posição 101 a 107
                header.Append(new string(' ', 287)); //Posição 108 a 394
                header.Append("000001"); //Posição 395 a 400

                return header.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        private string GerarDetalheRemessaCnab400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            var detalhe = new StringBuilder();

            try
            {
                detalhe.Append("1"); //001
                detalhe.Append(Utils.IdentificaTipoInscricaoSacado(boleto.Cedente.CpfCnpj)); //002 a 003 - Identifica se for CPF ou CNPJ.
                detalhe.Append(Utils.FitStringLength(boleto.Cedente.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""), 14, 14, '0', 0, true, true, true)); //004 a 017
                detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true)); //018 a 021
                detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true)); //022
                detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 8, 8, '0', 0, true, true, true)); //023 a 030
                detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true)); //031
                detalhe.Append(new string('0', 6)); //Posição 032 a 037, Convênio
                detalhe.Append(Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false)); //038 a 62
                detalhe.Append(Utils.FitStringLength(boleto.NossoNumero + boleto.DigitoNossoNumero, 12, 12, '0', 0, true, true, true)); //063 a 074
                detalhe.Append(Utils.FitStringLength(boleto.NumeroParcela.ToString(), 2, 2, '0', 0, true, true, true)); //075 a 076
                detalhe.Append("00");       //077 a 078
                detalhe.Append("   ");      //079 a 081
                detalhe.Append(" ");        //082
                detalhe.Append("   ");      //083 a 085
                detalhe.Append("000");      //086 a 088
                detalhe.Append("0");        //089
                detalhe.Append("00000");    //090 a 094
                detalhe.Append("0");        //095
                detalhe.Append(Utils.FitStringLength(boleto.Cedente.NumeroBordero.ToString(), 6, 6, '0', 0, true, true, true)); //096 a 101
                detalhe.Append(new string(' ', 4)); //102 a 105

                //Tipo de Emissão: 1 - Cooperativa 2 - Cliente.
                detalhe.Append(Utils.FitStringLength(boleto.Postagem ? "1" : "2", 1, 1, '0', 0, true, true, true)); //106 a 106

                detalhe.Append(Utils.FitStringLength(boleto.TipoModalidade, 2, 2, '0', 0, true, true, true));  //107 a 108
                detalhe.Append(Utils.FitStringLength(boleto.Remessa.CodigoOcorrencia, 2, 2, '0', 0, true, true, true)); //109 a 110 - (1)REGISTRO DE TITULOS (2)Solicitação de Baixa
                detalhe.Append(Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, '0', 0, true, true, true)); //111 a 120
                detalhe.Append(boleto.DataVencimento.ToString("ddMMyy")); //Posição 121 a 126
                detalhe.Append(Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 13, 13, '0', 0, true, true, true)); //Posição 127 a 139 
                detalhe.Append(boleto.Banco.Codigo); //Posição 140 a 142
                detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true)); //Posição 143 a 146
                detalhe.Append(Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true)); //Posição 147
                detalhe.Append(Utils.FitStringLength(boleto.EspecieDocumento.Codigo, 2, 2, '0', 0, true, true, true)); //Posição 148 a 149, Espécie é normalmente Dupicata - 01

                detalhe.Append(boleto.Aceite == "N" ? "0" : "1"); //Posição 150
                detalhe.Append(boleto.DataProcessamento.ToString("ddMMyy")); //Posição 151 a 156

                var inst1 = "00";
                var inst2 = "00";

                foreach (var inst in boleto.Instrucoes.Where(w => w.Codigo > 0))
                {
                    //99 = Protesto.
                    var aux = inst.Codigo == 99 ? inst.Dias : inst.Codigo;

                    if (inst1.Equals("00"))
                        inst1 = aux.ToString().PadLeft(2, '0');
                    else
                        inst2 = aux.ToString().PadLeft(2, '0');
                }

                /*
                  01 = COBRAR JUROS
                  03 = PROTESTAR 3 DIAS APÓS VENCIMENTO
                  04 = PROTESTAR 4 DIAS APÓS VENCIMENTO
                  05 = PROTESTAR 5 DIAS APÓS VENCIMENTO
                  10 = PROTESTAR 10 DIAS APÓS VENCIMENTO
                  15 = PROTESTAR 15 DIAS APÓS VENCIMENTO
                  20 = PROTESTAR 20 DIAS APÓS VENCIMENTO
                  07 = NAO PROTESTAR

                  22 = CONCEDER DESCONTO SÓ ATÉ DATA ESTIPULADA
                  42 = DEVOLVER APÓS 15 DIAS VENCIDO
                  43 = DEVOLVER APÓS 30 DIAS VENCIDO"
                 */       

                detalhe.Append(inst1); //Posição 157 a 158 
                detalhe.Append(inst2); //Posição 159 a 160

                detalhe.Append(Utils.FitStringLength(Convert.ToInt32(boleto.PercJurosMora * 10000).ToString(), 6, 6, '0', 1, true, true, true)); //161 a 166
                detalhe.Append(Utils.FitStringLength(Convert.ToInt32(boleto.PercMulta * 10000).ToString(), 6, 6, '0', 1, true, true, true)); //167 a 172
                detalhe.Append(boleto.Postagem ? "1" : "2"); //Posição 173
                detalhe.Append(Utils.FitStringLength((boleto.DataDesconto == DateTime.MinValue ? "0" : boleto.DataDesconto.ToString("ddMMyy")), 6, 6, '0', 0, true, true, true)); //Posição 174 a 179

                detalhe.Append(Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true)); //180 a 192
                detalhe.Append("9" + Utils.FitStringLength(boleto.Iof.ApenasNumeros(), 12, 12, '0', 0, true, true, true)); //193 a 205
                detalhe.Append(Utils.FitStringLength(boleto.Abatimento.ApenasNumeros(), 13, 13, '0', 0, true, true, true)); //206 a 218
                detalhe.Append(Utils.IdentificaTipoInscricaoSacado(boleto.Sacado.CpfCnpj)); //219 a 220
                detalhe.Append(Utils.FitStringLength(boleto.Sacado.CpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""), 14, 14, '0', 0, true, true, true)); //221 a 234
                detalhe.Append(Utils.FitStringLength(boleto.Sacado.Nome, 40, 40, ' ', 0, true, true, false)); //235 a 274
                detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.EndComNumComplemento, 37, 37, ' ', 0, true, true, false)); //275 a 311
                detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.Bairro, 15, 15, ' ', 0, true, true, false)); //312 a 326
                detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.Cep, 8, 8, '0', 0, true, true, true)); //327 a 334
                detalhe.Append(Utils.FitStringLength(boleto.Sacado.Endereco.Cidade, 15, 15, ' ', 0, true, true, false)); //335 a 349
                detalhe.Append(boleto.Sacado.Endereco.Uf); //350 a 351
                detalhe.Append(new string(' ', 40)); //352 a 391 - OBSERVACOES

                var prot = boleto.Instrucoes.FirstOrDefault(f => f.Codigo == 99)?.Dias ?? 0;
                detalhe.Append(prot.ToString().PadLeft(2, '0')); //392 a 393 - DIAS PARA PROTESTO

                detalhe.Append(" "); //394
                detalhe.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); //394 a 400

                return Utils.SubstituiCaracteresEspeciais(detalhe.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo de remessa do CNAB400.", ex);
            }
        }

        private string GerarTrailerRemessa400(int numeroRegistro)
        {
            var trailer = new StringBuilder();
            trailer.Append("9"); //Posição 001
            trailer.Append(new string(' ', 393)); //Posição 002 a 394
            trailer.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); //Posição 395 a 400

            //Retorno
            return Utils.SubstituiCaracteresEspeciais(trailer.ToString());
        }
        

        private string GerarHeaderRemessaCNAB240(Cedente cedente, int numRemessa)
        {
            try
            {
                var h = "756";                                                         //001 a 003  Código do Sicoob na Compensação: "756"
                h += "0000";                                                           //004 a 007  Lote de Serviço: "0000"
                h += "0";                                                              //008        Tipo de Registro: "0"
                h += new string(' ', 9);                                               //009 a 017  Uso Exclusivo FEBRABAN / CNAB: Brancos
                h += cedente.CpfCnpj.Length == 11 ? "1" : "2";                         //018        1=CPF  2=CGC/CNPJ
                h += Utils.FormatCode(cedente.CpfCnpj, "0", 14, true);                 //019 a 032  Número de Inscrição da Empresa.
                h += Utils.FormatCode("", " ", 20, true);                              //033 a 052  Código do Convênio no Sicoob: Brancos
                h += Utils.FormatCode(cedente.ContaBancaria.Agencia, 5);               //053 a 057  Prefixo da Cooperativa: vide planilha "Capa" deste arquivo
                h += Utils.FormatCode(cedente.ContaBancaria.DigitoAgencia, "0", 1);    //058 a 058  Digito Agência
                h += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);     //059 a 070
                h += cedente.ContaBancaria.DigitoConta;                                //071 a 071
                h += new string(' ', 1);                                               //072 a 072  Dígito Verificador da Ag/Conta: Brancos
                h += Utils.FormatCode(cedente.Nome, " ", 30);                          //073 a 102  Nome da Empresa.
                h += Utils.FormatCode("SICOOB", " ", 30);                              //103 a 132  Nome do Banco: SICOOB.
                h += Utils.FormatCode("", " ", 10);                                    //133 a 142  Uso Exclusivo FEBRABAN / CNAB: Brancos.
                h += "1";                                                              //103 a 142  Código Remessa / Retorno: "1".
                h += DateTime.Now.ToString("ddMMyyyy");                                //144 a 151  Data de Geração do Arquivo.
                h += DateTime.Now.ToString("HHmmss");                                  //152 a 157  Hora de Geração do Arquivo.
                h += numRemessa.ToString().PadLeft(6, '0');                            //158 a 163  Seqüência
                h += "081";                                                            //164 a 166  No da Versão do Layout do Arquivo: "081"
                h += "00000";                                                          //167 a 171  Densidade de Gravação do Arquivo: "00000"
                h += Utils.FormatCode("", " ", 69);
                return Utils.SubstituiCaracteresEspeciais(h);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }

        private string GerarHeaderLoteRemessaCNAB240(Cedente ced, int numRemessa)
        {
            try
            {
                var h = "756";                                                        //001 a 003  Código do Sicoob na Compensação: "756"
                h += "0001";                                                          //004 a 007  Número do lote no arquivo
                h += "1";                                                             //008        Tipo de Registro: "1"
                h += "R";                                                             //009        Tipo de Operação: "R"
                h += "01";                                                            //010 a 011  Tipo de Serviço: "01"
                h += new string(' ', 2);                                              //012 a 013  Uso Exclusivo FEBRABAN/CNAB: Brancos
                h += "040";                                                           //014 a 016  Nº da Versão do Layout do Lote: "040"
                h += new string(' ', 1);                                              //017        Uso Exclusivo FEBRABAN/CNAB: Brancos
                h += ced.CpfCnpj.Length == 11 ? "1" : "2";                            //018        1=CPF 2=CGC/CNPJ
                h += Utils.FormatCode(ced.CpfCnpj, "0", 15, true);                    //019 a 033  Número de Inscrição da Empresa
                h += Utils.FormatCode("", " ", 20, true);                             //034 a 053  Código do Convênio no Sicoob: Brancos
                h += Utils.FormatCode(ced.ContaBancaria.Agencia, "0", 5, true);       //054 a 058  Prefixo da Cooperativa: vide planilha "Capa" deste arquivo
                h += Utils.FormatCode(ced.ContaBancaria.DigitoAgencia, "0", 1, true); //059 a 059
                h += Utils.FormatCode(ced.ContaBancaria.Conta, "0", 12, true);        //060 a 071
                h += Utils.FormatCode(ced.ContaBancaria.DigitoConta, "0", 1, true);   //072 a 072
                h += new string(' ', 1);                                              //073        Dígito Verificador da Ag/Conta: Brancos
                h += Utils.FormatCode(ced.Nome, " ", 30);                             //074 a 103  Nome do Banco: SICOOB
                h += Utils.FormatCode("", " ", 40);                                   //104 a 143  Informação 1			
                h += Utils.FormatCode("", " ", 40);                                   //144 a 183  Informação 2
                h += Utils.FormatCode(numRemessa.ToString(), "0", 8, true);           //184 a 191  Número da remessa
                h += DateTime.Now.ToString("ddMMyyyy");                               //192 a 199  Data de Gravação Remessa/Retorno
                h += Utils.FormatCode("", "0", 8, true);                              //200 a 207  Data do Crédito: "00000000"
                h += new string(' ', 33);                                             //208 a 240  Uso Exclusivo FEBRABAN/CNAB: Brancos
                return Utils.SubstituiCaracteresEspeciais(h);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto bol, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                var d = Utils.FormatCode(Codigo.ToString(), 3);                      //001 a 003  Código do Sicoob na Compensação: "756".
                d += Utils.FormatCode("1", "0", 4, true);                            //004 a 007  Número Sequencial do lote.
                d += "3";                                                            //008        Tipo de Registro: "3".
                d += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);      //009 a 013  Número Sequencial dos registros de detalhe no lote.
                d += "P";                                                            //014        Cód. Segmento do Registro Detalhe: "P".
                d += " ";                                                            //015        Uso Exclusivo FEBRABAN/CNAB: Brancos.
                d += Utils.FormatCode(bol.Remessa.CodigoOcorrencia, 2);              //016 a 017  '01': Entrada de Títulos.
                d += Utils.FormatCode(bol.Cedente.ContaBancaria.Agencia, 5);         //018 a 022  Prefixo da Cooperativa: vide planilha "Capa" deste arquivo
                d += Utils.FormatCode(bol.Cedente.ContaBancaria.DigitoAgencia, "0", 1, true);  //023  Dígito Verificador do Prefixo: vide planilha "Capa" deste arquivo
                d += Utils.FormatCode(bol.Cedente.ContaBancaria.Conta, 12);          //024 a 035  Conta Corrente: vide planilha "Capa" deste arquivo
                d += Utils.FormatCode(bol.Cedente.ContaBancaria.DigitoConta, 1);     //036        Dígito Verificador da Conta: vide planilha "Capa" deste arquivo
                d += " ";                                                            //037        Dígito Verificador da Ag/Conta: Brancos
                d += bol.NossoNumero.PadLeft(9, '0') + 
                     bol.DigitoNossoNumero.PadLeft(1, '0') + "01" + 
                     bol.TipoModalidade.PadLeft(2, '0') + 
                     (bol.Postagem ? "3" : "4") + "     ";                           //038 a 057  Nosso Número, parcela, modalidade, tipo de formulário, 5 espaços em branco.
                d += Convert.ToInt16(bol.Carteira) == 1 ? "1" : "2";                 //058        Código da Carteira: vide planilha "Capa" deste arquivo
                d += "0";                                                            //059        Forma de Cadastr. do Título no Banco: "0"
                d += " ";                                                            //060        Tipo de Documento: Brancos.
                d += bol.Postagem ? "1" : "2";                                       //061        Identificação da Emissão do Boleto: 1=Sicoob Emite 2=Beneficiário Emite.
                d += bol.Postagem ? "1" : "2";                                       //062        Identificação da distribuição do Boleto: 1=Sicoob Emite 2=Beneficiário Emite.
                d += Utils.FormatCode(bol.NumeroDocumento.Truncate(15), " ", 15);    //063 a 077  Número do documento de cobrança.
                d += Utils.FormatCode(bol.DataVencimento.ToString("ddMMyyyy"), 8);   //078 a 085  Data do vencimento.
                var valorBoleto = bol.ValorBoleto.SemVirgulaPonto();
                d += valorBoleto.PadLeft(15, '0');                                   //086 a 100  Valor Nominal do Título.
                d += Utils.FormatCode("", "0", 5);                                   //101 a 105  Agência Encarregada da Cobrança: "00000".
                d += new string(' ', 1);                                             //106        Dígito Verificador da Agência: Brancos.
                d += Utils.FormatCode(bol.EspecieDocumento.Codigo, 2);               //107 a 108  Espécie do título.
                d += Utils.FormatCode(bol.Aceite, 1);                                //109        Identificação do título Aceito/Não Aceito.
                d += Utils.FormatCode(bol.DataProcessamento.ToString("ddMMyyyy"), 8);//110 a 117  Data Emissão do Título.

                d += Utils.FormatCode(bol.CodJurosMora, "2", 1);                     //118        Código do juros mora. Padrão: 2 = % ao mês.
                d += Utils.FormatCode(bol.DataVencimento.ToString("ddMMyyyy"), 8);   //119 a 126  Data do Juros de Mora: preencher com a Data de Vencimento do Título.
                var juros = (bol.CodJurosMora == "0" ? 0 : bol.CodJurosMora == "1" ? bol.JurosMora : bol.PercJurosMora).SemVirgulaPonto();
                d += juros.PadLeft(15, '0');                                         //127 a 141  Data do Juros de Mora: R$ ao dia ou % ao mês.

                if (bol.DataDesconto > DateTime.MinValue)
                {
                    d += "1";                                                                   //142        Código do desconto.
                    d += Utils.FormatCode(bol.DataDesconto.ToString("ddMMyyyy"), 8);            //143 a 150  Data do Desconto 1.
                    d += Utils.FormatCode(bol.ValorDesconto.SemVirgulaPonto(), "0", 15, true);  //151 a 165  Valor/Percentual a ser Concedido.
                }
                else
                {
                    d += "0";                                 //142        Código do desconto - Sem Desconto
                    d += Utils.FormatCode("", "0", 8, true);  //143 a 150  Data do Desconto
                    d += Utils.FormatCode("", "0", 15, true); //151 a 165  Valor/Percentual a ser Concedido.
                }

                d += Utils.FormatCode(bol.Iof.SemVirgulaPonto(), "0", 15, true);        //166 a 180  Valor do IOF a ser Recolhido.
                d += Utils.FormatCode(bol.Abatimento.SemVirgulaPonto(), "0", 15, true); //181 a 195  Valor do Abatimento.
                d += Utils.FormatCode(bol.NumeroDocumento.Truncate(25), " ", 25);       //196 a 220  Identificação do título.

                #region Instruções

                var codigoProtesto = "3"; //Código do protesto. 1 = Protestar dias coridos. 3 = Não protestar.
                var prazoProtesto = "00"; //Prazo para protesto. Preencher com 00 caso não protestar.

                foreach (var instrucao in bol.Instrucoes)
                {
                    switch ((EnumInstrucoes_Sicoob)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Sicoob.Protestar:
                            codigoProtesto = instrucao.Dias > 0 ? "1" : "3";
                            prazoProtesto = Utils.FitStringLength(instrucao.Dias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                    }
                }

                #endregion

                d += codigoProtesto;                                        //221        Código do protesto.
                d += Utils.FormatCode(prazoProtesto, 2);                    //222 a 223  Prazo do protesto, em dias.
                d += Utils.FormatCode("0", 1);                              //224        Código para Baixa/Devolução: "0"
                d += Utils.FormatCode("0", 3);                              //225 A 227  Número de Dias para Baixa/Devolução: Brancos
                d += Utils.FormatCode(bol.Moeda.ToString(), "0", 2, true);  //228 A 229  Código da Moeda
                d += Utils.FormatCode("", "0", 10, true);                   //230 A 239  Nº do Contrato da Operação de Créd.: "0000000000"
                d += " ";
                return Utils.SubstituiCaracteresEspeciais(d);
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do SEGMENTO P DO DETALHE do arquivo de REMESSA.", e);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var d = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                    //001 a 003  Código do Sicoob na Compensação: "756"
                d += Utils.FormatCode("1", "0", 4, true);                                     //004 a 007  Número Sequencial do lote.
                d += "3";                                                                     //008        Tipo de Registro: "3"
                d += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true);               //009 a 013  Número Sequencial
                d += "Q";                                                                     //014        Cód. Segmento do Registro Detalhe: "P"
                d += " ";                                                                     //015        Uso Exclusivo FEBRABAN/CNAB: Brancos
                d += Utils.FormatCode(boleto.Remessa.CodigoOcorrencia, 2);                    //016 a 017  '01' = Entrada de Títulos
                d += boleto.Sacado.CpfCnpj.Length == 11 ? "1" : "2";                          //018        1=CPF 2=CGC/CNPJ
                d += Utils.FormatCode(boleto.Sacado.CpfCnpj, "0", 15, true);                  //019 a 033  Número de Inscrição da Empresa
                d += boleto.Sacado.Nome.Truncate(40).PadRight(40);                            //034 a 073  Nome
                d += boleto.Sacado.Endereco.EndComNumComplemento.Truncate(40).PadRight(40);   //074 a 113  Endereço
                d += boleto.Sacado.Endereco.Bairro.Truncate(15).PadRight(15);                 //114 a 128  Bairro 
                d += Utils.FormatCode(boleto.Sacado.Endereco.Cep, 8);                         //129 a 136  CEP (5, N) + Sufixo do CEP (3, N) Total (8, N)
                d += boleto.Sacado.Endereco.Cidade.Truncate(15).PadRight(15);                 //137 a 151  Cidade 
                d += boleto.Sacado.Endereco.Uf.Truncate(2).PadRight(2);                       //152 a 153  Unidade da Federação
                d += boleto.Cedente.CpfCnpj.Length == 11 ? "1" : "2";                         //154        Tipo de Inscrição Sacador avalista
                d += Utils.FormatCode(boleto.Cedente.CpfCnpj, "0", 15, true);                 //155 a 169  Número de Inscrição / Sacador avalista TODO: Precisa disso?
                d += boleto.Cedente.Nome.Truncate(40).PadRight(40);                           //170 a 209  Nome / Sacador avalista
                d += "000";                                                                   //210 a 212  Código Bco. Corresp. na Compensação
                d += Utils.FormatCode("", " ", 20);                                           //213 a 232  Nosso N° no Banco Correspondente "1323739"
                d += Utils.FormatCode("", " ", 8);                                            //233 a 240  Uso Exclusivo FEBRABAN/CNAB
                return Utils.SubstituiCaracteresEspeciais(d).ToUpper();
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", e);
            }
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto bol, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var d = Utils.FormatCode(Codigo.ToString(), 3);                 //001 a 003  Código do Sicoob na Compensação: "756"
                d += Utils.FormatCode("1", "0", 4, true);                       //004 a 007  Número Sequencial do lote.
                d += "3";                                                       //008        Tipo de Registro: "3"
                d += Utils.FormatCode(numeroRegistro.ToString(), "0", 5, true); //009 a 013  Número Sequencial do detalhe dentro do lote.
                d += "R";                                                       //014        Cód. Segmento do Registro Detalhe: "R"
                d += " ";                                                       //015        Uso Exclusivo FEBRABAN/CNAB: Brancos
                d += Utils.FormatCode(bol.Remessa.CodigoOcorrencia, 2);         //016 a 017  Código da remessa,'01' = Entrada de Títulos

                //Desconto 2:
                d += "0";                               //018-018  Código do desconto.
                d += "00000000";                        //019-026  Data do Desconto 2.
                d += Utils.FormatCode("0", "0", 15);    //027-041  Valor do desconto.

                //Desconto 3:
                d += "0";                               //042-042  Código da desconto.
                d += "00000000";                        //043-050  Data do Desconto 3.
                d += Utils.FormatCode("0", "0", 15);    //050-065  Valor do desconto.

                //Multa:
                if (bol.PercMulta > 0)
                {
                    //Código da multa 2 - percentual.
                    d += "2";                                                                //066        Código da multa. 2: Percentual
                    d += Utils.FormatCode(bol.DataVencimento.ToString("ddMMyyyy"), 8);       //067 a 074  Data do Juros de Mora: preencher com a Data de Vencimento do Título.
                    d += Utils.FormatCode(bol.PercMulta.SemVirgulaPonto(), "0", 15, true);   //075 a 089  Percentual da multa.
                }
                else if (bol.ValorMulta > 0)
                {
                    //Código da multa 1 - valor fixo.
                    d += "1";                                                                //066        Código da multa. 1: Valor
                    d += Utils.FormatCode(bol.DataVencimento.ToString("ddMMyyyy"), 8);       //067 a 074  Data do Juros de Mora: preencher com a Data de Vencimento do Título.
                    d += Utils.FormatCode(bol.ValorMulta.SemVirgulaPonto(), "0", 15, true);  //075 a 089  Valor da multa.
                }
                else
                {
                    //Código da multa 0 - sem multa.
                    d += "0";                                                     //066        Código da multa. 1: Valor
                    d += Utils.FormatCode("", "0", 8);                            //067 a 074  Data do Juros de Mora: preencher com a Data de Vencimento do Título.
                    d += Utils.FormatCode(0m.SemVirgulaPonto(), "0", 15, true);   //075 a 089  Valor da multa.
                }

                d += Utils.FormatCode(" ", 10);             //090 a 099  Informação ao Pagador: Brancos.
                d += Utils.FormatCode(" ", 40);             //100 a 139  Informação 3.
                d += Utils.FormatCode(" ", 40);             //140 a 179  Informação 4.

                d += Utils.FormatCode(" ", 20);             //180 a 199  Uso Exclusivo FEBRABAN/CNAB: Brancos.
                d += Utils.FormatCode("", "0", 8, true);    //200 a 207  Cód. Ocor. do Pagador: "00000000".
                d += Utils.FormatCode("", "0", 3, true);    //208 a 210  Cód. do Banco na Conta do Débito: "000".
                d += Utils.FormatCode("", "0", 5, true);    //211 a 215  Código da Agência do Débito: "00000".
                d += " ";                                   //216        Dígito Verificador da Agência: Brancos.
                d += Utils.FormatCode("", "0", 12, true);   //217 a 228  Conta Corrente para Débito: "000000000000".
                d += " ";                                   //229        Verificador da Conta: Brancos.
                d += " ";                                   //230        Verificador Ag/Conta: Brancos.
                d += "0";                                   //231        Aviso para Débito Automático: "0".
                d += Utils.FormatCode("", 9);               //232 a 240  Uso Exclusivo FEBRABAN/CNAB: Brancos
                return Utils.SubstituiCaracteresEspeciais(d);
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", e);
            }
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                var t = Utils.FormatCode(Codigo.ToString(), "0", 3, true);        //001 a 003  Código do banco.
                t += Utils.FormatCode("1", "0", 4, true);                         //004 a 007  Número Lote.
                t += "5";                                                         //008        Tipo do registro: 5.
                t += Utils.FormatCode("", " ", 9);                                //009 a 017  Exclusivo FEBRABAN/CNAB: Brancos.
                t += Utils.FormatCode((numeroRegistro + 1).ToString(), "0", 6, true);   //018 a 023  Quantidade de registros no lote. Header e Footer contam.
                t += Utils.FormatCode("", "0", 6, true);                          //024 a 029  Quantidade de registros em cobrança simples. TODO?
                t += Utils.FormatCode("", "0", 17, true);                         //030 a 046  Valor total de registros em cobrança simples.
                t += Utils.FormatCode("", "0", 6, true);                          //047 a 052  Quantidade de registros em cobrança vinculada.
                t += Utils.FormatCode("", "0", 17, true);                         //053 a 069  Valor total de registros em cobrança vinculada.
                t += Utils.FormatCode("", "0", 6, true);                          //070 a 075  Quantidade de registros em cobrança caucionada.
                t += Utils.FormatCode("", "0", 17, true);                         //076 a 092  Valor total de registros em cobrança caucionada.
                t += Utils.FormatCode("", "0", 6, true);                          //093 a 098  Quantidade de registros em cobrança descontada.
                t += Utils.FormatCode("", "0", 17, true);                         //099 a 115  Valor total de registros em cobrança descontada.
                t += Utils.FormatCode("", "0", 8, true);                          //116 a 123  Número de aviso de lançamento: brancos.
                t += Utils.FormatCode("", " ", 117);                              //124 a 240  Uso exclusivo. Brancos.
                return Utils.SubstituiCaracteresEspeciais(t);
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do LOTE de REMESSA.", e);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numRegistro)
        {
            try
            {
                var t = Utils.FormatCode(Codigo.ToString(), "0", 3, true);   //001 a 003  Código do Banco na compensação.
                t += "9999";                                                 //004 a 007  Numero do lote remessa.
                t += "9";                                                    //008 a 008  Tipo de registro.
                t += Utils.FormatCode("", " ", 9);                           //009 a 017  Reservado (uso Banco).
                t += Utils.FormatCode("1", "0", 6, true);                    //018 a 023  Quantidade de lotes do arquivo.
                t += Utils.FormatCode(numRegistro.ToString(), "0", 6, true); //024 a 029  Quantidade de registros do arquivo.
                t += Utils.FormatCode("", "0", 6, true);                     //030 a 035  Quantidade de contas p/ conc. (lotes).
                t += Utils.FormatCode("", " ", 205);                         //036 a 240  Reservado (uso Banco).
                return Utils.SubstituiCaracteresEspeciais(t);
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do ARQUIVO de REMESSA.", e);
            }
        }

        #endregion

        #region ::. Arquivo de Retorno CNAB400 .::

        /// <summary>
        /// Rotina de retorno de remessa
        /// Criador: Adriano Trentim Augusto
        /// E-mail: adriano@setrin.com.br
        /// Data: 29/04/2014
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                var detalhe = new DetalheRetorno(registro);
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2)); //Tipo de Inscrição Empresa
                detalhe.NumeroInscricao = registro.Substring(3, 14); //Nº Inscrição da Empresa

                //Identificação da Empresa Cedente no Banco
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(22, 8));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(30, 1));

                detalhe.NumeroControle = registro.Substring(37, 25); //Nº Controle do Participante

                //Identificação do Título no Banco
                detalhe.NossoNumero = registro.Substring(62, 11);
                detalhe.DACNossoNumero = registro.Substring(73, 1);

                //TODO: Tá certo isso?
                switch (registro.Substring(106, 2)) // Carteira
                {
                    case "01":
                        detalhe.Carteira = "1";
                        break;
                    case "02":
                        detalhe.Carteira = "1";
                        break;
                    case "03":
                        detalhe.Carteira = "3";
                        break;
                }

                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2)); //Identificação de Ocorrência
                detalhe.DescricaoOcorrencia = this.Ocorrencia(registro.Substring(108, 2)); //Descrição da ocorrência
                detalhe.DataOcorrencia = Utils.ToDateTime(Utils.ToInt32(registro.Substring(110, 6)).ToString("##-##-##")); //Data da ocorrencia

                //Quando ocorrencia = Liquidação, pega a data.
                if (detalhe.CodigoOcorrencia == 5 || detalhe.CodigoOcorrencia == 6 || detalhe.CodigoOcorrencia == 15)
                    detalhe.DataLiquidacao = detalhe.DataOcorrencia;

                detalhe.NumeroDocumento = registro.Substring(116, 10); //Número do Documento
                detalhe.DataVencimento = Utils.ToDateTime(Utils.ToInt32(registro.Substring(146, 6)).ToString("##-##-##")); //Data de Vencimento
                detalhe.ValorTitulo = ((decimal)Convert.ToInt64(registro.Substring(152, 13))) / 100; //Valor do Titulo
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3)); //Banco Cobrador
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 4)); //Agência Cobradora
                detalhe.DACAgenciaCobradora = Utils.ToInt32(registro.Substring(172, 1)); // DV Agencia Cobradora
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2)); //Espécie do Título

                //Data de Crédito - Só vem preenchido quando liquidação
                if (registro.Substring(175, 6) != "000000")
                    detalhe.DataCredito = Utils.ToDateTime(Utils.ToInt32(registro.Substring(175, 6)).ToString("##-##-##"));
                else
                    detalhe.DataCredito = detalhe.DataOcorrencia;

                detalhe.ValorDespesa = ((decimal)Convert.ToUInt64(registro.Substring(181, 7))) / 100; //Valor das Tarifas
                detalhe.ValorOutrasDespesas = ((decimal)Convert.ToUInt64(registro.Substring(188, 13))) / 100; //Valor das Outras Despesas
                detalhe.ValorAbatimento = ((decimal)Convert.ToUInt64(registro.Substring(227, 13))) / 100; //Valor do abatimento
                detalhe.Descontos = ((decimal)Convert.ToUInt64(registro.Substring(240, 13))) / 100; //Valor do desconto
                detalhe.ValorPago = ((decimal)Convert.ToUInt64(registro.Substring(253, 13))) / 100; //Valor do Recebimento
                detalhe.JurosMora = ((decimal)Convert.ToUInt64(registro.Substring(266, 13))) / 100; //Valor de Juros
                detalhe.IdentificacaoTitulo = detalhe.NumeroDocumento; //Identificação do Título no Banco
                detalhe.OutrosCreditos = ((decimal)Convert.ToUInt64(registro.Substring(279, 13))) / 100; //Outros recebimentos

                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        private string Ocorrencia(string codigoOcorrencia)
        {
            switch (codigoOcorrencia)
            {
                case "02":
                    return "Confirmação Entrada Título";
                case "03":
                    return "Comando Recusado";
                case "04":
                    return "Transferência de Carteira - Entrada";
                case "05":
                    return "Liquidação Sem Registro";
                case "06":
                    return "Liquidação Normal";
                case "09":
                    return "Baixa de Título";
                case "10":
                    return "Baixa Solicitada";
                case "11":
                    return "Títulos em Ser";
                case "12":
                    return "Abatimento Concedido";
                case "13":
                    return "Abatimento Cancelado";
                case "14":
                    return "Alteração de Vencimento";
                case "15":
                    return "Liquidação em Cartório";
                case "19":
                    return "Confirmação Instrução Protesto";
                case "20":
                    return "Débito em Conta";
                case "21":
                    return "Alteração de nome do Sacado";
                case "22":
                    return "Alteração de endereço Sacado";
                case "23":
                    return "Encaminhado a Protesto";
                case "24":
                    return "Sustar Protesto";
                case "25":
                    return "Dispensar Juros";
                case "26":
                    return "Instrução Rejeitada";
                case "27":
                    return "Confirmação Alteração Dados";
                case "28":
                    return "Manutenção Título Vencido";
                case "30":
                    return "Alteração Dados Rejeitada";
                case "96":
                    return "Despesas de Protesto";
                case "97":
                    return "Despesas de Sustação de Protesto";
                case "98":
                    return "Despesas de Custas Antecipadas";
                default:
                    return "Ocorrência não cadastrada";
            }
        }

        #endregion

        #region ::. Arquivo de Retorno CNAB240 .::

        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            try
            {
                var detalhe = new DetalheSegmentoTRetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "T")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento T.");

                detalhe.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                detalhe.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                detalhe.Agencia = Convert.ToInt32(registro.Substring(17, 5));
                detalhe.DigitoAgencia = registro.Substring(22, 1);
                detalhe.Conta = Convert.ToInt32(registro.Substring(23, 12));
                detalhe.DigitoConta = registro.Substring(35, 1);

                detalhe.NossoNumeroComDv = registro.Substring(37, 10).TrimStart('0').PadLeft(8, '0');
                detalhe.NossoNumero = detalhe.NossoNumeroComDv.Substring(0, 7);
                detalhe.DigNossoNumero = detalhe.NossoNumeroComDv.Substring(7, 1);
                
                detalhe.CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1));
                detalhe.NumeroDocumento = registro.Substring(58, 15).Trim();

                detalhe.DataVencimento = DateTime.ParseExact(registro.Substring(73, 8), "ddMMyyyy", new CultureInfo("pt-BR"));

                detalhe.ValorTitulo = Convert.ToInt64(registro.Substring(81, 15)) / 100m;
                detalhe.IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                detalhe.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                detalhe.NumeroInscricao = registro.Substring(133, 15);
                detalhe.NomeSacado = registro.Substring(148, 40).Trim();
                detalhe.ValorTarifas = Convert.ToUInt64(registro.Substring(198, 15)) / 100m;
                detalhe.CodigoRejeicao = registro.Substring(213, 10).Trim();
                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO T.", ex);
            }
        }

        public override DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            try
            {
                var detalhe = new DetalheSegmentoURetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "U")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento U.");

                detalhe.CodigoOcorrenciaSacado = registro.Substring(15, 2);
                detalhe.JurosMultaEncargos = Convert.ToUInt64(registro.Substring(17, 15)) / 100m; //18-32, 15 chars. Acréscimos.
                detalhe.ValorDescontoConcedido = Convert.ToUInt64(registro.Substring(32, 15)) / 100m; //33-47, 15 chars. Descontos.
                detalhe.ValorAbatimentoConcedido = Convert.ToUInt64(registro.Substring(47, 15)) / 100m; //48-62, 15 chars. Abatimentos.
                detalhe.ValorIOFRecolhido = Convert.ToUInt64(registro.Substring(62, 15)) / 100m; //63-77, 15 chars. IOF.
                detalhe.ValorPagoPeloSacado = Convert.ToUInt64(registro.Substring(77, 15)) / 100m; //78-92, 15 chars. Valor pago.
                detalhe.ValorLiquidoASerCreditado = Convert.ToUInt64(registro.Substring(92, 15)) / 100m; //93-107, 15 chars. Valor líquido a ser creditado.
                detalhe.ValorOutrasDespesas = Convert.ToUInt64(registro.Substring(107, 15)) / 100m; //108-122, 15 chars. Outras despesas.
                detalhe.ValorOutrosCreditos = Convert.ToUInt64(registro.Substring(122, 15)) / 100m; //123-137, 15 chars, Outros créditos.

                DateTime date;
                DateTime.TryParseExact(registro.Substring(137, 8), "ddMMyyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out date);
                detalhe.DataOcorrencia = date;

                DateTime.TryParseExact(registro.Substring(145, 8), "ddMMyyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out date);
                detalhe.DataCredito = date;

                DateTime.TryParseExact(registro.Substring(157, 8), "ddMMyyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out date);
                detalhe.DataOcorrenciaSacado = date > DateTime.MinValue ? date : DateTime.Now;

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }

        #endregion

        /// <summary>
        /// Efetua as Validações dentro da classe Boleto, para garantir a geração da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            var vRetorno = true;
            var vMsg = string.Empty;
            ////IMPLEMENTACAO PENDENTE...
            mensagem = vMsg;
            return vRetorno;
        }
    }
}