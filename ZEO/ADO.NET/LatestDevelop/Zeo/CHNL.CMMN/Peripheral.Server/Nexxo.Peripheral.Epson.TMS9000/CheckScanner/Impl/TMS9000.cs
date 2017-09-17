using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using com.epson.bank.driver;

namespace MGI.Peripheral.Checkscanner.EpsonTMS9000.Impl
{
    public partial class TMS9000 : TMS9000Base
    {
        public bool PerformScan(String imageFormat)
        {
            Trace.WriteLine("Epson.TMS9000.Impl:PerformScan() Invoked", DateTime.Now.ToString());
            //Return if printer is not available
            objConfig.SetImageFormat(imageFormat);

                Trace.WriteLine("Epson.TMS9000.Initializing Scanner", DateTime.Now.ToString());
                if (Init() == false)
                {
                    Trace.WriteLine("Epson.TMS9000.Impl:PerformScan() Initialization Failed", DateTime.Now.ToString());
                    return false;
                }

                Trace.WriteLine("Epson.TMS9000.Opening Scanner", DateTime.Now.ToString());
                if (Open() == false)
                {
                    Trace.WriteLine("Epson.TMS9000.Impl:PerformScan() Open Failed", DateTime.Now.ToString());
                    if (EpsonException.errorCode == -400)
                    {
                        Close();
                    }
                    return false;
                }

                Trace.WriteLine("Epson.TMS9000.Scanning Check", DateTime.Now.ToString());
                if (ScanCheck() == false)
                {
                    Trace.WriteLine("Epson.TMS9000.Impl:PerformScan() ScanCheck() Failed Error: " + EpsonException.errorCode + " " + EpsonException.errorMessage + " " + EpsonException.errorDescription, DateTime.Now.ToString());
                    Close();
                    return false;
                }

                Trace.WriteLine("Epson.TMS9000.Closing Scanner", DateTime.Now.ToString());
                if (Close() == false)
                    return false;

                Trace.WriteLine("Epson.TMS9000.Impl:PerformScan() Completed", DateTime.Now.ToString());
                return true;
        }

        public bool Init()
        {
            try
            {
                scanTimer = new EpsonTimer();
                checkFrontImage = null;
                checkBackImage = null;
                deviceOpen = false;

                if ((errResult = InitDevice()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Initialization Error!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                EpsonException.SetException(ex);
                return false;
            }
            return true;
        }

        public bool Open()
        {
            try
            {
                if ((errResult = OpenDevice()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Error opening device.");
                    return false;
                }

                if ((errResult = ResetPrinter()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "The printer is being reset. Please retry the transaction.");
                    return false;
                }

                if ((errResult = RegisterEvents()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Error registering callbacks.");
                    return false;
                }

                if ((errResult = SetScanUnit()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Error setting scan unit.");
                    return false;
                }

                if ((errResult = SetScanPageSettings()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Errror while setting scan parameters");
                    return false;
                }

                if ((errResult = SetMICRSettings()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Error during MICR Settings");
                    return false;
                }

                if ((errResult = SetProcessSettings()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Error setting process parameters");
                    return false;
                }
            }
            catch (Exception ex)
            {
                EpsonException.SetException(ex);
                return false;
            }

            return true;
        }

        public bool ScanCheck()
        {
            try
            {
                Trace.WriteLine("Epson.TMS9000.Impl:Initiating Scan", DateTime.Now.ToString());
                if ((errResult = InitiateScan()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Error while trying to scan check.");
                    return false;
                }

                Trace.WriteLine("Epson.TMS9000.Impl:Initiate Scan Completed", DateTime.Now.ToString());

                while (true)
                {
                    if (transactionComplete == true)
                    {
                        Trace.WriteLine("Epson.TMS9000.Impl:Transaction Completed", DateTime.Now.ToString());
                        break;
                    }
                    if (scanTimer.HasTimedOut() == true)
                    {
                        Trace.WriteLine("Epson.TMS9000.Impl:Scanner Timed out", DateTime.Now.ToString());
                        break;
                    }
                    Thread.Sleep(1000 * 1);
                    Trace.WriteLine("Epson.TMS9000.Impl:Waiting for trasaction complted to true which is " + transactionComplete, DateTime.Now.ToString());

                }

                Trace.WriteLine("Epson.TMS9000.Impl:Out of timer loop...", DateTime.Now.ToString());

                if (EpsonException.errorStatus == true)
                    return false;

                if (scanTimer.HasTimedOut() == true)
                {
                    errResult = ErrorCode.ERR_TIMEOUT;
                    EpsonException.SetError(errResult, "Error while trying to scan check. Timeout encountered.");
                    return false;
                }

                if (CheckIntegrity() == false)
                {
                    errResult = ErrorCode.ERR_DATA_INVALID;
                    EpsonException.SetError(errResult, "Error while trying to scan check.");
                    return false;
                }

            }
            catch (Exception ex)
            {
                EpsonException.SetError(ErrorCode.ERR_EXEC_SCAN, "Initiate Scan Failed, Driver installed?");
                EpsonException.SetException(ex);
                return false;
            }

            return true;
        }

        public bool Close()
        {
            try
            {
                if ((errResult = CloseDevice()) != ErrorCode.SUCCESS)
                    return false;
            }
            catch (Exception ex)
            {
                EpsonException.SetException(ex);
                return false;
            }
            return true;
        }

        public string StoreScan(byte[] frontSide, byte[] backSide)
        {
            Trace.WriteLine("Epson.TMS9000.Impl:StoreScan()", DateTime.Now.ToString());
            string id = null;
            try
            {
      
                id = StoreImage(frontSide, backSide);
                if ( id == null)
                {
                    errResult = ErrorCode.ERR_DATA_INVALID;
                    EpsonException.SetError(errResult, "Error storing scanned checks.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                EpsonException.SetException(ex);
                return null;
            }
            return id;
        }


        public string StoreImage(byte[] fronSide, byte[] backSide)
        {
            Trace.WriteLine("Epson.TMS9000.Impl:StoreImage()", DateTime.Now.ToString());
            //Create Bitmap for Front & Back
            Trace.WriteLine("Epson.TMS9000.Impl:StoreImage() Rezise and Combine", DateTime.Now.ToString());
            MemoryStream frontStream = new MemoryStream(fronSide);
            Bitmap frontBitmap = ResizeImage((Bitmap)Image.FromStream(frontStream));

            MemoryStream backStream = new MemoryStream(backSide);
            Bitmap backBitmap = ResizeImage((Bitmap)Image.FromStream(backStream));

            //Create a new bitmap
            Bitmap combinedBitmap = new Bitmap(frontBitmap.Width, frontBitmap.Height + frontBitmap.Height + 10);
            Graphics combinedImage = Graphics.FromImage(combinedBitmap);
            combinedImage.Clear(System.Drawing.Color.White);

            Trace.WriteLine("Epson.TMS9000.Impl:StoreImage() Draw", DateTime.Now.ToString());
            //Append Images
            combinedImage.DrawImage(frontBitmap, 0, 0);
            combinedImage.DrawImage(backBitmap, 0, frontBitmap.Height+10);
            combinedImage.Save();


            //Change from time to micr if it is available
            Trace.WriteLine("Epson.TMS9000.Impl:StoreImage() for MICR " + micrStr, DateTime.Now.ToString());
            String imageId = micrStr ;
            imageId = imageId.TrimEnd('\0');
            if (micrStr == null)
                micrStr = String.Empty;
            if (micrStr == String.Empty)
                    imageId = DateTime.Now.ToString("yyyyMMddHHmmssfff");;
            if (imageId.Contains("?"))
                imageId = imageId.Replace('?', '_');

            String imageFile = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\" + imageId + ".jpg";

            Trace.WriteLine("Epson.TMS9000.Impl:Image Save in " + Path.GetFullPath(imageFile), DateTime.Now.ToString());
            combinedBitmap.Save(imageFile, System.Drawing.Imaging.ImageFormat.Jpeg);
            return imageId;
        }

        public Bitmap ResizeImage(Bitmap imgToResize)
        {
            int newWidth = Convert.ToInt16(imgToResize.Width / (2.7 ));
            int newHeight = Convert.ToInt16(imgToResize.Height / (2.7));
            Bitmap resizedImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage((Image)resizedImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                g.DrawImage(imgToResize, 0, 0, newWidth, newHeight);
            }
            return resizedImage;
        }

    }
}
