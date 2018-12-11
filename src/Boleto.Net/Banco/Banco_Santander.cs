using System;
using System.Globalization;
using System.Web.UI;
using BoletoNet.Util;
using System.Linq;
using BoletoNet.Excecoes;

[assembly: WebResource("BoletoNet.Imagens.033.jpg", "image/jpg")]
namespace BoletoNet
{
    internal sealed class Banco_Santander : AbstractBanco, IBanco
    {
        internal Banco_Santander()
        {
            Codigo = 033;
            Digito = "7";
            Nome = "Santander";
        }

        internal Banco_Santander(int codigo)
        {
            Codigo = ((codigo != 353) && (codigo != 8)) ? 033 : codigo;
            Digito = "0";
            Nome = "Banco_Santander";
        }

        #region IBanco Members

        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            var vRetorno = true;
            var vMsg = string.Empty;

            switch (tipoArquivo)
            {
                case TipoArquivo.Cnab240:
                    //vRetorno = ValidarRemessaCnab240(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.Cnab400:
                    vRetorno = ValidarRemessaCnab400(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.Outro:
                    throw new Exception("Tipo de arquivo inexistente.");
            }

            mensagem = vMsg;
            return vRetorno;
        }

        public bool ValidarRemessaCnab400(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            var vRetorno = true;
            var vMsg = string.Empty;

            #region Pré Validações

            if (banco == null)
            {
                vMsg += string.Concat("Remessa: O Banco é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (cedente == null)
            {
                vMsg += string.Concat("Remessa: O Cedente/Beneficiário é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }
            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += string.Concat("Remessa: Deverá existir ao menos 1 boleto para geração da remessa!", Environment.NewLine);
                vRetorno = false;
            }

            #endregion

            if (boletos != null)
                foreach (var boleto in boletos)
                {
                    #region Validação de cada boleto

                    if (boleto.Remessa == null)
                    {
                        vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else
                    {
                        #region Validações da Remessa que deverão estar preenchidas

                        if (string.IsNullOrEmpty(boleto.Remessa.CodigoOcorrencia))
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Código de Ocorrência!", Environment.NewLine);
                            vRetorno = false;
                        }

                        if (string.IsNullOrEmpty(boleto.NumeroDocumento))
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe um Número de Documento!", Environment.NewLine);
                            vRetorno = false;
                        }

                        if (!boleto.Sacado.CpfCnpj.Length.Equals(11) && !boleto.Sacado.CpfCnpj.Length.Equals(14))
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Cpf/Cnpj diferente de 11/14 caracteres!", Environment.NewLine);
                            vRetorno = false;
                        }

                        if (boleto.NossoNumero.Length != 12)
                        {
                            //Apenas 7 caracteres sem boleto.DigitoNossoNumero.
                            vMsg += "Boleto: " + boleto.NumeroDocumento + Environment.NewLine + "Remessa: O Nosso Número diferente de 12 caracteres: " +
                                boleto.NossoNumero + Environment.NewLine;
                            vRetorno = false;
                        }

                        if (boleto.Instrucoes.Count(w => w.Codigo > 0 && w.Codigo < 10) > 2)
                            throw new Exception("Apenas duas instruções com o código entre 2 e 8 são aceitas para o Banco Santander.");

                        #endregion
                    }

                    #endregion
                }

            mensagem = vMsg;
            return vRetorno;
        }

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	O Código de barra para cobrança contém 44 posições dispostas da seguinte forma:
        ///    01 a 03 -  3 - 033 fixo - Código do banco
        ///    04 a 04 -  1 - 9 fixo - Código da moeda (R$)
        ///    05 a 05 -  1 - Dígito verificador do Código de barras
        ///    06 a 09 -  4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor
        ///    20 a 20 -  1 - Fixo 9
        ///    21 a 27 -  7 - Código do cedente padrão satander
        ///    28 a 40 - 13 - Nosso número
        ///    41 - 41 - 1 -  IOS  - Seguradoras(Se 7% informar 7. Limitado  a 9%) Demais clientes usar 0 
        ///    42 - 44 - 3 - Tipo de modalidade da carteira 101, 102, 201
        /// 
        ///   *******
        /// 
        /// </summary>
        public override void FormataCodigoBarra(Boleto boleto)
        {
            var codigoBanco = Utils.FormatCode(Codigo.ToString(), 3);//3
            var codigoMoeda = boleto.Moeda.ToString();//1
            var fatorVencimento = FatorVencimento(boleto).ToString(); //4
            var valorNominal = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);//10
            var fixo = "9";//1
            var codigoCedente = boleto.Cedente.Codigo.PadLeft(7, '0').Substring(1) + Utils.FormatCode(boleto.Cedente.DigitoCedente.ToString(), 1); //7
            var nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Utils.FormatCode(boleto.DigitoNossoNumero, 1); //13
            var ios = boleto.PercentualIos.ToString(); //1
            var tipoCarteira = boleto.Carteira; //3;

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                codigoBanco, codigoMoeda, fatorVencimento, valorNominal, fixo, codigoCedente, nossoNumero, ios, tipoCarteira);

            var calculoDv = Mod10Mod11Santander(boleto.CodigoBarra.Codigo, 9).ToString();

            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}",
                codigoBanco, codigoMoeda, calculoDv, fatorVencimento, valorNominal, fixo, codigoCedente, nossoNumero, ios, tipoCarteira);
        }

        /// <summary>
        /// 
        ///   *******
        /// 
        ///	A Linha Digitavel para cobrança contém 44 posições dispostas da seguinte forma:
        ///   1º Grupo - 
        ///    01 a 03 -  3 - 033 fixo - Código do banco
        ///    04 a 04 -  1 - 9 fixo - Código da moeda (R$) outra moedas 8
        ///    05 a 05 –  1 - Fixo 9
        ///    06 a 09 -  4 - Código cedente padrão santander
        ///    10 a 10 -  1 - Código DV do primeiro grupo
        ///   2º Grupo -
        ///    11 a 13 –  3 - Restante do código cedente
        ///    14 a 20 -  7 - 7 primeiros campos do nosso número
        ///    21 a 21 - 13 - Código DV do segundo grupo
        ///   3º Grupo -  
        ///    22 - 27 - 6 -  Restante do nosso número
        ///    28 - 28 - 1 - IOS  - Seguradoras(Se 7% informar 7. Limitado  a 9%) Demais clientes usar 0 
        ///    29 - 31 - 3 - Tipo de carteira
        ///    32 - 32 - 1 - Código DV do terceiro grupo
        ///   4º Grupo -
        ///    33 - 33 - 1 - Composto pelo DV do código de barras
        ///   5º Grupo -
        ///    34 - 36 - 4 - Fator de vencimento
        ///    37 - 47 - 10 - Valor do título
        ///   *******
        /// 
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            var nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Utils.FormatCode(boleto.DigitoNossoNumero, 1); //13
            var codigoCedente = boleto.Cedente.Codigo.PadLeft(7 , '0').Substring(1) + Utils.FormatCode(boleto.Cedente.DigitoCedente.ToString(), 1);
            var fatorVencimento = FatorVencimento(boleto).ToString();

            #region Grupo 1

            var codigoBanco = Utils.FormatCode(Codigo.ToString(), 3); //3
            var codigoModeda = boleto.Moeda.ToString(); //1
            var fixo = "9"; //1
            var codigoCedente1 = codigoCedente.Substring(0, 4); //4
            var calculoDv1 = Mod10($"{codigoBanco}{codigoModeda}{fixo}{codigoCedente1}").ToString(); //1
            var grupo1 = $"{codigoBanco}{codigoModeda}{fixo}.{codigoCedente1}{calculoDv1}";

            #endregion

            #region Grupo 2

            var codigoCedente2 = codigoCedente.Substring(4, 3); //3
            var nossoNumero1 = nossoNumero.Substring(0, 7); //7
            var calculoDv2 = Mod10($"{codigoCedente2}{nossoNumero1}").ToString();
            var grupo2 = $"{codigoCedente2}{nossoNumero1}{calculoDv2}";
            grupo2 = " " + grupo2.Substring(0, 5) + "." + grupo2.Substring(5, 6);

            #endregion

            #region Grupo 3 - Nosso número, IOS, Tipo de carteira.

            var nossoNumero2 = nossoNumero.Substring(7, 6); //6
            var ios = boleto.PercentualIos.ToString(); //1
            var tipoCarteira = boleto.Carteira; //3

            var calculoDv3 = Mod10($"{nossoNumero2}{ios}{tipoCarteira}").ToString(); //1
            var grupo3 = $"{nossoNumero2}{ios}{tipoCarteira}{calculoDv3}";
            grupo3 = " " + grupo3.Substring(0, 5) + "." + grupo3.Substring(5, 6) + " ";

            #endregion

            #region Grupo 4 - Dígito verificador do código de barras

            var dVcodigoBanco = Utils.FormatCode(Codigo.ToString(), 3); //3
            var dVcodigoMoeda = boleto.Moeda.ToString(); //1
            var dVvalorNominal = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10); //10
            var dVnossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Utils.FormatCode(boleto.DigitoNossoNumero, 1); //1
            var dVtipoCarteira = boleto.Carteira; //3;

            var calculoDVcodigo = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                dVcodigoBanco, dVcodigoMoeda, fatorVencimento, dVvalorNominal, fixo, codigoCedente, dVnossoNumero, ios, dVtipoCarteira);

            var grupo4 = Mod10Mod11Santander(calculoDVcodigo, 9) + " ";
            
            #endregion

            #region Grupo 5 - Fator vencimento e Valor

            var valorNominal = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10); //10

            var grupo5 = $"{fatorVencimento}{valorNominal}";

            #endregion

            boleto.CodigoBarra.LinhaDigitavel = $"{grupo1}{grupo2}{grupo3}{grupo4}{grupo5}";
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            //boleto.NossoNumero = $"{Utils.FormatCode(boleto.NossoNumero, 7)}-{Utils.FormatCode(boleto.DigitoNossoNumero, 1)}";
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            //throw new NotImplementedException("Função não implementada.");
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            //throw new NotImplementedException("Função não implementada.");
            if (!((boleto.Carteira == "102") || (boleto.Carteira == "101") || (boleto.Carteira == "201")))
            {
                var mes = $"A carteira '{boleto.Carteira}' não foi implementada. Carteiras válidas: 101, 102 e 201.";
                throw new NotImplementedException(mes);
            }

            //Banco 353  - Utilizar somente 08 posições do Nosso Numero (07 posições + DV), zerando os 05 primeiros dígitos
            if (Codigo == 353)
            {
                if (boleto.NossoNumero.Length != 7)
                    throw new NotImplementedException("Nosso Número deve ter 7 posições para o banco 353.");
            }

            //Banco 008  - Utilizar somente 09 posições do Nosso Numero (08 posições + DV), zerando os 04 primeiros dígitos
            if (Codigo == 8)
            {
                if (boleto.NossoNumero.Length != 8)
                    throw new NotImplementedException("Nosso Número deve ter 7 posições para o banco 008.");
            }

            if (Codigo == 33)
            {
                //if (boleto.NossoNumero.Length < 12 && (boleto.Carteira.Equals("101") || boleto.Carteira.Equals("102")))
                //    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, "0", 12, true);

                if (boleto.NossoNumero.Length != 12)
                    throw new NotSupportedException("Nosso Número deve ter 12 posições para o banco 033.");
            }

            if (boleto.Cedente.Codigo.Length > 8)
                throw new NotImplementedException("Código cedente deve ter 8 posições.");

            boleto.LocalPagamento = "Pagar preferencialmente no Banco Santander.";

            if (EspecieDocumento.ValidaSigla(boleto.EspecieDocumento) == "")
                boleto.EspecieDocumento = new EspecieDocumento_Santander("2");

            if (boleto.PercentualIos > 10 & (Codigo == 8 || Codigo == 33 || Codigo == 353))
                throw new Exception("O percentual do IOS é limitado a 9% para o Banco Santander");

            if (boleto.Instrucoes.Count(w => w.Codigo > 0 && w.Codigo < 10) > 2)
                throw new Exception("Apenas duas instruções com o código entre 2 e 8 são aceitas para o Banco Santander.");

            boleto.FormataCampos();
        }

        private static int Mod11Santander(string seq, int lim)
        {
            var ndig = 0;
            var nresto = 0;
            var total = 0;
            var multiplicador = 5;

            while (seq.Length > 0)
            {
                var valorPosicao = Convert.ToInt32(seq.Substring(0, 1));
                total += valorPosicao * multiplicador;
                multiplicador--;

                if (multiplicador == 1)
                    multiplicador = 9;

                seq = seq.Remove(0, 1);
            }

            nresto = total - ((total / 11) * 11);

            if (nresto == 0 || nresto == 1)
                ndig = 0;
            else if (nresto == 10)
                ndig = 1;
            else
                ndig = (11 - nresto);

            return ndig;
        }

        private static int Mod10Mod11Santander(string seq, int lim)
        {
            var ndig = 0;
            var nresto = 0;
            var total = 0;
            var multiplicador = 2;

            var posicaoSeq = seq.ToCharArray();
            Array.Reverse(posicaoSeq);
            var sequencia = new string(posicaoSeq);

            while (sequencia.Length > 0)
            {
                var valorPosicao = Convert.ToInt32(sequencia.Substring(0, 1));
                total += valorPosicao * multiplicador;
                multiplicador++;

                if (multiplicador == 10)
                    multiplicador = 2;

                sequencia = sequencia.Remove(0, 1);
            }

            nresto = (total * 10) % 11; //nresto = (((total * 10) / 11) % 10); Jefhtavares em 19/03/14
            
            if (nresto == 0 || nresto == 1 || nresto == 10)
                ndig = 1;
            else
                ndig = nresto;

            return ndig;
        }

        private static int Mult10Mod11Santander(string seq, int lim, int flag)
        {
            var mult = 0;
            var total = 0;
            var pos = 1;
            var ndig = 0;
            var nresto = 0;
            var num = string.Empty;

            mult = 1 + (seq.Length % (lim - 1));

            if (mult == 1)
                mult = lim;

            while (pos <= seq.Length)
            {
                num = seq.Mid(pos, 1);
                total += Convert.ToInt32(num) * mult;

                mult -= 1;
                if (mult == 1)
                    mult = lim;

                pos += 1;
            }

            nresto = ((total * 10) % 11);

            if (flag == 1)
                return nresto;
            else
            {
                if (nresto == 0 || nresto == 1)
                    ndig = 0;
                else if (nresto == 10)
                    ndig = 1;
                else
                    ndig = (11 - nresto);

                return ndig;
            }
        }

        /// <summary>
        /// Verifica o tipo de ocorrência para o arquivo remessa
        /// </summary>
        public string Ocorrencia(string codigo)
        {
            switch (codigo)
            {
                case "01":
                    return "01-Título não existe";
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "06":
                    return "06-Liquidação";
                case "07":
                    return "07-Liquidação por conta";
                case "08":
                    return "08-Liquidação por saldo";
                case "09":
                    return "09-Baixa Automatica";
                case "10":
                    return "10-Baixa conf. instrução ou protesto";
                case "11":
                    return "11-Em Ser";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Prorrogação de Vencimento";
                case "15":
                    return "15-Enviado para Cartório";
                case "16":
                    return "16-Título já baixado/liquidado";
                case "17":
                    return "17-Liquidado em cartório";
                case "21":
                    return "21-Entrada em cartório";
                case "22":
                    return "22-Retirado de cartório";
                case "24":
                    return "24-Custas de cartório";
                case "25":
                    return "25-Protestar Título";
                case "26":
                    return "26-Sustar protesto";
                default:
                    return "";
            }
        }

        #region Métodos de geração do arquivo remessa

        #region HEADER REMESSA

        public override string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            return GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);
        }

        /// <summary>
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                var header = " ";

                base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        header = GerarHeaderRemessaCnab240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Cnab400:
                        header = GerarHeaderRemessaCnab400(0, cedente);
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

        /// <summary>
        /// Gera Header da Remessa para o CNAB 240
        /// </summary>
        /// <param name="cedente"></param>
        /// <param name="numeroArquivoRemessa"></param>
        /// <returns></returns>
        private string GerarHeaderRemessaCnab240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                //Código do Banco na compensação ==> 001 - 003
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Lote de serviço ==> 004 - 007
                header += "0000";

                //Tipo de registro ==> 008 - 008
                header += "0";

                //Reservado (uso Banco) ==> 009 - 016
                header += Utils.FormatCode("", " ", 8);

                //Tipo de inscrição da empresa ==> 017 - 017
                header += (cedente.CpfCnpj.Length == 11 ? "1" : "2");

                //Nº de inscrição da empresa ==> 018 – 032
                header += Utils.FormatCode(cedente.CpfCnpj, "0", 15, true);

                //Código de Transmissão ==> 033 – 047
                header += Utils.FormatCode(cedente.CodigoTransmissao, "0", 15, true);

                //Reservado (uso Banco) ==> 048 - 072
                header += Utils.FormatCode("", " ", 25);

                //Nome da empresa ==> 073 - 102
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();

                //Nome do Banco ==> 103 - 132
                header += Utils.FitStringLength("BANCO SANTANDER", 30, 30, ' ', 0, true, true, false);

                //Reservado (uso Banco) ==> 133 - 142
                header += Utils.FormatCode("", " ", 10);

                //Código remessa ==> 143 - 143
                header += "1";

                //Data de geração do arquivo ==> 144 - 151
                header += DateTime.Now.ToString("ddMMyyyy");

                //Reservado (uso Banco) ==> 152 - 157
                header += Utils.FormatCode("", " ", 6);
                //Nº seqüencial do arquivo ==> 158 - 163
                header += Utils.FormatCode(numeroArquivoRemessa.ToString(), "0", 6, true);

                //Nº da versão do layout do arquivo ==> 164 - 166
                header += "040";

                //Reservado (uso Banco) ==> 167 - 240
                header += Utils.FormatCode("", " ", 74);

                return Utils.SubstituiCaracteresEspeciais(header);

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }

        private string GerarHeaderRemessaCnab400(int numeroConvenio, Cedente cedente)
        {
            try
            {
                var header = "0";       //Código do registro ==> 001 - 001
                header += "1";          //Código da remessa ==> 002 - 002
                header += "REMESSA";    //Literal de transmissão ==> 003 - 009
                header += "01";         //Código do serviço ==> 010 - 011
                header += Utils.FitStringLength("COBRANCA", 15, 15, ' ', 0, true, true, false); //Literal de serviço ==> 012 - 026

                var contaMov = cedente.Codigo + cedente.DigitoCedente;
                var contaCob = cedente.ContaBancaria.Conta + cedente.ContaBancaria.DigitoConta;
                header += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true); //Código da agência cedente ==> 027 - 030
                header += Utils.FitStringLength(contaMov.Truncate(8), 8, 8, '0', 0, true, true, true);   //Conta movimento cedente ==> 031 - 038
                header += Utils.FitStringLength(contaCob.Truncate(8), 8, 8, '0', 0, true, true, true);   //Conta cobrança cedente ==> 039 - 046

                header += Utils.FitStringLength(cedente.Nome ?? "", 30, 30, ' ', 0, true, true, false);   //Nome do cedente  ==> 047 - 076
                header += Utils.FormatCode(Codigo.ToString(), "0", 3, true);    //Código do Banco ==> 077 - 079 
                header += Utils.FitStringLength("SANTANDER", 15, 15, ' ', 0, true, true, false);    //Nome do Banco ==> 080 - 094
                header += Utils.FitStringLength(DateTime.Now.ToString("ddMMyy"), 6, 6, ' ', 0, true, true, false);  //Data de Gravação ==> 095 - 100
                header += Utils.FitStringLength("0", 16, 16, '0', 0, true, true, false);    //Zeros ==> 101 - 116
                header += Utils.FitStringLength(" ", 47, 47, ' ', 0, true, true, false);    //Mensagem 1 ==> 117 - 163
                header += Utils.FitStringLength(" ", 47, 47, ' ', 0, true, true, false);    //Mensagem 2 ==> 164 - 210
                header += Utils.FitStringLength(" ", 47, 47, ' ', 0, true, true, false);    //Mensagem 3 ==> 211 - 257
                header += Utils.FitStringLength(" ", 47, 47, ' ', 0, true, true, false);    //Mensagem 4 ==> 258 - 304
                header += Utils.FitStringLength(" ", 47, 47, ' ', 0, true, true, false);    //Mensagem 5 ==> 305 - 351
                header += Utils.FitStringLength(" ", 40, 40, ' ', 0, true, true, false);    //Brancos ==> 352 - 391
                header += Utils.FitStringLength("0", 3, 3, '0', 0, true, true, true);   //Número da versão da remessa (opcional) ==> 392 - 394 //TODO: Seria isso o número sequencial da remessa?
                header += Utils.FitStringLength("1", 6, 6, '0', 0, true, true, true);   //Número sequencial do registro no arquivo ==> 395 - 400

                header = Utils.SubstituiCaracteresEspeciais(header);

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        #endregion

        #region HEADER LOTE REMESSA

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                var header = " ";

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        header = GerarHeaderLoteRemessaCnab240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Cnab400:
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

        private string GerarHeaderLoteRemessaCnab240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                //Código do Banco na compensação ==> 001 - 003
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                header += "0001";

                //Tipo de registro ==> 008 - 008
                header += "1";

                //Tipo de operação ==> 009 - 009
                header += "R";

                //Tipo de serviço ==> 010 - 011
                header += "01";

                //Reservado (uso Banco) ==> 012 - 013
                header += "  ";

                //Nº da versão do layout do lote ==> 014 - 016
                header += "030";

                //Reservado (uso Banco) ==> 017 - 017
                header += " ";

                //Tipo de inscrição da empresa ==> 018 - 018
                header += (cedente.CpfCnpj.Length == 11 ? "1" : "2");

                //Nº de inscrição da empresa ==> 019 - 033
                header += Utils.FormatCode(cedente.CpfCnpj, "0", 15, true);

                //Reservado (uso Banco) ==> 034 – 053
                header += Utils.FormatCode("", " ", 20);

                //Código de Transmissão ==> 054 - 068
                header += Utils.FormatCode(cedente.CodigoTransmissao, "0", 15, true);

                //Reservado uso Banco ==> 069 – 073
                header += Utils.FormatCode("", " ", 5);

                //Nome do Cedente ==> 074 - 103
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();

                //Mensagem 1 ==> 104 - 143
                header += Utils.FormatCode("", " ", 40);

                //Mensagem 2 ==> 144 - 183
                header += Utils.FormatCode("", " ", 40);
                //Número remessa/retorno ==> 184 - 191
                header += Utils.FormatCode(cedente.NumeroSequencial.ToString(), "0", 8, true);

                //Data da gravação remessa/retorno ==> 192 - 199
                header += DateTime.Now.ToString("ddMMyyyy");

                //Reservado (uso Banco) ==> 200 - 240
                header += Utils.FormatCode("", " ", 41);

                return Utils.SubstituiCaracteresEspeciais(header);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        #endregion

        #region DETALHE REMESSA

        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var detalhe = "";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
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
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                //Código do Banco na compensação ==> 001-003
                var segmentoP = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                segmentoP += Utils.FitStringLength("1", 4, 4, '0', 0, true, true, true);

                //Tipo de registro => 008 - 008
                segmentoP += "3";

                //Nº seqüencial do registro de lote ==> 009 - 013
                segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);

                //Cód. Segmento do registro detalhe ==> 014 - 014
                segmentoP += "P";

                //Reservado (uso Banco) ==> 015 - 015
                segmentoP += " ";

                //Código de movimento remessa ==> 016 - 017
                segmentoP += ObterCodigoDaOcorrencia(boleto);

                //Agência do Cedente ==> 018 –021
                segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);

                //Dígito da Agência do Cedente ==> 022 –022
                segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);

                //Número da conta corrente ==> 023 - 031
                segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 9, 9, '0', 0, true, true, true);

                //Dígito verificador da conta ==> 032 – 032
                segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);

                //Conta cobrança ==> 033 - 041
                segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 9, 9, '0', 0, true, true, true);

                //Dígito da conta cobrança ==> 042 - 042
                segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);

                //Reservado (uso Banco) ==> 043 - 044
                segmentoP += "  ";

                boleto.Valida();
                //Identificação do título no Banco ==> 045 –057
                segmentoP += Utils.FitStringLength(boleto.NossoNumero.Replace("-", ""), 13, 13, '0', 0, true, true, true);

                //Tipo de cobrança ==> 058 - 058
                segmentoP += "5";

                //Forma de Cadastramento ==> 059 - 059
                segmentoP += "1";

                //Tipo de documento ==> 060 - 060
                segmentoP += "1";

                //Reservado (uso Banco) ==> 061 –061
                segmentoP += " ";

                //Reservado (uso Banco) ==> 062 - 062
                segmentoP += " ";

                //Nº do documento ==> 063 - 077
                segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 15, 15, ' ', 0, true, true, false);

                //Data de vencimento do título ==> 078 - 085
                segmentoP += boleto.DataVencimento.ToString("ddMMyyyy");

                //Valor nominal do título ==> 086 - 100
                segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 15, 15, '0', 0, true, true, true);

                //Agência encarregada da cobrança ==> 101 - 104
                segmentoP += "0000";

                //Dígito da Agência do Cedente ==> 105 – 105
                segmentoP += "0";

                //Reservado (uso Banco) ==> 106 - 106
                segmentoP += " ";

                //Espécie do título ==> 107 – 108
                segmentoP += Utils.FitStringLength(boleto.EspecieDocumento.Codigo, 2, 2, '0', 0, true, true, true);

                //Identif. de título Aceito/Não Aceito ==> 109 - 109
                segmentoP += "N";

                //Data da emissão do título ==> 110 - 117
                segmentoP += boleto.DataDocumento.ToString("ddMMyyyy");

                if (boleto.JurosMora > 0)
                {
                    //Código do juros de mora ==> 118 - 118
                    if (!String.IsNullOrWhiteSpace(boleto.CodJurosMora)) //Possibilita passar o código 2 para JurosMora ao Mes, senão for setado, assume o valor padrão 1 para JurosMora ao Dia
                        segmentoP += Utils.FitStringLength(boleto.CodJurosMora.ToString(), 1, 1, '0', 0, true, true, true);
                    else
                        segmentoP += "1";

                    //Data do juros de mora ==> 119 - 126
                    segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);

                    //Valor da mora/dia ou Taxa mensal ==> 127 - 141
                    segmentoP += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    //Código do juros de mora ==> 118 - 118
                    segmentoP += "3";

                    //Data do juros de mora ==> 119 - 126
                    segmentoP += "00000000";

                    //Valor da mora/dia ou Taxa mensal ==> 127 - 141
                    segmentoP += "000000000000000";
                }

                if (boleto.ValorDesconto > 0)
                {
                    //Código do desconto 1 ==> 142 - 142
                    segmentoP += "1";

                    //Data de desconto 1 ==> 143 - 150
                    segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);

                    //Valor ou Percentual do desconto concedido ==> 151 - 165
                    segmentoP += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else
                    segmentoP += "0".PadLeft(24, '0');


                //Valor do IOF a ser recolhido ==> 166 - 180
                segmentoP += "0".PadLeft(15, '0');

                //Valor do abatimento ==> 181 - 195
                segmentoP += "0".PadLeft(15, '0');

                //Identificação do título na empresa ==> 196 - 220
                segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);

                var codigo_protesto = "0";
                var dias_protesto = "00";

                foreach (var instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Santander)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Santander.Protestar:
                            codigo_protesto = "1";
                            dias_protesto = Utils.FitStringLength(instrucao.Dias.ToString(), 2, 2, '0', 0, true, true, true); //Para código '1' – é possível, de 6 a 29 dias
                            break;
                        default:
                            break;
                    }
                }

                //Código para protesto ==> 221 - 221
                segmentoP += codigo_protesto;

                //Número de dias para protesto ==> 222 - 223
                segmentoP += dias_protesto;

                //Código para Baixa/Devolução ==> 222 - 223
                segmentoP += "3";

                //Reservado (uso Banco) ==> 225 – 225
                segmentoP += "0";

                //Número de dias para Baixa/Devolução ==> 226 - 227
                segmentoP += Utils.FitStringLength(boleto.NumeroDiasBaixa.ToString(), 2, 2, '0', 0, true, true, true);

                //Código da moeda ==> 228 - 229
                segmentoP += "00";

                //Reservado (uso Banco) ==> 230 –240
                segmentoP += " ".PadLeft(11, ' ');


                segmentoP = Utils.SubstituiCaracteresEspeciais(segmentoP);

                return segmentoP;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO P DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                //Código do Banco na compensação ==> 001 - 003
                var segmentoQ = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                segmentoQ += Utils.FitStringLength("1", 4, 4, '0', 0, true, true, true);

                //Tipo de registro ==> 008 - 008
                segmentoQ += "3";

                //Nº seqüencial do registro no lote ==> 009 - 013
                segmentoQ += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);

                //Cód. segmento do registro detalhe ==> 014 - 014
                segmentoQ += "Q";

                //Reservado (uso Banco) ==> 015 - 015
                segmentoQ += " ";

                //Código de movimento remessa ==> 016 - 017
                segmentoQ += ObterCodigoDaOcorrencia(boleto);

                if (boleto.Sacado.CpfCnpj.Length <= 11)
                    //Tipo de inscrição do sacado ==> 018 - 018
                    segmentoQ += "1";
                else
                    //Tipo de inscrição do sacado ==> 018 - 018
                    segmentoQ += "2";

                //Número de inscrição do sacado ==> 019 - 033
                segmentoQ += Utils.FitStringLength(boleto.Sacado.CpfCnpj, 15, 15, '0', 0, true, true, true);

                //Nome sacado ==> 034 - 073
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Endereço sacado ==> 074 - 113
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Bairro sacado ==> 114 - 128
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();

                //Cep sacado ==> 129 - 133
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cep.Substring(0, 5), 5, 5, ' ', 0, true, true, false).ToUpper();

                //Sufixo do Cep do sacado ==> 134 - 136
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cep.Substring(5, 3), 3, 3, ' ', 0, true, true, false).ToUpper();

                //Cidade do sacado ==> 137 - 151
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();

                //Unidade da federação do sacado ==> 152 - 153
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Uf, 2, 2, ' ', 0, true, true, false).ToUpper();

                //Tipo de inscrição sacador/avalista ==> 154 - 154
                segmentoQ += "0";

                //Nº de inscrição sacador/avalista ==> 155 - 169
                segmentoQ += "0".PadLeft(15, '0');

                //Nome do sacador/avalista ==> 170 - 209
                segmentoQ += " ".PadLeft(40, ' ');

                //Identificador de carne ==> 210 –212
                segmentoQ += "0".PadLeft(3, '0');

                //Seqüencial da Parcela ou número inicial da parcela ==> 213 –215
                segmentoQ += "0".PadLeft(3, '0');

                //Quantidade total de parcelas ==> 216 –218
                segmentoQ += "0".PadLeft(3, '0');

                //Número do plano ==> 219 – 221
                segmentoQ += "0".PadLeft(3, '0');

                //Reservado (uso Banco) ==> 222 - 240
                segmentoQ += " ".PadLeft(19, ' ');

                return Utils.SubstituiCaracteresEspeciais(segmentoQ);

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                //Código do Banco na compensação ==> 001 - 003
                var segmentoR = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                segmentoR += Utils.FitStringLength("1", 4, 4, '0', 0, true, true, true);

                //Tipo de registro ==> 008 - 008
                segmentoR += "3";

                //Nº seqüencial do registro de lote ==> 009 - 013
                segmentoR += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);

                //Código segmento do registro detalhe ==> 014 - 014
                segmentoR += "R";

                //Reservado (uso Banco) ==> 015 - 015
                segmentoR += " ";

                //Código de movimento ==> 016 - 017
                segmentoR += ObterCodigoDaOcorrencia(boleto);

                if (boleto.OutrosDescontos > 0)
                {
                    //Código do desconto 2 ==> 018 - 018
                    segmentoR += "1";

                    //Data do desconto 2 ==> 019 - 026
                    segmentoR += boleto.DataOutrosDescontos.ToString("ddMMyyyy");

                    //Valor/Percentual a ser concedido ==> 027 - 041
                    segmentoR += Utils.FitStringLength(boleto.OutrosDescontos.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    //Código do desconto 2 ==> 018 - 018
                    segmentoR += "0";

                    //Data do desconto 2 ==> 019 - 026
                    segmentoR += "0".PadLeft(8, '0');

                    //Valor/Percentual a ser concedido ==> 027 - 041
                    segmentoR += "0".PadLeft(15, '0');
                }

                //Reservado (uso Banco) ==> 042 – 065
                segmentoR += " ".PadLeft(24, ' ');

                if (boleto.PercMulta > 0)
                {
                    //Código da multa ==> 066 - 066
                    segmentoR += "2";

                    //Data da multa ==> 067 - 074
                    segmentoR += boleto.DataMulta.ToString("ddMMyyyy");

                    //Valor/Percentual a ser aplicado ==> 075 - 089
                    segmentoR += Utils.FitStringLength(boleto.PercMulta.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else if (boleto.ValorMulta > 0)
                {
                    //Código da multa ==> 066 - 066
                    segmentoR += "1";

                    //Data da multa ==> 067 - 074
                    segmentoR += boleto.DataMulta.ToString("ddMMyyyy");

                    //Valor/Percentual a ser aplicado ==> 075 - 089
                    segmentoR += Utils.FitStringLength(boleto.ValorMulta.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                }
                else
                {
                    //Código da multa ==> 066 - 066
                    segmentoR += "0";

                    //Data da multa ==> 067 - 074
                    segmentoR += "0".PadLeft(8, '0');

                    //Valor/Percentual a ser aplicado ==> 075 - 089
                    segmentoR += "0".PadLeft(15, '0');
                }

                //Reservado (uso Banco) ==> 090 - 099
                segmentoR += " ".PadLeft(10, ' ');

                //Mensagem 3 ==> 100 - 139
                segmentoR += " ".PadLeft(40, ' ');

                //Mensagem 4 ==> 140 - 179
                segmentoR += " ".PadLeft(40, ' ');

                //Reservado ==> 180 - 240
                segmentoR += " ".PadLeft(61, ' ');

                segmentoR = Utils.SubstituiCaracteresEspeciais(segmentoR);

                return segmentoR;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoSRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                //Código do Banco na compensação ==> 001 - 003
                var segmentoS = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                segmentoS += Utils.FitStringLength("1", 4, 4, '0', 0, true, true, true);

                //Tipo de registro ==> 008 - 008
                segmentoS += "3";

                //Nº seqüencial do registro de lote ==> 009 - 013
                segmentoS += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);

                //Cód. Segmento do registro detalhe ==> 014 - 014
                segmentoS += "S";

                //Reservado (uso Banco) ==> 015 - 015
                segmentoS += " ";

                //Código de movimento ==> 016 - 017
                segmentoS += ObterCodigoDaOcorrencia(boleto);

                //Identificação da impressão ==> 018 - 018
                segmentoS += "2";

                //Mensagem 5 ==> 019 - 058
                //Mensagem 6 ==> 059 - 098
                //Mensagem 7 ==> 099 - 138
                for (var i = 0; i < 3; i++)
                {
                    if (boleto.Instrucoes.Count > i)
                        segmentoS += Utils.FitStringLength(boleto.Instrucoes[i].Descricao, 40, 40, ' ', 0, true, true, false);
                    else
                        segmentoS += Utils.FitStringLength(" ", 40, 40, ' ', 0, true, true, false);
                }

                //Mensagem 8 ==> 139 - 178
                segmentoS += " ".PadLeft(40, ' ');

                //Mensagem 9 ==> 179 - 218
                segmentoS += " ".PadLeft(40, ' ');

                //Reservado (uso Banco) ==> 219 - 240
                segmentoS += " ".PadLeft(22, ' ');

                segmentoS = Utils.SubstituiCaracteresEspeciais(segmentoS);

                return segmentoS;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do SEGMENTO S DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarMensagemVariavelRemessa(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var detalhe = "";

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        throw new Exception("Mensagem Variavel nao existe para o tipo CNAB 240.");
                    case TipoArquivo.Cnab400:
                        detalhe = GerarMensagemVariavelRemessaCnab400(boleto, ref numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }

        private string GerarDetalheRemessaCnab400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                var detalhe = "1";  //Código do registro ==> 001 - 001
                detalhe += boleto.Cedente.CpfCnpj.Length <= 11 ? "01" : "02";   //CNPJ ou CPF do cedente ==> 002 - 003
                detalhe += Utils.FitStringLength(boleto.Cedente.CpfCnpj, 14, 14, '0', 0, true, true, true); //CNPJ ou CPF do cedente ==> 004 - 017

                var contaMov = boleto.Cedente.Codigo + boleto.Cedente.DigitoCedente;
                var contaCob = boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta;
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true); //Código da agência cedente ==> 018 - 021
                detalhe += Utils.FitStringLength(contaMov.Truncate(8), 8, 8, '0', 0, true, true, true);   //Conta movimento cedente ==> 022 - 029
                detalhe += Utils.FitStringLength(contaCob.Truncate(8), 8, 8, '0', 0, true, true, true);   //Conta cobrança cedente ==> 030 - 037

                detalhe += Utils.FitStringLength(boleto.NumeroControle ?? "", 25, 25, ' ', 0, true, true, false);   //Número de controle do participante, controle do cedente ==> 038 - 062 TODO: oq Seria isso

                //NossoNumero com DV, pegar os 8 primeiros dígitos, da direita para esquerda ==> 063 - 070
                var nossoNumero = Utils.FormatCode(boleto.NossoNumero, 12) + Utils.FormatCode(boleto.DigitoNossoNumero, 1); //Mod11Santander(Utils.FormatCode(boleto.NossoNumero, 12), 9); //13
                detalhe += Utils.Right(nossoNumero, 8, '0', true);

                detalhe += "000000";    //Data do segundo desconto (06) ==> 071 - 076
                detalhe += " ";         //Brancos ==> 077 - 077
                detalhe += boleto.PercMulta == 0 ? "0" : "4";   //Informação de multa = 4, senão houver informar zero ==> 078 - 078
                detalhe += Utils.FitStringLength(boleto.PercMulta.ApenasNumeros(), 4, 4, '0', 0, true, true, true); //Percentual multa por atraso % ==> 079 - 082

                detalhe += "00";    //Unidade de valor moeda corrente ==> 083 - 084
                detalhe += "0000000000000"; //Valor do título em outra unidade ==> 082 - 097
                detalhe += "    ";  //Brancos ==> 098 - 101
                detalhe += "000000";    //Data para cobrança de multa ==> 102 - 107 (Se for zeros, é cobrada logo após o vencimento.)

                //Código da carteira ==> 108 - 108
                //1 - Eletrônica COM registro
                //3 - Caucionada eletrônica
                //4 - Cobrança SEM registro
                //5 - Rápida COM registro
                //6 - Caucionada rápida
                //7 - Descontada eletrônica
                string carteira;
                switch (boleto.Carteira)
                {
                    case "5":
                    case "101": //Carteira 101 (Rápida com registro - impressão pelo beneficiário)
                        carteira = "5";
                        break;

                    case "1":
                    case "201": //Carteira 201 (Eletrônica com registro - impressão pelo banco)
                        carteira = "1";
                        break;
                    default:
                        throw new Exception("Carteira não implementada para emissão de remessa");
                }
                detalhe += carteira;

                //Código de ocorrência  ==> 109 - 110
                //01 - Entrada de Título
                //02 - Baixa de Título
                //04 - Concessão de abatimento
                //05 - Cancelamento de abatimento
                //06 - Prorrogação de vencimento
                //07 - Alteração Número de controle do cedente
                //08 - Alteração do seu Número
                //09 - Protestar
                //18 - Sustar protesto
                detalhe += ObterCodigoDaOcorrencia(boleto);

                detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, ' ', 0, true, true, false);    //Nº do documento ==> 111 - 120
                detalhe += boleto.DataVencimento.ToString("ddMMyy");    //Data de vencimento do título ==> 121 - 126
                detalhe += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 13, 13, '0', 0, true, true, true); //Valor do título - moeda corrente ==> 127 - 139

                detalhe += "033";   //Número do Banco cobrador ==> 140 - 142

                //Código da agência cobradora do Banco Santander informar somente se carteira for igual a 5, caso contrário, informar zeros. ==> 143 - 147
                if (carteira == "5")
                {
                    detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                    detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoAgencia, 1, 1, '0', 0, true, true, true);
                }
                else
                {
                    detalhe += Utils.FitStringLength("0", 5, 5, '0', 0, true, true, true);
                }

                detalhe += boleto.EspecieDocumento.Codigo.PadLeft(2, '0'); //Espécie de documento ==> 148 - 149

                //01 - Duplicata
                //02 - Nota promissória
                //04 - Apólice / Nota Seguro
                //05 - Recibo
                //06 - Duplicata de serviço
                //07 - Letra de cambio
                //var especieDoc = "00";

                //switch (boleto.EspecieDocumento.Codigo)
                //{
                //    case "2": //DuplicataMercantil, 
                //        especieDoc = "01";
                //        break;
                //    case "12": //NotaPromissoria
                //    case "13": //NotaPromissoriaRural
                //    case "98": //NotaPromissoariaDireta
                //        especieDoc = "02";
                //        break;
                //    case "20": //ApoliceSeguro
                //        especieDoc = "03";
                //        break;
                //    case "17": //Recibo
                //        especieDoc = "05";
                //        break;
                //    case "06": //DuplicataServico
                //        especieDoc = "06";
                //        break;
                //    case "07": //LetraCambio353
                //    case "30": //LetraCambio008
                //        especieDoc = "07";
                //        break;
                //    default:    //Cheque ou qualquer outro Código
                //        especieDoc = "01";
                //        break;
                //}

                //detalhe += especieDoc;

                detalhe += "N"; //Tipo de aceite ==> 150 - 150
                detalhe += boleto.DataDocumento.ToString("ddMMyy"); //Data da emissão do título ==> 151 - 156

                //Pega todas as instruções com o código entre 2 e 8;
                var instrucoes = boleto.Instrucoes.Where(w => w.Codigo > 0 && w.Codigo < 10).ToList();

                //Primeira instrução cobrança ==> 157 - 158
                if (instrucoes.Count > 0)
                    detalhe += Utils.FitStringLength(instrucoes[0].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                else
                    detalhe += "00"; //Não há instruções.

                //Segunda instrução cobrança==> 159 - 160
                if (instrucoes.Count > 1)
                    detalhe += Utils.FitStringLength(instrucoes[1].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                else
                    detalhe += "00"; //Não há instruções.

                detalhe += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 13, 13, '0', 0, true, true, true);   //Valor de mora a ser cobrado por dia de atraso == > 161 - 173
                detalhe += boleto.ValorDesconto > 0 ? boleto.DataVencimento.ToString("ddMMyy") : "000000";  //Data limite para concessão de desconto ==> 174 - 179
                detalhe += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true);   //Valor de desconto a ser concedido ==> 180 - 192
                detalhe += "0000000000000"; //Valor do IOF a ser recolhido pelo Banco para nota de seguro ==> 192 - 205
                detalhe += "0000000000000"; //Valor do abatimento a ser concedido ou valor do segundo desconto. ==> 206 - 218

                detalhe += boleto.Sacado.CpfCnpj.Length <= 11 ? "01" : "02"; //Tipo de inscrição do sacado  ==> 219 - 220
                detalhe += Utils.FitStringLength(boleto.Sacado.CpfCnpj, 14, 14, '0', 0, true, true, true).ToUpper();    //CNPJ ou CPF do sacado ==> 221 - 234
                detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();   //Nome do sacado ==> 235 - 274
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.EndComNumComplemento.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();   //Endereço do sacado ==> 275 - 314
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 12, 12, ' ', 0, true, true, false).ToUpper();    //Bairro do sacado (opcional) ==> 315 - 326
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cep, 8, 8, ' ', 0, true, true, true);   //CEP do sacado 327 - 334
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();    //Município do sacado ==> 335 - 349
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Uf, 2, 2, ' ', 0, true, true, false).ToUpper(); //UF Estado do sacado ==> 350 - 351

                detalhe += new string(' ', 30); //Nome do Sacador ou coobrigado ==> 352 - 381
                detalhe += " "; //Brancos ==> 382 - 382

                if (contaCob.Length == 10)
                {
                    detalhe += "I"; //Identificador de complemento de conta cobrança ==> 383 - 383
                    detalhe += contaCob.Substring(boleto.Cedente.ContaBancaria.Conta.Length - 1); //Complemento da conta ==> 384 - 385
                }
                else
                {
                    detalhe += "   ";
                }

                detalhe += "      "; //Brancos (6) ==> 386 - 391

                //Número de dias para protesto ==> 392 - 393
                if (boleto.Instrucoes.FirstOrDefault(x => x.Codigo == 6) is Instrucao instrucao)
                    detalhe += Utils.FitStringLength(instrucao.Dias.ToString(), 2, 2, '0', 0, true, true, true);
                else
                    detalhe += "00";

                detalhe += " "; //Brancos ==> 394 - 394
                detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);    //Número sequencial do registro no arquivo ==> 395 - 400

                return Utils.SubstituiCaracteresEspeciais(detalhe);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        private string GerarMensagemVariavelRemessaCnab400(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var instrucoes = boleto.Instrucoes.Where(w => w.Codigo == 0).Take(3).ToList(); //w.Codigo == 98 || w.Codigo == 99 || 

                if (!instrucoes.Any())
                    return "";

                var detalhe = "2"; //Código do registro = 2 (Recibo do Sacado) 3, 4, 5, 6 e 7 (Ficha de Compensação) ==> 001 - 001
                detalhe += new string(' ', 16); //Uso do Banco ==> 002 - 017

                var contaMov = boleto.Cedente.Codigo + boleto.Cedente.DigitoCedente;
                var contaCob = boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta;
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true); //Código da agência cedente ==> 018 - 021
                detalhe += Utils.FitStringLength(contaMov.Truncate(8), 8, 8, '0', 0, true, true, true);   //Conta movimento cedente ==> 022 - 029
                detalhe += Utils.FitStringLength(contaCob.Truncate(8), 8, 8, '0', 0, true, true, true);   //Conta cobrança cedente ==> 030 - 037

                detalhe += new string(' ', 10); //Uso do Banco ==> 038 - 047

                detalhe += "01";    //Sub-sequência do registro ==> 048 - 049
                detalhe += Utils.FitStringLength(instrucoes[0].Descricao, 50, 50, ' ', 0, true, true, false); //Mensagem variável por título ==> 050 - 099

                detalhe += "02";    //Sub-sequência do registro ==> 100 - 101
                if (instrucoes.Count > 1)
                    detalhe += Utils.FitStringLength(instrucoes[1].Descricao, 50, 50, ' ', 0, true, true, false); //Mensagem variável por título ==> 102 - 151
                else
                    detalhe += new string(' ', 50);

                detalhe += "03";    //Sub-sequência do registro ==> 152 - 153
                if (instrucoes.Count > 2)
                    detalhe += Utils.FitStringLength(instrucoes[2].Descricao, 50, 50, ' ', 0, true, true, false); //Mensagem variável por título ==> 154 - 203
                else
                    detalhe += new string(' ', 50);
                
                detalhe += new string(' ', 179);    //Uso do Banco ==> 204 - 382

                if (contaCob.Length == 10)
                {
                    detalhe += "I"; //Identificador de complemento de conta cobrança ==> 383 - 383
                    detalhe += contaCob.Substring(boleto.Cedente.ContaBancaria.Conta.Length - 1); //Complemento da conta ==> 384 - 385
                }
                else
                {
                    detalhe += "   ";
                }

                detalhe += new string(' ', 9);  //Brancos ==> 386 - 394
                detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);    //Número sequêncial do registro no arquivo ==> 395 - 400

                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe);

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        #endregion

        #region TRAILER CNAB240

        /// <summary>
        ///POS INI/FINAL	DESCRIÇÂO	                   A/N	TAM	DEC	CONTEÚDO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	Código do Banco na compensação	    N	003		341	
        ///004 - 007	Lote de serviço	                    N	004		Nota 5 
        ///008 - 008	Registro Trailer de Lote            N	001     5
        ///009 - 017	Complemento de Registros            A	009     Brancos
        ///018 - 023    Qtd. Registros do Lote              N   006     Nota 15     
        ///024 - 041    Soma Valor dos Débitos do Lote      N   018     Nota 14     
        ///042 - 059    Soma Qtd. de Moedas do Lote         N   018     Nota 14
        ///060 - 230    Complemento de Registros            A   171     Brancos
        ///231 - 240    Cód. Ocr. para Retorno              A   010     Brancos
        /// </summary>
        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                //Código do Banco na compensação ==> 001 - 003
                var trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                trailer += Utils.FormatCode("1", "0", 4, true);

                //Tipo de registro ==> 008 - 008
                trailer += "5";

                //Reservado (uso Banco) ==> 009 - 017
                trailer += Utils.FormatCode("", " ", 9);

                //Quantidade de registros do lote ==> 018 - 023
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);

                //Reservado (uso Banco) ==> 024 - 240
                trailer += Utils.FormatCode("", " ", 217);

                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do LOTE de REMESSA.", e);
            }
        }

        /// <summary>
        ///POS INI/FINAL	DESCRIÇÂO	                   A/N	TAM	DEC	CONTEÚDO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	Código do Banco na compensação	    N	003		341	
        ///004 - 007	Lote de serviço	                    N	004		9999 
        ///008 - 008	Registro Trailer de Arquivo         N	001     9
        ///009 - 017	Complemento de Registros            A	009     Brancos
        ///018 - 023    Qtd. Lotes do Arquivo               N   006     Nota 15     
        ///024 - 029    Qtd. Registros do Arquivo           N   006     Nota 15     
        ///030 - 240    Complemento de Registros            A   211     Brancos
        /// </summary>
        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                //Código do Banco na compensação ==> 001 - 003
                var trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);

                //Numero do lote remessa ==> 004 - 007
                trailer += "9999";

                //Tipo de registro ==> 008 - 008
                trailer += "9";

                //Reservado (uso Banco) ==> 009 - 017
                trailer += Utils.FormatCode("", " ", 9);

                //Quantidade de lotes do arquivo ==> 018 - 023
                trailer += Utils.FormatCode("1", "0", 6, true);

                //Quantidade de registros do arquivo ==> 024 - 029
                trailer += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);

                //Reservado (uso Banco) ==> 030 - 240
                trailer += Utils.FormatCode("", " ", 211);

                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception e)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do ARQUIVO de REMESSA.", e);
            }
        }

        #endregion

        #region TRAILER CNAB400

        /// <summary>
        /// TRAILER do arquivo CNAB
        /// Gera o TRAILER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                var trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        break;
                    case TipoArquivo.Cnab400:
                        trailer = GerarTrailerRemessa400(numeroRegistro, vltitulostotal);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        private string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
        {
            try
            {
                var complemento = new string('0', 374);

                var trailer = "9";      //Código do registro ==> 001 - 001
                trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);    //Quantidade de documentos no arquivo ==> 002 - 007
                trailer += Utils.FitStringLength(vltitulostotal.ApenasNumeros(), 13, 13, '0', 0, true, true, true); //Valor total dos títulos ==> 008 - 020
                trailer += complemento; //Zeros ==> 021 - 394
                trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);    //Número sequencial do registro no arquivo ==> 395 - 400

                return Utils.SubstituiCaracteresEspeciais(trailer);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        #endregion

        #region Método de leitura do arquivo retorno

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                var detalhe = new DetalheRetorno(registro);

                //001 -> 001, Código do registro.
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));  //002 -> 003, Tipo de Inscrição Empresa.
                detalhe.NumeroInscricao = registro.Substring(3, 14);                //004 -> 017, Nº Inscrição da Empresa.
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));         //018 -> 021, Identificação da Empresa Cedente no Banco.
                detalhe.Conta = Utils.ToInt32(registro.Substring(21, 8));           //022 -> 029, Conta movimento Beneficiário.
                //030 -> 037, Conta cobrança Beneficiário.
                detalhe.NumeroControle = registro.Substring(37, 25);    //038 -> 062, Número de controle.
                detalhe.NossoNumeroComDV = registro.Substring(62, 8).PadLeft(13, '0');   //063 -> 070, Nosso número.
                detalhe.NossoNumero = registro.Substring(62, 7).PadLeft(12, '0');        //063 -> 069, Nosso número (Sem DV).
                detalhe.DACNossoNumero = registro.Substring(69, 1);     //070 -> 070, DV do nosso número.
                //071 -> 107, Brancos.
                //108 -> 108, Código da carteira.
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));   //109 -> 110, Código de ocorrência.
                detalhe.DescricaoOcorrencia = Ocorrencia(registro.Substring(108, 2));   //109 -> 110, Descrição da ocorrência.
                detalhe.DataOcorrencia = DateTime.ParseExact(registro.Substring(110, 6), "ddMMyy", new CultureInfo("pt-BR"));   //111 -> 116, Data da ocorrência.
                detalhe.NumeroDocumento = registro.Substring(116, 10);          //117 -> 126, Seu número.
                detalhe.IdentificacaoTitulo = registro.Substring(126, 8);       //127 -> 134, Nosso número.
                //135 -> 136, Código Original da Remessa.
                detalhe.MotivoCodigoOcorrencia = registro.Substring(136, 9);    //137 -> 145, Códigos dos erros.
                //146 -> 146, Brancos.
                detalhe.DataVencimento = DateTime.ParseExact(registro.Substring(146, 6), "ddMMyy", new CultureInfo("pt-BR"));   //147 -> 152, Data de vencimento.
                detalhe.ValorTitulo = Convert.ToInt64(registro.Substring(152, 13)) / 100m;  //153 -> 165, Valor do título.
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));            //166 -> 168, Número do Banco cobrador.
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));       //169 -> 173, Código da agência recebedora do título.
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));                //174 -> 175, Espécie de documento.
                //176 -> 188, Valor da tarifa cobrada.
                detalhe.OutrasDespesas = Convert.ToUInt64(registro.Substring(188, 13)) / 100m;  //189 -> 201, Valor de outras despesas.
                //202 -> 214, Valor dos juros de atraso.
                detalhe.IOF = Convert.ToUInt64(registro.Substring(214, 13)) / 100m;                 //215 -> 227, Valor do IOF devido.
                detalhe.ValorAbatimento = Convert.ToUInt64(registro.Substring(227, 13)) / 100m;     //228 -> 240, Valor do abatimento concedido.
                detalhe.Descontos = Convert.ToUInt64(registro.Substring(240, 13)) / 100m;           //241 -> 253, Valor do desconto concedido.
                detalhe.ValorPago = Convert.ToUInt64(registro.Substring(253, 13)) / 100m;           //254 -> 266, Valor total recebido.
                detalhe.JurosMora = Convert.ToUInt64(registro.Substring(266, 13)) / 100m;           //267 -> 279, Valor dos juros de mora.
                detalhe.OutrosCreditos = Convert.ToUInt64(registro.Substring(279, 13)) / 100m;      //280 -> 292, Valor de outros créditos.
                //293 -> 293, Branco.
                //294 -> 294, Código de aceite = N.
                //295 -> 295, Branco.
                detalhe.DataCredito = DateTime.ParseExact(registro.Substring(295, 6), "ddMMyy", new CultureInfo("pt-BR"));  //296 -> 301, Data do crédito.
                detalhe.NomeSacado = registro.Substring(301, 36).TrimEnd();   //302 -> 337, Nome do pagador.
                //338 -> 338, Identificador do complemento.
                //339 -> 340, Unidade de valor moeda corrente = 00.
                //341 -> 353, Valor do título em outra unidade de valor [v9(8) ou v9(5)].
                //354 -> 366, Valor do IOF em outra unidade de valor [v9(8) ou v9(5)].
                //367 -> 379, Valor do débito ou crédito.
                //380 -> 380, Débito ou crédito (D ou C).
                //381 -> 383, Brancos.
                //384 -> 385, Complemento.
                //386 -> 389, Sigla da empresa no sistema.
                //390 -> 391, Brancos.
                //392 -> 394, Número da versão.
                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));   //395 -> 400, Número sequencial do registro no arquivo.
                                
                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            try
            {
                var detalhe = new DetalheSegmentoTRetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "T")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento T.");

                detalhe.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                detalhe.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                detalhe.Agencia = Convert.ToInt32(registro.Substring(17, 4));
                detalhe.DigitoAgencia = registro.Substring(21, 1);
                detalhe.Conta = Convert.ToInt32(registro.Substring(22, 9));
                detalhe.DigitoConta = registro.Substring(31, 1);

                detalhe.NossoNumero = registro.Substring(40, 13);
                detalhe.CodigoCarteira = Convert.ToInt32(registro.Substring(53, 1));
                detalhe.NumeroDocumento = registro.Substring(54, 15);
                var dataVencimento = registro.Substring(69, 8);
                //detalhe.DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));
                detalhe.DataVencimento = DateTime.ParseExact(dataVencimento, "ddMMyyyy", CultureInfo.InvariantCulture);
                decimal valorTitulo = Convert.ToInt64(registro.Substring(77, 15));
                detalhe.ValorTitulo = valorTitulo / 100;
                detalhe.IdentificacaoTituloEmpresa = registro.Substring(100, 25);
                detalhe.TipoInscricao = Convert.ToInt32(registro.Substring(127, 1));
                detalhe.NumeroInscricao = registro.Substring(128, 15);
                detalhe.NomeSacado = registro.Substring(143, 40);
                decimal valorTarifas = Convert.ToUInt64(registro.Substring(193, 15));
                detalhe.ValorTarifas = valorTarifas / 100;
                detalhe.CodigoRejeicao = registro.Substring(208, 10);
                detalhe.UsoFebraban = registro.Substring(218, 22);

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
                var DataCredito = Convert.ToInt32(registro.Substring(145, 8));
                detalhe.DataCredito = Convert.ToDateTime(DataCredito.ToString("##-##-####"));
                var DataOcorrencia = Convert.ToInt32(registro.Substring(137, 8));
                detalhe.DataOcorrencia = Convert.ToDateTime(DataOcorrencia.ToString("##-##-####"));
                var DataOcorrenciaSacado = Convert.ToInt32(registro.Substring(157, 8));
                if (DataOcorrenciaSacado > 0)
                    detalhe.DataOcorrenciaSacado = Convert.ToDateTime(DataOcorrenciaSacado.ToString("##-##-####"));
                else
                    detalhe.DataOcorrenciaSacado = DateTime.Now;

                decimal JurosMultaEncargos = Convert.ToUInt64(registro.Substring(17, 15));
                detalhe.JurosMultaEncargos = JurosMultaEncargos / 100;
                decimal ValorDescontoConcedido = Convert.ToUInt64(registro.Substring(32, 15));
                detalhe.ValorDescontoConcedido = ValorDescontoConcedido / 100;
                decimal ValorAbatimentoConcedido = Convert.ToUInt64(registro.Substring(47, 15));
                detalhe.ValorAbatimentoConcedido = ValorAbatimentoConcedido / 100;
                decimal ValorIOFRecolhido = Convert.ToUInt64(registro.Substring(62, 15));
                detalhe.ValorIOFRecolhido = ValorIOFRecolhido / 100;
                decimal ValorPagoPeloSacado = Convert.ToUInt64(registro.Substring(77, 15));
                detalhe.ValorPagoPeloSacado = ValorPagoPeloSacado / 100;
                decimal ValorLiquidoASerCreditado = Convert.ToUInt64(registro.Substring(92, 15));
                detalhe.ValorLiquidoASerCreditado = ValorLiquidoASerCreditado / 100;
                decimal ValorOutrasDespesas = Convert.ToUInt64(registro.Substring(107, 15));
                detalhe.ValorOutrasDespesas = ValorOutrasDespesas / 100;

                decimal ValorOutrosCreditos = Convert.ToUInt64(registro.Substring(122, 15));
                detalhe.ValorOutrosCreditos = ValorOutrosCreditos / 100;

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }


        }

        public override DetalheSegmentoYRetornoCNAB240 LerDetalheSegmentoYRetornoCNAB240(string registro)
        {
            try
            {
                var detalhe = new DetalheSegmentoYRetornoCNAB240(registro);

                if (registro.Substring(13, 1) != "Y")
                    throw new Exception("Registro inválido. O detalhe não possuí as características do segmento Y.");

                detalhe.CodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                detalhe.IdentificacaoRegistro = Convert.ToInt32(registro.Substring(17, 4));
                detalhe.IdentificacaoCheque1 = registro.Substring(19, 34);
                detalhe.IdentificacaoCheque2 = registro.Substring(43, 34);
                detalhe.IdentificacaoCheque3 = registro.Substring(87, 34);
                detalhe.IdentificacaoCheque4 = registro.Substring(121, 34);
                detalhe.IdentificacaoCheque5 = registro.Substring(155, 34);
                detalhe.IdentificacaoCheque6 = registro.Substring(189, 34);

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO Y.", ex);
            }


        }

        #endregion

        #endregion

        public override long ObterNossoNumeroSemConvenioOuDigitoVerificador(long convenio, string nossoNumero)
        {
            if (string.IsNullOrEmpty(nossoNumero) || nossoNumero.Length != 13)
                throw new TamanhoNossoNumeroInvalidoException();

            var nossoNumeroSemDv = nossoNumero.Substring(0, 12);

            if (long.TryParse(nossoNumeroSemDv, out var numero))
                return numero;

            throw new NossoNumeroInvalidoException();
        }
    }
}