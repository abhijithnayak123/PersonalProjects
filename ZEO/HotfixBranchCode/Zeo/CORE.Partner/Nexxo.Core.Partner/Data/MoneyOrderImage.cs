using MGI.Common.DataAccess.Data;
using MGI.Core.Partner.Data.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public class MoneyOrderImage : NexxoModel
	{
		public virtual Guid TrxId { get; set; }
		public virtual byte[] CheckFrontImage { get; set; }
		public virtual byte[] CheckBackImage { get; set; }
		public virtual MoneyOrder MoneyOrder { get; set; }
	}
}
