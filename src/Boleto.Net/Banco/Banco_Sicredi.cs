using System;
using System.Web.UI;
using BoletoNet.Util;
using BoletoNet.EDI.Banco;
using BoletoNet.Excecoes;

[assembly: WebResource("BoletoNet.Imagens.748.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <Author>
    /// Samuel Schmidt - Sicredi Nordeste RS / Felipe Eduardo - RS
    /// </Author>
    internal sealed class Banco_Sicredi : AbstractBanco, IBanco
    {
        internal Banco_Sicredi()
        {
            Codigo = 748;
            Digito = "X";
            Nome = "Banco Sicredi";
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            //Formata o tamanho do número da agência
            if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Formata o tamanho do número da conta corrente
            if (boleto.Cedente.ContaBancaria.Conta.Length < 5)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 5);

            if (boleto.Cedente.ContaBancaria.Conta.Length > 5)
                throw new Exception("Conta bancária com mais de 5 dígitos");

            //Atribui o nome do banco ao local de pagamento
            if (boleto.LocalPagamento == "Até o vencimento, preferencialmente no ")
                boleto.LocalPagamento += Nome;
            else boleto.LocalPagamento = "PAGÁVEL PREFERENCIALMENTE NAS COOPERATIVAS DE CRÉDITO DO SICREDI";

            //Verifica se o nosso número é válido
            if (Utils.ToInt64(boleto.NossoNumero) == 0 || boleto.NossoNumero.Length > 8)
                throw new Exception("Nosso número inválido");

            if (boleto.NossoNumero.Length == 6)
            {
                boleto.NossoNumero = DateTime.Now.ToString("yy") + boleto.NossoNumero;
                boleto.DigitoNossoNumero = DigNossoNumeroSicredi(boleto);
                boleto.NossoNumero += boleto.DigitoNossoNumero;
            }

            if (string.IsNullOrWhiteSpace(boleto.DigitoNossoNumero))
                boleto.DigitoNossoNumero = DigNossoNumeroSicredi(boleto);
            
            //Verifica se data do processamento é valida
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento é valida
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            FormataCodigoBarra(boleto);
            if (boleto.CodigoBarra.Codigo.Length != 44)
                throw new BoletoNetException("Código de barras é inválido");

            FormataLinhaDigitavel(boleto);
            //FormataNossoNumero(boleto);
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            var nossoNumero = boleto.NossoNumero;
            boleto.NossoNumero = string.Format("{0}/{1}-{2}", nossoNumero.Substring(0, 2), nossoNumero.Substring(2, 6), nossoNumero.Substring(8));
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função do fomata número do documento não implementada.");
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //041M2.1AAAd1  CCCCC.CCNNNd2  NNNNN.041XXd3  V FFFF9999999999

            var campo1 = "7489" + boleto.CodigoBarra.Codigo.Substring(19, 5);
            var d1 = Mod10Sicredi(campo1);
            campo1 = FormataCampoLD(campo1) + d1;

            var campo2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            var d2 = Mod10Sicredi(campo2);
            campo2 = FormataCampoLD(campo2) + d2;

            var campo3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            var d3 = Mod10Sicredi(campo3);
            campo3 = FormataCampoLD(campo3) + d3;

            var campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);

            var campo5 = boleto.CodigoBarra.Codigo.Substring(5, 14);

            boleto.CodigoBarra.LinhaDigitavel = campo1 + "  " + campo2 + "  " + campo3 + "  " + campo4 + "  " + campo5;
        }

        private string FormataCampoLD(string campo)
        {
            return string.Format("{0}.{1}", campo.Substring(0, 5), campo.Substring(5));
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            var valor = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "").PadLeft(10, '0');

            //Campo livre:
            //Tipo da cobranca     (1)  1-Com registro ou 3-Sem registro.
            //Tipo da carteira     (1)  1-Simples, 2-Caucionada ou 3-Descontada.
            //Nosso Número         (9)  Código do nosso número com 9 dígitos.
            //Agência              (6)  Código da agência com 4 dígitos e posto com 2 dígitos.
            //Cód. do beneficiário (5)  Número da conta do cliente sem o dígito de controle com 5 dígitos. Estava OperacaoConta ao invés de Conta.
            //"00" ou "10"         (2)  Será "10" se houver valor expresso na barra, senão "00".
            //Digito verificador   (1)  Dígito verificador calculado pelo módulo 11 para código de barras.
            var campoLivre = "11" + boleto.NossoNumero.PadLeft(8, '0') + boleto.DigitoNossoNumero.PadLeft(1, '0') 
                + boleto.Cedente.ContaBancaria.Agencia.PadLeft(4, '0') 
                + boleto.Cedente.ContaBancaria.DigitoAgencia.PadLeft(2, '0')
                + boleto.Cedente.ContaBancaria.Conta.PadLeft(5, '0') + "10";

            //Código de barras:
            //Código do banco     (3)  Preenchido com zeros à esquerda.
            //Código da moeda     (1)  Para Real, usar '9'.
            //Dígito verificador  (1)  Dígito verificador do código de barras.
            //Fator de vencimento (4)  Fator de vencimento com base no cálculo de data.
            //Valor do título     (10) Preencher com zeros à esquerda.
            //Campo livre         (25) União de outros valores, definidos acima.
            boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}{5}", Codigo.ToString().PadLeft(3, '0'), boleto.Moeda, 
                FatorVencimento(boleto), valor, campoLivre, DigSicredi(campoLivre));

            //Dígito de auto conferência.
            var dac = DigSicredi(boleto.CodigoBarra.Codigo);

            if (dac == 0 || dac > 9)
                dac = 1;
            
            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + dac + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }

        //public bool RegistroByCarteira(Boleto boleto)
        //{
        //    bool valida = false;
        //    if (boleto.Carteira == "112"
        //        || boleto.Carteira == "115"
        //        || boleto.Carteira == "104"
        //        || boleto.Carteira == "147"
        //        || boleto.Carteira == "188"
        //        || boleto.Carteira == "108"
        //        || boleto.Carteira == "109"
        //        || boleto.Carteira == "150"
        //        || boleto.Carteira == "121")
        //        valida = true;
        //    return valida;
        //}

        #region Métodos de Geração do Arquivo de Remessa

        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var detalhe = " ";

                //base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        detalhe = GerarDetalheRemessaCnab240(boleto, numeroRegistro, tipoArquivo);
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

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarDetalheRemessaCnab240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var detalhe = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                detalhe += Utils.FormatCode("", "0", 4, true);
                detalhe += "3";
                detalhe += Utils.FormatCode(numeroRegistro.ToString(), 5);
                detalhe += "P 01";
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 5);
                detalhe += "0";
                detalhe += Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 12);
                detalhe += boleto.Cedente.ContaBancaria.DigitoConta;
                detalhe += " ";
                detalhe += Utils.FormatCode(boleto.NossoNumero.Replace("/", "").Replace("-", ""), 20);
                detalhe += "1";
                detalhe += Convert.ToInt16(boleto.Carteira) == 1 ? "1" : "2";
                detalhe += "122";
                detalhe += Utils.FormatCode(boleto.NumeroDocumento, 15);
                detalhe += boleto.DataVencimento.ToString("ddMMyyyy");
                var valorBoleto = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "");
                valorBoleto = Utils.FormatCode(valorBoleto, 13);
                detalhe += valorBoleto;
                detalhe += "00000 99A";
                detalhe += boleto.DataDocumento.ToString("ddMMyyyy");
                detalhe += "200000000";
                valorBoleto = boleto.JurosMora.ToString("f").Replace(",", "").Replace(".", "");
                valorBoleto = Utils.FormatCode(valorBoleto, 13);
                detalhe += valorBoleto;
                detalhe += "1";
                detalhe += boleto.DataDesconto.ToString("ddMMyyyy");
                valorBoleto = boleto.ValorDesconto.ToString("f").Replace(",", "").Replace(".", "");
                valorBoleto = Utils.FormatCode(valorBoleto, 13);
                detalhe += valorBoleto;
                detalhe += Utils.FormatCode("", 26);
                detalhe += Utils.FormatCode("", " ", 25);
                detalhe += "0001060090000000000 ";

                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe);
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB240.", e);
            }
        }

        public override string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            return GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                var header = " ";

                base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.Cnab240:
                        header = GerarHeaderRemessaCnab240(cedente);
                        break;
                    case TipoArquivo.Cnab400:
                        header = GerarHeaderRemessaCnab400(0, cedente, numeroArquivoRemessa);
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

        private string GerarHeaderLoteRemessaCnab240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                return GerarHeaderRemessaCnab240(cedente);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
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
                        header = GerarHeaderLoteRemessaCnab240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Cnab400:
                        // não tem no CNAB 400 header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
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

        public string GerarHeaderRemessaCnab240(Cedente cedente)
        {
            try
            {
                var header = "748";
                header += "0000";
                header += "0";
                header += Utils.FormatCode("", " ", 9);
                header += cedente.CpfCnpj.Length == 11 ? "1" : "2";
                header += Utils.FormatCode(cedente.CpfCnpj, "0", 14, true);
                header += Utils.FormatCode(cedente.Convenio.ToString(), " ", 20);
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5, true);
                header += " ";
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12, true);
                header += cedente.ContaBancaria.DigitoConta;
                header += " ";
                header += Utils.FormatCode(cedente.Nome, " ", 30);
                header += Utils.FormatCode("SICREDI", " ", 30);
                header += Utils.FormatCode("", " ", 10);
                header += Utils.FormatCode(cedente.Nome, " ", 30);
                header += "1";
                header += DateTime.Now.ToString("ddMMyyyyHHmmss");
                header += Utils.FormatCode("", "0", 6);
                header += "081";
                header += "01600";
                header += Utils.FormatCode("", " ", 69);
                header = Utils.SubstituiCaracteresEspeciais(header);
                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
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
                        trailer = GerarTrailerRemessa240(numeroRegistro);
                        break;
                    case TipoArquivo.Cnab400:
                        trailer = GerarTrailerRemessa400(numeroRegistro, cedente);
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

        public string GerarTrailerRemessa240(int numeroRegistro)
        {
            try
            {
                var complemento = new string(' ', 205);

                var trailer = "74899999";
                trailer += Utils.FormatCode("", " ", 9);
                trailer += Utils.FormatCode("", 6);
                trailer += Utils.FormatCode(numeroRegistro.ToString(), 6);
                trailer += Utils.FormatCode("", 6);
                trailer += complemento;

                trailer = Utils.SubstituiCaracteresEspeciais(trailer);

                return trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        #region Métodos de Leitura do Arquivo de Retorno

        /*
         * Substituído Método de Leitura do Retorno pelo Interpretador de EDI;
        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                DetalheRetorno detalhe = new DetalheRetorno(registro);

                int idRegistro = Utils.ToInt32(registro.Substring(0, 1));
                detalhe.IdentificacaoDoRegistro = idRegistro;

                detalhe.NossoNumero = registro.Substring(47, 15);

                int codigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));
                detalhe.CodigoOcorrencia = codigoOcorrencia;

                //Data Ocorrência no Banco
                int dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                detalhe.SeuNumero = registro.Substring(116, 10);

                int dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));

                decimal valorTitulo = Convert.ToUInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;

                detalhe.EspecieTitulo = registro.Substring(174, 1);

                decimal despeasaDeCobranca = Convert.ToUInt64(registro.Substring(175, 13));
                detalhe.DespesaDeCobranca = despeasaDeCobranca / 100;

                decimal outrasDespesas = Convert.ToUInt64(registro.Substring(188, 13));
                detalhe.OutrasDespesas = outrasDespesas / 100;

                decimal abatimentoConcedido = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.Abatimentos = abatimentoConcedido / 100;

                decimal descontoConcedido = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = descontoConcedido / 100;

                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;

                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;

                int dataCredito = Utils.ToInt32(registro.Substring(328, 8));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("####-##-##"));

                detalhe.MotivosRejeicao = registro.Substring(318, 10);

                detalhe.NomeSacado = registro.Substring(19, 5);
                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }
        */

        #endregion Métodos de Leitura do Arquivo de Retorno

        public int Mod10Sicredi(string seq)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int s = 0, p = 2, b = 2;

            for (var i = seq.Length - 1; i >= 0; i--)
            {
                var r = Convert.ToInt32(seq.Substring(i, 1)) * p;

                if (r > 9)
                    r = SomaDezena(r);

                s = s + r;

                if (p < b)
                    p++;
                else
                    p--;
            }

            var d = Multiplo10(s);
            return d;
        }

        public int SomaDezena(int dezena)
        {
            var d = dezena.ToString();
            var d1 = Convert.ToInt32(d.Substring(0, 1));
            var d2 = Convert.ToInt32(d.Substring(1));
            return d1 + d2;
        }

        public int DigSicredi(string seq)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int s = 0, p = 2, b = 9;

            for (var i = seq.Length - 1; i >= 0; i--)
            {
                s = s + Convert.ToInt32(seq.Substring(i, 1)) * p;

                if (p < b)
                    p = p + 1;
                else
                    p = 2;
            }

            var d = 11 - s % 11;

            if (d > 9)
                d = 0;

            return d;
        }

        public string DigNossoNumeroSicredi(Boleto boleto)
        {
            var agencia = boleto.Cedente.ContaBancaria.Agencia;     //código da cooperativa de crédito/agência beneficiária (aaaa)
            var posto = boleto.Cedente.ContaBancaria.DigitoAgencia; //código do posto beneficiário (pp)
            var cedente = boleto.Cedente.Codigo;                    //código do beneficiário (ccccc)
            var nossoNumero = boleto.NossoNumero;                   //ano atual (yy), indicador de geração do nosso número (b) e o número seqüencial do beneficiário (nnnnn);

            var seq = string.Concat(agencia, posto, cedente, nossoNumero); // = aaaappcccccyybnnnnn

            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            //Atribui os pesos de {2..9}
            int s = 0, p = 2, b = 9;

            for (var i = seq.Length - 1; i >= 0; i--)
            {
                s = s + Convert.ToInt32(seq.Substring(i, 1)) * p;

                if (p < b)
                    p = p + 1;
                else
                    p = 2;
            }
            var d = 11 - s % 11;

            if (d > 9)
                d = 0;

            return d.ToString();
        }

        /// <summary>
        /// Efetua as Validações dentro da classe Boleto, para garantir a geração da remessa
        /// </summary>
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

        #region CNAB 400

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
                        #region Validações da Remessa que deverão estar preenchidas quando SICREDI

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

                        //Comentado, pois sempre vai utilizar a opção com registro.
                        //if (string.IsNullOrEmpty(boleto.Remessa.TipoDocumento))
                        //{
                        //    // Para o Sicredi, defini o Tipo de Documento sendo: 
                        //    // A = 'A' - SICREDI com Registro 
                        //    // C = 'C' - SICREDI sem Registro

                        //    vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Tipo Documento!", Environment.NewLine);
                        //    vRetorno = false;
                        //}

                        //if (!boleto.Remessa.TipoDocumento.Equals("A") && !boleto.Remessa.TipoDocumento.Equals("C"))
                        //{
                        //    vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, 
                        //        "; Remessa: Tipo de Documento Inválido! Deverão ser: A = SICREDI com Registro; C = SICREDI sem Registro;", Environment.NewLine);
                        //    vRetorno = false;
                        //}
                        //else if (boleto.Remessa.TipoDocumento.Equals("06") && !String.IsNullOrEmpty(boleto.NossoNumero))
                        //{
                        //    //Para o "Remessa.TipoDocumento = "06", não poderá ter NossoNumero Gerado!
                        //    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Não pode existir NossoNumero para o Tipo Documento '06 - cobrança escritural'!", Environment.NewLine);
                        //    vRetorno = false;
                        //}

                        if (!boleto.EspecieDocumento.Codigo.Equals("A") && //A - Duplicata Mercantil por Indicação
                            !boleto.EspecieDocumento.Codigo.Equals("B") && //B - Duplicata Rural;
                            !boleto.EspecieDocumento.Codigo.Equals("C") && //C - Nota Promissória;
                            !boleto.EspecieDocumento.Codigo.Equals("D") && //D - Nota Promissória Rural;
                            !boleto.EspecieDocumento.Codigo.Equals("E") && //E - Nota de Seguros;
                            !boleto.EspecieDocumento.Codigo.Equals("F") && //G – Recibo;
                            !boleto.EspecieDocumento.Codigo.Equals("H") && //H - Letra de Câmbio;
                            !boleto.EspecieDocumento.Codigo.Equals("I") && //I - Nota de Débito;
                            !boleto.EspecieDocumento.Codigo.Equals("J") && //J - Duplicata de Serviço por Indicação;
                            !boleto.EspecieDocumento.Codigo.Equals("O") && //O – Boleto Proposta
                            !boleto.EspecieDocumento.Codigo.Equals("K")) //K – Outros.
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, 
                                "; Remessa: Informe o Código da EspécieDocumento! São Aceitas:{A,B,C,D,E,F,H,I,J,O,K}", Environment.NewLine);
                            vRetorno = false;
                        }

                        if (!boleto.Sacado.CpfCnpj.Length.Equals(11) && !boleto.Sacado.CpfCnpj.Length.Equals(14))
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Cpf/Cnpj diferente de 11/14 caracteres!", Environment.NewLine);
                            vRetorno = false;
                        }

                        if (boleto.NossoNumero.Length != 8)
                        {
                            //Apenas 8 caracteres sem boleto.DigitoNossoNumero.
                            vMsg += "Boleto: " + boleto.NumeroDocumento + Environment.NewLine + "Remessa: O Nosso Número diferente de 8 caracteres: " + 
                                boleto.NossoNumero + Environment.NewLine;
                            vRetorno = false;
                        }

                        #endregion
                    }

                    #endregion
                }

            mensagem = vMsg;
            return vRetorno;
        }

        public string GerarHeaderRemessaCnab400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0001, 001, 0, "0", ' '));                             //001-001
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0002, 001, 0, "1", ' '));                             //002-002
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0003, 007, 0, "REMESSA", ' '));                       //003-009
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0010, 002, 0, "01", ' '));                            //010-011
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0012, 015, 0, "COBRANCA", ' '));                      //012-026
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0027, 005, 0, cedente.Codigo, ' '));                  //027-031
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0032, 014, 0, cedente.CpfCnpj, ' '));                 //032-045
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0046, 031, 0, "", ' '));                              //046-076
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0077, 003, 0, "748", ' '));                           //077-079
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0080, 015, 0, "SICREDI", ' '));                       //080-094
                reg.CamposEdi.Add(new CampoEdi(Dado.DataAAAAMMDD_________, 0095, 008, 0, DateTime.Now, ' '));                    //095-102
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0103, 008, 0, "", ' '));                              //103-110
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0111, 007, 0, numeroArquivoRemessa.ToString(), '0')); //111-117
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0118, 273, 0, "", ' '));                              //118-390
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0391, 004, 0, "2.00", ' '));                          //391-394
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0395, 006, 0, "000001", ' '));                        //395-400

                reg.CodificarLinha();

                return Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        public string GerarDetalheRemessaCnab400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

            return GerarDetalheRemessaCnab400_A(boleto, numeroRegistro, tipoArquivo);
        }

        public string GerarDetalheRemessaCnab400_A(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0001, 001, 0, "1", ' '));             //001-001
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0002, 001, 0, "A", ' '));             //002-002  'A' - SICREDI com Registro, cobrança
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0003, 001, 0, boleto.Carteira, ' ')); //003-003  'A' - Simples, carteira.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0004, 001, 0, "A", ' '));             //004-004  'A' – Normal, tipo de impressão.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0005, 012, 0, string.Empty, ' '));    //005-016
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0017, 001, 0, "A", ' '));             //017-017  Tipo de moeda: 'A' - REAL
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0018, 001, 0, "A", ' '));             //018-018  Tipo de desconto: 'A' - VALOR
                                                                                                                                    
                #region Código de Juros

                var codJuros = "A";
                decimal valorOuPercJuros = 0;

                if (boleto.JurosMora > 0)
                {
                    codJuros = "A";
                    valorOuPercJuros = boleto.JurosMora;
                }
                else if (boleto.PercJurosMora > 0)
                {
                    codJuros = "B";
                    valorOuPercJuros = boleto.PercJurosMora;
                }

                #endregion                                                                                                                                   
                                                                                                                                    
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0019, 001, 0, codJuros, ' '));        //019-019  Tipo de juros: 'A' - VALOR / 'B' PERCENTUAL
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0020, 028, 0, string.Empty, ' '));    //020-047

                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0048, 009, 0, boleto.NossoNumero + boleto.DigitoNossoNumero, '0'));//048-056
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0057, 006, 0, string.Empty, ' '));                                 //057-062
                reg.CamposEdi.Add(new CampoEdi(Dado.DataAAAAMMDD_________, 0063, 008, 0, boleto.DataProcessamento, ' '));                     //063-070
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0071, 001, 0, boleto.OutrosDadosAlterados, ' '));                  //071-071 Quando remessa = '31', alteração de outros dados.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0072, 001, 0, boleto.Postagem ? "S" : "N", ' '));                  //072-072 'S' Postar, 'N' - Não Postar e remeter para o beneficiário
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0073, 001, 0, string.Empty, ' '));                                 //073-073
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0074, 001, 0, boleto.Postagem ? "A" : "B", ' '));                  //074-074 'A' - Impressão feita pelo banco, 'B' – Impressão é feita pelo Beneficiário
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0075, 002, 0, 0, '0'));                                            //075-076
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0077, 002, 0, 0, '0'));                                            //077-078
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0079, 004, 0, string.Empty, ' '));                                 //079-082
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0083, 010, 2, 0, '0'));                                            //083-092 Valor por dia de antecipação.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0093, 004, 2, boleto.PercMulta, '0'));                             //093-096
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0097, 012, 0, string.Empty, ' '));                                 //097-108
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0109, 002, 0, boleto.Remessa.CodigoOcorrencia, ' '));              //109-110 01 - Cadastro de título, etc;
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0111, 010, 0, boleto.NumeroDocumento, ' '));                       //111-120
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));                        //121-126
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));                           //127-139
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0140, 009, 0, string.Empty, ' '));                                 //140-148
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0149, 001, 0, boleto.EspecieDocumento.Codigo, ' '));               //149-149
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));                                //150-150
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAA___________, 0151, 006, 0, boleto.DataProcessamento, ' '));                     //151-156

                #region Instruções de protesto

                var vInstrucao1 = "00"; //1ª instrução (2, N) Caso Queira colocar um cod de uma instrução. ver no Manual caso nao coloca 00
                var vInstrucao2 = "00"; //2ª instrução (2, N) Caso Queira colocar um cod de uma instrução. ver no Manual caso nao coloca 00

                foreach (Instrucao instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Sicredi)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Sicredi.AlteracaoOutrosDados_CancelamentoProtestoAutomatico:
                            vInstrucao1 = "00";
                            vInstrucao2 = "00";
                            break;
                        case EnumInstrucoes_Sicredi.PedidoProtesto:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = Utils.FitStringLength(instrucao.Dias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                    }
                }

                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0157, 002, 0, vInstrucao1, '0'));                          //157-158
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0159, 002, 0, vInstrucao2, '0'));                          //159-160
                
                #endregion

                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0161, 013, 2, valorOuPercJuros, '0'));                      //161-173 Valor/% de juros por dia de atraso
                
                #region DataDesconto

                var vDataDesconto = "000000";
                if (!boleto.DataDesconto.Equals(DateTime.MinValue))
                    vDataDesconto = boleto.DataDesconto.ToString("ddMMyy");

                #endregion

                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0174, 006, 0, vDataDesconto, '0'));                         //174-179
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0180, 013, 2, boleto.ValorDesconto, '0'));                  //180-192
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0193, 013, 0, 0, '0'));                                     //193-205
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0206, 013, 2, boleto.Abatimento, '0'));                     //206-218
                
                #region Regra Tipo de Inscrição Sacado

                var vCpfCnpjSac = "0";
                if (boleto.Sacado.CpfCnpj.Length.Equals(11))
                    vCpfCnpjSac = "1"; //Cpf é sempre 11;
                else if (boleto.Sacado.CpfCnpj.Length.Equals(14))
                    vCpfCnpjSac = "2"; //Cnpj é sempre 14;

                #endregion
                
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0219, 001, 0, vCpfCnpjSac, '0'));                           //219-219
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0220, 001, 0, "0", '0'));                                   //220-220
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0221, 014, 0, boleto.Sacado.CpfCnpj, '0'));                 //221-234
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0235, 040, 0, boleto.Sacado.Nome.ToUpper(), ' '));          //235-274
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0275, 040, 0, boleto.Sacado.Endereco.End.ToUpper(), ' '));  //275-314
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0315, 005, 0, 0, '0'));                                     //315-319
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0320, 006, 0, 0, '0'));                                     //320-325
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0326, 001, 0, string.Empty, ' '));                          //326-326
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0327, 008, 0, boleto.Sacado.Endereco.Cep, '0'));            //327-334
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0335, 005, 1, 0, '0'));                                     //335-339
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0340, 014, 0, string.Empty, ' '));                          //340-353
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0354, 041, 0, string.Empty, ' '));                          //354-394
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                        //395-400

                reg.CodificarLinha();

                var detalhe = Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        public string GerarTrailerRemessa400(int numeroRegistro,  Cedente cedente)
        {
            try
            {
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));                         //001-001
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0002, 001, 0, "1", ' '));                         //002-002
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0003, 003, 0, "748", ' '));                       //003-006
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0006, 005, 0, cedente.Codigo, ' '));              //006-010                
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0011, 384, 0, string.Empty, ' '));                //011-394
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));              //395-400
                
                reg.CodificarLinha();
                
                var vLinha = reg.LinhaRegistro;
                var trailer = Utils.SubstituiCaracteresEspeciais(vLinha);
                
                return trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                var reg = new TRegistroEDI_Sicredi_Retorno();
                reg.LinhaRegistro = registro;

                reg.DecodificarLinha();

                //Passa para o detalhe as propriedades de reg;
                var detalhe = new DetalheRetorno(registro);
                detalhe.IdentificacaoDoRegistro = Utils.ToInt32(reg.IdentificacaoRegDetalhe);
                //Filler1
                //TipoCobranca
                //CodigoPagadorAgenciaBeneficiario
                detalhe.NomeSacado = reg.CodigoPagadorJuntoAssociado;
                //BoletoDDA
                //Filler2
                #region NossoNumeroSicredi
                detalhe.NossoNumeroComDV = reg.NossoNumeroSicredi;
                detalhe.NossoNumero = reg.NossoNumeroSicredi.Substring(0, reg.NossoNumeroSicredi.Length - 1); //Nosso Número sem o DV!
                detalhe.DACNossoNumero = reg.NossoNumeroSicredi.Substring(reg.NossoNumeroSicredi.Length - 1); //DV do Nosso Numero
                #endregion
                //Filler3
                detalhe.CodigoOcorrencia = Utils.ToInt32(reg.Ocorrencia);
                var dataOcorrencia = Utils.ToInt32(reg.DataOcorrencia);
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                detalhe.NumeroDocumento = reg.SeuNumero;
                //Filler4
                if (!string.IsNullOrEmpty(reg.DataVencimento))
                {
                    var dataVencimento = Utils.ToInt32(reg.DataVencimento);
                    detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                }
                decimal valorTitulo = Convert.ToInt64(reg.ValorTitulo);
                detalhe.ValorTitulo = valorTitulo / 100;
                //Filler5
                //Despesas de cobrança para os Códigos de Ocorrência (Valor Despesa)
                if (!string.IsNullOrEmpty(reg.DespesasCobranca))
                {
                    decimal valorDespesa = Convert.ToUInt64(reg.DespesasCobranca);
                    detalhe.ValorDespesa = valorDespesa / 100;
                }
                //Outras despesas Custas de Protesto (Valor Outras Despesas)
                if (!string.IsNullOrEmpty(reg.DespesasCustasProtesto))
                {
                    decimal valorOutrasDespesas = Convert.ToUInt64(reg.DespesasCustasProtesto);
                    detalhe.ValorOutrasDespesas = valorOutrasDespesas / 100;
                }
                //Filler6
                //Abatimento Concedido sobre o Título (Valor Abatimento Concedido)
                decimal valorAbatimento = Convert.ToUInt64(reg.AbatimentoConcedido);
                detalhe.ValorAbatimento = valorAbatimento / 100;
                //Desconto Concedido (Valor Desconto Concedido)
                decimal valorDesconto = Convert.ToUInt64(reg.DescontoConcedido);
                detalhe.Descontos = valorDesconto / 100;
                //Valor Pago
                decimal valorPago = Convert.ToUInt64(reg.ValorEfetivamentePago);
                detalhe.ValorPago = valorPago / 100;
                //Juros Mora
                decimal jurosMora = Convert.ToUInt64(reg.JurosMora);
                detalhe.JurosMora = jurosMora / 100;
                //Filler7
                //SomenteOcorrencia19
                //Filler8
                detalhe.MotivoCodigoOcorrencia = reg.MotivoOcorrencia;
                var dataCredito = Utils.ToInt32(reg.DataPrevistaLancamentoContaCorrente);
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("####-##-##"));
                //Filler9
                detalhe.NumeroSequencial = Utils.ToInt32(reg.NumeroSequencialRegistro);
                //
                #region NAO RETORNADOS PELO SICREDI
                //detalhe.Especie = reg.TipoDocumento; //Verificar Espécie de Documentos...
                detalhe.OutrosCreditos = 0;
                detalhe.OrigemPagamento = string.Empty;
                detalhe.MotivoCodigoOcorrencia = reg.MotivoOcorrencia;
                //
                detalhe.IOF = 0;
                //Motivos das Rejeições para os Códigos de Ocorrência
                detalhe.MotivosRejeicao = string.Empty;
                //Número do Cartório
                detalhe.NumeroCartorio = 0;
                //Número do Protocolo
                detalhe.NumeroProtocolo = string.Empty;

                detalhe.CodigoInscricao = 0;
                detalhe.NumeroInscricao = string.Empty;
                detalhe.Agencia = 0;
                detalhe.Conta = 0;
                detalhe.DACConta = 0;

                detalhe.NumeroControle = string.Empty;
                detalhe.IdentificacaoTitulo = string.Empty;
                //Banco Cobrador
                detalhe.CodigoBanco = 0;
                //Agência Cobradora
                detalhe.AgenciaCobradora = 0;
                #endregion
                
                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                var header = new HeaderRetorno(registro);
                header.TipoRegistro = Utils.ToInt32(registro.Substring(000, 1));
                header.CodigoRetorno = Utils.ToInt32(registro.Substring(001, 1));
                header.LiteralRetorno = registro.Substring(002, 7);
                header.CodigoServico = Utils.ToInt32(registro.Substring(009, 2));
                header.LiteralServico = registro.Substring(011, 15);
                header.Agencia = Utils.ToInt32(registro.Substring(026, 5));
                header.ComplementoRegistro2 = registro.Substring(031, 14);
                header.CodigoBanco = Utils.ToInt32(registro.Substring(076, 3));
                header.NomeBanco = registro.Substring(079, 15);
                header.DataGeracao = Utils.ToDateTime(Utils.ToInt32(registro.Substring(094, 8)).ToString("##-##-##"));
                header.NumeroSequencialArquivoRetorno = Utils.ToInt32(registro.Substring(110, 7));
                header.Versao = registro.Substring(390, 5);
                header.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #endregion

        public override long ObterNossoNumeroSemConvenioOuDigitoVerificador(long convenio, string nossoNumero)
        {
            long num;
            if (nossoNumero.Length >= 8 && long.TryParse(nossoNumero.Substring(0, 8), out num))
                return num;
            
            throw new BoletoNetException("Nosso número é inválido!");
        }
    }
}