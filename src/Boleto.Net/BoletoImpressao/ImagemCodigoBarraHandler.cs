using System.Web;
using System.Drawing.Imaging;

namespace BoletoNet
{
    internal class ImagemCodigoBarraHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var code = context.Request.QueryString[0];
            context.Response.Write(code);
            var contentType = "image/jpeg";
            var filename = "barcode2of5.jpg";

            context.Response.Clear();
            context.Response.ContentType = contentType;
            context.Response.AddHeader("content-disposition", "outline;filename=" + filename);

            var img = new C2of5i(code, 1, 50, code.Length).ToBitmap();

            //img = img.GetThumbnailImage(460, 61, null, new IntPtr()) as System.Drawing.Bitmap;

            img.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            img.Dispose();

            //context.Response.BinaryWrite(new C2of5i(code, 1, 50, code.Length).ToByte());
            context.Response.Flush();
        }

        #endregion
    }
}
