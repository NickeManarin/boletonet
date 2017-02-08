namespace BoletoNet
{
    public interface ICodigoTarifas
    {
        IBanco Banco { get; }
        int Codigo { get; set;}
        string Descricao { get; }
    }
}
