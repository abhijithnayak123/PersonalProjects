using AutoMapper;
using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreMessage = MGI.Core.Partner.Data.Message;

namespace MGI.Biz.Partner.Impl
{
	public class MessageStoreImpl : IMessageStore
	{
		public MGI.Core.Partner.Contract.IMessageStore MessageStore { private get; set; }

		public MessageStoreImpl()
		{
			Mapper.CreateMap<CoreMessage, Message>();
		}
		public Message GetMessage(string messageKey, MGIContext mgiContext)
		{
			MGI.Core.Partner.Data.Language lang = MGI.Core.Partner.Data.Language.EN;
			CoreMessage messageStore = MessageStore.Lookup(mgiContext.ChannelPartnerId, messageKey, lang);			
			return Mapper.Map<Message>(messageStore);
		}
	}
}
