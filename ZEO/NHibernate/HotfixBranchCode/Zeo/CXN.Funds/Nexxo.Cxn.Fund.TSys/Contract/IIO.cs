using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Cxn.Fund.TSys.Data;

namespace MGI.Cxn.Fund.TSys.Contract
{
	public interface IIO
	{

        /// <summary>
        /// This method is used for validate new card account
        /// </summary>
        /// <param name="programId">Account number</param>
        /// <param name="kitId">Card number</param>
        /// /// <param name="cardNumber">Card number</param>
        /// <returns>TSys card details</returns>
		TSysIONewUser ValidateNewCardAccount(long programId, string kitId, long cardNumber);

        /// <summary>
        /// This method is used for existing TSys cardholders that are imported at registration time
        /// </summary>
        /// <param name="programId">Account number</param>
        /// <param name="kitId">Card number</param>
        /// <returns>TSys card details</returns>
		TSysIONewUser ValidateExistingCardAccount(long programId, string kitId);

        /// <summary>
        /// This method is used for validate card and account status
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="accountId">Account number</param>
        /// <param name="cardNumber">Card number</param>
		void ValidateCard(long userId, long accountId, long cardNumber);

        /// <summary>
        /// This method is used to get active card
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="accountId">Account number</param>
        /// <returns>Activated card number</returns>
		long GetActiveCard(long userId, long accountId);

        /// <summary>
        /// This method is used for update card and account status
        /// </summary>
        /// <param name="account">Account details</param>
        void UpdateCardAccount(TSysIOProfile account);

        /// <summary>
        /// This method is used to activate card account
        /// </summary>
        //// <param name="account">Account details</param>
		void ActivateCardAccount(TSysIOProfile account);

        /// <summary>
        /// This method is used for load card
        /// </summary>
        /// <param name="cardNumber">Card Number</param>
        /// <param name="amount">Amount</param>
        /// <param name="description">Description</param>
        /// <returns>Confirmation number</returns>
		string Load(string cardNumber, decimal amount, string description);

        /// <summary>
        /// This method is used for withdraw money from card
        /// </summary>
        /// <param name="accountId">Account Number</param>
        /// <param name="amount">Amount</param>
        /// <param name="description">Description</param>
        /// <returns>Confirmation Number</returns>
		string Withdraw(long accountId, decimal amount, string description);

        /// <summary>
        /// This method is used for apply fee to the card
        /// </summary>
        /// <param name="accountId">Account Number</param>
        /// <param name="fee">Amount</param>
        /// <param name="description">Description</param>
        /// <returns>Transaction id </returns>
		string ApplyFee(long accountId, decimal fee, string description);

        /// <summary>
        /// This method is used to get the balance of the card
        /// </summary>
        /// <param name="accountId">Account Number</param>
        /// <returns>Amount</returns>
		decimal GetBalance(long accountId);
	}
}
