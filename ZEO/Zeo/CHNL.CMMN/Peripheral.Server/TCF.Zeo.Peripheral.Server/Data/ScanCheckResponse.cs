using System;
using System.IO;
using System.Runtime.Serialization;

namespace TCF.Zeo.Peripheral.Server.Data
{
	public class ScanCheckResponse
	{
		[DataMember]
        public string Scan_FrontImageJPG { get; set; }

        [DataMember]
        public string Scan_BackImageJPG { get; set; }

        [DataMember]
        public string Scan_FrontImageTIFF { get; set; }

        [DataMember]
        public string Scan_BackImageTIFF { get; set; }

        [DataMember]
		public string Scan_FrontImage { get; set; }

		[DataMember]
		public string Scan_BackImage { get; set; }

        [DataMember]
		public string UniqueId { get; set; }

        [DataMember]
        public int MicrError { get; set; }

        [DataMember]
        public string MicrErrorMessage { get; set; }

        [DataMember]
		public string Micr { get; set; }

        [DataMember]
        public string MicrAccountNumber { get; set; }

        [DataMember]
        public String MicrAmount { get; set; }

        [DataMember]
        public String MicrEPC { get; set; }

        [DataMember]
        public String MicrTransitNumber { get; set; }

        [DataMember]
        public String MicrCheckType { get; set; }

        [DataMember]
        public String MicrCountryCode { get; set; }

        [DataMember]
        public String MicrOnUSField { get; set; }

        [DataMember]
        public String MicrAuxillatyOnUSField { get; set; }
	}
}
