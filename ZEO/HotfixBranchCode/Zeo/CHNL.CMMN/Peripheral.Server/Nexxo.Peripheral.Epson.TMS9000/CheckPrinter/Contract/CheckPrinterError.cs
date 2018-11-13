using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Peripheral.CheckPrinter.Contract
{
    public class CheckPrinterError
    {
        public bool errorStatus { get; set; }
        public int errorCode { get; set; }
        public String errorMessage { get; set; }
        public String errorDescription { get; set; }
        public String stackTrace { get; set; }
    }
}
