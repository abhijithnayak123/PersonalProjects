using System;
using System.Collections.Generic;
using TCF.Zeo.Biz.Contract;
using CommonUtil = TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Impl;
using ZeoData = TCF.Channel.Zeo.Data;
using AutoMapper;
using TCF.Zeo.Core.Data;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Biz.Common.Data.Exceptions;

namespace TCF.Zeo.Biz.Impl
{
    public class MessageCenterServiceImpl : IMessageCenterService
    {
        IMessageCenter msgCenter;
        IMapper mapper;

        public MessageCenterServiceImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ZeoData.AgentMessage, AgentMessage>().ReverseMap();
            });

            mapper = config.CreateMapper();
        }

        public List<ZeoData.AgentMessage> GetMessagesByAgentID(commonData.ZeoContext context)
        {
            try
            {
                msgCenter = new MessageCenterImpl();

                return mapper.Map<List<ZeoData.AgentMessage>>(msgCenter.GetMessagesByAgentId(context.AgentId, context));
            }
            catch (Exception ex)
            {
                if (CommonUtil.ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MessageCenterException(MessageCenterException.AGENT_MESSAGE_GET_FAILED, ex);
            }
        }

        public ZeoData.AgentMessage GetMessageDetails(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                msgCenter = new MessageCenterImpl();
                return mapper.Map<ZeoData.AgentMessage>(msgCenter.GetMessageByTransactionId(transactionId, context));
            }
            catch (Exception ex)
            {
                if (CommonUtil.ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MessageCenterException(MessageCenterException.AGENT_MESSAGE_GET_FAILED, ex);
            }
        }


    }
}
