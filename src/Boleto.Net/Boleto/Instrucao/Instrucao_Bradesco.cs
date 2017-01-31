using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Bradesco
    {
        Protestar = 9,
        NaoProtestar = 10,
        ProtestoFinsFalimentares = 42,
        ProtestarAposNDiasCorridos = 81,
        ProtestarAposNDiasUteis = 82,
        NaoReceberAposNDias = 91,
        DevolverAposNDias = 92,
        ComDesconto = 93,
        BoletoOriginal = 94,

        OutrasInstrucoes_ExibeMensagem_MoraDiaria = 900,
        OutrasInstrucoes_ExibeMensagem_MultaVencimento = 901
    }

    #endregion 

    public sealed class Instrucao_Bradesco : AbstractInstrucao
    {
        #region Construtores 

		public Instrucao_Bradesco()
		{
			try
			{
                Banco = new Banco(237);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public Instrucao_Bradesco(int cod, string descricao = null, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual, DateTime? data = null)
        {
            Carrega(cod, descricao, dias, valor, tipo, data);
        }

        #endregion Construtores

        #region Metodos Privados

        public override void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual, DateTime? data = null)
        {
            try
            {
                Banco = new Banco_Bradesco();

                Codigo = cod;
                Descricao = descricao;
                Dias = dias;
                Valor = valor;
                Tipo = tipo;

                Valida();

                switch ((EnumInstrucoes_Bradesco)cod)
                {
                    case EnumInstrucoes_Bradesco.Protestar:
                        Descricao = "Protestar";
                        break;
                    case EnumInstrucoes_Bradesco.NaoProtestar:
                        Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Bradesco.ProtestoFinsFalimentares:
                        Descricao = "Protesto para fins falimentares";
                        break;
                    case EnumInstrucoes_Bradesco.ProtestarAposNDiasCorridos:
                        Descricao = "Protestar após " + dias + " dias corridos do vencimento";
                        break;
                    case EnumInstrucoes_Bradesco.ProtestarAposNDiasUteis:
                        Descricao = "Protestar após " + dias + " dias úteis do vencimento";
                        break;
                    case EnumInstrucoes_Bradesco.NaoReceberAposNDias:
                        Descricao = "Não receber após " + dias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_Bradesco.DevolverAposNDias:
                        Descricao = "Devolver após " + dias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_Bradesco.OutrasInstrucoes_ExibeMensagem_MoraDiaria:
                        Codigo = 0;
                        Descricao = string.Format("Após vencimento cobrar juros de {0} {1} por dia de atraso",
                            tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                        break;
                    case EnumInstrucoes_Bradesco.OutrasInstrucoes_ExibeMensagem_MultaVencimento:
                        Codigo = 0;
                        Descricao = string.Format("Após vencimento cobrar multa de {0} {1}",
                            tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                        break;
                    case EnumInstrucoes_Bradesco.ComDesconto:
                        Descricao = string.Format("Desconto de pontualidade no valor de {0} {1} se pago até " + data.Value.ToShortDateString(),
                            tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("C"),
                            tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                        break;
                    case EnumInstrucoes_Bradesco.BoletoOriginal:
                        Descricao = "Vencimento " + data.Value.ToShortDateString() + ", no valor de " + valor.ToString("C") + "";
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
