using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace BoletoNet
{
    public class C2of5i : BarCodeBase
    {
        #region variables

        private readonly string[] _cPattern = new string[100];
        private const string Start = "0000";
        private const string Stop = "1000";
        private Bitmap _bitmap;
        private Graphics _g;

        #endregion

        #region Constructor

        public C2of5i()
        {}

        /// <summary>
        /// Code 2 of 5 intrelaced Constructor
        /// </summary>
        /// <param name="code">The string that contents the numeric code</param>
        /// <param name="barWidth">The Width of each bar</param>
        /// <param name="height">The Height of each bar</param>
        public C2of5i(string code, int barWidth, int height)
        {
            Code = code;
            Height = height;
            Width = barWidth;
        }

        /// <summary>
        /// Code 2 of 5 intrelaced Constructor
        /// </summary>
        /// <param name="code">The string that contents the numeric code</param>
        /// <param name="barWidth">The Width of each bar</param>
        /// <param name="height">The Height of each bar</param>
        /// <param name="digits">Number of digits of code</param>
        public C2of5i(string code, int barWidth, int height, int digits)
        {
            Code = code;
            Height = height;
            Width = barWidth;
            Digits = digits;
        }

        #endregion

        private void FillPatern()
        {
            if (_cPattern[0] != null)
                return;

            _cPattern[0] = "00110";
            _cPattern[1] = "10001";
            _cPattern[2] = "01001";
            _cPattern[3] = "11000";
            _cPattern[4] = "00101";
            _cPattern[5] = "10100";
            _cPattern[6] = "01100";
            _cPattern[7] = "00011";
            _cPattern[8] = "10010";
            _cPattern[9] = "01010";

            //Create a draw pattern for each char from 0 to 99
            for (var f1 = 9; f1 >= 0; f1--)
            {
                for (var f2 = 9; f2 >= 0; f2--)
                {
                    var f = f1 * 10 + f2;
                    var strTemp = "";

                    for (var i = 0; i < 5; i++)
                    {
                        strTemp += _cPattern[f1][i] + _cPattern[f2][i].ToString();
                    }

                    _cPattern[f] = strTemp;
                }
            }
        }

        /// <summary>
        /// Generate the Bitmap of Barcode.
        /// </summary>
        /// <returns>Return System.Drawing.Bitmap</returns>
        public Bitmap ToBitmap()
        {
            xPos = 0;
            yPos = 0;

            if (Digits == 0)
                Digits = Code.Length;
            
            if (Digits % 2 > 0) Digits++;

            while (Code.Length < Digits || Code.Length % 2 > 0)
            {
                Code = "0" + Code;
            }

            var width = (2 * Full + 3 * Thin) * (Digits) + 7 * Thin + Full;

            _bitmap = new Bitmap(width, Height);
            _g = Graphics.FromImage(_bitmap);

            //Start Pattern
            DrawPattern(ref _g, Start);

            //Draw code
            FillPatern();
            while (Code.Length > 0)
            {
                var i = Convert.ToInt32(Code.Substring(0, 2));

                Code = Code.Length > 2 ? Code.Substring(2, Code.Length - 2) : "";

                var ftemp = _cPattern[i];
                DrawPattern(ref _g, ftemp);
            }

            //Stop Patern
            DrawPattern(ref _g, Stop);

            return ResizeImage(_bitmap, (int)(width * 1.5), Height);
        }

        /// <summary>
        /// Returns the byte array of Barcode
        /// </summary>
        /// <returns>byte[]</returns>
        public byte[] ToByte()
        {
            return base.ToByte(ToBitmap());
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.PixelOffsetMode = PixelOffsetMode.None;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
