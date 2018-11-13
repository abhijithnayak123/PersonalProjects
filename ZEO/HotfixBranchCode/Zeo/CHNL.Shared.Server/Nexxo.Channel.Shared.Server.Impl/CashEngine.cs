using System;
using System.Collections.Generic;
using MGI.Biz.CashEngine.Contract;
using MGI.Channel.Shared.Server.Contract;
using Spring.Transaction.Interceptor;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.Shared.Server.Impl
{
    public partial class SharedEngine : ICashService
    {
        #region Injected Services

        public ICashEngine CashEngine { private get; set; }

        #endregion

        #region ICashService Impl

        [Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public long CashOut(long customerSessionId, decimal amount, MGIContext mgiContext)
        {
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(amount), "CashOut", AlloyLayerName.SERVICE, ModuleName.CashOut,
									  "Begin CashOut-MGI.Channel.Shared.Server.Impl.CashEngine", mgiContext);
			#endregion

			long cxeCashTrxId = CashEngine.CashOut(customerSessionId, amount, mgiContext);
            //Commented by Bijo as a quick fix Cash to Customer issue in UAT which is same as DE2816
			BIZShoppingCartService.AddCash(customerSessionId, cxeCashTrxId, mgiContext);
			int cashTrxnId;
			if (cxeCashTrxId > 0)
			{
				cashTrxnId = CommitCash(customerSessionId, cxeCashTrxId, mgiContext);
			}
			#region AL-3370 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(cxeCashTrxId), "CashOut", AlloyLayerName.SERVICE, ModuleName.CashOut,
									  "End CashOut-MGI.Channel.Shared.Server.Impl.CashEngine", mgiContext);
			#endregion
			return cxeCashTrxId;
        }

		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		public int CommitCash(long customerSessionId, long cxeTxnId, MGIContext mgiContext)
		{
			return CashEngine.Commit(customerSessionId, cxeTxnId, mgiContext);
		}
        #endregion
    }
}