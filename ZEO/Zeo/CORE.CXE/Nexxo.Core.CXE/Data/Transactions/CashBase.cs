using System;

namespace MGI.Core.CXE.Data.Transactions
{
	public abstract class CashBase : NexxoTransactionModel
	{
        public virtual int CashTrxType {get;set;}
	}
}
