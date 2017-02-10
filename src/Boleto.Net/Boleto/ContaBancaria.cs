namespace BoletoNet
{
    /// <summary>
    /// Classe para representação de Conta Bancária
    /// </summary>
    public class ContaBancaria
    {
        #region Constructors
        
        /// <summary>
        /// Cria uma nova instância de conta bancária
        /// </summary>
        public ContaBancaria()
        {}

        /// <summary>
        /// Cria uma nova instância de conta bancária
        /// </summary>
        /// <param name="agencia">Número da Agência</param>
        /// <param name="conta">Número da Conta</param>
        public ContaBancaria(string agencia, string conta)
        {
            Agencia = agencia;
            Conta = conta;
        }

        /// <summary>
        /// Cria uma nova instância de conta bancária
        /// </summary>
        /// <param name="agencia">Número da Agência</param>
        /// <param name="digitoAgencia">Dígito da Agência</param>
        /// <param name="conta">Número da Conta</param>
        /// <param name="digitoConta">Dígito da Conta</param>
        public ContaBancaria(string agencia, string digitoAgencia, string conta, string digitoConta)
        {
            Agencia = agencia;
            DigitoAgencia = digitoAgencia;
            Conta = conta;
            DigitoConta = digitoConta;
        }

        /// <summary>
        /// Cria uma nova instância de conta bancária
        /// </summary>
        /// <param name="agencia">Número da Agência</param>
        /// <param name="digitoAgencia">Dígito da Agência</param>
        /// <param name="conta">Número da Conta</param>
        /// <param name="digitoConta">Dígito da Conta</param>
        /// <param name="operacaoConta">Operação da Conta</param>
        public ContaBancaria(string agencia, string digitoAgencia, string conta, string digitoConta, string operacaoConta)
        {
            Agencia = agencia;
            DigitoAgencia = digitoAgencia;
            Conta = conta;
            DigitoConta = digitoConta;
            OperacaConta = operacaoConta;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Retorna o numero da agência.
        /// <remarks>
        /// Completar com zeros a esquerda quando necessario
        /// </remarks>
        /// </summary>
        public string Agencia {get; set;}

        /// <summary>
        /// Digito da Agência
        /// </summary>
        public string DigitoAgencia { get; set;}

        /// <summary>
        /// Número da Conta Corrente
        /// </summary>
        public string Conta {get; set;}

        /// <summary>
        /// Digito da Conta Corrente
        /// </summary>
        public string DigitoConta { get; set; }
        
        /// <summary>
        /// Operação da Conta Corrente
        /// </summary>
        public string OperacaConta { get; set; }

        #endregion
    }
}