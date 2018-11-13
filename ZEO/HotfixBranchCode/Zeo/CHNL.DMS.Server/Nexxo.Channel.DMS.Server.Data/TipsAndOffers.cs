using System;
using System.Runtime.Serialization;

namespace MGI.Channel.DMS.Server.Data
{
    [DataContract]
    public class TipsAndOffers
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ViewName { get; set; }
        [DataMember]
        public string ChannelPartnerName { get; set; }
        [DataMember]
        public string TipsAndOffersEn { get; set; }
        [DataMember]
        public string TipsAndOffersEs { get; set; }
        [DataMember]
        public string OptionalFilter { get; set; }
        //[DataMember]
        //public DateTime DTCreate { get; set; }
        //[DataMember]
        //public Nullable<DateTime> DTLastMod { get; set; }
        [DataMember]
        public string TipsAndOffersValue { get; set; }

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "Id = ", Id, "\r\n");
			str = string.Concat(str, "ViewName = ", ViewName, "\r\n");
			str = string.Concat(str, "ChannelPartnerName = ", ChannelPartnerName, "\r\n");
			str = string.Concat(str, "TipsAndOffersEn = ", TipsAndOffersEn, "\r\n");
			str = string.Concat(str, "TipsAndOffersEs = ", TipsAndOffersEs, "\r\n");
			str = string.Concat(str, "OptionalFilter = ", OptionalFilter, "\r\n");
            //str = string.Concat(str, "DTCreate = ", DTCreate, "\r\n");
            //str = string.Concat(str, "DTLastMod = ", DTLastMod, "\r\n");
			str = string.Concat(str, "TipsAndOffersValue = ", TipsAndOffersValue, "\r\n");
			return str;
		}
	}
}
