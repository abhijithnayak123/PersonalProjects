using System;
using System.Collections.Generic;
using System.ServiceModel;

using MGI.Channel.DMS.Server.Data; // Nexxo references.
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;

namespace MGI.Channel.DMS.Server.Contract
{
    /// <summary>
    /// The customer service with all customer related ops.
    /// </summary>
    [ServiceContract]
    public interface ICustomerService
    {
		/// <summary>
		/// This methods is to get customer profile
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="alloyId">This is unique identifier for getting the Alloy ID from the PTNR database</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns> Customer Profile</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		Customer Lookup(long customerSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// Get registration data in customer profile
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="alloyId">This is unique identifier for getting the Alloy ID from the PTNR database</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Prospect of customer registered details</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		Prospect GetProspect(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// Save initial registration data
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="prospect">This field is used to get the customer prospect registration in  PTNR database</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Part of customer Registration data stored</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract(Name = "Create")]
		string Save(long agentSessionId, Prospect prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// Save registration
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="alloyId">This is unique identifier for getting the Alloy ID from the PTNR database</param>
		/// <param name="prospect">This field is used to get the customer prospect registration in  PTNR database</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		void Save(long agentSessionId, long alloyId, Prospect prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// This method is used to update customer profile details
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="alloyId">This is unique identifier for getting the Alloy ID from the PTNR database</param>
		/// <param name="prospect">This field is used to get the customer prospect registration in  PTNR database</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		void UpdateCustomer(long agentSessionId, long alloyId, Prospect prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// This methods is Customer search
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="searchCriteria">This field is used to search the customer example for Last name and Phone number or SSN number</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Customer Profile</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		List<CustomerSearchResult> SearchCustomers(long agentSessionId, CustomerSearchCriteria searchCriteria, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// Begin customer session
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="authentication">This field is authenticate the initiate customer session used to alloy ID </param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Customer Session</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		CustomerSession InitiateCustomerSession(long agentSessionId, CustomerAuthentication authentication, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);
        
		/// <summary>
		/// Recording Identification Confirmation
		/// </summary>
		/// <param name="customerSessionId">This is unique identifier for getting the Customer session id from the customer details</param>
		/// <param name="agentId">This is unique identifier for Agent ID for agent details in PTNR database</param>
		/// <param name="IdentificationStatus"></param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Identifcation confirmation</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		string RecordIdentificationConfirmation(long customerSessionId, string agentId, bool IdentificationStatus, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);
        
		/// <summary>
		/// This method is used to get customer by phone+pin
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="Phone">This field is Phone Number in customers table for CXE database</param>
		/// <param name="PIN">This field is PIN code in Customers table for CXE database</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Get the customer details</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract(Name = "GetCustomer")]
		Customer Get(long agentSessionId, string Phone, string PIN, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// This method is used to get the Validate SSN Number
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="SSN">This field is SSN Number</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Booleans of SSN status</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		bool ValidateSSN(long agentSessionId, string SSN, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// This methods is used to get the Validate Customer RequiredFields
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="prospect">This field is used to get the prospect of the customer details</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Validated the required customers</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		bool ValidateCustomer(long agentSessionId, Prospect prospect, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// This methods is used to Get anonymous PAN
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="channelPartnerId">This is ChannelPartner ID</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		/// <returns>Anonymous Customer ID</returns>
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
		long GetAnonymousUserPAN(long agentSessionId, long channelPartnerId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

				
        /// <summary>
		///  This method is used to Search Customers by Customer LookUp Parameters
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="customerLookUpCriteria">This field is used Search Parameters example SSN number and DOB</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        /// <returns>Search Customers list</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		List<Prospect> CustomerLookUp(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// This method is used to validate the customer status example Active or Inactive or Closed
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="alloyId">This is unique identifier for getting the Alloy ID from the PTNR database</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		void ValidateCustomerStatus(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

        /// <summary>
		/// Alloy Registration. This method to register customer in alloy.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
        /// <param name="alloyId">This is unique identifier for getting the Alloy ID from the PTNR database</param>
        /// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		void NexxoActivate(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// This method is used to the register the Client Registration
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="alloyId">This is unique identifier for getting the Alloy ID from the PTNR database</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		void ClientActivate(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// Client Registration
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="alloyId">This is unique identifier for getting the Alloy ID from the PTNR database</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
		void UpdateCustomerToClient(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);

		/// <summary>
		/// SyncIn Customer Details from Client
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="alloyId">This is unique identifier for getting the Alloy ID from the PTNR database</param>
		/// <param name="context">This is the common dictionary parameter used to pass supplimental information</param>
		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
        void CustomerSyncInFromClient(long agentSessionId, long alloyId, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);
    	
		/// <summary>
		/// This method is used to Check profile status change permission
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for Agent SessionID for agent session details</param>
		/// <param name="profileStatus">This field is Checking whether user can able to change profile status</param>
		/// <returns>Customer profile Status permission</returns>
		[FaultContract(typeof(NexxoSOAPFault))]
		[OperationContract]
		bool CanChangeProfileStatus(long agentSessionId, long userId, string profileStatus, MGI.Channel.DMS.Server.Data.MGIContext mgiContext);
    }
}

