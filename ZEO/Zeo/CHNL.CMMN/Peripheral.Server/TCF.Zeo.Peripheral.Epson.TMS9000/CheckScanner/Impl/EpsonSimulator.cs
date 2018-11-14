using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using com.epson.bank.driver;

namespace TCF.Zeo.Peripheral.CheckScanner.EpsonTMS9000.Impl
{
	public partial class TMS9000 : TMS9000Base
	{
		public bool PerformSimulatorScan(String imageFormat)
		{
			Trace.WriteLine("Epson.TMS9000.Impl:PerformSimulatorScan() Invoked", DateTime.Now.ToString());

			checkFrontImage = null;
			checkBackImage = null;
			micrStr = "";
			EpsonException.Clear();
			objConfig.SetImageFormat(imageFormat);

			String scanDir = ConfigurationManager.AppSettings["SimulatorImageDir"];
			if (VerifyDirectory(scanDir) == false)
			{
				EpsonException.errorStatus = true;
				Trace.WriteLine("Epson.TMS9000.Impl:PerformSimulatorScan() Check Simulator Directory", DateTime.Now.ToString());
				return false;
			}
			if (LoadInfo(scanDir) == false)
			{
				EpsonException.errorStatus = true;
				Trace.WriteLine("Epson.TMS9000.Impl:PerformSimulatorScan() Cannot Load Image & MICR Data", DateTime.Now.ToString());
				return false;
			}
			Trace.WriteLine("Epson.TMS9000.Impl:PerformSimulatorScan() Completed", DateTime.Now.ToString());
			return true;
		}

		private bool LoadInfo(String scanDir)
		{
			try
			{
                string accNumberStart = Convert.ToString(ConfigurationManager.AppSettings["AccNumber"]);

                string routingNumberStart = Convert.ToString(ConfigurationManager.AppSettings["RoutingNumber"]);

                DirectoryInfo root = new DirectoryInfo(scanDir);

				FileInfo[] listfiles = root.GetFiles("*_Front.*");

				if (listfiles.Length == 1)
					checkFrontImage = LoadImage(listfiles[0].FullName);

				listfiles = root.GetFiles("*_Back.*");

				if (listfiles.Length == 1)
					checkBackImage = LoadImage(listfiles[0].FullName);

                /*
				listfiles = root.GetFiles("micr.txt");
				if (listfiles.Length == 1)
					micrStr = ReadMicr(listfiles[0].FullName);
				 */

                if (routingNumberStart == "00000000")
                {
                    micrStr = "o001216ot?18238018?280o";
                    checkType = "CHECK_UNKNOWN";
                    countryCode = "COUNTRY_UNKNOWN";
                    accountNumber = "";
                    amount = "";
                    auxillatyOnUSField = "";
                    EPC = "";
                    onUSField = null;
                    transitNumber = "";

                    EpsonException.micrError = 1;
                    EpsonException.errorStatus = false;
                    EpsonException.errorMessage = "Failed to read MICR : Simulator";

                    return true;
                }

                micrStr = String.Format("o039559ot121000248t{0}o", DateTime.Now.ToString("yyMMddHHmmss"));                

                accountNumber = accNumberStart + DateTime.Now.ToString("mmss");

                amount = "0";

                EPC = "0";
                
                transitNumber = routingNumberStart + DateTime.Now.ToString("mmss");

                Trace.WriteLine($"Simulator values : MICR = {micrStr}, Routing number = {transitNumber}, Account Number = {accountNumber}");

                checkType = "CHECK_UNKNOWN";

                countryCode = "COUNTRY_UNKNOWN";

                onUSField = DateTime.Now.ToString("yyMMddHHmmss");

                auxillatyOnUSField = DateTime.Now.ToString("yyMMddHHmmss");
			}
			catch (Exception e)
			{
				EpsonException.SetError(ErrorCode.ERR_IMAGE_FILEREAD, "Failed to read image and micr data.");

				EpsonException.stackTrace = e.StackTrace;

				return false;
			}

			return true;
		}

		private String ReadMicr(String fileName)
		{
			String micr = "";
			try
			{
				StreamReader streamReader = new StreamReader(fileName);
				micr = streamReader.ReadToEnd();
				streamReader.Close();
			}
			catch (Exception e)
			{
				EpsonException.SetError(ErrorCode.ERR_MICR_NODATA, "Could not read Micr Data");
				EpsonException.stackTrace = e.StackTrace;
			}
			return micr;
		}

		private byte[] LoadImage(String fileName)
		{
			Image image = Image.FromFile(fileName);
			MemoryStream ms = new MemoryStream();
			if (objConfig.imageFormat.ToString() == "bmp")
				image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
			if (objConfig.imageFormat.ToString() == "jpg")
				image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
			if (objConfig.imageFormat.ToString() == "tiff")
				image.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
			else
				image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
			return ms.ToArray();
		}

		private bool VerifyDirectory(String scanDir)
		{
			DirectoryInfo root = null;
			try
			{
				root = new DirectoryInfo(scanDir);
			}
			catch (Exception e)
			{
				EpsonException.SetError(ErrorCode.ERR_IMAGE_FILEREAD, "Simulator Image Directory Not Found");
				EpsonException.stackTrace = e.StackTrace;
				return false;
			}

			FileInfo[] listfiles = root.GetFiles("*.*");
			if (listfiles.Length >= 2)
			{
				//File exists
				foreach (FileInfo file in listfiles)
				{
					//Check if there is _Front.*, _Back.* and micr.txt
					String str = file.FullName;
					if (str.IndexOf("_Front.") == -1 && str.IndexOf("micr.txt") == -1 && str.IndexOf("_Back.") == -1)
					{
						EpsonException.SetError(ErrorCode.ERR_NO_TARGET, "Simulator Files are Invalid");
						return false;
					}
				}
				return true;
			}
			else
			{
				//Simulate a timeout
				Thread.Sleep(20 * 1000);
				EpsonException.SetError(ErrorCode.ERR_TIMEOUT, "Timed out while trying to locate image and micr files.");
				return false;
			}
		}
	}
}
