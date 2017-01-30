namespace BoletoNet.EDI.Banco
{
    /// <summary>
	/// Classe de Integração Sicredi
	/// </summary>
    public class TRegistroEDI_Sicredi_Retorno : TRegistroEDI
    {
        #region Atributos e Propriedades

        public string IdentificacaoRegDetalhe { get; set; }

        public string Filler1 { get; set; }

        public string TipoCobranca { get; set; }

        public string CodigoPagadorAgenciaBeneficiario { get; set; }

        public string CodigoPagadorJuntoAssociado { get; set; }

        public string BoletoDda { get; set; }

        public string Filler2 { get; set; }

        public string NossoNumeroSicredi { get; set; }

        public string Filler3 { get; set; }

        public string Ocorrencia { get; set; }

        public string DataOcorrencia { get; set; }

        public string SeuNumero { get; set; }

        public string Filler4 { get; set; }

        public string DataVencimento { get; set; }

        public string ValorTitulo { get; set; }

        public string Filler5 { get; set; }

        public string EspecieDocumento { get; set; }

        public string DespesasCobranca { get; set; }

        public string DespesasCustasProtesto { get; set; }

        public string Filler6 { get; set; }

        public string AbatimentoConcedido { get; set; }

        public string DescontoConcedido { get; set; }

        public string ValorEfetivamentePago { get; set; }

        public string JurosMora { get; set; }

        public string Multa { get; set; }

        public string Filler7 { get; set; }

        public string SomenteOcorrencia19 { get; set; }

        public string Filler8 { get; set; }

        public string MotivoOcorrencia { get; set; }

        public string DataPrevistaLancamentoContaCorrente { get; set; }

        public string Filler9 { get; set; }

        public string NumeroSequencialRegistro { get; set; }

        #endregion

        public TRegistroEDI_Sicredi_Retorno()
        {
            NumeroSequencialRegistro = string.Empty;
            Filler9 = string.Empty;
            DataPrevistaLancamentoContaCorrente = string.Empty;
            MotivoOcorrencia = string.Empty;
            Filler8 = string.Empty;
            SomenteOcorrencia19 = string.Empty;
            Filler7 = string.Empty;
            Multa = string.Empty;
            JurosMora = string.Empty;
            ValorEfetivamentePago = string.Empty;
            DescontoConcedido = string.Empty;
            AbatimentoConcedido = string.Empty;
            Filler6 = string.Empty;
            DespesasCustasProtesto = string.Empty;
            DespesasCobranca = string.Empty;
            EspecieDocumento = string.Empty;
            Filler5 = string.Empty;
            ValorTitulo = string.Empty;
            DataVencimento = string.Empty;
            Filler4 = string.Empty;
            SeuNumero = string.Empty;
            DataOcorrencia = string.Empty;
            Ocorrencia = string.Empty;
            Filler3 = string.Empty;
            NossoNumeroSicredi = string.Empty;
            Filler2 = string.Empty;
            BoletoDda = string.Empty;
            CodigoPagadorJuntoAssociado = string.Empty;
            CodigoPagadorAgenciaBeneficiario = string.Empty;
            TipoCobranca = string.Empty;
            Filler1 = string.Empty;
            IdentificacaoRegDetalhe = string.Empty;

            /*
             * Aqui é que iremos informar as características de cada campo do arquivo
             * Na classe base, TCampoRegistroEDI, temos a propriedade CamposEDI, que é uma coleção de objetos
             * TCampoRegistroEDI.
             */
            #region TODOS os Campos

            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0001, 001, 0, string.Empty, ' ')); //001-001
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0002, 012, 0, string.Empty, ' ')); //002-013
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0014, 001, 0, string.Empty, ' ')); //014-014
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0015, 005, 0, string.Empty, ' ')); //015-019
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0020, 005, 0, string.Empty, ' ')); //020-025
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0025, 001, 0, string.Empty, ' ')); //025-025
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0026, 022, 0, string.Empty, ' ')); //026-047
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0048, 015, 0, string.Empty, ' ')); //048-062
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0063, 046, 0, string.Empty, ' ')); //063-108
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' ')); //109-110
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0111, 006, 0, string.Empty, ' ')); //111-116
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0117, 010, 0, string.Empty, ' ')); //117-126
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0127, 020, 0, string.Empty, ' ')); //127-146
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0147, 006, 0, string.Empty, ' ')); //147-152
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0153, 013, 0, string.Empty, ' ')); //153-165
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0166, 009, 0, string.Empty, ' ')); //166-174
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0175, 001, 0, string.Empty, ' ')); //175-175
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0176, 013, 0, string.Empty, ' ')); //176-188
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0189, 013, 0, string.Empty, ' ')); //189-201
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0202, 026, 0, string.Empty, ' ')); //202-227
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0228, 013, 0, string.Empty, ' ')); //228-240
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0241, 013, 0, string.Empty, ' ')); //241-253
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0254, 013, 0, string.Empty, ' ')); //254-266
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0267, 013, 0, string.Empty, ' ')); //267-279
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0280, 013, 0, string.Empty, ' ')); //280-292
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0293, 002, 0, string.Empty, ' ')); //293-294
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0295, 001, 0, string.Empty, ' ')); //295-295
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0296, 023, 0, string.Empty, ' ')); //296-318
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0319, 010, 0, string.Empty, ' ')); //318-328
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0329, 008, 0, string.Empty, ' ')); //329-336
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0337, 058, 0, string.Empty, ' ')); //337-394
            _CamposEDI.Add(new TCampoRegistroEDI(TTiposDadoEDI.ediAlphaAliEsquerda_____, 0395, 006, 0, string.Empty, ' ')); //395-400

            #endregion
        }

        /// <summary>
        /// Aqui iremos atribuir os valores das propriedades em cada campo correspondente do Registro EDI
        /// e codificaremos a linha para obter uma string formatada com o nosso layout.
        /// Repare que declarei as propriedades em uma ordem tal que a adição dos objetos TCampoRegistroEDI na propriedade
        /// _CamposEDI siga a mesma ordem. Portanto, utilizarei o índice na atribuição.
        /// </summary>
        public override void CodificarLinha()
        {
            #region Todos os Campos
            //PARA LEITURA DO RETORNO BANCÁRIO NÃO PRECISAMOS IMPLEMENTAR ESSE MÉTODO           
            #endregion

            base.CodificarLinha(); //Aqui que eu chamo efetivamente a rotina de codificação; o resultado será exibido na propriedade LinhaRegistro.
        }

        /// <summary>
        /// Agora, faço o inverso da codificação. Decodifico o valor da propriedade LinhaRegistro e separo em cada campo.
        /// Cada campo é separado na propriedade ValorNatural de cada item da prop. _CamposEDI. Como esta é do tipo object, para atribuir
        /// nas propriedades do registro é necessário fazer um cast para o tipo de dado adequado. Caso ocorra algum erro na decodificação,
        /// uma exceção será disparada, provavelmente por causa de impossibilidade de fazer um cast na classe pai. Portanto, o layout deve estar
        /// correto!
        /// </summary>
        public override void DecodificarLinha()
        {
            base.DecodificarLinha();

            IdentificacaoRegDetalhe = (string)_CamposEDI[00].ValorNatural;
            Filler1 = (string)_CamposEDI[01].ValorNatural;
            TipoCobranca = (string)_CamposEDI[02].ValorNatural;
            CodigoPagadorAgenciaBeneficiario = (string)_CamposEDI[03].ValorNatural;
            CodigoPagadorJuntoAssociado = (string)_CamposEDI[04].ValorNatural;
            BoletoDda = (string)_CamposEDI[05].ValorNatural;
            Filler2 = (string)_CamposEDI[06].ValorNatural;
            NossoNumeroSicredi = (string)_CamposEDI[07].ValorNatural;
            Filler3 = (string)_CamposEDI[08].ValorNatural;
            Ocorrencia = (string)_CamposEDI[09].ValorNatural;
            DataOcorrencia = (string)_CamposEDI[10].ValorNatural;
            SeuNumero = (string)_CamposEDI[11].ValorNatural;
            Filler4 = (string)_CamposEDI[12].ValorNatural;
            DataVencimento = (string)_CamposEDI[13].ValorNatural;
            ValorTitulo = (string)_CamposEDI[14].ValorNatural;
            Filler5 = (string)_CamposEDI[15].ValorNatural;
            EspecieDocumento = (string)_CamposEDI[16].ValorNatural;
            DespesasCobranca = (string)_CamposEDI[17].ValorNatural;
            DespesasCustasProtesto = (string)_CamposEDI[18].ValorNatural;
            Filler6 = (string)_CamposEDI[19].ValorNatural;
            AbatimentoConcedido = (string)_CamposEDI[20].ValorNatural;
            DescontoConcedido = (string)_CamposEDI[21].ValorNatural;
            ValorEfetivamentePago = (string)_CamposEDI[22].ValorNatural;
            JurosMora = (string)_CamposEDI[23].ValorNatural;
            Multa = (string)_CamposEDI[24].ValorNatural;
            Filler7 = (string)_CamposEDI[25].ValorNatural;
            SomenteOcorrencia19 = (string)_CamposEDI[26].ValorNatural;
            Filler8 = (string)_CamposEDI[27].ValorNatural;
            MotivoOcorrencia = (string)_CamposEDI[28].ValorNatural;
            DataPrevistaLancamentoContaCorrente = (string)_CamposEDI[29].ValorNatural;
            Filler9 = (string)_CamposEDI[30].ValorNatural;
            NumeroSequencialRegistro = (string)_CamposEDI[31].ValorNatural;
        }
    }

    /// <summary>
    /// Classe que irá representar o arquivo EDI em si
    /// </summary>
    public class TArquivoSicrediRetorno_EDI : TEDIFile
    {
        /*
		 * De modo geral, apenas preciso sobreescrever o método de decodificação de linhas,
		 * pois preciso adicionar um objeto do tipo registro na coleção do arquivo, passar a linha que vem do arquivo
		 * neste objeto novo, e decodificá-lo para separar nos campos.
		 * O DecodeLine é chamado a partir do método LoadFromFile() (ou Stream) da classe base.
		 */
        protected override void DecodeLine(string Line)
        {
            base.DecodeLine(Line);
            Lines.Add(new TRegistroEDI_Sicredi_Retorno()); //Adiciono a linha a ser decodificada
            Lines[Lines.Count - 1].LinhaRegistro = Line; //Atribuo a linha que vem do arquivo
            Lines[Lines.Count - 1].DecodificarLinha(); //Finalmente, a separação das substrings na linha do arquivo.
        }
    }
}