using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class CardInfo
	{
		[DataMember]
		public string PromoCode { get; set; }
		[DataMember]
		public string TotalPointsEarned { get; set; }
	}
}
