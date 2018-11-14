using System;

namespace MGI.Core.Partner.Data.Transactions
{
	public enum TransactionType : int
	{
		Cash = 1,
		Check = 2,
		Funds = 3,
		BillPay = 4,
		MoneyOrder = 5,
		MoneyTransfer = 6
	}
}
