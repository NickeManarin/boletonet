namespace BoletoNet
{
    using System;

    public enum EnumInstrucoes_Sicoob
    {
        AusenciaDeInstrucoes = 0,
        CobrarJuros = 1,
        NaoProtestar = 7,
        ConcederDescontoApenasAteDataEstipulada = 22,
        DevolverApos15DiasVencido = 42,
        DevolverApos30DiasVencido = 43,
        Protestar = 99,

        Protestar3DiasUteis = 3,
        Protestar4DiasUteis = 4,
        Protestar5DiasUteis = 5,
        Protestar10DiasUteis = 10,
        Protestar15DiasUteis = 15,
        Protestar20DiasUteis = 20,
    }
    
    public sealed class Instrucao_Sicoob : AbstractInstrucao
	{
		#region Construtores

		public Instrucao_Sicoob()
		{
			try
			{
				Banco = new Banco(756);
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao carregar objeto", ex);
			}
		}

	    public Instrucao_Sicoob(int cod, string descricao = null, int dias = 0, decimal valor = 0m, decimal valorBoleto = 0m)
	    {
	        Carrega(cod, descricao, dias, valor, valorBoleto);
	    }

        #endregion

        #region Metodos Privados

	    public override void Carrega(int cod, string descricao = null, int dias = 0, decimal valor = 0, decimal valorBoleto = 0, DateTime? data = null)
	    {
	        Banco = new Banco_Sicoob();

	        Codigo = cod;
	        Descricao = descricao;
	        Dias = dias;
	        Valor = valor;
	        Tipo = EnumTipoValor.Percentual;

	        Valida();

	        switch ((EnumInstrucoes_Sicoob) cod)
	        {
	            case EnumInstrucoes_Sicoob.CobrarJuros:
	                Codigo = (int)EnumInstrucoes_Sicoob.CobrarJuros;
	                Descricao = "Cobrar Juros.";
	                break;
	            case EnumInstrucoes_Sicoob.NaoProtestar:
	                Codigo = (int)EnumInstrucoes_Sicoob.NaoProtestar;
	                Descricao = "Não protestar.";
	                break;
	            case EnumInstrucoes_Sicoob.DevolverApos15DiasVencido:
	                Codigo = (int)EnumInstrucoes_Sicoob.DevolverApos15DiasVencido;
	                Descricao = "Devolver após 15 dias vencido.";
	                break;
	            case EnumInstrucoes_Sicoob.DevolverApos30DiasVencido:
	                Codigo = (int)EnumInstrucoes_Sicoob.DevolverApos30DiasVencido;
	                Descricao = "Devolver após 30 dias vencido.";
	                break;
	            case EnumInstrucoes_Sicoob.ConcederDescontoApenasAteDataEstipulada:
	                Codigo = (int)EnumInstrucoes_Sicoob.ConcederDescontoApenasAteDataEstipulada;
	                Descricao = "Conceder desconto só até a data estipulada.";
	                break;
	            case EnumInstrucoes_Sicoob.Protestar:
	                Codigo = (int)EnumInstrucoes_Sicoob.Protestar;
	                Descricao = $"Protestar em {dias} dias úteis após o vencimento.";
	                break;
            }
        }

		#endregion
	}
}