using System;

namespace MGI.Core.Partner.Data
{
	public class NexxoIdType
	{
		public virtual Guid rowguid { get; set; }
		public virtual long Id { get; set; }
		public virtual string Name { get; set; }
		public virtual string Mask { get; set; }
		public virtual bool HasExpirationDate { get; set; }
		public virtual MasterCountry CountryId { get; set; }
		public virtual State StateId { get; set; }
		public virtual bool IsActive { get; set; }
	}
}
