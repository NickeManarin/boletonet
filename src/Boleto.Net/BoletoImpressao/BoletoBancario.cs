using BoletoNet.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Globalization;
using System.Linq;

[assembly: WebResource("BoletoNet.BoletoImpressao.BoletoNet.css", "text/css", PerformSubstitution = true)]
[assembly: WebResource("BoletoNet.Imagens.barra.gif", "image/gif")]
//[assembly: WebResource("BoletoNet.Imagens.corte.gif", "image/gif")]
//[assembly: WebResource("BoletoNet.Imagens.barrainterna.gif", "image/gif")]
//[assembly: WebResource("BoletoNet.Imagens.ponto.gif", "image/gif")]

namespace BoletoNet
{
    [Serializable, Designer(typeof(BoletoBancarioDesigner)), ToolboxBitmap(typeof(BoletoBancario)), ToolboxData("<{0}:BoletoBancario Runat=\"server\"></{0}:BoletoBancario>")]
    public class BoletoBancario : Control
    {
        #region Variáveis

        private Banco _ibanco;
        private short _codigoBanco;
        private Boleto _boleto;
        private string _instrucoesHtml = string.Empty;
        private bool _ajustaTamanhoFonte;
        private string _ajustaTamanhoFonteHtml;

        public BoletoBancario()
        {
            Instrucoes = new List<IInstrucao>();
        }

        #endregion Variaveis

        #region Propriedades

        [Browsable(true), Description("Remove o símbolo R$ do Valor do Documento")]
        public bool RemoveSimboloMoedaValorDocumento { get; set; }

        [Browsable(true), Description("Código do banco em que será gerado o boleto. Ex. 341-Itaú, 237-Bradesco")]
        public short CodigoBanco
        {
            get { return _codigoBanco; }
            set { _codigoBanco = value; }
        }

        /// <summary>
        /// Mostra o código da carteira
        /// </summary>
        [Browsable(true), Description("Mostra a descrição da carteira")]
        public bool MostrarCodigoCarteira { get; set; }


        [Browsable(true), Description("Gera um relatório com os valores que deram origem ao boleto")]
        public bool ExibirDemonstrativo { get; set; }

        /// <summary>
        /// Mostra o código da carteira
        /// </summary>
        [Browsable(true), Description("Formata o boleto no layout de carnê")]
        public bool FormatoCarne { get; set; }

        /// <summary>
        /// Logo de terceiros.
        /// </summary>
        [Browsable(true), Description("Logo de terceiros.")]
        public string Logo { get; set; }

        [Browsable(false)]
        public Boleto Boleto
        {
            get { return _boleto; }
            set
            {
                _boleto = value;

                if (_ibanco == null)
                {
                    _boleto.Banco = Banco;
                    _boleto.BancoCarteira = BancoCarteiraFactory.Fabrica(_boleto.Carteira, Banco.Codigo);
                }

                Cedente = _boleto.Cedente;
                Sacado = _boleto.Sacado;
            }
        }

        [Browsable(false)]
        public Sacado Sacado { get; private set; }

        [Browsable(false)]
        public Cedente Cedente { get; private set; }

        [Browsable(false)]
        public Banco Banco
        {
            get
            {
                if (_ibanco == null || _ibanco.Codigo != _codigoBanco)
                    _ibanco = new Banco(_codigoBanco);

                if (_boleto != null)
                    _boleto.Banco = _ibanco;

                return _ibanco;
            }
        }

        /// <summary>
        /// Caminho onde se encontra a ferramenta WkHtmlToPdf.
        /// Se os arquivos do WkHtmlToPdf não estiverem presentes no caminho,
        /// eles serão criados automaticamente pelo NReco PdfGenerator.
        /// </summary>
        [Browsable(true), Description("Caminho onde se encontra a ferramenta WkHtmlToPdf.")]
        public string PdfToolPath { get; set; }

        /// <summary>
        /// Caminho onde a NReco gera os arquivos temporários necessários para a construção do PDF.
        /// Se o caminho não for especificado a ferramenta utilizará a pasta retornada pelo método Path.GetTempPath().
        /// </summary>
        [Browsable(true), Description("Caminho onde a NReco gera os arquivos temporários necessários para a construção do PDF.")]
        public string TempFilesPath { get; set; }

        [Browsable(true), Description("Mostra o comprovante de entrega sem dados para marcar")]
        public bool MostrarComprovanteEntregaLivre
        {
            get { return Utils.ToBool(ViewState["1"]); }
            set { ViewState["1"] = value; }
        }

        [Browsable(true), Description("Mostra o comprovante de entrega")]
        public bool MostrarComprovanteEntrega
        {
            get { return Utils.ToBool(ViewState["2"]); }
            set { ViewState["2"] = value; }
        }

        [Browsable(true), Description("Oculta as intruções do boleto")]
        public bool OcultarEnderecoSacado
        {
            get { return Utils.ToBool(ViewState["3"]); }
            set { ViewState["3"] = value; }
        }

        [Browsable(true), Description("Oculta as intruções do boleto")]
        public bool OcultarInstrucoes
        {
            get { return Utils.ToBool(ViewState["4"]); }
            set { ViewState["4"] = value; }
        }

        [Browsable(true), Description("Oculta o recibo do sacado do boleto")]
        public bool OcultarReciboSacado
        {
            get { return Utils.ToBool(ViewState["5"]); }
            set { ViewState["5"] = value; }
        }

        [Browsable(true), Description("Gerar arquivo de remessa")]
        public bool GerarArquivoRemessa
        {
            get { return Utils.ToBool(ViewState["6"]); }
            set { ViewState["6"] = value; }
        }

        /// <summary> 
        /// Mostra o termo "Contra Apresentação" na data de vencimento do boleto
        /// </summary>
        public bool MostrarContraApresentacaoNaDataVencimento
        {
            get { return Utils.ToBool(ViewState["7"]); }
            set { ViewState["7"] = value; }
        }

        [Browsable(true), Description("Mostra o endereço do Cedente")]
        public bool MostrarEnderecoCedente
        {
            get { return Utils.ToBool(ViewState["8"]); }
            set { ViewState["8"] = value; }
        }

        /// <summary> 
        /// Instruções disponíveis no Boleto
        /// </summary>
        public List<IInstrucao> Instrucoes { get; private set; }

        #endregion Propriedades

        #region Override

        protected override void OnPreRender(EventArgs e)
        {
            var alias = "BoletoNet.BoletoImpressao.BoletoNet.css";
            var csslink = "<link rel=\"stylesheet\" type=\"text/css\" href=\"" +
                Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), alias) + "\" />";

            var include = new LiteralControl(csslink);
            Page.Header.Controls.Add(include);

            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e)
        { }

        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "Execution")]
        protected override void Render(HtmlTextWriter output)
        {
            if (_ibanco == null)
            {
                output.Write("<b>Erro gerando o boleto bancário: faltou definir o banco.</b>");
                return;
            }

            var urlImagemLogo = Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens." + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg");
            var urlImagemBarra = Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens.barra.gif");
            //string urlImagemBarraInterna = Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens.barrainterna.gif");
            //string urlImagemCorte = Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens.corte.gif");
            //string urlImagemPonto = Page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens.ponto.gif");

            //Atribui os valores ao html do boleto bancário
            //output.Write(MontaHtml(urlImagemCorte, urlImagemLogo, urlImagemBarra, urlImagemPonto, urlImagemBarraInterna,
            //    "<img src=\"ImagemCodigoBarra.ashx?code=" + Boleto.CodigoBarra.Codigo + "\" alt=\"Código de Barras\" />"));
            output.Write(MontaHtml(urlImagemLogo, urlImagemBarra, "<img src=\"ImagemCodigoBarra.ashx?code=" + Boleto.CodigoBarra.Codigo + "\" alt=\"Código de Barras\" />"));
        }

        #endregion Override

        #region Html

        public string GeraHtmlInstrucoes()
        {
            try
            {
                var html = new StringBuilder();

                var titulo = "Instruções de Impressão";
                var instrucoes = "Imprimir em impressora jato de tinta (ink jet) ou laser em qualidade normal. (Não use modo econômico).<br>Utilize folha A4 (210 x 297 mm) ou Carta (216 x 279 mm) - Corte na linha indicada<br>";

                html.Append(Html.Instrucoes);
                html.Append("<br />");

                return html.ToString().Replace("@TITULO", titulo).Replace("@INSTRUCAO", instrucoes);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }

        private string GeraHtmlCarne(string telefone, string htmlBoleto)
        {
            var html = new StringBuilder();

            html.Append(Html.Carne);

            return html.ToString().Replace("@TELEFONE", telefone).Replace("#BOLETO#", htmlBoleto);
        }

        public string GeraHtmlReciboSacado()
        {
            try
            {
                var html = new StringBuilder();

                html.Append(Html.ReciboSacadoParte1);
                html.Append("<br />");
                html.Append(Html.ReciboSacadoParte2);
                html.Append(Html.ReciboSacadoParte3);

                if (MostrarEnderecoCedente)
                    html.Append(Html.ReciboSacadoParte10);

                html.Append(Html.ReciboSacadoParte4);
                html.Append(Html.ReciboSacadoParte5);
                html.Append(Html.ReciboSacadoParte6);
                html.Append(Html.ReciboSacadoParte7);

                //if (Instrucoes.Count == 0)
                html.Append(Html.ReciboSacadoParte8);

                //Limpa as intruções para o Sacado
                _instrucoesHtml = "";

                MontaInstrucoes(Boleto.Instrucoes, Boleto.MensagemSac);

                if (Boleto.Sacado.Instrucoes.Count > 0)
                    MontaInstrucoes(Boleto.Sacado.Instrucoes, null);

                return html.ToString().Replace("@INSTRUCOES", _instrucoesHtml);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a execução da transação.", ex);
            }
        }

        public string GeraHtmlReciboCedente()
        {
            try
            {
                var html = new StringBuilder();

                html.Append(Html.ReciboCedenteParte1);
                html.Append(Html.ReciboCedenteParte2);
                html.Append(Html.ReciboCedenteParte3);
                html.Append(Html.ReciboCedenteParte4);
                html.Append(Html.ReciboCedenteParte5);
                html.Append(Html.ReciboCedenteParte6);
                html.Append(Html.ReciboCedenteParte7);
                html.Append(Html.ReciboCedenteParte8);
                html.Append(Html.ReciboCedenteParte9);
                html.Append(Html.ReciboCedenteParte10);
                html.Append(Html.ReciboCedenteParte11);
                html.Append(Html.ReciboCedenteParte12);

                //Para Banco Itaú, o texto "(Texto de responsabilidade do cedente)" deve ser
                //"(Todas as informações deste bloqueto são de exclusiva responsabilidade do cedente)".
                if (Boleto.Banco.Codigo == 341)
                    html.Replace("(Texto de responsabilidade do cedente)", "(Todas as informações deste bloqueto são de exclusiva responsabilidade do cedente)");

                //Limpa as intruções para o Cedente
                _instrucoesHtml = "";

                MontaInstrucoes(Boleto.Instrucoes);

                if (Boleto.Cedente.Instrucoes.Count > 0)
                    MontaInstrucoes(Boleto.Cedente.Instrucoes);

                return html.ToString().Replace("@INSTRUCOES", _instrucoesHtml);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na execução da transação.", ex);
            }
        }

        public string HtmlComprovanteEntrega
        {
            get
            {
                var html = new StringBuilder();

                html.Append(Html.ComprovanteEntrega1);
                html.Append(Html.ComprovanteEntrega2);
                html.Append(Html.ComprovanteEntrega3);
                html.Append(Html.ComprovanteEntrega4);
                html.Append(Html.ComprovanteEntrega5);
                html.Append(Html.ComprovanteEntrega6);

                html.Append(MostrarComprovanteEntregaLivre ? Html.ComprovanteEntrega71 : Html.ComprovanteEntrega7);

                html.Append("<br />");
                return html.ToString();
            }
        }

        private void MontaInstrucoes(IList<IInstrucao> instrucoes, string sac = null)
        {
            if (!string.IsNullOrEmpty(_instrucoesHtml))
                _instrucoesHtml = string.Concat(_instrucoesHtml, "<br />");

            if (instrucoes.Count <= 0)
                return;

            //_instrucoesHtml = string.Empty;
            foreach (var instrucao in instrucoes)
            {
                var pieces = (instrucao.Descricao ?? "").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var piece in pieces)
                {
                    var aux = ChunksUpto(piece, 100);

                    foreach (var aux1 in aux)
                        _instrucoesHtml += $"{aux1}<br />";
                }

                //Adiciona a instrução as instruções disponíveis no Boleto
                Instrucoes.Add(instrucao);
            }

            _instrucoesHtml = Strings.Left(_instrucoesHtml, _instrucoesHtml.Length - 6);

            if (!string.IsNullOrWhiteSpace(sac))
                _instrucoesHtml += "<br><br>" + sac;
        }

        private string MontaHtml(string urlImagemLogo, string urlImagemBarra, string imagemCodigoBarras)
        {
            var html = new StringBuilder();
            var enderecoCedente = "";

            if (_ajustaTamanhoFonte)
                html.Append(_ajustaTamanhoFonteHtml);

            //Oculta o cabeçalho das instruções do boleto
            if (!OcultarInstrucoes)
                html.Append(GeraHtmlInstrucoes());

            if (ExibirDemonstrativo && Boleto.Demonstrativos.Any())
            {
                html.Append(Html.ReciboCedenteRelatorioValores);
                html.Append(Html.ReciboCedenteParte5);

                html.Append(Html.CabecalhoTabelaDemonstrativo);

                var grupoDemonstrativo = new StringBuilder();

                foreach (var relatorio in Boleto.Demonstrativos)
                {
                    var first = true;

                    foreach (var item in relatorio.Itens)
                    {
                        grupoDemonstrativo.Append(Html.GrupoDemonstrativo);

                        if (first)
                        {
                            grupoDemonstrativo = grupoDemonstrativo.Replace("@DESCRICAOGRUPO", relatorio.Descricao);

                            first = false;
                        }
                        else
                        {
                            grupoDemonstrativo = grupoDemonstrativo.Replace("@DESCRICAOGRUPO", string.Empty);
                        }

                        grupoDemonstrativo = grupoDemonstrativo.Replace("@DESCRICAOITEM", item.Descricao);
                        grupoDemonstrativo = grupoDemonstrativo.Replace("@REFERENCIAITEM", item.Referencia);
                        grupoDemonstrativo = grupoDemonstrativo.Replace("@VALORITEM", item.Valor.ToString("R$ ##,##0.00"));
                    }

                    grupoDemonstrativo.Append(Html.TotalDemonstrativo);
                    grupoDemonstrativo = grupoDemonstrativo.Replace("@VALORTOTALGRUPO", relatorio.Itens.Sum(c => c.Valor).ToString("R$ ##,##0.00"));
                }

                html = html.Replace("@ITENSDEMONSTRATIVO", grupoDemonstrativo.ToString());
            }

            if (!FormatoCarne)
            {
                //Mostra o comprovante de entrega
                if (MostrarComprovanteEntrega | MostrarComprovanteEntregaLivre)
                {
                    html.Append(HtmlComprovanteEntrega);
                    //Html da linha pontilhada
                    if (OcultarReciboSacado)
                        html.Append(Html.ReciboSacadoParte8);
                }

                //Oculta o recibo do sacabo do boleto
                if (!OcultarReciboSacado)
                {
                    html.Append(GeraHtmlReciboSacado());

                    //Caso mostre o Endereço do Cedente
                    if (MostrarEnderecoCedente)
                    {
                        if (Cedente.Endereco == null)
                            throw new ArgumentNullException("Endereço do Cedente");

                        var numero = !string.IsNullOrEmpty(Cedente.Endereco.Numero) ? Cedente.Endereco.Numero + ", " : "SN, ";
                        var comp = !string.IsNullOrEmpty(Cedente.Endereco.Complemento) ? Cedente.Endereco.Complemento + ", " : "";
                        enderecoCedente = string.Concat(Cedente.Endereco.End, " , ", numero, comp);

                        if (Cedente.Endereco.Cep == string.Empty)
                            enderecoCedente += $"{Cedente.Endereco.Bairro} - {Cedente.Endereco.Cidade}/{Cedente.Endereco.Uf}";
                        else
                            enderecoCedente += $"{Cedente.Endereco.Bairro} - {Cedente.Endereco.Cidade}/{Cedente.Endereco.Uf} - CEP: {Utils.FormataCep(Cedente.Endereco.Cep)}";
                    }
                }
            }

            string sacado;

            //Flavio(fhlviana@hotmail.com) - adicionei a possibilidade de o boleto não ter, necessáriamente, que informar o CPF ou CNPJ do sacado.
            //Formata o CPF/CNPJ(se houver) e o Nome do Sacado para apresentação
            if (Sacado.CpfCnpj == string.Empty)
                sacado = Sacado.Nome;
            else
            {
                if (Sacado.CpfCnpj.Length <= 11)
                    sacado = $"{Sacado.Nome}<br/>CPF: {Utils.FormataCpf(Sacado.CpfCnpj)}";
                else
                    sacado = $"{Sacado.Nome}<br/>CNPJ: {Utils.FormataCnpj(Sacado.CpfCnpj)}";
            }

            var infoSacado = Sacado.InformacoesSacado.GeraHtml(false);

            //Caso não oculte o Endereço do Sacado.
            if (!OcultarEnderecoSacado)
            {
                string enderecoSacado;

                if (Sacado.Endereco.Cep == string.Empty)
                    enderecoSacado = $"{Sacado.Endereco.Bairro} - {Sacado.Endereco.Cidade}/{Sacado.Endereco.Uf}";
                else
                    enderecoSacado = $"{Sacado.Endereco.Bairro} - {Sacado.Endereco.Cidade}/{Sacado.Endereco.Uf} - CEP: {Utils.FormataCep(Sacado.Endereco.Cep)}";

                if (Sacado.Endereco.End != string.Empty && enderecoSacado != string.Empty)
                {
                    var numero = !string.IsNullOrEmpty(Sacado.Endereco.Numero) ? ", " + Sacado.Endereco.Numero : ", SN";
                    var comp = !string.IsNullOrEmpty(Sacado.Endereco.Complemento) ? ", " + Sacado.Endereco.Complemento : "";

                    if (infoSacado == string.Empty)
                        infoSacado += InfoSacado.Render(Sacado.Endereco.End + numero + comp, enderecoSacado, false);
                    else
                        infoSacado += InfoSacado.Render(Sacado.Endereco.End + numero + comp, enderecoSacado, true);
                }

                //"Informações do Sacado" foi introduzido para possibilitar que o boleto nao informe somente o endereço do sacado
                //como em outras situaçoes onde se imprime matriculas, codigos e etc, sobre o sacado.
                //Sendo assim o endereço do sacado passa a ser uma Informaçao do Sacado que é adicionada no momento da renderização
                //de acordo com a flag "OcultarEnderecoSacado"
            }

            var agenciaConta = Utils.FormataAgenciaConta(Cedente.ContaBancaria.Agencia, Cedente.ContaBancaria.DigitoAgencia, Cedente.ContaBancaria.Conta,
                Cedente.ContaBancaria.DigitoConta);

            // Trecho adicionado por Fabrício Nogueira de Almeida :fna - fnalmeida@gmail.com - 09/12/2008
            /* Esse código foi inserido pq no campo Agência/Cod Cedente, estava sendo impresso sempre a agência / número da conta
			 * No boleto da caixa que eu fiz, coloquei no método validarBoleto um trecho para calcular o dígito do cedente, e adicionei esse atributo na classe cedente
			 * O trecho abaixo testa se esse digito foi calculado, se foi insere no campo Agencia/Cod Cedente, a agência e o código com seu digito
			 * caso contrário mostra a agência / conta, como era anteriormente.
			 * Com esse código ele ira atender as necessidades do boleto caixa e não afetará os demais
			 * Caso queira que apareça o Agência/cod. cedente para outros boletos, basta calcular e setar o digito, como foi feito no boleto Caixa 
			 */

            string agenciaCodigoCedente;

            if (!Cedente.DigitoCedente.Equals(-1))
            {
                //if (!string.IsNullOrEmpty(Cedente.ContaBancaria.OperacaConta))
                //    agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}/{Cedente.ContaBancaria.OperacaConta}.{Utils.FormatCode(Cedente.Codigo, 6)}-{Cedente.DigitoCedente}";

                switch (Boleto.Banco.Codigo)
                {
                    case 33:
                        var codigoCedente = Cedente.Codigo.PadLeft(7, '0').Substring(1) + Utils.FormatCode(Cedente.DigitoCedente.ToString(), 1);
                        agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}/{codigoCedente}";
                        break;
                    case 748:
                        agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}.{Cedente.ContaBancaria.DigitoAgencia}.{Utils.FormatCode(Cedente.ContaBancaria.Conta, 5)}";
                        break;
                    case 1:
                        agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}-{Cedente.ContaBancaria.DigitoAgencia}/{Utils.FormatCode(Cedente.ContaBancaria.Conta, 6)}-{Cedente.ContaBancaria.DigitoConta}";
                        break;
                    case 399:
                        agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}/{Utils.FormatCode(Cedente.Codigo + Cedente.DigitoCedente, 7)}";
                        break;
                    case 41: //Banrisul.
                        //Removido o dígito verificador do Banrisul. .{Cedente.ContaBancaria.DigitoAgencia}
                        agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}/{Cedente.Codigo}.{Cedente.DigitoCedente}";
                        break;
                    case 104: //Caixa.
                        agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}/{Utils.FormatCode(Cedente.Codigo, 6)}-{Cedente.DigitoCedente}";
                        break;
                    default:
                        agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}-{Cedente.ContaBancaria.DigitoAgencia}/{Utils.FormatCode(Cedente.Codigo, 6)}-{Cedente.DigitoCedente}";
                        break;
                }
            }
            else
            {
                switch (Boleto.Banco.Codigo)
                {
                    case 33:
                        agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}-{Cedente.ContaBancaria.DigitoAgencia}/{Utils.FormatCode(Cedente.Codigo, 6)}";

                        if (string.IsNullOrEmpty(Cedente.ContaBancaria.DigitoAgencia))
                            agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}/{Utils.FormatCode(Cedente.Codigo, 6)}";
                        break;
                    case 41: //Banrisul.
                        //Removido o dígito verificador do Banrisul. .{Cedente.ContaBancaria.DigitoAgencia}
                        agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}/{Cedente.ContaBancaria.Conta.Substring(0, 6)}.{Cedente.ContaBancaria.Conta.Substring(6, 1)}.{Cedente.ContaBancaria.DigitoConta}";
                        break;
                    case 399:
                        agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}/{Utils.FormatCode(Cedente.Codigo, 7)}";
                        break;
                    case 748:
                        agenciaCodigoCedente = $"{Cedente.ContaBancaria.Agencia}.{Cedente.ContaBancaria.DigitoAgencia}.{Utils.FormatCode(Cedente.ContaBancaria.Conta, 5)}";
                        break;
                    default:
                        agenciaCodigoCedente = agenciaConta;
                        break;
                }

                //Verificar se é a mesma coisa que Utils.FormatCode(Cedente.ContaBancaria.Conta, 5) e Cedente.Codigo
            }

            html.Append(!FormatoCarne ? GeraHtmlReciboCedente() : GeraHtmlCarne("", GeraHtmlReciboCedente()));

            //Para carteiras "17-019" e "18-019" do Banco do Brasil, a ficha de compensação não possui código da carteira na formatação do campo.
            if (Boleto.Banco.Codigo == 1)
            {
                if (Boleto.CarteiraEVariacao.Equals("17-019") | Boleto.CarteiraEVariacao.Equals("17-027") |
                    Boleto.CarteiraEVariacao.Equals("18-019") | Boleto.CarteiraEVariacao.Equals("17-159") | Boleto.CarteiraEVariacao.Equals("17-140") |
                    Boleto.CarteiraEVariacao.Equals("17-067"))
                {
                    html.Replace("Carteira / ", "");
                }
                else
                {
                    html.Replace("@NOSSONUMERO", "@NOSSONUMEROBB");
                }
            }
            else
            {
                //Para SANTANDER, a ficha de compensação não possui código da carteira - por jsoda em 08/12/2012
                if (Boleto.Banco.Codigo == 33 || Boleto.Banco.Codigo == 41)
                    html.Replace("Carteira /", "");
            }

            #region Nosso número

            var nossoNumero = string.IsNullOrWhiteSpace(Boleto.DigitoNossoNumero) ? Boleto.NossoNumero : (Boleto.NossoNumero + "-" + Boleto.DigitoNossoNumero);

            if (Boleto.Banco.Codigo == 237)
                nossoNumero = Boleto.Carteira.PadLeft(2, '0') + "/" + Boleto.NossoNumero + "-" + Boleto.DigitoNossoNumero;
            else if (Boleto.Banco.Codigo == 33)
                nossoNumero = Boleto.NossoNumero.PadLeft(12, '0') + "-" + Boleto.DigitoNossoNumero;
            else if (Boleto.Banco.Codigo == 341) //Itaú
                nossoNumero = Boleto.Carteira + "/" + Boleto.NossoNumero + "-" + Boleto.DigitoNossoNumero;

            #endregion

            #region Cedente - mais informações

            var cedenteBoleto = !Cedente.MostrarCnpjNoBoleto ? Cedente.Nome : $"{Cedente.Nome}&nbsp;&nbsp;&nbsp;CNPJ: {Cedente.CpfCnpjComMascara}";

            if (Cedente.Endereco != null)
            {
                if (Boleto.Banco.Codigo == 237)
                    cedenteBoleto += " - " + Cedente.Endereco.EndComTudoMesmo;
            }

            #endregion

            var dataVencimento = Boleto.DataVencimento.ToString("dd/MM/yyyy");

            if (MostrarContraApresentacaoNaDataVencimento)
                dataVencimento = "Contra Apresentação";

            if (string.IsNullOrWhiteSpace(Logo) || !File.Exists(Logo))
                Logo = urlImagemLogo;

            var valorBoleto = Boleto.ValorBoleto == 0 ? "" : Boleto.ValorBoleto.ToString("C", CultureInfo.GetCultureInfo("PT-BR"));

            return html.Replace("@CODIGOBANCO", Utils.FormatCode(_ibanco.Codigo.ToString(), 3))
                .Replace("@DIGITOBANCO", _ibanco.Digito)
                //.Replace("@URLIMAGEMBARRAINTERNA", urlImagemBarraInterna)
                //.Replace("@URLIMAGEMCORTE", urlImagemCorte)
                //.Replace("@URLIMAGEMPONTO", urlImagemPonto)
                .Replace("@URLIMAGEMLOGO", urlImagemLogo)
                .Replace("@URLIMGCEDENTE", Logo)
                .Replace("@URLIMAGEMBARRA", urlImagemBarra)
                .Replace("@LINHADIGITAVEL", Boleto.CodigoBarra.LinhaDigitavel)
                .Replace("@LOCALPAGAMENTO", Boleto.LocalPagamento ?? "Até o vencimento, preferencialmente no banco de origem deste boleto.")
                .Replace("@DATAVENCIMENTO", dataVencimento)
                .Replace("@CEDENTE_BOLETO", cedenteBoleto)
                .Replace("@CEDENTE", Cedente.Nome)
                .Replace("@DATADOCUMENTO", Boleto.DataDocumento.ToString("dd/MM/yyyy"))
                .Replace("@NUMERODOCUMENTO", Boleto.NumeroDocumentoImpressao)
                .Replace("@ESPECIEDOCUMENTO", EspecieDocumento.ValidaSigla(Boleto.EspecieDocumento))
                .Replace("@DATAPROCESSAMENTO", Boleto.DataProcessamento.ToString("dd/MM/yyyy"))

                .Replace("@NOSSONUMEROBB", Boleto.Carteira + "/" + Boleto.NossoNumero)
                .Replace("@NOSSONUMERO", nossoNumero)
                .Replace("@CARTEIRA", FormataDescricaoCarteira())
                .Replace("@PARCELAS", Boleto.TotalParcelas <= 1 ? "ÚNICA" : $"{Boleto.Parcela} de {Boleto.TotalParcelas}")
                .Replace("@ESPECIE", Boleto.Especie)
                .Replace("@QUANTIDADE", Boleto.QuantidadeMoeda == 0 ? "" : Boleto.QuantidadeMoeda.ToString())
                .Replace("@VALORDOCUMENTO", Boleto.ValorMoeda)
                .Replace("@=VALORDOCUMENTO", valorBoleto)
                .Replace("@VALORCOBRADO", Boleto.ValorCobrado == 0 ? "" : Boleto.ValorCobrado.ToString("C", CultureInfo.GetCultureInfo("PT-BR")))
                .Replace("@OUTROSACRESCIMOS", Boleto.OutrosAcrescimos == 0 ? "" : Boleto.OutrosAcrescimos.ToString("C", CultureInfo.GetCultureInfo("PT-BR")))
                .Replace("@OUTRASDEDUCOES", "")
                .Replace("@DESCONTOS", Boleto.ValorDesconto == 0 ? "" : Boleto.ValorDesconto.ToString("C", CultureInfo.GetCultureInfo("PT-BR")))
                .Replace("@AGENCIACONTA", agenciaCodigoCedente)
                .Replace("@SACADO2", sacado)
                .Replace("@SACADO", sacado.Replace("<br/>", " - ")) //Com a quebra de linha apenas no recibo do sacado do carnê.
                .Replace("@INFOSACADO", infoSacado)
                .Replace("@AGENCIACODIGOCEDENTE", agenciaCodigoCedente)
                .Replace("@CPFCNPJ", Cedente.CpfCnpjComMascara)
                .Replace("@MORAMULTA", Boleto.ValorMulta == 0 ? "" : Boleto.ValorMulta.ToString("C", CultureInfo.GetCultureInfo("PT-BR")))
                .Replace("@AUTENTICACAOMECANICA", "")
                .Replace("@USODOBANCO", Boleto.UsoBanco)
                .Replace("@IMAGEMCODIGOBARRA", imagemCodigoBarras)
                .Replace("@ACEITE", Boleto.Aceite).ToString()
                .Replace("@ENDERECOCEDENTE", MostrarEnderecoCedente ? enderecoCedente : "")
                .Replace("@AVALISTA", string.Format("{0} - {1}", Boleto.Avalista != null ? "/ Sacador Avalista: " + Boleto.Avalista.Nome : "", Boleto.Avalista != null ? Boleto.Avalista.CpfCnpj : ""))
                .Replace("Ar\">R$", RemoveSimboloMoedaValorDocumento ? "Ar\">" : "Ar\">R$")
                .Replace("@SACBANRISUL", Boleto.Banco.Codigo == 41 ? "</br>SAC 0800 646 1515 / OUVIDORIA BANRISUL 0800 644 2200" : "");
            //.Replace("@TITULONOSSONUMERO", Boleto.Banco.Codigo == 41 ? "Nosso Número" : "Carteira / Nosso Número");
        }

        private string FormataDescricaoCarteira()
        {
            if (!MostrarCodigoCarteira)
            {
                if (Boleto.Banco.Codigo == 33 && Boleto.Carteira == "101")
                    return "RCR";

                return Boleto.Carteira;
            }

            string descricaoCarteira;
            var carteira = Utils.ToInt32(Boleto.Carteira);

            switch (Banco.Codigo)
            {
                case 1:
                    descricaoCarteira = new Carteira_BancoBrasil(carteira).Codigo;
                    break;
                case 353:
                case 8:
                case 33:
                    descricaoCarteira = new Carteira_Santander(carteira).Codigo;
                    break;
                case 104:
                    descricaoCarteira = new Carteira_Caixa(carteira).Codigo;
                    break;
                case 341:
                    descricaoCarteira = new Carteira_Itau(carteira).Codigo;
                    break;

                default:
                    throw new Exception($"A descrição para o banco {Boleto.Banco} não foi implementada.");
                    //throw new Exception(string.Format("A descrição da carteira {0} / banco {1} não foi implementada (marque false na propriedade MostrarCodigoCarteira)", carteira, Banco.Codigo));
            }

            if (string.IsNullOrEmpty(descricaoCarteira))
                throw new Exception("O código da carteira não foi implementado.");

            return $"{Boleto.Carteira} - {descricaoCarteira}";
        }

        private static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }

        #endregion Html

        #region Geração do Html OffLine

        /// <summary>
        /// Função utilizada para gerar o html do boleto sem que o mesmo esteja dentro de uma página Web.
        /// </summary>
        /// <param name="startText">Texto inicial.</param>
        /// <param name="srcLogo">Local apontado pela imagem de logo.</param>
        /// <param name="srcBarra">Local apontado pela imagem de barra.</param>
        /// <param name="srcCodigoBarra">Local apontado pela imagem do código de barras.</param>
        /// <param name="usaCssPdf">True se usa o estilo específico para o pdf.</param>
        /// <returns>StringBuilder conténdo o código html do boleto bancário.</returns>
        protected StringBuilder HtmlOffLine(string startText, string srcLogo, string srcBarra, string srcCodigoBarra, bool usaCssPdf = false)
        {
            OnLoad(EventArgs.Empty);

            var html = new StringBuilder();
            HtmlOfflineHeader(html, usaCssPdf);

            if (!string.IsNullOrEmpty(startText))
                html.Append(startText);

            html.Append(MontaHtml(srcLogo, srcBarra, "<img src=\"" + srcCodigoBarra + "\" alt=\"Código de Barras\" />"));
            HtmlOfflineFooter(html);
            return html;
        }

        /// <summary>
        /// Monta o Header de um email com pelo menos um boleto dentro.
        /// </summary>
        /// <param name="html">StringBuilder onde o conteudo será salvo. Não é necessário retornar o objeto, por causa da referência.</param>
        /// <param name="usaCssPdf">True se usa o CSS secundário, com estilos para o pdf.</param>
        protected static void HtmlOfflineHeader(StringBuilder html, bool usaCssPdf = false)
        {
            html.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n");
            html.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">\n");
            html.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\n");
            html.Append("<meta charset=\"utf-8\"/>\n");
            html.Append("<head>");
            html.Append("    <title>Boleto.Net</title>\n");

            #region Css

            var arquivoCss = usaCssPdf ? "BoletoNet.BoletoImpressao.BoletoNetPDF.css" : "BoletoNet.BoletoImpressao.BoletoNet.css";

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(arquivoCss))
            {
                if (stream != null)
                    using (var sr = new StreamReader(stream))
                    {
                        html.Append("<style>\n");
                        html.Append(sr.ReadToEnd());
                        html.Append("</style>\n");
                    }
            }

            #endregion Css

            html.Append("     </head>\n");
            html.Append("<body>\n");
        }

        /// <summary>
        /// Monta o Footer de um email com pelo menos um boleto dentro.
        /// </summary>
        /// <param name="saida">StringBuilder onde o conteudo sera salvo.</param>
        protected static void HtmlOfflineFooter(StringBuilder saida)
        {
            saida.Append("</body>\n");
            saida.Append("</html>\n");
        }

        /// <summary>
        /// Junta varios boletos em uma unica AlternateView, para todos serem mandados juntos no mesmo email
        /// </summary>
        /// <param name="arrayDeBoletos">Array contendo os boletos a serem mesclados</param>
        /// <returns></returns>
        public static AlternateView GeraHtmlDeVariosBoletosParaEmail(BoletoBancario[] arrayDeBoletos)
        {
            return GeraHtmlDeVariosBoletosParaEmail(null, arrayDeBoletos);
        }

        ///<summary>
        ///Junta vários boletos em uma unica AlternateView, para todos serem mandados juntos no mesmo email.
        ///</summary>
        ///<param name="textoNoComecoDoEmail">Texto em HTML a ser adicionado no comeco do email.</param>
        ///<param name="arrayDeBoletos">Array contendo os boletos a serem mesclados.</param>
        ///<returns>AlternateView com os dados de todos os boleto.</returns>
        public static AlternateView GeraHtmlDeVariosBoletosParaEmail(string textoNoComecoDoEmail, BoletoBancario[] arrayDeBoletos)
        {
            var corpoDoEmail = new StringBuilder();
            var linkedResources = new List<LinkedResource>();

            HtmlOfflineHeader(corpoDoEmail);

            if (!string.IsNullOrEmpty(textoNoComecoDoEmail))
                corpoDoEmail.Append(textoNoComecoDoEmail);

            foreach (var umBoleto in arrayDeBoletos.Where(umBoleto => umBoleto != null))
            {
                LinkedResource lrImagemLogo;
                LinkedResource lrImagemBarra;
                LinkedResource lrImagemCodigoBarra;
                umBoleto.GeraGraficosParaEmailOffLine(out lrImagemLogo, out lrImagemBarra, out lrImagemCodigoBarra);

                var theOutput = umBoleto.MontaHtml("cid:" + lrImagemLogo.ContentId, "cid:" + lrImagemBarra.ContentId,
                    "<img src=\"cid:" + lrImagemCodigoBarra.ContentId + "\" alt=\"Código de Barras\" />");

                corpoDoEmail.Append(theOutput);

                linkedResources.Add(lrImagemLogo);
                linkedResources.Add(lrImagemBarra);
                linkedResources.Add(lrImagemCodigoBarra);
            }

            HtmlOfflineFooter(corpoDoEmail);

            var av = AlternateView.CreateAlternateViewFromString(corpoDoEmail.ToString(), Encoding.Default, "text/html");

            foreach (var theResource in linkedResources)
                av.LinkedResources.Add(theResource);

            return av;
        }

        /// <summary>
        /// Função utilizada gerar o AlternateView necessário para enviar um boleto bancário por e-mail.
        /// </summary>
        /// <returns>AlternateView com os dados do boleto.</returns>
        public AlternateView HtmlBoletoParaEnvioEmail()
        {
            return HtmlBoletoParaEnvioEmail(null);
        }

        /// <summary>
        /// Função utilizada gerar o AlternateView necessário para enviar um boleto bancário por e-mail.
        /// </summary>
        /// <param name="textoNoComecoDoEmail">Texto (em HTML) a ser incluido no começo do Email.</param>
        /// <returns>AlternateView com os dados do boleto.</returns>
        public AlternateView HtmlBoletoParaEnvioEmail(string textoNoComecoDoEmail)
        {
            LinkedResource lrImagemLogo;
            LinkedResource lrImagemBarra;
            LinkedResource lrImagemCodigoBarra;

            GeraGraficosParaEmailOffLine(out lrImagemLogo, out lrImagemBarra, out lrImagemCodigoBarra);
            var html = HtmlOffLine(textoNoComecoDoEmail, "cid:" + lrImagemLogo.ContentId, "cid:" + lrImagemBarra.ContentId, "cid:" + lrImagemCodigoBarra.ContentId);

            var av = AlternateView.CreateAlternateViewFromString(html.ToString(), Encoding.Default, "text/html");

            av.LinkedResources.Add(lrImagemLogo);
            av.LinkedResources.Add(lrImagemBarra);
            av.LinkedResources.Add(lrImagemCodigoBarra);
            return av;
        }

        /// <summary>
        /// Gera as tres imagens necessárias para o Boleto
        /// </summary>
        /// <param name="lrImagemLogo">O Logo do Banco</param>
        /// <param name="lrImagemBarra">A Barra Horizontal</param>
        /// <param name="lrImagemCodigoBarra">O Código de Barras</param>
        void GeraGraficosParaEmailOffLine(out LinkedResource lrImagemLogo, out LinkedResource lrImagemBarra, out LinkedResource lrImagemCodigoBarra)
        {
            OnLoad(EventArgs.Empty);

            var randomSufix = new Random().Next().ToString(); // para podermos colocar no mesmo email varios boletos diferentes

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoletoNet.Imagens." + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg");
            lrImagemLogo = new LinkedResource(stream, MediaTypeNames.Image.Jpeg);
            lrImagemLogo.ContentId = "logo" + randomSufix;

            var ms = new MemoryStream(Utils.ConvertImageToByte(Html.barra));
            lrImagemBarra = new LinkedResource(ms, MediaTypeNames.Image.Gif);
            lrImagemBarra.ContentId = "barra" + randomSufix; ;

            var cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);
            ms = new MemoryStream(Utils.ConvertImageToByte(cb.ToBitmap()));

            lrImagemCodigoBarra = new LinkedResource(ms, MediaTypeNames.Image.Gif);
            lrImagemCodigoBarra.ContentId = "codigobarra" + randomSufix; ;
        }

        /// <summary>
        /// Função utilizada para gravar em um arquivo local o conteúdo do boleto. Este arquivo pode ser aberto em um browser sem que o site esteja no ar.
        /// </summary>
        /// <param name="fileName">Path do arquivo que deve conter o código html.</param>
        public void MontaHtmlNoArquivoLocal(string fileName)
        {
            using (var f = new FileStream(fileName, FileMode.Create))
            {
                var w = new StreamWriter(f, Encoding.Default);
                w.Write(MontaHtml());
                w.Close();
                f.Close();
            }
        }

        /// <summary>
        /// Monta o Html do boleto bancário
        /// </summary>
        /// <returns>string</returns>
        public string MontaHtml()
        {
            return MontaHtml(null, null);
        }

        /// <summary>
        /// Monta o Html do boleto bancário
        /// </summary>
        /// <param name="fileName">Caminho do arquivo</param>
        /// <param name="logoCedente">Caminho do logo do cedente</param>
        /// <returns>Html do boleto gerado</returns>
        public string MontaHtml(string fileName, string logoCedente)
        {
            if (fileName == null)
                fileName = Path.GetTempPath();

            if (logoCedente != null)
                Logo = logoCedente;

            OnLoad(EventArgs.Empty);

            var fnLogo = fileName + @"BoletoNet" + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg";

            if (!File.Exists(fnLogo))
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoletoNet.Imagens." + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg"))
                {
                    using (Stream file = File.Create(fnLogo))
                        CopiarStream(stream, file);
                }
            }

            var fnBarra = fileName + @"BoletoNetBarra.gif";

            if (!File.Exists(fnBarra))
            {
                var imgConverter = new ImageConverter();
                var imgBuffer = (byte[])imgConverter.ConvertTo(Html.barra, typeof(byte[]));

                using (var ms = new MemoryStream(imgBuffer))
                {
                    using (Stream stream = File.Create(fnBarra))
                    {
                        CopiarStream(ms, stream);
                        ms.Flush();
                    }
                }
            }

            var fnCodigoBarras = Path.GetTempFileName();
            var cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);
            cb.ToBitmap().Save(fnCodigoBarras);

            //return HtmlOffLine(fnCorte, fnLogo, fnBarra, fnPonto, fnBarraInterna, fnCodigoBarras).ToString();
            return HtmlOffLine(null, fnLogo, fnBarra, fnCodigoBarras).ToString();
        }

        /// <summary>
        /// Monta o Html do boleto bancário para View ASP.Net MVC
        /// <code>
        /// <para>Exemplo:</para>
        /// <para>public ActionResult VisualizarBoleto(string Id)</para>
        /// <para>{</para>
        /// <para>    BoletoBancario bb = new BoletoBancario();</para>
        /// <para>    //...</para>
        /// <para>    ViewBag.Boleto = bb.MontaHtml("/Content/Boletos/", "teste1");</para>
        /// <para>    return View();</para>
        /// <para>}</para>
        /// <para>//Na view</para>
        /// <para>@{Layout = null;}@Html.Raw(ViewBag.Boleto)</para>
        /// </code>
        /// </summary>
        /// <param name="url">Pasta dos boletos. Exemplo MontaHtml("/Content/Boletos/", "000100")</param>
        /// <param name="fileName">Nome do arquivo para o boleto</param>
        /// <returns>Html do boleto gerado</returns>
        /// <desenvolvedor>Sandro Ribeiro</desenvolvedor>
        /// <criacao>16/11/2012</criacao>
        public string MontaHtml(string url, string fileName, bool useMapPathSecure = true)
        {
            //Variável para o caminho físico do servidor
            var pathServer = "";

            //Verifica se o usuário informou uma url válida
            if (url == null)
            {
                //Obriga o usuário a especificar uma url válida
                throw new ArgumentException("Você precisa informar uma pasta padrão.");
            }

            if (useMapPathSecure)
            {
                //Verifica se o usuário usou barras no início e no final da url
                if (url.Substring(url.Length - 1, 1) != "/")
                    url = url + "/";

                if (url.Substring(0, 1) != "/")
                    url = url + "/";
                //Mapeia o caminho físico dos arquivos
                pathServer = MapPathSecure($"~{url}");
            }

            //Verifica se o caminho existe
            if (!Directory.Exists(pathServer))
                throw new ArgumentException("O caminho físico '{0}' não existe.", pathServer);

            //Verifica o nome do arquivo
            if (string.IsNullOrEmpty(fileName))
                fileName = DateTime.Now.Ticks.ToString();

            //Mantive o padrão 
            OnLoad(EventArgs.Empty);

            //Prepara o arquivo da logo para ser salvo
            var fnLogo = pathServer + @"BoletoNet" + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg";
            //Prepara o arquivo da logo para ser usado no html
            var fnLogoUrl = url + @"BoletoNet" + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg";

            //Salvo a imagem apenas 1 vez com o código do banco
            if (!File.Exists(fnLogo))
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BoletoNet.Imagens." + Utils.FormatCode(_ibanco.Codigo.ToString(), 3) + ".jpg"))
                {
                    using (Stream file = File.Create(fnLogo))
                        CopiarStream(stream, file);
                }
            }

            //Prepara o arquivo da barra para ser salvo
            var fnBarra = pathServer + @"BoletoNetBarra.gif";
            //Prepara o arquivo da barra para ser usado no html
            var fnBarraUrl = url + @"BoletoNetBarra.gif";

            //Salvo a imagem apenas 1 vez
            if (!File.Exists(fnBarra))
            {
                var imgConverter = new ImageConverter();
                var imgBuffer = (byte[])imgConverter.ConvertTo(Html.barra, typeof(byte[]));

                using (var ms = new MemoryStream(imgBuffer))
                {
                    using (Stream stream = File.Create(fnBarra))
                    {
                        CopiarStream(ms, stream);
                        ms.Flush();
                        ms.Dispose();
                    }
                }
            }

            //Prepara o arquivo do código de barras para ser salvo
            var fnCodigoBarras = string.Format("{0}{1}_codigoBarras.jpg", pathServer, fileName);
            //Prepara o arquivo do código de barras para ser usado no html
            var fnCodigoBarrasUrl = string.Format("{0}{1}_codigoBarras.jpg", url, fileName);

            var cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);

            //Salva o arquivo conforme o fileName
            cb.ToBitmap().Save(fnCodigoBarras);

            //Retorna o Html para ser usado na view
            return HtmlOffLine(null, fnLogoUrl, fnBarraUrl, fnCodigoBarrasUrl).ToString();
        }

        /// <summary>
        /// Monta o Html do boleto bancário com as imagens embutidas no conteúdo, sem necessidade de links externos
        /// de acordo com o padrão http://en.wikipedia.org/wiki/Data_URI_scheme
        /// </summary>
        /// <param name="convertLinhaDigitavelToImage">Converte a Linha Digitável para imagem, com o objetivo de evitar malwares.</param>
        /// <param name="usaCssPdf">True se usa o Css específico para o Pdf.</param>
        /// <returns>Html do boleto gerado</returns>
        /// <desenvolvedor>Iuri André Stona</desenvolvedor>
        /// <criacao>23/01/2014</criacao>
        /// <alteracao>08/08/2014</alteracao>
        public string MontaHtmlEmbedded(bool convertLinhaDigitavelToImage = false, bool usaCssPdf = false)
        {
            OnLoad(EventArgs.Empty);

            var assembly = Assembly.GetExecutingAssembly();

            var base64Logo = Convert.ToBase64String(ObterLogoDoBanco(CodigoBanco));
            var fnLogo = string.Format("data:image/gif;base64,{0}", base64Logo);

            string base64Barra;
            using (var streamBarra = assembly.GetManifestResourceStream("BoletoNet.Imagens.barra.gif"))
                base64Barra = Convert.ToBase64String(new BinaryReader(streamBarra).ReadBytes((int)streamBarra.Length));

            var fnBarra = string.Format("data:image/gif;base64,{0}", base64Barra);

            var cb = new C2of5i(Boleto.CodigoBarra.Codigo, 1, 50, Boleto.CodigoBarra.Codigo.Length);
            var base64CodigoBarras = Convert.ToBase64String(cb.ToByte());
            var fnCodigoBarras = string.Format("data:image/gif;base64,{0}", base64CodigoBarras);

            if (convertLinhaDigitavelToImage)
            {
                var linhaDigitavel = Boleto.CodigoBarra.LinhaDigitavel.Replace("  ", " ").Trim();

                string base64Linha;
                using (var imagemLinha = Utils.DrawText(linhaDigitavel, new Font("Arial", 30, FontStyle.Bold), Color.Black, Color.White))
                    base64Linha = Convert.ToBase64String(Utils.ConvertImageToByte(imagemLinha));

                var fnLinha = string.Format("data:image/gif;base64,{0}", base64Linha);

                Boleto.CodigoBarra.LinhaDigitavel = @"<img style=""max-width:420px; margin-bottom: 2px"" src=" + fnLinha + " />";
            }

            var s = HtmlOffLine(null, fnLogo, fnBarra, fnCodigoBarras, usaCssPdf).ToString();

            if (convertLinhaDigitavelToImage)
                s = s.Replace(".w500", "");

            return s;
        }

        public byte[] MontaBytesPdf(bool convertLinhaDigitavelToImage = false)
        {
            var converter = new NReco.PdfGenerator.HtmlToPdfConverter();

            if (!string.IsNullOrWhiteSpace(TempFilesPath))
                converter.TempFilesPath = TempFilesPath;

            if (!string.IsNullOrEmpty(PdfToolPath))
                converter.PdfToolPath = PdfToolPath;

            return converter.GeneratePdf(MontaHtmlEmbedded(convertLinhaDigitavelToImage, true));
        }

        /// <summary>
        /// Lista de Boletos, objetos do tipo
        /// BoletoBancario
        /// </summary>
        /// <param name="boletos">Lista de Boletos, objetos do tipo BoletoBancario</param>
        /// <param name="title">Título Que aparecerá na Aba do Navegador</param>
        /// <param name="args">Custom WkHtmlToPdf global options</param>
        /// <param name="header">Título No Início do PDF</param>
        /// <param name="grayscale">Preto e Branco = true</param>
        /// <param name="convertLinhaDigitavelToImage">bool Converter a Linha Digitavel Em Imagem</param>
        /// <returns>byte[], Vetor de bytes do PDF</returns>
        public byte[] MontaBytesListaBoletosPdf(List<BoletoBancario> boletos, string title = "", string args = "", string header = "", bool grayscale = false, bool convertLinhaDigitavelToImage = false)
        {
            var htmlBoletos = new StringBuilder();
            htmlBoletos.Append("<html><head><title>");
            htmlBoletos.Append(title);
            htmlBoletos.Append("</title><style type='text/css' media='screen,print'>");
            htmlBoletos.Append(".break{ display: block; clear: both; page-break-after: always;}");
            htmlBoletos.Append("</style></head><body>");

            if (!string.IsNullOrEmpty(header))
            {
                htmlBoletos.Append("<br/><center><h1>");
                htmlBoletos.Append(header);
                htmlBoletos.Append("</h1></center><br/>");
            }

            var index = 1;
            foreach (var boleto in boletos)
            {
                if (boleto.FormatoCarne)
                    boleto.OcultarInstrucoes = true;

                if ((boleto.FormatoCarne && index % 3 == 0) || !boleto.FormatoCarne)
                    htmlBoletos.Append("<div class='break'>");
                else
                    htmlBoletos.Append("<div>");

                htmlBoletos.Append(boleto.MontaHtmlEmbedded(convertLinhaDigitavelToImage, true));
                htmlBoletos.Append("</div>");

                index++;
            }

            htmlBoletos.Append("</body></html>");

            var converter = new NReco.PdfGenerator.HtmlToPdfConverter
            {
                CustomWkHtmlArgs = args,
                Grayscale = grayscale
            };

            if (!string.IsNullOrEmpty(PdfToolPath))
                converter.PdfToolPath = PdfToolPath;

            if (!string.IsNullOrEmpty(TempFilesPath))
                converter.TempFilesPath = TempFilesPath;

            return converter.GeneratePdf(htmlBoletos.ToString());
        }

        #endregion Geração do Html OffLine

        public Image GeraImagemCodigoBarras(Boleto boleto)
        {
            var cb = new C2of5i(boleto.CodigoBarra.Codigo, 1, 50, boleto.CodigoBarra.Codigo.Length);
            return cb.ToBitmap();
        }

        /// <summary>
        /// Obtem o array de bytes da logo do banco.
        /// </summary>
        /// <returns>bytes da logo</returns>
        public byte[] ObterLogoDoBanco(short codigoBanco)
        {
            if (!string.IsNullOrWhiteSpace(Logo) && File.Exists(Logo))
                return File.ReadAllBytes(Logo);

            var assembly = Assembly.GetExecutingAssembly();
            var streamLogo = assembly.GetManifestResourceStream(string.Format("BoletoNet.Imagens.{0:000}.jpg", codigoBanco));

            return new BinaryReader(streamLogo).ReadBytes((int)streamLogo.Length);
        }

        private void CopiarStream(Stream entrada, Stream saida)
        {
            var bytesLidos = 0;
            var imgBuffer = new byte[entrada.Length];

            while ((bytesLidos = entrada.Read(imgBuffer, 0, imgBuffer.Length)) > 0)
            {
                saida.Write(imgBuffer, 0, bytesLidos);
            }
        }

        /// <summary>
        /// Ajusta o tamanho dos textos e labels do boleto bancário.
        /// </summary>
        /// <param name="tamanhoFonteTextos">Padrão 10px</param>
        /// <param name="tamanhoFonteRotulos">Padrão 9.8px. É o maior tamanho sem que o exista uma quebra de linha no rótulo "Data processamento"</param>
        /// <param name="tamanhoFonteInstrucaoImpressao">Padrão 10px</param>
        /// <param name="tamanhoFonteInstrucoes">Padrão 10px</param>
        public void AjustaTamanhoFonte(double tamanhoFonteTextos = 10, double tamanhoFonteRotulos = 9.8, double tamanhoFonteInstrucaoImpressao = 9, double tamanhoFonteInstrucoes = 10)
        {
            _ajustaTamanhoFonte = true;

            var html = new StringBuilder();

            html.AppendLine("<style>");
            html.AppendFormat(".cp$1 font-size: {0}px !important; $2", tamanhoFonteTextos);
            html.AppendFormat(".ctN$1 font-size: {0}px !important; $2", tamanhoFonteRotulos);
            html.AppendFormat(".cpN$1 font-size: {0}px !important; $2", tamanhoFonteTextos);
            html.AppendFormat(".ti$1 font-size: {0}px !important; $2", tamanhoFonteInstrucaoImpressao);
            html.AppendFormat(".ct$1 font-size: {0}px !important; $2", tamanhoFonteRotulos);
            html.AppendFormat(".t$1 font-size: {0}px !important; $2", tamanhoFonteRotulos);
            html.AppendFormat(".it$1 font-size: {0}px !important; $2", tamanhoFonteInstrucoes);
            html.AppendLine("</style>");

            _ajustaTamanhoFonteHtml = html.ToString().Replace("$1", "{").Replace("$2", "}");
        }

        public static string UrlLogo(int banco)
        {
            var page = System.Web.HttpContext.Current.CurrentHandler as Page;

            return page.ClientScript.GetWebResourceUrl(typeof(BoletoBancario), "BoletoNet.Imagens." + Utils.FormatCode(banco.ToString(), 3) + ".jpg");
        }

        public byte[] BoletosEmPdf(List<BoletoBancario> boletos, string title = "", string args = "", string header = "", bool grayscale = false, bool convertLinhaDigitavelToImage = false)
        {
            var htmlBoletos = new StringBuilder();
            htmlBoletos.Append("<html><head><title>");
            htmlBoletos.Append(title);
            htmlBoletos.Append("</title><style type='text/css' media='screen,print'>");
            htmlBoletos.Append(".break{ display: block; clear: both; page-break-after: always;}");
            htmlBoletos.Append("</style></head><body>");

            if (!string.IsNullOrEmpty(header))
            {
                htmlBoletos.Append("<br/><center><h1>");
                htmlBoletos.Append(header);
                htmlBoletos.Append("</h1></center><br/>");
            }

            foreach (var boleto in boletos)
            {
                htmlBoletos.Append("<div class='break'>");
                htmlBoletos.Append(boleto.MontaHtmlEmbedded(convertLinhaDigitavelToImage, true));
                htmlBoletos.Append("</div>");
            }

            htmlBoletos.Append("</body></html>");

            var converter = new NReco.PdfGenerator.HtmlToPdfConverter
            {
                CustomWkHtmlArgs = args,
                Grayscale = grayscale
            };

            if (!string.IsNullOrEmpty(PdfToolPath))
                converter.PdfToolPath = PdfToolPath;

            if (!string.IsNullOrEmpty(TempFilesPath))
                converter.TempFilesPath = TempFilesPath;

            return converter.GeneratePdf(htmlBoletos.ToString());
        }
    }
}
