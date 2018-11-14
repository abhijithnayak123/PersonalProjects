using TCF.Zeo.Biz.Impl;
using TCF.Zeo.Common.Util;
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using commonData = TCF.Zeo.Common.Data;
using System;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IDataStructuresService
    {


        TCF.Zeo.Biz.Contract.IDataStructuresService dataStructureService;

        public Response GetPhoneTypes(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetPhoneTypes(commonContext) };
            return response;
        }

        public Response GetPhoneProviders(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetMobileProviders(commonContext) };
            return response;
        }

        public Response GetStates(string country, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetStates(country, commonContext) };
            return response;
        }

        public Response GetUSStates(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetUSStates(commonContext) };
            return response;
        }

        public Response GetLegalCodes(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetLegalCodes(commonContext) };
            return response;
        }

        public Response GetOccupations(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetOccupations(commonContext) };
            return response;
        }

        public Response GetIdCountries(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetIdCountries(commonContext) };
            return response;
        }

        public Response GetIdStates(string idType, string country, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetIdStates(idType, country, commonContext) };
            return response;
        }

        public Response GetIdTypes(string country, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetIdTypes(country, commonContext) };
            return response;
        }

        public Response GetMasterCountries(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetMasterCountries(commonContext) };
            return response;
        }

        public Response GetCountries(ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response;
                dataStructureService = new DataStructureServiceImpl();
                response = new Response() { Result = dataStructureService.GetCountries(commonContext) };

                scope.Complete();

                return response;
            }
        }

        public Response GetIdType(string IdType, string country, string stateName, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetIdType(IdType, country, stateName, commonContext) };
            return response;
        }

        public Response GetStateNameByCode(string stateCode, string countryCode, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetStateNameByCode(stateCode, countryCode, commonContext) };
            return response;
        }

        public Response GetBINDetails(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response;
            dataStructureService = new DataStructureServiceImpl();
            response = new Response() { Result = dataStructureService.GetBINDetails(commonContext) };
            return response;
        }
    }
}
