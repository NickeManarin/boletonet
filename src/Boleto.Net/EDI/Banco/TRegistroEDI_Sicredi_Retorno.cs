namespace BoletoNet.EDI.Banco
{
    /// <summary>
	/// Classe de Integração Sicredi
	/// </summary>
    public class TRegistroEDI_Sicredi_Retorno : RegistroEdi
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
             * Na classe base, CampoRegistroEdi, temos a propriedade CamposEdi, que é uma coleção de objetos
             * CampoRegistroEdi.
             */
            #region TODOS os Campos

            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0001, 001, 0, string.Empty, ' ')); //001-001
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0002, 012, 0, string.Empty, ' ')); //002-013
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0014, 001, 0, string.Empty, ' ')); //014-014
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0015, 005, 0, string.Empty, ' ')); //015-019
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0020, 005, 0, string.Empty, ' ')); //020-025
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0025, 001, 0, string.Empty, ' ')); //025-025
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0026, 022, 0, string.Empty, ' ')); //026-047
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0048, 015, 0, string.Empty, ' ')); //048-062
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0063, 046, 0, string.Empty, ' ')); //063-108
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' ')); //109-110
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0111, 006, 0, string.Empty, ' ')); //111-116
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0117, 010, 0, string.Empty, ' ')); //117-126
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0127, 020, 0, string.Empty, ' ')); //127-146
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0147, 006, 0, string.Empty, ' ')); //147-152
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0153, 013, 0, string.Empty, ' ')); //153-165
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0166, 009, 0, string.Empty, ' ')); //166-174
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0175, 001, 0, string.Empty, ' ')); //175-175
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0176, 013, 0, string.Empty, ' ')); //176-188
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0189, 013, 0, string.Empty, ' ')); //189-201
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0202, 026, 0, string.Empty, ' ')); //202-227
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0228, 013, 0, string.Empty, ' ')); //228-240
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0241, 013, 0, string.Empty, ' ')); //241-253
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0254, 013, 0, string.Empty, ' ')); //254-266
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0267, 013, 0, string.Empty, ' ')); //267-279
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0280, 013, 0, string.Empty, ' ')); //280-292
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0293, 002, 0, string.Empty, ' ')); //293-294
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0295, 001, 0, string.Empty, ' ')); //295-295
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0296, 023, 0, string.Empty, ' ')); //296-318
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0319, 010, 0, string.Empty, ' ')); //318-328
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0329, 008, 0, string.Empty, ' ')); //329-336
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0337, 058, 0, string.Empty, ' ')); //337-394
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0395, 006, 0, string.Empty, ' ')); //395-400

            #endregion
        }

        /// <summary>
        /// Aqui iremos atribuir os valores das propriedades em cada campo correspondente do Registro EDI
        /// e codificaremos a linha para obter uma string formatada com o nosso layout.
        /// Repare que declarei as propriedades em uma ordem tal que a adição dos objetos CampoRegistroEdi na propriedade
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

            IdentificacaoRegDetalhe = (string)CamposEdi[00].ValorNatural;
            Filler1 = (string)CamposEdi[01].ValorNatural;
            TipoCobranca = (string)CamposEdi[02].ValorNatural;
            CodigoPagadorAgenciaBeneficiario = (string)CamposEdi[03].ValorNatural;
            CodigoPagadorJuntoAssociado = (string)CamposEdi[04].ValorNatural;
            BoletoDda = (string)CamposEdi[05].ValorNatural;
            Filler2 = (string)CamposEdi[06].ValorNatural;
            NossoNumeroSicredi = (string)CamposEdi[07].ValorNatural;
            Filler3 = (string)CamposEdi[08].ValorNatural;
            Ocorrencia = (string)CamposEdi[09].ValorNatural;
            DataOcorrencia = (string)CamposEdi[10].ValorNatural;
            SeuNumero = (string)CamposEdi[11].ValorNatural;
            Filler4 = (string)CamposEdi[12].ValorNatural;
            DataVencimento = (string)CamposEdi[13].ValorNatural;
            ValorTitulo = (string)CamposEdi[14].ValorNatural;
            Filler5 = (string)CamposEdi[15].ValorNatural;
            EspecieDocumento = (string)CamposEdi[16].ValorNatural;
            DespesasCobranca = (string)CamposEdi[17].ValorNatural;
            DespesasCustasProtesto = (string)CamposEdi[18].ValorNatural;
            Filler6 = (string)CamposEdi[19].ValorNatural;
            AbatimentoConcedido = (string)CamposEdi[20].ValorNatural;
            DescontoConcedido = (string)CamposEdi[21].ValorNatural;
            ValorEfetivamentePago = (string)CamposEdi[22].ValorNatural;
            JurosMora = (string)CamposEdi[23].ValorNatural;
            Multa = (string)CamposEdi[24].ValorNatural;
            Filler7 = (string)CamposEdi[25].ValorNatural;
            SomenteOcorrencia19 = (string)CamposEdi[26].ValorNatural;
            Filler8 = (string)CamposEdi[27].ValorNatural;
            MotivoOcorrencia = (string)CamposEdi[28].ValorNatural;
            DataPrevistaLancamentoContaCorrente = (string)CamposEdi[29].ValorNatural;
            Filler9 = (string)CamposEdi[30].ValorNatural;
            NumeroSequencialRegistro = (string)CamposEdi[31].ValorNatural;
        }
    }

    /// <summary>
    /// Classe que irá representar o arquivo EDI em si
    /// </summary>
    public class TArquivoSicrediRetorno_EDI : EdiFile
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