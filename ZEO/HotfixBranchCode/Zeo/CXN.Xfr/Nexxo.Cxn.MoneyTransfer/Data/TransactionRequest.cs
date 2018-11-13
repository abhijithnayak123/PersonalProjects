using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.Data
{
	public class TransactionRequest
	{
		public long TransactionId { get; set; }
		public string ConfirmationNumber { get; set; }
		public long AccountId { get; set; }

		public  TransactionRequestType  TransactionRequestType {get; set;}

	}
}
