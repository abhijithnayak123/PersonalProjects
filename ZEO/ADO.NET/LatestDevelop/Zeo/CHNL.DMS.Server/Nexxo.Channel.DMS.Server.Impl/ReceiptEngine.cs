﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MGI.Biz.Partner.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using SharedData = MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;
using BizReceiptContract = MGI.Biz.Receipt.Contract;
using BizData = MGI.Biz.Receipt.Data;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.DMS.Server.Impl
{
	public partial class DesktopEngine : IReceiptsService
	{
		internal static void ReceiptsConverter()
		{
			Mapper.CreateMap<BizData.Receipt, ReceiptData>();
		}

		public BizReceiptContract.IReceiptService ReceiptService { private get; set; }

		[Transaction(ReadOnly = true)]
		public Response GetCheckReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<ReceiptData> receiptData = Mapper.Map<List<BizData.Receipt>, List<ReceiptData>>(ReceiptService.GetCheckReceipt(agentSessionId, customerSessionId, transactionId, isReprint, context));
			return new Response()
			{
				Result = receiptData
			};
		}

		[Transaction(ReadOnly = true)]
		public Response GetFundsReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<ReceiptData> receiptData = Mapper.Map<List<BizData.Receipt>, List<ReceiptData>>(ReceiptService.GetFundsReceipt(agentSessionId, customerSessionId, transactionId, isReprint, context));
			return new Response()
			{
				Result = receiptData
			};
		}

		[Transaction(ReadOnly = true)]
		public Response GetMoneyTransferReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<ReceiptData> receiptData = Mapper.Map<List<BizData.Receipt>, List<ReceiptData>>(ReceiptService.GetMoneyTransferReceipt(agentSessionId, customerSessionId, transactionId, isReprint, context));
			return new Response()
			{
				Result = receiptData
			};
		}

		[Transaction(ReadOnly = true)]
		public Response GetMoneyOrderReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			List<ReceiptData> receiptData = Mapper.Map<List<BizData.Receipt>, List<ReceiptData>>(ReceiptService.GetMoneyOrderReceipt(agentSessionId, customerSessionId, transactionId, isReprint, context));
			return new Response()
			{
				Result = receiptData
			};
		}

		[Transaction(ReadOnly = true)]
		public Response GetBillPayReceipt(long agentSessionId, long customerSessionId, long transactionId, bool isReprint, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<ReceiptData> receiptData = Mapper.Map<List<BizData.Receipt>, List<ReceiptData>>(ReceiptService.GetBillPayReceipt(agentSessionId, customerSessionId, transactionId, isReprint, context));
			return new Response()
			{
				Result = receiptData
			};
		}


		[Transaction(ReadOnly = true)]
		public Response GetDoddFrankReceipt(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<ReceiptData> receiptData = Mapper.Map<List<BizData.Receipt>, List<ReceiptData>>(ReceiptService.GetDoddFrankReceipt(agentSessionId, customerSessionId, transactionId, context));
			return new Response()
			{
				Result = receiptData
			};
		}
		[Transaction(ReadOnly = true)]
		public Response GetCheckDeclinedReceiptData(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<ReceiptData> receiptData = Mapper.Map<List<ReceiptData>>(ReceiptService.GetCheckDeclinedReceiptData(agentSessionId, customerSessionId, transactionId, context));
			return new Response()
			{
				Result = receiptData
			};
		}
		[Transaction(ReadOnly = true)]
		public Response GetSummaryReceipt(long customerSessionId, long cartId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(mgiContext.CustomerSessionId, Convert.ToString(cartId), "GetSummaryReceipt", AlloyLayerName.SERVICE,
				ModuleName.Receipt, "Begin GetSummaryReceipt - MGI.Channel.DMS.Server.Impl.ReceiptEngine",
				context);
			#endregion

			MGI.Biz.Partner.Data.ShoppingCart shoppingCart = BIZShoppingCartService.GetCartById(customerSessionId, cartId, context);


			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<MGI.Biz.Partner.Data.ShoppingCart>(mgiContext.CustomerSessionId, shoppingCart, "GetSummaryReceipt", AlloyLayerName.SERVICE,
				ModuleName.Receipt, "End GetSummaryReceipt - MGI.Channel.DMS.Server.Impl.ReceiptEngine",
				context);
			#endregion

			List<ReceiptData> receiptData = Mapper.Map<List<BizData.Receipt>, List<ReceiptData>>(ReceiptService.GetSummaryReceipt(shoppingCart, customerSessionId, context));
			return new Response()
			{
				Result = receiptData
			};
		}

		[Transaction(ReadOnly = true)]
		public Response GetSummaryReceipt(long agentSessionId, long customerSessionId, long transactionId, string transactiontype, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<ReceiptData> receiptData = Mapper.Map<List<BizData.Receipt>, List<ReceiptData>>(ReceiptService.GetSummaryReceipt(agentSessionId, customerSessionId, transactionId, transactiontype, context));
			return new Response()
			{
				Result = receiptData
			};
		}

		/// <summary>
		/// US1800 Referral promotions – Free check cashing to referrer and referee 
		/// Added new method to Get Coupon Receipt for the ChannelPartner
		/// </summary>
		/// <param name="customerSessionId"></param>		
		/// <returns></returns>
		[Transaction(ReadOnly = true)]
		public Response GetCouponCodeReceipt(long customerSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			List<ReceiptData> receiptData = Mapper.Map<List<BizData.Receipt>, List<ReceiptData>>(ReceiptService.GetCouponReceipt(customerSessionId, context));
			return new Response()
			{
				Result = receiptData
			};
		}
	}
}
