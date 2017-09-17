using System;
using System.Runtime.Serialization;

namespace MGI.Peripheral.Server.Data
{
	public class PrintResponse
	{
		[DataMember]
		public Guid UniqueId { get; set; }

		[DataMember]
		public string Result { get; set; }

        [DataMember]
        public string Scan_FrontImageTIFF { get; set; }

        [DataMember]
        public string Scan_BackImageTIFF { get; set; }

        [DataMember]
        public string Scan_FrontImageJPG { get; set; }

        [DataMember]
        public string Scan_BackImageJPG { get; set; }

	}
}
