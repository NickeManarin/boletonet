using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_BancoBrasil
    {
        Protestar = 9,                      // Emite aviso ao sacado após N dias do vencto, e envia ao cartório após 5 dias úteis
        NaoProtestar = 10,                  // Inibe protesto, quando houver instrução permanente na conta corrente
        ImportanciaporDiaDesconto = 30,
        Multa = 35,
        ProtestoFinsFalimentares = 42,
        ProtestarAposNDiasCorridos = 81,
        ProtestarAposNDiasUteis = 82,
        NaoReceberAposNDias = 91,
        DevolverAposNDias = 92,
        JurosdeMora = 998,
        DescontoporDia = 999,
    }

    #endregion

    public sealed class Instrucao_BancoBrasil : AbstractInstrucao
    {
        #region Construtores

        public Instrucao_BancoBrasil()
        {
            try
            {
                Banco = new Banco(001);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public Instrucao_BancoBrasil(int cod, string descricao = null, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual)
        {
            Carrega(cod, descricao, dias, valor, tipo);
        }

        #endregion

        #region Métodos

        public override void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual)
        {
            try
            {
                Banco = new Banco_Brasil();

                Codigo = cod;
                Dias = dias;
                Valor = valor;
                Tipo = tipo;

                Valida();

                switch ((EnumInstrucoes_BancoBrasil)cod)
                {
                    case EnumInstrucoes_BancoBrasil.Protestar:
                        Descricao = "Protestar após " + dias + " dias úteis.";
                        break;
                    case EnumInstrucoes_BancoBrasil.NaoProtestar:
                        Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_BancoBrasil.ImportanciaporDiaDesconto:
                        Descricao = "Importância por dia de desconto.";
                        break;
                    case EnumInstrucoes_BancoBrasil.ProtestoFinsFalimentares:
                        Descricao = "Protesto para fins falimentares";
                        break;
                    case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasCorridos:
                        Descricao = "Protestar no " + dias + "º dia corrido após vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasUteis:
                        Descricao = "Protestar no " + dias + "º dia útil após vencimento";
                        /*Jéferson (jefhtavares) em 02/12/2013 a pedido do setor de homologação do BB*/
                        break;
                    case EnumInstrucoes_BancoBrasil.NaoReceberAposNDias:
                        Descricao = "Não receber após " + dias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.DevolverAposNDias:
                        Descricao = "Devolver após " + dias + " dias do vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.DescontoporDia:
                        Descricao = string.Format("Conceder desconto de {0} {1} por dia de antecipação",
                            tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                        break;
                    case EnumInstrucoes_BancoBrasil.Multa:
                        Descricao = string.Format("Após vencimento cobrar multa de {0} {1}",
                            tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                        break;
                    case EnumInstrucoes_BancoBrasil.JurosdeMora:
                        Descricao = string.Format("Após vencimento cobrar juros de {0} {1} por dia de atraso",
                            tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
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
