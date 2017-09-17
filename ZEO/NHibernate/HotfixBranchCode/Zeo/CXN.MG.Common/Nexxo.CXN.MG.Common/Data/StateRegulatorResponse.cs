using System;
using System.Collections.Generic;

namespace MGI.CXN.MG.Common.Data
{
	public class StateRegulatorResponse
	{
		public string Version { get; set; }
		public DateTime TimeStamp { get; set; }
		public List<StateRegulator> StateRegulators { get; set; }
	}
}
