using System;
using com.epson.bank.driver;
using System.Diagnostics;
using MGI.Peripheral.CheckPrinter.Contract;

namespace MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl
{
    public partial class TMS9000Base
    {
        protected const string TMS_PRINTER_NAME = "TM-S9000U";
        
        protected MFDevice objMFDevice = null;
        protected MFBase objMFBase = null;

        protected CheckPrintData objData = null;
        protected byte numberOfPrints = 1;
        protected String printSimulator = "false";

        protected EpsonTimer scanTimer;
        protected bool transactionComplete = false;
        protected bool deviceOpen = false;

        //Scan Related Vars
        public byte[] checkFrontImage;
        public byte[] checkBackImage;

        protected MFScan objMFScanFront = null;
        protected MFScan objMFScanBack = null;
        protected MFMicr objMFMicr = null;
        protected MFProcess objMFProcess = null;

        
        protected ErrorCode errResult = ErrorCode.SUCCESS;
        
        protected ErrorCode InitDevice()
        {
            errResult = ErrorCode.SUCCESS;
            EpsonException.Clear();
            transactionComplete = false;
            objMFDevice = new MFDevice();
            objMFBase = new MFBase();
            objMFMicr = new MFMicr();
            objMFProcess = new MFProcess();
            objMFScanFront = new MFScan();
            objMFScanBack = new MFScan();
            return errResult;
        }

        public void SetData(CheckPrintData printObj)
        {
            objData = printObj;
        }

        protected ErrorCode SetCheckPrintSetting()
        {
            errResult = ErrorCode.SUCCESS;

            errResult = objMFDevice.SetPrintCutSheetSettings(numberOfPrints, ScanEnable.BI_PRINTCUTSHEET_NOSCAN);

            if (errResult != ErrorCode.SUCCESS)
            {
                return errResult;
            }
            errResult = ErrorCode.SUCCESS;

            errResult = objMFDevice.PrintCutSheet(objMFBase, FunctionType.MF_SET_BASE_PARAM);

            if (errResult != ErrorCode.SUCCESS)
            {
                return errResult;
            }            
            errResult = ErrorCode.SUCCESS;

            errResult = objMFDevice.SetPrintStation(PrintingStation.MF_ST_PHYSICAL_ENDORSEMENT);
                        
            return errResult;
        }
        
        protected ErrorCode OpenDevice()
        {
            errResult = ErrorCode.SUCCESS;
            errResult = objMFDevice.OpenMonPrinter(OpenType.TYPE_PRINTER, TMS_PRINTER_NAME);
            if (errResult != ErrorCode.SUCCESS)
                if (errResult != ErrorCode.ERR_OPENED)
                    return errResult;
                else
                    errResult = ErrorCode.SUCCESS;
            deviceOpen = true;
            return errResult;
        }

        protected ErrorCode CloseDevice()
        {
            errResult = ErrorCode.SUCCESS;
            //errResult = objMFDevice.SCNMICRCancelStatusBack();
            //if (errResult != ErrorCode.SUCCESS)
            //    return errResult;

            //errResult = ErrorCode.SUCCESS;
            //errResult = objMFDevice.StartEndorsementCancelStatusBack();
            //if (errResult != ErrorCode.SUCCESS)
            //    return errResult;


            //objMFDevice.SCNMICRCancelFunction(MfEjectType.MF_EJECT_DISCHARGE);
            //objMFDevice.SCNMICRCancelStatusBack();
            //objMFDevice.StartEndorsementCancelStatusBack();
            objMFDevice.CancelStatusBack();
            if (errResult != ErrorCode.SUCCESS)
            {
                Trace.WriteLine("Epson.TMS9000Base.CloseDevice() Failed to CancelStatusBack()", DateTime.Now.ToString());
            }
            errResult = ErrorCode.SUCCESS;
            if (deviceOpen == true)
            {
                Trace.WriteLine("Epson.TMS9000Base.CloseDevice() Device Open and tryicg to close", DateTime.Now.ToString());
                errResult = objMFDevice.CloseMonPrinter();
                if (errResult != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("Epson.TMS9000Base.CloseDevice() Was not able to Close the device.", DateTime.Now.ToString());

                }
                Trace.WriteLine("Epson.TMS9000Base.CloseDevice() Clearing all objects setting deviceopen to false.", DateTime.Now.ToString());
                ClearObjects();
                deviceOpen = false;
            }
            else
            {
                Trace.WriteLine("Epson.TMS9000Base.CloseDevice()Device is not open to be closed.", DateTime.Now.ToString());
            }
            return errResult;
        }

        private void ClearObjects()
        {
            objMFDevice = null;
            objMFBase = null;
            objMFMicr = null;
            objMFProcess = null;
            objMFScanFront = null;
            objMFScanBack = null;
        }

        public ErrorCode SetBufferedPrint()
        {
            errResult = ErrorCode.SUCCESS;
            errResult = objMFDevice.TemplatePrint(TemplatePrintMode.TEMPLATEPRINT_BUFFERING);
            return errResult;
        }

        public ErrorCode PrintBufferedData()
        {
            errResult = ErrorCode.SUCCESS;
            errResult = objMFDevice.TemplatePrint(TemplatePrintMode.TEMPLATEPRINT_EXEC);
            return errResult;
        }

        protected ErrorCode InitiateScan()
        {
            errResult = ErrorCode.SUCCESS;
            Trace.WriteLine("Epson.TMS9000Base.() Main call to initiate the scan process.", DateTime.Now.ToString());
            errResult = objMFDevice.SCNMICRFunctionContinuously(null, FunctionType.MF_EXEC);
            if (errResult != ErrorCode.SUCCESS)
            {
                Trace.WriteLine("Epson.TMS9000Base.InitiateScan() Failed." + errResult, DateTime.Now.ToString());
            }

            Trace.WriteLine("Epson.TMS9000Base.InitiateScan() Main call to initiate the scan process completed.", DateTime.Now.ToString());
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

            //objMFDevice.SetWaterfallMode(WaterfallMode.WATERFALL_MODE_DISABLE);
            return errResult;
        }

        public ErrorCode SetScanPageSettings()
        {
            errResult = ErrorCode.SUCCESS;
            objMFDevice.SetNumberOfDocuments(1);

            errResult = objMFDevice.SCNMICRFunctionContinuously(objMFScanFront, FunctionType.MF_GET_SCAN_FRONT_DEFAULT);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;
            objMFScanFront.Resolution = MfScanDpi.MF_SCAN_DPI_200;
            errResult = objMFDevice.SCNMICRFunctionContinuously(objMFScanFront, FunctionType.MF_SET_SCAN_FRONT_PARAM);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;
            errResult = objMFDevice.SCNSelectScanFace(ScanSide.MF_SCAN_FACE_FRONT);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            errResult = objMFDevice.SCNMICRFunctionContinuously(objMFScanBack, FunctionType.MF_GET_SCAN_BACK_DEFAULT);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;
            objMFScanBack.Resolution = MfScanDpi.MF_SCAN_DPI_200;
            errResult = objMFDevice.SCNMICRFunctionContinuously(objMFScanBack, FunctionType.MF_SET_SCAN_BACK_PARAM);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;
            errResult = objMFDevice.SCNSelectScanFace(ScanSide.MF_SCAN_FACE_BACK);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            errResult = objMFDevice.SCNSetImageTypeOption(ImageTypeOption.EPS_BI_SCN_OPTION_GRAYSCALE);
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

            //Set Process for data witing mode
            objMFProcess.EndorsePrintMode = MfEndorsePrintMode.MF_ENDORSEPRINT_MODE_DATAWAITING;

            //TO DO PAPER MISINSERTION, DOUBLE FEED, BAD MICR, MICR MAGNETIC WAVEFORM & NOISE ERROR HERE


            objMFProcess.PaperMisInsertionErrorSelect = MfErrorSelect.MF_ERROR_SELECT_DETECT;
            objMFProcess.PaperMisInsertionCancel = MfCancel.MF_CANCEL_DISABLE;
            objMFProcess.PaperMisInsertionErrorEject = MfEject.MF_EJECT_MAIN_POCKET;
            objMFProcess.NoiseErrorSelect = MfErrorSelect.MF_ERROR_SELECT_NODETECT;
            objMFProcess.NoiseCancel = MfCancel.MF_CANCEL_DISABLE;
            objMFProcess.NoiseErrorEject = MfEject.MF_EJECT_MAIN_POCKET;
            objMFProcess.DoubleFeedErrorSelect = MfErrorSelect.MF_ERROR_SELECT_NODETECT;
            objMFProcess.DoubleFeedCancel = MfCancel.MF_CANCEL_ENABLE;
            objMFProcess.DoubleFeedErrorEject = MfEject.MF_EJECT_MAIN_POCKET;
            objMFProcess.BaddataErrorSelect = MfErrorSelect.MF_ERROR_SELECT_NODETECT;
            objMFProcess.BaddataCancel = MfCancel.MF_CANCEL_ENABLE;
            objMFProcess.BaddataErrorEject = MfEject.MF_EJECT_MAIN_POCKET;
            objMFProcess.BaddataCount = 254;
            objMFProcess.NodataErrorSelect = MfErrorSelect.MF_ERROR_SELECT_NODETECT;
            objMFProcess.NodataCancel = MfCancel.MF_CANCEL_ENABLE;
            objMFProcess.NodataErrorEject = MfEject.MF_EJECT_MAIN_POCKET;

            errResult = objMFDevice.SCNMICRFunctionContinuously(objMFProcess, FunctionType.MF_SET_PROCESS_PARAM);
            return errResult;
        }

        public ErrorCode InitiatePrint()
        {
            errResult = ErrorCode.SUCCESS;

            errResult = objMFDevice.PrintCutSheet(FunctionType.MF_EXEC);

            return errResult;
        }
    }
}
