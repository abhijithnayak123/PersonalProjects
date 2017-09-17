using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using com.epson.bank.driver;
using MGI.Peripheral.Printer.Contract;

namespace MGI.Peripheral.Printer.EpsonTMS9000.Impl
{
	public partial class TMS9000Base
	{
		Bitmap simulatorBmp = null;
		int currentHeight = 0;

		public bool PerformSimulatorPrint(PrintData printObj)
		{
			Trace.WriteLine("Epson.TMS9000.Impl:PerformSimulatorPrint() Invoked", DateTime.Now.ToString());
			printSimulator = "true";
			PRINTER_SPACER = 34;
			if (SetObjType(printObj) == false)
			{
				Trace.WriteLine("Epson.TMS9000.Impl:PerformSimulatorPrint() Invalid Print Object", DateTime.Now.ToString());
				EpsonException.SetError(ErrorCode.ERR_NO_TARGET, "Invalid Data Object Type");
				return false;
			}

			int imageHeight = GetTotalImageHeight(printObj);

			InitImage(printObj.ReceiptData.Count * 15 + imageHeight);


			if (printObj.receiptType == "CashCheckReceipt")
				errResult = FormatCashCheckPrint();
			else if (printObj.receiptType == "CashDrawerReport")
				errResult = FormatCashDrawerReportPrint();
			else if (printObj.receiptType.ToLower() == "raw")
				errResult = FormatRawPrint();

			return SaveImage();
		}

		int GetTotalImageHeight(PrintData printObj)
		{
			int imageHeight = 0;
			for (int i = 0; i < printObj.ReceiptData.Count; i++)
			{
				String strLine = printObj.ReceiptData[i];
				if (strLine.StartsWith(STR_IMAGE_CACHE))
				{
					strLine = strLine.Substring(STR_IMAGE_CACHE.Length);
					if (strLine != "")
					{
						MemoryStream imagestream = new System.IO.MemoryStream(Convert.FromBase64String(strLine));
						Bitmap bmp = new Bitmap(imagestream);
						imageHeight += bmp.Height;
					}
				}
			}
			return imageHeight;
		}

		public void InitImage(int height)
		{
			simulatorBmp = new Bitmap(290, height);
			Graphics gImage = Graphics.FromImage(simulatorBmp);
			gImage.Clear(System.Drawing.Color.White);
		}

		public bool SaveImage()
		{
			Trace.WriteLine("Epson.TMS9000.Impl:PerformSimulatorPrint() Saving Simulator Image", DateTime.Now.ToString());
			String printDir = ConfigurationManager.AppSettings["PrinterImageDir"];
			String currTime = DateTime.Now.ToString("MM-dd-yyyy HH.mm.ss");
			String imageFile = printDir + "\\" + currTime + ".jpg";
			try
			{
				simulatorBmp.Save(imageFile, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
			catch
			{
				Trace.WriteLine("Epson.TMS9000.Impl:PerformSimulatorPrint() Invalid Simulator Directory Specified", DateTime.Now.ToString());
				EpsonException.SetError(ErrorCode.ERR_NOT_FOUND, "Invalid Printer Simulator Directory");
				return false;
			}
			Trace.WriteLine("Epson.TMS9000.Impl:PerformSimulatorPrint() Image Saved", DateTime.Now.ToString());
			return true;
		}


		public ErrorCode SetLogoOnSimulator(Bitmap bmp)
		{
			errResult = ErrorCode.SUCCESS;
			Graphics gImage = Graphics.FromImage(simulatorBmp);
			//MemoryStream ms = new MemoryStream(objData.logo);
			//Image logoImage = Image.FromStream(ms);
			//int imageWidth = logoImage.Width;
			//currentHeight = logoImage.Height + 10;
			int center = (290 - bmp.Width) / 2;
			gImage.DrawImage(bmp, new Point(center, 0));
			currentHeight += bmp.Height;
			return errResult;
		}

		public ErrorCode SetLogoOnSimulator()
		{
			errResult = ErrorCode.SUCCESS;
			if (objData.logo != null)
			{
				Graphics gImage = Graphics.FromImage(simulatorBmp);
				MemoryStream ms = new MemoryStream(objData.logo);
				Image logoImage = Image.FromStream(ms);
				int imageWidth = logoImage.Width;
				currentHeight = logoImage.Height + 10;
				int center = (280 - imageWidth) / 2;
				gImage.DrawImage(logoImage, new Point(center, 0));
			}
			else //For testing only
			{
				String printLogoFile = ConfigurationManager.AppSettings["PrinterLogoFile"];
				if (printLogoFile != null)
				{
					try
					{
						Graphics gImage = Graphics.FromImage(simulatorBmp);
						Image img = Image.FromFile(printLogoFile);
						byte[] arr;
						using (MemoryStream ms = new MemoryStream())
						{
							img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
							arr = ms.ToArray();
						}
						int w = img.Width;
						currentHeight = img.Height;
						int x = (280 - w) / 2;
						gImage.DrawImage(img, new Point(x, 0));
					}
					catch (Exception e)
					{
						//Do Nothing even if it is an invalid file
						;
						//EpsonException.SetError(ErrorCode.ERR_NOT_FOUND, "Invalid Logo File");
						EpsonException.stackTrace = e.StackTrace;
						//errResult = ErrorCode.ERR_NOT_FOUND;
					}
				}
			}
			return errResult;
		}

		public ErrorCode PrintInfoOnSimulator(String str, int alignment, System.Drawing.Font objFont, System.Drawing.FontStyle style, int size, int printIfNull)
		{
			errResult = ErrorCode.SUCCESS;
			if (str == "" && printIfNull == 0)
				return errResult;
			Graphics gImage = Graphics.FromImage(simulatorBmp);
			int xPos = 10;
			//objFont = new Font(FontFamily.GenericMonospace, 10, r, System.Drawing.GraphicsUnit.Pixel);style

			//str = str.Replace("\r", "");
			String[] splitStr = str.Split('\n');
			int thisSpacer = 34;
			if (size == 12)
				thisSpacer = 28;

			for (int i = 0; i < splitStr.Length; i++)
			{

				if (splitStr[i] == "")
					continue;

				String useStr = splitStr[i].Replace("\r", "");

				if (alignment == 1)
				{
					int totalSpacers = thisSpacer - useStr.Length;
					String spacers = "";
					for (int j = 0; j < totalSpacers / 2; j++)
						spacers += " ";
					useStr = spacers + useStr + spacers;
				}
				else if (alignment == 2)
				{
					int totalSpacers = thisSpacer - useStr.Length;
					String spacers = "";
					for (int j = 0; j < totalSpacers; j++)
						spacers += " ";
					useStr = spacers + useStr;
				}

				if (style == System.Drawing.FontStyle.Regular)
					gImage.DrawString(useStr, objFont, new SolidBrush(System.Drawing.Color.FromArgb(100, 100, 100)), xPos, currentHeight);
				else
					gImage.DrawString(useStr, objFont, new SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0)), xPos, currentHeight);

				//if (splitStr[i].IndexOf("\r") >= 0)
				currentHeight += 15;

			}
			return errResult;
		}

		public ErrorCode PrintInfoOnSimulator(String str, int alignment, int font, int printIfNull)
		{
			int size = 10;
			errResult = ErrorCode.SUCCESS;
			if (str == "" && printIfNull == 0)
				return errResult;
			Graphics gImage = Graphics.FromImage(simulatorBmp);
			Font objFont;
			int xPos = 10;
			objFont = new Font("Courier New", size, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

			//str = str.Replace("\r", "");
			String[] splitStr = str.Split('\n');

			for (int i = 0; i < splitStr.Length; i++)
			{

				if (splitStr[i] == "")
					continue;

				String useStr = splitStr[i].Replace("\r", "");

				if (alignment == 1)
				{
					int totalSpacers = 34 - useStr.Length;
					String spacers = "";
					for (int j = 0; j < totalSpacers / 2; j++)
						spacers += " ";
					useStr = spacers + useStr + spacers;
				}
				else if (alignment == 2)
				{
					int totalSpacers = 34 - useStr.Length;
					String spacers = "";
					for (int j = 0; j < totalSpacers; j++)
						spacers += " ";
					useStr = spacers + useStr;
				}

				if (font == 0)
					gImage.DrawString(useStr, objFont, new SolidBrush(System.Drawing.Color.FromArgb(100, 100, 100)), xPos, currentHeight);
				else
					gImage.DrawString(useStr, objFont, new SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0)), xPos, currentHeight);

				if (splitStr[i].IndexOf("\r") >= 0)
					currentHeight += 10;
			}
			return errResult;
		}
	}
}
