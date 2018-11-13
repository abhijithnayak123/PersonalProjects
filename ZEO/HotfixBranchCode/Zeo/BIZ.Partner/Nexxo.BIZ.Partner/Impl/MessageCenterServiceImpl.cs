using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Contract;
using MGI.Common.Util;
using CoreData = MGI.Core.Partner.Data;
using CoreContract = MGI.Core.Partner.Contract;
using BizData = MGI.Biz.Partner.Data;
using BizContract = MGI.Biz.Partner.Contract;
using AutoMapper;
using ICXECustomerService = MGI.Core.CXE.Contract.ICustomerService;
using MGI.Cxn.Check.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Core.Partner.Contract;
using MGI.Cxn.Common.Processor.Util;
using MGI.Cxn.Check.Contract;

namespace MGI.Biz.Partner.Impl
{
    public class MessageCenterServiceImpl : IMessageCenterService
    {
        public CoreContract.IAgentSessionService AgentSessionService { private get; set; }
        public CoreContract.IManageUsers ManageUserService { private get; set; }
        public CoreContract.IMessageCenter MessageCenterService { private get; set; }
        public ICXECustomerService CXECustomerService { private get; set; }
        public CoreContract.ITransactionService<CoreData.Transactions.Check> CoreCheckService { private get; set; }
        private CoreContract.IMessageStore MessageStore = null;
		public IProcessorRouter CheckProcessorRouter { private get; set; }
		public CoreContract.IChannelPartnerService ChannelPartnerSvc { private get; set; }

		public MessageCenterServiceImpl()
		{
			Mapper.CreateMap<Biz.Partner.Data.Transactions.Check, MGI.Core.Partner.Data.Transactions.Check>();
		}

		public bool Update(long customerSessionId, AgentMessage agentMessage, MGIContext mgiContext)
		{
			CoreData.AgentMessage agentMsg = MessageCenterService.Lookup(Mapper.Map<Biz.Partner.Data.Transactions.Check, MGI.Core.Partner.Data.Transactions.Check>(agentMessage.Transaction));

			agentMsg.IsParked = agentMessage.IsParked;
			agentMsg.IsActive = agentMessage.IsActive;

			return MessageCenterService.Update(agentMsg);
		}

		public List<AgentMessage> GetMessagesByAgentID(long agentSessionId, MGIContext mgiContext)
		{
			int agentId = (int)AgentSessionService.Lookup(agentSessionId).Agent.Id;

			DateTime nowDt = DateTime.Now;
			DateTime timeSpanDt = DateTime.Now.AddMinutes(-30);

			var messages = MessageCenterService.GetByAgentID(agentId)
				.Where(t => t.Transaction.CXEState == (int)CheckStatus.Pending
					|| t.DTServerLastModified > DateTime.Now.AddMinutes(-30))
				.OrderBy(t => t.Transaction.CXEState)
				.ThenByDescending(t => t.DTServerLastModified);


			List<Biz.Partner.Data.AgentMessage> agentMessages = new List<AgentMessage>();

			MGI.Core.CXE.Data.Customer customer = null;

			foreach (var message in messages)
			{
				customer = CXECustomerService.Lookup(message.Transaction.CustomerSession.Customer.CXEId);
				long CxnId = message.Transaction.CXNId;

				string channelPartnerName = ChannelPartnerSvc.ChannelPartnerConfig(message.Transaction.CustomerSession.Customer.ChannelPartnerId).Name;
				ICheckProcessor checkProcessor = _GetCheckProcessor(channelPartnerName);
				CheckTrx cxnCheck = checkProcessor.Get(CxnId);
				string _declineMessage = string.Empty;
				MGI.Core.Partner.Data.Language lang = MGI.Core.Partner.Data.Language.EN;
				if (cxnCheck.DeclineCode != 0)
				{
					//BizCustomerException
					MGI.Biz.CPEngine.Contract.BizCPEngineException bizcheckdeclineexception = new MGI.Biz.CPEngine.Contract.BizCPEngineException(cxnCheck.DeclineCode);
					MGI.Core.Partner.Data.Message _message = MessageStore.Lookup(customer.ChannelPartnerId, Convert.ToString(bizcheckdeclineexception.MajorCode) + "." + Convert.ToString(bizcheckdeclineexception.MinorCode), lang);
					_declineMessage = _message.Content;
				}
				Biz.Partner.Data.AgentMessage agentMessage = new AgentMessage()
				{
					Amount = message.Transaction.Amount.ToString(),
					TransactionState = ((CheckStatus)message.Transaction.CXEState).ToString(),
					TicketNumber = checkProcessor.Get(message.Transaction.CXNId).TicketId.ToString(),
					CustomerFirstName = customer.FirstName,
					CustomerLastName = customer.LastName,
					TransactionId = message.Transaction.Id,
					DeclineMessage = _declineMessage
				};

				agentMessages.Add(agentMessage);
			}

			return agentMessages;
		}

		public AgentMessage GetMessageDetails(long agentSessionId, long transactionId, MGIContext mgiContext)
		{
			CoreData.Transactions.Check ptnrCheck = CoreCheckService.Lookup(transactionId);
			MGI.Core.CXE.Data.Customer customer = CXECustomerService.Lookup(ptnrCheck.CustomerSession.Customer.CXEId);
			string channelPartnerName = ChannelPartnerSvc.ChannelPartnerConfig(ptnrCheck.CustomerSession.Customer.ChannelPartnerId).Name;
			return new AgentMessage()
			{
				Amount = ptnrCheck.Amount.ToString("0.00"),
				TransactionState = Convert.ToString((MGI.Core.CXE.Data.TransactionStates)ptnrCheck.CXEState),
				TicketNumber = _GetCheckProcessor(channelPartnerName).Get(ptnrCheck.CXNId).TicketId.ToString(),
				CustomerFirstName = customer.FirstName,
				CustomerLastName = customer.LastName,
				TransactionId = transactionId
			};
		}

		private ICheckProcessor _GetCheckProcessor(string channelPartner)
		{
			// get the fund processor for the channel partner.
			return (ICheckProcessor)CheckProcessorRouter.GetProcessor(channelPartner);
		}
	}
}
