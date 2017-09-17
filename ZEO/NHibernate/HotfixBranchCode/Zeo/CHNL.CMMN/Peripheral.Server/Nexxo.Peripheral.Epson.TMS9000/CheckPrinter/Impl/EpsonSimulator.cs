using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using MGI.Peripheral.CheckPrinter.Contract;
using com.epson.bank.driver;
using System.Drawing;
using System.Threading;

namespace MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl
{
	public partial class TMS9000 : TMS9000Base
	{
		public bool SimulatorPrint(CheckPrintData printObj)
		{
			try
			{
				string filePath = ConfigurationManager.AppSettings["PrinterImageDir"];
				string fileName = DateTime.Now.ToString("ddMMyyyyhhmmss") + ".txt";
				fileName = System.IO.Path.Combine(filePath, fileName);

				CreateCheckPrintFile(fileName, printObj);

				//Change to support Print & Scan Simulator
				EpsonException.Clear();
				checkFrontImage = null;
				checkBackImage = null;
				String scanDir = ConfigurationManager.AppSettings["SimulatorImageDir"];
				Trace.WriteLine("Scan Directory: " + scanDir, DateTime.Now.ToString());

				if (!LoadInfo(scanDir))
				{
					EpsonException.SetError(ErrorCode.ERR_IMAGE_FILEREAD, "Failed to read image from directory.");
					return false;
				}
				//Change to support Print & Scan Simulator
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Simulator print error : " + ex.Message, DateTime.Now.ToString());
				return false;
			}
			return true;

		}

		private bool LoadInfo(String scanDir)
		{
			Trace.WriteLine("LoadInfo: " + scanDir, DateTime.Now.ToString());
			try
			{
				DirectoryInfo root = new DirectoryInfo(scanDir);
				FileInfo[] listfiles = root.GetFiles("*_Front.*");
				Trace.WriteLine("Front File = : " + listfiles.Length, DateTime.Now.ToString());
				if (listfiles.Length == 1)
					checkFrontImage = LoadImage(listfiles[0].FullName);
				else
					return false;

				listfiles = root.GetFiles("*_Back.*");
				Trace.WriteLine("Back File = : " + listfiles.Length, DateTime.Now.ToString());

				if (listfiles.Length == 1)
					checkBackImage = LoadImage(listfiles[0].FullName);
				else
					return false;

			}
			catch (Exception ex)
			{
				EpsonException.SetError(ErrorCode.ERR_IMAGE_FILEREAD, "Failed to read image.");
				EpsonException.stackTrace = ex.StackTrace;
				return false;
			}

			return true;
		}

		private byte[] LoadImage(String fileName)
		{
			Trace.WriteLine("Load File = : " + fileName, DateTime.Now.ToString());
			MemoryStream ms = new MemoryStream();
			try
			{
				Image image = Image.FromFile(fileName);
				image.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
			}
			catch (Exception ex)
			{
				Trace.WriteLine("ExceptionMessage : " + ex.StackTrace);
			}
			return ms.ToArray();
		}

		public void CreateCheckPrintFile(string fileName, CheckPrintData printObj)
		{
			using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
			{
				for (int i = 0; i < printObj.CheckData.Count; i++)
				{
					if (printObj.CheckData[i].Trim() != "")
					{
						MFPrintAreaInfo objPrintAreaInfo = new MFPrintAreaInfo();
						MFDeviceFont objDeviceFont = new MFDeviceFont();
						MFTrueType customFont = new MFTrueType();
						CheckPrintAreaInfo checkPrintAreaInfo = new CheckPrintAreaInfo();

						SetPrintAreaDefaults(checkPrintAreaInfo);

						string[] items = printObj.CheckData[i].Split('^');

						foreach (string item in items)
						{
							SetAreaProperty(item, checkPrintAreaInfo);
						}
						objPrintAreaInfo = GetPrintAreaInfo(checkPrintAreaInfo);

						file.WriteLine("[" + objPrintAreaInfo.AreaName.TrimEnd('\0') + "]");
						file.WriteLine("OriginX : " + objPrintAreaInfo.OriginX);
						file.WriteLine("OriginY : " + objPrintAreaInfo.OriginY);
						file.WriteLine("Width : " + objPrintAreaInfo.Width);
						file.WriteLine("Height : " + objPrintAreaInfo.Height);
						file.WriteLine("Rotate : " + objPrintAreaInfo.Rotate);
						file.WriteLine("Measure : " + objPrintAreaInfo.Measure);

						if (checkPrintAreaInfo.AreaType == CheckPrintAreaInfo.AreaTypes.Text)
						{
							if (checkPrintAreaInfo.FontCategory == 2) //custom font
							{
								customFont = GetTrueTypeFont(checkPrintAreaInfo);

								file.WriteLine("Custom Font : " + customFont.Font.Name);
								file.WriteLine("Custom Font Size : " + customFont.Font.Size.ToString());
								file.WriteLine("Custom Underline : " + customFont.Font.Underline.ToString());
								file.WriteLine("Custom Bold : " + customFont.Font.Bold.ToString());
								file.WriteLine("Text : " + checkPrintAreaInfo.Data);
							}
							else //device default fonts
							{
								objDeviceFont = GetDeviceFont(checkPrintAreaInfo);

								file.WriteLine("Font : " + objDeviceFont.Font.ToString());
								file.WriteLine("Font Size : " + objDeviceFont.Size.ToString());
								file.WriteLine("Underline : " + objDeviceFont.Underline.ToString());
								file.WriteLine("Bold : " + objDeviceFont.Bold.ToString());
								file.WriteLine("Text : " + checkPrintAreaInfo.Data);
							}
						}
						else
						{
							file.WriteLine("Image : " + checkPrintAreaInfo.Data);
						}
						file.WriteLine("");
					}
				}
			}
		}
	}
}
