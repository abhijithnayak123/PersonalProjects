using com.epson.bank.driver;
using System.Diagnostics;
using System;

namespace MGI.Peripheral.CheckPrinter.Epson.TMS9000.Impl
{
    public partial class TMS9000 : TMS9000Base
    {
        public ErrorCode RetrieveImage(int inTransactionNumber)
        {
            Trace.WriteLine("Epson.Epson:Event.RetrieveImage Begin", DateTime.Now.ToString());
            errResult = ErrorCode.SUCCESS;
            errResult = objMFDevice.SCNSelectScanFace(ScanSide.MF_SCAN_FACE_FRONT);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            errResult = objMFDevice.SCNSelectScanImage(ImageType.MF_SCAN_IMAGE_VISIBLE);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            errResult = objMFDevice.SCNSetImageQuality(ColorDepth.EPS_BI_SCN_1BIT, 0, Color.EPS_BI_SCN_MONOCHROME, ExOption.EPS_BI_SCN_MANUAL);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            //Trace.WriteLine("Setting Front Face Image format to " + objConfig.imageFormat.ToString(), DateTime.Now.ToString());
            errResult = objMFDevice.SCNSetImageFormat(Format.EPS_BI_SCN_TIFF);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            errResult = objMFDevice.GetScanImage(inTransactionNumber, objMFScanFront);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            long dataLength = objMFScanFront.Data.Length;
            checkFrontImage = new byte[dataLength];
            objMFScanFront.Data.Read(checkFrontImage, 0, checkFrontImage.Length);


            errResult = objMFDevice.SCNSelectScanFace(ScanSide.MF_SCAN_FACE_BACK);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            errResult = objMFDevice.SCNSelectScanImage(ImageType.MF_SCAN_IMAGE_VISIBLE);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            errResult = objMFDevice.SCNSetImageQuality(ColorDepth.EPS_BI_SCN_1BIT, 0, Color.EPS_BI_SCN_MONOCHROME, ExOption.EPS_BI_SCN_MANUAL);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            //Trace.WriteLine("Setting Back Face Image format to " + objConfig.imageFormat.ToString(), DateTime.Now.ToString());
            errResult = objMFDevice.SCNSetImageFormat(Format.EPS_BI_SCN_TIFF);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            errResult = objMFDevice.GetScanImage(inTransactionNumber, objMFScanBack);
            if (errResult != ErrorCode.SUCCESS)
                return errResult;

            dataLength = objMFScanBack.Data.Length;
            checkBackImage = new byte[dataLength];
            objMFScanBack.Data.Read(checkBackImage, 0, checkBackImage.Length);

            Trace.WriteLine("Epson.Epson:Event.RetrieveImage Completed", DateTime.Now.ToString());
            return errResult;
        }
    }
}
