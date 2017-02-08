using System;

namespace BoletoNet
{
    //C�digos de rejeicoes de 1 a 64 associados ao c�digo de movimento 3, 26 e 30
    #region Enumerado

    public enum EnumCodigoRejeicao_BancoBrasil
    {
        CodigoBancoInvalido = 1,
        CodigoRegistroDetalheInvalido = 2,
        CodigoSegmentoInvalido = 3,
        CodigoMovimentoNaoPermitidoParaCarteira = 4,
        CodigoMovimentoInvalido = 5,
        TipoNumeroInscricaoCedenteInvalidos = 6,
        AgenciaContaDVInvalido = 7,
        NossoNumeroInvalido = 8,
        NossoNumeroDuplicado = 9,
        CarteiraInvalida = 10,
        FormaCadastramentoTituloInvalido = 11,
        TipoDocumentoInvalido = 12,
        IdentificacaoEmissaoBloquetoInvalida = 13,
        IdentificacaoDistribuicaoBloquetoInvalida = 14,
        CaracteristicasCobrancaIncompativeis = 15,
        DataVencimentoInvalida = 16,
        DataVencimentoAnteriorDataEmissao = 17,
        VencimentoForadoPrazodeOperacao = 18,
        TituloCargoBancosCorrespondentesVencimentoInferiorXXDias = 19,
        ValorTituloInvalido = 20,
        EspecieTituloInvalida = 21,
        EspecieNaoPermitidaParaCarteira = 22,
        AceiteInvalido = 23,
        DataEmissaoInvalida = 24,
        DataEmissaoPosteriorData = 25,
        CodigoJurosMoraInvalido = 26,
        ValorJurosMoraInvalido = 27,
        CodigoDescontoInvalido = 28,
        ValorDescontoMaiorIgualValorTitulo = 29,
        DescontoConcederNaoConfere = 30,
        ConcessaoDescontoJaExiste = 31,
        ValorIOFInvalido = 32,
        ValorAbatimentoInvalido = 33,
        ValorAbatimentoMaiorIgualValorTitulo = 34,
        AbatimentoConcederNaoConfere = 35,
        ConcessaoAbatimentoJaExiste = 36,
        CodigoProtestoInvalido = 37,
        PrazoProtestoInvalido = 38,
        PedidoProtestoNaoPermitidoParaTitulo = 39,
        TituloComOrdemProtestoEmitida = 40,
        PedidoCancelamentoParaTitulosSemInstrucaoProtesto = 41,
        CodigoParaBaixaDevolucaoInvalido = 42,
        PrazoParaBaixaDevolucaoInvalido = 43,
        CodigoMoedaInvalido = 44,
        NomeSacadoNaoInformado = 45,
        TipoNumeroInscricaoSacadoInvalidos = 46,
        EnderecoSacadoNaoInformado = 47,
        CEPInvalido = 48,
        CEPSemPracaCobranca = 49,
        CEPReferenteBancoCorrespondente = 50,
        CEPIncompativelComUnidadeFederacao = 51,
        UnidadeFederacaoInvalida = 52,
        TipoNumeroInscricaoSacadorAvalistaInvalido = 53,
        SacadorAvalistaNaoInformado = 54,
        NossoNumeroBancoCorrespondenteNaoInformado = 55,
        CodigoBancoCorrespondenteNaoInformado = 56,
        CodigoMultaInvalido = 57,
        DataMultaInvalida = 58,
        ValorPercentualMultaInvalido = 59,
        MovimentoParaTituloNaoCadastrado = 60,
        AlteracaoAgenciaCobradoraInvalida = 61,
        TipoImpressaoInvalido = 62,
        EntradaParaTituloJaCadastrado = 63,
        NumeroLinhaInvalido = 64,
        CodigoBancoDebitoInvalido = 65,
        AgenciaContaDVParaDebitoInvalido = 66,
        DadosParaDebitoIncompativel = 67,
        ArquivoEmDuplicidade = 88,
        ContratoInexistente = 99,
    }

    #endregion 

    public class CodigoRejeicao_BancoBrasil: AbstractCodigoRejeicao, ICodigoRejeicao
    {
        #region Construtores 

		public CodigoRejeicao_BancoBrasil()
		{}

        public CodigoRejeicao_BancoBrasil(int codigo)
        {
            try
            {
                carregar(codigo);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

		#endregion 

        #region Metodos Privados

        private void carregar(int codigo)
        {
            try
            {
                Banco = new Banco_Brasil();

                switch ((EnumCodigoRejeicao_BancoBrasil)codigo)
                {
                    case  EnumCodigoRejeicao_BancoBrasil.CodigoBancoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoBancoInvalido;
                        Descricao = "C�digo do banco inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoRegistroDetalheInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoRegistroDetalheInvalido;
                        Descricao = "C�digo do registro detalhe inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoSegmentoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoSegmentoInvalido;
                        Descricao = "C�digo do segmento inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoMovimentoNaoPermitidoParaCarteira:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMovimentoNaoPermitidoParaCarteira;
                        Descricao = "C�digo do movimento n�o permitido para a carteira";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoMovimentoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMovimentoInvalido;
                        Descricao = "C�digo do movimento inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoCedenteInvalidos:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoCedenteInvalidos;
                        Descricao = "Tipo/N�mero de inscri��o do cendente inv�lidos";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.AgenciaContaDVInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AgenciaContaDVInvalido;
                        Descricao = "Ag�ncia/Conta/DV inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.NossoNumeroInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NossoNumeroInvalido;
                        Descricao = "Nosso n�mero inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.NossoNumeroDuplicado:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NossoNumeroDuplicado;
                        Descricao = "Nosso n�mero duplicado";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CarteiraInvalida:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CarteiraInvalida;
                        Descricao = "Carteira inv�lida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.FormaCadastramentoTituloInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.FormaCadastramentoTituloInvalido;
                        Descricao = "Forma de cadastramento do t�tulo inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.TipoDocumentoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoDocumentoInvalido;
                        Descricao = "Tipo de documento inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.IdentificacaoEmissaoBloquetoInvalida:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.IdentificacaoEmissaoBloquetoInvalida;
                        Descricao = "Identifica��o da emiss�o do bloqueto inv�lida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.IdentificacaoDistribuicaoBloquetoInvalida:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.IdentificacaoDistribuicaoBloquetoInvalida;
                        Descricao = "Identifica��o da distribui��o do bloqueto inv�lida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CaracteristicasCobrancaIncompativeis:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CaracteristicasCobrancaIncompativeis;
                        Descricao = "Caracter�sticas da cobran�a incompat�veis";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.DataVencimentoInvalida:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataVencimentoInvalida;
                        Descricao = "Data de vencimento inv�lida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.DataVencimentoAnteriorDataEmissao:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataVencimentoAnteriorDataEmissao;
                        Descricao = "Data de vencimento anterior a data de emiss�o";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.VencimentoForadoPrazodeOperacao:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.VencimentoForadoPrazodeOperacao;
                        Descricao = "Vencimento fora do prazo de emiss�o";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.TituloCargoBancosCorrespondentesVencimentoInferiorXXDias:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TituloCargoBancosCorrespondentesVencimentoInferiorXXDias;
                        Descricao = "Titulo a cargo de bancos correspondentes com vencimento inferior a XX dias";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ValorTituloInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorTituloInvalido;
                        Descricao = "Valor do t�tulo inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.EspecieTituloInvalida:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EspecieTituloInvalida;
                        Descricao = "Esp�cie do t�tulo inv�lida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.EspecieNaoPermitidaParaCarteira:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EspecieNaoPermitidaParaCarteira;
                        Descricao = "Esp�cie n�o permitida para a carteira";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.AceiteInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AceiteInvalido;
                        Descricao = "Aceite inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.DataEmissaoInvalida:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataEmissaoInvalida;
                        Descricao = "Data de emiss�o inv�lida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.DataEmissaoPosteriorData:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataEmissaoPosteriorData;
                        Descricao = "Data de emiss�o posterior a data";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoJurosMoraInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoJurosMoraInvalido;
                        Descricao = "C�digo de juros de mora inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ValorJurosMoraInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorJurosMoraInvalido;
                        Descricao = "Valor/Taxa de juros de mora inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoDescontoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoDescontoInvalido;
                        Descricao = "C�digo do desconto inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ValorDescontoMaiorIgualValorTitulo:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorDescontoMaiorIgualValorTitulo;
                        Descricao = "Valor do desconto maior ou igual ao valor do t�tulo";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.DescontoConcederNaoConfere:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DescontoConcederNaoConfere;
                        Descricao = "Desconto a conceder n�o confere";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ConcessaoDescontoJaExiste:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ConcessaoDescontoJaExiste;
                        Descricao = "Concess�o de desconto - j� existe desconto anterior";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ValorIOFInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorIOFInvalido;
                        Descricao = "Valor do IOF inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ValorAbatimentoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorAbatimentoInvalido;
                        Descricao = "Valor do abatimento inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ValorAbatimentoMaiorIgualValorTitulo:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorAbatimentoMaiorIgualValorTitulo;
                        Descricao = "Valor do abatimento maior ou igual ao valor do t�tulo";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.AbatimentoConcederNaoConfere:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AbatimentoConcederNaoConfere;
                        Descricao = "Abatimento a conceder n�o confere";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.ConcessaoAbatimentoJaExiste:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ConcessaoAbatimentoJaExiste;
                        Descricao = "Concess�o de abatimento - j� existe abatimendo anterior";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoProtestoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoProtestoInvalido;
                        Descricao = "C�digo para protesto inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.PrazoProtestoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PrazoProtestoInvalido;
                        Descricao = "Prazo para protesto inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.PedidoProtestoNaoPermitidoParaTitulo:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PedidoProtestoNaoPermitidoParaTitulo;
                        Descricao = "Pedido de protesto n�o permitido para o t�tulo";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.TituloComOrdemProtestoEmitida:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TituloComOrdemProtestoEmitida;
                        Descricao = "T�tulo com ordem de protesto emitida";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.PedidoCancelamentoParaTitulosSemInstrucaoProtesto:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PedidoCancelamentoParaTitulosSemInstrucaoProtesto;
                        Descricao = "Pedido de cancelamento para t�tulos sem instru��o de protesto";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoParaBaixaDevolucaoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoParaBaixaDevolucaoInvalido;
                        Descricao = "C�digo para baixa/devolu��o inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.PrazoParaBaixaDevolucaoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PrazoParaBaixaDevolucaoInvalido;
                        Descricao = "Prazo para baixa/devolu��o inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CodigoMoedaInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMoedaInvalido;
                        Descricao = "C�digo da moeda inv�lido";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.NomeSacadoNaoInformado:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NomeSacadoNaoInformado;
                        Descricao = "Nome do sacado n�o informado";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoSacadoInvalidos:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoSacadoInvalidos;
                        Descricao = "Tipo/N�mero de inscri��o do sacado inv�lidos";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.EnderecoSacadoNaoInformado:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EnderecoSacadoNaoInformado;
                        Descricao = "Endere�o do sacado n�o informado";
                        break;
                    case EnumCodigoRejeicao_BancoBrasil.CEPInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPInvalido;
                        Descricao = "CEP inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.CEPSemPracaCobranca:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPSemPracaCobranca;
                        Descricao = "CEP sem pra�a de cobran�a";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.CEPReferenteBancoCorrespondente:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPReferenteBancoCorrespondente;
                        Descricao = "CEP referente a um banco correspondente";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.CEPIncompativelComUnidadeFederacao:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPIncompativelComUnidadeFederacao;
                        Descricao = "CEP incompat�vel com a UF";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.UnidadeFederacaoInvalida:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.UnidadeFederacaoInvalida;
                        Descricao = "UF inv�lida";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoSacadorAvalistaInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoSacadorAvalistaInvalido;
                        Descricao = "Tipo/N�mero de inscri��o do sacador/avalista inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.SacadorAvalistaNaoInformado:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.SacadorAvalistaNaoInformado;
                        Descricao = "Sacador/Avalista n�o informado";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.NossoNumeroBancoCorrespondenteNaoInformado:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NossoNumeroBancoCorrespondenteNaoInformado;
                        Descricao = "Nosso n�mero no banco correspondente n�o informado";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.CodigoBancoCorrespondenteNaoInformado:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoBancoCorrespondenteNaoInformado;
                        Descricao = "C�digo do banco correspondente n�o informado";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.CodigoMultaInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMultaInvalido;
                        Descricao = "C�digo da multa inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.DataMultaInvalida:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataMultaInvalida;
                        Descricao = "Data da multa inv�lida";
                        break;                        
                        case EnumCodigoRejeicao_BancoBrasil.ValorPercentualMultaInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorPercentualMultaInvalido;
                        Descricao = "Valor/Percentual da multa inv�lida";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.MovimentoParaTituloNaoCadastrado:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.MovimentoParaTituloNaoCadastrado;
                        Descricao = "Movimento para t�tulo n�o cadastrado";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.AlteracaoAgenciaCobradoraInvalida:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AlteracaoAgenciaCobradoraInvalida;
                        Descricao = "Altera��o da ag�ncia cobradora/dv inv�lida";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.TipoImpressaoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoImpressaoInvalido;
                        Descricao = "Tipo de impress�o inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.EntradaParaTituloJaCadastrado:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EntradaParaTituloJaCadastrado;
                        Descricao = "Entrada para t�tulo j� cadastrado";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.NumeroLinhaInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NumeroLinhaInvalido;
                        Descricao = "N�mero da linha inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.CodigoBancoDebitoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoBancoDebitoInvalido;
                        Descricao = "C�digo do banco para d�bito inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.AgenciaContaDVParaDebitoInvalido:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AgenciaContaDVParaDebitoInvalido;
                        Descricao = "Ag�ncia/Conta/DV para d�bito inv�lido";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.DadosParaDebitoIncompativel:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DadosParaDebitoIncompativel;
                        Descricao = "Dados para d�bito incompat�vel com a identifica��o da emiss�o do boleto";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.ArquivoEmDuplicidade:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ArquivoEmDuplicidade;
                        Descricao = "Arquivo em duplicidade";
                        break;
                        case EnumCodigoRejeicao_BancoBrasil.ContratoInexistente:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ContratoInexistente;
                        Descricao = "Contrato inexistente";
                        break;
                    default:
                        Codigo = 0;
                        Descricao = "( Selecione )";
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        private void Ler(int codigo)
        {
            try
            {
                switch (codigo)
                {
                    case 1:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoBancoInvalido;
                        Descricao = "C�digo do banco inv�lido";
                        break;
                    case 2:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoRegistroDetalheInvalido;
                        Descricao = "C�digo do registro detalhe inv�lido";
                        break;
                    case 3:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoSegmentoInvalido;
                        Descricao = "C�digo do segmento inv�lido";
                        break;
                    case 4:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMovimentoNaoPermitidoParaCarteira;
                        Descricao = "C�digo do movimento n�o permitido para a carteira";
                        break;
                    case 5:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMovimentoInvalido;
                        Descricao = "C�digo do movimento inv�lido";
                        break;
                    case 6:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoCedenteInvalidos;
                        Descricao = "Tipo/N�mero de inscri��o do cendente inv�lidos";
                        break;
                    case 7:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AgenciaContaDVInvalido;
                        Descricao = "Ag�ncia/Conta/DV inv�lido";
                        break;
                    case 8:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NossoNumeroInvalido;
                        Descricao = "Nosso n�mero inv�lido";
                        break;
                    case 9:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NossoNumeroDuplicado;
                        Descricao = "Nosso n�mero duplicado";
                        break;
                    case 10:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CarteiraInvalida;
                        Descricao = "Carteira inv�lida";
                        break;
                    case 11:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.FormaCadastramentoTituloInvalido;
                        Descricao = "Forma de cadastramento do t�tulo inv�lido";
                        break;
                    case 12:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoDocumentoInvalido;
                        Descricao = "Tipo de documento inv�lido";
                        break;
                    case 13:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.IdentificacaoEmissaoBloquetoInvalida;
                        Descricao = "Identifica��o da emiss�o do bloqueto inv�lida";
                        break;
                    case 14:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.IdentificacaoDistribuicaoBloquetoInvalida;
                        Descricao = "Identifica��o da distribui��o do bloqueto inv�lida";
                        break;
                    case 15:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CaracteristicasCobrancaIncompativeis;
                        Descricao = "Caracter�sticas da cobran�a incompat�veis";
                        break;
                    case 16:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataVencimentoInvalida;
                        Descricao = "Data de vencimento inv�lida";
                        break;
                    case 17:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataVencimentoAnteriorDataEmissao;
                        Descricao = "Data de vencimento anterior a data de emiss�o";
                        break;
                    case 18:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.VencimentoForadoPrazodeOperacao;
                        Descricao = "Vencimento fora do prazo de emiss�o";
                        break;
                    case 19:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TituloCargoBancosCorrespondentesVencimentoInferiorXXDias;
                        Descricao = "Titulo a cargo de bancos correspondentes com vencimento inferior a XX dias";
                        break;
                    case 20:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorTituloInvalido;
                        Descricao = "Valor do t�tulo inv�lido";
                        break;
                    case 21:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EspecieTituloInvalida;
                        Descricao = "Esp�cie do t�tulo inv�lida";
                        break;
                    case 22:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EspecieNaoPermitidaParaCarteira;
                        Descricao = "Esp�cie n�o permitida para a carteira";
                        break;
                    case 23:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AceiteInvalido;
                        Descricao = "Aceite inv�lido";
                        break;
                    case 24:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataEmissaoInvalida;
                        Descricao = "Data de emiss�o inv�lida";
                        break;
                    case 25:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataEmissaoPosteriorData;
                        Descricao = "Data de emiss�o posterior a data";
                        break;
                    case 26:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoJurosMoraInvalido;
                        Descricao = "C�digo de juros de mora inv�lido";
                        break;
                    case 27:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorJurosMoraInvalido;
                        Descricao = "Valor/Taxa de juros de mora inv�lido";
                        break;
                    case 28:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoDescontoInvalido;
                        Descricao = "C�digo do desconto inv�lido";
                        break;
                    case 29:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorDescontoMaiorIgualValorTitulo;
                        Descricao = "Valor do desconto maior ou igual ao valor do t�tulo";
                        break;
                    case 30:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DescontoConcederNaoConfere;
                        Descricao = "Desconto a conceder n�o confere";
                        break;
                    case 31:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ConcessaoDescontoJaExiste;
                        Descricao = "Concess�o de desconto - j� existe desconto anterior";
                        break;
                    case 32:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorIOFInvalido;
                        Descricao = "Valor do IOF inv�lido";
                        break;
                    case 33:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorAbatimentoInvalido;
                        Descricao = "Valor do abatimento inv�lido";
                        break;
                    case 34:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorAbatimentoMaiorIgualValorTitulo;
                        Descricao = "Valor do abatimento maior ou igual ao valor do t�tulo";
                        break;
                    case 35:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AbatimentoConcederNaoConfere;
                        Descricao = "Abatimento a conceder n�o confere";
                        break;
                    case 36:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ConcessaoAbatimentoJaExiste;
                        Descricao = "Concess�o de abatimento - j� existe abatimendo anterior";
                        break;
                    case 37:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoProtestoInvalido;
                        Descricao = "C�digo para protesto inv�lido";
                        break;
                    case 38:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PrazoProtestoInvalido;
                        Descricao = "Prazo para protesto inv�lido";
                        break;
                    case 39:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PedidoProtestoNaoPermitidoParaTitulo;
                        Descricao = "Pedido de protesto n�o permitido para o t�tulo";
                        break;
                    case 40:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TituloComOrdemProtestoEmitida;
                        Descricao = "T�tulo com ordem de protesto emitida";
                        break;
                    case 41:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PedidoCancelamentoParaTitulosSemInstrucaoProtesto;
                        Descricao = "Pedido de cancelamento para t�tulos sem instru��o de protesto";
                        break;
                    case 42:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoParaBaixaDevolucaoInvalido;
                        Descricao = "C�digo para baixa/devolu��o inv�lido";
                        break;
                    case 43:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.PrazoParaBaixaDevolucaoInvalido;
                        Descricao = "Prazo para baixa/devolu��o inv�lido";
                        break;
                    case 44:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMoedaInvalido;
                        Descricao = "C�digo da moeda inv�lido";
                        break;
                    case 45:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NomeSacadoNaoInformado;
                        Descricao = "Nome do sacado n�o informado";
                        break;
                    case 46:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AbatimentoConcederNaoConfere;
                        Descricao = "Tipo/N�mero de inscri��o do sacado inv�lidos";
                        break;
                    case 47:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EnderecoSacadoNaoInformado;
                        Descricao = "Endere�o do sacado n�o informado";
                        break;
                    case 48:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPInvalido;
                        Descricao = "CEP inv�lido";
                        break;
                        case 49:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPSemPracaCobranca;
                        Descricao = "CEP sem pra�a de cobran�a";
                        break;
                        case 50:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPReferenteBancoCorrespondente;
                        Descricao = "CEP referente a um banco correspondente";
                        break;
                        case 51:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CEPIncompativelComUnidadeFederacao;
                        Descricao = "CEP incompat�vel com a UF";
                        break;
                        case 52:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.UnidadeFederacaoInvalida;
                        Descricao = "UF inv�lida";
                        break;
                        case 53:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoNumeroInscricaoSacadorAvalistaInvalido;
                        Descricao = "Tipo/N�mero de inscri��o do sacador/avalista inv�lido";
                        break;
                        case 54:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.SacadorAvalistaNaoInformado;
                        Descricao = "Sacador/Avalista n�o informado";
                        break;
                        case 55:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NossoNumeroBancoCorrespondenteNaoInformado;
                        Descricao = "Nosso n�mero no banco correspondente n�o informado";
                        break;
                        case 56:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoBancoCorrespondenteNaoInformado;
                        Descricao = "C�digo do banco correspondente n�o informado";
                        break;
                        case 57:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoMultaInvalido;
                        Descricao = "C�digo da multa inv�lido";
                        break;
                        case 58:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DataMultaInvalida;
                        Descricao = "Data da multa inv�lida";
                        break;                        
                        case 59:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ValorPercentualMultaInvalido;
                        Descricao = "Valor/Percentual da multa inv�lida";
                        break;
                        case 60:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.MovimentoParaTituloNaoCadastrado;
                        Descricao = "Movimento para t�tulo n�o cadastrado";
                        break;
                        case 61:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AlteracaoAgenciaCobradoraInvalida;
                        Descricao = "Altera��o da ag�ncia cobradora/dv inv�lida";
                        break;
                        case 62:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.TipoImpressaoInvalido;
                        Descricao = "Tipo de impress�o inv�lido";
                        break;
                        case 63:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.EntradaParaTituloJaCadastrado;
                        Descricao = "Entrada para t�tulo j� cadastrado";
                        break;
                        case 64:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.NumeroLinhaInvalido;
                        Descricao = "N�mero da linha inv�lido";
                        break;
                        case 65:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.CodigoBancoDebitoInvalido;
                        Descricao = "C�digo do banco para d�bito inv�lido";
                        break;
                        case 66:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.AgenciaContaDVParaDebitoInvalido;
                        Descricao = "Ag�ncia/Conta/DV para d�bito inv�lido";
                        break;
                        case 67:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.DadosParaDebitoIncompativel;
                        Descricao = "Dados para d�bito incompat�vel com a identifica��o da emiss�o do boleto";
                        break;
                        case 88:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ArquivoEmDuplicidade;
                        Descricao = "Arquivo em duplicidade";
                        break;
                        case 99:
                        Codigo = (int)EnumCodigoRejeicao_BancoBrasil.ContratoInexistente;
                        Descricao = "Contrato inexistente";
                        break;
                    default:
                        Codigo = 0;
                        Descricao = "( Selecione )";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        #endregion
    }
}
