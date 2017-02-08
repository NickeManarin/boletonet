namespace BoletoNet
{
    public interface ICodigoMovimento
    {
        IBanco Banco { get; }
        int Codigo { get; set;}
        string Descricao { get; }
    }
}
