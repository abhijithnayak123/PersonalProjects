using TCF.Zeo.Biz.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Impl;
using AutoMapper;
using CoreData = TCF.Zeo.Core.Data;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Impl
{
    public class MessageStoreServiceImpl : IMessageStoreService
    {
        IMapper mapper;
        public MessageStoreServiceImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MessageStoreSearch, CoreData.MessageStoreSearch>();
                cfg.CreateMap<CoreData.Message, Message>();
            });

            mapper = config.CreateMapper();
        }

        public Message GetMessage(string messageKey, commonData.ZeoContext context)
        {
            IMessageStore store = new MessageStoreImpl();
            CoreData.Message message = store.GetMessage(messageKey, context);
            return mapper.Map<Message>(message);
        }

        public Message Lookup(MessageStoreSearch search, commonData.ZeoContext context)
        {
            IMessageStore store = new MessageStoreImpl();
            CoreData.MessageStoreSearch coreSearch = mapper.Map<CoreData.MessageStoreSearch>(search);
            CoreData.Message message = store.Lookup(coreSearch, context);
            return mapper.Map<Message>(message);
        }
    }
}
