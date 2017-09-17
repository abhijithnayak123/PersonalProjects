using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;
using System.Collections.Generic;
using System.ServiceModel;
using MGI.Channel.DMS.Server.Data;
using System;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface IMoneyTransferService
	{
        /// <summary>
        /// Method to get all receiver based on last name.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="lastName">Receiver last name [CXN-Database, tMGram_Receiver]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Receiver</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        IList<Receiver> GetReceivers(long customerSessionId, string lastName, MGIContext mgiContext);
        
        /// <summary>
        /// This method to get receiver based on receiver id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="Id">This is unique identifier for receiver. [MGReciverID, CXN-Database, tMGram_Receiver]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Receiver Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        Receiver GetReceiver(long customerSessionId, long Id, MGIContext mgiContext);

        /// <summary>
        /// Method to get receiver based on receiver full name [First Name, Last Name, CXN-Database, tMGram_Receiver].
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="fullName">The receiver full name. [FirstName+LastName, CXN-Database, tMGram_Receiver]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Receiver Details</returns>
		[OperationContract(Name = "GetReceiverByFullName")]
		[FaultContract(typeof(NexxoSOAPFault))]
        Receiver GetReceiver(long customerSessionId, string fullName, MGIContext mgiContext);

        /// <summary>
        /// Method to  add receiver [CXN-Database, tMGram_Receiver].
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="receiver">A transient receiver instance</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>unique identifier for receiver</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		long AddReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext);

        /// <summary>
        /// Method to  Edit/Update receiver [CXN-Database, tMGram_Receiver].
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="receiver">A transient receiver instance contains Updated State.</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>unique identifier for receiver</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		long EditReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext);

        /// <summary>
        /// This method to get frequently used receiver based on customer unique identifier.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Receiver</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        IList<Receiver> GetFrequentReceivers(long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// Method to get money transfer transaction fee.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="feeRequest">A transient instance of a persistent feeRequest</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>FeeResponse Details</returns>
		[OperationContract(Name = "GetMoneyTransferFee")]
		[FaultContract(typeof(NexxoSOAPFault))]
        FeeResponse GetFee(long customerSessionId, FeeRequest feeRequest, MGIContext mgiContext);

        /// <summary>
        /// This method to validate Money transfer.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="validateRequest">A transient instance of a persistent validateRequest</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Validate Response Details</returns>
		[OperationContract()]
		[FaultContract(typeof(NexxoSOAPFault))]
        ValidateResponse ValidateTransfer(long customerSessionId, ValidateRequest validateRequest, MGIContext mgiContext);

        /// <summary>
        /// This method to get receive transaction by ReceiveMoneyRequest details.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="receiveMoneyRequest">A transient instance of a persistent receiveMoneyRequest</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Money Transfer Transaction Details</returns>
		[OperationContract(Name = "ReceiveMoneySearch")]
		[FaultContract(typeof(NexxoSOAPFault))]
		MoneyTransferTransaction Get(long customerSessionId, ReceiveMoneyRequest receiveMoneyRequest, MGIContext mgiContext);

        /// <summary>
        /// This method to update Western Union Gold card Details.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="WUGoldCardNumber">The Western Union Gold Card Number.</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Western Union Gold Card Account Updated status</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        bool UpdateWUAccount(long customerSessionId, string WUGoldCardNumber, MGIContext mgiContext);

        /// <summary>
        /// This method to add Western Union Gold card account.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="paymentDetails">A transient instance of a persistent XferPaymentDetails[Contains all money transfer details]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Western Union Gold Card Details.</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		CardDetails WUCardEnrollment(long customerSessionId, XferPaymentDetails paymentDetails, MGIContext mgiContext);

        /// <summary>
        /// This method to search WU Customer Gold Card based on Card Lookup details.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="wucardlookupreq">A transient instance of a persistent card lookup details [CardLookupDetails]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of WU Customer Gold Card </returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<WUCustomerGoldCardResult> WUCardLookup(long customerSessionId, CardLookupDetails wucardlookupreq, MGIContext mgiContext);

        /// <summary>
        /// This method to get Western Union Gold card account details.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Western Union Gold Card Account Search status</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        bool GetWUCardAccount(long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get Western Union Gold card Account.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Western Union Gold card Account Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        Account DisplayWUCardAccountInfo(long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to update transaction Status in [CXE-Database, -tTxn_MoneyTransfer_Stage-table, tTxn_MoneyTransfer_Commit-table]
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Money Transfer Transaction.[CXE-DataBase, MoneyTransferID]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[OperationContract(Name = "CancelXfer")]
		[FaultContract(typeof(NexxoSOAPFault))]
		void Cancel(long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get total WU point earn by customer.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Returns the Card Info Details.</returns>
		[OperationContract(Name = "GetCardInfoXfer")]
		[FaultContract(typeof(NexxoSOAPFault))]
		MGI.Channel.Shared.Server.Data.CardInfo GetCardInfoXfer(long customerSessionId, MGIContext mgiContext);

		/// <summary>
		/// This method skeleton is added to add the "Past Receivers into DB" and in the favorite receivers in Send Money Screens for User Story # US1645.
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="cardNumber">This is the Western Union Gold card Number </param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void AddPastReceivers(long customerSessionId, string cardNumber, MGIContext mgiContext);

		/// <summary>
		///  AL-3502
		/// </summary>
		/// <param name="customerSessionId">his is unique identifier for customerSession</param>
		/// <param name="receiverId">This is unique identifier for getting the receiver id</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void DeleteFavoriteReceiver(long customerSessionId, long receiverId, MGIContext mgiContext);

		#region MoneyTransfer Refactoring
        /// <summary>
        /// This method to get Delivery Services based on Delivery Service Type [Method, Option]
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="request">A transient instance of a persistent DeliveryServiceRequest</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Delivery Services.</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<DeliveryService> GetDeliveryServices(long customerSessionId, DeliveryServiceRequest request, MGIContext mgiContext);

        /// <summary>
        /// It retrieves the dynamic fields required to make the bill pay or send money transaction based on parameters 
        /// like amount, country and delivery service
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="attributeRequest">A transient instance of a persistent attributeRequest[Class].</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns></returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<Field> GetXfrProviderAttributes(long customerSessionId, AttributeRequest attributeRequest, MGIContext mgiContext);

        /// <summary>
        /// This method to check WU South West Border Location State.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="locationState">This is the location state.</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Search status</returns>
		[OperationContract(Name = "IsSWBStateXfer")]
		[FaultContract(typeof(NexxoSOAPFault))]
        bool IsSWBStateXfer(long customerSessionId, string locationState, MGIContext mgiContext);

        /// <summary>
        /// This method to get Cashier Details.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Cashier Details</returns>
		[OperationContract(Name = "GetAgentXfer")]
		[FaultContract(typeof(NexxoSOAPFault))]
        CashierDetails GetAgentXfer(long agentSessionId, MGIContext mgiContext);

		#endregion

		#region  Send Money Refund
        /// <summary>
        /// This method to add Money transafer transaction in Partner database with transaction status Refound.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="moneyTransferRefund">A transient instance of a persistent RefundSendMoneyRequest[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Unique identifier[Primary Key]</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        long StageRefundSendMoney(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext);

        /// <summary>
        /// This method to commit refund Send money transaction.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="moneyTransferRefund">A transient instance of a persistent RefundSendMoneyRequest[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>The conformation Number[mtcn]</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        string SendMoneyRefund(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext);

		#endregion

		#region  Send Money Modify
        /// <summary>
        /// This method to get Money Transfer Transaction Status based on conformationNumber[mtcn].
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="confirmationNumber">This is unique number for money transfer transaction.[mtcn]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Money Transfer Transaction Status.</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        string GetStatus(long customerSessionId, string confirmationNumber, MGIContext mgiContext);

        /// <summary>
        /// Method to search send money transaction based on send money search request [MTCN, MoneyTransferTransactionStatus...]
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="request">A transient instance of a persistent SendMoneySearchRequest[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Send Money Search Response details.</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        SendMoneySearchResponse SendMoneySearch(long customerSessionId, SendMoneySearchRequest request, MGIContext mgiContext);
        
        /// <summary>
        /// Method to modify the money transfer transaction. 
        /// To update the transactionStatus to Modify.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="modifySendMoney">A transient instance of a persistent ModifySendMoneyRequest[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Modify Send Money Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        ModifySendMoneyResponse StageModifySendMoney(long customerSessionId, ModifySendMoneyRequest modifySendMoney, MGIContext mgiContext);

        /// <summary>
        /// This method to get Money Transfer Transaction Details Based on Transaction Id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is the unique identifier for Money Transfer transaction.</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Money Transfer Transaction Details.</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        MoneyTransferTransaction GetMoneyTransferDetailsTransaction(long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// This method to update Transaction Status to Authorized in [CXE and Partner Database.]
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="modifySendMoney">A transient instance of a persistent ModifySendMoneyRequest[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Modify Send Money Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
        ModifySendMoneyResponse AuthorizeModifySendMoney(long customerSessionId, ModifySendMoneyRequest modifySendMoney, MGIContext mgiContext);

		#endregion

	}
}
