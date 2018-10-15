using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Sicoob240
    {
        DuplicataMercantil = 2,
    }

    #endregion

    //Vale para CNAB240 apenas. CNAB400 tem outros códigos.
    public class EspecieDocumento_Sicoob240 : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Sicoob240()
        {}

        public EspecieDocumento_Sicoob240(string codigo)
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

        private string GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob240 especie)
        {
            return Convert.ToInt32(especie).ToString("00");
        }

        private EnumEspecieDocumento_Sicoob240 GetEnumEspecieByCodigo(string codigo)
        {
            return (EnumEspecieDocumento_Sicoob240) Convert.ToInt32(codigo);
        }

        private void Carregar(string idCodigo)
        {
            try
            {
                Banco = new Banco_Sicoob();

                switch (GetEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Sicoob240.DuplicataMercantil:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob240.DuplicataMercantil);
                        Especie = "Duplicata mercantil";
                        Sigla = "DM";
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

                var obj = new EspecieDocumento_Sicoob240();

                foreach (var item in Enum.GetValues(typeof (EnumEspecieDocumento_Sicoob240)))
                {
                    obj = new EspecieDocumento_Sicoob240(obj.GetCodigoEspecieByEnum((EnumEspecieDocumento_Sicoob240)item));
                    alEspeciesDocumento.Add(obj);
                }

                return alEspeciesDocumento;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Sicoob240(GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob240.DuplicataMercantil));
        }

        #endregion
    }
}