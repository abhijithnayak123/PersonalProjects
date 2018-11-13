using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

using MGI.Channel.DMS.Server.Contract;
using DMSServer = MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Contract;
using Spring.Transaction.Interceptor;
using CoreCustomerSession = MGI.Core.Partner.Data.CustomerSession;
using BizChannelPartnerDTO = MGI.Biz.Partner.Data.ChannelPartner;
// TODO: Need to revisit this
using MGI.Biz.Partner.Contract;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : IDesktopService
	{
		public DesktopEngine()
		{
			CustomerConverter();
			AgentConverter();
			CheckCashingConverter();
			DataStructuresConverter();
			ChannelPartnerConverter();
			ShoppingCartConverter();
			BillPayConverter();
			MoneyTransferConverter();
			FundEngineConverter();
			UserEngineConverter();
			LocationEngineConverter();
			NpsTerminalConveter();
			TerminalEngineConverter();
			TransactionEngineConverter();
			MoneyOrderConverter();
			MessageCenterConverter();
			ReceiptsConverter();
			LocationProcessorCredentialsEngineConverter();

			Mapper.CreateMap<MGI.Channel.DMS.Server.Data.MGIContext, MGI.Common.Util.MGIContext>();
		}
		public ISharedService SharedEngine { get; set; }
		private DMSServer.MGIContext _GetContext(long customerSessionId, DMSServer.MGIContext mgiContext = null)
		{
			CoreCustomerSession customerSession = CustomerSessionSvc.Lookup(customerSessionId);

			var channelPartner = customerSession.Customer.ChannelPartner;

			if (mgiContext == null)
				mgiContext = new DMSServer.MGIContext();

			if (mgiContext.Context == null)
				mgiContext.Context = new Dictionary<string, object>();

			mgiContext.TimeZone = customerSession.AgentSession.Terminal.Location.TimezoneID;
			mgiContext.ChannelPartnerId = channelPartner.Id;
			mgiContext.ChannelPartnerName = channelPartner.Name;
			mgiContext.AgentId = Convert.ToInt32(customerSession.AgentSession.Agent.Id);
			mgiContext.AlloyId = customerSession.Customer.Id;
			mgiContext.BankId = customerSession.AgentSession.Terminal.Location.BankID;
			mgiContext.BranchId = Convert.ToInt32(customerSession.AgentSession.Terminal.Location.BranchID);

			mgiContext.LocationName = customerSession.AgentSession.Terminal.Location.LocationName;

			mgiContext.LocationRowGuid = customerSession.AgentSession.Terminal.Location.rowguid;

			mgiContext.CustomerSessionId = customerSessionId;
			if (string.IsNullOrEmpty(mgiContext.AgentFirstName))
				mgiContext.AgentFirstName = customerSession.AgentSession.Agent.FirstName;
			if (string.IsNullOrEmpty(mgiContext.AgentLastName))
				mgiContext.AgentLastName = customerSession.AgentSession.Agent.LastName;

			mgiContext.AgentName = customerSession.AgentSession.Agent.UserName;

			//For INGO Check US2321  
			var INGOCredential = customerSession.AgentSession.Terminal.Location.LocationProcessorCredentials.Where(c => c.ProviderId == (int)DMSServer.ProviderIds.Ingo).FirstOrDefault();
			if (INGOCredential != null)
			{
				mgiContext.CheckUserName = INGOCredential.UserName;
				mgiContext.CheckPassword = INGOCredential.Password;
			}

			var certegyCredential = customerSession.AgentSession.Terminal.Location.LocationProcessorCredentials.Where(c => c.ProviderId == (int)DMSServer.ProviderIds.Certegy).FirstOrDefault();
			if (certegyCredential != null)
			{
				mgiContext.CertegySiteId = certegyCredential.Identifier;
			}

			mgiContext.TerminalName = customerSession.AgentSession.Terminal.Name;
			mgiContext.AgentSessionId = customerSession.AgentSession.Id;
			mgiContext.LocationId = Convert.ToString(customerSession.AgentSession.Terminal.Location.LocationIdentifier);

			//For TSys Prepaid Card US2321
			var TSysCredential = customerSession.AgentSession.Terminal.Location.LocationProcessorCredentials.Where(c => c.ProviderId == (int)DMSServer.ProviderIds.TSys).FirstOrDefault();
			if (TSysCredential != null)
			{
				mgiContext.TSysPartnerId = TSysCredential.Identifier;
			}
			mgiContext.ChannelPartnerRowGuid = channelPartner.rowguid;
			mgiContext.LocationStateCode = customerSession.AgentSession.Terminal.Location.State;

			var visaCredential = customerSession.AgentSession.Terminal.Location.LocationProcessorCredentials.Where(c => c.ProviderId == (int)DMSServer.ProviderIds.Visa).FirstOrDefault();

			mgiContext.VisaLocationNodeId = visaCredential == null ? -1 : long.Parse(visaCredential.Identifier);

			return mgiContext;
		}

		private DMSServer.MGIContext _GetContextFromAgentSession(long sessionId, DMSServer.MGIContext mgiContext = null)
		{
			if (mgiContext == null)
			{
				mgiContext = new DMSServer.MGIContext();
			}

			if (mgiContext.Context == null)
				mgiContext.Context = new Dictionary<string, object>();

			MGIContext context = new MGIContext();

			MGI.Biz.Partner.Data.Session agentSession = AgentService.GetSession(sessionId, context);
			if (agentSession.Terminal != null)
			{
				MGI.Biz.Partner.Data.Location location = agentSession.Terminal.Location;
				mgiContext.LocationName = location.LocationName;
				mgiContext.LocationRowGuid = location.RowGuid;
				mgiContext.BankId = location.BankID;
				mgiContext.BranchId = Convert.ToInt32(location.BranchID);
				mgiContext.TimeZone = location.TimezoneID;
				mgiContext.ChannelPartnerId = agentSession.Terminal.ChannelPartner.Id;
				mgiContext.ChannelPartnerName = agentSession.Terminal.ChannelPartner.Name;
				mgiContext.ChannelPartnerRowGuid = agentSession.Terminal.ChannelPartner.rowguid;

				mgiContext.TerminalName = agentSession.Terminal.Name;
				mgiContext.AgentSessionId = agentSession.Id;
				mgiContext.LocationId = Convert.ToString(agentSession.Terminal.Location.LocationIdentifier);

				//For INGO Check US2321  
				var INGOCredential = agentSession.Terminal.Location.ProcessorCredentials.Where(c => c.ProviderId == (int)DMSServer.ProviderIds.Ingo).FirstOrDefault();
				if (INGOCredential != null)
				{
					mgiContext.CheckUserName = INGOCredential.UserName;
					mgiContext.CheckPassword = INGOCredential.Password;
				}

				var TSysCredential = agentSession.Terminal.Location.ProcessorCredentials.Where(c => c.ProviderId == (int)DMSServer.ProviderIds.TSys).FirstOrDefault();
				if (TSysCredential != null)
				{
					mgiContext.TSysPartnerId = TSysCredential.Identifier;
				}
			}
			return mgiContext;
		}

		private bool _IsAnonymous(string firstName, string lastName)
		{
			if (firstName.ToLower().Equals("unregistered") && lastName.ToLower().Equals("customer"))
				return true;
			else
				return false;
		}

		[Transaction(ReadOnly = true)]
		private BizChannelPartnerDTO ChannelPartnerConfig(Guid rowid, MGI.Channel.DMS.Server.Data.MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizChannelPartnerDTO channelPartner = ChannelPartnerService.ChannelPartnerConfig(rowid, context);
			return channelPartner;
		}
	}
}

