using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Management;
using System.Printing;



namespace MGI.Peripheral.Printer.EpsonTMS9000.Impl
{
    public static class WinPrint
    {
        [DllImport("Winspool.drv")]
        private static extern bool SetDefaultPrinter(string printerName);


        public static String InitializePrinter()
        {
            Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter()", DateTime.Now.ToString());
            String availablePrinter = String.Empty;
            string printerName = String.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            foreach (ManagementObject printer in searcher.Get())
            {
                printerName = printer["Name"].ToString();
                Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() Found Printer " + printerName + "[Default:" + printer["Default"] + "]", DateTime.Now.ToString());
                if (printerName.Equals("EPSON TM-S9000 RollPaperEN"))
                {
                    if (!SetDefaultPrinter(printerName))
                        Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() Failed to Set Default Printer EPSON TM-S9000 RollPaperEN", DateTime.Now.ToString());
                    Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() Current Printer Status is = " + printer["PrinterStatus"].ToString(), DateTime.Now.ToString());
                    Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() Current Printer State is = " + printer["DetectedErrorState"].ToString(), DateTime.Now.ToString());
                    if (printer["PrinterStatus"].ToString() == "0" || printer["PrinterStatus"].ToString() == "1" || printer["PrinterStatus"].ToString() == "3" || printer["PrinterStatus"].ToString() == "4")
                    {
                        if (printer["DetectedErrorState"].ToString() == "0")
                        {
                            Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() : " + printerName + " is in Unknown State", DateTime.Now.ToString());
                            availablePrinter = printerName;
                        }
                        else
                        {
                            Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() : " + printerName + " Available Status = " + printer["PrinterStatus"].ToString(), DateTime.Now.ToString());
                            availablePrinter = printerName;
                        }
                    }
                    else
                    {
                        Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() : " + printerName + " Not Available Status = " + printer["PrinterStatus"].ToString(), DateTime.Now.ToString());
                    }
                    if (availablePrinter.Length > 0)
                        return availablePrinter;
                }
                if (printerName.Equals("EPSON TM-S9000 Roll Paper"))
                {
                    if (!SetDefaultPrinter(printerName))
                        Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() Failed to Set Default Printer EPSON TM-S9000 Roll Paper", DateTime.Now.ToString());
                    Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() Current Printer Status is = " + printer["PrinterStatus"].ToString(), DateTime.Now.ToString());
                    Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() Current Printer State is = " + printer["DetectedErrorState"].ToString(), DateTime.Now.ToString());
                    if (printer["PrinterStatus"].ToString() == "0" || printer["PrinterStatus"].ToString() == "1" || printer["PrinterStatus"].ToString() == "3" || printer["PrinterStatus"].ToString() == "4")
                    {
                        if (printer["DetectedErrorState"].ToString() == "0")
                        {
                            Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() : " + printerName + " is in Unknow State", DateTime.Now.ToString());
                            availablePrinter = printerName;
                        }
                        else
                        {
                            Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() : " + printerName + " Available", DateTime.Now.ToString());
                            availablePrinter = printerName;
                        }
                    }
                    else
                    {
                        Trace.WriteLine("Epson.TMS9000.Impl:InitializePrinter() : " + printerName + " Not Available", DateTime.Now.ToString());
                    }
                    return availablePrinter;
                }
            }
            return availablePrinter;
        }

        public static bool IsEpsonRollDefaultPrinter(String availablePrinter)
        {
            Trace.WriteLine("Epson.TMS9000.Impl:IsEpsonRollDefaultPrinter() Check Made for " + availablePrinter, DateTime.Now.ToString());
            string printerName = String.Empty;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            foreach (ManagementObject printer in searcher.Get())
            {
                printerName = printer["Name"].ToString();
                if (printerName.Equals(availablePrinter))
                {
                    if (printer["Default"].ToString().Equals("True"))
                    {
                        Trace.WriteLine("Epson.TMS9000.Impl:IsEpsonRollDefaultPrinter() Default Printer set to " + availablePrinter, DateTime.Now.ToString());
                        return true;
                    }
                    else
                    {
                        Trace.WriteLine("Epson.TMS9000.Impl:IsEpsonRollDefaultPrinter() Could not set default printer to " + availablePrinter, DateTime.Now.ToString());
                        return false;
                    }
                }
            }
            //Not a possibility but lets log
            Trace.WriteLine("Epson.TMS9000.Impl:IsEpsonRollDefaultPrinter() None  Found!", DateTime.Now.ToString());
            return false;
        }

        public static void CheckOnSpooler(String docName)
        {
            int PRINT_TIME_OUT = 10; //10 Seconds
            bool docPrinted = false;
            docName = Path.GetFileName(docName);
            docName = docName.Replace(".nxo", "");
            Trace.WriteLine("Epson.TMS9000.Impl:CheckSpooler() for " + docName, DateTime.Now.ToString());
            //we will wait for ten seconds to make sure that this job is removed from spooler

            try
            {
                for (int i = 0; i < PRINT_TIME_OUT; i++)
                {
                    SelectQuery query = new SelectQuery("Win32_PrintJob");
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                    using (ManagementObjectCollection printJobs = searcher.Get())
                        foreach (ManagementObject printJob in printJobs)
                        {
                            // The format of the Win32_PrintJob.Name property is "PrinterName,JobNumber"
                            string name = (string)printJob["Name"];
                            Trace.WriteLine("Epson.TMS9000.Impl:CheckOnSpooler() : " + " Job Name: " + name, DateTime.Now.ToString());
                            string[] nameParts = name.Split(',');
                            string printerName = nameParts[0];
                            string jobNumber = nameParts[1];
                            string document = (string)printJob["Document"];
                            string jobStatus = (string)printJob["JobStatus"];
                            Trace.WriteLine("Epson.TMS9000.Impl:CheckOnSpooler() : " + " Job Document: " + document, DateTime.Now.ToString());
                            Trace.WriteLine("Epson.TMS9000.Impl:CheckOnSpooler() : " + " Job Status: " + jobStatus, DateTime.Now.ToString());
                            if (document == docName)
                            {
                                Trace.WriteLine("Epson.TMS9000.Impl:CheckOnSpooler() : Found the relevant document waiting to be printed", DateTime.Now.ToString());

                                if (i == 9)
                                {
                                    Trace.WriteLine("Epson.TMS9000.Impl:CheckOnSpooler() : Waited for too much time and this document is not printed", DateTime.Now.ToString());
                                    Trace.WriteLine("Epson.TMS9000.Impl:CheckOnSpooler() : Deleting this print job", DateTime.Now.ToString());
                                    EpsonException.SetUserException(1023, "Timed out while trying to print the receipt. ", "Timeout occured while trying to print the receipt and could not delete current print job.The printer could be in non available state.", "");
                                    try
                                    {
                                        printJob.Delete();
                                        Trace.WriteLine("Epson.TMS9000.Impl:CheckOnSpooler() :Print Job Deleted", DateTime.Now.ToString());
                                    }
                                    catch (Exception ex)
                                    {
                                        Trace.WriteLine("Epson.TMS9000.Impl:CheckOnSpooler() :Print Job Deleted Exception " + ex.StackTrace, DateTime.Now.ToString());
                                        EpsonException.SetUserException(1025, "Timed out while trying to print the receipt. ", "Timeout while trying to print the receipt and could not delete current print job.The printer could be in non avilable state.", ex.StackTrace);
                                    }
                                    return;
                                }
                                else
                                {
                                    if (jobStatus == null)
                                    {
                                        docPrinted = true;
                                    }
                                }
                            }
                            else
                            {
                                Trace.WriteLine("Epson.TMS9000.Impl:CheckOnSpooler() : This document is yet to be spooled or has been printed or removed.", DateTime.Now.ToString());
                            }
                        }// For each Print Job
                    if (docPrinted == true)
                    {
                        Trace.WriteLine("Epson.TMS9000.Impl:CheckOnSpooler() : This job has been printed.", DateTime.Now.ToString());
                        Thread.Sleep(1000 * 1);
                        return;
                    }
                    Thread.Sleep(1000 * 1);
                }//Timer for loop
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Epson.TMS9000.Impl:CheckOnSpooler() :Spooler Exception " + ex.StackTrace, DateTime.Now.ToString());
                EpsonException.SetUserException(1026, "Exception during print. Please check for printer errors. ", "Error occured during a print. Check for printer power, paper jam, cover open or cable disconnect.", ex.StackTrace);
            }

        }

        public static void DoWinPrint(string file)
        {
            Trace.WriteLine("Epson.TMS9000.Impl:DoWinPrint()", DateTime.Now.ToString());
            try
            {
                // Change the default printer to EPSON TM-S9000 RollPaperEN or EPSON TM-S9000 RollPaper
                Trace.WriteLine("Epson.TMS9000.Impl:DoWinPrint() Initializing Printer", DateTime.Now.ToString());
                String availablePrinter = InitializePrinter();
                //Wait for some time and try again
                if (availablePrinter.Equals(""))
                {
                    Thread.Sleep(3 * 1000);
                    availablePrinter = InitializePrinter();
                    if (availablePrinter.Equals(""))
                    {
                        Trace.WriteLine("Epson.TMS9000.Impl:DoWinPrint() : " + " No EPSON Roll Paper Drivers were Detected!", DateTime.Now.ToString());
                        EpsonException.SetUserException(1018, "Initialization of printer failed.", "Printer is not powered or Printer cable is disconnected or Printer is in error state.", "");
                        return;
                    }
                }
                Trace.WriteLine("Epson.TMS9000.Impl:DoWinPrint() Initializing Printer completed", DateTime.Now.ToString());

                Trace.WriteLine("Epson.TMS9000.Impl:DoWinPrint() Get Default Printer", DateTime.Now.ToString());
                if (IsEpsonRollDefaultPrinter(availablePrinter) == false)
                {
                    Trace.WriteLine("Epson.TMS9000.Impl:DoWinPrint() : " + " No EPSON Roll Paper Drivers were default printers!", DateTime.Now.ToString());
                    EpsonException.SetUserException(1019, "Failed to set Epson Roll Printer as default Printer.", "Epson Roll Printer could not be set as default Printer.", "");
                    return;
                }
                Trace.WriteLine("Epson.TMS9000.Impl:DoWinPrint() Default Printer completed", DateTime.Now.ToString());

                //Printer is available and is in ready state, so lets print
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.Refresh();
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.Verb = "Print";
                //process.StartInfo.Arguments = "\"" + "EPSON TM-S9000 Roll Paper" + "\"";
                process.StartInfo.FileName = file;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                Trace.WriteLine("Epson.TMS9000.Impl:DoWinPrint() Start spooling", DateTime.Now.ToString());
                process.WaitForExit(5 * 1000); //5 seconds for spooling to be completed

                //Check the spooler if this document is printed else time out after ten seconds
                Trace.WriteLine("Epson.TMS9000.Impl:DoWinPrint() Spooler Check Initiated ", DateTime.Now.ToString());
                CheckOnSpooler(file);
                //Wait till the print is completed
                Thread.Sleep(1 * 1000);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Epson.TMS9000.Impl:DoWinPrint() Exception Thrown " + ex.StackTrace, DateTime.Now.ToString());
                EpsonException.SetUserException(1020, "Exception occurs during print.", "Exception occurred during spool. Could not initiate application for printing.", ex.StackTrace);
            }
        }
    }
}
