using TCF.Zeo.Common.Data;
using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface IDataStructuresService
    {
        /// <summary>
        /// For Fetching the Phone Types.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetPhoneTypes(Data.ZeoContext context);

        /// <summary>
        /// For Fetching the Phone Providers.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetPhoneProviders(Data.ZeoContext context);

        /// <summary>
        /// For Fetching the Master Countries.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetMasterCountries(Data.ZeoContext context);

        /// <summary>
        /// For Fetching the States.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetStates(string country, Data.ZeoContext context);

        /// <summary>
        /// For Fetching the US States.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetUSStates(Data.ZeoContext context);

        /// <summary>
        /// For Fetching the Id Countries.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetIdCountries(Data.ZeoContext context);

        /// <summary>
        /// For Fetching the Id States.
        /// </summary>
        /// <param name="idType"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetIdStates(string idType, string country, Data.ZeoContext context);

        /// <summary>
        /// For Fetching the Id Types.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetIdTypes(string country, Data.ZeoContext context);

        /// <summary>
        /// For Fetching the Legal Codes.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetLegalCodes(Data.ZeoContext context);

        /// <summary>
        /// For Fetching the Occupations.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetOccupations(Data.ZeoContext context);

        /// <summary>
        /// For Fetching the Countries.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetCountries(Data.ZeoContext context);

        /// <summary>
        /// For Fetching the IdType.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetIdType(string IdType, string country, string stateName, Data.ZeoContext context);

        /// <summary>
        /// For getting the state name based on state and country code. 
        /// </summary>
        /// <param name="stateCode"></param>
        /// <param name="countryCode"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetStateNameByCode(string stateCode, string countryCode, Data.ZeoContext context);

        /// <summary>
        /// This method is to get the BIN details.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ZeoSoapFault))]
        Response GetBINDetails(Data.ZeoContext context);
    }
}
