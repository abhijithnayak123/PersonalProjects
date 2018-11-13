using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using MGI.Peripheral.CheckFranking.Contract;
using MGI.Peripheral.Queue.Impl;


namespace MGI.Peripheral.CheckFranking.Epson.TMS9000.Impl
{
    public class EpsonCheckFranking : ICheckFranking
    {
        public CheckFrankingError FrankCheck(CheckFrankData printData)
        {
            try
            {
                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl.EpsonCheckFranking:FrankCheck() Invoked", DateTime.Now.ToString());
                
                TMS9000 printer = new TMS9000();
                JobQueue jobQueue = new JobQueue();                
                jobQueue.SetCheckFrankJob(printData);

                CheckFrankData printInfo = jobQueue.GetCheckFrankJob();
                if (printInfo == null)
                    Trace.WriteLine("??????? CHECK FRANK DATA IS NULLL ?????????", DateTime.Now.ToString());
                else
                {
                        printer.Frank(printInfo);
                }
                jobQueue.SetJobComplete();

                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl.EpsonCheckFranking:FrankCheck() Completed", DateTime.Now.ToString());

                return Error();
            }
            catch (Exception e)
            {
                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl.EpsonCheckFranking:FrankCheck() Exception Thrown", DateTime.Now.ToString());

                throw e;
            }
        }

        private CheckFrankingError Error()
        {
            CheckFrankingError printerErr = new CheckFrankingError()
            {
                errorStatus = EpsonException.errorStatus,
                errorCode = EpsonException.errorCode,
                errorMessage = EpsonException.errorMessage,
                errorDescription = EpsonException.errorDescription,
                stackTrace = EpsonException.stackTrace
            };
            return printerErr;
        }
    }
}
