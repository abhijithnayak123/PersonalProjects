using System;
using System.Diagnostics;
using com.epson.bank.driver;
using System.Threading;

namespace TCF.Zeo.Peripheral.CheckScanner.EpsonTMS9000.Impl
{
	public partial class TMS9000 : TMS9000Base
	{
		private ErrorCode RegisterEvents()
		{
			errResult = ErrorCode.SUCCESS;
			objMFDevice.SCNMICRStatusCallback += new MFDevice.SCNMICRStatusCallbackHandler(ScanStatus);
			errResult = objMFDevice.SCNMICRSetStatusBack();
			return errResult;
		}

		private void ScanStatus(int inTransactionNumber, MainStatus eMainStatus, ErrorCode eSubStatus, string strPortName)
		{
			if (eSubStatus != ErrorCode.SUCCESS)
			{
                Trace.WriteLine("Epson.Epson:Event ScanStatus Failed with " + eSubStatus, DateTime.Now.ToString());
                Thread.Sleep(5 * 1000);
                errResult = objMFDevice.SCNMICRCancelStatusBack();
                objMFDevice.SCNMICRStatusCallback -= new MFDevice.SCNMICRStatusCallbackHandler(ScanStatus);

                if (errResult != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("Cancel ScanStatus Failed " + errResult);
                }
                

                ErrorCode micrResult = RetrieveMicrData(inTransactionNumber);
                if (micrResult != ErrorCode.SUCCESS)
                    EpsonException.SetError(eSubStatus, eMainStatus.ToString());
                else
                    RetrieveImage(inTransactionNumber);
                Trace.WriteLine("Epson.Epson:Event Setting Transaction completed to true ", DateTime.Now.ToString());
				transactionComplete = true;
				return;
			}

			if (eMainStatus == MainStatus.MF_DATARECEIVE_DONE)
			{
				Trace.WriteLine("Epson.Epson:Event ScanStatus Data Receive", DateTime.Now.ToString());
				RetrieveMicrData(inTransactionNumber);
				RetrieveImage(inTransactionNumber);
				//transactionComplete = true;
			}
			if (eMainStatus == MainStatus.MF_FUNCTION_DONE)
			{
				Trace.WriteLine("Epson.Epson:Event ScanStatus Complete", DateTime.Now.ToString());
                errResult = objMFDevice.SCNMICRCancelStatusBack();
                if (errResult != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("Cancel ScanStatus MF_FUNCTION_DONE Failed " + errResult);
                }
                objMFDevice.SCNMICRStatusCallback -= new MFDevice.SCNMICRStatusCallbackHandler(ScanStatus);
                transactionComplete = true;
			}
		}
	}
}
