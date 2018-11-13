using System.Reflection;
using System.Resources;
using System.ServiceModel;
using System;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.DMS.Web.ServiceClient.DMSService;

namespace MGI.Channel.DMS.Web.Common
{
    public static class ExceptionHandler
    {

        public static string GetSOAPExceptionMessage(FaultException<MGI.Common.Sys.NexxoSOAPFault> nexxoFault)
        {
            string errorcode = string.Join(".", new object[] { nexxoFault.Detail.MajorCode, nexxoFault.Detail.MinorCode });

            if (!string.IsNullOrEmpty(nexxoFault.Detail.ProviderId))
                errorcode += "." + string.Join(".", new object[] { nexxoFault.Detail.ProviderId, nexxoFault.Detail.ProviderErrorCode });
            
            return string.Join("|", new object[] { string.IsNullOrEmpty(nexxoFault.Detail.Processor) ? "MGiAlloy" : nexxoFault.Detail.Processor, errorcode, nexxoFault.Detail.Details, string.IsNullOrEmpty(nexxoFault.Detail.AddlDetails) ? "Please contact the System Administrator" : nexxoFault.Detail.AddlDetails });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetExceptionMessage(Exception e)
        {
            if (e.Message == string.Empty || e.Message == null)
            { return "Error processing your request, please try again"; }
            else
            { return e.Message; }
        }

    }
}