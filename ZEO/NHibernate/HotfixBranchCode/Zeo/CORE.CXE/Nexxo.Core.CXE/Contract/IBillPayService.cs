using MGI.Core.CXE.Data;
using BillPayCommit = MGI.Core.CXE.Data.Transactions.Commit.BillPay;
using BillPayStage = MGI.Core.CXE.Data.Transactions.Stage.BillPay;

namespace MGI.Core.CXE.Contract
{
	public interface IBillPayService
	{
		/// <summary>
		/// This stages the bill pay
		/// </summary>
		/// <param name="billPay"> Bill pay stage details</param>
		/// <returns>bill pay stage id</returns>
		long Create(BillPayStage billPay);

		/// <summary>
		/// This method is to update the transaction status and confirmation number in 'BillPayStage' table.
		/// </summary>
		/// <param name="Id">This is unique identifier for bill pay stage</param>
		/// <param name="state">This is status to be updated in bill pay stage</param>
		/// <param name="confirmationNumber">This is confirmation number to be updated in bill pay stage</param>
		/// <returns></returns>
		void Update(long Id, TransactionStates state, string confirmationNumber);

		/// <summary>
		/// This method is to update the status,fee and confirmation number in 'BillPayStage' table.
		/// </summary>
		/// <param name="Id">This is unique identifier for bill pay stage</param>
		/// <param name="state">This is status to be updated in bill pay stage</param>
		/// <param name="confirmationNumber">This is confirmation number to be updated in bill pay stage</param>
		/// <param name="fee">This is fee to be updated in bill pay stage</param>
		/// <returns></returns>
		void Update(long Id, TransactionStates state, string confirmationNumber, decimal fee);

		/// <summary>
		/// This method is to update the biller name,account number and amount in 'BillPayStage' table.
		/// </summary>
		/// <param name="Id">This is unique identifier for bill pay stage</param>
		/// <param name="billerName">This is biller name to be updated in stged bill pay</param>
		/// <param name="accountNumber">This is account number to be updated in staged bill pay</param>
		/// <param name="amount">This is amount for bill pay stage</param>
		/// <returns></returns>
		void Update(long Id, string billerName, string accountNumber, decimal amount);

		/// <summary>
		/// This method is to commit the bill pay
		/// </summary>
		/// <param name="Id">This is unique identifier of committed bill pay</param>
		/// <returns></returns>
		void Commit(long Id);

		/// <summary>
		/// This method is to get the committed bill pay details
		/// </summary>
		/// <param name="Id">This is unique identifier of bill pay commit</param>
		/// <returns>committed bill pay details</returns>
		BillPayCommit Get(long Id);
	}
}
