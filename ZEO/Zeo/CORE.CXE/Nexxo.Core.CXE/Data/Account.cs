using System;

using MGI.Common.DataAccess.Data;

namespace MGI.Core.CXE.Data
{
	public class Account : NexxoModel
	{
		public virtual Customer Customer { get; set; }
		public virtual int Type { get; set; }
	}
}