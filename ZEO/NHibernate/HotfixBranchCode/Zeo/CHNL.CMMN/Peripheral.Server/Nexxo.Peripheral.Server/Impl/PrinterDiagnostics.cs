using System;
using System.Collections.Generic;
using System.Linq;
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



namespace MGI.Peripheral.Server.Impl
{
    public partial class PeripheralServiceImpl : IPrinter
    {
        public DiagnosticsResponse PrinterDiagnostics(String printparams)
        {
            Trace.WriteLine("--------------- BEGIN PRINTER DIAGNOSTICS ---------------", DateTime.Now.ToString());
            String npsVersion = "x.x.x";
            FaultInfo errObject = new FaultInfo();
            DiagnosticsResponse diagRes = new DiagnosticsResponse();
            try
            {
                IApplicationContext ctx = ContextRegistry.GetContext();
                Trace.WriteLine("ContextRegistry:GetContext() Completed", DateTime.Now.ToString());
                
                PeripheralServiceImpl redirectImpl = new PeripheralServiceImpl();
                Trace.WriteLine("Check for Print Redirect", DateTime.Now.ToString());

                int redirectRequired = redirectImpl.CheckForRedirect();
                if (redirectRequired == 1)
                {
                    Trace.WriteLine("Redirecting the request", DateTime.Now.ToString());
                    Object retObj = redirectImpl.RedirectRequest("printparams=" + printparams, "PrinterDiagnostics");
                    if (retObj.GetType() == typeof(FaultInfo))
                    {
                        errObject = (FaultInfo)retObj;
                        if (printparams != "startup")
                        {
                            Trace.WriteLine("Exception thrown during Prin Diag", DateTime.Now.ToString());
                            throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
                        }
                    }
                    return (DiagnosticsResponse)retObj;
                }
                else if (redirectRequired == -1)
                {
                    Trace.WriteLine("Failed to redirect the request", DateTime.Now.ToString());
                    errObject.ErrorMessage = "Failed to redirect the request to the printer.";
                    errObject.ErrorDetails = "Could not connect to the redirected Peripheral Server";
                    errObject.ErrorNo = 1100;
                    throw new WebFaultException<FaultInfo>((FaultInfo)errObject, System.Net.HttpStatusCode.InternalServerError);
                    return diagRes;
                }
                
                
                IPeripheralPrinter.IPrinter printer = (IPeripheralPrinter.IPrinter)ctx["TMS9000ReceiptPrinter"];
                IPeripheralPrinter.PrinterError errPrint = printer.RunDiagnostics();
                diagRes.DeviceName = errPrint.diag_deviceName;
                diagRes.DeviceStatus = errPrint.diag_deviceStatus.Trim(',', ' ');
                diagRes.SerialNumber = errPrint.diag_serialNumber;
                diagRes.FirmwareRevision = errPrint.diag_firmwareVersion;
                diagRes.Status = errPrint.diag_status;
                //diagRes.NpsVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                try
                {
                    npsVersion = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\" + "version.txt");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Epson.TMS9000.Impl:RunDiagnostics() Failed to get version info from version.txt missing file? " + ex.Message, DateTime.Now.ToString()); ;
                }
                diagRes.NpsVersion = npsVersion;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("PrinterDiagnostics: Failed Exception thrown " + ex.StackTrace, DateTime.Now.ToString());
                //throw new WebFaultException<FaultInfo>(errObject, System.Net.HttpStatusCode.InternalServerError);
            }
            Trace.WriteLine("--------------- END PRINTER DIAGNOSTICS ---------------", DateTime.Now.ToString());
            return diagRes;
        }
    }
}
