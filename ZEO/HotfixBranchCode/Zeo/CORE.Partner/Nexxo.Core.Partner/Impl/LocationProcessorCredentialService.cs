using MGI.Common.DataAccess.Contract;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.TimeStamp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Impl
{
    public class LocationProcessorCredentialService : ILocationProcessorCredentialService
    {
        public IRepository<LocationProcessorCredentials> LocationProcessorCredentialsRepo { private get; set; }

        public IList<Data.LocationProcessorCredentials> Get(long locationId)
        {
            var processorDetails = LocationProcessorCredentialsRepo.FilterBy(c => c.Location.Id == locationId);
            return processorDetails.ToList();
        }

        public bool Save(LocationProcessorCredentials processorDetails)
        {
			var existedProcessorDetails = LocationProcessorCredentialsRepo.FindBy(c => c.Id == processorDetails.Id);
            if (existedProcessorDetails == null)
            {
				processorDetails.DTServerCreate = DateTime.Now;
				processorDetails.DTTerminalCreate = Clock.DateTimeWithTimeZone(processorDetails.Location.TimezoneID);
				LocationProcessorCredentialsRepo.AddWithFlush(processorDetails);
            }
            else{
                existedProcessorDetails.Identifier = processorDetails.Identifier;
                existedProcessorDetails.UserName = processorDetails.UserName;
                existedProcessorDetails.Password = processorDetails.Password;
                existedProcessorDetails.DTServerLastModified = DateTime.Now;
				existedProcessorDetails.DTTerminalLastModified = Clock.DateTimeWithTimeZone(existedProcessorDetails.Location.TimezoneID);
            }
            return true;
        }
    }
}
