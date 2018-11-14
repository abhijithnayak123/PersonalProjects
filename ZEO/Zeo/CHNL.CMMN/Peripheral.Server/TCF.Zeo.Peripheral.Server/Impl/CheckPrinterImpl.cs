using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Spring.Context.Support;
using Spring.Context;
using System.Collections;
using System.ServiceModel.Web;
using TCF.Zeo.Peripheral.Server.Contract;
using TCF.Zeo.Peripheral.Server.Data;
using IPeripheralPrinter = TCF.Zeo.Peripheral.CheckPrinter.Contract;

namespace TCF.Zeo.Peripheral.Server.Impl
{
    public partial class PeripheralServiceImpl : ICheckPrinter
    {
        public PrintResponse PrintCheckStream(string printparams)
        {
            Trace.WriteLine("-------------- PRINT STREAM BEGIN ----------------", DateTime.Now.ToString());
            
            PrintResponse printResults = new PrintResponse();

            String printData = String.Empty;
            //REDIRECT CODE BEGIN - SV 02/01/2015
            PeripheralServiceImpl redirectImpl = new PeripheralServiceImpl();
            Trace.WriteLine("Check for PRINT Redirect", DateTime.Now.ToString());
            int redirectRequired = redirectImpl.CheckForRedirect();
            if (redirectRequired == 1)
            {
                printparams = System.Net.WebUtility.HtmlDecode(printparams);
                //printparams = printparams.Replace(' ', '+');
                Object retObj = redirectImpl.RedirectRequest("printparams=" + printparams, "PrintCheckStream");
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

            //test Code for offline print
            //Without Image
            //printResults = PrintCheck("LkFSRUFOQU1FLlVTRG9sbGFyc14uT1JJR0lOWC4xMjBeLk9SSUdJTlkuMjFeLldJRFRILjM1Xi5IRUlHSFQuMTJeLkZPTlRDQVRFR09SWS4yXi5GT05UTkFNRS5BcmlhbF4uRk9OVFNJWkUuMTAuMF4uQk9MRC4xXi5URVhULioqKlVTRE9MTEFSUyoqKg0JLkFSRUFOQU1FLlRyYW5zYWN0aW9uUmVmZXJlbmNlTm9eLk9SSUdJTlguMTIyXi5PUklHSU5ZLjI4Xi5XSURUSC41MF4uSEVJR0hULjEyXi5GT05UQ0FURUdPUlkuMl4uRk9OVE5BTUUuQXJpYWxeLkZPTlRTSVpFLjEwLjBeLkJPTEQuMV4uVEVYVC4qKioqKiowMDI2IDUwMDAwMg0JLkFSRUFOQU1FLlRyYW5zYWN0aW9uRGF0ZV4uT1JJR0lOWC4xNTReLk9SSUdJTlkuM14uV0lEVEguNDVeLkhFSUdIVC4xNV4uRk9OVENBVEVHT1JZLjJeLkZPTlROQU1FLkFyaWFsXi5GT05UU0laRS4xMC4wXi5CT0xELjFeLlRFWFQuRmVicnVhcnkgMDYgMjAxNQ0JLkFSRUFOQU1FLlRyYW5zYWN0aW9uQW1vdW50Xi5PUklHSU5YLjcwXi5PUklHSU5ZLjIxXi5XSURUSC40Ml4uSEVJR0hULjEwXi5GT05UQ0FURUdPUlkuMl4uRk9OVE5BTUUuQXJpYWxeLkZPTlRTSVpFLjEwLjBeLkJPTEQuMV4uVEVYVC4kJCQkJCQkMTUwLjAwKioqKioqDQkuQVJFQU5BTUUuVHJhbnNhY3Rpb25BbW91bnRJbldvcmRzXi5PUklHSU5YLjEwXi5PUklHSU5ZLjE1Xi5XSURUSC4xMTBeLkhFSUdIVC4xMl4uRk9OVENBVEVHT1JZLjJeLkZPTlROQU1FLkFyaWFsXi5GT05UU0laRS4xMC4wXi5CT0xELjFeLlRFWFQuKioqT05FIEhVTkRSRUQgRklGVFkgQU5EIDAwLzEwMCoqKgk=");
            //With Image
            //printResults = PrintCheck("LkFSRUFOQU1FLlRyYW5zYWN0aW9uSWReLk9SSUdJTlguMF4uT1JJR0lOWS4wXi5XSURUSC4zNV4uSEVJR0hULjZeLkZPTlRDQVRFR09SWS4wXi5GT05UU0laRS4wXi5CT0xELjFeLlRFWFQuMTAwMDAwMDEyMyANCS5BUkVBTkFNRS5DaGVja051bWJlcl4uT1JJR0lOWC4xMjVeLk9SSUdJTlkuMF4uV0lEVEguMzVeLkhFSUdIVC42Xi5GT05UQ0FURUdPUlkuMF4uRk9OVFNJWkUuMF4uQk9MRC4xXi5URVhULjAwMDkwNCANCS5BUkVBTkFNRS5WZXJpZmljYXRpb25eLk9SSUdJTlguMF4uT1JJR0lOWS40Xi5XSURUSC41MF4uSEVJR0hULjEyXi5GT05UQ0FURUdPUlkuMF4uRk9OVFNJWkUuMF4uQk9MRC4xXi5URVhULkZvciB2ZXJpZmljYXRpb24gY2FsbCANCS5BUkVBTkFNRS5QaG9uZV4uT1JJR0lOWC4wXi5PUklHSU5ZLjheLldJRFRILjUwXi5IRUlHSFQuMTJeLkZPTlRDQVRFR09SWS4wXi5GT05UU0laRS4wXi5CT0xELjFeLlRFWFQuMS03MTQtNjY3LTg0NDAgDQkuQVJFQU5BTUUuRGF0ZV4uT1JJR0lOWC4xNjJeLk9SSUdJTlkuNF4uV0lEVEguNTBeLkhFSUdIVC4xNV4uRk9OVENBVEVHT1JZLjBeLkZPTlRTSVpFLjBeLkJPTEQuMV4uVEVYVC4wMi8wOS8yMDE1IA0JLkFSRUFOQU1FLkFtb3VudF4uT1JJR0lOWC4xMzBeLk9SSUdJTlkuMTBeLkhFSUdIVC4xMV4uRk9OVENBVEVHT1JZLjJeLkZPTlROQU1FLk5leHhvU2VjdXJlRm9udDAxIEFyaWFsXi5GT05UU0laRS40OC4wXi5CT0xELjFeLlRFWFQuJDEwMC4wMCANCS5BUkVBTkFNRS5BbW91bnRJbldvcmRzXi5PUklHSU5YLjheLk9SSUdJTlkuMjZeLldJRFRILjExMF4uSEVJR0hULjEyXi5GT05UQ0FURUdPUlkuMl4uRk9OVE5BTUUuQXJpYWxeLkZPTlRTSVpFLjEzLjBeLkJPTEQuMF4uVEVYVC5PbmUgaHVuZHJlZCBkb2xsYXJzIGFuZCAwMC8xMDAgDQkuQVJFQU5BTUUuU2lnbmF0dXJlXi5PUklHSU5YLjE1NV4uT1JJR0lOWS4zNl4uV0lEVEguMjdeLkhFSUdIVC45Xi5JTUFHRS5RazJtR1FBQUFBQUFBSFlBQUFBb0FBQUF6UUFBQUQ0QUFBQUJBQVFBQUFBQUFEQVpBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQWdBQUFnQUFBQUlDQUFJQUFBQUNBQUlBQWdJQUFBSUNBZ0FEQXdNQUFBQUQvQUFEL0FBQUEvLzhBL3dBQUFQOEEvd0QvL3dBQS8vLy9BUC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy9BQS8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8rSC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy84QUQvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL0FQLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vd0FQLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy93RC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL0FBLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vZ0EvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLzhBRC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy9BUC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy93QVAvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vd0NQLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vQUEvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLzhBLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vOEFELy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy80QVAvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL3dBUC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vK0FELy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy9BQS8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy9nQWovLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy84QUQvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vNEFQLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vd0FQLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy93Q1AvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy9qLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL0FBLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vOEFqLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy84SS8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLzhBRC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLzRBUC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vM0QvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy93QVAvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8rQUQvLy8vLy8vLy8vNENQLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLzhJLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vQUFkM2QzZDNkM2QzZDNkM2QzZDNkM2QzZDNkM2R3ZDNkM2QzZDNkM2QzZDNkM2QzZDNkM2QzZDNkM0FBZDNkM2QzZDNkM2NBZDNkM2QzZDNkM2QzZDNkM2QzZDNkM2QzZDNkM2QzZDNkM2QzZDNkM2R3QjNkM2QzZDNkM2QzZDNkM2QzZDNkM2QzY0FDSWlJaUlpSWlIQi9pSWlJaUhpSWlJY0FDUGp3QUFmNGlJaUlpSWlJaUlpSWlJaUlpSWlJaUlpSWlJQUgrSWlJaUlpSWlJQUgrSWlJaUlpSWlJaUlpSWlQaDNkM0FBZjRpSWlJaUlpSWlJaUlpSWlJOEErSWlJaUlpSWlJaUlpSWlJaUlpSWlJaUFBUC8vLy8vLy8vOEFqLy8vLzRBSS8vOXdBQWYvY1BjSC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL3dDUC8vLy8vLy8vK0FCLy8vLy8vLy8vLy8vLy8vZ0FBQUFIQUEvLy8vLy8vLytQLy8vLy8vLzNCLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy9BQS8vLy8vLy8vLy9BSC8vLy85d0IvLy9BUGdBL3dEL0FQLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy84QWYvLy8vLy8vLy84QUQvLy8vLy8vLy8vLy8vOEEvLy8vLzNBUC8vLy8vLzhBRC8vLy8vLy85dy8vLy8vLy8vLy8vLy8vLy8vLy8vLy84QUQvLy8vLy8vLy8vM0IvLy8vL2NBRC8vd0QvQUg4QStBRC8vNENQLy8vLy8vLy8vLy8vLy8vLy8vLy8vL0FILy8vLy8vLy8vNEFBai8vLy8vLy8vLy8vL3dmLy8vLy8rQUIvLy8vLy8zQUFmLy8vLy8vL0FJLy8vLy8vLy8vLy8vLy8vLy8vLy8vd0FQLy8vLy8vLy8vd2NBai8vLy93QUEvL2dIL3dDSEIvY0gvL2NBLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy93Q1AvLy8vLy8vLzl3QVAvLy8vLy8vLy8vLy9jSS8vLy8vLzhBZi8vLy8vOEFBSC8vLy8vLy93RC8vLy8vLy8vLy8vLy8vLy8vLy8vL0FBLy8vLy8vLy8vLzhIY0EvLy8vOEFBUC80QVArQUFBLzNCLy93QVAvL0FIOEFBSC8vLy8vLy8vLy8vLy8vOEFmLy8vLy8vLy8vQUFBUC8vLy8vLy8vLy8vd0QvLy8vLy8vY0EvLy8vLzRBQUNQLy8vLy8vZ0EvLy8vLy8vLy8vLy8vLy8vLy8vLzhBRC8vLy8vLy8vLy8vZDRBSS8vLy9BQUFQK0FmLzl3QUg5d2ovOEFDUDhBQUFjQUFBQUlqLy8vLy8vLy8vLy9BSC8vLy8vLy8vL3dBQUNQLy8vLy8vLy8vLzhBLy8vLy8vLzRBSS8vLy8rQUJ3Zi8vLy8vLy9BUC8vLy8vLy8vLy8vLy8vLy8vLy93QVAvLy8vLy8vLy8vLzRlQUNQLy8vd0J3Q1BjSC8vY0FDUEFQLzNBQWZ3ZUFqLy8zQUFBSEQvLy8vLy8vLy8vd0NQLy8vLy8vLy84SDhBai8vLy8vLy8vLy8vQVAvLy8vLy8vd0NQLy8vL2NIZ0EvLy8vLy8rQWYvLy8vLy8vLy8vLy8vLy8vLy8vQUEvLy8vLy8vLy8vLy8vd2dBZi8vLzhBaHdmNEFQLy9BQWR3Ly84SGNBQi8vLy8vLy85d0FBZi8vLy8vLy8vLzhBZi8vLy8vLy8vM0FQY0gvLy8vLy8vLy8vL3dELy8vLy8vLzhBZi8vLy8zQjRBSS8vLy8vL2dBLy8vLy8vLy8vLy8vLy8vLy8vOEFELy8vLy8vLy8vLy8vLzl3QUEvLy8vY0hnQS93RC8rSEFBQUFqd2YvaVAvLy8vLy8vLy80QUFqLy8vLy8vLy8vQUgvLy8vLy8vLzl3ZjRBUC8vLy8vLy8vLy9nQS8vLy8vLy8vY0FqLy8vOXdmd0IvLy8vLy80QVAvLy8vLy8vLy8vLy8vLy8vL3dBUC8vLy8vLy8vLy8vLy8vZ0FBUC8vLzRCL0FIOEEvLzhBQndBQUIvLy8vLy8vLy8vLy8vLzNBSC8vLy8vLy8vL3dBUC8vLy8vLy8vY0g5d0NQLy8vLy8vLy8vL0FQLy8vLy8vLzRBSC8vLy9jSDhBRC8vLy8vOXdELy8vLy8vLy8vLy8vLy8vLy9BQS8vLy8vLy8vLy8vLy8vLzRBQUQvLy85d2Z3QUhBUC8vQ1ArSEFJLy8vLy8vLy8vLy8vLy8rQUFQLy8vLy8vLy84QWYvLy8vLy8vL3dEL2dBai8vLy8vLy8vLy93RC8vLy8vLy8rQUNQLy8vd0NQQUEvLy8vLy9BSC8vLy8vLy8vLy8vLy8vLy84QUQvLy8vLy8vLy8vLy8vLy8vd0FBai8vL2NIL3dBQWYvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLzl3RC8vLy8vLy8vL0FILy8vLy8vLy84QS8vY0gvLy8vLy8vLy8vZ0EvLy8vLy8vLzl3Zi8vLzhBLzNBSC8vLy8vM0IvLy8vLy8vLy8vLy8vLy8vd0FQLy8vLy8vLy8vLy8vLy8vLzl3QUkvLy8zQi85d0FJLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vZ0FmLy8vLy8vLy8zQi8vLy8vLy8vL0FQLzNBUC8vLy8vLy8vLzRBUC8vLy8vLy8vQUFpUC8vQUkrQUIvLy8vLzl3Zi8vLy8vLy8vLy8vLy8vL0FBLy8vLy8vLy8vLy8vLy8vLy8vOEFDUC8vOXdELzl3ai8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy9BQS8vLy8vLy8vOXdmLy8vLy8vLy93RC8rQUQvLy8vLy8vLy8vd0QvLy8vLy8vOEFBQUFBY0FCLzhBQ1AvLy8vY0kvLy8vLy8vLy8vLy8vLzhBRC8vLy8vLy8vLy8vLy8vLy8vLy9jQWYvLy9nQS8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vNEFJLy8vLy8vLy9nSC8vLy8vLy8vOEFqLzhBai8vLy8vLy8vLzhBai8vLy8vLy85M0FBQUFBQUFBQUFELy8vL3dCLy8vLy8vLy8vLy8vLy93QVAvLy8vLy8vLy8vLy8vLy8vLy8vNEFBLy8vL0FJLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8zQi8vLy8vLy8vNEIvLy8vLy8vLy9jSS8vY0gvLy8vLy8vLy8vQUgvLy8vLy8vLytBQ1ArSUFBQUFBQWQzZi84QWYvLy8vLy8vLy8vLy8vQUEvLy8vLy8vLy8vLy8vLy8vLy8vLy93Qi8vLy93RC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vK0FELy8vLy8vLzl3Zi8vLy8vLy8vd0QvLzRCLy8vLy8vLy8vL3dCLy8vLy8vLy8vOEFELy93RC8rQUFBQUFBQUFILy8vLy8vLy8vLy8vOEFELy8vLy8vLy8vLy8vLy8vLy8vLy8vOEFmLy8vOEEvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy9nQWovLy8vLy8vQUkvLy8vLy8vLzhBLy8rQWYvLy8vLy8vLy84QWYvLy8vLy8vLy9jQS8vOEEvLzl3QUlkd0FBQUFlUC8vLy8vLy8vL3dBUC8vLy8vLy8vLy8vLy8vLy8vLy8vLy9jSC8vLy9BUC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL0FBLy8vLy8vL3dELy8vLy8vLy8vQVAvL2dILy8vLy8vLy8vL2NILy8vLy8vLy8vNEFILzRBUC8vOEFELy8vZ0FBQUFBQUlpUC8vLy9BQS8vLy8vLy8vY0hpSS8vLy8vLy8vLy8vM2YvLy8vd0QvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8zQVAvLy8vLy84QS8vLy8vLy8vL3dELy80Qi8vLy8vLy8vLy80Qi8vLy8vLy8vLy8zQVArQUQvLy9jQS8vLy9BSS80aDNBQS8vLy84QUQvLy8vLy8vOEFBQUFBZUkvLy8vLy8vLy8vLy8vLzhBai8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL3dCLy8vLy8vL0FQLy8vLy8vLy84QWovL3dELy8vLy8vLy8vK0FELy8vLy8vLy8vL3dDUDhBLy8vNEFILy8vd0NQLy8vLy8vLy8vd0FQLy8vLy8vLy8rSWNBQUFBSC8vLy8vLy8vLy8vLy8vQUkvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8rQUIvLy8vLy93RC8vLy8vLy8vL0FILy84QS8vLy8vLy8vLy9nSC8vLy8vLy8vLy85d0QzQVAvLy8zQUkvLzhBLy8vLy8vLy8vL0FBLy8vLy8vLy8vLy8vK0hBQUFBZDQvLy8vLy8vLy8vLzNELy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLzl3Qi8vLy8vZ0gvLy8vLy8vLy8zQi8vL0FQLy8vLy8vLy8vL0FQLy8vLy8vLy8vL2dBZHdELy8vK0FCLy8vQUgvLy8vLy8vLzhBRC8vLy8vLy8vLy8vLy8vNGh3QUFBQWYvLy8vLy8vLy84QS8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vOEFELy8vL3dELy8vLy8vLy8vOXdmLy8zLy8vLy8vLy8vLzl3Zi8vLy8vLy8vLy8vY0FBSC8vLy84QUQvL3dDUC8vLy8vLy93QVAvLy8vLy8vLy8vLy8vLy8vLzRjQUFBQUFlUC8vLy8vNEIvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy9jQWovLy9nSC8vLy8vLy8vLy9jSC8vLy8vLy8vLy8vLy8vY0gvLy8vLy8vLy8vLy9BQUNQLy8vL2NBZi84QWovLy8vLy8vQUEvLy8vLy8vLy8vLy8vLy8vLy8vLytIZHdBQUFBZDNlUDl3ai8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL2NBZi8vd0NQLy8vLy8vLy8vNEIvLy8vLy8vLy8vLy8vLzNDUC8vLy8vLy8vLy8vM0FBai8vLy80QUFqL0FJLy8vLy8vOEFELy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL2gzY0FBQUFBQUhpSS8vLy8vLy8vaVAvLy8vLy8vLy8vLy8vLy8vLy8vLy8vY0FlUDhBLy8vLy8vLy8vLy93Q1AvLy8vLy8vLy8vLy85d2ovLy8vLy8vLy8vLy93QUkvLy8vLzNBSC93RC8vLy8vL3dBUC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vNGlBQUFBQUFBQUFCNCtQL3dBSS8vLy8vLy8vLy8vLy8vLy8vLy8vLy9nQUFBRC8vLy8vLy8vLy8vOEFqLy8vLy8vLy8vLy8vL0FQLy8vLy8vLy8vLy8vK0FCLy8vLy8vM0FBY0EvLy8vLy9BQS8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8rSWlIY0FBQUFBQUFBQWovLy8vLy8vLy8vLy8vLy8vLy8vLy8vL2lBQi8vLy8vLy8vLy8vLy9jSC8vLy8vLy8vLy8vLy93RC8vLy8vLy8vLy8vLy84QS8vLy8vLytIQUFCLy8vLy84QUQvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vaUlpSS8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL0FQLy8vLy8vLy8vLy8vZ0gvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLzNBQWovLy8vd0FQLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy93RC8vLy8vLy8vLy8vLzRELy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vd0NQLy8vL0FBLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vOEFmLy8vLy8vLy8vLy84QS8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLzhBRC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy9jQS8vLy8vLy8vLy8vNEIvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy93QVAvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL2NJLy8vLy8vLy8vLzhBLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vQUEvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy80QVAvLy8vLy8vLy8vQ1AvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vOEFELy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLzNBUC8vLy8vLy8vOEEvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL3dBUC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vd0IvLy8vLy8vLzRELy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy9BQS8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy93Q1AvLy8vLy9nSS8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy84QUQvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vOXdlUC8vLy8rQWovLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vd0FQLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy85d0NQLy8vL0IvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vL0FBLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLzl3QjQvLzhILy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLzhBRC8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vK0FBQUFBLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy93QVAvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vNEFBai8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vQUEvLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vLy8vOEFBPQk=");
            //return printResults;

            if ((printData = StreamData.AddStream(printparams)) != "")
            {
                printData.TrimEnd('\\');
                String[] printList = printData.Split('\\');
                for (int i = 0; i < printList.Length; i++)
                {
                    Trace.WriteLine("Check Print Raw Data = " + printList[i], DateTime.Now.ToString());
                    printResults = PrintCheck(printList[i]);
                }
            }
            return printResults;
        }

        public PrintResponse PrintCheck(String printparams)
        {
            Trace.WriteLine("-------------- BEGIN CHECK PRINT ----------------", DateTime.Now.ToString());

            printparams = printparams.Replace("\"", "");

            Trace.WriteLine("PeripheralServiceImpl:PrintCashCheckReceipt() Invoked", DateTime.Now.ToString());

            FaultInfo errObject = new FaultInfo();

            PrintResponse printResults = new PrintResponse();

            Trace.WriteLine("PeripheralServiceImpl:PrintCashCheckReceipt() Decode from Base64", DateTime.Now.ToString());

            byte[] decodedByt = Convert.FromBase64String(printparams);

            String decodedStr = System.Text.Encoding.UTF8.GetString(decodedByt);

            Trace.WriteLine("Retrieving ContextRegistry:GetContext()", DateTime.Now.ToString());

            IApplicationContext ctx = ContextRegistry.GetContext();

            Trace.WriteLine("ContextRegistry:GetContext() Completed", DateTime.Now.ToString());

            //TMS9000CheckPrinter String has to be retrieved from Service Call
            //from DeviceMapper. Need Clarity on how to get this!

            Trace.WriteLine("Instantiating Epson Print Impl", DateTime.Now.ToString());
            
            IPeripheralPrinter.ICheckPrinter printer = (IPeripheralPrinter.ICheckPrinter)ctx["TMS9000CheckPrinter"];

            IPeripheralPrinter.CheckPrintData printDataObj = new IPeripheralPrinter.CheckPrintData();

            Trace.WriteLine("Epson Print Impl Initiated", DateTime.Now.ToString());

            printDataObj.CheckData = decodedStr.Split('\t').ToList<String>();
            
            Trace.WriteLine("Calling Epson Print Impl", DateTime.Now.ToString());

            IPeripheralPrinter.CheckPrinterError errPrint = printer.PrintCheck(printDataObj);

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
            else
            {
                //get the images now and swap it since the check is placed the other way
                Trace.WriteLine("Epson Print Impl: Getting Scanned Images", DateTime.Now.ToString());
                printResults.Scan_BackImageTIFF = Convert.ToBase64String(printer.GetCheckFrontImage(String.Empty));
                printResults.Scan_FrontImageTIFF = Convert.ToBase64String(printer.GetCheckBackImage(String.Empty));
                printResults.Scan_BackImageJPG = Convert.ToBase64String(printer.GetCheckFrontImage("jpg"));
                printResults.Scan_FrontImageJPG = Convert.ToBase64String(printer.GetCheckBackImage("jpg"));
                Trace.WriteLine("Epson Print Impl: Got Scanned Images", DateTime.Now.ToString());
            }

            //test code for saving locally
            //TestSave(printResults);

            Trace.WriteLine("Print Completed with Success", DateTime.Now.ToString());
            printResults.Result = "Success";
            return printResults;
        }

        void TestSave(PrintResponse printResults)
        {
            byte[] back = Convert.FromBase64String(printResults.Scan_BackImageJPG);
            byte[] front = Convert.FromBase64String(printResults.Scan_FrontImageJPG);
            MemoryStream ms1 = new MemoryStream(front);
            MemoryStream ms2 = new MemoryStream(back);
            Image image1 = Image.FromStream(ms1);
            Image image2 = Image.FromStream(ms2);
            image1.Save("e:\\front.tiff");
            image2.Save("e:\\back.tiff");
        }

    }
}
    