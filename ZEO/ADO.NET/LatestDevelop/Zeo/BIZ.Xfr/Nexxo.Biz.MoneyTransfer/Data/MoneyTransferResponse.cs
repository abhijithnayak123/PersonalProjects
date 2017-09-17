using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
	public class MoneyTransferResponse
	{
		public long TrxId { get; set; }
		public bool HasLPMTError { get; set; }
	}
}
