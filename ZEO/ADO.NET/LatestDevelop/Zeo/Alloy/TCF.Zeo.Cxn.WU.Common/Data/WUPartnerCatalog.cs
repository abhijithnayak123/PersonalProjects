using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCF.Zeo.Cxn.WU.Common.Data
{
	public class WUPartnerCatalog : ZeoModel
	{
		public virtual string BillerName {get;set;}
		public virtual int ChannelPartnerId {get;set;}
		public virtual int ProviderId {get;set;}		
	}
}
