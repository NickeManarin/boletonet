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
            get => _codigoBanco;
            set => _codigoBanco = value;
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
            //Cria o boleto, e passa os parâmetros usuais
            var boletos = new List<BoletoBancario>();

            for (var i = 0; i < qtde; i++)
            {
                var conta = new ContaBancaria { Agencia = "0500", DigitoAgencia = "02", Conta = "005507" };

                var c = new Cedente
                {
                    ContaBancaria = conta,
                    CpfCnpj = "00.000.000/0000-00",
                    Nome = "Empresa de Atacado",
                    Codigo = "005507",
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

                var b = new Boleto
                {
                    Cedente = c,
                    DataProcessamento = DateTime.Now,
                    DataVencimento = DateTime.Now.AddDays(15),
                    ValorBoleto = 2469.69m,
                    Carteira = "01",
                    NossoNumero = "14222333777777777", //"14000000000000001",
                    NumeroDocumento = "1008073",
                    EspecieDocumento = new EspecieDocumento(104),

                    Sacado = new Sacado("Fulano de Silva")
                    {
                        CpfCnpj = "000.000.000-00",
                        Endereco =
                        {
                            End = "SSS 154 Bloco J Casa 23",
                            Bairro = "Testando",
                            Cidade = "Testelândia",
                            Cep = "70000000",
                            Uf = "RS"
                        }
                    }
                };

                //Juros/descontos.
                if (b.ValorDesconto == 0)
                {
                    var item3 = new Instrucao_Caixa(999, null, 1);
                    b.Instrucoes.Add(item3);
                }

                b.Instrucoes.Add(new Instrucao_Caixa(998, null, 0, 8.5m, 10));

                var bb = new BoletoBancario
                {
                    CodigoBanco = _codigoBanco,
                    MostrarEnderecoCedente = true,
                    Boleto = b
                };
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }

        private void GeraBoletoItau(int qtde)
        {
            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {
                var c = new Cedente("32.694.012/0001-50", "Empresa devedora", "0057", "12345", "8");

                var b2 = new Boleto(new DateTime(2019, 4, 1), 1642m, "198", "98712345", "1", c);
                b2.Especie = "01";
                b2.NumeroDocumento = "1008073";
                b2.Banco = new Banco(341);

                b2.Sacado = new Sacado("037.671.213-95", "Fernanda Freitas")
                {
                    Endereco =
                    {
                        End = "Av. Independência",
                        Numero = "1052",
                        Complemento = "Casa 3",
                        Bairro = "Centro",
                        Cidade = "Santa Cruz do Sul",
                        Cep = "96815326",
                        Uf = "RS"
                    }
                };

                b2.Instrucoes.Add(new InstrucaoItau((int)EnumInstrucoes_Itau.JurosdeMora, null, 0, 1.35m, 30));
                b2.Instrucoes.Add(new InstrucaoItau((int)EnumInstrucoes_Itau.MultaVencimento, null, 0, 1.57m, 30));
                b2.Instrucoes.Add(new InstrucaoItau((int)EnumInstrucoes_Itau.ProtestarAposNDiasCorridos, null, 10));

                if (b2.ValorDesconto == 0)
                {
                    var item3 = new InstrucaoItau(999, "");
                    item3.Descricao += ("1,00 por dia de antecipação.");
                    b2.Instrucoes.Add(item3);
                }

                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;
                bb.Boleto = b2;
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
                var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "2768", "5", "25832", "6");

                c.Convenio = 2650829;
                c.Codigo = "512493695";
                var b = new Boleto(vencimento, 1960.50m, "17", "1234567890", c) { VariacaoCarteira = "019" };

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
                b.Instrucoes.Add(new Instrucao(b.EspecieDocumento.Banco.Codigo, 998, null, 0, 8.5m, 10));

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
                    Nome = "Comercial Botanic Home e Garden Móveis e Decorações Eireli - EPP",
                    Endereco = new Endereco
                    {
                        End = "Av. Oscar Jost",
                        Numero = "2500",
                        Bairro = "Verena",
                        Complemento = "Casa 10",
                        Cidade = "Santa cruz do sul",
                        Cep = "12345678",
                        Uf = "RS"
                    }
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
                    NumeroDocumento = "0000000010",

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

                b.Instrucoes.Add(new Instrucao_Bradesco(82, null, 10, 3.1m));
                b.Instrucoes.Add(new Instrucao_Bradesco(901, null, 10, 3.1m));

                #region Dados para Remessa:

                b.EspecieDocumento = new EspecieDocumento(237, "2");
                b.Remessa = new Remessa
                {
                    CodigoOcorrencia = "01",
                    TipoDocumento = b.NossoNumero
                };

                #endregion

                var bb = new BoletoBancario
                {
                    CodigoBanco = _codigoBanco,
                    FormatoCarne = false,
                    OcultarInstrucoes = true,
                    OcultarReciboSacado = false,
                    OcultarEnderecoSacado = false,
                    MostrarComprovanteEntrega = true,
                    MostrarEnderecoCedente = true,
                    Boleto = b
                };
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
            //Cria o boleto, e passa os parâmetros usuais
            var boletos = new List<BoletoBancario>();
            for (var i = 0; i < qtde; i++)
            {
                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var vencimento = new DateTime(2018, 04, 22);

                var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado")
                {
                    ContaBancaria = new ContaBancaria
                    {
                        Agencia = "0156",
                        DigitoAgencia = "10",
                        Conta = "13550"
                    }
                };
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

                var b = new Boleto(vencimento, 586.35m, "A", "01000000001", c);
                b.NumeroDocumento = "01000000001";

                b.Sacado = new Sacado("000.000.000-00", "Eduardo Frare");
                b.Sacado.Endereco = end;

                b.Instrucoes.Add(new Instrucao(748, 900, null, 0, 8.5m, 10));
                b.Instrucoes.Add(new Instrucao(748, 312, null, 0, 8.5m, 10));

                b.NossoNumero = "18221434";
                b.DigitoNossoNumero = "7";

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

                var c = new Cedente("11.111.111/1111-11", "Empresa de Atacado", "1102", "48", "9000150", "46");
                c.Codigo = "9000150";
                c.MostrarCnpjNoBoleto = true;
                c.Endereco = new Endereco
                {
                    End = "Av. Independencia",
                    Complemento = "Casa 4",
                    Bairro = "Centro",
                    Cidade = "Torres",
                    Uf = "RS",
                    Cep = "96815-236"
                };

                var b = new Boleto(DateTime.Now, 1980.89m, "01", "22832563", c);
                b.DigitoNossoNumero = "51";

                b.CodJurosMora = "0";
                b.Sacado = new Sacado("000.000.000-00", "Eduardo Frare");
                b.Sacado.Endereco = new Endereco
                {
                    Bairro = "Lago Sul",
                    Cep = "71666660",
                    Cidade = "Brasília- DF",
                    Complemento = "Quadra XX Conjunto XX Casa XX",
                    End = "Condominio de Brasilia - Quadra XX Conjunto XX Casa XX",
                    Logradouro = "Cond. Brasilia",
                    Numero = "55",
                    Uf = "DF"
                }; ;

                //b.Instrucoes.Add(new Instrucao_Banrisul(18, null, 10, 0.2m, 10)); //"Não Receber após o vencimento");
                //b.Instrucoes.Add(new Instrucao_Banrisul(0, "123456789121222222222222222222222222222222222222222222222222222222222222222222222222222", 10, 0.2m, 10));
                //b.Instrucoes.Add(new Instrucao_Banrisul(0, "123456789121222222222222222222222222222222222222222222222222222222222222222222222222222", 10, 0.2m, 10));

                //bb.FormatoCarne = false;
                //bb.OcultarInstrucoes = true;
                //bb.ExibirDemonstrativo = true;
                //bb.MostrarComprovanteEntrega = true;
                //bb.MostrarEnderecoCedente = true;
                //bb.OcultarEnderecoSacado = false;
                //bb.OcultarReciboSacado = false;

                bb.MostrarEnderecoCedente = true;
                bb.OcultarEnderecoSacado = false;
                bb.MostrarComprovanteEntregaLivre = true;
                bb.MostrarComprovanteEntrega = false;
                bb.OcultarReciboSacado = false;
                bb.OcultarInstrucoes = false;

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }

        public void GeraBoletoSantander(int qtde)
        {
            //Cria o boleto, e passa os parâmetros usuais.
            var boletos = new List<BoletoBancario>();

            for (var i = 0; i < qtde; i++)
            {
                var bb = new BoletoBancario();
                bb.CodigoBanco = _codigoBanco;

                var c = new Cedente("11.111.111/1111-11", "Empresa de Atacado", "1163", "0", "013002219", "9");
                c.Codigo = "0067675";
                c.DigitoCedente = 6;
                c.MostrarCnpjNoBoleto = true;
                c.Endereco = new Endereco
                {
                    End = "Av. Independencia",
                    Complemento = "Casa 4",
                    Bairro = "Centro",
                    Cidade = "Torres",
                    Uf = "RS",
                    Cep = "96815-236"
                };

                var b = new Boleto(new DateTime(2019, 01, 2), 12.57m, "101", "000000000001", "9", c);

                b.Sacado = new Sacado("000.000.000-00", "Eduardo Frare");
                b.Sacado.Endereco = new Endereco
                {
                    Bairro = "Lago Sul",
                    Cep = "71666660",
                    Cidade = "Brasília- DF",
                    Complemento = "Quadra XX Conjunto XX Casa XX",
                    End = "Condominio de Brasilia - Quadra XX Conjunto XX Casa XX",
                    Logradouro = "Cond. Brasilia",
                    Numero = "55",
                    Uf = "DF"
                };

                bb.Instrucoes.Add(new Instrucao(33, 6, null, 10)); //Protestar
                bb.Instrucoes.Add(new Instrucao(33, 2)); //Baixar após 15
                bb.Instrucoes.Add(new Instrucao(33, 98)); //Juros
                bb.Instrucoes.Add(new Instrucao(33, 99)); //Mora

                bb.MostrarEnderecoCedente = true;
                bb.OcultarEnderecoSacado = false;
                bb.MostrarComprovanteEntregaLivre = true;
                bb.MostrarComprovanteEntrega = false;
                bb.OcultarReciboSacado = false;
                bb.OcultarInstrucoes = false;

                bb.Boleto = b;
                bb.Boleto.Valida();

                boletos.Add(bb);
            }

            GeraLayout(boletos);
        }

        public void GeraBoletoSicoob(int quant)
        {
            var boletos = new List<BoletoBancario>();

            for (var i = 0; i < quant; i++)
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
                bb.CodigoBanco = _codigoBanco;
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

                case 033: // Santander
                    GeraBoletoSantander((int)numericUpDown.Value);
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

                case 756: //Sicoob.
                    GeraBoletoSicoob((int)numericUpDown.Value);
                    break;
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _progresso.Close();

            // Cria um formulário com um componente WebBrowser dentro
            _impressaoBoleto.webBrowser.Navigate(_arquivo);
            _impressaoBoleto.ShowDialog();
        }

        private void BtFechar_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void BtEnviar_Click(object sender, EventArgs e)
        {
            if ((int)numericUpDown.Value > 1)
                MessageBox.Show(@"O exemplo de envio do boleto bancário como imagem só está implementado para somente um boleto por vez.",
                    @"Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.RunWorkerAsync();
            _progresso = new Progresso();
            _progresso.ShowDialog();
        }
    }
}