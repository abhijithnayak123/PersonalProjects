using System;

using MGI.Biz.Partner.Data;
using System.Collections.Generic;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Contract
{
	public interface IShoppingCartService
	{
        /// <summary>
        /// To fetch the transaction and add the transaction to shopping cart. 
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Shopping Cart Details</returns>
		ShoppingCart Get(long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to Update the Shopping Cart Status.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="Status">This is status of shopping cart</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void Update(long customerSessionId, ShoppingCartStatus Status, MGIContext mgiContext);

        /// <summary>
        /// This method to upadate IsReferral property to true or false. By Default its false.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="isReferral">This is referral status of shopping cart</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void Update(long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to fetch shopping cart details based on shopping cart id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="shoppingCartId">This is unique identifier of shopping cart.</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Shopping Cart Details</returns>
		ShoppingCart GetCartById(long customerSessionId, long shoppingCartId, MGIContext mgiContext);
        
        #region Add transaction Methods

        /// <summary>
        /// This method to add check transaction to shopping cart by check transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="checkId">This is unique identifier for check transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void AddCheck(long customerSessionId, long checkId, MGIContext mgiContext);

        /// <summary>
        /// This method to add fund transaction to shopping cart by fund transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="fundsId">This is unique identifier for fund transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
		void AddFunds(long customerSessionId, long fundsId, MGIContext mgiContext);

        /// <summary>
        /// This method to add billpay transaction to shopping cart by billpay transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="billPayId">This is unique identifier for billpay transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
		void AddBillPay(long customerSessionId, long billPayId, MGIContext mgiContext);

        /// <summary>
        /// This method to add money order transaction to shopping cart by money order transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="moneyOrderId">This is unique identifier for money order transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void AddMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext);

        /// <summary>
        /// This method to add money trnasfer transaction to shopping cart by money transfer transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="moneyTransferId">This is unique identifier for money transfer transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
		void AddMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext);


        /// <summary>
        /// This method to add cash transaction to shopping cart by cash transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="cashTxnId">This is unique identifier for cash transaction</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
		void AddCash(long customerSessionId, long cashTxnId, MGIContext mgiContext);
        
        #endregion

        #region Remove transaction Methods

        /// <summary>
        /// This method to remove check transaction from shopping cart by check transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="checkId">This is unique identifier for check transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        bool RemoveCheck(long customerSessionId, long checkId, bool isParkedTransaction, MGIContext mgiContext);

        /// <summary>
        /// This method to remove fund transaction from shopping cart by fund transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="fundsId">This is unique identifier for fund transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        void RemoveFunds(long customerSessionId, long fundsId, bool isParkedTransaction, MGIContext mgiContext);

        /// <summary>
        /// This method to remove billpay transaction from shopping cart by billpay transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="billPayId">This is unique identifier for billpay transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        void RemoveBillPay(long customerSessionId, long billPayId, bool isParkedTransaction, MGIContext mgiContext);

        /// <summary>
        /// This method to remove money order transaction from shopping cart by money order transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="moneyOrderId">This is unique identifier for money order transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        void RemoveMoneyOrder(long customerSessionId, long moneyOrderId, bool isParkedTransaction, MGIContext mgiContext);

        /// <summary>
        /// This method to remove money transfer transaction from shopping cart by money transfer transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="moneyTransferId">This is unique identifier for money transfer transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        void RemoveMoneyTransfer(long customerSessionId, long moneyTransferId, bool isParkedTransaction, MGIContext mgiContext);

		/// <summary>
		/// This method to remove cash in transaction
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="cashInId"></param>
		/// <param name="mgiContext"></param>
		void RemoveCashIn(long customerSessionId, long cashInId, MGIContext mgiContext);

        #endregion

        /// <summary>
        /// This method to closed shopping cart for current customer session.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		void CloseShoppingCart(long customerSesionId, MGIContext mgiContext);

        #region Park transaction Methods

        /// <summary>
        /// This method to park check transaction by check transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="checkId">This is unique identifier for check transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void ParkCheck(long customerSessionId, long checkId, MGIContext mgiContext);

        /// <summary>
        /// This method to park fund transaction by fund transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="fundsId">This is unique identifier for fund transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void ParkFunds(long customerSessionId, long fundsId, MGIContext mgiContext);

        /// <summary>
        /// This method to park billpay transaction by billpay transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="billPayId">This is unique identifier for billpay transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void ParkBillPay(long customerSessionId, long billPayId, MGIContext mgiContext);

        /// <summary>
        /// This method to park money transfer transaction by money transfer transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="moneyTransferId">This is unique identifier for money transfer transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void ParkMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext);

        /// <summary>
        /// This method to park money order transaction by money order transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="moneyOrderId">This is unique identifier for money order transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void ParkMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext);

        #endregion

        /// <summary>
        /// To fetch the all transaction based on shopping cart id. To print Summary Receipt. 
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="cart">A transient instance of ShoppingCart[Class]</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Shopping Cart Details</returns>
		ShoppingCart PopulateCart(long customerSessionId, MGI.Core.Partner.Data.ShoppingCart cart, MGIContext mgiContext);

        /// <summary>
        /// This method to commit all transaction, which are all there in shopping cart excluding park transaction.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="cashToCustomer">This parameter show the cash back to customer after transaction.</param>
        /// <param name="cashCollected">This parameter show the cash collected from customer for transaction.</param>
        /// <param name="priorCashCollected">This parameter show the previously cash collected from customer</param>
        /// <param name="cardNumber">This is unique identifier for customer</param>
        /// <param name="shoppingCartstatus">This parameter show the status of shopping cart checkout [Like for Money Order MOPrinting, Complete etc.]</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Shopping cart checkout status</returns>
		ShoppingCartCheckoutStatus Checkout(long customerSessionId, decimal cashToCustomer, string cardNumber, ShoppingCartCheckoutStatus shoppingCartstatus, MGIContext mgiContext);
		
        /// <summary>
        /// This method to Enable or Disable the referral section in shopping cart checkout page. This is configured with channel partner.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>referral section Enable or Disable Status.</returns>
		bool IsReferralApplicable(long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// Pushs all successfully committed transactions to client customer information system [TCF]
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param> 
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void PostFlush(long customerSessionId, MGIContext mgiContext);

        List<ParkedTransaction> GetAllParkedShoppingCartTransactions();
	}
}
