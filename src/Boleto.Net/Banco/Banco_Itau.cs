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
    /// Classe referente ao banco Itaú
    /// </summary>
    internal sealed class BancoItau : AbstractBanco, IBanco
    {
        #region Variáveis

        private int _dacBoleto = 0;

        #endregion

        #region Construtores

        internal BancoItau()
        {
            try
            {
                Codigo = 341;
                Digito = "7";
                Nome = "Itaú";
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

        #endregion

        #region Métodos de Instância

        /// <summary>
        /// Validações particulares do banco Itaú
        /// </summary>
        public override void ValidaBoleto(Boleto boleto)
        {
            try
            {
                //Carteiras válidas.
                var carteiras = new[] { "175", "176", "178", "109", "198", "107", "122", "142", "143", "196", "126", "131", "146", "150", "169", "121", "112", "104" };

                if (!carteiras.Contains(boleto.Carteira))
                {
                    var validas = carteiras.Aggregate("", (p, n) => p + (p.Length > 0 ? ", " : "") + n);
                    throw new NotImplementedException($"Carteira não implementada: {boleto.Carteira}\r\nCarteiras válidas: {validas}");
                }

                //Verifica se o NossoNumero é um inteiro válido.
                int aux;
                if (!int.TryParse(boleto.NossoNumero, out aux))
                    throw new NotImplementedException("Nosso número para a carteira " + boleto.Carteira + " inválido.");

                //Verifica se o tamanho para o NossoNumero são 8 dígitos.
                if (boleto.NossoNumero.Length > 8)
                    throw new NotImplementedException("A quantidade de dígitos do nosso número para a carteira " + boleto.Carteira + ", são 8 números.");

                if (boleto.NossoNumero.Length < 8)
                    boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 8);

                //É obrigatório o preenchimento do número do documento
                if (boleto.Carteira == "106" || boleto.Carteira == "107" || boleto.Carteira == "122" || boleto.Carteira == "142" || boleto.Carteira == "143" || boleto.Carteira == "195" || boleto.Carteira == "196" || boleto.Carteira == "198")
                    if (Utils.ToInt32(boleto.NumeroDocumento) == 0)
                        throw new NotImplementedException("O número do documento não pode ser igual a zero.");

                //Formato o número do documento 
                if (Utils.ToInt32(boleto.NumeroDocumento) > 0)
                    boleto.NumeroDocumento = Utils.FormatCode(boleto.NumeroDocumento, 7);

                //Calcula o DAC da Conta Corrente
                boleto.Cedente.ContaBancaria.DigitoConta = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta).ToString();

                //Calcula o DAC do Nosso Número a maioria das carteiras. Agência/conta/carteira/nosso número.
                //if (boleto.Carteira == "104" || boleto.Carteira == "112")
                //    _dacNossoNumero = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Cedente.ContaBancaria.DigitoConta + boleto.Carteira + boleto.NossoNumero);
                //else if (boleto.Carteira != "126" && boleto.Carteira != "131" && boleto.Carteira != "146" && boleto.Carteira != "150" && boleto.Carteira != "168")
                //    _dacNossoNumero = Mod10(boleto.Cedente.ContaBancaria.Agencia + boleto.Cedente.ContaBancaria.Conta + boleto.Carteira + boleto.NossoNumero);
                //else
                //    _dacNossoNumero = Mod10(boleto.Carteira + boleto.NossoNumero); //Excessão 126 - 131 - 146 - 150 - 168 carteira/nosso numero

                //boleto.DigitoNossoNumero = _dacNossoNumero.ToString();

                boleto.LocalPagamento = "Até o vencimento em qualquer banco ou correspondente não bancário. ";
                //"Após o vencimento, acesse itau.com.br/boletos e pague<br> em qualquer banco ou correspondente não bancário.";

                //Verifica se o nosso número é válido.
                if (Utils.ToInt64(boleto.NossoNumero) == 0)
                    throw new NotImplementedException("Nosso número inválido");

                //Verifica se data do processamento é valida.
                if (boleto.DataProcessamento == DateTime.MinValue)
                    boleto.DataProcessamento = DateTime.Now;

                //Verifica se data do documento é valida.
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

        #region Métodos de formatação do boleto

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
                throw new Exception("Erro ao formatar código de barras.", ex);
            }
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            try
            {
                var c2 = string.Empty;
                var c3 = string.Empty;

                #region Campo 1 (AAABC.CCDDX)

                var aaa = Utils.FormatCode(Codigo.ToString(), 3); //Código do banco, 341: Itaú.
                var b = boleto.Moeda.ToString(); //Tipo da moeda, 9: Real.
                var ccc = boleto.Carteira.PadLeft(3, '0'); //Carteira, 3 dígitos.
                var dd = boleto.NossoNumero.Substring(0, 2); //Nosso número, 2 primeiros dígitos.
                var x = Mod10(aaa + b + ccc + dd).ToString(); //Dígito verificador do campo 1, 1 dígito.

                var c1 = $"{aaa}{b}{ccc.Substring(0, 1)}.{ccc.Substring(1, 2)}{dd}{x}";

                #endregion

                if (boleto.Carteira == "175" || boleto.Carteira == "176" || boleto.Carteira == "178" || boleto.Carteira == "109" || boleto.Carteira == "121" || boleto.Carteira == "112" || boleto.Carteira == "104")
                {
                    #region Definições

                    /* AAABC.CCDDX.DDDDD.DEFFFY.FGGGG.GGHHHZ.K.UUUUVVVVVVVVVV
                     * ------------------------------------------------------
                     * Campo 1
                     * AAABC.CCDDX
                     * AAA - Código do Banco
                     * B   - Moeda
                     * CCC - Carteira
                     * DD  - 2 primeiros números Nosso Número
                     * X   - DAC Campo 1 (AAABC.CCDD) Mod10
                     * 
                     * Campo 2
                     * DDDDD.DEFFFY
                     * DDDDD.D - Restante Nosso Número
                     * E       - DAC (Agência/Conta/Carteira/Nosso Número)
                     * FFF     - Três primeiros da agência
                     * Y       - DAC Campo 2 (DDDDD.DEFFF) Mod10
                     * 
                     * Campo 3
                     * FGGGG.GGHHHZ
                     * F       - Restante da Agência
                     * GGGG.GG - Número Conta Corrente + DAC
                     * HHH     - Zeros (Não utilizado)
                     * Z       - DAC Campo 3
                     * 
                     * Campo 4
                     * K       - DAC Código de Barras
                     * 
                     * Campo 5
                     * UUUUVVVVVVVVVV
                     * UUUU       - Fator Vencimento
                     * VVVVVVVVVV - Valor do Título 
                     */

                    #endregion Definições

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
                    #region Definições

                    /* AAABC.CCDDX.DDDDD.DEEEEY.EEEFF.FFFGHZ.K.UUUUVVVVVVVVVV
                     * ------------------------------------------------------
                     * Campo 1 - AAABC.CCDDX
                     * AAA - Código do Banco
                     * B   - Moeda
                     * CCC - Carteira
                     * DD  - 2 primeiros números Nosso Número
                     * X   - DAC Campo 1 (AAABC.CCDD) Mod10
                     * 
                     * Campo 2 - DDDDD.DEEEEY
                     * DDDDD.D - Restante Nosso Número
                     * EEEE    - 4 primeiros numeros do número do documento
                     * Y       - DAC Campo 2 (DDDDD.DEEEEY) Mod10
                     * 
                     * Campo 3 - EEEFF.FFFGHZ
                     * EEE     - Restante do número do documento
                     * FFFFF   - Código do Cliente
                     * G       - DAC (Carteira/Nosso Numero(sem DAC)/Numero Documento/Codigo Cliente)
                     * H       - zero
                     * Z       - DAC Campo 3
                     * 
                     * Campo 4 - K
                     * K       - DAC Código de Barras
                     * 
                     * Campo 5 - UUUUVVVVVVVVVV
                     * UUUU       - Fator Vencimento
                     * VVVVVVVVVV - Valor do Título 
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
                    throw new NotImplementedException("Função não implementada.");
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
                throw new Exception("Erro ao formatar linha digitável.", ex);
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
                throw new Exception("Erro ao formatar nosso número", ex);
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
                throw new Exception("Erro ao formatar número do documento.", ex);
            }
        }

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
                case "04":
                    return "04-Alteração de Dados-Nova entrada ou Alteração/Exclusão de dados acatada";
                case "05":
                    return "05-Alteração de dados-Baixa";
                case "06":
                    return "06-Liquidação normal";
                case "08":
                    return "08-Liquidação em cartório";
                case "09":
                    return "09-Baixa simples";
                case "10":
                    return "10-Baixa por ter sido liquidado";
                case "11":
                    return "11-Em Ser (Só no retorno mensal)";
                case "12":
                    return "12-Abatimento Concedido";
                case "13":
                    return "13-Abatimento Cancelado";
                case "14":
                    return "14-Vencimento Alterado";
                case "15":
                    return "15-Baixas rejeitadas";
                case "16":
                    return "16-Instruções rejeitadas";
                case "17":
                    return "17-Alteração/Exclusão de dados rejeitados";
                case "18":
                    return "18-Cobrança contratual-Instruções/Alterações rejeitadas/pendentes";
                case "19":
                    return "19-Confirma Recebimento Instrução de Protesto";
                case "20":
                    return "20-Confirma Recebimento Instrução Sustação de Protesto/Tarifa";
                case "21":
                    return "21-Confirma Recebimento Instrução de Não Protestar";
                case "23":
                    return "23-Título enviado a Cartório/Tarifa";
                case "24":
                    return "24-Instrução de Protesto Rejeitada/Sustada/Pendente";
                case "25":
                    return "25-Alegações do Sacado";
                case "26":
                    return "26-Tarifa de Aviso de Cobrança";
                case "27":
                    return "27-Tarifa de Extrato Posição";
                case "28":
                    return "28-Tarifa de Relação das Liquidações";
                case "29":
                    return "29-Tarifa de Manutenção de Títulos Vencidos";
                case "30":
                    return "30-Débito Mensal de Tarifas (Para Entradas e Baixas)";
                case "32":
                    return "32-Baixa por ter sido Protestado";
                case "33":
                    return "33-Custas de Protesto";
                case "34":
                    return "34-Custas de Sustação";
                case "35":
                    return "35-Custas de Cartório Distribuidor";
                case "36":
                    return "36-Custas de Edital";
                case "37":
                    return "37-Tarifa de Emissão de Boleto/Tarifa de Envio de Duplicata";
                case "38":
                    return "38-Tarifa de Instrução";
                case "39":
                    return "39-Tarifa de Ocorrências";
                case "40":
                    return "40-Tarifa Mensal de Emissão de Boleto/Tarifa Mensal de Envio de Duplicata";
                case "41":
                    return "41-Débito Mensal de Tarifas-Extrato de Posição(B4EP/B4OX)";
                case "42":
                    return "42-Débito Mensal de Tarifas-Outras Instruções";
                case "43":
                    return "43-Débito Mensal de Tarifas-Manutenção de Títulos Vencidos";
                case "44":
                    return "44-Débito Mensal de Tarifas-Outras Ocorrências";
                case "45":
                    return "45-Débito Mensal de Tarifas-Protesto";
                case "46":
                    return "56-Débito Mensal de Tarifas-Sustação de Protesto";
                case "47":
                    return "47-Baixa com Transferência para Protesto";
                case "48":
                    return "48-Custas de Sustação Judicial";
                case "51":
                    return "51-Tarifa Mensal Ref a Entradas Bancos Correspondentes na Carteira";
                case "52":
                    return "52-Tarifa Mensal Baixas na Carteira";
                case "53":
                    return "53-Tarifa Mensal Baixas em Bancos Correspondentes na Carteira";
                case "54":
                    return "54-Tarifa Mensal de Liquidações na Carteira";
                case "55":
                    return "55-Tarifa Mensal de Liquidações em Bancos Correspondentes na Carteira";
                case "56":
                    return "56-Custas de Irregularidade";
                case "57":
                    return "57-Instrução Cancelada";
                case "59":
                    return "59-Baixa por Crédito em C/C Através do SISPAG";
                case "60":
                    return "60-Entrada Rejeitada Carnê";
                case "61":
                    return "61-Tarifa Emissão Aviso de Movimentação de Títulos";
                case "62":
                    return "62-Débito Mensal de Tarifa-Aviso de Movimentação de Títulos";
                case "63":
                    return "63-Título Sustado Judicialmente";
                case "64":
                    return "64-Entrada Confirmada com Rateio de Crédito";
                case "69":
                    return "69-Cheque Devolvido";
                case "71":
                    return "71-Entrada Registrada-Aguardando Avaliação";
                case "72":
                    return "72-Baixa por Crédito em C/C Através do SISPAG sem Título Correspondente";
                case "73":
                    return "73-Confirmação de Entrada na Cobrança Simples-Entrada Não Aceita na Cobrança Contratual";
                case "76":
                    return "76-Cheque Compensado";
                default:
                    return "";
            }
        }

        /// <summary>
        /// Verifica o código do motivo da rejeição informada pelo banco
        /// </summary>
        private string MotivoRejeicao(string codigo)
        {
            switch (codigo)
            {
                case "03": return "03-AG. COBRADORA - CEP SEM ATENDIMENTO DE PROTESTO NO MOMENTO";
                case "04": return "04-ESTADO - SIGLA DO ESTADO INVÁLIDA";
                case "05": return "05-DATA VENCIMENTO - PRAZO DA OPERAÇÃO MENOR QUE PRAZO MÍNIMO OU MAIOR QUE O MÁXIMO";
                case "07": return "07-VALOR DO TÍTULO - VALOR DO TÍTULO MAIOR QUE 10.000.000,00";
                case "08": return "08-NOME DO PAGADOR - NÃO INFORMADO OU DESLOCADO";
                case "09": return "09-AGENCIA/CONTA - AGÊNCIA ENCERRADA";
                case "10": return "10-LOGRADOURO - NÃO INFORMADO OU DESLOCADO";
                case "11": return "11-CEP - CEP NÃO NUMÉRICO OU CEP INVÁLIDO";
                case "12": return "12-SACADOR / AVALISTA - NOME NÃO INFORMADO OU DESLOCADO (BANCOS CORRESPONDENTES)";
                case "13": return "13-ESTADO/CEP - CEP INCOMPATÍVEL COM A SIGLA DO ESTADO";
                case "14": return "14-NOSSO NÚMERO - NOSSO NÚMERO JÁ REGISTRADO NO CADASTRO DO BANCO OU FORA DA FAIXA";
                case "15": return "15-NOSSO NÚMERO - NOSSO NÚMERO EM DUPLICIDADE NO MESMO MOVIMENTO";
                case "18": return "18-DATA DE ENTRADA - DATA DE ENTRADA INVÁLIDA PARA OPERAR COM ESTA CARTEIRA";
                case "19": return "19-OCORRÊNCIA - OCORRÊNCIA INVÁLIDA";
                case "21": return "21-AG. COBRADORA - CARTEIRA NÃO ACEITA DEPOSITÁRIA CORRESPONDENTE ESTADO DA AGÊNCIA DIFERENTE DO ESTADO DO PAGADOR AG. COBRADORA NÃO CONSTA NO CADASTRO OU ENCERRANDO";
                case "22": return "22-CARTEIRA - CARTEIRA NÃO PERMITIDA (NECESSÁRIO CADASTRAR FAIXA LIVRE)";
                case "26": return "26-AGÊNCIA/CONTA - AGÊNCIA/CONTA NÃO LIBERADA PARA OPERAR COM COBRANÇA";
                case "27": return "27-CNPJ INAPTO - CNPJ DO BENEFICIÁRIO INAPTO DEVOLUÇÃO DE TÍTULO EM GARANTIA";
                case "29": return "29-CÓDIGO EMPRESA - CATEGORIA DA CONTA INVÁLIDA";
                case "30": return "30-ENTRADA BLOQUEADA - ENTRADAS BLOQUEADAS, CONTA SUSPENSA EM COBRANÇA";
                case "31": return "31-AGÊNCIA/CONTA - CONTA NÃO TEM PERMISSÃO PARA PROTESTAR (CONTATE SEU GERENTE)";
                case "35": return "35-VALOR DO IOF - IOF MAIOR QUE 5%";
                case "36": return "36-QTDADE DE MOEDA - QUANTIDADE DE MOEDA INCOMPATÍVEL COM VALOR DO TÍTULO";
                case "37": return "37-CNPJ/CPF DO PAGADOR - NÃO NUMÉRICO OU IGUAL A ZEROS";
                case "42": return "42-NOSSO NÚMERO - NOSSO NÚMERO FORA DE FAIXA";
                case "52": return "52-AG. COBRADORA - EMPRESA NÃO ACEITA BANCO CORRESPONDENTE";
                case "53": return "53-AG. COBRADORA - EMPRESA NÃO ACEITA BANCO CORRESPONDENTE - COBRANÇA MENSAGEM";
                case "54": return "54-DATA DE VENCTO - BANCO CORRESPONDENTE - TÍTULO COM VENCIMENTO INFERIOR A 15 DIAS";
                case "55": return "55-DEP/BCO CORRESP - CEP NÃO PERTENCE À DEPOSITÁRIA INFORMADA";
                case "56": return "56-DT VENCTO/BCO CORRESP - VENCTO SUPERIOR A 180 DIAS DA DATA DE ENTRADA";
                case "57": return "57-DATA DE VENCTO - CEP SÓ DEPOSITÁRIA BCO DO BRASIL COM VENCTO INFERIOR A 8 DIAS";
                case "60": return "60-ABATIMENTO - VALOR DO ABATIMENTO INVÁLIDO";
                case "61": return "61-JUROS DE MORA - JUROS DE MORA MAIOR QUE O PERMITIDO";
                case "62": return "62-DESCONTO - VALOR DO DESCONTO MAIOR QUE VALOR DO TÍTULO";
                case "63": return "63-DESCONTO DE ANTECIPAÇÃO - VALOR DA IMPORTÂNCIA POR DIA DE DESCONTO (IDD) NÃO PERMITIDO";
                case "64": return "64-DATA DE EMISSÃO - DATA DE EMISSÃO DO TÍTULO INVÁLIDA";
                case "65": return "65-TAXA FINANCTO - TAXA INVÁLIDA (VENDOR)";
                case "66": return "66-DATA DE VENCTO - INVALIDA/FORA DE PRAZO DE OPERAÇÃO (MÍNIMO OU MÁXIMO)";
                case "67": return "67-VALOR/QTIDADE - VALOR DO TÍTULO/QUANTIDADE DE MOEDA INVÁLIDO";
                case "68": return "68-CARTEIRA - CARTEIRA INVÁLIDA OU NÃO CADASTRADA NO INTERCÂMBIO DA COBRANÇA";
                case "69": return "69-CARTEIRA - CARTEIRA INVÁLIDA PARA TÍTULOS COM RATEIO DE CRÉDITO";
                case "70": return "70-AGÊNCIA/CONTA - BENEFICIÁRIO NÃO CADASTRADO PARA FAZER RATEIO DE CRÉDITO";
                case "78": return "78-AGÊNCIA/CONTA - DUPLICIDADE DE AGÊNCIA/CONTA BENEFICIÁRIA DO RATEIO DE CRÉDITO";
                case "80": return "80-AGÊNCIA/CONTA - QUANTIDADE DE CONTAS BENEFICIÁRIAS DO RATEIO MAIOR DO QUE O PERMITIDO (MÁXIMO DE 30 CONTAS POR TÍTULO)";
                case "81": return "81-AGÊNCIA/CONTA - CONTA PARA RATEIO DE CRÉDITO INVÁLIDA / NÃO PERTENCE AO ITAÚ";
                case "82": return "82-DESCONTO/ABATI-MENTO - DESCONTO/ABATIMENTO NÃO PERMITIDO PARA TÍTULOS COM RATEIO DE CRÉDITO";
                case "83": return "83-VALOR DO TÍTULO - VALOR DO TÍTULO MENOR QUE A SOMA DOS VALORES ESTIPULADOS PARA RATEIO";
                case "84": return "84-AGÊNCIA/CONTA - AGÊNCIA/CONTA BENEFICIÁRIA DO RATEIO É A CENTRALIZADORA DE CRÉDITO DO BENEFICIÁRIO";
                case "85": return "85-AGÊNCIA/CONTA - AGÊNCIA/CONTA DO BENEFICIÁRIO É CONTRATUAL / RATEIO DE CRÉDITO NÃO PERMITIDO";
                case "86": return "86-TIPO DE VALOR - CÓDIGO DO TIPO DE VALOR INVÁLIDO / NÃO PREVISTO PARA TÍTULOS COM RATEIO DE CRÉDITO";
                case "87": return "87-AGÊNCIA/CONTA - REGISTRO TIPO 4 SEM INFORMAÇÃO DE AGÊNCIAS/CONTAS BENEFICIÁRIAS DO RATEIO";
                case "90": return "90-NRO DA LINHA - COBRANÇA MENSAGEM - NÚMERO DA LINHA DA MENSAGEM INVÁLIDO OU QUANTIDADE DE LINHAS EXCEDIDAS";
                case "97": return "97-SEM MENSAGEM - COBRANÇA MENSAGEM SEM MENSAGEM (SÓ DE CAMPOS FIXOS), PORÉM COM REGISTRO DO TIPO 7 OU 8";
                case "98": return "98-FLASH INVÁLIDO - REGISTRO MENSAGEM SEM FLASH CADASTRADO OU FLASH INFORMADO DIFERENTE DO CADASTRADO";
                case "99": return "99-FLASH INVÁLIDO - CONTA DE COBRANÇA COM FLASH CADASTRADO E SEM REGISTRO DE MENSAGEM CORRESPONDENTE";
                default:
                    return string.Empty;
            }
        }

        #endregion

        #region Métodos de geração do arquivo remessa

        #region Header

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cendente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
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
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        /// <summary>
        ///POS INI/FINAL	DESCRIÇÃO	                   A/N	TAM	DEC	CONTEÚDO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	Código do Banco na compensação	    N	003		341	
        ///004 - 007	Lote de serviço	                    N	004		0000	1 
        ///008 - 008	Registro Hearder de Arquivo         N	001		0	2
        ///009 - 017	Reservado (uso Banco)	            A	009		Brancos	  
        ///018 - 018	Tipo de inscrição da empresa	    N	001		1 = CPF,  2 = CNPJ 	
        ///019 – 032	Nº de inscrição da empresa	        N	014		Nota 1
        ///033 – 045	Código do Convênio no Banco   	    A	013	    Nota 2 
        ///046 - 052	Reservado (uso Banco)	            A	007		Brancos	
        ///053 - 053	Complemento de Registro             N	001     0			
        ///054 - 057	Agência Referente Convênio Ass.     N	004     Nota 1
        ///058 - 058    Complemento de Registro             A   001     Brancos
        ///059 - 065    Complemento de Registro             N   007     Brancos
        ///066 - 070    Número da C/C do Cliente            N   005     Nota 1
        ///071 - 071    Complemento de Registro             A   001     Brancos
        ///072 - 072    DAC da Agência/Conta                N   001     Nota 1
        ///073 - 102    Nome da Empresa                     A   030     Nome da Empresa
        ///103 - 132	Nome do Banco	                    A	030		Banco Itaú 	
        ///133 - 142	Reservado (uso Banco)	            A	010		Brancos	
        ///143 - 143	Código remessa 	                    N	001		1 = Remessa 	
        ///144 - 151	Data de geração do arquivo	        N	008		DDMMAAAA	
        ///152 - 157	Hora de geração do arquivo          N	006		HHMMSS
        ///158 - 163	Nº seqüencial do arquivo 	        N	006	    Nota 3
        ///164 - 166	Nº da versão do layout do arquivo	N	003		040
        ///167 - 171    Densidaded de Gravação do arquivo   N   005     00000
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
                        throw new Exception("Não implementado.");
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

        /// <summary>
        ///POS INI/FINAL	DESCRIÇÃO	                   A/N	TAM	DEC	CONTEÚDO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	Código do Banco na compensação	    N	003		341	
        ///004 - 007	Lote de serviço	                    N	004		Nota 5 
        ///008 - 008	Registro Hearder de Lote            N	001     1
        ///009 - 009	Tipo de Operação                    A	001		D
        ///010 - 011	Tipo de serviço             	    N	002		05
        ///012 – 013	Forma de Lançamento                 N	002		50
        ///014 – 016	Número da versão do Layout   	    A	003	    030
        ///017 - 017	Complemento de Registro             A	001		Brancos	
        ///019 – 032	Nº de inscrição da empresa	        N	014		Nota 1
        ///033 – 045	Código do Convênio no Banco   	    A	013	    Nota 2
        ///018 - 018	Tipo de inscrição da empresa	    N	001		1 = CPF,  2 = CNPJ
        ///046 - 052	Reservado (uso Banco)	            A	007		Brancos	
        ///053 - 053	Complemento de Registro             N	001     0			
        ///054 - 057	Agência Referente Convênio Ass.     N	004     Nota 1
        ///058 - 058    Complemento de Registro             A   001     Brancos
        ///059 - 065    Complemento de Registro             N   007     0000000
        ///066 - 070    Número da C/C do Cliente            N   005     Nota 1
        ///071 - 071    Complemento de Registro             A   001     Brancos
        ///072 - 072    DAC da Agência/Conta                N   001     Nota 1
        ///073 - 102    Nome da Empresa                     A   030     ENIX...
        ///103 - 142	Complemento de Registro             A	040		Brancos
        ///143 - 172	Endereço da Empresa                 A	030		Nome da rua, Av., Pça, etc.
        ///173 - 177	Número do local                     N	005		Número do Local da Empresa
        ///178 - 192	Complemento                         A	015		Casa, Apto., Sala, etc.
        ///193 - 212	Nome da Cidade                      A	020	    Sao Paulo
        ///213 - 220	CEP                             	N	008		CEP
        ///221 - 222    Sigla do Estado                     A   002     SP
        ///223 - 230	Complemento de Registro             A	008		Brancos
        ///231 - 240    Cód. Ocr. para Retorno              A   010     Brancos
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
                throw new Exception("Erro durante a geração do SEGMENTO P DO DETALHE do arquivo de REMESSA.", ex);
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
                throw new Exception("Erro durante a geração do SEGMENTO Q DO DETALHE do arquivo de REMESSA.", ex);
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
                    // Código da multa 2 - percentual
                    segmentoR += "2";
                }
                else if (boleto.ValorMulta > 0)
                {
                    // Código da multa 1 - valor fixo
                    segmentoR += "1";
                }
                else
                {
                    //Código da multa 0 - sem multa
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
                throw new Exception("Erro durante a geração do SEGMENTO R DO DETALHE do arquivo de REMESSA.", ex);
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
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }

        /// <summary>
        ///POS INI/FINAL	DESCRIÇÃO	                   A/N	TAM	DEC	CONTEÚDO	NOTAS
        ///--------------------------------------------------------------------------------
        ///001 - 003	Código do Banco na compensação	    N	003		341	
        ///004 - 007	Lote de serviço	                    N	004		Nota 5 
        ///008 - 008	Registro Detalhe de Lote            N	001     3
        ///009 - 013	Número Sequencial Registro Lote     N	005		Nota 6
        ///014 - 014	Código Segmento Reg. Detalhe   	    A	001		A
        ///015 – 017	Código da Instrução p/ Movimento    N	003		Nota 7
        ///018 - 020	Código da Câmara de Compensação     N	003	    000
        ///021 - 023	Código do Banco                     N	003	    341
        ///024 – 024	Complemento de Registros	        N	001		0
        ///025 – 028	Número Agencia Debitada       	    N	004	    
        ///029 - 029	Complemento de Registros            A	001		Brancos
        ///030 - 036	Complemento de Registros            N	007		0000000
        ///037 - 041	Número da Conta Debitada            N	005     
        ///042 - 042	Complemento de Registros            A	001     Brancos
        ///043 - 043    Dígito Verificador da AG/Conta      N   001     
        ///044 - 073    Nome do Debitado                    A   030     
        ///074 - 088    Nr. do Docum. Atribuído p/ Empresa  A   015     Nota 8
        ///089 - 093    Complemento de Registros            A   005     Brancos
        ///094 - 101    Data para o Lançamento do Débito    N   008     DDMMAAAA
        ///102 - 104    Tipo da Moeda                       A   005     Nota 9
        ///105 - 119	Quantidade da Moeda ou IOF          N	015		Nota 10
        ///120 - 134	Valor do Lançamento p/ Débito       N	015		Nota 10
        ///135 - 154	Complemento de Registros            A	020		Brancos
        ///155 - 162	Complemento de Registros            A	008		Brancos
        ///163 - 177	Complemento de Registros            N	015	    Brancos
        ///178 - 179	Tipo do Encargo por dia de Atraso 	N	002		Nota 12
        ///180 - 196    Valor do Encargo p/ dia de Atraso   N   017     Nota 12
        ///197 - 212	Info. Compl. p/ Histórico C/C       A	016		Nota 13
        ///213 - 216    Complemento de Registros            A   004     Brancos
        ///217 - 230    No. de Insc. do Debitado(CPF/CNPJ)  N   014     
        ///231 - 240    Cód. Ocr. para Retorno              A   010     Brancos
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
                detalhe += boleto.Cedente.CpfCnpj.Length <= 11 ? "01" : "02"; //002-003: Tipo de inscrição (CNPJ ou CPF).
                detalhe += Utils.FitStringLength(boleto.Cedente.CpfCnpj, 14, 14, '0', 0, true, true, true); //004-017: CNPJ ou CPF.
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 4, 4, '0', 0, true, true, true); //018-021: Agência.
                detalhe += "00";    //022-023: Zeros.
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 5, 5, '0', 0, true, true, true); //024-028: Conta corrente.
                detalhe += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, ' ', 0, true, true, false); //029-029: DV da conta corrente.
                detalhe += "    "; //030-033: Brancos.
                detalhe += "0000"; //034-037: Código da instrução/alegação a ser cancelada.

                detalhe += Utils.FitStringLength(boleto.NumeroControle ?? boleto.NumeroDocumento, 25, 25, ' ', 0, true, true, false); //038-062: Identificação do título na empresa.
                detalhe += Utils.FitStringLength(boleto.NossoNumero, 8, 8, '0', 0, true, true, true); //063-070: Nosso número.
                detalhe += "0000000000000"; //071-083: Quantidade de moeda variável, zeros quando for Real.
                detalhe += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true); //084-086: Carteira.
                detalhe += Utils.FitStringLength(" ", 21, 21, ' ', 0, true, true, true); //087-107: Identificação da operação.
                detalhe += "I"; //108-108: Código da carteira (I: Real, E:Dólar);
                detalhe += ObterCodigoDaOcorrencia(boleto); //109-110: Identificação da remessa.

                detalhe += Utils.FitStringLength(boleto.NumeroDocumento, 10, 10, ' ', 0, true, true, false); //111-120: Número do documento.
                detalhe += boleto.DataVencimento.ToString("ddMMyy"); //121-126: Data de vencimento.
                detalhe += Utils.FitStringLength(boleto.ValorBoleto.ApenasNumeros(), 13, 13, '0', 0, true, true, true); //127-139: Valor do boleto.
                detalhe += "341"; //140-142: Código do banco.
                detalhe += "00000"; //143-147: Agência onde o título será cobrado (no arquivo de remessa, preencher com zeros).
                detalhe += Utils.FitStringLength(EspecieDocumento.ValidaCodigo(boleto.EspecieDocumento), 2, 2, '0', 0, true, true, true); //148-149: Espécie do título.
                detalhe += Utils.FitStringLength(boleto.Aceite, 1, 1, ' ', 0, true, true, false); //150-150: Aceito ou Não aceito.
                detalhe += boleto.DataDocumento.ToString("ddMMyy"); //151-156: Data de emissão do título.

                //157-160: 2 instruções.
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
                detalhe += boleto.DataDesconto == DateTime.MinValue ? boleto.DataVencimento.ToString("ddMMyy") : boleto.DataDesconto.ToString("ddMMyy"); //174-179: Data limite para concessão do desconto.
                detalhe += Utils.FitStringLength(boleto.ValorDesconto.ApenasNumeros(), 13, 13, '0', 0, true, true, true); //180-192: Valor do desconto.
                detalhe += "0000000000000"; //193-205: Valor do IOF.
                detalhe += "0000000000000"; //206-218: Valor do Abatimento.

                detalhe += boleto.Sacado.CpfCnpj.Length <= 11 ? "01" : "02"; //219-220: Tipo de documento do sacado (CNPJ ou CPF).
                detalhe += Utils.FitStringLength(boleto.Sacado.CpfCnpj, 14, 14, '0', 0, true, true, true).ToUpper(); //221-234: CNPJ ou CPF do sacado.
                detalhe += Utils.FitStringLength(boleto.Sacado.Nome.TrimStart(' '), 30, 30, ' ', 0, true, true, false); //235-264: Nome do sacado.
                detalhe += new string(' ', 10); //265-274: Brancos.
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.EndComNumComplemento.TrimStart(' '), 40, 40, ' ', 0, true, true, false).ToUpper(); //275-314: Rua, número e complemento do sacado.
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Bairro.TrimStart(' '), 12, 12, ' ', 0, true, true, false).ToUpper(); //315-326: Bairro do sacado.
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cep, 8, 8, ' ', 0, true, true, false).ToUpper(); //327-334: CEP do sacado.
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Cidade, 15, 15, ' ', 0, true, true, false).ToUpper(); //335-349: Cidade do sacado.
                detalhe += Utils.FitStringLength(boleto.Sacado.Endereco.Uf, 2, 2, ' ', 0, true, true, false).ToUpper(); //350-351: UF do sacado.

                detalhe += Utils.FitStringLength(boleto.Sacado.Nome, 30, 30, ' ', 0, true, true, false).ToUpper(); //352-381: Nome do sacador ou avalista.
                detalhe += "    "; //382-385: Brancos.
                detalhe += boleto.DataVencimento.ToString("ddMMyy"); //386-391: Data de mora.

                //392-393: Prazo para as instruções. TODO
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
                detalhe += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); //395-400: Número sequencial do registro.

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
            detalhe.Append(Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true)); //395-400: Número sequencial do registro.

            return Utils.SubstituiCaracteresEspeciais(detalhe.ToString());
        }

        #endregion

        #region Trailer Cnab240

        /// <summary>
        ///POS INI/FINAL	DESCRIÇÃO	                   A/N	TAM	DEC	CONTEÚDO	NOTAS
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
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // código do banco na compensação - 001-003 9(03) - 341
                header += "0001";                                                                       // Lote de Serviço - 004-007 9(04) - Nota 1
                header += "5";                                                                          // Tipo de Registro - 008-008 9(01) - 5
                header += Utils.FormatCode("", " ", 9);                                                 // Complemento de Registro - 009-017 X(09) - Brancos
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 6, true);                    // Quantidade de Registros do Lote - 018-023 9(06) - Nota 26

                // Totalização da Cobrança Simples
                header += Utils.FormatCode("", "0", 6);                                                 // Quantidade de Títulos em Cobrança Simples - 024-029 9(06) - Nota 24
                header += Utils.FormatCode("", "0", 17);                                                // Valor Total dos Títulos em Carteiras - 030-046 9(15)V9(02) - Nota 24

                //Totalização cobrança vinculada
                header += Utils.FormatCode("", "0", 6);                                                 // Qtde de titulos em cobrança vinculada - 047-052 9(06) - Nota 24
                header += Utils.FormatCode("", "0", 17);                                                // Valor total títulos em cobrança vinculada - 053-069 9(15)V9(02) - Nota 24

                header += Utils.FormatCode("", "0", 46);                                                // Complemento de Registro - 070-115 X(08) - Zeros
                header += Utils.FormatCode("", " ", 8);                                                 // Referência do Aviso bancário - 116-123 X(08) - Nota 25
                header += Utils.FormatCode("", " ", 117);                                               // Complemento de Registro - 124-240 X(117) - Brancos

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar Trailer de Lote do arquivo de remessa.", e);
            }

            #region Informações geradas de forma inconsistente
            //suelton@gmail.com - 04/01/2017 - Gerando informações inconsistentes
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
            //    throw new Exception("Erro durante a geração do registro TRAILER do LOTE de REMESSA.", e);
            //} 
            #endregion
        }

        /// <summary>
        ///POS INI/FINAL	DESCRIÇÃO	                   A/N	TAM	DEC	CONTEÚDO	NOTAS
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
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);      // código do banco na compensação - 001-003 (03) - 341
                header += "9999";                                                    // Lote de Serviço - 004-007 9(04) - '9999'
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
                trailer += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); //Número sequencial do registro no arquivo.

                return Utils.SubstituiCaracteresEspeciais(trailer);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
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
                        throw new Exception("Mensagem Variavel não existe para o tipo CNAB 240.");
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
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }

        private string GerarMensagemVariavelRemessaCnab400(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var registroOpcional = "2"; //Identificação do Registro (1, N).

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

                registroOpcional += new string(' ', 6);     //Data limite para concessão de Desconto 2 (6, N) DDMMAA.
                registroOpcional += new string(' ', 13);    //Valor do Desconto (13, N).
                registroOpcional += new string(' ', 6);     //Data limite para concessão de Desconto 3 (6, N) DDMMAA.
                registroOpcional += new string(' ', 13);    //Valor do Desconto (13, N).
                registroOpcional += new string(' ', 7);     //Reserva (7, A).
                registroOpcional += Utils.FitStringLength(boleto.Carteira, 3, 3, '0', 0, true, true, true); //Carteira (3, N)
                registroOpcional += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Agencia, 5, 5, '0', 0, true, true, true);     //Agência (5, N) 
                registroOpcional += Utils.FitStringLength(boleto.Cedente.ContaBancaria.Conta, 7, 7, '0', 0, true, true, true);       //Conta Corrente (7, N)
                registroOpcional += Utils.FitStringLength(boleto.Cedente.ContaBancaria.DigitoConta, 1, 1, '0', 0, true, true, true); //Dígito C/C (1, A)
                registroOpcional += Utils.FitStringLength(boleto.NossoNumero, 11, 11, '0', 0, true, true, true);      //Nosso Número (11, N)
                registroOpcional += Utils.FitStringLength("0", 1, 1, '0', 0, true, true, true);                       //DAC Nosso Número (1, A)
                registroOpcional += Utils.FitStringLength(numeroRegistro.ToString(), 6, 6, '0', 0, true, true, true); //Nº Seqüencial do Registro (06, N)

                return Utils.SubstituiCaracteresEspeciais(registroOpcional);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar REGISTRO OPCIONAL do arquivo CNAB400.", ex);
            }
        }

        #endregion

        #region Métodos de processamento do arquivo retorno CNAB400

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

                //Descrição da ocorrência
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

                // 377 - Registros rejeitados ou alegação do sacado
                // 386 - 7 brancos

                detalhe.CodigoLiquidacao = registro.Substring(392, 2);
                detalhe.NumeroSequencial = Utils.ToInt32(registro.Substring(394, 6));
                detalhe.ValorPago = detalhe.ValorPrincipal;

                // A correspondência de Valor Pago no RETORNO ITAÚ é o Valor Principal (Valor lançado em Conta Corrente - Conforme Manual)
                // A determinação se Débito ou Crédito deverá ser feita nos aplicativos por se tratar de personalização.
                // Para isso, considerar o Código da Ocorrência e tratar de acordo com suas necessidades.
                // Alterado por jsoda em 04/06/2012
                //
                //// Valor principal é débito ou crédito ?
                //if ( (detalhe.ValorTitulo < detalhe.TarifaCobranca) ||
                //     ((detalhe.ValorTitulo - detalhe.Descontos) < detalhe.TarifaCobranca)
                //    )
                //{
                //    detalhe.ValorPrincipal *= -1; // Para débito coloca valor negativo
                //}

                //// Valor Pago é a soma do Valor Principal (Valor que entra na conta) + Tarifa de Cobrança
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

        public override long ObterNossoNumeroSemConvenioOuDigitoVerificador(long convenio, string nossoNumero)
        {
            //Itaú não utiliza DV ou convênio com o nosso número.
            if (long.TryParse(nossoNumero, out var numero))
                return numero;

            throw new NossoNumeroInvalidoException();
        }
    }
}