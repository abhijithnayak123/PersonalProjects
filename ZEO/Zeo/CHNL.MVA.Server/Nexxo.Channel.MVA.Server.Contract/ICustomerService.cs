using System.Collections.Generic;
using System.ServiceModel;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;

namespace MGI.Channel.MVA.Server.Contract
{
    /// <summary>
    /// The customer service with all customer related ops.
    /// </summary>
    [ServiceContract]
    public interface ICustomerService 
    {

        /// <summary>
        /// This method Finds customer by card in Nexxo repository
        /// If exists then returns customer session 
        /// if not exists lookup for customer in client repository and creates new customer and then returns customer session. 
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="channelPartnerId"></param>
        /// <returns>Customer Session ID</returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
        CustomerSession InitiateCustomerSession(string cardNumber, string channelPartnerName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="last4DigitsOfSSN"></param>
        /// <returns></returns>
        [FaultContract(typeof(NexxoSOAPFault))]
        [OperationContract]
        bool IsValidSSN(string customerSessionId, string last4DigitsOfSSN);
    }
}

