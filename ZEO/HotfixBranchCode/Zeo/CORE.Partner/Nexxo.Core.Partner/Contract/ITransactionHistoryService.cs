using MGI.Core.Partner.Data;
using System.Collections.Generic;
using System.Linq.Expressions;
using MGI.Core.Partner.Data.Transactions;
using MGI.Common.Util;

namespace MGI.Core.Partner.Contract
{
	public interface ITransactionHistoryService
	{

		/// <summary>
		/// This method is to persist a collection of transaction 
		/// </summary>
		/// <param name="expression">Compiles the lambda expression described by the expression tree into executable</param>
		List<TransactionHistory> Get(Expression<System.Func<TransactionHistory, bool>> expression);

		/// <summary>
		/// This method is to persist a collection of  past transaction 
		/// </summary>		
		/// <param name="customerId">This is customer id</param>
		/// <param name="transactionType">This is transaction type</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		List<PastTransaction> Get(long customerId, string transactionType, MGIContext mgiContext);
	}
}
