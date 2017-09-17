using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.WU.Common.Data
{
	public class Country : NexxoModel
	{
		public virtual string CountryCode { get; set; }
		public virtual string Name { get; set; }
	}
}
