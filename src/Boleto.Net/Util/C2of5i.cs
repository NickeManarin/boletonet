using System;
using System.Drawing;

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
        {
        }
        /// <summary>
        /// Code 2 of 5 intrelaced Constructor
        /// </summary>
        /// <param name="Code">The string that contents the numeric code</param>
        /// <param name="BarWidth">The Width of each bar</param>
        /// <param name="Height">The Height of each bar</param>
        public C2of5i(string Code, int BarWidth, int Height)
        {
            this.Code = Code;
            this.Height = Height;
            Width = BarWidth;
        }
        /// <summary>
        /// Code 2 of 5 intrelaced Constructor
        /// </summary>
        /// <param name="Code">The string that contents the numeric code</param>
        /// <param name="BarWidth">The Width of each bar</param>
        /// <param name="Height">The Height of each bar</param>
        /// <param name="Digits">Number of digits of code</param>
        public C2of5i(string Code, int BarWidth, int Height, int Digits)
        {
            this.Code = Code;
            this.Height = Height;
            Width = BarWidth;
            this.Digits = Digits;
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
            {
                Digits = Code.Length;
            }

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

            return _bitmap;
        }

        /// <summary>
        /// Returns the byte array of Barcode
        /// </summary>
        /// <returns>byte[]</returns>
        public byte[] ToByte()
        {
            return base.ToByte(ToBitmap());
        }
    }
}
