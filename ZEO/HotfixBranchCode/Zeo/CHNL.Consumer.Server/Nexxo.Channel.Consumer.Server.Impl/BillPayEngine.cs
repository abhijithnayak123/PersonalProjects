using System.Collections.Generic;
using AutoMapper;
using MGI.Channel.Consumer.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using Spring.Transaction.Interceptor;
using MGI.Common.Util;
using serverProduct =  MGI.Channel.Shared.Server.Data.Product;
using MGI.Common.TransactionalLogging.Data;
using System;

namespace MGI.Channel.Consumer.Server.Impl
{
    public partial class ConsumerEngine : IBillPayService
    {
		public TLoggerCommon MongoDBLogger { private get; set; }
        #region BillPayment Data Mapper

        internal static void BillPayConverter()
        {
			Mapper.CreateMap<serverProduct, FavoriteBiller>()
            .ForMember(d => d.BillerName, o => o.MapFrom(s => s.ProductName))
            .ForMember(d=> d.BillerId, o => o.MapFrom( s => s.Id ));
        }

        #endregion

        #region IBillPayService Impl

        #region Biller Related Methods

        [Transaction(ReadOnly = true)]
		public List<serverProduct> GetBillers(long customerSessionId, long channelPartnerID, string searchTerm, MGIContext mgiContext)
        {
            return SharedEngine.GetBillers(customerSessionId, channelPartnerID, searchTerm, mgiContext);
        }

        [Transaction(ReadOnly = true)]
		public List<FavoriteBiller> GetFrequentBillers(long customerSessionId, long alloyId, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(alloyId), "GetFrequentBillers", AlloyLayerName.SERVICE,
				ModuleName.BillPayment, "Begin GetFrequentBillers - MGI.Channel.Consumer.Server.Impl.BillPayEngine",
				mgiContext);
            #endregion

            List<serverProduct> products = SharedEngine.GetFrequentBillers(customerSessionId, alloyId, mgiContext);

			List<FavoriteBiller> favBillers = Mapper.Map<List<serverProduct>, List<FavoriteBiller>>(products);

            #region AL-1014 Transactional Log User Story
            MongoDBLogger.ListInfo<FavoriteBiller>(customerSessionId, favBillers, "GetFrequentBillers", AlloyLayerName.SERVICE,
				ModuleName.BillPayment, "End GetFrequentBillers - MGI.Channel.Consumer.Server.Impl.BillPayEngine",
				mgiContext);
            #endregion

            return favBillers;

        }

        [Transaction()]
        public void AddFavoriteBiller(long customerSessionId, FavoriteBiller favoriteBiller, MGIContext mgiContext)
        {
            SharedEngine.AddFavoriteBiller(customerSessionId, favoriteBiller, mgiContext);
        }

        [Transaction()]
        public bool UpdateFavoriteBillerAccountNumber(long customerSessionId, long billerId, string accountNumber, MGIContext mgiContext)
        {
            return SharedEngine.UpdateFavoriteBillerAccountNumber(customerSessionId, billerId, accountNumber, mgiContext);
        }

        [Transaction()]
        public bool UpdateFavoriteBillerStatus(long customerSessionId, long billerId, bool status, MGIContext mgiContext)
        {
            return SharedEngine.UpdateFavoriteBillerStatus(customerSessionId, billerId, status, mgiContext);
        }

        [Transaction(ReadOnly = true)]
        public BillerInfo GetBillerInfo(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
        {
            return SharedEngine.GetBillerInfo(customerSessionId, billerNameOrCode, mgiContext);
        }

		//Begin TA-191 Changes
		//       User Story Number: TA-191 | Server |   Developed by: Sunil Shetty     Date: 21.04.2015
		//       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. we are sending Customer session id and biller id.
		// the customer session id will get AlloyID/Pan ID and biller ID will help us to disable biller from tCustomerPreferedProducts table
		[Transaction()]
		public void DeleteFavoriteBiller(long customerSessionId, long billerID, MGIContext mgiContext)
		{
			SharedEngine.DeleteFavoriteBiller(customerSessionId, billerID, mgiContext);
		}
        #endregion

        #region BillPay Trx Methods 

        [Transaction()]
        public BillFee GetBillPaymentFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, BillerLocation location, MGIContext mgiContext)
        {
            return SharedEngine.GetBillPaymentFee(customerSessionId, billerNameOrCode, accountNumber, amount, location, mgiContext);
        }

        [Transaction()]
        public List<Field> GetBillPaymentProviderAttributes(long customerSessionId, string billerNameOrCode, string location, MGIContext mgiContext)
        {
            return SharedEngine.GetBillPaymentProviderAttributes(customerSessionId, billerNameOrCode, location, mgiContext);
        }

        [Transaction()]
        public long ValidateBillPayment(long customerSessionId, BillPayment billPayment, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<BillPayment>(customerSessionId, billPayment, "ValidateBillPayment", AlloyLayerName.SERVICE,
				ModuleName.BillPayment, "Begin ValidateBillPayment - MGI.Channel.Consumer.Server.Impl.BillPayEngine",
				mgiContext);
            #endregion

            long transactionId=  SharedEngine.ValidateBillPayment(customerSessionId, billPayment, mgiContext);      
            
            SharedEngine.StageBillPayment(customerSessionId, transactionId, mgiContext);

            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionId), "ValidateBillPayment", AlloyLayerName.SERVICE,
				ModuleName.BillPayment, "End ValidateBillPayment - MGI.Channel.Consumer.Server.Impl.BillPayEngine",
				mgiContext);
            #endregion

            return transactionId;
        }

        [Transaction(ReadOnly = true)]
        public BillPayTransaction GetBillerLastTransaction(long customerSessionId, string billerCode, long alloyId, MGIContext mgiContext)
        {
			return SharedEngine.GetBillerLastTransaction(customerSessionId, billerCode, alloyId, mgiContext);
        }

        [Transaction(ReadOnly = true)]
        public FavoriteBiller GetFavoriteBiller(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
        {
            return SharedEngine.GetFavoriteBiller(customerSessionId, billerNameOrCode, mgiContext);
        }
        #endregion

        #endregion
    }
}
