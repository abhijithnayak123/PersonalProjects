using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Peripheral.Service.CustomAction.DMSService;

namespace Peripheral.Service.CustomAction
{
    public class NPSActivites
    {

        /// <summary>
        /// Create Terminal
        /// </summary>
        public void CreateTerminal()
        {
            Log.WriteEvent("Creating Terminal");
            Log.Writer(PeripheralConfig.INSTALL, "Start::CreateTerminal()::");

            string portNumber = PeripheralConfig.HTTPS_PORT_NUMBER.ToString();
            string peripheralUrl = string.Format(PeripheralConfig.PERIPHERAL_SERVICE_URL, portNumber);
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ServiceConfig.TrustAllCertificatesCallback);
                DMSService.NpsInstallerServiceClient dmsService = new DMSService.NpsInstallerServiceClient();
                dmsService.Endpoint.Address = new System.ServiceModel.EndpointAddress(FileConfig.ServiceURL);
               
                NpsTerminal npsTerminal = new NpsTerminal()
                {
                    Name = System.Net.Dns.GetHostName(),
                    Description = string.Empty,
                    ChannelPartnerId = long.Parse(FileConfig.ChannelPartnerID),
                    IpAddress = Prerequisite.GetIP(),
                    PeripheralServiceUrl = peripheralUrl,
                    Port = portNumber,
                    Status = "Available"
                };

                Log.Writer(PeripheralConfig.INSTALL, "CreateTerminal()-DMSService::" + npsTerminal);
                try
                {
                    bool createNpsTerminal = dmsService.CreateTerminal(npsTerminal);
                    if (createNpsTerminal)
                    {
                        Log.Writer(PeripheralConfig.INSTALL, "CreateTerminal():: Success: inserted data into database");
                    }
                    else
                    {
                        Log.WriteEvent("Terminal creation failed");
                        Log.Writer(PeripheralConfig.INSTALL, "CreateTerminal():: Failed: Connection error while inserting data into database");
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteEvent("Terminal creation threw an exception");
                    Log.Writer(PeripheralConfig.INSTALL, "Warning:: CreateTerminal()-DMSService.CreateTerminal:HostName already exists " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Log.WriteEvent("Terminal creation threw an base exception");
                Log.Writer(PeripheralConfig.INSTALL, "Warning:: CreateTerminal():: NpsInstallerServiceClient: " + ex.Message);
            }
        }
    }
}
