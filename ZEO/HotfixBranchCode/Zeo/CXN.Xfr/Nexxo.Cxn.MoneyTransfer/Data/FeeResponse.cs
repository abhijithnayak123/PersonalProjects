using System.Collections.Generic;

namespace MGI.Cxn.MoneyTransfer.Data
{
	public class FeeResponse
	{
		public List<FeeInformation> FeeInformations { get; set; }
		public long TransactionId { get; set; }
		public Dictionary<string, object> MetaData { get; set; }
		//public string TestQuestionOption { get; set; }
		//public bool IsFixedOnSend { get; set; }

	}
}
