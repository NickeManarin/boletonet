using System;

namespace BoletoNet
{
    public class Banco : AbstractBanco, IBanco
    {
        #region Variaveis

        private IBanco _iBanco;

		#endregion Variaveis

		#region Construtores

        internal Banco() 
        {}

        public Banco(int codigoBanco)
        {
            try
            {
                InstanciaBanco(codigoBanco);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao instanciar objeto.", ex);
            }
        }

		#endregion

		#region Propriedades da Interface

        public override int Codigo
        {
            get { return _iBanco.Codigo; }
            set { _iBanco.Codigo = value; }
        }

        public override string Digito
        {
            get { return _iBanco.Digito; }
        }

        public override string Nome
        {
            get { return _iBanco.Nome; }
        }

		#endregion

        #region M�todos Privados

        private void InstanciaBanco(int codigoBanco)
        {
            try
            {
                switch (codigoBanco)
                {
                    //104 - Caixa
                    case 104:
                        _iBanco = new BancoCaixa();
                        break;
                    //341 - Ita�
                    case 341:
                        _iBanco = new BancoItau();
                        break;
                    //356 - Real
                    case 275:
                    case 356:
                        _iBanco = new Banco_Real();
                        break;
                    //422 - Safra
                    case 422:
                        _iBanco = new Banco_Safra();
                        break;
                    //237 - Bradesco
                    case 237:
                        _iBanco = new Banco_Bradesco();
                        break;
                    //347 - Sudameris
                    case 347:
                        _iBanco = new Banco_Sudameris();
                        break;
                    //353 - Santander
                    case 353:
                        _iBanco = new Banco_Santander();
                        break;
                    //070 - BRB
                    case 70:
                        _iBanco = new Banco_BRB();
                        break;
                    //479 - BankBoston
                    case 479:
                        _iBanco = new Banco_BankBoston();
                        break;
                    //001 - Banco do Brasil
                    case 1:
                        _iBanco = new Banco_Brasil();
                        break;
                    //399 - HSBC
                    case 399:
                        _iBanco = new Banco_HSBC();
                        break;
                    //003 - HSBC
                    case 3:
                        _iBanco = new Banco_Basa();
                        break;
                    //409 - Unibanco
                    case 409:
                        _iBanco = new Banco_Unibanco();
                        break;
                    //33 - Unibanco
                    case 33:
                        _iBanco = new Banco_Santander();
                        break;
                    //41 - Banrisul
                    case 41:
                        _iBanco = new Banco_Banrisul();
                        break;
                    //756 - Sicoob (Bancoob)
                    case 756:
                        _iBanco = new Banco_Sicoob();
                        break;
                    //748 - Sicredi
                    case 748:
                        _iBanco = new Banco_Sicredi();
                        break;
                    //21 - Banestes
                    case 21:
                        _iBanco = new Banco_Banestes();
                        break;
                    //004 - Nordeste
                    case 4:
                        _iBanco = new Banco_Nordeste();
                        break;
                    //85 - CECRED
                    case 85:
                        _iBanco = new Banco_Cecred();
                        break;
                    case 707:
                        _iBanco = new Banco_Daycoval();
                        break;
                    case 637:
                        _iBanco = new Banco_Sofisa();
                        break;
                    default:
                        throw new Exception("C�digo do banco n�o implementando: " + codigoBanco);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execu��o da transa��o.", ex);
            }
        }

        #endregion

        #region M�todos de Interface

        public override void FormataCodigoBarra(Boleto boleto)
        {
            try
            {
                _iBanco.FormataCodigoBarra(boleto);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a formata��o do c�digo de barra.", ex);
            }
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            try
            {
                _iBanco.FormataLinhaDigitavel(boleto);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a formata��o da linha digit�vel.", ex);
            }
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            try
            {
                _iBanco.FormataNossoNumero(boleto);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a formata��o do nosso n�mero.", ex);
            }
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            try
            {
                _iBanco.FormataNumeroDocumento(boleto);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a formata��o do n�mero do documento.", ex);
            }
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            //try
            //{
                _iBanco.ValidaBoleto(boleto);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Erro durante a valida��o do banco.", ex);
            //}
        }

        #endregion

        #region M�todos de Valida��o de gera��o de arquivo

        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            try
            {
                return _iBanco.ValidarRemessa(tipoArquivo, numeroConvenio, _iBanco, cedente, boletos, numeroArquivoRemessa, out mensagem);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a valida��o do arquivo de REMESSA.", ex);
            }
        }
        
        #endregion

        #region M�todos de gera��o de arquivo

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                return _iBanco.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro HEADER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            try
            {
                return _iBanco.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa, boletos);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro HEADER do arquivo de REMESSA.", ex);
            }
        }
        
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                return _iBanco.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o dos registros de DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                return _iBanco.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarTrailerRemessaComDetalhes(int numeroRegistro, int numeroRegistroDetalhes, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                return _iBanco.GerarTrailerRemessaComDetalhes(numeroRegistro, numeroRegistroDetalhes, tipoArquivo, cedente, vltitulostotal);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }
        
        public override string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                return _iBanco.GerarHeaderRemessa(cedente, tipoArquivo, numeroArquivoRemessa);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro HEADER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                return _iBanco.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro HEADER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                return _iBanco.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, tipoArquivo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro HEADER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo, Boleto boletos)
        {
            try
            {
                return _iBanco.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, tipoArquivo, boletos);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro HEADER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoARemessa(Boleto boleto, int numeroRegistro)
        {
            try
            {
                return _iBanco.GerarDetalheSegmentoARemessa(boleto, numeroRegistro);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o dos registros de DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoBRemessa(Boleto boleto, int numeroRegistro)
        {
            try
            {
                return _iBanco.GerarDetalheSegmentoBRemessa(boleto, numeroRegistro);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o dos registros de DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio)
        {
            try
            {
                return _iBanco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistro, numeroConvenio);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o dos registros de DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio,Cedente cedente)
        {
            try
            {
                return _iBanco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistro, numeroConvenio,cedente);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o dos registros de DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                return _iBanco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistro, tipoArquivo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o dos registros de DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, Sacado sacado)
        {
            try
            {
                return _iBanco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistro, sacado);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o dos registros de DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                return _iBanco.GerarDetalheSegmentoRRemessa(boleto, numeroRegistro, tipoArquivo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o dos registros de DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarDetalheSegmentoSRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                return _iBanco.GerarDetalheSegmentoSRemessa(boleto, numeroRegistro, tipoArquivo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o dos registros de DETALHE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro)
        {
            try
            {
                return _iBanco.GerarTrailerArquivoRemessa(numeroRegistro);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro, Boleto boletos)
        {
            try
            {
                return _iBanco.GerarTrailerArquivoRemessa(numeroRegistro, boletos);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro)
        {
            try
            {
                return _iBanco.GerarTrailerLoteRemessa(numeroRegistro);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro, Boleto boletos)
        {
            try
            {
                return _iBanco.GerarTrailerLoteRemessa(numeroRegistro, boletos);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarMensagemVariavelRemessa(Boleto boleto, ref int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                return _iBanco.GerarMensagemVariavelRemessa(boleto, ref numeroRegistro, tipoArquivo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro MENSAGEM VARIAVEL do arquivo de REMESSA.", ex);
            }
        }

        #endregion

        #region M�todos de Leitura do arquivo de Retorno

        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            return _iBanco.LerDetalheSegmentoTRetornoCNAB240(registro);
        }

        public override DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            return _iBanco.LerDetalheSegmentoURetornoCNAB240(registro);
        }

        public override DetalheSegmentoWRetornoCNAB240 LerDetalheSegmentoWRetornoCNAB240(string registro)
        {
            return _iBanco.LerDetalheSegmentoWRetornoCNAB240(registro);
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            return _iBanco.LerDetalheRetornoCNAB400(registro);
        }

        public override HeaderRetorno LerHeaderRetornoCNAB400(string registro)
        {
            return _iBanco.LerHeaderRetornoCNAB400(registro);
        }

        public override long ObterNossoNumeroSemConvenioOuDigitoVerificador(long convenio, string nossoNumero)
        {
            return _iBanco.ObterNossoNumeroSemConvenioOuDigitoVerificador(convenio, nossoNumero);
        }

        #endregion
    }
}
