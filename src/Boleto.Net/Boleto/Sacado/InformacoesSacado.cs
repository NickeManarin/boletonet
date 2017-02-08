using System.Collections.Generic;

namespace BoletoNet
{
    public class InformacoesSacado: List<InfoSacado>  
    {
        /// <summary>
        /// Retorna HTML representativo de todo conteudo
        /// </summary>
        public string GeraHtml(bool novaLinha)
        {   
            var rtn = "";

            if (Count <= 0)
                return rtn;

            foreach (InfoSacado I in this)
                rtn += I.Html;
                
            if (!novaLinha)
                rtn = rtn.Substring(6);

            return rtn;
        }
    }
}
