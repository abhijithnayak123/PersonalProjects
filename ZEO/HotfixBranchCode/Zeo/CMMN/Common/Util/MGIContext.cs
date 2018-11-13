using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Common.Util
{
	public class MGIContext
	{
		public MGIContext()
		{
			//setting the value to true by default,later we update in shopping cart
			ShouldIncludeShoppingCartItems = true;
		}

		public string AgentFirstName { get; set; }

		public int AgentId { get; set; }

		public string AgentLastName { get; set; }

		public string AgentName { get; set; }

		public String ApplicationName { get; set; }

		public string BankId { get; set; }

		public int BranchId { get; set; }

		public int IngoBranchId { get; set; }

		public int CardPresentedType { get; set; }

		public string CertegySiteId { get; set; }

		public long ChannelPartnerId { get; set; }

		public string ChannelPartnerName { get; set; }

		public Guid ChannelPartnerRowGuid { get; set; }

		public string CheckPassword { get; set; }

		public string CheckUserName { get; set; }

		public long CustomerSessionId { get; set; }

		public long CXECustomerId { get; set; }

		public DateTime BusinessDate { get; set; }

		public bool IsReferral { get; set; }

		public string LocationName { get; set; }

		public Guid LocationRowGuid { get; set; }

		public long ProcessorId { get; set; }

		public int ProviderId { get; set; }

		public string TerminalName { get; set; }

		public string TimeZone { get; set; }

		public string TSysPartnerId { get; set; }

		public string WUCounterId { get; set; }

		public long TrxId { get; set; }

		public long CxnAccountId { get; set; }

		public string RequestType { get; set; }

		public long CxnTransactionId { get; set; }

		public string AccountNumberRetryCount { get; set; }

		public long AgentSessionId { get; set; }

		public string CityCode { get; set; }

		public string BillerCode { get; set; }

		public bool IsAnonymous { get; set; }

		public long AlloyId { get; set; }

		public string PromotionCode { get; set; }

		public bool IsSystemApplied { get; set; }

		public string URL { get; set; }

		public string CompanyToken { get; set; }

		public int EmployeeId { get; set; }

		public bool ShouldIncludeShoppingCartItems { get; set; }

		public bool IsParked { get; set; }

		public int CheckType { get; set; }

		public decimal Amount { get; set; }

		public decimal Fee { get; set; }

		public string ProductType { get; set; }

		public string RMMTCN { get; set; }

		public string OriginatingCountryCode { get; set; }

		public string OriginatingCurrencyCode { get; set; }

		public string DestinationCountryCode { get; set; }

		public string DestinationCurrencyCode { get; set; }

		public string StateName { get; set; }

		public string StateCode { get; set; }

		public string CityName { get; set; }

		public string SVCCode { get; set; }

		public bool IsExistingAccount { get; set; }

		public string LocationId { get; set; }

		/// <summary>
		/// Send Money Transaction Type
		/// </summary>
		public string SMTrxType { get; set; }

		/// <summary>
		/// Receive Money Transaction Type
		/// </summary>
		public string RMTrxType { get; set; }

		public string ReferenceNumber { get; set; } 

		public string RegulatorInfoStateCode { get; set; }
		
		public int TrxSubType { get; set; }
		
		public string SSN { get; set; }
		
		public bool IsAvailable { get; set; }
		
		public bool EditMode { get; set; }

		public string Language { get; set; }

		public string LocationStateCode { get; set; }
		
		public int CardExpiryPeriod { get; set; }

		public long AddOnCustomerId { get; set; }

		public bool IsCompanionSearch { get; set; }

		public string GPRPromoCode { get; set; }

		public Dictionary<string, object> Context { get; set; }

		public int CardClass { get; set; }

		public long VisaLocationNodeId { get; set; }
	}
}
