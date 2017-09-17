using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IDataStructuresService
    {
        public Response GetCountries(ZeoContext context)
        {
            return serviceEngine.GetCountries(context);
        }

        public Response GetPhoneTypes(ZeoContext context)
        {
            return serviceEngine.GetPhoneTypes(context);
        }

        public Response GetLegalCodes(ZeoContext context)
        {
            return serviceEngine.GetLegalCodes(context);
        }

        public Response GetOccupations(ZeoContext context)
        {
            return serviceEngine.GetOccupations(context);
        }

        public Response GetIdCountries(ZeoContext context)
        {
            return serviceEngine.GetIdCountries(context);
        }

        public Response GetIdStates(string idType, string country, ZeoContext context)
        {
            return serviceEngine.GetIdStates(idType, country, context);
        }

        public Response GetIdTypes(string country, ZeoContext context)
        {
            return serviceEngine.GetIdTypes(country, context);
        }

        public Response GetMasterCountries(ZeoContext context)
        {
            return serviceEngine.GetMasterCountries(context);
        }

        public Response GetPhoneProviders(ZeoContext context)
        {
            return serviceEngine.GetPhoneProviders(context);
        }

        public Response GetStates(string country, ZeoContext context)
        {
            return serviceEngine.GetStates(country, context);
        }

        public Response GetUSStates(ZeoContext context)
        {
            return serviceEngine.GetUSStates(context);
        }

        public Response GetIdType(string IdType, string country, string stateName, ZeoContext context)
        {
            return serviceEngine.GetIdType(IdType, country, stateName, context);
        }

        public Response GetStateNameByCode(string stateCode, string countryCode, ZeoContext context)
        {
            return serviceEngine.GetStateNameByCode(stateCode, countryCode, context);
        }

        public Response GetBINDetails(ZeoContext context)
        {
            return serviceEngine.GetBINDetails(context);
        }

    }
}