using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Partner.Data;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Contract
{
    public interface ILocationProcessorCredentials
    {
        /// <summary>
        /// This method is to get the collection of location processor credentials.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent session</param>
        /// <param name="locationId">This is the unique identifier for location</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of Processor Credential Details</returns>
		IList<ProcessorCredentials> Get(long agentSessionId, long locationId, MGIContext mgiContext);

        /// <summary>
        /// This method is to save the location processor credentials.
        /// </summary>
        /// <param name="agentSessionId">This is the unique identifier for agent session</param>
        /// <param name="locationId">This is the unique identifier for location</param>
        /// <param name="processorCredentials">A transient  instance of ProcessorCredentials[Class] </param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns></returns>
		bool Save(long agentSessionId, long locationId, ProcessorCredentials processorCredentials, MGIContext mgiContext);
    }
}
