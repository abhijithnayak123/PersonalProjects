using System;
using MGI.Peripheral.Printer.Contract;
using MGI.Peripheral.CheckPrinter.Contract;


namespace MGI.Peripheral.Queue.Contract
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
