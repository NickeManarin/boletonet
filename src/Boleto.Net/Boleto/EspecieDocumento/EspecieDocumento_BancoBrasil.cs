using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumEspecieDocumento_BancoBrasil
    {
        Cheque = 1, //CH – CHEQUE
        DuplicataMercantil = 2, //DM – DUPLICATA MERCANTIL
        DuplicataMercantilIndicacao = 3, //DMI – DUPLICATA MERCANTIL P/ INDICAÇÃO
        DuplicataServico = 4, //DS –  DUPLICATA DE SERVIÇO
        DuplicataServicoIndicacao = 5, //DSI –  DUPLICATA DE SERVIÇO P/ INDICAÇÃO
        DuplicataRural = 6, //DR – DUPLICATA RURAL
        LetraCambio = 7, //LC – LETRA DE CAMBIO
        NotaCreditoComercial = 8, //NCC – NOTA DE CRÉDITO COMERCIAL
        NotaCreditoExportacao = 9, //NCE – NOTA DE CRÉDITO A EXPORTAÇÃO
        NotaCreditoIndustrial = 10, //NCI – NOTA DE CRÉDITO INDUSTRIAL
        NotaCreditoRural = 11, //NCR – NOTA DE CRÉDITO RURAL
        NotaPromissoria = 12, //NP – NOTA PROMISSÓRIA
        NotaPromissoriaRural = 13, //NPR –NOTA PROMISSÓRIA RURAL
        TriplicataMercantil = 14, //TM – TRIPLICATA MERCANTIL
        TriplicataServico = 15, //TS –  TRIPLICATA DE SERVIÇO
        NotaSeguro = 16, //NS – NOTA DE SEGURO
        Recibo = 17, //RC – RECIBO
        Fatura = 18, //FAT – FATURA
        NotaDebito = 19, //ND –  NOTA DE DÉBITO
        ApoliceSeguro = 20, //AP –  APÓLICE DE SEGURO
        MensalidadeEscolar = 21, //ME – MENSALIDADE ESCOLAR
        ParcelaConsorcio = 22, //PC –  PARCELA DE CONSÓRCIO
        Outros = 23 //OUTROS
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
                carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion

        #region Metodos Privados

        public string getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil especie)
        {
            switch (especie)
            {
                case EnumEspecieDocumento_BancoBrasil.Cheque: return "1";
                case EnumEspecieDocumento_BancoBrasil.DuplicataMercantil: return "2";
                case EnumEspecieDocumento_BancoBrasil.DuplicataMercantilIndicacao: return "3";
                case EnumEspecieDocumento_BancoBrasil.DuplicataServico: return "4";
                case EnumEspecieDocumento_BancoBrasil.DuplicataServicoIndicacao: return "5";
                case EnumEspecieDocumento_BancoBrasil.DuplicataRural: return "6";
                case EnumEspecieDocumento_BancoBrasil.LetraCambio: return "7";
                case EnumEspecieDocumento_BancoBrasil.NotaCreditoComercial: return "8";
                case EnumEspecieDocumento_BancoBrasil.NotaCreditoExportacao: return "9";
                case EnumEspecieDocumento_BancoBrasil.NotaCreditoIndustrial: return "10";
                case EnumEspecieDocumento_BancoBrasil.NotaCreditoRural: return "11";
                case EnumEspecieDocumento_BancoBrasil.NotaPromissoria: return "12";
                case EnumEspecieDocumento_BancoBrasil.NotaPromissoriaRural: return "13";
                case EnumEspecieDocumento_BancoBrasil.TriplicataMercantil: return "14";
                case EnumEspecieDocumento_BancoBrasil.TriplicataServico: return "15";
                case EnumEspecieDocumento_BancoBrasil.NotaSeguro: return "16";
                case EnumEspecieDocumento_BancoBrasil.Recibo: return "17";
                case EnumEspecieDocumento_BancoBrasil.Fatura: return "18";
                case EnumEspecieDocumento_BancoBrasil.NotaDebito: return "19";
                case EnumEspecieDocumento_BancoBrasil.ApoliceSeguro: return "20";
                case EnumEspecieDocumento_BancoBrasil.MensalidadeEscolar: return "21";
                case EnumEspecieDocumento_BancoBrasil.ParcelaConsorcio: return "22";
                case EnumEspecieDocumento_BancoBrasil.Outros: return "23";
                default: return "23";

            }
        }

        public EnumEspecieDocumento_BancoBrasil getEnumEspecieByCodigo(string codigo)
        {
            switch (codigo)
            {
                case "1": return EnumEspecieDocumento_BancoBrasil.Cheque;
                case "2": return EnumEspecieDocumento_BancoBrasil.DuplicataMercantil;
                case "3": return EnumEspecieDocumento_BancoBrasil.DuplicataMercantilIndicacao;
                case "4": return EnumEspecieDocumento_BancoBrasil.DuplicataServico;
                case "5": return EnumEspecieDocumento_BancoBrasil.DuplicataServicoIndicacao;
                case "6": return EnumEspecieDocumento_BancoBrasil.DuplicataRural;
                case "7": return EnumEspecieDocumento_BancoBrasil.LetraCambio;
                case "8": return EnumEspecieDocumento_BancoBrasil.NotaCreditoComercial;
                case "9": return EnumEspecieDocumento_BancoBrasil.NotaCreditoExportacao;
                case "10": return EnumEspecieDocumento_BancoBrasil.NotaCreditoIndustrial;
                case "11": return EnumEspecieDocumento_BancoBrasil.NotaCreditoRural;
                case "12": return EnumEspecieDocumento_BancoBrasil.NotaPromissoria;
                case "13": return EnumEspecieDocumento_BancoBrasil.NotaPromissoriaRural;
                case "14": return EnumEspecieDocumento_BancoBrasil.TriplicataMercantil;
                case "15": return EnumEspecieDocumento_BancoBrasil.TriplicataServico;
                case "16": return EnumEspecieDocumento_BancoBrasil.NotaSeguro;
                case "17": return EnumEspecieDocumento_BancoBrasil.Recibo;
                case "18": return EnumEspecieDocumento_BancoBrasil.Fatura;
                case "19": return EnumEspecieDocumento_BancoBrasil.NotaDebito;
                case "20": return EnumEspecieDocumento_BancoBrasil.ApoliceSeguro;
                case "21": return EnumEspecieDocumento_BancoBrasil.MensalidadeEscolar;
                case "22": return EnumEspecieDocumento_BancoBrasil.ParcelaConsorcio;
                case "23": return EnumEspecieDocumento_BancoBrasil.Outros;
                default: return EnumEspecieDocumento_BancoBrasil.DuplicataMercantil;
            }
        }

        private void carregar(string idCodigo)
        {
            try
            {
                Banco = new Banco_Brasil();

                switch (getEnumEspecieByCodigo(idCodigo))
                {
                    case EnumEspecieDocumento_BancoBrasil.Cheque:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Cheque);
                        Especie = "CHEQUE";
                        Sigla = "CH";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataMercantil:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantil);
                        Especie = "DUPLICATA MERCANTIL";
                        Sigla = "DM";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataMercantilIndicacao:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantilIndicacao);
                        Especie = "DUPLICATA MERCANTIL P/ INDICAÇÃO";
                        Sigla = "DMI";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataServico:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataServico);
                        Especie = "DUPLICATA DE SERVIÇO";
                        Sigla = "DS";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataServicoIndicacao:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataServicoIndicacao);
                        Especie = "DUPLICATA DE SERVIÇO P/ INDICAÇÃO";
                        Sigla = "DSI";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.DuplicataRural:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataRural);
                        Especie = "DUPLICATA RURAL";
                        Sigla = "DR";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.LetraCambio:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.LetraCambio);
                        Especie = "LETRA DE CAMBIO";
                        Sigla = "LC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaCreditoComercial:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoComercial);
                        Especie = "NOTA DE CRÉDITO COMERCIAL";
                        Sigla = "NCC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaCreditoExportacao:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoExportacao);
                        Especie = "NOTA DE CRÉDITO A EXPORTAÇÃO";
                        Sigla = "NCE";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaCreditoIndustrial:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoIndustrial);
                        Especie = "NOTA DE CRÉDITO INDUSTRIAL";
                        Sigla = "NCI";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaCreditoRural:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoRural);
                        Especie = "NOTA DE CRÉDITO RURAL";
                        Sigla = "NCR";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaPromissoria:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaPromissoria);
                        Especie = "NOTA PROMISSÓRIA";
                        Sigla = "NP";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaPromissoriaRural:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaPromissoriaRural);
                        Especie = "NOTA PROMISSÓRIA RURAL";
                        Sigla = "NPR";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.TriplicataMercantil:
                        Codigo =getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.TriplicataMercantil);
                        Especie = "TRIPLICATA MERCANTIL";
                        Sigla = "TM";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.TriplicataServico:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.TriplicataServico);
                        Especie = "TRIPLICATA DE SERVIÇO";
                        Sigla = "TS";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaSeguro:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaSeguro);
                        Especie = "NOTA DE SEGURO";
                        Sigla = "NS";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.Recibo:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Recibo);
                        Especie = "RECIBO";
                        Sigla = "RC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.Fatura:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Fatura);
                        Especie = "FATURA";
                        Sigla = "FAT";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.NotaDebito:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaDebito);
                        Especie = "NOTA DE DÉBITO";
                        Sigla = "ND";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.ApoliceSeguro:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.ApoliceSeguro);
                        Especie = "APÓLICE DE SEGURO";
                        Sigla = "AP";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.MensalidadeEscolar:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.MensalidadeEscolar);
                        Especie = "MENSALIDADE ESCOLAR";
                        Sigla = "ME";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.ParcelaConsorcio:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.ParcelaConsorcio);
                        Especie = "PARCELA DE CONSÓRCIO";
                        Sigla = "PC";
                        break;
                    case EnumEspecieDocumento_BancoBrasil.Outros:
                        Codigo = getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Outros);
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
            try
            {
                var alEspeciesDocumento = new EspeciesDocumento();
                var ed = new EspecieDocumento_BancoBrasil();

                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Cheque)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantilIndicacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataServicoIndicacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.LetraCambio)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoComercial)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoExportacao)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoIndustrial)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaCreditoRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaPromissoria)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaPromissoriaRural)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.TriplicataMercantil)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.TriplicataServico)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Recibo)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Fatura)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.NotaDebito)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.ApoliceSeguro)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.MensalidadeEscolar)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.ParcelaConsorcio)));
                alEspeciesDocumento.Add(new EspecieDocumento_BancoBrasil(ed.getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.Outros)));

                return alEspeciesDocumento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }

        public override IEspecieDocumento DuplicataMercantil()
        {
            return new EspecieDocumento_BancoBrasil(getCodigoEspecieByEnum(EnumEspecieDocumento_BancoBrasil.DuplicataMercantil));
        }

        #endregion
    }
}