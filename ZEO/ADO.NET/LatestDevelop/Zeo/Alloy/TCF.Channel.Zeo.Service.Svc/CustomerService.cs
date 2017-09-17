using TCF.Channel.Zeo.Service.Contract;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : ICustomerService
    {
        /// <summary>
        /// This method is used to search core customers from clients System Of Records (SOR).
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response SearchCoreCustomers(CustomerSearchCriteria searchCriteria, ZeoContext context)
        {
            return serviceEngine.SearchCoreCustomers(searchCriteria, context);
        }

        /// <summary>
        /// Search the card customer from the given criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response SearchCardCustomer(CustomerSearchCriteria criteria, ZeoContext context)
        {
            return serviceEngine.SearchCardCustomer(criteria, context);
        }

        /// <summary>
        /// This method is used to insert the customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response InsertCustomer(CustomerProfile customer, ZeoContext context)
        {
            return serviceEngine.InsertCustomer(customer, context);
        }

        /// <summary>
        /// This method is used to update the customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response UpdateCustomer(CustomerProfile customer, ZeoContext context)
        {
            return serviceEngine.UpdateCustomer(customer, context);
        }

        /// <summary>
        /// This method is used to validate the SSN.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response ValidateSSN(ZeoContext context)
        {
            return serviceEngine.ValidateSSN(context);
        }

        /// <summary>
        /// This method is used for activating the customer at client end
        /// </summary>
        /// <param name="context">commom context</param>
        /// <returns>Response</returns>
        public Response RegisterToClient(ZeoContext context)
        {
            return serviceEngine.RegisterToClient(context);
        }

        /// <summary>
        /// This method is used to get customer.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response GetCustomer(ZeoContext context)
        {
            return serviceEngine.GetCustomer(context);
        }

        /// <summary>
        /// This method is used to sync in the customer details with TCF.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response CustomerSyncInFromClient(ZeoContext context)
        {
            return serviceEngine.CustomerSyncInFromClient(context);
        }

        /// <summary>
        /// This method is used to initialize the customer session.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response InitiateCustomerSession(int cardSearchType, ZeoContext context)
        {
            return serviceEngine.InitiateCustomerSession(cardSearchType, context);
        }

        /// <summary>
        /// This method is updates customer at client side.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response UpdateCustomerToClient(ZeoContext context)
        {
            return serviceEngine.UpdateCustomerToClient(context);
        }

        /// <summary>
        /// This method is used to validate all required fields of core customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response ValidateCustomer(CustomerProfile customer, ZeoContext context)
        {
            return serviceEngine.ValidateCustomer(customer, context);
        }

        /// <summary>
        ///  This method is used to whether the agent has a permission to close the customer.
        /// </summary>
        /// <param name="profileStatus"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response CanChangeProfileStatus(string profileStatus, ZeoContext context)
        {
            return serviceEngine.CanChangeProfileStatus(profileStatus, context);
        }

        /// <summary>
        /// This method is used to search the customers either by card number or customer details.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response SearchCustomers(CustomerSearchCriteria criteria, ZeoContext context)
        {
            return serviceEngine.SearchCustomers(criteria, context);
        }

    }
}