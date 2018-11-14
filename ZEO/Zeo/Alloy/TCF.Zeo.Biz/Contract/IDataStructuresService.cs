using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Contract
{
    public interface IDataStructuresService
    {
        /// <summary>
        /// This method is used for fetching the Phone Types.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        List<string> GetPhoneTypes(commonData.ZeoContext context);

        /// <summary>
        /// This method is used for fetching the Phone Providers.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        List<string> GetMobileProviders(commonData.ZeoContext context);

        /// <summary>
        /// This method is used for fetching the Master Countries.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		List<MasterCountry> GetMasterCountries(commonData.ZeoContext context);

        /// <summary>
        /// This method is used for fetching the States.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        List<string> GetStates(string country, commonData.ZeoContext context);

        /// <summary>
        /// This method is used for fetching the US States.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        List<string> GetUSStates(commonData.ZeoContext context);

        /// <summary>
        /// This method is used for fetching the Id Countries.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
		List<string> GetIdCountries(commonData.ZeoContext context);

        /// <summary>
        /// This method is used for fetching the Id States.
        /// </summary>
        /// <param name="idType"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        List<string> GetIdStates(string idType, string country, commonData.ZeoContext context);

        /// <summary>
        /// This method is used for fetching the Id Types.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        List<string> GetIdTypes(string country, commonData.ZeoContext context);

        /// <summary>
        /// This method is used for fetching the Legal Codes.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        List<LegalCode> GetLegalCodes(commonData.ZeoContext context);

        /// <summary>
        /// This method is used for fetching the Occupations.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        List<Occupation> GetOccupations(commonData.ZeoContext context);

        /// <summary>
        /// This method is used for fetching the Countries.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        List<string> GetCountries(commonData.ZeoContext context);
        
        /// <summary>
        ///This method is used for fetching Idtypes 
        /// </summary>
        /// <param name="idType"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        IdType GetIdType(string idType, string country, string stateName, commonData.ZeoContext context);

        /// <summary>
        /// This method is used for getting the state name based on state and country code.
        /// </summary>
        /// <param name="stateCode"></param>
        /// <param name="countryCode"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        string GetStateNameByCode(string stateCode, string countryCode, commonData.ZeoContext context);

        /// <summary>
        /// This method is to get card details
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        List<KeyValuePair> GetBINDetails(commonData.ZeoContext context);
    }
}
