using System;
using System.Globalization;
using System.Web.UI;
using BoletoNet.EDI.Banco;

[assembly: WebResource("BoletoNet.Imagens.104.jpg", "image/jpg")]

namespace BoletoNet
{
    /// <summary>
    /// Classe referente ao banco Banco_Caixa Economica Federal
    /// </summary>
    internal sealed class BancoCaixa : AbstractBanco, IBanco
    {
        /* 
         * Para Cnab 240.
         * boleto.Remessa.TipoDocumento 1 - SICGB - Com registro
         * boleto.Remessa.TipoDocumento 2 - SICGB - Sem registro
         */

        private const int EmissaoCedente = 4;
        private const decimal Decimal100 = 100;

        private string _dacBoleto = string.Empty;

        private bool _protestar;
        private bool _baixaDevolver;
        private bool _desconto;
        private int _diasProtesto;
        private int _diasDevolucao;
        private int _diasDesconto;

        internal BancoCaixa()
        {
            Codigo = 104;
            Digito = "0";
            Nome = "Caixa Econ�mica Federal";
        }

        public override void FormataCodigoBarra(Boleto boleto)
        {
            var valor = Utils.FormatCode(boleto.ValorBoleto.ToString("f").Replace(",", "").Replace(".", ""), 10);
            var vencimento = FatorVencimento(boleto);
            var cedente = Utils.FormatCode(boleto.Cedente.Codigo, 6);
            var dvCedente = Mod11Base9(cedente).ToString();
            var num1 = boleto.NossoNumero.Substring(2, 3);
            const string const1 = "1"; //1 = Registrada.
            var num2 = boleto.NossoNumero.Substring(5, 3);
            const string const2 = "4"; //4 = Emiss�o do boleto pelo cedente.
            var num3 = boleto.NossoNumero.Substring(8, 9);

            var part1 = $"1049_{vencimento}{valor}"; //Posi��es 01 � 19.
            var part2 = $"{cedente}{dvCedente}{num1}{const1}{num2}{const2}{num3}"; //Posi��es 20 � 44.
            part2 = $"{part2}{Mod11Base9(part2)}";
            
            _dacBoleto = Mod11(part1.Replace("_", "") + part2, 9).ToString(); //N�o aceita zero, troca para 1.

            boleto.CodigoBarra.Codigo = part1.Replace("_", _dacBoleto) + part2;
        }

        public override void FormataLinhaDigitavel(Boleto boleto)
        {
            #region Campo 1

            var bbbm = boleto.CodigoBarra.Codigo.Substring(0, 4); //Posi��o 1 � 4 do c�digo de barras.
            var ccccc = boleto.CodigoBarra.Codigo.Substring(19, 5); //Posi��o 20 � 24 do c�digo de barras.
            var str3 = Mod10(bbbm + ccccc).ToString();

            var grupo1 = bbbm + ccccc + str3;
            grupo1 = $"{grupo1.Substring(0, 5)}.{grupo1.Substring(5)} ";

            #endregion

            #region Campo 2

            var cccccccccc = boleto.CodigoBarra.Codigo.Substring(24, 10); //Posi��o 25 � 34 do c�digo de barras.

            var grupo2 = $"{cccccccccc.Substring(0, 5)}.{cccccccccc.Substring(5, 5)}{Mod10(cccccccccc)} ";

            #endregion

            #region Campo 3

            cccccccccc = boleto.CodigoBarra.Codigo.Substring(34, 10); //Posi��o 35 � 44 do c�digo de barras.

            var grupo3 = $"{cccccccccc.Substring(0, 5)}.{cccccccccc.Substring(5, 5)}{Mod10(cccccccccc)} ";

            #endregion

            #region Campo 5

            var ffff = boleto.CodigoBarra.Codigo.Substring(5, 4); //Posi��o 6 � 9 do c�digo de barras.
            var vvvvvvvvvv = boleto.CodigoBarra.Codigo.Substring(9, 10); //Posi��o 10 � 19 do c�digo de barras.
            var grupo5 = $"{ffff}{vvvvvvvvvv}";

            #endregion

            boleto.CodigoBarra.LinhaDigitavel = $"{grupo1}{grupo2}{grupo3}{_dacBoleto} {grupo5}";
        }

        public override void FormataNossoNumero(Boleto boleto)
        {
            if (boleto.Carteira.Equals("SR"))
            {
                if (boleto.NossoNumero.Length == 14)
                    boleto.NossoNumero = "8" + boleto.NossoNumero;
            }

            //� utilizado apenas na impress�o.
            boleto.DigitoNossoNumero = Mod11Base9(boleto.NossoNumero).ToString();

            //boleto.NossoNumero = string.Format("{0}-{1}", boleto.NossoNumero, Mod11Base9(boleto.NossoNumero)); //
            //boleto.NossoNumero = string.Format("{0}{1}/{2}-{3}", boleto.Carteira, EMISSAO_CEDENTE, boleto.NossoNumero, Mod11Base9(boleto.Carteira + EMISSAO_CEDENTE + boleto.NossoNumero));
        }

        public override void FormataNumeroDocumento(Boleto boleto)
        { }

        public override void ValidaBoleto(Boleto boleto)
        {
            if (boleto.NossoNumero.Length != 17)
                throw new Exception("Nosso n�mero inv�lido. Para Caixa Econ�mica - SIGCB carteira r�pida, o nosso n�mero deve conter 17 caracteres.");

            if (boleto.Cedente.DigitoCedente == -1)
                boleto.Cedente.DigitoCedente = Mod11Base9(boleto.Cedente.Codigo);

            if (boleto.DataDocumento == DateTime.MinValue)
                boleto.DataDocumento = DateTime.Now;

            if (boleto.Cedente.Codigo.Length > 6)
                throw new Exception("O c�digo do cedente deve conter apenas 6 d�gitos");

            boleto.LocalPagamento = "PREFERENCIALMENTE NAS CASAS LOT�RICAS AT� O VALOR LIMITE.";

            if (!boleto.Carteira.Equals("CS"))
            {
                FormataCodigoBarra(boleto);
                FormataLinhaDigitavel(boleto);
                FormataNossoNumero(boleto);
            }

            boleto.MensagemSac = "SAC CAIXA: 0800 726 0101 (informa��es, reclama��es, sugest�es e elogios).<br>" +
                                 "Para pessoas com defici�ncia auditiva ou de fala: 0800 726 2492.<br>" +
                                 "Ouvidoria: 0800 725 7474. caixa.gov.br.";
        }

        #region M�todos de gera��o do arquivo remessa

        /// <summary>
        /// Efetua as Valida��es dentro da classe Boleto, para garantir a gera��o da remessa
        /// </summary>
        public override bool ValidarRemessa(TipoArquivo tipoArquivo, string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            var vRetorno = true;
            var vMsg = string.Empty;

            switch (tipoArquivo)
            {
                case TipoArquivo.Cnab240:
                    vRetorno = ValidarRemessaCnab240(numeroConvenio, banco, cedente, boletos, numeroArquivoRemessa, out vMsg);
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

        public override string GerarHeaderRemessa(Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            return GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);
        }

        /// <summary>
        /// HEADER do arquivo CNAB
        /// Gera o HEADER do arquivo remessa de acordo com o lay-out informado
        /// </summary>
        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa)
        {
            try
            {
                var header = " ";

                base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        header = GerarHeaderRemessaCnab240(cedente);
                        break;
                    case TipoArquivo.Cnab400:
                        header = GerarHeaderRemessaCnab400(0, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarHeaderRemessa(string numeroConvenio, Cedente cedente, TipoArquivo tipoArquivo, int numeroArquivoRemessa, Boleto boletos)
        {
            try
            {
                var header = " ";

                base.GerarHeaderRemessa("0", cedente, tipoArquivo, numeroArquivoRemessa);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
                            header = GerarHeaderRemessaCnab240Sigcb(cedente);
                        else
                            header = GerarHeaderRemessaCnab240(cedente);
                        break;
                    case TipoArquivo.Cnab400:
                        header = GerarHeaderRemessaCnab400(0, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER do arquivo de REMESSA.", ex);
            }
        }

        /// <summary>
        /// Gera as linhas de detalhe da remessa.
        /// </summary>
        /// <param name="boleto">Objeto do tipo <see cref="Boleto"/> para o qual as linhas ser�o geradas.</param>
        /// <param name="numeroRegistro">N�mero do registro.</param>
        /// <param name="tipoArquivo"><see cref="TipoArquivo"/> do qual as linhas ser�o geradas.</param>
        /// <returns>Linha gerada</returns>
        /// <remarks>
        /// Esta fun��o n�o existia, mas as fun��es que ela chama j� haviam sido implementadas. 
        /// S� criei esta fun��o pois a original estava chamando o m�todo abstrato em IBanco.
        /// </remarks>
        public new string GerarDetalheRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var detalhe = " ";

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        detalhe = GerarDetalheSegmentoPRemessaCnab240SIGCB(Cedente, boleto, numeroRegistro);
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
                throw new Exception("Erro durante a gera��o do DETALHE arquivo de REMESSA.", ex);
            }
        }

        #endregion

        public override string GerarDetalheSegmentoPRemessa(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente)
        {
            if (boleto.Remessa.TipoDocumento.Equals("2") || boleto.Remessa.TipoDocumento.Equals("1"))
                return GerarDetalheSegmentoPRemessaCnab240SIGCB(cedente, boleto, numeroRegistro);

            return GerarDetalheSegmentoPRemessaCNAB240(boleto, numeroRegistro, numeroConvenio, cedente);
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            return GerarDetalheSegmentoQRemessaCNAB240(boleto, numeroRegistro, tipoArquivo);
        }

        public override string GerarDetalheSegmentoQRemessa(Boleto boleto, int numeroRegistro, Sacado sacado)
        {
            return GerarDetalheSegmentoQRemessaCNAB240SIGCB(boleto, numeroRegistro, sacado);
        }

        public override string GerarDetalheSegmentoRRemessa(Boleto boleto, int numeroRegistroDetalhe, TipoArquivo cnab240)
        {
            if (boleto.Remessa.TipoDocumento.Equals("2") || boleto.Remessa.TipoDocumento.Equals("1"))
                return GerarDetalheSegmentoRRemessaCNAB240SIGCB(boleto, numeroRegistroDetalhe, cnab240);

            return GerarDetalheSegmentoRRemessaCNAB240(boleto, numeroRegistroDetalhe, cnab240);
        }

        public override string GerarTrailerLoteRemessa(int numeroRegistro, Boleto boletos)
        {
            if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
                return GerarTrailerLoteRemessaCNAC240SIGCB(numeroRegistro);

            return GerarTrailerLoteRemessaCNAB240(numeroRegistro);
        }

        /// <summary>
        /// Gera as linhas de trailer da remessa.
        /// </summary>
        /// <param name="numeroRegistro">N�mero do registro.</param>
        /// <param name="tipoArquivo"><see cref="TipoArquivo"/> do qual as linhas ser�o geradas.</param>
        /// <param name="cedente">Objeto do tipo <see cref="Cedente"/> para o qual o trailer ser� gerado.</param>
        /// <param name="vltitulostotal">Valor total dos t�tulos do arquivo.</param>
        /// <returns>Linha gerada.</returns>
        /// <remarks>Esta fun��o n�o existia, mas as fun��es que ela chama j� haviam sido implementadas. S� criei esta fun��o pois a original estava chamando o m�todo abstrato em IBanco.</remarks>
        public override string GerarTrailerRemessa(int numeroRegistro, TipoArquivo tipoArquivo, Cedente cedente, decimal vltitulostotal)
        {
            try
            {
                var trailer = " ";

                base.GerarTrailerRemessa(numeroRegistro, tipoArquivo, cedente, vltitulostotal);

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        trailer = GerarTrailerRemessaCNAB240SIGCB(numeroRegistro);
                        break;
                    case TipoArquivo.Cnab400:
                        trailer = GerarTrailerRemessa400(numeroRegistro, 0);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return trailer;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarTrailerArquivoRemessa(int numeroRegistro, Boleto boletos)
        {
            if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
                return GerarTrailerRemessaCNAB240SIGCB(numeroRegistro);

            return GerarTrailerArquivoRemessaCNAB240(numeroRegistro);
        }

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo)
        {
            try
            {
                var header = " ";

                switch (tipoArquivo)
                {

                    case TipoArquivo.Cnab240:
                        header = GerarHeaderLoteRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Cnab400:
                        //header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }

        public override string GerarHeaderLoteRemessa(string numeroConvenio, Cedente cedente, int numeroArquivoRemessa, TipoArquivo tipoArquivo, Boleto boletos)
        {
            try
            {
                var header = " ";

                switch (tipoArquivo)
                {
                    case TipoArquivo.Cnab240:
                        if (boletos.Remessa.TipoDocumento.Equals("2") || boletos.Remessa.TipoDocumento.Equals("1"))
                            header = GerarHeaderLoteRemessaCNAC240SIGCB(cedente, numeroArquivoRemessa);
                        else
                            header = GerarHeaderLoteRemessaCNAB240(cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Cnab400:
                        //header = GerarHeaderLoteRemessaCNAB400(0, cedente, numeroArquivoRemessa);
                        break;
                    case TipoArquivo.Outro:
                        throw new Exception("Tipo de arquivo inexistente.");
                }

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do HEADER DO LOTE do arquivo de REMESSA.", ex);
            }
        }

        #region CNAB 240

        private bool ValidarRemessaCnab240(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            var vRetorno = true;
            var vMsg = string.Empty;

            #region Pr� Valida��es

            if (banco == null)
            {
                vMsg += string.Concat("Remessa: O Banco � Obrigat�rio!", Environment.NewLine);
                vRetorno = false;
            }

            if (cedente == null)
            {
                vMsg += string.Concat("Remessa: O Cedente/Benefici�rio � Obrigat�rio!", Environment.NewLine);
                vRetorno = false;
            }

            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += string.Concat("Remessa: Dever� existir ao menos 1 boleto para gera��o da remessa!", Environment.NewLine);
                vRetorno = false;
            }

            #endregion

            //Valida��o de cada boleto
            foreach (var boleto in boletos)
            {
                #region Valida��o de cada boleto

                if (boleto.Remessa == null)
                {
                    vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                else if (boleto.Remessa.TipoDocumento != null && boleto.Remessa.TipoDocumento.Equals("1") && string.IsNullOrEmpty(boleto.Sacado.Endereco.Cep)) //1 - SICGB - Com registro
                {
                    //Para o "Remessa.TipoDocumento = "1", o CEP � Obrigat�rio!
                    vMsg += string.Concat("Para o Tipo Documento [1 - SIGCB - COM REGISTRO], o CEP do SACADO � Obrigat�rio!", Environment.NewLine);
                    vRetorno = false;
                }

                if (boleto.NossoNumero.Length > 15)
                    boleto.NossoNumero = boleto.NossoNumero.Substring(0, 15);

                //if (!boleto.Remessa.TipoDocumento.Equals("2")) //2 - SIGCB - SEM REGISTRO
                //{
                //    //Para o "Remessa.TipoDocumento = "2", n�o poder� ter NossoNumero Gerado!
                //    vMsg += String.Concat("Tipo Documento de boleto n�o Implementado!", Environment.NewLine);
                //    vRetorno = false;
                //}

                #endregion
            }

            mensagem = vMsg;
            return vRetorno;
        }

        /// <summary>
        /// Varre as instrucoes para inclusao no Segmento P
        /// </summary>
        /// <param name="boleto"></param>
        private void ValidaInstrucoes240(Boleto boleto)
        {
            if (boleto.Instrucoes.Count.Equals(0))
                return;

            _protestar = false;
            _baixaDevolver = false;
            _desconto = false;
            _diasProtesto = 0;
            _diasDevolucao = 0;
            _diasDesconto = 0;

            foreach (var instrucao in boleto.Instrucoes)
            {
                if (instrucao.Codigo.Equals(9) || instrucao.Codigo.Equals(42) || instrucao.Codigo.Equals(81) || instrucao.Codigo.Equals(82))
                {
                    _protestar = true;
                    _diasProtesto = instrucao.Dias;
                }
                else if (instrucao.Codigo.Equals(91) || instrucao.Codigo.Equals(92))
                {
                    _baixaDevolver = true;
                    _diasDevolucao = instrucao.Dias;
                }
                else if (instrucao.Codigo.Equals(999))
                {
                    _desconto = true;
                    _diasDesconto = instrucao.Dias;
                }
            }
        }

        public string GerarHeaderRemessaCnab240(Cedente cedente)
        {
            try
            {
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // c�digo do banco na compensa��o
                header += "0000";                                                                       // Lote de Servi�o 
                header += "0";                                                                          // Tipo de Registro 
                header += Utils.FormatCode("", " ", 9);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += (cedente.CpfCnpj.Length == 11 ? "1" : "2");                                   // Tipo de Inscri��o 
                header += Utils.FormatCode(cedente.CpfCnpj, "0", 15);                                   // CPF/CNPJ do cedente 
                header += Utils.FormatCode(cedente.Codigo + cedente.DigitoCedente, "0", 16); // C�digo do Conv�nio no Banco 
                header += Utils.FormatCode("", "0", 4);                                                 // Uso Exclusivo CAIXA
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5);                      // Ag�ncia Mantenedora da Conta 
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoAgencia, "0", 5);                // D�gito Verificador da Ag�ncia 
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12);                       // C�digo do Cedente (sem opera��o)  
                header += cedente.ContaBancaria.DigitoConta;                                            // D�g. Verif. Cedente (sem opera��o) 
                header += Mod11(cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta).ToString();// D�gito Verif. Ag./Ced  (sem opera��o)
                header += Utils.FormatCode(cedente.Nome, " ", 30);                                      // Nome do cedente
                header += Utils.FormatCode("CAIXA ECONOMICA FEDERAL", " ", 30);                         // Nome do Banco
                header += Utils.FormatCode("", " ", 10);                                                // Uso Exclusivo FEBRABAN/CNAB
                header += "1";                                                                          // C�digo 1 - Remessa / 2 - Retorno 
                header += DateTime.Now.ToString("ddMMyyyy");                                            // Data de Gera��o do Arquivo
                header += string.Format("{0:hh:mm:ss}", DateTime.Now).Replace(":", "");                  // Hora de Gera��o do Arquivo
                header += "000001";                                                                     // N�mero Seq�encial do Arquivo 
                header += "030";                                                                        // N�mero da Vers�o do Layout do Arquivo 
                header += "0";                                                                          // Densidade de Grava��o do Arquivo 
                header += Utils.FormatCode("", " ", 20);                                                // Para Uso Reservado do Banco
                // Na fase de teste deve conter "remessa-produ��o", ap�s aprovado deve conter espa�os em branco
                header += Utils.FormatCode("remessa-produ��o", " ", 20);                                // Para Uso Reservado da Empresa  
                //header += Utils.FormatCode("", " ", 20);                                              // Para Uso Reservado da Empresa
                header += Utils.FormatCode("", " ", 29);                                                // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240.", ex);
            }
        }

        private string GerarHeaderLoteRemessaCNAB240(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                         // c�digo do banco na compensa��o
                header += "0001";                                                                       // Lote de Servi�o
                header += "1";                                                                          // Tipo de Registro 
                header += "R";                                                                          // Tipo de Opera��o 
                header += "01";                                                                         // Tipo de Servi�o '01' = Cobran�a, '03' = Bloqueto Eletr�nico 
                header += "  ";                                                                         // Uso Exclusivo FEBRABAN/CNAB
                header += "020";                                                                        // N�mero da Vers�o do Layout do Arquivo 
                header += " ";                                                                          // Uso Exclusivo FEBRABAN/CNAB
                header += (cedente.CpfCnpj.Length == 11 ? "1" : "2");                                   // Tipo de Inscri��o 
                header += Utils.FormatCode(cedente.CpfCnpj, "0", 15);                                   // CPF/CNPJ do cedente
                header += Utils.FormatCode(cedente.Codigo + cedente.DigitoCedente, "0", 16); // C�digo do Conv�nio no Banco 
                header += Utils.FormatCode("", " ", 4);                                                 // Uso Exclusivo CAIXA
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5);                      // Ag�ncia Mantenedora da Conta 
                header += Utils.FormatCode(cedente.ContaBancaria.DigitoAgencia, "0", 5);                // D�gito Verificador da Ag�ncia 
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12);                       // N�mero da Conta Corrente 
                header += cedente.ContaBancaria.DigitoConta;                                            // Digito Verificador da Conta Corrente 
                header += Mod11(cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta).ToString();// D�gito Verif. Ag./Ced  (sem opera��o)
                header += Utils.FormatCode(cedente.Nome, " ", 30);                                      // Nome do cedente
                header += Utils.FormatCode("", " ", 40);                                                // Mensagem 1
                header += Utils.FormatCode("", " ", 40);                                                // Mensagem 2
                header += numeroArquivoRemessa.ToString("00000000");                                    // N�mero Remessa/Retorno
                header += DateTime.Now.ToString("ddMMyyyy");                                            // Data de Grava��o Remessa/Retorno 
                header += Utils.FormatCode("", "0", 8);                                                 // Data do Cr�dito 
                header += Utils.FormatCode("", " ", 33);                                                // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar HEADER DO LOTE do arquivo de remessa.", e);
            }
        }

        public string GerarDetalheSegmentoPRemessaCNAB240(Boleto boleto, int numeroRegistro, string numeroConvenio, Cedente cedente)
        {
            try
            {
                ValidaInstrucoes240(boleto); // Para protestar, devolver ou desconto.

                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // c�digo do banco na compensa��o
                header += "0001";                                                                       // Lote de Servi�o
                header += "3";                                                                          // Tipo de Registro 
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 5);                          // N� Sequencial do Registro no Lote 
                header += "P";                                                                          // C�d. Segmento do Registro Detalhe
                header += " ";                                                                          // Uso Exclusivo FEBRABAN/CNAB
                header += "01";                                                                         // C�digo de Movimento Remessa 
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5);                      // Ag�ncia Mantenedora da Conta 
                header += cedente.ContaBancaria.DigitoAgencia;                                          // D�gito Verificador da Ag�ncia 
                header += Utils.FormatCode(cedente.ContaBancaria.Conta, "0", 12);                       // N�mero da Conta Corrente 
                header += cedente.ContaBancaria.DigitoConta;                                            // Digito Verificador da Conta Corrente 
                header += Mod11(cedente.ContaBancaria.Agencia + cedente.ContaBancaria.Conta).ToString(); // D�gito Verif. Ag./Ced  (sem opera��o)
                header += Utils.FormatCode("", "0", 9);                                                 // Uso Exclusivo CAIXA
                header += Utils.FormatCode(boleto.NossoNumero, "0", 11);                                // Identifica��o do T�tulo no Banco 
                header += "01";                                                                         // C�digo da Carteira 
                header += (boleto.Carteira == "14" ? "2" : "1");                                        // Forma de Cadastr. do T�tulo no Banco 
                // '1' = Com Cadastramento (Cobran�a Registrada) 
                // '2' = Sem Cadastramento (Cobran�a sem Registro) 
                header += "2";                                                                          // Tipo de Documento 
                header += "2";                                                                          // Identifica��o da Emiss�o do Bloqueto 
                header += "2";                                                                          // Identifica��o da Distribui��o
                header += Utils.FormatCode(boleto.NumeroDocumento, "0", 11);                            // N�mero do Documento de Cobran�a 
                header += "    ";                                                                       // Uso Exclusivo CAIXA
                header += boleto.DataVencimento.ToString("ddMMyyyy");                                   // Data de Vencimento do T�tulo
                header += Utils.FormatCode(boleto.ValorBoleto.ToString().Replace(",", "").Replace(".", ""), "0", 13); // Valor Nominal do T�tulo 13
                header += Utils.FormatCode(cedente.ContaBancaria.Agencia, "0", 5);                      // Ag�ncia Encarregada da Cobran�a 
                header += cedente.ContaBancaria.DigitoAgencia;                                          // D�gito Verificador da Ag�ncia 
                header += boleto.EspecieDocumento.Codigo;                                // Esp�cie do T�tulo 
                header += boleto.Aceite;                                                                // Identific. de T�tulo Aceito/N�o Aceito
                // Data da Emiss�o do T�tulo 
                header += (boleto.DataProcessamento.ToString("ddMMyyyy") == "01010001" ? DateTime.Now.ToString("ddMMyyyy") : boleto.DataProcessamento.ToString("ddMMyyyy"));
                header += "1";                                                                          // C�digo do Juros de Mora '1' = Valor por Dia - '2' = Taxa Mensal 
                header += (boleto.DataMulta.ToString("ddMMyyyy") == "01010001" ? "00000000" : boleto.DataMulta.ToString("ddMMyyyy")); // Data do Juros de Mora 
                header += Utils.FormatCode(boleto.ValorMulta.ToString().Replace(",", "").Replace(".", ""), "0", 13); // Juros de Mora por Dia/Taxa 
                header += (_desconto ? "1" : "0");                                                       // C�digo do Desconto 
                header += (boleto.DataDesconto.ToString("ddMMyyyy") == "01010001" ? "00000000" : boleto.DataDesconto.ToString("ddMMyyyy")); // Data do Desconto
                header += Utils.FormatCode(boleto.ValorDesconto.ToString().Replace(",", "").Replace(".", ""), "0", 13); // Valor/Percentual a ser Concedido 
                header += Utils.FormatCode(boleto.Iof.ToString().Replace(",", "").Replace(".", ""), "0", 13); // Valor do IOF a ser Recolhido 
                header += Utils.FormatCode(boleto.Abatimento.ToString().Replace(",", "").Replace(".", ""), "0", 13); // Valor do Abatimento 
                header += Utils.FormatCode("", " ", 25);                                                // Identifica��o do T�tulo na Empresa
                header += (_protestar ? "1" : "3");                                                      // C�digo para Protesto
                header += _diasProtesto.ToString("00");                                                  // N�mero de Dias para Protesto 2 posi
                header += (_baixaDevolver ? "1" : "2");                                                  // C�digo para Baixa/Devolu��o 1 posi
                header += _diasDevolucao.ToString("00");                                                 // N�mero de Dias para Baixa/Devolu��o 3 posi
                header += boleto.Moeda.ToString("00");                                                  // C�digo da Moeda 
                header += Utils.FormatCode("", " ", 10);                                                // Uso Exclusivo FEBRABAN/CNAB 
                header += Utils.FormatCode("", " ", 1);                                                 // Uso Exclusivo FEBRABAN/CNAB 

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar SEGMENTO P do arquivo de remessa.", e);
            }
        }

        public string GerarDetalheSegmentoQRemessaCNAB240(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // c�digo do banco na compensa��o
                header += "0001";                                                                       // Lote de Servi�o
                header += "3";                                                                          // Tipo de Registro 
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 5);                          // N� Sequencial do Registro no Lote 
                header += "Q";                                                                          // C�d. Segmento do Registro Detalhe
                header += " ";                                                                          // Uso Exclusivo FEBRABAN/CNAB
                header += "01";                                                                         // C�digo de Movimento Remessa
                header += (boleto.Sacado.CpfCnpj.Length == 11 ? "1" : "2");                             // Tipo de Inscri��o 
                header += Utils.FormatCode(boleto.Sacado.CpfCnpj, "0", 15);                             // N�mero de Inscri��o 
                header += Utils.FormatCode(boleto.Sacado.Nome, " ", 40);                                // Nome
                header += Utils.FormatCode(boleto.Sacado.Endereco.End, " ", 40);                        // Endere�o
                header += Utils.FormatCode(boleto.Sacado.Endereco.Bairro, " ", 15);                     // Bairro 
                header += boleto.Sacado.Endereco.Cep;                                                   // CEP + Sufixo do CEP
                header += Utils.FormatCode(boleto.Sacado.Endereco.Cidade, " ", 15);                     // Cidade 
                header += boleto.Sacado.Endereco.Uf;                                                    // Unidade da Federa��o
                // Estes campos dever�o estar preenchidos quando n�o for o Cedente original do t�tulo.
                header += "0";                                                                          // Tipo de Inscri��o 
                header += Utils.FormatCode("", "0", 15);                                                // N�mero de Inscri��o CPF/CNPJ
                header += Utils.FormatCode("", " ", 40);                                                // Nome do Sacador/Avalista 
                //*********
                header += Utils.FormatCode("", " ", 31);                                                // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar SEGMENTO Q do arquivo de remessa.", e);
            }
        }

        public string GerarDetalheSegmentoRRemessaCNAB240(Boleto boleto, int numeroRegistroDetalhe, TipoArquivo CNAB240)
        {
            try
            {
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                         //C�digo do banco na compensa��o.
                header += "0001";                                                                       //Lote de Servi�o.
                header += "3";                                                                          //Tipo de Registro.
                header += Utils.FormatCode(numeroRegistroDetalhe.ToString(), "0", 5);                   //N� Sequencial do Registro no Lote.
                header += "R";                                                                          //C�d. Segmento do Registro Detalhe.
                header += " ";                                                                          //Uso Exclusivo FEBRABAN/CNAB.
                header += "01";                                                                         //C�digo de Movimento Remessa.
                header += Utils.FormatCode("", " ", 48);                                                //Uso Exclusivo FEBRABAN/CNAB.
                header += "1";                                                                          //C�digo da Multa '1' = Valor Fixo,'2' = Percentual,'0' = Sem Multa.
                header += boleto.DataMulta.ToString("ddMMyyyy");                                        //Data da Multa.
                header += Utils.FormatCode(boleto.ValorMulta.ToString().Replace(",", "").Replace(".", ""), "0", 13); //Valor/Percentual a Ser Aplicado.
                header += Utils.FormatCode("", " ", 10);                                                //Informa��o ao Sacado.
                header += Utils.FormatCode("", " ", 40);                                                //Mensagem 3.
                header += Utils.FormatCode("", " ", 40);                                                //Mensagem 4.
                header += Utils.FormatCode("", " ", 61);                                                //Uso Exclusivo FEBRABAN/CNAB.

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar SEGMENTO Q do arquivo de remessa.", e);
            }
        }

        public string GerarTrailerLoteRemessaCNAB240(int numeroRegistro)
        {
            try
            {
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // c�digo do banco na compensa��o
                header += "0001";                                                                       // Lote de Servi�o
                header += "5";                                                                          // Tipo de Registro 
                header += Utils.FormatCode("", " ", 61);                                                // Uso Exclusivo FEBRABAN/CNAB
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 5);                          // N� Sequencial do Registro no Lote 

                // Totaliza��o da Cobran�a Simples
                header += Utils.FormatCode("", "0", 6);                                                 // Quantidade de T�tulos em Cobran�a
                header += Utils.FormatCode("", "0", 15);                                                // Valor Total dos T�tulos em Carteiras

                header += Utils.FormatCode("", "0", 6);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += Utils.FormatCode("", "0", 15);                                                // Uso Exclusivo FEBRABAN/CNAB 

                // Totaliza��o da Cobran�a Caucionada
                header += Utils.FormatCode("", "0", 6);                                                 // Quantidade de T�tulos em Cobran�a
                header += Utils.FormatCode("", "0", 15);                                                // Valor Total dos T�tulos em Carteiras

                // Totaliza��o da Cobran�a Descontada
                header += Utils.FormatCode("", "0", 6);                                                 // Quantidade de T�tulos em Cobran�a
                header += Utils.FormatCode("", "0", 15);                                                // Valor Total dos T�tulos em Carteiras

                header += Utils.FormatCode("", " ", 8);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += Utils.FormatCode("", " ", 117);                                               // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar Trailer de Lote do arquivo de remessa.", e);
            }
        }

        public string GerarTrailerArquivoRemessaCNAB240(int numeroRegistro)
        {
            try
            {
                var header = Utils.FormatCode(Codigo.ToString(), "0", 3, true);                      // c�digo do banco na compensa��o
                header += "9999";                                                                       // Lote de Servi�o
                header += "9";                                                                          // Tipo de Registro 
                header += Utils.FormatCode("", " ", 9);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += "000001";                                                                     // Quantidade de Lotes do Arquivo
                header += Utils.FormatCode(numeroRegistro.ToString(), "0", 6);                          // Quantidade de Registros do Arquivo
                header += Utils.FormatCode("", " ", 6);                                                 // Uso Exclusivo FEBRABAN/CNAB
                header += Utils.FormatCode("", " ", 205);                                               // Uso Exclusivo FEBRABAN/CNAB

                return header;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar Trailer de arquivo de remessa.", e);
            }
        }

        public override DetalheSegmentoTRetornoCNAB240 LerDetalheSegmentoTRetornoCNAB240(string registro)
        {
            try
            {
                /* 05 */
                if (!registro.Substring(13, 1).Equals(@"T"))
                {
                    throw new Exception("Registro inv�lida. O detalhe n�o possu� as caracter�sticas do segmento T.");
                }
                var segmentoT = new DetalheSegmentoTRetornoCNAB240(registro);
                segmentoT.CodigoBanco = Convert.ToInt32(registro.Substring(0, 3));
                segmentoT.idCodigoMovimento = Convert.ToInt32(registro.Substring(15, 2));
                segmentoT.CodigoMovimento = new CodigoMovimento(001, segmentoT.idCodigoMovimento);
                segmentoT.NossoNumero = registro.Substring(39, 17);
                segmentoT.CodigoCarteira = Convert.ToInt32(registro.Substring(57, 1));
                segmentoT.NumeroDocumento = registro.Substring(58, 11);
                segmentoT.DataVencimento = registro.Substring(73, 8) == "00000000" ? DateTime.Now : DateTime.ParseExact(registro.Substring(73, 8), "ddMMyyyy", CultureInfo.InvariantCulture);
                segmentoT.ValorTitulo = Convert.ToDecimal(registro.Substring(81, 15)) / Decimal100;
                segmentoT.IdentificacaoTituloEmpresa = registro.Substring(105, 25);
                segmentoT.TipoInscricao = Convert.ToInt32(registro.Substring(132, 1));
                segmentoT.NumeroInscricao = registro.Substring(133, 15);
                segmentoT.NomeSacado = registro.Substring(148, 40);
                segmentoT.ValorTarifas = Convert.ToDecimal(registro.Substring(198, 15)) / Decimal100;
                segmentoT.CodigoRejeicao = registro.Substring(213, 10);

                return segmentoT;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO T.", ex);
            }
        }

        public override DetalheSegmentoURetornoCNAB240 LerDetalheSegmentoURetornoCNAB240(string registro)
        {
            try
            {
                if (!registro.Substring(13, 1).Equals(@"U"))
                {
                    throw new Exception("Registro inv�lida. O detalhe n�o possu� as caracter�sticas do segmento U.");
                }

                var segmentoU = new DetalheSegmentoURetornoCNAB240(registro);
                segmentoU.CodigoOcorrenciaSacado = registro.Substring(15, 2);
                segmentoU.JurosMultaEncargos = Convert.ToDecimal(registro.Substring(17, 15)) / Decimal100;
                segmentoU.ValorDescontoConcedido = Convert.ToDecimal(registro.Substring(32, 15)) / Decimal100;
                segmentoU.ValorAbatimentoConcedido = Convert.ToDecimal(registro.Substring(47, 15)) / Decimal100;
                segmentoU.ValorIOFRecolhido = Convert.ToDecimal(registro.Substring(62, 15)) / Decimal100;
                segmentoU.ValorOcorrenciaSacado = segmentoU.ValorPagoPeloSacado = Convert.ToDecimal(registro.Substring(77, 15)) / Decimal100;
                segmentoU.ValorLiquidoASerCreditado = Convert.ToDecimal(registro.Substring(92, 15)) / Decimal100;
                segmentoU.ValorOutrasDespesas = Convert.ToDecimal(registro.Substring(107, 15)) / Decimal100;
                segmentoU.ValorOutrosCreditos = Convert.ToDecimal(registro.Substring(122, 15)) / Decimal100;
                segmentoU.DataOcorrencia = segmentoU.DataOcorrencia = DateTime.ParseExact(registro.Substring(137, 8), "ddMMyyyy", CultureInfo.InvariantCulture);
                segmentoU.DataCredito = registro.Substring(145, 8).Equals("00000000") ? DateTime.Now : DateTime.ParseExact(registro.Substring(145, 8), "ddMMyyyy", CultureInfo.InvariantCulture);                

                return segmentoU;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar arquivo de RETORNO - SEGMENTO U.", ex);
            }
        }

        #endregion

        #region CNAB 240 - SIGCB

        public string GerarHeaderRemessaCnab240Sigcb(Cedente cedente)
        {
            try
            {
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0001, 003, 0, Codigo, '0'));                                 // posi��o 1 at� 3     (3) - c�digo do banco na compensa��o        
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0004, 004, 0, "0000", '0'));                                      // posi��o 4 at� 7     (4) - Lote de Servi�o
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0008, 001, 0, "0", '0'));                                         // posi��o 8 at� 8     (1) - Tipo de Registro
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0009, 009, 0, string.Empty, ' '));                                // posi��o 9 at� 17     (9) - Uso Exclusivo FEBRABAN/CNAB

                #region Regra Tipo de Inscri��o Cedente
                var vCpfCnpjEmi = "0";//n�o informado
                if (cedente.CpfCnpj.Length.Equals(11)) vCpfCnpjEmi = "1"; //Cpf
                else if (cedente.CpfCnpj.Length.Equals(14)) vCpfCnpjEmi = "2"; //Cnpj
                #endregion

                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0018, 001, 0, vCpfCnpjEmi, '0'));                                  // posi��o 18 at� 18   (1) - Tipo de Inscri��o 
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0019, 014, 0, cedente.CpfCnpj, '0'));                              // posi��o 19 at� 32   (14)- N�mero de Inscri��o da empresa
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0033, 020, 0, "0", '0'));                                          // posi��o 33 at� 52   (20)- Uso Exclusivo CAIXA
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0053, 005, 0, cedente.ContaBancaria.Agencia, '0'));                // posi��o 53 at� 57   (5) - Ag�ncia Mantenedora da Conta
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0058, 001, 0, cedente.ContaBancaria.DigitoAgencia.ToUpper(), ' '));// posi��o 58 at� 58   (1) - D�gito Verificador da Ag�ncia
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0059, 006, 0, cedente.Convenio, '0'));                             // posi��o 59 at� 64   (6) - C�digo do Conv�nio no Banco (C�digo do Cedente)
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0065, 007, 0, "0", '0'));                                          // posi��o 65 at� 71   (7) - Uso Exclusivo CAIXA
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0072, 001, 0, "0", '0'));                                       // posi��o 72 at� 72   (1) - Uso Exclusivo CAIXA
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0073, 030, 0, cedente.Nome.ToUpper(), ' '));                       // posi��o 73 at� 102  (30)- Nome da Empresa
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0103, 030, 0, "CAIXA ECONOMICA FEDERAL", ' '));                    // posi��o 103 at� 132 (30)- Nome do Banco
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0133, 010, 0, string.Empty, ' '));                                 // posi��o 133 at� 142 (10)- Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0143, 001, 0, "1", '0'));                                          // posi��o 143 at� 413 (1) - C�digo 1 - Remessa / 2 - Retorno
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAAAA_________, 0144, 008, 0, DateTime.Now, ' '));                                 // posi��o 144 at� 151 (8) - Data de Gera��o do Arquivo
                reg.CamposEdi.Add(new CampoEdi(Dado.HoraHHMMSS___________, 0152, 006, 0, DateTime.Now, ' '));                                 // posi��o 152 at� 157 (6) - Hora de Gera��o do Arquivo
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0158, 006, 0, cedente.NumeroSequencial, '0'));                     // posi��o 158 at� 163 (6) - N�mero Seq�encial do Arquivo
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0164, 003, 0, "050", '0'));                                        // posi��o 164 at� 166 (3) - Nro da Vers�o do Layout do Arquivo
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0167, 005, 0, "0", '0'));                                          // posi��o 167 at� 171 (5) - Densidade de Grava��o do Arquivo
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0172, 020, 0, string.Empty, ' '));                                 // posi��o 172 at� 191 (20)- Para Uso Reservado do Banco
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0192, 020, 0, "REMESSA-PRODUCAO", ' '));                           // posi��o 192 at� 211 (20)- Para Uso Reservado da Empresa
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0212, 004, 0, string.Empty, ' '));                                 // posi��o 212 at� 215 (4) - Vers�o Aplicativo CAIXA
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0216, 025, 0, string.Empty, ' '));                                 // posi��o 216 at� 240 (25)- Para Uso Reservado da Empresa

                reg.CodificarLinha();

                return Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }

        public string GerarHeaderLoteRemessaCNAC240SIGCB(Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0001, 003, 0, Codigo, '0'));                                   // posi��o 1 at� 3     (3) - c�digo do banco na compensa��o        
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0004, 004, 0, 1, '0'));                                  // posi��o 4 at� 7     (4) - Lote de Servi�o
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0008, 001, 0, "1", '0'));                                           // posi��o 8 at� 8     (1) - Tipo de Registro
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0009, 001, 0, "R", ' '));                                           // posi��o 9 at� 9     (1) - Tipo de Opera��o
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0010, 002, 0, "01", '0'));                                          // posi��o 10 at� 11   (2) - Tipo de Servi�o
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0012, 002, 0, "00", '0'));                                          // posi��o 12 at� 13   (2) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0014, 003, 0, "030", '0'));                                         // posi��o 14 at� 16   (3) - N� da Vers�o do Layout do Lote
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0017, 001, 0, string.Empty, ' '));                                  // posi��o 17 at� 17   (1) - Uso Exclusivo FEBRABAN/CNAB
                #region Regra Tipo de Inscri��o Cedente
                var vCpfCnpjEmi = "0";//n�o informado
                if (cedente.CpfCnpj.Length.Equals(11)) vCpfCnpjEmi = "1"; //Cpf
                else if (cedente.CpfCnpj.Length.Equals(14)) vCpfCnpjEmi = "2"; //Cnpj
                #endregion
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0018, 001, 0, vCpfCnpjEmi, '0'));                                   // posi��o 18 at� 18   (1) - Tipo de Inscri��o 
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0019, 015, 0, cedente.CpfCnpj, '0'));                               // posi��o 19 at� 33   (15)- N�mero de Inscri��o da empresa
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0034, 006, 0, cedente.Convenio, '0'));                              // posi��o 34 at� 39   (6) - C�digo do Conv�nio no Banco
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0040, 014, 0, "0", '0'));                                           // posi��o 40 at� 53   (14)- Uso Exclusivo CAIXA
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0054, 005, 0, cedente.ContaBancaria.Agencia, '0'));                 // posi��o 54 at� 58   (5) - Ag�ncia Mantenedora da Conta
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0059, 001, 0, cedente.ContaBancaria.DigitoAgencia.ToUpper(), ' ')); // posi��o 59 at� 59   (1) - D�gito Verificador da Ag�ncia
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0060, 006, 0, cedente.Convenio, '0'));                              // posi��o 60 at� 65   (6) - C�digo do Conv�nio no Banco                
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0066, 007, 0, "0", '0'));                                           // posi��o 66 at� 72   (7) - C�digo do Modelo Personalizado
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0073, 001, 0, "0", '0'));                                           // posi��o 73 at� 73   (1) - Uso Exclusivo CAIXA
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0074, 030, 0, cedente.Nome.ToUpper(), ' '));                        // posi��o 73 at� 103  (30)- Nome da Empresa     
                //TODO.: ROGER KLEIN - INSTRU��ES N�O TRATADAS
                #region Instru��es
                //string descricao = string.Empty;
                ////
                var vInstrucao1 = string.Empty;
                var vInstrucao2 = string.Empty;
                //foreach (Instrucao_Caixa instrucao in boleto.Instrucoes)
                //{
                //    switch ((EnumInstrucoes_Caixa)instrucao.Codigo)
                //    {
                //        case EnumInstrucoes_Caixa.Protestar:
                //            //
                //            //instrucao.Descricao.ToString().ToUpper();
                //            break;
                //        case EnumInstrucoes_Caixa.NaoProtestar:
                //            //
                //            break;
                //        case EnumInstrucoes_Caixa.ImportanciaporDiaDesconto:
                //            //
                //            break;
                //        case EnumInstrucoes_Caixa.ProtestoFinsFalimentares:
                //            //
                //            break;
                //        case EnumInstrucoes_Caixa.ProtestarAposNDiasCorridos:
                //            break;
                //    }
                //}
                #endregion
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0104, 040, 0, vInstrucao1, ' '));                                   // posi��o 104 at� 143 (40) - Mensagem 1
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0144, 040, 0, vInstrucao2, ' '));                                   // posi��o 144 at� 183 (40) - Mensagem 2
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0184, 008, 0, numeroArquivoRemessa, '0'));                          // posi��o 184 at� 191 (8)  - N�mero Remessa/Retorno
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAAAA_________, 0192, 008, 0, DateTime.Now, ' '));                                  // posi��o 192 at� 199 (8) - Data de Gera��o do Arquivo                
                /*Data do Cr�dito
                Data de efetiva��o do cr�dito referente ao pagamento do t�tulo de cobran�a. 
                Informa��o enviada somente no arquivo de retorno. 2.1 Data do Cr�dito Filler 200 207 9(008) Preencher com zeros C003 */
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0200, 008, 0, '0', '0'));                             // posi��o 200 at� 207 (8) - Data do Cr�dito
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0208, 033, 0, string.Empty, ' '));                                  // posi��o 208 at� 240(33) - Uso Exclusivo FEBRABAN/CNAB
                //
                reg.CodificarLinha();
                //
                var vLinha = reg.LinhaRegistro;
                var _headerLote = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _headerLote;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do lote no arquivo de remessa do CNAB400.", ex);
            }
        }

        #region Detalhes

        public string GerarDetalheSegmentoPRemessaCnab240SIGCB(Cedente cedente, Boleto boleto, int numeroRegistro)
        {
            try
            {
                #region Segmento P

                ValidaInstrucoes240(boleto);

                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0001, 003, 0, Codigo, '0'));                                        // posi��o 1 at� 3     (3) - C�digo do banco na compensa��o        
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0004, 004, 0, "1", '0'));                                           // posi��o 4 at� 7     (4) - Lote de Servi�o
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0008, 001, 0, "3", '0'));                                           // posi��o 8 at� 8     (1) - Tipo de Registro
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0009, 005, 0, numeroRegistro, '0'));                                // posi��o 9 at� 13    (5) - N� Sequencial do Registro no Lote
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0014, 001, 0, "P", '0'));                                           // posi��o 14 at� 14   (1) - C�d. Segmento do Registro Detalhe
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0015, 001, 0, string.Empty, ' '));                                  // posi��o 15 at� 15   (1) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0016, 002, 0, ObterCodigoDaOcorrencia(boleto), '0'));                                          // posi��o 16 at� 17   (2) - C�digo de Movimento Remessa
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0018, 005, 0, cedente.ContaBancaria.Agencia, '0'));                 // posi��o 18 at� 22   (5) - Ag�ncia Mantenedora da Conta
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0023, 001, 0, cedente.ContaBancaria.DigitoAgencia.ToUpper(), ' ')); // posi��o 23 at� 23   (1) - D�gito Verificador da Ag�ncia
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0024, 006, 0, cedente.Convenio, '0'));                              // posi��o 24 at� 29   (6) - C�digo do Conv�nio no Banco
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0030, 011, 0, "0", '0'));                                           // posi��o 30 at� 40   (11)- Uso Exclusivo CAIXA
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0041, 017, 0, boleto.NossoNumero, '0'));                            // posi��o 43 at� 57   (15)- Identifica��o do T�tulo no Banco
                                                                                                                                                                    //A modalidade s�o os dois algarimos iniciais do nosso n�mero, j� concatenados, ent�o passa direto o nosso nro que preenche os dois campos do leiaute.
                #region C�digo da Carteira

                //C�digo adotado pela FEBRABAN, para identificar a caracter�stica dos t�tulos dentro das modalidades de cobran�a existentes no banco.
                //�1� = Cobran�a Simples; �3� = Cobran�a Caucionada; �4� = Cobran�a Descontada
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0058, 001, 0, "1", '0'));                                           // posi��o 58 at� 58   (1) - C�digo Carteira

                #endregion

                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0059, 001, 0, "1", '0'));                                           // posi��o 59 at� 59   (1) - Forma de Cadastr. do T�tulo no Banco 1 - Com Registro 2 - Sem registro.

                //Tratamento do tipo Cobran�a (com ou sem registro).
                var emissao = boleto.Carteira.Equals("CS") ? "1" : "2";

                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0060, 001, 0, "2", '0'));                                           // posi��o 60 at� 60   (1) - Tipo de Documento
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0061, 001, 0, emissao, '0'));                                       // posi��o 61 at� 61   (1) - Identifica��o da Emiss�o do Bloqueto -- �1�-Banco Emite, '2'-entrega do bloqueto pelo Cedente                           
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0062, 001, 0, "0", '0'));                                           // posi��o 62 at� 62   (1) - Identifica��o da Entrega do Bloqueto /* �0� = Postagem pelo Cedente �1� = Sacado via Correios �2� = Cedente via Ag�ncia CAIXA*/ 
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0063, 011, 0, boleto.NumeroDocumento, ' '));                        // posi��o 63 at� 73   (11)- N�mero do Documento de Cobran�a
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0074, 004, 0, string.Empty, ' '));                                  // posi��o 74 at� 77   (4) - Uso Exclusivo CAIXA
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAAAA_________, 0078, 008, 0, boleto.DataVencimento, ' '));                         // posi��o 78 at� 85   (8) - Data de Vencimento do T�tulo
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0086, 015, 2, boleto.ValorBoleto, '0'));                            // posi��o 86 at� 100  (15)- Valor Nominal do T�tulo
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0101, 005, 2, "0", '0'));                                           // posi��o 101 at� 105 (5) - AEC = Ag�ncia Encarregada da Cobran�a //Sistema atribui AEC pelo CEP do sacado.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0106, 001, 0, "0", ' '));                                           // posi��o 106 at� 106 (1) - D�gito Verificador da Ag�ncia
                var espDoc = boleto.EspecieDocumento.Sigla.Equals("DM") ? "02" : boleto.EspecieDocumento.Codigo;
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0107, 002, 2, espDoc, '0'));                                        // posi��o 107 at� 108 (2) - Esp�cie do T�tulo
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0109, 001, 0, boleto.Aceite, ' '));                                 // posi��o 109 at� 109 (1) - Identific. de T�tulo Aceito/N�o Aceito
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAAAA_________, 0110, 008, 0, boleto.DataDocumento, '0'));                          // posi��o 110 at� 117 (8) - Data da Emiss�o do T�tulo

                #region C�digo de juros

                string codJurosMora;
                if (boleto.JurosMora == 0 && boleto.PercJurosMora == 0)
                    codJurosMora = "3";
                else
                    codJurosMora = "1";

                #endregion

                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0118, 001, 2, codJurosMora, '0'));                                  // posi��o 118 at� 118 (1) - C�digo do Juros de Mora
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAAAA_________, 0119, 008, 0, boleto.DataJurosMora, '0'));                          // posi��o 119 at� 126 (8) - Data do Juros de Mora
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0127, 015, 2, boleto.JurosMora, '0'));                              // posi��o 127 at� 141 (15)- Juros de Mora por Dia/Taxa
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0142, 001, 0, "0", '0'));                                           // posi��o 142 at� 142 (1) - C�digo do Desconto 1 - "0" = Sem desconto. "1"= Valor Fixo at� a data informada "2" = Percentual at� a data informada
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAAAA_________, 0143, 008, 0, boleto.DataDesconto, '0'));                           // posi��o 143 at� 150 (8) - Data do Desconto
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0151, 015, 2, boleto.ValorDesconto, '0'));                          // posi��o 151 at� 165 (15)- Valor/Percentual a ser Concedido
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0166, 015, 2, boleto.Iof, '0'));                                    // posi��o 166 at� 180 (15)- Valor do IOF a ser concedido
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0181, 015, 2, boleto.Abatimento, '0'));                             // posi��o 181 at� 195 (15)- Valor do Abatimento
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0196, 025, 0, boleto.NumeroDocumento, ' '));                        // posi��o 196 at� 220 (25)- Identifica��o do T�tulo na Empresa. Informar o N�mero do Documento - Seu N�mero (mesmo das posi��es 63-73 do Segmento P)                
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0221, 001, 0, (_protestar ? "1" : "3"), '0'));                      // posi��o 221 at� 221 (1) - C�digo para protesto  - �1� = Protestar. "3" = N�o Protestar. "9" = Cancelamento Protesto Autom�tico
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0222, 002, 0, _diasProtesto, '0'));                                 // posi��o 222 at� 223 (2) - N�mero de Dias para Protesto                
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0224, 001, 0, (_baixaDevolver ? "1" : "2"), '0'));                  // posi��o 224 at� 224 (1) - C�digo para Baixa/Devolu��o �1� = Baixar / Devolver. "2" = N�o Baixar / N�o Devolver
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0225, 003, 0, _diasDevolucao, '0'));                                // posi��o 225 at� 227 (3) - N�mero de Dias para Baixa/Devolu��o
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0228, 002, 0, "09", '0'));                                          // posi��o 228 at� 229 (2) - C�digo da Moeda. Informar fixo: �09� = REAL
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0230, 010, 2, "0", '0'));                                           // posi��o 230 at� 239 (10)- Uso Exclusivo CAIXA                
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0240, 001, 0, string.Empty, ' '));                                  // posi��o 240 at� 240 (1) - Uso Exclusivo FEBRABAN/CNAB

                reg.CodificarLinha();

                var vLinha = reg.LinhaRegistro;
                var segmentoP = Utils.SubstituiCaracteresEspeciais(vLinha);

                return segmentoP;

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do Segmento P no arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }

        public string GerarDetalheSegmentoQRemessaCNAB240SIGCB(Boleto boleto, int numeroRegistro, Sacado sacado)
        {
            try
            {
                #region Segmento Q
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0001, 003, 0, Codigo, '0'));                                  // posi��o 1 at� 3     (3) - c�digo do banco na compensa��o        
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0004, 004, 0, "1", '0'));                                          // posi��o 4 at� 7     (4) - Lote de Servi�o
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0008, 001, 0, "3", '0'));                                          // posi��o 8 at� 8     (1) - Tipo de Registro
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0009, 005, 0, numeroRegistro, '0'));                               // posi��o 9 at� 13    (5) - N� Sequencial do Registro no Lote
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0014, 001, 0, "Q", '0'));                                          // posi��o 14 at� 14   (1) - C�d. Segmento do Registro Detalhe
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0015, 001, 0, string.Empty, ' '));                                 // posi��o 15 at� 15   (1) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0016, 002, 0, ObterCodigoDaOcorrencia(boleto), '0'));                                         // posi��o 16 at� 17   (2) - C�digo de Movimento Remessa
                #region Regra Tipo de Inscri��o Cedente
                var vCpfCnpjEmi = "0";//n�o informado
                if (sacado.CpfCnpj.Length.Equals(11)) vCpfCnpjEmi = "1"; //Cpf
                else if (sacado.CpfCnpj.Length.Equals(14)) vCpfCnpjEmi = "2"; //Cnpj
                #endregion
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0018, 001, 0, vCpfCnpjEmi, '0'));                                  // posi��o 18 at� 18   (1) - Tipo de Inscri��o 
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0019, 015, 0, sacado.CpfCnpj, '0'));                               // posi��o 19 at� 33   (15)- N�mero de Inscri��o da empresa
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0034, 040, 0, sacado.Nome.ToUpper(), ' '));                        // posi��o 34 at� 73   (40)- Nome
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0074, 040, 0, sacado.Endereco.End.ToUpper(), ' '));                // posi��o 74 at� 113  (40)- Endere�o
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0114, 015, 0, sacado.Endereco.Bairro.ToUpper(), ' '));             // posi��o 114 at� 128 (15)- Bairro
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0129, 008, 0, sacado.Endereco.Cep, ' '));                          // posi��o 114 at� 128 (40)- CEP                
                // posi��o 134 at� 136 (3) - sufixo cep** j� incluso em CEP                                                                                                                                                                   
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0137, 015, 0, sacado.Endereco.Cidade.ToUpper(), ' '));             // posi��o 137 at� 151 (15)- Cidade
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0152, 002, 0, sacado.Endereco.Uf, ' '));                           // posi��o 152 at� 153 (15)- UF
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0154, 001, 0, "0", '0'));                                          // posi��o 154 at� 154 (1) - Tipo de Inscri��o Sacador Avalialista
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0155, 015, 0, "0", '0'));                                          // posi��o 155 at� 169 (15)- Inscri��o Sacador Avalialista
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0170, 040, 0, string.Empty, ' '));                                 // posi��o 170 at� 209 (40)- Nome do Sacador/Avalista
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0210, 003, 0, string.Empty, ' '));                                          // posi��o 210 at� 212 (3) - C�d. Bco. Corresp. na Compensa��o
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0213, 020, 0, string.Empty, ' '));                                 // posi��o 213 at� 232 (20)- Nosso N� no Banco Correspondente
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0233, 008, 0, string.Empty, ' '));                                 // posi��o 213 at� 232 (8)- Uso Exclusivo FEBRABAN/CNAB
                reg.CodificarLinha();
                //
                var vLinha = reg.LinhaRegistro;
                var _SegmentoQ = Utils.SubstituiCaracteresEspeciais(vLinha);

                return _SegmentoQ;
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do Segmento Q no arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }

        public string GerarDetalheSegmentoRRemessaCNAB240SIGCB(Boleto boleto, int numeroRegistro, TipoArquivo CNAB240)
        {
            try
            {
                #region Segmento R
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0001, 003, 0, Codigo, '0'));                                  // posi��o 1 at� 3     (3) - c�digo do banco na compensa��o        
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0004, 004, 0, "1", '0'));                                          // posi��o 4 at� 7     (4) - Lote de Servi�o
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0008, 001, 0, "3", '0'));                                          // posi��o 8 at� 8     (1) - Tipo de Registro
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0009, 005, 0, numeroRegistro, '0'));                               // posi��o 9 at� 13    (5) - N� Sequencial do Registro no Lote
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0014, 001, 0, "R", '0'));                                          // posi��o 14 at� 14   (1) - C�d. Segmento do Registro Detalhe
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0015, 001, 0, string.Empty, ' '));                                 // posi��o 15 at� 15   (1) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0016, 002, 0, ObterCodigoDaOcorrencia(boleto), '0'));              // posi��o 16 at� 17   (2) - C�digo de Movimento Remessa
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0018, 001, 0, "0", '0'));                                          // posi��o 18 at� 18   (1) - C�digo de desconto 2
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0019, 008, 0, "0", '0'));                                          // posi��o 19 at� 26   (8) - Data de Desconto 2
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0027, 015, 2, "0", '0'));                                          // posi��o 27 at� 41   (15) - Valor ou Percentual desconto 2
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0042, 001, 0, "0", '0'));                                          // posi��o 42 at� 42   (1) - C�digo de desconto 3
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0043, 008, 0, "0", '0'));                                          // posi��o 43 at� 50   (8) - Data de Desconto 3
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0051, 015, 2, "0", '0'));                                          // posi��o 51 at� 65   (15) - Valor ou Percentual desconto 3
                #region C�digo de Multa
                string CodMulta;
                decimal ValorOuPercMulta;
                if (boleto.ValorMulta > 0)
                {
                    CodMulta = "1";
                    ValorOuPercMulta = boleto.ValorMulta;
                }
                else if (boleto.PercMulta > 0)
                {
                    CodMulta = "2";
                    ValorOuPercMulta = boleto.PercMulta;
                }
                else
                {
                    CodMulta = "0";
                    ValorOuPercMulta = 0;
                }
                #endregion
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0066, 001, 0, CodMulta, '0'));                                     // posi��o 66 at� 66   (1) - C�digo da Multa
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAAAA_________, 0067, 008, 0, boleto.DataMulta, '0'));                             // posi��o 67 at� 74   (8) - Data da Multa
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0075, 015, 2, ValorOuPercMulta, '0'));                             // posi��o 75 at� 89   (15) - Valor ou Percentual Multa
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0090, 010, 0, string.Empty, ' '));                                 // posi��o 90 at� 99   (10) - Informa��o ao Pagador
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0100, 040, 0, string.Empty, ' '));                                 // posi��o 100 at� 139 (40) - Mensagem 3
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0140, 040, 0, string.Empty, ' '));                                 // posi��o 140 at� 179 (40) - Mensagem 4
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0180, 050, 0, string.Empty, ' '));                                 // posi��o 180 at� 229 (50) - E-mail pagador p/envio de informa��es
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0230, 011, 0, string.Empty, ' '));                                 // posi��o 230 at� 240 (11) - Filler
                reg.CodificarLinha();
                //
                var vLinha = reg.LinhaRegistro;
                var _SegmentoR = Utils.SubstituiCaracteresEspeciais(vLinha);

                return _SegmentoR;
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Gerar DETALHE do Segmento R no arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }

        public string GerarDetalheSegmentoYRemessaCNAB240SIGCB()
        {
            try
            {
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Gerar DETALHE do Segmento Y no arquivo de remessa do CNAB240 SIGCB.", ex);
            }
        }

        #endregion

        public string GerarTrailerLoteRemessaCNAC240SIGCB(int numeroRegistro)
        {
            try
            {
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0001, 003, 0, Codigo, '0'));                               // posi��o 1 at� 3     (3) - c�digo do banco na compensa��o        
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0004, 004, 0, "1", '0'));                                  // posi��o 4 at� 7     (4) - Lote de Servi�o
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0008, 001, 0, "5", '0'));                                  // posi��o 8 at� 8     (1) - Tipo de Registro
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0009, 009, 0, string.Empty, ' '));                         // posi��o 9 at� 17    (9) - Uso Exclusivo FEBRABAN/CNAB
                #region Pega o Numero de Registros - J� est� sendo Adicionado pelo ArquivoRemessaCNAB240
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0018, 006, 0, numeroRegistro, '0'));                                  // posi��o 18 at� 23   (6) - Quantidade de Registros no Lote
                #endregion
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0024, 006, 0, "0", '0'));                                  // posi��o 24 at� 29   (6) - Quantidade de T�tulos em Cobran�a
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0030, 017, 2, "0", '0'));                                  // posi��o 30 at� 46  (15) - Valor Total dos T�tulos em Carteiras
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0047, 006, 0, "0", '0'));                                  // posi��o 47 at� 52   (6) - Quantidade de T�tulos em Cobran�a
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0053, 017, 2, "0", '0'));                                  // posi��o 53 at� 69   (15) - Valor Total dos T�tulos em Carteiras                
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0070, 006, 0, "0", '0'));                                  // posi��o 70 at� 75   (6) - Quantidade de T�tulos em Cobran�a
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0076, 017, 2, "0", '0'));                                  // posi��o 76 at� 92   (15)- Quantidade de T�tulos em Carteiras 
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0093, 031, 0, string.Empty, ' '));                         // posi��o 93 at� 123  (31) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0124, 117, 0, string.Empty, ' '));                         // posi��o 124 at� 240  (117)- Uso Exclusivo FEBRABAN/CNAB                
                reg.CodificarLinha();
                //
                var vLinha = reg.LinhaRegistro;
                var _headerLote = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _headerLote;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do lote no arquivo de remessa do CNAB400.", ex);
            }
        }

        public string GerarTrailerRemessaCNAB240SIGCB(int numeroRegistro)
        {
            try
            {
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0001, 003, 0, Codigo, '0'));     //posi��o 1 at� 3      (3) - C�digo do Banco na Compensa��o
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0004, 004, 0, "9999", '0'));          // posi��o 4 at� 7     (4) - Lote de Servi�o
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0008, 001, 0, "9", '0'));             // posi��o 8 at� 8     (1) - Tipo de Registro
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0009, 009, 0, string.Empty, ' '));    // posi��o 9 at� 17    (9) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0018, 006, 0, "1", '0'));             // posi��o 18 at� 23   (6) - Quantidade de Lotes do Arquivo

                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0024, 006, 0, numeroRegistro, '0')); // posi��o 24 at� 29   (6) - Quantidade de Registros do Arquivo
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0030, 006, 0, string.Empty, ' '));    // posi��o 30 at� 35   (6) - Uso Exclusivo FEBRABAN/CNAB
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0036, 205, 0, string.Empty, ' '));    // posi��o 36 at� 240(205) - Uso Exclusivo FEBRABAN/CNAB
                //
                reg.CodificarLinha();
                //
                var vLinha = reg.LinhaRegistro;
                var _trailer = Utils.SubstituiCaracteresEspeciais(vLinha);
                //
                return _trailer;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        #endregion

        #region CNAB 400

        private bool ValidarRemessaCnab400(string numeroConvenio, IBanco banco, Cedente cedente, Boletos boletos, int numeroArquivoRemessa, out string mensagem)
        {
            var vRetorno = true;
            var vMsg = string.Empty;

            #region Pr� Valida��es

            if (banco == null)
            {
                vMsg += string.Concat("Remessa: O Banco � Obrigat�rio!", Environment.NewLine);
                vRetorno = false;
            }

            if (cedente == null)
            {
                vMsg += string.Concat("Remessa: O Cedente/Benefici�rio � Obrigat�rio!", Environment.NewLine);
                vRetorno = false;
            }

            if (boletos == null || boletos.Count.Equals(0))
            {
                vMsg += string.Concat("Remessa: Dever� existir ao menos 1 boleto para gera��o da remessa!", Environment.NewLine);
                vRetorno = false;
            }

            #endregion

            foreach (var boleto in boletos)
            {
                #region Valida��o de cada boleto

                if (boleto.Remessa == null)
                {
                    vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe as diretrizes de remessa!", Environment.NewLine);
                    vRetorno = false;
                }
                if (boleto.Sacado == null)
                {
                    vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Sacado: Informe os dados do sacado!", Environment.NewLine);
                    vRetorno = false;
                }
                else
                {
                    if (boleto.Sacado.Nome == null)
                    {
                        vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Nome: Informe o nome do sacado!", Environment.NewLine);
                        vRetorno = false;
                    }

                    if (boleto.Sacado.CpfCnpj == null || boleto.Sacado.CpfCnpj == "")
                    {
                        vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; CPF/CNPJ: Informe o CPF ou CNPJ do sacado!", Environment.NewLine);
                        vRetorno = false;
                    }

                    if (boleto.Sacado.Endereco == null)
                    {
                        vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Endere�o: Informe o endere�o do sacado!", Environment.NewLine);
                        vRetorno = false;
                    }
                    else
                    {
                        if (boleto.Sacado.Endereco.End == null || boleto.Sacado.Endereco.End == "")
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Endere�o: Informe o Endere�o do sacado!", Environment.NewLine);
                            vRetorno = false;
                        }
                        if (boleto.Sacado.Endereco.Bairro == null || boleto.Sacado.Endereco.Bairro == "")
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Endere�o: Informe o Bairro do sacado!", Environment.NewLine);
                            vRetorno = false;
                        }
                        if (boleto.Sacado.Endereco.Cep == null || boleto.Sacado.Endereco.Cep == "" || boleto.Sacado.Endereco.Cep == "00000000")
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Endere�o: Informe o CEP do sacado!", Environment.NewLine);
                            vRetorno = false;
                        }
                        if (boleto.Sacado.Endereco.Cidade == null || boleto.Sacado.Endereco.Cidade == "")
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Endere�o: Informe a cidade do sacado!", Environment.NewLine);
                            vRetorno = false;
                        }
                        if (boleto.Sacado.Endereco.Uf == null || boleto.Sacado.Endereco.Uf == "")
                        {
                            vMsg += string.Concat("Boleto: ", boleto.NumeroDocumento, "; Endere�o: Informe a UF do sacado!", Environment.NewLine);
                            vRetorno = false;
                        }
                    }
                }

                #region OLD
                //else
                //{
                //    //#region Valida��es da Remessa que dever�o estar preenchidas quando CAIXA
                //    //if (String.IsNullOrEmpty(boleto.Remessa.Ambiente))
                //    //{

                //    //    vMsg += String.Concat("Boleto: ", boleto.NumeroDocumento, "; Remessa: Informe o Tipo Documento!", Environment.NewLine);
                //    //    vRetorno = false;
                //    //}

                //    //#endregion
                //}
                #endregion OLD

                #endregion
            }

            mensagem = vMsg;
            return vRetorno;
        }

        private string GerarHeaderRemessaCnab400(int numeroConvenio, Cedente cedente, int numeroArquivoRemessa)
        {
            try
            {
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0001, 001, 0, "0", ' '));                           //001-001
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0002, 001, 0, "1", ' '));                           //002-002
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0003, 007, 0, "REMESSA", ' '));                     //003-009 REM.TST
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0010, 002, 0, "01", ' '));                          //010-011
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0012, 015, 0, "COBRANCA", ' '));                    //012-026
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0027, 004, 0, cedente.ContaBancaria.Agencia, ' ')); //027-030
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0031, 006, 0, cedente.Codigo, ' '));                //031-036 TODO
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0037, 010, 0, string.Empty, ' '));                  //037-046
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0047, 030, 0, cedente.Nome.ToUpper(), ' '));        //047-076
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0077, 003, 0, "104", ' '));                         //077-079
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0080, 015, 0, "C ECON FEDERAL", ' '));              //080-094
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAA___________, 0095, 006, 0, DateTime.Now, ' '));                  //095-100
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0101, 289, 0, "", ' '));                            //101-389
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliDireita______, 0390, 005, 0, numeroArquivoRemessa, '0'));          //390-394
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0395, 006, 0, "000001", ' '));                      //395-400

                reg.CodificarLinha();

                var vLinha = reg.LinhaRegistro;
                var header = Utils.SubstituiCaracteresEspeciais(vLinha);

                return header;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar HEADER do arquivo de remessa do CNAB400.", ex);
            }
        }

        private string GerarDetalheRemessaCnab400(Boleto boleto, int numeroRegistro, TipoArquivo tipoArquivo)
        {
            try
            {
                //Vari�veis Locais a serem Implementadas em n�vel de Config do Boleto...
                boleto.Remessa.CodigoOcorrencia = "01"; //remessa p/ CAIXA ECONOMICA FEDERAL

                base.GerarDetalheRemessa(boleto, numeroRegistro, tipoArquivo);

                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0001, 001, 0, "1", '0'));                                       //001-001

                #region Regra Tipo de Inscri��o Cedente

                var vCpfCnpjEmi = "00";
                if (boleto.Cedente.CpfCnpj.Length.Equals(11))
                    vCpfCnpjEmi = "01"; //Cpf � sempre 11;
                else if (boleto.Cedente.CpfCnpj.Length.Equals(14))
                    vCpfCnpjEmi = "02"; //Cnpj � sempre 14;

                #endregion

                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0002, 002, 0, vCpfCnpjEmi, '0'));                               //002-003
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0004, 014, 0, boleto.Cedente.CpfCnpj, '0'));                    //004-017
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0018, 004, 0, boleto.Cedente.ContaBancaria.Agencia, '0'));      //018-021
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0022, 006, 0, boleto.Cedente.Codigo, ' '));                     //022-027
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0028, 001, 0, boleto.Postagem ? "1" : "2", ' '));               //028-028 '1' Banco emite, '2' Cliente emite.
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0029, 001, 0, '0', ' '));                                       //029-029 �0� =  Postagem pelo Benefici�rio, �1� = Pagador via Correio, �2� = Benefici�rio via Ag�ncia CAIXA, �3� = Pagador via e-mail
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0030, 002, 0, "00", ' '));                                      //030-031
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0032, 025, 0, boleto.NumeroDocumento, '0'));                    //032-056
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0057, 017, 0, boleto.NossoNumero, '0'));                        //057-073
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0074, 002, 0, string.Empty, ' '));                              //074-075
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0076, 001, 0, string.Empty, ' '));                              //076-076
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0077, 030, 0, string.Empty, ' '));                              //077-106
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0107, 002, 0, "01", '0'));                                      //107-108
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0109, 002, 0, boleto.Remessa.CodigoOcorrencia, ' '));           //109-110   //REMESSA
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0111, 010, 0, boleto.NumeroDocumento, '0'));                    //111-120
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAA___________, 0121, 006, 0, boleto.DataVencimento, ' '));                     //121-126                
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0127, 013, 2, boleto.ValorBoleto, '0'));                        //127-139
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0140, 003, 0, "104", '0'));                                     //140-142
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0143, 005, 0, "00000", '0'));                                   //143-147
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliDireita______, 0148, 002, 0, boleto.EspecieDocumento.Codigo, '0'));            //148-149
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0150, 001, 0, boleto.Aceite, ' '));                             //150-150
                reg.CamposEdi.Add(new CampoEdi(Dado.DataDDMMAA___________, 0151, 006, 0, boleto.DataDocumento, ' '));                      //151-156

                #region Instru��es

                var vInstrucao1 = "0";
                var vInstrucao2 = "0";
                var vInstrucao3 = "0";
                var prazoProtestoDevolucao = 0;

                foreach (var instrucao in boleto.Instrucoes)
                {
                    switch ((EnumInstrucoes_Caixa)instrucao.Codigo)
                    {
                        case EnumInstrucoes_Caixa.Protestar:
                            vInstrucao1 = "01";
                            prazoProtestoDevolucao = instrucao.Dias;
                            break;
                        case EnumInstrucoes_Caixa.DevolverAposNDias:
                            vInstrucao1 = "02";
                            prazoProtestoDevolucao = instrucao.Dias;
                            break;
                    }
                }

                #region OLD
                //switch (boleto.Instrucoes.Count)
                //{
                //    case 1:
                //        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                //        vInstrucao2 = "0";
                //        vInstrucao3 = "0";
                //        break;
                //    case 2:
                //        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                //        vInstrucao2 = boleto.Instrucoes[1].Codigo.ToString();
                //        vInstrucao3 = "0";
                //        break;
                //    case 3:
                //        vInstrucao1 = boleto.Instrucoes[0].Codigo.ToString();
                //        vInstrucao2 = boleto.Instrucoes[1].Codigo.ToString();
                //        vInstrucao3 = boleto.Instrucoes[2].Codigo.ToString();
                //        break;
                //}
                #endregion OLD

                #endregion Instru��es

                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0157, 002, 0, vInstrucao1, '0'));                               //157-158
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0159, 002, 0, vInstrucao2, '0'));                               //159-160 Sempre zero.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0161, 013, 2, boleto.JurosMora, '0'));                          //161-173

                #region DataDesconto

                var vDataDesconto = "000000";
                if (!boleto.DataDesconto.Equals(DateTime.MinValue))
                    vDataDesconto = boleto.DataDesconto.ToString("ddMMyy");

                #endregion

                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0174, 006, 0, vDataDesconto, '0'));                             //174-179
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0180, 013, 2, boleto.ValorDesconto, '0'));                      //180-192
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0193, 013, 2, boleto.Iof, '0'));                                //193-205
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0206, 013, 2, boleto.Abatimento, '0'));                         //206-218

                #region Regra Tipo de Inscri��o Sacado

                var vCpfCnpjSac = "99";
                if (boleto.Sacado.CpfCnpj.Length.Equals(11))
                    vCpfCnpjSac = "01"; //Cpf � sempre 11;
                else if (boleto.Sacado.CpfCnpj.Length.Equals(14))
                    vCpfCnpjSac = "02"; //Cnpj � sempre 14;

                #endregion

                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0219, 002, 0, vCpfCnpjSac, '0'));                               //219-220
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0221, 014, 0, boleto.Sacado.CpfCnpj, '0'));                     //221-234
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0235, 040, 0, boleto.Sacado.Nome.ToUpper(), ' '));              //235-274
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0275, 040, 0, boleto.Sacado.Endereco.End.ToUpper(), ' '));      //275-314
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0315, 012, 0, boleto.Sacado.Endereco.Bairro.ToUpper(), ' '));   //315-326
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0327, 008, 0, boleto.Sacado.Endereco.Cep, '0'));                //327-334
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0335, 015, 0, boleto.Sacado.Endereco.Cidade.ToUpper(), ' '));   //335-349
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0350, 002, 0, boleto.Sacado.Endereco.Uf.ToUpper(), ' '));       //350-351

                #region DataMulta

                var vDataMulta = "000000";
                if (!boleto.DataMulta.Equals(DateTime.MinValue))
                    vDataMulta = boleto.DataMulta.ToString("ddMMyy");

                #endregion

                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0352, 006, 0, vDataMulta, '0'));                                //352-357
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0358, 010, 2, boleto.ValorMulta, '0'));                         //358-367
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0368, 022, 0, string.Empty, ' '));                              //368-389
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0390, 002, 0, vInstrucao3, '0'));                               //390-391 Zeros, n�o est� sendo usada.
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0392, 002, 0, prazoProtestoDevolucao, '0'));                    //392-393
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0394, 001, 0, 1, '0'));                                         //394-394
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0'));                            //395-400

                reg.CodificarLinha();

                return Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar DETALHE do arquivo CNAB400.", ex);
            }
        }

        private string GerarTrailerRemessa400(int numeroRegistro, decimal vltitulostotal)
        {
            try
            {
                var reg = new RegistroEdi();
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0001, 001, 0, "9", ' '));            //001-001
                reg.CamposEdi.Add(new CampoEdi(Dado.AlphaAliEsquerda_____, 0002, 393, 0, string.Empty, ' '));   //002-394
                reg.CamposEdi.Add(new CampoEdi(Dado.NumericoSemSeparador_, 0395, 006, 0, numeroRegistro, '0')); //395-400

                reg.CodificarLinha();

                return Utils.SubstituiCaracteresEspeciais(reg.LinhaRegistro);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro durante a gera��o do registro TRAILER do arquivo de REMESSA.", ex);
            }
        }

        public override DetalheRetorno LerDetalheRetornoCNAB400(string registro)
        {
            try
            {
                var reg = new TRegistroEDI_Caixa_Retorno();
                reg.LinhaRegistro = registro;
                reg.DecodificarLinha();

                //Passa para o detalhe as propriedades de reg;
                var detalhe = new DetalheRetorno(registro);

                //reg.CodigoIdentificadorTipoRegistro;
                //reg.TipoInscricaoEmpresa;
                detalhe.NumeroInscricao = reg.NumeroInscricaoEmpresa;
                detalhe.CodigoInscricao = Utils.ToInt32(reg.CodigoEmpresa);
                //reg.Branco1;
                detalhe.NumeroControle = reg.IdentificacaoTituloEmpresa_NossoNumero;

                detalhe.NossoNumeroComDV = reg.IdentificacaoTituloEmpresa_NossoNumero_Modalidde + reg.IdentificacaoTituloCaixa_NossoNumero;
                detalhe.NossoNumero = reg.IdentificacaoTituloEmpresa_NossoNumero_Modalidde + reg.IdentificacaoTituloCaixa_NossoNumero.Substring(0, reg.IdentificacaoTituloCaixa_NossoNumero.Length - 1); //Nosso N�mero sem o DV!
                detalhe.DACNossoNumero = reg.IdentificacaoTituloCaixa_NossoNumero.Substring(reg.IdentificacaoTituloCaixa_NossoNumero.Length - 1);
                //reg.Brancos2;
                detalhe.MotivosRejeicao = reg.CodigoMotivoRejeicao;
                //reg.IdentificacaoOperacao;
                detalhe.Carteira = reg.CodigoCarteira;
                detalhe.CodigoOcorrencia = Utils.ToInt32(reg.CodigoOcorrencia);
                detalhe.DescricaoOcorrencia = Ocorrencia(registro.Substring(108, 2));

                var dataOcorrencia = Utils.ToInt32(reg.DataOcorrencia);
                detalhe.DataOcorrencia = Utils.ToDateTime(dataOcorrencia.ToString("##-##-##"));
                detalhe.NumeroDocumento = reg.NumeroDocumento;
                //reg.Brancos3;
                var dataVencimento = Utils.ToInt32(reg.DataVencimentoTitulo);
                detalhe.DataVencimento = Utils.ToDateTime(dataVencimento.ToString("##-##-##"));
                detalhe.ValorTitulo = (Convert.ToInt64(reg.ValorTitulo) / Decimal100);
                detalhe.CodigoBanco = Utils.ToInt32(reg.CodigoBancoCobrador);
                detalhe.AgenciaCobradora = Utils.ToInt32(reg.CodigoAgenciaCobradora);

                //reg.EspecieTitulo;
                detalhe.ValorDespesa = (Convert.ToUInt64(reg.ValorDespesasCobranca) / Decimal100);
                detalhe.OrigemPagamento = reg.TipoLiquidacao;
                //reg.FormaPagamentoUtilizada;
                //reg.FloatNegociado;
                //reg.DataDebitoTarifaLiquidacao;
                //reg.Brancos4;
                detalhe.IOF = (Convert.ToUInt64(reg.ValorIOF) / Decimal100);
                detalhe.ValorAbatimento = (Convert.ToUInt64(reg.ValorAbatimentoConcedido) / Decimal100);
                detalhe.Descontos = (Convert.ToUInt64(reg.ValorDescontoConcedido) / Decimal100);
                detalhe.ValorPago = (Convert.ToUInt64(reg.ValorPago) / Decimal100);
                detalhe.JurosMora = (Convert.ToUInt64(reg.ValorJuros) / Decimal100);
                detalhe.TarifaCobranca = (Convert.ToUInt64(reg.ValorDespesasCobranca) / Decimal100);
                detalhe.ValorMulta = (Convert.ToUInt64(reg.ValorMulta) / Decimal100);
                //reg.ValorMulta;
                //reg.CodigoMoeda;
                var dataCredito = Utils.ToInt32(reg.DataCreditoConta);
                detalhe.DataCredito = Utils.ToDateTime(dataCredito.ToString("##-##-##"));
                //reg.Brancos5;

                detalhe.NumeroSequencial = Utils.ToInt32(reg.NumeroSequenciaRegistro);
                detalhe.ValorPrincipal = detalhe.ValorPago;

                #region NAO RETORNADOS PELA CAIXA
                detalhe.OutrosCreditos = 0;
                detalhe.ValorOutrasDespesas = 0;
                //
                detalhe.MotivoCodigoOcorrencia = string.Empty;
                detalhe.MotivosRejeicao = string.Empty;
                detalhe.NumeroCartorio = 0;
                detalhe.NumeroProtocolo = string.Empty;
                detalhe.NomeSacado = string.Empty;
                #endregion

                return detalhe;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler detalhe do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

        private string Ocorrencia(string codigo)
        {
            if (!int.TryParse(codigo, out var codigoMovimento))
                return $"Erro ao retornar descri��o para a ocorr�ncia {codigo}";

            var movimento = new CodigoMovimento_Caixa(codigoMovimento);
            return movimento.Descricao;
        }

        #endregion
    }
}