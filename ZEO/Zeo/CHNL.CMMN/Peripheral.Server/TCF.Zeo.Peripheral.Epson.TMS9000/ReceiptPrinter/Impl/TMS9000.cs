using System;
using System.Diagnostics;
using com.epson.bank.driver;
using TCF.Zeo.Peripheral.Printer.Contract;

namespace TCF.Zeo.Peripheral.Printer.EpsonTMS9000.Impl
{
	public partial class TMS9000 : TMS9000Base
	{
		public bool Print(PrintData printObj)
		{
			Trace.WriteLine("Epson.TMS9000.Impl:Print() Invoked", DateTime.Now.ToString());
			if (SetObjType(printObj) == false)
				return false;

			if (Init() == false)
				return false;

			if (Open() == false)
				return false;

			if (PrintRoll() == false)
			{
				Close();
				return false;
			}

			if (Close() == false)
				return false;

			Trace.WriteLine("Epson.TMS9000.Impl:Print() Completed", DateTime.Now.ToString());
			return true;
		}

		public bool Init()
		{
			Trace.WriteLine("Epson.TMS9000.Impl:Print():Init", DateTime.Now.ToString());
			try
			{
				deviceOpen = false;

				if ((errResult = InitDevice()) != ErrorCode.SUCCESS)
				{
					Trace.WriteLine("Epson.TMS9000.Impl:Print():Init Error", DateTime.Now.ToString());
					EpsonException.SetError(errResult, "Initialization Error!");
					return false;
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Epson.TMS9000.Impl:Print():Init Exception", DateTime.Now.ToString());
				EpsonException.SetException(ex);
				return false;
			}
			Trace.WriteLine("Epson.TMS9000.Impl:Print():Init Completed", DateTime.Now.ToString());
			return true;
		}

		public bool Open()
		{
			Trace.WriteLine("Epson.TMS9000.Impl:Print():Open Invoked", DateTime.Now.ToString());
			try
			{

				if ((errResult = OpenDevice()) != ErrorCode.SUCCESS)
				{
					Trace.WriteLine("Epson.TMS9000.Impl:Print():OpenDevice Failed", DateTime.Now.ToString());
					EpsonException.SetError(errResult, "Error opening device.");
					return false;
				}


				if ((errResult = RegisterEvents()) != ErrorCode.SUCCESS)
				{
					Trace.WriteLine("Epson.TMS9000.Impl:Print():RegisterEvents Failed", DateTime.Now.ToString());
					EpsonException.SetError(errResult, "Error registering callbacks.");
					return false;
				}

				if ((errResult = SetPrintUnit()) != ErrorCode.SUCCESS)
				{
					Trace.WriteLine("Epson.TMS9000.Impl:Print():SetPrintUnit Failed", DateTime.Now.ToString());
					EpsonException.SetError(errResult, "Error setting print unit.");
					return false;
				}

				if ((errResult = SetPrintSettings()) != ErrorCode.SUCCESS)
				{
					Trace.WriteLine("Epson.TMS9000.Impl:Print():SetPrintSettings Failed", DateTime.Now.ToString());
					EpsonException.SetError(errResult, "Errror while setting print parameters");
					return false;
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Epson.TMS9000.Impl:Print():Open caught Exception", DateTime.Now.ToString());
				EpsonException.SetException(ex);
				return false;
			}

			Trace.WriteLine("Epson.TMS9000.Impl:Print():Open Completed", DateTime.Now.ToString());
			return true;
		}

		public bool PrintRoll()
		{
			Trace.WriteLine("Epson.TMS9000.Impl:Print():PrintRoll Invoked", DateTime.Now.ToString());
			try
			{
				if ((errResult = InitiatePrint()) != ErrorCode.SUCCESS)
				{
					Trace.WriteLine("Epson.TMS9000.Impl:Print():InitiatePrint Failed", DateTime.Now.ToString());
					EpsonException.SetError(errResult, "Error while trying to print receipt.");
					return false;
				}


				if (EpsonException.errorStatus == true)
				{
					Trace.WriteLine("Epson.TMS9000.Impl:Print():InitiatePrint Error Check Failed", DateTime.Now.ToString());
					return false;
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine("Epson.TMS9000.Impl:Print():InitiatePrint Caught Exception", DateTime.Now.ToString());
				EpsonException.SetException(ex);
				return false;
			}

			Trace.WriteLine("Epson.TMS9000.Impl:Print():InitiatePrint Completed", DateTime.Now.ToString());
			return true;
		}

		public bool Close()
		{
			try
			{
				if ((errResult = CloseDevice()) != ErrorCode.SUCCESS)
					return false;
			}
			catch (Exception ex)
			{
				EpsonException.SetException(ex);
				return false;
			}
			return true;
		}
	}
}
