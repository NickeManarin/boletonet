using System;
using System.Windows.Forms;
using System.IO;

namespace BoletoNet.Arquivo
{
    public partial class ImpressaoBoleto : Form
    {
        public ImpressaoBoleto()
        {
            InitializeComponent();
        }

        private void visualizarImagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormVisualizarImagem(GerarImagem());
            form.ShowDialog();
        }

        private string GerarImagem()
        {
            var address = webBrowser.Url.ToString();
            var width = 670;
            var height = 805;

            var webBrowserWidth = 670;
            var webBrowserHeight = 805;

            var bmp = WebsiteThumbnailImageGenerator.GetWebSiteThumbnail(address, webBrowserWidth, webBrowserHeight, width, height);

            var file = Path.Combine(Environment.CurrentDirectory, "boleto.bmp");
            
            if (File.Exists(file))
                File.Delete(file);

            bmp.Save(file);

            return file;
        }

        private void enviarImagemPorEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new EnviarEmail(GerarImagem());
            form.ShowDialog();
        }
    }
}