using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class ZeoContext : BaseRequest
    {
        [DataMember]
        public string AgentFirstName { get; set; }

        [DataMember]
        public long AgentId { get; set; }

        [DataMember]
        public string AgentLastName { get; set; }

        [DataMember]
        public string AgentName { get; set; }

        [DataMember]
        public string BankId { get; set; }

        [DataMember]
        public string BranchId { get; set; }

        [DataMember]
        public int IngoBranchId { get; set; }

        [DataMember]
        public string CheckPassword { get; set; }

        [DataMember]
        public string CheckUserName { get; set; }

        [DataMember]
        public bool IsReferral { get; set; }

        [DataMember]
        public string LocationName { get; set; }

        [DataMember]
        public Guid LocationRowGuid { get; set; }

        [DataMember]
        public int ProviderId { get; set; }

        [DataMember]
        public string TerminalName { get; set; }

        [DataMember]
        public string TimeZone { get; set; }

        [DataMember]
        public string WUCounterId { get; set; }

        [DataMember]
        public long CxnAccountId { get; set; }

        [DataMember]
        public string RequestType { get; set; }

        [DataMember]
        public string PromotionCode { get; set; }

        [DataMember]
        public bool IsSystemApplied { get; set; }

        [DataMember]
        public string URL { get; set; }

        [DataMember]
        public string CompanyToken { get; set; }

        [DataMember]
        public int EmployeeId { get; set; }

        [DataMember]
        public bool ShouldIncludeShoppingCartItems { get; set; }

        [DataMember]
        public bool IsParked { get; set; }

        [DataMember]
        public string ProductType { get; set; }

        [DataMember]
        public string StateCode { get; set; }

        [DataMember]
        public string LocationId { get; set; }

        [DataMember]
        public string SMTrxType { get; set; }

        [DataMember]
        public string RMTrxType { get; set; }

        [DataMember]
        public string ReferenceNumber { get; set; }

        [DataMember]
        public string SSN { get; set; }

        [DataMember]
        public string LocationStateCode { get; set; }

        [DataMember]
        public int CardExpiryPeriod { get; set; }

        [DataMember]
        public long AddOnCustomerId { get; set; }

        [DataMember]
        public Dictionary<string, object> Context { get; set; }

        [DataMember]
        public int CardClass { get; set; }

        [DataMember]
        public long VisaLocationNodeId { get; set; }

        [DataMember]
        public long CustomerId { get; set; }

        [DataMember]
        public string WUCardNumber { get; set; }

        [DataMember]
        public string ClientAgentIdentifier { get; set; }

        [DataMember]
        public string LocationZipCode { get; set; }

        [DataMember]
        public long PromotionId { get; set; }

 [DataMember]
        public decimal MaxAmountOnUsCheck { get; set; }

        [DataMember]
        public int OnUSChecktype { get; set; }
    }
}
