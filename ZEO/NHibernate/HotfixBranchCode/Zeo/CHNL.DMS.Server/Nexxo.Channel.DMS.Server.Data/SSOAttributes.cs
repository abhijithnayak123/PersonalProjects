using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class SSOAttributes
	{
		[DataMember]
		public string UserId { get; set; }
		[DataMember]
		public string GivenName { get; set; }
		[DataMember]
		public string sn { get; set; }
		[DataMember]
		public string Role { get; set; }
		[DataMember]
		public string BankNumber { get; set; }
		[DataMember]
		public string LawsonId { get; set; }
		[DataMember]
		public string TellerNum { get; set; }
		[DataMember]
		public string CashDrawer { get; set; }
		[DataMember]
		public string BranchNumber { get; set; }
		[DataMember]
		public string MachineNumber { get; set; }
		[DataMember]
		public string AmPmInd { get; set; }
		[DataMember]
		public string DisplayName { get; set; }
		[DataMember]
		public string LU { get; set; }
		[DataMember]
		public string SoftwareVersion { get; set; }
		[DataMember]
		public string BuisnessDate { get; set; }
	}
}
