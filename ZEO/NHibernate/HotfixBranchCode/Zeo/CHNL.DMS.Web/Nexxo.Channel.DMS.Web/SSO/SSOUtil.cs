using System;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Linq;

namespace MGI.Channel.DMS.Web.SSO
{
	public static class SSOUtil
	{
		/// <summary>
		/// This method decodes SAML Base64 string and returns the decoded SAML XML Request as string
		/// </summary>
		/// <param name="base64EncodedData"></param>
		/// <returns></returns>
		public static string Base64Decode(string base64EncodedData)
		{
			if (base64EncodedData == null)
				return string.Empty;
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.ASCII.GetString(base64EncodedBytes);
		}

		/// <summary>
		/// This creates saml namespace
		/// </summary>
		/// <param name="assertion"></param>
		/// <returns></returns>
		public static XmlNamespaceManager AddSAMLNameSpace(XmlDocument assertion)
		{
			var ns = new XmlNamespaceManager(assertion.NameTable);
			ns.AddNamespace("samlp", @"urn:oasis:names:tc:SAML:2.0:protocol");
			ns.AddNamespace("saml", @"urn:oasis:names:tc:SAML:2.0:assertion");
			ns.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
			return ns;
		}

		public static string GetIssuer(string samlResponse)
		{
			string issuername = string.Empty;
			try
			{
				string decodedResponse = Base64Decode(samlResponse);
				byte[] byteArray = Encoding.UTF8.GetBytes(decodedResponse);
				MemoryStream stream = new MemoryStream(byteArray);
				XElement doc = XElement.Load(stream);
				// Add another from AddSAMLNameSpace
				issuername = doc.Elements()
					.Where(x => x.Attributes().Any(y => y.Value == @"urn:oasis:names:tc:SAML:2.0:assertion"))
					.Select(a => a.Value).FirstOrDefault();

				//assertion.InnerXml = decodedResponse;
				////Purpose: On Vera Code Scan, Improper Restriction of XML External Entity Reference ('XXE') FLaw,
				////to overcome it we are making xmlresolver null
				//assertion.XmlResolver = null;
				//XmlNamespaceManager ns2 = AddSAMLNameSpace(assertion);
				//issuername = assertion.SelectSingleNode("/samlp:Response/saml:Issuer", ns2).InnerText;
			}
			catch
			{
				throw new Exception(SSOErrorCodes.SAMLParseError.ToString());
			}
			return issuername;
		}

		public static Dictionary<string, object> SSOChannelPartner()
		{
			Dictionary<string, object> ssoPartner = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
			ssoPartner.Add("tcf-okta", "TCF");
			ssoPartner.Add("tcf", "TCF");
			ssoPartner.Add("synovus-okta", "Synovus");
			//ssoPartner.Add("synovus", "Synovus");
			ssoPartner.Add("carver-okta", "Carver");
			ssoPartner.Add("carver", "Carver");
			ssoPartner.Add("mgi-okta", "MGI");
			ssoPartner.Add("mgi", "MGI");
			ssoPartner.Add("redstone", "Redstone");
			ssoPartner.Add("redstone-okta", "Redstone");
			return ssoPartner;
		}
	}
}