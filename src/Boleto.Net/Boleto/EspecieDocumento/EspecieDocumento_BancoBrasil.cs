using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_BancoBrasil
    {
        DuplicataMercantil = 1,
        NotaPromissoria = 2,
        NotaDeSeguro = 3,
        Recibo = 5,
        LetraDeCambio = 8,
        Warrant = 9,
        Cheque = 10,
        DuplicataDeServiço = 12,
        NotaDeDébito = 13,
        ApólicedeSeguro = 15,
        DívidaAtivaDaUnião = 25,
        DívidaAtivaDoEstado = 26,
        DívidaAtivaDoMunicipio = 27
    }

    #endregion

    public class EspecieDocumento_BancoBrasil : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_BancoBrasil()
        {}

        public EspecieDocumento_BancoBrasil(string codigo)
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

        private string GetCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_BancoBrasil.DuplicataMercantil: return "01";
                case EnumEspecieDocumento_BancoBrasil.NotaPromissoria: return "02";
                case EnumEspecieDocumento_BancoBrasil.Recibo: return "05";
                case EnumEspecieDocumento_BancoBrasil.Cheque: return "10";
                default: return "01";
            }
        }

        private EnumEspecieDocumento_BancoBrasil GetEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "01": return EnumEspecieDocumento_BancoBrasil.DuplicataMercantil;
                case "02": return EnumEspecieDocumento_BancoBrasil.NotaPromissoria;
                case "03": return EnumEspecieDocumento_BancoBrasil.NotaDeSeguro;
                case "05": return EnumEspecieDocumento_BancoBrasil.Recibo;
                case "08": return EnumEspecieDocumento_BancoBrasil.LetraDeCambio;
                case "09": return EnumEspecieDocumento_BancoBrasil.Warrant;
                case "10": return EnumEspecieDocumento_BancoBrasil.Cheque;
                case "12": return EnumEspecieDocumento_BancoBrasil.DuplicataDeServiço;
                case "13": return EnumEspecieDocumento_BancoBrasil.NotaDeDébito;
                case "15": return EnumEspecieDocumento_BancoBrasil.ApólicedeSeguro;
                case "25": return EnumEspecieDocumento_BancoBrasil.DívidaAtivaDaUnião;
                case "26": return EnumEspecieDocumento_BancoBrasil.DívidaAtivaDoEstado;
                case "27": return EnumEspecieDocumento_BancoBrasil.DívidaAtivaDoMunicipio;

                default: return EnumEspecieDocumento_BancoBrasil.DuplicataMercantil;
            }
        }

        private void Carregar(string idCodigo)
        {
            try
            {
                Banco = new Banco_Brasil();

                switch (GetEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_BancoBrasil.Cheque:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Cheque);
                        Especie = "CHEQUE";
                        Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataMercantil:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantil);
                        Especie = "DUPLICATA MERCANTIL";
                        Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataDeServiço:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataDeServiço);
                        Especie = "DUPLICATA DE SERVIÇO";
                        Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.LetraDeCambio:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.LetraDeCambio);
                        Especie = "LETRA DE CAMBIO";
                        Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaPromissoria:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaPromissoria);
                        Especie = "NOTA PROMISSÓRIA";
                        Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.Recibo:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Recibo);
                        Especie = "RECIBO";
                        Sigla = "RC";
                        break;
                    default:
                        Codigo = "00";
                        //Especie = "OUTROS";
                        //Sigla = "OUTROS";
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
                var ed = new EspecieDocumento_BancoBrasil();

                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Cheque)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaPromissoria)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Recibo)));

                return alEspeciesDocumento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_BancoBrasil(GetCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantil));
        }

        #endregion
    }
}