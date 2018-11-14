using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using TCF.Channel.Zeo.Service.Contract;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.Impl;
using System.ServiceModel;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : ILocationProcessorCredentialsService
    {
        public TCF.Zeo.Biz.Contract.ILocationProcessorCredentialService LocationProcessorCredentialsService { get; set; }

        public Response SaveLocationProcessorCredential(LocationProcessorCredentials processorCredentials, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            LocationProcessorCredentialsService = new LocationProcessorCredentialServiceImpl();
            response.Result = LocationProcessorCredentialsService.SaveLocationProcessorCredentials(processorCredentials, commonContext);

            return response;
        }

        public Data.Response GetLocationProcessorCredentials(long locationId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            LocationProcessorCredentialsService = new LocationProcessorCredentialServiceImpl();
            response.Result = LocationProcessorCredentialsService.GetLocationProcessorCredentials(locationId, commonContext);

            return response;
        }

    }
}
