using System;
using System.Collections.Generic;
using AutoMapper;
using TCF.Zeo.Biz.Contract;
using TCF.Channel.Zeo.Data;
using coreData = TCF.Zeo.Core.Data;
using coreImpl = TCF.Zeo.Core.Impl;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Common.Data.Exceptions;

namespace TCF.Zeo.Biz.Impl
{
    public class ManageNpsTerminal : IManageNpsTerminal
    {
        IMapper mapper;
        TCF.Zeo.Core.Contract.INpsTerminal _NpsTerminalService;
        public TCF.Zeo.Core.Contract.IChannelPartnerService ChannelPartnerService { private get; set; }
        public ManageNpsTerminal()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NpsTerminal, coreData.NpsTerminal>();
                cfg.CreateMap<coreData.NpsTerminal, NpsTerminal>();
            });
            mapper = config.CreateMapper();
        }
        public bool CreateNpsTerminal(NpsTerminal npsTerminal, commonData.ZeoContext context)
        {
            try
            {
                using (_NpsTerminalService = new coreImpl.ZeoCoreImpl())
                {
                    coreData.NpsTerminal coreNpsTerminal = mapper.Map<NpsTerminal, coreData.NpsTerminal>(npsTerminal);
                    return _NpsTerminalService.CreateNpsTerminal(coreNpsTerminal, context.TimeZone, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new TerminalException(TerminalException.NPSTERMINAL_CREATE_FAILED, ex);
            }
        }

        public List<NpsTerminal> GetNpsTerminalBylocationId(string locationId, commonData.ZeoContext context)
        {
            try
            {
                using (_NpsTerminalService = new coreImpl.ZeoCoreImpl())
                {
                    List<coreData.NpsTerminal> npsTerminal = _NpsTerminalService.GetNpsTerminalBylocationId(locationId, context);
                    return mapper.Map<List<NpsTerminal>>(npsTerminal);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new TerminalException(TerminalException.NPSTERMINAL_GET_FAILED, ex);
            }
        }

        public NpsTerminal GetNpsTerminalByName(string name, long channelPartnerId, commonData.ZeoContext context)
        {
            try
            {
                using (_NpsTerminalService = new coreImpl.ZeoCoreImpl())
                {
                    return mapper.Map<NpsTerminal>(_NpsTerminalService.GetNpsTerminalByName(name, channelPartnerId, context));
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new TerminalException(TerminalException.NPSTERMINAL_GET_FAILED, ex);
            }
        }

        public List<NpsTerminal> GetNpsterminalByTerminalId(long terminalId, commonData.ZeoContext context)
        {
            try
            {
                using (_NpsTerminalService = new coreImpl.ZeoCoreImpl())
                {
                    List<coreData.NpsTerminal> npsTerminal = _NpsTerminalService.GetNpsterminalByTerminalId(terminalId, context);
                    return mapper.Map<List<NpsTerminal>>(npsTerminal);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new TerminalException(TerminalException.NPSTERMINAL_GET_FAILED, ex);
            }
        }

        public bool UpdateNpsTerminal(NpsTerminal npsTerminal, commonData.ZeoContext context)
        {
            try
            {
                using (_NpsTerminalService = new coreImpl.ZeoCoreImpl())
                {
                    coreData.NpsTerminal coreNpsTerminalLocation = mapper.Map<NpsTerminal, coreData.NpsTerminal>(npsTerminal);
                    return _NpsTerminalService.UpdateNpsTerminal(coreNpsTerminalLocation, context.TimeZone, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new TerminalException(TerminalException.NPSTERMINAL_UPDATE_FAILED, ex);
            }
        }
    }
}
