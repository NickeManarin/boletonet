using System;
using System.IO;
using System.Collections.Generic;
using BoletoNet.Util;

namespace BoletoNet
{
    public class ArquivoRetornoCNAB400 : AbstractArquivoRetorno, IArquivoRetorno
    {
        private HeaderRetorno _headerRetorno = new HeaderRetorno();
        private List<DetalheRetorno> _listaDetalhe = new List<DetalheRetorno>();

        public List<DetalheRetorno> ListaDetalhe
        {
            get { return _listaDetalhe; }
            set { _listaDetalhe = value; }
        }

        public HeaderRetorno HeaderRetorno
        {
            get { return _headerRetorno; }
            set { _headerRetorno = value; }
        }

        #region Construtores

        public ArquivoRetornoCNAB400()
		{
            TipoArquivo = TipoArquivo.Cnab400;
        }

        #endregion

        #region M�todos de inst�ncia

        public override void LerArquivoRetorno(IBanco banco, Stream arquivo)
        {
            try
            {
                var stream = new StreamReader(arquivo, System.Text.Encoding.UTF8);
                var linha = "";
                // Identifica��o do registro detalhe
                var IdsRegistroDetalhe = new List<string>();

                //Lendo o arquivo
                linha = stream.ReadLine();

                Header:
                HeaderRetorno = banco.LerHeaderRetornoCNAB400(linha);

                //Pr�xima linha (DETALHE)
                linha = stream.ReadLine();

                // 85 - CECRED - C�digo de registro detalhe 7 para CECRED
                // 1 - Banco do Brasil- C�digo de registro detalhe 7 para conv�nios com 7 posi��es, e detalhe 1 para conv�nios com 6 posi��es(colocado as duas, pois n�o interferem em cada tipo de arquivo)
                if (banco.Codigo == 85)
                {
                    IdsRegistroDetalhe.Add("7");
                }
                else if (banco.Codigo == 1)
                {
                    IdsRegistroDetalhe.Add("1");//Para conv�nios de 6 posi��es
                    IdsRegistroDetalhe.Add("7");//Para conv�nios de 7 posi��es
                }
                else
                {
                    IdsRegistroDetalhe.Add("1");
                }

                while (IdsRegistroDetalhe.Contains(DetalheRetorno.PrimeiroCaracter(linha)))
                {
                    var detalhe = banco.LerDetalheRetornoCNAB400(linha);
                    ListaDetalhe.Add(detalhe);
                    OnLinhaLida(detalhe, linha);

                    linha = stream.ReadLine();
                }

                //Se for Banrisul, � poss�vel que o arquivo seja om concatenado de v�rios arquivos de retorno.
                if (linha.Truncate(1) == "9" && banco.Codigo == 41)
                {
                    linha = CanReadLine(stream);

                    if (!string.IsNullOrEmpty(linha))
                        goto Header;
                }

                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler arquivo.", ex);
            }
        }

        private string CanReadLine(StreamReader reader)
        {
            try
            {
                return reader.ReadLine();
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}