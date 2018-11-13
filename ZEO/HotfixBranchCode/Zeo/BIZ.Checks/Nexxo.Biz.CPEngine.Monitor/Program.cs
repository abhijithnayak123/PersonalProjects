using System;

using Spring.Context;
using Spring.Context.Support;

using MGI.Biz.CPEngine.Contract;

namespace MGI.Biz.CPEngine.Monitor
{
	class Program
	{
		static void Main(string[] args)
		{
			IApplicationContext ctx = ContextRegistry.GetContext();
			ICPMonitor cpMonitor = (ICPMonitor)ctx.GetObject("CPMonitor");
			cpMonitor.Run(args);
		}
	}
}
