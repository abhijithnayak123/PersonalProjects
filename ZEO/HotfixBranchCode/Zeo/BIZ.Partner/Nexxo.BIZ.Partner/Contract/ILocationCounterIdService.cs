using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Partner.Data;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Contract
{
	public interface ILocationCounterIdService
	{
        /// <summary>
        /// This method to Update Counter Id.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for customer Session</param>
        /// <param name="context">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Updated status of counter id</returns>
		bool UpdateCounterId(long customerSessionId, MGIContext mgiContext);
	}
}
