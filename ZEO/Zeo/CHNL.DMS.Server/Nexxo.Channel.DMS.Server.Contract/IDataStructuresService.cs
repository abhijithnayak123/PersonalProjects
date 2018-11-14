using MGI.Channel.DMS.Server.Data;
using MGI.Common.Sys;
using System.ServiceModel;

namespace MGI.Channel.DMS.Server.Contract
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ServiceContract]
    public interface IDataStructuresService
    {
        /// <summary>
        /// This method to get all country from Partner database.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Countries</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response Countries(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get all state from Partner database based on country.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="country">the country code</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of States</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response States(long agentSessionId, string country, MGIContext mgiContext);

        /// <summary>
        /// This method to get all states within US country.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of States</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        Response USStates(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get all cities from Partner database based on state.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="state">the state code.</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Cities</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        Response Cities(long agentSessionId, string state, MGIContext mgiContext);

        /// <summary>
        /// This method to get ID [Goverment Id Type] counties based on channel partner id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="channelPartnerId">This is unique identifier for Channel partner</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Countries</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        Response IdCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext);

        /// <summary>
        /// This method to get list of Id [Goverment Id Type] Types based on Channel partner id and country code.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="channelPartnerId">This is unique identifier for Channel partner</param>
        /// <param name="country">The country code</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Goverment Id Types</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response IdTypes(long agentSessionId, long channelPartnerId, string country, MGIContext mgiContext);

        /// <summary>
        /// This method to get Legal codes [Resident Alien, Non Resident Alien, US Citizen].
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Legal Codes</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response GetLegalCodes(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get Occupations.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Occupations</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response GetOccupations(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get ID states based on channel partner id, country code, IdType [Goverment Id Type].
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="channelPartnerId">This is unique identifier for Channel partner</param>
        /// <param name="country">the country code</param>
        /// <param name="idType">Identification prof type [Goverment Id Type]</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of States</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response IdStates(long agentSessionId, long channelPartnerId, string country, string idType, MGIContext mgiContext);

        /// <summary>
        /// This method to get Goverment Id validations. 
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="channelPartnerId">This is unique identifier for Channel partner</param>
        /// <param name="country">Id issue country code.</param>
        /// <param name="idType">Identification prof type [Goverment Id Type]</param>
        /// <param name="idState">Id Issue state code.</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Goverment Id Details</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        Response IdRequirements(long agentSessionId, long channelPartnerId, string country, string idType, string idState, MGIContext mgiContext);

        /// <summary>
        /// This method to get all phone types.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Phone Types</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response PhoneTypes(long agentSessionId, MGIContext mgiContext);

        /// <summary>
        /// This method to get all Mobile provider.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="context">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Mobile Providers</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        Response MobileProviders(long agentSessionId, MGIContext context);

        /// <summary>
        /// This method to get countries from master table.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="channelPartnerId">This is unique identifier for Channel partner</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Countries</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response MasterCountries(long agentSessionId, long channelPartnerId, MGIContext mgiContext);

        /// <summary>
        /// This method to get Country From master table based on country code.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="masterCountryAbbr2">The country code.</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Country Details</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        Response GetMasterCountryByCode(long agentSessionId, string masterCountryAbbr2, MGIContext mgiContext);

        /// <summary>
        /// This method to get all profile status.[Active, InActive, Closed]
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agentSession</param>
        /// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>List Of Profile Status</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		Response ProfileStatus(long agentSessionId, MGIContext mgiContext);

    }
}
