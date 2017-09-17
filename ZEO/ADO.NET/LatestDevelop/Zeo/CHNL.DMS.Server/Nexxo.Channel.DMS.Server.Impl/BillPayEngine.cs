using System;
using System.Collections.Generic;
using AutoMapper;
using BizBillPayService = MGI.Biz.BillPay.Contract.IBillPayService;
using MGI.Biz.Catalog.Contract;
using SvcBillPayService = MGI.Channel.DMS.Server.Contract.IBillPayService;
using MGI.Channel.DMS.Server.Data;
using BizBillPayment = MGI.Biz.BillPay.Data.BillPayment;
using CoreCustomerSession = MGI.Core.Partner.Data.CustomerSession;
using Spring.Transaction.Interceptor;
using bizCustomer = MGI.Biz.Customer.Data.Customer;
using System.Linq;
using SharedData = MGI.Channel.Shared.Server.Data;
using dmsCashierResponse = MGI.Channel.DMS.Server.Data.CashierDetails;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.DMS.Server.Impl
{
    public partial class DesktopEngine : SvcBillPayService
    {
        public IProductCatalog ProductCatalog { get; set; }
        public BizBillPayService BizBillPayService { get; set; }

        private const int MAX_COUNT = 6;
        /// <summary>	
        /// 
        /// </summary>
        [Transaction()]
        internal static void BillPayConverter()
        {
            // Outputs
            Mapper.CreateMap<MGI.Core.Catalog.Data.Product, SharedData.Product>();
            Mapper.CreateMap<MGI.Biz.BillPay.Data.Product, SharedData.Product>()
                .ForMember(s => s.ProductName, d => d.MapFrom(c => c.BillerName));

            Mapper.CreateMap<MGI.Biz.Catalog.Data.Product, SharedData.Product>()
                .ForMember(s => s.ProductName, d => d.MapFrom(c => c.BillerName))
                .ForMember(s => s.Id, d => d.MapFrom(c => c.ProductId));

            Mapper.CreateMap<MGI.Biz.Catalog.Data.Presentment, MGI.Channel.Shared.Server.Data.Presentment>();

            Mapper.CreateMap<MGI.Biz.BillPay.Data.Location, SharedData.BillerLocation>();
            Mapper.CreateMap<MGI.Biz.BillPay.Data.CardInfo, SharedData.CardInfo>();
            Mapper.CreateMap<SharedData.CardInfo, MGI.Biz.BillPay.Data.CardInfo>();
			Mapper.CreateMap<MGI.Biz.BillPay.Data.BillPayLocation, SharedData.BillPayLocation>()
				.ForMember(x => x.BillerLocation, s=>s.MapFrom(c=> c.Location));
        }

        #region IBillPayService Impl

        #region Shared Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="channelPartnerID"></param>
        /// <param name="locationRegionID"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = true)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Search billers")]
		public Response GetBillers(long customerSessionId, long channelPartnerID, string searchTerm, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            Response response = new Response();

			var billers = SharedEngine.GetBillers(customerSessionId, channelPartnerID, searchTerm, context);

            if (billers != null)
            {
                int billerCount = billers.Count > MAX_COUNT ? MAX_COUNT : billers.Count;

                for (int i = 0; i < billerCount; i++)
                {
                    if(!string.IsNullOrEmpty(billers[i].BillerCode))
                    {
                        billers[i].ProductName = billers[i].ProductName + "/" + billers[i].BillerCode;
                    }
                }
                    response.Result = billers.Select(x => x.ProductName).Take(MAX_COUNT).ToList();
            }
            return response;
        }

        /// <summary>
        /// 
        /// </summary> 
		/// <param name="alloyId"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = true)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Get frequent billers for customer")]
		public Response GetFrequentBillers(long customerSessionId, long alloyId, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);	
		    Response response = new Response();
            response.Result = SharedEngine.GetFrequentBillers(customerSessionId, alloyId, context);
            return response;
        }

        [Transaction(ReadOnly = true)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Get biller message")]
		public Response GetBillerInfo(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            Response response = new Response();
			response.Result = SharedEngine.GetBillerInfo(customerSessionId, billerNameOrCode, context);
            return response;
        }

        [Transaction(ReadOnly = true)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Get BP fee")]
		public Response GetFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, SharedData.BillerLocation location, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            Response response = new Response();
			response.Result = SharedEngine.GetBillPaymentFee(customerSessionId, billerNameOrCode, accountNumber, amount, location, context);
            return response;
        }

        [Transaction(ReadOnly = true)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Get BP provider attributes")]
		public Response GetProviderAttributes(long customerSessionId, string billerNameOrCode, string location, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            Response response = new Response();
			response.Result = SharedEngine.GetBillPaymentProviderAttributes(customerSessionId, billerNameOrCode, location, context);
            return response;
        }

        /// <summary>
        /// Bill payment validation
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="billPayment"></param>
        /// <param name="context"></param>
        [Transaction()]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Validate BP request")]
		public Response ValidateBillPayment(long customerSessionId, SharedData.BillPayment billPayment, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            Response response = new Response();
			bizCustomer bizCustomer = BizCustomerService.GetCustomer(customerSessionId, context.AlloyId, context);
            context.IsAnonymous = _IsAnonymous(bizCustomer.Profile.FirstName, bizCustomer.Profile.LastName);
            response.Result = SharedEngine.ValidateBillPayment(customerSessionId, billPayment, context);
            return response;
        }

        /// <summary>
        /// Bill payment Adding into Shopping cart
        /// </summary>
        /// <param name="customerSessionID"></param>
        /// <param name="transactionID"></param>
        /// <param name="context"></param>
        [Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Begin BP transaction")]
		public Response StageBillPayment(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			SharedEngine.StageBillPayment(customerSessionId, transactionId, context);
            return new Response();
        }

        [Transaction(ReadOnly = true)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Get favorite biller")]
		public Response GetFavoriteBiller(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            Response response = new Response();
			response.Result = SharedEngine.GetFavoriteBiller(customerSessionId, billerNameOrCode, context);
            return response;
        }

		//Begin TA-191 Changes
		//       User Story Number: TA-191 | Server |   Developed by: Sunil Shetty     Date: 21.04.2015
		//       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. we are sending Customer session id and biller id.
		// the customer session id will get AlloyID/Pan ID and biller ID will help us to disable biller from tCustomerPreferedProducts table
		[Transaction(ReadOnly = true)]
		[DMSMethodAttribute(DMSFunctionalArea.BillPay, "Delete favorite biller")]
		public Response DeleteFavoriteBiller(long customerSessionId, long billerID, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			SharedEngine.DeleteFavoriteBiller(customerSessionId, billerID, context);
            return new Response();
		}
		//End TA-191 Changes
        #endregion

        #region DMS Methods

        /// <summary>
        /// New method to support CFP
        /// </summary>
        /// <param name="billerId"></param>
        /// <param name="customerSessionId"></param>
        /// <returns>Biller</returns>
        [Transaction(ReadOnly = true)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Get biller by ID")]
		public Response GetBiller(long customerSessionId, long billerID, MGIContext mgiContext)
        {
			#region AL-1014 Transactional Log User Story
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(billerID), "GetBiller", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"Begin GetBiller - MGI.Channel.DMS.Server.Impl.BillPayEngine",
				context);
            #endregion

            MGI.Biz.Catalog.Data.Product _product = ProductCatalog.Get(billerID);

            //mapping provider id from Partner
            MGI.Core.Partner.Contract.ProviderIds enumProviderId = (MGI.Core.Partner.Contract.ProviderIds)_product.ProviderId;
            _product.ProviderName = enumProviderId.ToString();

            SharedData.Product product = Mapper.Map<MGI.Biz.Catalog.Data.Product, SharedData.Product>(_product);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<SharedData.Product>(customerSessionId, product, "GetBiller", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"End GetBiller - MGI.Channel.DMS.Server.Impl.BillPayEngine",
				context);
            #endregion

            return new Response() { Result = product };
        }

        [Transaction(ReadOnly = true)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Get biller by name")]
		public Response GetBillerByName(long customerSessionId, long channelPartnerId, string billerNameOrCode, MGIContext mgiContext)
        {
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, billerNameOrCode, "GetBillerByName", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"Begin GetBillerByName - MGI.Channel.DMS.Server.Impl.BillPayEngine",
				context);
            #endregion

            MGI.Biz.Catalog.Data.Product _product = ProductCatalog.Get(channelPartnerId, billerNameOrCode);

            //mapping provider id from Partner
            MGI.Core.Partner.Contract.ProviderIds enumProviderId = (MGI.Core.Partner.Contract.ProviderIds)_product.ProviderId;
            _product.ProviderName = enumProviderId.ToString();

            SharedData.Product product = Mapper.Map<MGI.Biz.Catalog.Data.Product, SharedData.Product>(_product);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<SharedData.Product>(customerSessionId, product, "GetBillerByName", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"End GetBillerByName - MGI.Channel.DMS.Server.Impl.BillPayEngine",
				context);
            #endregion

            return new Response() { Result = product };
        }

        /// <summary>
        /// Get all products by channel partner
        /// </summary>
        /// <param name="ChannelPartnerID"></param>
        /// <param name="LocationRegionId"></param>
        /// <returns></returns>
        [Transaction(ReadOnly = true)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Get all billers")]
		public Response GetAllBillers(long customerSessionId, long channelPartnerID, Guid locationRegionID, MGIContext mgiContext)
        {
			#region AL-1014 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Channel Partner Id:" + Convert.ToString(channelPartnerID));
			details.Add("Location Id:" + Convert.ToString(locationRegionID));
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetAllBillers", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"Begin GetAllBillers - MGI.Channel.DMS.Server.Impl.BillPayEngine", context);
            #endregion

            int providerId = 401;
            var _products = ProductCatalog.GetAll((int)channelPartnerID, providerId);
            List<SharedData.Product> products = Mapper.Map<List<MGI.Biz.Catalog.Data.Product>, List<SharedData.Product>>(_products);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.ListInfo<SharedData.Product>(customerSessionId, products, "GetAllBillers", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"End GetAllBillers - MGI.Channel.DMS.Server.Impl.BillPayEngine", context);
            #endregion

            return new Response() { Result = products };
        }

		[Transaction]
		[DMSMethodAttribute(DMSFunctionalArea.BillPay, "CancelBillPayment")]
		public Response CancelBillPayment(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
			BizBillPayService.Cancel(customerSessionId, transactionId, context);
            return new Response();
		}

        

        [Transaction(ReadOnly = true)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Get BP locations")]
		public Response GetLocations(long customerSessionId, string billerName, string accountNumber, decimal amount, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            Response response = new Response();
			#region AL-1014 Transactional Log User Story
			List<string> billerDetails = new List<string>();
			billerDetails.Add("Biller Name:" + billerName);
			billerDetails.Add("Account Number:" + Convert.ToString(accountNumber));
			billerDetails.Add("Amount:" + Convert.ToString(amount));
			MongoDBLogger.ListInfo<string>(customerSessionId, billerDetails, "GetLocations", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"Begin GetLocations - MGI.Channel.DMS.Server.Impl.BillPayEngine",
				context);
            #endregion

            var locations = BizBillPayService.GetLocations(customerSessionId, billerName, accountNumber, amount, context);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<Biz.BillPay.Data.BillPayLocation>(customerSessionId, locations, "GetLocations", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"End GetLocations - MGI.Channel.DMS.Server.Impl.BillPayEngine",
				context);
            #endregion
            response.Result = Mapper.Map<SharedData.BillPayLocation>(locations);
            return response;
        }
        

        [Transaction(ReadOnly = true)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Get BP fee")]
		public Response GetBillPayFee(long customerSessionId, string providerName, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            Response response = new Response();
            response.Result = BizBillPayService.GetBillPayFee(customerSessionId, providerName, context);
            return response;
        }

        [Transaction(ReadOnly = true)]
        [DMSMethodAttribute(DMSFunctionalArea.BillPay, "Get card info")]
		public Response GetCardInfo(long customerSessionId, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            Response response = new Response();
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId,string.Empty, "GetCardInfo", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"Begin GetCardInfo - MGI.Channel.DMS.Server.Impl.BillPayEngine",
				context);
            #endregion

            var cardInfo = BizBillPayService.GetCardInfo(customerSessionId, context);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<Biz.BillPay.Data.CardInfo>(customerSessionId, cardInfo, "GetCardInfo", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"End GetCardInfo - MGI.Channel.DMS.Server.Impl.BillPayEngine",
				context);
            #endregion
            response .Result = Mapper.Map<SharedData.CardInfo>(cardInfo);
            return response;
        }

        private string GetTimeZone(long customerSessionId)
        {
            CoreCustomerSession customerSession = CustomerSessionSvc.Lookup(customerSessionId);
            return customerSession.AgentSession.Terminal.Location.TimezoneID;
        }

        /// <summary>
        /// This method will activate Biz.. Methods for Add Past Billers to DB. Added for User Story # US1646.
        /// </summary>
        /// <param name="customerSessionId">Session ID</param>
        /// <param name="cardNumber">Card Number</param>
        /// <param name="context">Context - Dictionary</param>
        [Transaction(ReadOnly = true)]
		public Response AddPastBillers(long customerSessionId, string cardNumber, MGIContext mgiContext)
        {
			mgiContext = _GetContext(customerSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            Response response = new Response();
            BizBillPayService.AddPastBillers(customerSessionId, cardNumber, context);
            return response;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="customerSessionId"></param>
		/// <returns></returns>
		[Transaction()]
		[DMSMethodAttribute(DMSFunctionalArea.BillPay, "Get Agent")]
		public Response GetAgent(long agentSessionId, MGIContext mgiContext)
		{
			mgiContext = _GetContextFromAgentSession(agentSessionId, mgiContext);
			MGI.Common.Util.MGIContext context = Mapper.Map<MGI.Common.Util.MGIContext>(mgiContext);
            Response response = new Response();
			response.Result = GetAgentDetails(Convert.ToString(agentSessionId), context);
            return response;
		}
        #endregion

        #endregion
    }
}
