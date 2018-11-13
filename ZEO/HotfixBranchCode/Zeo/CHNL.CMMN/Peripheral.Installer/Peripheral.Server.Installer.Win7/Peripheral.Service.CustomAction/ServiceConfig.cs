using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;

namespace Peripheral.Service.CustomAction
{
    public static class ServiceConfig
    {
		public static DMSService.NpsInstallerServiceClient GetService(string serviceUrl)
        {
		    var _service = new DMSService.NpsInstallerServiceClient();
            if (!string.IsNullOrEmpty(serviceUrl))
            {
				var basicHttpBinding = new WSHttpBinding("WSHttpBinding_INpsInstallerService");
                var endpointAddress   = new EndpointAddress(serviceUrl);

				_service = new DMSService.NpsInstallerServiceClient(basicHttpBinding, endpointAddress);
            }
            return _service;
        }

        public static bool TrustAllCertificatesCallback(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error)
        {
            // Ignore DMS Service https errors
            return true;
        }
    }
}