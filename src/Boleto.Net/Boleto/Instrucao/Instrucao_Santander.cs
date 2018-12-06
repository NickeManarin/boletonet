using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_Santander
    {
        BaixarApos15Dias = 02,
        BaixarApos30Dias = 03,
        NaoBaixar = 04,
        Protestar = 06,
        NaoProtestar = 07,
        NaoCobrarJurosDeMora = 08,
        JurosAoDia = 98,
        MultaVencimento = 99,        
    }

    #endregion

    public sealed class Instrucao_Santander : AbstractInstrucao, IInstrucao
    {
        #region Construtores

        public Instrucao_Santander()
        {
            try
            {
                Banco = new Banco(33);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_Santander(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorBoleto = 0m)
        {
            Carrega(cod, descricao, dias, valor, valorBoleto);
        }

        #endregion

        #region Metodos Privados

        public override void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0, decimal valorBoleto = 0, DateTime? data = null)
        {
            try
            {
                Codigo = cod;
                Descricao = descricao;
                Dias = dias;
                Valor = valor;
                Tipo = EnumTipoValor.Percentual;
                Banco = new Banco_Santander();

                switch ((EnumInstrucoes_Santander)cod)
                {
                    case EnumInstrucoes_Santander.BaixarApos15Dias:
                        Descricao = "Baixar após quinze dias do vencimento";
                        break;
                    case EnumInstrucoes_Santander.BaixarApos30Dias:
                        Descricao = "Baixar após 30 dias do vencimento";
                        break;
                    case EnumInstrucoes_Santander.NaoBaixar:
                        Descricao = "Não baixar";
                        break;
                    case EnumInstrucoes_Santander.Protestar:
                        Descricao = $"Protestar após {cod} dias do vencimento";
                        Dias = dias;
                        break;
                    case EnumInstrucoes_Santander.NaoProtestar:
                        Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Santander.NaoCobrarJurosDeMora:
                        Descricao = "Não cobrar juros de mora";
                        break;
                    case EnumInstrucoes_Santander.JurosAoDia:
                        Descricao = $"Após vencimento cobrar juros de R$ {decimal.Round((valor * valorBoleto) / 100m, 2, MidpointRounding.ToEven):F2} ({valor:F2} %) por dia de atraso";

                        //Descricao = string.Format("Após vencimento cobrar juros de {0} {1} por dia de atraso",
                        //    (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                        //    (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                        break;
                    case EnumInstrucoes_Santander.MultaVencimento:
                        Descricao = $"Após vencimento cobrar multa de {valor:F2} %";

                        //Descricao = String.Format("Após vencimento cobrar multa de {0} {1}",
                        //    (tipoValor.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2")),
                        //    (tipoValor.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2")));
                        break;
                    default:
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