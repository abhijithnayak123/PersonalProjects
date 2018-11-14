using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.MoneyTransfer.Data;
using MGI.Common.Util;

namespace MGI.Biz.MoneyTransfer.Contract
{
	public interface IMoneyTransferEngine : IMoneyTransferSetupEngine
	{
		/// <summary>
		/// Return provider/processor fee for send money transaction based on amount, country, delivery service
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="feeRequest">This field contains the details to get the fee from provider/processor</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Transaction fee</returns>
		FeeResponse GetFee(long customerSessionId, FeeRequest feeRequest, MGIContext mgiContext);

		/// <summary>
		/// Sends money transfer to provider/processor for validatrion
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="validateRequest">Validate Request of send money transaction parameters</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Transaction ID and HasLPMTError</returns>
		ValidateResponse Validate(long customerSessionId, ValidateRequest validateRequest, MGIContext mgiContext);

		/// <summary>
		/// Commits money transfer transaction to provider/processor
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="ptnrTransactionId">This is unique identifier for getting the Partner Transaction ID from the CXN database</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Transaction Status</returns>
		int Commit(long customerSessionId, long ptnrTransactionId, MGIContext mgiContext);

		/// <summary>
		/// Add receiver details for Send money transaction 
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="receiver">This field is used to the get the receiver details</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Receiver details</returns>
		long AddReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext); // Will return cxn-account ID for receiver

		/// <summary>
		/// This method is used to edit the receiver detials
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="receiver">This field is used to the get the receiver details</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Receiver details</returns>
		long EditReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the receiver information by id
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="Id">This is unique identifier for getting the receiver id from the tMGram_Receiver table in CXN database</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Receiver information</returns>
		Receiver GetReceiver(long customerSessionId, long Id, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the receiver informations by full name
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="fullName">This field is used to getting the first and last name</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Receiver information</returns>
		Receiver GetReceiver(long customerSessionId, string fullName, MGIContext mgiContext);

		/// <summary>
		/// This method is used to list of receiver details based on customer session id and receiver last name
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="lastName">Last Name</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Receiver details</returns>
		List<Receiver> GetReceivers(long customerSessionId, string lastName, MGIContext mgiContext);

		/// <summary>
		/// This method is used to which is applicable "active" status of receiver details
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="lastName">Last Name</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>list of receiver details</returns>
		List<Receiver> GetActiveReceivers(long customerSessionId, string lastName, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the Get Frequent Receivers
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>List of frequent receiver information</returns>
		List<Receiver> GetFrequentReceivers(long customerSessionId, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the send money transaction details 
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="transactionRequest">Transaction request from PTNR transaction ID</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>MoneyTransfer Transaction</returns>
		MoneyTransferTransaction Get(long customerSessionId, TransactionRequest transactionRequest, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the receive money transaction details 
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="request">This field is used to get the request confirmation number in receive money</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>MoneyTransfer Transaction detials</returns>
		MoneyTransferTransaction Get(long customerSessionId, ReceiveMoneyRequest request, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the receiver last transactions
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="receiverId">This is unique identifier for getting the receiver id</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		MoneyTransferTransaction GetReceiverLastTransaction(long customerSessionId, long receiverId, MGIContext mgiContext);

		/// <summary>
		/// This method is used to update the My WU account details
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="WUGoldCardNumber">My WU Number</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Card status</returns>
		bool UpdateAccount(long customerSessionId, string WUGoldCardNumber, MGIContext mgiContext);

		/// <summary>
		///  AL-3502
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="receiverId">This is unique identifier for getting the receiver id</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		void DeleteFavoriteReceiver(long customerSessionId, long receiverId, MGIContext mgiContext);

		/// <summary>
		/// Enrolls WU card to customer
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="paymentDetails">This field is used to get the payment details</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Card Details</returns>
		CardDetails WUCardEnrollment(long customerSessionId, PaymentDetails paymentDetails, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the WU card Lookup details
		/// </summary>
		/// <param name="customerAccountId">This is unique identifier for getting tWUnion_Account from CXN database</param>
		/// <param name="LookupDetails">Card customer lookup details</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>WU Card Details</returns>
		CardLookupDetails WUCardLookup(long customerAccountId, CardLookupDetails LookupDetails, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the WU Card Account Details
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>WU Card Account</returns>
		bool GetAccount(long customerSessionId, MGI.Common.Util.MGIContext mgiContext);

		/// <summary>
		/// This method is used to get WU card sender Information details 
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>WU Card Account Number</returns>
		Account DisplayWUCardAccountInfo(long customerSessionId, MGIContext mgiContext);

		/// <summary>
		/// This method is used to getting status Canceled is updating the status on CXE and PTNR
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="transactionId">This is unique identifier for getting the transaction id from the PTNR database</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		void Cancel(long customerSessionId, long transactionId, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the Enrolled WU Card information
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Card Info</returns>
		CardInfo GetCardInfo(long customerSessionId, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the send money Pay status enquiry response 
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="confirmationNumber">This field is used to get the Confirmation Number </param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>SendMoney Pay Status Resonse</returns>
		string GetStatus(long customerSessionId, string confirmationNumber, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the Send money search response
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="request">Request parameter</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Search Response</returns>
		SearchResponse Search(long customerSessionId, SearchRequest request, MGIContext mgiContext);

		/// <summary>
		/// This mthod is used to get the stage of Send money modify process
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="modifySendMoney">Modify Request Parameter</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Modify send money response</returns>
		ModifyResponse StageModify(long customerSessionId, ModifyRequest modifySendMoney, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the Authorize send money to updates the transaction in CXE and PTNR database
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="modifySendMoney">Modify Request Parameter</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		void AuthorizeModify(long customerSessionId, ModifyRequest modifySendMoney, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the Send money modify transaction
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="modifySendMoney">Modify Request Parameter</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Transaction states as committed</returns>
		int Modify(long customerSessionId, ModifyRequest modifySendMoney, MGIContext mgiContext);
		/// <summary>
		/// The method is used to get the based on stage refund for Send money transaction 
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="refundSendMoney">Refund request parameter</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Transaction ID</returns>
		long StageRefund(long customerSessionId, RefundRequest refundSendMoney, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the refund of send money transaction 
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="refundSendMoney">This field is Refund request of send money parameter</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Confirmation Number</returns>
		string Refund(long customerSessionId, RefundRequest refundSendMoney, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the Past receivers information
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="cardNumber">WU Card Number</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		void AddPastReceivers(long customerSessionId, string cardNumber, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the Delivery Services
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="request">Request Parameter</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Delivery Service Options</returns>
		List<DeliveryService> GetDeliveryServices(long customerSessionId, DeliveryServiceRequest request, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the fields of Provider attributes
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="attributeRequest">Attribute request</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
		List<Field> GetProviderAttributes(long customerSessionId, AttributeRequest attributeRequest, MGIContext mgiContext);

		/// <summary>
		/// This metod is used to get the South West Border to display the Agree and Disagree popup
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="stateCode">State Code</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>SWB State</returns>
		bool IsSWBState(long customerSessionId, string stateCode, MGIContext mgiContext);

		/// <summary>
		/// This method used for updating transaction status to faild when the faild responce come from processor.
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="transactionId">This is the unique identifier for biller</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		void UpdateTransactionStatus(long customerSessionId, long transactionId, MGIContext mgiContext);
	}
}
