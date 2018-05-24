namespace BoletoNet
{
    using System;

    #region EnumInstrucoes_Sicoob enum

    public enum EnumInstrucoes_Sicoob
    {
        AusenciaDeInstrucoes = 0,

        CobrarJuros = 1,

        Protestar3DiasUteis = 3,

        Protestar4DiasUteis = 4,

        Protestar5DiasUteis = 5,

        NaoProtestar = 7,

        Protestar10DiasUteis = 10,

        Protestar15DiasUteis = 15,

        Protestar20DiasUteis = 20,

        ConcederDescontoApenasAteDataEstipulada = 22,

        DevolverApos15DiasVencido = 42,

        DevolverApos30DiasVencido = 43
    }

    #endregion
    
    public class Instrucao_Sicoob : AbstractInstrucao, IInstrucao
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

		public Instrucao_Sicoob(int codigo)
		{
			carregar(codigo, 0, 0);
		}

		public Instrucao_Sicoob(int codigo, int nrDias)
		{
			carregar(codigo, nrDias, (double)0.0);
		}

		public Instrucao_Sicoob(int codigo, double percentualMultaDia)
		{
			carregar(codigo, 0, percentualMultaDia);
		}

		public Instrucao_Sicoob(int codigo, int nrDias, double percentualMultaDia)
		{
			carregar(codigo, nrDias, percentualMultaDia);
		}

		#endregion

		#region Metodos Privados

        private void carregar(int idInstrucao, int nrDias, double percentualMultaDia)
        {
            try
            {
                Banco = new Banco_Banrisul();
                Valida();

                switch ((EnumInstrucoes_Sicoob)idInstrucao)
                {
                    case EnumInstrucoes_Sicoob.AusenciaDeInstrucoes:
                        break;
                    case EnumInstrucoes_Sicoob.CobrarJuros:
                        Codigo = (int)EnumInstrucoes_Sicoob.CobrarJuros;
                        Descricao = "Cobrar Juros";
                        break;
                    case EnumInstrucoes_Sicoob.Protestar3DiasUteis:
                        Codigo = (int)EnumInstrucoes_Sicoob.Protestar3DiasUteis;
                        Descricao = "Protestar 3 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.Protestar4DiasUteis:
                        Codigo = (int)EnumInstrucoes_Sicoob.Protestar4DiasUteis;
                        Descricao = "Protestar 4 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.Protestar5DiasUteis:
                        Codigo = (int)EnumInstrucoes_Sicoob.Protestar5DiasUteis;
                        Descricao = "Protestar 5 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.NaoProtestar:
                        Codigo = (int)EnumInstrucoes_Sicoob.NaoProtestar;
                        Descricao = "Não protestar";
                        break;
                    case EnumInstrucoes_Sicoob.Protestar10DiasUteis:
                        Codigo = (int)EnumInstrucoes_Sicoob.Protestar10DiasUteis;
                        Descricao = "Protestar 10 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.Protestar15DiasUteis:
                        Codigo = (int)EnumInstrucoes_Sicoob.Protestar15DiasUteis;
                        Descricao = "Protestar 15 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.Protestar20DiasUteis:
                        Codigo = (int)EnumInstrucoes_Sicoob.Protestar20DiasUteis;
                        Descricao = "Protestar 20 dias úteis após vencimento";
                        break;
                    case EnumInstrucoes_Sicoob.ConcederDescontoApenasAteDataEstipulada:
                        Codigo = (int)EnumInstrucoes_Sicoob.ConcederDescontoApenasAteDataEstipulada;
                        Descricao = "Conceder desconto só até a data estipulada";
                        break;
                    case EnumInstrucoes_Sicoob.DevolverApos15DiasVencido:
                        Codigo = (int)EnumInstrucoes_Sicoob.DevolverApos15DiasVencido;
                        Descricao = "Devolver após 15 dias vencido";
                        break;
                    case EnumInstrucoes_Sicoob.DevolverApos30DiasVencido:
                        Codigo = (int)EnumInstrucoes_Sicoob.DevolverApos30DiasVencido;
                        Descricao = "Devolver após 30 dias vencido";
                        break;
                    default:
                        Codigo = 0;
                        Descricao = " (Selecione) ";
                        break;

                }

				Dias = nrDias;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro ao carregar objeto", ex);
			}
		}

		public override void Valida()
		{
	        base.Valida();
		}

		#endregion
	}
}