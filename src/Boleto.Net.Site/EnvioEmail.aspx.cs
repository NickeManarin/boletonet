using System;
using System.IO;
using System.Net.Mail;
using System.Web.UI;
using BoletoNet;

public partial class EnvioEmail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected MailMessage PreparaMail()
    {
        var mail = new MailMessage();
        mail.To.Add(new MailAddress(TextBox1.Text));
        mail.Subject = "Teste de envio de Boleto Bancário";
        mail.IsBodyHtml = true;
        mail.Priority = MailPriority.High;
        return mail;
    }

    protected BoletoBancario PreparaBoleto()
    {
        var vencimento = new DateTime(2007, 9, 10);

        var item1 = new Instrucao_Itau(9, 5);
        var item2 = new Instrucao_Itau(81, 10);
        var c = new Cedente("00.000.000/0000-00", "Empresa de Atacado", "0542", "13000");
        //Na carteira 198 o código do Cedente é a conta bancária
        c.Codigo = "13000";

        var b = new Boleto(vencimento, 1642, "198", "92082835", c);
        b.NumeroDocumento = "1008073";

        b.Sacado = new Sacado("000.000.000-00", "Fulano de Silva");
        b.Sacado.Endereco.End = "SSS 154 Bloco J Casa 23";
        b.Sacado.Endereco.Bairro = "Testando";
        b.Sacado.Endereco.Cidade = "Testelândia";
        b.Sacado.Endereco.CEP = "70000000";
        b.Sacado.Endereco.UF = "DF";

        item2.Descricao += " " + item2.Dias + " dias corridos do vencimento.";
        b.Instrucoes.Add(item1);
        b.Instrucoes.Add(item2);
        
        var itau = new BoletoBancario();
        itau.CodigoBanco = 341;
        itau.Boleto = b;

        return itau;
    }


    protected void Button1_Click(object sender, EventArgs e)
    {


        var  itau = PreparaBoleto();
        var mail = PreparaMail();

        if (RadioButton1.Checked)
        {
            mail.Subject += " - On-Line";
            Panel1.Controls.Add(itau);

            var sw = new System.IO.StringWriter();
            var htmlTW = new HtmlTextWriter(sw);
            Panel1.RenderControl(htmlTW);
            var html = sw.ToString();
            //
            mail.Body = html;
        }
        else
        {
            mail.Subject += " - Off-Line";
            mail.AlternateViews.Add(itau.HtmlBoletoParaEnvioEmail());
        }

        MandaEmail(mail);
        Label1.Text = "Boleto simples enviado para o email: " + TextBox1.Text;
    }
    protected void Button1_Click2(object sender, EventArgs e)
    {

        var itau = PreparaBoleto();

        // embora estou mandando o mesmo boleto duas vezes, voce pode obviamente mandar boletos distintos
        var arrayDeBoletos = new BoletoBancario[] { itau, itau };
        var  av = BoletoBancario.GeraHtmlDeVariosBoletosParaEmail("Isto é um email com <b>dois</b> boletos", arrayDeBoletos);

        var  mail = PreparaMail();
        mail.Subject += " - Off-Line - Múltiplo";
        mail.AlternateViews.Add(av);

        MandaEmail(mail);
        Label1.Text = "Boleto múltimplo enviado para o email: " + TextBox1.Text;
    }


    void MandaEmail(MailMessage mail)
    {
        var objSmtpClient = new SmtpClient();

        objSmtpClient.Host = "smtp.dominio.com.br";
        objSmtpClient.Port = 25;
        objSmtpClient.EnableSsl = false;
        objSmtpClient.Credentials = new System.Net.NetworkCredential("stiven@callas.com.br", "123456");
        objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        objSmtpClient.Timeout = 10000;
        objSmtpClient.Send(mail);
    }
}
