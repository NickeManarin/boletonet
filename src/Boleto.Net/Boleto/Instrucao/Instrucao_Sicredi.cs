using System;
using System.Globalization;

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

    public sealed class Instrucao_Sicredi : AbstractInstrucao
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

        public Instrucao_Sicredi(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorBoleto = 0m)
        {
            Carrega(cod, descricao, dias, valor, valorBoleto);
        }

        #endregion

        #region Métodos

        public override void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0, decimal valorBoleto = 0, DateTime? data = null)
        {
            Banco = new Banco_Sicredi();

            Codigo = cod;
            Descricao = descricao;
            Dias = dias;
            Valor = valor;
            Tipo = EnumTipoValor.Percentual;

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
                    Descricao = "  - PROTESTAR APÓS " + dias + (dias > 4 ? " DIAS CORRIDOS DO VENCIMENTO" : " DIAS ÚTEIS DO VENCIMENTO");
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
                    Descricao = $"  - APÓS VENCIMENTO COBRAR JUROS DE R$ {decimal.Round((valor * valorBoleto) / 100m, 2, MidpointRounding.ToEven):F2} ({valor:F2} %) POR DIA DE ATRASO";
                    break;
                case EnumInstrucoes_Sicredi.OutrasInstrucoes_ExibeMensagem_MultaVencimento:
                    Descricao = $"  - APÓS VENCIMENTO COBRAR MULTA DE {valor:F2} %";
                    break;
                case EnumInstrucoes_Sicredi.AlteracaoOutrosDados_Desconto:
                    Descricao = "  - CONCEDER DESCONTO DE R$ " + valor;
                    break;
                case EnumInstrucoes_Sicredi.AlteracaoOutrosDados_DescontoAntecipacao:
                    Descricao = "  - CONCEDER DESCONTO DE R$ " + string.Format(new CultureInfo("pt-BR"), "{0:###,###.00}", valor) + "POR DIA DE ANTECIPAÇÃO";
                    break;
                case EnumInstrucoes_Sicredi.AlteracaoOutrosDados_JuroDia:
                    Descricao = $"  - APÓS VENCIMENTO COBRAR JUROS DE {valor:F2} % (R$ {decimal.Round((valor * valorBoleto) / 100m, 2, MidpointRounding.ToEven):F2}) POR DIA DE ATRASO";
                    break;
                default:
                    Descricao = descricao;
                    Codigo = 0;
                    break;
            }
        }

        #endregion
    }
}