using MGI.Cxn.MoneyTransfer.Data;
using System;
using System.Collections.Generic;

namespace MGI.Cxn.MoneyTransfer.Contract
{
	public interface IMoneyTransferSetup
	{
		/// <summary>
		/// This method is used to get list of all countries
		/// </summary>
		/// <returns>list of countries</returns>
		List<MasterData> GetCountries();

		/// <summary>
		/// This method is used to get list of all States
		/// </summary>
		/// <param name="countryCode">country code</param>
		/// <returns>list of states</returns>
		List<MasterData> GetStates(string countryCode);

		/// <summary>
		/// This method is used to get list of all cities
		/// </summary>
		/// <param name="stateCode">state code</param>
		/// <returns>list of cities</returns>
		List<MasterData> GetCities(string stateCode);
        		
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
	}
}
