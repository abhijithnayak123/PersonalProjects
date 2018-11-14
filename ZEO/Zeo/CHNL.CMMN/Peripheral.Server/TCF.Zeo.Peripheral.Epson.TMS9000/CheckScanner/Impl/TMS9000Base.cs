using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Configuration;
using com.epson.bank.driver;
using System.Threading;

namespace TCF.Zeo.Peripheral.CheckScanner.EpsonTMS9000.Impl
{
	public class TMS9000Base
	{
		protected MFDevice objMFDevice = null;
		protected MFBase objMFBase = null;
		protected MFScan objMFScanFront = null;
		protected MFScan objMFScanBack = null;
		protected MFMicr objMFMicr = null;
		protected MFProcess objMFProcess = null;
		//public EpsonException epsonException = new EpsonException();

		protected EpsonTimer scanTimer;
		protected bool transactionComplete = false;
		protected bool deviceOpen = false;
		protected ErrorCode errResult = ErrorCode.SUCCESS;
		protected EpsonConfig objConfig = new EpsonConfig();

		//CHECK MICR DATA
		public String accountNumber;
		public String bankNumber;
		public String micrStr;
		public String serialNumber;
        public String amount;
        public String EPC;
        public String transitNumber;
        public String checkType;
        public String countryCode;
        public String onUSField;
        public String auxillatyOnUSField;

		//CHECK IMAGE
		public byte[] checkFrontImage;
		public byte[] checkBackImage;

        //Unique ID
        public string uniqueId;

		protected ErrorCode InitDevice()
		{
			errResult = ErrorCode.SUCCESS;
			EpsonException.Clear();
			transactionComplete = false;
			objMFDevice = new MFDevice();
			objMFBase = new MFBase();
			objMFScanFront = new MFScan();
			objMFScanBack = new MFScan();
			objMFMicr = new MFMicr();
			objMFProcess = new MFProcess();
			return errResult;
		}

		protected ErrorCode SetScanUnit()
		{
			errResult = ErrorCode.SUCCESS;
			errResult = objMFDevice.SCNSelectScanUnit(ScanUnit.EPS_BI_SCN_UNIT_CHECKPAPER);
			if (errResult != ErrorCode.SUCCESS)
				return errResult;
			errResult = objMFDevice.SCNMICRFunctionContinuously(objMFBase, FunctionType.MF_GET_BASE_DEFAULT);
			if (errResult != ErrorCode.SUCCESS)
				return errResult;
			objMFBase.ErrorEject = MfEjectType.MF_EXIT_ERROR_DISCHARGE;
			objMFDevice.SCNMICRFunctionContinuously(objMFBase, FunctionType.MF_SET_BASE_PARAM);

			objMFDevice.SetWaterfallMode(WaterfallMode.WATERFALL_MODE_DISABLE);
			return errResult;
		}

		public ErrorCode SetScanPageSettings()
		{
			errResult = ErrorCode.SUCCESS;
			objMFDevice.SetNumberOfDocuments(1);

			errResult = objMFDevice.SCNMICRFunctionContinuously(objMFScanFront, FunctionType.MF_GET_SCAN_FRONT_DEFAULT);
			if (errResult != ErrorCode.SUCCESS)
				return errResult;
			objMFScanFront.Resolution = objConfig.resolution;
			errResult = objMFDevice.SCNMICRFunctionContinuously(objMFScanFront, FunctionType.MF_SET_SCAN_FRONT_PARAM);
			if (errResult != ErrorCode.SUCCESS)
				return errResult;
			errResult = objMFDevice.SCNSelectScanFace(ScanSide.MF_SCAN_FACE_FRONT);
			if (errResult != ErrorCode.SUCCESS)
				return errResult;

			errResult = objMFDevice.SCNMICRFunctionContinuously(objMFScanBack, FunctionType.MF_GET_SCAN_BACK_DEFAULT);
			if (errResult != ErrorCode.SUCCESS)
				return errResult;
			objMFScanBack.Resolution = objConfig.resolution;
			errResult = objMFDevice.SCNMICRFunctionContinuously(objMFScanBack, FunctionType.MF_SET_SCAN_BACK_PARAM);
			if (errResult != ErrorCode.SUCCESS)
				return errResult;
			errResult = objMFDevice.SCNSelectScanFace(ScanSide.MF_SCAN_FACE_BACK);
			if (errResult != ErrorCode.SUCCESS)
				return errResult;

			errResult = objMFDevice.SCNSetImageTypeOption(objConfig.imageType);
			//errResult = objMFDevice.SCNSetImageQuality(objConfig.colorDepth, 0, objConfig.imageColor, ExOption.EPS_BI_SCN_MANUAL);
			return errResult;
		}

		public ErrorCode SetMICRSettings()
		{
			errResult = ErrorCode.SUCCESS;
			errResult = objMFDevice.SCNMICRFunctionContinuously(objMFMicr, FunctionType.MF_GET_MICR_DEFAULT);
			if (errResult != ErrorCode.SUCCESS)
				return errResult;
			objMFMicr.Font = MfMicrFont.MF_MICR_FONT_E13B;
			objMFMicr.Parsing = true;
			errResult = objMFDevice.SCNMICRFunctionContinuously(objMFMicr, FunctionType.MF_SET_MICR_PARAM);
			if (errResult != ErrorCode.SUCCESS)
				return errResult;
			errResult = objMFDevice.MICRClearSpaces(RemoveSpace.CLEAR_SPACE_ENABLE);
			if (errResult != ErrorCode.SUCCESS)
				return errResult;

			return errResult;
		}

		protected ErrorCode SetProcessSettings()
		{
			errResult = ErrorCode.SUCCESS;
			errResult = objMFDevice.SCNMICRFunctionContinuously(objMFProcess, FunctionType.MF_GET_PROCESS_DEFAULT);
			if (errResult != ErrorCode.SUCCESS)
				return errResult;
			objMFProcess.ActivationMode = MfActivateMode.MF_ACTIVATE_MODE_HIGH_SPEED;

			//TO DO PAPER MISINSERTION, DOUBLE FEED, BAD MICR, MICR MAGNETIC WAVEFORM & NOISE ERROR HERE
			objMFProcess.PaperMisInsertionErrorSelect = MfErrorSelect.MF_ERROR_SELECT_DETECT;
			objMFProcess.PaperMisInsertionCancel = MfCancel.MF_CANCEL_DISABLE;
			objMFProcess.PaperMisInsertionErrorEject = objConfig.errInsertionPocket;
			objMFProcess.NoiseErrorSelect = MfErrorSelect.MF_ERROR_SELECT_NODETECT;
			objMFProcess.NoiseCancel = MfCancel.MF_CANCEL_DISABLE;
			objMFProcess.NoiseErrorEject = objConfig.errInsertionPocket;
			objMFProcess.DoubleFeedErrorSelect = MfErrorSelect.MF_ERROR_SELECT_NODETECT;
			objMFProcess.DoubleFeedCancel = MfCancel.MF_CANCEL_ENABLE;
			objMFProcess.DoubleFeedErrorEject = objConfig.errInsertionPocket;
			objMFProcess.BaddataErrorSelect = MfErrorSelect.MF_ERROR_SELECT_NODETECT;
			objMFProcess.BaddataCancel = MfCancel.MF_CANCEL_ENABLE;
			objMFProcess.BaddataErrorEject = objConfig.errInsertionPocket;
			objMFProcess.BaddataCount = 254;
			objMFProcess.NodataErrorSelect = MfErrorSelect.MF_ERROR_SELECT_NODETECT;
			objMFProcess.NodataCancel = MfCancel.MF_CANCEL_ENABLE;
			objMFProcess.NodataErrorEject = objConfig.errInsertionPocket;

			errResult = objMFDevice.SCNMICRFunctionContinuously(objMFProcess, FunctionType.MF_SET_PROCESS_PARAM);
			return errResult;
		}

		protected ErrorCode OpenDevice()
		{
			errResult = ErrorCode.SUCCESS;
			//Console.WriteLine("Opening Printer \"" + objConfig.epsonPrinterName + "\"");
            Trace.WriteLine("Epson.TMS9000Base.OpenDevice() Opening Printer : " + OpenType.TYPE_PRINTER + " " + objConfig.epsonPrinterName, DateTime.Now.ToString());
			errResult = objMFDevice.OpenMonPrinter(OpenType.TYPE_PRINTER, objConfig.epsonPrinterName);
            if (errResult != ErrorCode.SUCCESS)
            {
                //To resolve race condition -- SV 01/02/2013
                if (errResult != ErrorCode.ERR_OPENED)
                {
                    Trace.WriteLine("Epson.TMS9000Base.OpenDevice() Failed with " + errResult, DateTime.Now.ToString());
                    return errResult;
                }
                else
                {
                    Trace.WriteLine("Epson.TMS9000Base.OpenDevice() Printer is already open", DateTime.Now.ToString());
                    deviceOpen = true;
                    errResult = ErrorCode.SUCCESS;
                }
            }
            Trace.WriteLine("Epson.TMS9000Base.OpenDevice() DeviceOpen has been set to true", DateTime.Now.ToString());
			deviceOpen = true;
			return errResult;
		}

        protected ErrorCode ResetPrinter()
        {
            Trace.WriteLine("Epson.TMS9000Base.ResetPrinter()", DateTime.Now.ToString());
            String resetFile = AppDomain.CurrentDomain.BaseDirectory + "\\reset.log";
            Trace.WriteLine("Epson.TMS9000Base.ResetPrinter() Reset File Path = " + resetFile, DateTime.Now.ToString());
            errResult = ErrorCode.SUCCESS;
            String ihcTime = ConfigurationManager.AppSettings["InkHeadCleanTime"];
            if ( ihcTime == null )
             ihcTime = "22:00";
            Trace.WriteLine("Epson.TMS9000Base.ResetPrinter() Ink Head Clean Time " + ihcTime, DateTime.Now.ToString());
            DateTime lastIHC = Convert.ToDateTime(ihcTime);
            if (lastIHC > DateTime.Now)
                lastIHC = lastIHC.AddDays(-1);
            DateTime nextIHC = lastIHC.AddDays(1);
            //Read Reset.log
            Trace.WriteLine("Epson.TMS9000Base.ResetPrinter() Reading from reset.log", DateTime.Now.ToString());
            DateTime resetTime = Convert.ToDateTime(File.ReadAllText(resetFile));
            if (resetTime > lastIHC && resetTime <= nextIHC)
                Trace.WriteLine("Epson.TMS9000Base.ResetPrinter() reset is not required", DateTime.Now.ToString());
            else
            {
                Trace.WriteLine("Epson.TMS9000Base.ResetPrinter() Reset of Printer is required", DateTime.Now.ToString());
                errResult = objMFDevice.CancelStatusBack();
                if (errResult != ErrorCode.SUCCESS)
                    Trace.WriteLine("CancelStatusBack :" + errResult.ToString(), DateTime.Now.ToString());
                errResult = objMFDevice.CancelInkStatusBack();
                if (errResult != ErrorCode.SUCCESS)
                    Trace.WriteLine("CancelInkStatusBack:" + errResult.ToString(), DateTime.Now.ToString());
                errResult = objMFDevice.CancelError();
                if (errResult != ErrorCode.SUCCESS)
                    Trace.WriteLine("CancelError:" + errResult.ToString(), DateTime.Now.ToString());
                objMFDevice.ResetLastError();
                Thread.Sleep(1000 * 3);
                Trace.WriteLine("Epson.TMS9000Base.ResetPrinter() writing to reset.log current time", DateTime.Now.ToString());
                File.WriteAllText(resetFile, DateTime.Now.ToString());

                //Reopen the printer
                Trace.WriteLine("Epson.TMS9000Base.ResetPrinter() Resetting the printer", DateTime.Now.ToString());
                errResult = objMFDevice.CloseMonPrinter();
                if (errResult != ErrorCode.SUCCESS)
                    Trace.WriteLine("Epson.TMS9000Base.ResetPrinter() Closing Printer returned" + errResult.ToString(), DateTime.Now.ToString());
                errResult = OpenDevice();
                Trace.WriteLine("Epson.TMS9000Base.ResetPrinter() Resetting completed with open returning " + errResult.ToString(), DateTime.Now.ToString());
            }
            
            return errResult;
        }


		protected ErrorCode InitiateScan()
		{
			errResult = ErrorCode.SUCCESS;
            Trace.WriteLine("Epson.TMS9000Base.InitiateScan() Main call to initiate the scan process.", DateTime.Now.ToString());
			errResult = objMFDevice.SCNMICRFunctionContinuously(null, FunctionType.MF_EXEC);
            Trace.WriteLine("Epson.TMS9000Base.InitiateScan() Main call to initiate the scan process completed.", DateTime.Now.ToString());
            return errResult;
		}



		protected bool CheckIntegrity()
		{
			if (checkFrontImage == null)
				return false;
			if (checkFrontImage.Length <= 0)
				return false;
			if (checkBackImage == null)
				return false;
			if (checkBackImage.Length <= 0)
				return false;
            return true;
		}

		protected ErrorCode CloseDevice()
		{
            errResult = objMFDevice.CancelStatusBack();
            if (errResult != ErrorCode.SUCCESS)
            {
                Trace.WriteLine("Epson.TMS9000Base.CloseDevice() Failed to CancelStatusBack()", DateTime.Now.ToString());
            }
            errResult = ErrorCode.SUCCESS;
			//if (objMFDevice.SCNMICRCancelStatusBack() != ErrorCode.SUCCESS)
			//	return errResult;
            if (deviceOpen == true)
            {
                Trace.WriteLine("Epson.TMS9000Base.CloseDevice() Device Open and tryicg to close", DateTime.Now.ToString());
                errResult = objMFDevice.CloseMonPrinter();
                if (errResult != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("Epson.TMS9000Base.CloseDevice() Was not able to Close the device.", DateTime.Now.ToString());

                }
                Trace.WriteLine("Epson.TMS9000Base.CloseDevice() Clearing all objects setting deviceopen to false.", DateTime.Now.ToString());
                deviceOpen = false;
            }
            else
            {
                Trace.WriteLine("Epson.TMS9000Base.CloseDevice()Device is not open to be closed.", DateTime.Now.ToString());
            }
            ClearObjects();
            return errResult;
		}

		private void ClearObjects()
		{
			objMFDevice = null;
			objMFBase = null;
			objMFScanFront = null;
			objMFScanBack = null;
			objMFMicr = null;
			objMFProcess = null;
		}
	}
}
