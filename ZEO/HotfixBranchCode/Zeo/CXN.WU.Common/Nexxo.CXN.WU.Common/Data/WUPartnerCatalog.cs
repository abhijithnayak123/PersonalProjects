using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.WU.Common.Data
{
	public class WUPartnerCatalog : NexxoModel
	{
		public virtual string BillerName {get;set;}
		public virtual int ChannelPartnerId {get;set;}
		public virtual int ProviderId {get;set;}		
	}
}
