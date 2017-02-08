using System;
using System.IO;

namespace BoletoNet
{
    public interface IArquivoRetorno
    {
        /// <summary>
        /// Ler arquivo de Retorno
        /// </summary>
        void LerArquivoRetorno(IBanco banco, Stream arquivo);

        IBanco Banco { get; }
        TipoArquivo TipoArquivo { get; }

        event EventHandler<LinhaDeArquivoLidaArgs> LinhaDeArquivoLida;
    }
}
