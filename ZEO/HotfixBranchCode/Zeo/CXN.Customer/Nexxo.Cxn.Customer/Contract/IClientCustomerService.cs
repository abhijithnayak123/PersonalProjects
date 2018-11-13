using MGI.Cxn.Customer.Data;
using System.Collections.Generic;
using MGI.Common.Util;

namespace MGI.Cxn.Customer.Contract
{
    public interface IClientCustomerService 
    {
		/// <summary>
		/// This method will fetch the customer details from client.
		/// </summary>
		/// <param name="search">This is dictionary object which contains customer search criteria  </param>
		/// <returns>Returns customer data</returns>
        CustomerProfile Fetch(MGIContext search);

		/// <summary>
		/// This method will fetch the list of customer details from client.
		/// </summary>
		/// <param name="customerLookUpCriteria">This is dictionary object which contains customer search criteria </param>
		/// <param name="cxnContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Returns list of customer data</returns>
        List<CustomerProfile> FetchAll(Dictionary<string, object> customerLookUpCriteria, MGIContext cxnContext); 

		/// <summary>
		/// This method will push the customer data to client.
		/// </summary>
		/// <param name="customer">This parameter contains the customer details that need to be pushed to client</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Returns client customer id</returns>
        long Add(CustomerProfile customer, MGIContext mgiContext);

		/// <summary>
		/// Update CXN customer account details
		/// </summary>
		/// <param name="id">This parameter contains CXN id</param>
		/// <param name="customer"> This parameter contains updated customer data</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        void Update(string id, CustomerProfile customer, MGIContext mgiContext);

		/// <summary>
		/// Add miscellaneous account in client. Used to associate  account relationship in client for connections and for connects customers with their client customer id 
		/// </summary>
		/// <param name="account">This parameter contains the customer details</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Returns client customer id</returns>
        long AddAccount(CustomerProfile account,MGIContext mgiContext);

		/// <summary>
		/// Validate Customer status against client
		/// </summary>
		/// <param name="CXNId">This parameter contains CXN id</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        void ValidateCustomerStatus(long CXNId, MGIContext mgiContext);

		/// <summary>
		/// Add CXN customer account details
		/// </summary>
		/// <param name="customer">This parameter contains customer data to be added in CXN database</param>
		/// <param name="mgiContext">This is the common class used to pass supplimental information</param>
		/// <returns>Returns CXN Id</returns>
        long AddCXNAccount(CustomerProfile customer, MGIContext mgiContext);

		/// <summary>
		/// This method will used to find customer status against client
		/// </summary>
		/// <param name="cxnAccountId">This parameter contains CXN id</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Returns customer profile status against client</returns>
		ProfileStatus GetClientProfileStatus(long cxnAccountId, MGIContext mgiContext);

		/// <summary>
		/// This method will used to get the client customer Id
		/// </summary>
		/// <param name="cxnAccountId">This parameter contains CXN Id</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
		/// <returns>Retuns client customer id</returns>
		string GetClientCustID(long cxnAccountId, MGIContext mgiContext);

		/// <summary>
		/// This method will used to identify if the customer is newly registered in Alloy and has performed any transaction or not
		/// </summary>
		/// <param name="cxnAccountId">This parameter contains CXN id</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Returns false if the customer is newly created and has not performed any transaction, otherwise retuns true.</returns>
		bool GetCustInd(long cxnAccountId, MGIContext mgiContext);
    }
}
