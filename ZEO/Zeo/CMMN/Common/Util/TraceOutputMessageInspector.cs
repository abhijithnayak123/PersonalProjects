using Spring.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;


namespace MGI.Common.Util
{
	public class TraceOutputMessageInspector : IClientMessageInspector
	{
		public NLoggerCommon NLogger { get; set; }

        public TraceOutputMessageInspector()
        {
            NLogger = new NLoggerCommon();
        }

		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
			WriteLog(reply, "RESPONSE:");
		}

		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			WriteLog(request, "REQUEST:");
			return null;
		}

		private void WriteLog(Message message, string messageHeader)
		{
			List<string> nologsattributes = new List<string>() { "governmentidnumber", "id_number", "cardnumber", "Pan", "AlloyID", "idNbr", "ssn", "SSN", "ID", "debtor_account_number", "CardNbr", "cardNumber", "ns2:number", "acctNbr", "GovernmentIdIdentifier", "CardIdentifier", "cardNum", "AcctNum" };

			XmlDocument document = new XmlDocument();
			//Purpose: On Vera Code Scan, Improper Restriction of XML External Entity Reference ('XXE') FLaw, to overcome it we are making xmlresolver null
			document.XmlResolver = null;

			document.LoadXml(message.ToString());

			foreach (var item in nologsattributes)
			{
				XmlNodeList xelements = document.GetElementsByTagName(item);


				foreach (XmlNode element in xelements)
				{
					if (element.Name == "ID" && element.SelectSingleNode("Value") == null)
						continue;

					if (element.Name == "ID" && element.SelectSingleNode("Value") != null && element.SelectSingleNode("Value").InnerText != "")
					{
						XmlNode idNode = element.SelectSingleNode("Value");
						int itemlength = idNode.InnerText.Length;
						
						if (itemlength > 4)
						{
							idNode.InnerText = idNode.InnerText.Substring(itemlength - 4);
							idNode.InnerText = idNode.InnerText.PadLeft(itemlength, 'X');
						} 
						
						continue;
					}
					if (element.Name == "CardIdentifier")
					{
						int itemlength = element.InnerText.Length;
						if (itemlength > 4)
						{
							element.InnerText = element.InnerText.Substring(itemlength - 4);
							element.InnerText = element.InnerText.PadLeft(itemlength, 'X');
						}
						break;
					}

					if (element.InnerText != "" && element.InnerText.Length > 3)
					{
						if (element.Name == "CardNbr" && element.InnerText != "" && element.InnerText.Length == 6)
						{
							int itemlength = element.InnerText.Length;
						}
						else
						{
							int itemlength = element.InnerText.Length;
							if (itemlength > 4)
							{
								element.InnerText = element.InnerText.Substring(itemlength - 4);
								element.InnerText = element.InnerText.PadLeft(itemlength, 'X');
							}
						}
					}

				}
			}
			//NLogger.WriteLog(messageHeader + Environment.NewLine + message.ToString());

			NLogger.Info(messageHeader + Environment.NewLine + FormatXMLString(document.InnerXml.ToString()));


		}
		// below method is for xml indentation in logfile 
		public static string FormatXMLString(string sUnformattedXML)
		{
			XmlDocument xd = new XmlDocument();
			//Purpose: On Vera Code Scan, Improper Restriction of XML External Entity Reference ('XXE') FLaw, to overcome it we are making xmlresolver null
			xd.XmlResolver = null;
			xd.LoadXml(sUnformattedXML);
			StringBuilder sb = new StringBuilder();
			StringWriter sw = new StringWriter(sb);
			XmlTextWriter xtw = null;
			try
			{
				xtw = new XmlTextWriter(sw);
				xtw.Formatting = Formatting.Indented;
				xd.WriteTo(xtw);
			}
			finally
			{
				if (xtw != null)
					xtw.Close();
			}
			return sb.ToString();
		}
	}
}
