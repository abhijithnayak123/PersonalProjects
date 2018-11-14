using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MGI.Common.DataAccess.Contract;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Common.Util;
using System;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Core.Partner.Impl
{
	public class TransactionHistoryServiceImpl : ITransactionHistoryService
	{
		public IRepository<TransactionHistory> TransactionHistoryRepo { private get; set; }
        public IRepository<PastTransaction> PastTransactionRepo { private get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }

		public List<TransactionHistory> Get(Expression<System.Func<TransactionHistory, bool>> expression)
		{
			try
			{
				// todo: check empty scenario
				IQueryable<TransactionHistory> transactionHistory = TransactionHistoryRepo.FilterBy(expression).OrderByDescending(txn => txn.TransactionDate);
				return transactionHistory.ToList<TransactionHistory>();
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(expression), "Get", AlloyLayerName.CORE, ModuleName.Transaction,
							"Error in Get - MGI.Core.Partner.Impl.TransactionHistoryServiceImpl", ex.Message, ex.StackTrace);
				throw new TransactionHistoryException(TransactionHistoryException.GET_TRANSACTION_FAILED, ex);
			}
		}

        public List<PastTransaction> Get(long customerId, string transactionType, MGIContext mgiContext)
        {
			try
			{
				IQueryable<PastTransaction> pastTransaction = PastTransactionRepo.FilterBy(x => x.TransactionType == transactionType && x.CustomerId == customerId).OrderByDescending(txn => txn.DTTerminalLastModified);
				return pastTransaction.ToList<PastTransaction>();
			}
			catch (Exception ex)
			{
				MongoDBLogger.Error<string>(Convert.ToString(customerId), "Get", AlloyLayerName.CORE, ModuleName.Transaction,
							"Error in Get - MGI.Core.Partner.Impl.TransactionHistoryServiceImpl", ex.Message, ex.StackTrace);
				throw new TransactionHistoryException(TransactionHistoryException.GET_PAST_TRANSACTION_FAILED, ex);
			}
        }
	}
}
