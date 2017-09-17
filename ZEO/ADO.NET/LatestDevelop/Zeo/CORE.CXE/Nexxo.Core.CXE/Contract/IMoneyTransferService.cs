using MGI.Core.CXE.Data;
using MoneyTransferCommit = MGI.Core.CXE.Data.Transactions.Commit.MoneyTransfer;
using MoneyTransferStage = MGI.Core.CXE.Data.Transactions.Stage.MoneyTransfer;

namespace MGI.Core.CXE.Contract
{
	public interface IMoneyTransferService
	{

		/// <summary>
		/// This stages money transfer 
		/// </summary>
		/// <param name="moneytransfer">This is money transfer details</param>
		/// <returns>unique identifier of staged money transfer /returns>
		long Create(MoneyTransferStage moneytransfer);

		/// <summary>
		/// This method is to update the transaction state of staged money transfer 
		/// </summary>
		/// <param name="Id">Unique identifier of staged money transfer </param>
		/// <param name="state">This is transaction state of staged money transfer </param>		
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Update(long Id, TransactionStates state, string timezone);

		/// <summary>
		/// This method is to update the transaction state and confirmation number of staged money transfer 
		/// </summary>
		/// <param name="Id">Unique identifier of staged money transfer </param>
		/// <param name="state">This is transaction state of staged money transfer </param>	
		/// <param name="confirmationNumber">This is confirmation number of staged money transfer </param>
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Update(long Id, TransactionStates state, string timezone, string confirmationNumber);

		/// <summary>
		/// This method is to commit the staged money transfer
		/// </summary>
		/// <param name="Id">Unique identifier of staged money transfer </param>
		/// <returns></returns>
		void Commit(long Id);

		/// <summary>
		/// This method is to get the committed money transfer details
		/// </summary>
		/// <param name="Id">Unique identifier of money transfer commit</param>
		/// <returns>Committed money transfer details</returns>
		MoneyTransferCommit Get(long Id);

		/// <summary>
		/// This method is to get the staged money transfer details
		/// </summary>
		/// <param name="Id">Unique identifier of money transfer stage</param>
		/// <returns>Money transfer stage details</returns>
		MoneyTransferStage GetStage(long Id);

		/// <summary>
		/// This method is to update date time of staged money transfer details
		/// </summary>
		/// <param name="moneytransfer">This is staged money transfer details</param>
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Update(MoneyTransferStage moneytransfer, string timezone);
	}
}
