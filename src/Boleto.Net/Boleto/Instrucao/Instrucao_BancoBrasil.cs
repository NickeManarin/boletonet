using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumInstrucoes_BancoBrasil
    {
        NaoProtestar = 7, //Inibe protesto, quando houver instrução permanente na conta corrente.
        Protestar3DiasUteis = 3,
        Protestar4DiasUteis = 4,
        Protestar5DiasUteis = 5,
        ProtestarNDiasCorridos = 6, //Com prazo de 6 a 29, 35 ou 40 dias Corridos.
        JurosdeMora = 998,
        Multa = 35,
        
        //Protestar = 9,  //Emite aviso ao sacado após N dias do vencto, e envia ao cartório após 5 dias úteis.
        //ImportanciaporDiaDesconto = 30,
        //ProtestoFinsFalimentares = 42,
        //ProtestarAposNDiasCorridos = 81,
        //ProtestarAposNDiasUteis = 82,
        //NaoReceberAposNDias = 91,
        //DevolverAposNDias = 92,
        //DescontoporDia = 999,
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

        public override void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual, DateTime? data = null)
        {
            try
            {
                Banco = new Banco_Brasil();

                Codigo = cod;
                Descricao = descricao;
                Dias = dias;
                Valor = valor;
                Tipo = tipo;

                Valida();

                switch ((EnumInstrucoes_BancoBrasil)cod)
                {
                    case EnumInstrucoes_BancoBrasil.NaoProtestar:
                        Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_BancoBrasil.Protestar3DiasUteis:
                        Descricao = "Protestar no 3º dia útil após vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.Protestar4DiasUteis:
                        Descricao = "Protestar no 4º dia útil após vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.Protestar5DiasUteis:
                        Descricao = "Protestar no 5º dia útil após vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.ProtestarNDiasCorridos:
                        Descricao = "Protestar no " + dias + "º dia corrido após vencimento";
                        break;
                    case EnumInstrucoes_BancoBrasil.JurosdeMora:
                        Descricao = string.Format("Após vencimento cobrar juros de {0} {1} por dia de atraso",
                            tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                        break;
                    case EnumInstrucoes_BancoBrasil.Multa:
                        Descricao = string.Format("Após vencimento cobrar multa de {0} {1}",
                            tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                            tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                        break;

                    //case EnumInstrucoes_BancoBrasil.Protestar:
                    //    Descricao = "Protestar após " + dias + " dias úteis.";
                    //    break;
                    //case EnumInstrucoes_BancoBrasil.ImportanciaporDiaDesconto:
                    //    Descricao = "Importância por dia de desconto.";
                    //    break;
                    //case EnumInstrucoes_BancoBrasil.ProtestoFinsFalimentares:
                    //    Descricao = "Protesto para fins falimentares";
                    //    break;
                    //case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasCorridos:
                    //    Descricao = "Protestar no " + dias + "º dia corrido após vencimento";
                    //    break;
                    //case EnumInstrucoes_BancoBrasil.ProtestarAposNDiasUteis:
                    //    Descricao = "Protestar no " + dias + "º dia útil após vencimento";
                    //    /*Jéferson (jefhtavares) em 02/12/2013 a pedido do setor de homologação do BB*/
                    //    break;
                    //case EnumInstrucoes_BancoBrasil.NaoReceberAposNDias:
                    //    Descricao = "Não receber após " + dias + " dias do vencimento";
                    //    break;
                    //case EnumInstrucoes_BancoBrasil.DevolverAposNDias:
                    //    Descricao = "Devolver após " + dias + " dias do vencimento";
                    //    break;
                    //case EnumInstrucoes_BancoBrasil.DescontoporDia:
                    //    Descricao = string.Format("Conceder desconto de {0} {1} por dia de antecipação",
                    //        tipo.Equals(EnumTipoValor.Reais) ? "R$ " : valor.ToString("F2"),
                    //        tipo.Equals(EnumTipoValor.Percentual) ? "%" : valor.ToString("F2"));
                    //    break;
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