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
                        Descricao = "Caixa eletr�nico do Banco Ita�.";
                        Recurso = "Dispon�vel";
                        break;
                    case EnumCodigoLiquidacao_Itau.PagamentoCartorioAutomatizado:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioAutomatizado;
                        Codigo = "AC";
                        Descricao = "Pagamento em cart�rio automatizado.";
                        Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.BancosCorrespondentes:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.BancosCorrespondentes;
                        Codigo = "BC";
                        Descricao = "Bancos correspondentes.";
                        Recurso = "Dispon�vel";
                        break;
                    case EnumCodigoLiquidacao_Itau.ItauBankFone:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankFone;
                        Codigo = "BF";
                        Descricao = "Ita� BankFone.";
                        Recurso = "Dispon�vel";
                        break;
                    case EnumCodigoLiquidacao_Itau.ItauBankLine:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankLine;
                        Codigo = "BL";
                        Descricao = "Ita� BankLine.";
                        Recurso = "Dispon�vel";
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
                        Descricao = "Outros bancos - Pelo c�digo de barras.";
                        Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.OB_PelaLinhaDigitavel:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PelaLinhaDigitavel;
                        Codigo = "B2";
                        Descricao = "Outros bancos - Pelo linha digit�vel.";
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
                        Descricao = "Outros bancos - Recebimento em casa lot�rica.";
                        Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.ComChequeOutroBanco:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.ComChequeOutroBanco;
                        Codigo = "CC";
                        Descricao = "Ag�ncia Ita� - Com cheque de outro banco.";
                        Recurso = "A compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.Sispag:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.Sispag;
                        Codigo = "CK";
                        Descricao = "SISPAG - Sistema de contas a pagar Ita�.";
                        Recurso = "Dispon�vel";
                        break;
                    case EnumCodigoLiquidacao_Itau.DebitoContaCorrente:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.DebitoContaCorrente;
                        Codigo = "CP";
                        Descricao = "Ag�ncia Ita� - Por d�bito em conta corrente, cheque ou dinheiro.";
                        Recurso = "Dispon�vel";
                        break;
                    case EnumCodigoLiquidacao_Itau.CapturadoOffline:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.CapturadoOffline;
                        Codigo = "DG";
                        Descricao = "Ag�ncia Ita� - Capturado offline.";
                        Recurso = "Dispon�vel";
                        break;
                    case EnumCodigoLiquidacao_Itau.PagamentoCartorioProtestoComCheque:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioProtestoComCheque;
                        Codigo = "LC";
                        Descricao = "Pagamento em cart�rio de protesto com cheque.";
                        Recurso = "A Compensar";
                        break;
                    case EnumCodigoLiquidacao_Itau.PagamentoAgendadoViaBankLine:
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoAgendadoViaBankLine;
                        Codigo = "Q0";
                        Descricao = "Agendamento - Pagamento agendado via BankLine ou outro canal eletr�nico.";
                        Recurso = "Dispon�vel";
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
                        Descricao = "Caixa eletr�nico do banco Ita�";
                        Codigo = "AA";
                        Recurso = "Dispon�vel";
                        break;
                    case "AC":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioAutomatizado;
                        Descricao = "Pagamento em cart�rio automatizado";
                        Codigo = "AC";
                        Recurso = "A compensar";
                        break;
                    case "BC":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.BancosCorrespondentes;
                        Descricao = "Bancos correspondentes";
                        Codigo = "BC";
                        Recurso = "Dispon�vel";
                        break;
                    case "BF":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankFone;
                        Descricao = "Ita� Bankfone";
                        Codigo = "BF";
                        Recurso = "Dispon�vel";
                        break;
                    case "BL":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.ItauBankLine;
                        Descricao = "Ita� Bankline";
                        Codigo = "BL";
                        Recurso = "Dispon�vel";
                        break;
                    case "B0":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_RecebimentoOffline;
                        Descricao = "Outros bancos - recebimento offline";
                        Codigo = "B0";
                        Recurso = "A compensar";
                        break;
                    case "B1":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PeloCodigoBarras;
                        Descricao = "Outros bancos - pelo c�digo de barras";
                        Codigo = "B1";
                        Recurso = "A compensar";
                        break;
                    case "B2":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.OB_PelaLinhaDigitavel;
                        Descricao = "Outros bancos - pelo linha digit�vel";
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
                        Descricao = "Outros bancos - recebimento em casa lot�rica";
                        Codigo = "B4";
                        Recurso = "A compensar";
                        break;
                    case "CC":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.ComChequeOutroBanco;
                        Descricao = "Ag�cnia Ita� - com cheque de outro banco";
                        Codigo = "CC";
                        Recurso = "A compensar";
                        break;
                    case "CK":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.Sispag;
                        Descricao = "SISPAG - Sistema de contas a pagar Ita�";
                        Codigo = "CK";
                        Recurso = "Dispon�vel";
                        break;
                    case "CP":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.DebitoContaCorrente;
                        Descricao = "Ag�ncia Ita� - por d�bito em conta-corrente, cheque ou dinheiro";
                        Codigo = "CP";
                        Recurso = "Dispon�vel";
                        break;
                    case "DG":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.CapturadoOffline;
                        Descricao = "Ag�ncia Ita� - capturado em offline";
                        Codigo = "DG";
                        Recurso = "Dispon�vel";
                        break;
                    case "LC":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoCartorioProtestoComCheque;
                        Descricao = "Pagamento em cart�rio de protesto com cheque";
                        Codigo = "LC";
                        Recurso = "A compensar";
                        break;
                    case "Q0":
                        Enumerado = (int)EnumCodigoLiquidacao_Itau.PagamentoAgendadoViaBankLine;
                        Descricao = "Agendamento - Pagamento agendado via BankLine ou outro canal eletr�nico.";
                        Codigo = "Q0";
                        Recurso = "Dispon�vel";
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
