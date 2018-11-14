namespace TCF.Zeo.Peripheral.Printer.Contract
{
    public interface IPrinter
    {
        PrinterError Print(PrintData printData);
        PrinterError PrintDocument(PrintData printData);
		PrinterError RunDiagnostics();
    }
}
