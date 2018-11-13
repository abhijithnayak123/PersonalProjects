using MGI.Common.Util;
using MGI.Cxn.BillPay.MG.AgentConnectService;
using MGI.Cxn.BillPay.MG.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace MGI.Cxn.BillPay.MG.Impl
{
	public class SimulatorIO : IIO
	{
		public TLoggerCommon MongoDBLogger { private get; set; }
		public BillerSearchResponse BillerSearch(BillerSearchRequest billerSearchRequest)
		{
			return BillerSearch();
		}

		public BpValidationResponse BpValidation(BpValidationRequest validationRequest, MGIContext mgiContext)
		{
			return BillpayValidation(validationRequest);
		}

		private BillerSearchResponse BillerSearch()
		{
			BillerSearchResponse billerSearchResp = new BillerSearchResponse();	

			var billerInfor = new BillerInfo[1];
			billerInfor[0] = new  BillerInfo()
			{
				billerNotes = "Same Day Notification",
				expectedPostingTimeFrame = "",
				billerState = "NEW YORK",
				billerCity = "AMHERST",
				
			};
			billerSearchResp.billerInfo = billerInfor;
			return billerSearchResp;
		}

		private BpValidationResponse BillpayValidation(BpValidationRequest validationRequest)
		{
			BpValidationResponse bpvalidationResp = new BpValidationResponse();
			bpvalidationResp.serviceOfferingID = "";
			bpvalidationResp.printMGICustomerServiceNumber = false;
			bpvalidationResp.agentTransactionId = "";
			bpvalidationResp.readyForCommit = true;
			bpvalidationResp.processingFee = 0;
			bpvalidationResp.infoFeeIndicator = true;
			bpvalidationResp.exchangeRateApplied = 1;
			bpvalidationResp.sendAmounts = new SendAmountInfo()
			{
				sendAmount = validationRequest.amount,
				sendCurrency = "USD",
				totalSendFees = 7.95m,
				totalSendTaxes=0	
			};
			bpvalidationResp.sendAmounts.totalAmountToCollect = (validationRequest.amount + bpvalidationResp.sendAmounts.totalSendFees);			
			return bpvalidationResp;
		}
	}
}
