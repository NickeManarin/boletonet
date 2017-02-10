using System;

namespace BoletoNet.EDI.Banco
{
    /// <summary>
	/// Classe de Integração Banrisul
    /// CBR643
    /// Convênio 6 posições -- Único com Identificação do Registro Detalhe = 1
	/// </summary>
    public class TRegistroEDI_BancoBrasil_Retorno : RegistroEdi
    {
        #region Atributos e Propriedades

        private string _Identificacao = String.Empty;

        public string Identificacao
        {
            get { return _Identificacao; }
            set { _Identificacao = value; }
        }
        private string _Zeros1 = String.Empty;

        public string Zeros1
        {
            get { return _Zeros1; }
            set { _Zeros1 = value; }
        }
        private string _Zeros2 = String.Empty;

        public string Zeros2
        {
            get { return _Zeros2; }
            set { _Zeros2 = value; }
        }
        private string _PrefixoAgencia = String.Empty;

        public string PrefixoAgencia
        {
            get { return _PrefixoAgencia; }
            set { _PrefixoAgencia = value; }
        }
        private string _DVPrefixoAgencia = String.Empty;

        public string DVPrefixoAgencia
        {
            get { return _DVPrefixoAgencia; }
            set { _DVPrefixoAgencia = value; }
        }
        private string _ContaCorrente = String.Empty;

        public string ContaCorrente
        {
            get { return _ContaCorrente; }
            set { _ContaCorrente = value; }
        }
        private string _DVContaCorrente = String.Empty;

        public string DVContaCorrente
        {
            get { return _DVContaCorrente; }
            set { _DVContaCorrente = value; }
        }
        private string _NumeroConvenioCobranca = String.Empty;

        public string NumeroConvenioCobranca
        {
            get { return _NumeroConvenioCobranca; }
            set { _NumeroConvenioCobranca = value; }
        }
        private string _NumeroControleParticipante = String.Empty;

        public string NumeroControleParticipante
        {
            get { return _NumeroControleParticipante; }
            set { _NumeroControleParticipante = value; }
        }
        private string _NossoNumero = String.Empty;

        public string NossoNumero
        {
            get { return _NossoNumero; }
            set { _NossoNumero = value; }
        }
        private string _TipoCobranca = String.Empty;

        public string TipoCobranca
        {
            get { return _TipoCobranca; }
            set { _TipoCobranca = value; }
        }
        private string _TipoCobrancaEspecifico = String.Empty;

        public string TipoCobrancaEspecifico
        {
            get { return _TipoCobrancaEspecifico; }
            set { _TipoCobrancaEspecifico = value; }
        }
        private string _DiasCalculo = String.Empty;

        public string DiasCalculo
        {
            get { return _DiasCalculo; }
            set { _DiasCalculo = value; }
        }
        private string _NaturezaRecebimento = String.Empty;

        public string NaturezaRecebimento
        {
            get { return _NaturezaRecebimento; }
            set { _NaturezaRecebimento = value; }
        }
        private string _PrefixoTitulo = String.Empty;

        public string PrefixoTitulo
        {
            get { return _PrefixoTitulo; }
            set { _PrefixoTitulo = value; }
        }
        private string _VariacaoCarteira = String.Empty;

        public string VariacaoCarteira
        {
            get { return _VariacaoCarteira; }
            set { _VariacaoCarteira = value; }
        }
        private string _ContaCaucao = String.Empty;

        public string ContaCaucao
        {
            get { return _ContaCaucao; }
            set { _ContaCaucao = value; }
        }
        private string _TaxaDesconto = String.Empty;

        public string TaxaDesconto
        {
            get { return _TaxaDesconto; }
            set { _TaxaDesconto = value; }
        }
        private string _TaxaIOF = String.Empty;

        public string TaxaIOF
        {
            get { return _TaxaIOF; }
            set { _TaxaIOF = value; }
        }
        private string _Brancos1 = String.Empty;

        public string Brancos1
        {
            get { return _Brancos1; }
            set { _Brancos1 = value; }
        }
        private string _Carteira = String.Empty;

        public string Carteira
        {
            get { return _Carteira; }
            set { _Carteira = value; }
        }
        private string _Comando = String.Empty;

        public string Comando
        {
            get { return _Comando; }
            set { _Comando = value; }
        }
        private string _DataLiquidacao = String.Empty;

        public string DataLiquidacao
        {
            get { return _DataLiquidacao; }
            set { _DataLiquidacao = value; }
        }
        private string _NumeroTituloCedente = String.Empty;

        public string NumeroTituloCedente
        {
            get { return _NumeroTituloCedente; }
            set { _NumeroTituloCedente = value; }
        }
        private string _Brancos2 = String.Empty;

        public string Brancos2
        {
            get { return _Brancos2; }
            set { _Brancos2 = value; }
        }
        private string _DataVencimento = String.Empty;

        public string DataVencimento
        {
            get { return _DataVencimento; }
            set { _DataVencimento = value; }
        }
        private string _ValorTitulo = String.Empty;

        public string ValorTitulo
        {
            get { return _ValorTitulo; }
            set { _ValorTitulo = value; }
        }
        private string _CodigoBancoRecebedor = String.Empty;

        public string CodigoBancoRecebedor
        {
            get { return _CodigoBancoRecebedor; }
            set { _CodigoBancoRecebedor = value; }
        }
        private string _PrefixoAgenciaRecebedora = String.Empty;

        public string PrefixoAgenciaRecebedora
        {
            get { return _PrefixoAgenciaRecebedora; }
            set { _PrefixoAgenciaRecebedora = value; }
        }
        private string _DVPrefixoRecebedora = String.Empty;

        public string DVPrefixoRecebedora
        {
            get { return _DVPrefixoRecebedora; }
            set { _DVPrefixoRecebedora = value; }
        }
        private string _EspecieTitulo = String.Empty;

        public string EspecieTitulo
        {
            get { return _EspecieTitulo; }
            set { _EspecieTitulo = value; }
        }
        private string _DataCredito = String.Empty;

        public string DataCredito
        {
            get { return _DataCredito; }
            set { _DataCredito = value; }
        }
        private string _ValorTarifa = String.Empty;

        public string ValorTarifa
        {
            get { return _ValorTarifa; }
            set { _ValorTarifa = value; }
        }
        private string _OutrasDespesas = String.Empty;

        public string OutrasDespesas
        {
            get { return _OutrasDespesas; }
            set { _OutrasDespesas = value; }
        }
        private string _JurosDesconto = String.Empty;

        public string JurosDesconto
        {
            get { return _JurosDesconto; }
            set { _JurosDesconto = value; }
        }
        private string _IOFDesconto = String.Empty;

        public string IOFDesconto
        {
            get { return _IOFDesconto; }
            set { _IOFDesconto = value; }
        }
        private string _ValorAbatimento = String.Empty;

        public string ValorAbatimento
        {
            get { return _ValorAbatimento; }
            set { _ValorAbatimento = value; }
        }
        private string _DescontoConcedido = String.Empty;

        public string DescontoConcedido
        {
            get { return _DescontoConcedido; }
            set { _DescontoConcedido = value; }
        }
        private string _ValorRecebido = String.Empty;

        public string ValorRecebido
        {
            get { return _ValorRecebido; }
            set { _ValorRecebido = value; }
        }
        private string _JurosMora = String.Empty;

        public string JurosMora
        {
            get { return _JurosMora; }
            set { _JurosMora = value; }
        }
        private string _OutrosRecebimentos = String.Empty;

        public string OutrosRecebimentos
        {
            get { return _OutrosRecebimentos; }
            set { _OutrosRecebimentos = value; }
        }
        private string _AbatimentoNaoAproveitado = String.Empty;

        public string AbatimentoNaoAproveitado
        {
            get { return _AbatimentoNaoAproveitado; }
            set { _AbatimentoNaoAproveitado = value; }
        }
        private string _ValorLancamento = String.Empty;

        public string ValorLancamento
        {
            get { return _ValorLancamento; }
            set { _ValorLancamento = value; }
        }
        private string _IndicativoDebitoCredito = String.Empty;

        public string IndicativoDebitoCredito
        {
            get { return _IndicativoDebitoCredito; }
            set { _IndicativoDebitoCredito = value; }
        }
        private string _IndicadorValor = String.Empty;

        public string IndicadorValor
        {
            get { return _IndicadorValor; }
            set { _IndicadorValor = value; }
        }
        private string _ValorAjuste = String.Empty;

        public string ValorAjuste
        {
            get { return _ValorAjuste; }
            set { _ValorAjuste = value; }
        }
        private string _Brancos3 = String.Empty;

        public string Brancos3
        {
            get { return _Brancos3; }
            set { _Brancos3 = value; }
        }
        private string _Brancos4 = String.Empty;

        public string Brancos4
        {
            get { return _Brancos4; }
            set { _Brancos4 = value; }
        }
        private string _Zeros3 = String.Empty;

        public string Zeros3
        {
            get { return _Zeros3; }
            set { _Zeros3 = value; }
        }
        private string _Zeros4 = String.Empty;

        public string Zeros4
        {
            get { return _Zeros4; }
            set { _Zeros4 = value; }
        }
        private string _Zeros5 = String.Empty;

        public string Zeros5
        {
            get { return _Zeros5; }
            set { _Zeros5 = value; }
        }
        private string _Zeros6 = String.Empty;

        public string Zeros6
        {
            get { return _Zeros6; }
            set { _Zeros6 = value; }
        }
        private string _Zeros7 = String.Empty;

        public string Zeros7
        {
            get { return _Zeros7; }
            set { _Zeros7 = value; }
        }
        private string _Zeros8 = String.Empty;

        public string Zeros8
        {
            get { return _Zeros8; }
            set { _Zeros8 = value; }
        }
        private string _Brancos5 = String.Empty;

        public string Brancos5
        {
            get { return _Brancos5; }
            set { _Brancos5 = value; }
        }
        private string _CanalPagamento = String.Empty;

        public string CanalPagamento
        {
            get { return _CanalPagamento; }
            set { _CanalPagamento = value; }
        }
        private string _NumeroSequenciaRegistro = String.Empty;

        public string NumeroSequenciaRegistro
        {
            get { return _NumeroSequenciaRegistro; }
            set { _NumeroSequenciaRegistro = value; }
        }
        #endregion
        
        public TRegistroEDI_BancoBrasil_Retorno()
        {
            /*
             * Aqui é que iremos informar as características de cada campo do arquivo
             * Na classe base, CampoRegistroEdi, temos a propriedade CamposEdi, que é uma coleção de objetos
             * CampoRegistroEdi.
             */
            #region TODOS os Campos
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0001, 001, 0, string.Empty, ' ')); //001-001
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0002, 002, 0, string.Empty, ' ')); //002-003
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0004, 014, 0, string.Empty, ' ')); //004-017
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0018, 004, 0, string.Empty, ' ')); //018-021
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0022, 001, 0, string.Empty, ' ')); //022-022
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0023, 008, 0, string.Empty, ' ')); //023-030
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0031, 001, 0, string.Empty, ' ')); //031-031
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0032, 007, 0, string.Empty, ' ')); //032-038
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0039, 025, 0, string.Empty, ' ')); //039-063
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0064, 017, 0, string.Empty, ' ')); //064-080
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0081, 001, 0, string.Empty, ' ')); //081-081
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0082, 001, 0, string.Empty, ' ')); //082-082
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0083, 004, 0, string.Empty, ' ')); //083-086
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0087, 002, 0, string.Empty, ' ')); //087-088
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0089, 003, 0, string.Empty, ' ')); //089-091
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0092, 003, 0, string.Empty, ' ')); //092-094
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0095, 001, 0, string.Empty, ' ')); //095-095
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0096, 005, 0, string.Empty, ' ')); //096-100
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0101, 004, 0, string.Empty, ' ')); //101-105
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0106, 001, 0, string.Empty, ' ')); //106-106
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0107, 002, 0, string.Empty, ' ')); //107-108
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' ')); //109-110
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0111, 006, 0, string.Empty, ' ')); //111-116
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0117, 010, 0, string.Empty, ' ')); //117-126
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0127, 020, 0, string.Empty, ' ')); //127-146
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0147, 006, 0, string.Empty, ' ')); //147-152
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0153, 013, 0, string.Empty, ' ')); //153-165
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0166, 003, 0, string.Empty, ' ')); //166-168
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0169, 004, 0, string.Empty, ' ')); //169-172
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0173, 001, 0, string.Empty, ' ')); //173-173
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0174, 002, 0, string.Empty, ' ')); //174-175
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0176, 006, 0, string.Empty, ' ')); //176-181
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0182, 007, 0, string.Empty, ' ')); //182-188
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0189, 013, 0, string.Empty, ' ')); //189-201
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0202, 013, 0, string.Empty, ' ')); //202-214
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0215, 013, 0, string.Empty, ' ')); //215-227
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0228, 013, 0, string.Empty, ' ')); //228-240
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0241, 013, 0, string.Empty, ' ')); //241-253
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0254, 013, 0, string.Empty, ' ')); //254-266
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0267, 013, 0, string.Empty, ' ')); //267-279
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0280, 013, 0, string.Empty, ' ')); //280-292
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0293, 013, 0, string.Empty, ' ')); //293-305
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0306, 013, 0, string.Empty, ' ')); //306-318
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0319, 001, 0, string.Empty, ' ')); //319-319
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0320, 001, 0, string.Empty, ' ')); //320-320
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0321, 012, 0, string.Empty, ' ')); //321-332
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0333, 001, 0, string.Empty, ' ')); //333-333
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0334, 009, 0, string.Empty, ' ')); //334-342
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0343, 007, 0, string.Empty, ' ')); //343-349
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0350, 009, 0, string.Empty, ' ')); //350-358
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0359, 007, 0, string.Empty, ' ')); //359-365
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0366, 009, 0, string.Empty, ' ')); //366-374
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0375, 007, 0, string.Empty, ' ')); //375-381
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0382, 009, 0, string.Empty, ' ')); //382-390
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0391, 002, 0, string.Empty, ' ')); //391-392
            CamposEdi.Add(new CampoRegistroEdi(TiposDadoEdi.ediAlphaAliEsquerda_____, 0393, 002, 0, string.Empty, ' ')); //393-394
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
            
            _Identificacao = (string)CamposEdi[0].ValorNatural;
            _Zeros1 = (string)CamposEdi[1].ValorNatural;
            _Zeros2 = (string)CamposEdi[2].ValorNatural;
            _PrefixoAgencia = (string)CamposEdi[3].ValorNatural;
            _DVPrefixoAgencia = (string)CamposEdi[4].ValorNatural;
            _ContaCorrente = (string)CamposEdi[5].ValorNatural;
            _DVContaCorrente = (string)CamposEdi[6].ValorNatural;
            _NumeroConvenioCobranca = (string)CamposEdi[7].ValorNatural;
            _NumeroControleParticipante = (string)CamposEdi[8].ValorNatural;
            _NossoNumero = (string)CamposEdi[9].ValorNatural;
            _TipoCobranca = (string)CamposEdi[10].ValorNatural;
            _TipoCobrancaEspecifico = (string)CamposEdi[11].ValorNatural;
            _DiasCalculo = (string)CamposEdi[12].ValorNatural;
            _NaturezaRecebimento = (string)CamposEdi[13].ValorNatural;
            _PrefixoTitulo = (string)CamposEdi[14].ValorNatural;
            _VariacaoCarteira = (string)CamposEdi[15].ValorNatural;
            _ContaCaucao = (string)CamposEdi[16].ValorNatural;
            _TaxaDesconto = (string)CamposEdi[17].ValorNatural;
            _TaxaIOF = (string)CamposEdi[18].ValorNatural;
            _Brancos1 = (string)CamposEdi[19].ValorNatural;
            _Carteira = (string)CamposEdi[20].ValorNatural;
            _Comando = (string)CamposEdi[21].ValorNatural;
            _DataLiquidacao = (string)CamposEdi[22].ValorNatural;
            _NumeroTituloCedente = (string)CamposEdi[23].ValorNatural;
            _Brancos2 = (string)CamposEdi[24].ValorNatural;
            _DataVencimento = (string)CamposEdi[25].ValorNatural;
            _ValorTitulo = (string)CamposEdi[26].ValorNatural;
            _CodigoBancoRecebedor = (string)CamposEdi[27].ValorNatural;
            _PrefixoAgenciaRecebedora = (string)CamposEdi[28].ValorNatural;
            _DVPrefixoRecebedora = (string)CamposEdi[29].ValorNatural;
            _EspecieTitulo = (string)CamposEdi[30].ValorNatural;
            _DataCredito = (string)CamposEdi[31].ValorNatural;
            _ValorTarifa = (string)CamposEdi[32].ValorNatural;
            _OutrasDespesas = (string)CamposEdi[33].ValorNatural;
            _JurosDesconto = (string)CamposEdi[34].ValorNatural;
            _IOFDesconto = (string)CamposEdi[35].ValorNatural;
            _ValorAbatimento = (string)CamposEdi[36].ValorNatural;
            _DescontoConcedido = (string)CamposEdi[37].ValorNatural;
            _ValorRecebido = (string)CamposEdi[38].ValorNatural;
            _JurosMora = (string)CamposEdi[39].ValorNatural;
            _OutrosRecebimentos = (string)CamposEdi[40].ValorNatural;
            _AbatimentoNaoAproveitado = (string)CamposEdi[41].ValorNatural;
            _ValorLancamento = (string)CamposEdi[42].ValorNatural;
            _IndicativoDebitoCredito = (string)CamposEdi[43].ValorNatural;
            _IndicadorValor = (string)CamposEdi[44].ValorNatural;
            _ValorAjuste = (string)CamposEdi[45].ValorNatural;
            _Brancos3 = (string)CamposEdi[46].ValorNatural;
            _Brancos4 = (string)CamposEdi[47].ValorNatural;
            _Zeros3 = (string)CamposEdi[48].ValorNatural;
            _Zeros4 = (string)CamposEdi[49].ValorNatural;
            _Zeros5 = (string)CamposEdi[50].ValorNatural;
            _Zeros6 = (string)CamposEdi[51].ValorNatural;
            _Zeros7 = (string)CamposEdi[52].ValorNatural;
            _Zeros8 = (string)CamposEdi[53].ValorNatural;
            _Brancos5 = (string)CamposEdi[54].ValorNatural;
            _CanalPagamento = (string)CamposEdi[55].ValorNatural;
            _NumeroSequenciaRegistro = (string)CamposEdi[56].ValorNatural;
            //
		}
	}

	/// <summary>
	/// Classe que irá representar o arquivo EDI em si
	/// </summary>
    public class TArquivoBancoBrasilRetorno_EDI : EdiFile
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
            Lines.Add(new TRegistroEDI_BancoBrasil_Retorno()); //Adiciono a linha a ser decodificada
			Lines[Lines.Count - 1].LinhaRegistro = Line; //Atribuo a linha que vem do arquivo
            Lines[Lines.Count - 1].DecodificarLinha(); //Finalmente, a separação das substrings na linha do arquivo.
		}
	}
	

}
