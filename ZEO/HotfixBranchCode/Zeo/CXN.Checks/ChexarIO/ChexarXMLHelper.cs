using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ChexarIO
{
	public static class ChexarXMLHelper
	{
		public static decimal GetDecimalToken(XElement xml, string tag)
		{
			return decimal.Parse(GetXMLValue(xml, tag));
		}

		public static int GetIntToken(XElement xml, string tag)
		{
			int tagValue = int.MinValue;
			int.TryParse(GetXMLValue(xml, tag), out tagValue);
			return tagValue;
		}

		public static DateTime GetDateTimeToken(XElement xml, string tag)
		{
			return DateTime.Parse(GetXMLValue(xml, tag));
		}

		public static bool GetBoolToken(XElement xml, string tag)
		{
			return Convert.ToBoolean(GetIntToken(xml, tag));
		}

		public static string GetXMLValue(XElement xml, string tag)
		{
			return xml.Element(tag).Value;
		}
	}
}
