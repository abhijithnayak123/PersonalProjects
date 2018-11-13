using System.Collections.Generic;

namespace MGI.Biz.MoneyTransfer.Data
{
	public class FeeResponse
	{
		public List<FeeInformation> FeeInformations { get; set; }
		public long TransactionId { get; set; }
		public Dictionary<string, object> MetaData { get; set; }

		//public string TestQuestionOption { get; set; }
		//public bool IsFixedOnSend { get; set; }
		//public string ReferenceNo { get; set; }
	}
}
