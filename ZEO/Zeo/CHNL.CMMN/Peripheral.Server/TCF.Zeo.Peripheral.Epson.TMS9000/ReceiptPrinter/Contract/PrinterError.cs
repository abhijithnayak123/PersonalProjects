using System;

namespace TCF.Zeo.Peripheral.Printer.Contract
{
	public class PrinterError
	{
		public int errorCode { get; set; }
		public bool errorStatus { get; set; }
		public String errorMessage { get; set; }
		public String errorDescription { get; set; }
		public String stackTrace { get; set; }

		//Ink Status
		public string inkStatus { get; set; }

		//Diagnostics Information
		public string diag_deviceName { get; set; }
		public string diag_status { get; set; }
		public string diag_serialNumber { get; set; }
		public string diag_firmwareVersion { get; set; }
		public string diag_deviceStatus { get; set; }

	}
}
