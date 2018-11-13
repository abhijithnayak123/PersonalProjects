using System;
using System.Linq;
using System.Collections.Generic;
using MGI.Channel.Shared.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;
using AutoMapper;
using BizFundsService = MGI.Biz.FundsEngine.Contract.IFundsEngine;
using BizFundsAccount = MGI.Biz.FundsEngine.Data.FundsAccount;
using BizFundsTrx = MGI.Biz.FundsEngine.Data.Funds;
using AgentSessionSvc = MGI.Biz.Partner.Contract.IAgentService;
using CustomerSessionSvc = MGI.Core.Partner.Contract.ICustomerSessionService;
using CoreCustomerSession = MGI.Core.Partner.Data.CustomerSession;
using BizFundType = MGI.Biz.FundsEngine.Data.FundType;
using BizCommon = MGI.Biz.Common.Data;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.Shared.Server.Impl
{
    public partial class SharedEngine : IFundsProcessorService
    {
        #region Injected Services

        public BizFundsService BizFundsService { private get; set; }
        
        #endregion

        #region BillPay Data Mapper

        public static void FundEngineConverter()
        {
            Mapper.CreateMap<Funds, BizFundsTrx>();
            Mapper.CreateMap<FundsProcessorAccount, BizFundsAccount>();
            Mapper.CreateMap<BizFundsAccount, FundsProcessorAccount>();
            Mapper.CreateMap<FundType, BizFundType>();
            Mapper.CreateMap<BizCommon.TransactionFee, TransactionFee>();
        }

        #endregion

		public long WithdrawFunds(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<Funds>(customerSessionId, funds, "WithdrawFunds", AlloyLayerName.SERVICE,
				ModuleName.Funds, "Begin WithdrawFunds - MGI.Channel.Shared.Server.Impl.FundsEngine",
				mgiContext);
			#endregion

			BizFundsTrx bizFundsTrx = null;

            bizFundsTrx = Mapper.Map<BizFundsTrx>(funds);
            long cxeFundsTrxId = BizFundsService.Withdraw(customerSessionId, bizFundsTrx, mgiContext);
            BIZShoppingCartService.AddFunds(customerSessionId, cxeFundsTrxId, mgiContext);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(cxeFundsTrxId), "WithdrawFunds", AlloyLayerName.SERVICE,
				ModuleName.Funds, "End WithdrawFunds - MGI.Channel.Shared.Server.Impl.FundsEngine",
				mgiContext);
			#endregion

			return cxeFundsTrxId;
        }

		public FundsProcessorAccount LookupFundsAccount(long customerSessionId, MGIContext mgiContext)
        {
            try
            {
                return Mapper.Map<FundsProcessorAccount>(BizFundsService.GetAccount(customerSessionId, mgiContext));
            }
            catch (System.Exception ex)
			{
				//AL-3372 Transactional Log User Story
				MongoDBLogger.Error<string>(string.Empty, "LookupFundsAccount", AlloyLayerName.SERVICE,
					ModuleName.Funds, "End LookupFundsAccount - MGI.Channel.Shared.Server.Impl.FundsEngine",
					ex.Message, ex.StackTrace);
				
				MGI.Biz.FundsEngine.Contract.BizFundsException bizFundsex = ((MGI.Biz.FundsEngine.Contract.BizFundsException)ex);
                if (bizFundsex != null && bizFundsex.MajorCode == 1003 && (bizFundsex.MinorCode == 2019 || bizFundsex.MinorCode == 6003))
                    //for the first time there can be a scenario where there will not be an account for funds
                    return null;
                else
                    throw ex;
			}
		}

		public TransactionFee GetFundsFee(long customerSessionId, decimal amount, FundType fundsType, MGIContext mgiContext)
		{
			#region AL-3372 transaction information for GPR cards.
			List<string> details = new List<string>();
			details.Add("Amount : " + amount);
			details.Add("Funds Type : " + Convert.ToString(fundsType));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetFundsFee", AlloyLayerName.SERVICE,
				ModuleName.Funds, "Begin GetFundsFee - MGI.Channel.Shared.Server.Impl.FundsEngine",
				mgiContext);
			#endregion

			BizCommon.TransactionFee fee = BizFundsService.GetFee(customerSessionId, amount, Mapper.Map<BizFundType>(fundsType), mgiContext);

			#region AL-3372 transaction information for GPR cards.
			MongoDBLogger.Info<BizCommon.TransactionFee>(customerSessionId, fee, "GetFundsFee", AlloyLayerName.SERVICE,
				ModuleName.Funds, "End GetFundsFee - MGI.Channel.Shared.Server.Impl.FundsEngine",
				mgiContext);
			#endregion

			return Mapper.Map<TransactionFee>(fee);
        }

    }
}
