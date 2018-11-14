using System;

using MGI.Biz.Partner.Data;
using System.Collections.Generic;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Contract
{
	public interface INexxoDataStructuresService
	{
        /// <summary>
        /// This method is to get the Alloy id type by channel partner id, name, country and state
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="channelPartnerId">This is unique identifier for Channel partner</param>
        /// <param name="name">Goverment Id Type</param>
        /// <param name="country">Id issue country code.</param>
        /// <param name="state">Id Issuer State</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Goverment Id Details[Alloy Id Types]</returns>
		NexxoIdType Find(long agentSessionId, long channelPartnerId, string name, string country, string state, MGIContext mgiContext);

        /// <summary>
        /// This method to fetch all country from Partner database.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Countries</returns>
		List<string> Countries(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to fetch all state from Partner database based on country.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="country">the country code</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of States</returns>
		List<string> States(long agentSessionId, string country, MGIContext mgiContext);

        /// <summary>
        /// This method to fetch all states within US country.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of States</returns>
		List<string> USStates(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to fetch ID [Goverment Id Type] counties based on channel partner id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="channelPartnerId">This is unique identifier for Channel partner</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Collection Of Countries</returns>
		List<string> IdCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext);

        /// <summary>
        /// This method to get ID states based on channel partner id, country code, IdType [Goverment Id Type].
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="channelPartnerId">This is unique identifier for Channel partner</param>
        /// <param name="country">the country code</param>
        /// <param name="idType">Identification prof type [Goverment Id Type]</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Collection of States</returns>
		List<string> IdStates(long agentSessionId, long channelPartnerId, string country, string idType, MGIContext mgiContext);

        /// <summary>
        /// This method to get list of Id [Goverment Id Type] Types based on Channel partner id and country code.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="channelPartnerId">This is unique identifier for Channel partner</param>
        /// <param name="country">the country code</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Goverment Id Types</returns>
		List<string> IdTypes(long agentSessionId, long channelPartnerId, string country, MGIContext mgiContext);

        /// <summary>
        /// This method to fetch Legal codes [Resident Alien, Non Resident Alien, US Citizen].
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Legal Codes</returns>
		List<LegalCode> GetLegalCodes(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to fetch Occupations.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Collection Of Occupations</returns>
		List<Occupation> GetOccupations(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to fetch all phone types.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Phone Types</returns>
		List<string> PhoneTypes(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get all Mobile provider.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Mobile Providers</returns>
		List<string> MobileProviders(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get countries from master table.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="channelPartnerId">This is unique identifier for Channel partner</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Countries</returns>
		List<MasterCountry> MasterCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext);

        /// <summary>
        /// This method to get Country From master table based on country code.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="masterCountryAbbr2">The country code.</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Country Details</returns>
		MasterCountry GetMasterCountryByCode(long agentSessionId, string masterCountryAbbr2, MGIContext mgiContext);
	}
}
