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

        public Instrucao_Bradesco(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorTotal = 0m, DateTime? data = null)
        {
            Carrega(cod, descricao, dias, valor, valorTotal, data);
        }

        #endregion Construtores

        #region Metodos Privados

        public override void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorTotal = 0m, DateTime? data = null)
        {
            try
            {
                Banco = new Banco_Bradesco();

                Codigo = cod;
                Descricao = descricao;
                Dias = dias;
                Valor = valor;
                Tipo = EnumTipoValor.Percentual;

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
                        Descricao = $"Após vencimento cobrar juros de R$ {decimal.Round((valor * valorTotal) / 100m, 2, MidpointRounding.ToEven):F2} ({valor:F2} %) por dia de atraso";
                        break;
                    case EnumInstrucoes_Bradesco.OutrasInstrucoes_ExibeMensagem_MultaVencimento:
                        Codigo = 0;
                        Descricao = $"Após vencimento cobrar multa de {valor:F2} %";
                        break;
                    case EnumInstrucoes_Bradesco.ComDesconto:
                        Descricao = string.Format($"Desconto de pontualidade no valor de {valor:F2} % se pago até " + data.Value.ToShortDateString());
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