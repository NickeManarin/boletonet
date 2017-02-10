using System;

namespace BoletoNet.EDI.Banco
{
    /// <summary>
	/// Classe de Integração Banrisul
    /// CBR643
    /// Convênio 6 posições -- Único com Identificação do Registro Detalhe = 1
	/// </summary>
    public class RegistroEdi_Banco_Nordeste_Retorno : RegistroEdi
    {
        #region Atributos e Propriedades

        public string Identificacao { get; set; }

        public string Zeros1 { get; set; }

        public string Zeros2 { get; set; }

        public string PrefixoAgencia { get; set; }

        public string DvPrefixoAgencia { get; set; }

        public string ContaCorrente { get; set; }

        public string DvContaCorrente { get; set; }

        public string NumeroConvenioCobranca { get; set; }

        public string NumeroControleParticipante { get; set; }

        public string NossoNumero { get; set; }

        public string NossoNumeroDv { get; set; }

        public string TipoCobranca { get; set; }

        public string TipoCobrancaEspecifico { get; set; }

        public string DiasCalculo { get; set; }

        public string NaturezaRecebimento { get; set; }

        public string PrefixoTitulo { get; set; }

        public string VariacaoCarteira { get; set; }

        public string ContaCaucao { get; set; }

        public string TaxaDesconto { get; set; }

        public string TaxaIof { get; set; }

        public string Brancos1 { get; set; }

        public string Carteira { get; set; }

        public string Comando { get; set; }

        public string DataOcorrencia { get; set; }

        public string NumeroTituloCedente { get; set; }

        public string Brancos2 { get; set; }

        public string DataVencimento { get; set; }

        public string ValorTitulo { get; set; }

        public string CodigoBancoRecebedor { get; set; }

        public string PrefixoAgenciaRecebedora { get; set; }

        public string DvPrefixoRecebedora { get; set; }

        public string EspecieTitulo { get; set; }

        public string DataCredito { get; set; }

        public string ValorTarifa { get; set; }

        public string OutrasDespesas { get; set; }

        public string JurosDesconto { get; set; }

        public string IofDesconto { get; set; }

        public string ValorAbatimento { get; set; }

        public string DescontoConcedido { get; set; }

        public string ValorRecebido { get; set; }

        public string JurosMora { get; set; }

        public string OutrosRecebimentos { get; set; }

        public string AbatimentoNaoAproveitado { get; set; }

        public string ValorLancamento { get; set; }

        public string IndicativoDebitoCredito { get; set; }

        public string IndicadorValor { get; set; }

        public string ValorAjuste { get; set; }

        public string Brancos3 { get; set; }

        public string Brancos4 { get; set; }

        public string Zeros3 { get; set; }

        public string Zeros4 { get; set; }

        public string Zeros5 { get; set; }

        public string Zeros6 { get; set; }

        public string Zeros7 { get; set; }

        public string Zeros8 { get; set; }

        public string Brancos5 { get; set; }

        public string CanalPagamento { get; set; }

        public string NumeroSequenciaRegistro { get; set; }

        #endregion

        public RegistroEdi_Banco_Nordeste_Retorno()
        {
            Zeros4 = string.Empty;
            Zeros3 = string.Empty;
            Brancos4 = string.Empty;
            Brancos3 = string.Empty;
            ValorAjuste = string.Empty;
            IndicadorValor = string.Empty;
            IndicativoDebitoCredito = string.Empty;
            ValorLancamento = string.Empty;
            AbatimentoNaoAproveitado = string.Empty;
            OutrosRecebimentos = string.Empty;
            JurosMora = string.Empty;
            ValorRecebido = string.Empty;
            DescontoConcedido = string.Empty;
            ValorAbatimento = string.Empty;
            IofDesconto = string.Empty;
            JurosDesconto = string.Empty;
            OutrasDespesas = string.Empty;
            ValorTarifa = string.Empty;
            DataCredito = string.Empty;
            DvPrefixoRecebedora = string.Empty;
            EspecieTitulo = string.Empty;
            PrefixoAgenciaRecebedora = string.Empty;
            CodigoBancoRecebedor = string.Empty;
            ValorTitulo = string.Empty;
            DataVencimento = string.Empty;
            Brancos2 = string.Empty;
            NumeroTituloCedente = string.Empty;
            DataOcorrencia = string.Empty;
            Comando = string.Empty;
            Carteira = string.Empty;
            NumeroSequenciaRegistro = string.Empty;
            CanalPagamento = string.Empty;
            Brancos5 = string.Empty;
            Zeros5 = string.Empty;
            Zeros6 = string.Empty;
            Zeros7 = string.Empty;
            Zeros8 = string.Empty;
            Brancos1 = string.Empty;
            TaxaIof = string.Empty;
            TaxaDesconto = string.Empty;
            ContaCaucao = string.Empty;
            VariacaoCarteira = string.Empty;
            PrefixoTitulo = string.Empty;
            NaturezaRecebimento = string.Empty;
            DiasCalculo = string.Empty;
            TipoCobrancaEspecifico = string.Empty;
            TipoCobranca = string.Empty;
            NossoNumero = string.Empty;
            NumeroControleParticipante = string.Empty;
            NumeroConvenioCobranca = string.Empty;
            DvContaCorrente = string.Empty;
            ContaCorrente = string.Empty;
            DvPrefixoAgencia = string.Empty;
            PrefixoAgencia = string.Empty;
            Zeros2 = string.Empty;
            Zeros1 = string.Empty;
            Identificacao = string.Empty;

            #region TODOS os Campos

            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0001, 001, 0, string.Empty, ' ')); //001-001 Preenchido com o número “1”
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0002, 002, 0, string.Empty, ' ')); //002-003 Preenchido com o tipo de inscrição do Cedente
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0004, 014, 0, string.Empty, ' ')); //004-017 Preenchido com o CGC ou CPF
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0018, 004, 0, string.Empty, ' ')); //018-021 Preenchido com o código da Agência na qual o Cliente opera
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0022, 002, 0, string.Empty, ' ')); //022-023 Zeros
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0024, 007, 0, string.Empty, ' ')); //024-030 Preenchido com o número da Conta Corrente do Cliente cadastrado na cobrança como cedente
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0031, 001, 0, string.Empty, ' ')); //031-031 Preenchido com o Dígito da Conta do Cliente
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0032, 006, 0, string.Empty, ' ')); //032-037 Brancos
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0038, 025, 0, string.Empty, ' ')); //038-062 Número Controle
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0063, 007, 0, string.Empty, ' ')); //063-069 Nosso Número
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0070, 001, 0, string.Empty, ' ')); //070-070 Dígito do Nosso Número
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0071, 010, 0, string.Empty, ' ')); //071-080 Número do Contrato
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0081, 027, 0, string.Empty, ' ')); //081-107 Brancos 
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0108, 001, 0, string.Empty, ' ')); //108-108 Carteira
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0109, 002, 0, string.Empty, ' ')); //109-010 Código de Serviço. 
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0111, 006, 0, string.Empty, ' ')); //111-116 Data de Ocorrência
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0117, 010, 0, string.Empty, ' ')); //117-126 Seu Número
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0127, 007, 0, string.Empty, ' ')); //127-133 Confirmação do Nosso Número
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0134, 001, 0, string.Empty, ' ')); //134-134 Confirmação do Dígito do Nosso Número
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0135, 012, 0, string.Empty, ' ')); //135-146 Brancos
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0147, 006, 0, string.Empty, ' ')); //147-152 Data de Vencimento
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0153, 013, 0, string.Empty, ' ')); //153-165 Valor Nominal do Título
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0166, 003, 0, string.Empty, ' ')); //166-168 Número do Banco.
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0169, 004, 0, string.Empty, ' ')); //169-172 Agência Cobradora. 
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0173, 001, 0, string.Empty, ' ')); //173-173 Branco
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0174, 002, 0, string.Empty, ' ')); //174-175 Espécie
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0176, 013, 0, string.Empty, ' ')); //176-188 Tarifa de Cobrança
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0189, 013, 0, string.Empty, ' ')); //189-201 Outras 
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0202, 011, 0, string.Empty, ' ')); //202-214 Juros
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0215, 013, 0, string.Empty, ' ')); //215-227 IOC de Operações de Seguro
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0228, 013, 0, string.Empty, ' ')); //228-240 Abatimento Concedido
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0241, 013, 0, string.Empty, ' ')); //241-253 Desconto Concedido
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0254, 013, 0, string.Empty, ' ')); //254-266 Valor Recebido
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0267, 013, 0, string.Empty, ' ')); //267-279 Juros de Mora
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0280, 115, 0, string.Empty, ' ')); //280-394 Tabela de Erros
            CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0395, 006, 0, string.Empty, ' ')); //395-400 Sequencial do Registro

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
            PrefixoAgencia = (string)CamposEdi[3].ValorNatural;
            ContaCorrente = (string)CamposEdi[5].ValorNatural;
            DvContaCorrente = (string)CamposEdi[6].ValorNatural;
            NumeroControleParticipante = (string)CamposEdi[8].ValorNatural;
            NossoNumero = (string)CamposEdi[9].ValorNatural;
            NossoNumeroDv = (string)CamposEdi[10].ValorNatural;
            Carteira = (string)CamposEdi[13].ValorNatural;

            Comando = (string)CamposEdi[14].ValorNatural;            
            DataOcorrencia = (string)CamposEdi[15].ValorNatural;            
            NumeroTituloCedente = (string)CamposEdi[16].ValorNatural;
            DataVencimento = (string)CamposEdi[20].ValorNatural;
            ValorTitulo = (string)CamposEdi[21].ValorNatural;
            EspecieTitulo = (string)CamposEdi[24].ValorNatural;
            ValorTarifa = (string)CamposEdi[26].ValorNatural;
            OutrasDespesas = (string)CamposEdi[27].ValorNatural;
            JurosDesconto = (string)CamposEdi[28].ValorNatural;
            ValorAbatimento = (string)CamposEdi[30].ValorNatural;
            DescontoConcedido = (string)CamposEdi[31].ValorNatural;
            ValorRecebido = (string)CamposEdi[32].ValorNatural;
            JurosMora = (string)CamposEdi[33].ValorNatural;
            NumeroSequenciaRegistro = (string)CamposEdi[35].ValorNatural;

            /*
            this._TipoCobranca = (string)this._CamposEDI[10].ValorNatural;
            this._TipoCobrancaEspecifico = (string)this._CamposEDI[11].ValorNatural;
            this._DiasCalculo = (string)this._CamposEDI[12].ValorNatural;
            this._NaturezaRecebimento = (string)this._CamposEDI[13].ValorNatural;
            this._PrefixoTitulo = (string)this._CamposEDI[14].ValorNatural;
            this._VariacaoCarteira = (string)this._CamposEDI[15].ValorNatural;
            this._ContaCaucao = (string)this._CamposEDI[16].ValorNatural;
            this._TaxaDesconto = (string)this._CamposEDI[17].ValorNatural;
            this._TaxaIOF = (string)this._CamposEDI[18].ValorNatural;
            this._Brancos1 = (string)this._CamposEDI[19].ValorNatural;
            this._Carteira = (string)this._CamposEDI[20].ValorNatural;
            this._Comando = (string)this._CamposEDI[21].ValorNatural;
            
            this._NumeroTituloCedente = (string)this._CamposEDI[23].ValorNatural;
            this._Brancos2 = (string)this._CamposEDI[24].ValorNatural;
            this._DataVencimento = (string)this._CamposEDI[25].ValorNatural;
            this._ValorTitulo = (string)this._CamposEDI[26].ValorNatural;
            this._CodigoBancoRecebedor = (string)this._CamposEDI[27].ValorNatural;
            this._PrefixoAgenciaRecebedora = (string)this._CamposEDI[28].ValorNatural;
            this._DVPrefixoRecebedora = (string)this._CamposEDI[29].ValorNatural;
            this._EspecieTitulo = (string)this._CamposEDI[30].ValorNatural;
            this._DataCredito = (string)this._CamposEDI[31].ValorNatural;
            this._ValorTarifa = (string)this._CamposEDI[32].ValorNatural;
            this._OutrasDespesas = (string)this._CamposEDI[33].ValorNatural;
            this._JurosDesconto = (string)this._CamposEDI[34].ValorNatural;
            this._IOFDesconto = (string)this._CamposEDI[35].ValorNatural;
            this._ValorAbatimento = (string)this._CamposEDI[36].ValorNatural;
            this._DescontoConcedido = (string)this._CamposEDI[37].ValorNatural;
            this._ValorRecebido = (string)this._CamposEDI[38].ValorNatural;
            this._JurosMora = (string)this._CamposEDI[39].ValorNatural;
            this._OutrosRecebimentos = (string)this._CamposEDI[40].ValorNatural;
            this._AbatimentoNaoAproveitado = (string)this._CamposEDI[41].ValorNatural;
            this._ValorLancamento = (string)this._CamposEDI[42].ValorNatural;
            this._IndicativoDebitoCredito = (string)this._CamposEDI[43].ValorNatural;
            this._IndicadorValor = (string)this._CamposEDI[44].ValorNatural;
            this._ValorAjuste = (string)this._CamposEDI[45].ValorNatural;
            this._Brancos3 = (string)this._CamposEDI[46].ValorNatural;
            this._Brancos4 = (string)this._CamposEDI[47].ValorNatural;
            this._Zeros3 = (string)this._CamposEDI[48].ValorNatural;
            this._Zeros4 = (string)this._CamposEDI[49].ValorNatural;
            this._Zeros5 = (string)this._CamposEDI[50].ValorNatural;
            this._Zeros6 = (string)this._CamposEDI[51].ValorNatural;
            this._Zeros7 = (string)this._CamposEDI[52].ValorNatural;
            this._Zeros8 = (string)this._CamposEDI[53].ValorNatural;
            this._Brancos5 = (string)this._CamposEDI[54].ValorNatural;
            this._CanalPagamento = (string)this._CamposEDI[55].ValorNatural;
            this._NumeroSequenciaRegistro = (string)this._CamposEDI[56].ValorNatural;*/
            
		}
	}

	/// <summary>
	/// Classe que irá representar o arquivo EDI em si
	/// </summary>
    public class ArquivoBanco_Nordeste_Retorno_Edi : EdiFile
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

            Lines.Add(new RegistroEdi_Banco_Nordeste_Retorno()); //Adiciono a linha a ser decodificada
			Lines[Lines.Count - 1].LinhaRegistro = Line; //Atribuo a linha que vem do arquivo
            Lines[Lines.Count - 1].DecodificarLinha(); //Finalmente, a separação das substrings na linha do arquivo.
		}
	}
}
