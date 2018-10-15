using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Bradesco
    {
        DuplicataMercantil,
        NotaPromissoria,
        NotaSeguro,
        CobrancaSeriada,
        Recibo,
        LetraCambio,
        NotaDebito,
        DuplicataServico,
        Outros,
    }

    #endregion

    public class EspecieDocumento_Bradesco : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Bradesco()
        {}

        public EspecieDocumento_Bradesco(string codigo)
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

        private string GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Bradesco.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Bradesco.NotaPromissoria: return "2";
                case EnumEspecieDocumento_Bradesco.NotaSeguro: return "3";
                case EnumEspecieDocumento_Bradesco.CobrancaSeriada: return "4";
                case EnumEspecieDocumento_Bradesco.Recibo: return "5";
                case EnumEspecieDocumento_Bradesco.LetraCambio: return "10";
                case EnumEspecieDocumento_Bradesco.NotaDebito: return "11";
                case EnumEspecieDocumento_Bradesco.DuplicataServico: return "12";
                case EnumEspecieDocumento_Bradesco.Outros: return "99";
                default: return "99";
            }
        }

        private EnumEspecieDocumento_Bradesco GetEnumEspecieByCodigo(string codigo)
        {
            switch (codigo.TrimStart('0'))
            {
                case "1": return EnumEspecieDocumento_Bradesco.DuplicataMercantil;
                case "2": return EnumEspecieDocumento_Bradesco.NotaPromissoria;
                case "3": return EnumEspecieDocumento_Bradesco.NotaSeguro;
                case "4": return EnumEspecieDocumento_Bradesco.CobrancaSeriada;
                case "5": return EnumEspecieDocumento_Bradesco.Recibo;
                case "10": return EnumEspecieDocumento_Bradesco.LetraCambio;
                case "11": return EnumEspecieDocumento_Bradesco.NotaDebito;
                case "12": return EnumEspecieDocumento_Bradesco.DuplicataServico;
                case "99": return EnumEspecieDocumento_Bradesco.Outros;
                default: return EnumEspecieDocumento_Bradesco.Outros;
            }
        }

        private void Carregar(string idCodigo)
        {
            try
            {
                Banco = new Banco_Bradesco();

                switch (GetEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Bradesco.DuplicataMercantil:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.DuplicataMercantil);
                        Especie = "Duplicata mercantil";
                        Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Bradesco.NotaPromissoria:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.NotaPromissoria);
                        Especie = "Nota promissória";
                        Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Bradesco.NotaSeguro:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.NotaSeguro);
                        Especie = "Nota de seguro";
                        Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Bradesco.CobrancaSeriada:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.CobrancaSeriada);
                        Especie = "Cobrança seriada";
                        Sigla = "CS";
                        break;
                    case EnumEspecieDocumento_Bradesco.Recibo:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.Recibo);
                        Especie = "Recibo";
                        Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_Bradesco.LetraCambio:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.LetraCambio);
                        Sigla = "LC";
                        Especie = "Letra de câmbio";
                        break;
                    case EnumEspecieDocumento_Bradesco.NotaDebito:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.NotaDebito);
                        Sigla = "ND";
                        Especie = "Nota de débito";
                        break;
                    case EnumEspecieDocumento_Bradesco.DuplicataServico:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.DuplicataServico);
                        Sigla = "DS";
                        Especie = "Duplicata de serviço";
                        break;
                    case EnumEspecieDocumento_Bradesco.Outros:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.Outros);
                        Especie = "Outros";
                        Sigla = "OU";
                        break;
                    default:
                        Codigo = "0";
                        Especie = "( Selecione )";
                        Sigla = "";
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
            try
            {
                var alEspeciesDocumento = new EspeciesDocumento();
                var obj = new EspecieDocumento_Bradesco();

                obj = new EspecieDocumento_Bradesco(obj.GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.DuplicataMercantil));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.NotaPromissoria));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.NotaSeguro));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.CobrancaSeriada));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.Recibo));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.LetraCambio));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.NotaDebito));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.DuplicataServico));
                alEspeciesDocumento.Add(obj);

                obj = new EspecieDocumento_Bradesco(obj.GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.Outros));
                alEspeciesDocumento.Add(obj);

                return alEspeciesDocumento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Bradesco(GetCodigoEspecieByEnum(EnumEspecieDocumento_Bradesco.DuplicataMercantil));
        }

        #endregion
    }
}