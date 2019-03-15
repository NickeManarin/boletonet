using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Caixa
    {
        Multa = 8,
        Protestar = 9,                      // Emite aviso ao sacado após N dias do vencto, e envia ao cartório após 5 dias úteis
        //NaoProtestar = 10,                  // Inibe protesto, quando houver instrução permanente na conta corrente
        //ImportanciaporDiaDesconto = 30,
        //ProtestoFinsFalimentares = 42,
        //ProtestarAposNDiasCorridos = 81,
        //ProtestarAposNDiasUteis = 82,
        //NaoReceberAposNDias = 91,
        DevolverAposNDias = 92,
        JurosdeMora = 998,
        DescontoporDia = 999,
    }

    #endregion

    public sealed class Instrucao_Caixa : AbstractInstrucao
    {
        #region Construtores

        public Instrucao_Caixa()
        {
            try
            {
                Banco = new Banco(104);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Caixa(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorTotal = 0m)
        {
            Carrega(cod, descricao, dias, valor, valorTotal);
        }

        #endregion

        #region Métodos

        public override void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorTotal = 0m, DateTime? data = null)
        {
            try
            {
                Banco = new BancoCaixa();

                Codigo = cod;
                Descricao = descricao;
                Dias = dias;
                Valor = valor;
                Tipo = EnumTipoValor.Percentual;

                Valida();

                switch ((EnumInstrucoes_Caixa)cod)
                {
                    case EnumInstrucoes_Caixa.Protestar:
                        //De 02 a 05 = dias úteis, Acima de 05 = dias corridos
                        Descricao = $"Protestar após {dias} dias {(dias > 5 ? "corridos" : "úteis")}.";
                        break;
                    //case EnumInstrucoes_Caixa.NaoProtestar:
                    //    Descricao = "Não protestar";
                    //    break;
                    //case EnumInstrucoes_Caixa.ImportanciaporDiaDesconto:
                    //    Descricao = "Importância por dia de desconto.";
                    //    break;
                    //case EnumInstrucoes_Caixa.ProtestoFinsFalimentares:
                    //    Descricao = "Protesto para fins falimentares";
                    //    break;
                    //case EnumInstrucoes_Caixa.ProtestarAposNDiasCorridos:
                    //    Descricao = "Protestar após " + dias + " dias corridos do vencimento";
                    //    break;
                    //case EnumInstrucoes_Caixa.ProtestarAposNDiasUteis:
                    //    Descricao = "Protestar após " + dias + " dias úteis do vencimento";
                    //    break;
                    //case EnumInstrucoes_Caixa.NaoReceberAposNDias:
                    //    Descricao = "Não receber após " + dias + " dias do vencimento";
                    //    break;
                    case EnumInstrucoes_Caixa.DevolverAposNDias:
                        Descricao = "Devolver após " + dias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_Caixa.JurosdeMora:
                        Descricao = $"Após vencimento cobrar juros de R$ {decimal.Round((valor * valorTotal) / 100m, 2, MidpointRounding.ToEven):F2} ({valor:F2} %) por dia de atraso";
                        break;
                    case EnumInstrucoes_Caixa.Multa:
                        Descricao = $"Após vencimento cobrar multa de {valor:F2} %";
                        break;
                    case EnumInstrucoes_Caixa.DescontoporDia:
                        Descricao = "Conceder desconto de " + valor + "%" + " por dia de antecipação";
                        break;
                    default:
                        Descricao = descricao;
                        Codigo = 0;
                        break;
                }

                Dias = dias;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion
    }
}