using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizPartnerData = MGI.Biz.Partner.Data;
using BizPartnerContract = MGI.Biz.Partner.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;
using AutoMapper;
using Spring.Transaction.Interceptor;

using SharedData = MGI.Channel.Shared.Server.Data;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Server.Impl
{
    public partial class DesktopEngine : ITransactionHistoryService
    {
        public BizPartnerContract.ITransactionHistoryService BIZPartnerTransactionHistoryService { private get; set; }

        public static void TransactionEngineConverter()
        {
            Mapper.CreateMap<BizPartnerData.TransactionHistory, TransactionHistory>();
            Mapper.CreateMap<BizPartnerData.FundTransaction, FundTransaction>();
            Mapper.CreateMap<BizPartnerData.CheckTransaction, CheckTransaction>();
            Mapper.CreateMap<BizPartnerData.MoneyTransferTransaction, MoneyTransferTransaction>()
                .ForMember(x => x.TransactionAmount, o => o.MapFrom(x => x.Amount))
				.ForMember(x => x.TransactionType, o => o.MapFrom(x => x.TransferType)).AfterMap((s, d) =>
				{
					if (s.ReceiverId != 0)
					{
						d.Receiver = new Receiver()
						{
							Id = s.ReceiverId,
							FirstName = s.ReceiverFirstName,
							LastName = s.ReceiverLastName,
							SecondLastName = s.ReceiverSecondLastName,
							SecurityQuestion = s.ReceiverSecurityQuestion,
							SecurityAnswer = s.ReceiverSecurityAnswer,
							IsReceiverHasPhotoId = s.IsReceiverHasPhotoId
						};
					}
				});
            Mapper.CreateMap<BizPartnerData.MoneyOrderTransaction, MoneyOrderTransaction>();
			Mapper.CreateMap<BizPartnerData.BillPayTransaction, Server.Data.BillPayTransaction>();
		 	Mapper.CreateMap<BizPartnerData.CashTransaction, CashTransaction>();        
		}

        [Transaction()]
        public Response GetTransactionHistory(long customerSessionId, long customerId, string transactionType, string location, DateTime dateRange, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<BizPartnerData.TransactionHistory> trnxHistory = BIZPartnerTransactionHistoryService.GetTransactionHistory(customerSessionId, customerId, transactionType, location, dateRange, context);
			response.Result = ManualMapper(trnxHistory);
			return response;
		}

		[Transaction()]
        public Response GetAgentTransactionHistory(long agentSessionId, long? agentId, string transactionType, string location, bool showAll, long transactionId, int duration, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<BizPartnerData.TransactionHistory> trnxHistory = BIZPartnerTransactionHistoryService.GetTransactionHistory(agentSessionId, agentId, transactionType, location, showAll, transactionId, duration, context);
			response.Result = AutoMapper.Mapper.Map<List<TransactionHistory>>(trnxHistory);
			return response;
		}

        [Transaction()]
        public Response GetFundTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            BizPartnerData.FundTransaction fundTrx = BIZPartnerTransactionHistoryService.GetFundTransaction(agentSessionId, customerSessionId, transactionId, context);
			response.Result = AutoMapper.Mapper.Map<FundTransaction>(fundTrx);
			return response;
        }

        [Transaction()]
		public Response GetCheckTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.CheckTransaction checkTrx = BIZPartnerTransactionHistoryService.GetCheckTransaction(agentSessionId, customerSessionId, transactionId, context);
			response.Result = AutoMapper.Mapper.Map<CheckTransaction>(checkTrx);
			return response;
		}

        private List<TransactionHistory> ManualMapper(List<BizPartnerData.TransactionHistory> transHistory)
        {
            List<TransactionHistory> transHistoryList = new List<TransactionHistory>();
            foreach (var item in transHistory)
            {
                TransactionHistory transaction = new TransactionHistory();
                transaction.CustomerId = item.CustomerId;
                transaction.CustomerName = item.CustomerName;
                transaction.Location = item.Location;
                transaction.SessionId = item.SessionId;
                transaction.Teller = item.Teller;
                //transaction.TellerId = item.TellerId;
                transaction.TotalAmount = item.TotalAmount;
                transaction.TransactionDate = item.TransactionDate;
                transaction.TransactionDetail = item.TransactionDetail;
                transaction.TransactionId = item.TransactionId;
                transaction.TransactionStatus = item.TransactionStatus;
                transaction.TransactionType = item.TransactionType;

                transHistoryList.Add(transaction);
            }

            return transHistoryList;
        }

        [Transaction()]
		public Response GetMoneyTransferTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.MoneyTransferTransaction moneyTransferTrx = BIZPartnerTransactionHistoryService.GetMoneyTransferTransaction(agentSessionId, customerSessionId, transactionId, context);
			response.Result = Mapper.Map<SharedData.MoneyTransferTransaction>(moneyTransferTrx);
			return response;
		}

        [Transaction()]
		public Response GetMoneyOrderTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.MoneyOrderTransaction moneyOrderTrx = BIZPartnerTransactionHistoryService.GetMoneyOrderTransaction(agentSessionId, customerSessionId, transactionId, context);
			response.Result = Mapper.Map<MoneyOrderTransaction>(moneyOrderTrx);
			return response;
		}

		[Transaction()]
		public Response GetBillPayTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.BillPayTransaction billPayTrx = BIZPartnerTransactionHistoryService.GetBillPayTransaction(agentSessionId, customerSessionId, transactionId, context);
			response.Result = Mapper.Map<Server.Data.BillPayTransaction>(billPayTrx);
			return response;
		}

        [Transaction()]
        public Response GetCashTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
			Response response = new Response();
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizPartnerData.CashTransaction cashTrx = BIZPartnerTransactionHistoryService.GetCashTransaction(agentSessionId, customerSessionId, transactionId, context);
			response.Result = Mapper.Map<CashTransaction>(cashTrx);
			return response;
		}
	}
}
