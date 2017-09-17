using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Peripheral.Service.CustomAction
{
	public static class PreRequisiteStatus
	{
		public static bool IsTCPOutBound { get; set; }
		public static bool IsDiskSpaceAvailable { get; set; }
		public static bool IsMultiFunctionDeviceAvailable { get; set; }
		public static bool IsWinInstallerAvailable { get; set; }
		public static bool IsDMSServiceAvailable { get; set; }
		public static bool IsSystemMemoryAvailable { get; set; }
		public static bool IsWindowsEdition { get; set; }
		public static bool IsStunnel { get; set; }
		public static bool IsHttpsOutBound { get; set; }
        public static bool IsWordWriter { get; set; }
        public static bool IsEPSONRollPrinter { get; set; }
	}
}
