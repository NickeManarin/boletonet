using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Banrisul
    {
        NaoDispensarComissaoPermanencia = 1, //01 - N�o dispensar comiss�o de perman�ncia
        NaoCobrarComissaoPermanencia = 8,    //08 - N�o cobrar comiss�o de perman�ncia 
        Protestar = 9,                       //09 - Protestar caso impago NN dias ap�s vencimento (posi��es 370-371 = NN). Obs.: O n�mero de dias para protesto dever� ser igual ou maior do que '03'. 
        DevolverAposNDias = 15,              //15 - Devolver se impago ap�s NN dias do vencimento (posi��es 370-371 = NN). Obs.: Para o n�mero de dias igual a '00' ser� impresso no bloqueto: 'N�O RECEBER AP�S O VENCIMENTO'.
        CobrarMultaAposNDias = 18,           //18 - Ap�s NN dias do vencimento, cobrar xx,x% de multa. 
        CobrarMultaOuFracaoAposNDias = 20,   //20 - Ap�s NN dias do vencimento, cobrar xx,x% de multa ao m�s ou fra��o. 
        NaoProtestar = 23,                   //23 - N�o protestar.
    }

    #endregion

    public sealed class Instrucao_Banrisul : AbstractInstrucao
    {
        #region Construtores

        public Instrucao_Banrisul()
        {
            try
            {
                Banco = new Banco(41);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Banrisul(int cod, string descricao = null, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual)
        {
            Carrega(cod, descricao, dias, valor, tipo);
        }
        
        #endregion

        #region M�todos

        public override void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual)
        {
            try
            {
                Banco = new Banco_Banrisul();

                Codigo = cod;
                Dias = dias;
                Valor = valor;
                Tipo = tipo;

                Valida();

                switch ((EnumInstrucoes_Banrisul)cod)
                {
                    case EnumInstrucoes_Banrisul.NaoDispensarComissaoPermanencia:
                        Descricao = "N�o dispensar comiss�o de perman�ncia"; //01
                        break;
                    case EnumInstrucoes_Banrisul.NaoCobrarComissaoPermanencia:
                        Descricao = "N�o cobrar comiss�o de perman�ncia"; //08
                        break;
                    case EnumInstrucoes_Banrisul.Protestar:
                        Descricao = "Protestar caso n�o pago at� " + dias + " dias ap�s vencimento"; //09
                        break;
                    case EnumInstrucoes_Banrisul.DevolverAposNDias:
                        Descricao = "Devolver se n�o pago ap�s " + dias + " dias do vencimento"; //15
                        break;
                    case EnumInstrucoes_Banrisul.CobrarMultaAposNDias:
                        Descricao = "Ap�s " + dias + " dias do vencimento, cobrar " + valor + "% de multa"; //18
                        break;
                    case EnumInstrucoes_Banrisul.CobrarMultaOuFracaoAposNDias:
                        Descricao = "Ap�s " + dias + " dias do vencimento, cobrar " + valor + "% de multa ao m�s ou fra��o"; //20
                        break;
                    case EnumInstrucoes_Banrisul.NaoProtestar:
                        Descricao = "N�o protestar"; //23
                        break;
                    default:
                        Descricao = descricao;
                        Codigo = 0;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion
    }
}