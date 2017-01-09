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

    public class Instrucao_Sicredi: AbstractInstrucao, IInstrucao
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

        public Instrucao_Sicredi(int codigo)
        {
            carregar(codigo, 0);
        }

        public Instrucao_Sicredi(int codigo, int nrDias)
        {
            carregar(codigo, nrDias);
        }
        public Instrucao_Sicredi(int codigo, double valor)
        {
            carregar(codigo, valor);
        }

        public Instrucao_Sicredi(int codigo, double valor, EnumTipoValor tipoValor)
        {
            carregar(codigo, valor, tipoValor);
        }
        #endregion

        #region Metodos Privados

        private void carregar(int idInstrucao, double valor, EnumTipoValor tipoValor = EnumTipoValor.Percentual)
        {
            try
            {
                Banco = new Banco_Sicredi();
                Valida();

                switch ((EnumInstrucoes_Sicredi)idInstrucao)
                {
                    case EnumInstrucoes_Sicredi.OutrasInstrucoes_ExibeMensagem_MoraDiaria:
                        Codigo = (int)EnumInstrucoes_Sicredi.AlteracaoOutrosDados;
                        Descricao = string.Format("  - APÓS VENCIMENTO COBRAR JUROS DE {0} {1} POR DIA DE ATRASO",
                            tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                        break;
                    case EnumInstrucoes_Sicredi.OutrasInstrucoes_ExibeMensagem_MultaVencimento:
                        Codigo = (int)EnumInstrucoes_Sicredi.AlteracaoOutrosDados;
                        Descricao = string.Format("  - APÓS VENCIMENTO COBRAR MULTA DE {0} {1}",
                            tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                        break;
                    case EnumInstrucoes_Sicredi.AlteracaoOutrosDados_Desconto:
                        Codigo = (int)EnumInstrucoes_Sicredi.AlteracaoOutrosDados;
                        Descricao = "  - CONCEDER DESCONTO DE R$ " + valor;
                        break;
                    case EnumInstrucoes_Sicredi.AlteracaoOutrosDados_DescontoAntecipacao:
                        Codigo = (int)EnumInstrucoes_Sicredi.AlteracaoOutrosDados;
                        Descricao = "  - CONCEDER DESCONTO DE R$ " + valor + "POR DIA DE ANTECIPAÇÃO";
                        break;
                    case EnumInstrucoes_Sicredi.AlteracaoOutrosDados_JuroDia:
                        Codigo = (int)EnumInstrucoes_Sicredi.AlteracaoOutrosDados;
                        Descricao = "  - APÓS VENCIMENTO COBRAR JURO DE " + valor + "% POR DIA DE ATRASO";
                        break;
                    default:
                        Codigo = 0;
                        Descricao = " (Selecione) ";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void carregar(int idInstrucao, int nrDias)
        {
            try
            {
                Banco = new Banco_Sicredi();
                Valida();

                switch ((EnumInstrucoes_Sicredi)idInstrucao)
                {
                    case EnumInstrucoes_Sicredi.CadastroDeTitulo:
                        Codigo = (int)EnumInstrucoes_Sicredi.CadastroDeTitulo;
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.PedidoBaixa:
                        Codigo = (int)EnumInstrucoes_Sicredi.PedidoBaixa;
                        Descricao = "";
                        break;   
                    case EnumInstrucoes_Sicredi.ConcessaoAbatimento:
                        Codigo = (int)EnumInstrucoes_Sicredi.ConcessaoAbatimento;
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.CancelamentoAbatimentoConcedido:
                        Codigo = (int)EnumInstrucoes_Sicredi.CancelamentoAbatimentoConcedido;
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.AlteracaoVencimento:
                        Codigo = (int)EnumInstrucoes_Sicredi.AlteracaoVencimento;
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.PedidoProtesto:
                        Codigo = (int)EnumInstrucoes_Sicredi.PedidoProtesto;
                        Descricao = "  - PROTESTAR APÓS " + nrDias + " DIAS ÚTEIS DO VENCIMENTO";
                        break;
                    case EnumInstrucoes_Sicredi.SustarProtestoBaixarTitulo:
                        Codigo = (int)EnumInstrucoes_Sicredi.SustarProtestoBaixarTitulo;
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.SustarProtestoManterCarteira:
                        Codigo = (int)EnumInstrucoes_Sicredi.SustarProtestoManterCarteira;
                        Descricao = "";
                        break;
                    case EnumInstrucoes_Sicredi.AlteracaoOutrosDados:
                        Codigo = (int)EnumInstrucoes_Sicredi.AlteracaoOutrosDados;
                        Descricao = "";
                        break;
                    default:
                        Codigo = 0;
                        Descricao = " (Selecione) ";
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
