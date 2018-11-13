using System;
using System.IO;

namespace MGI.Peripheral.CheckScanner.Contract
{
    public interface ICheckScanner
    {
        CheckScannerError ScanCheck(String imageFormat, String saveRequired);
        byte[] GetCheckFrontImage(String format);
        byte[] GetCheckBackImage(String format);
        String GetMicr();
        String GetMicrAccountNumber();
        String GetUniqueId();
        Stream GetImage(string id);
        String ConvertStream(String frontSide, String backSide);
        String GetMicrAmount();
        String GetMicrECP();
        String GetMicrTransitNumber();
        String GetMicrCheckType();
        String GetMicrCountryCode();
        String GetMicrOnUSField();
        String GetMicrAuxillatyOnUSField();
    }
}
