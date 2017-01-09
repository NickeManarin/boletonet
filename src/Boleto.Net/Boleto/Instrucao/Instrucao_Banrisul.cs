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

    public class Instrucao_Banrisul : AbstractInstrucao, IInstrucao
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

        public Instrucao_Banrisul(int codigo)
        {
            carregar(codigo, 0, 0);
        }

        public Instrucao_Banrisul(int codigo, int nrDias)
        {
            carregar(codigo, nrDias, (double)0.0);
        }

        public Instrucao_Banrisul(int codigo, double percentualMultaDia)
        {
            carregar(codigo, 0, percentualMultaDia);
        }

        public Instrucao_Banrisul(int codigo, int nrDias, double percentualMultaDia)
        {
            carregar(codigo, nrDias, percentualMultaDia);
        }

        #endregion

        #region Metodos Privados

        private void carregar(int idInstrucao, int nrDias, double percentualMultaDia)
        {
            try
            {
                Banco = new Banco_Banrisul();
                Valida();

                switch ((EnumInstrucoes_Banrisul)idInstrucao)
                {
                    case EnumInstrucoes_Banrisul.NaoDispensarComissaoPermanencia:
                        Codigo = (int)EnumInstrucoes_Banrisul.NaoDispensarComissaoPermanencia;
                        Descricao = "N�o dispensar comiss�o de perman�ncia"; //01
                        break;
                    case EnumInstrucoes_Banrisul.NaoCobrarComissaoPermanencia:
                        Codigo = (int)EnumInstrucoes_Banrisul.NaoCobrarComissaoPermanencia;
                        Descricao = "N�o cobrar comiss�o de perman�ncia"; //08
                        break;
                    case EnumInstrucoes_Banrisul.Protestar:
                        Codigo = (int)EnumInstrucoes_Banrisul.Protestar;
                        Descricao = "Protestar caso impago " + nrDias + " dias ap�s vencimento"; //09
                        break;
                    case EnumInstrucoes_Banrisul.DevolverAposNDias:
                        Codigo = (int)EnumInstrucoes_Banrisul.DevolverAposNDias;
                        Descricao = "Devolver se impago ap�s " + nrDias + " dias do vencimento"; //15
                        break;
                    case EnumInstrucoes_Banrisul.CobrarMultaAposNDias:
                        Codigo = (int)EnumInstrucoes_Banrisul.CobrarMultaAposNDias;
                        Descricao = "Ap�s " + nrDias + " dias do vencimento, cobrar " + percentualMultaDia + "% de multa"; //18
                        break;
                    case EnumInstrucoes_Banrisul.CobrarMultaOuFracaoAposNDias:
                        Codigo = (int)EnumInstrucoes_Banrisul.CobrarMultaOuFracaoAposNDias;
                        Descricao = "Ap�s " + nrDias + " dias do vencimento, cobrar " + percentualMultaDia + "% de multa ao m�s ou fra��o"; //20
                        break;
                    case EnumInstrucoes_Banrisul.NaoProtestar:
                        Codigo = (int)EnumInstrucoes_Banrisul.NaoProtestar;
                        Descricao = "N�o protestar"; //23
                        break;
                    default:
                        Codigo = 0;
                        Descricao = "( Selecione )";
                        break;
                }

                QuantidadeDias = nrDias;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public override void Valida()
        {
            //base.Valida();
        }

        #endregion
    }
}