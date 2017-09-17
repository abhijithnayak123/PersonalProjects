using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Core.Partner.Data;

namespace MGI.Core.Partner.Contract
{
	public interface IReportService
	{

		/// <summary>
		/// This method is to get collection of cash drawer report
		/// </summary>		
		/// <param name="agentId">This is agent id</param>
		/// <param name="locationId">This is location id</param>
		/// <returns>Collection of cash drawer report</returns>
		List<CashDrawerReport> Get(int agentId, long locationId);

	}
}