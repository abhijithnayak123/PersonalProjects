using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
	public class MoneyTransferResponse
	{
		public long TrxId { get; set; }
		public bool HasLPMTError { get; set; }
		public decimal TransferTax { get; set; }
		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "TrxId = ", TrxId, "\r\n");
			str = string.Concat(str, "HasLPMTError = ", HasLPMTError, "\r\n");
			str = string.Concat(str, "TransferTax = ", TransferTax, "\r\n");
			return str;
		}
	}
}
