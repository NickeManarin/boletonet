using System;
using System.IO;
using System.Text;

namespace BoletoNet
{
    internal class ArquivoRemessaCNAB400 : AbstractArquivoRemessa, IArquivoRemessa
    {
        #region Construtores

        public ArquivoRemessaCNAB400()
        {
            TipoArquivo = TipoArquivo.Cnab400;
        }

        #endregion

        #region Métodos de instância

        /// <summary>
        /// Método que fará a verificação se a classe está devidamente implementada para a geração da Remessa
        /// </summary>
        public override bool ValidarArquivoRemessa(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            try
            {
                var vRetorno = true;
                var vMsg = string.Empty;

                if (boletos != null && boletos.Count > 0)
                {
                    var boleto = boletos[0];

                    var vMsgBol = string.Empty;
                    var vRetBol = boleto.Banco.ValidarRemessa(TipoArquivo, numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsgBol);

                    if (!vRetBol && !string.IsNullOrEmpty(vMsgBol))
                    {
                        vMsg += vMsgBol;
                        vRetorno = vRetBol;
                    }
                }

                mensagem = vMsg;
                return vRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void GerarArquivoRemessa(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, Stream arquivo, int numeroArquivoRemessa)
        {
            try
            {
                var numeroRegistro = 2;
                decimal vltitulostotal = 0; //Uso apenas no registro TRAILER do banco Santander - jsoda em 09/05/2012 - Add no registro TRAILER do banco Banrisul - sidneiklein em 08/08/2013

                using (var incluiLinha = new StreamWriter(arquivo, Encoding.GetEncoding("ISO-8859-1")))
                {
                    cedente.Carteira = boletos[0].Carteira;

                    var strline = banco.GerarHeaderRemessa(numeroConvenio, cedente, TipoArquivo.Cnab400, numeroArquivoRemessa);
                    incluiLinha.WriteLine(strline);

                    foreach (var boleto in boletos)
                    {
                        boleto.Banco = banco;
                        strline = boleto.Banco.GerarDetalheRemessa(boleto, numeroRegistro, TipoArquivo.Cnab400);
                        incluiLinha.WriteLine(strline);
                        vltitulostotal += boleto.ValorBoleto; //Uso apenas no registro TRAILER do banco Santander - jsoda em 09/05/2012 - Add no registro TRAILER do banco Banrisul - sidneiklein em 08/08/2013
                        numeroRegistro++;

                        if (boleto.PercMulta > 0)
                        {
                            if (banco.Codigo == 1) //Banco do brasil.
                            {
                                var bancoAux = new Banco_Brasil();
                                strline = bancoAux.GerarRegistroDetalhe5(boleto, numeroRegistro);
                                incluiLinha.WriteLine(strline);
                                numeroRegistro++;
                            }
                            else if(banco.Codigo == 85) //CECRED
                            {
                                var bancoAux = new Banco_Cecred();
                                var line = bancoAux.GerarRegistroDetalhe5(boleto, numeroRegistro, TipoArquivo.Cnab400);
                                incluiLinha.WriteLine(line);
                                numeroRegistro++;
                            }
                            else if (banco.Codigo == 341) //Banco Itau
                            {
                                var bancoAux = new Banco_Itau();
                                strline = bancoAux.GerarRegistroDetalhe5(boleto, numeroRegistro);
                                incluiLinha.WriteLine(strline);
                                numeroRegistro++;
                            }
                        }
                        
                        if ((boleto.Instrucoes != null && boleto.Instrucoes.Count > 0) || (boleto.Sacado.Instrucoes != null && boleto.Sacado.Instrucoes.Count > 0))
                        {
                            strline = boleto.Banco.GerarMensagemVariavelRemessa(boleto, ref numeroRegistro, TipoArquivo.Cnab400);

                            if (!string.IsNullOrEmpty(strline) && !string.IsNullOrWhiteSpace(strline))
                            {
                                incluiLinha.WriteLine(strline);
                                numeroRegistro++;
                            }
                        }
                    }

                    strline = banco.GerarTrailerRemessa(numeroRegistro, TipoArquivo.Cnab400, cedente, vltitulostotal);

                    incluiLinha.WriteLine(strline);
                    incluiLinha.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar arquivo remessa.", ex);
            }
        }

        #endregion
    }
}
