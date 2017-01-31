using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Caixa
    {
        Protestar = 9,                      // Emite aviso ao sacado após N dias do vencto, e envia ao cartório após 5 dias úteis
        NaoProtestar = 10,                  // Inibe protesto, quando houver instrução permanente na conta corrente
        ImportanciaporDiaDesconto = 30,
        ProtestoFinsFalimentares = 42,
        ProtestarAposNDiasCorridos = 81,
        ProtestarAposNDiasUteis = 82,
        NaoReceberAposNDias = 91,
        DevolverAposNDias = 92,
        JurosdeMora = 998,
        DescontoporDia = 999,
        Multa = 8
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

        public Instrucao_Caixa(int cod, string descricao = null, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual)
        {
            Carrega(cod, descricao, dias, valor, tipo);
        }

        #endregion

        #region Métodos

        public override void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual, DateTime? data = null)
        {
            try
            {
                Banco = new Banco_Caixa();

                Codigo = cod;
                Descricao = descricao;
                Dias = dias;
                Valor = valor;
                Tipo = tipo;

                Valida();

                switch ((EnumInstrucoes_Caixa)cod)
                {
                    case EnumInstrucoes_Caixa.Protestar:
                        Descricao = "Protestar após " + dias + " dias úteis.";
                        break;
                    case EnumInstrucoes_Caixa.NaoProtestar:
                        Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Caixa.ImportanciaporDiaDesconto:
                        Descricao = "Importância por dia de desconto.";
                        break;
                    case EnumInstrucoes_Caixa.ProtestoFinsFalimentares:
                        Descricao = "Protesto para fins falimentares";
                        break;
                    case EnumInstrucoes_Caixa.ProtestarAposNDiasCorridos:
                        Descricao = "Protestar após " + dias + " dias corridos do vencimento";
                        break;
                    case EnumInstrucoes_Caixa.ProtestarAposNDiasUteis:
                        Descricao = "Protestar após " + dias + " dias úteis do vencimento";
                        break;
                    case EnumInstrucoes_Caixa.NaoReceberAposNDias:
                        Descricao = "Não receber após " + dias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_Caixa.DevolverAposNDias:
                        Descricao = "Devolver após " + dias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_Caixa.JurosdeMora:
                        Descricao = string.Format("Após vencimento cobrar juros de {0} {1} por dia de atraso",
                            tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                        break;
                    case EnumInstrucoes_Caixa.Multa:
                        Descricao = string.Format("Após vencimento cobrar multa de {0} {1}",
                            tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
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
