using System;
using System.Diagnostics;
using com.epson.bank.driver;

namespace MGI.Peripheral.CheckFranking.Epson.TMS9000.Impl
{
	public partial class TMS9000 : TMS9000Base
	{
		private ErrorCode RegisterEvents()
		{
            //register callback function for status update
            errResult = ErrorCode.SUCCESS;

            objMFDevice.SCNMICRStatusCallback += new MFDevice.SCNMICRStatusCallbackHandler(CheckFrankStatusCallback);
            objMFDevice.StartEndorsementStatusCallback += new MFDevice.StartEndorsementStatusCallbackHandler(CheckPrinterReadyCallback);

            errResult = objMFDevice.SCNMICRSetStatusBack();
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            errResult = objMFDevice.StartEndorsementSetStatusBack();
            return errResult;
		}

        public void CheckFrankStatusCallback(int transactionNumber, MainStatus mainStatus, ErrorCode subStatus, string portName)
        {
            if (subStatus != ErrorCode.SUCCESS)
            {
                Trace.WriteLine("MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl.EpsonEvent CheckPrintStatusCallback Failed : " + errResult.ToString(), DateTime.Now.ToString());

                objMFDevice.SCNMICRCancelStatusBack();
                objMFDevice.StartEndorsementCancelStatusBack();
                EpsonException.SetError(subStatus, mainStatus.ToString());

                transactionComplete = true;
                return;
            }
            if (mainStatus == MainStatus.MF_FUNCTION_DONE)
            {
                transactionComplete = true;
            }
        }

        public void CheckPrinterReadyCallback(int TransactionNumber, string portName)
        {
            if (errResult != ErrorCode.SUCCESS)
            {
                Trace.WriteLine("MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl.EpsonEvent CheckPrinterReadyCallback Failed : " + errResult.ToString(), DateTime.Now.ToString());
                return;
            }
            errResult = ErrorCode.SUCCESS;
            errResult = objMFDevice.TemplatePrint(TemplatePrintMode.TEMPLATEPRINT_EXEC);
            if (errResult != ErrorCode.SUCCESS)
            {
                SetException(errResult);
                return;
            }
        }

        private void SetException(ErrorCode errResult)
        {
            Trace.WriteLine("MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl.EpsonEvent CheckPrintStatusCallback Failed : " + errResult.ToString(), DateTime.Now.ToString());

            objMFDevice.SCNMICRCancelStatusBack();
            objMFDevice.StartEndorsementCancelStatusBack();

            EpsonException.SetError(errResult, errResult.ToString());
            transactionComplete = true;
        }
	}
}
