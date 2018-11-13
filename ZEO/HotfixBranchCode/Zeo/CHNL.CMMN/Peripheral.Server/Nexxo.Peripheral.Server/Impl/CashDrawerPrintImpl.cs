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
        public PrintResponse PrintCashDrawerReport(String printparams)
        {
            Trace.WriteLine("--------------- BEGIN CASH DRAWER REPORT PRINT ---------------", DateTime.Now.ToString());
            Trace.WriteLine("PeripheralServiceImpl:PrintCashDrawerReport() Invoked", DateTime.Now.ToString());
            FaultInfo errObject = new FaultInfo();
            PrintResponse printResults = new PrintResponse();
            CashDrawerPrintRequest printParameters = new DeSerialize().GetCashDrawerPrintParams(printparams);
            Trace.WriteLine("Retrieving ContextRegistry:GetContext()", DateTime.Now.ToString());
            IApplicationContext ctx = ContextRegistry.GetContext();
            Trace.WriteLine("ContextRegistry:GetContext() Completed", DateTime.Now.ToString());
            //TMS9000ReceiptPrinter String has to be retrieved from Service Call
            //from DeviceMapper. Need Clarity on how to get this!
            Trace.WriteLine("Instantiating Epson Print Impl", DateTime.Now.ToString());
            IPeripheralPrinter.IPrinter printer = (IPeripheralPrinter.IPrinter)ctx["TMS9000ReceiptPrinter"];
            IPeripheralPrinter.CashDrawerReportData printDataObj = new IPeripheralPrinter.CashDrawerReportData();
            Trace.WriteLine("Epson Print Impl Initiated", DateTime.Now.ToString());

            printDataObj.receiptType = printParameters.ReceiptType;
            printParameters.CashDrawerTellerIDList = ">" + printParameters.CashDrawerTellerIDList.Replace(",", "\r\n>");
            printDataObj.cashDrawerDateTime = printParameters.CashDrawerDateTime;
            printDataObj.cashDrawerTellerID = printParameters.CashDrawerTellerID;
            printDataObj.cashDrawerTellerIDList = printParameters.CashDrawerTellerIDList;
            printDataObj.cashDrawerCashInAmount = printParameters.CashDrawerCashInAmount;
            printDataObj.cashDrawerCashOutAmount = printParameters.CashDrawerCashOutAmount;

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
}
