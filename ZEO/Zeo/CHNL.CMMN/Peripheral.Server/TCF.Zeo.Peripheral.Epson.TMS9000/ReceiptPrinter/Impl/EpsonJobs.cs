using System;
using System.Collections.Generic;
using TCF.Zeo.Peripheral.Printer.Contract;

namespace TCF.Zeo.Peripheral.Printer.EpsonTMS9000.Impl
{
	public static class EpsonJobs
	{
		public static List<PrintData> _Jobs = new List<PrintData>();
		public static List<String> _JobStatus = new List<String>();
	}
}
