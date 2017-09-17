using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.WU.Common.Data
{
	public class WUMasterCatalog : NexxoModel
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
