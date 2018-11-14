using System;

using Spring.Context;
using Spring.Context.Support;

using MGI.Biz.CPEngine.Contract;
using System.Diagnostics;

namespace MGI.Biz.CPEngine.Monitor
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Start Time :" + DateTime.Now);

			Stopwatch watch = new Stopwatch();
			watch.Start();

			IApplicationContext ctx = ContextRegistry.GetContext();
			ICPMonitor cpMonitor = (ICPMonitor)ctx.GetObject("CPMonitor");
			cpMonitor.Run(args);

			watch.Stop();

			Console.WriteLine("End Time :" + DateTime.Now);

			Console.WriteLine("Elapsed MilliSeconds :" + watch.ElapsedMilliseconds);
		}
	}
}
