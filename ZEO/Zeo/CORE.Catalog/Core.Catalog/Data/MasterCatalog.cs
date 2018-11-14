using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Catalog.Data
{
	public class MasterCatalog : NexxoModel
	{		
		public virtual long ProviderCatalogId { get; set; }
        public virtual string BillerCode { get; set; }
		public virtual string BillerName { get; set; }
        public virtual string Keywords { get; set; }
		public virtual int ChannelPartnerId { get; set; }
		public virtual int ProviderId { get; set; }
		public virtual System.Nullable<int> ProductType { get; set; }
		public virtual string LogoURL { get; set; }
		public virtual bool IsActive { get; set; }
		public virtual IList<PartnerCatalog> PartnerCatalogs { get; set; }
	}
}
