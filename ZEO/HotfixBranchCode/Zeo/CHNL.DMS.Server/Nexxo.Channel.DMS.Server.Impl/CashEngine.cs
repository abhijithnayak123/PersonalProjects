using System;
using System.Collections.Generic;
using System.Diagnostics;
using MGI.Biz.CashEngine.Contract;
using MGI.Channel.DMS.Server.Contract;
using MGI.Core.Catalog.Contract;
using Spring.Transaction.Interceptor;
using MGI.Biz.CashEngine.Data;
using MGI.Channel.DMS.Server.Data;
using AutoMapper;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine: ICashService
	{
        private IProductService _productService;
        public IProductService ProductService
        {
            set { _productService = value; }
        }

        public ICashEngine CashEngine { private get; set; }

        [Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		[DMSMethodAttribute(DMSFunctionalArea.Cash, "Cash in")]
		public long CashIn(long customerSessionId, decimal amount, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			string cashInAmt = "Cash In Amount:" + Convert.ToString(amount);

			MongoDBLogger.Info<string>(customerSessionId, cashInAmt, "CashIn", AlloyLayerName.SERVICE,
				ModuleName.CashIn, "Begin CashIn - MGI.Channel.DMS.Server.Impl.CashEngine",
				context);
			#endregion

			long cxeCashTrxId = CashEngine.CashIn(customerSessionId, amount, context);
            BIZShoppingCartService.AddCash(customerSessionId, cxeCashTrxId, context);
			//AL-2729 Commiting the cash in transaction has been moved to biz partner.

			#region AL-1071 Transactional Log class for MO flow
			string cashTranId = "CXE Cash In TransactionId:" + Convert.ToString(cxeCashTrxId);
			MongoDBLogger.Info<string>(customerSessionId, cashTranId, "CashIn", AlloyLayerName.SERVICE,
				ModuleName.CashIn, "End CashIn - MGI.Channel.DMS.Server.Impl.CashEngine",
				context);
			#endregion
		return cxeCashTrxId;
		}

		//AL-2729 user story Used for updating the cash-in transaction
		[Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
		[DMSMethodAttribute(DMSFunctionalArea.Cash, "Cash in")]
        public long UpdateCash(long customerSessionId, long trxId, decimal amount, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			return CashEngine.Update(customerSessionId, trxId, amount, context);
		}
    }
}
