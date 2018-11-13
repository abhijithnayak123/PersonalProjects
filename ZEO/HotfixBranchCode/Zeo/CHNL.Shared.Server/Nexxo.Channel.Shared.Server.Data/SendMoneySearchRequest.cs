using System.Runtime.Serialization;
using System.Text;
using System.Collections.Generic;

namespace MGI.Channel.Shared.Server.Data
{
	[DataContract]
	public class SendMoneySearchRequest
	{		
		[DataMember]
		public string ConfirmationNumber { get; set; }
		[DataMember]
		public SearchRequestType SearchRequestType { get; set; }
	}

	public enum SearchRequestType
	{
		Modify,
		Refund,
		RefundWithStage
	}
}