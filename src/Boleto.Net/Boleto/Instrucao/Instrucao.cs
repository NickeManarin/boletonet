using System;

namespace BoletoNet
{
    public sealed class Instrucao : AbstractInstrucao
    {
        #region Variaveis

        private IInstrucao _interface;

        #endregion

        #region Construtores

        public Instrucao(int banco, int cod = 0, string descricao = null, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual)
        {
            Bancos(banco, cod, descricao, dias, valor, tipo);         
        }

        #endregion

        #region Métodos Privados

        private void Bancos(int codigoBanco, int cod = 0, string descricao = null, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual)
        {
            try
            {
                switch (codigoBanco)
                {
                    //399 - HSBC
                    case 399:
                        _interface = new Instrucao_HSBC();
                        break;
                    //104 - Caixa
                    case 104:
                        _interface = new Instrucao_Caixa(cod, descricao, dias, valor, tipo);
                        break;
                    //341 - Itaú
                    case 341:
                        _interface = new Instrucao_Itau();
                        break;
                    //1 - Banco do Brasil
                    case 1:
                        _interface = new Instrucao_BancoBrasil(cod, descricao, dias, valor, tipo);
                        break;
                    //356 - Real
                    case 356:
                        _interface = new Instrucao_Real();
                        break;
                    //422 - Safra
                    case 422:
                        _interface = new Instrucao_Safra();
                        break;
                    //237 - Bradesco
                    case 237:
                        _interface = new Instrucao_Bradesco(cod, descricao, dias, valor, tipo);
                        break;
                    //347 - Sudameris
                    case 347:
                        _interface = new Instrucao_Sudameris();
                        break;
                    //353 - Santander
                    case 353:
                    case 33:
                    case 8:
                        //case 8:
                        _interface = new Instrucao_Santander();
                        break;
                    //070 - BRB
                    case 70:
                        _interface = new Instrucao_BRB();
                        break;
                    //479 - BankBoston
                    case 479:
                        _interface = new Instrucao_BankBoston();
                        break;
                    //41 - Banrisul
                    case 41:
                        _interface = new Instrucao_Banrisul(cod, descricao, dias, valor, tipo);
                        break;
                    //756 - Sicoob
                    case 756:
                        _interface = new Instrucao_Sicoob();
                        break;
                    //85 - CECRED
                    case 85:
                        _interface = new Instrucao_Cecred();
                        break;
                    //748 - Sicredi
                    case 748:
                        _interface = new Instrucao_Sicredi(cod, descricao, dias, valor, tipo);
                        break;
                    default:
                        throw new Exception("Código do banco não implementando: " + codigoBanco);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }

        #endregion

        #region Propriedades da interface

        public override IBanco Banco
        {
            get { return _interface.Banco; }
            set { _interface.Banco = value; }
        }

        public override int Codigo
        {
            get { return _interface.Codigo; }
            set { _interface.Codigo = value; }
        }

        public override string Descricao
        {
            get { return _interface.Descricao; }
            set { _interface.Descricao = value; }
        }

        public override int Dias
        {
            get { return _interface.Dias; }
            set { _interface.Dias = value; }
        }

        public override decimal Valor
        {
            get { return _interface.Valor; }
            set { _interface.Valor = value; }
        }

        public override EnumTipoValor Tipo
        {
            get { return _interface.Tipo; }
            set { _interface.Tipo = value; }
        }

        //public override IBanco Banco { get; set; }
        //public override int Codigo { get; set; }
        //public override string Descricao { get; set; }
        //public override int Dias { get; set; }
        //public override decimal Valor { get; set; }
        //public override EnumTipoValor Tipo { get; set; }

        #endregion

        #region Métodos de interface

        public override void Valida()
        {
            try
            {
                _interface.Valida();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a validação dos campos.", ex);
            }
        }

        public override void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual)
        {
            _interface.Carrega(cod, descricao, dias, valor, tipo);
        }

        #endregion
    }
}
