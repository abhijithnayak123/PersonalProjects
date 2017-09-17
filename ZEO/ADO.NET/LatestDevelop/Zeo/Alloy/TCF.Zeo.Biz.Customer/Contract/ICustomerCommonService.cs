using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Customer.Contract
{
    /// <summary>
    /// This interface will have the methods common to all channel partners. This will talk to Core layer to get the data.
    /// This interface will not added to each channel partner as it will be a logic duplication for all the channel partner files.
    /// </summary>
    public interface ICustomerCommonService
    {
        /// <summary>
        /// This method is used to validate the customer SSN against the exsisting customer SSN.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool ValidateSSN(commonData.ZeoContext context);

        /// <summary>
        /// This method is used to create a new customer in Alloy.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        long InsertCustomer(CustomerProfile customer, commonData.ZeoContext context);

        /// <summary>
        /// This method is used to update the existing customer in Alloy.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool UpdateCustomer(CustomerProfile customer, commonData.ZeoContext context);

        /// <summary>
        /// This method is used to get customer by customer Id.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        CustomerProfile GetCustomer(commonData.ZeoContext context);

        /// <summary>
        /// This method is used to create a customer session.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        CustomerSession InitiateCustomerSession(CardSearchType cardSearchType, commonData.ZeoContext context);

        /// <summary>
        /// This method is used to whether the agent has a permission to close the customer.
        /// </summary>
        /// <param name="profileStatus"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool CanChangeProfileStatus(string profileStatus, commonData.ZeoContext context);

        /// <summary>
        /// This method is used to update error message for customer
        /// </summary>
        /// <param name="errorReason">reason for customer registration failure</param>
        /// <param name="context"></param>
        void UpdateCustomerRegistrationStatus(ProfileStatus status, string clientId, string errorReason,
            bool isRCIFSuccess, commonData.ZeoContext context);

    }
}
