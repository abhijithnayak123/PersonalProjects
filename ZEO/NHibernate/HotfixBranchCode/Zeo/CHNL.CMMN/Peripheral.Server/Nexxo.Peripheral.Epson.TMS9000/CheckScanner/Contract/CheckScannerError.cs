using System;

namespace MGI.Peripheral.CheckScanner.Contract
{
	public class CheckScannerError
	{
		public bool errorStatus { get; set; }
        public int errorCode { get; set; }
        public int micrError { get; set; }
        public String errorMessage { get; set; }
		public String errorDescription { get; set; }
		public String stackTrace { get; set; }
	}
}
