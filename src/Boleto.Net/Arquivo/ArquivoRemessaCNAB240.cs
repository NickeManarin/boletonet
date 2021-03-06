using System;
using System.IO;

namespace BoletoNet
{
    internal class ArquivoRemessaCNAB240 : AbstractArquivoRemessa, IArquivoRemessa
    {
        #region Construtores

        public ArquivoRemessaCNAB240()
        {
            this.TipoArquivo = TipoArquivo.Cnab240;
        }

        #endregion

        #region M�todos de inst�ncia

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
                    var vRetBol = boleto.Banco.ValidarRemessa(this.TipoArquivo, numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsgBol);

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
                var numeroRegistro = 0;
                var numeroRegistroDetalhe = 1;
                string strline;
                var incluiLinha = new StreamWriter(arquivo);

                #region Header do arquivo

                //Quando � caixa verifica o modelo de leiatue que � est� em boletos.remssa.tipodocumento.
                if (banco.Codigo == 104)
                    strline = banco.GerarHeaderRemessa(numeroConvenio, cedente, TipoArquivo.Cnab240, numeroArquivoRemessa, boletos[0]);
                else
                    strline = banco.GerarHeaderRemessa(numeroConvenio, cedente, TipoArquivo.Cnab240, numeroArquivoRemessa);

                numeroRegistro++;
                
                incluiLinha.WriteLine(strline);
                OnLinhaGerada(null, strline, EnumTipodeLinha.HeaderDeArquivo);

                #endregion

                #region Header do lote

                //Quando � caixa verifica o modelo de leiatue que � est� em boletos.Remessa.TipoDocumento.
                if (banco.Codigo == 104)
                    strline = banco.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, TipoArquivo.Cnab240, boletos[0]);
                else
                    strline = banco.GerarHeaderLoteRemessa(numeroConvenio, cedente, numeroArquivoRemessa, TipoArquivo.Cnab240);

                if (strline != "")
                {
                    incluiLinha.WriteLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.HeaderDeLote);
                    numeroRegistro++;
                }

                #endregion

                if (banco.Codigo == 341)
                {
                    #region se Banco Itau - 341

                    foreach (var boleto in boletos)
                    {
                        boleto.Banco = banco;

                        //suelton@gmail.com - 03 / 01 / 2017
                        //strline = boleto.Banco.GerarDetalheRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.CNAB240);

                        //Segmento P - Obrigat�rio - suelton@gmail.com - 03/01/2017
                        strline = boleto.Banco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistroDetalhe, numeroConvenio);
                        incluiLinha.WriteLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        //Seqgmento Q - Obrigat�rio - suelton@gmail.com - 03/01/2017
                        strline = boleto.Banco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.Cnab240);
                        incluiLinha.WriteLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoQ);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        //Segmento R - Opcional - suelton@gmail.com - 03/01/2017
                        if (boleto.ValorMulta > 0 || boleto.PercMulta > 0)
                        {
                            strline = boleto.Banco.GerarDetalheSegmentoRRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.Cnab240);
                            incluiLinha.WriteLine(strline);
                            OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoR);
                            numeroRegistro++;
                            numeroRegistroDetalhe++;
                        }
                    }

                    //numeroRegistro--;
                    strline = banco.GerarTrailerLoteRemessa(numeroRegistro);
                    incluiLinha.WriteLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeLote);

                    ++numeroRegistro;
                    ++numeroRegistro; //Iniciou do 0 ent�o tem que somar +1 para totoalizar a quantidade de linhas

                    strline = banco.GerarTrailerArquivoRemessa(numeroRegistro);
                    incluiLinha.WriteLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeArquivo);

                    incluiLinha.Close();

                    #endregion
                }
                else if (banco.Codigo == 104) // S� validar boleto.Remessa quando o banco for Caixa porque quando o banco for diferente de 104 a propriedade "Remessa" fica null
                {                    
                    #region se Banco Caixa - 104 e tipo de arquivo da remessa SIGCB

                    if (boletos[0].Remessa.TipoDocumento.Equals("2") || boletos[0].Remessa.TipoDocumento.Equals("1"))
                    {
                        foreach (var boleto in boletos)
                        {
                            boleto.Banco = banco;
                            strline = boleto.Banco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistroDetalhe, numeroConvenio, cedente);
                            incluiLinha.WriteLine(strline);
                            OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                            numeroRegistro++;
                            numeroRegistroDetalhe++;

                            strline = boleto.Banco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistroDetalhe, boleto.Sacado);
                            incluiLinha.WriteLine(strline);
                            OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoQ);
                            numeroRegistro++;
                            numeroRegistroDetalhe++;

                            if (boleto.ValorMulta > 0 || boleto.PercMulta > 0)
                            {
                                strline = boleto.Banco.GerarDetalheSegmentoRRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.Cnab240);
                                incluiLinha.WriteLine(strline);
                                OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoR);
                                numeroRegistro++;
                                numeroRegistroDetalhe++;
                            }
                        }

                        //numeroRegistro--;
                        strline = banco.GerarTrailerLoteRemessa(numeroRegistro, boletos[0]);
                        incluiLinha.WriteLine(strline);
                        OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeLote);

                        numeroRegistro++;
                        numeroRegistro++;

                        strline = banco.GerarTrailerArquivoRemessa(numeroRegistro, boletos[0]);
                        incluiLinha.WriteLine(strline);
                        OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeArquivo);

                        incluiLinha.Close();
                    }

                    #endregion
                }
                else if (banco.Codigo == 33)
                {
                    #region se Banco Santander - 33

                    foreach (var boleto in boletos)
                    {
                        boleto.Banco = banco;
                        boleto.Remessa.NumeroLote = numeroArquivoRemessa;

                        strline = boleto.Banco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistroDetalhe, numeroConvenio);

                        incluiLinha.WriteLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        if (boletos[0].Remessa.CodigoOcorrencia == "01")
                        {
                            strline = boleto.Banco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.Cnab240);
                            incluiLinha.WriteLine(strline);
                            OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoQ);
                            numeroRegistro++;
                            numeroRegistroDetalhe++;

                            if (boleto.ValorMulta > 0 || boleto.OutrosDescontos > 0 || boleto.PercMulta > 0)
                            {
                                strline = boleto.Banco.GerarDetalheSegmentoRRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.Cnab240);
                                incluiLinha.WriteLine(strline);
                                OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoR);
                                numeroRegistro++;
                                numeroRegistroDetalhe++;
                            }

                            strline = boleto.Banco.GerarDetalheSegmentoSRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.Cnab240);
                            incluiLinha.WriteLine(strline);
                            OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoS);
                            numeroRegistro++;
                            numeroRegistroDetalhe++;
                        }
                    }

                    strline = banco.GerarTrailerLoteRemessa(numeroRegistro);
                    incluiLinha.WriteLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeLote);

                    numeroRegistro++;
                    numeroRegistro++;

                    strline = banco.GerarTrailerArquivoRemessa(numeroRegistro);
                    incluiLinha.WriteLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeArquivo);

                    incluiLinha.Close();

                    #endregion
                }
                else if (banco.Codigo == 237)
                {
                    #region Bradesco

                    decimal totalTitulos = 0;
                    foreach (var boleto in boletos)
                    {
                        boleto.Banco = banco;
                        strline = boleto.Banco.GerarDetalheSegmentoARemessa(boleto, numeroRegistroDetalhe);
                        incluiLinha.WriteLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        strline = boleto.Banco.GerarDetalheSegmentoBRemessa(boleto, numeroRegistroDetalhe);
                        incluiLinha.WriteLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        totalTitulos += boleto.ValorBoleto;
                    }

                    numeroRegistro++;
                
                    strline = banco.GerarTrailerRemessaComDetalhes(numeroRegistro, boletos.Count,  TipoArquivo.Cnab240, cedente, totalTitulos);
                    incluiLinha.WriteLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeArquivo);

                    incluiLinha.Close();

                    #endregion
                }
                else if (banco.Codigo == 756)
                {
                    #region Sicoob

                    foreach (var boleto in boletos)
                    {
                        boleto.Banco = banco;

                        incluiLinha.WriteLine(boleto.Banco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistroDetalhe, numeroConvenio));
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        incluiLinha.WriteLine(boleto.Banco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.Cnab240));
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        if (boleto.PercMulta > 0 || boleto.ValorMulta > 0)
                        {
                            incluiLinha.WriteLine(boleto.Banco.GerarDetalheSegmentoRRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.Cnab240));
                            numeroRegistro++;
                            numeroRegistroDetalhe++;
                        }
                    }

                    incluiLinha.WriteLine(banco.GerarTrailerLoteRemessa(numeroRegistroDetalhe));
                    numeroRegistro++;
                    
                    incluiLinha.WriteLine(banco.GerarTrailerArquivoRemessa(numeroRegistro));
                    incluiLinha.Close();

                    #endregion
                }
                else //Para qualquer outro banco, gera CNAB240 com segmentos abaixo
                {
                    #region Outros bancos

                    foreach (var boleto in boletos)
                    {
                        boleto.Banco = banco;
                        strline = boleto.Banco.GerarDetalheSegmentoPRemessa(boleto, numeroRegistroDetalhe, numeroConvenio);
                        incluiLinha.WriteLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoP);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        strline = boleto.Banco.GerarDetalheSegmentoQRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.Cnab240);
                        incluiLinha.WriteLine(strline);
                        OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoQ);
                        numeroRegistro++;
                        numeroRegistroDetalhe++;

                        if (boleto.PercMulta > 0 || boleto.ValorMulta > 0)
                        {
                            strline = boleto.Banco.GerarDetalheSegmentoRRemessa(boleto, numeroRegistroDetalhe, TipoArquivo.Cnab240);
                            incluiLinha.WriteLine(strline);
                            OnLinhaGerada(boleto, strline, EnumTipodeLinha.DetalheSegmentoR);
                            numeroRegistro++;
                            numeroRegistroDetalhe++;
                        }
                    }
                    
                    numeroRegistro--;
                    strline = banco.GerarTrailerLoteRemessa(numeroRegistro);
                    incluiLinha.WriteLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeLote);

                    numeroRegistro++;
                    numeroRegistro++;

                    strline = banco.GerarTrailerArquivoRemessa(numeroRegistro);
                    incluiLinha.WriteLine(strline);
                    OnLinhaGerada(null, strline, EnumTipodeLinha.TraillerDeArquivo);

                    incluiLinha.Close();

                    #endregion
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