using System;
using System.Diagnostics;
using com.epson.bank.driver;
using TCF.Zeo.Peripheral.CheckFranking.Contract;

namespace TCF.Zeo.Peripheral.CheckFranking.Epson.TMS9000.Impl
{
    public partial class TMS9000Base
    {
        protected const string TMS_PRINTER_NAME = "TM-S9000U";
        
        protected MFDevice objMFDevice = null;
        protected MFBase objMFBase = null;

        protected CheckFrankData objData = null;
        protected byte numberOfPrints = 1;
        protected String printSimulator = "false";

        protected EpsonTimer scanTimer;
        protected bool transactionComplete = false;
        protected bool deviceOpen = false;
        
        protected ErrorCode errResult = ErrorCode.SUCCESS;
        
        protected ErrorCode InitDevice()
        {
            errResult = ErrorCode.SUCCESS;
            EpsonException.Clear();
            transactionComplete = false;
            objMFDevice = new MFDevice();
            objMFBase = new MFBase();
            return errResult;
        }

        public void SetData(CheckFrankData printObj)
        {
            objData = printObj;
        }

        protected ErrorCode SetCheckFrankSetting()
        {
            errResult = ErrorCode.SUCCESS;
            errResult = objMFDevice.SCNSelectScanUnit(ScanUnit.EPS_BI_SCN_UNIT_CHECKPAPER);
            if (errResult != ErrorCode.SUCCESS) return errResult;
            errResult = objMFDevice.SetPrintCutSheetSettings(1, ScanEnable.BI_PRINTCUTSHEET_NOSCAN);
            if (errResult != ErrorCode.SUCCESS) return errResult;
            errResult = objMFDevice.PrintCutSheet(objMFBase, FunctionType.MF_SET_BASE_PARAM);
            if (errResult != ErrorCode.SUCCESS) return errResult;
            errResult = objMFDevice.SetPrintStation(PrintingStation.MF_ST_FEEDER);
            if (errResult != ErrorCode.SUCCESS) return errResult;
            errResult = objMFDevice.TemplatePrint(TemplatePrintMode.TEMPLATEPRINT_BUFFERING);
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
            errResult = objMFDevice.SCNMICRCancelStatusBack();
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            errResult = ErrorCode.SUCCESS;
            errResult = objMFDevice.StartEndorsementCancelStatusBack();
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            errResult = ErrorCode.SUCCESS;
            if (deviceOpen == true)
            {
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


        public ErrorCode PrintBufferedData()
        {
            if (objData.FontFace == null) objData.FontFace = "Arial";
            if (objData.FontType == null) objData.FontType = "Regular";
            if (objData.FontSize == 0) objData.FontSize = 12;
            if (objData.Orientation == null) objData.Orientation = "Horizontal";
            if (objData.Width == 0) objData.Width = 25;
            if (objData.Height == 0) objData.Height = 5;
            Trace.WriteLine("Franking Parameters: XPos=" + objData.XPos + ",YPos=" + objData.YPos +",Width=" + objData.Width + ",Height=" + objData.Height, DateTime.Now.ToString());
            MFPrintAreaInfo printInfo = new MFPrintAreaInfo();
            printInfo.AreaName = "checkfrank";
            printInfo.OriginX = objData.XPos;
            printInfo.OriginY = objData.YPos;
            printInfo.Width = objData.Width;
            printInfo.Height = objData.Height;
            printInfo.Rotate = ImageRotate.IMAGEROTATE_0;
            if ( objData.Orientation.Equals("Vertical") )
                printInfo.Rotate = ImageRotate.IMAGEROTATE_90;
            printInfo.Measure = ImageMeasure.MEASURE_MM;


            errResult = objMFDevice.SetTemplatePrintArea(AreaSelectMode.SELECTPRINTAREA_DIRECT, "", printInfo, 0x40000000);
            if (errResult != ErrorCode.SUCCESS)  return errResult;

            System.Drawing.FontStyle fontStyle = new System.Drawing.FontStyle();
            if (objData.FontType.Equals("Bold")) fontStyle = System.Drawing.FontStyle.Bold;
            else if (objData.FontType.Equals("Regular")) fontStyle = System.Drawing.FontStyle.Regular;
            else if (objData.FontType.Equals("Italic")) fontStyle = System.Drawing.FontStyle.Italic;
            else fontStyle = System.Drawing.FontStyle.Regular;

            System.Drawing.Font font = new System.Drawing.Font(objData.FontFace, objData.FontSize, fontStyle);
            MFTrueType fontType = new MFTrueType(font);
            errResult = objMFDevice.PrintText(objData.FrankData, fontType);
            return errResult;
        }

        public ErrorCode InitiatePrint()
        {
            errResult = ErrorCode.SUCCESS;
            errResult = PrintBufferedData();
            if (errResult != ErrorCode.SUCCESS) return errResult;
            errResult = objMFDevice.PrintCutSheet(FunctionType.MF_EXEC);
            return errResult;
        }
    }
}
