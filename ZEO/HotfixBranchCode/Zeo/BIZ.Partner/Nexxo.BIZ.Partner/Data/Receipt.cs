using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MGI.Biz.Partner.Data
{
	[DataContract]
	public class Receipt
	{
		[DataMember]
		public List<string> Lines;
	}
}
