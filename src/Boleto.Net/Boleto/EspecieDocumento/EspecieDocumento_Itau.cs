using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_Itau
    {
        DuplicataMercantil = 1,
        NotaPromissoria = 2,
        NotaSeguro = 3,
        MensalidadeEscolar = 4,
        Recibo = 5,
        Contrato = 6,
        Cosseguros = 7,
        DuplicataServico = 8,
        LetraCambio = 9,
        NotaDebito = 13,
        DocumentoDivida = 15,
        EncargosCondominais = 16,
        Diversos = 99,
    }

    #endregion

    public class EspecieDocumento_Itau : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Itau()
        {}

        public EspecieDocumento_Itau(string codigo)
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

        public string GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Itau.DuplicataMercantil: return "1";
                case EnumEspecieDocumento_Itau.NotaPromissoria: return "2";
                case EnumEspecieDocumento_Itau.NotaSeguro: return "3";
                case EnumEspecieDocumento_Itau.MensalidadeEscolar: return "4";
                case EnumEspecieDocumento_Itau.Recibo: return "5";
                case EnumEspecieDocumento_Itau.Contrato: return "6";
                case EnumEspecieDocumento_Itau.Cosseguros: return "7";
                case EnumEspecieDocumento_Itau.DuplicataServico: return "8";
                case EnumEspecieDocumento_Itau.LetraCambio: return "9";
                case EnumEspecieDocumento_Itau.NotaDebito: return "13";
                case EnumEspecieDocumento_Itau.DocumentoDivida: return "15";
                case EnumEspecieDocumento_Itau.EncargosCondominais: return "16";
                case EnumEspecieDocumento_Itau.Diversos: return "99";
                default: return "99";
            }
        }

        public EnumEspecieDocumento_Itau GetEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1":
                case "01":return EnumEspecieDocumento_Itau.DuplicataMercantil;
                case "02":return EnumEspecieDocumento_Itau.NotaPromissoria;
                case "03":return EnumEspecieDocumento_Itau.NotaSeguro;
                case "04":return EnumEspecieDocumento_Itau.MensalidadeEscolar;
                case "05":return EnumEspecieDocumento_Itau.Recibo;
                case "06":return EnumEspecieDocumento_Itau.Contrato;
                case "07":return EnumEspecieDocumento_Itau.Cosseguros;
                case "08":return EnumEspecieDocumento_Itau.DuplicataServico;
                case "09":return EnumEspecieDocumento_Itau.LetraCambio;
                case "13":return EnumEspecieDocumento_Itau.NotaDebito;
                case "15":return EnumEspecieDocumento_Itau.DocumentoDivida;
                case "16":return EnumEspecieDocumento_Itau.EncargosCondominais;
                case "99": return EnumEspecieDocumento_Itau.Diversos;
                default: return EnumEspecieDocumento_Itau.Diversos;
            }
        }

        private void Carregar(string idCodigo)
        {
            try
            {
                Banco = new BancoItau();
                var ed = new EspecieDocumento_Itau();

                switch (GetEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Itau.DuplicataMercantil:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.DuplicataMercantil);
                        Especie = "Duplicata mercantil";
                        Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Itau.NotaPromissoria:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.NotaPromissoria);
                        Especie = "Nota promissória";
                        Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Itau.NotaSeguro:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.NotaSeguro);
                        Especie = "Nota de seguro";
                        Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Itau.MensalidadeEscolar:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.MensalidadeEscolar);
                        Especie = "Mensalidade escolar";
                        Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_Itau.Recibo:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.Recibo);
                        Especie = "Recibo";
                        Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Itau.Contrato:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.Contrato);
                        Sigla = "C";
                        Especie = "Contrato";
                        break;
                    case EnumEspecieDocumento_Itau.Cosseguros:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.Cosseguros);
                        Sigla = "CS";
                        Especie = "Cosseguros";
                        break;
                    case EnumEspecieDocumento_Itau.DuplicataServico:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.DuplicataServico);
                        Sigla = "DS";
                        Especie = "Duplicata de serviço";
                        break;
                    case EnumEspecieDocumento_Itau.LetraCambio:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.LetraCambio);
                        Sigla = "LC";
                        Especie = "Letra de câmbio";
                        break;
                    case EnumEspecieDocumento_Itau.NotaDebito:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.NotaDebito);
                        Sigla = "ND";
                        Especie = "Nota de débito";
                        break;
                    case EnumEspecieDocumento_Itau.DocumentoDivida:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.DocumentoDivida);
                        Sigla = "DD";
                        Especie = "Documento de dívida";
                        break;
                    case EnumEspecieDocumento_Itau.EncargosCondominais:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.EncargosCondominais);
                        Sigla = "EC";
                        Especie = "Encargos condominais";
                        break;
                    case EnumEspecieDocumento_Itau.Diversos:
                        Codigo = ed.GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.Diversos);
                        Especie = "Diversos";
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
            var esp = new EspeciesDocumento();
            var ed = new EspecieDocumento_Itau();

            foreach (EnumEspecieDocumento_Itau item in Enum.GetValues(typeof(EnumEspecieDocumento_Itau)))
                esp.Add(new EspecieDocumento_Itau(ed.GetCodigoEspecieByEnum(item)));

            return esp;
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Itau(GetCodigoEspecieByEnum(EnumEspecieDocumento_Itau.DuplicataMercantil));
        }

        #endregion
    }
}