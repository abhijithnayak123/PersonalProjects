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
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels; 




namespace MGI.Peripheral.Server.Impl
{
	public partial class PeripheralServiceImpl : IPrinter
	{
		public HostNameResponse GetHostName(bool localNps)
		{
			Trace.WriteLine("--------------- BEGIN HOST NAME RETRIEVAL ---------------", DateTime.Now.ToString());
            try
            {
                Trace.WriteLine("ISecurityCapabilities localNps NPS " + localNps, DateTime.Now.ToString());
                HostNameResponse hostNameRes = new HostNameResponse();
                if (localNps == true)
                {
                    
                    hostNameRes.HostName = System.Net.Dns.GetHostName();
                    hostNameRes.MachineName = Environment.MachineName;
                    hostNameRes.FQDN = System.Net.Dns.GetHostEntry("localhost").HostName;

                    Trace.WriteLine("GetHostName():Response =" + hostNameRes.HostName + ":" + hostNameRes.MachineName + ":" + hostNameRes.FQDN, DateTime.Now.ToString());
                    Trace.WriteLine("--------------- END HOST NAME RETRIEVAL FOR NON LOCAL NPS---------------", DateTime.Now.ToString());
                }
                else
                {
                    OperationContext context  = OperationContext.Current;
                    MessageProperties prop = context.IncomingMessageProperties;
                    RemoteEndpointMessageProperty endpoint =
                     prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                    string ip = endpoint.Address;
                    Trace.WriteLine("GetHostName():Vistors IP =" + ip, DateTime.Now.ToString());
                    IPHostEntry visitorIP = System.Net.Dns.GetHostEntry(ip);
                    if (visitorIP != null)
                    {
                        String[] hostDetails = visitorIP.HostName.Split('.');
                        hostNameRes.HostName = hostDetails[0];
                        hostNameRes.MachineName = hostDetails[0];
                        hostNameRes.FQDN = visitorIP.HostName;
                    }
                    Trace.WriteLine("GetHostName():Response =" + hostNameRes.HostName + ":" + hostNameRes.MachineName + ":" + hostNameRes.FQDN, DateTime.Now.ToString());
                    Trace.WriteLine("--------------- END HOST NAME RETRIEVAL FOR LOCAL NPS---------------", DateTime.Now.ToString());
                }
                return hostNameRes;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("GetHostName():Exception Caught " + ex.StackTrace, DateTime.Now.ToString());
                return null;
            }
		}
	}
}
