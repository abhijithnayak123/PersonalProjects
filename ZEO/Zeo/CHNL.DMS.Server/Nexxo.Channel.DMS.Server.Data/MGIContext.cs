using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	[KnownType(typeof(Dictionary<string,object>))]
	public class MGIContext
	{
		public MGIContext()
		{
			ApplicationName = "DMS-Server";
			ProcessorId = 14;
			// This value will be updated during billpay or send money
			WUCounterId = string.Empty;
			//setting the value to true by default,later we update in shopping cart
			ShouldIncludeShoppingCartItems = true;
		}
		[DataMember]
		public string AgentFirstName { get; set; }
		[DataMember]
		public int AgentId { get; set; }
		[DataMember]
		public string AgentLastName { get; set; }
		[DataMember]
		public string AgentName { get; set; }
		[DataMember]
		public String ApplicationName { get; set; }
		[DataMember]
		public string BankId { get; set; }
		[DataMember]
		public string BranchId { get; set; }
        [DataMember]
        public int IngoBranchId { get; set; }
		[DataMember]
		public DateTime BusinessDate { get; set; }
		[DataMember]
		public int CardPresentedType { get; set; }
		[DataMember]
		public string CertegySiteId { get; set; }
		[DataMember]
		public long ChannelPartnerId { get; set; }
		[DataMember]
		public string ChannelPartnerName { get; set; }
		[DataMember]
		public Guid ChannelPartnerRowGuid { get; set; }
		[DataMember]
		public string CheckPassword { get; set; }
		[DataMember]
		public string CheckUserName { get; set; }
		[DataMember]
		public long CustomerSessionId { get; set; }
		[DataMember]
		public long CXECustomerId { get; set; }
		[DataMember]
		public long CxnAccountId;
		[DataMember]
		public bool IsReferral { get; set; }
		[DataMember]
		public string LocationName { get; set; }
		[DataMember]
		public Guid LocationRowGuid { get; set; }
		[DataMember]
		public long ProcessorId { get; set; }
		[DataMember]
		public int ProviderId { get; set; }
		[DataMember]
		public string TerminalName { get; set; }
		[DataMember]
		public string TimeZone { get; set; }
		[DataMember]
		public string TSysPartnerId { get; set; }
		[DataMember]
		public string WUCounterId { get; set; }
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
		public int CheckType { get; set; }
		[DataMember]
		public decimal Amount { get; set; }
		[DataMember]
		public decimal Fee { get; set; }
		[DataMember]
		public long TrxId { get; set; }
		[DataMember]
		public string RequestType { get; set; }
		[DataMember]
		public long CxnTransactionId { get; set; }
		[DataMember]
		public string AccountNumberRetryCount { get; set; }
		[DataMember]
		public long AgentSessionId { get; set; }
		[DataMember]
		public string CityCode { get; set; }
		[DataMember]
		public string BillerCode { get; set; }
		[DataMember]
		public bool IsAnonymous { get; set; }
		[DataMember]
		public long AlloyId { get; set; }
		[DataMember]
		public bool IsExistingAccount { get; set; }
		[DataMember]
		public string LocationId { get; set; }
		[DataMember]
		public string ProductType { get; set; }
		[DataMember]
		public string OriginatingCountryCode { get; set; }
		[DataMember]
		public string OriginatingCurrencyCode { get; set; }
		[DataMember]
		public string DestinationCountryCode { get; set; }
		[DataMember]
		public string DestinationCurrencyCode { get; set; }
		[DataMember]
		public string StateName { get; set; }
		[DataMember]
		public string StateCode { get; set; }
		[DataMember]
		public string CityName { get; set; }
		[DataMember]
		public string SVCCode { get; set; }
        [DataMember]
        public string RegulatorInfoStateCode { get; set; }
        [DataMember]
        public int TrxSubType { get; set; }
		[DataMember]
		public string TellerNumber { get; set; }
		[DataMember]
		public bool EditMode { get; set; }
		[DataMember]
		public bool IsAvailable { get; set; }
		[DataMember]
		public string Language { get; set; }
		[DataMember]
		public string LocationStateCode { get; set; }
		[DataMember]
		public long CashInTrxId { get; set; } //AL-2729 for updating the cash-in transaction 
		[DataMember]
		public long AddOnCustomerId { get; set; }
		[DataMember]
		public bool IsCompanionSearch { get; set; }
		[DataMember]
		public Dictionary<string, object> Context { get; set; }
		[DataMember]
		public int CardClass { get; set; }
		[DataMember]
		public long VisaLocationNodeId { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("MGIContext: ");
			sb.AppendLine(string.Format("	AgentFirstName: {0}", AgentFirstName));
			sb.AppendLine(string.Format("	AgentId: {0}", AgentId));
			sb.AppendLine(string.Format("	AgentLastName: {0}", AgentLastName));
			sb.AppendLine(string.Format("	AgentName: {0}", AgentName));
			sb.AppendLine(string.Format("	ApplicationName: {0}", ApplicationName));
			sb.AppendLine(string.Format("	BankId: {0}", BankId));
			sb.AppendLine(string.Format("	BranchId: {0}", BranchId));
            sb.AppendLine(string.Format("	Ingo BranchId: {0}", IngoBranchId));
			sb.AppendLine(string.Format("	CertegySiteId: {0}", CertegySiteId));
			sb.AppendLine(string.Format("	ChannelPartnerId: {0}", ChannelPartnerId));
			sb.AppendLine(string.Format("	ChannelPartnerName: {0}", ChannelPartnerName));
			sb.AppendLine(string.Format("	ChannelPartnerRowGuid: {0}", ChannelPartnerRowGuid));
			sb.AppendLine(string.Format("	CheckPassword: {0}", CheckPassword));
			sb.AppendLine(string.Format("	CheckUserName: {0}", CheckUserName));
			sb.AppendLine(string.Format("	CustomerSessionId: {0}", CustomerSessionId));
			sb.AppendLine(string.Format("	LocationName: {0}", LocationName));
			sb.AppendLine(string.Format("	LocationRowGuid: {0}", LocationRowGuid));
			sb.AppendLine(string.Format("	ProcessorId: {0}", ProcessorId));
			sb.AppendLine(string.Format("	TerminalName: {0}", TerminalName));
			sb.AppendLine(string.Format("	TimeZone: {0}", TimeZone));
			sb.AppendLine(string.Format("	TSysPartnerId: {0}", TSysPartnerId));
			sb.AppendLine(string.Format("	WUCounterId: {0}", WUCounterId));
			sb.AppendLine(string.Format("	TrxId: {0}", TrxId));
			sb.AppendLine(string.Format("	IsAnonymous: {0}", IsAnonymous));
			sb.AppendLine(string.Format("	BillerCode: {0}", BillerCode));
			sb.AppendLine(string.Format("	CityCode: {0}", CityCode));
			sb.AppendLine(string.Format("	AgentSessionId: {0}", AgentSessionId));
			sb.AppendLine(string.Format("	AccountNumberRetryCount: {0}", AccountNumberRetryCount));
			sb.AppendLine(string.Format("	CxnAccountId: {0}", CxnAccountId));
			sb.AppendLine(string.Format("	CxnTransactionId: {0}", CxnTransactionId));
			sb.AppendLine(string.Format("	RequestType: {0}", RequestType));
			sb.AppendLine(string.Format("	AlloyId: {0}", AlloyId));
			sb.AppendLine(string.Format("	ProductType: {0}", ProductType));
			sb.AppendLine(string.Format("	OriginatingCountryCode: {0}", OriginatingCountryCode));
			sb.AppendLine(string.Format("	OriginatingCurrencyCode: {0}", OriginatingCurrencyCode));
			sb.AppendLine(string.Format("	DestinationCountryCode: {0}", DestinationCountryCode));
			sb.AppendLine(string.Format("	DestinationCurrencyCode: {0}", DestinationCurrencyCode));
			sb.AppendLine(string.Format("	StateName: {0}", StateName));
			sb.AppendLine(string.Format("	StateCode: {0}", StateCode));
			sb.AppendLine(string.Format("	CityName: {0}", CityName));
			sb.AppendLine(string.Format("	SVCCode: {0}", SVCCode));
			sb.AppendLine(string.Format("	IsExistingAccount: {0}", IsExistingAccount));
			sb.AppendLine(string.Format("	LocationId: {0}", LocationId));
            sb.AppendLine(string.Format("	RegulatorInfoStateCode: {0}", RegulatorInfoStateCode));
			sb.AppendLine(string.Format("	TrxSubType: {0}", TrxSubType));
			sb.AppendLine(string.Format("	TellerNumber: {0}", TellerNumber));
			sb.AppendLine(string.Format("	EditMode: {0}", EditMode));
			sb.AppendLine(string.Format("	IsAvailable: {0}", IsAvailable));
			sb.AppendLine(string.Format("	Language: {0}", Language));
			sb.AppendLine(string.Format("	LocationStateCode: {0}", LocationStateCode));
			sb.AppendLine(string.Format("   CashInTrxId: {0}", CashInTrxId));
			sb.AppendLine(string.Format("   AddOnCustomerId: {0}", AddOnCustomerId));
			sb.AppendLine(string.Format("   IsCompanionSearch: {0}", IsCompanionSearch));
			sb.AppendLine(string.Format("	CardClass: {0}", CardClass));
			sb.AppendLine(string.Format("	VisaLocationNodeId: {0}", VisaLocationNodeId));
			return sb.ToString();
		}

	}
}
