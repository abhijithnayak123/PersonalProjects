using System;
using MoneyOrderStage = MGI.Core.CXE.Data.Transactions.Stage.MoneyOrder;
using MoneyOrderCommit = MGI.Core.CXE.Data.Transactions.Commit.MoneyOrder;
using System.Collections.Generic;


namespace MGI.Core.CXE.Contract
{
	public interface IMoneyOrderService
	{
		/// <summary>
		/// This stages the money order
		/// </summary>
		/// <param name="moneyOrder">This is money order details</param>
		/// <param name="timezone">This is time zone</param>
		/// <returns>unique identifier of staged money order </returns>
		long Create(MoneyOrderStage moneyOrder, string timezone);

		/// <summary>
		/// This method is to update the status of staged money order 
		/// </summary>
		/// <param name="Id">Unique identifier of staged money order </param>
		/// <param name="newStatus">This is new status of staged money order </param>
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Update(long Id, int newStatus, string timezone);

		/// <summary>
		/// This method is to update the check number,account number, routing number and MICR in  staged money order 
		/// </summary>
		/// <param name="Id">Unique identifier of moneyorder stage</param>
		/// <param name="checkNumber">This is check number of staged money order </param>
		/// <param name="accountNumber">This is account number of staged money order </param>
		/// <param name="routingNumber">This is routing number of staged money order </param>
		/// <param name="micr">This is MICR of staged money order </param>
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Update(long Id, string checkNumber, string accountNumber, string routingNumber, string micr, string timezone);

		/// <summary>
		/// This method is to update the fee of money order stage
		/// </summary>
		/// <param name="Id">Unique identifier of staged money order</param>
		/// <param name="fee">This is fee of staged money order</param>
		/// <param name="timezone">This is time zone</param>
		/// <returns></returns>
		void Update(long Id, decimal fee, string timezone);

		/// <summary>
		/// This method is to commit the staged money order
		/// </summary>
		/// <param name="Id">Unique identifier of staged money order </param>		
		/// <returns></returns>
		void Commit(long Id);

		/// <summary>
		/// This method is to get the committed money order details
		/// </summary>
		/// <param name="Id">Unique identifier of money order commit</param>		
		/// <returns>committed money order details</returns>
		MoneyOrderCommit Get(long Id);

		/// <summary>
		/// This method is to get the staged money order details
		/// </summary>
		/// <param name="Id">Unique identifier of staged money order details</param>		
		/// <returns>Staged money order details</returns>
		MoneyOrderStage GetStage(long Id);
       
        /// <summary>
        /// This method is to get the MoneyOrder MICR  details
        /// </summary>
        /// <param name="micr">MICR value of money order commit</param>		
        /// <returns>committed money order details</returns>
        IList<MoneyOrderCommit> GetMOByMICR(string micr);
	}
}
