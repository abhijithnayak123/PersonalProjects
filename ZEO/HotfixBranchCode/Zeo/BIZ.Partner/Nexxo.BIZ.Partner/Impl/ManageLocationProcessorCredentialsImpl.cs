using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Partner.Contract;
using AutoMapper;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Impl
{
    class ManageLocationProcessorCredentialsImpl : ILocationProcessorCredentials
    {
        private MGI.Core.Partner.Contract.ILocationProcessorCredentialService _locationProcessorCredentialsService;
        public MGI.Core.Partner.Contract.ILocationProcessorCredentialService LocationProcessorCredentialsService
        {
            get { return _locationProcessorCredentialsService; }
            set { _locationProcessorCredentialsService = value; }
        }

        private MGI.Core.Partner.Contract.IManageLocations _locationService;
        public MGI.Core.Partner.Contract.IManageLocations LocationService
        {
            get { return _locationService; }
            set { _locationService = value; }
        }

        public ManageLocationProcessorCredentialsImpl()
        {
            Mapper.CreateMap<Biz.Partner.Data.ProcessorCredentials, MGI.Core.Partner.Data.LocationProcessorCredentials>();
            Mapper.CreateMap<MGI.Core.Partner.Data.LocationProcessorCredentials, Biz.Partner.Data.ProcessorCredentials>();
            Mapper.CreateMap<MGI.Core.Partner.Data.Location, Biz.Partner.Data.Location>();
            Mapper.CreateMap<Biz.Partner.Data.Location, MGI.Core.Partner.Data.Location>();
        }

        public IList<Data.ProcessorCredentials> Get(long agentSessionId, long locationId, MGIContext mgiContext)
        {
            var processorCredentials = LocationProcessorCredentialsService.Get(locationId);

            return Mapper.Map<IList<MGI.Core.Partner.Data.LocationProcessorCredentials>, IList<Biz.Partner.Data.ProcessorCredentials>>(processorCredentials); 
        }

		public bool Save(long agentSessionId, long locationId, Data.ProcessorCredentials processorCredentials, MGIContext mgiContext)
        {
            var _processorCredentials = Mapper.Map<Biz.Partner.Data.ProcessorCredentials, MGI.Core.Partner.Data.LocationProcessorCredentials>(processorCredentials);

            _processorCredentials.Location = LocationService.Lookup(locationId);
            
            return LocationProcessorCredentialsService.Save(_processorCredentials);
        }
    }
}
