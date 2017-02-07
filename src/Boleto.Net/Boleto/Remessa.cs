namespace BoletoNet
{
    /// <summary>
    /// Contém informações que são pertinentes a um boleto, mas para geração da Remessa. Não são necessárias para impressão do Boleto
    /// </summary>
    public class Remessa
    {
        public enum TipoAmbiemte
        {
            Homologacao,
            Producao
        }

        #region Propriedades

        /// <summary>
        /// Variável que define se a Remessa é para Testes ou Produção
        /// </summary>
        public TipoAmbiemte Ambiente { get; set; }

        /// <summary>
        /// Tipo Documento Utilizado na geração da remessa. |Identificado no Banrisul by sidneiklein|
        /// Tipo Cobranca Utilizado na geração da remessa.  |Identificado no Sicredi by sidneiklein|
        /// </summary>
        public string TipoDocumento { get; set; }

        /// <summary>
        /// Código de Ocorrência Utilizado na geração da Remessa.
        /// Banrisul        como "CODIGO OCORRENCIA"
        /// Banco do Brasil como "COMANDO"           
        /// Santander       como "CÓDIGO DE MOVIMENTO REMESSA"
        /// </summary>
        public string CodigoOcorrencia { get; set; }

        /// <summary>
        /// Numero do lote de remessa
        /// </summary>
        public int NumeroLote { get; set; }

        #endregion

    }
}
