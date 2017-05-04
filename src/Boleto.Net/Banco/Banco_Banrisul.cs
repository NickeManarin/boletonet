using System;
using System.Linq;
using System.Web.UI;
using BoletoNet.Util;
using BoletoNet.EDI.Banco;

[assembly: WebResource("BoletoNet.Imagens.041.jpg", "image/jpg")]
namespace BoletoNet
{
    internal sealed class Banco_Banrisul : AbstractBanco, IBanco
    {
        private int _firstDigit;
        private int _secondDigit;

        internal Banco_Banrisul()
        {
            Codigo = 041;
            Digito = "8";
            Nome = "Banco Banrisul";
        }

        public override void ValidaBoleto(Boleto boleto)
        {
            boleto.ContaBancaria = boleto.Cedente.ContaBancaria;

            //Formata o tamanho do número da agência
            if (boleto.Cedente.ContaBancaria.Agencia.Length < 4)
                throw new Exception("Número da agência inválido");

            //Formata o tamanho do número da conta corrente
            if (boleto.Cedente.ContaBancaria.Conta.Length != 7)
                boleto.Cedente.ContaBancaria.Conta = Utils.FormatCode(boleto.Cedente.ContaBancaria.Conta, 7);

            if (boleto.Instrucoes.Count(x => x.Codigo != 0) > 2)
                throw new Exception("Máximo de duas instruções permitidas por boleto.");

            //Formata o tamanho do número de nosso número.
            if (boleto.NossoNumero.Length < 8)
                boleto.NossoNumero = Utils.FormatCode(boleto.NossoNumero, 8);
            else if (boleto.NossoNumero.Length > 8)
                throw new NotSupportedException("Para o banco Banrisul, o nosso número deve ter 08 posições e 02 dígitos verificadores (calculados automaticamente).");

            if (string.IsNullOrWhiteSpace(boleto.DigitoNossoNumero))
                boleto.DigitoNossoNumero = CalcularDigitosNossoNumero(boleto.NossoNumero);

            //Aceite. Usa-se 'A' ao invés de 'S'.
            if (boleto.Aceite == "S")
                boleto.Aceite = "A";

            //Atribui o nome do banco ao local de pagamento
            if (boleto.LocalPagamento == "Até o vencimento, preferencialmente no ")
                boleto.LocalPagamento += Nome;

            //Verifica se data do processamento é valida
            if (boleto.DataProcessamento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataProcessamento = DateTime.Now;

            //Verifica se data do documento é valida
            if (boleto.DataDocumento == DateTime.MinValue) // diegomodolo (diego.ribeiro@nectarnet.com.br)
                boleto.DataDocumento = DateTime.Now;

            FormataCodigoBarra(boleto);
            FormataLinhaDigitavel(boleto);
            //FormataNossoNumero(boleto);
        }

        private string CalcularDigitosNossoNumero(string nossoNumero)
        {
            //O módulo 11 sempre devolve os dois dígitos, pois as vezes o dígito calculado no mod10 será incrementado em 1.
            var dv1 = Mod10Banri(nossoNumero);
            return Mod11Banri(nossoNumero, dv1).ToString("00");
        }

        private string CalcularDigitosCodBarras(string seq)
        {
            var dv1 = Mod10Banri(seq);
            var dv2 = Mod11Banri(seq, dv1);
            //O módulo 11 sempre devolve os dois dígitos, pois, as vezes, o dígito calculado no mod10 será incrementado em 1

            return dv2.ToString("00");
        }

        //public override void FormataNossoNumero(Boleto boleto)
        //{
        //    if (boleto.NossoNumero.Length == 10)
        //        boleto.NossoNumero = boleto.NossoNumero.Substring(0, 8) + "-" + boleto.NossoNumero.Substring(8, 2);
        //    else
        //        throw new Exception("Erro ao tentar formatar nosso número, verifique o tamanho do campo");
        //}

        public override void FormataNumeroDocumento(Boleto boleto)
        {
            throw new NotImplementedException("Função do fomata número do documento não implementada.");
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            //041M2.1AAAd1bb ACCCC.CCCNNd2bb NNNNN.N40XXd3bb V FFFF9999999999

            #region Campo 1 - 041M2.1AAAd1bb

            var metade1 = "041" + boleto.Moeda + "2";
            var metade2 = "1" + boleto.Cedente.ContaBancaria.Agencia.PadLeft(4, '0').Substring(0, 3);

            var d1 = Mod10Banri(metade1 + metade2).ToString();
            var campo1 = metade1 + "." + metade2 + d1;

            #endregion

            #region Campo 2 - ACCCC.CCCNNd2

            metade1 = boleto.Cedente.ContaBancaria.Agencia.PadLeft(4, '0').Substring(3, 1) + boleto.Cedente.Codigo.Substring(0, 4);
            metade2 = boleto.Cedente.Codigo.Substring(4, 3) + boleto.NossoNumero.Substring(0, 2);

            var d2 = Mod10Banri(metade1 + metade2).ToString();
            var campo2 = metade1 + "." + metade2 + d2;

            #endregion

            #region Campo 3 - NNNNN.N40XXd3

            metade1 = boleto.NossoNumero.Substring(2, 5);
            metade2 = boleto.NossoNumero.Substring(7, 1) + "40" + _firstDigit + _secondDigit;

            var d3 = Mod10Banri(metade1 + metade2).ToString();
            var campo3 = metade1 + "." + metade2 + d3;

            #endregion

            //Campo 4 - V (Dígito de auto conferência do código de barras)
            var campo4 = boleto.CodigoBarra.Codigo.Substring(4, 1);

            #region Campo 5 - FFFF9999999999

            var fatorVenc = FatorVencimento(boleto).ToString("0000");
            var valor = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "").PadLeft(10, '0');

            var campo5 = fatorVenc + valor;

            #endregion

            boleto.CodigoBarra.LinhaDigitavel = campo1 + "  " + campo2 + "  " + campo3 + "  " + campo4 + " " + campo5;
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            var campo1 = "041" + boleto.Moeda;

            var fatorVenc = FatorVencimento(boleto).ToString("0000");
            var valor = boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", "").PadLeft(10, '0');
            var campo2 = fatorVenc + valor;

            var nossoNumero = boleto.NossoNumero.Replace(".", "").Replace("-", "").Substring(0, 8);
            var campoLivre = (boleto.Postagem ? "1" : "2") + "1" +
                boleto.Cedente.ContaBancaria.Agencia.Substring(0, 4) + boleto.Cedente.ContaBancaria.Conta.PadLeft(7, '0').Substring(0, 7) + nossoNumero + "40";

            //"1" Cobrança Normal, Fichário emitido pelo BANRISUL.
            //"2" Cobrança Direta, Fichário emitido pelo CLIENTE

            var digitBarra = CalcularDigitosCodBarras(campoLivre);

            int.TryParse(digitBarra.Substring(0, 1), out _firstDigit);
            int.TryParse(digitBarra.Substring(1, 1), out _secondDigit);

            campoLivre += digitBarra;

            var dacCodBarras = Mod11Peso2a9Banri(campo1 + campo2 + campoLivre);
            boleto.CodigoBarra.Codigo = campo1 + dacCodBarras + campo2 + campoLivre;
        }

        private int Mod10Banri(string seq)
        {
            /* (N1*1-9) + (N2*2-9) + (N3*1-9) + (N4*2-9) + (N5*1-9) + (N6*2-9) + (N7*1-9) + (N8*2-9)
             * Observação:
             * a) a subtração do 9 somente será feita se o produto obtido da multiplicação individual for maior do que 9. 
             * b) quando o somatório for menor que 10, o resto da divisão por 10 será o próprio somatório. 
             * c) quando o resto for 0, o primeiro DV é igual a 0.
             */
            int soma = 0, resto, peso = 2;

            for (var i = seq.Length - 1; i >= 0; i--)
            {
                var n = Convert.ToInt32(seq.Substring(i, 1));
                var result = n * peso > 9 ? n * peso - 9 : n * peso;
                soma += result;

                peso = peso == 2 ? 1 : 2;
            }

            if (soma < 10)
                resto = soma;
            else
                resto = soma % 10;

            var dv1 = resto == 0 ? 0 : 10 - resto;
            return dv1;
        }

        private int Mod11Banri(string seq, int dv1)
        {
            /* Obter somatório (peso de 2 a 7), sempre da direita para a esquerda (N1*4)+(N2*3)+(N3*2)+(N4*7)+(N5*6)+(N6*5)+(N7*4)+(N8*3)+(N9*2)
             * Caso o somatório obtido seja menor que "11", considerar como resto da divisão o próprio somatório.
             * Caso o ''resto'' obtido no cálculo do módulo ''11'' seja igual a ''1'', considera-se o DV inválido. 
             * Soma-se, então, "1" ao DV obtido do módulo "10" e refaz-se o cálculo do módulo 11 . 
             * Se o dígito obtido pelo módulo 10 era igual a "9", considera-se então (9+1=10) DV inválido. 
             * Neste caso, o DV do módulo "10" automaticamente será igual a "0" e procede-se assim novo cálculo pelo módulo "11". 
             * Caso o ''resto'' obtido no cálculo do módulo "11" seja ''0'', o segundo ''NC'' será igual ao próprio ''resto''
             */
            int peso = 2;
            int sum = 0, dv2, b = 7;
            seq += dv1.ToString();

            for (var i = seq.Length - 1; i >= 0; i--)
            {
                var n = Convert.ToInt32(seq.Substring(i, 1));
                var mult = n * peso;
                sum += mult;
                if (peso < b)
                    peso++;
                else
                    peso = 2;
            }

            seq = seq.Substring(0, seq.Length - 1);
            var rest = sum < 11 ? sum : sum % 11;

            var dvInvalido = rest == 1;

            if (dvInvalido)
            {
                var novoDv1 = dv1 == 9 ? 0 : dv1 + 1;
                dv2 = Mod11Banri(seq, novoDv1);
            }
            else
            {
                dv2 = rest == 0 ? 0 : 11 - rest;
            }

            if (!dvInvalido)
            {
                var digitos = dv1.ToString() + dv2;
                return Convert.ToInt32(digitos);
            }

            return dv2;
        }

        private int Mod11BaseIndef(string seq, int b)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             */

            int s = 0, peso = 2;

            for (var i = seq.Length; i > 0; i--)
            {
                s = s + Convert.ToInt32(seq.Mid( i, 1)) * peso;
                if (peso == b)
                    peso = 2;
                else
                    peso = peso + 1;
            }

            var digito = 11 - s % 11;
            
            if (digito > 9 || digito == 0 || digito == 1)
                digito = 1;

            return digito;
        }

        private int Mod11Peso2a9Banri(string seq)
        {
            /* Variáveis
             * -------------
             * d - Dígito
             * s - Soma
             * p - Peso
             * b - Base
             * r - Resto
             * n - Numero (string convertida)
             */

            int d, r, s = 0, p = 2, b = 9, n;

            for (var i = seq.Length - 1; i >= 0; i--)
            {
                n = Convert.ToInt32(seq.Substring(i, 1));

                s = s + n * p;

                if (p < b)
                    p = p + 1;
                else
                    p = 2;
            }

            r = s % 11;

            if (r == 0 || r == 1 || r > 9)
                d = 1;
            else
                d = 11 - r;

            return d;
        }

        private int CalculaSoma(string numero)
        {
            int x;
            var soma = 0;
            var mult = 2;
            var I = numero.Length;

            //Para começar o cálculo pelo nº final (sempre começa multiplicando por 2)
            for (x = 1; x <= numero.Length; x++)
            {
                if (Codigo == 41)
                {
                    //Banrisul só vai até 7
                    if (mult == 8)
                        mult = 2;
                }
                else
                {
                    if (mult == 10)
                        mult = 2;
                }
                var y = Convert.ToInt32(Strings.Mid(numero, I, 1));
                var resul = y * mult;
                soma = soma + resul;
                mult = mult + 1;
                I = I - 1;
            }

            if (Codigo == 41 | Codigo == 33 | Codigo == 353)
            {
                return soma;
                // calcula no retorno pois tem umas exceções
            }

            var resto = soma % 11;

            if (resto == 0)
                resto = 1;

            return resto;
        }

        #region Métodos de validação e geração do arquivo remessa

        /// <summary>
        /// Efetua as Validações dentro da classe Boleto, para garantir a geração da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            var vRetorno = true;
            var vMsg = string.Empty;
                        
            switch (tipoArquivo)
            {
                case TipoArquivo.Cnab240:
                    vRetorno = ValidarRemessaCNAB240(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.Cnab400:
                    vRetorno = ValidarRemessaCnab400(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
                    break;
                case TipoArquivo.Outro:
                    throw new Exception("Tipo de arquivo inexistente.");
            }
            
            mensagem = vMsg;
            return vRetorno;
        }

        /// <summary>
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                var header = " ";

                base.GerarHeaderRemessa(numeroConvenio, cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        header = GerarHeaderRemessaCNAB240();
                        break;
                    case TipoArquivo.Cnab400:
                        header = GerarHeaderRemessaCnab400(int.Parse(numeroConvenio), cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do HEADER do arquivo de REMESSA.", ex);
            }
        }

        /// <summary>
        /// DETALHE do arquivo CNAB
        /// Gera o DETALHE do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        detalhe = GerarDetalheRemessaCNAB240();
                        break;
                    case TipoArquivo.Cnab400:
                        detalhe = GerarDetalheRemessaCnab400(boleto, numeroRegistro, tipoArquivo);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do DETALHE arquivo de REMESSA.", ex);
            }
        }

        /// <summary>
        /// TRAILER do arquivo CNAB
        /// Gera o TRAILER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                var trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        trailer = GerarTrailerRemessa240();
                        break;
                    case TipoArquivo.Cnab400:
                        trailer = GerarTrailerRemessa400(numeroRegistro, vltitulostotal);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        #endregion

        #region CNAB 240

        public bool ValidarRemessaCNAB240(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarHeaderRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarDetalheRemessaCNAB240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        public string GerarTrailerRemessa240()
        {
            throw new NotImplementedException("Função não implementada.");
        }

        #endregion

        #region CNAB 400

        public bool ValidarRemessaCnab400(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            var vRetorno = true;
            var vMsg = string.Empty;
            
            #region Pré Validações

            if (banco == null)
            {
                vMsg += string.Concat("Remessa: O Banco é obrigatório!", Environment.NewLine);
                vRetorno = false;
            }

            if (cedente == null)
            {
                vMsg += string.Concat("Remessa: O Cedente/Beneficiário é Obrigatório!", Environment.NewLine);
                vRetorno = false;
            }

            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += string.Concat("Remessa: Deverá existir ao menos 1 boleto para geração da remessa!", Environment.NewLine);
                vRetorno = false;
            }

            if (cedente != null && (string.IsNullOrWhiteSpace(cedente.Codigo) || cedente.Codigo.Length != 7))
            {
                vMsg += string.Concat("Remessa: O código do cedente deve ser de 7 posições.", Environment.NewLine);
                vRetorno = false;
            }

            #endregion
            
            foreach (var boleto in boletos)
            {
                #region Validação de cada boleto

                if (boleto.Remessa == null)
                {
                    vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                else
                {
                    #region Validações da Remessa que deverão estar preenchidas quando BANRISUL

                    if (string.IsNullOrEmpty(boleto.Remessa.CodigoOcorrencia))
                    {
                        vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Código de Ocorrência!", Environment.NewLine);
                        vRetorno = false;
                    }

                    //if (string.IsNullOrEmpty(boleto.Remessa.TipoDocumento))
                    //{
                    //    vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Tipo Documento!", Environment.NewLine);
                    //    vRetorno = false;
                    //}
                    //else if (boleto.Remessa.TipoDocumento.Equals("06") && !string.IsNullOrEmpty(boleto.NossoNumero))
                    //{
                    //    //Para o "Remessa.TipoDocumento = "06", não poderá ter NossoNumero Gerado!
                    //    vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Não pode existir NossoNumero para o Tipo Documento '06 - cobrança escritural'!", Environment.NewLine);
                    //    vRetorno = false;
                    //}

                    #endregion
                }

                if (boleto.Instrucoes.Count(x => x.Codigo != 0) > 2)
                {
                    vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Apenas 2 instruções são permitidas no boleto!", Environment.NewLine);
                    vRetorno = false;
                }

                #endregion
            }
            
            mensagem = vMsg;
            return vRetorno;
        }

        public string GerarHeaderRemessaCnab400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0001, 010, 0, "01REMESSA", ' '));            //001-009
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0010, 016, 0, "", ' '));                     //010-026
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0027, 013, 0, 
                    cedente.ContaBancaria.Agencia.PadLeft(4, '0') + cedente.Codigo.PadLeft(7, '0') + cedente.ContaBancaria.DigitoConta.PadLeft(2, '0'), ' ')); //027-039
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0040, 007, 0, "", ' '));                     //040-046
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome.ToUpper(), ' ')); //047-076
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0077, 011, 0, "041BANRISUL", ' '));          //077-087
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0088, 007, 0, "", ' '));                     //088-094
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' '));           //095-100 Data de Gravação do Arquivo
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0101, 009, 0, "", ' '));                     //101-109 Brancos
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0110, 004, 0, "", ' '));                     //110-113 Código do Serviço (Somente para carteiras R/S/X)
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0114, 001, 0, "", ' '));                     //114-114 Branco
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0115, 001, 0, "", ' '));                     //115-115 Tipo de Processamento (Somente para carteiras R/S/X)
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0116, 001, 0, "", ' '));                     //116-116 Branco
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0117, 010, 0, "", ' '));                     //117-126 Código do Cliente no Office Banking (Somente para carteiras R/S/X)
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0127, 268, 0, "", ' '));                     //126-394 Brancos
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0395, 006, 0, "000001", ' '));               //395-400 Literal '000001'
                
                reg.CodificarLinha();

                return Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        public string GerarDetalheRemessaCnab400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);
                
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 001, 01, 0, "1", ' '));                              //001-001 Literal '1'
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 002, 16, 0, string.Empty, ' '));                     //002-017 Brancos
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 018, 13, 0, boleto.Cedente.ContaBancaria.Agencia.PadLeft(4, '0') + 
                    boleto.Cedente.Codigo.PadLeft(7, '0') + boleto.ContaBancaria.DigitoConta.PadLeft(2, '0'), ' ')); //018-030 Agência + Código do Cedente.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 031, 07, 0, string.Empty, ' '));                     //031-037 Brancos
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 038, 25, 0, boleto.NumeroDocumento, ' '));           //038-062 Identificação de Título (Alfanumérico opcional)
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 063, 10, 0, boleto.NossoNumero + boleto.DigitoNossoNumero, '0')); //063-072 Nosso Número (2 dígitos verificadores);
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 073, 32, 0, string.Empty, ' '));                     //073-104 Mensagem
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 105, 03, 0, string.Empty, ' '));                     //105-107 Brancos
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 108, 01, 0, boleto.Carteira, ' '));                  //108-108 Tipo de carteira
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 109, 02, 0, boleto.Remessa.CodigoOcorrencia, ' '));  //109-110 Código da ocorrência (ação do que fazer).
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 111, 10, 0, boleto.NumeroDocumento, ' '));           //111-120 Seu número. Usualmente a mesma coisa que o número do documento.
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAA___________, 121, 06, 0, boleto.DataVencimento, ' '));            //121-126 Data de vencimento.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 127, 13, 2, boleto.ValorBoleto, '0'));               //127-139 Valor do boleto.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 140, 03, 0, "041", ' '));                            //140-142 Literal '041'.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 143, 05, 0, string.Empty, ' '));                     //143-147 Brancos.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 148, 02, 0, boleto.Postagem ? "09" : "08", ' '));    //148-149 Tipo do documento.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 150, 01, 0, boleto.Aceite, ' '));                    //150-150 Aceite.
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAA___________, 151, 06, 0, boleto.DataProcessamento, ' '));         //151-156 Data de processamento/emissão.

                #region Instruções

                var diasProtestoDevolução = 0;
                var diasJurosMulta = 0;
                var percJurosMulta = 0m;

                var vInstrucao1 = "";
                var vInstrucao2 = "";

                var instList = boleto.Instrucoes.Where(x => x.Codigo != 0).ToList();

                if (instList.Any())
                {
                    vInstrucao1 = instList[0].Codigo.ToString().PadLeft(2, '0');

                    if (instList[0].Codigo == 9 || instList[0].Codigo == 15)
                        diasProtestoDevolução = instList[0].Dias;

                    if (instList[0].Codigo == 18 || instList[0].Codigo == 20)
                    {
                        diasJurosMulta = instList[0].Dias;
                        percJurosMulta = instList[0].Valor;
                    }

                    if (instList.Count > 1)
                    {
                        vInstrucao2 = instList[1].Codigo.ToString().PadLeft(2, '0');

                        if (instList[1].Codigo == 9 || instList[1].Codigo == 15)
                            diasProtestoDevolução = instList[1].Dias;

                        if (instList[1].Codigo == 18 || instList[1].Codigo == 20)
                        {
                            diasJurosMulta = instList[1].Dias;
                            percJurosMulta = instList[1].Valor;
                        }
                    }
                }

                //switch (boleto.Instrucoes.Count)
                //{
                //    case 1:
                //        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString().PadLeft(2, '0');
                //        vInstrucao2 = string.Empty;

                //        if (boleto.Instrucoes[0].Codigo == 9 || boleto.Instrucoes[0].Codigo == 15)
                //            diasProtestoDevolução = boleto.Instrucoes[0].Dias;               
                //        break;
                //    case 2:
                //        vInstrucao1 += boleto.Instrucoes[0].Codigo.ToString().PadLeft(2, '0');
                        
                //        if (boleto.Instrucoes[0].Codigo == 9 || boleto.Instrucoes[0].Codigo == 15)
                //            diasProtestoDevolução = boleto.Instrucoes[0].Dias;
                        
                //        vInstrucao2 += boleto.Instrucoes[1].Codigo.ToString().PadLeft(2, '0');
                        
                //        if (boleto.Instrucoes[1].Codigo == 9 || boleto.Instrucoes[1].Codigo == 15)
                //            diasProtestoDevolução = boleto.Instrucoes[1].Dias;                    
                //        break;
                //}
                
                #endregion
                
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 157, 02, 0, vInstrucao1, ' '));       //157-158 Instrução 1.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 159, 02, 0, vInstrucao2, ' '));       //159-160 Instrução 2.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 161, 01, 0, "0", ' '));               //161-161 Código de mora. (0: Diário, 1:Mensal)
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 162, 12, 2, boleto.JurosMora, '0'));  //162-173 Juros de mora.
                                                                                                                     
                #region Data Desconto

                var vDataDesconto = "000000";
                if (!boleto.DataDesconto.Equals(DateTime.MinValue))
                    vDataDesconto = boleto.DataDesconto.ToString("ddMMyy");
                
                #endregion                                                                                          
                                                                                                                     
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 174, 06, 0, vDataDesconto, '0'));         //174-179 Data para concessão do desconto.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 180, 13, 2, boleto.ValorDesconto, '0'));  //180-192 Valor do desconto.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 193, 13, 2, boleto.Iof, '0'));            //193-205 Valor do Iof.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 206, 13, 2, boleto.Abatimento, '0'));     //206-218 Valor do abatimento.
                
                #region Regra Tipo de Inscrição Sacado

                var vCpfCnpjSac = "99";
                if (boleto.Sacado.CpfCnpj.Length.Equals(11))
                    vCpfCnpjSac = "01"; //Cpf é sempre 11;
                else if (boleto.Sacado.CpfCnpj.Length.Equals(14))
                    vCpfCnpjSac = "02"; //Cnpj é sempre 14;

                #endregion

                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 219, 02, 0, vCpfCnpjSac, '0'));                             //219-220 Tipo de ID do pagador. Cpf ou Cnpj ou Outros.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 221, 14, 0, boleto.Sacado.CpfCnpj, '0'));                   //221-234 Cpf/Cnpj do pagador.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 235, 35, 0, boleto.Sacado.Nome.ToUpper(), ' '));            //235-269 Nome do pagador.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 270, 05, 0, string.Empty, ' '));                            //270-274 Brancos.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 275, 40, 0, boleto.Sacado.Endereco.EndComNumero.ToUpper(), ' '));    //275-314 Endereço.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 315, 07, 0, string.Empty, ' '));                            //315-321 Brancos.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 322, 03, 0, percJurosMulta, '0'));                          //322-324 Percentual de juros após o vencimento.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 325, 02, 0, diasJurosMulta, '0'));                          //325-326 Dias para juros.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 327, 08, 0, boleto.Sacado.Endereco.Cep, '0'));              //327-334 Cep.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 335, 15, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' ')); //335-349 Cidade.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 350, 02, 0, boleto.Sacado.Endereco.Uf.ToUpper(), ' '));     //350-351 Uf.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 352, 04, 1, 0, '0'));                                       //352-355 Taxa de desconto ao dia de antecipação.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 356, 01, 0, string.Empty, ' '));                            //356-356 Brancos.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 357, 13, 2, 0, '0'));                                       //357-369 Valor para cálculo do desconto.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 370, 02, 0, diasProtestoDevolução, '0'));                   //370-371 Números de dia para protesto ou devolução automática.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 372, 23, 0, string.Empty, ' '));                            //372-394 Brancos.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 395, 06, 0, numeroRegistro, '0'));                          //395-400 Número do registro.

                reg.CodificarLinha();
                
                return Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        public string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
        {
            try
            {
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));            //001-001
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0002, 026, 0, string.Empty, ' '));   //002-027
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0028, 013, 2, vltitulostotal, '0')); //027-039
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0041, 354, 0, string.Empty, ' '));   //040-394
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0')); //395-400
                
                reg.CodificarLinha();

                return Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a geração do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                var reg = new RegistroEdi_Banrisul_Retorno();
                reg.LinhaRegistro = registro;
                reg.DecodificarLinha();

                //Passa para o detalhe as propriedades de reg;
                var detalhe = new DetalheRetorno(registro);
                
                //detalhe. = Constante1;
                detalhe.CodigoInscricao = Utils.ToInt32(reg.TipoInscricao);
                detalhe.NumeroInscricao = reg.CpfCnpj;
                //detalhe.Agencia = Utils.ToInt32(reg.CodigoCedente.Substring(0, 3));
                //detalhe.Conta = Utils.ToInt32(reg.CodigoCedente.Substring(4, 7));
                //detalhe.DACConta = Utils.ToInt32(reg.CodigoCedente.Substring(36, 1));

                //detalhe. = reg.EspecieCobrancaRegistrada;
                //detalhe. = reg.Branco1;
                detalhe.NumeroControle = reg.IdentificacaoTituloCedente;
                detalhe.IdentificacaoTitulo = reg.IdentificacaoTituloBancoNossoNumero;
                //detalhe. = reg.IdentificacaoTituloBanco_NossoNumeroOpcional;
                //detalhe. = reg.NumeroContratoBLU;
                //detalhe. = reg.Brancos2;
                //detalhe. = reg.TipoCarteira;
                detalhe.CodigoOcorrencia = Utils.ToInt32(reg.CodigoOcorrencia);
                
                var dataOcorrencia = Utils.ToInt32(reg.DataOcorrenciaBanco);
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                detalhe.NumeroDocumento = reg.SeuNumero;
                detalhe.NossoNumeroComDV = reg.NossoNumero;
                detalhe.NossoNumero = reg.NossoNumero.Substring(0, reg.NossoNumero.Length - 1); //Nosso Número sem o DV!
                detalhe.DACNossoNumero = reg.NossoNumero.Substring(reg.NossoNumero.Length - 1); //DV
                
                var dataVencimento = Utils.ToInt32(reg.DataVencimentoTitulo);
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                
                decimal valorTitulo = Convert.ToInt64(reg.ValorTitulo);
                detalhe.ValorTitulo = valorTitulo / 100;

                //Banco Cobrador
                detalhe.CodigoBanco = Utils.ToInt32(reg.CodigoBancoCobrador);
                //Agência Cobradora
                detalhe.AgenciaCobradora = Utils.ToInt32(reg.CodigoAgenciaCobradora);
                //
                //detalhe.Especie = reg.TipoDocumento; //Verificar Espécie de Documentos...
                //Despesas de cobrança para os Códigos de Ocorrência (Valor Despesa)
                decimal valorDespesa = Convert.ToUInt64(reg.ValorDespesasCobranca);
                detalhe.ValorDespesa = valorDespesa / 100;
                //Outras despesas Custas de Protesto (Valor Outras Despesas)
                decimal valorOutrasDespesas = Convert.ToUInt64(reg.OutrasDespesas);
                detalhe.ValorOutrasDespesas = valorOutrasDespesas / 100;
                //detalhe. = reg.Zeros1;
                //detalhe. = reg.ValorAvista;
                //detalhe. = reg.SituacaoIOF;
                //detalhe. = reg.Zeros2;

                //Abatimento Concedido sobre o Título (Valor Abatimento Concedido)
                decimal valorAbatimento = Convert.ToUInt64(reg.ValorAbatimentoDeflacaoConcedido);
                detalhe.ValorAbatimento = valorAbatimento / 100;
                //Desconto Concedido (Valor Desconto Concedido)
                decimal valorDesconto = Convert.ToUInt64(reg.ValorDescontoConcedido);
                detalhe.Descontos = valorDesconto / 100;
                //Valor Pago
                decimal valorPago = Convert.ToUInt64(reg.ValorPago);
                detalhe.ValorPago = valorPago / 100;
                //Juros Mora
                decimal jurosMora = Convert.ToUInt64(reg.ValorJuros);
                detalhe.JurosMora = jurosMora / 100;
                //Outros Créditos
                decimal outrosCreditos = Convert.ToUInt64(reg.ValorOutrosRecebimentos);
                detalhe.OutrosCreditos = outrosCreditos / 100;
                //detalhe. = reg.Brancos3;
                var dataCredito = Utils.ToInt32(reg.DataCreditoConta);
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                //detalhe. = reg.Brancos4;
                detalhe.OrigemPagamento = reg.PagamentoDinheiroCheque;
                //detalhe. = reg.Brancos5;
                detalhe.MotivoCodigoOcorrencia = reg.MotivoOcorrencia;
                //detalhe. = reg.Brancos6;
                detalhe.NumeroSequencial = Utils.ToInt32(reg.NumeroSequenciaRegistro);

                #region Não retornados pelo Banrisul

                detalhe.IOF = 0;
                //Motivos das Rejeições para os Códigos de Ocorrência
                detalhe.MotivosRejeicao = string.Empty;
                //Número do Cartório
                detalhe.NumeroCartorio = 0;
                //Número do Protocolo
                detalhe.NumeroProtocolo = string.Empty;
                //Nome do Sacado
                detalhe.NomeSacado = "";

                #endregion

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        #endregion
    }
}
