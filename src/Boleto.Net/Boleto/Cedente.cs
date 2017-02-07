using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BoletoNet
{
    [Serializable, Browsable(false)]
    public class Cedente
    {
        #region Vari�veis

        private string _codigo = "0";
        private string _cpfcnpj;
        private string _nome;
        private ContaBancaria _contaBancaria;
        private long _convenio = 0;
        private int _numeroSequencial;
        private string _codigoTransmissao;
        private int _numeroBordero;
        private int _digitoCedente = -1;
        private string _carteira;
        private Endereco _endereco;
        private IList<IInstrucao> _instrucoes = new List<IInstrucao>();
        private bool _mostrarCnpjNoBoleto = false;

        #endregion Variaveis

        public Cedente()
        { }

        public Cedente(ContaBancaria contaBancaria)
        {
            _contaBancaria = contaBancaria;
        }

        public Cedente(string cpfCnpj, string nome)
        {
            CpfCnpj = cpfCnpj;
            _nome = nome;
        }

        public Cedente(string cpfCnpj, string nome, string agencia, string digitoAgencia, string conta, string digitoConta, string operacaoConta) :
            this(cpfCnpj, nome, agencia, digitoAgencia, conta, digitoConta)
        {
            _contaBancaria = new ContaBancaria
            {
                Agencia = agencia,
                DigitoAgencia = digitoAgencia,
                Conta = conta,
                DigitoConta = digitoConta,
                OperacaConta = operacaoConta
            };
        }

        public Cedente(string cpfCnpj, string nome, string agencia, string digitoAgencia, string conta, string digitoConta)
            : this(cpfCnpj, nome)
        {
            _contaBancaria = new ContaBancaria
            {
                Agencia = agencia,
                DigitoAgencia = digitoAgencia,
                Conta = conta,
                DigitoConta = digitoConta
            };
        }

        public Cedente(string cpfCnpj, string nome, string agencia, string conta, string digitoConta) :
            this(cpfCnpj, nome)
        {
            _contaBancaria = new ContaBancaria();
            _contaBancaria.Agencia = agencia;
            _contaBancaria.Conta = conta;
            _contaBancaria.DigitoConta = digitoConta;
        }

        public Cedente(string cpfCnpj, string nome, string agencia, string conta)
            : this(cpfCnpj, nome)
        {
            _contaBancaria = new ContaBancaria();
            _contaBancaria.Agencia = agencia;
            _contaBancaria.Conta = conta;
        }

        #region Propriedades

        /// <summary>
        /// C�digo do Cedente
        /// </summary>
        public string Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        public int DigitoCedente
        {
            get { return _digitoCedente; }
            set { _digitoCedente = value; }
        }

        /// <summary>
        /// Retona o CPF ou CNPJ do Cedente
        /// </summary>
        public string CpfCnpj
        {
            get
            {
                return _cpfcnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            }
            set
            {
                var o = value.Replace(".", "").Replace("-", "").Replace("/", "");

                if (o == null || (o.Length != 11 && o.Length != 14))
                    throw new ArgumentException("O CPF/CNPJ inv�lido. Utilize 11 d�gitos para CPF ou 14 para CNPJ.");

                _cpfcnpj = value;
            }
        }

        /// <summary>
        /// Retona o CPF ou CNPJ do Cedente (com m�scara)
        /// </summary>
        public string CpfCnpjComMascara
        {
            get { return _cpfcnpj; }
        }

        /// <summary>
        /// Nome do Cedente
        /// </summary>
        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        /// <summary>
        /// Conta Correnta do Cedente
        /// </summary>
        public ContaBancaria ContaBancaria
        {
            get { return _contaBancaria; }
            set { _contaBancaria = value; }
        }

        /// <summary>
        /// N�mero do Conv�nio
        /// </summary>
        public long Convenio
        {
            get { return _convenio; }
            set { _convenio = Convert.ToInt64(value); }
        }

        /// <summary>
        /// N�mero sequencial para gera��o de remessa
        /// </summary>
        public int NumeroSequencial
        {
            get { return _numeroSequencial; }
            set { _numeroSequencial = value; }
        }

        /// <summary>
        /// C�digo de Transmiss�o para gera��o de remessa
        /// </summary>
        public string CodigoTransmissao
        {
            get { return _codigoTransmissao; }
            set { _codigoTransmissao = value; }
        }

        /// <summary>
        /// N�mero bordero do cliente
        /// </summary>
        public int NumeroBordero
        {
            get { return _numeroBordero; }
            set { _numeroBordero = value; }
        }

        /// <summary>
        /// N�mero da Carteira
        /// </summary>
        public string Carteira
        {
            get { return _carteira; }
            set { _carteira = value; }
        }

        public Endereco Endereco
        {
            get { return _endereco; }
            set { _endereco = value;}
        }

        public IList<IInstrucao> Instrucoes
        {
            get { return _instrucoes; }
            set { _instrucoes = value; }
        }

        public bool MostrarCnpjNoBoleto
        {
            get { return _mostrarCnpjNoBoleto; }
            set { _mostrarCnpjNoBoleto = value;}
        }

        #endregion
    }
}
