using System;

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
        /// Valida os dados referentes à instrução
        /// </summary>
        void Valida();

        void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorTotal = 0m, DateTime? data = null);
    }
}