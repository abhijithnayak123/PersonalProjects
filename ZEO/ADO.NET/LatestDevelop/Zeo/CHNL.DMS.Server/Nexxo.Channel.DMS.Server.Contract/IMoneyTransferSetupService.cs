using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace MGI.Channel.DMS.Server.Contract
{
	[ServiceContract]
	public interface IMoneyTransferSetupService
	{
		/// <summary>
		/// This method is used to get the list of countries
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Collection of countries</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetXfrCountries(long customerSessionId, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the list of states
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="countryCode">This field is used to the country code</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>collection of states</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetXfrStates(long customerSessionId, string countryCode, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the list of cities
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="stateCode">This field is used to state code</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Collection of cities</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetXfrCities(long customerSessionId, string stateCode, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the currency code based on country list
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="countryCode">This field is used to get the country code</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>currency code</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetCurrencyCode(long customerSessionId, string countryCode, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the currency code based on country list
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="countryCode">This field is used to get the country code</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>List of currency code</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetCurrencyCodeList(long customerSessionId, string countryCode, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the display of Western Union Banner message
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>List of messages in WU</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response WUGetAgentBannerMessage(long agentSessionId, MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the list of refund reasons 
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="request">This field is request of transaction type</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>List of refund reasons</returns>
		[OperationContract]
		[FaultContract(typeof(NexxoSOAPFault))]
		Response GetRefundReasons(long customerSessionId, ReasonRequest request, MGIContext mgiContext);
	}
}
