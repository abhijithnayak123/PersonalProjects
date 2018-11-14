using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;
using MGI.Common.Util;
using System.Collections.Generic;
using System.ServiceModel;

namespace MGI.Channel.Consumer.Server.Contract
{
	public interface IMoneyTransferService
    {

        #region Xfr Setup Service
        /// <summary>
        /// This method to get List of countries from master table from PTNR database.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Countries</returns>
        List<XferMasterData> GetXfrCountries(long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get List of states based on Country code.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="countryCode">This is the unique identifier for country.</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of States</returns>
        List<XferMasterData> GetXfrStates(long customerSessionId, string countryCode, MGIContext mgiContext);

        /// <summary>
        /// To get list of cities based on state.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="stateCode">This is the unique identifier for state</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Cities</returns>
        List<XferMasterData> GetXfrCities(long customerSessionId, string stateCode, MGIContext mgiContext);

        /// <summary>
        /// To get currency code based country code.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="countryCode">This is the unique identifier for country</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Currnecy Code</returns>
        List<XferMasterData> GetCurrencyCodeList(long customerSessionId, string countryCode, MGIContext mgiContext);

        #endregion

        #region Xfr Receiver Methods
        /// <summary>
        /// Method to  add receiver.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="receiver">A transient receiver instance</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>unique identifier for receiver</returns>
        long AddReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext);

        /// <summary>
        /// Method to Edit/Update receiver.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="receiver">A transient receiver instance containing Updated State.</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>unique identifier for receiver</returns>
        long EditReceiver(long customerSessionId, Receiver receiver, MGIContext mgiContext);

        /// <summary>
        /// This method to get frequently used receiver based on customer unique identifier.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Receiver</returns>
        IList<Receiver> GetFrequentReceivers(long customerSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get receiver based on receiver id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="Id">This is unique identifier for receiver. [MGReciverID, CXN-Database, tMGram_Receiver]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Receiver Details</returns>
        Receiver GetReceiver(long customerSessionId, long Id, MGIContext mgiContext);

        /// <summary>
        /// This method to get last transaction done by receiver based on receiver id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="receiverId">This is unique identifier for receiver. [MGReciverID, CXN-Database, tMGram_Receiver]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Money Transfer Transaction Details.</returns>
        MoneyTransferTransaction GetReceiverLastTransaction(long customerSessionId, long receiverId, MGIContext mgiContext);

		/// <summary>
		/// This method to delete favorite receiver
		/// </summary>
		/// <param name="customerSessionId">>This is unique identifier for customerSession</param>
		/// <param name="receiverId">This is unique identifier for receiver.</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		void DeleteFavoriteReceiver(long customerSessionId, long receiverId, MGIContext mgiContext);

        #endregion

        #region Xfr trx Methods
        /// <summary>
        /// Method to get money transfer transaction fee.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="feeRequest">A transient instance of a persistent FeeRequest[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>FeeResponse Details</returns>
        FeeResponse GetXfrFee(long customerSessionId, FeeRequest feeRequest, MGIContext mgiContext);
        
        /// <summary>
        /// This method to validate Money transfer.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="validateRequest">>A transient instance of a persistent ValidateRequest[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Validate Response Details</returns>
        ValidateResponse ValidateXfr(long customerSessionId, ValidateRequest validateRequest, MGIContext mgiContext);

        /// <summary>
        /// It retrieves the dynamic fields required to make the bill pay or send money transaction based on parameters 
        /// like amount, country and delivery service
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="attributeRequest">A transient instance of a persistent AttributeRequest[Class].</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns></returns>
        List<Field> GetXfrProviderAttributes(long customerSessionId, AttributeRequest attributeRequest, MGIContext mgiContext);

        /// <summary>
        /// To get money transfer transaction details based on transaction id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="transactionId">This is unique identifier for Money transfer transaction</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Money Transfer Transaction Details</returns>
        MoneyTransferTransaction GetXfrTransaction(long customerSessionId, long transactionId, MGIContext mgiContext);

        /// <summary>
        /// Method to search send money transaction based on send money search request [MTCN, MoneyTransferTransactionStatus...]
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="request">A transient instance of a persistent SendMoneySearchRequest[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Send Money Search Response details.</returns>
        SendMoneySearchResponse SendMoneySearch(long customerSessionId, SendMoneySearchRequest request, MGIContext mgiContext);

        /// <summary>
        /// Method to modify the money transfer transaction. 
        /// To update the transactionStatus to Modify.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="moneyTransferModify">A transient instance of a persistent ModifySendMoneyRequest[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Modify Send Money Details</returns>
        ModifySendMoneyResponse StageModifySendMoney(long customerSessionId, ModifySendMoneyRequest moneyTransferModify, MGIContext mgiContext);

        /// <summary>
        /// This method to update Transaction Status to Authorized in [CXE and Partner Database.]
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="modifySendMoney">A transient instance of a persistent ModifySendMoneyRequest[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Modify Send Money Details</returns>
        ModifySendMoneyResponse AuthorizeModifySendMoney(long customerSessionId, ModifySendMoneyRequest modifySendMoney, MGIContext mgiContext);

        /// <summary>
        /// This method to add Money transafer transaction in Partner database with transaction status Refound.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="moneyTransferRefund">A transient instance of a persistent ModifySendMoneyRequest[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Unique identifier[Primary Key]</returns>
        long StageRefundSendMoney(long customerSessionId, RefundSendMoneyRequest moneyTransferRefund, MGIContext mgiContext);

        /// <summary>
        /// This method is used to get the list of refund reasons 
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customerSession</param>
        /// <param name="request">A transient instance of a persistent ReasonRequest[Class]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List of refund reasons</returns>
	    List<MoneyTransferReason> GetRefundReasons(long customerSessionId, ReasonRequest request,MGIContext mgiContext);

	    #endregion

    }
}
