using MGI.Common.DataAccess.Data;
using System;

namespace MGI.Cxn.Fund.Visa.Data
{
	public class CardClass : NexxoModel
	{
		public virtual string StateCode { get; set; }
		public virtual int Class { get; set; }
		public virtual long ChannelPartnerId { get; set; }
	}
}
