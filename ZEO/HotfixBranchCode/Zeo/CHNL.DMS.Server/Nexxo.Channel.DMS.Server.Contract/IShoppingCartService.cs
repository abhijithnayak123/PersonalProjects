using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;
using MGI.Channel.DMS.Server.Data;


namespace MGI.Channel.DMS.Server.Contract
{
    [ServiceContract]
	public interface IShoppingCartService
    {
        //TODO this method need to be moved to IDesktopService
        void SetSelf(IDesktopService dts);

        //TODO these methods Not used In DMS need to be discussed and removed
        #region Add transaction Methods
        // Platform changes methods starts here
		/// <summary>
		/// This method to add check transaction to shopping cart by check transaction id.
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="checkId">This is unique identifier for check transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        [OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void AddCheck(long customerSessionId, long checkId, MGIContext mgiContext);

        /// <summary>
        /// This method to add fund transaction to shopping cart by fund transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="fundsId">This is unique identifier for fund transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void AddFunds(long customerSessionId, long fundsId, MGIContext mgiContext);

        /// <summary>
        /// This method to add billpay transaction to shopping cart by billpay transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="billPayId">This is unique identifier for billpay transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void AddBillPay(long customerSessionId, long billPayId, MGIContext mgiContext);

        /// <summary>
        /// This method to add money order transaction to shopping cart by money order transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="moneyOrderId">This is unique identifier for money order transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void AddMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext);

        /// <summary>
        /// This method to add money trnasfer transaction to shopping cart by money transfer transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="moneyTransferId">This is unique identifier for money transfer transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		void AddMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext);

        /// <summary>
        /// This method to add cash transaction to shopping cart by cash transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="cashTxnId">This is unique identifier for cash transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        [OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void AddCash(long customerSessionId, long cashTxnId, MGIContext mgiContext);

        #endregion

        #region Remove Transaction Method
        /// <summary>
        /// This method to remove check transaction from shopping cart by check transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="checkId">This is unique identifier for check transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        [OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		bool RemoveCheck(long customerSessionId, long checkId, bool isParkedTransaction, MGIContext mgiContext);

        /// <summary>
        /// This method to remove fund transaction from shopping cart by fund transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="fundsId">This is unique identifier for fund transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        void RemoveFunds(long customerSessionId, long fundsId, bool isParkedTransaction, MGIContext mgiContext);

        /// <summary>
        /// This method to remove billpay transaction from shopping cart by billpay transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="billPayId">This is unique identifier for billpay transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        void RemoveBillPay(long customerSessionId, long billPayId, bool isParkedTransaction, MGIContext mgiContext);

        /// <summary>
        /// This method to remove money order transaction from shopping cart by money order transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="moneyOrderId">This is unique identifier for money order transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        void RemoveMoneyOrder(long customerSessionId, long moneyOrderId, bool isParkedTransaction, MGIContext mgiContext);

        /// <summary>
        /// This method to remove money transfer transaction from shopping cart by money transfer transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="moneyTransferId">This is unique identifier for money transfer transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        void RemoveMoneyTransfer(long customerSessionId, long moneyTransferId, bool isParkedTransaction, MGIContext mgiContext);

		/// <summary>
		/// This method to remove CashIn transaction
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <param name="cashId"></param>
		/// <param name="mgiContext"></param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void RemoveCashIn(long customerSessionId, long cashId, MGIContext mgiContext);

        #endregion

        // Platform changes methods ends here
        /// <summary>
        /// This method to get all transaction from shopping cart by customerSessionId.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List of Transaction</returns>
		[OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		ShoppingCart ShoppingCart(long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to commit all transaction, which are all there in shopping cart excluding park transaction.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="cashToCustomer">This parameter show the cash back to customer after transaction.</param>
        /// <param name="cashCollected">This parameter show the cash collected from customer for transaction.</param>
        /// <param name="priorCashCollected">This parameter show the previously cash collected from customer</param>
        /// <param name="cardNumber">This is unique identifier for customer</param>
        /// <param name="shoppingCartstatus">This parameter show the status of shopping cart checkout [Like for Money Order MOPrinting, Complete etc.]</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Shopping cart checkout status</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		ShoppingCartCheckoutStatus Checkout(long customerSessionId, decimal cashToCustomer, string cardNumber, ShoppingCartCheckoutStatus shoppingCartstatus, MGIContext mgiContext);

        /// <summary>
        /// This method to get receipts for all commited transaction.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="shoppingCartId">This is unique identifier for Shopping cart</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Receipts GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, MGIContext mgiContext);

        /// <summary>
        /// This method to closed shopping cart for current customer session.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		void CloseShoppingCart(long customerSessionId, MGIContext mgiContext);

        #region Park Transaction Methods
        /// <summary>
        /// This method to park check transaction by check transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="checkId">This is unique identifier for check transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		void ParkCheck(long customerSessionId, long checkId, MGIContext mgiContext);

        /// <summary>
        /// This method to park fund transaction by fund transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="fundsId">This is unique identifier for fund transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		void ParkFunds(long customerSessionId, long fundsId, MGIContext mgiContext);

        /// <summary>
        /// This method to park billpay transaction by billpay transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="billPayId">This is unique identifier for billpay transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		void ParkBillPay(long customerSessionId, long billPayId, MGIContext mgiContext);

        /// <summary>
        /// This method to park money transfer transaction by money transfer transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="moneyTransferId">This is unique identifier for money transfer transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		void ParkMoneyTransfer(long customerSessionId, long moneyTransferId, MGIContext mgiContext);

        /// <summary>
        /// This method to park money order transaction by money order transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="moneyOrderId">This is unique identifier for money order transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		void ParkMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext);
        #endregion

        /// <summary>
        /// This method to re submit the check by check transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="checkId">This is unique identifier for check transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
		void ReSubmitCheck(long customerSessionId, long checkId, MGIContext mgiContext);

        /// <summary>
        /// This method to re submit the money order by money order transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="moneyOrderId">This is unique identifier for money order transaction</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
		void ReSubmitMO(long customerSessionId, long moneyOrderId, MGIContext mgiContext);

        /// <summary>
        /// This method to Enable or Disable the referral section in shopping cart checkout page. This is configured with channel partner.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>referral section Enable or Disable Status.</returns>
		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
		bool IsReferralApplicable(long customerSessionId, MGIContext mgiContext);
        
        /// <summary>
        /// Pushs all successfully committed transactions to client customer information system [TCF]
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void PostFlush(long customerSessionId, MGIContext mgiContext);

		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void RemoveCheckFromCart(long customerSessionId, long checkId, MGIContext mgiContext);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        List<ParkedTransaction> GetAllParkedShoppingCartTransactions();
    }
}
