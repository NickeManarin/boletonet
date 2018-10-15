using System;

namespace BoletoNet
{
    #region Enumerador

    public enum EnumEspecieDocumento_Caixa
    {
        DuplicataMercantil,
        NotaPromissoria,
        DuplicataServico,
        NotaSeguro,
        LetraCambio,
        Outros
    }

    #endregion

    public class EspecieDocumento_Caixa : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Caixa()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public EspecieDocumento_Caixa(string codigo)
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

        private string GetCodigoEspecieByEnum(EnumEspecieDocumento_Caixa especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Caixa.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Caixa.NotaPromissoria : return "2";
                case EnumEspecieDocumento_Caixa.DuplicataServico: return "3";
                case EnumEspecieDocumento_Caixa.NotaSeguro : return "5";
                case EnumEspecieDocumento_Caixa.LetraCambio : return "6";
                case EnumEspecieDocumento_Caixa.Outros: return "9";
                default: return "23";
            }
        }

        private EnumEspecieDocumento_Caixa GetEnumEspecieByCodigo(string codigo)
        {
            switch (codigo.TrimStart('0'))
            {
                case "1": return EnumEspecieDocumento_Caixa.DuplicataMercantil;
                case "2": return EnumEspecieDocumento_Caixa.NotaPromissoria;
                case "3": return EnumEspecieDocumento_Caixa.DuplicataServico;
                case "5": return EnumEspecieDocumento_Caixa.NotaSeguro;
                case "6": return EnumEspecieDocumento_Caixa.LetraCambio;
                case "9": return EnumEspecieDocumento_Caixa.Outros;
                default: return EnumEspecieDocumento_Caixa.DuplicataMercantil;
            }
        }

        private void Carregar(string idCodigo)
        {
            try
            {
                Banco = new Banco_Caixa();

                switch (GetEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Caixa.DuplicataMercantil:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.DuplicataMercantil);
                        Especie = "DUPLICATA MERCANTIL";
                        Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Caixa.NotaPromissoria:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.NotaPromissoria);
                        Especie = "NOTA PROMISSORIA";
                        Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Caixa.DuplicataServico:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.DuplicataServico);
                        Especie = "DUPLICATA DE PRESTACAO DE SERVICOS";
                        Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Caixa.NotaSeguro:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.NotaSeguro);
                        Especie = "NOTA DE SEGURO";
                        Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Caixa.LetraCambio:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.LetraCambio);
                        Especie = "LETRA DE CAMBIO";
                        Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_Caixa.Outros:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.Outros);
                        Especie = "OUTROS";
                        Sigla = "OU";
                        break;
                    default:
                        Codigo = "0";
                        Especie = "( Selecione )";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public static EspeciesDocumento CarregaTodas()
        {
            var especiesDocumento = new EspeciesDocumento();
            var ed = new EspecieDocumento_Caixa();

            foreach (EnumEspecieDocumento_Caixa item in Enum.GetValues(typeof(EnumEspecieDocumento_Caixa)))
                especiesDocumento.Add(new EspecieDocumento_Caixa(ed.GetCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Caixa(GetCodigoEspecieByEnum(EnumEspecieDocumento_Caixa.DuplicataMercantil));
        }

        #endregion
    }
}