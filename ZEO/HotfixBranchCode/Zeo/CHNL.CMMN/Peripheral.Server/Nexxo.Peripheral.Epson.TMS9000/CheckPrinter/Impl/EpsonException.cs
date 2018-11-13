using System;
using com.epson.bank.driver;
using System.Diagnostics;

namespace MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl
{
	public static class EpsonException
	{
		public static String errorMessage;
		public static String errorDescription;
		public static int errorCode;
		public static bool errorStatus;
		public static String stackTrace;

		public static void Clear()
		{
			errorMessage = "";
			errorDescription = "";
			errorCode = 0;
			errorStatus = false;
		}

		public static void SetError(ErrorCode err, String msg)
		{
			if (err != ErrorCode.SUCCESS)
			{
				errorCode = (int)err;
				errorMessage = msg;
				errorStatus = true;
				errorDescription = GetDescription(err);
				if (errorDescription == "") errorDescription = errorMessage;
                Trace.WriteLine("Error in previous Operation : ErrorCode : " + errorCode.ToString() + ", Error message : " + msg, DateTime.Now.ToString());
			}
		}

		public static void SetException(Exception e)
		{
			Exception inner = e.InnerException;
			if (inner != null)
			{
				SetException(inner);
			}
			if (e is Exception)
			{
				errorMessage = "Exception Occured";
				errorDescription = "See Stack Trace";
				stackTrace = e.StackTrace;
				errorCode = (int)ErrorCode.ERR_UNKNOWN;
				errorStatus = true;
			}
		}

		private static String GetDescription(ErrorCode err)
		{
			String str = "";
			switch (err)
			{
				case ErrorCode.ERR_UNKNOWN:
					str = "Unknown Error Occured, Please restart your printer or scanner.";
					break;
				case ErrorCode.ERR_EXEC_PRINT_VALIDATION:
					str = "Error executing print validation.";
					break;
				case ErrorCode.ERR_EXEC_PRINT_ROLLPAPER:
					str = "Error executing print on roll paper.";
					break;
				case ErrorCode.ERR_EXEC_SCAN_IDCARD:
					str = "Error executing scan ID Card.";
					break;
				case ErrorCode.ERR_EXEC_SCAN_CHECK_ONEBYONE:
					str = "";
					break;
				case ErrorCode.ERR_EXEC_SCAN_CHECK_CONTINUOUS:
					str = "";
					break;
				case ErrorCode.ERR_PRINT_DATA_UNRECEIVE:
					str = "";
					break;
				case ErrorCode.ERR_PRINT_DATA_LENGTH_EXCEED:
					str = "";
					break;
				case ErrorCode.ERR_IMAGE_FILEREMOVE:
					str = "";
					break;
				case ErrorCode.ERR_MSRW_NODATA:
					str = "";
					break;
				case ErrorCode.ERR_NET_CONNECTED:
					str = "";
					break;
				case ErrorCode.ERR_UNLOCKED:
					str = "";
					break;
				case ErrorCode.ERR_BARCODE_NODATA:
					str = "";
					break;
				case ErrorCode.ERR_SCN_IQA:
					str = "";
					break;
				case ErrorCode.ERR_LESS_CHECKS:
					str = "";
					break;
				case ErrorCode.ERR_PAPER_INSERT:
					str = "Please check whether the check is inserted appropriately.";
					break;
				case ErrorCode.ERR_PAPER_EXIST:
					str = "Error Paper Exists.";
					break;
				case ErrorCode.ERR_SCN_COMPRESS:
					str = "";
					break;
				case ErrorCode.ERR_MICR_NOISE:
					str = "Noise in MICR.";
					break;
				case ErrorCode.ERR_MICR_PARSE:
					str = "Unable to parse MICR Data";
					break;
				case ErrorCode.ERR_MICR_BADDATA:
					str = "Bad MICR Data";
					break;
				case ErrorCode.ERR_MICR_NODATA:
					str = "MIC Data could not be detected.";
					break;
				case ErrorCode.ERR_COVER_OPEN:
					str = "The printer cover is open.";
					break;
				case ErrorCode.ERR_PAPER_JAM:
					str = "Printer encountered a Paper Jam, Please clear the paper and try again.";
					break;
				case ErrorCode.ERR_PAPER_PILED:
					str = "Paper piled, Please insert one check a time.";
					break;
				case ErrorCode.ERR_SIZE:
					str = "";
					break;
				case ErrorCode.ERR_DATA_INVALID:
					str = "Invalid Data.";
					break;
				case ErrorCode.ERR_NOT_EXEC:
					str = "";
					break;
				case ErrorCode.ERR_LINE_OVERFLOW:
                    str = "Line overflow occurred during transaction printing";
					break;
				case ErrorCode.ERR_SCAN:
					str = "";
					break;
				case ErrorCode.ERR_MICR:
					str = "";
					break;
				case ErrorCode.ERR_ABORT:
					str = "";
					break;
				case ErrorCode.ERR_THREAD:
					str = "";
					break;
				case ErrorCode.ERR_RESET:
					str = "";
					break;
				case ErrorCode.ERR_SPL_PAUSED:
					str = "";
					break;
				case ErrorCode.ERR_SPL_NOT_EXIST:
					str = "";
					break;
				case ErrorCode.ERR_SS_NOT_EXIST:
					str = "";
					break;
				case ErrorCode.ERR_EXEC_SCAN:
					str = "";
					break;
				case ErrorCode.ERR_EXEC_MICR:
					str = "";
					break;
				case ErrorCode.ERR_EXEC_FUNCTION:
					str = "";
					break;
				case ErrorCode.ERR_PAPERINSERT_TIMEOUT:
                    str = "Failed to insert paper";
					break;
				case ErrorCode.ERR_IMAGE_FILEREAD:
					str = "";
					break;
				case ErrorCode.ERR_WORKAREA_FAILED:
					str = "";
					break;
				case ErrorCode.ERR_WORKAREA_UNKNOWNFORMAT:
					str = "";
					break;
				case ErrorCode.ERR_WORKAREA_NO_MEMORY:
					str = "";
					break;
				case ErrorCode.ERR_IMAGE_FAILED:
					str = "";
					break;
				case ErrorCode.ERR_IMAGE_UNKNOWNFORMAT:
					str = "";
					break;
				case ErrorCode.ERR_IMAGE_FILEOPEN:
					str = "";
					break;
				case ErrorCode.ERR_NOT_FOUND:
					str = "";
					break;
				case ErrorCode.ERR_EXIST:
					str = "";
					break;
				case ErrorCode.ERR_CROPAREAID:
					str = "";
					break;
				case ErrorCode.ERR_ENTRY_OVER:
					str = "";
					break;
				case ErrorCode.ERR_NO_IMAGE:
					str = "";
					break;
				case ErrorCode.ERR_DISK_FULL:
					str = "";
					break;
				case ErrorCode.ERR_ENABLE:
					str = "";
					break;
				case ErrorCode.ERR_REGISTRY:
					str = "";
					break;
				case ErrorCode.ERR_BUFFER_OVER_FLOW:
					str = "";
					break;
				case ErrorCode.ERR_WITHOUT_CB:
					str = "";
					break;
				case ErrorCode.ERR_NOT_EPSON:
					str = "";
					break;
				case ErrorCode.ERR_OFFLINE:
					str = "";
					break;
				case ErrorCode.ERR_NOT_SUPPORT:
					str = "";
					break;
				case ErrorCode.ERR_PARAM:
					str = "Invalid parameter/setting.";
					break;
				case ErrorCode.ERR_ACCESS:
					str = "The printer is not accessible. There could be another scan or printing in progress or the device has been powered off, Try again later.";
					break;
				case ErrorCode.ERR_TIMEOUT:
					str = "Timed out trying to complete the operation. Please try again.";
					break;
				case ErrorCode.ERR_HANDLE:
					str = "";
					break;
				case ErrorCode.ERR_NO_MEMORY:
					str = "";
					break;
				case ErrorCode.ERR_NO_TARGET:
					str = "";
					break;
				case ErrorCode.ERR_NO_PRINTER:
					str = "Printer could not be detected.";
					break;
				case ErrorCode.ERR_OPENED:
					str = "";
					break;
				case ErrorCode.ERR_TYPE:
					str = "";
					break;
			}
			return str;
		}
	}
}
