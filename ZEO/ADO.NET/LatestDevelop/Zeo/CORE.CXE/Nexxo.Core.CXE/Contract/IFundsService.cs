using MGI.Core.CXE.Data;
using System;
using FundsCommit = MGI.Core.CXE.Data.Transactions.Commit.Funds;
using FundsStage = MGI.Core.CXE.Data.Transactions.Stage.Funds;

namespace MGI.Core.CXE.Contract
{
	public interface IFundsService
	{
		/// <summary>
		/// This stages the fund transaction
		/// </summary>
		/// <param name="funds">This is fund stage details</param>
		/// <returns>Unique identifier of fund stage</returns>
		long Create(FundsStage funds);

		/// <summary>
		/// This method is to update the transaction state of staged fund
		/// </summary>
		/// <param name="Id">Unique identifier of staged fund</param>
		/// <param name="state">Transaction state to be updated in staged fund</param>
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Update(long Id, TransactionStates state, string timezone);

		/// <summary>
		/// This method is to commit staged fund
		/// </summary>
		/// <param name="Id">Unique identifier of staged fund</param>		
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Commit(long Id, string timezone);

		/// <summary>
		/// This method is to get the committed fund details
		/// </summary>
		/// <param name="Id">Unique identifier of fund commit</param>				
		/// <returns>Committed fund details</returns>
		FundsCommit Get(long Id);

		/// <summary>
		/// This method is to update the amount of staged fund
		/// </summary>
		/// <param name="Id">Unique identifier of staged fund</param>
		/// <param name="amount">Amount to be updated in staged fund</param>
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void UpdateAmount(long Id, decimal amount, string timezone);
	}
}
