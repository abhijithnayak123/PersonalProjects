using System.Collections.Generic;
using MGI.Channel.MVA.Server.Contract;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.MVA.Server.Svc
{
    public partial class MVAWSImpl : ICustomerService
    {
        public CustomerSession InitiateCustomerSession(string cardNumber, string channelPartnerName)
        {
            return MVAEngine.InitiateCustomerSession(cardNumber, channelPartnerName);
        }

        public bool IsValidSSN(string customerSessionId, string last4DigitsOfSSN)
        {
            return MVAEngine.IsValidSSN(customerSessionId, last4DigitsOfSSN);
        }
    }
}