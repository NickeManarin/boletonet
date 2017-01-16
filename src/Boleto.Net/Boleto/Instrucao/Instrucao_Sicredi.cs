using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Sicredi
    {
        CadastroDeTitulo = 1,                      
        PedidoBaixa = 2,                  
        ConcessaoAbatimento = 4,
        CancelamentoAbatimentoConcedido = 5,
        AlteracaoVencimento = 6,
        PedidoProtesto = 9,
        SustarProtestoBaixarTitulo = 18,
        SustarProtestoManterCarteira = 19,
        AlteracaoOutrosDados = 31,
        AlteracaoOutrosDados_Desconto = 311,
        AlteracaoOutrosDados_JuroDia = 312,
        AlteracaoOutrosDados_DescontoAntecipacao = 313,
        AlteracaoOutrosDados_DataLimiteDesconto = 314,
        AlteracaoOutrosDados_CancelamentoProtestoAutomatico = 315,
        //AlteracaoOutrosDados_CarteiraDeCobranca = 316,  não disponivel...

        OutrasInstrucoes_ExibeMensagem_MoraDiaria = 900,
        OutrasInstrucoes_ExibeMensagem_MultaVencimento = 901
    }

    #endregion 

    public sealed class Instrucao_Sicredi: AbstractInstrucao
    {
        #region Construtores 

		public Instrucao_Sicredi()
		{
			try
			{
                Banco = new Banco(748);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public Instrucao_Sicredi(int cod, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual)
        {
            Carrega(cod, dias, valor, tipo);
        }

        #endregion

        #region Métodos

        public override void Carrega(int cod, int dias = 0, decimal valor = 0, EnumTipoValor tipo = EnumTipoValor.Percentual)
        {
            try
            {
                Banco = new Banco_Sicredi();
                Valida();

                Codigo = cod;
                Dias = dias;
                Valor = valor;
                Tipo = tipo;

                Valida();

                switch ((EnumInstrucoes_Sicredi)cod)
                {
                    case EnumInstrucoes_Sicredi.CadastroDeTitulo:
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.PedidoBaixa:
                        Descricao = "";
                        break;   
                    case EnumInstrucoes_Sicredi.ConcessaoAbatimento:
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.CancelamentoAbatimentoConcedido:
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.AlteracaoVencimento:
                        Codigo = (int)EnumInstrucoes_Sicredi.AlteracaoVencimento;
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.PedidoProtesto:
                        Descricao = "  - PROTESTAR APÓS " + dias + " DIAS ÚTEIS DO VENCIMENTO";
                        break;
                    case EnumInstrucoes_Sicredi.SustarProtestoBaixarTitulo:
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.SustarProtestoManterCarteira:
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.AlteracaoOutrosDados:
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.OutrasInstrucoes_ExibeMensagem_MoraDiaria:
                        Descricao = string.Format("  - APÓS VENCIMENTO COBRAR JUROS DE {0} {1} POR DIA DE ATRASO",
                            tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                        break;
                    case EnumInstrucoes_Sicredi.OutrasInstrucoes_ExibeMensagem_MultaVencimento:
                        Descricao = string.Format("  - APÓS VENCIMENTO COBRAR MULTA DE {0} {1}",
                            tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                        break;
                    case EnumInstrucoes_Sicredi.AlteracaoOutrosDados_Desconto:
                        Descricao = "  - CONCEDER DESCONTO DE R$ " + valor;
                        break;
                    case EnumInstrucoes_Sicredi.AlteracaoOutrosDados_DescontoAntecipacao:
                        Descricao = "  - CONCEDER DESCONTO DE R$ " + valor + "POR DIA DE ANTECIPAÇÃO";
                        break;
                    case EnumInstrucoes_Sicredi.AlteracaoOutrosDados_JuroDia:
                        Descricao = "  - APÓS VENCIMENTO COBRAR JURO DE " + valor + "% POR DIA DE ATRASO";
                        break;
                    default:
                        Codigo = 0;
                        Descricao = "(Selecione)";
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
