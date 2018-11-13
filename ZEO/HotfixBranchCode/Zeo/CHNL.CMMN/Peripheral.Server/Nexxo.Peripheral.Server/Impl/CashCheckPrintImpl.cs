using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Spring.Context.Support;
using Spring.Context;
using System.Collections;
using System.ServiceModel.Web;
using MGI.Peripheral.Server.Contract;
using MGI.Peripheral.Server.Data;
using MGI.Peripheral.Server.JSON.Impl;
using IPeripheralPrinter = MGI.Peripheral.Printer.Contract;
namespace MGI.Peripheral.Server.Impl
{
    public partial class PeripheralServiceImpl : IPrinter
    {
        public PrintResponse PrintDocStream(String printparams)
        {
            Trace.WriteLine("-------------- BEGIN DOC PRINT ----------------", DateTime.Now.ToString());
            PrintResponse printResults = new PrintResponse();
            FaultInfo errObject = new FaultInfo();
            errObject.ErrorNo = 0;
            //try
            {
                //REDIRECT CODE BEGIN - SV 02/01/2015
                PeripheralServiceImpl redirectImpl = new PeripheralServiceImpl();
                Trace.WriteLine("Check for PRINT Redirect", DateTime.Now.ToString());
                int redirectRequired = redirectImpl.CheckForRedirect();
                if (redirectRequired == 1)
                {
                    //printparams = System.Net.WebUtility.HtmlDecode(printparams);
                    printparams = printparams.Replace(' ', '+');
                    Object retObj = redirectImpl.RedirectRequest("printparams=" + printparams, "PrintDocStream");
                    if (retObj.GetType() == typeof(FaultInfo))
                        throw new WebFaultException<FaultInfo>((FaultInfo)retObj, System.Net.HttpStatusCode.InternalServerError);
                    return (PrintResponse)retObj;
                }
                else if (redirectRequired == -1)
                {
                    Trace.WriteLine("Failed to redirect the request", DateTime.Now.ToString());
                    errObject.ErrorMessage = "Failed to redirect the request to the printer.";
                    errObject.ErrorDetails = "Could not connect to the redirected Peripheral Server";
                    errObject.ErrorNo = 1100;
                    throw new WebFaultException<FaultInfo>((FaultInfo)errObject, System.Net.HttpStatusCode.InternalServerError);
                    return printResults;
                }
                //REDIRECT CODE END - SV 02/01/2015

                

                String printData = String.Empty;
                if ((printData = StreamData.AddStream(printparams)) == "")
                     return printResults;
              
                Trace.WriteLine("-------------- COMPLETE STREAM RECEIVED PROCESSING DOC PRINT ----------------", DateTime.Now.ToString());
                Trace.WriteLine("PeripheralServiceImpl:PrintDocXReceipt() Invoked", DateTime.Now.ToString());

                Trace.WriteLine("Retrieving ContextRegistry:GetContext()", DateTime.Now.ToString());
                IApplicationContext ctx = ContextRegistry.GetContext();
                Trace.WriteLine("ContextRegistry:GetContext() Completed", DateTime.Now.ToString());
                Trace.WriteLine("Instantiating Epson Print Impl", DateTime.Now.ToString());


                PrintRequest printRequest = new PrintRequest();
                printRequest.ReceiptType = "docx";
                printRequest.ReceiptData = printparams;
                IPeripheralPrinter.IPrinter printer = (IPeripheralPrinter.IPrinter)ctx["TMS9000ReceiptPrinter"];
                IPeripheralPrinter.CashCheckReceiptData printDataObj = new IPeripheralPrinter.CashCheckReceiptData();
                Trace.WriteLine("Doc Print Impl Initiated", DateTime.Now.ToString());
                printDataObj.receiptType = "docx";

                Trace.WriteLine("Calling Doc Print Impl", DateTime.Now.ToString());
                printData.TrimEnd('\\');
                String[] printList = printData.Split('\\');
                IPeripheralPrinter.PrinterError errPrint;
                printDataObj.ReceiptData = printList.ToList();
                errPrint = printer.PrintDocument(printDataObj);

                Trace.WriteLine("Windows Print Impl Completed", DateTime.Now.ToString());

                if (errPrint.errorStatus == true)
                {
                    Trace.WriteLine("Print returned with Error Code:" + errPrint.errorCode + ",Message:" + errPrint.errorMessage + ",Description:" + errPrint.errorDescription, DateTime.Now.ToString());
                    printResults.Result = "Error occured";
                    errObject.ErrorNo = errPrint.errorCode;
                    errObject.ErrorMessage = errPrint.errorMessage;
                    errObject.ErrorDetails = errPrint.errorDescription;
                    throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
                }
                Trace.WriteLine("Print Completed with Success", DateTime.Now.ToString());
                printResults.Result = "Success";
                return printResults;
            }
                /*
            catch (Exception ex)
            {
                errObject.ErrorNo = -1;
                errObject.ErrorMessage = "Exception occured during receipt print.";
                errObject.ErrorDetails = ex.StackTrace;
                throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
            }
                 * */
        }

        public PrintResponse PrintStream(String printparams)
        {
            Trace.WriteLine("-------------- PRINT STREAM BEGIN ----------------", DateTime.Now.ToString());
            PrintResponse printResults = new PrintResponse();

            String printData = String.Empty;
            if ( (printData = StreamData.AddStream(printparams)) != "" )
            {
                printData.TrimEnd('\\');
                String[] printList = printData.Split('\\');
                for (int i = 0; i < printList.Length; i++)
                    printResults = PrintCashCheckReceipt(printList[i]);
            }
            return printResults;
        }

        public PrintResponse PrintCashCheckReceipt(String printparams)
        {
            Trace.WriteLine("-------------- BEGIN CASH CHECK RECEIPT PRINT ----------------", DateTime.Now.ToString());

            printparams = printparams.Replace("\"", "");
            Trace.WriteLine("PeripheralServiceImpl:PrintCashCheckReceipt() Invoked", DateTime.Now.ToString());
            FaultInfo errObject = new FaultInfo();
            PrintResponse printResults = new PrintResponse();
            Trace.WriteLine("PeripheralServiceImpl:PrintCashCheckReceipt() Decode from Base64", DateTime.Now.ToString());
            byte[] decodedByt = Convert.FromBase64String(printparams);
            String decodedStr = System.Text.Encoding.UTF8.GetString(decodedByt);

            //CashCheckPrintRequest printParameters = new DeSerialize().GetCashCheckPrintParams(printparams);
            Trace.WriteLine("Retrieving ContextRegistry:GetContext()", DateTime.Now.ToString());
            IApplicationContext ctx = ContextRegistry.GetContext();
            Trace.WriteLine("ContextRegistry:GetContext() Completed", DateTime.Now.ToString());
            //TMS9000ReceiptPrinter String has to be retrieved from Service Call
            //from DeviceMapper. Need Clarity on how to get this!
            Trace.WriteLine("Instantiating Epson Print Impl", DateTime.Now.ToString());
            PrintRequest printRequest = new PrintRequest();
            printRequest.ReceiptType = "raw";
            printRequest.ReceiptData = decodedStr;
            IPeripheralPrinter.IPrinter printer = (IPeripheralPrinter.IPrinter)ctx["TMS9000ReceiptPrinter"];
            IPeripheralPrinter.CashCheckReceiptData printDataObj = new IPeripheralPrinter.CashCheckReceiptData();
            Trace.WriteLine("Epson Print Impl Initiated", DateTime.Now.ToString());
            printDataObj.ReceiptData = decodedStr.Split('\t').ToList<String>();
            printDataObj.receiptType = "raw";

            /*
            printDataObj.receiptType = printParameters.ReceiptType;
            if (printDataObj.receiptType == "CashCheckReceipt")
            {
                if (printParameters.LogoImage != null)
                    printDataObj.logo = Convert.FromBase64String(printParameters.LogoImage);
                printDataObj.kioskID = printParameters.KioskName;
                printDataObj.approverAuthority = printParameters.ChannelPartnerName;
                printDataObj.kioskAddress = printParameters.Location;
                printDataObj.transactionDateTime = printParameters.TransactionDateValue;
                printDataObj.checkAmount = printParameters.CheckAmountValue;
                printDataObj.feeAmount = printParameters.FeeAmount;
                printDataObj.totalAmount = printParameters.TotalAmount;
                printDataObj.cardBalance = printParameters.NexxoCardBalanceValue;
                printDataObj.cardNumber = printParameters.CardNoValue;
                printDataObj.cardHolderName = printParameters.CardHolderValue;
                printDataObj.receiptNumber = printParameters.ReceiptNoValue;
            }
            if (printDataObj.receiptType.ToLower() == "raw")
            {
                printDataObj.ReceiptData = printParameters.ReceiptData.Split('\t').ToList<String>();
            }
             * */
            Trace.WriteLine("Calling Epson Print Impl", DateTime.Now.ToString());
            IPeripheralPrinter.PrinterError errPrint = printer.Print(printDataObj);
            Trace.WriteLine("Epson Print Impl Completed", DateTime.Now.ToString());

            if (errPrint.errorStatus == true)
            {
                Trace.WriteLine("Print returned with Error " + errPrint.errorMessage + "," + errPrint.errorMessage, DateTime.Now.ToString());
                printResults.Result = "Error occured";
                errObject.ErrorNo = errPrint.errorCode;
                errObject.ErrorMessage = errPrint.errorMessage;
                errObject.ErrorDetails = errPrint.errorDescription;
                throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
            }
            Trace.WriteLine("Print Completed with Success", DateTime.Now.ToString());
            printResults.Result = "Success";
            return printResults;
        }
    }

    public static class StreamData
    {
        public static ArrayList myList = new ArrayList();
        private static Object thisLock = new Object();

        public static String AddStream(String stream)
        {
            String finalStream = String.Empty;
            lock (thisLock)
            {
                if (stream[0] == 'E')
                {
                    finalStream =  StreamData.PrepareStream(stream.Substring(1, 17));
                    Trace.WriteLine("FINAL STREAM " + finalStream.Length + " " + stream.Substring(1, 17), DateTime.Now.ToString());
                    return finalStream;
                }
                else if (stream[0] == 'A')
                {
                    Trace.WriteLine("ADD STREAM " + stream.Length + " " + stream.Substring(1, 17), DateTime.Now.ToString());
                    Trace.Write(stream);
                    StreamData.myList.Add(stream);
                }
            }
            return finalStream;
        }
        public static String PrepareStream(string id)
        {
            //Iterate through the array to determine all strings with id and put them in a list
            String finalStr = String.Empty;
            for (int i = 0; i < StreamData.myList.Count; i++)
            {
                if (myList[i].ToString().Substring(1, 17) == id)
                {
                    finalStr += StreamData.myList[i].ToString().Substring(18);
                }
            }
            
            for (int i = 0; i < StreamData.myList.Count; i++)
            {
                if (myList[i].ToString().Substring(1, 17) == id)
                {
                    Trace.WriteLine("REMOVE STREAM SUCCEEDED ", DateTime.Now.ToString());
                    myList.RemoveAt(i);
                }
            }

            return finalStr;
        }
    }

}
