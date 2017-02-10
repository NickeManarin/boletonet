namespace BoletoNet
{
    public abstract class AbstractEspecieDocumento : IEspecieDocumento
    {
        #region Propriedades

        public virtual IBanco Banco { get; set; }

        public virtual string Codigo { get; set; }

        public virtual string Sigla { get; set; }

        public virtual string Especie { get; set; }

        #endregion
    }
}