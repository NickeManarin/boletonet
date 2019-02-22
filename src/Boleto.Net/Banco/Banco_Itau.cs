using BoletoNet.Excecoes;
using BoletoNet.Util;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;

[assembly: WebResource("BoletoNet.Imagens.341.jpg", "image/jpg")]
namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao banco Ita�
    /// </summary>
    internal sealed class BancoItau : AbstractBanco, IBanco
    {
        #region Vari�veis

        private int _dacBoleto = 0;

        #endregion

        #region Construtores

        internal BancoItau()
        {
            try
            {
                Codigo = 341;
                Digito = "7";
                Nome = "Ita�";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        #endregion

        #region M�todos de Inst�ncia

        /// <summary>
        /// Valida��es particulares do banco Ita�
        /// </summary>
        public override void ValidaBoleto(Boleto boleto)
        {
            try
            {
                //Carteiras v�lidas.
                var carteiras = new[] { "175", "176", "178", "109", "198", "107", "122", "142", "143", "196", "126", "131", "146", "150", "169", "121", "112", "104" };

                if (!carteiras.Contains(boleto.Carteira))
                {
                    var validas = carteiras.Aggregate("", (p, n) => p + (p.Length > 0 ? ", " : "") + n);
                    throw new NotImplementedException($"Carteira n�o implementada: {boleto.Carteira}\r\nCarteiras v�lidas: {validas}");
                }

                //Verifica se o NossoNumero � um inteiro v�lido.
                int aux;
                if (!int.TryParse(boleto.NossoNumero, out aux))
                    throw new NotImplementedException("Nosso n�mero para a carteira " + boleto.Carteira + " inv�lido.");

                //Verifica se o tamanho para o NossoNumero s�o 8 d�gitos.
                if (boleto.NossoNumero.Length > 8)
                    throw new NotImplementedException("A quantidade de d�gitos do nosso n�mero para a carteira " + boleto.Carteira + ", s�o 8 n�meros.");

                if (boleto.NossoNumero.Length < 8)
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 8);

                //� obrigat�rio o preenchimento do n�mero do documento
                if (boleto.Carteira == "106" || boleto.Carteira == "107" || boleto.Carteira == "122" || boleto.Carteira == "142" || boleto.Carteira == "143" || boleto.Carteira == "195" || boleto.Carteira == "196" || boleto.Carteira == "198")
                    if (Utils.ToInt32(boleto.NumeroDocumento) == 0)
                        throw new NotImplementedException("O n�mero do documento n�o pode ser igual a zero.");

                //Formato o n�mero do documento 
                if (Utils.ToInt32(boleto.NumeroDocumento) > 0)
                    boleto.NumeroDocumento = Utils.FormatCode(boleto.NumeroDocumento, 7);

                //Calcula o DAC da Conta Corrente
                boleto.Cedente.ContaBancaria.DigitoConta = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta).ToString();

                //Calcula o DAC do Nosso N�mero a maioria das carteiras. Ag�ncia/conta/carteira/nosso n�mero.
                //if (boleto.Carteira == "104" || boleto.Carteira == "112")
                //    _dacNossoNumero = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta + boleto.Carteira + boleto.NossoNumero);
                //else if (boleto.Carteira != "126" && boleto.Carteira != "131" && boleto.Carteira != "146" && boleto.Carteira != "150" && boleto.Carteira != "168")
                //    _dacNossoNumero = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Carteira + boleto.NossoNumero);
                //else
                //    _dacNossoNumero = Mod10(boleto.Carteira + boleto.NossoNumero); //Excess�o 126 - 131 - 146 - 150 - 168 carteira/nosso numero

                //boleto.DigitoNossoNumero = _dacNossoNumero.ToString();

                boleto.LocalPagamento = "At� o vencimento em qualquer banco ou correspondente n�o banc�rio. ";
                //"Ap�s o vencimento, acesse itau.com.br/boletos e pague<br> em qualquer banco ou correspondente n�o banc�rio.";

                //Verifica se o nosso n�mero � v�lido.
                if (Utils.ToInt64(boleto.NossoNumero) == 0)
                    throw new NotImplementedException("Nosso n�mero inv�lido");

                //Verifica se data do processamento � valida.
                if (boleto.DataProcessamento == DateTime.MinValue)
                    boleto.DataProcessamento = DateTime.Now;

                //Verifica se data do documento � valida.
                if (boleto.DataDocumento == DateTime.MinValue)
                    boleto.DataDocumento = DateTime.Now;

                boleto.FormataCampos();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao validar boletos.", e);
            }
        }

        #endregion

        #region M�todos de formata��o do boleto

        public override void FormataCodigoBarra(Boleto boleto)
        {
            try
            {
                var valor = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);

                boleto.CodigoBarra.Codigo = $"{Codigo}{boleto.Moeda}{FatorVencimento(boleto)}{valor}{boleto.Carteira}{boleto.NossoNumero}";

                switch (boleto.Carteira)
                {
                    case "104":
                    case "109":
                    case "112":
                    case "121":
                    case "175":
                    case "176":
                    case "178":
                    {
                        boleto.CodigoBarra.Codigo += $"{boleto.DigitoNossoNumero}{boleto.Cedente.ContaBancaria.Agencia}{Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 5)}" +
                                                     $"{boleto.Cedente.ContaBancaria.DigitoConta}000";
                        break;
                    }

                    case "107":
                    case "122":
                    case "142":
                    case "143":
                    case "196":
                    case "198":
                    {
                        var numeroDocumento = Utils.FormatCode(boleto.NumeroDocumento, 7);
                        var codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo, 5);

                        boleto.CodigoBarra.Codigo += $"{numeroDocumento}{codigoCedente}{Mod10(boleto.Carteira + boleto.NossoNumero + numeroDocumento + codigoCedente)}0";
                        break;
                    }
                }

                _dacBoleto = Mod11(boleto.CodigoBarra.Codigo, 9, 0);

                boleto.CodigoBarra.Codigo = boleto.CodigoBarra.Codigo.Left(4) + _dacBoleto + boleto.CodigoBarra.Codigo.Right(39);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar c�digo de barras.", ex);
            }
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            try
            {
                var c2 = string.Empty;
                var c3 = string.Empty;

                #region Campo 1 (AAABC.CCDDX)

                var aaa = Utils.FormatCode(Codigo.ToString(), 3); //C�digo do banco, 341: Ita�.
                var b = boleto.Moeda.ToString(); //Tipo da moeda, 9: Real.
                var ccc = boleto.Carteira.PadLeft(3, '0'); //Carteira, 3 d�gitos.
                var dd = boleto.NossoNumero.Substring(0, 2); //Nosso n�mero, 2 primeiros d�gitos.
                var x = Mod10(aaa + b + ccc + dd).ToString(); //D�gito verificador do campo 1, 1 d�gito.

                var c1 = $"{aaa}{b}{ccc.Substring(0, 1)}.{ccc.Substring(1, 2)}{dd}{x}";

                #endregion

                if (boleto.Carteira == "175" || boleto.Carteira == "176" || boleto.Carteira == "178" || boleto.Carteira == "109" || boleto.Carteira == "121" || boleto.Carteira == "112" || boleto.Carteira == "104")
                {
                    #region Defini��es

                    /* AAABC.CCDDX.DDDDD.DEFFFY.FGGGG.GGHHHZ.K.UUUUVVVVVVVVVV
                     * ------------------------------------------------------
                     * Campo 1
                     * AAABC.CCDDX
                     * AAA - C�digo do Banco
                     * B   - Moeda
                     * CCC - Carteira
                     * DD  - 2 primeiros n�meros Nosso N�mero
                     * X   - DAC Campo 1 (AAABC.CCDD) Mod10
                     * 
                     * Campo 2
                     * DDDDD.DEFFFY
                     * DDDDD.D - Restante Nosso N�mero
                     * E       - DAC (Ag�ncia/Conta/Carteira/Nosso N�mero)
                     * FFF     - Tr�s primeiros da ag�ncia
                     * Y       - DAC Campo 2 (DDDDD.DEFFF) Mod10
                     * 
                     * Campo 3
                     * FGGGG.GGHHHZ
                     * F       - Restante da Ag�ncia
                     * GGGG.GG - N�mero Conta Corrente + DAC
                     * HHH     - Zeros (N�o utilizado)
                     * Z       - DAC Campo 3
                     * 
                     * Campo 4
                     * K       - DAC C�digo de Barras
                     * 
                     * Campo 5
                     * UUUUVVVVVVVVVV
                     * UUUU       - Fator Vencimento
                     * VVVVVVVVVV - Valor do T�tulo 
                     */

                    #endregion Defini��es

                    var agencia = Utils.FormatCode(boleto.Cedente.ContaBancaria.Agencia, 4);

                    #region Campo 2 (DDDDD.DEFFFY)

                    var dddddd = boleto.NossoNumero.Substring(2, 6);
                    var e = boleto.DigitoNossoNumero;
                    var fff = agencia.Substring(0, 3);
                    var y = Mod10(dddddd + e + fff).ToString();

                    c2 = $"{dddddd.Substring(0, 5)}.{dddddd.Substring(5, 1)}{e}{fff}{y} ";

                    #endregion

                    #region Campo 3 (FGGGG.GGHHHZ)

                    var f = agencia.Substring(3, 1);
                    var gggggg = boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta;
                    const string hhh = "000";
                    var z = Mod10(f + gggggg + hhh).ToString();

                    c3 = $"{f}{gggggg.Substring(0, 4)}.{gggggg.Substring(4, 2)}{hhh}{z}";

                    #endregion
                }
                else if (boleto.Carteira == "198" || boleto.Carteira == "107" || boleto.Carteira == "122" || boleto.Carteira == "142" || boleto.Carteira == "143" || boleto.Carteira == "196")
                {
                    #region Defini��es

                    /* AAABC.CCDDX.DDDDD.DEEEEY.EEEFF.FFFGHZ.K.UUUUVVVVVVVVVV
                     * ------------------------------------------------------
                     * Campo 1 - AAABC.CCDDX
                     * AAA - C�digo do Banco
                     * B   - Moeda
                     * CCC - Carteira
                     * DD  - 2 primeiros n�meros Nosso N�mero
                     * X   - DAC Campo 1 (AAABC.CCDD) Mod10
                     * 
                     * Campo 2 - DDDDD.DEEEEY
                     * DDDDD.D - Restante Nosso N�mero
                     * EEEE    - 4 primeiros numeros do n�mero do documento
                     * Y       - DAC Campo 2 (DDDDD.DEEEEY) Mod10
                     * 
                     * Campo 3 - EEEFF.FFFGHZ
                     * EEE     - Restante do n�mero do documento
                     * FFFFF   - C�digo do Cliente
                     * G       - DAC (Carteira/Nosso Numero(sem DAC)/Numero Documento/Codigo Cliente)
                     * H       - zero
                     * Z       - DAC Campo 3
                     * 
                     * Campo 4 - K
                     * K       - DAC C�digo de Barras
                     * 
                     * Campo 5 - UUUUVVVVVVVVVV
                     * UUUU       - Fator Vencimento
                     * VVVVVVVVVV - Valor do T�tulo 
                     */

                    #endregion

                    var numeroDocumento = Utils.FormatCode(boleto.NumeroDocumento, 7);
                    var codigoCedente = Utils.FormatCode(boleto.Cedente.Codigo, 5);

                    #region Campo 2 (DDDDD.DEEEEY)

                    var dddddd = boleto.NossoNumero.Substring(2, 6);
                    var eeee = numeroDocumento.Substring(0, 4);
                    var y = Mod10(dddddd + eeee).ToString();

                    c2 = $"{dddddd.Substring(0, 5)}.{dddddd.Substring(5, 1)}{eeee}{y}";

                    #endregion

                    #region Campo 3 (EEEFF.FFFGHZ)

                    var eee = numeroDocumento.Substring(4, 3);
                    var fffff = codigoCedente;
                    var g = Mod10(boleto.Carteira + boleto.NossoNumero + numeroDocumento + codigoCedente).ToString();
                    const string h = "0";
                    var z = Mod10(eee + fffff + g + h).ToString();
                    c3 = $"{eee}{fffff.Substring(0, 2)}.{fffff.Substring(2, 3)}{g}{h}{z}";

                    #endregion
                }
                else if (boleto.Carteira == "126" || boleto.Carteira == "131" || boleto.Carteira == "146" || boleto.Carteira == "150" || boleto.Carteira == "168")
                {
                    throw new NotImplementedException("Fun��o n�o implementada.");
                }

                #region Campo 5 (UUUUVVVVVVVVVV)

                var uuuu = FatorVencimento(boleto).ToString();
                var vvvvvvvvvv = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);

                var c5 = uuuu + vvvvvvvvvv;

                #endregion

                boleto.CodigoBarra.LinhaDigitavel = $"{c1} {c2} {c3} {_dacBoleto} {c5}";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar linha digit�vel.", ex);
            }
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            try
            {
                //boleto.NossoNumero = $"{boleto.Carteira}/{boleto.NossoNumero}-{boleto.DigitoNossoNumero}";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar nosso n�mero", ex);
            }
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            try
            {
                boleto.NumeroDocumento = $"{boleto.NumeroDocumento}-{Mod10(boleto.NumeroDocumento)}";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao formatar n�mero do documento.", ex);
            }
        }

        /// <summary>
        /// Verifica o tipo de ocorr�ncia para o arquivo remessa
        /// </summary>
        private string Ocorrencia(string codigo)
        {
            switch (codigo)
            {
                case "02":
                    return "02-Entrada Confirmada";
                case "03":
                    return "03-Entrada Rejeitada";
                case "04":
                    return "04-Altera��o de Dados-Nova entrada ou Altera��o/Exclus�o de dados acatada";
                case "05":
                    return "05-Altera��o de dados-Baixa";
                case "06":
                    return "06-Liquida��o normal";
                case "08":
                    return "08-Liquida��o em cart�rio";
                case "09":
                    return "09-Baixa simples";
                case "10":
                    return "10-Baixa por ter sido liquidado";
                case "11":
                    return "11-Em Ser (S� no retorno mensal)";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Vencimento Alterado";
                case "15":
                    return "15-Baixas rejeitadas";
                case "16":
                    return "16-Instru��es rejeitadas";
                case "17":
                    return "17-Altera��o/Exclus�o de dados rejeitados";
                case "18":
                    return "18-Cobran�a contratual-Instru��es/Altera��es rejeitadas/pendentes";
                case "19":
                    return "19-Confirma Recebimento Instru��o de Protesto";
                case "20":
                    return "20-Confirma Recebimento Instru��o Susta��o de Protesto/Tarifa";
                case "21":
                    return "21-Confirma Recebimento Instru��o de N�o Protestar";
                case "23":
                    return "23-T�tulo enviado a Cart�rio/Tarifa";
                case "24":
                    return "24-Instru��o de Protesto Rejeitada/Sustada/Pendente";
                case "25":
                    return "25-Alega��es do Sacado";
                case "26":
                    return "26-Tarifa de Aviso de Cobran�a";
                case "27":
                    return "27-Tarifa de Extrato Posi��o";
                case "28":
                    return "28-Tarifa de Rela��o das Liquida��es";
                case "29":
                    return "29-Tarifa de Manuten��o de T�tulos Vencidos";
                case "30":
                    return "30-D�bito Mensal de Tarifas (Para Entradas e Baixas)";
                case "32":
                    return "32-Baixa por ter sido Protestado";
                case "33":
                    return "33-Custas de Protesto";
                case "34":
                    return "34-Custas de Susta��o";
                case "35":
                    return "35-Custas de Cart�rio Distribuidor";
                case "36":
                    return "36-Custas de Edital";
                case "37":
                    return "37-Tarifa de Emiss�o de Boleto/Tarifa de Envio de Duplicata";
                case "38":
                    return "38-Tarifa de Instru��o";
                case "39":
                    return "39-Tarifa de Ocorr�ncias";
                case "40":
                    return "40-Tarifa Mensal de Emiss�o de Boleto/Tarifa Mensal de Envio de Duplicata";
                case "41":
                    return "41-D�bito Mensal de Tarifas-Extrato de Posi��o(B4EP/B4OX)";
                case "42":
                    return "42-D�bito Mensal de Tarifas-Outras Instru��es";
                case "43":
                    return "43-D�bito Mensal de Tarifas-Manuten��o de T�tulos Vencidos";
                case "44":
                    return "44-D�bito Mensal de Tarifas-Outras Ocorr�ncias";
                case "45":
                    return "45-D�bito Mensal de Tarifas-Protesto";
                case "46":
                    return "56-D�bito Mensal de Tarifas-Susta��o de Protesto";
                case "47":
                    return "47-Baixa com Transfer�ncia para Protesto";
                case "48":
                    return "48-Custas de Susta��o Judicial";
                case "51":
                    return "51-Tarifa Mensal Ref a Entradas Bancos Correspondentes na Carteira";
                case "52":
                    return "52-Tarifa Mensal Baixas na Carteira";
                case "53":
                    return "53-Tarifa Mensal Baixas em Bancos Correspondentes na Carteira";
                case "54":
                    return "54-Tarifa Mensal de Liquida��es na Carteira";
                case "55":
                    return "55-Tarifa Mensal de Liquida��es em Bancos Correspondentes na Carteira";
                case "56":
                    return "56-Custas de Irregularidade";
                case "57":
                    return "57-Instru��o Cancelada";
                case "59":
                    return "59-Baixa por Cr�dito em C/C Atrav�s do SISPAG";
                case "60":
                    return "60-Entrada Rejeitada Carn�";
                case "61":
                    return "61-Tarifa Emiss�o Aviso de Movimenta��o de T�tulos";
                case "62":
                    return "62-D�bito Mensal de Tarifa-Aviso de Movimenta��o de T�tulos";
                case "63":
                    return "63-T�tulo Sustado Judicialmente";
                case "64":
                    return "64-Entrada Confirmada com Rateio de Cr�dito";
                case "69":
                    return "69-Cheque Devolvido";
                case "71":
                    return "71-Entrada Registrada-Aguardando Avalia��o";
                case "72":
                    return "72-Baixa por Cr�dito em C/C Atrav�s do SISPAG sem T�tulo Correspondente";
                case "73":
                    return "73-Confirma��o de Entrada na Cobran�a Simples-Entrada N�o Aceita na Cobran�a Contratual";
                case "76":
                    return "76-Cheque Compensado";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Verifica o c�digo do motivo da rejei��o informada pelo banco
        /// </summary>
        private string MotivoRejeicao(string codigo)
        {
            switch (codigo)
            {
                case "03": return "03-AG. COBRADORA - CEP SEM ATENDIMENTO DE PROTESTO NO MOMENTO";
                case "04": return "04-ESTADO - SIGLA DO ESTADO INV�LIDA";
                case "05": return "05-DATA VENCIMENTO - PRAZO DA OPERA��O MENOR QUE PRAZO M�NIMO OU MAIOR QUE O M�XIMO";
                case "07": return "07-VALOR DO T�TULO - VALOR DO T�TULO MAIOR QUE 10.000.000,00";
                case "08": return "08-NOME DO PAGADOR - N�O INFORMADO OU DESLOCADO";
                case "09": return "09-AGENCIA/CONTA - AG�NCIA ENCERRADA";
                case "10": return "10-LOGRADOURO - N�O INFORMADO OU DESLOCADO";
                case "11": return "11-CEP - CEP N�O NUM�RICO OU CEP INV�LIDO";
                case "12": return "12-SACADOR / AVALISTA - NOME N�O INFORMADO OU DESLOCADO (BANCOS CORRESPONDENTES)";
                case "13": return "13-ESTADO/CEP - CEP INCOMPAT�VEL COM A SIGLA DO ESTADO";
                case "14": return "14-NOSSO N�MERO - NOSSO N�MERO J� REGISTRADO NO CADASTRO DO BANCO OU FORA DA FAIXA";
                case "15": return "15-NOSSO N�MERO - NOSSO N�MERO EM DUPLICIDADE NO MESMO MOVIMENTO";
                case "18": return "18-DATA DE ENTRADA - DATA DE ENTRADA INV�LIDA PARA OPERAR COM ESTA CARTEIRA";
                case "19": return "19-OCORR�NCIA - OCORR�NCIA INV�LIDA";
                case "21": return "21-AG. COBRADORA - CARTEIRA N�O ACEITA DEPOSIT�RIA CORRESPONDENTE ESTADO DA AG�NCIA DIFERENTE DO ESTADO DO PAGADOR AG. COBRADORA N�O CONSTA NO CADASTRO OU ENCERRANDO";
                case "22": return "22-CARTEIRA - CARTEIRA N�O PERMITIDA (NECESS�RIO CADASTRAR FAIXA LIVRE)";
                case "26": return "26-AG�NCIA/CONTA - AG�NCIA/CONTA N�O LIBERADA PARA OPERAR COM COBRAN�A";
                case "27": return "27-CNPJ INAPTO - CNPJ DO BENEFICI�RIO INAPTO DEVOLU��O DE T�TULO EM GARANTIA";
                case "29": return "29-C�DIGO EMPRESA - CATEGORIA DA CONTA INV�LIDA";
                case "30": return "30-ENTRADA BLOQUEADA - ENTRADAS BLOQUEADAS, CONTA SUSPENSA EM COBRAN�A";
                case "31": return "31-AG�NCIA/CONTA - CONTA N�O TEM PERMISS�O PARA PROTESTAR (CONTATE SEU GERENTE)";
                case "35": return "35-VALOR DO IOF - IOF MAIOR QUE 5%";
                case "36": return "36-QTDADE DE MOEDA - QUANTIDADE DE MOEDA INCOMPAT�VEL COM VALOR DO T�TULO";
                case "37": return "37-CNPJ/CPF DO PAGADOR - N�O NUM�RICO OU IGUAL A ZEROS";
                case "42": return "42-NOSSO N�MERO - NOSSO N�MERO FORA DE FAIXA";
                case "52": return "52-AG. COBRADORA - EMPRESA N�O ACEITA BANCO CORRESPONDENTE";
                case "53": return "53-AG. COBRADORA - EMPRESA N�O ACEITA BANCO CORRESPONDENTE - COBRAN�A MENSAGEM";
                case "54": return "54-DATA DE VENCTO - BANCO CORRESPONDENTE - T�TULO COM VENCIMENTO INFERIOR A 15 DIAS";
                case "55": return "55-DEP/BCO CORRESP - CEP N�O PERTENCE � DEPOSIT�RIA INFORMADA";
                case "56": return "56-DT VENCTO/BCO CORRESP - VENCTO SUPERIOR A 180 DIAS DA DATA DE ENTRADA";
                case "57": return "57-DATA DE VENCTO - CEP S� DEPOSIT�RIA BCO DO BRASIL COM VENCTO INFERIOR A 8 DIAS";
                case "60": return "60-ABATIMENTO - VALOR DO ABATIMENTO INV�LIDO";
                case "61": return "61-JUROS DE MORA - JUROS DE MORA MAIOR QUE O PERMITIDO";
                case "62": return "62-DESCONTO - VALOR DO DESCONTO MAIOR QUE VALOR DO T�TULO";
                case "63": return "63-DESCONTO DE ANTECIPA��O - VALOR DA IMPORT�NCIA POR DIA DE DESCONTO (IDD) N�O PERMITIDO";
                case "64": return "64-DATA DE EMISS�O - DATA DE EMISS�O DO T�TULO INV�LIDA";
                case "65": return "65-TAXA FINANCTO - TAXA INV�LIDA (VENDOR)";
                case "66": return "66-DATA DE VENCTO - INVALIDA/FORA DE PRAZO DE OPERA��O (M�NIMO OU M�XIMO)";
                case "67": return "67-VALOR/QTIDADE - VALOR DO T�TULO/QUANTIDADE DE MOEDA INV�LIDO";
                case "68": return "68-CARTEIRA - CARTEIRA INV�LIDA OU N�O CADASTRADA NO INTERC�MBIO DA COBRAN�A";
                case "69": return "69-CARTEIRA - CARTEIRA INV�LIDA PARA T�TULOS COM RATEIO DE CR�DITO";
                case "70": return "70-AG�NCIA/CONTA - BENEFICI�RIO N�O CADASTRADO PARA FAZER RATEIO DE CR�DITO";
                case "78": return "78-AG�NCIA/CONTA - DUPLICIDADE DE AG�NCIA/CONTA BENEFICI�RIA DO RATEIO DE CR�DITO";
                case "80": return "80-AG�NCIA/CONTA - QUANTIDADE DE CONTAS BENEFICI�RIAS DO RATEIO MAIOR DO QUE O PERMITIDO (M�XIMO DE 30 CONTAS POR T�TULO)";
                case "81": return "81-AG�NCIA/CONTA - CONTA PARA RATEIO DE CR�DITO INV�LIDA / N�O PERTENCE AO ITA�";
                case "82": return "82-DESCONTO/ABATI-MENTO - DESCONTO/ABATIMENTO N�O PERMITIDO PARA T�TULOS COM RATEIO DE CR�DITO";
                case "83": return "83-VALOR DO T�TULO - VALOR DO T�TULO MENOR QUE A SOMA DOS VALORES ESTIPULADOS PARA RATEIO";
                case "84": return "84-AG�NCIA/CONTA - AG�NCIA/CONTA BENEFICI�RIA DO RATEIO � A CENTRALIZADORA DE CR�DITO DO BENEFICI�RIO";
                case "85": return "85-AG�NCIA/CONTA - AG�NCIA/CONTA DO BENEFICI�RIO � CONTRATUAL / RATEIO DE CR�DITO N�O PERMITIDO";
                case "86": return "86-TIPO DE VALOR - C�DIGO DO TIPO DE VALOR INV�LIDO / N�O PREVISTO PARA T�TULOS COM RATEIO DE CR�DITO";
                case "87": return "87-AG�NCIA/CONTA - REGISTRO TIPO 4 SEM INFORMA��O DE AG�NCIAS/CONTAS BENEFICI�RIAS DO RATEIO";
                case "90": return "90-NRO DA LINHA - COBRAN�A MENSAGEM - N�MERO DA LINHA DA MENSAGEM INV�LIDO OU QUANTIDADE DE LINHAS EXCEDIDAS";
                case "97": return "97-SEM MENSAGEM - COBRAN�A MENSAGEM SEM MENSAGEM (S� DE CAMPOS FIXOS), POR�M COM REGISTRO DO TIPO 7 OU 8";
                case "98": return "98-FLASH INV�LIDO - REGISTRO MENSAGEM SEM FLASH CADASTRADO OU FLASH INFORMADO DIFERENTE DO CADASTRADO";
                case "99": return "99-FLASH INV�LIDO - CONTA DE COBRAN�A COM FLASH CADASTRADO E SEM REGISTRO DE MENSAGEM CORRESPONDENTE";
                default:
                    return string.Empty;
            }
        }

        #endregion

        #region M�todos de gera��o do arquivo remessa

        #region Header

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Fun��o n�o implementada.");
        }

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
                throw new Exception("Erro durante a gera��o do HEADER do arquivo de REMESSA.", ex);
            }
        }

        /// <summary>
        ///POS INI/FINAL	DESCRI��O	                   A/N	TAM	DEC	CONTE�DO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	C�digo do Banco na compensa��o	    N	003		341	
        ///004 - 007	Lote de servi�o	                    N	004		0000	1 
        ///008 - 008	Registro Hearder de Arquivo         N	001		0	2
        ///009 - 017	Reservado (uso Banco)	            A	009		Brancos	  
        ///018 - 018	Tipo de inscri��o da empresa	    N	001		1 = CPF,  2 = CNPJ 	
        ///019 � 032	N� de inscri��o da empresa	        N	014		Nota 1
        ///033 � 045	C�digo do Conv�nio no Banco   	    A	013	    Nota 2 
        ///046 - 052	Reservado (uso Banco)	            A	007		Brancos	
        ///053 - 053	Complemento de Registro             N	001     0			
        ///054 - 057	Ag�ncia Referente Conv�nio Ass.     N	004     Nota 1
        ///058 - 058    Complemento de Registro             A   001     Brancos
        ///059 - 065    Complemento de Registro             N   007     Brancos
        ///066 - 070    N�mero da C/C do Cliente            N   005     Nota 1
        ///071 - 071    Complemento de Registro             A   001     Brancos
        ///072 - 072    DAC da Ag�ncia/Conta                N   001     Nota 1
        ///073 - 102    Nome da Empresa                     A   030     Nome da Empresa
        ///103 - 132	Nome do Banco	                    A	030		Banco Ita� 	
        ///133 - 142	Reservado (uso Banco)	            A	010		Brancos	
        ///143 - 143	C�digo remessa 	                    N	001		1 = Remessa 	
        ///144 - 151	Data de gera��o do arquivo	        N	008		DDMMAAAA	
        ///152 - 157	Hora de gera��o do arquivo          N	006		HHMMSS
        ///158 - 163	N� seq�encial do arquivo 	        N	006	    Nota 3
        ///164 - 166	N� da vers�o do layout do arquivo	N	003		040
        ///167 - 171    Densidaded de Grava��o do arquivo   N   005     00000
        ///172 - 240	Reservado (uso Banco)	            A	069		Brancos	
        /// </summary>
        private string GerarHeaderRemessaCnab240(Cedente cedente)
        {
            try
            {
                var header = "341";
                header += "0001";
                header += "0";
                header += Utils.FormatCode("", " ", 9);
                header += (cedente.CpfCnpj.Length == 11 ? "1" : "2");
                header += Utils.FormatCode(cedente.CpfCnpj, "0", 14, true);
                header += Utils.FormatCode("", " ", 20);
                header += "0";
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, " ", 4, true);
                header += " ";
                header += "0000000";
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 5, true);
                header += " ";
                header += Utils.FormatCode(String.IsNullOrEmpty(cedente.ContaBancaria.DigitoConta) ? " " : cedente.ContaBancaria.DigitoConta, " ", 1, true);
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);
                header += Utils.FormatCode("BANCO ITAU SA", " ", 30);
                header += Utils.FormatCode("", " ", 10);
                header += "1";
                header += DateTime.Now.ToString("ddMMyyyyHHmmss");
                header += Utils.FormatCode("", "0", 6, true);
                header += "040";
                header += "00000";
                header += Utils.FormatCode("", " ", 54);
                header += "000";
                header += Utils.FormatCode("", " ", 12);

                return Utils.SubstituiCaracteresEspeciais(header);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }

        private string GerarHeaderRemessaCnab400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var header = "01REMESSA01COBRANCA       ";
                header += Utils.FitStringLength(cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                header += "00";     //031-032: Zeros
                header += Utils.FitStringLength(cedente.ContaBancaria.Conta, 5, 5, '0', 0, true, true, true);
                header += Utils.FitStringLength(cedente.ContaBancaria.DigitoConta, 1, 1, ' ', 0, true, true, false);
                header += "        "; //039-046: Brancos
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false).ToUpper();
                header += "341";
                header += "BANCO ITAU SA  ";
                header += DateTime.Now.ToString("ddMMyy");
                header += new string(' ', 294);
                header += "000001";

                return Utils.SubstituiCaracteresEspeciais(header);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        #endregion

        #region Header do Lote

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
                        throw new Exception("N�o implementado.");
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }

        /// <summary>
        ///POS INI/FINAL	DESCRI��O	                   A/N	TAM	DEC	CONTE�DO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	C�digo do Banco na compensa��o	    N	003		341	
        ///004 - 007	Lote de servi�o	                    N	004		Nota 5 
        ///008 - 008	Registro Hearder de Lote            N	001     1
        ///009 - 009	Tipo de Opera��o                    A	001		D
        ///010 - 011	Tipo de servi�o             	    N	002		05
        ///012 � 013	Forma de Lan�amento                 N	002		50
        ///014 � 016	N�mero da vers�o do Layout   	    A	003	    030
        ///017 - 017	Complemento de Registro             A	001		Brancos	
        ///019 � 032	N� de inscri��o da empresa	        N	014		Nota 1
        ///033 � 045	C�digo do Conv�nio no Banco   	    A	013	    Nota 2
        ///018 - 018	Tipo de inscri��o da empresa	    N	001		1 = CPF,  2 = CNPJ
        ///046 - 052	Reservado (uso Banco)	            A	007		Brancos	
        ///053 - 053	Complemento de Registro             N	001     0			
        ///054 - 057	Ag�ncia Referente Conv�nio Ass.     N	004     Nota 1
        ///058 - 058    Complemento de Registro             A   001     Brancos
        ///059 - 065    Complemento de Registro             N   007     0000000
        ///066 - 070    N�mero da C/C do Cliente            N   005     Nota 1
        ///071 - 071    Complemento de Registro             A   001     Brancos
        ///072 - 072    DAC da Ag�ncia/Conta                N   001     Nota 1
        ///073 - 102    Nome da Empresa                     A   030     ENIX...
        ///103 - 142	Complemento de Registro             A	040		Brancos
        ///143 - 172	Endere�o da Empresa                 A	030		Nome da rua, Av., P�a, etc.
        ///173 - 177	N�mero do local                     N	005		N�mero do Local da Empresa
        ///178 - 192	Complemento                         A	015		Casa, Apto., Sala, etc.
        ///193 - 212	Nome da Cidade                      A	020	    Sao Paulo
        ///213 - 220	CEP                             	N	008		CEP
        ///221 - 222    Sigla do Estado                     A   002     SP
        ///223 - 230	Complemento de Registro             A	008		Brancos
        ///231 - 240    C�d. Ocr. para Retorno              A   010     Brancos
        /// </summary>
        private string GerarHeaderLoteRemessaCnab240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                header += "0001";
                header += "1";
                header += "R";
                header += "01";
                header += "00";
                header += "030";
                header += " ";
                header += (cedente.CpfCnpj.Length == 11 ? "1" : "2");
                header += Utils.FormatCode(cedente.CpfCnpj, "0", 15, true);
                header += Utils.FormatCode("", " ", 20);
                header += "0";
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 4, true);
                header += " ";
                header += Utils.FormatCode("", "0", 7);
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 5, true);
                header += " ";
                header += Utils.FormatCode(String.IsNullOrEmpty(cedente.ContaBancaria.DigitoConta) ? " " : cedente.ContaBancaria.DigitoConta, " ", 1, true);
                header += Utils.FitStringLength(cedente.Nome, 30, 30, ' ', 0, true, true, false);
                header += Utils.FormatCode("", " ", 80);
                header += Utils.FormatCode("", "0", 8, true);
                header += DateTime.Now.ToString("ddMMyyyy");
                header += DateTime.Now.ToString("ddMMyyyy");
                header += Utils.FormatCode("", " ", 33);

                return Utils.SubstituiCaracteresEspeciais(header);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                var segmentoP = "341";
                segmentoP += "0001";
                segmentoP += "3";
                segmentoP += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                segmentoP += "P";
                segmentoP += " ";
                segmentoP += ObterCodigoDaOcorrencia(boleto);
                segmentoP += "0";
                segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true);
                segmentoP += " ";
                segmentoP += "0000000";
                segmentoP += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 5, 5, '0', 0, true, true, true);
                segmentoP += " ";
                segmentoP += Utils.FormatCode(String.IsNullOrEmpty(boleto.Cedente.ContaBancaria.DigitoConta) ? " " : boleto.Cedente.ContaBancaria.DigitoConta, " ", 1, true);
                segmentoP += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true);
                segmentoP += Utils.FitStringLength(boleto.NossoNumero, 8, 8, '0', 0, true, true, true);
                segmentoP += Utils.FitStringLength(boleto.DigitoNossoNumero, 1, 1, '0', 1, true, true, true);
                segmentoP += "        ";
                segmentoP += "00000";
                segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, ' ', 0, true, true, false);
                segmentoP += "     ";
                segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                segmentoP += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                segmentoP += "00000";
                segmentoP += " ";
                segmentoP += "01";
                segmentoP += "A";
                segmentoP += Utils.FitStringLength(boleto.DataDocumento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                segmentoP += "0";
                segmentoP += Utils.FitStringLength(boleto.DataJurosMora.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                segmentoP += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                segmentoP += "0";
                segmentoP += Utils.FitStringLength(boleto.DataVencimento.ToString("ddMMyyyy"), 8, 8, ' ', 0, true, true, false);
                segmentoP += Utils.FitStringLength("0", 15, 15, '0', 0, true, true, true);
                segmentoP += Utils.FitStringLength("0", 15, 15, '0', 0, true, true, true);
                segmentoP += Utils.FitStringLength("0", 15, 15, '0', 0, true, true, true);
                segmentoP += Utils.FitStringLength(boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false);
                segmentoP += "0";
                segmentoP += "00";
                segmentoP += "0";
                segmentoP += "00";
                segmentoP += "0000000000000";
                segmentoP += " ";

                return Utils.SubstituiCaracteresEspeciais(segmentoP);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do SEGMENTO P DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var segmentoQ = "341";
                segmentoQ += "0001";
                segmentoQ += "3";
                segmentoQ += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                segmentoQ += "Q";
                segmentoQ += " ";
                segmentoQ += ObterCodigoDaOcorrencia(boleto);
                segmentoQ += boleto.Sacado.CpfCnpj.Length <= 11 ? "1" : "2";
                segmentoQ += Utils.FitStringLength(boleto.Sacado.CpfCnpj, 15, 15, '0', 0, true, true, true);
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 30, 30, ' ', 0, true, true, false).ToUpper();
                segmentoQ += "          ";
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.End.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper();
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cep, 8, 8, ' ', 0, true, true, false).ToUpper();
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade.TrimStart(' '), 15, 15, ' ', 0, true, true, false).ToUpper();
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Endereco.Uf, 2, 2, ' ', 0, true, true, false).ToUpper();
                segmentoQ += boleto.Sacado.CpfCnpj.Length <= 11 ? "1" : "2";
                segmentoQ += Utils.FitStringLength(boleto.Sacado.CpfCnpj, 15, 15, '0', 0, true, true, true);
                segmentoQ += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 30, 30, ' ', 0, true, true, false).ToUpper();
                segmentoQ += new string(' ', 10);
                segmentoQ += "000";
                segmentoQ += new string(' ', 28);

                segmentoQ = Utils.SubstituiCaracteresEspeciais(segmentoQ);

                return segmentoQ;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var segmentoR = "341";
                segmentoR += "0001";
                segmentoR += "3";
                segmentoR += Utils.FitStringLength(numeroRegistro.ToString(), 5, 5, '0', 0, true, true, true);
                segmentoR += "R ";
                segmentoR += ObterCodigoDaOcorrencia(boleto);
                // Desconto 2
                segmentoR += "000000000000000000000000"; //24 zeros
                // Desconto 3
                segmentoR += "000000000000000000000000"; //24 zeros

                if (boleto.PercMulta > 0)
                {
                    // C�digo da multa 2 - percentual
                    segmentoR += "2";
                }
                else if (boleto.ValorMulta > 0)
                {
                    // C�digo da multa 1 - valor fixo
                    segmentoR += "1";
                }
                else
                {
                    //C�digo da multa 0 - sem multa
                    segmentoR += "0";
                }

                segmentoR += Utils.FitStringLength(boleto.DataMulta.ToString("ddMMyyyy"), 8, 8, '0', 0, true, true, false);
                segmentoR += Utils.FitStringLength(boleto.ValorMulta.ApenasNumeros(), 15, 15, '0', 0, true, true, true);
                segmentoR += new string(' ', 110);
                segmentoR += "0000000000000000"; //16 zeros
                segmentoR += " "; //1 branco
                segmentoR += "000000000000"; //12 zeros
                segmentoR += "  "; //2 brancos
                segmentoR += "0"; //1 zero
                segmentoR += new string(' ', 9);

                return Utils.SubstituiCaracteresEspeciais(segmentoR);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        #region Detalhe

        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        return GerarDetalheRemessaCnab240(boleto, numeroRegistro, tipoArquivo);
                    case TipoArquivo.Cnab400:
                        return GerarDetalheRemessaCnab400(boleto, numeroRegistro, tipoArquivo);
                    default:
                        throw new Exception("Tipo de arquivo inexistente.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do DETALHE arquivo de REMESSA.", ex);
            }
        }

        /// <summary>
        ///POS INI/FINAL	DESCRI��O	                   A/N	TAM	DEC	CONTE�DO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	C�digo do Banco na compensa��o	    N	003		341	
        ///004 - 007	Lote de servi�o	                    N	004		Nota 5 
        ///008 - 008	Registro Detalhe de Lote            N	001     3
        ///009 - 013	N�mero Sequencial Registro Lote     N	005		Nota 6
        ///014 - 014	C�digo Segmento Reg. Detalhe   	    A	001		A
        ///015 � 017	C�digo da Instru��o p/ Movimento    N	003		Nota 7
        ///018 - 020	C�digo da C�mara de Compensa��o     N	003	    000
        ///021 - 023	C�digo do Banco                     N	003	    341
        ///024 � 024	Complemento de Registros	        N	001		0
        ///025 � 028	N�mero Agencia Debitada       	    N	004	    
        ///029 - 029	Complemento de Registros            A	001		Brancos
        ///030 - 036	Complemento de Registros            N	007		0000000
        ///037 - 041	N�mero da Conta Debitada            N	005     
        ///042 - 042	Complemento de Registros            A	001     Brancos
        ///043 - 043    D�gito Verificador da AG/Conta      N   001     
        ///044 - 073    Nome do Debitado                    A   030     
        ///074 - 088    Nr. do Docum. Atribu�do p/ Empresa  A   015     Nota 8
        ///089 - 093    Complemento de Registros            A   005     Brancos
        ///094 - 101    Data para o Lan�amento do D�bito    N   008     DDMMAAAA
        ///102 - 104    Tipo da Moeda                       A   005     Nota 9
        ///105 - 119	Quantidade da Moeda ou IOF          N	015		Nota 10
        ///120 - 134	Valor do Lan�amento p/ D�bito       N	015		Nota 10
        ///135 - 154	Complemento de Registros            A	020		Brancos
        ///155 - 162	Complemento de Registros            A	008		Brancos
        ///163 - 177	Complemento de Registros            N	015	    Brancos
        ///178 - 179	Tipo do Encargo por dia de Atraso 	N	002		Nota 12
        ///180 - 196    Valor do Encargo p/ dia de Atraso   N   017     Nota 12
        ///197 - 212	Info. Compl. p/ Hist�rico C/C       A	016		Nota 13
        ///213 - 216    Complemento de Registros            A   004     Brancos
        ///217 - 230    No. de Insc. do Debitado(CPF/CNPJ)  N   014     
        ///231 - 240    C�d. Ocr. para Retorno              A   010     Brancos
        /// </summary>
        private string GerarDetalheRemessaCnab240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var detalhe = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                detalhe += Utils.FormatCode("", "0", 4, true);
                detalhe += "3";
                detalhe += Utils.FormatCode("", "0", 5, true);
                detalhe += "A";
                detalhe += Utils.FormatCode("", "0", 3, true);
                detalhe += "000";
                detalhe += Utils.FormatCode(Codigo.ToString(), "0", 3, true);
                detalhe += "0";
                detalhe += Utils.FormatCode("", "0", 4, true);
                detalhe += " ";
                detalhe += Utils.FormatCode("", "0", 7);
                detalhe += Utils.FormatCode("", "4", 5, true);
                detalhe += " ";
                detalhe += Utils.FormatCode("", "0", 1);
                detalhe += Utils.FitStringLength(boleto.Sacado.Nome, 30, 30, ' ', 0, true, true, false);
                detalhe += Utils.FormatCode(boleto.NossoNumero, " ", 15);
                detalhe += Utils.FormatCode("", " ", 5);
                detalhe += DateTime.Now.ToString("ddMMyyyy");
                detalhe += Utils.FormatCode("", " ", 3);
                detalhe += Utils.FormatCode("", "0", 15, true);
                detalhe += Utils.FormatCode("", "0", 15, true);
                detalhe += Utils.FormatCode("", " ", 20);
                detalhe += Utils.FormatCode("", " ", 8);
                detalhe += Utils.FormatCode("", " ", 15);
                detalhe += Utils.FormatCode("", "0", 2, true);
                detalhe += Utils.FormatCode("", "0", 17, true);
                detalhe += Utils.FormatCode("", " ", 16);
                detalhe += Utils.FormatCode("", " ", 4);
                detalhe += Utils.FormatCode(boleto.Cedente.CpfCnpj, "0", 14, true);
                detalhe += Utils.FormatCode("", " ", 10);
                detalhe = Utils.SubstituiCaracteresEspeciais(detalhe);
                return detalhe;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB240.", e);
            }
        }

        private string GerarDetalheRemessaCnab400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                var detalhe = "1";                                            //001-001: Tipo de registro. 
                detalhe += boleto.Cedente.CpfCnpj.Length <= 11 ? "01" : "02"; //002-003: Tipo de inscri��o (CNPJ ou CPF).
                detalhe += Utils.FitStringLength(boleto.Cedente.CpfCnpj, 14, 14, '0', 0, true, true, true); //004-017: CNPJ ou CPF.
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true); //018-021: Ag�ncia.
                detalhe += "00";    //022-023: Zeros.
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 5, 5, '0', 0, true, true, true); //024-028: Conta corrente.
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, ' ', 0, true, true, false); //029-029: DV da conta corrente.
                detalhe += "    "; //030-033: Brancos.
                detalhe += "0000"; //034-037: C�digo da instru��o/alega��o a ser cancelada.

                detalhe += Utils.FitStringLength(boleto.NumeroControle ?? boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false); //038-062: Identifica��o do t�tulo na empresa.
                detalhe += Utils.FitStringLength(boleto.NossoNumero, 8, 8, '0', 0, true, true, true); //063-070: Nosso n�mero.
                detalhe += "0000000000000"; //071-083: Quantidade de moeda vari�vel, zeros quando for Real.
                detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true); //084-086: Carteira.
                detalhe += Utils.FitStringLength(" ", 21, 21, ' ', 0, true, true, true); //087-107: Identifica��o da opera��o.
                detalhe += "I"; //108-108: C�digo da carteira (I: Real, E:D�lar);
                detalhe += ObterCodigoDaOcorrencia(boleto); //109-110: Identifica��o da remessa.

                detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, ' ', 0, true, true, false); //111-120: N�mero do documento.
                detalhe += boleto.DataVencimento.ToString("ddMMyy"); //121-126: Data de vencimento.
                detalhe += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 13, 13, '0', 0, true, true, true); //127-139: Valor do boleto.
                detalhe += "341"; //140-142: C�digo do banco.
                detalhe += "00000"; //143-147: Ag�ncia onde o t�tulo ser� cobrado (no arquivo de remessa, preencher com zeros).
                detalhe += Utils.FitStringLength(EspecieDocumento.ValidaCodigo(boleto.EspecieDocumento), 2, 2, '0', 0, true, true, true); //148-149: Esp�cie do t�tulo.
                detalhe += Utils.FitStringLength(boleto.Aceite, 1, 1, ' ', 0, true, true, false); //150-150: Aceito ou N�o aceito.
                detalhe += boleto.DataDocumento.ToString("ddMMyy"); //151-156: Data de emiss�o do t�tulo.

                //157-160: 2 instru��es.
                switch (boleto.Instrucoes.Count)
                {
                    case 0:
                        detalhe += "0000";
                        break;
                    case 1:
                        detalhe += Utils.FitStringLength(boleto.Instrucoes[0].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                        detalhe += "00";
                        break;
                    default:
                        detalhe += Utils.FitStringLength(boleto.Instrucoes[0].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                        detalhe += Utils.FitStringLength(boleto.Instrucoes[1].Codigo.ToString(), 2, 2, '0', 0, true, true, true);
                        break;
                }

                detalhe += Utils.FitStringLength(boleto.JurosMora.ApenasNumeros(), 13, 13, '0', 0, true, true, true); //161-173: Valor de mora por dia de atraso.
                detalhe += boleto.DataDesconto == DateTime.MinValue ? boleto.DataVencimento.ToString("ddMMyy") : boleto.DataDesconto.ToString("ddMMyy"); //174-179: Data limite para concess�o do desconto.
                detalhe += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true); //180-192: Valor do desconto.
                detalhe += "0000000000000"; //193-205: Valor do IOF.
                detalhe += "0000000000000"; //206-218: Valor do Abatimento.

                detalhe += boleto.Sacado.CpfCnpj.Length <= 11 ? "01" : "02"; //219-220: Tipo de documento do sacado (CNPJ ou CPF).
                detalhe += Utils.FitStringLength(boleto.Sacado.CpfCnpj, 14, 14, '0', 0, true, true, true).ToUpper(); //221-234: CNPJ ou CPF do sacado.
                detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 30, 30, ' ', 0, true, true, false); //235-264: Nome do sacado.
                detalhe += new string(' ', 10); //265-274: Brancos.
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.EndComNumComplemento.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper(); //275-314: Rua, n�mero e complemento do sacado.
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 12, 12, ' ', 0, true, true, false).ToUpper(); //315-326: Bairro do sacado.
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cep, 8, 8, ' ', 0, true, true, false).ToUpper(); //327-334: CEP do sacado.
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade, 15, 15, ' ', 0, true, true, false).ToUpper(); //335-349: Cidade do sacado.
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Uf, 2, 2, ' ', 0, true, true, false).ToUpper(); //350-351: UF do sacado.

                detalhe += Utils.FitStringLength(boleto.Sacado.Nome, 30, 30, ' ', 0, true, true, false).ToUpper(); //352-381: Nome do sacador ou avalista.
                detalhe += "    "; //382-385: Brancos.
                detalhe += boleto.DataVencimento.ToString("ddMMyy"); //386-391: Data de mora.

                //392-393: Prazo para as instru��es. TODO
                if (boleto.Instrucoes.Count > 0)
                {
                    for (var i = 0; i < boleto.Instrucoes.Count; i++)
                    {
                        if (boleto.Instrucoes[i].Codigo == (int)EnumInstrucoes_Itau.Protestar ||
                            boleto.Instrucoes[i].Codigo == (int)EnumInstrucoes_Itau.ProtestarAposNDiasCorridos ||
                            boleto.Instrucoes[i].Codigo == (int)EnumInstrucoes_Itau.ProtestarAposNDiasUteis)
                        {
                            detalhe += boleto.Instrucoes[i].Dias.ToString("00");
                            break;
                        }

                        if (i == boleto.Instrucoes.Count - 1)
                            detalhe += "00";
                    }
                }
                else
                {
                    detalhe += "00";
                }

                detalhe += " "; //394-394: Branco.
                detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); //395-400: N�mero sequencial do registro.

                return Utils.SubstituiCaracteresEspeciais(detalhe);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        public string GerarRegistroDetalhe2(Boleto boleto, int numeroRegistro)
        {
            var dataMulta = boleto.DataMulta == DateTime.MinValue ? boleto.DataVencimento : boleto.DataMulta;

            var detalhe = new StringBuilder();
            detalhe.Append("2");                                        //001-001: Identificador do registro.
            detalhe.Append("2");                                        //002-002: 1: Valor em reais, 2: Valor em percentual.
            detalhe.Append(dataMulta.ToString("ddMMyyyy"));             //003-010: Data da multa.
            detalhe.Append(Utils.FitStringLength(Convert.ToInt32(boleto.PercMulta * 100).ToString(new CultureInfo("pt-BR")), 13, 13, '0', 0, true, true, true)); //011-023: Precentual da multa, duas casas decimais.
            detalhe.Append(new string(' ', 371));                       //024-394: Brancos.
            detalhe.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); //395-400: N�mero sequencial do registro.

            return Utils.SubstituiCaracteresEspeciais(detalhe.ToString());
        }

        #endregion

        #region Trailer Cnab240

        /// <summary>
        ///POS INI/FINAL	DESCRI��O	                   A/N	TAM	DEC	CONTE�DO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	C�digo do Banco na compensa��o	    N	003		341	
        ///004 - 007	Lote de servi�o	                    N	004		Nota 5 
        ///008 - 008	Registro Trailer de Lote            N	001     5
        ///009 - 017	Complemento de Registros            A	009     Brancos
        ///018 - 023    Qtd. Registros do Lote              N   006     Nota 15     
        ///024 - 041    Soma Valor dos D�bitos do Lote      N   018     Nota 14     
        ///042 - 059    Soma Qtd. de Moedas do Lote         N   018     Nota 14
        ///060 - 230    Complemento de Registros            A   171     Brancos
        ///231 - 240    C�d. Ocr. para Retorno              A   010     Brancos
        /// </summary>
        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // c�digo do banco na compensa��o - 001-003 9(03) - 341
                header += "0001";                                                                       // Lote de Servi�o - 004-007 9(04) - Nota 1
                header += "5";                                                                          // Tipo de Registro - 008-008 9(01) - 5
                header += Utils.FormatCode("", " ", 9);                                                 // Complemento de Registro - 009-017 X(09) - Brancos
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);                    // Quantidade de Registros do Lote - 018-023 9(06) - Nota 26

                // Totaliza��o da Cobran�a Simples
                header += Utils.FormatCode("", "0", 6);                                                 // Quantidade de T�tulos em Cobran�a Simples - 024-029 9(06) - Nota 24
                header += Utils.FormatCode("", "0", 17);                                                // Valor Total dos T�tulos em Carteiras - 030-046 9(15)V9(02) - Nota 24

                //Totaliza��o cobran�a vinculada
                header += Utils.FormatCode("", "0", 6);                                                 // Qtde de titulos em cobran�a vinculada - 047-052 9(06) - Nota 24
                header += Utils.FormatCode("", "0", 17);                                                // Valor total t�tulos em cobran�a vinculada - 053-069 9(15)V9(02) - Nota 24

                header += Utils.FormatCode("", "0", 46);                                                // Complemento de Registro - 070-115 X(08) - Zeros
                header += Utils.FormatCode("", " ", 8);                                                 // Refer�ncia do Aviso banc�rio - 116-123 X(08) - Nota 25
                header += Utils.FormatCode("", " ", 117);                                               // Complemento de Registro - 124-240 X(117) - Brancos

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar Trailer de Lote do arquivo de remessa.", e);
            }

            #region Informa��es geradas de forma inconsistente
            //suelton@gmail.com - 04/01/2017 - Gerando informa��es inconsistentes
            //try
            //{
            //    string trailer = Utils.FormatCode(Codigo.ToString(), "0", 3, true);
            //    trailer += Utils.FormatCode("", "0", 4, true);
            //    trailer += "5";
            //    trailer += Utils.FormatCode("", " ", 9);
            //    trailer += Utils.FormatCode("", "0", 6, true);
            //    trailer += Utils.FormatCode("", "0", 18, true);
            //    trailer += Utils.FormatCode("", "0", 18, true);
            //    trailer += Utils.FormatCode("", " ", 171);
            //    trailer += Utils.FormatCode("", " ", 10);
            //    trailer = Utils.SubstituiCaracteresEspeciais(trailer);

            //    return trailer;
            //}
            //catch (Exception e)
            //{
            //    throw new Exception("Erro durante a gera��o do registro TRAILER do LOTE de REMESSA.", e);
            //} 
            #endregion
        }

        /// <summary>
        ///POS INI/FINAL	DESCRI��O	                   A/N	TAM	DEC	CONTE�DO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	C�digo do Banco na compensa��o	    N	003		341	
        ///004 - 007	Lote de servi�o	                    N	004		9999 
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
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);      // c�digo do banco na compensa��o - 001-003 (03) - 341
                header += "9999";                                                    // Lote de Servi�o - 004-007 9(04) - '9999'
                header += "9";                                                       // Tipo de Registro - 008-008 9(1) - '9'
                header += Utils.FormatCode("", " ", 9);                              // Complemento de Registro - 009-017 X(09) - Brancos
                header += "000001";                                                  // Quantidade de Lotes do Arquivo - 018-023 9(06) - Nota 26
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true); // Quantidade de Registros do Arquivo - 024-029 9(06) - Nota 26
                header += Utils.FormatCode("", "0", 6);                              // Complemento de Registro - 030-035 9(06)
                header += Utils.FormatCode("", " ", 205);                            // Complemento de Registro - 036-240 X(205) - Brancos

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar Trailer de arquivo de remessa.", e);
            }
        }

        #endregion

        #region Trailer Cnab400

        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        throw new Exception("Tipo de arquivo inexistente.");
                    case TipoArquivo.Cnab400:
                        return GerarTrailerRemessa400(numeroRegistro);
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return " ";
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        private string GerarTrailerRemessa400(int numeroRegistro)
        {
            try
            {
                var trailer = "9";
                trailer += new string(' ', 393);
                trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); //N�mero sequencial do registro no arquivo.

                return Utils.SubstituiCaracteresEspeciais(trailer);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        /// <summary>
        /// DETALHE do arquivo CNAB
        /// Gera o DETALHE do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarMensagemVariavelRemessa(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var detalhe = "";

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        throw new Exception("Mensagem Variavel n�o existe para o tipo CNAB 240.");
                    case TipoArquivo.Cnab400:
                        //detalhe = GerarMensagemVariavelRemessaCnab400(boleto, ref numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return detalhe;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do DETALHE arquivo de REMESSA.", ex);
            }
        }

        private string GerarMensagemVariavelRemessaCnab400(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var registroOpcional = "2"; //Identifica��o do Registro (1, N).

                //Mensagem 1 (80, A).
                if (boleto.Instrucoes?.Count > 0)
                    registroOpcional += boleto.Instrucoes[0].Descricao.PadRight(80, ' ').Substring(0, 80);
                else
                    registroOpcional += new string(' ', 80);

                //Mensagem 2 (80, A).
                if (boleto.Instrucoes?.Count > 1)
                    registroOpcional += boleto.Instrucoes[1].Descricao.PadRight(80, ' ').Substring(0, 80);
                else
                    registroOpcional += new string(' ', 80);

                //Mensagem 3 (80, A).
                if (boleto.Instrucoes?.Count > 2)
                    registroOpcional += boleto.Instrucoes[2].Descricao.PadRight(80, ' ').Substring(0, 80);
                else
                    registroOpcional += new string(' ', 80);

                //Mensagem 4 (80, A).
                if (boleto.Instrucoes?.Count > 3)
                    registroOpcional += boleto.Instrucoes[3].Descricao.PadRight(80, ' ').Substring(0, 80);
                else
                    registroOpcional += new string(' ', 80);

                registroOpcional += new string(' ', 6);     //Data limite para concess�o de Desconto 2 (6, N) DDMMAA.
                registroOpcional += new string(' ', 13);    //Valor do Desconto (13, N).
                registroOpcional += new string(' ', 6);     //Data limite para concess�o de Desconto 3 (6, N) DDMMAA.
                registroOpcional += new string(' ', 13);    //Valor do Desconto (13, N).
                registroOpcional += new string(' ', 7);     //Reserva (7, A).
                registroOpcional += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true); //Carteira (3, N)
                registroOpcional += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);     //Ag�ncia (5, N) 
                registroOpcional += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 7, 7, '0', 0, true, true, true);       //Conta Corrente (7, N)
                registroOpcional += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true); //D�gito C/C (1, A)
                registroOpcional += Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true);      //Nosso N�mero (11, N)
                registroOpcional += Utils.FitStringLength("0", 1, 1, '0', 0, true, true, true);                       //DAC Nosso N�mero (1, A)
                registroOpcional += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); //N� Seq�encial do Registro (06, N)

                return Utils.SubstituiCaracteresEspeciais(registroOpcional);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar REGISTRO OPCIONAL do arquivo CNAB400.", ex);
            }
        }

        #endregion

        #region M�todos de processamento do arquivo retorno CNAB400

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                var dataOcorrencia = Utils.ToInt32(registro.Substring(110, 6));
                var dataVencimento = Utils.ToInt32(registro.Substring(146, 6));
                var dataCredito = Utils.ToInt32(registro.Substring(295, 6));

                var detalhe = new DetalheRetorno(registro);

                detalhe.CodigoInscricao = Utils.ToInt32(registro.Substring(1, 2));
                detalhe.NumeroInscricao = registro.Substring(3, 14);
                detalhe.Agencia = Utils.ToInt32(registro.Substring(17, 4));
                detalhe.Conta = Utils.ToInt32(registro.Substring(23, 5));
                detalhe.DACConta = Utils.ToInt32(registro.Substring(28, 1));
                detalhe.UsoEmpresa = registro.Substring(37, 25);

                detalhe.Carteira = registro.Substring(82, 1);
                detalhe.NossoNumeroComDV = registro.Substring(85, 9);
                detalhe.NossoNumero = registro.Substring(85, 8); //Sem o DV
                detalhe.DACNossoNumero = registro.Substring(93, 1); //DV

                detalhe.CodigoOcorrencia = Utils.ToInt32(registro.Substring(108, 2));

                //Descri��o da ocorr�ncia
                detalhe.DescricaoOcorrencia = Ocorrencia(registro.Substring(108, 2));

                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                detalhe.NumeroDocumento = registro.Substring(116, 10);

                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                decimal valorTitulo = Convert.ToInt64(registro.Substring(152, 13));
                detalhe.ValorTitulo = valorTitulo / 100;
                detalhe.CodigoBanco = Utils.ToInt32(registro.Substring(165, 3));
                detalhe.BancoCobrador = Utils.ToInt32(registro.Substring(165, 3));
                detalhe.AgenciaCobradora = Utils.ToInt32(registro.Substring(168, 4));
                detalhe.Especie = Utils.ToInt32(registro.Substring(173, 2));
                decimal tarifaCobranca = Convert.ToUInt64(registro.Substring(175, 13));
                detalhe.TarifaCobranca = tarifaCobranca / 100;
                // 26 brancos
                decimal iof = Convert.ToUInt64(registro.Substring(214, 13));
                detalhe.IOF = iof / 100;
                decimal valorAbatimento = !String.IsNullOrWhiteSpace(registro.Substring(227, 13)) ? Convert.ToUInt64(registro.Substring(227, 13)) : 0;
                detalhe.ValorAbatimento = valorAbatimento / 100;

                decimal valorDescontos = Convert.ToUInt64(registro.Substring(240, 13));
                detalhe.Descontos = valorDescontos / 100;

                decimal valorPrincipal = Convert.ToUInt64(registro.Substring(253, 13));
                detalhe.ValorPrincipal = valorPrincipal / 100;

                decimal jurosMora = Convert.ToUInt64(registro.Substring(266, 13));
                detalhe.JurosMora = jurosMora / 100;
                // 293 - 3 brancos
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                detalhe.InstrucaoCancelada = Utils.ToInt32(registro.Substring(301, 4));
                // 306 - 6 brancos
                // 311 - 13 zeros
                detalhe.NomeSacado = registro.Substring(324, 30);
                // 354 - 23 brancos
                detalhe.Erros = registro.Substring(377, 8);

                if (!string.IsNullOrWhiteSpace(detalhe.Erros))
                {
                    var detalheErro = detalhe.Erros;

                    var motivo1 = MotivoRejeicao(detalhe.Erros.Substring(0, 2));
                    var motivo2 = MotivoRejeicao(detalhe.Erros.Substring(2, 2));
                    var motivo3 = MotivoRejeicao(detalhe.Erros.Substring(4, 2));

                    if (!string.IsNullOrWhiteSpace(motivo1))
                        detalheErro += " - " + motivo1;

                    if (!string.IsNullOrWhiteSpace(motivo2))
                        detalheErro += " / " + motivo2;

                    if (!string.IsNullOrWhiteSpace(motivo3))
                        detalheErro += " / " + motivo3;

                    detalhe.Erros = detalheErro;
                }

                // 377 - Registros rejeitados ou alega��o do sacado
                // 386 - 7 brancos

                detalhe.CodigoLiquidacao = registro.Substring(392, 2);
                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));
                detalhe.ValorPago = detalhe.ValorPrincipal;

                // A correspond�ncia de Valor Pago no RETORNO ITA� � o Valor Principal (Valor lan�ado em Conta Corrente - Conforme Manual)
                // A determina��o se D�bito ou Cr�dito dever� ser feita nos aplicativos por se tratar de personaliza��o.
                // Para isso, considerar o C�digo da Ocorr�ncia e tratar de acordo com suas necessidades.
                // Alterado por jsoda em 04/06/2012
                //
                //// Valor principal � d�bito ou cr�dito ?
                //if ( (detalhe.ValorTitulo < detalhe.TarifaCobranca) ||
                //     ((detalhe.ValorTitulo - detalhe.Descontos) < detalhe.TarifaCobranca)
                //    )
                //{
                //    detalhe.ValorPrincipal *= -1; // Para d�bito coloca valor negativo
                //}

                //// Valor Pago � a soma do Valor Principal (Valor que entra na conta) + Tarifa de Cobran�a
                //detalhe.ValorPago = detalhe.ValorPrincipal + detalhe.TarifaCobranca;

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
                header.Agencia = Utils.ToInt32(registro.Substring(026, 4));
                header.ComplementoRegistro1 = Utils.ToInt32(registro.Substring(030, 2));
                header.Conta = Utils.ToInt32(registro.Substring(032, 5));
                header.DACConta = Utils.ToInt32(registro.Substring(037, 1));
                header.ComplementoRegistro2 = registro.Substring(038, 8);
                header.NomeEmpresa = registro.Substring(046, 30);
                header.CodigoBanco = Utils.ToInt32(registro.Substring(076, 3));
                header.NomeBanco = registro.Substring(079, 15);
                header.DataGeracao = Utils.ToDateTime(Utils.ToInt32(registro.Substring(094, 6)).ToString("##-##-##"));
                header.Densidade = Utils.ToInt32(registro.Substring(100, 5));
                header.UnidadeDensidade = registro.Substring(105, 3);
                header.NumeroSequencialArquivoRetorno = Utils.ToInt32(registro.Substring(108, 5));
                header.DataCredito = Utils.ToDateTime(Utils.ToInt32(registro.Substring(113, 6)).ToString("##-##-##"));
                header.ComplementoRegistro3 = registro.Substring(119, 275);
                header.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));
                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler header do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #endregion

        /// <summary>
        /// Efetua as Valida��es dentro da classe Boleto, para garantir a gera��o da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            var vRetorno = true;
            var vMsg = string.Empty;
            ////IMPLEMENTACAO PENDENTE...
            mensagem = vMsg;
            return vRetorno;
        }

        public override long ObterNossoNumeroSemConvenioOuDigitoVerificador(long convenio, string nossoNumero)
        {
            //Ita� n�o utiliza DV ou conv�nio com o nosso n�mero.
            if (long.TryParse(nossoNumero, out var numero))
                return numero;

            throw new NossoNumeroInvalidoException();
        }
    }
}