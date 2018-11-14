using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;
using System.IO;

namespace TCF.Zeo.Core.Contract
{
    public interface ICustomerService : IDisposable
    {
        /// <summary>
        /// This method is used to get the Validate SSN Number.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool ValidateSSN(ZeoContext context);

        /// <summary>
        /// This method is used to create a new customer in Alloy.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        long InsertCustomer(CustomerProfile customer, ZeoContext context);

        /// <summary>
        /// This method is used to update the existing Alloy customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool UpdateCustomer(CustomerProfile customer, ZeoContext context);

        /// <summary>
        /// This method is used to get the customer.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        CustomerProfile GetCustomer(ZeoContext context);

        /// <summary>
        /// This method is used to search the customer from search criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        List<CustomerProfile> SearchCustomer(CustomerSearchCriteria criteria, ZeoContext context);

        /// <summary>
        /// This method is used to create a customer session.
        /// </summary>
        /// <param name="customerSession"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        CustomerSession CreateCustomerSession(CustomerSession customerSession, ZeoContext context);

        /// <summary>
        /// This method is used to update error message for customer
        /// </summary>
        /// <param name="errorReason">reason for customer registration failure</param>
        /// <param name="context"></param>
        void UpdateCustomerRegistrationStatus(ProfileStatus status, string clientId, string errorReason,
            bool isRCIFSuccess, ZeoContext context);

    }
}
