using System.Collections.Generic;
using BizPartnerData = MGI.Biz.Partner.Data;
using BizPartnerTrxData = MGI.Biz.Partner.Data.Transactions;
using BizPartnerContract = MGI.Biz.Partner.Contract;
using AutoMapper;
using MGI.Channel.Shared.Server.Data;
using MGI.Channel.Shared.Server.Contract;
using MGI.Common.Util;

namespace MGI.Channel.Shared.Server.Impl
{
    public partial class SharedEngine : ITransactionHistoryService
    {
        #region Injected Services

        public BizPartnerContract.ITransactionHistoryService BIZPartnerTransactionHistoryService { private get; set; }

        #endregion

        #region TransactionHistoryService Data Mapper

        public static void TransactionEngineConverter()
        {
            Mapper.CreateMap<BizPartnerData.Account, Account>();
            Mapper.CreateMap<BizPartnerData.MoneyTransferTransaction, MoneyTransferTransaction>()
                .ForMember(x => x.TransactionAmount, o => o.MapFrom(x => x.Amount))
                .ForMember(x => x.TransactionType, o => o.MapFrom(x => x.TransferType))
                .AfterMap((s, d) =>
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
                            IsReceiverHasPhotoId = s.IsReceiverHasPhotoId,
                            NickName = s.ReceiverNickName,
                            PhoneNumber = s.ReceiverPhone
                        };
                    }
                });

            Mapper.CreateMap<BizPartnerTrxData.PastTransaction, PastTransaction>()
                .ForMember(x => x.TransactionId, o => o.MapFrom(x => x.Id));
            Mapper.CreateMap<BizPartnerData.BillPayTransaction, Server.Data.BillPayTransaction>();
        }

        #endregion

        #region ITransactionHistoryService Impl

		public MoneyTransferTransaction GetMoneyTransferTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
			BizPartnerData.MoneyTransferTransaction moneyTransferTrx = BIZPartnerTransactionHistoryService.GetMoneyTransferTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
            return Mapper.Map<MoneyTransferTransaction>(moneyTransferTrx);
        }

		public List<PastTransaction> GetPastTransactions(long agentSessionId, long customerSessionId, long customerId, string transactionType, MGIContext mgiContext)
        {
			var corePastTranssction = BIZPartnerTransactionHistoryService.GetPastTransactions(agentSessionId, customerSessionId, customerId, transactionType, mgiContext);
            return Mapper.Map<List<PastTransaction>>(corePastTranssction);
        }


		public BillPayTransaction GetBillPayTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
			var billPayTrx = BIZPartnerTransactionHistoryService.GetBillPayTransaction(agentSessionId, customerSessionId, transactionId, mgiContext);
            return Mapper.Map<BillPayTransaction>(billPayTrx);
        }

        #endregion
    }
}
