namespace BoletoNet
{
    public interface ICodigoRejeicao
    {
        IBanco Banco { get; }
        int Codigo { get; set;}
        string Descricao { get; }
    }
}
