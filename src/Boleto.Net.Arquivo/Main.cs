using System;
using System.IO;
using System.Windows.Forms;

namespace BoletoNet.Arquivo
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        #region Remessa

        public void GeraArquivoCNAB400(IBanco banco, Cedente cedente, Boletos boletos)
        {
            try
            {
                saveFileDialog.Filter = "Arquivos de Retorno (*.rem)|*.rem|Todos Arquivos (*.*)|*.*";

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                var arquivo = new ArquivoRemessa(TipoArquivo.Cnab400);

                //Valida a Remessa Correspondentes antes de Gerar a mesma...
                string vMsgRetorno;
                var sit = arquivo.ValidarArquivoRemessa(cedente.Convenio.ToString(), banco, cedente, boletos, 1, out vMsgRetorno);

                if (!sit)
                {
                    MessageBox.Show(String.Concat("Foram localizados inconsistências na validação da remessa!", Environment.NewLine, vMsgRetorno),
                        "Teste", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    arquivo.GerarArquivoRemessa("0", banco, cedente, boletos, saveFileDialog.OpenFile(), 1);

                    MessageBox.Show("Arquivo gerado com sucesso!", "Teste",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GeraArquivoCNAB240(IBanco banco, Cedente cedente, Boletos boletos)
        {
            saveFileDialog.Filter = "Arquivos de Retorno (*.rem)|*.rem|Todos Arquivos (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var arquivo = new ArquivoRemessa(TipoArquivo.Cnab240);
                arquivo.GerarArquivoRemessa("1200303001417053", banco, cedente, boletos, saveFileDialog.OpenFile(), 1);

                MessageBox.Show("Arquivo gerado com sucesso!", "Teste", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        public void GeraDadosItau(TipoArquivo tipoArquivo)
        {
            var vencimento = new DateTime(2007, 9, 10);

            var item1 = new Instrucao_Itau(9, 5);
            var item2 = new Instrucao_Itau(81, 10);
            var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0542", "13000");
            //Na carteira 198 o código do Cedente é a conta bancária
            c.Codigo = "13000";

            var b = new Boleto(vencimento, 1642, "198", "92082835", c);
            b.NumeroDocumento = "1008073";

            b.DataVencimento = Convert.ToDateTime("12-12-12");

            b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testelândia";
            b.Sacado.Endereco.Cep = "70000000";
            b.Sacado.Endereco.Uf = "DF";

            item2.Descricao += item2.Dias + " dias corridos do vencimento.";
            b.Instrucoes.Add(item1);
            b.Instrucoes.Add(item2);
            b.Cedente.ContaBancaria.DigitoAgencia = "1";
            b.Cedente.ContaBancaria.DigitoAgencia = "2";

            b.Banco = new Banco(341);

            var boletos = new Boletos();
            boletos.Add(b);

            var b2 = new Boleto(vencimento, 1642, "198", "92082835", c);
            b2.NumeroDocumento = "1008073";

            b2.DataVencimento = Convert.ToDateTime("12-12-12");

            b2.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b2.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b2.Sacado.Endereco.Bairro = "Testando";
            b2.Sacado.Endereco.Cidade = "Testelândia";
            b2.Sacado.Endereco.Cep = "70000000";
            b2.Sacado.Endereco.Uf = "DF";

            item2.Descricao += item2.Dias + " dias corridos do vencimento.";
            b2.Instrucoes.Add(item1);
            b2.Instrucoes.Add(item2);
            b2.Cedente.ContaBancaria.DigitoAgencia = "1";
            b2.Cedente.ContaBancaria.DigitoAgencia = "2";

            b2.Banco = new Banco(341);

            boletos.Add(b2);

            switch (tipoArquivo)
            {
                case TipoArquivo.Cnab240:
                    GeraArquivoCNAB240(b2.Banco, c, boletos);
                    break;
                case TipoArquivo.Cnab400:
                    GeraArquivoCNAB400(b2.Banco, c, boletos);
                    break;             
                default:
                    break;
            }            
                
        }

        public void GeraDadosBanrisul()
        {
            var conta = new ContaBancaria();
            conta.Agencia = "0510";
            conta.DigitoAgencia = "02";
            conta.Conta = "0013000";
            conta.DigitoConta = "30";
            
            var c = new Cedente();
            c.ContaBancaria = conta;
            c.CpfCnpj = "12.345.678/0001-11";
            c.Nome = "Empresa de Atacado";
            c.Codigo = "3560029";
            c.Convenio = 124522;
            
            var b = new Boleto();
            b.Cedente = c;
            b.ContaBancaria = conta;
            b.DataProcessamento = DateTime.Now;
            b.DataVencimento = DateTime.Now.AddDays(15);
            b.ValorBoleto = Convert.ToDecimal(2469.69);
            b.Carteira = "1";
            b.NossoNumero = "92082835";
            b.NumeroDocumento = "1008073";
            
            b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testelândia";
            b.Sacado.Endereco.Cep = "70000000";
            b.Sacado.Endereco.Uf = "RS";

            var item = new Instrucao_Banrisul(18, null, 10, 3.1m);
            b.Instrucoes.Add(item);

            b.Banco = new Banco(041);

            #region Dados para Remessa:

            b.EspecieDocumento = new EspecieDocumento(41, "01");
            b.Remessa = new Remessa
            {
                CodigoOcorrencia = "01",
            };

            #endregion

            var boletos = new Boletos();
            boletos.Add(b);

            GeraArquivoCNAB400(b.Banco, c, boletos);
        }

        public void GeraDadosBancoDoBrasil()
        {
            var conta = new ContaBancaria();
            conta.Agencia = "2768";
            conta.DigitoAgencia = "5";
            conta.Conta = "51249369";
            conta.DigitoConta = "3";

            var c = new Cedente();
            c.ContaBancaria = conta;
            c.CpfCnpj = "12.345.678/0001-11";
            c.Nome = "Empresa de Atacado";
            c.Codigo = "51249369";
            c.DigitoCedente = 3;
            c.Convenio = 2650829;
            
            var b = new Boleto();
            b.Cedente = c;
            b.ContaBancaria = conta;
            b.DataProcessamento = DateTime.Now;
            b.DataVencimento = DateTime.Now.AddDays(15);
            b.ValorBoleto = Convert.ToDecimal(2469.69);
            b.Carteira = "17";
            b.VariacaoCarteira = "019";
            b.NossoNumero = "92082835";
            b.NumeroDocumento = "1008073";

            b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testelândia";
            b.Sacado.Endereco.Cep = "70000000";
            b.Sacado.Endereco.Uf = "RS";

            var item = new Instrucao_BancoBrasil(82, null, 10, 3.1m);
            b.Instrucoes.Add(item);

            b.Banco = new Banco(001);

            #region Dados para Remessa:

            b.EspecieDocumento = new EspecieDocumento(1, "01");
            b.Remessa = new Remessa
            {
                CodigoOcorrencia = "01",
                TipoDocumento = b.NossoNumero
            };

            #endregion

            var boletos = new Boletos();
            boletos.Add(b);
            b.Valida();

            GeraArquivoCNAB400(b.Banco, c, boletos);
        }

        public void GeraDadosSicredi()
        {
            var conta = new ContaBancaria();
            conta.Agencia = "051";
            conta.DigitoAgencia = "2";
            conta.Conta = "13000";
            conta.DigitoConta = "3";
            
            var c = new Cedente();
            c.ContaBancaria = conta;
            c.CpfCnpj = "00000000000000";
            c.Nome = "Empresa de Atacado";
            //Na carteira 198 o código do Cedente é a conta bancária
            c.Codigo = "12345";//No Banrisul, esse código está no manual como 12 caracteres, por eu(sidneiklein) isso tive que alterar o tipo de int para string;
            c.Convenio = 124522;
            
            var b = new Boleto();
            b.Cedente = c;
            
            b.DataProcessamento = DateTime.Now;
            b.DataVencimento = DateTime.Now.AddDays(15);
            b.ValorBoleto = Convert.ToDecimal(2469.69);
            b.Carteira = "1";
            b.VariacaoCarteira = "02";
            b.NossoNumero = string.Empty; //"92082835"; //** Para o "Remessa.TipoDocumento = "06", não poderá ter NossoNúmero Gerado!
            b.NumeroDocumento = "1008073";
            
            b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testelândia";
            b.Sacado.Endereco.Cep = "70000000";
            b.Sacado.Endereco.Uf = "RS";

            var item1 = new Instrucao_Sicredi(9, null, 5);
            b.Instrucoes.Add(item1);
            //b.Instrucoes.Add(item2);
            b.Banco = new Banco(748);

            var especiedocumento = new EspecieDocumento(748, "A");//(341, 1);
            b.EspecieDocumento = especiedocumento;
            
            #region Dados para Remessa:
            b.Remessa = new Remessa();
            b.Remessa.TipoDocumento = "A"; //A = 'A' - SICREDI com Registro
            #endregion

            var boletos = new Boletos();
            boletos.Add(b);

            GeraArquivoCNAB400(b.Banco, c, boletos);
        }

        public void GeraDadosSantander()
        {
            var boletos = new Boletos();

            var vencimento = new DateTime(2003, 5, 15);

            var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "2269", "130000946");
            c.Codigo = "1795082";

            var b = new Boleto(vencimento, 0.20m, "101", "566612457800", c);

            //NOSSO NÚMERO
            //############################################################################################################################
            //Número adotado e controlado pelo Cliente, para identificar o título de cobrança.
            //Informação utilizada pelos Bancos para referenciar a identificação do documento objeto de cobrança.
            //Poderá conter número da duplicata, no caso de cobrança de duplicatas, número de apólice, no caso de cobrança de seguros, etc.
            //Esse campo é devolvido no arquivo retorno.
            b.NumeroDocumento = "0282033";

            b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testelândia";
            b.Sacado.Endereco.Cep = "70000000";
            b.Sacado.Endereco.Uf = "DF";

            //b.Instrucoes.Add("Não Receber após o vencimento");
            //b.Instrucoes.Add("Após o Vencimento pague somente no Bradesco");
            //b.Instrucoes.Add("Instrução 2");
            //b.Instrucoes.Add("Instrução 3");

            //Espécie Documento - [R] Recibo
            b.EspecieDocumento = new EspecieDocumento_Santander("17");

            boletos.Add(b);

            GeraArquivoCNAB240(new Banco(33), c, boletos);
        }

        public void GeraDadosCaixa()
        {
            var conta = new ContaBancaria();
            conta.OperacaConta = "OPE";
            conta.Agencia = "345";
            conta.DigitoAgencia = "6";
            conta.Conta = "87654321";
            conta.DigitoConta = "0";
            //
            var c = new Cedente();
            c.ContaBancaria = conta;
            c.CpfCnpj = "00.000.000/0000-00";
            c.Nome = "Empresa de Atacado";
            //Na carteira 198 o código do Cedente é a conta bancária
            c.Codigo = String.Concat(conta.Agencia, conta.DigitoAgencia, conta.OperacaConta, conta.Conta, conta.DigitoConta); //Na Caixa, esse código está no manual como 16 caracteres AAAAOOOCCCCCCCCD;
            //
            var b = new Boleto();
            b.Cedente = c;
            //
            b.DataProcessamento = DateTime.Now;
            b.DataVencimento = DateTime.Now.AddDays(15);
            b.ValorBoleto = Convert.ToDecimal(2469.69);
            b.Carteira = "SR";
            b.NossoNumero = "92082835";
            b.NumeroDocumento = "1008073";
            var ED = new EspecieDocumento(104);
            b.EspecieDocumento = ED;

            //
            b.Sacado = new Sacado("Fulano de Silva");
            b.Sacado.CpfCnpj = "000.000.000-00";
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testelândia";
            b.Sacado.Endereco.Cep = "70000000";
            b.Sacado.Endereco.Uf = "RS";

            var item1 = new Instrucao_Caixa(9, null, 5);
            b.Instrucoes.Add(item1);
            //b.Instrucoes.Add(item2);
            b.Banco = new Banco(104);

            #region Dados para Remessa:
            b.Remessa = new Remessa();
            b.Remessa.TipoDocumento = "2"; // SIGCB - SEM REGISTRO
            b.Remessa.CodigoOcorrencia = string.Empty;
            #endregion

            //
            var boletos = new Boletos();
            boletos.Add(b);

            GeraArquivoCNAB240(b.Banco, c, boletos);
        }

        public void GeraDadosBancoDoNordeste()
        {
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

            #region Dados para Remessa:
            b.Remessa = new Remessa();
            b.Remessa.TipoDocumento = "A";
            #endregion


            var boletos = new Boletos();
            boletos.Add(b);

            GeraArquivoCNAB400(b.Banco, c, boletos);
        }

        #endregion Remessa

        #region Retorno

        private void LerRetorno(int codigo)
        {
            try
            {
                var bco = new Banco(codigo);

                openFileDialog.FileName = "";
                openFileDialog.Title = "Selecione um arquivo de retorno";
                openFileDialog.Filter = "Arquivos de Retorno (*.ret;*.crt)|*.ret;*.crt|Todos Arquivos (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (radioButtonCNAB400.Checked)
                    {
                        ArquivoRetornoCNAB400 cnab400 = null;

                        if (openFileDialog.CheckFileExists)
                        {
                            cnab400 = new ArquivoRetornoCNAB400();
                            cnab400.LinhaDeArquivoLida += cnab400_LinhaDeArquivoLida;
                            cnab400.LerArquivoRetorno(bco, openFileDialog.OpenFile());
                        }

                        if (cnab400 == null)
                        {
                            MessageBox.Show("Arquivo não processado!");
                            return;
                        }

                        lstReturnFields.Items.Clear();

                        foreach (var detalhe in cnab400.ListaDetalhe)
                        {
                            var li = new ListViewItem(detalhe.NomeSacado.Trim());
                            li.Tag = detalhe;

                            li.SubItems.Add(detalhe.DataVencimento.ToString("dd/MM/yy"));
                            li.SubItems.Add(detalhe.DataCredito.ToString("dd/MM/yy"));

                            li.SubItems.Add(detalhe.ValorTitulo.ToString("###,###.00"));

                            li.SubItems.Add(detalhe.ValorPago.ToString("###,###.00"));
                            li.SubItems.Add(detalhe.CodigoOcorrencia.ToString());
                            li.SubItems.Add("");
                            li.SubItems.Add(detalhe.NossoNumeroComDV); // = detalhe.NossoNumero.ToString() + "-" + detalhe.DACNossoNumero.ToString());
                            li.SubItems.Add(detalhe.NumeroDocumento);
                            lstReturnFields.Items.Add(li);
                        }
                    }
                    else if (radioButtonCNAB240.Checked)
                    {
                        ArquivoRetornoCNAB240 cnab240 = null;

                        if (openFileDialog.CheckFileExists)
                        {
                            cnab240 = new ArquivoRetornoCNAB240();
                            cnab240.LinhaDeArquivoLida += cnab240_LinhaDeArquivoLida;
                            cnab240.LerArquivoRetorno(bco, openFileDialog.OpenFile());
                        }

                        if (cnab240 == null)
                        {
                            MessageBox.Show("Arquivo não processado!");
                            return;
                        }
                        
                        lstReturnFields.Items.Clear();

                        foreach (var detalhe in cnab240.ListaDetalhes)
                        {
                            var li = new ListViewItem(detalhe.SegmentoT.NomeSacado.Trim());
                            li.Tag = detalhe;

                            li.SubItems.Add(detalhe.SegmentoT.DataVencimento.ToString("dd/MM/yy"));
                            li.SubItems.Add(detalhe.SegmentoU.DataCredito.ToString("dd/MM/yy"));
                            li.SubItems.Add(detalhe.SegmentoT.ValorTitulo.ToString("###,###.00"));
                            li.SubItems.Add(detalhe.SegmentoU.ValorPagoPeloSacado.ToString("###,###.00"));
                            li.SubItems.Add(detalhe.SegmentoU.CodigoOcorrenciaSacado);
                            li.SubItems.Add("");
                            li.SubItems.Add(detalhe.SegmentoT.NossoNumero);
                            lstReturnFields.Items.Add(li);
                        }
                    }

                    MessageBox.Show("Arquivo aberto com sucesso!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao abrir arquivo de retorno.");
            }
        }

        void cnab240_LinhaDeArquivoLida(object sender, LinhaDeArquivoLidaArgs e)
        {
            MessageBox.Show(e.Linha);
        }

        void cnab400_LinhaDeArquivoLida(object sender, LinhaDeArquivoLidaArgs e)
        {
            MessageBox.Show(e.Linha);
        }

        #endregion Retorno

        #region Exemplos de arquivos de retorno

        public void GeraArquivoCNAB400Itau(Stream arquivo)
        {
            try
            {
                var gravaLinha = new StreamWriter(arquivo);

                #region Variáveis

                var n275 = new string(' ', 275);
                var n025 = new string(' ', 25);
                var n023 = new string(' ', 23);
                var n039 = new string('0', 39);
                var n026 = new string('0', 26);
                var n090 = new string(' ', 90);
                var n160 = new string(' ', 160);

                #endregion

                #region HEADER

                var header = "02RETORNO01COBRANCA       347700232610        ALLMATECH TECNOLOGIA DA INFORM341BANCO ITAU SA  ";
                header += "08010800000BPI00000201207";
                header += n275;
                header += "000001";

                gravaLinha.WriteLine(header);

                #endregion

                #region DETALHE

                var detalhe1 = "10201645738000250097700152310        " + n025 + "00000001            112000000000             ";
                detalhe1 += "I06201207000000000100000000            261207000000002000034134770010000000000500" + n025 + " ";
                detalhe1 += n039 + "0000000020000" + n026 + "   2112070000      0000000000000POLITEC LTDA                  " + n023 + "               ";
                detalhe1 += "AA000002";

                gravaLinha.WriteLine(detalhe1);

                var detalhe2 = "10201645738000250097700152310        " + n025 + "00000002            112000000000             ";
                detalhe2 += "I06201207000000000100000000            261207000000002000034134770010000000000500" + n025 + " ";
                detalhe2 += n039 + "0000000020000" + n026 + "   2112070000      0000000000000POLITEC LTDA                  " + n023 + "               ";
                detalhe2 += "AA000003";

                gravaLinha.WriteLine(detalhe2);

                var detalhe3 = "10201645738000250097700152310        " + n025 + "00000003            112000000000             ";
                detalhe3 += "I06201207000000000100000000            261207000000002000034134770010000000000500" + n025 + " ";
                detalhe3 += n039 + "0000000020000" + n026 + "   2112070000      0000000000000POLITEC LTDA                  " + n023 + "               ";
                detalhe3 += "AA000004";

                gravaLinha.WriteLine(detalhe3);

                #endregion

                #region TRAILER

                var trailer = "9201341          0000000300000000060000                  0000000000000000000000        ";
                trailer += n090 + "0000000000000000000000        000010000000300000000060000" + n160 + "000005";
                ;

                gravaLinha.WriteLine(trailer);

                #endregion

                gravaLinha.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar arquivo.", ex);
            }
        }

        #endregion Exemplos de arquivos de retorno

        private void impressãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new NBoleto();

            if (radioButtonItau.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonItau.Tag);
            else if (radioButtonUnibanco.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonUnibanco.Tag);
            else if (radioButtonSudameris.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonSudameris.Tag);
            else if (radioButtonSafra.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonSafra.Tag);
            else if (radioButtonReal.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonReal.Tag);
            else if (radioButtonHsbc.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonHsbc.Tag);
            else if (radioButtonBancoBrasil.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonBancoBrasil.Tag);
            else if (radioButtonBradesco.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonBradesco.Tag);
            else if (radioButtonCaixa.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonCaixa.Tag);
            else if (radioButtonBNB.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonBNB.Tag);
            else if (radioButtonSicredi.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonSicredi.Tag);
            else if (radioButtonBanrisul.Checked)
                form.CodigoBanco = Convert.ToInt16(radioButtonBanrisul.Tag);

            form.ShowDialog();
        }

        private void cNABToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (radioButtonCNAB400.Checked)
            {
                if (radioButtonItau.Checked)
                    GeraDadosItau(TipoArquivo.Cnab400);
                else if (radioButtonBancoBrasil.Checked)
                    GeraDadosBancoDoBrasil();
                else if (radioButtonBanrisul.Checked)
                    GeraDadosBanrisul();
                else if (radioButtonCaixa.Checked)
                    GeraDadosCaixa();
                else if (radioButtonSicredi.Checked)
                    GeraDadosSicredi();
                else if (radioButtonBNB.Checked)
                    GeraDadosBancoDoNordeste();
            }
            else if (radioButtonCNAB240.Checked)
            {
                if (radioButtonItau.Checked)
                    GeraDadosItau(TipoArquivo.Cnab240);
                else if (radioButtonSantander.Checked)
                    GeraDadosSantander();
                else if (radioButtonBanrisul.Checked)
                    MessageBox.Show("Não Implementado!");
                else if (radioButtonCaixa.Checked)
                    GeraDadosCaixa();
            }
        }

        private void lerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (radioButtonItau.Checked)
                LerRetorno(341);
            else if (radioButtonSudameris.Checked)
                LerRetorno(347);
            else if (radioButtonSantander.Checked)
                LerRetorno(33);
            else if (radioButtonReal.Checked)
                LerRetorno(356);
            else if (radioButtonCaixa.Checked)
                LerRetorno(104);
            else if (radioButtonBradesco.Checked)
                LerRetorno(237);
            else if (radioButtonSicredi.Checked)
                LerRetorno(748);
            else if (radioButtonBanrisul.Checked)
                LerRetorno(041);
            else if (radioButtonBNB.Checked)
                LerRetorno(4);
            else if (radioButtonBancoBrasil.Checked)
                LerRetorno(1);
        }

        private void gerarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Arquivos de Retorno (*.ret)|*.ret|Todos Arquivos (*.*)|*.*";

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (radioButtonCNAB400.Checked)
            {
                if (radioButtonItau.Checked)
                    GeraArquivoCNAB400Itau(saveFileDialog.OpenFile());
            }
            else
            {
                //if (radioButtonSantander.Checked)
                //    GeraArquivoCNAB240Santander(saveFileDialog.OpenFile());

            }

            MessageBox.Show("Arquivo gerado com sucesso!", "Teste", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}