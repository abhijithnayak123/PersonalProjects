using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Compliance.Data
{
	public enum TransactionTypes : int
	{
		Cash = 1,
		Check = 2,
		Funds = 3,
		BillPay = 4,
		MoneyOrder = 5,
		MoneyTransfer = 6,
		CashWithdrawal = 7,
		LoadToGPR = 8,
		ActivateGPR = 9,
		DebitGPR = 10
	}
}
