using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Sicoob
    {
        DuplicataMercantil = 1,
        NotaPromissoria = 2,
        NotaSeguro = 3,
        Recibo = 5,
        DuplicataRural = 6,
        LetraCambio = 8,
        Warrant = 9,
        Cheque = 10,
        DuplicataServico = 12,
        NotaDebito = 13,
        TriplicataMercantil = 14,
        TriplicataServico = 15,
        Fatura = 18,
        ApoliceSeguro = 20,
        MensalidadeEscolar = 21,
        ParcelaConsorcio = 22,
        Outros = 99,
    }

    #endregion

    //Vale para CNAB400 apenas. CNAB240 tem outros códigos.
    public class EspecieDocumento_Sicoob : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Sicoob()
        {}

        public EspecieDocumento_Sicoob(string codigo)
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

        private string GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob especie)
        {
            return Convert.ToInt32(especie).ToString("00");
        }

        private EnumEspecieDocumento_Sicoob GetEnumEspecieByCodigo(string codigo)
        {
            return (EnumEspecieDocumento_Sicoob) Convert.ToInt32(codigo);
        }

        private void Carregar(string idCodigo)
        {
            try
            {
                Banco = new Banco_Sicoob();

                switch (GetEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Sicoob.DuplicataMercantil:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.DuplicataMercantil);
                        Especie = "Duplicata mercantil";
                        Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Sicoob.NotaPromissoria:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.NotaPromissoria);
                        Especie = "Nota promissória";
                        Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Sicoob.NotaSeguro:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.NotaSeguro);
                        Especie = "Nota de seguro";
                        Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Sicoob.Recibo:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.Recibo);
                        Especie = "Recibo";
                        Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_Sicoob.DuplicataRural:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.DuplicataRural);
                        Especie = "Duplicata Rural";
                        Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_Sicoob.LetraCambio:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.LetraCambio);
                        Sigla = "LC";
                        Especie = "Letra de Câmbio";
                        break;
                    case EnumEspecieDocumento_Sicoob.Warrant:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.Warrant);
                        Sigla = "WR";
                        Especie = "Warrant";
                        break;
                    case EnumEspecieDocumento_Sicoob.Cheque:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.Cheque);
                        Sigla = "CH";
                        Especie = "Cheque";
                        break;
                    case EnumEspecieDocumento_Sicoob.DuplicataServico:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.DuplicataServico);
                        Sigla = "DS";
                        Especie = "Duplicata de serviço";
                        break;
                    case EnumEspecieDocumento_Sicoob.NotaDebito:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.NotaDebito);
                        Sigla = "ND";
                        Especie = "Nota de débito";
                        break;
                    case EnumEspecieDocumento_Sicoob.TriplicataMercantil:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.TriplicataMercantil);
                        Sigla = "TP";
                        Especie = "Triplicata Mercantil";
                        break;
                    case EnumEspecieDocumento_Sicoob.TriplicataServico:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.TriplicataServico);
                        Sigla = "TS";
                        Especie = "Triplicata de Serviço";
                        break;
                    case EnumEspecieDocumento_Sicoob.Fatura:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.Fatura);
                        Sigla = "FT";
                        Especie = "Fatura";
                        break;
                    case EnumEspecieDocumento_Sicoob.ApoliceSeguro:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.ApoliceSeguro);
                        Sigla = "AP";
                        Especie = "Apólice de Seguro";
                        break;
                    case EnumEspecieDocumento_Sicoob.MensalidadeEscolar:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.MensalidadeEscolar);
                        Sigla = "ME";
                        Especie = "Mensalidade Escolar";
                        break;
                    case EnumEspecieDocumento_Sicoob.ParcelaConsorcio:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.ParcelaConsorcio);
                        Sigla = "PC";
                        Especie = "Parcela de Consórcio";
                        break;
                    case EnumEspecieDocumento_Sicoob.Outros:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.Outros);
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

                var obj = new EspecieDocumento_Sicoob();

                foreach (var item in Enum.GetValues(typeof (EnumEspecieDocumento_Sicoob)))
                {
                    obj = new EspecieDocumento_Sicoob(obj.GetCodigoEspecieByEnum((EnumEspecieDocumento_Sicoob)item));
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
            return new EspecieDocumento_Sicoob(GetCodigoEspecieByEnum(EnumEspecieDocumento_Sicoob.DuplicataMercantil));
        }

        #endregion
    }
}