using System;
using System.Collections.Generic;
using System.Globalization;

namespace BoletoNet
{
    //Classes básicas para manipulação de registros para geração/interpretação de EDI

    /// <summary>
    /// Classe para ordenação pela propriedade Posição no Registro EDI
    /// </summary>
    internal class OrdenacaoPorPosEdi : IComparer<CampoEdi>
    {
        public int Compare(CampoEdi x, CampoEdi y)
        {
            return x.OrdemNoRegistroEdi.CompareTo(y.OrdemNoRegistroEdi);
        }
    }

    /// <summary>
    /// Representa cada tipo de dado possível em um arquivo EDI.
    /// </summary>
    public enum Dado
    {
        /// <summary>
        /// Representa um campo alfanumérico, alinhado à esquerda e com brancos à direita. A propriedade ValorNatural é do tipo String
        /// </summary>
        AlphaAliEsquerda_____,
        /// <summary>
        /// Representa um campo alfanumérico, alinhado à direita e com brancos à esquerda. A propriedade ValorNatural é do tipo String
        /// </summary>
        AlphaAliDireita______,
        /// <summary>
        /// Representa um campo numérico inteiro alinhado à direita com zeros à esquerda. A propriedade ValorNatural é do tipo Int ou derivados
        /// </summary>
        Inteiro______________,
        /// <summary>
        /// Representa um campo numérico com decimais, sem o separador de decimal. A propriedade ValorNatural é do tipo Double
        /// </summary>
        NumericoSemSeparador_,
        /// <summary>
        /// Representa um campo numérico com decimais, com o caracter ponto (.) como separador decimal,
        /// alinhado à direita com zeros à esquerda. A propriedade ValorNatural é do tipo Double
        /// </summary>
        NumericoComPonto_____,
        /// <summary>
        /// Representa um campo numérico com decimais, com o caracter vírgula (,) como separador decimal,
        /// alinhado à direita com zeros à esquerda. A propriedade ValorNatural é do tipo Double
        /// </summary>
        NumericoComVirgula___,
        /// <summary>
        /// Representa um campo de data no formato ddm/mm/aaaa. A propriedade ValorNatural é do tipo DateTime
        /// </summary>
        DataDDMMAAAA_________,
        /// <summary>
        /// Representa um campo de data no formato aaaa/mm/dd. A propriedade ValorNatural é do tipo DateTime
        /// </summary>
        DataAAAAMMDD_________,
        /// <summary>
        /// Representa um campo de data no formato dd/mm. A propriedade ValorNatural é do tipo DateTime, com o ano igual a 1900
        /// </summary>
        DataDDMM_____________,
        /// <summary>
        /// Representa um campo de data no formato mm/aaaa. A propriedade ValorNatural é do tipo DateTime, com o dia igual a 01
        /// </summary>
        DataMMAAAA___________,
        /// <summary>
        /// Representa um campo de data no formato mm/dd. A propriedade ValorNatural é do tipo DateTime com o ano igual a 1900
        /// </summary>
        DataMMDD_____________,
        /// <summary>
        /// Representa um campo de hora no formato HH:MM. A propriedade ValorNatural é do tipo DateTime, com a data igual a 01/01/1900
        /// </summary>
        HoraHHMM_____________,
        /// <summary>
        /// Representa um campo de hora no formato HH:MM:SS. A propriedade ValorNatural é do tipo DateTime, com a data igual a 01/01/1900
        /// </summary>
        HoraHHMMSS___________,
        /// <summary>
        /// Representa um campo de data no formato DD/MM/AAAA. A propriedade ValorNatural é do tipo DateTime.
        /// </summary>
        DataDDMMAA___________,
        /// <summary>
        /// Representa um campo de data no formato DD/MM/AAAA, porém colocando zeros no lugar de espaços no ValorFormatado. A propriedade
        /// ValorNatural é do tipo DateTime, e este deve ser nulo caso queira que a data seja zero.
        /// </summary>
        DataDDMMAAAAWithZeros,
        /// <summary>
        /// Representa um campo de data no formato AAAA/MM/DD, porém colocando zeros no lugar de espaços no ValorFormatado. A propriedade
        /// ValorNatural é do tipo DateTime, e este deve ser nulo caso queira que a data seja zero.
        /// </summary>
        DataAAAAMMDDWithZeros,
    }

    public class CampoEdi
    {
        #region Propriedades

        /// <summary>
        /// Descrição do campo no registro EDI (meramente descritivo)
        /// </summary>
        public string DescricaoCampo { get; set; }

        /// <summary>
        /// Tipo de dado de ORIGEM das informações do campo EDI.
        /// </summary>
        public Dado Campo { get; set; }

        /// <summary>
        /// Tamanho em caracteres do campo no arquivo EDI (DESTINO)
        /// </summary>
        public int TamanhoCampo { get; set; }

        /// <summary>
        /// Quantidade de casas decimais do campo, caso ele seja do tipo numérico sem decimais. Caso
        /// não se aplique ao tipo de dado, o valor da propriedade será ignorado nas funções de formatação.
        /// </summary>
        public int QtdDecimais { get; set; }

        /// <summary>
        /// Valor de ORIGEM do campo, sem formatação, no tipo de dado adequado ao campo. O valor deve ser atribuido
        /// com o tipo de dado adequado ao seu proposto, por exemplo, Double para representar valor, DateTime para
        /// representar datas e/ou horas, etc.
        /// </summary>
        public object ValorNatural { get; set; }

        /// <summary>
        /// Valor formatado do campo, pronto para ser utilizado no arquivo EDI. A formatação será de acordo
        /// com a especificada na propriedade Campo, com numéricos alinhados à direita e zeros à esquerda
        /// e campos alfanuméricos alinhados à esquerda e com brancos à direita.
        /// Também pode receber o valor vindo do arquivo EDI, para ser decodificado e o resultado da decodificação na propriedade
        /// ValorNatural
        /// </summary>
        public string ValorFormatado { get; set; }

        /// <summary>
        /// Número de ordem do campo no registro EDI
        /// </summary>
        public int OrdemNoRegistroEdi { get; set; }

        /// <summary>
        /// Caractere separador dos elementos de campos com o tipo DATA. Colocar null caso esta propriedade
        /// não se aplique ao tipo de dado.
        /// </summary>
        public string SeparadorDatas { get; set; }

        /// <summary>
        /// Caractere separador dos elementos de campos com o tipo HORA. Colocar null caso esta propriedade
        /// não se aplique ao tipo de dado.
        /// </summary>
        public string SeparadorHora { get; set; }

        /// <summary>
        /// Posição do caracter inicial do campo no arquivo EDI
        /// </summary>
        public int PosicaoInicial { get; set; }

        /// <summary>
        /// Posição do caracter final do campo no arquivo EDI
        /// </summary>
        public int PosicaoFinal { get; set; }

        /// <summary>
        /// Caractere de Preenchimento do campo da posição inicial até a posição final
        /// </summary>
        public char Preenchimento { get; set; }

        #endregion

        #region Construtores

        /// <summary>
        /// Cria um objeto CampoEdi
        /// </summary>
        public CampoEdi()
        {
            Preenchimento = ' ';
        }

        /// <summary>
        /// Cria um objeto do tipo CampoEdi inicializando as propriedades básicas.
        /// </summary>
        /// <param name="pCampo">Tipo de dado de origem dos dados</param>
        /// <param name="pPosicaoInicial">Posição Inicial do Campo no Arquivo</param>
        /// <param name="pTamanho">Tamanho em caracteres do campo (destino)</param>
        /// <param name="pDecimais">Quantidade de decimais do campo (destino)</param>
        /// <param name="pValor">Valor do campo (Origem), no tipo de dado adequado ao propósito do campo</param>
        /// <param name="pPreenchimento">Caractere de Preenchimento do campo caso o valor não ocupe todo o tamanho</param>
        /// <param name="pSeparadorHora">Separador de hora padrão; null para sem separador</param>
        /// <param name="pSeparadorData">Separador de data padrão; null para sem separador</param>
        public CampoEdi(Dado pCampo, int pPosicaoInicial, int pTamanho, int pDecimais, object pValor, char pPreenchimento, string pSeparadorHora, string pSeparadorData)
        {
            Campo = pCampo;
            TamanhoCampo = pTamanho;
            QtdDecimais = pDecimais;
            ValorNatural = pValor;
            SeparadorHora = pSeparadorHora;
            SeparadorDatas = pSeparadorData;
            OrdemNoRegistroEdi = 0;
            DescricaoCampo = "";
            PosicaoInicial = pPosicaoInicial - 1; //Compensa a indexação com base em zero
            PosicaoFinal = pPosicaoInicial + TamanhoCampo;
            Preenchimento = pPreenchimento;
        }

        /// <summary>
        /// Cria um objeto do tipo CampoEdi inicializando as propriedades básicas.
        /// </summary>
        /// <param name="pCampo">Tipo de dado de origem dos dados</param>
        /// <param name="pPosicaoInicial">Posição Inicial do Campo no Arquivo</param>
        /// <param name="pTamanho">Tamanho em caracteres do campo (destino)</param>
        /// <param name="pDecimais">Quantidade de decimais do campo (destino)</param>
        /// <param name="pValor">Valor do campo (Origem), no tipo de dado adequado ao propósito do campo</param>
        /// <param name="pPreenchimento">Caractere de Preenchimento do campo caso o valor não ocupe todo o tamanho</param>
        public CampoEdi(Dado pCampo, int pPosicaoInicial, int pTamanho, int pDecimais, object pValor, char pPreenchimento)
        {
            Campo = pCampo;
            TamanhoCampo = pTamanho;
            QtdDecimais = pDecimais;
            ValorNatural = pValor;
            SeparadorHora = null;
            SeparadorDatas = null;
            OrdemNoRegistroEdi = 0;
            DescricaoCampo = "";
            PosicaoInicial = pPosicaoInicial - 1; //Compensa a indexação com base em zero
            PosicaoFinal = pPosicaoInicial + TamanhoCampo;
            Preenchimento = pPreenchimento;
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Aplica formatação ao valor do campo em ValorNatural, colocando o resultado na propriedade ValorFormatado
        /// </summary>
        public void CodificarNaturalParaEdi()
        {
            switch (Campo)
            {
                case Dado.AlphaAliEsquerda_____:
                    {
                        if (ValorNatural != null)
                        {
                            if (ValorNatural.ToString().Trim().Length >= TamanhoCampo)
                                ValorFormatado = ValorNatural.ToString().Trim().Substring(0, TamanhoCampo);
                            else
                                ValorFormatado = ValorNatural.ToString().Trim().PadRight(TamanhoCampo, Preenchimento); //' '
                        }
                        else
                            ValorFormatado = string.Empty.PadRight(TamanhoCampo, Preenchimento); //' '
                        break;
                    }
                case Dado.AlphaAliDireita______:
                    {
                        if (ValorNatural != null)
                        {
                            if (ValorNatural.ToString().Trim().Length >= TamanhoCampo)
                                ValorFormatado = ValorNatural.ToString().Trim().Substring(0, TamanhoCampo);
                            else
                                ValorFormatado = ValorNatural.ToString().Trim().PadLeft(TamanhoCampo, Preenchimento); //' '
                        }
                        else
                            ValorFormatado = string.Empty.PadLeft(TamanhoCampo, Preenchimento); //' '
                        break;
                    }
                case Dado.Inteiro______________:
                    {
                        ValorFormatado = ValorNatural.ToString().Trim().PadLeft(TamanhoCampo, Preenchimento); //'0'
                        break;
                    }
                case Dado.NumericoSemSeparador_:
                    {
                        if (ValorNatural == null)
                        {
                            ValorFormatado = "".Trim().PadLeft(TamanhoCampo, ' '); //Se o Número for NULL, preenche com espaços em branco
                        }
                        else
                        {
                            ValorFormatado = string.Format(new CultureInfo("pt-BR"), "{0:f" + QtdDecimais + "}", ValorNatural).Replace(",", "").Replace(".", "").Trim().PadLeft(TamanhoCampo, Preenchimento);

                            if (ValorFormatado.Length > TamanhoCampo)
                                throw new Exception("Tamanho do campo (" + ValorFormatado + ") superior ao definido: " + ValorFormatado.Length + " para o máximo de: " + TamanhoCampo);
                        }
                        break;
                    }
                case Dado.NumericoComPonto_____:
                    {
                        var formatacao = "{0:f" + QtdDecimais + "}";
                        ValorFormatado = string.Format(formatacao, ValorNatural).Replace(",", ".").Trim().PadLeft(TamanhoCampo, Preenchimento); //'0'
                        break;
                    }
                case Dado.NumericoComVirgula___:
                    {
                        var formatacao = "{0:f" + QtdDecimais + "}";
                        ValorFormatado = string.Format(formatacao, ValorNatural).Replace(".", ",").Trim().PadLeft(TamanhoCampo, Preenchimento); //'0'
                        break;
                    }
                case Dado.DataAAAAMMDD_________:
                    {
                        if ((DateTime)ValorNatural != DateTime.MinValue)
                        {
                            var sep = SeparadorDatas ?? "";
                            var formatacao = "{0:yyyy" + sep + "MM" + sep + "dd}";
                            ValorFormatado = string.Format(formatacao, ValorNatural);
                        }
                        else
                        {
                            ValorNatural = "";
                            goto case Dado.AlphaAliEsquerda_____;
                        }
                        break;
                    }
                case Dado.DataDDMM_____________:
                    {
                        if ((DateTime)ValorNatural != DateTime.MinValue)
                        {
                            var sep = SeparadorDatas ?? "";
                            var formatacao = "{0:dd" + sep + "MM}";
                            ValorFormatado = string.Format(formatacao, ValorNatural);
                        }
                        else
                        {
                            ValorNatural = "";
                            goto case Dado.AlphaAliEsquerda_____;
                        }
                        break;
                    }
                case Dado.DataDDMMAAAA_________:
                    {
                        if ((DateTime)ValorNatural != DateTime.MinValue)
                        {
                            var sep = SeparadorDatas ?? "";
                            var formatacao = "{0:dd" + sep + "MM" + sep + "yyyy}";
                            ValorFormatado = string.Format(formatacao, ValorNatural);
                        }
                        else
                        {
                            ValorNatural = "";
                            goto case Dado.AlphaAliEsquerda_____;
                        }
                        break;
                    }
                case Dado.DataDDMMAA___________:
                    {
                        if ((DateTime)ValorNatural != DateTime.MinValue)
                        {
                            var sep = SeparadorDatas ?? "";
                            var formatacao = "{0:dd" + sep + "MM" + sep + "yy}";
                            ValorFormatado = string.Format(formatacao, ValorNatural);
                        }
                        else
                        {
                            ValorNatural = "";
                            goto case Dado.AlphaAliEsquerda_____;
                        }
                        break;
                    }
                case Dado.DataMMAAAA___________:
                    {
                        if ((DateTime)ValorNatural != DateTime.MinValue)
                        {
                            var sep = SeparadorDatas ?? "";
                            var formatacao = "{0:MM" + sep + "yyyy}";
                            ValorFormatado = string.Format(formatacao, ValorNatural);
                        }
                        else
                        {
                            ValorNatural = "";
                            goto case Dado.AlphaAliEsquerda_____;
                        }
                        break;
                    }
                case Dado.DataMMDD_____________:
                    {
                        if ((DateTime)ValorNatural != DateTime.MinValue)
                        {
                            var sep = SeparadorDatas ?? "";
                            var formatacao = "{0:MM" + sep + "dd}";
                            ValorFormatado = string.Format(formatacao, ValorNatural);
                        }
                        else
                        {
                            ValorNatural = "";
                            goto case Dado.AlphaAliEsquerda_____;
                        }
                        break;
                    }
                case Dado.HoraHHMM_____________:
                    {
                        var sep = SeparadorHora ?? "";
                        var formatacao = "{0:HH" + sep + "mm}";
                        ValorFormatado = string.Format(formatacao, ValorNatural);
                        break;
                    }
                case Dado.HoraHHMMSS___________:
                    {
                        var sep = SeparadorHora ?? "";
                        var formatacao = "{0:HH" + sep + "mm" + sep + "ss}";
                        ValorFormatado = string.Format(formatacao, ValorNatural);
                        break;
                    }
                case Dado.DataDDMMAAAAWithZeros:
                    {
                        var sep = SeparadorDatas ?? "";
                        if (ValorNatural != null || !ValorNatural.ToString().Trim().Equals(""))
                        {
                            var formatacao = "{0:dd" + sep + "MM" + sep + "yyyy}";
                            ValorFormatado = string.Format(formatacao, ValorNatural);
                        }
                        else
                        {
                            ValorFormatado = "00" + sep + "00" + sep + "0000";
                        }
                        break;
                    }
                case Dado.DataAAAAMMDDWithZeros:
                    {
                        var sep = SeparadorDatas ?? "";

                        if (ValorNatural != null)
                        {
                            var formatacao = "{0:yyyy" + sep + "MM" + sep + "dd}";
                            ValorFormatado = string.Format(formatacao, ValorNatural);
                        }
                        else
                        {
                            ValorFormatado = "00" + sep + "00" + sep + "0000";
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// Transforma o valor vindo do campo do registro EDI da propriedade ValorFormatado para o valor natural (com o tipo
        /// de dado adequado) na propriedade ValorNatural
        /// </summary>
        public void DecodificarEdiParaNatural()
        {
            if (ValorFormatado.Trim().Equals(""))
            {
                ValorNatural = null;
            }
            else
            {
                switch (Campo)
                {
                    case Dado.AlphaAliEsquerda_____:
                        {
                            ValorNatural = ValorFormatado.Trim();
                            break;
                        }
                    case Dado.AlphaAliDireita______:
                        {
                            ValorNatural = ValorFormatado.Trim();
                            break;
                        }
                    case Dado.Inteiro______________:
                        {
                            ValorNatural = long.Parse(ValorFormatado.Trim());
                            break;
                        }
                    case Dado.NumericoSemSeparador_:
                        {
                            var s = ValorFormatado.Substring(0, ValorFormatado.Length - QtdDecimais) + "," + ValorFormatado.Substring(ValorFormatado.Length - QtdDecimais, QtdDecimais);
                            ValorNatural = double.Parse(s.Trim());
                            break;
                        }
                    case Dado.NumericoComPonto_____:
                        {
                            ValorNatural = double.Parse(ValorFormatado.Replace(".", ",").Trim());
                            break;
                        }
                    case Dado.NumericoComVirgula___:
                        {
                            ValorNatural = double.Parse(ValorFormatado.Trim().Replace(".", ","));
                            break;
                        }
                    case Dado.DataAAAAMMDD_________:
                        {
                            if (!ValorFormatado.Trim().Equals(""))
                            {
                                string cAno;
                                string cMes;
                                string cDia;

                                if (SeparadorDatas != null)
                                {
                                    var split = ValorFormatado.Split(SeparadorDatas.ToCharArray());
                                    cAno = split[0];
                                    cMes = split[1];
                                    cDia = split[2];
                                }
                                else
                                {
                                    cAno = ValorFormatado.Substring(0, 4);
                                    cMes = ValorFormatado.Substring(4, 2);
                                    cDia = ValorFormatado.Substring(6, 2);
                                }
                                if (cDia.Equals("00") && cMes.Equals("00") && cAno.Equals("0000"))
                                {
                                    ValorNatural = null;
                                }
                                else
                                {
                                    ValorNatural = DateTime.Parse(cDia + "/" + cMes + "/" + cAno);
                                }
                            }
                            else
                            {
                                ValorNatural = null;
                            }
                            break;
                        }
                    case Dado.DataDDMM_____________:
                        {
                            if (!ValorFormatado.Trim().Equals(""))
                            {
                                var cAno = "1900";
                                var cMes = "";
                                var cDia = "";
                                if (SeparadorDatas != null)
                                {
                                    var split = ValorFormatado.Split(SeparadorDatas.ToCharArray());
                                    cMes = split[1];
                                    cDia = split[0];
                                }
                                else
                                {
                                    cMes = ValorFormatado.Substring(2, 2);
                                    cDia = ValorFormatado.Substring(0, 2);
                                }
                                ValorNatural = DateTime.Parse(cDia + "/" + cMes + "/" + cAno);
                            }
                            else
                            {
                                ValorNatural = null;
                            }
                            break;
                        }
                    case Dado.DataDDMMAAAA_________:
                        {
                            var cDia = "";
                            var cMes = "";
                            var cAno = "";
                            if (SeparadorDatas != null)
                            {
                                var split = ValorFormatado.Split(SeparadorDatas.ToCharArray());
                                cAno = split[2];
                                cMes = split[1];
                                cDia = split[0];
                            }
                            else
                            {
                                cDia = ValorFormatado.Substring(0, 2);
                                cMes = ValorFormatado.Substring(2, 2);
                                cAno = ValorFormatado.Substring(4, 4);
                            }
                            if (cDia.Equals("00") && cMes.Equals("00") && cAno.Equals("0000") || ValorFormatado.Trim().Equals(""))
                            {
                                ValorNatural = DateTime.Parse("01/01/1900"); //data start
                            }
                            else
                            {
                                ValorNatural = DateTime.Parse(cDia + "/" + cMes + "/" + cAno);
                            }
                            break;
                        }
                    case Dado.DataDDMMAA___________:
                        {
                            var cDia = "";
                            var cMes = "";
                            var cAno = "";
                            if (SeparadorDatas != null)
                            {
                                var split = ValorFormatado.Split(SeparadorDatas.ToCharArray());
                                cAno = split[2];
                                cMes = split[1];
                                cDia = split[0];
                            }
                            else
                            {
                                cDia = ValorFormatado.Substring(0, 2);
                                cMes = ValorFormatado.Substring(2, 2);
                                cAno = ValorFormatado.Substring(4, 2);
                            }
                            ValorNatural = DateTime.Parse(cDia + "/" + cMes + "/" + cAno);
                            break;
                        }
                    case Dado.DataMMAAAA___________:
                        {
                            var cDia = "01";
                            var cMes = "";
                            var cAno = "";
                            if (SeparadorDatas != null)
                            {
                                var split = ValorFormatado.Split(SeparadorDatas.ToCharArray());
                                cAno = split[1];
                                cMes = split[0];
                            }
                            else
                            {
                                cMes = ValorFormatado.Substring(0, 2);
                                cAno = ValorFormatado.Substring(2, 4);
                            }
                            ValorNatural = DateTime.Parse(cDia + "/" + cMes + "/" + cAno);
                            break;
                        }
                    case Dado.DataMMDD_____________:
                        {
                            var cDia = "";
                            var cMes = "";
                            var cAno = "1900";
                            if (SeparadorDatas != null)
                            {
                                var split = ValorFormatado.Split(SeparadorDatas.ToCharArray());
                                cMes = split[0];
                                cDia = split[1];
                            }
                            else
                            {
                                cDia = ValorFormatado.Substring(2, 2);
                                cMes = ValorFormatado.Substring(0, 2);
                            }
                            ValorNatural = DateTime.Parse(cDia + "/" + cMes + "/" + cAno);
                            break;
                        }
                    case Dado.HoraHHMM_____________:
                        {
                            var cHora = "";
                            var cMinuto = "";
                            if (SeparadorHora != null)
                            {
                                var split = ValorFormatado.Split(SeparadorHora.ToCharArray());
                                cHora = split[0];
                                cMinuto = split[1];
                            }
                            else
                            {
                                cHora = ValorFormatado.Substring(0, 2);
                                cMinuto = ValorFormatado.Substring(2, 2);
                            }
                            ValorNatural = DateTime.Parse(cHora + ":" + cMinuto + ":00");
                            break;
                        }
                    case Dado.HoraHHMMSS___________:
                        {
                            var cHora = "";
                            var cMinuto = "";
                            var cSegundo = "";
                            if (SeparadorHora != null)
                            {
                                var split = ValorFormatado.Split(SeparadorHora.ToCharArray());
                                cHora = split[0];
                                cMinuto = split[1];
                                cSegundo = split[2];
                            }
                            else
                            {
                                cHora = ValorFormatado.Substring(0, 2);
                                cMinuto = ValorFormatado.Substring(2, 2);
                                cSegundo = ValorFormatado.Substring(4, 2);
                            }
                            ValorNatural = DateTime.Parse(cHora + ":" + cMinuto + ":00");
                            break;
                        }
                    case Dado.DataDDMMAAAAWithZeros:
                        {
                            goto case Dado.DataDDMMAAAA_________;
                        }
                    case Dado.DataAAAAMMDDWithZeros:
                        {
                            goto case Dado.DataAAAAMMDD_________;
                        }
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Indica os tipos de registro possíveis em um arquivo EDI
    /// </summary>
    public enum TipoRegistroEdi
    {
        /// <summary>
        /// Indicador de registro Header
        /// </summary>
        Header,
        /// <summary>
        /// Indica um registro detalhe
        /// </summary>
        Detalhe,
        /// <summary>
        /// Indica um registro Trailler
        /// </summary>
        Trailler,
        /// <summary>
        /// Indica um registro sem definições, utilizado para transmissão socket ou similar
        /// </summary>
        LinhaUnica
    }

    /// <summary>
    /// Classe representativa de um registro (linha) de um arquivo EDI
    /// </summary>
    public class RegistroEdi
    {
        #region Variáveis Privadas e Protegidas

        protected int TamanhoMaximo = 0;
        protected char CaracterPreenchimento = ' ';

        public RegistroEdi()
        {
            CamposEdi = new List<CampoEdi>();
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Tipo de Registro da linha do arquivo EDI
        /// </summary>
        public TipoRegistroEdi TipoRegistro { get; protected set; }

        /// <summary>
        /// Seta a linha do registro para a decodificação nos campos;
        /// Obtém a linha decodificada a partir dos campos.
        /// </summary>
        public string LinhaRegistro { get; set; }

        /// <summary>
        /// Coleção dos campos do registro EDI
        /// </summary>
        public List<CampoEdi> CamposEdi { get; set; }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Codifica uma linha a partir dos campos; o resultado irá na propriedade LinhaRegistro
        /// </summary>
        public virtual void CodificarLinha()
        {
            LinhaRegistro = "";
            foreach (var campos in CamposEdi)
            {
                campos.CodificarNaturalParaEdi();
                LinhaRegistro += campos.ValorFormatado;
            }
        }

        /// <summary>
        /// Decodifica uma linha a partir da propriedade LinhaRegistro nos campos do registro
        /// </summary>
        public virtual void DecodificarLinha()
        {
            foreach (var campos in CamposEdi)
            {
                if (TamanhoMaximo > 0)
                    LinhaRegistro = LinhaRegistro.PadRight(TamanhoMaximo, CaracterPreenchimento);

                campos.ValorFormatado = LinhaRegistro.Substring(campos.PosicaoInicial, campos.TamanhoCampo);
                campos.DecodificarEdiParaNatural();
            }
        }

        #endregion
    }
}
