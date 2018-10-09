namespace BoletoNet.EDI.Banco
{
    /// <summary>
	/// Classe de Integração Banrisul
	/// </summary>
    public class RegistroEdi_Banrisul_Retorno : RegistroEdi
    {
        #region Atributos e Propriedades

        public string Constante1 { get; set; }

        public string TipoInscricao { get; set; }

        public string CpfCnpj { get; set; }

        public string CodigoCedente { get; set; }

        public string EspecieCobrancaRegistrada { get; set; }

        public string Branco1 { get; set; }

        public string IdentificacaoTituloCedente { get; set; }

        public string IdentificacaoTituloBancoNossoNumero { get; set; }

        public string IdentificacaoTituloBancoNossoNumeroOpcional { get; set; }

        public string NumeroContratoBlu { get; set; }

        public string Brancos2 { get; set; }

        public string TipoCarteira { get; set; }

        public string CodigoOcorrencia { get; set; }

        public string DataOcorrenciaBanco { get; set; }

        public string SeuNumero { get; set; }

        public string NossoNumero { get; set; }

        public string DataVencimentoTitulo { get; set; }

        public string ValorTitulo { get; set; }

        public string CodigoBancoCobrador { get; set; }

        public string CodigoAgenciaCobradora { get; set; }

        public string TipoDocumento { get; set; }

        public string ValorDespesasCobranca { get; set; }

        public string OutrasDespesas { get; set; }

        public string Zeros1 { get; set; }

        public string ValorAbatimentoDeflacaoConcedido { get; set; }

        public string ValorDescontoConcedido { get; set; }

        public string ValorPago { get; set; }

        public string ValorJuros { get; set; }

        public string ValorOutrosRecebimentos { get; set; }

        public string Brancos3 { get; set; }

        public string DataCreditoConta { get; set; }

        public string Brancos4 { get; set; }

        public string PagamentoDinheiroCheque { get; set; }

        public string Brancos5 { get; set; }

        public string MotivoOcorrencia { get; set; }

        public string Brancos6 { get; set; }

        public string NumeroSequenciaRegistro { get; set; }

        #endregion

        public RegistroEdi_Banrisul_Retorno()
        {
            Constante1 = string.Empty;
            TipoInscricao = string.Empty;
            CpfCnpj = string.Empty;
            CodigoCedente = string.Empty;
            EspecieCobrancaRegistrada = string.Empty;
            Branco1 = string.Empty;
            IdentificacaoTituloCedente = string.Empty;
            IdentificacaoTituloBancoNossoNumero = string.Empty;
            IdentificacaoTituloBancoNossoNumeroOpcional = string.Empty;
            NumeroContratoBlu = string.Empty;
            Brancos2 = string.Empty;
            TipoCarteira = string.Empty;
            CodigoOcorrencia = string.Empty;
            DataOcorrenciaBanco = string.Empty;
            SeuNumero = string.Empty;
            NossoNumero = string.Empty;
            DataVencimentoTitulo = string.Empty;
            ValorTitulo = string.Empty;
            CodigoBancoCobrador = string.Empty;
            CodigoAgenciaCobradora = string.Empty;
            TipoDocumento = string.Empty;
            ValorDespesasCobranca = string.Empty;
            OutrasDespesas = string.Empty;
            ValorOutrosRecebimentos = string.Empty;
            Brancos3 = string.Empty;
            DataCreditoConta = string.Empty;
            Brancos4 = string.Empty;
            PagamentoDinheiroCheque = string.Empty;
            Brancos5 = string.Empty;
            MotivoOcorrencia = string.Empty;
            Brancos6 = string.Empty;
            NumeroSequenciaRegistro = string.Empty;
            Zeros1 = string.Empty;
            ValorAbatimentoDeflacaoConcedido = string.Empty;
            ValorDescontoConcedido = string.Empty;
            ValorJuros = string.Empty;
            ValorPago = string.Empty;

            #region TODOS os Campos

            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0001, 001, 0, string.Empty, ' ')); //001-001
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0002, 002, 0, string.Empty, ' ')); //002-003
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0004, 014, 0, string.Empty, ' ')); //004-017
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0018, 013, 0, string.Empty, ' ')); //018-030 É 12 caracteres ou 13? Parece um erro do banco.
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0031, 006, 0, string.Empty, ' ')); //031-036
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0037, 001, 0, string.Empty, ' ')); //037-037
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0038, 025, 0, string.Empty, ' ')); //038-062
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0063, 010, 0, string.Empty, ' ')); //063-072
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0073, 010, 0, string.Empty, ' ')); //073-082
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0083, 022, 0, string.Empty, ' ')); //083-104
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0105, 003, 0, string.Empty, ' ')); //105-107
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0108, 001, 0, string.Empty, ' ')); //108-108
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' ')); //109-110
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0111, 006, 0, string.Empty, ' ')); //111-116
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0117, 010, 0, string.Empty, ' ')); //117-126
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0127, 020, 0, string.Empty, ' ')); //127-146
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0147, 006, 0, string.Empty, ' ')); //147-152
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0153, 013, 2, string.Empty, ' ')); //153-165
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0166, 003, 0, string.Empty, ' ')); //166-168
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0169, 005, 0, string.Empty, ' ')); //169-173
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0174, 002, 0, string.Empty, ' ')); //174-175
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0176, 013, 2, string.Empty, ' ')); //176-188
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0189, 013, 2, string.Empty, ' ')); //189-201
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0202, 026, 0, string.Empty, ' ')); //202-227
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0228, 013, 2, string.Empty, ' ')); //228-240
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0241, 013, 2, string.Empty, ' ')); //241-253
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0254, 013, 2, string.Empty, ' ')); //254-266
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0267, 013, 2, string.Empty, ' ')); //267-279
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0280, 013, 2, string.Empty, ' ')); //280-292
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0293, 003, 0, string.Empty, ' ')); //293-295
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0296, 006, 0, string.Empty, ' ')); //296-301
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0302, 041, 0, string.Empty, ' ')); //302-342
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0343, 001, 0, string.Empty, ' ')); //343-343
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0344, 039, 0, string.Empty, ' ')); //344-382
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0383, 010, 0, string.Empty, ' ')); //383-392
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0393, 002, 0, string.Empty, ' ')); //393-394
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0395, 006, 0, string.Empty, ' ')); //395-400

            #endregion
        }

        /// <summary>
        /// Aqui iremos atribuir os valores das propriedades em cada campo correspondente do Registro EDI
        /// e codificaremos a linha para obter uma string formatada com o nosso layout.
        /// Repare que declarei as propriedades em uma ordem tal que a adição dos objetos CampoEdi na propriedade
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
            
            Constante1 = (string)CamposEdi[0].ValorNatural;
            TipoInscricao = (string)CamposEdi[1].ValorNatural;
            CpfCnpj = (string)CamposEdi[2].ValorNatural;
            CodigoCedente = (string)CamposEdi[3].ValorNatural;
            EspecieCobrancaRegistrada = (string)CamposEdi[4].ValorNatural;
            Branco1 = (string)CamposEdi[5].ValorNatural;
            IdentificacaoTituloCedente = (string)CamposEdi[6].ValorNatural;
            IdentificacaoTituloBancoNossoNumero = (string)CamposEdi[7].ValorNatural;
            IdentificacaoTituloBancoNossoNumeroOpcional = (string)CamposEdi[8].ValorNatural;
            NumeroContratoBlu = (string)CamposEdi[9].ValorNatural;
            Brancos2 = (string)CamposEdi[10].ValorNatural;
            TipoCarteira = (string)CamposEdi[11].ValorNatural;
            CodigoOcorrencia = (string)CamposEdi[12].ValorNatural;
            DataOcorrenciaBanco = (string)CamposEdi[13].ValorNatural;
            SeuNumero = (string)CamposEdi[14].ValorNatural;
            NossoNumero = (string)CamposEdi[15].ValorNatural;
            DataVencimentoTitulo = (string)CamposEdi[16].ValorNatural;
            ValorTitulo = (string)CamposEdi[17].ValorNatural;
            CodigoBancoCobrador = (string)CamposEdi[18].ValorNatural;
            CodigoAgenciaCobradora = (string)CamposEdi[19].ValorNatural;
            TipoDocumento = (string)CamposEdi[20].ValorNatural;
            ValorDespesasCobranca = (string)CamposEdi[21].ValorNatural;
            OutrasDespesas = (string)CamposEdi[22].ValorNatural;
            Zeros1 = (string)CamposEdi[23].ValorNatural;
            //this._ValorAvista = (string)this._CamposEDI[0].ValorNatural;
            //this._SituacaoIOF = (string)this._CamposEDI[0].ValorNatural;
            //this._Zeros2 = (string)this._CamposEDI[0].ValorNatural;
            ValorAbatimentoDeflacaoConcedido = (string)CamposEdi[24].ValorNatural;
            ValorDescontoConcedido = (string)CamposEdi[25].ValorNatural;
            ValorPago = (string)CamposEdi[26].ValorNatural;
            ValorJuros = (string)CamposEdi[27].ValorNatural;
            ValorOutrosRecebimentos = (string)CamposEdi[28].ValorNatural;
            Brancos3 = (string)CamposEdi[29].ValorNatural;
            DataCreditoConta = (string)CamposEdi[30].ValorNatural;
            Brancos4 = (string)CamposEdi[31].ValorNatural;
            PagamentoDinheiroCheque = (string)CamposEdi[32].ValorNatural;
            Brancos5 = (string)CamposEdi[33].ValorNatural;
            MotivoOcorrencia = (string)CamposEdi[34].ValorNatural;
            Brancos6 = (string)CamposEdi[35].ValorNatural;
            NumeroSequenciaRegistro = (string)CamposEdi[36].ValorNatural;
        }
    }

    /// <summary>
    /// Classe que irá representar o arquivo EDI em si
    /// </summary>
    public class ArquivoBanrisulRetornoEdi : EdiFile
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

            Lines.Add(new RegistroEdi_Banrisul_Retorno()); //Adiciono a linha a ser decodificada
            Lines[Lines.Count - 1].LinhaRegistro = Line; //Atribuo a linha que vem do arquivo
            Lines[Lines.Count - 1].DecodificarLinha(); //Finalmente, a separação das substrings na linha do arquivo.
        }
    }
}
