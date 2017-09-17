using TCF.Zeo.Biz.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using AutoMapper;
using coredata = TCF.Zeo.Core.Data;
using commonData = TCF.Zeo.Common.Data;
using coreImpl = TCF.Zeo.Core.Impl;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Common.Data.Exceptions;

namespace TCF.Zeo.Biz.Impl
{
    public class ManageTerminal : IManageTerminal
    {
        IMapper mapper;
        TCF.Zeo.Core.Contract.ITerminalService _TerminalService;
        TCF.Zeo.Core.Contract.INpsTerminal _npsTerminalService;

        public ManageTerminal()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<coredata.Terminal, Terminal>().ReverseMap();
            });
            mapper = config.CreateMapper();
        }

        public bool CreateTerminal(Terminal terminal, commonData.ZeoContext context)
        {
            coredata.Terminal coreTerminal = mapper.Map<Terminal, coredata.Terminal>(terminal);
            try
            {
                using (_TerminalService = new coreImpl.ZeoCoreImpl())
                {
                    coredata.ChannelPartner partner = _TerminalService.GetChannelPartnerTim(terminal.LocationId, context);
                    if (partner.TIM == (short)TerminalIdentificationMechanism.HostName)
                    {
                        coredata.NpsTerminal npsterminal = new coredata.NpsTerminal();
                        npsterminal = updateNPSTerminal(coreTerminal, context);
                        if (npsterminal.NpsTerminalId != 0)
                        {
                            coreTerminal.PeripheralServerId = Convert.ToString(npsterminal.NpsTerminalId);
                        }

                    }

                    return _TerminalService.CreateTerminal(coreTerminal, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new TerminalException(TerminalException.TERMINAL_CREATE_FAILED, ex);
            }
        }

        public Terminal GetTerminalById(long terminalId, commonData.ZeoContext context)
        {
            try
            {
                using (_TerminalService = new coreImpl.ZeoCoreImpl())
                {
                    coredata.Terminal coreTerminal = _TerminalService.GetTerminalById(terminalId, context);
                    return mapper.Map<Terminal>(coreTerminal);
                }

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new TerminalException(TerminalException.TERMINAL_GET_FAILED, ex);
            }
        }


        public Terminal GetTerminalByName(string terminalName, commonData.ZeoContext context)
        {
            try
            {
                using (_TerminalService = new coreImpl.ZeoCoreImpl())
                {
                    coredata.Terminal coreTerminal = _TerminalService.GetTerminalByName(terminalName, context);
                    return mapper.Map<Terminal>(coreTerminal);

                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new TerminalException(TerminalException.TERMINAL_GET_FAILED, ex);
            }
        }

        public bool UpdateTerminal(Terminal terminal, commonData.ZeoContext context)
        {
            try
            {
                using (_TerminalService = new coreImpl.ZeoCoreImpl())
                {
                    _npsTerminalService = new coreImpl.ZeoCoreImpl();
                    coredata.Terminal coreTerminal = mapper.Map<Terminal, coredata.Terminal>(terminal);
                    coredata.ChannelPartner partner = _TerminalService.GetChannelPartnerTim(terminal.LocationId, context);
                    List<coredata.NpsTerminal> npsTerminal = _npsTerminalService.GetNpsterminalByTerminalId(Convert.ToInt32(terminal.PeripheralServerId), context);
                    if (partner.TIM == (short)TerminalIdentificationMechanism.HostName)
                    {
                        updateNPSFromTerminal(coreTerminal, context);
                    }
                    return _TerminalService.UpdateTerminal(coreTerminal, context);

                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new TerminalException(TerminalException.TERMINAL_UPDATE_FAILED, ex); ;
            }
        }

        public Terminal GetNpsdiagnosticInfo(long terminalId, commonData.ZeoContext context)
        {
            try
            {
                using (_TerminalService = new coreImpl.ZeoCoreImpl())
                {
                    coredata.Terminal coreTerminal = _TerminalService.GetNpsdiagnosticInfo(terminalId, context);
                    return mapper.Map<Terminal>(coreTerminal);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new TerminalException(TerminalException.TERMINAL_GET_DIAGNOSTIC_FAILED, ex);
            }
        }

        private coredata.NpsTerminal updateNPSTerminal(coredata.Terminal terminal, commonData.ZeoContext context)
        {
            using (_npsTerminalService = new coreImpl.ZeoCoreImpl())
            {
                List<coredata.NpsTerminal> npsTerminals = new List<coredata.NpsTerminal>();
                coredata.NpsTerminal npsTerminal = npsTerminals.FirstOrDefault();

                //AlloyClient.Location location = locations.FirstOrDefault();
                if (terminal.PeripheralServerId == null)
                    npsTerminal = _npsTerminalService.GetNpsTerminalByName(terminal.Name, context.ChannelPartnerId, context);
                else
                    npsTerminals = _npsTerminalService.GetNpsterminalByTerminalId(Convert.ToInt32(terminal.PeripheralServerId), context);
                npsTerminal = npsTerminals.FirstOrDefault();
                if (npsTerminal != null && npsTerminal.NpsTerminalId != 0)
                {
                    npsTerminal.LocationId = terminal.LocationId;
                    _npsTerminalService.UpdateNpsTerminal(npsTerminal, context.TimeZone, context);
                }
                else
                {
                    npsTerminal = new coredata.NpsTerminal();
                    {
                        npsTerminal.Description = "";
                        npsTerminal.Name = terminal.Name;
                        npsTerminal.Status = "Available";
                        npsTerminal.PeripheralServiceUrl = "https://nps.nexxofinancial.com:18732/Peripheral/";
                        npsTerminal.LocationId = terminal.LocationId;
                        npsTerminal.ChannelPartnerId = terminal.ChannelPartnerId;
                    }
                    _npsTerminalService.CreateNpsTerminal(npsTerminal, context.TimeZone, context);

                }
                return npsTerminal;
            }

        }

        private void updateNPSFromTerminal(coredata.Terminal terminal, commonData.ZeoContext context)
        {

            using (_npsTerminalService = new coreImpl.ZeoCoreImpl())
            {
                List<coredata.NpsTerminal> npsTerminals = new List<coredata.NpsTerminal>();

                if (terminal.PeripheralServerId != null)
                {
                    npsTerminals = _npsTerminalService.GetNpsterminalByTerminalId(Convert.ToInt32(terminal.PeripheralServerId), context);
                    coredata.NpsTerminal npsTerminal = npsTerminals.FirstOrDefault();
                    _npsTerminalService.UpdateNpsTerminal(npsTerminal, context.TimeZone, context);
                }
            }
        }
    }
}
