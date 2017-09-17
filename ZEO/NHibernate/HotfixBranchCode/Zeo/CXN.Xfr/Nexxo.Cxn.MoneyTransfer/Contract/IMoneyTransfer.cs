using MGI.Common.Util;
using MGI.Cxn.Common.Processor.Contract;
using MGI.Cxn.MoneyTransfer.Data;
using System.Collections.Generic;

namespace MGI.Cxn.MoneyTransfer.Contract
{
	public interface IMoneyTransfer : IMoneyTransferSetup, IProcessor
	{
		/// <summary>
		/// This method is used for getting delivery Services
		/// </summary>
		/// <param name="request">delivery service request</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>delivery Services</returns>
		List<DeliveryService> GetDeliveryServices(DeliveryServiceRequest request, MGIContext mgiContext);
		
		/// <summary>
		/// This method is used for getting fee information
		/// </summary>
		/// <param name="feeRequest">fee request</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>fee details</returns>
		FeeResponse GetFee(FeeRequest feeRequest, MGIContext mgiContext);
		
		/// <summary>
		/// This method is used to validate send money transaction
		/// </summary>
		/// <param name="validateRequest">validate request information</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>validate details</returns>
		ValidateResponse Validate(ValidateRequest validateRequest, MGIContext mgiContext);
		
		/// <summary>
		/// This method is used to search the send money transaction for modify and refund
		/// </summary>
		/// <param name="request">sending search request </param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>search details</returns>
		SearchResponse Search(SearchRequest request, MGIContext mgiContext);

		/// <summary>
		/// This method is used for getting send money pay status like the refund is full or 
		/// only principle amount
		/// </summary>
		/// <param name="confirmationNumber">confirmation number i.e mtcn number</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>send money pay status description</returns>
		string GetStatus(string confirmationNumber, MGIContext mgiContext);


		/// <summary>
		/// This method is used for getting the list of refund reasons to be used during send money refund procedure
		/// </summary>
		/// <param name="request">reason request</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>list of refund reasons </returns>
		List<Reason> GetRefundReasons(ReasonRequest request, MGIContext mgiContext);

		/// <summary>
		/// This method is used for comiting send-money-modify transactions   
		/// </summary>
		/// <param name="transactionId">transaction unique identifier</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		void Modify(long transactionId, MGIContext mgiContext);

		/// <summary>
		/// This method is used to send-money-refund  transaction done by custommer
		/// </summary>
		/// <param name="refundRequest">refund request</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>refund mtcn</returns>
		string Refund(RefundRequest refundRequest, MGIContext mgiContext);

		/// <summary>
		/// This method is used to stage send-money-modify transaction
		/// </summary>
		/// <param name="modifySendMoney">modify send money request</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>transaction unique identifier for send money modify transaction</returns>
		ModifyResponse StageModify(ModifyRequest modifySendMoney, MGIContext mgiContext);

		/// <summary>
		/// This method is used for stage send-money-refund transaction
		/// </summary>
		/// <param name="refundSendMoney">refund send money request</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>transaction unique identifier for refund transaction</returns>
        long StageRefund(RefundRequest refundSendMoney, MGIContext mgiContext);

		/// <summary>
		/// This method is used for getting transaction details
		/// </summary>
		/// <param name="request">transaction request</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>transaction details</returns>
		Transaction GetTransaction(TransactionRequest request, MGIContext mgiContext);

		/// <summary>
		/// This method is used to adding customer account details 
		/// </summary>
		/// <param name="account">customer information to create CXN account for money transfer</param>
		/// <param name="cxnContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>account unique identifier</returns>
        long AddAccount(Account account, MGIContext mgiContext);

		/// <summary>
		/// This method is used for updating customer account details
		/// </summary>
		/// <param name="account">customer information to update CXN account for money transfer</param>
		/// <param name="cxnContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>account unique identifier</returns>
        long UpdateAccount(Account account, MGIContext mgiContext);

		/// <summary>
		/// This method is used for getting customer account details
		/// </summary>
		/// <param name="cxnAccountId">account unique identifier</param>
		/// <param name="cxnContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>account details</returns>
        Account GetAccount(long cxnAccountId, MGIContext mgiContext);

		/// <summary>
		/// This method is used for commiting transactions send money and Recive money
		/// </summary>
		/// <param name="transactionId">unique transaction identifier associated with each commit transaction</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>commit status</returns>
		bool Commit(long transactionId, MGIContext mgiContext); // using cxn-transactionId receiver & paymentdetail will be retrieved

		/// <summary>
		/// This method is used for saving receiver details 
		/// </summary>
		/// <param name="receiver">receiver information</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>receiver unique identifier</returns>
		long SaveReceiver(Receiver receiver, MGIContext mgiContext); // Will have CustomerPK(long) associated with Receiver & retrun CXNId

		/// <summary>
		/// This method is used for getting list of frequent send money receivers 
		/// </summary>
		/// <param name="CustomerId">customer unique identifer</param>
		/// <returns>list of frequent receivers</returns>
		List<Receiver> GetFrequentReceivers(long CustomerId); // CXEId -> Customer PK from CXE

		/// <summary>
		/// This method is used for getting list of receiver details
		/// </summary>
		/// <param name="CustomerId">customer unique identifer</param>
		/// <param name="lastName">last name of receiver</param>
		/// <returns>list of receiver details</returns>
		List<Receiver> GetReceivers(long CustomerId, string lastName);

		/// <summary>
		/// This method is used for getting receiver details
		/// </summary>
		/// <param name="Id">receiver unique identifer</param>
		/// <returns>receiver details</returns>
		Receiver GetReceiver(long Id);

		/// <summary>
		/// This method is used for getting receiver details
		/// </summary>
		/// <param name="customerId">customer unique identifer</param>
		/// <param name="fullName">receiver full name</param>
		/// <returns>receiver details</returns>
		Receiver GetReceiver(long customerId, string fullName);

		/// <summary>
		/// This method is used for getting the WU Banner message
		/// </summary>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>list of banner message</returns>
		List<MasterData> GetBannerMsgs(MGIContext mgiContext);

		/// <summary>
		/// This method is used to enroll WU card enrollment for the customer
		/// </summary>
		/// <param name="sender">sender account information</param>
		/// <param name="paymentDetails">sender payment details</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>WU card enrollment details</returns>
		MGI.Cxn.MoneyTransfer.Data.CardDetails WUCardEnrollment(Account sender, PaymentDetails paymentDetails, MGIContext mgiContext);

		/// <summary>
		/// This method is used for WU card lookup
		/// </summary>
		/// <param name="customerAccountId">customer account unique identifier</param>
		/// <param name="LookupDetails">lookup card information</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>card lookup details</returns>
		MGI.Cxn.MoneyTransfer.Data.CardLookupDetails WUCardLookup(long customerAccountId, MGI.Cxn.MoneyTransfer.Data.CardLookupDetails LookupDetails, MGIContext mgiContext);

		/// <summary>
		/// This method is used for getting WU card account information
		/// </summary>
		/// <param name="customerAccountId">customer account unique identifier</param>
		/// <returns>WU card account status</returns>
		bool GetWUCardAccount(long customerAccountId);

		/// <summary>
		/// This method is used for getting WU card account information
		/// </summary>
		/// <param name="cxnAccountId">cxn account unique identifier</param>
		/// <returns>WU card account details</returns>
		MGI.Cxn.MoneyTransfer.Data.Account DisplayWUCardAccountInfo(long cxnAccountId);

		/// <summary>
		/// This method imports recently used receivers from WU and persists in CXN tWUnion_Receiver table
		/// </summary>
		/// <param name="customerSessionId">customer session unique identifier</param>
		/// <param name="cardNumber">gold card number</param>
		/// <param name="cxnContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
        bool GetPastReceivers(long customerSessionId, string cardNumber, MGIContext mgiContext);

		/// <summary>
		/// This method is used for saving gold card information
		/// </summary>
		/// <param name="accountId">account unique identifier</param>
		/// <param name="WUGoldCardNumber">WU Gold card number</param>
		/// <param name="cxnContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>updated gold card details</returns>
        bool UseGoldcard(long accountId, string WUGoldCardNumber, MGIContext mgiContext);

		/// <summary>
		/// This method is used for getting card information like total card points
		/// </summary>
		/// <param name="cardNumber">Gold card number</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>card details</returns>
		CardInfo GetCardInfo(string cardNumber, MGIContext mgiContext);

		/// <summary>
		/// This method is used for getting reciever last transaction
		/// </summary>
		/// <param name="receiverId">receiver unique identifier</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>receiver transaction details</returns>
		Transaction GetReceiverLastTransaction(long receiverId, MGIContext mgiContext);


		/// <summary>
		/// This method is used to get the dynamic fields need to make a successful MGI send money transaction
		/// </summary>
		/// <param name="attributeRequest"></param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>list of provider fields</returns>
		List<Field> GetProviderAttributes(AttributeRequest attributeRequest, MGIContext mgiContext);

		/// <summary>
		/// This method is used to check the given state code is a SWB (South West Border) state
		/// </summary>
		/// <param name="stateCode">location state code</param>
		/// <returns>bool</returns>
		bool IsSWBState(string stateCode);

		/// <summary>
		/// This method is used for updating gold card points
		/// </summary>
		/// <param name="transactionId">transaction unique identifier</param>
		/// <param name="totalPointsEarned">cotains total points earned</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		void UpdateGoldCardPoints(long transactionId, string totalPointsEarned, MGIContext mgiContext);
        
        /// <summary>
        /// This method to get Delivery service in Spanish
        /// </summary>
        /// <param name="serviceName">This is the delivery service Name</param>
        /// <param name="language">the languge</param>
        /// <returns>Transalated Delivery Service Name</returns>
        string GetDeliveryServiceTransalation(string serviceName, string language);

        /// <summary>
        /// This method to get country name in Spanish
        /// </summary>
        /// <param name="countryCode">the country code</param>
        /// <param name="language">the languge</param>
        /// <returns>Transalated Country Name</returns>
        string GetCountryTransalation(string countryCode, string language);

		/// <summary>
		///
		/// </summary>
		/// <param name="receiver">receiver information<</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns></returns>
		bool DeleteFavoriteReceiver(Receiver receiver, MGIContext mgiContext);
	}
}
