using System;
using System.Diagnostics;
using com.epson.bank.driver;
using System.Threading;

namespace MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl
{
	public partial class TMS9000 : TMS9000Base
	{
		private ErrorCode RegisterEvents()
		{
            //register callback function for status update
            errResult = ErrorCode.SUCCESS;

            objMFDevice.SCNMICRStatusCallback += new MFDevice.SCNMICRStatusCallbackHandler(ScanStatus);

            errResult = objMFDevice.SCNMICRSetStatusBack();

            if (errResult != ErrorCode.SUCCESS)
            {
                return errResult;
            }

            //register Call back function when printer is reay to print
            errResult = ErrorCode.SUCCESS;

            objMFDevice.StartEndorsementStatusCallback += new MFDevice.StartEndorsementStatusCallbackHandler(CheckPrinterReadyCallback);
            
            errResult = objMFDevice.StartEndorsementSetStatusBack();

            if (errResult != ErrorCode.SUCCESS)
            {
                return errResult;
            }

            errResult = ErrorCode.SUCCESS;
            objMFDevice.EndEndorsementStatusCallback += new MFDevice.EndEndorsementStatusCallbackHandler(EndEndorseStatus);
            errResult = objMFDevice.EndEndorsementSetStatusBack();
            if (errResult != ErrorCode.SUCCESS)
            {
                return errResult;
            }

            return errResult;
		}

        public void EndEndorseStatus(int TransactionNumber, string portName)
        {
            errResult = objMFDevice.EndEndorsementCancelStatusBack();
            if (errResult != ErrorCode.SUCCESS)
            {
                Trace.WriteLine("Epson.Epson:CheckPrinterReadCllback Faile to Cancel EnndorsementCancellation " + errResult, DateTime.Now.ToString());
                return;
            }
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

                Trace.WriteLine("Epson.Epson:Event.RetrieveImage()", DateTime.Now.ToString());
                RetrieveImage(inTransactionNumber);
                Trace.WriteLine("Epson.Epson:Event Setting Transaction completed to true ", DateTime.Now.ToString());
                transactionComplete = true;
                return;
            }

            if (eMainStatus == MainStatus.MF_DATARECEIVE_DONE)
            {
                Trace.WriteLine("Epson.Epson:Event ScanStatus Data Receive", DateTime.Now.ToString());
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


        public void CheckPrintStatusCallback(int transactionNumber, MainStatus mainStatus, ErrorCode subStatus, string portName)
        {
            if (subStatus != ErrorCode.SUCCESS)
            {
                Trace.WriteLine("MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl.EpsonEvent CheckPrintStatusCallback Failed : " + errResult.ToString(), DateTime.Now.ToString());
                Thread.Sleep(5 * 1000);

                errResult = objMFDevice.StartEndorsementCancelStatusBack();
                if (errResult != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("Cancel ScanEndorsementStatus Failed " + errResult);
                }

                objMFDevice.StartEndorsementStatusCallback -= new MFDevice.StartEndorsementStatusCallbackHandler(CheckPrinterReadyCallback);

                errResult = objMFDevice.SCNMICRCancelStatusBack();
                if (errResult != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("Cancel ScanMICRStatus Failed " + errResult);
                }
                objMFDevice.SCNMICRStatusCallback -= new MFDevice.SCNMICRStatusCallbackHandler(CheckPrintStatusCallback);

                Trace.WriteLine("Epson.Epson:Event Setting Transaction completed to true ", DateTime.Now.ToString());
                //Wait till we complete reading the image data
                //transactionComplete = true;
                return;
            }
            if (mainStatus == MainStatus.MF_FUNCTION_DONE)
            {
                Trace.WriteLine("Epson.Epson:Event ScanStatus MF_FUNCTION DONE", DateTime.Now.ToString());
                errResult = objMFDevice.StartEndorsementCancelStatusBack();
                if (errResult != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("Cancel ScanStatus Failed " + errResult);
                }
                Trace.WriteLine("Epson.Epson:Event ScanStatus Negating Endorsement", DateTime.Now.ToString());
                objMFDevice.StartEndorsementStatusCallback -= new MFDevice.StartEndorsementStatusCallbackHandler(CheckPrinterReadyCallback);

                Trace.WriteLine("Epson.Epson:Event ScanStatus Cancelling MICR Statusback", DateTime.Now.ToString());
                errResult = objMFDevice.SCNMICRCancelStatusBack();
                if (errResult != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("Cancel ScanMICRStatus Failed " + errResult);
                }
                Trace.WriteLine("Epson.Epson:Event ScanStatus Negating MICR Statusback", DateTime.Now.ToString());
                objMFDevice.SCNMICRStatusCallback -= new MFDevice.SCNMICRStatusCallbackHandler(CheckPrintStatusCallback);
                Trace.WriteLine("Epson.Epson:Event ScanStatus Complete Setting to true for a successfull exit", DateTime.Now.ToString());
                transactionComplete = true;
            }
        }

        public void CheckPrinterReadyCallback(int TransactionNumber, string portName)
        {
            errResult = objMFDevice.StartEndorsementCancelStatusBack();
            if (errResult != ErrorCode.SUCCESS)
            {
                Trace.WriteLine("Epson.Epson:CheckPrinterReadCllback Faile to Cancel EnndorsementCancellation " + errResult, DateTime.Now.ToString());
                return;
            }
            errResult = ErrorCode.SUCCESS;

            errResult = SetBufferedPrint();

            if (errResult != ErrorCode.SUCCESS)
            {
                Trace.WriteLine("Epson.Epson:CheckPrinterReadCllback exception " + errResult, DateTime.Now.ToString());
                SetException(errResult);

                return;
            }
            errResult = ErrorCode.SUCCESS;

            errResult = AddPrintDataToBuffer(TransactionNumber);

            if (errResult != ErrorCode.SUCCESS)
            {
                Trace.WriteLine("Epson.Epson:CheckPrinterReadCllback Add Print data exception " + errResult, DateTime.Now.ToString());
                SetException(errResult);
                return;
            }
            errResult = PrintBufferedData();

            if (errResult != ErrorCode.SUCCESS)
            {
                Trace.WriteLine("Epson.Epson:CheckPrinterReadCllback Print Buffered Data exception " + errResult, DateTime.Now.ToString());
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
