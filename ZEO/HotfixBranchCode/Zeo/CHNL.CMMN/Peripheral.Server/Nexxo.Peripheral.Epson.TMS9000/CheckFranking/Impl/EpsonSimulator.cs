using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using MGI.Peripheral.CheckFranking.Contract;
using com.epson.bank.driver;

namespace MGI.Peripheral.CheckFranking.Epson.TMS9000.Impl
{
    public partial class TMS9000 : TMS9000Base
    {
        public bool SimulatorPrint(CheckFrankData printObj)
        {
            try
            {
                string filePath = ConfigurationManager.AppSettings["PrinterImageDir"];
                string fileName = DateTime.Now.ToString("ddMMyyyyhhmmss") + ".txt";
                fileName = System.IO.Path.Combine(filePath, fileName);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Simulator print error : " + ex.Message, DateTime.Now.ToString());
                return false;
            }
            return true;

        }
    }
}
