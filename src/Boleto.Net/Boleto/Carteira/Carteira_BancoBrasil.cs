using System;

namespace BoletoNet
{
    #region Enumerado

    public enum EnumCarteiras_BancoBrasil
    {
        CobrancaSimples = 1,
        CobrancaVinculada = 2,
        CobrancaCaucionada = 3,
        CobrancaDescontada = 4,
        CobrançaDiretaEspecialCarteira17 = 7,
    }

    #endregion 

    public class Carteira_BancoBrasil: AbstractCarteira, ICarteira
    {
        #region Construtores 

		public Carteira_BancoBrasil()
		{
			try
			{
                Banco = new Banco(1);
			}
			catch (Exception ex)
			{
                throw new Exception("Erro ao carregar objeto", ex);
			}
		}

        public Carteira_BancoBrasil(int carteira)
        {
            try
            {
                carregar(carteira);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

		#endregion 

        #region Metodos Privados

        private void carregar(int carteira)
        {
            try
            {
                Banco = new Banco_Brasil();

                switch ((EnumCarteiras_BancoBrasil)carteira)
                {
                    case EnumCarteiras_BancoBrasil.CobrancaSimples:
                        NumeroCarteira = (int)EnumCarteiras_BancoBrasil.CobrancaSimples;
                        Codigo = "S";
                        Tipo = "S";
                        Descricao = "Cobrança simples";
                        break;
                    case EnumCarteiras_BancoBrasil.CobrancaVinculada:
                        NumeroCarteira = (int)EnumCarteiras_BancoBrasil.CobrancaVinculada;
                        Codigo = "S";
                        Tipo = "S";
                        Descricao = "Cobrança vincluada";
                        break;
                    case EnumCarteiras_BancoBrasil.CobrancaCaucionada:
                        NumeroCarteira = (int)EnumCarteiras_BancoBrasil.CobrancaCaucionada;
                        Codigo = "S";
                        Tipo = "S";
                        Descricao = "Cobrança caucionada";
                        break;
                    case EnumCarteiras_BancoBrasil.CobrancaDescontada:
                        NumeroCarteira = (int)EnumCarteiras_BancoBrasil.CobrancaDescontada;
                        Codigo = "S";
                        Tipo = "S";
                        Descricao = "Cobrança descontada";
                        break;
                    case EnumCarteiras_BancoBrasil.CobrançaDiretaEspecialCarteira17:
                        NumeroCarteira = (int)EnumCarteiras_BancoBrasil.CobrançaDiretaEspecialCarteira17;
                        Codigo = "S";
                        Tipo = "S";
                        Descricao = "Cobrança direta especial - Carteira 17";
                        break;                    
                    default:
                        NumeroCarteira = 0;
                        Codigo = " ";
                        Tipo = " ";
                        Descricao = "( Selecione )";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar objeto", ex);
            }
        }

        public static Carteiras CarregaTodas()
        {
            try
            {
                Carteiras alCarteiras = new Carteiras();

                Carteira_BancoBrasil obj;

                obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrancaSimples);
                alCarteiras.Add(obj);

                obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrancaVinculada);
                alCarteiras.Add(obj);

                obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrancaCaucionada);
                alCarteiras.Add(obj);

                obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrancaDescontada);
                alCarteiras.Add(obj);

                obj = new Carteira_BancoBrasil((int)EnumCarteiras_BancoBrasil.CobrançaDiretaEspecialCarteira17);
                alCarteiras.Add(obj);

               return alCarteiras;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar objetos", ex);
            }
        }


        #endregion
    }
}