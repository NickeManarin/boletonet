namespace BoletoNet
{
    /// <summary>
    /// Classe para representa��o de Conta Banc�ria
    /// </summary>
    public class ContaBancaria
    {
        #region Constructors
        
        /// <summary>
        /// Cria uma nova inst�ncia de conta banc�ria
        /// </summary>
        public ContaBancaria()
        {}

        /// <summary>
        /// Cria uma nova inst�ncia de conta banc�ria
        /// </summary>
        /// <param name="agencia">N�mero da Ag�ncia</param>
        /// <param name="conta">N�mero da Conta</param>
        public ContaBancaria(string agencia, string conta)
        {
            Agencia = agencia;
            Conta = conta;
        }

        /// <summary>
        /// Cria uma nova inst�ncia de conta banc�ria
        /// </summary>
        /// <param name="agencia">N�mero da Ag�ncia</param>
        /// <param name="digitoAgencia">D�gito da Ag�ncia</param>
        /// <param name="conta">N�mero da Conta</param>
        /// <param name="digitoConta">D�gito da Conta</param>
        public ContaBancaria(string agencia, string digitoAgencia, string conta, string digitoConta)
        {
            Agencia = agencia;
            DigitoAgencia = digitoAgencia;
            Conta = conta;
            DigitoConta = digitoConta;
        }

        /// <summary>
        /// Cria uma nova inst�ncia de conta banc�ria
        /// </summary>
        /// <param name="agencia">N�mero da Ag�ncia</param>
        /// <param name="digitoAgencia">D�gito da Ag�ncia</param>
        /// <param name="conta">N�mero da Conta</param>
        /// <param name="digitoConta">D�gito da Conta</param>
        /// <param name="operacaoConta">Opera��o da Conta</param>
        public ContaBancaria(string agencia, string digitoAgencia, string conta, string digitoConta, string operacaoConta)
        {
            Agencia = agencia;
            DigitoAgencia = digitoAgencia;
            Conta = conta;
            DigitoConta = digitoConta;
            OperacaConta = operacaoConta;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Retorna o numero da ag�ncia.
        /// <remarks>
        /// Completar com zeros a esquerda quando necessario
        /// </remarks>
        /// </summary>
        public string Agencia {get; set;}

        /// <summary>
        /// Digito da Ag�ncia
        /// </summary>
        public string DigitoAgencia { get; set;}

        /// <summary>
        /// N�mero da Conta Corrente
        /// Santander: Conta cobran�a.
        /// </summary>
        public string Conta {get; set;}

        /// <summary>
        /// Digito da Conta Corrente
        /// </summary>
        public string DigitoConta { get; set; }
        
        /// <summary>
        /// Opera��o da Conta Corrente
        /// </summary>
        public string OperacaConta { get; set; }

        #endregion
    }
}