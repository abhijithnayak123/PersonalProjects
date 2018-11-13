using MGI.Core.CXE.Data;
using System;
using CashCommit = MGI.Core.CXE.Data.Transactions.Commit.Cash;
using CashStage = MGI.Core.CXE.Data.Transactions.Stage.Cash;

namespace MGI.Core.CXE.Contract
{
	public interface ICashService
	{
		/// <summary>
		/// This stages the cash transaction
		/// </summary>
		/// <param name="cash">This is cash stage details</param>
		/// <returns>Unique identifier of cash stage</returns>
		long Create(CashStage cash);

		/// <summary>
		/// This committed the bill pay transaction
		/// </summary>
		/// <param name="Id">This is unique identifier of staged cash transaction</param>	
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Commit(long Id, string timezone);


		/// <summary>
		/// This method is to update the transaction state of staged cash
		/// </summary>
		/// <param name="Id">Unique identifier of staged cash</param>
		/// <param name="state">Transaction state to be updated in staged cash</param>
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Update(long Id, TransactionStates state, string timezone);


		//AL-2729 user story Used for updating the cash-in transaction
		/// <summary>
		/// Used for updating the cash transaction type amount during multiple transactions
		/// </summary>
		/// <param name="Id">This is unique identifier of staged cash transaction</param>
		/// <param name="amount">Amount to be updated</param>
		/// <param name="timezone">his is time zone</param>
		void UpdateAmount(long Id, decimal amount, string timezone);
	}
}
