using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace MGI.Channel.DMS.Server.Data
{
	public class ShippingTypes
	{
		[DataMember]
		public string Code { get; set; }

		[DataMember]
		public string Name { get; set; }
	}
}
