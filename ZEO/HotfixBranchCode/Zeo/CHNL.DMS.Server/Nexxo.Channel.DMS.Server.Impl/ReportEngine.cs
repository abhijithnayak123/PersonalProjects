using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;

using Spring.Transaction.Interceptor;

using bizReportService = MGI.Biz.Partner.Contract.IReportService;
using bizCashDrawer = MGI.Biz.Partner.Data.CashDrawerReport;
using AutoMapper;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : IReportService
	{
		bizReportService ReportService { get; set; }

		[Transaction()]
		public CashDrawer CashDrawerReport(long agentSessionId, int agentId, long locationId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			bizCashDrawer cashDrawer = ReportService.GetCashDrawerReport(agentSessionId, agentId, locationId, context);

			return new CashDrawer()
			{
				CashIn = cashDrawer.CashIn,
				CashOut = cashDrawer.CashOut,
				ReportingDate = cashDrawer.ReportDate,
				TellerName = cashDrawer.AgentName,
				ReportTemplate = cashDrawer.ReportTemplate,
			};
		}
	}
}
