using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MGI.Common.DataAccess.Contract;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Common.Util;

namespace MGI.Core.Partner.Impl
{
	public class TransactionHistoryServiceImpl : ITransactionHistoryService
	{
		public IRepository<TransactionHistory> TransactionHistoryRepo { private get; set; }
        public IRepository<PastTransaction> PastTransactionRepo { private get; set; }

		public List<TransactionHistory> Get(Expression<System.Func<TransactionHistory, bool>> expression)
		{
			// todo: check empty scenario
			IQueryable<TransactionHistory> transactionHistory = TransactionHistoryRepo.FilterBy(expression).OrderByDescending(txn => txn.TransactionDate);
			return transactionHistory.ToList<TransactionHistory>();
		}

        public List<PastTransaction> Get(long customerId, string transactionType, MGIContext mgiContext)
        {
            IQueryable<PastTransaction> pastTransaction = PastTransactionRepo.FilterBy(x => x.TransactionType == transactionType && x.CustomerId == customerId).OrderByDescending(txn => txn.DTTerminalLastModified);
            return pastTransaction.ToList<PastTransaction>();
        }
	}
}
