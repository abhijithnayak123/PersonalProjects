using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using Spring.Context.Support;
using Spring.Context;
using System.Collections;
using System.ServiceModel.Web;
using MGI.Peripheral.Server.Contract;
using MGI.Peripheral.Server.Data;
using MGI.Peripheral.Server.JSON.Impl;
using IPeripheralPrinter = MGI.Peripheral.Printer.Contract;
using System.Net;
using System.Net.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Security.Cryptography.X509Certificates;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace MGI.Peripheral.Server.Impl
{
	public partial class PeripheralServiceImpl : IPrinter
	{
        RedirectResponse redirectRes = new RedirectResponse();
        FaultInfo errObject = new FaultInfo();

        private const int EXCEPTION_CODE = 1000;
        private const string EXCEPTION_MESSAGE = "Exception occurs during redirect.";
        private const string REDIRECT_FILE = "redirect.txt";
        private const int REDIRECT_DEL_CODE = 1001;
        private const string REDIRECT_DEL_FAILED = "Failed to delete redirect file";
        private const int REDIRECT_ADD_CODE = 1002;
        private const string REDIRECT_ADD_FAILED = "Failed to make entry in redirect file";
        private const int REDIRECT_INVALID_CODE = 1003;
        private const string REDIRECT_INVALID_FAILED = "Invalid host specified";

        private const int REDIRECT_CYCLIC_CODE = 1004;
        private const string REDIRECT_CYCLIC_FAILED = "Cyclic Redirection Error";
        private const string REDIRECT_CYCLIC_MESSAGE = "Redirect host has an entry pointing to the same source.";

		public RedirectResponse SetRedirectHost(string redirectHost)
		{
			Trace.WriteLine("--------------- BEGIN SET REDIRECT HOST ---------------", DateTime.Now.ToString());

            try
            {
                Trace.WriteLine("Redirect Host being set to " + redirectHost, DateTime.Now.ToString());

                if (CheckInputParam(redirectHost) == false)
                {
                    MakeErrEntry(REDIRECT_INVALID_CODE,REDIRECT_INVALID_FAILED,REDIRECT_INVALID_FAILED );
                    throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
                }
                //Check if redirect is to the same system
                string hostName = System.Net.Dns.GetHostName();

                if (RemoveRedirectFile() == false)
                {
                    Trace.WriteLine("Removal of redirect file failed.", DateTime.Now.ToString());
                    MakeErrEntry(REDIRECT_DEL_CODE,REDIRECT_DEL_FAILED,REDIRECT_DEL_FAILED );
                    throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
                }
                if (hostName != redirectHost)
                {
                    //Make a redirect entry
                    if (AddRedirectEntry(redirectHost) == false)
                    {
                        Trace.WriteLine("AddRedirectEntry file failed.", DateTime.Now.ToString());
                        MakeErrEntry(REDIRECT_ADD_CODE, REDIRECT_ADD_FAILED, REDIRECT_ADD_FAILED);
                        throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
                    }
                }
                redirectRes.RedirectHost = redirectHost;
                Trace.WriteLine("--------------- END REDIRECT HOST ---------------", DateTime.Now.ToString());
                return redirectRes;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("SetRedirectHost():Exception Caught " + ex.StackTrace, DateTime.Now.ToString());
                MakeErrEntry(EXCEPTION_CODE,ex.Message,ex.StackTrace );
                throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
            }
		}

        public RedirectResponse GetRedirectHost()
        {
            //Trace.WriteLine("--------------- BEGIN GET REDIRECT HOST ---------------", DateTime.Now.ToString());
            string redirectHost = String.Empty;
            try
            {
                Trace.WriteLine("GetRedirectHost()", DateTime.Now.ToString());
                string domainDir =  AppDomain.CurrentDomain.BaseDirectory;
                if ( !domainDir.EndsWith("\\"))
                    domainDir += "\\";
                string redirectFile = domainDir + REDIRECT_FILE;

                Trace.WriteLine("GetRedirectHost() Seraching for file " + redirectFile, DateTime.Now.ToString());
                if (File.Exists(redirectFile))
                {
                    //Trace.WriteLine("GetRedirectHost() File Found " + redirectFile, DateTime.Now.ToString());
                    redirectHost = File.ReadAllText(redirectFile);
                }
                else
                {
                    //Trace.WriteLine("GetRedirectHost() File Not Found " + redirectFile, DateTime.Now.ToString());
                }
                //Trace.WriteLine("Redirect Host is " + redirectHost, DateTime.Now.ToString());
                redirectRes.RedirectHost = redirectHost;
                Trace.WriteLine("--------------- END GET REDIRECT HOST ---------------", DateTime.Now.ToString());
                return redirectRes;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("GetRedirectHost():Exception Caught " + ex.StackTrace, DateTime.Now.ToString());
                MakeErrEntry(EXCEPTION_CODE, ex.Message, ex.StackTrace);
                throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private bool CheckInputParam(string hostName)
        {
            bool res = false;
            if (hostName == null)
                return res;
            if (hostName.Length <= 0)
                return res;
            return true;
        }

        private bool AddRedirectEntry(string redirectHost)
        {
            bool res = true;
            try
            {
                Trace.WriteLine("Removing existing redirect file.", DateTime.Now.ToString());
                if (RemoveRedirectFile() == false)
                    return false;
                Trace.WriteLine("Creating redirect file.", DateTime.Now.ToString());
                string domainDir = AppDomain.CurrentDomain.BaseDirectory;
                if (!domainDir.EndsWith("\\"))
                    domainDir += "\\";
                string redirectFile = domainDir + REDIRECT_FILE;
                Trace.WriteLine("Redirect file " + redirectFile, DateTime.Now.ToString());

                System.IO.File.WriteAllText(redirectFile, redirectHost);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("AddRedirectFile():Exception Caught " + ex.StackTrace, DateTime.Now.ToString());
                res = false;
            }
            return res;
        }

        private bool RemoveRedirectFile()
        {
            bool res = true;
            try
            {
                //Check for Existence of Redirect File
                string domainDir = AppDomain.CurrentDomain.BaseDirectory;
                if (!domainDir.EndsWith("\\"))
                    domainDir += "\\";
                string redirectFile = domainDir + REDIRECT_FILE;
                Trace.WriteLine("Redirect file " + redirectFile, DateTime.Now.ToString());
                if (File.Exists(redirectFile))
                {
                    Trace.WriteLine("Deleting file " + redirectFile, DateTime.Now.ToString());
                    File.WriteAllText(redirectFile, "");
                    //File.Delete(redirectFile);
                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine("RemoveRedirectFile():Exception Caught " + ex.StackTrace, DateTime.Now.ToString());
                res = false;
            }
            return res;
        }

        private void MakeErrEntry( int code, string message, string details)
        {
            errObject.ErrorNo = code;
            errObject.ErrorMessage = message;
            errObject.ErrorDetails = details;
        }

        public int CheckForRedirect()
        {
            int res = 0;
            Trace.WriteLine("Calling GetRedirectHost()", DateTime.Now.ToString());
            string redirectHost = GetRedirectHost().RedirectHost;
            if (redirectHost == null)
                return res;
            if (redirectHost == String.Empty)
                return res;
            string hostName = System.Net.Dns.GetHostName();

            if (hostName == redirectHost)
                    return res;

            if (CheckCyclic() == true)
            {
                return -1;
            }
            return 1;
        }


        public Object RedirectRequest(string data, string requestType)
        {
            Object res = null;
            try
            {
                string hostName = GetRedirectHost().RedirectHost;
                string uri = "https://" + hostName + ":18732/Peripheral/" + requestType;
                if (data != "")
                    uri += "?" + data;
                Trace.WriteLine("URL : = " + uri);
                NPSCertificate.OverrideValidation();

                WebRequest request = null;
                WebResponse response = null;

                try
                { 
                    request = WebRequest.Create(uri);
                    if (request != null)
                    {
                        response = request.GetResponse();
                        if (response == null)
                            return res;

                        if (requestType == "GetImage")
                            return (Stream) response.GetResponseStream();

                        var strWebResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        if (strWebResponse == null)
                            return res;

                        JavaScriptSerializer dataSerializer = new JavaScriptSerializer();
                        JToken webToken = JObject.Parse(strWebResponse);
                        JToken resultToken = webToken[requestType + "Result"];
                        string resultString = resultToken.ToString();

                        if (requestType == "PrinterDiagnostics")
                            res = dataSerializer.Deserialize<DiagnosticsResponse>(resultString);
                        else if (requestType == "ScanCheck")
                            res = dataSerializer.Deserialize<ScanCheckResponse>(resultString);
                        else if (requestType == "PrintDocStream")
                            res = dataSerializer.Deserialize<PrintResponse>(resultString);
                        else if (requestType == "FrankCheck")
                            res = dataSerializer.Deserialize<PrintResponse>(resultString);
                        else if (requestType == "PrintCheckStream")
                            res = dataSerializer.Deserialize<PrintResponse>(resultString);
                        else if (requestType == "GetImage")
                            res = dataSerializer.Deserialize<Stream>(resultString);
                    }
                }
                catch(WebException ex)
                {
                    if (ex.Response != null)
                    {
                        var error = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        errObject = serializer.Deserialize<FaultInfo>(error);
                        return errObject;
                    }
                    errObject.ErrorMessage = ex.Message;
                    errObject.ErrorDetails = ex.StackTrace;
                    return res;
                }

            }
            catch(Exception ex)
            {
                Trace.WriteLine("RedirectRequest():Exception Caught " + ex.StackTrace, DateTime.Now.ToString());
                MakeErrEntry(EXCEPTION_CODE, ex.Message, EXCEPTION_MESSAGE);
                return errObject;
            }
            return res;
        }

        private bool CheckCyclic()
        {
            bool res = true;
            string hostName = GetRedirectHost().RedirectHost;
            string uri = "https://" + hostName + ":18732/Peripheral/GetRedirectHost";
            NPSCertificate.OverrideValidation();

            WebRequest request = null;
            WebResponse response = null;

            try
            {
                request = WebRequest.Create(uri);
                if (request != null)
                {
                    response = request.GetResponse();
                    if (response == null)
                        return res;

                    var strWebResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    if (strWebResponse == null)
                        return res;

                    JavaScriptSerializer dataSerializer = new JavaScriptSerializer();
                    JToken webToken = JObject.Parse(strWebResponse);
                    JToken resultToken = webToken["GetRedirectHost" + "Result"];
                    string resultString = resultToken.ToString();
                    RedirectResponse redirectRes = dataSerializer.Deserialize<RedirectResponse>(resultString);


                    if ( redirectRes!= null)
                    {
                        string myHostName = System.Net.Dns.GetHostName();
                        if (redirectRes.RedirectHost != myHostName)
                            return false;
                        Trace.WriteLine("RedirectRequest():Exception Thrown" + "Cyclic Entry Detected", DateTime.Now.ToString());
                        MakeErrEntry(REDIRECT_CYCLIC_CODE, REDIRECT_CYCLIC_FAILED, REDIRECT_CYCLIC_MESSAGE);
                    }
                }
                return res;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    var error = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    errObject = serializer.Deserialize<FaultInfo>(error);
                    return res;
                }
                errObject.ErrorMessage = ex.Message;
                errObject.ErrorDetails = ex.StackTrace;
                return res;
            }

        }
	}


    public static class NPSCertificate
    {
        private static bool OnValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public static void OverrideValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback = OnValidateCertificate;
            ServicePointManager.Expect100Continue = true;
        }
    }
}
