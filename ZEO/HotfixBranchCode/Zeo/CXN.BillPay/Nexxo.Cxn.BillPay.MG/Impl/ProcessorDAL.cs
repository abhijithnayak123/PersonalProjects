using MGI.Common.DataAccess.Contract;
using MGI.Cxn.BillPay.MG.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Cxn.BillPay.MG.Impl
{
	public class ProcessorDAL
	{
		#region Dependencies
		public IRepository<Transaction> MGTransactionRepo { get; set; }
        public IRepository<BillerLimit> MgBillerLimitRepo { get; set; }
        public IRepository<BillerDenomination> MgBillerDenominationRepo { get; set; }  
		#endregion

		public long AddTransaction(Transaction transaction)
		{
            MGTransactionRepo.AddWithFlush(transaction);
            return transaction.Id;
		}

		public Transaction GetTransactionxById(long transactionId)
		{
            Transaction trx = MGTransactionRepo.FindBy(a => a.Id == transactionId);
			return trx;
		}

        public BillerLimit GetBillerLimit(string billerCode)
        {
            return MgBillerLimitRepo.FindBy(x => x.ReceiveCode == billerCode);
        }

        public Transaction GetBillerLastTransaction( string billerCode, long accountId)
        {
            return MGTransactionRepo.FilterBy(a => a.ReceiveCode == billerCode && a.Account.Id == accountId && a.ReferenceNumber != null ).OrderByDescending(a => a.DTTerminalCreate).FirstOrDefault();
        }
	}
}
