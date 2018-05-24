using System;

namespace BoletoNet
{
    #region Enum

    public enum EnumTipoValor
    {
        Percentual = 1,
        Reais = 2
    }

    #endregion

    public abstract class AbstractInstrucao : IInstrucao
    {
        #region Propriedades

        public virtual IBanco Banco { get; set; }

        public virtual int Codigo { get; set; }

        public virtual string Descricao { get; set; }

        public virtual int Dias { get; set; }

        public virtual decimal Valor { get; set; }

        public virtual EnumTipoValor Tipo { get; set; }

        #endregion

        #region Metódos

        public virtual void Valida()
        {
            if (Codigo == 0 && string.IsNullOrWhiteSpace(Descricao))
                throw new Exception("O código da instrução deve ser diferente de zero quando a descrição estiver vazia.");
        }

        public virtual void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorTotal = 0m, DateTime? data = null)
        {}

        #endregion
    }
}