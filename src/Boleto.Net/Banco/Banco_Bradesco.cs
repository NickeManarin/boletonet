using System;
using System.Linq;
using System.Web.UI;
using BoletoNet.Util;

[assembly: WebResource("BoletoNet.Imagens.237.jpg", "image/jpg")]

namespace BoletoNet
{
    internal sealed class Banco_Bradesco : AbstractBanco, IBanco
    {
        private string _dacNossoNumero = string.Empty;
        private int _dacBoleto = 0;

        /// <summary>
        /// Classe responsavel em criar os campos do Banco Banco_Bradesco.
        /// </summary>
        internal Banco_Bradesco()
        {
            Codigo = 237;
            Digito = "2";
            Nome = "Bradesco";
        }

        /// <summary>
        /// Calcula o digito do Nosso Numero
        /// </summary>
        public string CalcularDigitoNossoNumero(Boleto boleto)
        {
            return Mod11Bradesco(boleto.Carteira + Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true), 7);
        }

        #region IBanco Members

        /// <summary>
        /// A linha digitável será composta por cinco campos:
        ///      1º campo
        ///          composto pelo código de Banco, código da moeda, as cinco primeiras posições do campo 
        ///          livre e o dígito verificador deste campo;
        ///      2º campo
        ///          composto pelas posições 6ª a 15ª do campo livre e o dígito verificador deste campo;
        ///      3º campo
        ///          composto pelas posições 16ª a 25ª do campo livre e o dígito verificador deste campo;
        ///      4º campo
        ///          composto pelo dígito verificador do código de barras, ou seja, a 5ª posição do código de 
        ///          barras;
        ///      5º campo
        ///          Composto pelo fator de vencimento com 4(quatro) caracteres e o valor do documento com 10(dez) caracteres, sem separadores e sem edição.
        /// 
        /// </summary>
        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //BBBMC.CCCCD1 CCCCC.CCCCCD2 CCCCC.CCCCCD3 D4 FFFFVVVVVVVVVV
            
            #region Campo 1

            var bbb = boleto.CodigoBarra.Codigo.Substring(0, 3);
            var m = boleto.CodigoBarra.Codigo.Substring(3, 1);
            var ccccc = boleto.CodigoBarra.Codigo.Substring(19, 5);
            var d1 = Mod10(bbb + m + ccccc).ToString();

            var grupo1 = string.Format("{0}{1}{2}.{3}{4} ", bbb, m, ccccc.Substring(0, 1), ccccc.Substring(1, 4), d1);

            #endregion Campo 1

            #region Campo 2

            var cccccccccc2 = boleto.CodigoBarra.Codigo.Substring(24, 10);
            var d2 = Mod10(cccccccccc2).ToString();

            var grupo2 = string.Format("{0}.{1}{2} ", cccccccccc2.Substring(0, 5), cccccccccc2.Substring(5, 5), d2);

            #endregion Campo 2

            #region Campo 3

            var cccccccccc3 = boleto.CodigoBarra.Codigo.Substring(34, 10);
            var d3 = Mod10(cccccccccc3).ToString();

            var grupo3 = string.Format("{0}.{1}{2} ", cccccccccc3.Substring(0, 5), cccccccccc3.Substring(5, 5), d3);

            #endregion Campo 3

            #region Campo 4

            var d4 = _dacBoleto.ToString();

            var grupo4 = string.Format("{0} ", d4);

            #endregion Campo 4

            #region Campo 5

            //string FFFF = boleto.CodigoBarra.Codigo.Substring(5, 4);//FatorVencimento(boleto).ToString() ;
            var ffff = FatorVencimento(boleto).ToString();

            //if (boleto.Carteira == "06" && !Utils.DataValida(boleto.DataVencimento))
            //    FFFF = "0000";

            var vvvvvvvvvv = boleto.ValorBoleto.ToString("N2").Replace(",", "").Replace(".", "");
            vvvvvvvvvv = Utils.FormatCode(vvvvvvvvvv, 10);

            //if (Utils.ToInt64(VVVVVVVVVV) == 0)
            //    VVVVVVVVVV = "000";

            var grupo5 = string.Format("{0}{1}", ffff, vvvvvvvvvv);

            #endregion Campo 5

            boleto.CodigoBarra.LinhaDigitavel = grupo1 + grupo2 + grupo3 + grupo4 + grupo5;
        }

        /// <summary>
        ///	O código de barra para cobrança contém 44 posições dispostas da seguinte forma:
        ///    01 a 03 - 3 - Identificação  do  Banco
        ///    04 a 04 - 1 - Código da Moeda
        ///    05 a 05 – 1 - Dígito verificador do Código de Barras
        ///    06 a 09 - 4 - Fator de vencimento
        ///    10 a 19 - 10 - Valor
        ///    20 a 44 – 25 - Campo Livre
        /// </summary>
        public override void FormataCodigoBarra(Boleto boleto)
        {
            var valorBoleto = boleto.ValorBoleto.ToString("N2").Replace(",", "").Replace(".", "");
            valorBoleto = Utils.FormatCode(valorBoleto, 10);

            if (boleto.Carteira == "02" || boleto.Carteira == "03" || boleto.Carteira == "09" || boleto.Carteira == "19" || boleto.Carteira == "26") // Com registro
            {
                boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo, boleto.Moeda,
                FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));
            }
            else if (boleto.Carteira == "06" || boleto.Carteira == "16" || boleto.Carteira == "25") // Sem Registro
            {
                if (boleto.ValorBoleto == 0)
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}0000{2}{3}", Codigo, boleto.Moeda,
                        valorBoleto, FormataCampoLivre(boleto));
                }
                else
                {
                    boleto.CodigoBarra.Codigo = string.Format("{0}{1}{2}{3}{4}", Codigo, boleto.Moeda,
                        FatorVencimento(boleto), valorBoleto, FormataCampoLivre(boleto));
                }
            }
            else
            {
                throw new NotImplementedException("Carteira ainda não implementada.");
            }
            
            _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9);

            boleto.CodigoBarra.Codigo = Strings.Left(boleto.CodigoBarra.Codigo, 4) + _dacBoleto + Strings.Right(boleto.CodigoBarra.Codigo, 39);
        }

        ///<summary>
        /// Campo Livre
        ///    20 a 23 -  4 - Agência Cedente (Sem o digito verificador,completar com zeros a esquerda quandonecessário)
        ///    24 a 25 -  2 - Carteira
        ///    26 a 36 - 11 - Número do Nosso Número(Sem o digito verificador)
        ///    37 a 43 -  7 - Conta do Cedente (Sem o digito verificador,completar com zeros a esquerda quando necessário)
        ///    44 a 44	- 1 - Zero            
        ///</summary>
        public string FormataCampoLivre(Boleto boleto)
        {
            var formataCampoLivre = string.Format("{0}{1}{2}{3}{4}", boleto.Cedente.ContaBancaria.Agencia, boleto.Carteira,
                boleto.NossoNumero, boleto.Cedente.ContaBancaria.Conta, "0");

            return formataCampoLivre;
        }


        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função ainda não implementada.");
        }
        
        public override void FormataNossoNumero(Boleto boleto)
        {
            boleto.NossoNumero = string.Format("{0}/{1}-{2}", Utils.FormatCode(boleto.Carteira, 3), boleto.NossoNumero, boleto.DigitoNossoNumero);
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            if (boleto.Carteira != "02" && boleto.Carteira != "03" && boleto.Carteira != "06" && boleto.Carteira != "09" && boleto.Carteira != "16" && boleto.Carteira != "19" && boleto.Carteira != "25" && boleto.Carteira != "26")
                throw new Exception("Carteira não implementada. Carteiras implementadas 02, 03, 06, 09, 16, 19, 25, 26.");

            //O valor é obrigatório para a carteira 03
            if (boleto.Carteira == "03")
            {
                if (boleto.ValorBoleto == 0)
                    throw new Exception("Para a carteira 03, o valor do boleto não pode ser igual a zero");
            }

            //O valor é obrigatório para a carteira 09
            if (boleto.Carteira == "09")
            {
                if (boleto.ValorBoleto == 0)
                    throw new Exception("Para a carteira 09, o valor do boleto não pode ser igual a zero");
            }

            //Verifica se o nosso número é válido
            if (boleto.NossoNumero.Length > 11)
                throw new Exception("A quantidade de dígitos do nosso número, são 11 números.");

            if (boleto.NossoNumero.Length < 11)
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 11);

            //Verificar se a Agencia esta correta
            if (boleto.Cedente.ContaBancaria.Agencia.Length > 4)
                throw new Exception("A quantidade de dígitos da Agência deve ser de 4 dígitos. Foram informados " + boleto.Cedente.ContaBancaria.Agencia.Length + " dígitos.");

            if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                boleto.Cedente.ContaBancaria.Agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

            //Verificar se a Conta esta correta
            if (boleto.Cedente.ContaBancaria.Conta.Length > 7)
                throw new Exception("A quantidade de dígitos da Conta deve ser de 7 dígitos. Foram informados " + boleto.Cedente.ContaBancaria.Conta.Length + " dígitos.");

            if (boleto.Cedente.ContaBancaria.Conta.Length < 7)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7);

            //Verifica se data do processamento é valida
            //if (boleto.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;
            
            //Verifica se data do documento é valida
            //if (boleto.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            boleto.QuantidadeMoeda = 0;

            // Atribui o nome do banco ao local de pagamento
            //if (string.IsNullOrEmpty(boleto.LocalPagamento))
            boleto.LocalPagamento = "Pagável preferencialmente na Rede Bradesco ou Bradesco Expresso.";

            // Calcula o DAC do Nosso Número
            _dacNossoNumero = CalcularDigitoNossoNumero(boleto);
            boleto.DigitoNossoNumero = _dacNossoNumero;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            //FormataNossoNumero(boleto);
        }

        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            var vRetorno = true;
            var vMsg = string.Empty;
            ////IMPLEMENTACAO PENDENTE...
            mensagem = vMsg;
            return vRetorno;
        }

        #endregion

        /// <summary>
        /// Verifica o tipo de ocorrência para o arquivo remessa
        /// </summary>
        private string Ocorrencia(string codigo)
        {
            switch (codigo)
            {
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "06":
                    return "06-Liquidação normal";
                case "09":
                    return "09-Baixado Automaticamente via Arquivo";
                case "10":
                    return "10-Baixado conforme instruções da Agência";
                case "11":
                    return "11-Em Ser - Arquivo de Títulos pendentes";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Vencimento Alterado";
                case "15":
                    return "15-Liquidação em Cartório";
                case "17":
                    return "17-Liquidação após baixa ou Título não registrado";
                case "18":
                    return "18-Acerto de Depositária";
                case "19":
                    return "19-Confirmação Recebimento Instrução de Protesto";
                case "20":
                    return "20-Confirmação Recebimento Instrução Sustação de Protesto";
                case "21":
                    return "21-Acerto do Controle do Participante";
                case "23":
                    return "23-Entrada do Título em Cartório";
                case "24":
                    return "24-Entrada rejeitada por CEP Irregular";
                case "27":
                    return "27-Baixa Rejeitada";
                case "28":
                    return "28-Débito de tarifas/custas";
                case "30":
                    return "30-Alteração de Outros Dados Rejeitados";
                case "32":
                    return "32-Instrução Rejeitada";
                case "33":
                    return "33-Confirmação Pedido Alteração Outros Dados";
                case "34":
                    return "34-Retirado de Cartório e Manutenção Carteira";
                case "35":
                    return "35-Desagendamento ) débito automático";
                case "68":
                    return "68-Acerto dos dados ) rateio de Crédito";
                case "69":
                    return "69-Cancelamento dos dados ) rateio";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Verifica o código do motivo da rejeição informada pelo banco
        /// </summary>
        public string MotivoRejeicao(string codigo)
        {
            switch (codigo)
            {
                case "02":
                    return "02-Código do registro detalhe inválido";
                case "03":
                    return "03-Código da ocorrência inválida";
                case "04":
                    return "04-Código de ocorrência não permitida para a carteira";
                case "05":
                    return "05-Código de ocorrência não numérico";
                case "07":
                    return "07-Agência/conta/Digito - Inválido";
                case "08":
                    return "08-Nosso número inválido";
                case "09":
                    return "09-Nosso número duplicado";
                case "10":
                    return "10-Carteira inválida";
                case "16":
                    return "16-Data de vencimento inválida";
                case "18":
                    return "18-Vencimento fora do prazo de operação";
                case "20":
                    return "20-Valor do Título inválido";
                case "21":
                    return "21-Espécie do Título inválida";
                case "22":
                    return "22-Espécie não permitida para a carteira";
                case "24":
                    return "24-Data de emissão inválida";
                case "38":
                    return "38-Prazo para protesto inválido";
                case "44":
                    return "44-Agência Cedente não prevista";
                case "50":
                    return "50-CEP irregular - Banco Correspondente";
                case "63":
                    return "63-Entrada para Título já cadastrado";
                case "68":
                    return "68-Débito não agendado - erro nos dados de remessa";
                case "69":
                    return "69-Débito não agendado - Sacado não consta no cadastro de autorizante";
                case "70":
                    return "70-Débito não agendado - Cedente não autorizado pelo Sacado";
                case "71":
                    return "71-Débito não agendado - Cedente não participa da modalidade de débito automático";
                case "72":
                    return "72-Débito não agendado - Código de moeda diferente de R$";
                case "73":
                    return "73-Débito não agendado - Data de vencimento inválida";
                case "74":
                    return "74-Débito não agendado - Conforme seu pedido, Título não registrado";
                case "75":
                    return "75-Débito não agendado - Tipo de número de inscrição do debitado inválido";
                default:
                    return "";
            }
        }

        private string Mod11Bradesco(string seq, int b)
        {
            #region Trecho do manual layout_cobranca_port.pdf do BRADESCO

            /* 
            Para o cálculo do dígito, será necessário acrescentar o número da carteira à esquerda antes do Nosso Número, 
            e aplicar o módulo 11, com base 7.
            Multiplicar cada algarismo que compõe o número pelo seu respectivo multiplicador (PESO).
            Os multiplicadores(PESOS) variam de 2 a 7.
            O primeiro dígito da direita para a esquerda deverá ser multiplicado por 2, o segundo por 3 e assim sucessivamente.
             
              Carteira   Nosso Numero
                ______   _________________________________________
                1    9   0   0   0   0   0   0   0   0   0   0   2
                x    x   x   x   x   x   x   x   x   x   x   x   x
                2    7   6   5   4   3   2   7   6   5   4   3   2
                =    =   =   =   =   =   =   =   =   =   =   =   =
                2 + 63 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 0 + 4 = 69

            O total da soma deverá ser dividido por 11: 69 / 11 = 6 tendo como resto = 3
            A diferença entre o divisor e o resto, será o dígito de autoconferência: 11 - 3 = 8 (dígito de auto-conferência)
            
            Se o resto da divisão for “1”, desprezar o cálculo de subtração e considerar o dígito como “P”. 
            Se o resto da divisão for “0”, desprezar o cálculo de subtração e considerar o dígito como “0”.
            */

            #endregion

            /* Variáveis
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int s = 0, p = 2;

            for (var i = seq.Length; i > 0; i--)
            {
                s = s + (Convert.ToInt32(seq.Mid(i, 1)) * p);

                if (p == b)
                    p = 2;
                else
                    p = p + 1;
            }

            var r = s % 11;

            if (r == 0)
                return "0";

            if (r == 1)
                return "P";

            return (11 - r).ToString();
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
                header.CodigoEmpresa = registro.Substring(026, 20);
                header.NomeEmpresa = registro.Substring(046, 30);
                header.CodigoBanco = Utils.ToInt32(registro.Substring(076, 3));
                header.NomeBanco = registro.Substring(079, 15);
                header.DataGeracao = Utils.ToDateTime(Utils.ToInt32(registro.Substring(094, 6)).ToString("##-##-##"));
                header.Densidade = Utils.ToInt32(registro.Substring(100, 8));
                header.NumeroSequencialArquivoRetorno = Utils.ToInt32(registro.Substring(108, 5));
                header.ComplementoRegistro2 = registro.Substring(113, 266);
                header.DataCredito = Utils.ToDateTime(Utils.ToInt32(registro.Substring(379, 6)).ToString("##-##-##"));
                header.ComplementoRegistro3 = registro.Substring(385, 9);
                header.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                var detalhe = new DetalheRetorno(registro);
                // Identificação do Registro ==> 001 a 001
                detalhe.IdentificacaoDoRegistro = Utils.ToInt32(registro.Substring(0, 1));

                //Tipo de Inscrição Empresa ==> 002 a 003
                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));

                //Nº Inscrição da Empresa ==> 004 a 017
                detalhe.NumeroInscricao = registro.Substring(3, 14);

                //Identificação da Empresa Cedente no Banco ==> 021 a 037
                detalhe.Agencia = Utils.ToInt32(registro.Substring(24, 6));
                detalhe.Conta = Utils.ToInt32(registro.Substring(30, 7));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(36, 1));

                //Nº Controle do Participante ==> 038 a 062
                detalhe.NumeroControle = registro.Substring(37, 25);

                //Identificação do Título no Banco ==> 071 a 082
                detalhe.NossoNumeroComDV = registro.Substring(70, 12);

                //Identificação do Título no Banco ==> 071 a 081
                detalhe.NossoNumero = registro.Substring(70, 11);//Sem o DV

                //Identificação do Título no Banco ==> 082 a 082
                detalhe.DACNossoNumero = registro.Substring(81, 1); //DV

                //Carteira ==> 108 a 108
                detalhe.Carteira = registro.Substring(107, 1);

                //Identificação de Ocorrência ==> 109 a 110
                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));

                //Descrição da ocorrência
                detalhe.DescricaoOcorrencia = Ocorrencia(registro.Substring(108, 2));

                //Data Ocorrência no Banco ==> 111 a 116
                var dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));

                //Número do Documento ==> 117 a 126
                detalhe.NumeroDocumento = registro.Substring(116, 10);

                //Identificação do Título no Banco ==> 127 a 146
                detalhe.IdentificacaoTitulo = registro.Substring(126, 20);

                //Data Vencimento do Título ==> 147 a 152
                var dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));

                //Valor do Título ==> 153 a 165
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;

                //Banco Cobrador ==> 166 a 168
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));

                //Agência Cobradora ==> 169 a 173
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 5));

                //Espécie do Título ==> 174 a 175
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));

                //Despesas de cobrança para os Códigos de Ocorrência (Valor Despesa) ==> 176 a 188
                decimal valorDespesa = Convert.ToUInt64(registro.Substring(175, 13));
                detalhe.ValorDespesa = valorDespesa / 100;

                //Outras despesas Custas de Protesto (Valor Outras Despesas) ==> 189 a 201
                decimal valorOutrasDespesas = Convert.ToUInt64(registro.Substring(188, 13));
                detalhe.ValorOutrasDespesas = valorOutrasDespesas / 100;

                //Juros Operação em Atraso ==> 202 a 214
                decimal OutrosCreditos = Convert.ToUInt64(registro.Substring(201, 13));
                detalhe.OutrosCreditos = OutrosCreditos / 100;

                // IOF ==> 215 a 227
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                detalhe.IOF = iof / 100;

                //Abatimento Concedido sobre o Título (Valor Abatimento Concedido) ==> 228 a 240
                decimal valorAbatimento = Convert.ToUInt64(registro.Substring(227, 13));
                detalhe.ValorAbatimento = valorAbatimento / 100;

                //Desconto Concedido (Valor Desconto Concedido) ==> 241 a 253
                decimal valorDesconto = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = valorDesconto / 100;

                //Valor Pago ==> 254 a 266
                decimal valorPago = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPago = valorPago / 100;

                //Juros Mora ==> 267 a 279
                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;

                //Outros Créditos ==> 280 a 292
                decimal outrosCreditos = Convert.ToUInt64(registro.Substring(279, 13));
                detalhe.OutrosCreditos = outrosCreditos / 100;

                //Motivo do Código de Ocorrência 19 (Confirmação de Instrução de Protesto) ==> 295 a 295
                detalhe.MotivoCodigoOcorrencia = registro.Substring(294, 1);

                // Data do Crédito ==> 296 a 301
                var dataCredito = Utils.ToInt32(registro.Substring(295, 6));
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));

                //Origem Pagamento ==> 302 a 304
                detalhe.OrigemPagamento = registro.Substring(301, 3);

                //Motivos das Rejeições para os Códigos de Ocorrência ==> 319 a 328
                detalhe.MotivosRejeicao = registro.Substring(318, 10);

                //Número do Cartório ==> 369 a 370
                detalhe.NumeroCartorio = Utils.ToInt32(registro.Substring(365, 2));

                //Número do Protocolo ==> 371 a 380
                detalhe.NumeroProtocolo = registro.Substring(365, 2);

                //Nome do Sacado
                detalhe.NomeSacado = "";

                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
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
                detalhe.IdentificacaoRegistro = Convert.ToInt32(registro.Substring(17, 2));
                detalhe.TipoInscricao = registro.Substring(19, 1);
                detalhe.NumeroInscricao = registro.Substring(20, 15);

                detalhe.NomeSacador = registro.Substring(35, 40);
                detalhe.EnderecoSacador = registro.Substring(75, 40);
                detalhe.BairroSacador = registro.Substring(115, 15);
                detalhe.CEPSacador = registro.Substring(130, 8);
                detalhe.CidadeSacador = registro.Substring(138, 15);
                detalhe.UFSacador = registro.Substring(153, 2);

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO Y.", ex);
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
                detalhe.Agencia = Convert.ToInt32(registro.Substring(17, 5));
                detalhe.DigitoAgencia = registro.Substring(22, 1);
                detalhe.Conta = Convert.ToInt32(registro.Substring(23, 12));
                detalhe.DigitoConta = registro.Substring(35, 1);

                detalhe.NossoNumero = registro.Substring(37, 20);
                detalhe.CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1));
                detalhe.NumeroDocumento = registro.Substring(58, 15);
                var dataVencimento = Convert.ToInt32(registro.Substring(73, 8));
                detalhe.DataVencimento = Convert.ToDateTime(dataVencimento.ToString("##-##-####"));
                decimal valorTitulo = Convert.ToInt64(registro.Substring(91, 15));
                detalhe.ValorTitulo = valorTitulo / 100;
                detalhe.IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                detalhe.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                detalhe.NumeroInscricao = registro.Substring(133, 15);
                detalhe.NomeSacado = registro.Substring(148, 40);
                decimal valorTarifas = Convert.ToUInt64(registro.Substring(198, 15));
                detalhe.ValorTarifas = valorTarifas / 100;
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
                var dataCredito = Convert.ToInt32(registro.Substring(145, 8));
                detalhe.DataCredito = Convert.ToDateTime(dataCredito.ToString("##-##-####"));
                var dataOcorrencia = Convert.ToInt32(registro.Substring(137, 8));
                detalhe.DataOcorrencia = Convert.ToDateTime(dataOcorrencia.ToString("##-##-####"));
                var dataOcorrenciaSacado = Convert.ToInt32(registro.Substring(157, 8));

                if (dataOcorrenciaSacado > 0)
                    detalhe.DataOcorrenciaSacado = Convert.ToDateTime(dataOcorrenciaSacado.ToString("##-##-####"));
                else
                    detalhe.DataOcorrenciaSacado = DateTime.Now;

                decimal jurosMultaEncargos = Convert.ToUInt64(registro.Substring(17, 15));
                detalhe.JurosMultaEncargos = jurosMultaEncargos / 100;
                decimal valorDescontoConcedido = Convert.ToUInt64(registro.Substring(32, 15));
                detalhe.ValorDescontoConcedido = valorDescontoConcedido / 100;
                decimal valorAbatimentoConcedido = Convert.ToUInt64(registro.Substring(47, 15));
                detalhe.ValorAbatimentoConcedido = valorAbatimentoConcedido / 100;
                decimal valorIofRecolhido = Convert.ToUInt64(registro.Substring(62, 15));
                detalhe.ValorIOFRecolhido = valorIofRecolhido / 100;
                decimal valorPagoPeloSacado = Convert.ToUInt64(registro.Substring(77, 15));
                detalhe.ValorPagoPeloSacado = valorPagoPeloSacado / 100;
                decimal valorLiquidoASerCreditado = Convert.ToUInt64(registro.Substring(92, 15));
                detalhe.ValorLiquidoASerCreditado = valorLiquidoASerCreditado / 100;
                decimal valorOutrasDespesas = Convert.ToUInt64(registro.Substring(107, 15));
                detalhe.ValorOutrasDespesas = valorOutrasDespesas / 100;

                decimal valorOutrosCreditos = Convert.ToUInt64(registro.Substring(122, 15));
                detalhe.ValorOutrosCreditos = valorOutrosCreditos / 100;

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }
        

        #region Header

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                var header = " ";

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {

                    case TipoArquivo.Cnab240:
                        header = GerarHeaderRemessaCnab240();
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

        private string GerarHeaderRemessaCnab240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        private string GerarHeaderRemessaCnab400(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var header = "01REMESSA01COBRANCA       ";  //Ok
                header += Utils.FitStringLength(cedente.Codigo, 20, 20, '0', 0, true, true, true);  //Código no banco, acesso escritural.
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper(); //Razão social.
                header += "237";                            //Ok.
                header += "BRADESCO       ";                //Ok.
                header += DateTime.Now.ToString("ddMMyy");  //Ok.
                header += "        ";                       //Ok.
                header += "MX";                             //Ok.
                header += Utils.FitStringLength(numeroArquivoRemessa.ToString(), 7, 7, '0', 0, true, true, true);   //Ok.
                header += new string(' ', 277);             //Ok.
                header += "000001";                         //Ok.

                header = Utils.SubstituiCaracteresEspeciais(header);

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        #endregion

        #region Detalhe

        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        detalhe = GerarDetalheRemessaCnab240();
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

        private string GerarDetalheRemessaCnab240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        private string GerarDetalheRemessaCnab400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                //Especificação (tamanho, tipo) A = Alfanumérico, N = Numérico

                var detalhe = "1";
                detalhe += "00000"; //Agencia de Debito            (5, N) Não Usado
                detalhe += " "; //Dig da Agencia                   (1, A) Não Usado
                detalhe += "00000"; //Razao da Conta Corrente      (5, N) Não Usado
                detalhe += "0000000"; //Conta Corrente             (7, N) Não Usado
                detalhe += " "; //Dig da Conta Corrente            (1, A) Não Usado

                //Identificação da Empresa Cedente no Banco (17, A)
                detalhe += "0";
                detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true);                          //Código da carteira (3)
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);     //Número da agência  (5)
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 7, 7, '0', 0, true, true, true);       //Conta Corrente     (7)
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true); //Dígito da conta    (1)
                
                detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false); //Uso livre da empresa (25, A)
                detalhe += "000"; //Código do Banco, só deve ser preenchido quando cliente cedente optar por "Débito Automático".

                //0 = sem multa, 2 = com multa (1, N)
                if (boleto.PercMulta > 0)
                {
                    detalhe += "2";
                    detalhe += Utils.FitStringLength(boleto.PercMulta.ApenasNumeros(), 4, 4, '0', 0, true, true, true); //Percentual Multa 9(2)V99 - (4)
                }
                else
                {
                    detalhe += "0";
                    detalhe += "0000";
                }

                //Identificação do Título no Banco (12, A)
                detalhe += Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true); //Nosso Número (11)
                detalhe += Utils.FitStringLength(boleto.DigitoNossoNumero, 1, 1, '0', 0, true, true, false); //Dígito do Nosso Número (1)
                //detalhe += Mod11Bradesco(boleto.Carteira.PadLeft(2, '0') + Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true), 7); // Digito de Auto Conferencia do Nosso Número (01)
                detalhe += "0000000000"; //Desconto Bonificação por dia (10, N)

                detalhe += boleto.Postagem ? "1" : "2"; //Condição para Emissão da Papeleta de Cobrança, 1 é quando o banco emite. (1, N) [193-193]
                detalhe += "N"; //Ident. se emite papeleta para Débito Automático (1, A)
                detalhe += new string(' ', 10); //Identificação da Operação do Banco (10, A) Em Branco

                //Somente deverá ser preenchido com a Letra “R”, se a Empresa participa da rotina de rateio de crédito, caso não participe, informar Branco.
                detalhe += " "; //Indicador de Rateio de Crédito (1, A)

                //1 = emite aviso, e assume o endereço do Sacado constante do Arquivo-Remessa;
                //2 = não emite aviso;
                //Diferente de 1 ou 2 = emite e assume o endereço do cliente debitado, constante do nosso cadastro.
                detalhe += "2"; //Endereçamento para Aviso do Débito Automático em Conta Corrente (1, N)
                detalhe += "  "; //Quantidade possíveis de pagamento, não usado (2, A)

                //Identificação ocorrência(2, N)
                /*
                    01..Remessa
                    02..Pedido de baixa
                    04..Concessão de abatimento
                    05..Cancelamento de abatimento concedido
                    06..Alteração de vencimento
                    07..Alteração do controle do participante
                    08..Alteração de seu número
                    09..Pedido de protesto
                    18..Sustar protesto e baixar Título
                    19..Sustar protesto e manter em carteira
                    31..Alteração de outros dados
                    35..Desagendamento do débito automático
                    68..Acerto nos dados do rateio de Crédito
                    69..Cancelamento do rateio de crédito.
                */

                if (string.IsNullOrEmpty(boleto.Remessa?.CodigoOcorrencia.Trim()))
                    detalhe += "01";
                else
                    detalhe += boleto.Remessa.CodigoOcorrencia.PadLeft(2, '0');

                detalhe += Utils.Right(boleto.NumeroDocumento.TrimStart(' '), 10, '0', true); //Nº do Documento (10, A)
                detalhe += boleto.DataVencimento.ToString("ddMMyy"); //Data do Vencimento do Título (10, N) DDMMAA
                detalhe += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 13, 13, '0', 0, true, true, true); //Valor do Título (13, N)

                detalhe += "000";   //Banco Encarregado da Cobrança, não usado (3, N)
                detalhe += "00000"; //Agência Depositária, não usado (5, N)

                /*Espécie de Título (2,N)
                *
                    01-Duplicata
                    02-Nota Promissória
                    03-Nota de Seguro
                    04-Cobrança Seriada
                    05-Recibo
                    10-Letras de Câmbio
                    11-Nota de Débito
                    12-Duplicata de Serv.
                    99-Outros
                */
                detalhe += Utils.FitStringLength(boleto.EspecieDocumento.Codigo, 2, 2, '0', 0, true, true, true);

                detalhe += "N"; //Identificação, sempre N (1, A)
                detalhe += boleto.DataProcessamento.ToString("ddMMyy"); //Data da emissão do Título (6, N) DDMMAA

                var vInstrucao1 = "00"; //1ª instrução (2, N)
                var vInstrucao2 = "00"; //2ª instrução (2, N)

                foreach (var instrucao in boleto.Instrucoes.OfType<Instrucao_Bradesco>())
                {
                    switch ((EnumInstrucoes_Bradesco)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Bradesco.Protestar:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = instrucao.Dias.ToString().PadLeft(2, '0');
                            break;
                        case EnumInstrucoes_Bradesco.NaoProtestar:
                            vInstrucao1 = "00";
                            vInstrucao2 = "00";
                            break;
                        case EnumInstrucoes_Bradesco.ProtestoFinsFalimentares:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = instrucao.Dias.ToString().PadLeft(2, '0');
                            break;
                        case EnumInstrucoes_Bradesco.ProtestarAposNDiasCorridos:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = Utils.FitStringLength(instrucao.Dias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_Bradesco.ProtestarAposNDiasUteis:
                            vInstrucao1 = "06"; //Indicar o código “06” - (Protesto)
                            vInstrucao2 = Utils.FitStringLength(instrucao.Dias.ToString(), 2, 2, '0', 0, true, true, true);
                            break;
                        case EnumInstrucoes_Bradesco.NaoReceberAposNDias:
                            vInstrucao1 = "00";
                            vInstrucao2 = "00";
                            break;
                        case EnumInstrucoes_Bradesco.DevolverAposNDias:
                            vInstrucao1 = "00";
                            vInstrucao2 = "00";
                            break;
                    }
                }

                detalhe += vInstrucao1; //posições: 157 a 158 do leiaute
                detalhe += vInstrucao2; //posições: 159 a 160 do leiaute
                
                // Valor a ser cobrado por Dia de Atraso (13, N)
                detalhe += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                //Data Limite P/Concessão de Desconto (06, N)
                //if (boleto.DataDesconto.ToString("dd/MM/yyyy") == "01/01/0001")
                if (boleto.DataDesconto == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                    detalhe += "000000"; //Caso nao tenha data de vencimento
                else
                    detalhe += boleto.DataDesconto.ToString("ddMMyy");
                
                //Valor do Desconto (13, N)
                detalhe += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                //Valor do IOF (13, N)
                detalhe += Utils.FitStringLength(boleto.Iof.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                //Valor do Abatimento a ser concedido ou cancelado (13, N)
                detalhe += Utils.FitStringLength(boleto.Abatimento.ApenasNumeros(), 13, 13, '0', 0, true, true, true);

                /*Identificação do Tipo de Inscrição do Sacado (02, N)
                    01-CPF
                    02-CNPJ
                    03-PIS/PASEP
                    98-Não tem
                    99-Outros 
                    00-Outros 
                */
                if (boleto.Sacado.CpfCnpj.Length <= 11)
                    detalhe += "01";  //CPF
                else
                    detalhe += "02"; //CNPJ

                //Nº Inscrição do Sacado (14, N)
                var cpfCnpj = boleto.Sacado.CpfCnpj.Replace("/", "").Replace(".", "").Replace("-", "");
                detalhe += Utils.FitStringLength(cpfCnpj, 14, 14, '0', 0, true, true, true);

                //Nome do Sacado (40, A)
                detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                //Endereço Completo (40, A)
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();

                var mensagem = new string(' ', 12);
                var mensagem2 = new string(' ', 60);

                //1ª Mensagem (12, A)
                //Campo livre para uso da Empresa. A mensagem enviada nesse campo será impressa somente no boleto e não será confirmada no Arquivo Retorno.
                detalhe += Utils.FitStringLength(mensagem, 12, 12, ' ', 0, true, true, false);

                //CEP (5, N) + Sufixo do CEP (3, N) Total (8, N)
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cep.Replace("-", ""), 8, 8, '0', 0, true, true, true);

                //Sacador|Avalista ou 2ª Mensagem (60, A)
                detalhe += Utils.FitStringLength(mensagem2, 60, 60, ' ', 0, true, true, false);

                //Nº Seqüencial do Registro (06, N)
                detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe);

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        #endregion

        #region Mensagem

        public override string GerarMensagemVariavelRemessa(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var detalhe = "";

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        throw new Exception("Registro de mensagem não existe para Cnab200.");
                    case TipoArquivo.Cnab400:
                        detalhe = GerarMensagemRemessaCnab400(boleto, ref numeroRegistro);
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

        private string GerarMensagemRemessaCnab400(Boleto boleto, ref int numeroRegistro)
        {
            try
            {
                var detalhe = "";
                if (!string.IsNullOrEmpty(detalhe)) //TODO
                    detalhe += Environment.NewLine;

                detalhe += "2"; //Código do registro, tipo mensagem.

                //Mensagem 1 ==> 002 a 081
                //Mensagem 2 ==> 082 a 161
                //Mensagem 3 ==> 162 a 241
                //Mensagem 4 ==> 242 a 321
                for (var i = 0; i < 4; i++)
                {
                    if (boleto.Instrucoes.Count > i)
                        detalhe += Utils.FitStringLength(boleto.Instrucoes[i].Descricao, 80, 80, ' ', 0, true, true, false);
                    else
                        detalhe += new string(' ', 80);
                }

                detalhe += new string('0', 6); //Data limite para concessão de Desconto 2 ==> 322 a 327
                detalhe += new string('0', 13); //Valor do Desconto - 328 a 340
                detalhe += new string('0', 6); //Data limite para concessão de Desconto 3 - 341 a 346
                detalhe += new string('0', 13); //Valor do Desconto - 347 a 359
                detalhe += new string(' ', 7); //Reserva - 360 a 366
                
                detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true); //Carteira - 367 a 369
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true); //Carteira - 370 a 374
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 7, 7, '0', 0, true, true, true); //Carteira - 375 a 381
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true);//D da conta(1) //Carteira - 382 a 382
                detalhe += Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true); //Nosso Numero (11) //Nosso Número - 383 a 393

                // Força o NossoNumero a ter 11 dígitos. Alterado por Luiz Ponce 07/07/2012 - 394 a 394
                //detalhe += Mod11Bradesco(boleto.Carteira + Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true), 7); // Digito de Auto Conferencia do Nosso Número (01)
                detalhe += Utils.FitStringLength(CalcularDigitoNossoNumero(boleto), 1, 1, '0', 0, true, true, true); //DAC Nosso Número (1, A)

                //Nº Seqüencial do Registro (06, N) - 395 a 400
                detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true);

                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe.Replace("(", " ").Replace(")", " "));

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        #endregion

        #region Trailer

        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                var trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        trailer = GerarTrailerRemessa240();
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
                throw new Exception("", ex);
            }
        }

        private string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        private string GerarTrailerRemessa400(int numeroRegistro)
        {
            try
            {
                var trailer = "9";
                trailer += new string(' ', 393);
                trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); // Número sequencial do registro no arquivo.

                return Utils.SubstituiCaracteresEspeciais(trailer);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        #endregion
    }
}