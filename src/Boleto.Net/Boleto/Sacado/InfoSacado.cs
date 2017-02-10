namespace BoletoNet
{
    public class InfoSacado 
    {
        readonly string[] _data;

        /// <summary></summary>
        /// <param name="info">Texto da informação</param>
        public InfoSacado(string info)
        {
            _data = new[] { info };
        }
        
        /// <summary></summary>
        /// <param name="linha1">Texto da primeira linha</param>
        /// <param name="linha2">Texto da segunda linha</param>
        public InfoSacado(string linha1, string linha2)
        {
            _data = new[]{linha1,linha2};
        }

        /// <summary></summary>
        /// <param name="linhas">Vetor com as infomaçoes do Sacado, onde cada posição é uma linha da informação no boleto</param>
        public InfoSacado(string[] linhas) 
        {
            _data = linhas;
        }

        public string Html
        {
            get
            {
                var rtn = "";

                foreach (var s in _data)
                    rtn += "<br />" + s;

                return rtn;
            }
        }

        public static string Render(string linha1, string linha2, bool novaLinha)
        {
            return Render(new[] { linha1, linha2 }, novaLinha);
        }

        public static string Render(string[] linhas, bool novaLinha)
        {
            var rtn = "";
            foreach (var s in linhas)
                rtn += "<br />" + s;
            
            if (!novaLinha) rtn = rtn.Substring(6);

            return rtn;
        }
    }
}