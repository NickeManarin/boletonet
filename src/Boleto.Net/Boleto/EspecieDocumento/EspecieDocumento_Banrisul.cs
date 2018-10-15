using System;

namespace BoletoNet
{
    #region Enumerador

    public enum EnumEspecieDocumento_Banrisul
    {
        Cheque,
        DuplicataMercantil,
        DuplicataMercantilIndicacao ,
        DuplicataServico ,
        DuplicataServicoIndicacao ,
        DuplicataRural ,
        LetraCambio ,
        NotaCreditoComercial ,
        NotaCreditoExportacao ,
        NotaCreditoIndustrial ,
        NotaCreditoRural ,
        NotaPromissoria,
        NotaPromissoriaRural,
        TriplicataMercantil,
        TriplicataServico,
        NotaSeguro,
        Recibo,
        Fatura,
        NotaDebito,
        ApoliceSeguro,
        MensalidadeEscolar,
        ParcelaConsorcio,
        Outros,
    }

    #endregion

    public class EspecieDocumento_Banrisul : AbstractEspecieDocumento, IEspecieDocumento
    {
        #region Construtores

        public EspecieDocumento_Banrisul()
        {}

        public EspecieDocumento_Banrisul(string codigo)
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

        private string GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_Banrisul.Cheque: return "1";
                case EnumEspecieDocumento_Banrisul.DuplicataMercantil: return "2";
                case EnumEspecieDocumento_Banrisul.DuplicataMercantilIndicacao: return "3";
                case EnumEspecieDocumento_Banrisul.DuplicataServico: return "4";
                case EnumEspecieDocumento_Banrisul.DuplicataServicoIndicacao: return "5";
                case EnumEspecieDocumento_Banrisul.DuplicataRural: return "6";
                case EnumEspecieDocumento_Banrisul.LetraCambio : return "7";
                case EnumEspecieDocumento_Banrisul.NotaCreditoComercial: return "8";
                case EnumEspecieDocumento_Banrisul.NotaCreditoExportacao : return "9";
                case EnumEspecieDocumento_Banrisul.NotaCreditoIndustrial: return "10";
                case EnumEspecieDocumento_Banrisul.NotaCreditoRural : return "11";
                case EnumEspecieDocumento_Banrisul.NotaPromissoria : return "12";
                case EnumEspecieDocumento_Banrisul.NotaPromissoriaRural: return "13";
                case EnumEspecieDocumento_Banrisul.TriplicataMercantil: return "14";
                case EnumEspecieDocumento_Banrisul.TriplicataServico : return "15";
                case EnumEspecieDocumento_Banrisul.NotaSeguro : return "16";
                case EnumEspecieDocumento_Banrisul.Recibo: return "17";
                case EnumEspecieDocumento_Banrisul.Fatura: return "18";
                case EnumEspecieDocumento_Banrisul.NotaDebito: return "19";
                case EnumEspecieDocumento_Banrisul.ApoliceSeguro: return "20";
                case EnumEspecieDocumento_Banrisul.MensalidadeEscolar: return "21";
                case EnumEspecieDocumento_Banrisul.ParcelaConsorcio: return "22";
                case EnumEspecieDocumento_Banrisul.Outros: return "23";
                default: return "23";
            }
        }

        private EnumEspecieDocumento_Banrisul GetEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_Banrisul.Cheque;
                case "2": return EnumEspecieDocumento_Banrisul.DuplicataMercantil;
                case "3": return EnumEspecieDocumento_Banrisul.DuplicataMercantilIndicacao;
                case "4": return EnumEspecieDocumento_Banrisul.DuplicataServico;
                case "5": return EnumEspecieDocumento_Banrisul.DuplicataServicoIndicacao;
                case "6": return EnumEspecieDocumento_Banrisul.DuplicataRural;
                case "7": return EnumEspecieDocumento_Banrisul.LetraCambio;
                case "8": return EnumEspecieDocumento_Banrisul.NotaCreditoComercial;
                case "9": return EnumEspecieDocumento_Banrisul.NotaCreditoExportacao;
                case "10": return EnumEspecieDocumento_Banrisul.NotaCreditoIndustrial;
                case "11": return EnumEspecieDocumento_Banrisul.NotaCreditoRural;
                case "12": return EnumEspecieDocumento_Banrisul.NotaPromissoria;
                case "13": return EnumEspecieDocumento_Banrisul.NotaPromissoriaRural;
                case "14": return EnumEspecieDocumento_Banrisul.TriplicataMercantil;
                case "15": return EnumEspecieDocumento_Banrisul.TriplicataServico;
                case "16": return EnumEspecieDocumento_Banrisul.NotaSeguro;
                case "17": return EnumEspecieDocumento_Banrisul.Recibo;
                case "18": return EnumEspecieDocumento_Banrisul.Fatura;
                case "19": return EnumEspecieDocumento_Banrisul.NotaDebito;
                case "20": return EnumEspecieDocumento_Banrisul.ApoliceSeguro;
                case "21": return EnumEspecieDocumento_Banrisul.MensalidadeEscolar;
                case "22": return EnumEspecieDocumento_Banrisul.ParcelaConsorcio;
                case "23": return EnumEspecieDocumento_Banrisul.Outros;

                case "01":
                default:
                    return EnumEspecieDocumento_Banrisul.DuplicataMercantil;
            }
        }

        private void Carregar(string idCodigo)
        {
            try
            {
                Banco = new Banco_Banrisul();

                switch (GetEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_Banrisul.Cheque:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.Cheque);
                        Especie = "CHEQUE";
                        Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_Banrisul.DuplicataMercantil:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.DuplicataMercantil);
                        Especie = "DUPLICATA MERCANTIL";
                        Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_Banrisul.DuplicataMercantilIndicacao:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.DuplicataMercantilIndicacao);
                        Especie = "DUPLICATA MERCANTIL P/ INDICAÇÃO";
                        Sigla = "DMI";
                        break;
                    case EnumEspecieDocumento_Banrisul.DuplicataServico:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.DuplicataServico);
                        Especie = "DUPLICATA DE SERVIÇO";
                        Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_Banrisul.DuplicataServicoIndicacao:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.DuplicataServicoIndicacao);
                        Especie = "DUPLICATA DE SERVIÇO P/ INDICAÇÃO";
                        Sigla = "DSI";
                        break;
                    case EnumEspecieDocumento_Banrisul.DuplicataRural:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.DuplicataRural);
                        Especie = "DUPLICATA RURAL";
                        Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_Banrisul.LetraCambio:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.LetraCambio);
                        Especie = "LETRA DE CAMBIO";
                        Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaCreditoComercial:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaCreditoComercial);
                        Especie = "NOTA DE CRÉDITO COMERCIAL";
                        Sigla = "NCC";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaCreditoExportacao:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaCreditoExportacao);
                        Especie = "NOTA DE CRÉDITO A EXPORTAÇÃO";
                        Sigla = "NCE";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaCreditoIndustrial:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaCreditoIndustrial);
                        Especie = "NOTA DE CRÉDITO INDUSTRIAL";
                        Sigla = "NCI";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaCreditoRural:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaCreditoRural);
                        Especie = "NOTA DE CRÉDITO RURAL";
                        Sigla = "NCR";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaPromissoria:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaPromissoria);
                        Especie = "NOTA PROMISSÓRIA";
                        Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaPromissoriaRural:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaPromissoriaRural);
                        Especie = "NOTA PROMISSÓRIA RURAL";
                        Sigla = "NPR";
                        break;
                    case EnumEspecieDocumento_Banrisul.TriplicataMercantil:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.TriplicataMercantil);
                        Especie = "TRIPLICATA MERCANTIL";
                        Sigla = "TM";
                        break;
                    case EnumEspecieDocumento_Banrisul.TriplicataServico:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.TriplicataServico);
                        Especie = "TRIPLICATA DE SERVIÇO";
                        Sigla = "TS";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaSeguro:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaSeguro);
                        Especie = "NOTA DE SEGURO";
                        Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_Banrisul.Recibo:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.Recibo);
                        Especie = "RECIBO";
                        Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_Banrisul.Fatura:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.Fatura);
                        Especie = "FATURA";
                        Sigla = "FAT";
                        break;
                    case EnumEspecieDocumento_Banrisul.NotaDebito:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.NotaDebito);
                        Especie = "NOTA DE DÉBITO";
                        Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_Banrisul.ApoliceSeguro:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.ApoliceSeguro);
                        Especie = "APÓLICE DE SEGURO";
                        Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_Banrisul.MensalidadeEscolar:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.MensalidadeEscolar);
                        Especie = "MENSALIDADE ESCOLAR";
                        Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_Banrisul.ParcelaConsorcio:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.ParcelaConsorcio);
                        Especie = "PARCELA DE CONSÓRCIO";
                        Sigla = "PC";
                        break;
                    case EnumEspecieDocumento_Banrisul.Outros:
                        Codigo = GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.Outros);
                        Especie = "OUTROS";
                        Sigla = "OUTROS";
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
            var ed = new EspecieDocumento_Banrisul();

            foreach (EnumEspecieDocumento_Banrisul item in Enum.GetValues(typeof(EnumEspecieDocumento_Banrisul)))
                especiesDocumento.Add(new EspecieDocumento_Banrisul(ed.GetCodigoEspecieByEnum(item)));

            return especiesDocumento;
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_Banrisul(GetCodigoEspecieByEnum(EnumEspecieDocumento_Banrisul.DuplicataMercantil));
        }

        #endregion
    }
}