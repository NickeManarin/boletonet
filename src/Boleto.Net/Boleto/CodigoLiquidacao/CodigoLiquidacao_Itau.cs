using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCodigoLiquidacao_Itau
    {
        CaixaEletronicoBancoItau = 1,
        PagamentoCartorioAutomatizado = 2,
        BancosCorrespondentes = 4,
        ItauBankFone = 5,
        ItauBankLine = 6,
        OB_RecebimentoOffline = 7,
        OB_PeloCodigoBarras = 8,
        OB_PelaLinhaDigitavel = 9,
        OB_PeloAutoAtendimento = 10,
        OB_RecebimentoCasaLoterica = 11,
        ComChequeOutroBanco = 12,
        Sispag = 13,
        DebitoContaCorrente = 14,
        CapturadoOffline = 15,
        PagamentoCartorioProtestoComCheque = 16,
        PagamentoAgendadoViaBankLine = 17,
    }

    #endregion 

    public class CodigoLiquidacao_Itau: AbstractCodigoLiquidacao, ICodigoLiquidacao
    {
        #region Construtores 

		public CodigoLiquidacao_Itau()
		{
			try
			{
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public CodigoLiquidacao_Itau(int codigo)
        {
            try
            {
                Carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

		#endregion 

        #region Metodos Privados

        private void Carregar(int idCodigo)
        {
            try
            {
                Banco = new BancoItau();

                switch ((EnumCodigoLiquidacao_Itau)idCodigo)
                {
                    case  EnumCodigoLiquidacao_Itau.CaixaEletronicoBancoItau:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.CaixaEletronicoBancoItau;
                        Codigo = "AA";
                        Descricao = "Caixa eletrônico do Banco Itaú.";
                        Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.PagamentoCartorioAutomatizado:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioAutomatizado;
                        Codigo = "AC";
                        Descricao = "Pagamento em cartório automatizado.";
                        Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.BancosCorrespondentes:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.BancosCorrespondentes;
                        Codigo = "BC";
                        Descricao = "Bancos correspondentes.";
                        Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.ItauBankFone:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankFone;
                        Codigo = "BF";
                        Descricao = "Itaú BankFone.";
                        Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.ItauBankLine:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankLine;
                        Codigo = "BL";
                        Descricao = "Itaú BankLine.";
                        Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.OB_RecebimentoOffline:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_RecebimentoOffline;
                        Codigo = "B0";
                        Descricao = "Outros bancos - Recebimento offline.";
                        Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.OB_PeloCodigoBarras:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PeloCodigoBarras;
                        Codigo = "B1";
                        Descricao = "Outros bancos - Pelo código de barras.";
                        Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.OB_PelaLinhaDigitavel:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PelaLinhaDigitavel;
                        Codigo = "B2";
                        Descricao = "Outros bancos - Pelo linha digitável.";
                        Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.OB_PeloAutoAtendimento:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PeloAutoAtendimento;
                        Codigo = "B3";
                        Descricao = "Outros bancos - Pelo auto-atendimento.";
                        Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.OB_RecebimentoCasaLoterica:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_RecebimentoCasaLoterica;
                        Codigo = "B4";
                        Descricao = "Outros bancos - Recebimento em casa lotérica.";
                        Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.ComChequeOutroBanco:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.ComChequeOutroBanco;
                        Codigo = "CC";
                        Descricao = "Agência Itaú - Com cheque de outro banco.";
                        Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.Sispag:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.Sispag;
                        Codigo = "CK";
                        Descricao = "SISPAG - Sistema de contas a pagar Itaú.";
                        Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.DebitoContaCorrente:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.DebitoContaCorrente;
                        Codigo = "CP";
                        Descricao = "Agência Itaú - Por débito em conta corrente, cheque ou dinheiro.";
                        Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.CapturadoOffline:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.CapturadoOffline;
                        Codigo = "DG";
                        Descricao = "Agência Itaú - Capturado offline.";
                        Recurso = "Disponível";
                        break;
                    case EnumCodigoLiquidacao_Itau.PagamentoCartorioProtestoComCheque:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioProtestoComCheque;
                        Codigo = "LC";
                        Descricao = "Pagamento em cartório de protesto com cheque.";
                        Recurso = "A Compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.PagamentoAgendadoViaBankLine:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoAgendadoViaBankLine;
                        Codigo = "Q0";
                        Descricao = "Agendamento - Pagamento agendado via BankLine ou outro canal eletrônico.";
                        Recurso = "Disponível";
                        break;
                    default:
                        Enumerado = 0;
                        Codigo = " ";
                        Descricao = "( Selecione )";
                        Recurso = "Sem Recurso";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void Ler(string Code)
        {
            try
            {
                switch (Code)
                {
                    case "AA":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.CaixaEletronicoBancoItau;
                        Descricao = "Caixa eletrônico do banco Itaú";
                        Codigo = "AA";
                        Recurso = "Disponível";
                        break;
                    case "AC":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioAutomatizado;
                        Descricao = "Pagamento em cartório automatizado";
                        Codigo = "AC";
                        Recurso = "A compensar";
                        break;
                    case "BC":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.BancosCorrespondentes;
                        Descricao = "Bancos correspondentes";
                        Codigo = "BC";
                        Recurso = "Disponível";
                        break;
                    case "BF":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankFone;
                        Descricao = "Itaú Bankfone";
                        Codigo = "BF";
                        Recurso = "Disponível";
                        break;
                    case "BL":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankLine;
                        Descricao = "Itaú Bankline";
                        Codigo = "BL";
                        Recurso = "Disponível";
                        break;
                    case "B0":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_RecebimentoOffline;
                        Descricao = "Outros bancos - recebimento offline";
                        Codigo = "B0";
                        Recurso = "A compensar";
                        break;
                    case "B1":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PeloCodigoBarras;
                        Descricao = "Outros bancos - pelo código de barras";
                        Codigo = "B1";
                        Recurso = "A compensar";
                        break;
                    case "B2":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PelaLinhaDigitavel;
                        Descricao = "Outros bancos - pelo linha digitável";
                        Codigo = "B2";
                        Recurso = "A compensar";
                        break;
                    case "B3":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PeloAutoAtendimento;
                        Descricao = "Outros bancos - pelo auto-atendimento";
                        Codigo = "B3";
                        Recurso = "A compensar";
                        break;
                    case "B4":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_RecebimentoCasaLoterica;
                        Descricao = "Outros bancos - recebimento em casa lotérica";
                        Codigo = "B4";
                        Recurso = "A compensar";
                        break;
                    case "CC":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.ComChequeOutroBanco;
                        Descricao = "Agêcnia Itaú - com cheque de outro banco";
                        Codigo = "CC";
                        Recurso = "A compensar";
                        break;
                    case "CK":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.Sispag;
                        Descricao = "SISPAG - Sistema de contas a pagar Itaú";
                        Codigo = "CK";
                        Recurso = "Disponível";
                        break;
                    case "CP":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.DebitoContaCorrente;
                        Descricao = "Agência Itaú - por débito em conta-corrente, cheque ou dinheiro";
                        Codigo = "CP";
                        Recurso = "Disponível";
                        break;
                    case "DG":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.CapturadoOffline;
                        Descricao = "Agência Itaú - capturado em offline";
                        Codigo = "DG";
                        Recurso = "Disponível";
                        break;
                    case "LC":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioProtestoComCheque;
                        Descricao = "Pagamento em cartório de protesto com cheque";
                        Codigo = "LC";
                        Recurso = "A compensar";
                        break;
                    case "Q0":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoAgendadoViaBankLine;
                        Descricao = "Agendamento - Pagamento agendado via BankLine ou outro canal eletrônico.";
                        Codigo = "Q0";
                        Recurso = "Disponível";
                        break;
                    default:
                        Enumerado = 0;
                        Descricao = " (Selecione) ";
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
