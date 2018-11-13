using System;
using com.epson.bank.driver;

namespace MGI.Peripheral.Checkscanner.EpsonTMS9000.Impl
{
	public partial class TMS9000 : TMS9000Base
	{
		public ErrorCode RetrieveMicrData(int inTransactionNumber)
		{
			errResult = ErrorCode.SUCCESS;
			errResult = objMFDevice.GetMicrText(inTransactionNumber, objMFMicr);
            if (errResult != ErrorCode.SUCCESS && errResult != ErrorCode.ERR_MICR_NODATA)
            {
                return errResult;
            }

            accountNumber = objMFMicr.AccountNumber.TrimEnd('\0');
            bankNumber = objMFMicr.BankNumber.TrimEnd('\0');
            micrStr = objMFMicr.MicrStr.TrimEnd('\0');
            serialNumber = objMFMicr.SerialNumber.TrimEnd('\0');
            amount = string.IsNullOrEmpty(objMFMicr.Amount.TrimEnd('\0'))
                         ? ""
                         : objMFMicr.Amount.TrimEnd('\0');
            EPC = string.IsNullOrEmpty(objMFMicr.EPC.TrimEnd('\0'))
                      ? ""
                      : objMFMicr.EPC.TrimEnd('\0');
            transitNumber = string.IsNullOrEmpty(objMFMicr.TransitNumber.TrimEnd('\0'))
                                ? ""
                                : objMFMicr.TransitNumber.TrimEnd('\0');
            checkType = string.IsNullOrEmpty(objMFMicr.CheckType.ToString())
                            ? ""
                            : objMFMicr.CheckType.ToString();
            countryCode = string.IsNullOrEmpty(objMFMicr.CountryCode.ToString())
                              ? ""
                              : objMFMicr.CountryCode.ToString();
            onUSField = string.IsNullOrEmpty(objMFMicr.OnUSField.TrimEnd('\0'))
                            ? ""
                            : objMFMicr.OnUSField.TrimEnd('\0');
            auxillatyOnUSField = string.IsNullOrEmpty(objMFMicr.AuxillatyOnUSField.TrimEnd('\0'))
                                     ? ""
                                     : objMFMicr.AuxillatyOnUSField.TrimEnd('\0');

            if ( objMFMicr.Detail != 0x40 )
            {
                EpsonException.errorCode = objMFMicr.Detail;
                EpsonException.micrError = 1;
                switch (objMFMicr.Detail)
                {
                    case 0x41:
                        EpsonException.errorMessage = "Failed to read Micr Data. Check was not read.";
                        EpsonException.errorDescription = "Check sheet reading has not ever been executed.";
                        break;
                    case 0x44:
                        EpsonException.errorMessage = "Failed to read Micr Data. Delivery error.";
                        EpsonException.errorDescription = "A delivery error occurred in the processing before reading.";
                        break;
                    case 0x45:
                        EpsonException.errorMessage = "Failed to read Micr Data. Magnetic waveform error.";
                        EpsonException.errorDescription = "A magnetic waveform cannot be detected.";
                        break;
                    case 0x46:
                        EpsonException.errorMessage = "Failed to read Micr Data. Invalid characters.";
                        EpsonException.errorDescription = "Characters that cannot be analyzed were detected in the analysis processing.";
                        break;
                    case 0x47:
                        EpsonException.errorMessage = "Failed to read Micr Data. Double feed error.";
                        EpsonException.errorDescription = "A double-feeding error or an insertion direction error occurred during check sheet reading.";
                        break;
                    case 0x48:
                        EpsonException.errorMessage = "Failed to read Micr Data. Noise abnormality.";
                        EpsonException.errorDescription = "An abnormality was detected in noise measurement.";
                        break;
                    case 0x49:
                        EpsonException.errorMessage = "Failed to read Micr Data. Feed error.";
                        EpsonException.errorDescription = "Check sheet reading was stopped due to a feeding error.";
                        break;
                    case 0x4B:
                        EpsonException.errorMessage = "Failed to read Micr Data. Check too long.";
                        EpsonException.errorDescription = "In reading, an error of paper length too long occurred.";
                        break;
                    default:
                        EpsonException.errorMessage = "Failed to read Micr Data. No Micr detected.";
                        EpsonException.errorDescription = "No Micr data on check.";
                        break;
                }
                
                errResult = ErrorCode.SUCCESS;
            }
			return errResult;
		}
	}
}
