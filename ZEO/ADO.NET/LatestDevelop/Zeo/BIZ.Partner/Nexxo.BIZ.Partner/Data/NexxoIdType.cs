using System;

namespace MGI.Biz.Partner.Data
{
	public class NexxoIdType
	{
		public virtual Guid rowguid { get; set; }
		public virtual long Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Mask { get; set; }
		public virtual bool HasExpirationDate { get; set; }
		public virtual string Country { get; set; }
		public virtual string State { get; set; }
	}
}
