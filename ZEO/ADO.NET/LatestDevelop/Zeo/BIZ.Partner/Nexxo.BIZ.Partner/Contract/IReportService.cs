using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Biz.Partner.Data;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Contract
{
    public interface IReportService
    {
        /// <summary>
        /// This method to fetch the cash drawer report.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for agent session</param>
        /// <param name="agentId">This is unique identifier for agent</param>
        /// <param name="locationId">This is unique identifier for location</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplemental information</param>
        /// <returns>Cash Drawer Report Details</returns>
		CashDrawerReport GetCashDrawerReport(long agentSessionId, int agentId, long locationId, MGIContext mgiContext);
    }
}
