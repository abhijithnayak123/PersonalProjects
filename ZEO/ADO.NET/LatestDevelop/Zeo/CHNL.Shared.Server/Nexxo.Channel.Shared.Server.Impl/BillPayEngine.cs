using System;
using System.Collections.Generic;
using AutoMapper;
using MGI.Channel.Shared.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using MGI.Core.Catalog.Data;
using CoreCatalogData = MGI.Core.Catalog.Data;
using Spring.Transaction.Interceptor;
using BizBillPayService = MGI.Biz.BillPay.Contract.IBillPayService;
using MGI.Biz.Catalog.Contract;
using BizBillPayment = MGI.Biz.BillPay.Data.BillPayment;
using Product = MGI.Channel.Shared.Server.Data.Product;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Channel.Shared.Server.Impl
{
    public partial class SharedEngine : IBillPayService
    {
        #region Injected Services

        public IProductCatalog ProductCatalog { get; set; }
        public BizBillPayService BizBillPayService { get; set; }

        #endregion

        #region BillPay Data Mapper

        /// <summary>
        /// 
        /// </summary>
        internal static void BillPayConverter()
        {
            Mapper.CreateMap<FavoriteBiller, Biz.BillPay.Data.FavoriteBiller>();

            Mapper.CreateMap<PartnerCatalog, Product>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.BillerName));

            Mapper.CreateMap<Biz.BillPay.Data.Product, Product>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.BillerName));
            Mapper.CreateMap<Biz.BillPay.Data.BillerInfo, BillerInfo>();
            Mapper.CreateMap<BillerLocation, Biz.BillPay.Data.Location>();
            Mapper.CreateMap<Biz.BillPay.Data.Fee, BillFee>();
            Mapper.CreateMap<Biz.BillPay.Data.DeliveryMethod, DeliveryMethod>();
            Mapper.CreateMap<Biz.BillPay.Data.Field, Field>();
            Mapper.CreateMap<BillPayment, BizBillPayment>();
            Mapper.CreateMap<Biz.BillPay.Data.FavoriteBiller, FavoriteBiller>();
            Mapper.CreateMap<Biz.BillPay.Data.BillPayTransaction, BillPayTransaction>();
        }

        #endregion

        #region IBillPayService Impl

        #region Biller Related Methods

        public List<Product> GetBillers(long customerSessionId, long channelPartnerID, string searchTerm, MGIContext mgiContext)
        {
			#region AL-1014 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Channel Partner Id:" + Convert.ToString(channelPartnerID));
			details.Add("Search Term:" + searchTerm);
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetBillers", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"Begin GetBillers - MGI.Channel.Shared.Server.Impl.BillPayEngine", mgiContext);
            #endregion

            List<PartnerCatalog> products = ProductCatalog.GetProducts(searchTerm, (int)channelPartnerID);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.ListInfo<PartnerCatalog>(customerSessionId, products, "GetBillers", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"End GetBillers - MGI.Channel.Shared.Server.Impl.BillPayEngine", mgiContext);
            #endregion

            return Mapper.Map<List<PartnerCatalog>, List<Product>>(products);

        }

		public List<Product> GetFrequentBillers(long customerSessionId, long alloyId, MGIContext mgiContext)
        {
			#region AL-1014 Transactional Log User Story	
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(alloyId), "GetFrequentBillers", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"Begin GetFrequentBillers - MGI.Channel.Shared.Server.Impl.BillPayEngine",
				mgiContext);
            #endregion

            List<Biz.BillPay.Data.Product> products = BizBillPayService.GetPreferredProducts(customerSessionId, alloyId, mgiContext);
			
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.ListInfo<Biz.BillPay.Data.Product>(customerSessionId, products, "GetFrequentBillers", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"End GetFrequentBillers - MGI.Channel.Shared.Server.Impl.BillPayEngine",
				mgiContext);
            #endregion

            return Mapper.Map<List<MGI.Biz.BillPay.Data.Product>, List<Product>>(products);
        }

        public void AddFavoriteBiller(long customerSessionId, FavoriteBiller favoriteBiller, MGIContext mgiContext)
        {
            BizBillPayService.AddFavoriteBiller(customerSessionId, Mapper.Map<Biz.BillPay.Data.FavoriteBiller>(favoriteBiller), mgiContext);
        }

        public bool UpdateFavoriteBillerAccountNumber(long customerSessionId, long billerId, string accountNumber, MGIContext mgiContext)
        {
            return BizBillPayService.UpdateFavoriteBillerAccountNumber(customerSessionId, billerId, accountNumber, mgiContext);
        }

        public bool UpdateFavoriteBillerStatus(long customerSessionId, long billerId, bool status, MGIContext mgiContext)
        {
            return BizBillPayService.UpdateFavoriteBillerStatus(customerSessionId, billerId, status, mgiContext);
        }

        public BillerInfo GetBillerInfo(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
        {
            return Mapper.Map<BillerInfo>(BizBillPayService.GetBillerInfo(customerSessionId, billerNameOrCode, mgiContext));
        }

        public FavoriteBiller GetFavoriteBiller(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
        {
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId,billerNameOrCode, "GetFavoriteBiller", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"Begin GetFavoriteBiller - MGI.Channel.Shared.Server.Impl.BillPayEngine",
				mgiContext);
            #endregion

            var favoriteBiller = BizBillPayService.GetFavoriteBiller(customerSessionId, billerNameOrCode, mgiContext);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<MGI.Biz.BillPay.Data.FavoriteBiller>(customerSessionId, favoriteBiller, "GetFavoriteBiller", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"End GetFavoriteBiller - MGI.Channel.Shared.Server.Impl.BillPayEngine",
				mgiContext);
            #endregion

            return Mapper.Map<Biz.BillPay.Data.FavoriteBiller, FavoriteBiller>(favoriteBiller);
        }

		//Begin TA-191 Changes
		//       User Story Number: TA-191 | Server |   Developed by: Sunil Shetty     Date: 21.04.2015
		//       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. we are sending Customer session id and biller id.
		// the customer session id will get AlloyID/Pan ID and biller ID will help us to disable biller from tCustomerPreferedProducts table
		public void DeleteFavoriteBiller(long customerSessionId, long billerID, MGIContext mgiContext)
		{
			BizBillPayService.DeleteFavoriteBiller(customerSessionId, billerID, mgiContext);
		}
        #endregion

        #region BillPay Trx Methods

        public BillFee GetBillPaymentFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, BillerLocation location, MGIContext mgiContext)
        {
			#region AL-1014 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Biller Name/Code:" + billerNameOrCode);
			details.Add("Account Number:" + accountNumber);
			details.Add("Amount:" + Convert.ToString(amount));
			details.Add("Biller Location:" + Convert.ToString(location));
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetBillPaymentFee", AlloyLayerName.SERVICE, ModuleName.BillPayment,
				"Begin GetBillPaymentFee - MGI.Channel.Shared.Server.Impl.BillPayEngine", mgiContext);
            #endregion
            var bizLocation = Mapper.Map<Biz.BillPay.Data.Location>(location);
            var fee = BizBillPayService.GetFee(customerSessionId, billerNameOrCode, accountNumber, amount, bizLocation, mgiContext);
			
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<Biz.BillPay.Data.Fee>(customerSessionId, fee, "GetBillPaymentFee", AlloyLayerName.SERVICE, ModuleName.BillPayment,
			"End GetBillPaymentFee - MGI.Channel.Shared.Server.Impl.BillPayEngine", mgiContext);
            #endregion

            return Mapper.Map<BillFee>(fee);
        }

        public List<Field> GetBillPaymentProviderAttributes(long customerSessionId, string billerNameOrCode, string location, MGIContext mgiContext)
        {
			#region AL-1014 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Biller Name/Code:" + billerNameOrCode);
			details.Add("Location:" + location);
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetBillPaymentProviderAttributes", AlloyLayerName.SERVICE, ModuleName.BillPayment,
			"Begin GetBillPaymentProviderAttributes - MGI.Channel.Shared.Server.Impl.BillPayEngine", mgiContext);
            #endregion

            var fields = BizBillPayService.GetProviderAttributes(customerSessionId, billerNameOrCode, location, mgiContext);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.ListInfo<Biz.BillPay.Data.Field>(customerSessionId, fields, "GetBillPaymentProviderAttributes", AlloyLayerName.SERVICE, ModuleName.BillPayment,
			"End GetBillPaymentProviderAttributes - MGI.Channel.Shared.Server.Impl.BillPayEngine", mgiContext);
            #endregion

            return Mapper.Map<List<Field>>(fields);
        }

        public long ValidateBillPayment(long customerSessionId, BillPayment billPayment, MGIContext mgiContext)
        {
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<BillPayment>(customerSessionId, billPayment, "ValidateBillPayment", AlloyLayerName.SERVICE, ModuleName.BillPayment,
			"Begin ValidateBillPayment - MGI.Channel.Shared.Server.Impl.BillPayEngine", mgiContext);
            #endregion

            BizBillPayment billPayRequest = Mapper.Map<BizBillPayment>(billPayment);						
            long transactionID =  BizBillPayService.Validate(customerSessionId, billPayRequest, mgiContext);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionID), "ValidateBillPayment", AlloyLayerName.SERVICE, ModuleName.BillPayment,
			"End ValidateBillPayment - MGI.Channel.Shared.Server.Impl.BillPayEngine", mgiContext);
            #endregion

            return transactionID;
        }

        public void StageBillPayment(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionId), "StageBillPayment", AlloyLayerName.SERVICE, ModuleName.BillPayment,
			"Begin StageBillPayment - MGI.Channel.Shared.Server.Impl.BillPayEngine", mgiContext);
            #endregion

            //Stage Billpay in CXN
            BizBillPayService.Add(customerSessionId, transactionId, mgiContext );

            //Add Billpay to shoppingcart 
            BIZShoppingCartService.AddBillPay(customerSessionId, transactionId, mgiContext);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<string>(customerSessionId, null, "StageBillPayment", AlloyLayerName.SERVICE, ModuleName.BillPayment,
			"End StageBillPayment - MGI.Channel.Shared.Server.Impl.BillPayEngine", mgiContext);
            #endregion
        }

		public BillPayTransaction GetBillerLastTransaction(long customerSessionId, string billerCode, long alloyId, MGIContext mgiContext)
        {
			#region AL-1014 Transactional Log User Story
			List<string> details = new List<string>();
			details.Add("Biller Code:" + billerCode);
			details.Add("Alloy Id:" + Convert.ToString(alloyId));
			MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetBillerLastTransaction", AlloyLayerName.SERVICE, ModuleName.BillPayment,
			"Begin GetBillerLastTransaction - MGI.Channel.Shared.Server.Impl.BillPayEngine", mgiContext);
            #endregion

            var billPayTransaction = BizBillPayService.GetBillerLastTransaction(customerSessionId, billerCode, alloyId, mgiContext);

			#region AL-1014 Transactional Log User Story
			MongoDBLogger.Info<Biz.BillPay.Data.BillPayTransaction>(customerSessionId, billPayTransaction, "GetBillerLastTransaction", AlloyLayerName.SERVICE, ModuleName.BillPayment,
			"End GetBillerLastTransaction - MGI.Channel.Shared.Server.Impl.BillPayEngine", mgiContext);
            #endregion

            return Mapper.Map<Biz.BillPay.Data.BillPayTransaction, BillPayTransaction>(billPayTransaction);
        }

        #endregion

        #endregion
    }
}
