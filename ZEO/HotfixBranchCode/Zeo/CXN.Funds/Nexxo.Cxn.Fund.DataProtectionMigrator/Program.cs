using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;

using Spring.Context;
using Spring.Context.Support;

using MGI.Common.Util;

namespace MGI.Cxn.Fund.DataProtectionMigrator
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IApplicationContext ctx = ContextRegistry.GetContext();
			IDataMigrator dataMigrator = (IDataMigrator)ctx.GetObject("DataMigrator");

			int oldSlot;
			int newSlot;

			if (args.Length > 3 || args.Length < 2 || (args.Length == 3 && args[2] != "-a") || !int.TryParse(args[0], out oldSlot) || !int.TryParse(args[1], out newSlot))
			{
				ShowUsage();
				return;
			}

			bool testRun = args.Length == 2;

			string appName = Assembly.GetExecutingAssembly().GetName().Name;
			// Send Trace Log to a log file
			string LogFileName = string.Format(@"{0}\{1}.{2}.log", ConfigurationManager.AppSettings["LogPath"], DateTime.Now.ToString("yyyyMMdd"), appName);
			TextWriterTraceListenerWithTime LogFileTraceLog = TextWriterTraceListenerWithTime.MakeLogFile(LogFileName, false);
			Trace.Listeners.Add(LogFileTraceLog);
			Trace.AutoFlush = true;

			Trace.WriteLine(string.Format("{0} {1} BEGIN", appName, NexxoUtil.AppVersion));
			Trace.WriteLine(string.Format("old slot: {0}, new slot: {1}", oldSlot, newSlot));

			if (testRun)
			{
				Console.WriteLine("Test run - will not commit changes");
				Trace.WriteLine("Test run - will not commit changes");
			}

			dataMigrator.Run(testRun, oldSlot, newSlot);

			Trace.WriteLine(string.Format("{0} {1} END", appName, NexxoUtil.AppVersion));
		}

		public static void ShowUsage()
		{
			Console.WriteLine("Usage: DataProtectionMigrator <old slot> <new slot> [-a]");
			Console.WriteLine("");
			Console.WriteLine("       -a: apply changes to DB");
			Console.WriteLine("");
			Console.WriteLine("       if <old slot> and <new slot> are different, then");
			Console.WriteLine("       DataProtectionMigrator will decrypt values with ");
			Console.WriteLine("       NEKMA keys from <old slot>, re-encrypt using ");
			Console.WriteLine("       NEKMA keys from <new slot> and then update the DB");
		}
	}
}
