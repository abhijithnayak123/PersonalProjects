using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class CardInfo
	{
        [DataMember]
        public virtual string PromoCode { get; set; }
        [DataMember]
        public virtual string TotalPointsEarned { get; set; }
	}
}
