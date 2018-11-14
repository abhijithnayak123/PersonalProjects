using System;
using System.Collections.Generic;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;


namespace MGI.Core.Partner.Contract
{
	public interface ITransactionService<T> where T : Transaction
	{
		/// <summary>
		/// This method is to persist a transaction to the database
		/// </summary>
		/// <param name="transaction">This is transaction details to persist</param>
		void Create(T transaction);

		/// <summary>
		/// This method is to get the transaction details
		/// </summary>
		/// <param name="Id">This is transaction Id</param>
		/// <returns></returns>
		T Lookup(long Id);

		/// <summary>
		/// This method is to update the CXE Status stored in the transaction table for a specific transaction
		/// </summary>
		/// <param name="Id">This is transaction Id</param>
		/// <param name="CXEState">This is state of the transaction in the CXE space</param>
		void UpdateCXEStatus(long Id, int CXEState);

		/// <summary>
		/// This method is to update the CXN status stored in the transaction table for a specific transaction
		/// </summary>
		/// <param name="Id">This is transaction Id</param>
		/// <param name="CXNState">This is state of the transaction in the CXN space</param>
		void UpdateCXNStatus(long Id, int CXNState);

		/// <summary>
		/// This method is to update the CXN status stored in the transaction table for a specific transaction
		/// </summary>
		/// <param name="Id">This is transaction id</param>
		/// <param name="CXNStatus">This is state of the transaction in the CXN space</param>
		/// <param name="cxnId">This is CXN transaction id</param>
		void UpdateCXNStatus(long Id, long cxnId, int CXNStatus);

		/// <summary>
		/// This method is to update the CXN status stored in the transaction table for a specific transaction
		/// </summary>
		/// <param name="Id">This is transaction id</param>
		/// <param name="cxnId">This is CXN transaction id</param>
		/// <param name="CXNStatus">This is state of the transaction in the CXN space</param>
		/// <param name="amount">This is amount for this transaction</param>
		/// <param name="fee">This is fee calculated for this transaction</param>
		/// <param name="confirmationNumber">This is confirmation number</param>
		void UpdateCXNStatus(long Id, long cxnId, int CXNStatus, decimal amount, decimal fee, string confirmationNumber);

		/// <summary>
		///  This method is to update the CXE & CXN States stored in the transaction table for a specific transaction
		/// </summary>
		/// <param name="Id">This is transaction Id</param>
		/// <param name="CXEState">This is state of the transaction in the CXE space</param>
		/// <param name="CXNState">This is state of the transaction in the CXN space</param>
		/// <param name="description">This is description for transaction</param>
		void UpdateStates(long Id, int CXEState, int CXNState, string description = "");

		/// <summary>
		/// This method is to update the CXE, CXN and Confirmation Number for the transaction after commiting the transaction
		/// </summary>
		/// <param name="Id">This is transaction Id</param>
		/// <param name="CXEState">This is state of the transaction in the CXE space</param>
		/// <param name="CXNState">This is state of the transaction in the CXN space</param>
		/// <param name="ConfirmationNumber">This is confirmation number</param>
		/// <param name="Type">This is transaction type</param>
		void UpdateTransactionDetails(long Id, int CXEState, int CXNState, string ConfirmationNumber, int Type);

		/// <summary>
		/// This method is to update the transaction details
		/// </summary>
		/// <param name="transaction">This is transaction details</param>		
		void UpdateTransactionDetails(T transaction);

		/// <summary>
		/// This method is to updates the fee 
		/// </summary>
		/// <param name="Id">This is transaction Id</param>
		/// <param name="Fee">This is fee calculated for this transaction</param>
		/// <returens></returens>
		void UpdateFee(long Id, decimal Fee);

        /// <summary>
		/// This method is to update the amount of the transaction
		/// Required for GPR transaction
		/// </summary>
		/// <param name="Id">This is transaction id</param>
		/// <param name="amount">This is amount for this transaction</param>
        void UpdateAmount(long Id, decimal amount);

		/// <summary>
		/// This method is to  update the entire transaction record
		/// </summary>		
		/// <param name="transaction">This is transaction details</param>
		void Update(T transaction);

		/// <summary>
		/// This method is to retrieves collection of all transaction records for a customer
		/// </summary>
		/// <param name="customerId">Customer ID</param>
		/// <returns>Collection of transaction records</returns>
		List<T> GetAllForCustomer(long customerId);

		/// <summary>
		/// This method is to get collection of all transaction records
		/// </summary>
		/// <param name="CustomerId">This is customer id</param>
		/// <returns>Collection of all transaction records</returns>
		List<T> GetAll(long CustomerId);

	}
}
