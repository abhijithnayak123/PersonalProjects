using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using MGI.Peripheral.Printer.Contract;
using MGI.Peripheral.Queue.Impl;

namespace MGI.Peripheral.Printer.EpsonTMS9000.Impl
{
    public class EpsonPrinter : IPrinter
    {
        String npsVersion = "x.x.x";
        public PrinterError PrintDocument(PrintData printData)
        {
            Trace.WriteLine("Epson.TMS9000.Impl:PrintDocument() Invoked", DateTime.Now.ToString());
            EpsonException.Clear();

            JobQueue jobQueue = new JobQueue();
            jobQueue.SetPrintJob(printData);
            Trace.WriteLine("Epson.TMS9000.Impl:PrintDocument() Waiting for Print Queue", DateTime.Now.ToString());
            PrintData printInfo = jobQueue.GetPrintJob();
            Trace.WriteLine("Epson.TMS9000.Impl:PrintDocument() Freed from Print Queue", DateTime.Now.ToString());

            String isPrintDirAvailable = AppDomain.CurrentDomain.BaseDirectory + "Temp\\";
            if (isPrintDirAvailable.Equals(String.Empty))
            {
                EpsonException.SetUserException(1002, "Print Save Directory Not Defined", "Invalid Print Save Directory. Please verify configuration file", "");
            }
            else if (printData.ReceiptData.Equals(String.Empty))
            {
                EpsonException.SetUserException(1003, "Empty Print Data", "Print Data is Empty.", "");
            }
            else
            {
                try
                {
                    for (int i = 0; i < printData.ReceiptData.Count - 1; i++)
                    {
                        Trace.WriteLine("Epson.TMS9000.Impl:WindowsPrint() Processing Receipt " + (i + 1) + " of " + printData.ReceiptData.Count, DateTime.Now.ToString());
                        Trace.WriteLine("Epson.TMS9000.Impl:WindowsPrint() Receipt Data Length is " + printData.ReceiptData[i].Length, DateTime.Now.ToString());
                        DocXCreator docObj = new DocXCreator();
                        Trace.WriteLine("Epson.TMS9000.Impl:Prepare Document() Initiated", DateTime.Now.ToString());
                        String docFile = docObj.PrepareDocument(printData.ReceiptData[i]);

                        if (File.Exists(docFile))
                        {
                            String printMode = ConfigurationManager.AppSettings["PrinterIsSimulator"];
                            if (printMode == "false")
                            {
                                FileInfo fInfo = new FileInfo(docFile);
                                if (fInfo.Length > 0)
                                {
                                    Trace.WriteLine("Epson.TMS9000.Impl:DoWinPrint() Initiated", DateTime.Now.ToString());
                                    WinPrint.DoWinPrint(docFile);
                                    Trace.WriteLine("Epson.TMS9000.Impl:DoWinPrint() File Deleted", DateTime.Now.ToString());
                                }
                                else
                                {
                                    Trace.WriteLine("Epson.TMS9000.Impl:WinPrint File Length 0! " + i, DateTime.Now.ToString());
                                    EpsonException.SetUserException(1006, "Print File Length Check Failed", "Print File Length check failed.", "");
                                }
                                File.Delete(docFile);
                            }
                            else
                            {
                                Trace.WriteLine("Print Simulator File is stored in " + docFile, DateTime.Now.ToString());
                            }
                        }
                        else
                        {
                            Trace.WriteLine("Epson.TMS9000.Impl:WinPrint No File Found! " + i, DateTime.Now.ToString());
                            EpsonException.SetUserException(1004, "Print File Creation Failed", "Print File creation failed.", "");
                        }
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Epson.TMS9000.Impl:PrintDocument() Exception Thrown " + e.StackTrace, DateTime.Now.ToString());
                    EpsonException.SetUserException(1021, "Print File creation failed.", "Exception occured during spool. Could not initiate application for printing.", e.StackTrace);
                }
            }
            jobQueue.SetJobComplete();
            //Error Code Log
            Trace.WriteLine("Epson.TMS9000.Impl:PrintDocument() Error Code " + EpsonException.errorCode, DateTime.Now.ToString());
            Trace.WriteLine("Epson.TMS9000.Impl:PrintDocument() Error Desc " + EpsonException.errorDescription, DateTime.Now.ToString());
            Trace.WriteLine("Epson.TMS9000.Impl:PrintDocument() Error Msg " + EpsonException.errorMessage, DateTime.Now.ToString());
            Trace.WriteLine("Epson.TMS9000.Impl:PrintDocument() Error Status " + EpsonException.errorStatus, DateTime.Now.ToString());
            Trace.WriteLine("Epson.TMS9000.Impl:PrintDocument() Stack Trace " + EpsonException.stackTrace, DateTime.Now.ToString());
            Trace.WriteLine("Epson.TMS9000.Impl:PrintDocument() Completed", DateTime.Now.ToString());
            return Error();
        }


        public PrinterError Print(PrintData printData)
        {
            try
            {
                Trace.WriteLine("Epson.TMS9000.Impl:Print() Invoked", DateTime.Now.ToString());
                TMS9000 printer = new TMS9000();
                String printMode = ConfigurationManager.AppSettings["PrinterIsSimulator"];

                JobQueue jobQueue = new JobQueue();
                jobQueue.SetPrintJob(printData);

                PrintData printInfo = jobQueue.GetPrintJob();
                if (printInfo == null)
                {
                    Trace.WriteLine("??????? PRINTDATA IS NULLL ?????????", DateTime.Now.ToString());
                }
                else
                {
                    if (printMode == null) printMode = "false";
                    if (printMode == "true")
                    {
                        printer.PerformSimulatorPrint(printInfo);
                    }
                    else
                    {
                        Trace.WriteLine("Printing First Job......", DateTime.Now.ToString());
                        printer.Print(printInfo);

                    }
                }

                jobQueue.SetJobComplete();
                Trace.WriteLine("Epson.TMS9000.Impl:Print() Completed", DateTime.Now.ToString());

                return Error();
            }
            catch (Exception e)
            {
                Trace.WriteLine("Epson.TMS9000.Impl:Print() Exception Thrown", DateTime.Now.ToString());
                throw e;
            }
        }

        private PrinterError Error()
        {
            PrinterError printerErr = new PrinterError()
            {
                errorStatus = EpsonException.errorStatus,
                errorCode = EpsonException.errorCode,
                errorMessage = EpsonException.errorMessage,
                errorDescription = EpsonException.errorDescription,
                stackTrace = EpsonException.stackTrace
            };
            return printerErr;
        }

        public PrinterError RunDiagnostics()
        {
            try
            {
                Trace.WriteLine("Epson.TMS9000.Impl:RunDiagnostics() Invoked", DateTime.Now.ToString());

                //Test Print Check for Ink Status
                TMS9000 printer = new TMS9000();
                //JobQueue jobQueue = new JobQueue();
                PrintData printData = new PrintData();

                PrinterError err = Error();
                string status, deviceStatus, printerName, serialNumber, fwVersion;
                printer.RunDiagnostics(out status, out deviceStatus, out printerName, out serialNumber, out fwVersion);
                err.diag_deviceName = printerName;
                err.diag_deviceStatus = deviceStatus;
                deviceStatus += err.inkStatus;
                err.diag_firmwareVersion = fwVersion;
                err.diag_status = status;
                err.diag_serialNumber = serialNumber;

                //Commenting below since this has to be printed as a doc template
                /*
				jobQueue.SetPrintJob(printData);
				PrintData printInfo = jobQueue.GetPrintJob();


				printInfo.receiptType = "raw";
				printInfo.ReceiptData.Add(".c..h10..b.Printer Diagnostics\r");
				printInfo.ReceiptData.Add(".c..h10..b.-------------------------------------------------------------\r");
				printInfo.ReceiptData.Add(".h10.Printer Name : " + printerName + "\r");
				printInfo.ReceiptData.Add(".h10.Device Status : " + deviceStatus + "\r");
				printInfo.ReceiptData.Add(".h10.Firmware Version : " + fwVersion + "\r");
				printInfo.ReceiptData.Add(".h10.Serial Number : " + serialNumber + "\r");
				printInfo.ReceiptData.Add(".c..h10..b.-------------------------------------------------------------\r");
				printInfo.ReceiptData.Add(".c.\r");
				printInfo.ReceiptData.Add(".c.\r");
				printer.Print(printInfo);
				jobQueue.SetJobComplete();
              */

                //Get Printer Info since there was no exception

                if (err.errorCode == -30)
                    err.diag_status = "Not connected";

                //Retrieve NPS Version from version.txt
                try
                {
                    npsVersion = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\" + "version.txt");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Epson.TMS9000.Impl:RunDiagnostics() Failed to get version info from version.txt missing file? " + ex.Message, DateTime.Now.ToString()); ;
                }

				String ipData = "{NPSVersion}|" + npsVersion + "|{Date}|" + DateTime.Now.ToString() + "|{DeviceName}|" + printerName + "|{DeviceStatus}|" + deviceStatus + "|{SerialNumber}|" + serialNumber + "|{FirmwareRevision}|" + fwVersion + "|{PrinterStatus}|" + status + "|{data}|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";
                ipData += "\\";
                String[] printList = ipData.Split('\\');
                printData.ReceiptData = printList.ToList();
                PrintDocument(printData);

                Trace.WriteLine("Epson.TMS9000.Impl:RunDiagnostics() Completed", DateTime.Now.ToString());
                return err;
            }
            catch (Exception e)
            {
                Trace.WriteLine("Epson.TMS9000.Impl:RunDiagnostics() Exception Thrown", DateTime.Now.ToString());
                throw e;
            }
        }

    }
}
