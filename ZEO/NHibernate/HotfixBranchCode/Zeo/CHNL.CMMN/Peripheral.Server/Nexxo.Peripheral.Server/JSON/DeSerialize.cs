using System;
using System.Web.Script.Serialization;
using MGI.Peripheral.Server.Data;

namespace MGI.Peripheral.Server.JSON.Impl
{
	public class DeSerialize
	{
		public ScanCheckRequest GetScanCheckParams(String scanparams)
		{
			JavaScriptSerializer convertToParams = new JavaScriptSerializer();
			return convertToParams.Deserialize<ScanCheckRequest>(scanparams);
		}

        public ConvertStreamRequest GetConvertStreamParams(String scanparams)
        {
            JavaScriptSerializer convertToParams = new JavaScriptSerializer();
            return convertToParams.Deserialize<ConvertStreamRequest>(scanparams);
        }

        public CashCheckPrintRequest GetCashCheckPrintParams(String printparams)
		{
			JavaScriptSerializer convertToParams = new JavaScriptSerializer();
			return convertToParams.Deserialize<CashCheckPrintRequest>(printparams);
		}

		public CashDrawerPrintRequest GetCashDrawerPrintParams(String printparams)
		{
			JavaScriptSerializer convertToParams = new JavaScriptSerializer();
			return convertToParams.Deserialize<CashDrawerPrintRequest>(printparams);
		}

        public CheckPrintRequest GetCheckPrintParams(String printparams)
        {
            JavaScriptSerializer convertToParams = new JavaScriptSerializer();
            return convertToParams.Deserialize<CheckPrintRequest>(printparams);
        }

        public CheckFrankRequest GetFrankCheckParams(String frankparams)
        {
            JavaScriptSerializer convertToParams = new JavaScriptSerializer();
            return convertToParams.Deserialize<CheckFrankRequest>(frankparams);
        }
	}
}
