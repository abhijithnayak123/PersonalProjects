#region Zeo References
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using TCF.Zeo.Biz.Customer.Impl;
using BizCustomer = TCF.Zeo.Biz.Customer.Contract;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using System.Collections.Generic;
using TCF.Zeo.Biz.Impl;
using System.Linq;
using TCF.Zeo.Biz.Check.Impl;
using TCF.Zeo.Biz.MoneyOrder.Impl;
#endregion

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : ICustomerService
    {
        public BizCustomer.ICustomerService CustomerService;

        /// <summary>
        /// This method is used to search core customers from clients System Of Records (SOR).
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response SearchCoreCustomers(CustomerSearchCriteria searchCriteria, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            CustomerService = new CustomerService();
            response.Result = CustomerService.SearchCoreCustomers(searchCriteria, commonContext);

            return response;

        }

        /// <summary>
        /// This method is used to search the card customer.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response SearchCardCustomer(CustomerSearchCriteria criteria, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            CustomerService = new CustomerService();
            response.Result = CustomerService.SearchCardCustomer(criteria, commonContext);
            return response;
        }

        /// <summary>
        /// This method is used to check whether the SSN is valid or not.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response ValidateSSN(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            CustomerService = new CustomerService();
            response.Result = CustomerService.ValidateSSN(commonContext);
            return response;
        }

        /// <summary>
        /// This method is used to insert the customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response InsertCustomer(CustomerProfile customer, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            CustomerService = new CustomerService();
            response.Result = CustomerService.InsertCustomer(customer, commonContext);
            return response;
        }

        /// <summary>
        /// This method is used to update the customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response UpdateCustomer(CustomerProfile customer, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            CustomerService = new CustomerService();
            response.Result = CustomerService.UpdateCustomer(customer, commonContext);

            return response;
        }

        /// <summary>
        /// This method is create customer at client side.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response RegisterToClient(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            CustomerService = new CustomerService();
            response.Result = CustomerService.RegisterToClient(commonContext);

            return response;
        }

        /// <summary>
        /// This method is used to get customer by customer Id.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response GetCustomer(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            CustomerService = new CustomerService();
            response.Result = CustomerService.GetCustomer(commonContext);
            return response;
        }

        /// <summary>
        /// This method is used to sync in the customer details with TCF.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response CustomerSyncInFromClient(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            CustomerService = new CustomerService();
            CustomerService.CustomerSyncInFromClient(commonContext);
            return response;
        }

        /// <summary>
        /// This method is used to initialize the customer session.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response InitiateCustomerSession(int cardSearchType, ZeoContext context)
        {
            CustomerSession customerSession = null;

            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
                Response response = new Response();
                CustomerService = new CustomerService();
                response.Result = CustomerService.InitiateCustomerSession((TCF.Zeo.Common.Util.Helper.CardSearchType)cardSearchType, commonContext);
                customerSession = response.Result as CustomerSession;

                if (customerSession != null && customerSession.CartId != 0)
                    GetRevisedFeeForParkedTransactions(customerSession.CustomerSessionId, commonContext);

                scope.Complete();

                return response;
            }
        }

        /// <summary>
        /// This method is updates customer at client side.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response UpdateCustomerToClient(ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);
            Response response = new Response();
            CustomerService = new CustomerService();
            CustomerService.UpdateCustomerToClient(commonContext);
            return response;
        }

        /// <summary>
        /// This method is used to validate all required fields of core customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response ValidateCustomer(CustomerProfile customer, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            CustomerService = new CustomerService();
            CustomerService.ValidateCustomer(customer, commonContext);
            return response;
        }

        /// <summary>
        /// This method is used to whether the agent has a permission to close the customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response CanChangeProfileStatus(string profileStatus, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            CustomerService = new CustomerService();
            response.Result = CustomerService.CanChangeProfileStatus(profileStatus, commonContext);
            return response;
        }

        /// <summary>
        /// This method is used to search the customers either by card number or customer details.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response SearchCustomers(CustomerSearchCriteria criteria, Data.ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            CustomerService = new CustomerService();
            response.Result = CustomerService.SearchCustomers(criteria, commonContext);
            return response;
        }


        private void GetRevisedFeeForParkedTransactions(long customerSessionId, commonData.ZeoContext commonContext)
        {
            commonContext.CustomerSessionId = customerSessionId;

            BIZShoppingCartService = new ShoppingCartServiceImpl();

            CheckService = new CPServiceImpl();

            MoneyOrderService = new MoneyOrderServicesImpl();

            List<long> checkTransactionIds = BIZShoppingCartService.GetResubmitTransactions((int)Helper.Product.ProcessCheck, customerSessionId, commonContext).OrderBy(i => i).ToList();

            foreach (long transactionId in checkTransactionIds)
            {
                CheckService.Resubmit(transactionId, commonContext);
            }

            List<long> moneyOrderTrxIds = BIZShoppingCartService.GetResubmitTransactions((int)Helper.Product.MoneyOrder, customerSessionId, commonContext).OrderBy(i => i).ToList();

            foreach (long transactionId in moneyOrderTrxIds)
            {
                MoneyOrderService.Resubmit(transactionId, commonContext);
            }
        }

    }
}
