using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TCF.Zeo.Common.Data
{
    public class ZeoContext
    {
        public ZeoContext()
        {
            WUCounterId = string.Empty;
        }

        public string AgentFirstName { get; set; }
        public long AgentId { get; set; }
        public string AgentLastName { get; set; }
        public string AgentName { get; set; }
        public string ClientAgentIdentifier { get; set; }
        public string WUCounterId { get; set; }
        public bool ShouldIncludeShoppingCartItems { get; set; }
        public long CustomerId { get; set; }
        public string TimeZone { get; set; }
        public string BankId { get; set; }
        public string BranchId { get; set; }
        public long CXNAccountId { get; set; }
        public int IngoBranchId { get; set; }
        public long ChannelPartnerId { get; set; }
        public string ChannelPartnerName { get; set; }
        public string CheckPassword { get; set; }
        public string CheckUserName { get; set; }
        public long CustomerSessionId { get; set; }
        public bool IsReferral { get; set; }
        public string LocationName { get; set; }
        public long LocationID { get; set; }
        public int ProviderId { get; set; }
        public string TerminalName { get; set; }
        public string RequestType { get; set; }
        public long AgentSessionId { get; set; }
        public string PromotionCode { get; set; }
        public bool IsSystemApplied { get; set; }
        public string URL { get; set; }
        public string CompanyToken { get; set; }
        public int EmployeeId { get; set; }
        public bool IsParked { get; set; }
        public string ProductType { get; set; }
        public string StateCode { get; set; }
        public string LocationId { get; set; }
        public string SMTrxType { get; set; }
        public string RMTrxType { get; set; }
        public string ReferenceNumber { get; set; }
        public string SSN { get; set; }
        public string LocationStateCode { get; set; }
        public int CardExpiryPeriod { get; set; }
        public long AddOnCustomerId { get; set; }
        public int CardClass { get; set; }
        public long VisaLocationNodeId { get; set; }
        public Dictionary<string, object> Context { get; set; }
        public string WUCardNumber { get; set; }
		public Dictionary<string, object> SSOAttributes { get; set; }
        public string LocationZipCode { get; set; }
        public long PromotionId { get; set; }
    public decimal MaxAmountOnUsCheck { get; set; }
        public int OnUSChecktype { get; set; }
    }
}
