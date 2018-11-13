using AutoMapper;
using MGI.Biz.Compliance.Contract;
using MGI.Biz.Compliance.Data;
using MGI.Biz.MoneyTransfer.Contract;
using MGI.Biz.MoneyTransfer.Data;
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.Util;
using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Cxn.Common.Processor.Util;
using MGI.Cxn.MoneyTransfer.Contract;
using Spring.Transaction.Interceptor;
using System;
using System.Collections.Generic;
using System.Linq;
using bizAccount = MGI.Biz.MoneyTransfer.Data.Account;
using bizCardDetails = MGI.Biz.MoneyTransfer.Data.CardDetails;
using bizCardLookupDetails = MGI.Biz.MoneyTransfer.Data.CardLookupDetails;
using bizMasterData = MGI.Biz.MoneyTransfer.Data.MasterData;
using bizModifyMTSearch = MGI.Biz.MoneyTransfer.Data.SearchRequest;
using bizModifySendMoney = MGI.Biz.MoneyTransfer.Data.ModifyRequest;
using bizMoneyTransferStage = MGI.Biz.MoneyTransfer.Data.MoneyTransferStage;
using bizPaymentDetails = MGI.Biz.MoneyTransfer.Data.PaymentDetails;
using bizPaymentTransaction = MGI.Biz.MoneyTransfer.Data.PaymentTranasction;
using bizReceiver = MGI.Biz.MoneyTransfer.Data.Receiver;
using bizRefundMTRequest = MGI.Biz.MoneyTransfer.Data.RefundRequest;
using bizRefundSearch = MGI.Biz.MoneyTransfer.Data.SearchRequest;
using CxeAccount = MGI.Core.CXE.Data.Account;
using CxeCustomer = MGI.Core.CXE.Data.Customer;
using cxeMoneyTransferStage = MGI.Core.CXE.Data.Transactions.Stage.MoneyTransfer;
using cxnAccount = MGI.Cxn.MoneyTransfer.Data.Account;
using cxnCardDetails = MGI.Cxn.MoneyTransfer.Data.CardDetails;
using cxnCardLookupDetails = MGI.Cxn.MoneyTransfer.Data.CardLookupDetails;
using CxnData = MGI.Cxn.MoneyTransfer.Data;
using cxnMasterData = MGI.Cxn.MoneyTransfer.Data.MasterData;
using cxnModifySendMoney = MGI.Cxn.MoneyTransfer.Data.ModifySearchRequest;
using cxnPaymentDetails = MGI.Cxn.MoneyTransfer.Data.PaymentDetails;
using cxnPaymentTransaction = MGI.Cxn.MoneyTransfer.Data.PaymentTransaction;
using cxnReceiver = MGI.Cxn.MoneyTransfer.Data.Receiver;
using cxnRefundMTRequest = MGI.Cxn.MoneyTransfer.Data.RefundRequest;
using cxnTransaction = MGI.Cxn.MoneyTransfer.Data.Transaction;
using PTNRCustomerService = MGI.Core.Partner.Contract.ICustomerService;
using PTNRMoneyTransfer = MGI.Core.Partner.Data.Transactions.MoneyTransfer;
using PTNRMoneyTransferService = MGI.Core.Partner.Contract.ITransactionService<MGI.Core.Partner.Data.Transactions.MoneyTransfer>;

namespace MGI.Biz.MoneyTransfer.Impl
{
    public partial class MoneyTransferEngine : IMoneyTransferEngine
    {

        #region Injected Services

        public IMoneyTransferService CXEMoneyTransferService { private get; set; }
        public IMoneyTransfer CXNMoneyTransferService { private get; set; }
        public MGI.Core.CXE.Contract.ICustomerService CXECustomerService { private get; set; }
        public IAccountService CXEAccountService { private get; set; }
        public PTNRCustomerService PTNRCustomerService { private get; set; }
        public PTNRMoneyTransferService PTNRMoneyTransferService { private get; set; }
        public ICustomerSessionService CustomerSessionService { private get; set; }
        public IChannelPartnerService PTNRChannelPartnerService { private get; set; }
        public ILimitService LimitService { private get; set; }
        public INexxoDataStructuresService PTNRDataStructureService { private get; set; }
        public IProcessorRouter MoneyTransferProcessorSvc { private get; set; }
        //public MGI.Biz.Partner.Contract.IShoppingCartService ShoppingcartService { get; set; }
        public MGI.Core.Partner.Contract.ILocationCounterIdService LocationCounterIdService { private get; set; }
        public MGI.Core.Partner.Contract.ICustomerSessionCounterIdService CustomerSessionCounterIdService { private get; set; }
        public bool IsHardCodedCounterId { get; set; }
        public TLoggerCommon MongoDBLogger { get; set; }

        #endregion


        public MoneyTransferEngine()
        {
            Mapper.CreateMap<cxnAccount, bizAccount>();
            Mapper.CreateMap<bizReceiver, cxnReceiver>();
            Mapper.CreateMap<bizReceiver, bizReceiver>();
            Mapper.CreateMap<cxnReceiver, bizReceiver>();
            Mapper.CreateMap<CxnData.FeeResponse, Data.FeeResponse>();
            Mapper.CreateMap<Data.FeeResponse, CxnData.FeeResponse>();
            Mapper.CreateMap<CxnData.FeeRequest, Data.FeeRequest>();
            Mapper.CreateMap<Data.FeeRequest, CxnData.FeeRequest>();
            Mapper.CreateMap<CxnData.FeeInformation, Data.FeeInformation>();
            Mapper.CreateMap<Data.FeeInformation, CxnData.FeeInformation>();

            Mapper.CreateMap<Data.ValidateResponse, CxnData.ValidateResponse>();
            Mapper.CreateMap<CxnData.ValidateResponse, Data.ValidateResponse>();
            Mapper.CreateMap<CxnData.ValidateRequest, Data.ValidateRequest>();
            Mapper.CreateMap<Data.ValidateRequest, CxnData.ValidateRequest>();

            Mapper.CreateMap<bizPaymentDetails, cxnPaymentDetails>();
            Mapper.CreateMap<cxeMoneyTransferStage, bizMoneyTransferStage>()
                .ForMember(d => d.Amount, s => s.MapFrom(c => c.Amount))
                .ForMember(d => d.ConfirmationNumber, s => s.MapFrom(c => c.ConfirmationNumber))
                .ForMember(d => d.Destination, s => s.MapFrom(c => c.Destination))
                //.ForMember(d => d.Dtcreate, s => s.MapFrom(c => c.DTTerminalCreate))
                //.ForMember(d => d.Dtlastmod, s => s.MapFrom(c => c.DTTerminalLastModified))
                .ForMember(d => d.Fee, s => s.MapFrom(c => c.Fee))
                .ForMember(d => d.Id, s => s.MapFrom(c => c.Id))
                .ForMember(d => d.ReceiverName, s => s.MapFrom(c => c.ReceiverName))
                .ForMember(d => d.Rowguid, s => s.MapFrom(c => c.rowguid))
                .ForMember(d => d.Status, s => s.MapFrom(c => c.Status))
                ;

            Mapper.CreateMap<MGI.Cxn.MoneyTransfer.Data.Transaction, MoneyTransferTransaction>();
            Mapper.CreateMap<cxnCardDetails, bizCardDetails>();
            Mapper.CreateMap<bizCardDetails, cxnCardDetails>();
            Mapper.CreateMap<cxnCardLookupDetails, bizCardLookupDetails>();
            Mapper.CreateMap<bizCardLookupDetails, cxnCardLookupDetails>();
            Mapper.CreateMap<MGI.Cxn.MoneyTransfer.Data.CardInfo, CardInfo>();
            Mapper.CreateMap<cxnModifySendMoney, bizModifySendMoney>()
                    .ForMember(d => d.FirstName, s => s.MapFrom(c => c.paymentTransaction.receiver.FirstName))
                    .ForMember(d => d.SecondLastName, s => s.MapFrom(c => c.paymentTransaction.receiver.SecondLastName))
                    .ForMember(d => d.LastName, s => s.MapFrom(c => c.paymentTransaction.receiver.LastName))
                    .ForMember(d => d.ConfirmationNumber, s => s.MapFrom(c => c.paymentTransaction.mtcn))
                    .ForMember(d => d.TestQuestion, s => s.MapFrom(c => c.paymentTransaction.paymentDetails.TestQuestion))
                    .ForMember(d => d.TestAnswer, s => s.MapFrom(c => c.paymentTransaction.paymentDetails.TestAnswer))
                    ;
            Mapper.CreateMap<bizPaymentTransaction, cxnPaymentTransaction>();
            Mapper.CreateMap<bizModifyMTSearch, CxnData.SearchRequest>();
            Mapper.CreateMap<MGI.Cxn.MoneyTransfer.Data.SearchResponse, Biz.MoneyTransfer.Data.SearchResponse>()
                .ForMember(d => d.ConfirmationNumber, s => s.MapFrom(c => c.ConfirmationNumber));
            Mapper.CreateMap<bizRefundSearch, CxnData.SearchRequest>();
            Mapper.CreateMap<bizRefundMTRequest, cxnRefundMTRequest>();

            Mapper.CreateMap<cxnMasterData, bizMasterData>();
            Mapper.CreateMap<Data.DeliveryService, MGI.Cxn.MoneyTransfer.Data.DeliveryService>();
            Mapper.CreateMap<MGI.Cxn.MoneyTransfer.Data.DeliveryService, Data.DeliveryService>();
            Mapper.CreateMap<MGI.Cxn.MoneyTransfer.Data.DeliveryServiceRequest, Data.DeliveryServiceRequest>();
            Mapper.CreateMap<Data.DeliveryServiceRequest, MGI.Cxn.MoneyTransfer.Data.DeliveryServiceRequest>();
            Mapper.CreateMap<MGI.Cxn.MoneyTransfer.Data.Reason, Data.Reason>();
            Mapper.CreateMap<ReasonRequest, MGI.Cxn.MoneyTransfer.Data.ReasonRequest>();

            Mapper.CreateMap<CxnData.Field, Field>();
            Mapper.CreateMap<AttributeRequest, CxnData.AttributeRequest>();
        }

        #region Private Methods

        private string GetOccupation(CustomerEmploymentDetails custEmpDetails)
        {
            string selectedOccupation = string.Empty;

            if (custEmpDetails != null)
            {
                if (!string.IsNullOrWhiteSpace(custEmpDetails.OccupationDescription))
                    return custEmpDetails.OccupationDescription;

                selectedOccupation = custEmpDetails.Occupation;

                List<Occupation> occupations = PTNRDataStructureService.GetOccupations();
                var occupation = occupations.SingleOrDefault(a => a.Code == selectedOccupation);

                if (occupation != null)
                    return occupation.Name;
            }

            return selectedOccupation;
        }

        #endregion

        #region Public Methods

        public Data.FeeResponse GetFee(long customerSessionId, FeeRequest feeRequest, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<FeeRequest>(customerSessionId, feeRequest, "GetFee", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin GetFee-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            long customerAccountId = long.MinValue;
            long transactionId = 0L;
            long cxnTransactionId = 0L;

            CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

            // check against limit
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(session.Customer.ChannelPartnerId);

            Data.FeeResponse feeResponse = new Data.FeeResponse();


            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }

            GetCustomerSessionCounterId(session, ref mgiContext);

            if (feeRequest.TransactionId == 0)
            {
                feeRequest.ReferenceNo = DateTime.Now.ToString("yyyyMMddhhmmssff");
                transactionId = StageMoneyTransfer(customerSessionId, feeRequest, mgiContext);

            }
            else
            {
                transactionId = feeRequest.TransactionId;
                cxnTransactionId = UpdateMoneyTransferStage(feeRequest, mgiContext.TimeZone);
                feeRequest.TransactionId = cxnTransactionId;
            }

            CxeCustomer cxeCustomer = CXECustomerService.Lookup(session.Customer.CXEId);
            CxeAccount cxeAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.MoneyTransfer);

            if (cxeAccount != null)
            {
                customerAccountId = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccount.Id).CXNId;
            }

            IMoneyTransfer moneyTransferProcessor = _GetMoneyTransferProcessor(channelPartner.Name);

            CxnData.FeeRequest request = Mapper.Map<CxnData.FeeRequest>(feeRequest);
            request.AccountId = customerAccountId;

            CxnData.FeeResponse cxnFeeResponse = moneyTransferProcessor.GetFee(request, mgiContext);

            feeResponse = Mapper.Map<CxnData.FeeResponse, Data.FeeResponse>(cxnFeeResponse);

            feeResponse.TransactionId = UpdateMoneyTransferStage(transactionId, feeResponse, mgiContext);
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<Data.FeeResponse>(customerSessionId, feeResponse, "GetFee", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "End GetFee-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return feeResponse;

        }
        /// <summary>
        /// US2054
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="stateCode"></param>
        /// <returns></returns>
        public bool IsSWBState(long customerSessionId, string stateCode, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, stateCode, "IsSWBState", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin IsSWBState-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            bool isExists = false;
            if (!string.IsNullOrWhiteSpace(stateCode))
            {
                CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
                ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);
                isExists = _GetMoneyTransferProcessor(channelPartner.Name).IsSWBState(stateCode);
            }
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(isExists), "IsSWBState", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End IsSWBState-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return isExists;
        }

        public ValidateResponse Validate(long customerSessionId, ValidateRequest validateRequest, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<ValidateRequest>(customerSessionId, validateRequest, "Validate", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin Validate-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(validateRequest.TransactionId);
            long cxnTransactionId = ptnrMoneyTransfer.CXNId;
            if (mgiContext.ProviderId != 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }
            UpdateMoneyTransferStage(validateRequest, mgiContext);
            CustomerSession session = CustomerSessionService.Lookup(customerSessionId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(session.Customer.ChannelPartnerId);

            // check against limit
            decimal transactionAmount = Convert.ToDecimal(validateRequest.Amount + validateRequest.Fee + validateRequest.Tax + validateRequest.OtherFee);

            decimal currentTransactionAmount = validateRequest.Amount;

            if (validateRequest.TransferType == TransferType.SendMoney)
            {
                decimal minimumAmount = LimitService.GetProductMinimum(channelPartner.ComplianceProgramName, TransactionTypes.MoneyTransfer, mgiContext);
                decimal maximumAmount = LimitService.CalculateTransactionMaximumLimit(customerSessionId, channelPartner.ComplianceProgramName, TransactionTypes.MoneyTransfer, mgiContext);

                if (currentTransactionAmount < minimumAmount)
                {
                    throw new BizComplianceLimitException(BizComplianceLimitException.MONEY_TRANSFER_MINIMUM_LIMIT_CHECK, minimumAmount);
                }
                if (currentTransactionAmount > maximumAmount)
                {
                    throw new BizComplianceLimitException(BizComplianceLimitException.MONEY_TRANSFER_LIMIT_EXCEEDED, maximumAmount);
                }
            }

            // get compliance information
            CxeCustomer customer = CXECustomerService.Lookup(session.Customer.CXEId);

            CxnData.ValidateRequest cxnValidateRequest = Mapper.Map<CxnData.ValidateRequest>(validateRequest);

            cxnValidateRequest.TransactionId = cxnTransactionId;
            cxnValidateRequest.SecondIdType = "SSN";
            cxnValidateRequest.SecondIdNumber = customer.SSN;
            cxnValidateRequest.SecondIdCountryOfIssue = "US";
            cxnValidateRequest.Occupation = GetOccupation(customer.EmploymentDetails); //(customer.EmploymentDetails != null && customer.EmploymentDetails.Occupation != null) ? customer.EmploymentDetails.Occupation : string.Empty;
            if (customer.DateOfBirth != null)
            {
                cxnValidateRequest.DateOfBirth = customer.DateOfBirth.Value.ToString("MM/dd/yyyy");
            }

            if (customer.GovernmentId != null)
            {
                NexxoIdType govtIDInfo = PTNRDataStructureService.Find(channelPartner.Id, customer.GovernmentId.IdTypeId);

                if (govtIDInfo != null)
                {
                    cxnValidateRequest.PrimaryIdType = govtIDInfo.Name;
                    cxnValidateRequest.PrimaryIdNumber = customer.GovernmentId.Identification;
                    cxnValidateRequest.PrimaryIdCountryOfIssue = govtIDInfo.CountryId != null ? govtIDInfo.CountryId.Name : string.Empty;
                    cxnValidateRequest.PrimaryIdPlaceOfIssue = govtIDInfo.StateId != null ? govtIDInfo.StateId.Name : string.Empty;
                    cxnValidateRequest.PrimaryCountryCodeOfIssue = govtIDInfo.CountryId != null ? govtIDInfo.CountryId.Abbr2 : string.Empty;
                }
            }
            MasterCountry masterCountry = PTNRDataStructureService.GetMasterCountryByCode(customer.CountryOfBirth);
            if (masterCountry != null)
            {
                cxnValidateRequest.CountryOfBirthAbbr2 = masterCountry.Abbr2;
                cxnValidateRequest.CountryOfBirthAbbr3 = masterCountry.Abbr3;
                cxnValidateRequest.CountryOfBirth = masterCountry.Name;
            }
            if (validateRequest.TransferType == TransferType.ReceiveMoney)
            {
                cxnValidateRequest.MetaData.Add("receiverAddress", customer.Address1);
                cxnValidateRequest.MetaData.Add("receiverCity", customer.City);
                cxnValidateRequest.MetaData.Add("receiverZipCode", customer.ZipCode);
                cxnValidateRequest.MetaData.Add("receiverState", customer.State);
                cxnValidateRequest.MetaData.Add("receiverPhone", customer.Phone1);
            }

            //Sender's Middle Name and Last Name2
            if (!String.IsNullOrEmpty(customer.MiddleName))
                cxnValidateRequest.MetaData.Add("senderMiddleName", customer.MiddleName);
            if (!String.IsNullOrEmpty(customer.LastName2))
                cxnValidateRequest.MetaData.Add("senderLastName2", customer.LastName2);

            GetCustomerSessionCounterId(session, ref mgiContext);

            MGI.Cxn.MoneyTransfer.Data.ValidateResponse response = _GetMoneyTransferProcessor(channelPartner.Name).Validate(cxnValidateRequest, mgiContext);

            if (response.HasLPMTError)
            {
                return new ValidateResponse() { HasLPMTError = true };
            }

            // update  cxe for payment details          
            CXEMoneyTransferService.Update(ptnrMoneyTransfer.CXEId, TransactionStates.Authorized, mgiContext.TimeZone);

            // update ptnr txn for payment details
            PTNRMoneyTransferService.UpdateStates(ptnrMoneyTransfer.Id, (int)TransactionStates.Authorized, (int)TransactionStates.Authorized);

            MGI.Cxn.MoneyTransfer.Data.TransactionRequest request = new MGI.Cxn.MoneyTransfer.Data.TransactionRequest()
            {
                TransactionId = response.TransactionId,
                TransactionRequestType = CxnData.TransactionRequestType.CXNTransaction
            };

            //Author : Abhijith
            //Defect Fix : Transaction fees does not have "Additional Charges" and "taxes" added to that so we are getting values mismatch in receipts and other places in screen.
            //Description : Added this logic to update the Fee from CXN to PTNR database.
            //because CXN will have latest additional charges, tax etc. This logic should happen only for "Send Money"
            //For "Receive Money" we have to pass the fee "0" as receiver has to get the principal amount (without fees, additional charges and taxes)
            //Starts Here
            MGI.Cxn.MoneyTransfer.Data.Transaction transaction = _GetMoneyTransferProcessor(channelPartner.Name).GetTransaction(request, mgiContext);

            decimal fee = 0;

            if (validateRequest.TransferType == TransferType.SendMoney)
            {
                // checks key exists in metadata and handles null
                decimal transferTax = Convert.ToDecimal(NexxoUtil.GetDecimalDictionaryValueIfExists(transaction.MetaData, "TransferTax"));
                decimal additionalCharges = Convert.ToDecimal(NexxoUtil.GetDecimalDictionaryValueIfExists(transaction.MetaData, "AdditionalCharges"));
                fee = transaction.Fee + transferTax + additionalCharges;
            }
            PTNRMoneyTransferService.UpdateFee(ptnrMoneyTransfer.Id, fee);
            //Ends Here

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "Validate", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End Validate-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return new ValidateResponse()
            {
                TransactionId = ptnrMoneyTransfer.Id,
                HasLPMTError = response.HasLPMTError
            };
        }

        [Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
        public int Commit(long customerSessionId, long ptnrTransactionId, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(ptnrTransactionId), "Commit", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin Commit-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            // Commit in CXN
            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(ptnrTransactionId);

            CustomerSession customerSession = GetCustomerSession(customerSessionId);

            //TODO This line is commented as part of MVA Impl , cxeCustomer is not used need to be verifyed and removed
            //CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }

            GetCustomerSessionCounterId(customerSession, ref mgiContext);

            if (string.IsNullOrEmpty(mgiContext.SMTrxType) && ptnrMoneyTransfer.TransferType == (int)TransferType.SendMoney)
                mgiContext.SMTrxType = MGI.Cxn.MoneyTransfer.WU.Data.MTReleaseStatus.Release.ToString();
            if (string.IsNullOrEmpty(mgiContext.RMTrxType) && ptnrMoneyTransfer.TransferType == (int)TransferType.ReceiveMoney)
                mgiContext.RMTrxType = MGI.Cxn.MoneyTransfer.WU.Data.MTReleaseStatus.Release.ToString();

            _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName).Commit(ptnrMoneyTransfer.CXNId, mgiContext); // Moneytransfer Commit -mtcn return null as it simulator,need clarification

            if (!string.IsNullOrEmpty(mgiContext.SMTrxType) &&
                string.Equals(mgiContext.SMTrxType, MGI.Cxn.MoneyTransfer.WU.Data.MTReleaseStatus.Release.ToString(), StringComparison.InvariantCultureIgnoreCase)
                || !string.IsNullOrEmpty(mgiContext.RMTrxType) &&
                string.Equals(mgiContext.RMTrxType, MGI.Cxn.MoneyTransfer.WU.Data.MTReleaseStatus.Release.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {

                cxnTransaction cxnTransaction = new cxnTransaction();
                if (!string.IsNullOrEmpty(mgiContext.SMTrxType) &&
                    string.Equals(mgiContext.SMTrxType, MGI.Cxn.MoneyTransfer.WU.Data.MTReleaseStatus.Release.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    //updating the reciever details. 

                    CxnData.TransactionRequest request = new CxnData.TransactionRequest()
                    {
                        TransactionId = ptnrMoneyTransfer.CXNId
                    };
                    cxnTransaction = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName).GetTransaction(request, mgiContext);
                    //UpdateReceiverName(customerSessionId, cxnTransaction, mgiContext);
                    ptnrMoneyTransfer.ConfirmationNumber = cxnTransaction.ConfirmationNumber;
                    CXEMoneyTransferService.Update(ptnrMoneyTransfer.CXEId, TransactionStates.Committed, mgiContext.TimeZone, cxnTransaction.ConfirmationNumber);
                }


                // Commit in CXE                
                CXEMoneyTransferService.Commit(ptnrMoneyTransfer.CXEId);

                // Update CXE stage state as committed
                CXEMoneyTransferService.Update(ptnrMoneyTransfer.CXEId, TransactionStates.Committed, mgiContext.TimeZone);

                //Updating Partner Transactions              
                PTNRMoneyTransferService.UpdateStates(ptnrMoneyTransfer.Id, (int)TransactionStates.Committed, (int)TransactionStates.Committed);

                //TODO ConfirmationNumber to be saved in CXE and PTNR

                #region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(TransactionStates.Committed), "Commit", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                          "End Commit-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
                #endregion
                return (int)TransactionStates.Committed;
            }
            else if (ptnrMoneyTransfer.TransferType == (int)TransferType.Refund)
            {
                // Commit in CXE                
                CXEMoneyTransferService.Commit(ptnrMoneyTransfer.CXEId);

                // Update CXE stage state as committed
                CXEMoneyTransferService.Update(ptnrMoneyTransfer.CXEId, TransactionStates.Committed, mgiContext.TimeZone);

                //Updating Partner Transactions              
                PTNRMoneyTransferService.UpdateStates(ptnrMoneyTransfer.Id, (int)TransactionStates.Committed, (int)TransactionStates.Committed);
                #region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<string>(customerSessionId, string.Empty, "Commit", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                          "End Commit-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
                #endregion
                return (int)TransactionStates.Committed;
            }
            else
                #region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<string>(customerSessionId, string.Empty, "Commit", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                          "End Commit-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
                #endregion
            return 0; // For Cancel - Commit 
        }

        public long AddReceiver(long customerSessionId, bizReceiver receiver, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<bizReceiver>(customerSessionId, receiver, "AddReceiver", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin AddReceiver-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            if (!ValidateReceiver(receiver))
                throw new Exception("Receiver Details not matched the Criteria");

            CustomerSession customerSession = GetCustomerSession(customerSessionId);

            receiver.CustomerId = customerSession.Customer.Id;

            cxnReceiver cxnreceiver = Mapper.Map<bizReceiver, cxnReceiver>(receiver);

            IMoneyTransfer cxnMoneyTransfer = GetCxnTransferService(customerSession);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "AddReceiver", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End AddReceiver-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return cxnMoneyTransfer.SaveReceiver(cxnreceiver, mgiContext);

        }

        public long EditReceiver(long customerSessionId, bizReceiver receiver, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<bizReceiver>(customerSessionId, receiver, "EditReceiver", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin EditReceiver-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            if (!ValidateReceiver(receiver))
                throw new Exception("Receiver Details not matched the Criteria");

            CustomerSession customerSession = GetCustomerSession(customerSessionId);

            receiver.CustomerId = customerSession.Customer.Id;

            cxnReceiver cxnReceiver = Mapper.Map<bizReceiver, cxnReceiver>(receiver);

            IMoneyTransfer cxnMoneyTransfer = GetCxnTransferService(customerSession);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "EditReceiver", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End EditReceiver-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return cxnMoneyTransfer.SaveReceiver(cxnReceiver, mgiContext);

        }

        public bizReceiver GetReceiver(long customerSessionId, long Id, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(Id), "GetReceiver", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin GetReceiver-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            CustomerSession customerSession = GetCustomerSession(customerSessionId);

            IMoneyTransfer CXNMoneyTransfer = GetCxnTransferService(customerSession);

            cxnReceiver cxnreceiver = CXNMoneyTransfer.GetReceiver(Id);

            bizReceiver bizreceiver = Mapper.Map<cxnReceiver, bizReceiver>(cxnreceiver);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<bizReceiver>(customerSessionId, bizreceiver, "GetReceiver", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "End GetReceiver-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion

            return bizreceiver;
        }

        public bizReceiver GetReceiver(long customerSessionId, string fullName, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, fullName, "GetReceiver", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin GetReceiver-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            CustomerSession customerSession = GetCustomerSession(customerSessionId);

            long customerId = customerSession != null ? customerSession.Customer.Id : 0;

            IMoneyTransfer CXNMoneyTransfer = GetCxnTransferService(customerSession);

            cxnReceiver cxnreceiver = CXNMoneyTransfer.GetReceiver(customerId, fullName);

            bizReceiver bizreceiver = Mapper.Map<cxnReceiver, bizReceiver>(cxnreceiver);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<bizReceiver>(customerSessionId, bizreceiver, "GetReceiver", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "End GetReceiver-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion

            return bizreceiver;
        }

        public List<bizReceiver> GetReceivers(long customerSessionId, string lastName, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, lastName, "GetReceivers", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin GetReceivers-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            CustomerSession customerSession = GetCustomerSession(customerSessionId);

            IMoneyTransfer CXNMoneyTransfer = GetCxnTransferService(customerSession);

            List<cxnReceiver> cxnReceivers = CXNMoneyTransfer.GetReceivers(customerSession.Customer.Id, lastName);

            List<bizReceiver> bizReceivers = Mapper.Map<List<cxnReceiver>, List<bizReceiver>>(cxnReceivers);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.ListInfo<bizReceiver>(customerSessionId, bizReceivers, "GetReceivers", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End GetReceivers-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return bizReceivers;
        }

        public List<bizReceiver> GetActiveReceivers(long customerSessionId, string lastName, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, lastName, "GetActiveReceivers", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin GetActiveReceivers-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            CustomerSession customerSession = GetCustomerSession(customerSessionId);

            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);

            IMoneyTransfer CXNMoneyTransfer = _GetMoneyTransferProcessor(channelPartner.Name);

            List<cxnReceiver> cxnReceivers = CXNMoneyTransfer.GetReceivers(customerSession.Customer.Id, lastName);

            cxnReceivers = cxnReceivers.Where(c => c.Status == "Active").ToList();

            List<bizReceiver> bizReceivers = Mapper.Map<List<cxnReceiver>, List<bizReceiver>>(cxnReceivers);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.ListInfo<bizReceiver>(customerSessionId, bizReceivers, "GetActiveReceivers", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End GetActiveReceivers-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return bizReceivers;
        }

        public List<bizReceiver> GetFrequentReceivers(long customerSessionId, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetFrequentReceivers", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin GetFrequentReceivers-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            CustomerSession customerSession = GetCustomerSession(customerSessionId);

            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);

            IMoneyTransfer CXNMoneyTransfer = _GetMoneyTransferProcessor(channelPartner.Name);

            List<cxnReceiver> cxnReceivers = CXNMoneyTransfer.GetFrequentReceivers(customerSession.Customer.Id);

            List<bizReceiver> bizReceivers = Mapper.Map<List<cxnReceiver>, List<bizReceiver>>(cxnReceivers);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.ListInfo<bizReceiver>(customerSessionId, bizReceivers, "GetFrequentReceivers", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "End GetFrequentReceivers-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return bizReceivers;

        }

        public void DeleteFavoriteReceiver(long customerSessionId, long receiverId, MGIContext mgiContext)
        {
            #region Transactional Log
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(receiverId), "DeleteFavoriteReceiver", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin DeleteFavoriteReceiver-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            CustomerSession customerSession = GetCustomerSession(customerSessionId);
            IMoneyTransfer CXNMoneyTransfer = GetCxnTransferService(customerSession);
            cxnReceiver cxnreceiver = CXNMoneyTransfer.GetReceiver(receiverId);
            cxnreceiver.Status = "InActive";
            CXNMoneyTransfer.DeleteFavoriteReceiver(cxnreceiver, mgiContext);
            #region Transactional Log
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "DeleteFavoriteReceiver", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin DeleteFavoriteReceiver-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
        }


        public MoneyTransferTransaction Get(long customerSessionId, TransactionRequest request, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<TransactionRequest>(customerSessionId, request, "Get", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin Get-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            long Id;
            PTNRMoneyTransfer ptnrMoneyTransfer = null;
            if (request.PTNRTransactionId != 0)
            {
                ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(request.PTNRTransactionId);
                Id = ptnrMoneyTransfer.CXNId;
            }
            else
                Id = request.CXNTransactionId;

            CustomerSession customerSession = GetCustomerSession(customerSessionId);

            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);

            CxnData.TransactionRequest cxnrequest = new CxnData.TransactionRequest()
            {
                TransactionId = Id
            };

            MGI.Cxn.MoneyTransfer.Data.Transaction moneyTransferTran = _GetMoneyTransferProcessor(channelPartner.Name).GetTransaction(cxnrequest, mgiContext);
            MoneyTransferTransaction bizTrx = null;

            if (moneyTransferTran.TransactionType.ToLower() == ((int)TransferType.ReceiveMoney).ToString())
            {
                bizTrx = new MoneyTransferTransaction()
                {
                    Receiver = new bizReceiver()
                    {
                        LastName = moneyTransferTran.ReceiverLastName,
                        FirstName = moneyTransferTran.ReceiverFirstName,
                        SecondLastName = moneyTransferTran.ReceiverSecondLastName
                    },
                    AmountToReceiver = moneyTransferTran.AmountToReceiver,
                    Account = new MGI.Biz.MoneyTransfer.Data.Account() { FirstName = moneyTransferTran.SenderName, LastName = string.Empty },
                    ConfirmationNumber = moneyTransferTran.ConfirmationNumber,
                    Fee = moneyTransferTran.Fee,
                    TransactionAmount = moneyTransferTran.TransactionAmount,
                    TransactionID = moneyTransferTran.TransactionID,
                    TransactionType = moneyTransferTran.TransactionType,
                    DestinationCountryCode = moneyTransferTran.DestinationCountryCode,
                    DestinationCurrencyCode = moneyTransferTran.DestinationCurrencyCode,
                    DestinationState = moneyTransferTran.DestinationState,
                    TaxAmount = moneyTransferTran.TaxAmount,
                    GrossTotalAmount = moneyTransferTran.GrossTotalAmount,
                    ExchangeRate = moneyTransferTran.ExchangeRate,
                    MetaData = moneyTransferTran.MetaData,
                    DeliveryServiceName = moneyTransferTran.DeliveryServiceName,
                    DeliveryServiceDesc = moneyTransferTran.DeliveryServiceDesc,
                    IsDomesticTransfer = moneyTransferTran.IsDomesticTransfer,
                    PromotionsCode = moneyTransferTran.PromotionsCode,
                    PersonalMessage = moneyTransferTran.PersonalMessage,
                    OriginatingCountryCode = moneyTransferTran.OriginatingCountryCode,
                    OriginatingCurrencyCode = moneyTransferTran.OriginatingCurrencyCode
                };
            }
            else
            {
                decimal messageCharge = NexxoUtil.GetDecimalDictionaryValueIfExists(moneyTransferTran.MetaData, "MessageCharge");
                decimal additionalCharge = NexxoUtil.GetDecimalDictionaryValueIfExists(moneyTransferTran.MetaData, "AdditionalCharges");
                decimal transferTax = NexxoUtil.GetDecimalDictionaryValueIfExists(moneyTransferTran.MetaData, "TransferTax");

                bizTrx = new MoneyTransferTransaction()
                {
                    Receiver = Mapper.Map<bizReceiver>(moneyTransferTran.Receiver),
                    Account = Mapper.Map<bizAccount>(moneyTransferTran.Account),
                    ConfirmationNumber = moneyTransferTran.ConfirmationNumber,
                    Fee = moneyTransferTran.Fee + additionalCharge + transferTax,
                    TransactionAmount = moneyTransferTran.TransactionAmount,
                    TransactionID = moneyTransferTran.TransactionID,
                    TransactionType = moneyTransferTran.TransactionType,
                    DestinationCountryCode = moneyTransferTran.DestinationCountryCode,
                    DestinationCurrencyCode = moneyTransferTran.DestinationCurrencyCode,
                    DestinationState = moneyTransferTran.DestinationState,
                    TaxAmount = moneyTransferTran.TaxAmount,
                    GrossTotalAmount = moneyTransferTran.GrossTotalAmount,
                    ExchangeRate = moneyTransferTran.ExchangeRate,
                    PromotionDiscount = moneyTransferTran.PromotionDiscount,
                    PromotionsCode = moneyTransferTran.PromotionsCode,
                    DeliveryServiceName = moneyTransferTran.DeliveryServiceName,
                    DeliveryServiceDesc = moneyTransferTran.DeliveryServiceDesc,
                    OriginatingCountryCode = moneyTransferTran.OriginatingCountryCode,
                    OriginatingCurrencyCode = moneyTransferTran.OriginatingCurrencyCode,
                    TransactionSubType = moneyTransferTran.TransactionSubType,
                    IsModifiedOrRefunded = moneyTransferTran.IsModifiedOrRefunded,
                    OriginalTransactionId = moneyTransferTran.OriginalTransactionID,
                    ReceiverFirstName = moneyTransferTran.ReceiverFirstName,
                    ReceiverLastName = moneyTransferTran.ReceiverLastName,
                    ReceiverSecondLastName = moneyTransferTran.ReceiverSecondLastName,
                    AmountToReceiver = moneyTransferTran.AmountToReceiver,
                    DTAvailableForPickup = moneyTransferTran.DTAvailableForPickup,
                    MetaData = moneyTransferTran.MetaData,
                    PersonalMessage = moneyTransferTran.PersonalMessage,
                    ProviderId = moneyTransferTran.ProviderId,
                    ExpectedPayoutStateCode = moneyTransferTran.ExpectedPayoutStateCode
                };
            }

            if (ptnrMoneyTransfer != null)
            {
                if (bizTrx.MetaData == null)
                {
                    bizTrx.MetaData = new Dictionary<string, object>();
                }
                bizTrx.MetaData.Add("TransactionTimeZone", ptnrMoneyTransfer.CustomerSession.TimezoneID);
            }

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<MoneyTransferTransaction>(customerSessionId, bizTrx, "Get", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End Get-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return bizTrx;
        }

        public MoneyTransferTransaction GetReceiverLastTransaction(long customerSessionId, long receiverId, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(receiverId), "GetReceiverLastTransaction", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin GetReceiverLastTransaction-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            MGI.Cxn.MoneyTransfer.Data.Transaction moneyTransferTran = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName).GetReceiverLastTransaction(receiverId, mgiContext);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<MGI.Cxn.MoneyTransfer.Data.Transaction>(customerSessionId, moneyTransferTran, "GetReceiverLastTransaction", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End GetReceiverLastTransaction-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return Mapper.Map<MGI.Cxn.MoneyTransfer.Data.Transaction, MoneyTransferTransaction>(moneyTransferTran);
        }

        public bool UpdateAccount(long customerSessionId, string WUGoldCardNumber, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, WUGoldCardNumber, "UpdateAccount", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin UpdateAccount-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            bool isUpdated = false;

            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }
            long accountId = 0;

            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(mgiContext.ChannelPartnerId);

            CustomerSession customerSession = GetCustomerSession(customerSessionId);
            CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
            CxeAccount cxeAccount = GetCXEAccount(customerSession, cxeCustomer, channelPartner, mgiContext);

            GetCustomerSessionCounterId(customerSession, ref mgiContext);

            accountId = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccount.Id).CXNId;
            isUpdated = _GetMoneyTransferProcessor(channelPartner.Name).UseGoldcard(accountId, WUGoldCardNumber, mgiContext);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(isUpdated), "UpdateAccount", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End UpdateAccount-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return isUpdated;

        }

        public MoneyTransferTransaction Get(long customerSessionId, ReceiveMoneyRequest request, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<ReceiveMoneyRequest>(customerSessionId, request, "Get", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin Get-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            string confirmationNumber = request.ConfirmationNumber;

            CustomerSession customerSession = GetCustomerSession(customerSessionId);

            CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);

            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(cxeCustomer.ChannelPartnerId);

            CxeAccount cxeAccount = GetCXEAccount(customerSession, cxeCustomer, channelPartner, mgiContext);

            cxeMoneyTransferStage cxeMoneyTransferStage = new cxeMoneyTransferStage
            {
                Account = cxeAccount,
                ConfirmationNumber = confirmationNumber,
                Status = (int)TransactionStates.Initiated,
                DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                DTServerCreate = DateTime.Now,
                DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                DTServerLastModified = DateTime.Now,
            };

            long accountId = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccount.Id).CXNId;

            long cxeMoneyTransferId = CXEMoneyTransferService.Create(cxeMoneyTransferStage);

            if (mgiContext.ProviderId == 0)
            {
                var provider = _GetMoneyTransferProvider(channelPartner.Name);
                if (!string.IsNullOrEmpty(provider))
                {
                    mgiContext.ProviderId = (int)Enum.Parse(typeof(ProviderIds), provider);
                }
                else
                {
                    mgiContext.ProviderId = (int)ProviderIds.WesternUnion;
                }
            }
            mgiContext.RMMTCN = confirmationNumber;
            GetCustomerSessionCounterId(customerSession, ref mgiContext);


            WritePTNRRMMoneyTransfer(customerSession, cxeAccount, cxeMoneyTransferId, 0L, mgiContext);

            CxnData.TransactionRequest transactionRequest = new CxnData.TransactionRequest()
            {
                ConfirmationNumber = confirmationNumber,
                AccountId = accountId,
                TransactionRequestType = CxnData.TransactionRequestType.ReceiveTransaction
            };

            CxnData.Transaction transaction = _GetMoneyTransferProcessor(channelPartner.Name).GetTransaction(transactionRequest, mgiContext); ;


            //

            // update ptnr txn for payment details
            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(cxeMoneyTransferId);
            ptnrMoneyTransfer.Amount = transaction.DestinationPrincipalAmount;
            ptnrMoneyTransfer.CXNId = Convert.ToInt64(transaction.TransactionID);
            ptnrMoneyTransfer.Fee = 0.00M;
            PTNRMoneyTransferService.Update(ptnrMoneyTransfer);

            // update  cxe for payment details

            cxeMoneyTransferStage moneyTransfer = CXEMoneyTransferService.GetStage(cxeMoneyTransferId);
            moneyTransfer.Amount = transaction.GrossTotalAmount;
            moneyTransfer.DestinationAmount = transaction.DestinationPrincipalAmount;
            moneyTransfer.Fee = transaction.Fee;
            CXEMoneyTransferService.Update(moneyTransfer, mgiContext.TimeZone);

            transaction.TransactionID = cxeMoneyTransferId.ToString(); // In cxn , cxn id is stroed in same property,,
            // cxnid overwritten by cxeid ???
            // The down code is WU specific - ???
            string senderStateCode = Convert.ToString(NexxoUtil.GetDictionaryValueIfExists(transaction.MetaData, "SenderStateCode"));

            if (transaction.OriginatingCountryCode.Equals("US") || transaction.OriginatingCountryCode.Equals("USA"))
            {
                string originatingCountry = PTNRDataStructureService.GetCountry("US");

                if (!string.IsNullOrEmpty(senderStateCode))
                {
                    string originatingState = senderStateCode.Substring((senderStateCode.Length - 3), 2);
                    senderStateCode = PTNRDataStructureService.GetIDState(originatingCountry, originatingState);
                }
                else
                {
                    senderStateCode = originatingCountry;
                }
            }
            else
            {
                senderStateCode = PTNRDataStructureService.GetCountry(transaction.OriginatingCountryCode);
            }

            transaction.MetaData["SenderStateCode"] = senderStateCode;

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<cxnTransaction>(customerSessionId, transaction, "Get", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End Get-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return Mapper.Map<cxnTransaction, MoneyTransferTransaction>(transaction);
        }

        public CardDetails WUCardEnrollment(long customerSessionId, PaymentDetails paymentDetails, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<PaymentDetails>(customerSessionId, paymentDetails, "WUCardEnrollment", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin WUCardEnrollment-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            CustomerSession session = CustomerSessionService.Lookup(customerSessionId);
            CxeCustomer cxeCustomer = CXECustomerService.Lookup(session.Customer.CXEId);

            cxnAccount account = new cxnAccount()
            {
                Address = cxeCustomer.Address1,
                City = cxeCustomer.City,
                PostalCode = cxeCustomer.ZipCode,
                State = cxeCustomer.State,
                ContactPhone = cxeCustomer.Phone1,
                Email = cxeCustomer.Email,
                FirstName = cxeCustomer.FirstName,
                LastName = cxeCustomer.LastName,
                MobilePhone = GetCustomerMobileNumber(cxeCustomer)

            };

            CardDetails cDetails = new CardDetails();
            cxnPaymentDetails cxnPaymentDetails = Mapper.Map<bizPaymentDetails, cxnPaymentDetails>(paymentDetails);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(session.Customer.ChannelPartnerId);

            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }

            GetCustomerSessionCounterId(session, ref mgiContext);

            cxnCardDetails cxnCardDetails = _GetMoneyTransferProcessor(channelPartner.Name).WUCardEnrollment(account, cxnPaymentDetails, mgiContext);
            cDetails = Mapper.Map<cxnCardDetails, bizCardDetails>(cxnCardDetails);

            string WUGoldCardNumber = cDetails.AccountNumber;
            if (WUGoldCardNumber != string.Empty)
            {
                CxeAccount cxeAccount = GetCXEAccount(session, cxeCustomer, channelPartner, mgiContext);
                long accountId = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccount.Id).CXNId;


                // Dictionary<string, object> cxnContext = GetCxnContext(context);

                cxnAccount cxnAccount = _GetMoneyTransferProcessor(channelPartner.Name).GetAccount(accountId, mgiContext);
                cxnAccount.LoyaltyCardNumber = WUGoldCardNumber;
                _GetMoneyTransferProcessor(channelPartner.Name).UpdateAccount(cxnAccount, mgiContext);
            }

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<CardDetails>(customerSessionId, cDetails, "WUCardEnrollment", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End WUCardEnrollment-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return cDetails;
        }

        public CardLookupDetails WUCardLookup(long customerSessionId, CardLookupDetails LookupDetails, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<CardLookupDetails>(customerSessionId, LookupDetails, "WUCardLookup", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin WUCardLookup-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);
            CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
            CxeAccount cxeAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.MoneyTransfer);
            cxnCardLookupDetails cxnCardLookupDetails = null;

            long customerAccountId = 0;
            if (cxeAccount != null)
            {
                customerAccountId = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccount.Id).CXNId;
            }

            cxnCardLookupDetails = Mapper.Map<CardLookupDetails, cxnCardLookupDetails>(LookupDetails);

            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }
            GetCustomerSessionCounterId(customerSession, ref mgiContext);

            cxnCardLookupDetails = _GetMoneyTransferProcessor(channelPartner.Name).WUCardLookup(customerAccountId, cxnCardLookupDetails, mgiContext);

            bizCardLookupDetails bizCardLookupDetails = new bizCardLookupDetails();

            int cnt = 0;
            bizCardLookupDetails.Sender = new bizAccount[cxnCardLookupDetails.Sender.Count()];
            foreach (MGI.Cxn.MoneyTransfer.Data.Account sender in cxnCardLookupDetails.Sender)
            {
                bizCardLookupDetails.Sender[cnt] = new MGI.Biz.MoneyTransfer.Data.Account()
                {
                    Address = sender.Address,
                    FirstName = sender.FirstName,
                    LastName = sender.LastName,
                    MiddleName = sender.MiddleName,
                    SecondLastName = sender.SecondLastName,
                    LoyaltyCardNumber = sender.LoyaltyCardNumber,
                    MobilePhone = sender.MobilePhone,
                    PostalCode = sender.PostalCode
                };
                cnt++;
            }
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<bizCardLookupDetails>(customerSessionId, bizCardLookupDetails, "WUCardLookup", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End WUCardLookup-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return bizCardLookupDetails;
        }

        public bool GetAccount(long customerSessionId, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetAccount", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin GetAccount-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);
            CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
            CxeAccount cxeAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.MoneyTransfer);
            if (cxeAccount != null)
            {
                long customerAccountId = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccount.Id).CXNId;
                #region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(customerAccountId), "GetAccount", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                          "End GetAccount-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
                #endregion
                return _GetMoneyTransferProcessor(channelPartner.Name).GetWUCardAccount(customerAccountId);
            }
            else
            {
                #region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<CxeAccount>(customerSessionId, cxeAccount, "GetAccount", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                          "End GetAccount-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
                #endregion
                return false;
            }
        }

        public bizAccount DisplayWUCardAccountInfo(long customerSessionId, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "DisplayWUCardAccountInfo", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin DisplayWUCardAccountInfo-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);
            CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
            CxeAccount cxeAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.MoneyTransfer);
            long customerAccountId = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccount.Id).CXNId;

            MGI.Cxn.MoneyTransfer.Data.Account WUAccountDetails = new cxnAccount();
            WUAccountDetails = _GetMoneyTransferProcessor(channelPartner.Name).DisplayWUCardAccountInfo(customerAccountId);

            MGI.Biz.MoneyTransfer.Data.Account SenderDetails = new bizAccount()
            {
                LoyaltyCardNumber = WUAccountDetails.LoyaltyCardNumber
            };
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<MGI.Biz.MoneyTransfer.Data.Account>(customerSessionId, SenderDetails, "DisplayWUCardAccountInfo", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End DisplayWUCardAccountInfo-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return SenderDetails;
        }

        public void Cancel(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionId), "Cancel", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin Cancel-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(transactionId);

            CustomerSession customerSession = GetCustomerSession(customerSessionId);

            CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);

            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);

            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }

            // Update CXE Space
            CXEMoneyTransferService.Update(ptnrMoneyTransfer.CXEId, TransactionStates.Canceled, mgiContext.TimeZone);

            // Update Partner Space
            PTNRMoneyTransferService.UpdateStates(ptnrMoneyTransfer.Id, (int)TransactionStates.Canceled, (int)TransactionStates.Canceled);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<ChannelPartner>(customerSessionId, channelPartner, "Cancel", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End Cancel-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
        }

        public void AddPastReceivers(long customerSessionId, string cardNumber, MGIContext mgiContext)
        {
            try
            {
                #region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<string>(customerSessionId, cardNumber, "AddPastReceivers", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                          "Begin AddPastReceivers-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
                #endregion
                //context has to be framed in server.impl - Todo
                CustomerSession customerSession = GetCustomerSession(customerSessionId);
                if (string.IsNullOrEmpty(mgiContext.TimeZone))
                {
                    mgiContext.TimeZone = customerSession.AgentSession.Terminal.Location.TimezoneID;
                }

                if (!String.IsNullOrEmpty(mgiContext.ProductType) && (mgiContext.ProductType.ToUpper() == "SENDMONEY"))
                {
                    if (!string.IsNullOrEmpty(cardNumber))
                        GetCustomerSessionCounterId(customerSession, ref mgiContext);
                    CXNMoneyTransferService.GetPastReceivers(customerSession.Customer.Id, cardNumber, mgiContext);
                }
                #region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<string>(customerSessionId, string.Empty, "AddPastReceivers", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                          "End AddPastReceivers-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
                #endregion
            }

            catch (MoneyTransferException mte)
            {
                throw new MoneyTransferException(MoneyTransferException.CUSTOMER_NAME_NOT_MATCH, mte.Message, mte);
            }

        }

        public CardInfo GetCardInfo(long customerSessionId, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "GetCardInfo", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin GetCardInfo-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            MGI.Cxn.MoneyTransfer.Data.CardInfo cxnResponse = new MGI.Cxn.MoneyTransfer.Data.CardInfo();
            CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);
            CardInfo cardInfo;

            CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
            CxeAccount cxeAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.MoneyTransfer);

            long customerAccountId = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccount.Id).CXNId;
            MGI.Cxn.MoneyTransfer.Data.Account WUAccountDetails = _GetMoneyTransferProcessor(channelPartner.Name).DisplayWUCardAccountInfo(customerAccountId);

            GetCustomerSessionCounterId(customerSession, ref mgiContext);

            // This is a quick fix for the reference error.
            if (!string.IsNullOrEmpty(WUAccountDetails.LoyaltyCardNumber))
            {
                cxnResponse = _GetMoneyTransferProcessor(channelPartner.Name).GetCardInfo(WUAccountDetails.LoyaltyCardNumber, mgiContext);
                cardInfo = Mapper.Map<CardInfo>(cxnResponse);
            }
            else
                cardInfo = null;

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<CardInfo>(customerSessionId, cardInfo, "GetCardInfo", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End GetCardInfo-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return cardInfo;
        }

        /// <summary>
        /// Get Pay Status - US1687
        /// </summary>
        /// <param name="confirmationNumber"></param>
        /// <param name="context"></param>
        /// <returns></returns>

        public string GetStatus(long customerSessionId, string confirmationNumber, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, confirmationNumber, "GetStatus", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin GetStatus-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }
            CustomerSession customerSession = CustomerSessionService.Lookup(customerSessionId);
            GetCustomerSessionCounterId(customerSession, ref mgiContext);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<CustomerSession>(customerSessionId, customerSession, "GetStatus", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "End GetStatus-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName).GetStatus(confirmationNumber, mgiContext);
        }

        /// <summary>
        /// Modify Send Money Search - US1685
        /// </summary>
        /// <param name="mtcn"></param>
        /// <param name="referenceNo"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SearchResponse Search(long customerSessionId, SearchRequest request, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<SearchRequest>(customerSessionId, request, "Search", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin Search-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }
            CustomerSession session = CustomerSessionService.Lookup(customerSessionId);
            GetCustomerSessionCounterId(session, ref mgiContext);
            CxnData.SearchRequest searchRequest = new CxnData.SearchRequest();

            CxnData.SearchResponse searchResponse = new CxnData.SearchResponse();

            PTNRMoneyTransfer ptnrMoneyTransfer = null;

            if (request.SearchRequestType == SearchRequestType.Refund || request.SearchRequestType == SearchRequestType.RefundWithStage)
            {
                searchRequest.ReasonCode = request.ReasonCode;
                searchRequest.ReasonDesc = request.ReasonDesc;
                searchRequest.Comments = request.Comments;

                if (request.SearchRequestType == SearchRequestType.Refund)
                    searchRequest.SearchRequestType = CxnData.SearchRequestType.Refund;
                else
                {
                    searchRequest.SearchRequestType = CxnData.SearchRequestType.RefundWithStage;
                    ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(request.TransactionId);
                    searchRequest.TransactionId = ptnrMoneyTransfer.CXNId;
                }
            }
            else if (request.SearchRequestType == SearchRequestType.Lookup)
            {
                searchRequest.SearchRequestType = CxnData.SearchRequestType.LookUp;
            }
            else
            {
                searchRequest.SearchRequestType = CxnData.SearchRequestType.Modify;
            }
            searchRequest.ConfirmationNumber = request.ConfirmationNumber;

            MGI.Cxn.MoneyTransfer.Data.SearchResponse cxnResponse = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName).Search(searchRequest, mgiContext);

            SearchResponse bizSearchResponse = Mapper.Map<CxnData.SearchResponse, SearchResponse>(cxnResponse);

            if (request.SearchRequestType == SearchRequestType.RefundWithStage)
            {

                //Creating Trx and getting CXEAccount in CXE

                CxeAccount cxeAccount;
                long cxeMoneyTransferRefundID, cxeMoneyTransferCancelID;

                MGI.Cxn.MoneyTransfer.Data.TransactionRequest cxnRequest = new CxnData.TransactionRequest()
                {
                    TransactionId = ptnrMoneyTransfer.CXNId
                };

                MGI.Cxn.MoneyTransfer.Data.Transaction moneyTransferTran = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName).GetTransaction(cxnRequest, mgiContext);

                request.ReceiverFirstName = moneyTransferTran.ReceiverFirstName;
                request.ReceiverLastName = moneyTransferTran.ReceiverLastName;
                request.Amount = ptnrMoneyTransfer.Amount;
                request.Fee = ptnrMoneyTransfer.Fee;
                // New CXE
                cxeMoneyTransferCancelID = WriteCXEMoneyTransfer(session, request, mgiContext, out cxeAccount);
                cxeMoneyTransferRefundID = WriteCXEMoneyTransfer(session, request, mgiContext, out cxeAccount);

                request.TransactionSubType = ((int)TransactionSubTypes.MTType.Cancel).ToString();
                request.OriginalTransactionId = request.TransactionId;
                //Creating Partner Tx 
                WritePTNRMoneyTransfer(session, request, cxeAccount, cxeMoneyTransferCancelID, cxnResponse.CancelTransactionId);

                request.TransactionSubType = ((int)TransactionSubTypes.MTType.Refund).ToString();
                //Creating Partner Tx 
                WritePTNRMoneyTransfer(session, request, cxeAccount, cxeMoneyTransferRefundID, cxnResponse.RefundTransactionId);

                bizSearchResponse.CancelTransactionId = cxeMoneyTransferCancelID;//As PTNR And CXE Ids  are same
                bizSearchResponse.RefundTransactionId = cxeMoneyTransferRefundID;
            }
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<SearchResponse>(customerSessionId, bizSearchResponse, "Search", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "End Search-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return bizSearchResponse;
        }

        /// <summary>
        ///  Initiate Send Money Modify - US1685
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public ModifyResponse StageModify(long customerSessionId, ModifyRequest modifySendMoney, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<ModifyRequest>(customerSessionId, modifySendMoney, "StageModify", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin StageModify-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }
            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(modifySendMoney.TransactionId);
            CustomerSession session = CustomerSessionService.Lookup(customerSessionId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(session.Customer.ChannelPartnerId);
            CxeCustomer customer = CXECustomerService.Lookup(session.Customer.CXEId);
            //Creating Trx and getting CXEAccount in CXE
            CxeAccount cxeAccount;
            long cxeMoneyTransferModifyID, cxeMoneyTransferCancelID;
            GetCustomerSessionCounterId(session, ref mgiContext);
            modifySendMoney.Amount = ptnrMoneyTransfer.Amount;
            modifySendMoney.Fee = ptnrMoneyTransfer.Fee;
            // New CXE
            cxeMoneyTransferCancelID = WriteCXEMoneyTransfer(session, modifySendMoney, mgiContext, out cxeAccount);
            cxeMoneyTransferModifyID = WriteCXEMoneyTransfer(session, modifySendMoney, mgiContext, out cxeAccount);


            MGI.Cxn.MoneyTransfer.Data.ModifyRequest cxnModifyRequest = new MGI.Cxn.MoneyTransfer.Data.ModifyRequest();
            cxnModifyRequest.FirstName = modifySendMoney.FirstName;
            cxnModifyRequest.SecondLastName = modifySendMoney.SecondLastName;
            cxnModifyRequest.MiddleName = modifySendMoney.MiddleName;
            cxnModifyRequest.LastName = modifySendMoney.LastName;
            cxnModifyRequest.TestQuestion = modifySendMoney.TestQuestion;
            cxnModifyRequest.TestAnswer = modifySendMoney.TestAnswer;
            cxnModifyRequest.TransactionId = ptnrMoneyTransfer.CXNId;
            CxnData.ModifyResponse modifySendMoneyResponse = _GetMoneyTransferProcessor(channelPartner.Name).StageModify(cxnModifyRequest, mgiContext);

            modifySendMoney.TransactionSubType = ((int)TransactionSubTypes.MTType.Cancel).ToString();
            modifySendMoney.OriginalTransactionId = modifySendMoney.TransactionId;
            //Creating Partner Tx 
            WritePTNRMoneyTransfer(session, modifySendMoney, cxeAccount, cxeMoneyTransferCancelID, modifySendMoneyResponse.CancelTransactionId);

            modifySendMoney.TransactionSubType = ((int)TransactionSubTypes.MTType.Modify).ToString();
            //Creating Partner Tx 
            WritePTNRMoneyTransfer(session, modifySendMoney, cxeAccount, cxeMoneyTransferModifyID, modifySendMoneyResponse.ModifyTransactionId);

            Biz.MoneyTransfer.Data.ModifyResponse bizModifySendMoneyResponse = new Biz.MoneyTransfer.Data.ModifyResponse()
            {
                CancelTransactionId = cxeMoneyTransferCancelID,
                ModifyTransactionId = cxeMoneyTransferModifyID
            };//As PTNR And CXE Ids  are same

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<Biz.MoneyTransfer.Data.ModifyResponse>(customerSessionId, bizModifySendMoneyResponse, "StageModify", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "End StageModify-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return bizModifySendMoneyResponse;
        }

        public long StageRefund(long customerSessionId, RefundRequest refundRequest, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<RefundRequest>(customerSessionId, refundRequest, "StageRefund", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin StageRefund-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }
            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(refundRequest.TransactionId);
            CustomerSession session = CustomerSessionService.Lookup(customerSessionId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(session.Customer.ChannelPartnerId);

            //Creating Trx and getting CXEAccount in CXE
            CxeAccount cxeAccount;
            long cxeTrxId;

            //Calculate amount based on RefundType
            if (refundRequest.RefundStatus == RefundType.FullAmount.ToString())
            {
                refundRequest.Amount = ptnrMoneyTransfer.Amount + ptnrMoneyTransfer.Fee;
            }
            else
            {
                refundRequest.Amount = ptnrMoneyTransfer.Amount;
            }
            refundRequest.Fee = 0;

            cxeTrxId = WriteCXEMoneyTransfer(session, refundRequest, mgiContext, out cxeAccount);

            //Creating CXN Tx
            MGI.Cxn.MoneyTransfer.Data.RefundRequest cxnRefundRequest = new MGI.Cxn.MoneyTransfer.Data.RefundRequest();
            cxnRefundRequest.ReasonDesc = refundRequest.ReasonDesc;
            cxnRefundRequest.ReasonCode = refundRequest.ReasonCode;
            cxnRefundRequest.RefundStatus = refundRequest.RefundStatus;
            cxnRefundRequest.FeeRefund = refundRequest.FeeRefund;
            cxnRefundRequest.TransactionId = ptnrMoneyTransfer.CXNId;
            cxnRefundRequest.ReferenceNumber = refundRequest.ConfirmationNumber;
            long cxnTransactionId = _GetMoneyTransferProcessor(channelPartner.Name).StageRefund(cxnRefundRequest, mgiContext);

            //Creating Partner Tx
            refundRequest.OriginalTransactionId = refundRequest.TransactionId;
            WritePTNRMoneyTransfer(session, refundRequest, cxeAccount, cxeTrxId, cxnTransactionId);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(cxeTrxId), "StageRefund", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End StageRefund-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return cxeTrxId;
        }

        /// <summary>
        /// Authorize SendMoneyModify - US1685 4th
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="modifySendMoney"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void AuthorizeModify(long customerSessionId, ModifyRequest modifySendMoney, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<ModifyRequest>(customerSessionId, modifySendMoney, "AuthorizeModify", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin AuthorizeModify-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            CXEMoneyTransferService.Update(modifySendMoney.CancelTransactionId, TransactionStates.Authorized, mgiContext.TimeZone);
            PTNRMoneyTransferService.UpdateStates(modifySendMoney.CancelTransactionId, (int)TransactionStates.Authorized, (int)TransactionStates.Authorized);

            CXEMoneyTransferService.Update(modifySendMoney.ModifyTransactionId, TransactionStates.Authorized, mgiContext.TimeZone);
            PTNRMoneyTransferService.UpdateStates(modifySendMoney.ModifyTransactionId, (int)TransactionStates.Authorized, (int)TransactionStates.Authorized);
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, string.Empty, "AuthorizeModify", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "End AuthorizeModify-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
        }

        /// <summary>
        /// Modify Send Money Transaction - US1685 - 5th Modify Send 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="transactionId"></param>
        /// <param name="modifySendMoneySearch"></param>
        /// <param name="context"></param>
        /// <returns></returns> 
        [Transaction(Spring.Transaction.TransactionPropagation.RequiresNew)]
        public int Modify(long customerSessionId, ModifyRequest modifySendMoney, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<ModifyRequest>(customerSessionId, modifySendMoney, "Modify", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin Modify-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }
            CustomerSession session = CustomerSessionService.Lookup(customerSessionId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(session.Customer.ChannelPartnerId);
            CxeCustomer customer = CXECustomerService.Lookup(session.Customer.CXEId);

            GetCustomerSessionCounterId(session, ref mgiContext);

            // update the ptnr to commit
            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(modifySendMoney.CancelTransactionId);
            CXEMoneyTransferService.Update(modifySendMoney.CancelTransactionId, TransactionStates.Committed, mgiContext.TimeZone);
            PTNRMoneyTransferService.UpdateStates(modifySendMoney.CancelTransactionId, (int)TransactionStates.Committed, (int)TransactionStates.Committed);

            ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(modifySendMoney.ModifyTransactionId);
            // update the cxe to commit
            CXEMoneyTransferService.Update(modifySendMoney.ModifyTransactionId, TransactionStates.Committed, mgiContext.TimeZone);

            _GetMoneyTransferProcessor(channelPartner.Name).Modify(ptnrMoneyTransfer.CXNId, mgiContext);
            // update the ptnr to commit	
            PTNRMoneyTransferService.UpdateStates(modifySendMoney.ModifyTransactionId, (int)TransactionStates.Committed, (int)TransactionStates.Committed);

            // update the receiver name and commit

            CxnData.TransactionRequest request = new CxnData.TransactionRequest()
            {
                TransactionId = ptnrMoneyTransfer.CXNId
            };
            cxnTransaction cxnTransaction = _GetMoneyTransferProcessor(channelPartner.Name).GetTransaction(request, mgiContext);
            UpdateReceiverName(customerSessionId, cxnTransaction, mgiContext);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(TransactionStates.Committed), "Modify", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End Modify-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return (int)TransactionStates.Committed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <param name="refundSendMoney"></param>
        /// <param name="refundCancelKey"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        //public List<MoneyTransferModifyTransaction> RefundSearch(long customerSessionId, RefundRequest refundSendMoney, out string refundCancelKey, Dictionary<string, object> context)
        //{
        //	string timezone = GetTimeZone(context);

        //	Dictionary<string, object> cxnContext = GetCxnContext(context);

        //	CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

        //	ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(session.Customer.ChannelPartnerId);

        //	CxeCustomer customer = CXECustomerService.Lookup(session.Customer.CXEId);


        //	GetCustomerSessionCounterId(session, ref cxnContext);

        //	List<MoneyTransferModifyTransaction> sendmoneyTransactions = new List<MoneyTransferModifyTransaction>();
        //	string refundflag;

        //	if (refundSendMoney.TransactionId == 0) // only do search and return
        //	{
        //		_GetMoneyTransferProcessor(channelPartner.Name).SearchSendMoneyRefund(refundSendMoney.TransactionId, searchRequest, out refundflag, GetCxnContext(context));
        //		refundCancelKey = refundflag;
        //		return sendmoneyTransactions;
        //	}
        //	else
        //	{
        //		CxeAccount cxeAccount;
        //		long cxeMoneyTransferRefundID, cxeMoneyTransferCancelID;

        //if (refundSendMoney.TransactionID == 0) // only do search and return
        //{
        //	_GetMoneyTransferProcessor(channelPartner.Name).SearchSendMoneyRefund(refundSendMoney.TransactionID, searchRequest, out refundflag, cxnContext);
        //	refundCancelKey = refundflag;
        //	return sendmoneyTransactions;
        //}
        //else
        //{
        //	CxeAccount cxeAccount;
        //	long cxeMoneyTransferRefundID, cxeMoneyTransferCancelID;

        //		MGI.Cxn.MoneyTransfer.Data.Transaction moneyTransferTran = _GetMoneyTransferProcessor(channelPartner.Name).GetTransaction(request, context);

        //		PaymentDetails paymentDetails = new PaymentDetails();
        //		paymentDetails.Fee = moneyTransferTran.Fee;
        //		paymentDetails.OriginatorsPrincipalAmount = moneyTransferTran.TransactionAmount;
        //		decimal messageCharge = NexxoUtil.GetDecimalDictionaryValueIfExists(moneyTransferTran.MetaData, "MessageCharge");
        //		decimal otherCharges = NexxoUtil.GetDecimalDictionaryValueIfExists(moneyTransferTran.MetaData, "OtherCharges");
        //		paymentDetails.MessageCharge = messageCharge;
        //		paymentDetails.OtherFees = otherCharges;
        //		// Writing Cancel Record
        //		if (!cxnContext.ContainsKey("TrxSubType"))
        //			cxnContext.Add("TrxSubType", (int)TransactionSubTypes.MTType.Cancel);

        //		// New CXE
        //		WriteCXEMoneyTransfer(paymentDetails, session, moneyTransferTran.Receiver.Id, out cxeAccount, out cxeMoneyTransferCancelID);

        //		long CanceltranID = 0; //_GetMoneyTransferProcessor(channelPartner.Name).StageSendMoneyModify(null, cxnContext); // context carry cancelled

        //		paymentDetails.TransactionSubType = ((int)TransactionSubTypes.MTType.Cancel).ToString();
        //		paymentDetails.OriginalTransactionId = refundSendMoney.TransactionId;

        //		//Creating Partner Tx 
        //		WritePTNRMoneyTransfer(paymentDetails, session, cxeAccount, cxeMoneyTransferCancelID, CanceltranID);

        //		cxnContext.Remove("TrxSubType");
        //		cxnContext.Add("TrxSubType", (int)TransactionSubTypes.MTType.Refund);

        //		WriteCXEMoneyTransfer(paymentDetails, session, moneyTransferTran.Receiver.Id, out cxeAccount, out cxeMoneyTransferRefundID);

        //		long RefundtranID = _GetMoneyTransferProcessor(channelPartner.Name).SearchSendMoneyRefund(ptnrMoneyTransfer.CXNId, searchRequest, out refundflag, cxnContext); // context carry refund , creating trx in CXN
        //		refundCancelKey = refundflag;

        //		paymentDetails.TransactionSubType = ((int)TransactionSubTypes.MTType.Refund).ToString();
        //		//Creating Partner Tx 
        //		WritePTNRMoneyTransfer(paymentDetails, session, cxeAccount, cxeMoneyTransferRefundID, RefundtranID);

        //		CXEMoneyTransferService.Update(cxeMoneyTransferCancelID, TransactionStates.Authorized, timezone);
        //		PTNRMoneyTransferService.UpdateStates(cxeMoneyTransferCancelID, (int)TransactionStates.Authorized, (int)TransactionStates.Authorized);

        //		CXEMoneyTransferService.Update(cxeMoneyTransferRefundID, TransactionStates.Authorized, timezone);
        //		PTNRMoneyTransferService.UpdateStates(cxeMoneyTransferRefundID, (int)TransactionStates.Authorized, (int)TransactionStates.Authorized);

        //		sendmoneyTransactions.Add(new MoneyTransferModifyTransaction { TransactionID = cxeMoneyTransferRefundID, TransactionSubType = (int)TransactionSubTypes.MTType.Refund });
        //		sendmoneyTransactions.Add(new MoneyTransferModifyTransaction { TransactionID = cxeMoneyTransferCancelID, TransactionSubType = (int)TransactionSubTypes.MTType.Cancel });

        //		return sendmoneyTransactions;
        //	}
        //}

        public string Refund(long customerSessionId, RefundRequest refundSendMoney, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<RefundRequest>(customerSessionId, refundSendMoney, "Refund", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin Refund-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            CustomerSession session = CustomerSessionService.Lookup(customerSessionId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(session.Customer.ChannelPartnerId);
            CxeCustomer customer = CXECustomerService.Lookup(session.Customer.CXEId);

            if (mgiContext.ProviderId == 0)
            {
                mgiContext.ProviderId = GetProviderID(mgiContext.ChannelPartnerName);
            }
            GetCustomerSessionCounterId(session, ref mgiContext);
            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(refundSendMoney.CancelTransactionId);

            CXEMoneyTransferService.Update(refundSendMoney.CancelTransactionId, TransactionStates.Committed, mgiContext.TimeZone);
            // update the ptnr to commit	
            PTNRMoneyTransferService.UpdateStates(refundSendMoney.CancelTransactionId, (int)TransactionStates.Committed, (int)TransactionStates.Committed);

            ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(refundSendMoney.RefundTransactionId);
            // update the cxe to commit
            CXEMoneyTransferService.Update(refundSendMoney.RefundTransactionId, TransactionStates.Committed, mgiContext.TimeZone);

            cxnRefundMTRequest cxnRefund = new cxnRefundMTRequest()
            {
                ReasonDesc = refundSendMoney.ReasonDesc,
                ReasonCode = refundSendMoney.ReasonCode,
                Comments = refundSendMoney.Comments,
                RefundStatus = refundSendMoney.RefundStatus,
                TransactionId = ptnrMoneyTransfer.CXNId
            };

            string confirmationNumber = _GetMoneyTransferProcessor(channelPartner.Name).Refund(cxnRefund, mgiContext);

            // update the ptnr to commit	
            PTNRMoneyTransferService.UpdateStates(refundSendMoney.RefundTransactionId, (int)TransactionStates.Committed, (int)TransactionStates.Committed);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, confirmationNumber, "Refund", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "End Refund-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return confirmationNumber;
        }

        #endregion

        #region Private Methods

        private CustomerSession GetCustomerSession(long customerSessionId)
        {
            CustomerSession session = CustomerSessionService.Lookup(customerSessionId);

            if (session == null)
            {
                throw new TransferException(TransferException.INVALID_SESSION);
            }

            return session;

        }



        //private void CreateCustomerSessionCounterId(CustomerSession session, string counterId, Dictionary<string, object> context)
        //{
        //	CustomerSessionCounter customerSessionCounterId = new CustomerSessionCounter()
        //	{
        //		CustomerSession = session,
        //		CounterId = counterId,
        //		DTCreate = DateTime.Now,
        //		DTLastMod = DateTime.Now
        //	};

        //	session.CustomerSessionCounter = customerSessionCounterId;

        //	CustomerSessionService.Save(session);

        //}

        private void UpdateLocationCounterIdStatus(Guid locationRowGuid, string counterId, int providerId, bool isAvailable, MGIContext mgiContext)
        {
            LocationCounterId locationCounterId = LocationCounterIdService.Get(locationRowGuid, counterId, providerId);

            if (locationCounterId != null)
            {
                locationCounterId.IsAvailable = isAvailable;
                locationCounterId.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(mgiContext.TimeZone);
                LocationCounterIdService.Update(locationCounterId);
            }
        }





        private long WriteCXEMoneyTransfer(CustomerSession customerSession, SearchRequest searchRequest, MGIContext mgiContext, out CxeAccount cxeAccount)
        {
            cxeAccount = null;
            long cxeMoneyTransferId = 0;
            CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(cxeCustomer.ChannelPartnerId);

            if (cxeCustomer != null)
            {
                cxeAccount = GetCXEAccount(customerSession, cxeCustomer, channelPartner, mgiContext);
                cxeMoneyTransferStage cxeMoneyTransferStage = new cxeMoneyTransferStage
                {
                    ReceiverName = searchRequest.ReceiverFirstName + " " + searchRequest.ReceiverLastName,
                    Account = cxeAccount,
                    Amount = searchRequest.Amount,
                    Fee = searchRequest.Fee,
                    Status = (int)TransactionStates.Initiated,
                    DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                    DTServerCreate = DateTime.Now,
                    DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                    DTServerLastModified = DateTime.Now,
                };
                cxeMoneyTransferId = CXEMoneyTransferService.Create(cxeMoneyTransferStage);
            }

            return cxeMoneyTransferId;
        }

        private long WriteCXEMoneyTransfer(CustomerSession customerSession, FeeRequest feeRequest, MGIContext mgiContext, out CxeAccount cxeAccount)
        {
            long cxeMoneyTransferId = 0L;
            cxeAccount = null;
            CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(cxeCustomer.ChannelPartnerId);
            if (cxeCustomer != null)
            {
                cxeAccount = GetCXEAccount(customerSession, cxeCustomer, channelPartner, mgiContext);
                bizReceiver receiver = GetReceiver(customerSession.Id, feeRequest.ReceiverId, mgiContext);

                cxeMoneyTransferStage cxeMoneyTransferStage = new cxeMoneyTransferStage
                {
                    ReceiverName = receiver != null ? string.Format("{0} {1}", receiver.FirstName, receiver.LastName) : "",
                    Account = cxeAccount,
                    Amount = feeRequest.Amount, /// bizpaymentDetails.OriginatorsPrincipalAmount,

                    //Added for User Story # 1684
                    //Fee =  bizpaymentDetails.Fee + bizpaymentDetails.MessageCharge + bizpaymentDetails.OtherFees,
                    Status = (int)TransactionStates.Initiated,

                    DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                    DTServerCreate = DateTime.Now,
                    DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                    DTServerLastModified = DateTime.Now,
                };

                cxeMoneyTransferId = CXEMoneyTransferService.Create(cxeMoneyTransferStage);
            }

            return cxeMoneyTransferId;
        }

        private long WriteCXEMoneyTransfer(CustomerSession customerSession, RefundRequest refundRequest, MGIContext mgiContext, out CxeAccount cxeAccount)
        {
            cxeAccount = null;
            long cxeMoneyTransferId = 0;
            CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(cxeCustomer.ChannelPartnerId);

            if (cxeCustomer != null)
            {
                cxeAccount = GetCXEAccount(customerSession, cxeCustomer, channelPartner, mgiContext);
                cxeMoneyTransferStage cxeMoneyTransferStage = new cxeMoneyTransferStage
                {
                    Account = cxeAccount,
                    Amount = refundRequest.Amount,
                    Fee = refundRequest.Fee,
                    ConfirmationNumber = refundRequest.ConfirmationNumber,
                    Status = (int)TransactionStates.Authorized,
                    DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                    DTServerCreate = DateTime.Now,
                    DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                    DTServerLastModified = DateTime.Now,
                };
                cxeMoneyTransferId = CXEMoneyTransferService.Create(cxeMoneyTransferStage);
            }

            return cxeMoneyTransferId;
        }
        private CxeAccount GetCXEAccount(CustomerSession customerSession, CxeCustomer cxeCustomer, ChannelPartner channelPartner, MGIContext mgiContext)
        {
            string timezone = customerSession.TimezoneID;
            mgiContext.TimeZone = timezone;

            CxeAccount cxeAccount = null;
            if (cxeCustomer.Accounts.Count(x => x.Type == (int)AccountTypes.MoneyTransfer) == 0)
            {
                // Add CXE Account
                cxeCustomer.Accounts.Add(CXEAccountService.AddCustomerMoneyTransferAccount(cxeCustomer));

                cxeAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.MoneyTransfer);

                cxnAccount account = new cxnAccount()
                {
                    Address = cxeCustomer.Address1,
                    City = cxeCustomer.City,
                    PostalCode = cxeCustomer.ZipCode,
                    State = cxeCustomer.State,
                    ContactPhone = cxeCustomer.Phone1,
                    Email = cxeCustomer.Email,
                    FirstName = cxeCustomer.FirstName,
                    LastName = cxeCustomer.LastName,
                    MiddleName = cxeCustomer.MiddleName,
                    SecondLastName = cxeCustomer.LastName2,
                    MobilePhone = GetCustomerMobileNumber(cxeCustomer),
                    LoyaltyCardNumber = "",
                    LevelCode = "",
                    SmsNotificationFlag = cxeCustomer.SMSEnabled ? "Y" : "N"
                };

                // Add CXN Account
                long cxnAccountID = _GetMoneyTransferProcessor(channelPartner.Name).AddAccount(account, mgiContext);

                //Changes for MGI 
                // Create PTNR account , Map CXE and CXN Account Ids.
                var provider = _GetMoneyTransferProvider(channelPartner.Name);

                if (provider == null)
                    customerSession.Customer.AddAccount((int)ProviderIds.WesternUnion, cxeAccount.Id, cxnAccountID);
                else
                    customerSession.Customer.AddAccount((int)Enum.Parse(typeof(ProviderIds), provider), cxeAccount.Id, cxnAccountID);
                //Changes for MGI 

                CustomerSessionService.Save(customerSession);
            }
            else
            {
                cxeAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.MoneyTransfer);
                long cxnAccountId = PTNRCustomerService.Lookup(cxeCustomer.Id).FindAccountByCXEId(cxeAccount.Id).CXNId;
                if (cxnAccountId <= 0)
                    throw new BizMoneyTransferException(BizMoneyTransferException.ACCOUNT_NOT_FOUND);
                cxnAccount cxnAccount = _GetMoneyTransferProcessor(channelPartner.Name).GetAccount(cxnAccountId, mgiContext);
                cxnAccount.Address = cxeCustomer.Address1;
                cxnAccount.City = cxeCustomer.City;
                cxnAccount.PostalCode = cxeCustomer.ZipCode;
                cxnAccount.State = cxeCustomer.State;
                cxnAccount.ContactPhone = cxeCustomer.Phone1;
                cxnAccount.Email = cxeCustomer.Email;
                cxnAccount.FirstName = cxeCustomer.FirstName;
                cxnAccount.LastName = cxeCustomer.LastName;
                cxnAccount.MobilePhone = GetCustomerMobileNumber(cxeCustomer);
                cxnAccount.SmsNotificationFlag = cxeCustomer.SMSEnabled ? "Y" : "N";
                _GetMoneyTransferProcessor(channelPartner.Name).UpdateAccount(cxnAccount, mgiContext);
            }
            return cxeAccount;
        }

        private void WritePTNRMoneyTransfer(FeeRequest feeRequest, CustomerSession customerSession, CxeAccount cxeAccount, long cxeMoneyTransferId, long cxnMoneyTransferId)
        {
            PTNRMoneyTransfer ptnrMoneyTransfer = new PTNRMoneyTransfer
            {
                Id = cxeMoneyTransferId,
                CXEId = cxeMoneyTransferId,
                CXNId = cxnMoneyTransferId,
                Amount = feeRequest.Amount,

                //Added for User Story # US1684
                //Fee = paymentDetails.Fee + paymentDetails.MessageCharge + paymentDetails.OtherFees,
                CustomerSession = customerSession,
                CXEState = (int)TransactionStates.Initiated,
                CXNState = (int)TransactionStates.Initiated,
                TransferType = (int)TransferType.SendMoney,
                //timestamp changes
                DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                DTServerCreate = DateTime.Now,
                Account = customerSession.Customer.FindAccountByCXEId(cxeAccount.Id),
                OriginalTransactionId = feeRequest.TransactionId, // paymentDetails.OriginalTransactionId,
            };

            PTNRMoneyTransferService.Create(ptnrMoneyTransfer);
        }

        private long WriteCXEMoneyTransfer(CustomerSession customerSession, bizModifySendMoney modifySendMoneyRequest, MGIContext mgiContext, out CxeAccount cxeAccount)
        {
            cxeAccount = null;
            long cxeMoneyTransferId = 0;
            CxeCustomer cxeCustomer = CXECustomerService.Lookup(customerSession.Customer.CXEId);
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(cxeCustomer.ChannelPartnerId);

            if (cxeCustomer != null)
            {
                cxeAccount = GetCXEAccount(customerSession, cxeCustomer, channelPartner, mgiContext);
                cxeMoneyTransferStage cxeMoneyTransferStage = new cxeMoneyTransferStage
                {
                    ReceiverName = modifySendMoneyRequest.FirstName + " " + modifySendMoneyRequest.LastName,
                    Account = cxeAccount,
                    Amount = modifySendMoneyRequest.Amount,
                    Fee = modifySendMoneyRequest.Fee,
                    Status = (int)TransactionStates.Initiated,
                    DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                    DTServerCreate = DateTime.Now,
                    DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                    DTServerLastModified = DateTime.Now,
                };
                cxeMoneyTransferId = CXEMoneyTransferService.Create(cxeMoneyTransferStage);
            }

            return cxeMoneyTransferId;
        }

        private void WritePTNRMoneyTransfer(CustomerSession customerSession, ModifyRequest modifySendMoneyRequest, CxeAccount cxeAccount, long cxeMoneyTransferId, long cxnMoneyTransferId)
        {
            PTNRMoneyTransfer ptnrMoneyTransfer = new PTNRMoneyTransfer
            {
                Id = cxeMoneyTransferId,
                CXEId = cxeMoneyTransferId,
                CXNId = cxnMoneyTransferId,
                Amount = modifySendMoneyRequest.Amount,
                Fee = modifySendMoneyRequest.Fee,
                CustomerSession = customerSession,
                CXEState = (int)TransactionStates.Initiated,
                CXNState = (int)TransactionStates.Initiated,
                TransferType = (int)TransferType.SendMoney,
                DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                DTServerCreate = DateTime.Now,
                Account = customerSession.Customer.FindAccountByCXEId(cxeAccount.Id),
                OriginalTransactionId = modifySendMoneyRequest.OriginalTransactionId,
                TransactionSubType = modifySendMoneyRequest.TransactionSubType
            };

            PTNRMoneyTransferService.Create(ptnrMoneyTransfer);
        }

        private void WritePTNRMoneyTransfer(CustomerSession customerSession, RefundRequest refundRequest, CxeAccount cxeAccount, long cxeMoneyTransferId, long cxnMoneyTransferId)
        {
            PTNRMoneyTransfer ptnrMoneyTransfer = new PTNRMoneyTransfer
            {
                Id = cxeMoneyTransferId,
                CXEId = cxeMoneyTransferId,
                CXNId = cxnMoneyTransferId,
                Amount = refundRequest.Amount,
                Fee = refundRequest.Fee,
                ConfirmationNumber = refundRequest.ConfirmationNumber,
                CustomerSession = customerSession,
                CXEState = (int)TransactionStates.Authorized,
                CXNState = (int)TransactionStates.Authorized,
                TransferType = (int)TransferType.Refund,
                DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                DTServerCreate = DateTime.Now,
                Account = customerSession.Customer.FindAccountByCXEId(cxeAccount.Id),
                OriginalTransactionId = refundRequest.OriginalTransactionId
            };

            PTNRMoneyTransferService.Create(ptnrMoneyTransfer);
        }

        //	GetCustomerSessionCounterId(customerSession, ref context);

        //	cxnMoneyTransferId = _GetMoneyTransferProcessor(channelPartner.Name).Initiate(cxnId, receiverId, sender, cxnpaymentDetails, out cxnTranFinancials, context);

        //	PTNRMoneyTransferService.Create(ptnrMoneyTransfer);
        //}
        private void WritePTNRMoneyTransfer(CustomerSession customerSession, SearchRequest searchRequest, CxeAccount cxeAccount, long cxeMoneyTransferId, long cxnMoneyTransferId)
        {
            PTNRMoneyTransfer ptnrMoneyTransfer = new PTNRMoneyTransfer
            {
                Id = cxeMoneyTransferId,
                CXEId = cxeMoneyTransferId,
                CXNId = cxnMoneyTransferId,
                Amount = searchRequest.Amount,
                Fee = searchRequest.Fee,
                CustomerSession = customerSession,
                CXEState = (int)TransactionStates.Initiated,
                CXNState = (int)TransactionStates.Initiated,
                TransferType = (int)TransferType.SendMoney,
                DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(customerSession.TimezoneID),
                DTServerCreate = DateTime.Now,
                Account = customerSession.Customer.FindAccountByCXEId(cxeAccount.Id),
                OriginalTransactionId = searchRequest.OriginalTransactionId,
                TransactionSubType = searchRequest.TransactionSubType
            };

            PTNRMoneyTransferService.Create(ptnrMoneyTransfer);
        }
        private bool ValidateReceiver(bizReceiver receiver)
        {
            //TODO
            return true;
        }

        private MGI.Cxn.MoneyTransfer.Contract.IMoneyTransfer _GetMoneyTransferProcessor(string channelPartner)
        {
            // get the moneytransfer processor for the channel partner.
            return (MGI.Cxn.MoneyTransfer.Contract.IMoneyTransfer)MoneyTransferProcessorSvc.GetProcessor(channelPartner);
        }

        private string _GetMoneyTransferProvider(string channelPartner)
        {
            // get the moneytransfer provider for the channel partner.
            return MoneyTransferProcessorSvc.GetProvider(channelPartner);
        }

        private void WritePTNRRMMoneyTransfer(CustomerSession customerSession, CxeAccount cxeAccount, long cxeMoneyTransferId, long cxnMoneyTransferId, MGIContext mgiContext)
        {
            PTNRMoneyTransfer ptnrMoneyTransfer = new PTNRMoneyTransfer
            {
                Id = cxeMoneyTransferId,
                CXEId = cxeMoneyTransferId,
                CXNId = cxnMoneyTransferId,
                CustomerSession = customerSession,
                CXEState = (int)TransactionStates.Initiated,
                CXNState = (int)TransactionStates.Initiated,
                DTServerCreate = DateTime.Now,
                Account = customerSession.Customer.FindAccountByCXEId(cxeAccount.Id),
                TransferType = (int)TransferType.ReceiveMoney,
                ConfirmationNumber = mgiContext.RMMTCN
            };

            PTNRMoneyTransferService.Create(ptnrMoneyTransfer);
        }

        private int GetProviderID(string channelPartnerName)
        {
            int providerID = 0;
            string provider = _GetMoneyTransferProvider(channelPartnerName);
            if (!string.IsNullOrEmpty(provider))
            {
                providerID = (int)Enum.Parse(typeof(ProviderIds), provider);
            }
            return providerID;
        }

        private long StageMoneyTransfer(long customerSessionId, FeeRequest feeRequest, MGIContext mgiContext)
        {
            CustomerSession customerSession = GetCustomerSession(customerSessionId);

            //Creating Trx and getting CXEAccount in CXE
            CxeAccount cxeAccount;
            long cxeMoneyTransferId = WriteCXEMoneyTransfer(customerSession, feeRequest, mgiContext, out cxeAccount);

            //Creating Partner Tx 
            long cxnMoneyTransferId = 0L;

            WritePTNRMoneyTransfer(feeRequest, customerSession, cxeAccount, cxeMoneyTransferId, cxnMoneyTransferId);

            return cxeMoneyTransferId;
        }

        private long UpdateMoneyTransferStage(long transactionId, Data.FeeResponse feeResponse, MGIContext mgiContext)
        {
            FeeInformation feeInfo = feeResponse.FeeInformations.FirstOrDefault() != null ? feeResponse.FeeInformations.FirstOrDefault() : new FeeInformation();
            // checks key exists in metadata and handles null
            decimal transferTax = Convert.ToDecimal(NexxoUtil.GetDecimalDictionaryValueIfExists(feeInfo.MetaData, "TransferTax"));
            decimal fee = feeInfo.Fee + transferTax;
            if (feeInfo.MetaData != null)
            {
                decimal plusCharges = Convert.ToDecimal(NexxoUtil.GetDecimalDictionaryValueIfExists(feeInfo.MetaData, "PlusCharges"));
                fee = fee + plusCharges + feeInfo.MessageFee;
            }

            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(transactionId);
            ptnrMoneyTransfer.Fee = fee;
            ptnrMoneyTransfer.CXNId = feeResponse.TransactionId;
            PTNRMoneyTransferService.Update(ptnrMoneyTransfer);
            feeResponse.TransactionId = ptnrMoneyTransfer.Id; // writing back transaction id to payment details
            return feeResponse.TransactionId;
        }

        private void UpdateMoneyTransferStage(Data.ValidateRequest validateRequest, MGIContext mgiContext)
        {
            decimal fee = validateRequest.Fee + validateRequest.OtherFee + validateRequest.MessageFee + validateRequest.Tax;

            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(validateRequest.TransactionId);

            ptnrMoneyTransfer.Amount = validateRequest.Amount;
            ptnrMoneyTransfer.Fee = fee;
            PTNRMoneyTransferService.Update(ptnrMoneyTransfer);

            // update  cxe for payment details

            cxeMoneyTransferStage moneyTransfer = CXEMoneyTransferService.GetStage(ptnrMoneyTransfer.CXEId);
            // Has to check for other Fields has to be updated
            moneyTransfer.Amount = validateRequest.Amount;
            moneyTransfer.DestinationAmount = validateRequest.ReceiveAmount;

            moneyTransfer.Fee = validateRequest.Fee + validateRequest.MessageFee + validateRequest.OtherFee + validateRequest.Tax;
            CXEMoneyTransferService.Update(moneyTransfer, mgiContext.TimeZone);

            validateRequest.TransactionId = ptnrMoneyTransfer.Id; // writing back transaction id to payment details
            return;
        }

        private long UpdateMoneyTransferStage(FeeRequest feeRequest, string timeZone)
        {
            // update ptnr txn for payment details
            long transactionId = 0L;
            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(feeRequest.TransactionId);
            transactionId = ptnrMoneyTransfer.CXNId;
            PTNRMoneyTransferService.UpdateAmount(feeRequest.TransactionId, feeRequest.Amount);

            // update  cxe for payment details
            cxeMoneyTransferStage moneyTransfer = CXEMoneyTransferService.GetStage(ptnrMoneyTransfer.CXEId);
            // Has to check for other Fields has to be updated
            moneyTransfer.Amount = feeRequest.Amount;
            moneyTransfer.DestinationAmount = feeRequest.ReceiveAmount;

            CXEMoneyTransferService.Update(moneyTransfer, timeZone);

            return transactionId;
        }

        private long UpdateInitiate(long customerSessionId, long receiverId, bizPaymentDetails paymentDetails, MGIContext mgiContext)
        {
            string timezone = mgiContext.TimeZone != null ? mgiContext.TimeZone : string.Empty;

            if (string.IsNullOrEmpty(timezone))
                throw new Exception("Time zone not provided in the context");

            // update ptnr txn for payment details
            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(paymentDetails.TransactionId);

            PTNRMoneyTransferService.UpdateAmount(paymentDetails.TransactionId, paymentDetails.OriginatorsPrincipalAmount);
            PTNRMoneyTransferService.UpdateFee(paymentDetails.TransactionId, paymentDetails.Fee + paymentDetails.OtherFees + paymentDetails.MessageCharge);

            // update  cxe for payment details

            cxeMoneyTransferStage moneyTransfer = CXEMoneyTransferService.GetStage(ptnrMoneyTransfer.CXEId);
            // Has to check for other Fields has to be updated
            moneyTransfer.Amount = paymentDetails.OriginatorsPrincipalAmount;
            moneyTransfer.DestinationAmount = paymentDetails.DestinationPrincipalAmount;

            //Added for User Story # US1684;

            moneyTransfer.Fee = paymentDetails.Fee + paymentDetails.MessageCharge + paymentDetails.OtherFees;
            CXEMoneyTransferService.Update(moneyTransfer, timezone);

            // update cxn txn for payment details

            CustomerSession session = CustomerSessionService.Lookup(customerSessionId);
            CxeCustomer cxeCustomer = CXECustomerService.Lookup(session.Customer.CXEId);
            CxeAccount cxeAccount = cxeCustomer.Accounts.FirstOrDefault(x => x.Type == (int)AccountTypes.MoneyTransfer);
            paymentDetails.TransactionId = ptnrMoneyTransfer.CXNId;

            //WriteCxnMoneyTransfer(receiverId, cxeAccount.Id, paymentDetails, session, context);

            paymentDetails.TransactionId = ptnrMoneyTransfer.Id; // writing back transaction id to payment details
            return paymentDetails.TransactionId; // As PaymentDetails object carrying CXN id not referring back paymentdetails.transactionid
        }

        private string GetCustomerMobileNumber(CxeCustomer cxeCustomer)
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

        private IMoneyTransfer GetCxnTransferService(CustomerSession customerSession)
        {
            ChannelPartner channelPartner = PTNRChannelPartnerService.ChannelPartnerConfig(customerSession.Customer.ChannelPartnerId);

            IMoneyTransfer CXNMoneyTransfer = _GetMoneyTransferProcessor(channelPartner.Name);
            return CXNMoneyTransfer;
        }

        private void UpdateReceiverName(long customerSessionId, cxnTransaction cxnTransaction, MGIContext mgiContext)
        {
            bizReceiver reciever = GetReceiver(customerSessionId, cxnTransaction.Receiver.Id, mgiContext);

            if (cxnTransaction.Receiver != null)
            {
                if (!string.IsNullOrEmpty(cxnTransaction.Receiver.FirstName))
                    reciever.FirstName = cxnTransaction.Receiver.FirstName;
                if (!string.IsNullOrEmpty(cxnTransaction.Receiver.LastName))
                    reciever.LastName = cxnTransaction.Receiver.LastName;
                if (!string.IsNullOrEmpty(cxnTransaction.Receiver.MiddleName))
                    reciever.MiddleName = cxnTransaction.Receiver.MiddleName;
                if (!string.IsNullOrEmpty(cxnTransaction.Receiver.SecondLastName))
                    reciever.SecondLastName = cxnTransaction.Receiver.SecondLastName;
            }
            if (!string.IsNullOrEmpty(cxnTransaction.DestinationCountryCode))
                reciever.PickupCountry = cxnTransaction.DestinationCountryCode;
            if (!string.IsNullOrEmpty(cxnTransaction.ExpectedPayoutStateCode))
                reciever.PickupState_Province = cxnTransaction.ExpectedPayoutStateCode;
            else { reciever.PickupState_Province = null; }

            string expectedPayoutCityName = NexxoUtil.GetDictionaryValueIfExists(cxnTransaction.MetaData, "ExpectedPayoutCity").ToNullSafeString();

            if (!string.IsNullOrEmpty(expectedPayoutCityName))
                reciever.PickupCity = expectedPayoutCityName;
            else { reciever.PickupCity = null; }

            string deliveryMethod = NexxoUtil.GetDictionaryValueIfExists(cxnTransaction.MetaData, "DeliveryOption").ToNullSafeString();
            if (!string.IsNullOrEmpty(deliveryMethod))
                reciever.DeliveryMethod = deliveryMethod;

            EditReceiver(customerSessionId, reciever, mgiContext);
        }

        #endregion

        public List<DeliveryService> GetDeliveryServices(long customerSessionId, DeliveryServiceRequest request, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<DeliveryServiceRequest>(customerSessionId, request, "GetDeliveryServices", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "Begin GetDeliveryServices-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            IMoneyTransfer moneyTransferProcessor = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName);

            CxnData.DeliveryServiceRequest cxnRequest = Mapper.Map<CxnData.DeliveryServiceRequest>(request);
            request.CustomerSessionId = request.CustomerSessionId == 0 ? mgiContext.CustomerSessionId : request.CustomerSessionId;
            CustomerSession customerSession = CustomerSessionService.Lookup(request.CustomerSessionId);
            GetCustomerSessionCounterId(customerSession, ref mgiContext);

            List<CxnData.DeliveryService> deliveryServices = moneyTransferProcessor.GetDeliveryServices(cxnRequest, mgiContext);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.ListInfo<CxnData.DeliveryService>(customerSessionId, deliveryServices, "GetDeliveryServices", AlloyLayerName.BIZ, ModuleName.SendMoney,
                                      "End GetDeliveryServices-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            return Mapper.Map<List<CxnData.DeliveryService>, List<Data.DeliveryService>>(deliveryServices);

        }

        public List<Field> GetProviderAttributes(long customerSessionId, AttributeRequest attributeRequest, MGIContext mgiContext)
        {
            try
            {
                #region AL-3370 Transactional Log User Story
                MongoDBLogger.Info<AttributeRequest>(customerSessionId, attributeRequest, "GetProviderAttributes", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                          "Begin GetProviderAttributes-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
                #endregion
                if (mgiContext.TrxId != 0)
                {
                    PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(mgiContext.TrxId);
                    mgiContext.CxnTransactionId = ptnrMoneyTransfer.CXNId;
                }
                IMoneyTransfer moneyTransferProcessor = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName);

                var cxnAttributeRequest = Mapper.Map<CxnData.AttributeRequest>(attributeRequest);
                List<CxnData.Field> fields = moneyTransferProcessor.GetProviderAttributes(cxnAttributeRequest, mgiContext);

                #region AL-3370 Transactional Log User Story
                MongoDBLogger.ListInfo<CxnData.Field>(customerSessionId, fields, "GetProviderAttributes", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                          "End GetProviderAttributes-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
                #endregion
                return Mapper.Map<List<Field>>(fields);
            }
            catch (Exception ex)
            {
                throw new TransferException(TransferException.PROVIDER_ERROR, ex);
            }
        }

        public void UpdateTransactionStatus(long customerSessionId, long transactionId, MGIContext mgiContext)
        {
            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<string>(customerSessionId, Convert.ToString(transactionId), "UpdateTransactionStatus", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "Begin UpdateTransactionStatus-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
            PTNRMoneyTransfer ptnrMoneyTransfer = PTNRMoneyTransferService.Lookup(transactionId);

            // Update CXE Space
            CXEMoneyTransferService.Update(ptnrMoneyTransfer.CXEId, TransactionStates.Failed, mgiContext.TimeZone);

            // Update Partner Space
            PTNRMoneyTransferService.UpdateStates(ptnrMoneyTransfer.Id, (int)TransactionStates.Failed, (int)TransactionStates.Failed);

            #region AL-3370 Transactional Log User Story
            MongoDBLogger.Info<PTNRMoneyTransfer>(customerSessionId, ptnrMoneyTransfer, "UpdateTransactionStatus", AlloyLayerName.BIZ, ModuleName.MoneyTransfer,
                                      "End UpdateTransactionStatus-MGI.Biz.MoneyTransfer.Impl.MoneyTransferEngine", mgiContext);
            #endregion
        }
    }
}
