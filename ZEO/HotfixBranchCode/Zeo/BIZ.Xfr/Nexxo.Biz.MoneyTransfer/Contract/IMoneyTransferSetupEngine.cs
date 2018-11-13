using MGI.Biz.MoneyTransfer.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;

namespace MGI.Biz.MoneyTransfer.Contract
{
	public interface IMoneyTransferSetupEngine
	{
		/// <summary>
		/// This method is used to get the list of WU Countries
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details"</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>List of WU Countries</returns>
		List<MasterData> GetCountries(long customerSessionId,MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the list of States
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details"</param>
		/// <param name="countryCode">Country Code</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>list of states</returns>
		List<MasterData> GetStates(long customerSessionId, string countryCode,MGIContext mgiContext);

		/// <summary>
		/// The method is used to get the list of Cities
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details"</param>
		/// <param name="stateCode">State Code</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>list of cities</returns>
		List<MasterData> GetCities(long customerSessionId, string stateCode,MGIContext mgiContext);

		/// <summary>
		/// This metod is used to get the currency code
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details"</param>
		/// <param name="countryCode">Currency Code</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Currency Code</returns>
		string GetCurrencyCode(long customerSessionId, string countryCode,MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the list the currency code
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details"</param>
		/// <param name="countryCode">Country Code</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>List of Currency Code </returns>
		List<MasterData> GetCurrencyCodeList(long customerSessionId, string countryCode,MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the agent banner messages
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>String of Banner Content list</returns>
		List<MasterData> GetBannerMsgs(long agentSessionId,MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the list of Refund reasons
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details"</param>
		/// <param name="request">Refund parameter based on list of reason</param>
		/// <param name="context">This is the common class parameter used to pass supplimental information</param>
		/// <returns>List of Refund Reasons</returns>
		List<Reason> GetRefundReasons(long customerSessionId, ReasonRequest request,MGIContext mgiContext);
	}
}
