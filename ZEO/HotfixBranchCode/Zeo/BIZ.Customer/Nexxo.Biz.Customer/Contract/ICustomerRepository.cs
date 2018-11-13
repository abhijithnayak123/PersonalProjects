using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Customer.Data;
using MGI.Common.Util;

namespace MGI.Biz.Customer.Contract
{
    public interface ICustomerRepository
    {        
        /// <summary>
        /// This method to Get the Customer by customer LookUp Criteria.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerLookUpCriteria">This field is used Search Parameters example SSN number and DOB</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>List Of Customer</returns>
        List<Biz.Customer.Data.Customer> FetchAll(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGIContext mgiContext);
        
        /// <summary>
        /// This method is used to validate the customer status example Active or Inactive or Closed.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="CXNId">This is the unique identifier for customer</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        void ValidateCustomerStatus(long agentSessionId, long CXNId, MGIContext mgiContext);

        /// <summary>
        /// Get Client Profile status
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="cxnAccountId">This is the unique identifier for customer</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Customer Profile Status</returns>
        ProfileStatus GetClientProfileStatus(long agentSessionId, long cxnAccountId, MGIContext mgiContext);

        /// <summary>
        /// Validate Customer Required Fields.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerProfile">A transient instance of Customer[Class]</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>bool</returns>
        bool ValidateCustomerRequiredFields(long agentSessionId, Biz.Customer.Data.Customer customerProfile, MGIContext mgiContext);
    }
}
