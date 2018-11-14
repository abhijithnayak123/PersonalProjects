#region References
#region System References
using System.Collections.Generic;
#endregion
#region Zeo References
using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.Customer.Data;
#endregion
#endregion 
namespace TCF.Zeo.Cxn.Customer.Contract
{
    public interface IClientCustomerService
    {
        /// <summary>
        /// This method is used to search the customer in provider.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        List<CustomerProfile> SearchCoreCustomers(CustomerSearchCriteria criteria, ZeoContext context);

        /// <summary>
        /// This method is used to push the customer data to client.
        /// </summary>
        /// <param name="customer">This parameter contains the customer details that need to be pushed to client</param>
        /// <param name="AlloyContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Returns client customer id</returns>
        string Add(CustomerProfile customer, ZeoContext context);

        /// <summary>
        /// This method is used to push the customer data to client.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="AlloyContext"></param>
        /// <returns></returns>
        long AddCXNAccount(CustomerProfile customer, ZeoContext context);

        /// <summary>
        /// This method is used to get client account data.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        CustomerProfile GetAccountByCustomerId(ZeoContext context);

        /// <summary>
        /// This method is used to get client account data.
        /// </summary>
        /// <param name="cxnAccountId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        CustomerProfile GetAccountById(long cxnAccountId, ZeoContext context);

        /// <summary>
        /// This method is used to search the customer with card number.
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        List<CustomerProfile> SearchCustomerWithCardNumber(string cardNumber, ZeoContext context);

        /// <summary>
        /// This method is used to sync in the customer details with TCF.
        /// </summary>
        /// <param name="custProfile"></param>
        /// <returns></returns>
        CustomerProfile CustomerSyncInFromClient(CustomerProfile custProfile, ZeoContext context);

        /// <summary>
        /// To get the matching customers from the Zeo database based on the PKY number and SSN
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        List<CustomerProfile> GetCustomersForSingleSearch(List<CustomerProfile> customers, string ssn, ZeoContext context);

        /// <summary>
        /// To get the matching customers from the Zeo database based on the PKY number, SSN, DOB and Last name
        /// </summary>
        /// <param name="customers"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        List<CustomerProfile> GetCustomersForAutoSearch(List<CustomerProfile> customers, CustomerSearchCriteria cxnCriteria, ZeoContext context);

        /// <summary>
        /// Register the Customer directly with out using the message broker.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        string RegisterCustomer(CustomerProfile customer, ZeoContext context);

        #region Legacy Contracts - For Reference Only

        ///// <summary>
        ///// This method will fetch the customer details from client.
        ///// </summary>
        ///// <param name="search">This is dictionary object which contains customer search criteria  </param>
        ///// <returns>Returns customer data</returns>
        //CustomerProfile Fetch(AlloyContext search);

        ///// <summary>
        ///// This method will fetch the list of customer details from client.
        ///// </summary>
        ///// <param name="customerLookUpCriteria">This is dictionary object which contains customer search criteria </param>
        ///// <param name="cxnContext">This is the common dictionary parameter used to pass supplimental information</param>
        ///// <returns>Returns list of customer data</returns>
        //List<CustomerProfile> FetchAll(Dictionary<string, object> customerLookUpCriteria, AlloyContext cxnContext);

        ///// <summary>
        ///// This method will push the customer data to client.
        ///// </summary>
        ///// <param name="customer">This parameter contains the customer details that need to be pushed to client</param>
        ///// <param name="AlloyContext">This is the common class parameter used to pass supplimental information</param>
        ///// <returns>Returns client customer id</returns>
        //long Add(CustomerProfile customer, AlloyContext AlloyContext);

        ///// <summary>
        ///// Update CXN customer account details
        ///// </summary>
        ///// <param name="id">This parameter contains CXN id</param>
        ///// <param name="customer"> This parameter contains updated customer data</param>
        ///// <param name="AlloyContext">This is the common class parameter used to pass supplimental information</param>
        //void Update(string id, CustomerProfile customer, AlloyContext AlloyContext);

        ///// <summary>
        ///// Add miscellaneous account in client. Used to associate  account relationship in client for connections and for connects customers with their client customer id 
        ///// </summary>
        ///// <param name="account">This parameter contains the customer details</param>
        ///// <param name="AlloyContext">This is the common class parameter used to pass supplimental information</param>
        ///// <returns>Returns client customer id</returns>
        //long AddAccount(CustomerProfile account, AlloyContext AlloyContext);

        ///// <summary>
        ///// Validate Customer status against client
        ///// </summary>
        ///// <param name="CXNId">This parameter contains CXN id</param>
        ///// <param name="AlloyContext">This is the common dictionary parameter used to pass supplimental information</param>
        //void ValidateCustomerStatus(long CXNId, AlloyContext AlloyContext);

        ///// <summary>
        ///// Add CXN customer account details
        ///// </summary>
        ///// <param name="customer">This parameter contains customer data to be added in CXN database</param>
        ///// <param name="AlloyContext">This is the common class used to pass supplimental information</param>
        ///// <returns>Returns CXN Id</returns>
        //long AddCXNAccount(CustomerProfile customer, AlloyContext AlloyContext);

        ///// <summary>
        ///// This method will used to find customer status against client
        ///// </summary>
        ///// <param name="cxnAccountId">This parameter contains CXN id</param>
        ///// <param name="AlloyContext">This is the common class parameter used to pass supplimental information</param>
        ///// <returns>Returns customer profile status against client</returns>
        //ProfileStatus GetClientProfileStatus(long cxnAccountId, AlloyContext AlloyContext);

        ///// <summary>
        ///// This method will used to get the client customer Id
        ///// </summary>
        ///// <param name="cxnAccountId">This parameter contains CXN Id</param>
        ///// <param name="AlloyContext">This is the common class parameter used to pass supplimental information</param>
        ///// <returns>Retuns client customer id</returns>
        //string GetClientCustID(long cxnAccountId, AlloyContext AlloyContext);

        ///// <summary>
        ///// This method will used to identify if the customer is newly registered in Alloy and has performed any transaction or not
        ///// </summary>
        ///// <param name="cxnAccountId">This parameter contains CXN id</param>
        ///// <param name="AlloyContext">This is the common dictionary parameter used to pass supplimental information</param>
        ///// <returns>Returns false if the customer is newly created and has not performed any transaction, otherwise retuns true.</returns>
        //bool GetCustInd(long cxnAccountId, AlloyContext AlloyContext);

        ///// <summary>
        ///// This method is used to search the customers from the channel partners core customers systems.
        ///// </summary>
        ///// <param name="criteria">search parameters to search the customer</param>
        ///// <param name="context">context object</param>
        ///// <returns></returns>        

        #endregion
    }
}
