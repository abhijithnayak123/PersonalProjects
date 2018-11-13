using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

using Spring.Transaction.Interceptor;

using MGI.Common.Util;

using MGI.Cxn.Check.Data;
using MGI.Cxn.Check.Contract;
using MGI.Cxn.Check.Chexar.Data;
using MGI.Cxn.Check.Chexar.Contract;

namespace MGI.Cxn.Check.Chexar.Monitor
{
	public class ChexarMonitor : ICPMonitor
	{
		#region Dependencies

		private ICheckProcessor _cxnCheckSvc;
		public ICheckProcessor CxnCheckSvc
		{
			set { _cxnCheckSvc = value; }
		}
		#endregion

		[Transaction()]
		public void Run()
		{
			// 1. get list of pending checks
			List<CheckTrx> openChecks = _cxnCheckSvc.PendingChecks();

			Trace.WriteLine(string.Format("{0} pending checks found", openChecks.Count));

			// 2. for each check, poll status and take action
			foreach (CheckTrx pendingCheck in openChecks)
			{
				Trace.WriteLine(string.Format("Processing check {0}", pendingCheck.Id));
				CheckStatus status = _cxnCheckSvc.Status(pendingCheck.Id, TimeZone.CurrentTimeZone.StandardName);
				Trace.WriteLine(string.Format("   Status: {0}", status));
			}
		}
	}
}
