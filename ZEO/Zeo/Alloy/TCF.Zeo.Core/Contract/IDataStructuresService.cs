using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Contract
{
    public interface IDataStructuresService
    {
        /// <summary>
        /// This method is to get the collection of all phone types.
        /// </summary>
        /// <returns>Collection phone types</returns>
        List<string> PhoneTypes(ZeoContext context);

        /// <summary>
        /// This method is to get the collection of mobile providers.
        /// </summary>
        /// <returns>Collection mobile providers</returns>
        List<string> MobileProviders(ZeoContext context);

        /// <summary>
		/// This method is to get the collection of master countries.
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <returns>Collection of master countries</returns>
		List<MasterCountry> MasterCountries(long channelPartnerId, ZeoContext context);

        /// <summary>
        /// This method is to get the collection of all states belongs to the country which is in 'country' parameter.
        /// </summary>
        /// <param name="country">This is country name</param>
        /// <returns>Collection of states</returns>
        List<string> States(string country, ZeoContext context);

        /// <summary>
        /// This method is to get the collection of all states belongs to the USA.
        /// </summary>
        /// <returns>Collection of states</returns>
        List<string> USStates(ZeoContext context);

        /// <summary>
		/// This method is to get the collection of all countries belongs to the channel partner.
		/// </summary>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <returns>Collection of countries</returns>
		List<string> IdCountries(long channelPartnerId, ZeoContext context);

        /// <summary>
        /// This method is to get the collection of all states by channel partner id, country and Alloy id type.
        /// </summary>
        /// <param name="channelPartnerId">This is channel partner id</param>
        /// <param name="country">This is country name</param>
        /// <param name="idType">This is Alloy id type</param>
        /// <returns>Collection of states</returns>
        List<string> IdStates(long channelPartnerId, string country, string idType, ZeoContext context);

        /// <summary>
        /// This method is to get the collection of all Alloy id types by channel partner id and country.
        /// </summary>
        /// <param name="channelPartnerId">This is channel  partner id</param>
        /// <param name="country">This is country name</param>
        /// <returns>Collection of Alloy id types</returns>
        List<string> IdTypes(long channelPartnerId, string country, ZeoContext context);

        /// <summary>
        /// This method is to get the collection of legal code.
        /// </summary>
        /// <returns>Collection of legal code</returns>
        List<LegalCode> GetLegalCodes(ZeoContext context);

        /// <summary>
        /// This method is to get the collection of Occupations.
        /// </summary>
        /// <returns>Collection of Occupations</returns>
        List<Occupation> GetOccupations(ZeoContext context);

        /// <summary>
		/// This method is to get the collection of all countries.
		/// </summary>
		/// <returns>Collection of contries</returns>
		List<string> Countries(long channelPartnerId, ZeoContext context);

        /// <summary>
        /// This method is used to get the master country by code.
        /// </summary>
        /// <param name="masterCountryAbbr2"></param>
        /// <returns></returns>
        MasterCountry GetMasterCountryByCode(string masterCountryAbbr2, ZeoContext context);

        /// <summary>
        /// This method is to get the Id type based on the country, state and IdType
        /// </summary>
        /// <returns></returns>
        IdType GetIdType(string idType, string country, string stateName, ZeoContext context);

        /// <summary>
        /// This method is used to get the state name from the state and country code.
        /// </summary>
        /// <param name="stateCode"></param>
        /// <param name="countryCode"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        string GetStateNameByCode(string stateCode, string countryCode, ZeoContext context);

        /// <summary>
        /// This method is to Get the Card details
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        List<KeyValuePair> GetBINDetails(ZeoContext context);
    }
}
