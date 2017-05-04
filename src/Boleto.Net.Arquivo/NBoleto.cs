using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BoletoNet.Arquivo
{
    public partial class NBoleto : Form
    {
        private short _codigoBanco = 0;
        private Progresso _progresso;
        private string _arquivo = string.Empty;
        private readonly ImpressaoBoleto _impressaoBoleto = new ImpressaoBoleto();

        public short CodigoBanco
        {
            get { return _codigoBanco; }
            set { _codigoBanco = value; }
        }

        public NBoleto()
        {
            InitializeComponent();

            _impressaoBoleto.FormClosed += ImpressaoBoleto_FormClosed;
        }

        void ImpressaoBoleto_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
            Dispose();
        }

        private void GeraLayout(IEnumerable<BoletoBancario> boletos)
        {
            var html = new StringBuilder();
            foreach (var o in boletos)
            {
                html.Append(o.MontaHtml());
                //html.Append("</br></br></br></br></br></br></br></br></br></br>");
            }

            _arquivo = Path.GetTempFileName();

            using (var f = new FileStream(_arquivo, FileMode.Create))
            {
                var w = new StreamWriter(f, Encoding.UTF8);
                w.Write(html.ToString());
                w.Close();
                f.Close();
            }
        }

        private void GeraBoletoCaixa(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais

            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {

                var bb = new BoletoBancario
                {
                    CodigoBanco = _codigoBanco,
                    MostrarEnderecoCedente = true
                };
                var vencimento = DateTime.Now.AddDays(10);

                var item1 = new Instrucao_Caixa(9, null, 5);
                var item2 = new Instrucao_Caixa(81, null, 10);
                var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0132", "00542");

                var b = new Boleto(vencimento, 460, "SR", "00000000010001", c);
                var endCed = new Endereco();

                endCed.End = "Rua Testando o Boleto";
                endCed.Bairro = "BairroTest";
                endCed.Cidade = "CidadeTes";
                endCed.Cep = "70000000";
                endCed.Uf = "MG";
                b.Cedente.Endereco = endCed;

                b.NumeroDocumento = Convert.ToString(1000 + i);

                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23ddddddddddddddddddddddddddd";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.Cep = "70000000";
                b.Sacado.Endereco.Uf = "DF";

                b.Instrucoes.Add(item1);
                b.Instrucoes.Add(item2);

                // juros/descontos
                if (b.ValorDesconto == 0)
                {
                    var item3 = new Instrucao_Caixa(999, null, 1);
                    b.Instrucoes.Add(item3);
                }

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }

        private void GeraBoletoItau(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais

            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {

                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var vencimento = DateTime.Now.AddDays(10);

                var item1 = new Instrucao_Itau(9, 5);
                var item2 = new Instrucao_Itau(81, 10);
                var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0542", "13000");
                //Na carteira 198 o código do Cedente é a conta bancária
                c.Codigo = "13000";

                var b = new Boleto(vencimento, 1642, "198", "92082835", c, new EspecieDocumento(341, "1"));
                b.NumeroDocumento = Convert.ToString(10000 + i);

                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.Cep = "70000000";
                b.Sacado.Endereco.Uf = "DF";

                item2.Descricao += " " + item2.Dias + " dias corridos do vencimento.";
                b.Instrucoes.Add(item1);
                b.Instrucoes.Add(item2);

                // juros/descontos

                if (b.ValorDesconto == 0)
                {
                    var item3 = new Instrucao_Itau(999, 1);
                    item3.Descricao += ("1,00 por dia de antecipação.");
                    b.Instrucoes.Add(item3);
                }

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }

        public void GeraBoletoUnibanco(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais

            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {

                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var vencimento = DateTime.Now.AddDays(10);

                var instr = new Instrucao(001);
                var c = new Cedente("00.000.000/0000-00", "Next Consultoria Ltda.", "0123", "100618", "9");
                c.Codigo = "203167";

                var b = new Boleto(vencimento, 2952.95m, "20", "1803029901", c);
                b.NumeroDocumento = b.NossoNumero;

                b.Sacado = new Sacado("000.000.000-00", "Marlon Oliveira");
                b.Sacado.Endereco.End = "Rua Dr. Henrique Portugal, XX";
                b.Sacado.Endereco.Bairro = "São Francisco";
                b.Sacado.Endereco.Cidade = "Niterói";
                b.Sacado.Endereco.Cep = "24360080";
                b.Sacado.Endereco.Uf = "RJ";
                b.Sacado.Endereco.Logradouro = "Rua Dr. Henrique Portugal";
                b.Sacado.Endereco.Numero = "XX";
                b.Sacado.Endereco.Complemento = "Casa";

                instr.Descricao = "Não Receber após o vencimento";
                b.Instrucoes.Add(instr);

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }

        public void GeraBoletoSudameris(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais

            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {
                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var vencimento = DateTime.Now.AddDays(10);
                var instr = new Instrucao(001);

                var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0501", "6703255");
                c.Codigo = "13000";

                //Nosso número com 7 dígitos
                var nn = "0003020";
                //Nosso número com 13 dígitos
                //nn = "0000000003025";

                var b = new Boleto(vencimento, 1642, "198", nn, c);// EnumEspecieDocumento_Sudameris.DuplicataMercantil);
                b.NumeroDocumento = "1008073";

                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.Cep = "70000000";
                b.Sacado.Endereco.Uf = "DF";

                instr.Descricao = "Não Receber após o vencimento";
                b.Instrucoes.Add(instr);

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }

        public void GeraBoletoSafra(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais
            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {
                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var vencimento = DateTime.Now.AddDays(10);
                var instr = new Instrucao(001);

                var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0542", "5413000");
                c.Codigo = "13000";

                var b = new Boleto(vencimento, 1642, "198", "02592082835", c);
                b.NumeroDocumento = "1008073";

                b.Sacado = new Sacado("000.000.000-00", "Eduardo Frare");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.Cep = "70000000";
                b.Sacado.Endereco.Uf = "DF";

                instr.Descricao = "Não Receber após o vencimento";
                b.Instrucoes.Add(instr);

                bb.Boleto = b;
                bb.Boleto.Valida();
                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }

        public void GeraBoletoReal(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais
            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {
                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var vencimento = DateTime.Now.AddDays(10);
                var instr = new Instrucao(001);
                var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0542", "13000");
                c.Codigo = "13000";

                var b = new Boleto(vencimento, 1642, "57", "92082835", c);
                b.NumeroDocumento = "1008073";

                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.Cep = "70000000";
                b.Sacado.Endereco.Uf = "DF";

                instr.Descricao = "Não Receber após o vencimento";
                b.Instrucoes.Add(instr);

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }

        public void GeraBoletoHsbc(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais
            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {
                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var vencimento = DateTime.Now.AddDays(10);
                var instr = new Instrucao(001);
                var c = new Cedente("00.000.000/0000-00", "Minha empresa", "0000", "", "00000", "00");
                // Código fornecido pela agencia, NÃO é o numero da conta
                c.Codigo = "0000000"; // 7 posicoes

                var b = new Boleto(vencimento, 2, "CNR", "888888888", c); //cod documento
                b.NumeroDocumento = "9999999999999"; // nr documento

                b.Sacado = new Sacado("000.000.000-00", "Fulano de Tal");
                b.Sacado.Endereco.End = "lala";
                b.Sacado.Endereco.Bairro = "lala";
                b.Sacado.Endereco.Cidade = "Curitiba";
                b.Sacado.Endereco.Cep = "82000000";
                b.Sacado.Endereco.Uf = "PR";

                instr.Descricao = "Não Receber após o vencimento";
                b.Instrucoes.Add(instr);

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }

        public void GeraBoletoBancoBrasil(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais
            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {
                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var vencimento = DateTime.Now.AddDays(10);
                var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "1234", "5", "12345678", "9");

                c.Codigo = "00000000504";
                var b = new Boleto(vencimento, 45.50m, "11", "12345678901", c);                
                
                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.Cep = "70000000";
                b.Sacado.Endereco.Uf = "DF";

                b.EspecieDocumento = new EspecieDocumento(Convert.ToInt16(1), "01");

                //Adiciona as instruções ao boleto
                //Protestar
                //var item = new Instrucao_BancoBrasil(9, 5);
                //b.Instrucoes.Add(item);
                ////ImportanciaporDiaDesconto
                //item = new Instrucao_BancoBrasil(30, 0);
                //b.Instrucoes.Add(item);
                ////ProtestarAposNDiasCorridos
                //item = new Instrucao_BancoBrasil(81, 15);
                //b.Instrucoes.Add(item);

                b.Instrucoes.Add(new Instrucao(b.EspecieDocumento.Banco.Codigo, 0, "Ha", 5, 1));

                b.NumeroDocumento = "12345678901";

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }
        
        public void GeraBoletoBradesco(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais
            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {
                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var vencimento = DateTime.Now.AddDays(10);
                var item = new Instrucao_Bradesco(9, null, 5);

                var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "1234", "5", "123456", "7");
                c.Codigo = "13000";

                var end = new Endereco();
                end.Bairro = "Lago Sul";
                end.Cep = "71666660";
                end.Cidade = "Brasília- DF";
                end.Complemento = "Quadra XX Conjunto XX Casa XX";
                end.End = "Condominio de Brasilia - Quadra XX Conjunto XX Casa XX";
                end.Logradouro = "Cond. Brasilia";
                end.Numero = "55";
                end.Uf = "DF";

                var b = new Boleto(vencimento, 1.01m, "02", "01000000001", c);
                b.NumeroDocumento = "01000000001";

                b.Sacado = new Sacado("000.000.000-00", "Eduardo Frare");
                b.Sacado.Endereco = end;

                item.Descricao += " após " + item.Dias + " dias corridos do vencimento.";
                b.Instrucoes.Add(item); //"Não Receber após o vencimento");

                bb.FormatoCarne = true;
                bb.OcultarInstrucoes = true;
                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }

        public void GeraBoletoBnb(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais

            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {
                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var conta = new ContaBancaria();
                conta.Agencia = "21";
                conta.DigitoAgencia = "0";
                conta.Conta = "12717";
                conta.DigitoConta = "8";

                var c = new Cedente();
                c.ContaBancaria = conta;
                c.CpfCnpj = "00.000.000/0000-00";
                c.Nome = "Empresa de Atacado";

                var b = new Boleto();
                b.Cedente = c;
                //
                b.DataProcessamento = DateTime.Now;
                b.DataVencimento = DateTime.Now.AddDays(15);
                b.ValorBoleto = Convert.ToDecimal(1);
                b.Carteira = "4";
                b.NossoNumero = "7777777";
                b.NumeroDocumento = "2525";
                //
                b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
                b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
                b.Sacado.Endereco.Bairro = "Testando";
                b.Sacado.Endereco.Cidade = "Testelândia";
                b.Sacado.Endereco.Cep = "70000000";
                b.Sacado.Endereco.Uf = "RS";

                b.Banco = new Banco(004);

                var especiedocumento = new EspecieDocumento(004, "1");//Duplicata Mercantil
                b.EspecieDocumento = especiedocumento;

                bb.Boleto = b;
                bb.Boleto.Valida();
                boletos.Add(bb);
            }
            GeraLayout(boletos);


        }

        public void GeraBoletoSicredi(int qtde)
        {
            // Cria o boleto, e passa os parâmetros usuais
            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {
                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var vencimento = DateTime.Now.AddDays(10);

                var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "1234", "5", "12345", "7");
                //c.Codigo = "13000";

                var end = new Endereco();
                end.Bairro = "Lago Sul";
                end.Cep = "71666660";
                end.Cidade = "Brasília- DF";
                end.Complemento = "Quadra XX Conjunto XX Casa XX";
                end.End = "Condominio de Brasilia - Quadra XX Conjunto XX Casa XX";
                end.Logradouro = "Cond. Brasilia";
                end.Numero = "55";
                end.Uf = "DF";

                var b = new Boleto(vencimento, 1.01m, "02", "01000000001", c);
                b.NumeroDocumento = "01000000001";

                b.Sacado = new Sacado("000.000.000-00", "Eduardo Frare");
                b.Sacado.Endereco = end;

                b.NossoNumero = "17200002";

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }

        public void GeraBoletoBanrisul(int qtde)
        {
            //Cria o boleto, e passa os parâmetros usuais.
            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {
                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var item = new Instrucao_Bradesco(9, null, 5);

                var c = new Cedente("11.111.111/1111-11", "Empresa de Atacado", "1102", "48", "9000150", "46");
                c.Codigo = "9000150";
                c.MostrarCnpjNoBoleto = true;
                
                var end = new Endereco();
                end.Bairro = "Lago Sul";
                end.Cep = "71666660";
                end.Cidade = "Brasília- DF";
                end.Complemento = "Quadra XX Conjunto XX Casa XX";
                end.End = "Condominio de Brasilia - Quadra XX Conjunto XX Casa XX";
                end.Logradouro = "Cond. Brasilia";
                end.Numero = "55";
                end.Uf = "DF";

                var b = new Boleto(new DateTime(2010, 07, 04), 550m, "01", "22832563", c);
                b.DigitoNossoNumero = "51";

                b.Sacado = new Sacado("000.000.000-00", "Eduardo Frare");
                b.Sacado.Endereco = end;

                item.Descricao += " após " + item.Dias + " dias corridos do vencimento.";
                b.Instrucoes.Add(item); //"Não Receber após o vencimento");

                bb.FormatoCarne = false;
                bb.OcultarInstrucoes = true;
                bb.ExibirDemonstrativo = true;
                bb.MostrarComprovanteEntrega = true;
                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }
        
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            switch (CodigoBanco)
            {
                case 1: // Banco do Brasil
                    GeraBoletoBancoBrasil((int)numericUpDown.Value);
                    break;

                case 041: // Banrisul
                    GeraBoletoBanrisul((int)numericUpDown.Value);
                    break;

                case 409: // Unibanco
                    GeraBoletoUnibanco((int)numericUpDown.Value);
                    break;

                case 347: // Sudameris
                    GeraBoletoSudameris((int)numericUpDown.Value);
                    break;

                case 422: // Safra
                    GeraBoletoSafra((int)numericUpDown.Value);
                    break;

                case 341: // Itau
                    GeraBoletoItau((int)numericUpDown.Value);
                    break;

                case 356: // Real
                    GeraBoletoReal((int)numericUpDown.Value);
                    break;

                case 399: // Hsbc
                    GeraBoletoItau((int)numericUpDown.Value);
                    break;

                case 237: // Bradesco
                    GeraBoletoBradesco((int)numericUpDown.Value);
                    break;

                case 104: // Caixa
                    GeraBoletoCaixa((int)numericUpDown.Value);
                    break;
                case 4: //BNB
                    GeraBoletoBnb((int)numericUpDown.Value);
                    break;

                case 748: //Sicredi.
                    GeraBoletoSicredi((int)numericUpDown.Value);
                    break;
            }

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _progresso.Close();

            // Cria um formulário com um componente WebBrowser dentro
            _impressaoBoleto.webBrowser.Navigate(_arquivo);
            _impressaoBoleto.ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((int)numericUpDown.Value > 1)
                MessageBox.Show("O exemplo de envio do boleto bancário como imagem só está implementado para somente um boleto por vez.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
            _progresso = new Progresso();
            _progresso.ShowDialog();
        }
    }
}
