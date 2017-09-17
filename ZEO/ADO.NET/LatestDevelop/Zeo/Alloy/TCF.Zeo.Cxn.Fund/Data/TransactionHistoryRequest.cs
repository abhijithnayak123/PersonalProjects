using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Cxn.Fund.Data
{
	public class TransactionHistoryRequest
	{
		public long AliasId { get; set; }
		public Helper.TransactionStatus TransactionStatus { get; set; }
		public int DateRange { get; set; }
	}
}
