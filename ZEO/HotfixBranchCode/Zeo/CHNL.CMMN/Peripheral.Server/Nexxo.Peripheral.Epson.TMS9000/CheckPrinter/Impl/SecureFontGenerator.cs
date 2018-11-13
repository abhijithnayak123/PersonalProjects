using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl
{
    public class SecureFontGenerator
    {
        public string CreateNexxoSecureFont01(string font, float size, ImageFormat imageFormat, string textToPrint, bool bold, out int imageWidth, out int imageHeight)
        {
            System.Drawing.Font objFont = new System.Drawing.Font((string.IsNullOrEmpty(font) ? "Arial" : font),
                (size == 0.00 ? 12 : size),
                (bold ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular),
                System.Drawing.GraphicsUnit.Pixel);
            
            Form form=new Form();
            Graphics objGraphics = form.CreateGraphics();

            // This is where the bitmap size is determined.
            int intWidth = (int)objGraphics.MeasureString(textToPrint, objFont).Width;
            int intHeight = (int)objGraphics.MeasureString(textToPrint, objFont).Height;
                        
            Bitmap objBmpImage = new Bitmap(intWidth + intHeight, intHeight);

            // Create a graphics object to measure the text's width and height.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background And Image text
            objGraphics.Clear(System.Drawing.Color.White);
            objGraphics.FillPie(new SolidBrush(System.Drawing.Color.Black), new Rectangle(0, 0, intHeight, intHeight), 90, 180);
            objGraphics.FillRectangle(new SolidBrush(System.Drawing.Color.Black), Convert.ToInt32(intHeight / 2), 1, intWidth, intHeight);
            objGraphics.DrawString(textToPrint, objFont, new SolidBrush(System.Drawing.Color.White), Convert.ToInt32(intHeight / 2), 0, StringFormat.GenericDefault);
            objGraphics.FillPie(new SolidBrush(System.Drawing.Color.Black), new Rectangle(intWidth - 1, 0, intHeight, intHeight), 270, 180);
            objGraphics.SmoothingMode = SmoothingMode.HighQuality;
            objGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            objGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            objGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            objGraphics.Flush();

            // Convert the image to byte[]
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            objBmpImage.Save(stream, imageFormat);
            byte[] imageBytes = stream.ToArray();

            Rectangle section = new Rectangle(0, 0, intWidth + intHeight, intHeight);
            objGraphics.DrawImage(objBmpImage, 0, 0, section, GraphicsUnit.Pixel);

            imageWidth = intWidth;
            imageHeight = intHeight;

            return System.Convert.ToBase64String(imageBytes);
        }
    }
}
