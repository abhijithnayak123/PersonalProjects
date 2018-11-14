using System;
using System.Diagnostics;
using System.Threading;
using com.epson.bank.driver;
using TCF.Zeo.Peripheral.Printer.Contract;

namespace TCF.Zeo.Peripheral.Printer.EpsonTMS9000.Impl
{
	public partial class TMS9000Base
	{
		protected MFDevice objMFDevice = null;
		protected MFBase objMFBase = null;
		MFDeviceFont objFont = new MFDeviceFont();
		protected PrintData objData = null;

		protected bool transactionComplete = false;
		protected bool deviceOpen = false;
		protected ErrorCode errResult = ErrorCode.SUCCESS;

		protected String printSimulator = "false";

		protected ErrorCode InitDevice()
		{
			errResult = ErrorCode.SUCCESS;
			EpsonException.Clear();
			transactionComplete = false;
			objMFDevice = new MFDevice();
			objMFBase = new MFBase();
			return errResult;
		}

		public bool SetObjType(PrintData printObj)
		{
			if (printObj.receiptType == "CashCheckReceipt")
				objData = (CashCheckReceiptData)printObj;
			else if (printObj.receiptType == "CashDrawerReport")
				objData = (CashDrawerReportData)printObj;
			else if (printObj.receiptType.ToLower() == "raw")
				objData = printObj;
			else
			{
				EpsonException.SetError(ErrorCode.ERR_NO_TARGET, "No Print Type Specified");
				return false;
			}
			return true;
		}

		public void RunDiagnostics(out string status, out string deviceStatus, out string printerName, out string serialNumber, out string fwVersion)
		{
			deviceStatus = string.Empty;
			status = string.Empty;
			printerName = string.Empty;
			serialNumber = string.Empty;
			fwVersion = string.Empty;

			InitDevice();
			OpenDevice();

			byte prnID = 65;
			objMFDevice.GetPrnCapability(prnID, out fwVersion);

			string devName;
			prnID = 66;
			objMFDevice.GetPrnCapability(prnID, out devName);

			prnID = 67;
			objMFDevice.GetPrnCapability(prnID, out printerName);
			printerName = devName + " " + printerName;

			prnID = 68;
			objMFDevice.GetPrnCapability(prnID, out serialNumber);

			status = "Connected";

			ASB outData;
			objMFDevice.GetRealStatus(out outData);
			if (outData == ASB.ASB_NO_RESPONSE)
				deviceStatus = "No Response";


            if ((outData & ASB.ASB_COVER_OPEN) == ASB.ASB_COVER_OPEN)
                deviceStatus += "COVER OPEN,";

            if ((outData & ASB.ASB_RECEIPT_NEAR_END) == ASB.ASB_RECEIPT_NEAR_END)
                deviceStatus += "RECEIPT NEAR END,";

            if ((outData & ASB.ASB_RECEIPT_END) == ASB.ASB_RECEIPT_END)
                deviceStatus += "RECEIPT END,";

            if ((outData & ASB.ASB_OFF_LINE) == ASB.ASB_OFF_LINE)
                deviceStatus += "PRINTER OFFLINE,";

            if (objMFDevice.InkStatus == InkASB.INK_ASB_END)
                deviceStatus += "INK CARTRIDGE EMPTY";

            if (objMFDevice.InkStatus == InkASB.INK_ASB_NEAR_END)
                deviceStatus += "REMAINING INK LEVEL LOW";

			CloseDevice();
		}

		protected ErrorCode SetPrintUnit()
		{
			errResult = ErrorCode.SUCCESS;
			errResult = objMFDevice.SetPrintStation(PrintingStation.MF_ST_ROLLPAPER);
			return errResult;
		}

		protected ErrorCode SetPrintSettings()
		{
			errResult = ErrorCode.SUCCESS;
			errResult = objMFDevice.BufferedPrint(PrintBuffer.MF_PRT_BUFFERING);
			errResult = objMFDevice.SetPrintAlignment(Alignment.MF_PRINT_ALIGNMENT_CENTER);
			errResult = objMFDevice.SetPrintSize(60, 23);
			return errResult;
		}

		protected ErrorCode OpenDevice()
		{
			errResult = ErrorCode.SUCCESS;
			//Console.WriteLine("Opening Printer \"" + objConfig.epsonPrinterName + "\"");
			errResult = objMFDevice.OpenMonPrinter(OpenType.TYPE_PRINTER, "TM-S9000U");
			if (errResult != ErrorCode.SUCCESS)
				if (errResult != ErrorCode.ERR_OPENED)
					return errResult;
				else
					errResult = ErrorCode.SUCCESS;
			deviceOpen = true;
			return errResult;
		}

		protected ErrorCode InitiatePrint()
		{
			errResult = ErrorCode.SUCCESS;
			if (objData.receiptType == "CashCheckReceipt")
				errResult = FormatCashCheckPrint();
			else if (objData.receiptType == "CashDrawerReport")
				errResult = FormatCashDrawerReportPrint();
			else if (objData.receiptType == "raw")
				errResult = FormatRawPrint();

			errResult = objMFDevice.BufferedPrint(PrintBuffer.MF_PRT_EXEC);
			errResult = objMFDevice.AutoCutRollPaper(AutoCutType.AUTOCUT_CUT);
			return errResult;
		}

		protected ErrorCode CloseDevice()
		{
			errResult = ErrorCode.SUCCESS;
			if (deviceOpen == true)
			{
                errResult = objMFDevice.CancelStatusBack();
                if (errResult != ErrorCode.SUCCESS)
                    Trace.WriteLine("Epson.TMS9000Base:CloseDevice() CancelStatusBack:" + errResult.ToString(), DateTime.Now.ToString());
                errResult = objMFDevice.CancelInkStatusBack();
                if (errResult != ErrorCode.SUCCESS)
                    Trace.WriteLine("Epson.TMS9000Base:CloseDevice() CancelInkStatusBack:" + errResult.ToString(), DateTime.Now.ToString());
                errResult = objMFDevice.CancelError();
                if (errResult != ErrorCode.SUCCESS)
                    Trace.WriteLine("Epson.TMS9000Base:CloseDevice() CancelError:" + errResult.ToString(), DateTime.Now.ToString());

                objMFDevice.ResetLastError();
                Thread.Sleep(1000 * 3);

				errResult = objMFDevice.CloseMonPrinter();
				ClearObjects();
				deviceOpen = false;
			}
			return errResult;
		}

		private void ClearObjects()
		{
			objMFDevice = null;
			objMFBase = null;
		}
	}
}
