using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : ICheckCashingService
	{
		public Check SubmitCheck(long customerSessionId, CheckSubmission check, MGIContext mgiContext)
		{
			return DesktopEngine.SubmitCheck(customerSessionId, check, mgiContext);
		}

        public void CancelCheck(long customerSessionId, string check, MGIContext mgiContext)
        {
            DesktopEngine.CancelCheck(customerSessionId, check, mgiContext);
        }

		public Check GetCheckStatus(long customerSessionId, string checkId, MGIContext mgiContext)
		{
			return DesktopEngine.GetCheckStatus(customerSessionId, checkId, mgiContext);
		}

		public bool CanResubmit(long customerSessionId, string checkId, MGIContext mgiContext)
		{
			return DesktopEngine.CanResubmit(customerSessionId, checkId, mgiContext);
		}

		public List<string> GetCheckTypes(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetCheckTypes(customerSessionId, mgiContext);
		}

		public TransactionFee GetCheckFee(long customerSessionId, CheckSubmission checkSubmit, MGIContext mgiContext)
		{
			return DesktopEngine.GetCheckFee(customerSessionId, checkSubmit, mgiContext);
		}

		public CheckTransactionDetails GetCheckTranasactionDetails(long agentSessionId, long customerSessionId, string checkId, MGIContext mgiContext)
		{
			return DesktopEngine.GetCheckTranasactionDetails(agentSessionId, customerSessionId, checkId, mgiContext);
		}

		public string GetCheckFrankingData(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetCheckFrankingData(customerSessionId, transactionId, mgiContext);
		}

		public CheckProcessorInfo GetCheckProcessorInfo(long agentSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetCheckProcessorInfo(agentSessionId, mgiContext);
		}

		public void UpdateTransactionFranked(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			DesktopEngine.UpdateTransactionFranked(customerSessionId, transactionId, mgiContext);
		}

		public CheckLogin GetCheckSession(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetCheckSession(customerSessionId, mgiContext);
		}
	}
}