using System.Collections.Generic;

#region AO
using ZeoClient = TCF.Channel.Zeo.Web.ServiceClient.ZeoService;
#endregion

namespace TCF.Channel.Zeo.Web.Models
{
    public class TransactionHistoryViewModel
	{
		public List<ZeoClient.TransactionHistory> Transactions { get; set; }

		public string AgentFullName { get; set; }

		public long AgentId { get; set; }

		public string TransactionSummaryDate { get; set; }
	}
}