using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TCF.Zeo.Peripheral.CheckPrinter.Contract
{
    public interface ICheckPrinter
    {
        CheckPrinterError PrintCheck(CheckPrintData printData);
        byte[] GetCheckFrontImage(string format);
        byte[] GetCheckBackImage(string format);

    }
}
