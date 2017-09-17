using System;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
    [DataContract]
    public class CashDrawer
    {
        public CashDrawer() { }

        [DataMember]
        public DateTime ReportingDate { get; set; }
        [DataMember]
        public string TellerName { get; set; }
        [DataMember]
        public string LocationName { get; set; }
        [DataMember]
        public decimal CashIn { get; set; }
        [DataMember]
        public decimal CashOut { get; set; }
        [DataMember]
        public string ReportTemplate { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "ReportingDate = ", ReportingDate, "\r\n");
			str = string.Concat(str, "TellerName = ", TellerName, "\r\n");
			str = string.Concat(str, "LocationName = ", LocationName, "\r\n");
			str = string.Concat(str, "CashIn = ", CashIn, "\r\n");
			str = string.Concat(str, "CashOut = ", CashOut, "\r\n");
			str = string.Concat(str, "ReportTemplate = ", ReportTemplate, "\r\n");
			return str;
		}
	}
}
