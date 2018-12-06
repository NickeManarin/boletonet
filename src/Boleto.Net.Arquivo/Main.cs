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

        private void GeraArquivoCnab400(IBanco banco, Cedente cedente, Boletos boletos)
        {
            try
            {
                saveFileDialog.Filter = @"Arquivos de Retorno (*.rem)|*.rem|Todos Arquivos (*.*)|*.*";

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                var arquivo = new ArquivoRemessa(TipoArquivo.Cnab400);
                var sit = arquivo.ValidarArquivoRemessa(cedente.Convenio.ToString(), banco, cedente, boletos, 1, out var vMsgRetorno);

                if (!sit)
                {
                    MessageBox.Show(String.Concat("Foram localizados inconsistências na validação da remessa!", Environment.NewLine, vMsgRetorno),
                        @"Teste", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    arquivo.GerarArquivoRemessa("0", banco, cedente, boletos, saveFileDialog.OpenFile(), 1);

                    MessageBox.Show(@"Arquivo gerado com sucesso!", @"Teste", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GeraArquivoCnab240(IBanco banco, Cedente cedente, Boletos boletos)
        {
            saveFileDialog.Filter = "Arquivos de Retorno (*.rem)|*.rem|Todos Arquivos (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var arquivo = new ArquivoRemessa(TipoArquivo.Cnab240);
                arquivo.GerarArquivoRemessa("12345678", banco, cedente, boletos, saveFileDialog.OpenFile(), 1);

                MessageBox.Show("Arquivo gerado com sucesso!", "Teste", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void GeraDadosItau(TipoArquivo tipoArquivo)
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
                    GeraArquivoCnab240(b2.Banco, c, boletos);
                    break;
                case TipoArquivo.Cnab400:
                    GeraArquivoCnab400(b2.Banco, c, boletos);
                    break;
            }
        }

        private void GeraDadosBanrisul(TipoArquivo tipoArquivo = TipoArquivo.Cnab400)
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
            b.CodJurosMora = "1"; //"0 = Valor diário, 1 = Taxa mensal"
            b.JurosMora = 5;

            b.DataDesconto = DateTime.Today;
            b.ValorDesconto = 10m;

            b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testelândia";
            b.Sacado.Endereco.Cep = "70000000";
            b.Sacado.Endereco.Uf = "RS";

            var item = new Instrucao_Banrisul(18, null, 10, 3.1m);
            b.Instrucoes.Add(item);
            //b.Instrucoes.Add(new Instrucao_Banrisul(999, null, 10, 3.1m));
            //b.Instrucoes.Add(new Instrucao_Banrisul(998, null, 0, 4.16m));
            b.Instrucoes.Add(new Instrucao_Banrisul(0, "Exemplo de mensagem", 0, 0m));

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

            switch (tipoArquivo)
            {
                case TipoArquivo.Cnab240:
                    GeraArquivoCnab240(b.Banco, c, boletos);
                    break;
                case TipoArquivo.Cnab400:
                    GeraArquivoCnab400(b.Banco, c, boletos);
                    break;
            }
        }

        private void GeraDadosBancoDoBrasil()
        {
            var boletos = new Boletos();

            for (int i = 0; i < 2; i++)
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

                b.Instrucoes.Add(new Instrucao_BancoBrasil(82, null, 10, 3.1m));
                b.Instrucoes.Add(new Instrucao_BancoBrasil(82, null, 10, 3.1m));

                b.Banco = new Banco(001);

                #region Dados para Remessa:

                b.EspecieDocumento = new EspecieDocumento(1, "01");
                b.Remessa = new Remessa
                {
                    CodigoOcorrencia = "01",
                    TipoDocumento = b.NossoNumero
                };

                #endregion

                boletos.Add(b);
                b.Valida();
            }

            GeraArquivoCnab400(boletos[0].Banco, boletos[0].Cedente, boletos);
        }

        private void GeraDadosBradesco()
        {
            var boletos = new Boletos();

            for (int i = 0; i < 2; i++)
            {
                var conta = new ContaBancaria
                {
                    Agencia = "2166",
                    Conta = "16648",
                    DigitoConta = "0"
                };

                var c = new Cedente
                {
                    ContaBancaria = conta,
                    Codigo = "5054314",
                    CpfCnpj = "22734178000280",
                    Nome = "Comercial Botanic Home e Garden Móveis e Decorações Eireli - EPP"
                };

                var b = new Boleto
                {
                    Banco = new Banco(237),
                    Cedente = c,
                    ContaBancaria = conta,
                    DataProcessamento = DateTime.Now,
                    DataVencimento = DateTime.Now.AddDays(15),
                    ValorBoleto = Convert.ToDecimal(2471.69),
                    Carteira = "09",
                    NossoNumero = "00000000001",
                    DigitoNossoNumero = "P",
                    NumeroDocumento = "0000000001",

                    Sacado = new Sacado
                    {
                        CpfCnpj = "01484092023",
                        Nome = "Fulano de Silva",
                        Endereco =
                        {
                            End = "Av. Independência, 1809",
                            Bairro = "Centro",
                            Cidade = "Santa cruz do sul",
                            Cep = "12345678",
                            Uf = "RS"
                        }
                    }
                };

                b.Instrucoes.Add(new Instrucao_Bradesco(9, null, 10));
                b.Instrucoes.Add(new Instrucao_Bradesco(82, null, 10, 3.1m));
                b.Instrucoes.Add(new Instrucao_Bradesco(901, null, 10, 3.1m));

                #region Dados para Remessa:

                b.EspecieDocumento = new EspecieDocumento(237, "01");
                b.Remessa = new Remessa
                {
                    CodigoOcorrencia = "01",
                    TipoDocumento = b.NossoNumero
                };

                #endregion

                boletos.Add(b);
                b.Valida();
            }

            GeraArquivoCnab400(boletos[0].Banco, boletos[0].Cedente, boletos);
        }

        private void GeraDadosSicredi()
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

            GeraArquivoCnab400(b.Banco, c, boletos);
        }

        private void GeraDadosSantander()
        {
            var boletos = new Boletos();

            var vencimento = new DateTime(2018, 11, 29);

            var c = new Cedente
            {
                CpfCnpj = "36.348.313/0001-82",
                Nome = "Eu mesmo",
                CodigoTransmissao = "1163006767560130022199",
                MostrarCnpjNoBoleto = true,

                ContaBancaria = new ContaBancaria
                {
                    Agencia = "1163",
                    DigitoAgencia = "0",
                    Conta = "013002219",
                    DigitoConta = "9"
                },
                Codigo = "0067675",
                DigitoCedente = 6,
                Endereco = new Endereco
                {
                    End = "Av. Independencia",
                    Complemento = "Casa 4",
                    Bairro = "Centro",
                    Cidade = "Torres",
                    Uf = "RS",
                    Cep = "96815-236"
                }
            };

            var b = new Boleto(vencimento, 0.20m, "101", "000000000001", "9", c);
            b.NumeroDocumento = "64";

            b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
            b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
            b.Sacado.Endereco.Bairro = "Testando";
            b.Sacado.Endereco.Cidade = "Testelândia";
            b.Sacado.Endereco.Cep = "70000000";
            b.Sacado.Endereco.Uf = "DF";

            b.Instrucoes.Add(new Instrucao(33, 6, null, 10)); //Protestar
            b.Instrucoes.Add(new Instrucao(33, 2)); //Baixar após 15
            b.Instrucoes.Add(new Instrucao(33, 98)); //Juros
            b.Instrucoes.Add(new Instrucao(33, 99)); //Mora

            //Espécie Documento - Duplicata.
            b.EspecieDocumento = new EspecieDocumento_Santander("01");
            b.Banco = new Banco(33);

            b.Remessa = new Remessa
            {
                CodigoOcorrencia = "01"
            };

            boletos.Add(b);

            GeraArquivoCnab400(new Banco(33), c, boletos);
        }

        private void GeraDadosCaixa()
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

            GeraArquivoCnab240(b.Banco, c, boletos);
        }

        private void GeraDadosBancoDoNordeste()
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

            GeraArquivoCnab400(b.Banco, c, boletos);
        }

        private void GeraBoletoSicoob(TipoArquivo tipoArquivo = TipoArquivo.Cnab400)
        {
            var conta = new ContaBancaria
            {
                Agencia = "3066",
                DigitoAgencia = "0",
                Conta = "12345678",
                DigitoConta = "2"
            };

            var b = new Boleto
            {
                DataProcessamento = DateTime.Now,
                DataDocumento = DateTime.Now,
                DataVencimento = new DateTime(2018, 10, 15),
                ValorBoleto = 192.09m,
                Aceite = "N",
                NossoNumero = "1234567",
                DigitoNossoNumero = "3",
                NumeroDocumento = "1234567/01",
                Postagem = false,
                PercJurosMora = 0.2m,
                PercMulta = 2m,
                Carteira = "1", //TODO
                Parcela = 1, //TODO

                //O credor, a empresa que está emitindo este boleto, quem vai receber os valores quando pago.
                Cedente = new Cedente
                {
                    ContaBancaria = conta,
                    CpfCnpj = "94.212.349/0001-30",
                    Nome = "Empresa do Seu Juca",
                    Codigo = "1234567", //Em alguns bancos, isso é diferente, no Sicoob esse número deve ser preenchido.
                                        //Convenio = Convert.ToInt32("0" + movBoleto.MovCpr.Banco.CodConvenio), //Alguns bancos não tem isso.
                    MostrarCnpjNoBoleto = true,

                    Endereco = new Endereco
                    {
                        End = "Av. Independência",
                        Numero = "108",
                        Complemento = "Casa 7",
                        Bairro = "Universitário",
                        Cidade = "Santa Cruz do Sul",
                        Cep = "96815326",
                        Uf = "RS"
                    }
                },

                //Cliente.
                Sacado = new Sacado
                {
                    CpfCnpj = "",
                    Nome = "Julia Silveira",

                    Endereco = new Endereco
                    {
                        End = "Av. Thomas Flores",
                        Numero = "1580",
                        Complemento = "Casa 20",
                        Bairro = "Avenida",
                        Cidade = "Santa Cruz do Sul",
                        Cep = "96815326",
                        Uf = "RS"
                    }
                }
            };

            b.Banco = new Banco(756);
            b.EspecieDocumento = new EspecieDocumento(756, "01");
            b.Remessa = new Remessa
            {
                CodigoOcorrencia = "01", //Ação da remessa, geralmente '01'.
                Ambiente = Remessa.TipoAmbiente.Producao
            };

            b.DataDesconto = b.DataVencimento.AddDays(5 * -1); //5 dias atrás, ermite desconto.
            b.ValorDesconto = Math.Round(b.ValorBoleto * (2m / 100), 2); //2% de desconto.

            b.Instrucoes.Add(new Instrucao_Sicoob(1));
            b.Instrucoes.Add(new Instrucao_Sicoob(99, null, 20));

            var bb = new BoletoBancario();
            bb.CodigoBanco = 756;
            bb.FormatoCarne = false;
            bb.OcultarInstrucoes = true;
            bb.ExibirDemonstrativo = true;
            bb.MostrarComprovanteEntrega = true;
            bb.Boleto = b;
            bb.Boleto.Valida();

            var boletos = new Boletos();
            boletos.Add(b);

            switch (tipoArquivo)
            {
                case TipoArquivo.Cnab240:
                    GeraArquivoCnab240(b.Banco, b.Cedente, boletos);
                    break;
                case TipoArquivo.Cnab400:
                    GeraArquivoCnab400(b.Banco, b.Cedente, boletos);
                    break;
            }
        }

        #endregion

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
                            //cnab400.LinhaDeArquivoLida += cnab400_LinhaDeArquivoLida;
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
                            //cnab240.LinhaDeArquivoLida += cnab240_LinhaDeArquivoLida;
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

        private void GeraArquivoCnab400Itau(Stream arquivo)
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

                gravaLinha.WriteLine(trailer);

                #endregion

                gravaLinha.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar arquivo.", ex);
            }
        }

        #endregion Retorno

        #region Eventos

        private void ImpressãoMenuItem_Click(object sender, EventArgs e)
        {
            var form = new NBoleto();

            if (RbItau.Checked)
                form.CodigoBanco = Convert.ToInt16(RbItau.Tag);

            else if (RbUnibanco.Checked)
                form.CodigoBanco = Convert.ToInt16(RbUnibanco.Tag);

            else if (RbSudameris.Checked)
                form.CodigoBanco = Convert.ToInt16(RbSudameris.Tag);

            else if (RbSafra.Checked)
                form.CodigoBanco = Convert.ToInt16(RbSafra.Tag);

            else if (RbReal.Checked)
                form.CodigoBanco = Convert.ToInt16(RbReal.Tag);

            else if (RbHsbc.Checked)
                form.CodigoBanco = Convert.ToInt16(RbHsbc.Tag);

            else if (RbBancoBrasil.Checked)
                form.CodigoBanco = Convert.ToInt16(RbBancoBrasil.Tag);

            else if (RbBradesco.Checked)
                form.CodigoBanco = Convert.ToInt16(RbBradesco.Tag);

            else if (RbCaixa.Checked)
                form.CodigoBanco = Convert.ToInt16(RbCaixa.Tag);

            else if (RbSantander.Checked)
                form.CodigoBanco = 33;

            else if (RbBNB.Checked)
                form.CodigoBanco = Convert.ToInt16(RbBNB.Tag);

            else if (RbSicredi.Checked)
                form.CodigoBanco = Convert.ToInt16(RbSicredi.Tag);

            else if (RbSicoob.Checked)
                form.CodigoBanco = 756;

            else if (RbBanrisul.Checked)
                form.CodigoBanco = Convert.ToInt16(RbBanrisul.Tag);

            form.ShowDialog();
        }

        private void RemessaMenuItem_Click(object sender, EventArgs e)
        {
            if (radioButtonCNAB400.Checked)
            {
                if (RbItau.Checked)
                    GeraDadosItau(TipoArquivo.Cnab400);
                else if (RbBancoBrasil.Checked)
                    GeraDadosBancoDoBrasil();
                else if (RbBradesco.Checked)
                    GeraDadosBradesco();
                else if (RbBanrisul.Checked)
                    GeraDadosBanrisul();
                else if (RbSantander.Checked)
                    GeraDadosSantander();
                else if (RbCaixa.Checked)
                    GeraDadosCaixa();
                else if (RbSicredi.Checked)
                    GeraDadosSicredi();
                else if (RbBNB.Checked)
                    GeraDadosBancoDoNordeste();
                else if (RbSicoob.Checked)
                    GeraBoletoSicoob();
            }
            else if (radioButtonCNAB240.Checked)
            {
                if (RbItau.Checked)
                    GeraDadosItau(TipoArquivo.Cnab240);
                else if (RbSantander.Checked)
                    GeraDadosSantander();
                else if (RbBanrisul.Checked)
                    MessageBox.Show("Não Implementado!");
                else if (RbCaixa.Checked)
                    GeraDadosCaixa();
                else if (RbSicoob.Checked)
                    GeraBoletoSicoob(TipoArquivo.Cnab240);
            }
        }

        private void RetornoMenuItem_Click(object sender, EventArgs e)
        {
            if (RbItau.Checked)
                LerRetorno(341);
            else if (RbSudameris.Checked)
                LerRetorno(347);
            else if (RbSantander.Checked)
                LerRetorno(33);
            else if (RbReal.Checked)
                LerRetorno(356);
            else if (RbCaixa.Checked)
                LerRetorno(104);
            else if (RbBradesco.Checked)
                LerRetorno(237);
            else if (RbSicredi.Checked)
                LerRetorno(748);
            else if (RbBanrisul.Checked)
                LerRetorno(041);
            else if (RbBNB.Checked)
                LerRetorno(4);
            else if (RbBancoBrasil.Checked)
                LerRetorno(1);
            else if (RbSicoob.Checked)
                LerRetorno(756);
        }

        private void GeraRetornoMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Arquivos de Retorno (*.ret)|*.ret|Todos Arquivos (*.*)|*.*";

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            if (radioButtonCNAB400.Checked)
            {
                if (RbItau.Checked)
                    GeraArquivoCnab400Itau(saveFileDialog.OpenFile());
            }
            else
            {
                //if (radioButtonSantander.Checked)
                //    GeraArquivoCNAB240Santander(saveFileDialog.OpenFile());

            }

            MessageBox.Show("Arquivo gerado com sucesso!", "Teste", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion
    }
}