using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Banrisul
    {
        NaoDispensarComissaoPermanencia = 1, //01 - Não dispensar comissão de permanência
        NaoCobrarComissaoPermanencia = 8,    //08 - Não cobrar comissão de permanência 
        Protestar = 9,                       //09 - Protestar caso impago NN dias após vencimento (posições 370-371 = NN). Obs.: O número de dias para protesto deverá ser igual ou maior do que '03'. 
        DevolverAposNDias = 15,              //15 - Devolver se impago após NN dias do vencimento (posições 370-371 = NN). Obs.: Para o número de dias igual a '00' será impresso no bloqueto: 'NÃO RECEBER APÓS O VENCIMENTO'.
        CobrarMultaAposNDias = 18,           //18 - Após NN dias do vencimento, cobrar xx,x% de multa. 
        CobrarMultaOuFracaoAposNDias = 20,   //20 - Após NN dias do vencimento, cobrar xx,x% de multa ao mês ou fração. 
        NaoProtestar = 23,                   //23 - Não protestar.
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

        public Instrucao_Banrisul(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorTotal = 0m)
        {
            Carrega(cod, descricao, dias, valor, valorTotal);
        }
        
        #endregion

        #region Métodos

        public override void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorTotal = 0m, DateTime? data = null)
        {
            try
            {
                Banco = new Banco_Banrisul();

                Codigo = cod;
                Descricao = descricao;
                Dias = dias;
                Valor = valor;
                Tipo = EnumTipoValor.Percentual;

                Valida();

                switch ((EnumInstrucoes_Banrisul)cod)
                {
                    case EnumInstrucoes_Banrisul.NaoDispensarComissaoPermanencia:
                        Descricao = "Não dispensar comissão de permanência"; //01
                        break;
                    case EnumInstrucoes_Banrisul.NaoCobrarComissaoPermanencia:
                        Descricao = "Não cobrar comissão de permanência"; //08
                        break;
                    case EnumInstrucoes_Banrisul.Protestar:
                        Descricao = "Protestar caso não pago até " + dias + " dias após vencimento"; //09
                        break;
                    case EnumInstrucoes_Banrisul.DevolverAposNDias:
                        Descricao = "Devolver se não pago após " + dias + " dias do vencimento"; //15
                        break;
                    case EnumInstrucoes_Banrisul.CobrarMultaAposNDias:
                        Descricao = "Após " + dias + " dias do vencimento, cobrar " + valor + "% de multa"; //18
                        break;
                    case EnumInstrucoes_Banrisul.CobrarMultaOuFracaoAposNDias:
                        Descricao = "Após " + dias + " dias do vencimento, cobrar " + valor + "% de multa ao mês ou fração"; //20
                        break;
                    case EnumInstrucoes_Banrisul.NaoProtestar:
                        Descricao = "Não protestar"; //23
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