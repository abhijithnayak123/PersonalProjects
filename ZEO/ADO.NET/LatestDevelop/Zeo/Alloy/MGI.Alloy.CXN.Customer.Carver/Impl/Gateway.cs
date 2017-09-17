using System;
using System.Collections.Generic;
using MGI.Alloy.Common.Data;
using MGI.Alloy.CXN.Customer.Contract;
using MGI.Alloy.CXN.Customer.Data;

namespace MGI.Alloy.CXN.Customer.Carver.Impl
{
    public class Gateway : IClientCustomerService
    {
        IO IOImpl;

        public Gateway()
        {
            IOImpl = getIO();
        }

        #region Public Methods

        /// <summary>
        /// This method is used to search the customer in provider.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<CustomerProfile> SearchCoreCustomers(CustomerSearchCriteria criteria, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to push the customer data to client.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="AlloyContext"></param>
        /// <returns></returns>
        public long Add(CustomerProfile customer, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to push the customer data to client.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="AlloyContext"></param>
        /// <returns></returns>
        public long AddCXNAccount(CustomerProfile customer, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to get client account data.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public CustomerProfile GetAccountByCustomerId(ZeoContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to get client account data.
        /// </summary>
        /// <param name="cxnAccountId"></param>
        /// <returns></returns>
        public CustomerProfile GetAccountById(long cxnAccountId, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to search the customer with card number.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public List<CustomerProfile> SearchCustomerWithCardNumber(string cardNumber, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is used to sync in the customer details.
        /// </summary>
        /// <param name="custProfile"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public CustomerProfile CustomerSyncInFromClient(CustomerProfile custProfile, ZeoContext context)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private IO getIO()
        {
            if (IOImpl == null)
            { return new IO(); }

            return IOImpl;
        }


        #endregion
    }
}
