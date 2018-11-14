using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Data
{
	public class TransactionHistoryRequest
	{
		public long AliasId { get; set; }
		public Helper.TransactionStatus TransactionStatus { get; set; }
		public int DateRange { get; set; }
	}
}
