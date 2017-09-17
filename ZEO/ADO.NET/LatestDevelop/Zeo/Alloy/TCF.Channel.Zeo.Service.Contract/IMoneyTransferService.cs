using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CommonData = TCF.Zeo.Common.Data;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface IMoneyTransferService
    {
        /// <summary>
        /// This method to add Western Union Gold card account.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response WUCardEnrollment(ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetDeliveryServices(DeliveryServiceRequest request, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response Validate(ValidateRequest validateRequest, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetFeeMoneyTransfer(FeeRequest feeRequest, ZeoContext context);

        /// <summary>
		/// This method is used to get the list of countries
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Collection of countries</returns>
		[OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetXfrCountries(ZeoContext context);

        /// <summary>
        /// This method is used to get the list of states
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
        /// <param name="countryCode">This field is used to the country code</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>collection of states</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetXfrStates(string countryCode, ZeoContext context);

        /// <summary>
        /// This method is used to get the list of cities
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
        /// <param name="stateCode">This field is used to state code</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Collection of cities</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetXfrCities(string stateCode, ZeoContext context);

        /// <summary>
        /// Method to  add receiver.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="receiver">A transient receiver instance</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>unique identifier for receiver</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response AddReceiver(Receiver receiver, ZeoContext context);

        /// <summary>
        /// Method to  Edit/Update receiver .
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="receiver">A transient receiver instance contains Updated State.</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>unique identifier for receiver</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response EditReceiver(Receiver receiver, ZeoContext context);
        /// <summary>
        /// Method to getFrequesnt receiver in send money
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetFrequentReceivers(ZeoContext context);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiverId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetReceiver(long receiverId, ZeoContext context);

        /// <summary>
		///  AL-3502
		/// </summary>
		/// <param name="customerSessionId">his is unique identifier for customerSession</param>
		/// <param name="receiverId">This is unique identifier for getting the receiver id</param>
		/// <param name="context">This is the common class parameter used to pass supplemental information</param>
		[OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response DeleteFavoriteReceiver(long receiverId, ZeoContext context);

        /// <summary>
        /// For Import Functionality.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response AddPastReceivers(string cardNumber, ZeoContext context);

        /// <summary>
        /// For WU Card Lookup functionality.
        /// </summary>
        /// <param name="lookupDetails"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response WUCardLookup(CardLookupDetails lookupDetails, ZeoContext context);

        /// <summary>
        /// Update a WU account when Use My WU is clicked from the page.
        /// </summary>
        /// <param name="WUGoldCardNumber"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response UpdateWUAccount(string WUGoldCardNumber, ZeoContext context);
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetCurrencyCodeList(string countryCode, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response CancelXfer(long transactionId, ZeoContext context);


        /// <summary>
        /// This method is used to get the display of Western Union Banner message
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List of messages in WU</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response WUGetAgentBannerMessage(long agentSessionId, ZeoContext context);

        /// <summary>
		/// This method is used to get the currency code based on country list
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="countryCode">This field is used to get the country code</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>currency code</returns>
		[OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetCurrencyCode(string countryCode, ZeoContext context);
    
	    /// <summary>
        /// When clicked on Sbmit button while doing SM modify this method is called to add multiple trx in
        /// database with Cancelled and Modified Statuses.
        /// </summary>
        /// <param name="modifySendMoney"></param>
        /// <param name="context"></param>
        /// <returns></returns>
		[OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response StageModify(ModifyRequest modifySendMoney, ZeoContext context);

       	/// <summary>
	    /// Search the Modify/refund transaction. 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response Search(SearchRequest request, ZeoContext context);

        /// <summary>
		/// This method is used to get the list of refund reasons 
		/// </summary>
		/// <param name="request">This field is request of transaction type</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>List of refund reasons</returns>
		[OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetRefundReasons(ReasonRequest request, ZeoContext context);

        [OperationContract(Name = "GetMoneyTransferTransaction")]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetTransaction(long transactionId, ZeoContext context);
		
		/// <summary>
        /// 
        /// </summary>
        /// <param name="mtcn"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetStatus(string mtcn, ZeoContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response IsSWBStateXfer(ZeoContext context);

        /// <summary>
        /// This method to update Transaction Status to Authorized in [CXE and Partner Database.]
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="modifySendMoney">A transient instance of a persistent ModifySendMoneyRequest[Class]</param>
        /// <param name="context">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Modify Send Money Details</returns>
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response AuthorizeModifySendMoney(ModifyRequest modifySendMoney, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response ReceiveMoneySearch(ReceiveMoneyRequest receiveMoneyRequest, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response SendMoneyRefund(SendMoneyRefundRequest moneyTransferRefund, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response CancelSendMoneyModify(long modifyTransactionId, long cancelTransactionId, ZeoContext context);

    }
}
