using System;
using System.IO;
using System.Drawing;
using System.Configuration;
using System.Diagnostics;
using com.epson.bank.driver;
using System.ServiceModel;
using System.ServiceModel.Web;
using MGI.Peripheral.CheckScanner.Contract;
using MGI.Peripheral.Queue.Impl;

namespace MGI.Peripheral.Checkscanner.EpsonTMS9000.Impl
{
	public class CheckScanner : ICheckScanner
	{
        TMS9000 scanner = new TMS9000();
		public CheckScannerError ScanCheck(String imageFormat, String saveRequired)
		{
			Trace.WriteLine("Epson.TMS9000.Impl:ScanCheck() Invoked", DateTime.Now.ToString());
			String scanMode = ConfigurationManager.AppSettings["ScannerIsSimulator"];
			JobQueue jobQueue = new JobQueue();
			if (jobQueue.SetScanJob(imageFormat))
			{
				jobQueue.GetScanJob();

                
                if (scanMode == null) 
                    scanMode = "false";
                if (scanMode == "true")
                {
                    if ( scanner.PerformSimulatorScan(imageFormat) == true )
                        if (saveRequired == "true")
                            scanner.uniqueId = scanner.StoreScan(scanner.checkFrontImage, scanner.checkBackImage);

                }
                else
                {
                    if (scanner.PerformScan(imageFormat) == true)
                    {
                        if (saveRequired == "true")
                            scanner.uniqueId = scanner.StoreScan(scanner.checkFrontImage, scanner.checkBackImage);
                    }
                }
				jobQueue.SetJobComplete();
			}
			else
			{
				CheckScannerError scannerErr = new CheckScannerError()
				{
					errorStatus = true,
					errorCode = Convert.ToInt32(ErrorCode.ERR_ACCESS),
					errorMessage = "Scanner resource is busy, Please try again later",
					errorDescription = "Another job is in progress",
					stackTrace = String.Empty
				};
				return scannerErr;
			}
			Trace.WriteLine("Returning from Epson.TMS9000.Impl:ScanCheck()", DateTime.Now.ToString());
			return Error();
		}

        public byte[] GetCheckFrontImage(String format)
        {
            if (format.Equals("jpg"))
                return ConvertToJpeg(scanner.checkFrontImage);
            else
                return scanner.checkFrontImage;// ConvertToTiff(scanner.checkFrontImage);
        }

        public byte[] GetCheckBackImage(String format)
        {
            if (format.Equals("jpg"))
                return ConvertToJpeg(scanner.checkBackImage);
            else
                return scanner.checkBackImage;
        }

        public byte[] ConvertToJpeg(byte[] tiffBytes)
        {
            try
            {
                Trace.WriteLine("Converting to JPEG", DateTime.Now.ToString());
                using (MemoryStream inStream = new MemoryStream(tiffBytes))
                using (MemoryStream outStream = new MemoryStream())
                {
                    Trace.WriteLine("Saving Stream as JPEG", DateTime.Now.ToString());
                    System.Drawing.Bitmap.FromStream(inStream).Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return outStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception is = " + ex.Message + " ====" + ex.StackTrace + "====" + ex.InnerException);
            }
            return null;
        }

        public byte[] ConvertToTiff(byte[] jpgBytes)
        {
            Byte[] outBytes;
            using (MemoryStream inStream = new MemoryStream(jpgBytes))
            using (MemoryStream outStream = new MemoryStream())
            {
                System.Drawing.Bitmap.FromStream(inStream).Save(outStream, System.Drawing.Imaging.ImageFormat.Tiff);
                return outStream.ToArray();
                //Convert to monochrome
                Bitmap tiffBmp = new Bitmap(outStream);
                using (Bitmap targetBmp = tiffBmp.Clone(new Rectangle(0, 0, tiffBmp.Width, tiffBmp.Height), System.Drawing.Imaging.PixelFormat.Format1bppIndexed))
                using (MemoryStream targetStream = new MemoryStream())
                {
                    targetBmp.Save(targetStream, System.Drawing.Imaging.ImageFormat.Tiff);
                    outBytes = targetStream.ToArray();
                }
            }
            return outBytes;
        }
        
        public Stream GetImage(String id)
        {
            string imageFile = AppDomain.CurrentDomain.BaseDirectory + "Temp\\" + id + ".jpg";
            FileStream fs = File.OpenRead(imageFile);
            WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";
            return fs;
        }

        public String ConvertStream(String frontSide, String backSide)
        {
            Trace.WriteLine("Epson.TMS9000.Impl:ConvertStream() Invoked", DateTime.Now.ToString());
            return scanner.StoreScan(Convert.FromBase64String(frontSide), Convert.FromBase64String(backSide));
            Trace.WriteLine("Epson.TMS9000.Impl:ConvertStream() Exited", DateTime.Now.ToString());

        }
                    
        public String GetMicr()
        {
            return scanner.micrStr;
        }

        public String GetMicrAccountNumber()
        {
            return scanner.accountNumber;
        }

        public String GetUniqueId()
        {
            return scanner.uniqueId;
        }

        public string GetMicrAmount()
        {
            return scanner.amount;
        }

        public string GetMicrECP()
        {
            return scanner.EPC;
        }

        public string GetMicrTransitNumber()
        {
            return scanner.transitNumber;
        }

        public string GetMicrCheckType()
        {
            return scanner.checkType;
        }

        public string GetMicrCountryCode()
        {
            return scanner.countryCode;
        }

        public string GetMicrOnUSField()
        {
            return scanner.onUSField;
        }

        public string GetMicrAuxillatyOnUSField()
        {
            return scanner.auxillatyOnUSField;
        }

		private CheckScannerError Error()
		{
			CheckScannerError scannerErr = new CheckScannerError()
			{
				errorStatus = EpsonException.errorStatus,
				errorCode = EpsonException.errorCode,
				errorMessage = EpsonException.errorMessage,
				errorDescription = EpsonException.errorDescription,
				stackTrace = EpsonException.stackTrace,
                micrError = EpsonException.micrError
			};
			return scannerErr;
		}
	}
}
