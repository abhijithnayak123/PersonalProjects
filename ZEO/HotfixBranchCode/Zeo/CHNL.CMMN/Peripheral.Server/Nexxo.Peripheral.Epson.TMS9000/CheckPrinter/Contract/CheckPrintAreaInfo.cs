using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.epson.bank.driver;

namespace MGI.Peripheral.CheckPrinter.Contract
{
    public class CheckPrintAreaInfo
    {
        public AreaTypes AreaType { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        
        public int OriginX { get; set; }
        public int OriginY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ImageRotate Rotate { get; set; }
        public ImageMeasure Measure { get; set; }
        public int FontCategory { get; set; }

        public string FontName { get; set; }
        public FontScale FontScale { get; set; }
        public Single FontSize { get; set; }

        public bool Underline { get; set; }
        public bool Bold { get; set; }
        public PrintColor Color { get; set; }

        public enum AreaTypes
        {
            Text,
            Image
        }
        public FontType PrintFontType
        {
            get
            {
                return FontCategory == 0 ? FontType.MF_PRINT_FONT_A : FontType.MF_PRINT_FONT_B;
            }
        }

    }
    
}
 