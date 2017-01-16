namespace BoletoNet
{
    public interface IInstrucao
    {
        IBanco Banco { get; set; }
        int Codigo { get; set; }
        string Descricao { get; set; }
        int Dias { get; set; }
        decimal Valor { get; set; }
        EnumTipoValor Tipo { get; set; }

        /// <summary>
        /// Valida os dados referentes � instru��o
        /// </summary>
        void Valida();

        void Carrega(int cod, int dias = 0, decimal valor = 0m, EnumTipoValor tipo = EnumTipoValor.Percentual);
    }
}
