namespace BoletoNet
{
    public class ArquivoRemessa : AbstractArquivoRemessa, IArquivoRemessa
    {
        public ArquivoRemessa(TipoArquivo tipoarquivo)
            : base(tipoarquivo)
        {}
    }
}
