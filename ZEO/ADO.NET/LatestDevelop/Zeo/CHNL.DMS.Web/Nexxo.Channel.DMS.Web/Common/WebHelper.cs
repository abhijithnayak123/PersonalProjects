using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Common
{
	public static class WebHelper
	{
		public static Dictionary<string, string> GetRelatedActionController()
		{
			Dictionary<string, string> result = new Dictionary<string,string>();
			string controller = "CustomerSearch";
			string actionMethod = "CustomerSearch";
			if (HttpContext.Current.Session["EditProspect"] != null && (bool)(HttpContext.Current.Session["EditProspect"]) == true)
			{
				controller = "Product";
				actionMethod = "ProductInformation";
			}
			result.Add("controller", controller);
			result.Add("actionMethod", actionMethod);

			return result;
		}

        public static bool GetCheckBoxValue(string value)
        {
            string val = value.Split(',').First();
            return bool.Parse(val);
        }

        public static string MaskSSNNumber(string SSN)
        {
            var maskedValue = "***-**-";
            string result = string.Empty;
            var subResult = string.Empty;

            if (!string.IsNullOrWhiteSpace(SSN))
            {
                subResult = SSN.Substring(SSN.Length - Math.Min(4, SSN.Length));
                result = maskedValue + subResult;
            }
            return result;
        }

		// to get the Cookie
		public static Dictionary<string, object> GetCookie(string cookieName)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];

			//creating dic to return as collection.
			Dictionary<string, object> cookieContext = new Dictionary<string, object>();
			//Check whether the cookie available or not.

			if (cookie != null)
			{
				//Getting multiple values from single cookie.
				NameValueCollection nameValueCollection = cookie.Values;

				//Iterate the unique keys.
				foreach (string key in nameValueCollection.AllKeys)
				{
					cookieContext.Add(key, cookie[key]);
				}
			}

			return cookieContext;

		}

        public static string GetAppSettingValue(string key)
        {
            string configvalue = System.Configuration.ConfigurationManager.AppSettings[key];
            return configvalue;
        }
        
        public static bool VerifyException(ZeoClient.Response response)
        {
            bool isErrorRaised = false;
            if (response != null && response.Error != null)
            {
                isErrorRaised = true;
            }
            return isErrorRaised;
        }
    }
}
