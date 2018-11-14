using com.epson.bank.driver;

namespace TCF.Zeo.Peripheral.Printer.EpsonTMS9000.Impl
{
	public partial class TMS9000 : TMS9000Base
	{
		private ErrorCode RegisterEvents()
		{
			errResult = ErrorCode.SUCCESS;
			objMFDevice.StatusCallback += new MFDevice.StatusCallbackHandler(PrintStatus);
			errResult = objMFDevice.SetStatusBack();
			return errResult;
		}

		public void PrintStatus(ASB status)
		{
			if (status == ASB.ASB_PRINT_SUCCESS)
			{
				transactionComplete = true;
			}
			//Can Ignore Sensor Error for Printng
		}
	}
}
