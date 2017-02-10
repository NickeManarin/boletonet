namespace BoletoNet
{
    public interface IEspecieDocumento
    {
        IBanco Banco { get; set; }
        string Codigo { get; set;}
        string Sigla { get; set; }
        string Especie { get; set;}
    }
}