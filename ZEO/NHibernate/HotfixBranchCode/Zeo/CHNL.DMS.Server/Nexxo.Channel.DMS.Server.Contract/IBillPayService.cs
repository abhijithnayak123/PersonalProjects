using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MGI.Common.Sys;
using MGI.Channel.Shared.Server.Data;
using MGI.Channel.DMS.Server.Data;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface IBillPayService
	{
		/// <summary>
		/// This methods is used to get the List of Billers Information 
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="channelPartnerID">This field is Channel Partner ID</param>
		/// <param name="searchTerm">This field is used to serarch the term of billers name</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>List of Billers Information</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<string> GetBillers(long customerSessionId, long channelPartnerID, string searchTerm, MGIContext mgiContext);


		/// <summary>
		/// This method is used to Get biller by ID
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="billerID">This is unique identifier for Biller ID is provided by PTNR database</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Biller Information</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Product GetBiller(long customerSessionId, long billerID, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the Get biller by name
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="channelPartnerID">This field is Channel Parnter Id</param>
		/// <param name="billerNameOrCode">This field is Billername or Code</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Biller Information</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Product GetBillerByName(long customerSessionId, long channelPartnerID, string billerNameOrCode, MGIContext mgiContext);

		/// <summary>
		/// This method is used to Get frequent billers for customer
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="alloyId">This is unique identifier for alloy id in used to Customer CustomerPreferedProducts in PTNR database</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>List of Frequently used Billers</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<Product> GetFrequentBillers(long customerSessionId, long alloyId, MGIContext mgiContext);

		/// <summary>
		/// This method is used to Get all billers
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="channelPartnerID">This field is Channel Parnter Id</param>
		/// <param name="locationRegionID">This is unique identifier for location region id </param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>List of all billers</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<Product> GetAllBillers(long customerSessionId, long channelPartnerID, Guid locationRegionID, MGIContext mgiContext);
        
		/// <summary>
		/// This method is used to Get Bill Pay locations
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="billerName">This field is Biller Name</param>
		/// <param name="accountNumber">This is unique identifier for account number</param>
		/// <param name="amount">Amount</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Bill Pay Locations</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		BillPayLocation GetLocations(long customerSessionId, string billerName, string accountNumber, decimal amount, MGIContext mgiContext);

		/// <summary>
		/// This methd is used to Get Bill Pay fee
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="billerNameOrCode">BillerName or Code</param>
		/// <param name="accountNumber">This is unique identifier for account number</param>
		/// <param name="amount">Bill Pay Amount</param>
		/// <param name="location">This field is locataion based on the fee</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Bill Pay Fee</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		BillFee GetFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, BillerLocation location, MGIContext mgiContext);

		/// <summary>
		/// This method is used to Get biller message
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="billerNameOrCode">Biller Name or Code</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Biller Info</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		BillerInfo GetBillerInfo(long customerSessionId, string billerNameOrCode, MGIContext mgiContext);

		/// <summary>
		/// This method is used to Get BP provider attributes
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="billerNameOrCode">Biller Name or Code</param>
		/// <param name="location">Location Name</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Bill Pay Provider Attributes</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		List<Field> GetProviderAttributes(long customerSessionId, string billerNameOrCode, string location, MGIContext mgiContext);

		/// <summary>
		/// This method is used to Get favorite biller
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="billerNameOrCode">Biller Name or Code</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Favorite Biller Details</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		FavoriteBiller GetFavoriteBiller(long customerSessionId, string billerNameOrCode, MGIContext mgiContext);

		/// <summary>
		/// This methd is used to Get Bill Pay fee
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="providerName">This is Provider Name</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Bill Pay Fee</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		decimal GetBillPayFee(long customerSessionId, string providerName, MGIContext mgiContext);

		/// <summary>
		/// This method is used Begin BP transaction
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="transactionID">This is unique identifier for transaction id</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void StageBillPayment(long customerSessionId, long transactionID, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get Validate BP request
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="billPayment">Bill Payment</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Bill Pay Transaction ID</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		long ValidateBillPayment(long customerSessionId, BillPayment billPayment, MGIContext mgiContext);

		/// <summary>
		/// This method is used to Get card info
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Card Info</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		MGI.Channel.Shared.Server.Data.CardInfo GetCardInfo(long customerSessionId, MGIContext mgiContext);

		/// <summary>
		/// This method is used to Get Agent Details
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Ageent Session ID</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Agent Session ID</returns>
		[OperationContract(Name = "GetAgent")]
		[FaultContract(typeof(NexxoSOAPFault))]
		CashierDetails GetAgent(long agentSessionId, MGIContext mgiContext);

		/// <summary>
		/// This method skeleton is added to add the "Past Billers into DB" and in the favorite billers in Bill Pay Screens for User Story # US1646.
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="cardNumber">Card Number</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void AddPastBillers(long customerSessionId, string cardNumber, MGIContext mgiContext);

		/// <summary>
		/// This method is used to Cancel the Bill Payment Transaction
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="transactionId">This is unique identifier for Transaction ID</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void CancelBillPayment(long customerSessionId, long transactionId, MGIContext mgiContext);

		/// <summary>
		/// This method is used to delete a specific biller from the Favorite Billers list
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for Customer session id from PTNR database</param>
		/// <param name="billerID">This is unique identifier for Bill ID</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		void DeleteFavoriteBiller(long customerSessionId, long billerID, MGIContext mgiContext);

	}
}
