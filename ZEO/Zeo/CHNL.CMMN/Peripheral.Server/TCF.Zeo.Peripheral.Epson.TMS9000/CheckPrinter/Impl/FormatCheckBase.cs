using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using com.epson.bank.driver;
using System.Diagnostics;
using TCF.Zeo.Peripheral.CheckPrinter.Contract;

namespace TCF.Zeo.Peripheral.CheckPrinter.Epson.TMS9000.Impl
{
    public partial class TMS9000Base
    {
        public const string AREA_NAME = ".AREANAME.";            // Unique name for each area
        public const string AREA_ORIGINX = ".ORIGINX.";            // X position of area in MM
        public const string AREA_ORIGINY = ".ORIGINY.";            // Y position of area in MM
        public const string AREA_WIDTH = ".WIDTH.";
        public const string AREA_HEIGHT = ".HEIGHT.";
        public const string AREA_ROTATE = ".ROTATE.";             // 0 ( default), 90, 180, 270 - to rotate area
        public const string AREA_FONT_CATEGORY = ".FONTCATEGORY.";        // 0 , 1, 2 : 0 and 1 for default printer fonts A & B, 2 for custom fonts. custom font names can be set using FONTNAME field. 
        public const string AREA_FONT_NAME = ".FONTNAME.";
        public const string AREA_FONT_UNDERLINE = ".UNDERLINE.";    // 0 , 1, 2
        public const string AREA_FONT_SIZE = ".FONTSIZE.";         // 0 , 1, 2, 3 - Normal, tall, wide, tall & wide
        public const string AREA_FONT_BOLD = ".BOLD.";         // 0, 1
        public const string TEXT = ".TEXT.";
        public const string IMAGE = ".IMAGE.";
        public const string NEXXO_SECUREFONT = "NEXXOSECUREFONT";
        ImageFormat SECUREIMAGEFORMAT = ImageFormat.Bmp;

        private const uint TRANS_NO_SPECIFIED = 0x40000000;
        private uint transactionNumber = 0;

        public void SetPrintAreaDefaults(CheckPrintAreaInfo checkPrintAreaInfo)
        {
            checkPrintAreaInfo.Rotate = ImageRotate.IMAGEROTATE_0;
            checkPrintAreaInfo.Measure = ImageMeasure.MEASURE_MM;

            checkPrintAreaInfo.FontCategory = 0;
            checkPrintAreaInfo.Underline = false;
            checkPrintAreaInfo.FontScale = FontScale.MF_PRINT_FONT_W1_H1;
            checkPrintAreaInfo.Bold = false;
            checkPrintAreaInfo.Color = PrintColor.MF_PRINT_BLACK;
            checkPrintAreaInfo.FontSize = 0.0f;
            checkPrintAreaInfo.FontName = "Arial";
        }

        /// <summary>
        /// Iterates through each line, parse it and set print area
        /// </summary>
        /// <returns></returns>
        public ErrorCode AddPrintDataToBuffer(int transactionNumber)
        {
            errResult = ErrorCode.SUCCESS;
            this.transactionNumber = (uint)transactionNumber;

            for (int i = 0; i < objData.CheckData.Count; i++)
            {
                if (objData.CheckData[i].Trim() != "")
                {
                    errResult = SetPrintArea(objMFDevice, objData.CheckData[i]);

                    if (errResult != ErrorCode.SUCCESS)
                    {
                        return errResult;
                    }
                }
            }
            return errResult;
        }

        /// <summary>
        /// Parse one line and set print area
        /// </summary>
        /// <param name="objMFDevice"></param>
        /// <param name="areaLine"></param>
        /// <returns></returns>
        private ErrorCode SetPrintArea(MFDevice objMFDevice, string areaLine)
        {
            MFPrintAreaInfo objPrintAreaInfo = new MFPrintAreaInfo();
            MFDeviceFont objDeviceFont = new MFDeviceFont();
            MFTrueType customFont = new MFTrueType();

            int imageWidth = 0;
            int imageHeight = 0;

            CheckPrintAreaInfo checkPrintAreaInfo = new CheckPrintAreaInfo();
            SecureFontGenerator secureFontGenerator = new SecureFontGenerator();

            errResult = ErrorCode.SUCCESS;

            SetPrintAreaDefaults(checkPrintAreaInfo);

            string[] items = areaLine.Split('^');

            foreach (string item in items)
            {
                SetAreaProperty(item, checkPrintAreaInfo);
            }
            if (checkPrintAreaInfo.FontName.ToUpper().StartsWith(NEXXO_SECUREFONT))
            {
                if (checkPrintAreaInfo.FontName.ToUpper().StartsWith("NEXXOSECUREFONT01"))
                {
                    checkPrintAreaInfo.AreaType = CheckPrintAreaInfo.AreaTypes.Image;

                    checkPrintAreaInfo.Data =
                        secureFontGenerator.CreateNexxoSecureFont01(
                            checkPrintAreaInfo.FontName.Substring(NEXXO_SECUREFONT.Length + 3),
                            checkPrintAreaInfo.FontSize, SECUREIMAGEFORMAT, checkPrintAreaInfo.Data,
                            checkPrintAreaInfo.Bold, out imageWidth, out imageHeight);

                    checkPrintAreaInfo.Width = checkPrintAreaInfo.Height * imageWidth / imageHeight;
                }
            }
            objPrintAreaInfo = GetPrintAreaInfo(checkPrintAreaInfo);

            if (objPrintAreaInfo.OriginX > 154)
                objPrintAreaInfo.OriginX = 154;
            errResult = objMFDevice.SetTemplatePrintArea(AreaSelectMode.SELECTPRINTAREA_DIRECT, null, objPrintAreaInfo, 1);

            if (errResult != ErrorCode.SUCCESS)
            {
                Trace.WriteLine("FormatCheckPrint.SetPrintArea Error - X : " + objPrintAreaInfo.OriginX.ToString() + " Y : " + objPrintAreaInfo.OriginY.ToString() + " Width : " + objPrintAreaInfo.Width.ToString() + " Height : " + objPrintAreaInfo.Height.ToString() + " Text/Image : " + checkPrintAreaInfo.Data, DateTime.Now.ToString());

                return errResult;
            }            
            if (checkPrintAreaInfo.AreaType == CheckPrintAreaInfo.AreaTypes.Image)
            {
                try
                {
                    errResult = objMFDevice.SCNPrintMemoryImage(transactionNumber, GetImageFromBase64String(checkPrintAreaInfo.Data));
                }
                catch
                {
                    Trace.WriteLine("FormatCheckPrint.SetPrintArea Invalid Image Data Error - X : " + objPrintAreaInfo.OriginX.ToString() + " Y : " + objPrintAreaInfo.OriginY.ToString() + " Width : " + objPrintAreaInfo.Width.ToString() + " Height : " + objPrintAreaInfo.Height.ToString() + " Text/Image : " + checkPrintAreaInfo.Data, DateTime.Now.ToString());

                    return(ErrorCode.ERR_DATA_INVALID);
                }
            }
            else
            {
                if (checkPrintAreaInfo.FontCategory == 2) //custom font
                {
                    customFont = GetTrueTypeFont(checkPrintAreaInfo);

                    errResult = objMFDevice.SCNPrintText(transactionNumber, checkPrintAreaInfo.Data, customFont);
                }
                else //device default fonts
                {
                    //objDeviceFont = GetDeviceFont(checkPrintAreaInfo);
                    //errResult = objMFDevice.PrintText(checkPrintAreaInfo.Data, objDeviceFont);
                    customFont = GetDeviceMappedFont(checkPrintAreaInfo);
                    errResult = objMFDevice.SCNPrintText(1, checkPrintAreaInfo.Data, customFont);
                }
            }
            return errResult;
        }

        public MFTrueType GetDeviceMappedFont(CheckPrintAreaInfo checkPrintAreaInfo)
        {
            MFTrueType trueType = new MFTrueType();
            Font font;

            checkPrintAreaInfo.FontName = "Arial"; // All Device font family now maps to Arial
            if (checkPrintAreaInfo.FontSize == 0) //Default Font Size if 0 is found
                checkPrintAreaInfo.FontSize = 10.0f;
            else if (checkPrintAreaInfo.FontSize == 1) 
                checkPrintAreaInfo.FontSize = 11.0f;
            else if (checkPrintAreaInfo.FontSize == 2)
                checkPrintAreaInfo.FontSize = 12.0f;
            else if (checkPrintAreaInfo.FontSize == 3)
                checkPrintAreaInfo.FontSize = 14.0f;

            if (checkPrintAreaInfo.Bold == true && checkPrintAreaInfo.Underline == true)
            {
                font = new System.Drawing.Font("Arial", checkPrintAreaInfo.FontSize, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            else if (checkPrintAreaInfo.Bold == true && checkPrintAreaInfo.Underline == false)
            {
                font = new System.Drawing.Font("Arial", checkPrintAreaInfo.FontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            else if (checkPrintAreaInfo.Underline == true)
            {
                font = new System.Drawing.Font("Arial", checkPrintAreaInfo.FontSize, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            else
            {
                font = new System.Drawing.Font("Arial", checkPrintAreaInfo.FontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            trueType.Font = font;

            return trueType;
        }


        public MFDeviceFont GetDeviceFont(CheckPrintAreaInfo checkPrintAreaInfo)
        {
            MFDeviceFont deviceFont = new MFDeviceFont();

            deviceFont.Font = checkPrintAreaInfo.PrintFontType;
            deviceFont.Bold = checkPrintAreaInfo.Bold;
            deviceFont.Underline = checkPrintAreaInfo.Underline == false ? UnderlineType.MF_PRINT_NO_UNDERLINE : UnderlineType.MF_PRINT_UNDERLINE_1;
            deviceFont.Size = checkPrintAreaInfo.FontScale;

            return deviceFont;
        }

        public MFTrueType GetTrueTypeFont(CheckPrintAreaInfo checkPrintAreaInfo)
        {
            MFTrueType trueType = new MFTrueType();

            Font font;

            if (checkPrintAreaInfo.Bold == true && checkPrintAreaInfo.Underline == true)
            {
                font = new Font(checkPrintAreaInfo.FontName, checkPrintAreaInfo.FontSize, FontStyle.Bold | FontStyle.Underline);
            }
            else if (checkPrintAreaInfo.Bold == true && checkPrintAreaInfo.Underline == false)
            {
                font = new Font(checkPrintAreaInfo.FontName, checkPrintAreaInfo.FontSize, FontStyle.Bold);
            }
            else if (checkPrintAreaInfo.Underline == true)
            {
                font = new Font(checkPrintAreaInfo.FontName, checkPrintAreaInfo.FontSize, FontStyle.Underline);
            }
            else
            {
                font = new Font(checkPrintAreaInfo.FontName, checkPrintAreaInfo.FontSize);
            }
            trueType.Font = font;

            return trueType;
        }

        public MFPrintAreaInfo GetPrintAreaInfo(CheckPrintAreaInfo checkPrintAreaInfo)
        {
            MFPrintAreaInfo objPrintAreaInfo = new MFPrintAreaInfo();

            objPrintAreaInfo.OriginX = checkPrintAreaInfo.OriginX;
            objPrintAreaInfo.OriginY = checkPrintAreaInfo.OriginY;
            objPrintAreaInfo.AreaName = checkPrintAreaInfo.Name;
            objPrintAreaInfo.Height = checkPrintAreaInfo.Height;
            objPrintAreaInfo.Measure = checkPrintAreaInfo.Measure;
            objPrintAreaInfo.Rotate = checkPrintAreaInfo.Rotate;
            objPrintAreaInfo.Width = checkPrintAreaInfo.Width;

            return objPrintAreaInfo;
        }

        private Bitmap GetImageFromBase64String(string data)
        {
            MemoryStream imagestream = new System.IO.MemoryStream(Convert.FromBase64String(data));
            return new Bitmap(imagestream);
        }

        public void SetAreaProperty(string areaItem, CheckPrintAreaInfo checkPrintAreaInfo)
        {
            if (areaItem.StartsWith(AREA_NAME))
            {
                checkPrintAreaInfo.Name = areaItem.Substring(AREA_NAME.Length);
            }
            else if (areaItem.StartsWith(AREA_ORIGINX))
            {
                checkPrintAreaInfo.OriginX = int.Parse(areaItem.Substring(AREA_ORIGINX.Length));
            }
            else if (areaItem.StartsWith(AREA_ORIGINY))
            {
                checkPrintAreaInfo.OriginY = int.Parse(areaItem.Substring(AREA_ORIGINY.Length));
            }
            else if (areaItem.StartsWith(AREA_WIDTH))
            {
                checkPrintAreaInfo.Width = int.Parse(areaItem.Substring(AREA_WIDTH.Length));
            }
            else if (areaItem.StartsWith(AREA_HEIGHT))
            {
                checkPrintAreaInfo.Height = int.Parse(areaItem.Substring(AREA_HEIGHT.Length));
            }
            else if (areaItem.StartsWith(AREA_ROTATE))
            {
                int rotation = int.Parse(areaItem.Substring(AREA_ROTATE.Length));

                switch (rotation)
                {
                    case 90:
                        checkPrintAreaInfo.Rotate = ImageRotate.IMAGEROTATE_90;
                        break;
                    case 180:
                        checkPrintAreaInfo.Rotate = ImageRotate.IMAGEROTATE_180;
                        break;
                    case 270:
                        checkPrintAreaInfo.Rotate = ImageRotate.IMAGEROTATE_270;
                        break;
                    default:
                        checkPrintAreaInfo.Rotate = ImageRotate.IMAGEROTATE_0;
                        break;
                }
            }
            else if (areaItem.StartsWith(AREA_FONT_CATEGORY))
            {
                checkPrintAreaInfo.FontCategory = int.Parse(areaItem.Substring(AREA_FONT_CATEGORY.Length));
            }
            else if (areaItem.StartsWith(AREA_FONT_NAME))
            {
                checkPrintAreaInfo.FontName = areaItem.Substring(AREA_FONT_NAME.Length);
            }
            else if (areaItem.StartsWith(AREA_FONT_UNDERLINE))
            {
                int underline = int.Parse(areaItem.Substring(AREA_FONT_UNDERLINE.Length));

                checkPrintAreaInfo.Underline = underline == 0 ? false : true;
            }
            else if (areaItem.StartsWith(AREA_FONT_BOLD))
            {
                checkPrintAreaInfo.Bold = int.Parse(areaItem.Substring(AREA_FONT_BOLD.Length)) == 0 ? false : true;
            }
            else if (areaItem.StartsWith(AREA_FONT_SIZE))
            {
                Single emSize = Single.Parse(areaItem.Substring(AREA_FONT_SIZE.Length));

                int size;

                if (int.TryParse(areaItem.Substring(AREA_FONT_SIZE.Length), out size))
                {
                    if (size == 0)
                    {
                        checkPrintAreaInfo.FontScale = FontScale.MF_PRINT_FONT_W1_H1;
                    }
                    else if (size == 1)
                    {
                        checkPrintAreaInfo.FontScale = FontScale.MF_PRINT_FONT_W1_H2;
                    }
                    else if (size == 2)
                    {
                        checkPrintAreaInfo.FontScale = FontScale.MF_PRINT_FONT_W2_H1;
                    }
                    else if (size == 3)
                    {
                        checkPrintAreaInfo.FontScale = FontScale.MF_PRINT_FONT_W2_H2;
                    }
                }
                checkPrintAreaInfo.FontSize = emSize;
            }
            else if (areaItem.StartsWith(TEXT))
            {
                checkPrintAreaInfo.AreaType = CheckPrintAreaInfo.AreaTypes.Text;

                checkPrintAreaInfo.Data = areaItem.Substring(TEXT.Length);
            }
            else if (areaItem.StartsWith(IMAGE))
            {
                checkPrintAreaInfo.AreaType = CheckPrintAreaInfo.AreaTypes.Image;

                checkPrintAreaInfo.Data = areaItem.Substring(IMAGE.Length);
            }
        }

    }
}
