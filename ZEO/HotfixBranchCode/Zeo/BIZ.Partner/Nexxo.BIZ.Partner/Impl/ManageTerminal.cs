using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using BizData = MGI.Biz.Partner.Data;
using BizContract = MGI.Biz.Partner.Contract;
using CoreData = MGI.Core.Partner.Data;
using CoreContract = MGI.Core.Partner.Contract;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Impl
{
	public class ManageTerminal : BizContract.IManageTerminal
	{
		public CoreContract.ITerminalService PartnerTerminalService { private get; set; }
		public CoreContract.IChannelPartnerService ChannelPartnerService { private get; set; }
		public CoreContract.INpsTerminal PartnerNpsTerminalService { private get; set; }

		public ManageTerminal()
		{
			Mapper.CreateMap<CoreData.Terminal, BizData.Terminal>();
			Mapper.CreateMap<BizData.Terminal, CoreData.Terminal>();
            Mapper.CreateMap<BizData.ChannelPartnerProductProvider, CoreData.ChannelPartnerProductProvider>();
		}

        public BizData.Terminal Lookup(long Id, MGIContext mgiContext)
		{
			CoreData.Terminal terminal = PartnerTerminalService.Lookup(Id);
			return Mapper.Map<BizData.Terminal>(terminal);
		}

        public bool Create(long agentSessionId, BizData.Terminal terminal, MGIContext mgiContext)
		{
			CoreData.Terminal coreTerminal = Mapper.Map<CoreData.Terminal>(terminal);

			if (coreTerminal.ChannelPartner.TIM == (short)BizData.TerminalIdentificationMechanism.HostName)
			{	//Create or Update NPSTerminal from Terminal Data	
				coreTerminal.PeripheralServer = updateNPSTerminal(coreTerminal);
			}

			Guid terminalId = PartnerTerminalService.Create(coreTerminal);

			return terminalId != Guid.Empty;
		}


		private CoreData.NpsTerminal updateNPSTerminal(CoreData.Terminal coreTerminal)
		{
			CoreData.NpsTerminal npsterminal;

			if (coreTerminal.PeripheralServer == null)
				npsterminal = PartnerNpsTerminalService.Lookup(coreTerminal.Name, coreTerminal.ChannelPartner);
			else
				npsterminal = coreTerminal.PeripheralServer;

			if (npsterminal != null)
			{
				npsterminal.Location = coreTerminal.Location;
				PartnerNpsTerminalService.Update(npsterminal);
			}
			else
			{
				npsterminal = new CoreData.NpsTerminal()
				{
					ChannelPartner = coreTerminal.ChannelPartner,
					Description = "",
					Location = coreTerminal.Location,
					Name = coreTerminal.Name,
					Status = "Available",
					PeripheralServiceUrl = "https://nps.nexxofinancial.com:18732/Peripheral/"
				};

				PartnerNpsTerminalService.Create(npsterminal);
			}
			return npsterminal;
		}


        public bool Update(long agentSessionId, BizData.Terminal terminal, MGIContext mgiContext)
		{
			CoreData.Terminal coreTerminal = Mapper.Map<CoreData.Terminal>(terminal);

			if (coreTerminal.ChannelPartner.TIM == (short)BizData.TerminalIdentificationMechanism.HostName)
			{
				//Update NPSTerminal With Location if Not Location Mapped
				updateNPSFromTerminal(coreTerminal);
			}

			return PartnerTerminalService.Update(coreTerminal);
		}


		private void updateNPSFromTerminal(CoreData.Terminal terminal)
		{
			if (terminal.PeripheralServer != null)
			{
				CoreData.NpsTerminal npsterminal = PartnerNpsTerminalService.Lookup(terminal.PeripheralServer.Id);

				if (npsterminal != null && npsterminal.Location == null)
				{
					npsterminal.Location = terminal.Location;
				}

				PartnerNpsTerminalService.Update(npsterminal);
			}
		}

        public BizData.Terminal Lookup(long agentSessionId, string terminalName, int channelPartnerId, MGIContext mgiContext)
		{
			CoreData.ChannelPartner channelPartner = ChannelPartnerService.ChannelPartnerConfig(channelPartnerId);
			CoreData.Terminal terminal = PartnerTerminalService.Lookup(terminalName, channelPartner, mgiContext);

			if (terminal != null && terminal.PeripheralServer != null)
			{
				if (terminal.Location == null && terminal.PeripheralServer.Location != null)
				{
					terminal.Location = terminal.PeripheralServer.Location;
					PartnerTerminalService.Update(terminal);
				}
			}

			return Mapper.Map<BizData.Terminal>(terminal);
		}
	}
}
