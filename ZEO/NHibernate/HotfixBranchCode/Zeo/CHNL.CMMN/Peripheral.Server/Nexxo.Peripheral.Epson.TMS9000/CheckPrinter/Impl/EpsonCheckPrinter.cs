using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Configuration;
using MGI.Peripheral.CheckPrinter.Contract;
using MGI.Peripheral.Queue.Impl;


namespace MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl
{
    public class EpsonCheckPrinter : ICheckPrinter
    {
        TMS9000 printer = new TMS9000();

        public CheckPrinterError PrintCheck(CheckPrintData printData)
        {
            try
            {
                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl.EpsonCheckPrinter:PrintCheck() Invoked", DateTime.Now.ToString());
                

                String printMode = ConfigurationManager.AppSettings["CheckPrinterIsSimulator"];

                JobQueue jobQueue = new JobQueue();
                
                jobQueue.SetCheckPrintJob(printData);
                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl.EpsonCheckPrinter:PrintCheck() Completed SetCheckPrintJob()", DateTime.Now.ToString());

                CheckPrintData printInfo = jobQueue.GetCheckPrintJob();

                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl.EpsonCheckPrinter:PrintCheck() Have Print Job to Print Check", DateTime.Now.ToString());
                if (printInfo == null)
                {
                    Trace.WriteLine("??????? CHECK PRINT DATA IS NULLL ?????????", DateTime.Now.ToString());
                }
                else
                {
                    if (printMode == null) printMode = "false";

                    if (printMode == "true")
                    {
                        Trace.WriteLine("Simulator print called......", DateTime.Now.ToString());
                        printer.SimulatorPrint(printInfo);
                    }
                    else
                    {
                        Trace.WriteLine("Printing First Job Check Printer", DateTime.Now.ToString());


                        if (printer.Print(printInfo))
                        {
                            Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl.EpsonCheckPrinter:Print() Has Failed", DateTime.Now.ToString());
                        }

                    }
                }
                jobQueue.SetJobComplete();

                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl.EpsonCheckPrinter:PrintCheck() Completed", DateTime.Now.ToString());

                return Error();
            }
            catch (Exception e)
            {
                Trace.WriteLine("CheckPrinter.Epson.TMS9000.Impl.EpsonCheckPrinter:PrintCheck() Exception Thrown", DateTime.Now.ToString());

                throw e;
            }
        }

        public byte[] GetCheckFrontImage(String format)
        {
            if (format.Equals("jpg"))
                return ConvertToJpeg(printer.checkFrontImage);
            else
                return printer.checkFrontImage;// ConvertToTiff(scanner.checkFrontImage);
        }

        public byte[] GetCheckBackImage(String format)
        {
            if (format.Equals("jpg"))
                return ConvertToJpeg(printer.checkBackImage);
            else
                return printer.checkBackImage;
        }

        public byte[] ConvertToJpeg(byte[] tiffBytes)
        {
            try
            {
                Trace.WriteLine("Converting to JPEG", DateTime.Now.ToString());
                using (MemoryStream inStream = new MemoryStream(tiffBytes))
                using (MemoryStream outStream = new MemoryStream())
                {
                    Trace.WriteLine("Saving Stream as JPEG", DateTime.Now.ToString());
                    System.Drawing.Bitmap.FromStream(inStream).Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return outStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception is = " + ex.Message + " ====" + ex.StackTrace + "====" + ex.InnerException);
            }
            return null;
        }

        private CheckPrinterError Error()
        {
            CheckPrinterError printerErr = new CheckPrinterError()
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
