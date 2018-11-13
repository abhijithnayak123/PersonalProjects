using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IReceiptsService
	{

		public List<ReceiptData> GetCheckReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			return DesktopEngine.GetCheckReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
		}

		public List<ReceiptData> GetFundsReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			return DesktopEngine.GetFundsReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
		}

		public List<ReceiptData> GetMoneyTransferReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			return DesktopEngine.GetMoneyTransferReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
		}

		public List<ReceiptData> GetDoddFrankReceipt(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetDoddFrankReceipt(agentSessionId, customerSessionId, transactionId, mgiContext);
		}

		public List<ReceiptData> GetCheckDeclinedReceiptData(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetCheckDeclinedReceiptData(agentSessionId, customerSessionId, transactionId, mgiContext);
		}

		public List<ReceiptData> GetMoneyOrderReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			return DesktopEngine.GetMoneyOrderReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
		}

		public List<ReceiptData> GetBillPayReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			return DesktopEngine.GetBillPayReceipt(agentSessionId, customerSessionId, transactionId, isReprint, mgiContext);
		}

		public List<ReceiptData> GetSummaryReceipt(long customerSessionId, long cartId, MGIContext mgiContext)
		{
			return DesktopEngine.GetSummaryReceipt(customerSessionId, cartId, mgiContext);
		}

		public List<ReceiptData> GetSummaryReceipt(long agentSessionId, long customerSessionId, long transactionId, string transactiontype, MGIContext mgiContext)
		{
			return DesktopEngine.GetSummaryReceipt(agentSessionId, customerSessionId, transactionId, transactiontype, mgiContext);
		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Added new method to Get CouponCode Receipt
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <returns></returns>
		public List<ReceiptData> GetCouponCodeReceipt(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetCouponCodeReceipt(customerSessionId, mgiContext);
		}
	}
}
