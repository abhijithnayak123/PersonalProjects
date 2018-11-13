using System;
using MGI.Common.DataAccess.Data;
using System.Collections.Generic;
using MGI.Cxn.Check.Data;

namespace MGI.Cxn.Check.Certegy.Data
{
	public class Transaction : NexxoModel
	{
		public virtual decimal CheckAmount { get; set; }		
		public virtual decimal CertegyFee { get; set; }
		public virtual DateTime CheckDate { get; set; }
		public virtual string CheckNumber { get; set; }
		public virtual string RoutingNumber { get; set; }
		public virtual string AccountNumber { get; set; }
		public virtual string Micr { get; set; }
		public virtual string MicrEntryType { get; set; }

		public virtual string CertegyUID { get; set; }
		public virtual string ApprovalNumber { get; set; }
		public virtual string SettlementID { get; set; }
		public virtual int ResponseCode { get; set; }		
		public virtual CheckStatus CheckStatus { get; set; }
						
		public virtual string SiteID { get; set; }

		public virtual int AlloySubmitType { get; set; }
		public virtual int AlloyReturnType { get; set; }
		public virtual string CertegySubmitType { get; set; }
		public virtual string CertegyReturnType { get; set; }

		public virtual string TranType { get; set; }
		public virtual string FundsAvail { get; set; }
		public virtual string Version { get; set; }
		public virtual string IdType { get; set; }
		public virtual string ExpansionType { get; set; }
		

		public virtual string DeviceType { get; set; }
		public virtual string DeviceId { get; set; }
		public virtual string DeviceIP { get; set; }

		public virtual long ChannelPartnerID { get; set; }		
		public virtual bool IsCheckFranked { get; set; }
		
		public virtual Account CertegyAccount { get; set; }		
	}
}
