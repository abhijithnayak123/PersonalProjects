using System.Collections.Generic;

namespace TCF.Zeo.Peripheral.CheckPrinter.Contract
{
	public class CheckPrintData
	{
        public CheckPrintData()
		{
            CheckData = new List<string>();
		}

		public List<string> CheckData { get; set; }
	}
}
