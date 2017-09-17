using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Drawing;
using System.IO;
using MGI.Common.Util;


namespace MGI.Channel.DMS.Web.Common
{

    public class FileUtility
    {
        static string STR_IMAGE_CACHE = ".IMAGECACHE:";
        static string STR_NEXXO_LOGO = ".NexxoLogo.";
        static string STR_PRINT_IMAGE = ".IMAGE.";

        public static byte[] GetFileData(string filePath)
        {
            byte[] fileData = null;
            if (!string.IsNullOrEmpty(filePath))
            {
                System.IO.FileStream fileObject = System.IO.File.Open(filePath, System.IO.FileMode.Open,FileAccess.Read);
                fileData = new byte[fileObject.Length];
                fileObject.Read(fileData, 0, (int)fileObject.Length);
                fileObject.Flush();
                fileObject.Close();
            }
            return fileData;
        }

        public static string[] UpdateReceiptDataForLogo(string[] receiptData)
        {
            for (int i = 0; i < receiptData.Length; i++)
            {
                String lineItem = receiptData[i];
                if (lineItem.StartsWith(STR_IMAGE_CACHE))
                {
                    String imageFileName = lineItem.Substring(STR_IMAGE_CACHE.Length);
                    string base64Str = GetLogoBase64(imageFileName);
                    receiptData[i] = STR_IMAGE_CACHE + base64Str;
                }
                else if (lineItem.StartsWith(STR_NEXXO_LOGO))
                {
                    String imageFileName = lineItem.Substring(STR_NEXXO_LOGO.Length);
                    string base64Str = GetLogoBase64(imageFileName);
                    receiptData[i] = STR_NEXXO_LOGO + base64Str;
                }
            }

            return receiptData;
        }

        static string GetLogoBase64(String fileName)
        {
            string str = String.Empty;
            //get the url from web.config
            String url = ConfigurationManager.AppSettings["ReceiptLogoUrl"] + fileName;
            try
            {
                System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                _HttpWebRequest.AllowWriteStreamBuffering = true;
                System.Net.WebResponse _WebResponse = _HttpWebRequest.GetResponse();
                System.IO.Stream _WebStream = _WebResponse.GetResponseStream();
                Image _tmpImage = Image.FromStream(_WebStream);
                using (MemoryStream ms = new MemoryStream())
                {
                    _tmpImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageBytes = ms.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        public static string[] ConvertCheckPrintImagesToBase64(string[] checkPrintLineItems)
        {
            for (int lineItemIndex = 0; lineItemIndex < checkPrintLineItems.Length; lineItemIndex++)
            {
                string[] lineParts = checkPrintLineItems[lineItemIndex].Split('^');//Splitting each items to Array

                for (int partsIndex = 0; partsIndex < lineParts.Length; partsIndex++)
                {
                    string linePart = lineParts[partsIndex];

                    if (lineParts[partsIndex].StartsWith(STR_PRINT_IMAGE))//Checking any Part contains IMAGE Part 
                    {
                        String imageFileName = linePart.Substring(STR_PRINT_IMAGE.Length);

                        string base64Str = GetImageBase64(imageFileName);//Getting Base64 String for Image

                        checkPrintLineItems[lineItemIndex] = checkPrintLineItems[lineItemIndex].Replace(imageFileName, base64Str);//Replacing Image with Base64 code

                        break;
                    }
                }

            }

            return checkPrintLineItems;
        }

        public static string GetImageBase64(String fileName)
        {
            String url = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/CheckPrint/") + fileName;
            string str = String.Empty;

            try
            {
                str = Convert.ToBase64String(FileUtility.GetFileData(url));
            }
            catch (Exception e)
            {
                NLogHelper.Error(string.Format("Exception  : {0} Trace : {1}", e.Message, e.StackTrace));
              
                return str;
            }
            return str;
        }

    }
}