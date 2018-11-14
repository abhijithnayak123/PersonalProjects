using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Spring.Context.Support;
using Spring.Context;
using System.Collections;
using System.Drawing;
using System.ServiceModel.Web;
using TCF.Zeo.Peripheral.Server.Contract;
using TCF.Zeo.Peripheral.Server.Data;
using TCF.Zeo.Peripheral.Server.JSON.Impl;
using IPeripheralScanner = TCF.Zeo.Peripheral.CheckScanner.Contract;
using IPeripheralPrinter = TCF.Zeo.Peripheral.Printer.Contract;
using System.Web.Script.Serialization;
using System.Drawing.Imaging;

namespace TCF.Zeo.Peripheral.Server.Impl
{
    public partial class PeripheralServiceImpl : ICheckScanner
    {
        public ScanCheckResponse ScanCheck(String scanparams)
        {
            ScanCheckResponse scanResults = new ScanCheckResponse();
            Trace.WriteLine("--------------- BEGIN SCAN CHECK DEBUG VER 1.0------------------", DateTime.Now.ToString());
            Trace.WriteLine("PeripheralServiceImpl:ScanCheck() Invoked", DateTime.Now.ToString());
            FaultInfo errObject = new FaultInfo();

            if (scanparams == null)
                scanparams = "{\"ScanFileType\":\"tiff\", \"SaveRequired\":\"false\"}";
            ScanCheckRequest scanParameters = new DeSerialize().GetScanCheckParams(scanparams);
            if (scanParameters.ScanFileType == null)
                scanParameters.ScanFileType = "tiff";
            if (scanParameters.SaveRequired == null)
                scanParameters.SaveRequired = "false";

            Trace.WriteLine("Retrieving ContextRegistry:GetContext()", DateTime.Now.ToString());
            IApplicationContext ctx = ContextRegistry.GetContext();
            Trace.WriteLine("ContextRegistry:GetContext() Completed", DateTime.Now.ToString());
            //TMS9000CheckScanner String has to be retrieved from Service Call
            //from DeviceMapper. Need Clarity on how to get this!


            //REDIRECT CODE BEGIN - SV 02/01/2015
            PeripheralServiceImpl redirectImpl = new PeripheralServiceImpl();
            Trace.WriteLine("Check for ScanCheck Redirect", DateTime.Now.ToString());
            int redirectRequired = redirectImpl.CheckForRedirect();
            if (redirectRequired == 1)
            {
                Trace.WriteLine("Redirecting the request", DateTime.Now.ToString());
                Object retObj = redirectImpl.RedirectRequest("scanparams=" + scanparams, "ScanCheck");
                if (retObj.GetType() == typeof(FaultInfo))
                    throw new WebFaultException<FaultInfo>((FaultInfo)retObj, System.Net.HttpStatusCode.InternalServerError);
                return (ScanCheckResponse)retObj;
            }
            else if ( redirectRequired == -1)
            {
                Trace.WriteLine("Failed to redirect the request", DateTime.Now.ToString());
                errObject.ErrorMessage = "Failed to redirect the request to the printer.";
                errObject.ErrorDetails = "Could not connect to the redirected Peripheral Server";
                errObject.ErrorNo = 1100;
                throw new WebFaultException<FaultInfo>((FaultInfo)errObject, System.Net.HttpStatusCode.InternalServerError);
                return scanResults;
            }
            //REDIRECT CODE END - SV 02/01/2015

            Trace.WriteLine("Instantiating Epson Scanner Impl", DateTime.Now.ToString());
            IPeripheralScanner.ICheckScanner scanner = (IPeripheralScanner.ICheckScanner)ctx["TMS9000CheckScanner"];
            Trace.WriteLine("Epson Scanner Impl Initiated", DateTime.Now.ToString());

            Trace.WriteLine("Calling Epson ScanCheck Impl", DateTime.Now.ToString());
            //IPeripheralScanner.CheckScannerError errScan = scanner.ScanCheck(scanParameters.ScanFileType, scanParameters.SaveRequired);
            //TIFF Will be default format to save on size
            IPeripheralScanner.CheckScannerError errScan = scanner.ScanCheck("tiff", scanParameters.SaveRequired);
            Trace.WriteLine("Epson ScanCheck Impl Completed", DateTime.Now.ToString());

            if (errScan.errorStatus == false)
            {
                Trace.WriteLine("Epson ScanCheck Retrieving Params", DateTime.Now.ToString());
                scanResults.MicrError = errScan.micrError;
                scanResults.Micr = scanner.GetMicr();
                scanResults.MicrAccountNumber = scanner.GetMicrAccountNumber();
                
                scanResults.Scan_FrontImageJPG = Convert.ToBase64String(scanner.GetCheckFrontImage("jpg"));
                scanResults.Scan_BackImageJPG = Convert.ToBase64String(scanner.GetCheckBackImage("jpg"));
                scanResults.Scan_FrontImageTIFF = Convert.ToBase64String(scanner.GetCheckFrontImage("tiff"));
                scanResults.Scan_BackImageTIFF = Convert.ToBase64String(scanner.GetCheckBackImage("tiff"));
                scanResults.Scan_FrontImage = scanResults.Scan_FrontImageTIFF;
                scanResults.Scan_BackImage = scanResults.Scan_BackImageTIFF;
                
                scanResults.UniqueId = scanner.GetUniqueId();
                scanResults.MicrAmount = scanner.GetMicrAmount();
                scanResults.MicrEPC = scanner.GetMicrECP();
                scanResults.MicrTransitNumber = scanner.GetMicrTransitNumber();
                scanResults.MicrCheckType = scanner.GetMicrCheckType();
                scanResults.MicrCountryCode = scanner.GetMicrCountryCode();
                scanResults.MicrOnUSField = scanner.GetMicrOnUSField();
                scanResults.MicrAuxillatyOnUSField = scanner.GetMicrAuxillatyOnUSField();
                LogCompression(scanner.GetCheckFrontImage("tiff") ,"Front");
                LogCompression(scanner.GetCheckFrontImage("tiff"), "Back");

                if (scanResults.MicrError == 1)
                    scanResults.MicrErrorMessage = errScan.errorMessage;
            }
            else
            {
                Trace.WriteLine("Scan returned with Error " + errScan.errorMessage + "," + errScan.stackTrace, DateTime.Now.ToString());
                errObject.ErrorNo = errScan.errorCode;
                errObject.ErrorMessage = errScan.errorMessage;
                errObject.ErrorDetails = errScan.errorDescription;
                throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
            }
            Trace.WriteLine("Scan Completed with Success", DateTime.Now.ToString());
            
            return scanResults;
    }

        private static void LogCompression(byte[] imgBytes, string imageType)
        {
            MemoryStream strMemory = new MemoryStream(imgBytes);
            Image returnImage = Image.FromStream(strMemory);
            int compressionTagIndex = Array.IndexOf(returnImage.PropertyIdList, 0x103);
            PropertyItem compressionTag = returnImage.PropertyItems[compressionTagIndex];
            int compressionType = BitConverter.ToInt16(compressionTag.Value, 0);
            Trace.WriteLine(imageType + " Compression Type : " + compressionType, DateTime.Now.ToString());
        }

        
        public Stream GetImage(String imageId)
        {
            Trace.WriteLine("--------------- BEGIN GET IMAGE DEBUG ------------------", DateTime.Now.ToString());
            Trace.WriteLine("Get Image() Params: " + imageId, DateTime.Now.ToString());
            FaultInfo errObject = new FaultInfo();

            //REDIRECT CODE BEGIN - SV 02/01/2015
            PeripheralServiceImpl redirectImpl = new PeripheralServiceImpl();
            Trace.WriteLine("Check for GetImage Redirect", DateTime.Now.ToString());
            int redirectRequired = redirectImpl.CheckForRedirect();
            if (redirectRequired == 1)
            {
                Trace.WriteLine("Redirecting the request", DateTime.Now.ToString());
                Object retObj = redirectImpl.RedirectRequest("id=" + imageId, "GetImage");
                if (retObj.GetType() == typeof(FaultInfo))
                    throw new WebFaultException<FaultInfo>((FaultInfo)retObj, System.Net.HttpStatusCode.InternalServerError);
                return (Stream)retObj;
            }
            else if (redirectRequired == -1)
            {
                Trace.WriteLine("Failed to redirect the request", DateTime.Now.ToString());
                errObject.ErrorMessage = "Failed to redirect the request to the printer.";
                errObject.ErrorDetails = "Could not connect to the redirected Peripheral Server";
                errObject.ErrorNo = 1100;
                throw new WebFaultException<FaultInfo>((FaultInfo)errObject, System.Net.HttpStatusCode.InternalServerError);
                return null;
            }
            //REDIRECT CODE END - SV 02/01/2015

            
            IApplicationContext ctx = ContextRegistry.GetContext();
            Trace.WriteLine("ContextRegistry:GetContext() Completed", DateTime.Now.ToString());
            IPeripheralScanner.ICheckScanner scanner = (IPeripheralScanner.ICheckScanner)ctx["TMS9000CheckScanner"];
            Stream imageStream = (Stream)scanner.GetImage(imageId);
            Trace.WriteLine("--------------- END GET IMAGE DEBUG ------------------", DateTime.Now.ToString());
            WebOperationContext.Current.OutgoingResponse.ContentType = "image/jpeg";
            return imageStream; //tmp
        }

        public String ConvertStream(String streamparams)
        {
            Trace.WriteLine("---------------+ BEGIN CONVERT STREAM DEBUG + ------------------", DateTime.Now.ToString());
            Trace.WriteLine("PeripheralServiceImpl:ConvertStream() Invoked", DateTime.Now.ToString());
            FaultInfo errObject = new FaultInfo();
            IApplicationContext ctx = ContextRegistry.GetContext();
            Trace.WriteLine("ContextRegistry:GetContext() Completed", DateTime.Now.ToString());
            IPeripheralScanner.ICheckScanner scanner = (IPeripheralScanner.ICheckScanner)ctx["TMS9000CheckScanner"];
            if ( streamparams== null)
                Trace.WriteLine("NULL Paramaters", DateTime.Now.ToString());
            else
                Trace.WriteLine(streamparams, DateTime.Now.ToString());

            Trace.WriteLine("ContextRegistry:Deserializing Now", DateTime.Now.ToString());
            ConvertStreamRequest streamParameters = new DeSerialize().GetConvertStreamParams(streamparams);
            Trace.WriteLine("JSon Serializer Completed", DateTime.Now.ToString());
            Trace.WriteLine(streamParameters.CheckFrontImage, DateTime.Now.ToString());
            Trace.WriteLine(streamParameters.CheckBackImage, DateTime.Now.ToString());
            String uniqueId = scanner.ConvertStream(streamParameters.CheckFrontImage, streamParameters.CheckBackImage);
            Trace.WriteLine("--------------- END CONVERT STREAM DEBUG ------------------", DateTime.Now.ToString());
            return uniqueId;
        }
        /*
        public int BuildStream(String streamparams)
        {
            //Trace.WriteLine(" REQUEST ARRIVED " + streamparams, DateTime.Now.ToString());
            StreamData.AddStream(streamparams);
            return 1;
           
            for (int i = 0; i < StreamData.myList.Count; i++)
            {
                Trace.WriteLine(StreamData.myList[i], DateTime.Now.ToString());
            }
             
        }
    */
 
    }

    /*
    public static class StreamData
    {
        public static ArrayList myList = new ArrayList();
        private static Object thisLock = new Object();

        public static void AddStream(String stream)
        {
            lock (thisLock)
            {
                if (stream[0] == 'E')
                    StreamData.PrepareStream(stream.Substring(1, 17));
                else if (stream[0] == 'A')
                    StreamData.myList.Add(stream);
            }
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
            Trace.WriteLine("FINAL STREAM : " + finalStr, DateTime.Now.ToString());
            for (int i = 0; i < StreamData.myList.Count; i++)
            {
                if (myList[i].ToString().Substring(1, 17) == id)
                {
                    myList.RemoveAt(i);
                }
            }

            return finalStr;
        }
    }
     * */
}
