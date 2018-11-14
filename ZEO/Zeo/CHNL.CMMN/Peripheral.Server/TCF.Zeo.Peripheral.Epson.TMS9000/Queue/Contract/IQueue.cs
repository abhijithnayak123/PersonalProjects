using System;
using TCF.Zeo.Peripheral.Printer.Contract;
using TCF.Zeo.Peripheral.CheckPrinter.Contract;


namespace TCF.Zeo.Peripheral.Queue.Contract
{
    public interface IQueue
    {
        void SetPrintJob(PrintData printData);
        bool SetScanJob(String imageFormat);
        void SetCheckPrintJob(CheckPrintData printData);
        PrintData GetPrintJob();
        String GetScanJob();
        CheckPrintData GetCheckPrintJob();
        void SetJobComplete();
    }
}
