using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

using Spring.Context;
using Spring.Context.Support;

using MGI.Common.Util;

using MGI.Cxn.Check.Contract;

namespace MGI.Cxn.Check.Chexar.Monitor
{
	public class Program
	{
		static void Main(string[] args)
		{
			IApplicationContext ctx = ContextRegistry.GetContext();
			ICPMonitor chexarMonitor = (ICPMonitor)ctx.GetObject("ChexarMonitor");

			string appName = Assembly.GetExecutingAssembly().GetName().Name;
			// Send Trace Log to a log file
			string LogFileName = string.Format(@"Logs\{0}{1}.log", DateTime.Now.ToString("yyyyMMdd"), appName);
			TextWriterTraceListenerWithTime LogFileTraceLog = TextWriterTraceListenerWithTime.MakeLogFile(LogFileName, false);
			Trace.Listeners.Add(LogFileTraceLog);
			Trace.AutoFlush = true;

			Trace.WriteLine(string.Format("{0} {1} BEGIN", appName, NexxoUtil.AppVersion));

			chexarMonitor.Run();

			Trace.WriteLine(string.Format("{0} {1} END", appName, NexxoUtil.AppVersion));
		}
	}
}
