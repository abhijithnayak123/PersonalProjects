using System;
using System.Collections.Generic;
using AutoMapper;
using BizContract = MGI.Biz.Partner.Contract;
using BizData = MGI.Biz.Partner.Data;
using CoreContract = MGI.Core.Partner.Contract;
using CoreData = MGI.Core.Partner.Data;
using MGI.Biz.Partner.Data;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Impl
{
	public class ManageNpsTerminal : BizContract.IManageNpsTerminal
	{
		#region Dependencies

		public CoreContract.INpsTerminal PartnerNpsTerminalService { private get; set; }
		public CoreContract.IChannelPartnerService ChannelPartnerService { private get; set; }
		#endregion

		public ManageNpsTerminal()
		{
			Mapper.CreateMap<BizData.NpsTerminal, CoreData.NpsTerminal>();
			Mapper.CreateMap<CoreData.NpsTerminal, BizData.NpsTerminal>();
            Mapper.CreateMap<BizData.Location, CoreData.Location>();
            Mapper.CreateMap<CoreData.Location, BizData.Location>();
			Mapper.CreateMap<BizData.ChannelPartner, CoreData.ChannelPartner>();
		}

		public bool Create(long agentSessionId, BizData.NpsTerminal npsTerminal, MGIContext mgiContext)
		{

			CoreData.NpsTerminal coreNpsTerminal = Mapper.Map<CoreData.NpsTerminal>(npsTerminal);
			coreNpsTerminal.ChannelPartner = ChannelPartnerService.ChannelPartnerConfig(npsTerminal.ChannelPartnerId);
			return PartnerNpsTerminalService.Create(coreNpsTerminal);
		}

		public bool Update(long agentSessionId, BizData.NpsTerminal npsTerminal, MGIContext mgiContext)
		{

			CoreData.NpsTerminal coreNpsTerminal = Mapper.Map<CoreData.NpsTerminal>(npsTerminal);
			return PartnerNpsTerminalService.Update(coreNpsTerminal);
		}

		public BizData.NpsTerminal Lookup(string agentSessionId, long Id, MGIContext mgiContext)
		{
			CoreData.NpsTerminal bizNpsTerminal = PartnerNpsTerminalService.Lookup(Id);
			return Mapper.Map<BizData.NpsTerminal>(bizNpsTerminal);
		}

		public BizData.NpsTerminal Lookup(long agentSessionId, Guid Id, MGIContext mgiContext)
		{
			CoreData.NpsTerminal bizNpsTerminal = PartnerNpsTerminalService.Lookup(Id);
			return Mapper.Map<BizData.NpsTerminal>(bizNpsTerminal);
		}

		public BizData.NpsTerminal Lookup(long agentSessionId, string ipAddress, MGIContext mgiContext)
		{
			CoreData.NpsTerminal bizNpsTerminal = PartnerNpsTerminalService.Lookup(ipAddress);
			return Mapper.Map<BizData.NpsTerminal>(bizNpsTerminal);
		}

		public BizData.NpsTerminal Lookup(long agentSessionId, string name, ChannelPartner channelPartner, MGIContext mgiContext)
		{
			CoreData.ChannelPartner coreChannelpartner = Mapper.Map<BizData.ChannelPartner,CoreData.ChannelPartner>(channelPartner);
			CoreData.NpsTerminal bizNpsTerminal = PartnerNpsTerminalService.Lookup(name, coreChannelpartner);
			return Mapper.Map<BizData.NpsTerminal>(bizNpsTerminal);
		}

		public List<BizData.NpsTerminal> GetByLocationID(long agentSessionId, long locationId, MGIContext mgiContext)
		{
			List<CoreData.NpsTerminal> bizNpsTerminals = PartnerNpsTerminalService.GetByLocationID(locationId);
			return Mapper.Map<List<BizData.NpsTerminal>>(bizNpsTerminals);
		}
	}
}
