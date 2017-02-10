using System.Collections.Generic;
using System.IO;

namespace BoletoNet
{
    /// <summary>
    /// Classe básica de um arquivo EDI
    /// </summary>
    public class EdiFile
    {
        #region Propriedades

        public List<RegistroEdi> Lines = new List<RegistroEdi>();

        #endregion

        #region Métodos Privados e Protegidos

        /// <summary>
        /// Decodifica a linha do registro EDI para os campos; O tipo de campo/registro EDI depende
        /// do layout da entidade.
        /// </summary>
        /// <param name="line">Linha do arquivo a ser decodificada</param>
        protected virtual void DecodeLine(string line)
        { }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Carrega um arquivo EDI
        /// </summary>
        /// <param name="fileName">Nome do arquivo a ser carregado</param>
        public virtual void LoadFromFile(string fileName)
        {
            using (var sr = new StreamReader(fileName))
            {
                Lines.Clear();

                while (!sr.EndOfStream)
                {
                    DecodeLine(sr.ReadLine());
                }
            }
        }

        public virtual void LoadFromStream(Stream s)
        {
            Lines.Clear();

            using (var sr = new StreamReader(s))
            {
                while (!sr.EndOfStream)
                {
                    DecodeLine(sr.ReadLine());
                }
            }
        }

        /// <summary>
        /// Grava um arquivo EDI em disco
        /// </summary>
        /// <param name="fileName">Nome do arquivo EDI a ser salvo</param>
        public virtual void SaveToFile(string fileName)
        {
            using (var sw = new StreamWriter(fileName))
            {
                foreach (var linha in Lines)
                {
                    linha.CodificarLinha();
                    sw.WriteLine(linha.LinhaRegistro);
                }
            }
        }

        #endregion
    }
}
