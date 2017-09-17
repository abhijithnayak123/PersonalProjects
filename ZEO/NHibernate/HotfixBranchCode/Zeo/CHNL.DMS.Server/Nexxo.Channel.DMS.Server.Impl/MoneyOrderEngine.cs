using System;
using System.Linq;
using System.Collections.Generic;

using AutoMapper;
using MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;

using MGI.Biz.MoneyOrderEngine.Contract;
using BizMOData = MGI.Biz.MoneyOrderEngine.Data;
using BizPartnerContract = MGI.Biz.Partner.Contract;

using BizCommon = MGI.Biz.Common.Data;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : IMoneyOrderService
	{
		public IMoneyOrderEngineService MoneyOrderEngineService { private get; set; }
		public BizPartnerContract.IShoppingCartService ShoppingCartService { private get; set; }
		public MGI.Common.Util.TLoggerCommon MongoDBLogger { private get; set; }

		internal static void MoneyOrderConverter()
		{
			Mapper.CreateMap<BizMOData.MoneyOrder, MoneyOrder>();
			Mapper.CreateMap<MoneyOrderPurchase, BizMOData.MoneyOrderPurchase>();
			Mapper.CreateMap<BizCommon.TransactionFee, TransactionFee>();
			Mapper.CreateMap<MoneyOrderTransaction, BizMOData.MoneyOrder>();
			Mapper.CreateMap<MoneyOrder, BizMOData.MoneyOrder>();
			Mapper.CreateMap<MoneyOrderData, BizMOData.MoneyOrderData>();
		}

		#region IMoneyOrderService Impl

		#region Shared Methods

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyOrder, "Get money order")]
		public MoneyOrder GetMoneyOrderStage(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			return SharedEngine.GetMoneyOrderStage(customerSessionId, moneyOrderId, context);
		}

		#endregion

		#region DMS Methods

		[Transaction()]
		public TransactionFee GetMoneyOrderFee(long customerSessionId, MoneyOrderData moneyOrder, MGIContext mgiContext)
		{
            try
            {
                mgiContext = _GetContext(customerSessionId, mgiContext);
                MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

				#region AL-1071 Transactional Log class for MO flow
				MongoDBLogger.Info<MoneyOrderData>(customerSessionId, moneyOrder, "GetMoneyOrderFee", AlloyLayerName.SERVICE,
					ModuleName.MoneyOrder, "Begin GetMoneyOrderFee - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine",	context);
				#endregion

                BizCommon.TransactionFee fee = MoneyOrderEngineService.GetMoneyOrderFee(customerSessionId, Mapper.Map<BizMOData.MoneyOrderData>(moneyOrder), context);

				#region AL-1071 Transactional Log class for MO flow
				MongoDBLogger.Info<BizCommon.TransactionFee>(customerSessionId, fee, "GetMoneyOrderFee", AlloyLayerName.SERVICE,
					ModuleName.MoneyOrder, "End GetMoneyOrderFee - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine", context);
				#endregion

                return Mapper.Map<TransactionFee>(fee);
            }
            catch (MGI.Common.Sys.NexxoException nEx)
            {
				MongoDBLogger.Error<MoneyOrderData>(moneyOrder, "GetMoneyOrderFee", AlloyLayerName.SERVICE, ModuleName.MoneyOrder,
						"Error in GetMoneyOrderFee - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine", nEx.Message, nEx.StackTrace);

				throw nEx;
            }
            catch (Exception ex)
            {
				MongoDBLogger.Error<MoneyOrderData>(moneyOrder, "GetMoneyOrderFee", AlloyLayerName.SERVICE, ModuleName.MoneyOrder,
						"Error in GetMoneyOrderFee - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine", ex.Message, ex.StackTrace);

				throw ex;
            }
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyOrder, "Begin MO transaction")]
		public MoneyOrder PurchaseMoneyOrder(long customerSessionId, MoneyOrderPurchase moneyOrderPurchase, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<MoneyOrderPurchase>(customerSessionId, moneyOrderPurchase, "PurchaseMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.MoneyOrder, "Begin PurchaseMoneyOrder - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine",
				context);
			#endregion

			var bizMoneyOrderPurchase = Mapper.Map<BizMOData.MoneyOrderPurchase>(moneyOrderPurchase);

			var bizMoneyOrder = MoneyOrderEngineService.Add(customerSessionId, bizMoneyOrderPurchase, context);

			BIZShoppingCartService.AddMoneyOrder(customerSessionId, Convert.ToInt64(bizMoneyOrder.Id), context);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<BizMOData.MoneyOrder>(customerSessionId, bizMoneyOrder, "PurchaseMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.MoneyOrder, "End PurchaseMoneyOrder - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine",
				context);
			#endregion

			return Mapper.Map<MoneyOrder>(bizMoneyOrder);
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyOrder, "Cancel MO transaction")]
		public void CancelMoneyOrder(long customerSessionId, long moneyOrderId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrderId:" + Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "CancelMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.MoneyOrder, "Begin CancelMoneyOrder - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine",
				context);
			#endregion


			BIZShoppingCartService.RemoveMoneyOrder(customerSessionId, moneyOrderId, false, context);

			MoneyOrderEngineService.UpdateMoneyOrderStatus(customerSessionId, moneyOrderId, (int)MGI.Core.CXE.Data.TransactionStates.Canceled, context);

			MGI.Biz.Partner.Data.ShoppingCart shoppingcart = BIZShoppingCartService.Get(customerSessionId, context);

			var availableMOs = shoppingcart.MoneyOrders.Where(x => x.Id > moneyOrderId);

			foreach (var trx in availableMOs)
			{
				MoneyOrderEngineService.Resubmit(customerSessionId, trx.Id, context);
			}

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<MGI.Biz.Partner.Data.ShoppingCart>(customerSessionId, shoppingcart, "CancelMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.MoneyOrder, "End CancelMoneyOrder - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine",
				context);
			#endregion
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyOrder, "Add MO check number")]
		public void UpdateMoneyOrder(long customerSessionId, MoneyOrderTransaction moneyOrderTransaction, long moneyOrderId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<MoneyOrderTransaction>(customerSessionId, moneyOrderTransaction, "UpdateMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.MoneyOrder, "Begin UpdateMoneyOrder - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine",
				context);
			#endregion

			var bizMoneyOrder = Mapper.Map<BizMOData.MoneyOrder>(moneyOrderTransaction);

			MoneyOrderEngineService.UpdateMoneyOrder(customerSessionId, bizMoneyOrder, moneyOrderId, context);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<BizMOData.MoneyOrder>(customerSessionId, bizMoneyOrder, "UpdateMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.MoneyOrder, "End UpdateMoneyOrder - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine",
				context);
			#endregion
		}

		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.MoneyOrder, "Update MO status")]
		public void UpdateMoneyOrderStatus(long customerSessionId, long moneyOrderId, int newMoneyOrderStatus, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1071 Transactional Log class for MO flow
			List<string> details = new List<string>();
			details.Add("MoneyOrder Id:" + Convert.ToString(moneyOrderId));
			details.Add("New Money Order Status:" + Convert.ToString(newMoneyOrderStatus));

			MongoDBLogger.ListInfo<string>(customerSessionId, details, "UpdateMoneyOrderStatus", AlloyLayerName.SERVICE,
				ModuleName.MoneyOrder, "Begin UpdateMoneyOrderStatus - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine",
				context);
			#endregion

			MoneyOrderEngineService.UpdateMoneyOrderStatus(customerSessionId, moneyOrderId, newMoneyOrderStatus, context);

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "UpdateMoneyOrderStatus", AlloyLayerName.SERVICE,
				ModuleName.MoneyOrder, "End UpdateMoneyOrderStatus - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine",
				context);
			#endregion
		}

		[Transaction()]
		public Data.CheckPrint GenerateCheckPrintForMoneyOrder(long moneyOrderId, long customerSessionId, MGIContext mgiContext)
		{
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			Data.CheckPrint checkPrint = new Data.CheckPrint();

			#region AL-1071 Transactional Log class for MO flow
			string id = "MoneyOrderId:" + Convert.ToString(moneyOrderId);

			MongoDBLogger.Info<string>(customerSessionId, id, "GenerateCheckPrintForMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.MoneyOrder, "Begin GenerateCheckPrintForMoneyOrder - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine",
				context);
			#endregion

			checkPrint.Lines = MoneyOrderEngineService.GetMoneyOrderCheck(customerSessionId, moneyOrderId, context).Lines;

			#region AL-1071 Transactional Log class for MO flow
			MongoDBLogger.Info<Data.CheckPrint>(customerSessionId, checkPrint, "GenerateCheckPrintForMoneyOrder", AlloyLayerName.SERVICE,
				ModuleName.MoneyOrder, "End GenerateCheckPrintForMoneyOrder - MGI.Channel.DMS.Server.Impl.MoneyOrderEngine",
				context);
			#endregion

			return checkPrint;
		}

		[Transaction()]
		public Data.CheckPrint GenerateMoneyOrderDiagnostics(long agentSessionId, MGIContext mgiContext)
		{
			Data.CheckPrint checkPrint = new Data.CheckPrint();
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			checkPrint.Lines = MoneyOrderEngineService.GetMoneyOrderDiagnostics(agentSessionId, context).Lines;

			return checkPrint;
		}

		//GetMoneyOrderDiagnostics

		#endregion

		#endregion

	}
}
