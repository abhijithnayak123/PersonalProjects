using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NHibernate.Mapping;
using MGI.Biz.BillPay.Contract;
using MGI.Biz.BillPay.Data;
using MGI.Core.Catalog.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Cxn.BillPay.Contract;
using CXNData = MGI.Cxn.BillPay.Data;
using BizBillPayService = MGI.Biz.BillPay.Contract.IBillPayService;
using CatalogData = MGI.Core.Catalog.Data;
using CXEAccountService = MGI.Core.CXE.Contract.IAccountService;
using CXEBillPay = MGI.Core.CXE.Data.Transactions.Stage.BillPay;
using CXEBillPayService = MGI.Core.CXE.Contract.IBillPayService;
using CXEBillPaySetupService = MGI.Core.CXE.Contract.IBillPaySetupService;
using CXECustomer = MGI.Core.CXE.Data.Customer;
using PTNRCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using PTNRData = MGI.Core.Partner.Data;
using PTNRContract = MGI.Core.Partner.Contract;
using MGI.Cxn.BillPay.Data;
using MGI.TimeStamp;
using MGI.Cxn.Common.Processor.Util;
using Location = MGI.Core.Partner.Data.Location;
using MGI.Biz.Compliance.Contract;
using MGI.Biz.Compliance.Data;
using Spring.Transaction.Interceptor;
using MGI.Common.Util;
using MGI.Common.DataProtection.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Biz.BillPay.Impl
{
    public class BillPayServiceImpl : BizBillPayService
    {
        public BillPayServiceImpl()
        {
            Mapper.CreateMap<MGI.Core.Catalog.Data.MasterCatalog, MGI.Biz.BillPay.Data.Product>();

            Mapper.CreateMap<CXNData.Location, Data.Location>();
            Mapper.CreateMap<Data.Location, CXNData.Location>();

            Mapper.CreateMap<CXNData.Fee, Data.Fee>();
            Mapper.CreateMap<Data.Fee, CXNData.Fee>();

            Mapper.CreateMap<CXNData.Field, Data.Field>();
            Mapper.CreateMap<Data.Field, CXNData.Field>();

            Mapper.CreateMap<CXNData.DeliveryMethod, Data.DeliveryMethod>();
            Mapper.CreateMap<Data.DeliveryMethod, CXNData.DeliveryMethod>();

            Mapper.CreateMap<CXNData.BillPayTransaction, Data.BillPayTransaction>();
            Mapper.CreateMap<CXNData.BillPayAccount, Data.BillPayAccount>();

            Mapper.CreateMap<CXNData.CardInfo, Data.CardInfo>();
            Mapper.CreateMap<Data.CardInfo, CXNData.CardInfo>();

            Mapper.CreateMap<CXNData.BillerInfo, Data.BillerInfo>();

        }

        #region Public Properties

        public CXEBillPayService CXEBillPayService { private get; set; }
        public CXEAccountService CXEAccountService { private get; set; }
        public CXEBillPaySetupService CXEBillPaySetup { private get; set; }
        public IProductService ProductService { private get; set; }
        public MGI.Core.CXE.Contract.ICustomerService CustomerService { private get; set; }
        public ITransactionService<MGI.Core.Partner.Data.Transactions.BillPay> PRTNRBillSvc { private get; set; }
        public ICustomerSessionService SessionSvc { private get; set; }
        public PTNRCustomerService PTNRCustomerService { private get; set; }
        public IProcessorRouter ProcessorRouter { private get; set; }
        public PTNRContract.IChannelPartnerService CoreChannelPartnerService { private get; set; }
        public INexxoDataStructuresService PTNRDataStructureService { private get; set; }
        public PTNRContract.IFeeService FeeSvc { private get; set; }
        public ILimitService LimitService { private get; set; }
        public MGI.Core.Partner.Contract.ILocationCounterIdService LocationCounterIdService { private get; set; }
        public MGI.Core.Partner.Contract.ICustomerSessionCounterIdService CustomerSessionCounterIdService { private get; set; }
        public IDataProtectionService BPDataProtectionSvc { private get; set; }
        public bool IsHardCodedCounterId { get; set; }
        public TLoggerCommon MongoDBLogger { get; set; }
        #endregion

        #region Public Methods

        public long UpdateWUCardDetails(long customerSessionId, string WUGoldCardNumber, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, WUGoldCardNumber, "UpdateWUCardDetails", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "Begin UpdateWUCardDetails-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion

            CustomerSession customerSession = SessionSvc.Lookup(customerSessionId);
            CXECustomer cxeCustomer = CustomerService.Lookup(customerSession.Customer.CXEId);

            CXNData.BillPayRequest billPayRequest = new CXNData.BillPayRequest()
            {
                //todo: Need to pass once WU Gold card integration is complete
                CardNumber = WUGoldCardNumber,
                CustomerFirstName = cxeCustomer.FirstName,
                CustomerLastName = cxeCustomer.LastName,
                CustomerAddress1 = cxeCustomer.Address1,
                CustomerAddress2 = cxeCustomer.Address2,
                CustomerCity = cxeCustomer.City,
                CustomerState = cxeCustomer.State,
                CustomerZip = cxeCustomer.ZipCode,
                CustomerDateOfBirth = Convert.ToDateTime(cxeCustomer.DateOfBirth),
                CustomerPhoneNumber = cxeCustomer.Phone1,
                Amount = 0,
                ProductName = string.Empty,
                AccountNumber = string.Empty,
                Fee = 0,
                MetaData = new Dictionary<string, object>()
                               {
                                   {"Location",string.Empty},
                                   {"DeliveryCode",string.Empty},
                                   {"SessionCookie",string.Empty}
                               },
                CustomerMobileNumber = GetCustomerMobileNumber(cxeCustomer)
            };

            long cxeAccountId = _GetBillPayAccount((int)ProviderIds.WesternUnionBillPay, cxeCustomer, customerSession, billPayRequest, mgiContext);

            long cxnAccountId = customerSession.Customer.GetAccount((int)ProviderIds.WesternUnionBillPay).CXNId;

            IBillPayProcessor billPayProcessor = GetProcessor(customerSession.Customer.ChannelPartnerId, (int)ProviderIds.WesternUnionBillPay);

            billPayProcessor.UpdateCardDetails(cxnAccountId, WUGoldCardNumber, mgiContext, mgiContext.TimeZone);

            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(cxeAccountId), "UpdateWUCardDetails", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "End UpdateWUCardDetails-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            return cxeAccountId;
        }

        public long Validate(long customerSessionId, BillPayment billPayment, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<BillPayment>(customerSessionId, billPayment, "Validate", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "Begin Validate-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            decimal paymentAmount = billPayment.PaymentAmount;
            decimal fee = billPayment.Fee;
            CatalogData.MasterCatalog product = null;
            if (mgiContext.ProviderId == 0)
            {
                product = ProductService.Get(billPayment.billerID);
                mgiContext.ProviderId = product.ProviderId;
            }
            ChannelPartner channelPartner = CoreChannelPartnerService.ChannelPartnerConfig(mgiContext.ChannelPartnerId);
            decimal minimumAmount = LimitService.GetProductMinimum(channelPartner.ComplianceProgramName, TransactionTypes.BillPay, mgiContext);
            decimal maximumAmount = LimitService.CalculateTransactionMaximumLimit(customerSessionId, channelPartner.ComplianceProgramName, TransactionTypes.BillPay, mgiContext);

            if (paymentAmount < minimumAmount)
            {
                throw new BizComplianceLimitException(BizComplianceLimitException.BILL_PAY_MINIMUM_LIMIT_CHECK, minimumAmount);
            }
            if (paymentAmount > maximumAmount)
            {
                throw new BizComplianceLimitException(BizComplianceLimitException.BILL_PAY_LIMIT_EXCEEDED, maximumAmount);
            }

            long transactionID = 0;
            long cxnAccountID = 0;

            // Can session be null here? and cxeCustomer can also be null? if so, what to do
            CustomerSession session = SessionSvc.Lookup(customerSessionId);
            CXECustomer cxeCustomer = CustomerService.Lookup(session.Customer.CXEId);

            //Build CXN BillPayRequest
            CXNData.BillPayRequest billPayRequest = BuildCxnRequest(billPayment, cxeCustomer);

            // Stage.Create
            if (mgiContext.TrxId != 0)
            {
                transactionID = mgiContext.TrxId;
            }
            else
            {
                if (product == null)
                {
                    product = ProductService.Get(billPayment.billerID);
                }
                transactionID = _StageBillPayment(product, billPayment, cxeCustomer, session, billPayRequest, mgiContext);
            }

            MGI.Core.CXE.Data.Account cxeAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.BillPay);
            // CXN.Validate
            if (transactionID < 0)
            {
                // todo: throw exception
                return 0;
            }

            PTNRData.Transactions.BillPay billPayTrx = PRTNRBillSvc.Lookup(transactionID);
            mgiContext.TrxId = billPayTrx.CXNId;


            //TODO: Shouldn't account provider id be an enum or picked from elsewhere?
            PTNRData.Account ptnrCheckAcct = session.Customer.GetAccount((int)ProviderIds.WesternUnionBillPay);
            cxnAccountID = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccount.Id, mgiContext.ProviderId).CXNId;

            IBillPayProcessor billPayProcessor = GetProcessor(session.Customer.ChannelPartnerId, mgiContext.ProviderId);
            if (billPayProcessor != null)
            {

                //get the Gold card number from account and update to the request.
                CXNData.BillPayAccount billPayAccount = billPayProcessor.GetBillPayAccount(cxnAccountID);
                billPayRequest.CardNumber = billPayAccount.CardNumber;

                //US2028 CounterId Changes
                GetCustomerSessionCounterId(session, mgiContext.ProviderId, ref mgiContext);

                long cxnTransactionId = billPayProcessor.Validate(cxnAccountID, billPayRequest, mgiContext);

                CXNData.BillPayTransaction cxnTrx = billPayProcessor.GetTransaction(cxnTransactionId);

                if (cxnTrx != null)
                {
                    fee = cxnTrx.Fee;
                }

                TransactionStates txnState = (cxnTransactionId > 0) ? TransactionStates.Authorized : TransactionStates.AuthorizationFailed;

                if (mgiContext.TrxId != 0)
                {
                    CXEBillPayService.Update(transactionID, txnState, cxnTrx.ConfirmationNumber, fee);
                    PRTNRBillSvc.UpdateCXEStatus(transactionID, (int)txnState);
                    PRTNRBillSvc.UpdateCXNStatus(transactionID, cxnTransactionId, (int)txnState, paymentAmount, fee, cxnTrx.ConfirmationNumber);
                }
                else
                {
                    if (product == null)
                    {
                        product = ProductService.Get(billPayment.billerID);
                    }
                    _CreatePartnerTransactions(transactionID, cxnTransactionId, paymentAmount, fee, session, (int)txnState, product, cxeAccount, mgiContext.TimeZone);

                    if (!mgiContext.IsAnonymous)
                        _UpdatePreferredProducts(cxeCustomer, cxnTransactionId, product.Id, customerSessionId, mgiContext);

                }
            }
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionID), "Validate", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "End Validate-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            return transactionID;
        }

        public void Add(long customerSessionId, long transactionID, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionID), "Add", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "Begin Add-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            MGI.Core.Partner.Data.Transactions.BillPay cxnBPTrx = PRTNRBillSvc.Lookup(transactionID);

            int providerId = 0;

            if (mgiContext.ProviderId != 0)
            {
                providerId = mgiContext.ProviderId;
            }
            else
            {
                CatalogData.MasterCatalog product = ProductService.Get(cxnBPTrx.ProductId);
                providerId = product.ProviderId;
                mgiContext.ProviderId = providerId;
            }

            CustomerSession session = SessionSvc.Lookup(customerSessionId);

            IBillPayProcessor billPayProcessor = GetProcessor(session.Customer.ChannelPartnerId, providerId);

            //US2028 CounterId Changes
            GetCustomerSessionCounterId(session, providerId, ref mgiContext);

            billPayProcessor.Commit(cxnBPTrx.CXNId, mgiContext);

            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "Add", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "End Add-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
        }

        [Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
        public void Commit(long customerSessionId, long transactionID, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionID), "Commit", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "Begin Commit-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            MGI.Core.Partner.Data.Transactions.BillPay PARTNERBPTrx = PRTNRBillSvc.Lookup(transactionID);

            // Retrives product information
            int providerId = 0;

            CatalogData.MasterCatalog product = ProductService.Get(PARTNERBPTrx.ProductId);
            providerId = product.ProviderId;
            mgiContext.ProviderId = providerId;

            CustomerSession session = SessionSvc.Lookup(customerSessionId);

            IBillPayProcessor billPayProcessor = GetProcessor(session.Customer.ChannelPartnerId, providerId);

            //US2028 CounterId Changes
            GetCustomerSessionCounterId(session, providerId, ref mgiContext);

            long cxnTransactionId = billPayProcessor.Commit(PARTNERBPTrx.CXNId, mgiContext);

            RequestType requestStatus = (RequestType)Enum.Parse(typeof(RequestType), mgiContext.RequestType);

            if (requestStatus == RequestType.CANCEL)
                return;
            // Commit.Create			
            if (cxnTransactionId > 0)
            {
                CXNData.BillPayTransaction cxnTrx = billPayProcessor.GetTransaction(cxnTransactionId);

                CXEBillPayService.Update(transactionID, TransactionStates.Committed, cxnTrx.ConfirmationNumber);

                CXEBillPayService.Commit(transactionID);

                PRTNRBillSvc.UpdateCXEStatus(transactionID, (int)TransactionStates.Committed);

                PRTNRBillSvc.UpdateCXNStatus(transactionID, cxnTransactionId, (int)TransactionStates.Committed, cxnTrx.Amount, cxnTrx.Fee, cxnTrx.ConfirmationNumber);

            }
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "Commit", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "End Commit-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
        }

        public List<MGI.Biz.BillPay.Data.Product> GetPreferredProducts(long customerSessionId, long alloyId, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(alloyId), "GetPreferredProducts", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "Begin GetPreferredProducts-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion

            List<MGI.Biz.BillPay.Data.Product> products = new List<MGI.Biz.BillPay.Data.Product>();

            long[] productIDs = CXEBillPaySetup.GetPrefered(alloyId);

            var coreProducts = ProductService.GetProductsByIDs(productIDs);

            products = Mapper.Map<List<MGI.Biz.BillPay.Data.Product>>(coreProducts);

            #region AL-1014 Transactional Log User Story
            MongoDBLogger.ListInfo<MGI.Biz.BillPay.Data.Product>(customerSessionId, products, "GetPreferredProducts", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "End GetPreferredProducts-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion


            return products;
        }

        public Data.BillPayLocation GetLocations(long customerSessionId, string billerName, string accountNumber, decimal amount, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            List<string> details = new List<string>();
            details.Add("BillerName :" + billerName);
            details.Add("AccountNumber :" + accountNumber);
            details.Add("Amount :" + Convert.ToString(amount));

            MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetLocations", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "Begin GetLocations-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion

            CustomerSession session = SessionSvc.Lookup(customerSessionId);

            IBillPayProcessor billPayProcessor = GetProcessor(session.Customer.ChannelPartnerId, (int)ProviderIds.WesternUnionBillPay);

            long cxnAccountId = 0;
            long transactionId = 0;
            PTNRData.Account account = session.Customer.GetAccount((int)ProviderIds.WesternUnionBillPay);

            if (account != null)
                cxnAccountId = account.CXNId;

            mgiContext.ProviderId = (int)ProviderIds.WesternUnionBillPay;

            GetCustomerSessionCounterId(session, (int)ProviderIds.WesternUnionBillPay, ref mgiContext);
            CXECustomer cxeCustomer = CustomerService.Lookup(session.Customer.CXEId);

            BillPayment billPayment = BuildBillPayment(billerName, accountNumber, amount);

            //Build CXN BillPayRequest
            CXNData.BillPayRequest billPayRequest = BuildCxnRequest(billPayment, cxeCustomer);

            if (!mgiContext.Context.ContainsKey("BillPayRequest"))
            {
                mgiContext.Context.Add("BillPayRequest", billPayRequest);
            }

            CatalogData.MasterCatalog product = ProductService.Get(mgiContext.ChannelPartnerId, billerName);

            if (mgiContext.TrxId != 0)
            {
                transactionId = mgiContext.TrxId;
                CXEBillPayService.Update(transactionId, product.BillerName, accountNumber, amount);
            }
            else
            {
                transactionId = _StageBillPayment(product, billPayment, cxeCustomer, session, billPayRequest, mgiContext);
            }

            cxnAccountId = session.Customer.GetAccount(product.ProviderId).CXNId;
            mgiContext.CxnAccountId = cxnAccountId;
            //end 

            List<CXNData.Location> locations = billPayProcessor.GetLocations(billerName, accountNumber, amount, mgiContext);
            Data.BillPayLocation billPayLocation = new Data.BillPayLocation();
            billPayLocation.Location = Mapper.Map<List<Data.Location>>(locations);
            billPayLocation.TransactionId = transactionId;
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<Data.BillPayLocation>(customerSessionId, billPayLocation, "GetLocations", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "End GetLocations-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion

            return billPayLocation;
        }

        public Data.Fee GetFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, Data.Location location, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            List<string> details = new List<string>();
            details.Add("BillerNameorCode : " + billerNameOrCode);
            details.Add("AccountNumber : " + accountNumber);
            details.Add("Amount : " + Convert.ToString(amount));
            details.Add("Location : " + Convert.ToString(location));

            MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetFee", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                       "Begin GetFee-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            long transactionId = 0;

            CustomerSession customerSession = SessionSvc.Lookup(customerSessionId);

            CXECustomer cxeCustomer = CustomerService.Lookup(customerSession.Customer.CXEId);

            CXNData.BillPayRequest billPayRequest = GetBillPayRequest(cxeCustomer, amount);


            CatalogData.MasterCatalog product = ProductService.Get(mgiContext.ChannelPartnerId, billerNameOrCode);

            BillPayment billPayment = new BillPayment()
            {
                BillerName = product.BillerName,
                AccountNumber = accountNumber,
                PaymentAmount = amount
            };

            if (mgiContext.TrxId != 0)
            {
                transactionId = mgiContext.TrxId;
                //first time transaction is created in tTxn_BillPay_Stage table in get location method but transaction is not created in
                // tTxn_BillPay table. so it will throw the exception, so that added exception.
                try
                {
                    //MGI.Core.Partner.Data.Transactions.BillPay PARTNERBPTrx = PRTNRBillSvc.Lookup(transactionId);
                    //mgiContext["CxnTransactionId"] = PARTNERBPTrx.CXNId;
                    CXEBillPayService.Update(transactionId, product.BillerName, accountNumber, amount);
                }
                catch { }
            }
            else
            {
                transactionId = _StageBillPayment(product, billPayment, cxeCustomer, customerSession, billPayRequest, mgiContext);
            }

            MGI.Core.CXE.Data.Account cxeAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.BillPay);

            long cxnAccountId = customerSession.Customer.GetAccount(product.ProviderId).CXNId;

            string timezone = mgiContext.TimeZone;

            mgiContext.Context.AddIfNotExist("BillPayRequest", billPayRequest);
            mgiContext.CxnAccountId = cxnAccountId;
            mgiContext.ProviderId = product.ProviderId;
            mgiContext.BillerCode = product.BillerCode;

            CXNData.Location cxnLocation = Mapper.Map<CXNData.Location>(location);

            IBillPayProcessor billPayProcessor = GetProcessor(customerSession.Customer.ChannelPartnerId, product.ProviderId);

            GetCustomerSessionCounterId(customerSession, (int)ProviderIds.WesternUnionBillPay, ref mgiContext);
            CXNData.Fee fee = billPayProcessor.GetFee(billerNameOrCode, accountNumber, amount, cxnLocation, mgiContext);

            // Stage.Update
            var txnState = TransactionStates.Initiated;

            decimal dummyFee = 0M;

            if (fee != null && fee.TransactionId > 0)
            {
                CXEBillPayService.Update(transactionId, txnState, timezone, dummyFee);

                _CreatePartnerTransactions(transactionId, fee.TransactionId, amount, dummyFee, customerSession, (int)txnState, product, cxeAccount, timezone);

                // Adding to favorite billers list
                _UpdatePreferredProducts(cxeCustomer, fee.TransactionId, product.Id, customerSessionId, mgiContext);

                fee.TransactionId = transactionId;
            }
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<CXNData.Fee>(customerSessionId, fee, "GetFee", AlloyLayerName.BIZ, ModuleName.BillPayment,
                           "End GetFee-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion

            return Mapper.Map<Data.Fee>(fee);
        }

        public Data.BillerInfo GetBillerInfo(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, billerNameOrCode, "GetBillerInfo", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "Begin GetBillerInfo-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            CustomerSession session = SessionSvc.Lookup(customerSessionId);
            CatalogData.MasterCatalog product = ProductService.Get(mgiContext.ChannelPartnerId, billerNameOrCode);
            if (string.IsNullOrWhiteSpace(mgiContext.BillerCode))
                mgiContext.BillerCode = product.BillerCode;

            IBillPayProcessor billPayProcessor = GetProcessor(session.Customer.ChannelPartnerId, product.ProviderId);

            GetCustomerSessionCounterId(session, (int)ProviderIds.WesternUnionBillPay, ref mgiContext);
            CXNData.BillerInfo cxnCardInfo = billPayProcessor.GetBillerInfo(billerNameOrCode, mgiContext);
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<CXNData.BillerInfo>(customerSessionId, cxnCardInfo, "GetBillerInfo", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "End GetBillerInfo-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            return Mapper.Map<Data.BillerInfo>(cxnCardInfo);
        }

        public List<Data.Field> GetProviderAttributes(long customerSessionId, string billerNameOrCode, string location, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            List<string> details = new List<string>();
            details.Add("BillerNameOrCode :" + billerNameOrCode);
            details.Add("Location :" + location);

            MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetProviderAttributes", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "Begin GetProviderAttributes-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            CustomerSession session = SessionSvc.Lookup(customerSessionId);

            CatalogData.MasterCatalog product = ProductService.Get(mgiContext.ChannelPartnerId, billerNameOrCode);

            IBillPayProcessor billPayProcessor = GetProcessor(session.Customer.ChannelPartnerId, product.ProviderId);
            //US2028 CounterId Changes
            GetCustomerSessionCounterId(session, (int)ProviderIds.WesternUnionBillPay, ref mgiContext);
            //long transactionID = 0;
            //if (mgiContext.ContainsKey("TrxId"))
            //{
            //	transactionID = Convert.ToInt64(mgiContext["TrxId"]);
            //	MGI.Core.Partner.Data.Transactions.BillPay PARTNERBPTrx = PRTNRBillSvc.Lookup(transactionID);
            //	mgiContext["CxnTransactionId"] = PARTNERBPTrx.CXNId;
            //}

            List<CXNData.Field> fields = billPayProcessor.GetProviderAttributes(billerNameOrCode, location, mgiContext);
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.ListInfo<CXNData.Field>(customerSessionId, fields, "GetProviderAttributes", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "End GetProviderAttributes-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            return Mapper.Map<List<Data.Field>>(fields);
        }

        public FavoriteBiller GetFavoriteBiller(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, billerNameOrCode, "GetFavoriteBiller", AlloyLayerName.BIZ, ModuleName.BillPayment,
                             "Begin GetFavoriteBiller-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion


            FavoriteBiller favoriteBiller = new FavoriteBiller();

            CustomerSession customerSession = SessionSvc.Lookup(customerSessionId);

            var coreProduct = ProductService.Get(mgiContext.ChannelPartnerId, billerNameOrCode);

            var preferredProduct = CXEBillPaySetup.Get(coreProduct.Id, customerSession.Customer.CXEId);

            PTNRContract.ProviderIds enumProviderId = (PTNRContract.ProviderIds)coreProduct.ProviderId;

            if (preferredProduct != null)
            {

                favoriteBiller = new FavoriteBiller()
                {
                    AccountNumber = Decrypt(preferredProduct.AccountNumber),
                    BillerName = coreProduct.BillerName,
                    ChannelPartnerId = coreProduct.ChannelPartnerId,
                    ProviderId = coreProduct.ProviderId,
                    BillerId = preferredProduct.ProductId.ToString(),
                    TenantId = preferredProduct.TenantId,
                    ProviderName = enumProviderId.ToString(),
                    BillerCode = coreProduct.BillerCode
                };
            }
            else
            {
                favoriteBiller = new FavoriteBiller() { BillerId = Convert.ToString(coreProduct.Id), ProviderName = enumProviderId.ToString() };
            }
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<FavoriteBiller>(customerSessionId, favoriteBiller, "GetFavoriteBiller", AlloyLayerName.BIZ, ModuleName.BillPayment,
                            "End GetFavoriteBiller-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion

            return favoriteBiller;
        }

        public Data.BillPayTransaction GetTransaction(long customerSessionId, long ptnrTrxId, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(ptnrTrxId), "GetTransaction", AlloyLayerName.BIZ, ModuleName.BillPayment,
                             "Begin GetTransaction-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            CustomerSession session = SessionSvc.Lookup(customerSessionId);

            MGI.Core.Partner.Data.Transactions.BillPay PARTNERBPTrx = PRTNRBillSvc.Lookup(ptnrTrxId);

            // Retrives product information
            CatalogData.MasterCatalog product = ProductService.Get(PARTNERBPTrx.ProductId);

            IBillPayProcessor billPayProcessor = GetProcessor(session.Customer.ChannelPartnerId, product.ProviderId);

            CXNData.BillPayTransaction cxnTrx = billPayProcessor.GetTransaction(PARTNERBPTrx.CXNId);

            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<CXNData.BillPayTransaction>(customerSessionId, cxnTrx, "GetTransaction", AlloyLayerName.BIZ, ModuleName.BillPayment,
                             "End GetTransaction-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            return Mapper.Map<Data.BillPayTransaction>(cxnTrx);
        }

        public decimal GetBillPayFee(long customerSessionId, string providerName, MGIContext mgiContext)
        {
            return FeeSvc.GetBillPayFee(providerName, mgiContext.ChannelPartnerId, mgiContext);
        }

        public Data.CardInfo GetCardInfo(long customerSessionId, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetCardInfo", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                         "Begin GetCardInfo-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            Data.CardInfo cardInfo = null;

            CustomerSession customerSession = SessionSvc.Lookup(customerSessionId);

            CXECustomer cxeCustomer = CustomerService.Lookup(customerSession.Customer.CXEId);

            MGI.Core.CXE.Data.Account cxeAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.BillPay);

            //Todo following logic has to be varified by opteamix Billpay team
            long cxnAccountID = 0;

            PTNRData.Account account = PTNRCustomerService.Lookup(cxeCustomer.Id).GetAccount((int)ProviderIds.WesternUnionBillPay);

            if (cxeAccount != null && account != null)
            {
                //long cxnAccountID = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccount.Id).CXNId;
                cxnAccountID = account.CXNId;

                // Hard coded for the time being as we don't have provider mapping with 
                //channel partner persists on database. This is a word around till we fix this issue.

                int providerId = (int)ProviderIds.WesternUnionBillPay;
                IBillPayProcessor billPayProcessor = GetProcessor(customerSession.Customer.ChannelPartnerId, providerId);

                //get the Gold card number from account and update to the request.
                CXNData.BillPayAccount billPayAccount = billPayProcessor.GetBillPayAccount(cxnAccountID);

                if (!string.IsNullOrWhiteSpace(billPayAccount.CardNumber))
                {
                    //US2028 CounterId Changes
                    GetCustomerSessionCounterId(customerSession, providerId, ref mgiContext);
                    CXNData.CardInfo cxnCardInfo = billPayProcessor.GetCardInfo(billPayAccount.CardNumber, mgiContext);
                    cardInfo = Mapper.Map<Data.CardInfo>(cxnCardInfo);
                }
            }
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<MGI.Biz.BillPay.Data.CardInfo>(customerSessionId, cardInfo, "GetCardInfo", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                        "End GetCardInfo-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            return cardInfo;
        }

        /// <summary>
        /// This concrete method is added to add the Past Billers for a particular customer into tCustomerPreferedProducts in CXE DB for User Story # US1646.
        /// </summary>
        /// <param name="customerSessionId">Session ID</param>
        /// <param name="cardNumber">Card Number</param>
        /// <param name="mgiContext">Context</param>
        public void AddPastBillers(long customerSessionId, string cardNumber, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, cardNumber, "AddPastBillers", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "Begin AddPastBillers-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            CatalogData.MasterCatalog product = null;
            CustomerSession session = SessionSvc.Lookup(customerSessionId);
            CXECustomer cxeCustomer = CustomerService.Lookup(session.Customer.CXEId);
            long channelPartnerId = 0;

            if (mgiContext.ChannelPartnerId != 0)
            {
                channelPartnerId = mgiContext.ChannelPartnerId;
            }
            else
            {
                ChannelPartner channelPartner = CoreChannelPartnerService.ChannelPartnerConfig(session.Customer.ChannelPartnerId);
                channelPartnerId = channelPartner.Id;
            }

            mgiContext.CxnAccountId = session.Customer.GetAccount((int)ProviderIds.WesternUnionBillPay).CXNId;


            IBillPayProcessor billPayProcessor = GetProcessor(session.Customer.ChannelPartnerId, (int)ProviderIds.WesternUnionBillPay);

            List<Biller> listBillers = new List<Biller>();

            if (!string.IsNullOrEmpty(cardNumber))
            {
                //US2028 CounterId Changes
                GetCustomerSessionCounterId(session, (int)ProviderIds.WesternUnionBillPay, ref mgiContext);
                listBillers = billPayProcessor.GetPastBillers(customerSessionId, cardNumber, mgiContext);
            }

            //If there are no billers in WU.
            if (listBillers != null)
            {
                // This is total number of responses from WU that is to be added to DMS.
                foreach (Biller biller in listBillers)
                {
                    // Retrives product information from the Business Name and Attention (debtor_account_number) from WU Card Lookup Request.
                    if (!string.IsNullOrEmpty(biller.Name) && !string.IsNullOrEmpty(biller.AccountNumber))
                    {
                        product = ProductService.Get(channelPartnerId, biller.Name);
                        if (product != null)
                            _UpdatePreferredProducts(cxeCustomer, biller.AccountNumber, product.Id, customerSessionId, mgiContext);
                    }
                }
            }
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "AddPastBillers", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "End AddPastBillers-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
        }

        public void AddFavoriteBiller(long customerSessionId, FavoriteBiller favoriteBiller, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<FavoriteBiller>(customerSessionId, favoriteBiller, "AddFavoriteBiller", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "Begin AddFavoriteBiller-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            long billerID = Convert.ToInt64(favoriteBiller.BillerId);
            PTNRData.CustomerSession session = SessionSvc.Lookup(customerSessionId);
            CustomerPreferedProduct product = CXEBillPaySetup.Get(billerID, session.Customer.Id);
            if (product == null)
            {
                product = new CustomerPreferedProduct()
                {
                    ProductId = billerID,
                    AlloyID = session.Customer.Id,
                    AccountNumber = Encrypt(favoriteBiller.AccountNumber),
                    BillerCode = favoriteBiller.BillerCode,
                    DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(session.TimezoneID),
                    DTServerCreate = DateTime.Now,
                    Enabled = true,
                };

                CXEBillPaySetup.Create(product);
            }

            else
            {
                product.ProductId = billerID;
                product.AccountNumber = Encrypt(favoriteBiller.AccountNumber);
                product.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(session.TimezoneID);
                product.DTServerLastModified = DateTime.Now;
                product.Enabled = true;
                CXEBillPaySetup.Update(product, session.TimezoneID);
            }
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "AddFavoriteBiller", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "End AddFavoriteBiller-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion

        }


        public bool UpdateFavoriteBillerAccountNumber(long customerSessionId, long billerId, string accountNumber, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            List<string> details = new List<string>();
            details.Add("billerId : " + Convert.ToString(billerId));
            details.Add("accountNumber : " + accountNumber);
            MongoDBLogger.ListInfo<string>(customerSessionId, details, "UpdateFavoriteBillerAccountNumber", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                      "Begin UpdateFavoriteBillerAccountNumber-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            PTNRData.CustomerSession session = SessionSvc.Lookup(customerSessionId);
            CustomerPreferedProduct product = CXEBillPaySetup.Get(billerId, session.Customer.Id);
            product.AccountNumber = Encrypt(accountNumber);
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "UpdateFavoriteBillerAccountNumber", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                    "End UpdateFavoriteBillerAccountNumber-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            return CXEBillPaySetup.Update(product, mgiContext.TimeZone);

        }

        public bool UpdateFavoriteBillerStatus(long customerSessionId, long billerId, bool status, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            List<string> details = new List<string>();
            details.Add("BillerId : " + Convert.ToString(billerId));
            details.Add("Status : " + Convert.ToString(status));
            MongoDBLogger.ListInfo<string>(customerSessionId, details, "UpdateFavoriteBillerStatus", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                    "Begin UpdateFavoriteBillerStatus-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            PTNRData.CustomerSession session = SessionSvc.Lookup(customerSessionId);
            CustomerPreferedProduct product = CXEBillPaySetup.Get(billerId, session.Customer.Id);
            product.Enabled = status;
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "UpdateFavoriteBillerStatus", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                   "End UpdateFavoriteBillerStatus-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            return CXEBillPaySetup.Update(product, mgiContext.TimeZone);
        }

        public Data.BillPayTransaction GetBillerLastTransaction(long customerSessionId, string billerCode, long alloyId, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            List<string> details = new List<string>();
            details.Add("billerCode : " + billerCode);
            details.Add("AlloyId : " + Convert.ToString(alloyId));

            MongoDBLogger.ListInfo<string>(customerSessionId, details, "GetBillerLastTransaction", AlloyLayerName.BIZ, ModuleName.BillPayment,
                             "Begin GetBillerLastTransaction-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            var PtnrCustomer = PTNRCustomerService.Lookup(alloyId);

            CatalogData.MasterCatalog product = ProductService.Get(billerCode, mgiContext.ChannelPartnerId);

            var account = PtnrCustomer.GetAccount(product.ProviderId);
            long cxnAccountId = 0L;

            if (account != null)
                cxnAccountId = account.CXNId;
            else
                return null;

            IBillPayProcessor billPayProcessor = GetProcessor(PtnrCustomer.ChannelPartnerId, product.ProviderId);

            CXNData.BillPayTransaction cxnTrx = billPayProcessor.GetBillerLastTransaction(billerCode, cxnAccountId, mgiContext);
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<CXNData.BillPayTransaction>(customerSessionId, cxnTrx, "GetBillerLastTransaction", AlloyLayerName.BIZ, ModuleName.BillPayment,
                          "End GetBillerLastTransaction-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion

            return Mapper.Map<Data.BillPayTransaction>(cxnTrx);
        }

        public void Cancel(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionId), "Cancel", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                   "Begin Cancel-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            MGI.Core.Partner.Data.Transactions.BillPay PARTNERBPTrx = PRTNRBillSvc.Lookup(transactionId);

            // Retrives product information
            CatalogData.MasterCatalog product = ProductService.Get(PARTNERBPTrx.ProductId);

            CXEBillPayService.Update(PARTNERBPTrx.CXEId, TransactionStates.Canceled, mgiContext.TimeZone);

            PRTNRBillSvc.UpdateStates(PARTNERBPTrx.Id, (int)TransactionStates.Canceled, (int)TransactionStates.Canceled);
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "Cancel", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                   "End Cancel-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
        }

        //Begin TA-191 Changes
        //       User Story Number: TA-191 | Biz |   Developed by: Sunil Shetty     Date: 21.04.2015
        //       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. we are sending Customer session id and biller id.
        // the customer session id will get AlloyID/Pan ID and biller ID will help us to disable biller from tCustomerPreferedProducts table
        public void DeleteFavoriteBiller(long customerSessionId, long billerID, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(billerID), "DeleteFavoriteBiller", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                   "Begin DeleteFavoriteBiller-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            PTNRData.CustomerSession session = SessionSvc.Lookup(customerSessionId);
            CustomerPreferedProduct customerPreferedProduct = CXEBillPaySetup.Get(billerID, session.Customer.CXEId);
            customerPreferedProduct.Enabled = false;
            CXEBillPaySetup.Update(customerPreferedProduct, mgiContext.TimeZone);
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "DeleteFavoriteBiller", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                   "End DeleteFavoriteBiller-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
        }

        public void UpdateTransactionStatus(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionId), "UpdateTransactionStatus", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                   "Begin UpdateTransactionStatus-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
            MGI.Core.Partner.Data.Transactions.BillPay partnerBillPay = PRTNRBillSvc.Lookup(transactionId);

            CXEBillPayService.Update(partnerBillPay.CXEId, TransactionStates.Failed, mgiContext.TimeZone);

            PRTNRBillSvc.UpdateStates(partnerBillPay.Id, (int)TransactionStates.Failed, (int)TransactionStates.Failed);
            #region AL-1014 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "UpdateTransactionStatus", AlloyLayerName.BIZ, ModuleName.BillPayment,
                                   "End UpdateTransactionStatus-MGI.Biz.BillPay.Impl.BillPayServiceImpl", mgiContext);
            #endregion
        }

        #endregion

        #region Private Methods

        private void _CreatePartnerTransactions(long transactionID, long cxnTransactionId, decimal paymentAmount,
          decimal fee, CustomerSession customerSession, int txnState, CatalogData.MasterCatalog product,
          MGI.Core.CXE.Data.Account cxeAccount, string timezone)
        {
            MGI.Core.Partner.Data.Transactions.BillPay billPay = null;

            // check transaction exists, if yes, update or create 
            try
            {
                billPay = PRTNRBillSvc.Lookup(transactionID);
            }
            catch { }

            if (billPay != null)
            {
                billPay.ProductId = product.Id;
                billPay.Amount = paymentAmount;
                billPay.CXNId = cxnTransactionId;
                PRTNRBillSvc.UpdateTransactionDetails(billPay);
            }
            else
            {
                billPay = new MGI.Core.Partner.Data.Transactions.BillPay
                {
                    Id = transactionID,
                    CXEId = transactionID,
                    CXNId = cxnTransactionId,
                    Amount = paymentAmount,
                    Fee = fee,
                    CustomerSession = customerSession,
                    CXEState = (int)txnState,
                    CXNState = (int)txnState,
                    DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),
                    DTServerCreate = DateTime.Now,
                    Description = product.Id.ToString(),
                    ProductId = product.Id,
                    Account = customerSession.Customer.FindAccountByCXEId(cxeAccount.Id, product.ProviderId)
                };
                PRTNRBillSvc.Create(billPay);
            }
        }



        private void _UpdatePreferredProducts(CXECustomer customer, long cxnTransactionID, long productID, long customerSessionId, MGIContext mgiContext)
        {
            // Retrives product information
            CatalogData.MasterCatalog catalog = ProductService.Get(productID);

            PTNRData.CustomerSession session = SessionSvc.Lookup(customerSessionId);
            long cxeAccountID = session.Customer.GetAccount(catalog.ProviderId).CXEId;
            //customer.GetAccount((int)Core.CXE.Data.AccountTypes.BillPay, 8).Id;
            long cxnAccountID = PTNRCustomerService.Lookup(customer.Id).FindAccountByCXEId(cxeAccountID, catalog.ProviderId).CXNId;

            IBillPayProcessor billPayProcessor = GetProcessor(session.Customer.ChannelPartnerId, catalog.ProviderId);

            CXNData.BillPayTransaction cxnTrx = billPayProcessor.GetTransaction(cxnTransactionID);
            string customerAccountNo = cxnTrx.AccountNumber;
            string tenantId = cxnTrx.MetaData != null && cxnTrx.MetaData.ContainsKey("TenantId") && cxnTrx.MetaData["TenantId"] != null
                                  ? Convert.ToString(cxnTrx.MetaData["TenantId"])
                                  : "";

            CXNData.BillPayAccount billPayAccount = billPayProcessor.GetBillPayAccount(cxnAccountID);

            // Get if the preferred product, if available.
            // need to create product irrespective of customer prefferred has or not.
            CustomerPreferedProduct product = CXEBillPaySetup.Get(productID, customer.Id);

            if (product == null)
            {
                product = new CustomerPreferedProduct()
               {
                   ProductId = productID,
                   AlloyID = customer.Id,
                   AccountNumber = Encrypt(customerAccountNo),
                   PhoneNumber = Convert.ToInt64(billPayAccount.ContactPhone),
                   //Changes for timestamp
                   DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
                   DTServerCreate = DateTime.Now,
                   DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
                   DTServerLastModified = DateTime.Now,
                   AccountDOB = customer.DateOfBirth,
                   TenantId = tenantId,
                   Enabled = true,
               };

                CXEBillPaySetup.Create(product);
            }
            else
            {
                product.ProductId = productID;
                product.AccountNumber = Encrypt(customerAccountNo);
                product.TenantId = tenantId;
                product.PhoneNumber = Convert.ToInt64(billPayAccount.ContactPhone);
                //Changes for timestamp
                product.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
                product.DTServerLastModified = DateTime.Now;
                product.AccountDOB = customer.DateOfBirth;
                //Need to pass timezone
                CXEBillPaySetup.Update(product, mgiContext.TimeZone);
            }
        }

        /// <summary>
        /// This is the new method added to add new billers from WU to tCustomerPreferedProducts in CXE DMS Database for User Story # US1646.
        /// </summary>
        /// <param name="customer">CXE Customer</param>
        /// <param name="customerAccountNo">Customer Account Number</param>
        /// <param name="productID">Product ID</param>
        /// <param name="customerSessionId">Customer Session ID</param>
        /// <param name="index">Receiver Index</param>
        private void _UpdatePreferredProducts(CXECustomer customer, string customerAccountNo, long productID, long customerSessionId, MGIContext mgiContext)
        {
            PTNRData.CustomerSession session = SessionSvc.Lookup(customerSessionId);
            long cxeAccountID = session.Customer.GetAccount((int)ProviderIds.WesternUnionBillPay).CXEId;
            long cxnAccountID = PTNRCustomerService.Lookup(customer.Id).FindAccountByCXEId(cxeAccountID).CXNId;

            IBillPayProcessor billPayProcessor = GetProcessor(session.Customer.ChannelPartnerId, (int)ProviderIds.WesternUnionBillPay);

            CXNData.BillPayAccount billPayAccount = billPayProcessor.GetBillPayAccount(cxnAccountID);

            #region This if condition is added for User Story # US1646. IMPORTANT METHOD TO SAVE/MODIFY tCustomerPreferedProducts Table.

            // This if condition is added for User Story # US1646. This will add the ReceiverIndex Number into DMS DB Table.
            if (productID > 0)
            {
                CustomerPreferedProduct productReceiver = CXEBillPaySetup.GetBillerReceiverIndex(productID, long.Parse(session.Customer.CXEId.ToString()));

                //If there are matching records for particular biller then update the biller record.
                if (productReceiver != null)
                {
                    productReceiver.ProductId = productID;
                    productReceiver.ReceiverIndexNo = productReceiver.ReceiverIndexNo;
                    productReceiver.AccountNumber = Encrypt(customerAccountNo);
                    productReceiver.PhoneNumber = Convert.ToInt64(billPayAccount.ContactPhone);
                    productReceiver.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
                    productReceiver.DTServerLastModified = DateTime.Now;
                    productReceiver.AccountDOB = customer.DateOfBirth;

                    CXEBillPaySetup.Update(productReceiver, mgiContext.TimeZone);
                }

                //If there are NO Matching records for particular receiver then add new biller record in tCustomerPreferedProducts Table.
                else
                {
                    productReceiver = new CustomerPreferedProduct()
                    {
                        ProductId = productID,

                        AlloyID = customer.Id,
                        // ReceiverIndexNo = index,
                        AccountNumber = Encrypt(customerAccountNo),
                        PhoneNumber = Convert.ToInt64(billPayAccount.ContactPhone),
                        DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
                        DTServerCreate = DateTime.Now,
                        DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
                        DTServerLastModified = DateTime.Now,
                        AccountDOB = customer.DateOfBirth,
                        Enabled = true,
                    };

                    CXEBillPaySetup.Create(productReceiver);
                }

                return;
            }

            #endregion

        }

        private long _StageBillPayment(CatalogData.MasterCatalog product, BillPayment billPayment, CXECustomer cxeCustomer, CustomerSession session, BillPayRequest billPayRequest, MGIContext mgiContext)
        {
            if (cxeCustomer != null)
            {
                long cxeAccountId = _GetBillPayAccount(product.ProviderId, cxeCustomer, session, billPayRequest, mgiContext);

                CXEBillPay billPay = new CXEBillPay()
                {
                    ProductId = (int)product.Id,
                    ProductName = billPayment.BillerName,
                    AccountNumber = Encrypt(billPayment.AccountNumber),
                    Amount = billPayment.PaymentAmount,
                    Fee = billPayment.Fee,
                    Account = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.BillPay),
                    Status = (int)TransactionStates.Pending,
                    //Changes for timestamp
                    DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
                    DTServerCreate = DateTime.Now,
                    DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
                    DTServerLastModified = DateTime.Now
                };
                return CXEBillPayService.Create(billPay);
            }
            else
            {
                return 0;
            }
        }

        private long _GetBillPayAccount(int providerId, CXECustomer cxeCustomer, CustomerSession session, BillPayRequest billPayRequest, MGIContext mgiContext)
        {
            long cxeAccountId = 0;

            MGI.Core.CXE.Data.Account cxeAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.BillPay);

            if (cxeAccount != null)
                cxeAccountId = cxeAccount.Id;

            //  Add  CXE Account
            if (cxeAccountId == 0)
                cxeCustomer.Accounts.Add(CXEAccountService.AddCustomerBillPayAccount(cxeCustomer));

            //Add to CXN and CustomerSession if not exists
            if (!session.Customer.IsAccountExists(cxeAccountId, providerId))
            {

                MGI.Core.CXE.Data.Account cxeBillPayAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.BillPay);

                IBillPayProcessor billPayProcessor = GetProcessor(session.Customer.ChannelPartnerId, providerId);

                // Add CXN Account, assumption is if CXEAccount is not there for Billpay then CXN would also not be there.
                long cxnAccountID = billPayProcessor.AddBillPayAccount(billPayRequest, mgiContext.TimeZone);

                // Create PTNR account , Map CXE and CXN Account ids here.
                //PTNRData.Customer pntrCustomer = PTNRCustomerService.Lookup(cxeCustomer.Id);
                //pntrCustomer.AddAccount((int)ProviderIds.WesternUnionBillPay, cxeBillPayAccount.Id, cxnAccountID);
                session.Customer.AddAccount(providerId, cxeBillPayAccount.Id, cxnAccountID);
                SessionSvc.Save(session);
                cxeAccountId = cxeBillPayAccount.Id;
            }
            return cxeAccountId;
        }

        private IBillPayProcessor GetProcessor(Guid channelPartnerId, int providerId)
        {

            ChannelPartner channelPartner = CoreChannelPartnerService.ChannelPartnerConfig(channelPartnerId);

            PTNRContract.ProviderIds enumProviderId = (PTNRContract.ProviderIds)providerId;

            return (IBillPayProcessor)ProcessorRouter.GetProcessor(channelPartner.Name, enumProviderId.ToString());
        }

        private IBillPayProcessor GetProcessor(Guid channelPartnerId)
        {
            // get the fund processor for the channel partner.
            ChannelPartner channelPartner = CoreChannelPartnerService.ChannelPartnerConfig(channelPartnerId);
            return (IBillPayProcessor)ProcessorRouter.GetProcessor(channelPartner.Name);
        }

        private BillPayRequest BuildCxnRequest(BillPayment billPayment, CXECustomer cxeCustomer)
        {

            NexxoIdType govtIDInfo = null;
            if (cxeCustomer.GovernmentId != null)
                govtIDInfo = PTNRDataStructureService.Find(cxeCustomer.ChannelPartnerId, cxeCustomer.GovernmentId.IdTypeId);

            MasterCountry masterCountry = PTNRDataStructureService.GetMasterCountryByCode(cxeCustomer.CountryOfBirth);

            return new BillPayRequest()
                       {
                           //Customer personal details 
                           CustomerFirstName = cxeCustomer.FirstName,
                           CustomerMiddleName = cxeCustomer.MiddleName,
                           CustomerLastName = cxeCustomer.LastName,
                           CustomerLastName2 = cxeCustomer.LastName2,
                           CustomerAddress1 = cxeCustomer.Address1,
                           CustomerAddress2 = cxeCustomer.Address2,
                           CustomerCity = cxeCustomer.City,
                           CustomerState = cxeCustomer.State,
                           CustomerZip = cxeCustomer.ZipCode,
                           SSN = cxeCustomer.SSN,
                           CustomerDateOfBirth = Convert.ToDateTime(cxeCustomer.DateOfBirth),
                           CustomerPhoneNumber = cxeCustomer.Phone1,
                           CustomerEmail = cxeCustomer.Email,
                           CustomerMobileNumber = GetCustomerMobileNumber(cxeCustomer),

                           //customer government details and other details
                           PrimaryIdType = govtIDInfo != null ? govtIDInfo.Name : string.Empty,
                           PrimaryIdNumber = govtIDInfo != null || cxeCustomer.GovernmentId != null ? cxeCustomer.GovernmentId.Identification : string.Empty,
                           PrimaryIdCountryOfIssue = govtIDInfo != null ? govtIDInfo.CountryId.Name : string.Empty,
                           PrimaryIdPlaceOfIssue = govtIDInfo != null && govtIDInfo.StateId != null ? govtIDInfo.StateId.Name : string.Empty,

                           PrimaryIdCountryOfIssueCode = govtIDInfo != null ? govtIDInfo.CountryId.Abbr2 : string.Empty,
                           PrimaryIdPlaceOfIssueCode = govtIDInfo != null && govtIDInfo.StateId != null ? govtIDInfo.StateId.Abbr : string.Empty,

                           SecondIdType = "SSN",
                           SecondIdNumber = cxeCustomer.SSN,
                           SecondIdCountryOfIssue = "US",
                           Occupation = GetOccupation(cxeCustomer.EmploymentDetails),
                           CountryOfBirth = cxeCustomer.CountryOfBirth, // Abbr2
                           CountryOfBirthAbbr3 = masterCountry != null ? masterCountry.Abbr3 : null,
                           //Gateway specific
                           //todo: Need to pass once WU Gold card integration is complete
                           CardNumber = string.Empty,

                           //PhotoIdType = cx

                           //Trx specific details
                           Amount = billPayment.PaymentAmount,
                           ProductId = billPayment.billerID,
                           ProductName = billPayment.BillerName,
                           Fee = billPayment.Fee,
                           CouponCode = billPayment.CouponCode,
                           AccountNumber = billPayment.AccountNumber,

                           //Provider specific
                           MetaData = billPayment.MetaData,
                       };

        }

        private CXNData.BillPayRequest GetBillPayRequest(CXECustomer cxeCustomer, decimal amount)
        {
            //2080
            NexxoIdType govtIDInfo = null;
            if (cxeCustomer.GovernmentId != null)
                govtIDInfo = PTNRDataStructureService.Find(cxeCustomer.ChannelPartnerId, cxeCustomer.GovernmentId.IdTypeId);

            MasterCountry masterCountry = PTNRDataStructureService.GetMasterCountryByCode(cxeCustomer.CountryOfBirth);

            CXNData.BillPayRequest billPayRequest = new CXNData.BillPayRequest()
            {
                CustomerFirstName = cxeCustomer.FirstName,
                CustomerLastName = cxeCustomer.LastName,
                CustomerAddress1 = cxeCustomer.Address1,
                CustomerAddress2 = cxeCustomer.Address2,
                CustomerCity = cxeCustomer.City,
                CustomerState = cxeCustomer.State,
                CustomerZip = cxeCustomer.ZipCode,
                CustomerDateOfBirth = Convert.ToDateTime(cxeCustomer.DateOfBirth),
                CustomerPhoneNumber = cxeCustomer.Phone1,
                CustomerEmail = cxeCustomer.Email,

                PrimaryIdType = govtIDInfo != null ? govtIDInfo.Name : string.Empty,

                PrimaryIdNumber = govtIDInfo != null || cxeCustomer.GovernmentId != null ? cxeCustomer.GovernmentId.Identification : string.Empty,
                PrimaryIdCountryOfIssue = govtIDInfo != null ? govtIDInfo.CountryId.Name : string.Empty,
                PrimaryIdPlaceOfIssue = govtIDInfo != null && govtIDInfo.StateId != null ? govtIDInfo.StateId.Name : string.Empty,

                PrimaryIdCountryOfIssueCode = govtIDInfo != null ? govtIDInfo.CountryId.Abbr2 : string.Empty,
                PrimaryIdPlaceOfIssueCode = govtIDInfo != null && govtIDInfo.StateId != null ? govtIDInfo.StateId.Abbr : string.Empty,

                SecondIdType = "SSN",
                SecondIdNumber = cxeCustomer.SSN,
                SecondIdCountryOfIssue = "US",
                Occupation = GetOccupation(cxeCustomer.EmploymentDetails),
                CountryOfBirth = cxeCustomer.CountryOfBirth, // Abbr2
                // Changes made for AL-2834
                CountryOfBirthAbbr3 = masterCountry != null ? masterCountry.Abbr3 : string.Empty,
                Amount = amount,
                ProductName = string.Empty,
                Fee = 0,
                MetaData = new Dictionary<string, object>() 
                { 
                    {"Location", string.Empty},
                    {"SessionCookie", string.Empty},
                    {"AailableBalance", string.Empty},
                    {"AccountHolder", string.Empty},
                    {"Attention", string.Empty},
                    {"Reference", string.Empty},
                    {"DateOfBirth", string.Empty},
                    {"DeliveryCode", string.Empty},
                },
                CustomerMobileNumber = GetCustomerMobileNumber(cxeCustomer),
            };
            return billPayRequest;
        }

        private string GetOccupation(CustomerEmploymentDetails custEmpDetails)
        {
            //AL-1755: Sunil Shetty :10/09/2015: 
            //A issue was found in UAT, Synovus found with new application there were getting error on 
            //billpay after entering amount. we found that it happened with old customer created by genesis. 
            //in genesis if we create customer without occupation then tcustomermeploymentdetails record is not 
            //created and in mitra we create blank record. on get fee we need to send occupation with 
            //billpayrequest so in absence of below check constarint an error would occur
            if (custEmpDetails == null)
                return string.Empty;

            if (!string.IsNullOrWhiteSpace(custEmpDetails.OccupationDescription))
                return custEmpDetails.OccupationDescription;

            string strOccupation = custEmpDetails.Occupation;
            List<Occupation> occupations = PTNRDataStructureService.GetOccupations();

            if (custEmpDetails != null)
            {
                var occupation = occupations.SingleOrDefault(a => a.Code == custEmpDetails.Occupation);

                if (occupation != null)
                {
                    strOccupation = occupation.Name;
                }
            }

            return strOccupation;
        }

        //US2028 CounterID changes
        private void GetCustomerSessionCounterId(CustomerSession session, int providerId, ref MGIContext mgiContext)
        {
            if (!IsHardCodedCounterId)
            {
                if (mgiContext.LocationRowGuid != null)
                {
                    string wuCounterId = string.Empty;

                    if (session.CustomerSessionCounter == null)
                    {
                        wuCounterId = LocationCounterIdService.Get(mgiContext.LocationRowGuid, providerId);

                        if (!string.IsNullOrEmpty(wuCounterId))
                        {
                            UpdateLocationCounterIdStatus(mgiContext.LocationRowGuid, wuCounterId, providerId, false);

                            CreateCustomerSessionCounterId(session, wuCounterId, mgiContext);
                        }
                    }
                    else
                    {
                        wuCounterId = session.CustomerSessionCounter.CounterId;
                    }
                    mgiContext.WUCounterId = wuCounterId;
                }
            }
        }

        private void CreateCustomerSessionCounterId(CustomerSession session, string counterId, MGIContext mgiContext)
        {
            CustomerSessionCounter customerSessionCounterId = new CustomerSessionCounter()
            {
                CustomerSession = session,
                CounterId = counterId,
                DTServerCreate = DateTime.Now,
                DTServerLastModified = DateTime.Now,
                DTTerminalCreate = Clock.DateTimeWithTimeZone(mgiContext.TimeZone),
                DTTerminalLastModified = Clock.DateTimeWithTimeZone(mgiContext.TimeZone)
            };

            session.CustomerSessionCounter = customerSessionCounterId;

            SessionSvc.Save(session);

        }

        private void UpdateLocationCounterIdStatus(Guid locationuidRowG, string counterId, int providerId, bool isAvailable)
        {
            LocationCounterId locationCounterId = LocationCounterIdService.Get(locationuidRowG, counterId, providerId);

            if (locationCounterId != null)
            {
                locationCounterId.IsAvailable = isAvailable;

                LocationCounterIdService.Update(locationCounterId);
            }
        }

        private string _GetBillPayProvider(string channelPartner)
        {
            // get the moneytransfer provider for the channel partner.
            return ProcessorRouter.GetProvider(channelPartner);
        }
        private BillPayment BuildBillPayment(string billerName, string accountNumber, decimal amount)
        {
            BillPayment billPayment = new BillPayment()
            {
                BillerName = billerName,
                AccountNumber = accountNumber,
                PaymentAmount = amount,
                MetaData = new Dictionary<string, object>() 
			    { 
			        {"Location", string.Empty},
			        {"SessionCookie", string.Empty},
			        {"AailableBalance", string.Empty},
			        {"AccountHolder", string.Empty},
			        {"Attention", string.Empty},
			        {"Reference", string.Empty},
			        {"DateOfBirth", string.Empty},
			        {"DeliveryCode", string.Empty},
			    }
            };
            return billPayment;
        }

        private static string GetCustomerMobileNumber(CXECustomer cxeCustomer)
        {
            string mobileNumber = string.Empty;
            if (!string.IsNullOrEmpty(cxeCustomer.Phone1) && cxeCustomer.Phone1Type == "Cell")
            {
                mobileNumber = cxeCustomer.Phone1;
            }
            else if (!string.IsNullOrEmpty(cxeCustomer.Phone2) && cxeCustomer.Phone2Type == "Cell")
            {
                mobileNumber = cxeCustomer.Phone2;
            }
            return mobileNumber;
        }

        private string Encrypt(string plainString)
        {
            if (!string.IsNullOrWhiteSpace(plainString) && plainString.IsCreditCardNumber())
            {
                return BPDataProtectionSvc.Encrypt(plainString, 0);
            }
            return plainString;
        }

        private string Decrypt(string encryptedString)
        {
            string decryptString = encryptedString;
            if (!string.IsNullOrWhiteSpace(encryptedString))
            {
                try
                {
                    decryptString = BPDataProtectionSvc.Decrypt(encryptedString, 0);
                }
                catch
                {
                    decryptString = encryptedString;
                }
            }
            return decryptString;
        }
        #endregion


    }
}
