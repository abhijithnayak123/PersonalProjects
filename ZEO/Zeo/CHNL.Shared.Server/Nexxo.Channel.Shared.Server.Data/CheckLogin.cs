using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class CheckLogin
	{
		[DataMember]
		public string URL { get; set; }
		[DataMember]
		public string CompanyToken { get; set; }
		[DataMember]
		public int EmployeeId { get; set; }
		[DataMember]
		public int BranchId { get; set; }
	}
}
