using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Itau
    {
        Protestar = 9,                      //Emite aviso ao sacado após N dias do vencto, e envia ao cartório após 5 dias úteis.
        NaoProtestar = 10,                  //Inibe protesto, quando houver instrução permanente na conta corrente.
        ImportanciaporDiaDesconto = 30,
        ProtestoFinsFalimentares = 42,
        ProtestarAposNDiasCorridos = 34,
        ProtestarAposNDiasUteis = 35,
        NaoReceberAposNDias = 91,
        DevolverAposNDias = 92,
        MultaVencimento = 997,
        JurosdeMora = 998,
        DescontoporDia = 999,
    }

    #endregion 

    public sealed class InstrucaoItau: AbstractInstrucao, IInstrucao
    {
        #region Construtores 

		public InstrucaoItau()
		{
			try
			{
                Banco = new Banco(341);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public InstrucaoItau(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorBoleto = 0m)
        {
            Carrega(cod, descricao, dias, valor, valorBoleto);
        }

        #endregion

        #region Metodos Privados

        private void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorBoleto = 0m)
        {
            try
            {
                Banco = new BancoItau();

                Codigo = cod;
                Descricao = descricao;
                Dias = dias;
                Valor = valor;
                Tipo = EnumTipoValor.Percentual;

                Valida();

                switch ((EnumInstrucoes_Itau)cod)
                {
                    //case EnumInstrucoes_Itau.Protestar:
                    //    Descricao = "Protestar após 5 dias úteis.";
                    //    break;
                    case EnumInstrucoes_Itau.NaoProtestar:
                        Descricao = "Não protestar.";
                        break;
                    //case EnumInstrucoes_Itau.ImportanciaporDiaDesconto:
                    //    Descricao = "Importância por dia de desconto.";
                    //    break;
                    //case EnumInstrucoes_Itau.ProtestoFinsFalimentares:
                    //    Descricao = "Protesto para fins falimentares.";
                    //    break;
                    case EnumInstrucoes_Itau.ProtestarAposNDiasCorridos:
                        Descricao = $"Protestar após {dias} dias corridos do vencimento.";
                        break;
                    case EnumInstrucoes_Itau.ProtestarAposNDiasUteis:
                        Descricao = $"Protestar após {dias} dias úteis do vencimento.";
                        break;
                    //case EnumInstrucoes_Itau.NaoReceberAposNDias:
                    //    Descricao = "Não receber após N dias do vencimento.";
                    //    break;
                    //case EnumInstrucoes_Itau.DevolverAposNDias:
                    //    Descricao = "Devolver após N dias do vencimento.";
                    //    break;                  
                    //case EnumInstrucoes_Itau.DescontoporDia:
                    //    Descricao = "Conceder desconto de R$ "; //por dia de antecipação
                    //    break;
                    case EnumInstrucoes_Itau.JurosdeMora:
                        Codigo = 0;
                        Descricao = $"Após vencimento cobrar juros de R$ {decimal.Round((valor * valorBoleto) / 100m, 2, MidpointRounding.ToEven):F2} ({valor:F2} %) por dia de atraso.";
                        break;
                    case EnumInstrucoes_Itau.MultaVencimento:
                        Codigo = 0;
                        Descricao = $"Após vencimento cobrar multa de R$ {decimal.Round((valor * valorBoleto) / 100m, 2, MidpointRounding.ToEven):F2} ({valor:F2} %).";
                        break;
                    default:
                        Codigo = 0;
                        Descricao = descricao;
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
