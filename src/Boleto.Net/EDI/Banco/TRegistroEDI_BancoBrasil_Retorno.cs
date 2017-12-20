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

        public string Identificacao { get; set; } = string.Empty;

        public string Zeros1 { get; set; } = string.Empty;

        public string Zeros2 { get; set; } = string.Empty;

        public string PrefixoAgencia { get; set; } = string.Empty;

        public string DVPrefixoAgencia { get; set; } = string.Empty;

        public string ContaCorrente { get; set; } = string.Empty;

        public string DVContaCorrente { get; set; } = string.Empty;

        public string NumeroConvenioCobranca { get; set; } = string.Empty;

        public string NumeroControleParticipante { get; set; } = string.Empty;

        public string NossoNumero { get; set; } = string.Empty;

        public string TipoCobranca { get; set; } = string.Empty;

        public string TipoCobrancaEspecifico { get; set; } = string.Empty;

        public string DiasCalculo { get; set; } = string.Empty;

        public string NaturezaRecebimento { get; set; } = string.Empty;

        public string PrefixoTitulo { get; set; } = string.Empty;

        public string VariacaoCarteira { get; set; } = string.Empty;

        public string ContaCaucao { get; set; } = string.Empty;

        public string TaxaDesconto { get; set; } = string.Empty;

        public string TaxaIOF { get; set; } = string.Empty;

        public string Brancos1 { get; set; } = string.Empty;

        public string Carteira { get; set; } = string.Empty;

        public string Comando { get; set; } = string.Empty;

        public string DataLiquidacao { get; set; } = string.Empty;

        public string NumeroTituloCedente { get; set; } = string.Empty;

        public string Brancos2 { get; set; } = string.Empty;

        public string DataVencimento { get; set; } = string.Empty;

        public string ValorTitulo { get; set; } = string.Empty;

        public string CodigoBancoRecebedor { get; set; } = string.Empty;

        public string PrefixoAgenciaRecebedora { get; set; } = string.Empty;

        public string DVPrefixoRecebedora { get; set; } = string.Empty;

        public string EspecieTitulo { get; set; } = string.Empty;

        public string DataCredito { get; set; } = string.Empty;

        public string ValorTarifa { get; set; } = string.Empty;

        public string OutrasDespesas { get; set; } = string.Empty;

        public string JurosDesconto { get; set; } = string.Empty;

        public string IOFDesconto { get; set; } = string.Empty;

        public string ValorAbatimento { get; set; } = string.Empty;

        public string DescontoConcedido { get; set; } = string.Empty;

        public string ValorRecebido { get; set; } = string.Empty;

        public string JurosMora { get; set; } = string.Empty;

        public string OutrosRecebimentos { get; set; } = string.Empty;

        public string AbatimentoNaoAproveitado { get; set; } = string.Empty;

        public string ValorLancamento { get; set; } = string.Empty;

        public string IndicativoDebitoCredito { get; set; } = string.Empty;

        public string IndicadorValor { get; set; } = string.Empty;

        public string ValorAjuste { get; set; } = string.Empty;

        public string Brancos3 { get; set; } = string.Empty;

        public string Brancos4 { get; set; } = string.Empty;

        public string Zeros3 { get; set; } = string.Empty;

        public string Zeros4 { get; set; } = string.Empty;

        public string Zeros5 { get; set; } = string.Empty;

        public string Zeros6 { get; set; } = string.Empty;

        public string Zeros7 { get; set; } = string.Empty;

        public string Zeros8 { get; set; } = string.Empty;

        public string Brancos5 { get; set; } = string.Empty;

        public string CanalPagamento { get; set; } = string.Empty;

        public string NumeroSequenciaRegistro { get; set; } = string.Empty;

        #endregion
        
        public TRegistroEDI_BancoBrasil_Retorno()
        {
            /*
             * Aqui é que iremos informar as características de cada campo do arquivo
             * Na classe base, CampoEdi, temos a propriedade CamposEdi, que é uma coleção de objetos
             * CampoEdi.
             */

            #region TODOS os Campos

            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0001, 001, 0, string.Empty, ' ')); //001-001
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0002, 002, 0, string.Empty, ' ')); //002-003
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0004, 014, 0, string.Empty, ' ')); //004-017
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0018, 004, 0, string.Empty, ' ')); //018-021
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0022, 001, 0, string.Empty, ' ')); //022-022
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0023, 008, 0, string.Empty, ' ')); //023-030
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0031, 001, 0, string.Empty, ' ')); //031-031
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0032, 007, 0, string.Empty, ' ')); //032-038 - Convênio
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0039, 025, 0, string.Empty, ' ')); //039-063
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0064, 017, 0, string.Empty, ' ')); //064-080 - Nosso número. 01234560000000001
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0081, 001, 0, string.Empty, ' ')); //081-081
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0082, 001, 0, string.Empty, ' ')); //082-082
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0083, 004, 0, string.Empty, ' ')); //083-086
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0087, 002, 0, string.Empty, ' ')); //087-088 - Naturaza do recebimento
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0089, 003, 0, string.Empty, ' ')); //089-091
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0092, 003, 0, string.Empty, ' ')); //092-094
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0095, 001, 0, string.Empty, ' ')); //095-095
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0096, 005, 0, string.Empty, ' ')); //096-100
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0101, 004, 0, string.Empty, ' ')); //101-105
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0106, 001, 0, string.Empty, ' ')); //106-106
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0107, 002, 0, string.Empty, ' ')); //107-108
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' ')); //109-110
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0111, 006, 0, string.Empty, ' ')); //111-116
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0117, 010, 0, string.Empty, ' ')); //117-126
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0127, 020, 0, string.Empty, ' ')); //127-146
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0147, 006, 0, string.Empty, ' ')); //147-152
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0153, 013, 0, string.Empty, ' ')); //153-165
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0166, 003, 0, string.Empty, ' ')); //166-168
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0169, 004, 0, string.Empty, ' ')); //169-172
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0173, 001, 0, string.Empty, ' ')); //173-173
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0174, 002, 0, string.Empty, ' ')); //174-175
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0176, 006, 0, string.Empty, ' ')); //176-181
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0182, 007, 0, string.Empty, ' ')); //182-188
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0189, 013, 0, string.Empty, ' ')); //189-201
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0202, 013, 0, string.Empty, ' ')); //202-214
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0215, 013, 0, string.Empty, ' ')); //215-227
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0228, 013, 0, string.Empty, ' ')); //228-240
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0241, 013, 0, string.Empty, ' ')); //241-253
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0254, 013, 0, string.Empty, ' ')); //254-266
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0267, 013, 0, string.Empty, ' ')); //267-279
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0280, 013, 0, string.Empty, ' ')); //280-292
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0293, 013, 0, string.Empty, ' ')); //293-305
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0306, 013, 0, string.Empty, ' ')); //306-318
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0319, 001, 0, string.Empty, ' ')); //319-319
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0320, 001, 0, string.Empty, ' ')); //320-320
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0321, 012, 0, string.Empty, ' ')); //321-332
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0333, 001, 0, string.Empty, ' ')); //333-333
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0334, 009, 0, string.Empty, ' ')); //334-342
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0343, 007, 0, string.Empty, ' ')); //343-349
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0350, 009, 0, string.Empty, ' ')); //350-358
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0359, 007, 0, string.Empty, ' ')); //359-365
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0366, 009, 0, string.Empty, ' ')); //366-374
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0375, 007, 0, string.Empty, ' ')); //375-381
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0382, 009, 0, string.Empty, ' ')); //382-390
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0391, 002, 0, string.Empty, ' ')); //391-392
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
            
            Identificacao = (string)CamposEdi[0].ValorNatural;
            Zeros1 = (string)CamposEdi[1].ValorNatural;
            Zeros2 = (string)CamposEdi[2].ValorNatural;
            PrefixoAgencia = (string)CamposEdi[3].ValorNatural;
            DVPrefixoAgencia = (string)CamposEdi[4].ValorNatural;
            ContaCorrente = (string)CamposEdi[5].ValorNatural;
            DVContaCorrente = (string)CamposEdi[6].ValorNatural;
            NumeroConvenioCobranca = (string)CamposEdi[7].ValorNatural;
            NumeroControleParticipante = (string)CamposEdi[8].ValorNatural;
            NossoNumero = (string)CamposEdi[9].ValorNatural;
            TipoCobranca = (string)CamposEdi[10].ValorNatural;
            TipoCobrancaEspecifico = (string)CamposEdi[11].ValorNatural;
            DiasCalculo = (string)CamposEdi[12].ValorNatural;
            NaturezaRecebimento = (string)CamposEdi[13].ValorNatural;
            PrefixoTitulo = (string)CamposEdi[14].ValorNatural;
            VariacaoCarteira = (string)CamposEdi[15].ValorNatural;
            ContaCaucao = (string)CamposEdi[16].ValorNatural;
            TaxaDesconto = (string)CamposEdi[17].ValorNatural;
            TaxaIOF = (string)CamposEdi[18].ValorNatural;
            Brancos1 = (string)CamposEdi[19].ValorNatural;
            Carteira = (string)CamposEdi[20].ValorNatural;
            Comando = (string)CamposEdi[21].ValorNatural;
            DataLiquidacao = (string)CamposEdi[22].ValorNatural;
            NumeroTituloCedente = (string)CamposEdi[23].ValorNatural;
            Brancos2 = (string)CamposEdi[24].ValorNatural;
            DataVencimento = (string)CamposEdi[25].ValorNatural;
            ValorTitulo = (string)CamposEdi[26].ValorNatural;
            CodigoBancoRecebedor = (string)CamposEdi[27].ValorNatural;
            PrefixoAgenciaRecebedora = (string)CamposEdi[28].ValorNatural;
            DVPrefixoRecebedora = (string)CamposEdi[29].ValorNatural;
            EspecieTitulo = (string)CamposEdi[30].ValorNatural;
            DataCredito = (string)CamposEdi[31].ValorNatural;
            ValorTarifa = (string)CamposEdi[32].ValorNatural;
            OutrasDespesas = (string)CamposEdi[33].ValorNatural;
            JurosDesconto = (string)CamposEdi[34].ValorNatural;
            IOFDesconto = (string)CamposEdi[35].ValorNatural;
            ValorAbatimento = (string)CamposEdi[36].ValorNatural;
            DescontoConcedido = (string)CamposEdi[37].ValorNatural;
            ValorRecebido = (string)CamposEdi[38].ValorNatural;
            JurosMora = (string)CamposEdi[39].ValorNatural;
            OutrosRecebimentos = (string)CamposEdi[40].ValorNatural;
            AbatimentoNaoAproveitado = (string)CamposEdi[41].ValorNatural;
            ValorLancamento = (string)CamposEdi[42].ValorNatural;
            IndicativoDebitoCredito = (string)CamposEdi[43].ValorNatural;
            IndicadorValor = (string)CamposEdi[44].ValorNatural;
            ValorAjuste = (string)CamposEdi[45].ValorNatural;
            Brancos3 = (string)CamposEdi[46].ValorNatural;
            Brancos4 = (string)CamposEdi[47].ValorNatural;
            Zeros3 = (string)CamposEdi[48].ValorNatural;
            Zeros4 = (string)CamposEdi[49].ValorNatural;
            Zeros5 = (string)CamposEdi[50].ValorNatural;
            Zeros6 = (string)CamposEdi[51].ValorNatural;
            Zeros7 = (string)CamposEdi[52].ValorNatural;
            Zeros8 = (string)CamposEdi[53].ValorNatural;
            Brancos5 = (string)CamposEdi[54].ValorNatural;
            CanalPagamento = (string)CamposEdi[55].ValorNatural;
            NumeroSequenciaRegistro = (string)CamposEdi[56].ValorNatural;

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
