using System;
using com.epson.bank.driver;

namespace MGI.Peripheral.Checkscanner.EpsonTMS9000.Impl
{
	public class EpsonConfig
	{
		public String deviceTranslator = "TMS9000";
		public String deviceIP;
		public int devicePort;

		public String scannerSettings;
		public ColorDepth colorDepth = ColorDepth.EPS_BI_SCN_1BIT;
		public String epsonPrinterName = "TM-S9000U";
		public MfScanDpi resolution = MfScanDpi.MF_SCAN_DPI_200;
		public ImageTypeOption imageType = ImageTypeOption.EPS_BI_SCN_OPTION_GRAYSCALE;
		public Color imageColor = Color.EPS_BI_SCN_MONOCHROME;
		public Format imageFormat = Format.EPS_BI_SCN_TIFF;

		public RemoveSpace micrRemoveSpace = RemoveSpace.CLEAR_SPACE_ENABLE;
		public MfEject errInsertionPocket = MfEject.MF_EJECT_MAIN_POCKET;
		public MfEject errNoisePocket = MfEject.MF_EJECT_MAIN_POCKET;
		public MfEject errDoubleFeedPocket = MfEject.MF_EJECT_MAIN_POCKET;
		public MfEject errBadDataPocket = MfEject.MF_EJECT_MAIN_POCKET;
		public MfEject errNoDataPocket = MfEject.MF_EJECT_MAIN_POCKET;


		// Depth Can be 1, 8 or 24
		public void SetColorDepth(int depth)
		{
			switch (depth)
			{
				case 1:
					colorDepth = ColorDepth.EPS_BI_SCN_1BIT;
					break;
				case 8:
					colorDepth = ColorDepth.EPS_BI_SCN_8BIT;
					break;
				case 24:
					colorDepth = ColorDepth.EPS_BI_SCN_24BIT;
					break;
				default:
					colorDepth = ColorDepth.EPS_BI_SCN_24BIT;
					break;
			}
		}


		public void SetPrinterName(String printerName)
		{
			if (printerName != "")
				epsonPrinterName = printerName;
			else
				epsonPrinterName = "TM-S9000U";
		}

		// Resolution can be 0,100,120,200,240,300,600 
		//(600 point to 300 as the printer currently does not support 600)
		public void SetResolution(int res)
		{
			switch (res)
			{
				case 0:
					resolution = MfScanDpi.MF_SCAN_DPI_DEFAULT;
					break;
				case 100:
					resolution = MfScanDpi.MF_SCAN_DPI_100;
					break;
				case 120:
					resolution = MfScanDpi.MF_SCAN_DPI_120;
					break;
				case 200:
					resolution = MfScanDpi.MF_SCAN_DPI_200;
					break;
				case 240:
					resolution = MfScanDpi.MF_SCAN_DPI_240;
					break;
				case 300:
					resolution = MfScanDpi.MF_SCAN_DPI_300;
					break;
				case 600:
					resolution = MfScanDpi.MF_SCAN_DPI_300;
					break;
				default:
					resolution = MfScanDpi.MF_SCAN_DPI_DEFAULT;
					break;
			}
		}

		//Can be Monochrome or Color
		public void SetImageColor(String type)
		{
			if (type == "monochrome")
				imageColor = Color.EPS_BI_SCN_MONOCHROME;
			else if (type == "color")
				imageColor = Color.EPS_BI_SCN_COLOR;
			else
                imageColor = Color.EPS_BI_SCN_MONOCHROME;
		}

		//Can be tiff, bmp, or jpg
		public void SetImageFormat(String format)
		{
			if (format.ToLower() == "tiff")
				imageFormat = Format.EPS_BI_SCN_TIFF;
			else if (format.ToLower() == "bmp")
				imageFormat = Format.EPS_BI_SCN_BITMAP;
			else if (format.ToLower() == "jpg")
				imageFormat = Format.EPS_BI_SCN_JPEGNORMAL;
			else
                imageFormat = Format.EPS_BI_SCN_TIFF;
		}
	}
}