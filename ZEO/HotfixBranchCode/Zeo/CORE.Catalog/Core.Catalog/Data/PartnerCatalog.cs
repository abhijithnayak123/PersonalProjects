using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Catalog.Data
{
	public class PartnerCatalog : NexxoModel
	{
		//public virtual long Id { get; set; }
		public virtual System.Guid PartnerCatalogPK { get; set; }
		public virtual string BillerName { get; set; }
        public virtual string BillerCode { get; set; }
        public virtual string Keywords { get; set; }
		public virtual int ChannelPartnerId { get; set; }
		public virtual int ProviderId { get; set; }
		public virtual MasterCatalog MasterCatalog { get; set; }
	}
}
