using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
    [DataContract]
    public class MoneyOrderData
    {
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string PromotionCode { get; set; }
		[DataMember]
		public bool IsSystemApplied { get; set; }
    }
}
