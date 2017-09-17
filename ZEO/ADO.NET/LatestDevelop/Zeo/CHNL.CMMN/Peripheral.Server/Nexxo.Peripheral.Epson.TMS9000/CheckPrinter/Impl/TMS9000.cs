using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using com.epson.bank.driver;
using MGI.Peripheral.CheckPrinter.Contract;

namespace MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl
{
    public partial class TMS9000 : TMS9000Base
    {
        public bool Print(CheckPrintData printObj)
        {
            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print() Invoked", DateTime.Now.ToString());

            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print SetData", DateTime.Now.ToString());
            SetData(printObj);

            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print Init()", DateTime.Now.ToString());
            if (Init() == false)
                return false;

            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print Open()", DateTime.Now.ToString());
            if (Open() == false)
                return false;

            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print PrintCheck()", DateTime.Now.ToString());
            if (PrintCheck() == false)
            {
                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print Close()", DateTime.Now.ToString());
                Close();
                return false;
            }

            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print Close()", DateTime.Now.ToString());
            if (Close() == false)
                return false;

            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print() Completed", DateTime.Now.ToString());
            return true;
        }

        public bool Init()
        {
            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():Init", DateTime.Now.ToString());
            try
            {
                scanTimer = new EpsonTimer();
                
                deviceOpen = false;

                if ((errResult = InitDevice()) != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():Init Error", DateTime.Now.ToString());
                    EpsonException.SetError(errResult, "Initialization Error!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():Init Exception", DateTime.Now.ToString());
                EpsonException.SetException(ex);
                return false;
            }
            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():Init Completed", DateTime.Now.ToString());
            return true;
        }

        public bool Open()
        {
            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():Open Invoked", DateTime.Now.ToString());
            try
            {

                if ((errResult = OpenDevice()) != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():OpenDevice Failed", DateTime.Now.ToString());
                    EpsonException.SetError(errResult, "Error opening device.");
                    return false;
                }

                if ((errResult = RegisterEvents()) != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():RegisterEvents Failed", DateTime.Now.ToString());
                    EpsonException.SetError(errResult, "Error registering callbacks.");
                    return false;
                }
                if ((errResult = SetScanUnit()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Error setting scan unit.");
                    return false;
                }

                if ((errResult = SetScanPageSettings()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Errror while setting scan parameters");
                    return false;
                }

                if ((errResult = SetMICRSettings()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Error during MICR Settings");
                    return false;
                }

                if ((errResult = SetProcessSettings()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Error setting process parameters");
                    return false;
                }
                if ((errResult = SetCheckPrintSetting()) != ErrorCode.SUCCESS)
                {
                    EpsonException.SetError(errResult, "Error setting print settings");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():Open caught Exception", DateTime.Now.ToString());
                EpsonException.SetException(ex);
                return false;
            }

            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():Open Completed", DateTime.Now.ToString());
            return true;
        }

        public bool PrintCheck()
        {
            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():PrintCheck Invoked", DateTime.Now.ToString());
            try
            {
                /*
                if ((errResult = SetCheckPrintSetting()) != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():SetCheckPrintSetting Failed", DateTime.Now.ToString());
                    EpsonException.SetError(errResult, "Error setting print unit.");
                    return false;
                } 

                if ((errResult = InitiatePrint()) != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():InitiatePrint Failed", DateTime.Now.ToString());
                    EpsonException.SetError(errResult, "Error while trying to initiate print.");
                    return false;
                }
                 * */
                if ((errResult = InitiateScan()) != ErrorCode.SUCCESS)
                {
                    Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():InitiateScan Failed", DateTime.Now.ToString());
                    EpsonException.SetError(errResult, "Error while trying to initiate print.");
                    return false;
                }

                while (true)
                {
                    if (transactionComplete == true)
                    {
                        Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Transaction Completed", DateTime.Now.ToString());
                        break;
                    }
                    if (scanTimer.HasTimedOut() == true)
                    {
                        Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Scanner Timed out", DateTime.Now.ToString());
                        break;
                    }
                    Thread.Sleep(1000 * 1);
                    Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Waiting for trasaction completed to true which is " + transactionComplete, DateTime.Now.ToString());
                }
                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Out of timer loop...", DateTime.Now.ToString());

                if (EpsonException.errorStatus == true)
                {
                    Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Determine the exception :)", DateTime.Now.ToString());
                    return false;
                }

                if (scanTimer.HasTimedOut() == true)
                {
                    errResult = ErrorCode.ERR_TIMEOUT;
                    EpsonException.SetError(errResult, "Error while trying to print check. Timeout encountered.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():PrintCheck Caught Exception", DateTime.Now.ToString());
                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():PrintCheck Caught Exception" + ex.StackTrace, DateTime.Now.ToString());
                EpsonException.SetException(ex);
                return false;
            }

            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl:Print():PrintCheck Completed", DateTime.Now.ToString());
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
