using System.Collections.Generic;

#region Zeo References
using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;
#endregion

namespace TCF.Zeo.Biz.Customer.Contract
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// This method is used to push the customer data to client.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        long RegisterToClient(commonData.ZeoContext context);

        /// <summary>
        /// This method will update the customer data to client.
        /// </summary>
        /// <param name="context"></param>
        void UpdateCustomerToClient(commonData.ZeoContext context);

        /// <summary>
        /// This method is used to sync in the customer details with TCF.
        /// </summary>
        /// <param name="context"></param>
        void CustomerSyncInFromClient(commonData.ZeoContext context);

        /// <summary>
        /// This method is used to validate all required fields of core customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool ValidateCustomer(CustomerProfile customer, commonData.ZeoContext context);

        /// <summary>
        /// This method is used to search the customers either by card number or customer details.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        List<CustomerProfile> SearchCustomers(CustomerSearchCriteria criteria, commonData.ZeoContext context);

        /// <summary>
        /// To get the customers from the card number 
        /// </summary>
        /// <param name="cardnumber"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        bool IsCardValidForCustomer(string cardnumber, commonData.ZeoContext context);

    }
}
