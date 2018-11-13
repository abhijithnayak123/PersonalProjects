using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using com.epson.bank.driver;

namespace MGI.Peripheral.Printer.EpsonTMS9000.Impl
{
	public partial class TMS9000Base
	{
		int PRINTER_SPACER = 42;
		string STR_IMAGE_CACHE = ".IMAGECACHE:";
		string STR_NEXXO_LOGO = ".NexxoLogo.";
		string STR_ALIGNMENT = ".c.";
		string STR_FONT_ARIAL = ".f";
		string STR_FONT_ARIAL_NARROW = ".g";
		string STR_FONT_COURIER = ".h";
		string STR_FONT_STYLE_BOLD = ".b.";

		protected ErrorCode FormatCashDrawerReportPrint()
		{
			errResult = ErrorCode.SUCCESS;
			errResult = SetPrintBuffer(cashDrawerPrintList);
			return errResult;
		}

		protected ErrorCode FormatCashCheckPrint()
		{
			errResult = ErrorCode.SUCCESS;
			errResult = SetPrintBuffer(cashCheckPrintList);
			return errResult;
		}

		protected ErrorCode FormatRawPrint()
		{
			errResult = ErrorCode.SUCCESS;
			errResult = SetRawPrintBuffer();
			return errResult;
		}

		public ErrorCode SetRawPrintBuffer()
		{
			errResult = ErrorCode.SUCCESS;
			for (int i = 0; i < objData.ReceiptData.Count; i++)
			{
				BufferLine(objMFDevice, objData.ReceiptData[i]);
			}

			if (printSimulator == "false")
			{
				errResult = objMFDevice.PrintText("\r\n", objFont);
				errResult = objMFDevice.PrintText("\r\n", objFont);
				errResult = objMFDevice.PrintText("\r\n", objFont);
			}
			return errResult;
		}

		public void BufferLine(MFDevice objMFDevice, String lineItem)
		{
			int alignment = 0; //0=left, 1 = centered, 2 = right
			int fontSize = 10;//Default size
			int fontStyle = 0; //0 = regular; 1 = bold; 2 = Italic
			String fontFamily = "Arial";//Default
			String printLine = "";
			lineItem = lineItem.TrimStart('\r');
			if (lineItem.StartsWith(STR_ALIGNMENT))
			{
				alignment = 1;
				lineItem = lineItem.Substring(STR_ALIGNMENT.Length);
			}
			if (lineItem.StartsWith(STR_FONT_ARIAL))
			{
				fontSize = int.Parse(lineItem.Substring(2, 2));
				fontFamily = "Arial";
				lineItem = lineItem.Substring(STR_FONT_ARIAL.Length + 3);
			}
			if (lineItem.StartsWith(STR_FONT_ARIAL_NARROW))
			{
				fontSize = int.Parse(lineItem.Substring(2, 2));
				fontFamily = "Courier New"; //Shuld b Arial Narrow, till alignment is solved
				lineItem = lineItem.Substring(STR_FONT_ARIAL_NARROW.Length + 3);
			}
			if (lineItem.StartsWith(STR_FONT_COURIER))
			{
				fontSize = int.Parse(lineItem.Substring(2, 2));
				fontFamily = "Courier New";
				lineItem = lineItem.Substring(STR_FONT_COURIER.Length + 3);
			}
			if (lineItem.StartsWith(STR_FONT_STYLE_BOLD))
			{
				fontStyle = 1;
				lineItem = lineItem.Substring(STR_FONT_STYLE_BOLD.Length);
			}
			if (lineItem.StartsWith(STR_IMAGE_CACHE))
			{
				PrintBase64Image(lineItem.Substring(STR_IMAGE_CACHE.Length), alignment);
				lineItem = "";
			}
			if (lineItem.StartsWith(STR_NEXXO_LOGO))
			{
				PrintBase64Image(lineItem.Substring(STR_NEXXO_LOGO.Length), alignment);
				lineItem = "";
			}
			if (lineItem.IndexOf("^") >= 0)
			{
				Console.WriteLine("Multi Format Line encountered");
				String part1Str = "";
				String part2Str = "";
				string[] parts = lineItem.Split('^');
				for (int j = 0; j < parts.Length; j++)
				{
					Console.WriteLine("\tPart[" + j + "] = " + parts[j]);
					if (j == 0)
						part1Str = parts[j];
					if (j == 1 && parts[j] != "")
						part1Str = part1Str + "   " + parts[j];
					if (j == 2)
						part2Str = parts[j];
				}
				//Commented since calculate for spaces handles this
				//if (part1Str != "")
				{
					printLine = CalculateForSpaces(alignment, fontFamily, fontSize, fontStyle, part1Str, part2Str);
					alignment = 0;
				}
				//else
				//{
				//    printLine = part2Str;
				//    alignment = 2;
				//}
				lineItem = "";
			}
			else if (lineItem.IndexOf("|") >= 0)
			{
				string[] parts = lineItem.Split('|');
				int p = 0;
				for (int j = 0; j < parts.Length; j++)
				{
					p = 0;
					if (parts[j].Length > 0)
					{
						p = int.Parse(parts[j].Substring(0, 3));
						if (p <= 0)
							p = 5;
						printLine = printLine + GetSpaces(p) + parts[j].Substring(3);
					}
				}
				lineItem = "";
			}

			if (lineItem != "")
				printLine = lineItem;

			if (printLine == "")
				printLine = "\r\n";

			Console.WriteLine("-------- PRINT THIS ----------- " + printLine);

			System.Drawing.FontStyle style = System.Drawing.FontStyle.Regular;
			if (fontStyle == 0)
				style = System.Drawing.FontStyle.Regular;
			else if (fontStyle == 1)
				style = System.Drawing.FontStyle.Bold;
			else if (fontStyle == 2)
				style = System.Drawing.FontStyle.Italic;

			System.Drawing.Font trueFont = new System.Drawing.Font(fontFamily, fontSize, style);
			MFTrueType font = new MFTrueType(trueFont);

			if (printSimulator == "false")
			{
				if (alignment == 0)
					objMFDevice.SetPrintAlignment(Alignment.MF_PRINT_ALIGNMENT_LEFT);
				if (alignment == 1)
					objMFDevice.SetPrintAlignment(Alignment.MF_PRINT_ALIGNMENT_CENTER);
				if (alignment == 2)
					objMFDevice.SetPrintAlignment(Alignment.MF_PRINT_ALIGNMENT_RIGHT);
				objMFDevice.PrintText(printLine, font);
			}
			else if (printSimulator == "true")
			{
				PrintInfoOnSimulator(printLine, alignment, trueFont, style, fontSize, 1);
			}
		}

		public void PrintBase64Image(String base64Str, int alignment)
		{
			errResult = ErrorCode.SUCCESS;

			if (base64Str != "")
			{
				MemoryStream imagestream = new System.IO.MemoryStream(Convert.FromBase64String(base64Str));
				Bitmap bmp = new Bitmap(imagestream);
				if (printSimulator == "false")
				{
					errResult = objMFDevice.SetPrintAlignment(Alignment.MF_PRINT_ALIGNMENT_CENTER);
					objMFDevice.PrintMemoryImage(bmp);
				}
				else
				{
					SetLogoOnSimulator(bmp);
				}
			}
		}

		public String CalculateForSpaces(int alignment, String family, int size, int style, String str1, String str2)
		{
			if (family == "Courier New")
			{
				String totalStr = str1 + str2;
				int totalLen = totalStr.Length;
				if (size == 10)
					return str1 + GetSpaces(34 - totalLen) + str2;
				else
					return str1 + GetSpaces(30 - totalLen) + str2;
			}
			else if (family == "Arial")
			{
				float finMeasure = GetMeasureUnit(family, size, style, str1 + str2);
				float spacers = 71 - finMeasure;
				if (size == 11)
					spacers = spacers - 2;
				double iSpacer = Math.Round(spacers);
				return str1 + GetSpaces(Convert.ToInt32(iSpacer)) + str2;
			}
			else if (family == "Arial Narrow")
			{
				float finMeasure = GetMeasureUnit(family, size, style, str1 + str2);
				float spacers = 78 - finMeasure;
				if (style == 1 || size == 11)
					spacers = spacers - 2;
				double iSpacer = Math.Round(spacers);
				return str1 + GetSpaces(Convert.ToInt32(iSpacer)) + str2;
			}
			return str1 + " " + str2;

			/*
			Console.WriteLine("Calculate Spaces for alignment = {0}, family = {1}, size = {2}", alignment, family, size);
			float resultSpace = GetMeasureUnit(family, size, "X x") - GetMeasureUnit(family, size, "Xx");
			float result1 = GetMeasureUnit(family, size, str1);
			float result2 = GetMeasureUnit(family, size, str2);
			Console.WriteLine("Space = {0}, Result1={1},Result2={2}", resultSpace, result1, result2);

			if (alignment == 1) // centered
			{
				float result = GetMeasureUnit(family, size, str1);
				float leftSpaces = ((71 - result) / 2) / resultSpace;
				str1 = GetSpaces(Convert.ToInt32(leftSpaces)) + str1;
			}


			float emptySpace = (71 - (result1 + result2)) / resultSpace;
			int actualEmptySpace = Convert.ToInt32(emptySpace);


			return str1 + GetSpaces(actualEmptySpace) + str2;
			 */
		}

		public float GetMeasureUnit(String family, int size, int style, String str)
		{
			System.Drawing.Font font = null;
			if (style == 0)
				font = new System.Drawing.Font(family, size, FontStyle.Regular);
			else
				font = new System.Drawing.Font(family, size, FontStyle.Bold);

			System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero);
			graphics.PageUnit = System.Drawing.GraphicsUnit.Millimeter;
			return graphics.MeasureString(str, font, int.MaxValue, System.Drawing.StringFormat.GenericTypographic).Width;
		}

		public static String GetSpaces(int spaces)
		{
			String strSpaces = "";
			for (int i = 0; i < spaces; i++)
				strSpaces += " ";
			return strSpaces;
		}

		public ErrorCode SetPrintBuffer(IList<PrintDefinition> printList)
		{
			errResult = ErrorCode.SUCCESS;
			for (int i = 0; i < printList.Count(); i++)
			{
				PrintDefinition printLineItem = printList.ElementAt(i);

				if (printLineItem.type == "HEADER")
					SetPrintBuffer(headerPrintList);
				else if (printLineItem.type == "FOOTER")
					SetPrintBuffer(footerPrintList);
				else if (printLineItem.type == "LOGO")
				{
					if (printSimulator == "false")
						SetLogoOnBuffer();
					else
						SetLogoOnSimulator();
				}
				else if (printLineItem.type == "TEXT")
				{
					String str = "";
					if (printLineItem.spacersRequired == 1)
						str = GetLeftRightFormat(printLineItem.text1, GetData(printLineItem.text2));
					else
					{
						if (printLineItem.text1.Length > 0)
							str = printLineItem.text1 + " " + GetData(printLineItem.text2);
						else
							str = GetData(printLineItem.text2);
					}

					if (str == null) str = "";

					if (printSimulator == "false")
						PrintInfoOnBuffer(str, printLineItem.alignment, printLineItem.fontType, printLineItem.printIfNull);
					else
						PrintInfoOnSimulator(str, printLineItem.alignment, printLineItem.fontType, printLineItem.printIfNull);
				}
				else if (printLineItem.type == "STATIC")
				{
					if (printSimulator == "false")
						PrintInfoOnBuffer(printLineItem.text1, printLineItem.alignment, printLineItem.fontType, printLineItem.printIfNull);
					else
						PrintInfoOnSimulator(printLineItem.text1, printLineItem.alignment, printLineItem.fontType, printLineItem.printIfNull);
				}
			}
			return errResult;
		}

		public ErrorCode SetLogoOnBuffer()
		{
			errResult = ErrorCode.SUCCESS;
			errResult = objMFDevice.SetPrintAlignment(Alignment.MF_PRINT_ALIGNMENT_CENTER);
			if (objData.logo != null)
			{
				MemoryStream imagestream = new System.IO.MemoryStream(objData.logo);
				Bitmap bmp = new Bitmap(imagestream);
				objMFDevice.PrintMemoryImage(bmp);
			}
			return errResult;
		}

		public ErrorCode PrintInfoOnBuffer(String str, int alignment, int font, int printIfNull)
		{
			errResult = ErrorCode.SUCCESS;
			if (printIfNull == 0 && str == "")
				return errResult;

			if (alignment == 0)
				errResult = objMFDevice.SetPrintAlignment(Alignment.MF_PRINT_ALIGNMENT_LEFT);
			else if (alignment == 1)
				errResult = objMFDevice.SetPrintAlignment(Alignment.MF_PRINT_ALIGNMENT_CENTER);
			else if (alignment == 2)
				errResult = objMFDevice.SetPrintAlignment(Alignment.MF_PRINT_ALIGNMENT_RIGHT);

			if (font == 1)
				objFont.Bold = true;
			else if (font == 0)
				objFont.Bold = false;

			errResult = objMFDevice.PrintText(str, objFont);
			return errResult;
		}

		private String GetLeftRightFormat(String leftStr, String rightStr)
		{
			if (leftStr == null)
				return "";
			if (rightStr == null)
				rightStr = "";

			int spacers = PRINTER_SPACER - (leftStr.Length + rightStr.Length);

			String spaceStr = "";
			for (int i = 0; i < spacers; i++)
				spaceStr += " ";

			return leftStr + spaceStr + rightStr;
		}
	}
}
