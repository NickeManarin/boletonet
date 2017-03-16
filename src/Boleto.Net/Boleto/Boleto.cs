namespace BoletoNet
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
    using System.Collections.ObjectModel;
    using DemonstrativoValoresBoleto;

	[Serializable, Browsable(false)]
	public class Boleto
	{
		#region Variaveis

		private readonly CodigoBarra _codigoBarra = new CodigoBarra();

		private string _carteira = string.Empty;
		private string _variacaoCarteira = string.Empty;
		private string _nossoNumero = string.Empty;
		private string _digitoNossoNumero = string.Empty;
        //private bool _apenasRegistrar = false;
		private DateTime _dataVencimento;
		private DateTime _dataDocumento;
		private DateTime _dataProcessamento;
		private int _numeroParcela;
		private decimal _valorBoleto;
		private decimal _valorCobrado;
		private string _localPagamento = "Até o vencimento, preferencialmente no ";
		private int _quantidadeMoeda = 1;
		private string _valorMoeda = string.Empty;
		private IList<IInstrucao> _instrucoes = new List<IInstrucao>();
		private IEspecieDocumento _especieDocumento = new EspecieDocumento();
		private string _aceite = "N";
		private string _numeroDocumento = string.Empty;
		private string _especie = "R$";
		private int _moeda = 9;
		private string _usoBanco = string.Empty;

		private Cedente _cedente;
		private int _categoria;

		// private string _instrucoesHtml = string.Empty;
		private IBanco _banco;
		private ContaBancaria _contaBancaria = new ContaBancaria();
		private decimal _valorDesconto;
		private Sacado _sacado;
		private bool _jurosPermanente;

		private decimal _percJurosMora;
		private decimal _jurosMora;
        private string _codJurosMora = string.Empty;
		private decimal _iof;
		private decimal _abatimento;
		private decimal _percMulta;
		private decimal _valorMulta;
		private decimal _outrosAcrescimos;
		private decimal _outrosDescontos;
		private DateTime _dataJurosMora;
		private DateTime _dataMulta;
		private DateTime _dataDesconto;
		private DateTime _dataOutrosAcrescimos;
		private DateTime _dataOutrosDescontos;
		private short _percentualIos;
        private short _modalidadeCobranca;
        private short _numeroDiasBaixa;
		private string _numeroControle;

		private string _tipoModalidade = string.Empty;
		private Remessa _remessa;

		private ObservableCollection<GrupoDemonstrativo> _demonstrativos;

        #endregion

        #region Construtor

        public Boleto()
		{}

		public Boleto(DateTime dataVencimento, decimal valorBoleto, string carteira, string nossoNumero, Cedente cedente, EspecieDocumento especieDocumento)
		{
			_carteira = carteira;
			_nossoNumero = nossoNumero;
			_dataVencimento = dataVencimento;
			_valorBoleto = valorBoleto;
			_valorBoleto = valorBoleto;
			_valorCobrado = ValorCobrado;
			_cedente = cedente;

			_especieDocumento = especieDocumento;
		}

		public Boleto(decimal valorBoleto, string carteira, string nossoNumero, Cedente cedente)
		{
			_carteira = carteira;
			_nossoNumero = nossoNumero;
			_valorBoleto = valorBoleto;
			_valorBoleto = valorBoleto;
			_valorCobrado = ValorCobrado;
			_cedente = cedente;
		}

		public Boleto(DateTime dataVencimento, decimal valorBoleto, string carteira, string nossoNumero, Cedente cedente)
		{
			_carteira = carteira;
			_nossoNumero = nossoNumero;
			_dataVencimento = dataVencimento;
			_valorBoleto = valorBoleto;
			_valorBoleto = valorBoleto;
			_valorCobrado = ValorCobrado;
			_cedente = cedente;
		}

		public Boleto(DateTime dataVencimento, decimal valorBoleto, string carteira, string nossoNumero, string digitoNossoNumero, Cedente cedente)
		{
			_carteira = carteira;
			_nossoNumero = nossoNumero;
			_digitoNossoNumero = digitoNossoNumero;
			_dataVencimento = dataVencimento;
			_valorBoleto = valorBoleto;
			_valorBoleto = valorBoleto;
			_valorCobrado = ValorCobrado;
			_cedente = cedente;
		}

		public Boleto(DateTime dataVencimento, decimal valorBoleto, string carteira, string nossoNumero, string agencia, string conta)
		{
			_carteira = carteira;
			_nossoNumero = nossoNumero;
			_dataVencimento = dataVencimento;
			_valorBoleto = valorBoleto;
			_valorBoleto = valorBoleto;
			_valorCobrado = ValorCobrado;
			_cedente = new Cedente(new ContaBancaria(agencia, conta));
		}

		#endregion

		#region Properties

		public ObservableCollection<GrupoDemonstrativo> Demonstrativos
		{
			get
			{
				return _demonstrativos ?? (_demonstrativos = new ObservableCollection<GrupoDemonstrativo>());
			}
		}

		/// <summary> 
		/// Retorna a Categoria do boleto
		/// </summary>
		public int Categoria
		{
			get { return _categoria; }
			set { _categoria = value; }
		}

		/// <summary> 
		/// Retorna o numero da carteira.
		/// </summary>
		public string Carteira
		{
			get { return _carteira; }
			set { _carteira = value; }
		}

		/// <summary> 
		/// Retorna a Variação da carteira.
		/// </summary>
		public string VariacaoCarteira
		{
			get { return _variacaoCarteira; }
			set { _variacaoCarteira = value; }
		}

		/// <summary> 
		/// Retorna a data do vencimento do titulo.
		/// </summary>
		public DateTime DataVencimento
		{
			get { return _dataVencimento; }
			set { _dataVencimento = value; }
		}

		/// <summary> 
		/// Retorna o valor do titulo.
		/// </summary>
		public decimal ValorBoleto
		{
			get { return _valorBoleto; }
			set { _valorBoleto = value; }
		}

		/// <summary> 
		/// Retorna o valor Cobrado.
		/// </summary>
		public decimal ValorCobrado
		{
			get { return _valorCobrado; }
			set { _valorCobrado = value; }
		}

		/// <summary> 
		/// Retorna o campo para a 1 linha da instrucao.
		/// </summary>
		public IList<IInstrucao> Instrucoes
		{
			get { return _instrucoes; }
			set { _instrucoes = value; }
		}

		/// <summary> 
		/// Retorna o local de pagamento.
		/// </summary>
		public string LocalPagamento
		{
			get { return _localPagamento; }
			set { _localPagamento = value; }
		}

		/// <summary> 
		/// Retorna a quantidade da moeda.
		/// </summary>
		public int QuantidadeMoeda
		{
			get { return _quantidadeMoeda; }
			set { _quantidadeMoeda = value; }
		}

		/// <summary> 
		/// Retorna o valor da moeda
		/// </summary>
		public string ValorMoeda
		{
			get { return _valorMoeda; }
			set { _valorMoeda = value; }
		}

		/// <summary> 
		/// Retorna o campo aceite que por padrao vem com N.
		/// </summary>
		public string Aceite
		{
			get { return _aceite; }
			set { _aceite = value; }
		}

		/// <summary> 
		/// Retorna o campo especie do documento que por padrao vem com DV
		/// </summary>
		public string Especie
		{
			get { return _especie; }
			set { _especie = value; }
		}

        /// <summary> 
        /// Se verdadeiro, habilita a impressão e postagem do boleto pelo banco. 
        /// </summary>
        public bool Postagem { get; set; }

        /// <summary> 
        /// Campo utilizado pelo Sicredi, para indicar qual campo foi alterado.
        /// A – Desconto; 
        /// B - Juros por dia; 
        /// C - Desconto por dia de antecipação; 
        /// D - Data limite para concessão de desconto; 
        /// E - Cancelamento de protesto automático; 
        /// F - Carteira de cobrança - não disponível. 
        /// </summary>
        public string OutrosDadosAlterados { get; set; }

        /// <summary> 
        /// Retorna o campo especie do documento que por padrao vem com DV
        /// </summary>
        public IEspecieDocumento EspecieDocumento
		{
			get { return _especieDocumento ?? (_especieDocumento = new EspecieDocumento()); }
			set { _especieDocumento = value; }
		}

		/// <summary> 
		/// Retorna a data do documento.
		/// </summary>        
		public DateTime DataDocumento
		{
			get { return _dataDocumento; }
			set { _dataDocumento = value; }
		}

		/// <summary> 
		/// Retorna a data do processamento
		/// </summary>        
		public DateTime DataProcessamento
		{
			get { return _dataProcessamento; }
			set { _dataProcessamento = value; }
		}

        /// <summary> 
        /// Retorna o numero de parcelas
        /// </summary>        
        public int NumeroParcela
		{
			get { return _numeroParcela; }
			set { _numeroParcela = value; }
		}

		/// <summary> 
		/// Recupera o número do documento
		/// </summary>        
		public string NumeroDocumento
		{
			get { return _numeroDocumento; }
			set { _numeroDocumento = value; }
		}

        /// <summary>
        /// Informações do documento utilizado apenas para a impressão.
        /// </summary>
        public string NumeroDocumentoImpressao { get; set; }

		/// <summary> 
		/// Recupera o digito nosso número 
		/// </summary>        
		public string DigitoNossoNumero
		{
			get { return _digitoNossoNumero; }
			set { _digitoNossoNumero = value; }
		}

		/// <summary> 
		/// Recupara o nosso número 
		/// </summary>        
		public string NossoNumero
		{
			get { return _nossoNumero; }
			set { _nossoNumero = value; }
		}

        /// <summary> 
        /// Condição para Emissão da Papeleta de Cobrança
        /// 1 = Banco emite e Processa o registro. 2 = Cliente emite e o Banco somente processa o registro
        /// </summary>        
        //public bool ApenasRegistrar
        //{
        //    get { return _apenasRegistrar; }
        //    set { _apenasRegistrar = value; }
        //}

		/// <summary> 
		/// Recupera o valor da moeda 
		/// </summary>  
		public int Moeda
		{
			get { return _moeda; }
			set { _moeda = value; }
		}

		public Cedente Cedente
		{
			get { return _cedente; }
			set { _cedente = value; }
		}

		public CodigoBarra CodigoBarra
		{
			get { return _codigoBarra; }
		}

		public IBanco Banco
		{
			get { return _banco; }
			set { _banco = value; }
		}

		public ContaBancaria ContaBancaria
		{
			get { return _contaBancaria; }
			set { _contaBancaria = value; }
		}

		/// <summary> 
		/// Retorna o valor do desconto do titulo.
		/// </summary>
		public decimal ValorDesconto
		{
			get { return _valorDesconto; }
			set { _valorDesconto = value; }
		}

		/// <summary>
		/// Retorna do Sacado
		/// </summary>
		public Sacado Sacado
		{
			get { return _sacado; }
			set { _sacado = value; }
		}

		/// <summary>
		/// Dados do avalista.
		/// Este campo é necessário para correspondentes bancários, como 
		/// por exemplo o Banco Daycoval.
		/// O avalista deve ser exibido para que estes bancos homologuem.
		/// </summary>
		public Cedente Avalista { get; set; }

		/// <summary> 
		/// Para uso do banco 
		/// </summary>        
		public string UsoBanco
		{
			get { return _usoBanco; }
			set { _usoBanco = value; }
		}

		/// <summary>
		/// Percentual de Juros de Mora (ao dia, ao mes setar codJurosMora com "2")
		/// </summary>
		public decimal PercJurosMora
		{
			get { return _percJurosMora; }
			set { _percJurosMora = value; }
		}

		/// <summary> 
		/// Juros de mora (ao dia)
		/// </summary>  
		public decimal JurosMora
		{
			get { return _jurosMora; }
			set { _jurosMora = value; }
		}

        /// <summary> 
		/// Código de Juros de mora (1 = ao dia, 2 = ao mes)
		/// </summary>  
        public string CodJurosMora
        {
            get { return _codJurosMora; }
            set { _codJurosMora = value; }
        }

        /// <summary>
        /// Caso a empresa tenha no convênio Juros permanentes cadastrados
        /// </summary>
        public bool JurosPermanente
		{
			get { return _jurosPermanente; }
			set { _jurosPermanente = value; }
		}

		/// <summary> 
		/// IOF
		/// </summary>  
		public decimal Iof
		{
			get { return _iof; }
			set { _iof = value; }
		}

		/// <summary> 
		/// Abatimento
		/// </summary>  
		public decimal Abatimento
		{
			get { return _abatimento; }
			set { _abatimento = value; }
		}

		/// <summary> 
		/// Percentual da Multa
		/// </summary>  
		public decimal PercMulta
		{
			get { return _percMulta; }
			set { _percMulta = value; }
		}

		/// <summary> 
		/// Valor da Multa
		/// </summary>  
		public decimal ValorMulta
		{
			get { return _valorMulta; }
			set { _valorMulta = value; }
		}

		/// <summary> 
		/// Outros Acréscimos
		/// </summary>  
		public decimal OutrosAcrescimos
		{
			get { return _outrosAcrescimos; }
			set { _outrosAcrescimos = value; }
		}

		/// <summary> 
		/// Outros descontos
		/// </summary>  
		public decimal OutrosDescontos
		{
			get { return _outrosDescontos; }
			set { _outrosDescontos = value; }
		}

		/// <summary> 
		/// Data do Juros de Mora
		/// </summary>  
		public DateTime DataJurosMora
		{
			get { return _dataJurosMora; }
			set { _dataJurosMora = value; }
		}

		/// <summary> 
		/// Data do Juros da Multa
		/// </summary>  
		public DateTime DataMulta
		{
			get { return _dataMulta; }
			set { _dataMulta = value; }
		}

		/// <summary> 
		/// Data do Juros do Desconto
		/// </summary>  
		public DateTime DataDesconto
		{
			get { return _dataDesconto; }
			set { _dataDesconto = value; }
		}

		/// <summary> 
		/// Data de Outros Acréscimos
		/// </summary>  
		public DateTime DataOutrosAcrescimos
		{
			get { return _dataOutrosAcrescimos; }
			set { _dataOutrosAcrescimos = value; }
		}

		/// <summary> 
		/// Data de Outros Descontos
		/// </summary>  
		public DateTime DataOutrosDescontos
		{
			get { return _dataOutrosDescontos; }
			set { _dataOutrosDescontos = value; }
		}

		/// <summary> 
		/// Retorna o tipo da modalidade
		/// </summary>
		public string TipoModalidade
		{
			get { return _tipoModalidade; }
			set { _tipoModalidade = value; }
		}

		/// <summary> 
		/// Retorna o percentual IOS para Seguradoras no caso do Banco Santander
		/// </summary>
		public short PercentualIos
		{
			get { return _percentualIos; }
			set { _percentualIos = value; }
		}

        /// <summary> 
        /// C006 - Retorna a modalidade de cobrança/código carteira 1-Cobrança Simples 2-Cobrança Vinculada 3-Cobrança Caucionada 4-Cobrança Descontada 5-Cobrança Vendor 
        /// </summary>
        public short ModalidadeCobranca
        {
            get { return _modalidadeCobranca; }
            set { _modalidadeCobranca = value; }
        }

        /// <summary> 
        /// Número de dias para Baixa/Devolução
        /// </summary>
        public short NumeroDiasBaixa
        {
            get { return _numeroDiasBaixa; }
            set { _numeroDiasBaixa = value; }
        }

        /// <summary>
        /// Retorna os Parâmetros utilizados na geração da Remessa para o Boleto
        /// </summary>
        public Remessa Remessa
		{
			get { return _remessa; }
			set { _remessa = value; }
		}

        /// <summary> 
        /// Recupara o número do Controle de participante.
        /// </summary>        
        public string NumeroControle
        {
            get { return _numeroControle; }
            set { _numeroControle = value; }
        }

        /// <summary>
        /// Número da parcela atual.
        /// </summary>
        public int Parcela { get; set; }

        /// <summary>
        /// Total de parcelas. Se 1, será informado como parcela única no boleto.
        /// </summary>
        public int TotalParcelas { get; set; }

        public IBancoCarteira BancoCarteira { get; set; }

		#endregion Properties

		public void Valida()
		{
			// Validações básicas, caso ainda tenha implementada na classe do banco.ValidaBoleto()
			if (Cedente == null)
				throw new Exception("Cedente não cadastrado.");

			// Atribui o nome do banco ao local de pagamento
			// Comentada por duplicidade no nome do banco
			////this.LocalPagamento += this.Banco.Nome + string.Empty;

			// Verifica se data do processamento é valida
			// if (this.DataProcessamento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
				DataProcessamento = DateTime.Now;

			// Verifica se data do documento é valida
			////if (this.DataDocumento.ToString("dd/MM/yyyy") == "01/01/0001")
			if (DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
				DataDocumento = DateTime.Now;

			Banco.ValidaBoleto(this);
		}

		public void FormataCampos()
		{
			try
			{
				QuantidadeMoeda = 0;
				Banco.FormataCodigoBarra(this);
				Banco.FormataLinhaDigitavel(this);
				Banco.FormataNossoNumero(this);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro durante a formatação dos campos.", ex);
			}
		}
	}
}
