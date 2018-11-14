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
            return nexxoFault.Message;
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