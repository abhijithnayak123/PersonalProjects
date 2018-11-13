using System;
using System.Collections.Generic;
using MGI.Biz.Partner.Data;
using MGI.Channel.Consumer.Server.Contract;
using MGI.Channel.MVA.Server.Contract;
using Spring.Transaction.Interceptor;
using CoreCustomerSession = MGI.Core.Partner.Data.CustomerSession;
using CustomerSessionSvc = MGI.Core.Partner.Contract.ICustomerSessionService;
using BizChannelPartnerDTO = MGI.Biz.Partner.Data.ChannelPartner;
using MGI.Common.Util;

namespace MGI.Channel.MVA.Server.Impl
{
	public partial class MVAEngine : IMVAService
	{
		#region Injected Services
		public IConsumerService ConsumerEngine { get; set; }
		public CustomerSessionSvc CustomerSessionSvc { private get; set; }
		#endregion

		private IMVAService Self;
		public MVAEngine()
		{

		}

		#region IMVAService Impl
		public void SetSelf(IMVAService dts)
		{
			Self = dts;
		}

		[Transaction(ReadOnly = true)]
		public MGIContext GetPartnerContext(string channelPartnerName)
		{
			MGI.Common.Util.MGIContext mgiContext = new Common.Util.MGIContext();

			//TODO This has to be moved to channel partner Engine 
			ChannelPartner channelPartner = ChannelPartnerService.ChannelPartnerConfig(channelPartnerName, mgiContext);

			mgiContext.ChannelPartnerId = channelPartner.Id;
			mgiContext.ChannelPartnerName = channelPartner.Name;
			mgiContext.ApplicationName = "MVA";
			//TODO How to fetch TimeZone TBD
			mgiContext.TimeZone = "Central Standard Time";
			return mgiContext;
		}

		[Transaction(ReadOnly = true)]
		public MGIContext GetCustomerContext(long customerSessionId)
		{
			CoreCustomerSession customerSession = CustomerSessionSvc.Lookup(customerSessionId);

			BizChannelPartnerDTO channelPartner = ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);

			MGI.Common.Util.MGIContext mgiContext = new Common.Util.MGIContext();
			mgiContext.ChannelPartnerName = channelPartner.Name;
			mgiContext.ChannelPartnerId = channelPartner.Id;
			mgiContext.TimeZone = customerSession.TimezoneID;
			//TODO How to fetch LocationId TBD (LocationId is used as counter id for WU provider )
			mgiContext.WUCounterId = "1313992502";
			mgiContext.LocationRowGuid = new Guid("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17");
			//this keys is used in CXN level methods what is the actual use of this? need to be discussed 
			mgiContext.ProcessorId = 14;
			//TODO: Tsys Funds provider requieres this context need to be discussed
			mgiContext.LocationName = "MoneyGram International";

			//BIZ compliance Funds Limit requieres
			mgiContext.Context["ComplianceTerminalState"] = "CA";
			mgiContext.Context["AlloyID"] = customerSession.Customer.Id;
			mgiContext.IsReferral = false;
			// TODO sendmoney - There is no Agent in MVA, so hard coded it as MVAgent(MobileVirtualAgent)
			mgiContext.AgentName = "MVAgent";



			return mgiContext;
		}
		#endregion

		#region Private methods

		private BizChannelPartnerDTO ChannelPartnerConfig(Guid rowid)
		{
			MGI.Common.Util.MGIContext context = new Common.Util.MGIContext();
			BizChannelPartnerDTO channelPartner = ChannelPartnerService.ChannelPartnerConfig(rowid, context);
			return channelPartner;
		}
		#endregion

	}
}
