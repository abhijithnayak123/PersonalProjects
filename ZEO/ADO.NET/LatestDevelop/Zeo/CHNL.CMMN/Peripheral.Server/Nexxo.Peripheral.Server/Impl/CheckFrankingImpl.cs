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
using IPeripheralPrinter = MGI.Peripheral.CheckFranking.Contract;

namespace MGI.Peripheral.Server.Impl
{
    public partial class PeripheralServiceImpl : ICheckFranking
    {
        public PrintResponse FrankCheck(string frankparams)
        {
            Trace.WriteLine("-------------- FRANK CHECK BEGIN ----------------", DateTime.Now.ToString());
            PrintResponse printResults = new PrintResponse();
            FaultInfo errObject = new FaultInfo();

            if (frankparams == null)
            {
                Trace.WriteLine("WARNING: NULL Frank Parameters!", DateTime.Now.ToString());
                frankparams = String.Empty;
            }

            //REDIRECT CODE BEGIN - SV 02/01/2015
            PeripheralServiceImpl redirectImpl = new PeripheralServiceImpl();
            Trace.WriteLine("Check for FRANK Redirect", DateTime.Now.ToString());
            int redirectRequired = redirectImpl.CheckForRedirect();
            if (redirectRequired == 1)
            {
                Object retObj = redirectImpl.RedirectRequest("frankparams=" + frankparams, "FrankCheck");
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


            //frankparams = "frankdata:SynCon 43678|Width:85|Height:10|FontFace:ArialNarrow|FontSize:12|FontType:Normal|XPos:0|YPos:0|Orientation:Horizontal";
            //frankparams = "{\"frankdata\":\"SynCon CAN8745\",\"Width\":\"65\", \"Height\":\"6\", \"FontFace\":\"Arial Narrow\", \"FontSize\":\"12\", \"FontType\":\"Regular\",\"XPos\":\"0\", \"YPos\":\"0\"}";
            //frankparams = "{\"FrankData\":\"0000\", \"Orientation\":\"0\", \"FontFace\":\"Arial\", \"FontType\":\"Bold\", \"FontSize\":\"12\", \"XPos\":\"50\", \"YPos\":\"50\",\"Width\":\"30\",\"Height\":\"45\"}";
            //CheckFrankRequest scanParameters = new DeSerialize().GetFrankCheckParams(frankparams);

            //Suresh - 06/12/2014 Frank Parmetrization has changed
            CheckFrankRequest scanParameters = new CheckFrankRequest();
            //parse incoming request as "frankdata:SynCon CAN8745|Width:85|Height:20|FontFace:Verdana|FontSize:16|FontType:Bold|XPos:50|YPos:20|Orientation:Horizontal"
            //Initialize default values
            scanParameters.FrankData = String.Empty;
            scanParameters.Width = 85; scanParameters.Height = 20;
            scanParameters.FontFace = "Verdana"; scanParameters.FontSize = 12; scanParameters.FontType = "Bold";
            scanParameters.XPos = 50; scanParameters.YPos = 20; scanParameters.Orientation = "Horizontal";

            string[] frankList = frankparams.Split('|');
            for (int i = 0; i < frankList.Count(); i++)
            {
                string[] paramList = frankList[i].Split(':');
                if (paramList.Count() == 2 )
                {
                    if (paramList[0].Equals("frankdata"))
                        scanParameters.FrankData = paramList[1];
                    else if (paramList[0].Equals("Width"))
                        scanParameters.Width = Convert.ToInt32(paramList[1]);
                    else if (paramList[0].Equals("Height"))
                        scanParameters.Height = Convert.ToInt32(paramList[1]);
                    else if (paramList[0].Equals("FontFace"))
                        scanParameters.FontFace = paramList[1];
                    else if (paramList[0].Equals("FontSize"))
                        scanParameters.FontSize = Convert.ToInt32(paramList[1]);
                    else if (paramList[0].Equals("FontType"))
                        scanParameters.FontType = paramList[1];
                    else if (paramList[0].Equals("XPos"))
                        scanParameters.XPos = Convert.ToInt32(paramList[1]);
                    else if (paramList[0].Equals("YPos"))
                        scanParameters.YPos = Convert.ToInt32(paramList[1]);
                    else if (paramList[0].Equals("Orientation"))
                        scanParameters.Orientation = paramList[1];
                }
            }

            Trace.WriteLine("Retrieving ContextRegistry:GetContext()", DateTime.Now.ToString());
            IApplicationContext ctx = ContextRegistry.GetContext();
            Trace.WriteLine("ContextRegistry:GetContext() Completed", DateTime.Now.ToString());
            //TMS9000CheckScanner String has to be retrieved from Service Call
            //from DeviceMapper. Need Clarity on how to get this!
            Trace.WriteLine("Instantiating Epson Scanner Impl", DateTime.Now.ToString());
            IPeripheralPrinter.ICheckFranking scanner = (IPeripheralPrinter.ICheckFranking)ctx["TMS9000CheckFranking"];
            Trace.WriteLine("Epson Scanner Impl Initiated", DateTime.Now.ToString());

            IPeripheralPrinter.CheckFrankData frankDataObj = new IPeripheralPrinter.CheckFrankData();
            frankDataObj.FrankData = scanParameters.FrankData;
            frankDataObj.Height = scanParameters.Height;
            frankDataObj.Orientation = scanParameters.Orientation;
            frankDataObj.Width = scanParameters.Width;
            frankDataObj.XPos = scanParameters.XPos;
            frankDataObj.YPos = scanParameters.YPos;
            frankDataObj.FontFace = scanParameters.FontFace;
            frankDataObj.FontType = scanParameters.FontType;
            frankDataObj.FontSize = scanParameters.FontSize;
            IPeripheralPrinter.CheckFrankingError errPrint = scanner.FrankCheck(frankDataObj);
            if (errPrint.errorStatus == true)
            {
                Trace.WriteLine("Check Frank returned with Error " + errPrint.errorMessage + "," + errPrint.errorMessage, DateTime.Now.ToString());
                errObject.ErrorNo = errPrint.errorCode;
                errObject.ErrorMessage = errPrint.errorMessage;
                errObject.ErrorDetails = errPrint.errorDescription;
                printResults.Result = "Failed";
                throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
            }
            else
            {
                printResults.Result = "Success";
                printResults.UniqueId = System.Guid.NewGuid();
            }
            Trace.WriteLine("Check Frank Completed with Success", DateTime.Now.ToString());
            return printResults;
        }
    }
}
    