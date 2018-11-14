using System;
using System.Collections.Generic;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Cxn.WU.Common.Data
{
	public class WUMasterCatalog : ZeoModel
	{		
		public virtual long ProviderCatalogId {get;set;}
		public virtual string BillerName {get;set;}
		public virtual long ChannelPartnerId {get;set;}
		public virtual int ProviderId {get;set;}
		public virtual int ProductType {get;set;}
		public virtual bool IsActive{get;set;}
		public virtual string LogoURL { get; set; }
		public virtual IList<WUPartnerCatalog> PartnerCatalogs { get; set; }
	}
}
