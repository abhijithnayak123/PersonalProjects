using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Net;
using System.IO;

using MGI.Cxn.Fund.FirstView.Data;

namespace MGI.Cxn.Fund.FirstView.Impl
{
    public class FirstViewIO
    {
        public CardResponse ValidateCard(CardRequest request)
        {
            CardResponse response = new CardResponse();
			
            string requestUrl = BuildURL<CardRequest>(request);

            ExecuteService(requestUrl, response);

            return response;
        }

        public TransactionResponse Clearing(TransactionRequest request)
        {
            TransactionResponse response = new TransactionResponse();

            string requestUrl = BuildURL<TransactionRequest>(request);

            ExecuteService(requestUrl, response);

            return response;
        }

        public AccountResponse UpdateCardInfo(AccountRequest request)
        {
            AccountResponse response = new AccountResponse();

            string requestUrl = BuildURL<AccountRequest>(request);

            ExecuteService(requestUrl, response);

            return response;
        }

        private string BuildURL<T>(T request)
        {
            string url = string.Empty;
            bool isFirstParam = true;

            if (request != null)
            {
                var urlBuilder = new StringBuilder();
                string serviceURL = string.Empty;
                urlBuilder.Append("serviceURL?");

                PropertyInfo[] properties = request.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(request, null);
                    if (propertyValue != null && property.Name.Equals("ServiceUrl"))
                    {
                        serviceURL = propertyValue.ToString();
                    }
                    else
                    {
                        var format = string.Empty;
                        propertyValue = propertyValue != null ? Uri.EscapeDataString(propertyValue.ToString()) : propertyValue;
                        if (isFirstParam)
                        {
                            format = "{0}={1}";
                            isFirstParam = false;
                        }
                        else
                        {
                            format = "&{0}={1}";
                        }
                        urlBuilder.AppendFormat(format, new object[] { property.Name, propertyValue });
                    }
                }
                url = urlBuilder.ToString().Replace("serviceURL", serviceURL);
            }
            return url;
        }

        private void ExecuteService(string requestUrl, object response)
        {

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(requestUrl);

            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
			/****************************Begin TA-50 Changes************************************************/
			//     User Story Number: TA-50 | ALL |   Developed by: Sunil Shetty     Date: 03.03.2015
			//     Purpose: On Vera Code Scan, Improper Restriction of XML External Entity Reference ('XXE') FLaw, to overcome it we making xmlresolver null
			XmlDocument xdoc = new XmlDocument();
			using (StreamReader sr = new StreamReader(stream))
			{
				string responsetext = sr.ReadToEnd();				
				xdoc.XmlResolver = null;
                if (responsetext.Length > 0)
				    xdoc.LoadXml(responsetext);
			}
            if (response != null)
            {
                PropertyInfo[] propinfo = ((Type)response.GetType()).GetProperties();

                foreach (PropertyInfo p in propinfo)
                {
                    if (xdoc.SelectSingleNode("//" + p.Name) != null)
                    { p.SetValue(response, xdoc.SelectSingleNode("//" + p.Name).InnerText, null); }
                }
            }
        }
    }
}