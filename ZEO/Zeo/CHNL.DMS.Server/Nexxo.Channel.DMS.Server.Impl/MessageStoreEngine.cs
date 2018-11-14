using AutoMapper;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using Spring.Transaction.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizPartnerContract = MGI.Biz.Partner.Contract;
using BizMessage = MGI.Biz.Partner.Data.Message;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : IMessageStoreService
	{
		public BizPartnerContract.IMessageStore MessageStore { private get; set; }
				
		internal static void MoneyStoreConverter()
		{
			Mapper.CreateMap<BizMessage, Message>();
		}
		#region IMessageStoreService Impl
		
		[Transaction()]
		public Response GetMessage(long agentSessionId, string messageKey, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizMessage bizMessage = MessageStore.GetMessage(messageKey, context);
			if(bizMessage == null)
			{
				string defaultErrorCode ="1000.100.9999";
				bizMessage = MessageStore.GetMessage(defaultErrorCode, context);
			}
			Message message = Mapper.Map<BizMessage, Message>(bizMessage);
			response.Result = message;
			return response;
		}

		#endregion
	}
}
