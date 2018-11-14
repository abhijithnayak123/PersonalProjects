using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.MoneyTransfer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.Contract
{
    public interface IMoneyTransferService : IReceiver
    {
        /// <summary>
		/// This method is used for getting delivery Services
		/// </summary>
		/// <param name="request">delivery service request</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>delivery Services</returns>
		List<DeliveryService> GetDeliveryServices(DeliveryServiceRequest request, ZeoContext context);
		
		/// <summary>
		/// This method is used for getting fee information
		/// </summary>
		/// <param name="feeRequest">fee request</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>fee details</returns>
		FeeResponse GetFee(FeeRequest feeRequest, ZeoContext context);
		
		/// <summary>
		/// This method is used to validate send money transaction
		/// </summary>
		/// <param name="validateRequest">validate request information</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>validate details</returns>
		ValidateResponse Validate(ValidateRequest validateRequest, ZeoContext context);
		
		/// <summary>
		/// This method is used to search the send money transaction for modify and refund
		/// </summary>
		/// <param name="request">sending search request </param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>search details</returns>
		SearchResponse Search(SearchRequest request, ZeoContext context);

		/// <summary>
		/// This method is used for getting send money pay status like the refund is full or 
		/// only principle amount
		/// </summary>
		/// <param name="confirmationNumber">confirmation number i.e mtcn number</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>send money pay status description</returns>
		string GetStatus(string confirmationNumber, ZeoContext context);


		/// <summary>
		/// This method is used for getting the list of refund reasons to be used during send money refund procedure
		/// </summary>
		/// <param name="request">reason request</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>list of refund reasons </returns>
		List<Reason> GetRefundReasons(ReasonRequest request, ZeoContext context);

		/// <summary>
		/// This method is used for comiting send-money-modify transactions   
		/// </summary>
		/// <param name="transactionId">transaction unique identifier</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		void Modify(long transactionId, ZeoContext AlloyContext);

		/// <summary>
		/// This method is used to send-money-refund  transaction done by custommer
		/// </summary>
		/// <param name="refundRequest">refund request</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>refund mtcn</returns>
		string Refund(RefundRequest refundRequest, ZeoContext context);

        /// <summary>
        /// This method is used to stage send-money-modify transaction.
        /// </summary>
        /// <param name="modifySendMoney"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        ModifyResponse StageModify(ModifyRequest modifySendMoney, ZeoContext context);

		/// <summary>
		/// This method is used to adding customer account details 
		/// </summary>
		/// <param name="account">customer information to create CXN account for money transfer</param>
		/// <param name="cxnContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>account unique identifier</returns>
        long AddAccount(Account account, ZeoContext context);

        /// <summary>
        /// This method is used for updating customer account details.
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="account"></param>
        void UpdateAccountWithCardNumber(long customerSessionId, Account account);		

		/// <summary>
		/// This method is used for commiting transactions send money and Recive money
		/// </summary>
		/// <param name="transactionId">unique transaction identifier associated with each commit transaction</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>commit status</returns>
		bool Commit(long transactionId, ZeoContext context); // using cxn-transactionId receiver & paymentdetail will be retrieved

		
		/// <summary>
		/// This method is used for getting the WU Banner message
		/// </summary>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>list of banner message</returns>
		List<MasterData> GetBannerMsgs(ZeoContext context);

        /// <summary>
        /// This method is used to enroll WU card enrollment for the customer.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        CardDetails WUCardEnrollment(ZeoContext context);

        /// <summary>
        /// This method is used for WU card lookup.
        /// </summary>
        /// <param name="LookupDetails"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        CardLookupDetails WUCardLookup(CardLookupDetails LookupDetails, ZeoContext context);

		/// <summary>
		/// This method imports recently used receivers from WU and persists in CXN tWUnion_Receiver table
		/// </summary>
		/// <param name="customerSessionId">customer session unique identifier</param>
		/// <param name="cardNumber">gold card number</param>
		/// <param name="cxnContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns></returns>
        bool GetPastReceivers(long customerSessionId, string cardNumber, ZeoContext context);

        /// <summary>
        /// This method is used for saving gold card information.
        /// </summary>
        /// <param name="WUGoldCardNumber"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool UseGoldcard(string WUGoldCardNumber, ZeoContext context);
        
		/// <summary>
		/// This method is used for updating gold card points
		/// </summary>
		/// <param name="transactionId">transaction unique identifier</param>
		/// <param name="totalPointsEarned">cotains total points earned</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		//void UpdateGoldCardPoints(long transactionId, string totalPointsEarned, AlloyContext context);
        
        /// <summary>
        /// This method is used to get country currency code
        /// </summary>
        /// <param name="countryCode">country code</param>
        /// <returns>country currency code</returns>
        string GetCurrencyCode(string countryCode);

        /// <summary>
        /// This method is used to get list of currency code
        /// </summary>
        /// <param name="countryCode">country code</param>
        /// <returns>list of country currency code</returns>
        List<MasterData> GetCurrencyCodeList(string countryCode);

        /// <summary>
        /// Get the Wu countries.
        /// </summary>
        /// <returns></returns>
        List<MasterData> GetXfrCountries();

        /// <summary>
        /// Get the Wu states.
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        List<MasterData> GetXfrStates(string countryCode);

        /// <summary>
        /// Get the Wu cities.
        /// </summary>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        List<MasterData> GetXfrCities(string stateCode);

        /// <summary>
        /// Update WU Account.
        /// </summary>
        /// <param name="wuAccount"></param>
        /// <returns></returns>
        //long UpdateWUAccount(WUAccount wuAccount);

        /// <summary>
        /// Get the Wu Account.
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        //WUAccount GetWUAccount(long customerSessionId);

        /// <summary>
        /// Get the WU Transaction.
        /// </summary>
        /// <param name="wuTransactionId"></param>
        /// <returns></returns>
        WUTransaction GetWUTransaction(long wuTransactionId);

        void UpdateReceiverDetails(long transactionId, long customerSessionId, DateTime dtTerminalDate, DateTime dtServerDate);

        /// <summary>
        /// To check wheather modify or refund available
        /// </summary>
        /// <param name="transactionId">Unique transaction Id</param>
        /// <param name="customerId">customer unique Id</param>
        /// <returns></returns>
        bool IsSendMoneyModifyRefundAvailable(long transactionId, long customerId);

        WUTransaction GetReceiveTransaction(string confirmationNumber, long wuTrxId, ZeoContext context);

        /// <summary>
        /// To update the gold card points for Money Transfer.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="wUCardNumber"></param>
        /// <param name="context"></param>
        void UpdateMoneyTransferGoldCardPoints(long transactionId, string wUCardNumber, ZeoContext context);

        /// <summary>
		/// This method is used to get consumer fraud limit
		/// </summary>
		/// <param name="countryCode">country code</param>
		/// <returns>consumer fraud limit</returns>
		decimal GetFraudLimit(string countryCode, ZeoContext context);

        /// <summary>
        /// This method is used to get the destination amount
        /// </summary>
        /// <param name="feeRequest"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        decimal GetDestinationAmount(FeeRequest feeRequest, ZeoContext context);

        /// <summary>
        /// get the unblocked countries
        /// </summary>
        /// <returns></returns>
        List<MasterData> GetUnblockedCountries(ZeoContext context);
    }
}
