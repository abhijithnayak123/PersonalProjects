using System;
using System.Collections.Generic;

using MGI.Biz.Customer.Data;
using MGI.Common.Util;

namespace MGI.Biz.Customer.Contract
{
	public interface ICustomerService
	{
		/// <summary>
        /// Activates the prospect customer details in alloy.
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="sessionContext">A transient instance of SessionContext[Class]</param>
		/// <param name="alloyId">This is the unique identifier for Customer in Alloy</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        void Register(long agentSessionId, SessionContext sessionContext, long alloyId, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// This method to search customer based on search criteria.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="criteria">A transient instance of CustomerSearchCriteria[Class] </param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>List Of Customers</returns>
        List<CustomerSearchResult> Search(long agentSessionId, CustomerSearchCriteria criteria, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// To Create the customer session[Begin customer session].
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="alloyId">This is the unique identifier for Customer in Alloy</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Customer Session Details</returns>
        CustomerSession InitiateCustomerSession(long agentSessionId, long alloyId, MGI.Common.Util.MGIContext mgiContext);

		/// <summary>
		/// To fetch customer based on alloy id.
		/// </summary>
        /// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
        /// <param name="alloyId">This is the unique identifier for Customer in Alloy</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Customer Details</returns>
        Data.Customer GetCustomer(long customerSessionId, long alloyId, MGI.Common.Util.MGIContext mgiContext);

		/// <summary>
        /// Save the Customer record
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent Session</param>
		/// <param name="alloyId">This is the unique identifier for Customer in Alloy</param>
        /// <param name="customer">A transient instance of Customer[Class]</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        void SaveCustomer(long agentSessionId, long alloyId, Data.Customer customer, MGI.Common.Util.MGIContext mgiContext);

		/// <summary>
        /// This method to Check for agent role to change customer profile status.
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent Session</param>
		/// <param name="userId">This is the unique identifier for agent</param>
		/// <param name="profileStatus">Customer profile status</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Customer profile Status permission</returns>
        bool CanChangeProfileStatus(long agentSessionId, long userId, string profileStatus, MGI.Common.Util.MGIContext mgiContext);
        
        /// <summary>
        /// To fetch customer based on phone number and pin code.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="phone">This field is Phone Number in customers table for CXE database</param>
        /// <param name="pin">This field is PIN code in Customers table for CXE database</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Customer Details</returns>
        Data.Customer Get(long agentSessionId, string phone, string pin, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// To fetch customer based on card number.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="cardNumber">This is the card number</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns></returns>
        Data.Customer GetCustomerForCardNumber(long agentSessionId, string cardNumber, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// This method is used to get the Validate SSN Number.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="SSN">This is the unique id.</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Validation status</returns>
        bool ValidateSSN(long agentSessionId, string SSN, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// This method is used to validates last 4 digits of customer's SSN.
        /// </summary>
        /// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
        /// <param name="last4DigitsOfSSN">Last 4 digits of customer SSN</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Validation Status</returns>
        bool IsValidSSN(long customerSessionId, string last4DigitsOfSSN, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// This method to get anonymous User PAN Number.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="channelPartnerId">This is unique identifier for channel partner</param>
        /// <param name="firstName">Customer first Name</param>
        /// <param name="lastName">Customer Last Name </param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>PAN Number</returns>
        long GetAnonymousUserPAN(long agentSessionId, long channelPartnerId, string firstName, string lastName, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// This method is used to Search Customers by Customer LookUp Criteria.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customerLookUpCriteria">This field is used Search Parameters example SSN number and DOB</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>List Of Customer</returns>
        List<Data.Customer> CustomerLookUp(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// This method to Validate Customer.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="alloyId">This is the unique identifier for Customer in Alloy</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        void ValidateCustomerStatus(long agentSessionId, long alloyId, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// Publish events to register a customer to a client.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="alloyId">This is the unique identifier for Customer in Alloy</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        void RegisterToClient(long agentSessionId, long alloyId, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// Publish events to Update a customer details to client.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="alloyId">This is the unique identifier for Customer in Alloy</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        void UpdateCustomerToClient(long agentSessionId, long alloyId, MGI.Common.Util.MGIContext mgiContext);

		/// <summary>
        /// Publish events to SyncIn customer details from client.
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent Session</param>
		/// <param name="alloyId">This is the unique identifier for Customer in Alloy</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        void CustomerSyncInFromClient(long agentSessionId, long alloyId, MGI.Common.Util.MGIContext mgiContext);
		
        /// <summary>
        /// Get client profile status
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="alloyId">This is the unique identifier for Customer in Alloy</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>CUstomer Profile status</returns>
        ProfileStatus GetClientProfileStatus(long agentSessionId, long alloyId, MGI.Common.Util.MGIContext mgiContext);

        /// <summary>
        /// Validate Customer Required Fields.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="customer">A transient instance of Customer[Class]</param>
        /// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Validation Status</returns>
        bool ValidateCustomer(long agentSessionId, Biz.Customer.Data.Customer customer, MGI.Common.Util.MGIContext mgiContext);
        
	}
}