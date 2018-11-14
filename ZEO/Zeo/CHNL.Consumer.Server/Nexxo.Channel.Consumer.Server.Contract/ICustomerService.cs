using System;
using System.Collections.Generic;
using System.ServiceModel;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Channel.Consumer.Server.Contract
{
    /// <summary>
    /// The customer service with all customer related ops.
    /// </summary>
    public interface ICustomerService
    {
		/// <summary>
		/// This method is used to the create prospect 
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for each agent session</param>
		/// <param name="prospect">This consist of all the data of the customer</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplemental information</param>
		/// <returns></returns>
        long CreateProspect(long agentSessionId, Prospect prospect, MGIContext mgiContext);

		/// <summary>
		/// This method used to save the prospect
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for each agent session</param>
		/// <param name="alloyId">This is unique identifier for customer in Alloy</param>
		/// <param name="prospect">This consist of all the data of the cutomer</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplemental information</param>
		void SaveProspect(long agentSessionId, long alloyId, Prospect prospect, MGIContext mgiContext);

        /// <summary>
        /// This method is used for Nexxo Registration
        /// </summary>
		/// <param name="agentSessionId">This is unique identifier for each agent session</param>
		/// <param name="alloyId">This is unique identifier for customer in Alloy</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplemental information</param>
        /// <returns></returns>
		void NexxoActivate(long agentSessionId, long alloyId, MGIContext mgiContext);

        /// <summary>
        ///This method is used for registaring client
        /// </summary>
		/// <param name="agentSessionId">This is unique identifier for each agent session</param>
		/// <param name="alloyId">This is unique identifier for customer in Alloy</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplemental information</param>
        /// <returns></returns>
		void ClientActivate(long agentSessionId, long alloyId, MGIContext mgiContext);

        /// <summary>
        /// This method is used for  Updating Customer Details
        /// </summary>
		/// <param name="agentSessionId">This is unique identifier for each agent session</param>
		/// <param name="alloyId">This is unique identifier for customer in Alloy</param>
		/// <param name="prospect">This consist of all the data of the customer</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplemental information</param>
		void UpdateCustomer(long agentSessionId, long alloyId, Prospect prospect, MGIContext mgiContext);

        /// <summary>
        ///  This method is used for Updating the CXN Account table and third party
        /// </summary>
		/// <param name="agentSessionId">This is unique identifier for each agent session</param>
		/// <param name="alloyId">This is unique identifier for customer in Alloy</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplemental information</param>
		void UpdateCustomerToClient(long agentSessionId, long alloyId, MGIContext mgiContext);
		
		/// <summary>
		/// This method is used to intiate the customer session.
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for each agent session</param>
		/// <param name="alloyId">This is unique identifier for customer in Alloy</param>
		/// <param name="mgiContext">This is the common dictionary parameter used to pass supplemental information</param>
		/// <returns></returns>
		CustomerSession InitiateCustomerSession(long agentSessionId, long alloyId, MGIContext mgiContext);

        /// <summary>
        /// This method is used to search Customer by CardNumber or some other Parameters in Client repository
        /// </summary>
		/// <param name="agentSessionId">This is unique identifier for each agent session</param>
		/// <param name="customerLookUpCriteria">This is the dictionary parameter used to pass supplemental information for look up criteria</param>
        /// <returns></returns>
        Prospect CustomerLookUp(long agentSessionId, Dictionary<string, object> customerLookUpCriteria, MGIContext mgiContext);


		/// <summary>
		/// This method is to get the customer based on card number and other parameters.
		/// </summary>
		/// <param name="agentSessionId">This is unique identifier for each agent session</param>
		/// <param name="cardNumber">This is a cardnumber based on which the customer have to be search</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
		/// <returns></returns>
        Customer GetCustomerByCardNumber(long agentSessionId, string cardNumber, MGIContext mgiContext);

        /// <summary>
        /// This method is used to validates last 4 digits of customer's SSN
        /// </summary>
		/// <param name="customerSessionId">his is unique identifier for each customer Session Id</param>
        /// <param name="last4DigitsOfSSN">Last 4 digits of customer SSN</param>
        /// <returns></returns>
		bool IsValidSSN(long customerSessionId, string last4DigitsOfSSN, MGIContext mgiContext);
    }
}

