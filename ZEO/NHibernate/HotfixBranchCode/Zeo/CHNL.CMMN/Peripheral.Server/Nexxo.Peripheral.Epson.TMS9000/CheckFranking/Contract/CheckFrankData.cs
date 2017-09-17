using System;
using System.Collections.Generic;

namespace MGI.Peripheral.CheckFranking.Contract
{
    public class CheckFrankData
    {
        public String FrankData { get; set; }
        public String Orientation { get; set; }
        public String FontFace { get; set; }
        public String FontType { get; set; }
        public int FontSize { get; set; }
        public int XPos { get; set; }
        public int YPos { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
