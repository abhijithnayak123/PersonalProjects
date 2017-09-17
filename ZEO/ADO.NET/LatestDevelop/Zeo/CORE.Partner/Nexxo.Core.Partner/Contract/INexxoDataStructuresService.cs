using System;

using MGI.Core.Partner.Data;
using System.Collections.Generic;

namespace MGI.Core.Partner.Contract
{
	public interface INexxoDataStructuresService
	{
		/// <summary>
		/// This method is to get the Alloy id type by channel partner id, name, country and state
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <param name="name">This is the name of  Alloy id type</param>
		/// <param name="country">This is country name</param>
		/// <param name="state">This is state name</param>
		/// <returns>Alloy Id type details</returns>
		NexxoIdType Find(long channelPartnerId, string name, string country, string state);

		/// <summary>
		/// This method is to get the Alloy id type by channel partner id,
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <param name="idTypeId">This is Alloy type id</param>
		/// <returns>Alloy Id type details</returns>
		NexxoIdType Find(long channelPartnerId, long idTypeId);

		/// <summary>
		/// This method is to get the collection of all countries
		/// </summary>
		/// <returns>Collection of contries</returns>
		List<string> Countries();

		/// <summary>
		/// This method is to get the collection of all states belongs to the country which is in 'country' parameter
		/// </summary>
		/// <param name="country">This is country name</param>
		/// <returns>Collection of states</returns>
		List<string> States(string country);

		/// <summary>
		/// This method is to get the collection of all states belongs to the USA 
		/// </summary>
		/// <returns>Collection of states</returns>
		List<string> USStates();

		/// <summary>
		/// This method is to get the collection of all countries belongs to the channel partner 
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <returns>Collection of countries</returns>
		List<string> IdCountries(long channelPartnerId);

		/// <summary>
		/// This method is to get the collection of all states by channel partner id, country and Alloy id type
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <param name="country">This is country name</param>
		/// <param name="idType">This is Alloy id type</param>
		/// <returns>Collection of states</returns>
		List<string> IdStates(long channelPartnerId, string country, string idType);

		/// <summary>
		/// This method is to get the collection of all Alloy id types by channel partner id and country
		/// </summary>
		/// <param name="channelPartnerId">This is channel  partner id</param>
		/// <param name="country">This is country name</param>
		/// <returns>Collection of Alloy id types</returns>
		List<string> IdTypes(long channelPartnerId, string country);

		/// <summary>
		/// This method is to get the collection of all phone types
		/// </summary>
		/// <returns>Collection phone types</returns>
		List<string> PhoneTypes();

		/// <summary>
		/// This method is to get the collection of mobile providers
		/// </summary>
		/// <returns>Collection mobile providers</returns>
		List<string> MobileProviders();

		/// <summary>
		/// This method is to get the states by country and state abbreviation
		/// </summary>
		/// <param name="country">This is Country name</param>
		/// <param name="stateAbbr">This is state abbreviations</param>
		/// <returns>State name</returns>
		string GetIDState(string country, string stateAbbr);

		/// <summary>
		/// This method is to get the country by country abbreviation
		/// </summary>
		/// <param name="countryAbbr">This is country abbreviation</param>		
		/// <returns>Contry name</returns>
		string GetCountry(string countryAbbr);

		/// <summary>
		/// This method is to get the collection of legal code
		/// </summary>
		/// <returns>Collection of legal code</returns>
		List<LegalCode> GetLegalCodes();

		/// <summary>
		/// This method is to get the collection of Occupations
		/// </summary>
		/// <returns>Collection of Occupations</returns>
		List<Occupation> GetOccupations();

		/// <summary>
		/// This method is to get the collection of master countries
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <returns>Collection of master countries</returns>
		List<MasterCountry> MasterCountries(long channelPartnerId);

		/// <summary>
		/// This method is to get the master country by country code
		/// </summary>
		/// <param name="masterCountryAbbr">This is master country abbreviation name</param>
		/// <returns>Master country name</returns>
		MasterCountry GetMasterCountryByCode(string masterCountryAbbr);
	}
}
