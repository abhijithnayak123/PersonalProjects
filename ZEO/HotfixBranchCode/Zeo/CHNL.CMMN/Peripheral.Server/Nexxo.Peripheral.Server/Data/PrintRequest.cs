using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MGI.Peripheral.Server.Data
{
	public class PrintRequest
	{
		[DataMember]
		public String ReceiptData { get; set; }

		[DataMember]
		public Guid UniqueId { get; set; }

		[DataMember]
		public String ReceiptType { get; set; }
	}
}
