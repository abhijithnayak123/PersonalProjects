﻿using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.Fund.Data;
using TCF.Zeo.Cxn.Fund.Data.Exceptions;
using TCF.Zeo.Cxn.Fund.Visa.Data.Exceptions;
using TCF.Zeo.Cxn.Fund.Visa.Contract;
using TCF.Zeo.Cxn.Fund.Visa.Prepaid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace TCF.Zeo.Cxn.Fund.Visa.Impl
{
    public class IO : IIO
    {
        #region Private Members

        private const long FundingAccountAliasId = -1;
        private const long LocationNodeId = -1;
        private const string CurrencyCode = "840";
        private const string CountryCode = "USA";
        private const string UseStandardizedAddress = "0";
        private const string RegistrationFeeNumber = "1000";
        private const string LoadFeeNumber = "1057";
        private const string WithdrawFeeNumber = "1011";
        private const string WithdrawCheckPaperFeeNumber = "1049";
        private const string CardIdentifierType_Proxy = "1";
        private const string EmbossType = "1";
        private const string PhoneNumberKind = "1";
        private const string FundingSourcePaymentType = "99";
        private const string BindingName = "VisaPrepaidServices";
        private const string ReplaceStockId = "127CS202";
        private const string ReplaceFeeNumber = "1008";
        private const string MailOrderFeeNumber = "1013";
        private const string InstantIssueFeeNumber = "1111";
        private const double InstantIssueAmount = 10;
        private const double InstantIssueOriginalAmount = 0;
        private const string CardIdentifierTypePan = "0";
        private const string CardIdentifierTypeProxyId = "1";
        private const string CardIdentifierTypeAccountNumber = "2";
        private const string CompanianCardPhoneNumberKind = "2";
        private const string CompanianCardUseStandardizedAddress = "3";
        private const string CompanianCardDeliveryFeeNumber = "1002";

        #endregion

        public void Diagnostics(Credential credential)
        {
            PrepaidServicesClient proxy = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            GetDiagnosticMessageRequestType messageRequest = new GetDiagnosticMessageRequestType()
            {
                ConnectionConfirmationRequest = new ConfirmConnectionRequestType()
            };

            try
            {
                GetDiagnosticMessageResponseType response = proxy.GetDiagnosticMessage(ref claimListHeader, messageRequest);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public CardInfo GetCardInfoByProxyId(string proxyId, Credential credential, ZeoContext context)
        {
            CardInfo cardInformation = new CardInfo();

            PrepaidServicesClient proxy = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            GetSearchMessageRequestType messageRequest = new GetSearchMessageRequestType()
            {
                CardholderSearchRequest = new CardholderSearchRequestType()
                {
                    Item = new SearchCardholderByProxyIdType()
                    {
                        CardProgramIdentifier = new CardProgramIdentifierType()
                        {
                            CardProgramNodeId = credential.CardProgramNodeId,
                            ClientNodeId = credential.ClientNodeId
                        },
                        Paging = new PagingParametersType()
                        {
                            MaxRows = 1,
                            StartRow = 0
                        },
                        ProxyId = proxyId,
                    }
                }
            };

            try
            {
                GetSearchMessageResponseType response = proxy.GetSearchMessage(ref claimListHeader, messageRequest);
                if (response != null && response.CardholderSearchResponse != null && response.CardholderSearchResponse.CardholderSearchList.Any())
                {
                    CardholderSearchResultType cardholderSearch = response.CardholderSearchResponse.CardholderSearchList.FirstOrDefault();
                    if (cardholderSearch != null)
                    {
                        if (cardholderSearch.CardholderIdentifier != null)
                        {
                            cardInformation.AliasId = cardholderSearch.CardholderIdentifier.AliasId;
                        }

                        if (cardholderSearch.CardSummary != null)
                        {
                            cardInformation.CardNumber = cardholderSearch.CardSummary.Pan;
                            if (cardholderSearch.CardSummary.CardAvailableBalance != null)
                            {
                                cardInformation.Balance = cardholderSearch.CardSummary.CardAvailableBalance.Amount;
                                cardInformation.CurrencyCode = cardholderSearch.CardSummary.CardAvailableBalance.CurrencyCode;
                            }

                            if (cardholderSearch.CardSummary.CardStatus != null)
                            {
                                cardInformation.Status = cardholderSearch.CardSummary.CardStatus.Value;
                            }

                            if (cardholderSearch.CardSummary.CardExpiryDate != null)
                            {
                                cardInformation.ExpirationMonth = cardholderSearch.CardSummary.CardExpiryDate.Month;
                                cardInformation.ExpirationYear = cardholderSearch.CardSummary.CardExpiryDate.Year;
                            }

                            if (cardholderSearch.CardSummary.CardOrderDate != null)
                            {
                                cardInformation.CardIssueDate = cardholderSearch.CardSummary.CardOrderDate;
                            }
                        }

                        if (cardholderSearch.CardholderIdentifier != null)
                        {
                            cardInformation.SubClientNodeId = cardholderSearch.CardholderIdentifier.SubClientNodeId;
                        }
                        cardInformation.ProxyId = cardholderSearch.ProxyId;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new FundException(FundException.CARD_INFORMATION_RETRIEVAL_ERROR, ex);
            }

            return cardInformation;
        }

        public string GetPsedoDDAFromAliasId(long aliasId, Credential credential, ZeoContext context)
        {
            string accountNumber = string.Empty;

            PrepaidServicesClient prepaidServicesClient = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            GetCardholderMessageRequestType messageRequest = new GetCardholderMessageRequestType()
            {
                AchAccountDetailsRequest = new AliasIdRequestType()
                {
                    AliasId = aliasId,
                }
            };

            try
            {
                GetCardholderMessageResponseType response = prepaidServicesClient.GetCardholderMessage(ref claimListHeader, messageRequest);

                if (response != null && response.AchAccountDetailsResponse != null && response.AchAccountDetailsResponse.AccountDetails != null)
                {
                    accountNumber = response.AchAccountDetailsResponse.AccountDetails.AccountNumber;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new FundException(FundException.CARD_INFORMATION_RETRIEVAL_ERROR, ex);
            }
            return accountNumber;
        }

        public CardPurchaseResponse IssueCard(CardAccount account, CustomerInfo customer, double initialLoadAmount, CardInfo cardInformation, Credential credential, ZeoContext context)
        {
            CardPurchaseResponse issueCardResponse = null;

            PrepaidServicesClient prepaidServicesClient = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            var cardOrderDetails = new CardOrderDetailsType()
            {
                CardIdentifier = new InstantIssueCardIdentifierType()
                {
                    CardIdentifier = cardInformation.ProxyId,
                    CardIdentifierType = CardIdentifierType_Proxy,
                },
                EmbossType = EmbossType,
                ExpirationDate = new ExpirationDateType()
                {
                    Month = cardInformation.ExpirationMonth,
                    Year = cardInformation.ExpirationYear
                },
                InitialValueLoad = new MonetaryValueType()
                {
                    Amount = initialLoadAmount,
                    CurrencyCode = CurrencyCode
                },
                IsBuyersCard = true,
                StockId = credential.StockId
            };

            var cardholderDetails = new MailingAddressDemographicUpdateBaseType()
            {
                Address = new UpdatePostalAddressType()
                {
                    AddressLine1 = AlloyUtil.TrimString(customer.Address1, 30),
                    AddressLine2 = AlloyUtil.TrimString(customer.Address2, 30),
                    City = AlloyUtil.TrimString(customer.City, 19),
                    CountryCode = CountryCode,
                    Region = customer.State,
                    ZipCode = customer.ZipCode,
                },
                ChallengeList = new List<UpdateChallengeType>()
                {
                    new UpdateChallengeType()
                    {
                        SecurityQuestion = string.Empty,
                        SecurityAnswer = "-999"
                    },
                },
                DateOfBirth = customer.DateOfBirth,
                EmailAddress = GetTrimmedEmail(customer.Email),
                GovernmentId = new UpdateGovernmentIdType()
                {
                    Country = CountryCode,
                    GovernmentIdIdentifier = customer.SSN,
                    IdKind = MapIDKind(customer.IDCode)
                },
                Name = new PersonNameType()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName
                },
                PhoneNumber = new PhoneNumberType()
                {
                    Number = customer.Phone,
                    Kind = PhoneNumberKind,
                },
                StandardizedAddress = new UpdateStandardizedPostalAddressType()
                {
                    ContainsGenDelAddr = false,
                    ContainsPoBox = false,
                    AddressLine1 = AlloyUtil.TrimString(customer.Address1, 30),
                    AddressLine2 = AlloyUtil.TrimString(customer.Address2, 30),
                    City = AlloyUtil.TrimString(customer.City, 19),
                    CountryCode = CountryCode,
                    Region = customer.State,
                    ZipCode = customer.ZipCode,
                },
                UseStandardizedAddress = UseStandardizedAddress,
            };

            var buyerDemographic = new DemographicUpdateBaseType()
            {
                Address = new UpdatePostalAddressType()
                {
                    AddressLine1 = AlloyUtil.TrimString(customer.Address1, 30),
                    AddressLine2 = AlloyUtil.TrimString(customer.Address2, 30),
                    City = AlloyUtil.TrimString(customer.City, 19),
                    CountryCode = CountryCode,
                    Region = customer.State,
                    ZipCode = customer.ZipCode,
                },
                ChallengeList = new List<UpdateChallengeType>()
                {
                    new UpdateChallengeType()
                    {
                        SecurityQuestion = string.Empty,
                        SecurityAnswer = "-999"
                    },
                },
                DateOfBirth = customer.DateOfBirth,
                EmailAddress = GetTrimmedEmail(customer.Email),
                GovernmentId = new UpdateGovernmentIdType()
                {
                    Country = CountryCode,
                    GovernmentIdIdentifier = customer.SSN,
                    IdKind = MapIDKind(customer.IDCode)
                },
                Name = new PersonNameType()
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName
                },
                PhoneNumber = new PhoneNumberType()
                {
                    Number = customer.Phone,
                    Kind = PhoneNumberKind,
                },
                StandardizedAddress = new UpdateStandardizedPostalAddressType()
                {
                    ContainsGenDelAddr = false,
                    ContainsPoBox = false,
                    AddressLine1 = AlloyUtil.TrimString(customer.Address1, 30),
                    AddressLine2 = AlloyUtil.TrimString(customer.Address2, 30),
                    City = AlloyUtil.TrimString(customer.City, 19),
                    CountryCode = CountryCode,
                    Region = customer.State,
                    ZipCode = customer.ZipCode,
                },
                UseStandardizedAddress = UseStandardizedAddress,
            };

            if (!string.IsNullOrEmpty(customer.MothersMaidenName))
            {
                cardholderDetails.CustomFields = new CustomFieldsType()
                {
                    CustomField01 = new CustomFieldType()
                    {
                        Value = customer.MothersMaidenName
                    }
                };
            }

            UpdateInstantIssueMessageRequestType message = new UpdateInstantIssueMessageRequestType()
            {
                IssueReloadableCardRequest = new IssueCardRequestType()
                {
                    CardOrderList = new List<CardOrderType>()
                    {
                        new CardOrderType
                        {
                            CardOrderDetails = cardOrderDetails,
                            CardholderDetails = cardholderDetails
                        }
                    },
                    FundingSource = new CreateOrderFundingAccountType()
                    {
                        PaymentType = FundingSourcePaymentType,
                        Item = new FundingAccountCashPaymentType()
                    },
                    CardOptionsIdentifier = new CardOptionsIdentifierType()
                    {
                        CardProgramNodeId = credential.CardProgramNodeId,
                        ClientNodeId = credential.ClientNodeId,
                        SubClientNodeId = cardInformation.SubClientNodeId
                    },
                    BuyerDetails = new UpdateExternalBuyerProfileRequestType()
                    {
                        BuyerAliasId = -1,
                        BuyerDemographic = buyerDemographic
                    }
                }
            };

            if (!string.IsNullOrWhiteSpace(cardInformation.PromotionCode))
            {
                message.IssueReloadableCardRequest.PromotionCode = cardInformation.PromotionCode;
            }

            try
            {
                UpdateInstantIssueMessageResponseType response = prepaidServicesClient.UpdateInstantIssueMessage(ref claimListHeader, message);

                if (response.IssueReloadableCardResponse.IssueCardError != null)
                {
                    throw new VisaProviderException(VisaProviderException.CARD_REGISTRATION_FAILED, response.IssueReloadableCardResponse.IssueCardError.ErrorMessage);
                }

                if (response != null && response.IssueReloadableCardResponse != null)
                {
                    issueCardResponse = new CardPurchaseResponse()
                    {
                        AccountAliasId = response.IssueReloadableCardResponse.AccountAliasId,
                        ConfirmationNumber = response.IssueReloadableCardResponse.TransactionKey
                    };
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);

                throw new FundException(FundException.CARD_ACTIVATION_FAILED, ex);
            }
            return issueCardResponse;
        }

        public CardBalanceInfo GetBalance(long aliasId, Credential credential, ZeoContext context)
        {
            CardBalanceInfo cardBalance = new CardBalanceInfo();

            PrepaidServicesClient prepaidServicesClient = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            GetCardholderMessageRequestType messageRequest = new GetCardholderMessageRequestType()
            {
                CardDetailsRequest = new CardholderDataWithFraudRequestType()
                {
                    AliasId = aliasId,
                }
            };

            try
            {
                GetCardholderMessageResponseType response = prepaidServicesClient.GetCardholderMessage(ref claimListHeader, messageRequest);

                if (response != null && response.CardDetailsResponse != null && response.CardDetailsResponse.CardDetails != null)
                {
                    CardBalancesType balances = response.CardDetailsResponse.CardDetails.Balances;

                    if (balances != null)
                    {
                        cardBalance.Balance = Convert.ToDecimal(balances.Available.Amount);
                        cardBalance.AccountBalance = balances.Ledger.Amount;
                        cardBalance.CardStatus = response.CardDetailsResponse.CardDetails.CardStatus.Value;
                        cardBalance.NewCardNumber = response.CardDetailsResponse.CardDetails.CurrentPan.Pan;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new FundException(FundException.CARD_BALANCE_RETRIEVAL_ERROR, ex);
            }

            return cardBalance;
        }

        public LoadResponse Load(long aliasId, double loadAmount, Credential credential, ZeoContext context)
        {
            LoadResponse visaTransaction = new LoadResponse();

            PrepaidServicesClient prepaidServicesClient = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            UpdateCardholderMessageRequestType message = new UpdateCardholderMessageRequestType()
            {
                CreateCardLoadRequest = new CreateCardLoadRequestType()
                {
                    CardholderAliasId = aliasId,
                    Item = new CreateCardLoadImmediateType()
                    {
                        FundingAccountAliasId = FundingAccountAliasId,
                        LoadAmount = new MonetaryValueType()
                        {
                            Amount = loadAmount,
                            CurrencyCode = CurrencyCode
                        },
                        LoadFee = new FeeType()
                        {
                            IsDefined = false,
                            IsDiverted = false,
                            IsWaived = false,
                            Number = LoadFeeNumber,
                            Value = new MonetaryValueType()
                            {
                                Amount = 0,
                                CurrencyCode = CurrencyCode
                            }
                        },
                        // Note: insert the branch Subclient Node ID where the load is being performed.  
                        // This is for Reload Reconciliation. 
                        LocationNodeId = credential.VisaLocationNodeId // Need to figure out the value need to be passed for this.
                    },
                }
            };

            try
            {
                UpdateCardholderMessageResponseType response = prepaidServicesClient.UpdateCardholderMessage(ref claimListHeader, message);

                if (response != null && response.CreateCardLoadResponse != null)
                {
                    // Note: because this is a real time load, use the transaction ID. 
                    //This value can also be matched to the VERF.   Nexxo will pass this value to TCF to match to the VERF.
                    visaTransaction.TransactionKey = response.CreateCardLoadResponse.TransactionKey;
                    visaTransaction.ReloadAliasId = response.CreateCardLoadResponse.ReloadAliasId;
                    visaTransaction.TransationId = response.CreateCardLoadResponse.TransactionId;
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);

                throw new FundException(FundException.CARD_LOAD_ERROR, ex);
            }

            return visaTransaction;
        }

        public bool Withdraw(long aliasId, double amount, Credential credential, ZeoContext context)
        {
            bool withdrawSuccess = false;

            PrepaidServicesClient prepaidServicesClient = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            UpdateCardholderMessageRequestType message = new UpdateCardholderMessageRequestType()
            {
                CreateCardUnloadRequest = new CreateCardUnloadRequestType()
                {
                    AccountAliasId = FundingAccountAliasId,
                    CardholderAliasId = aliasId,
                    Amount = new MonetaryValueType
                    {
                        Amount = amount,
                        CurrencyCode = CurrencyCode
                    },
                    PaperCheckFee = new FeeType()
                    {
                        IsDefined = false,
                        IsDiverted = false,
                        IsWaived = false,
                        Number = WithdrawCheckPaperFeeNumber,
                        Value = new MonetaryValueType()
                        {
                            Amount = 0,
                            CurrencyCode = CurrencyCode
                        }
                    },
                    UnloadType = "0",
                    ValueUnloadFee = new FeeType()
                    {
                        IsDefined = false,
                        IsDiverted = false,
                        IsWaived = false,
                        Number = WithdrawFeeNumber,
                        Value = new MonetaryValueType()
                        {
                            Amount = 0,
                            CurrencyCode = CurrencyCode
                        }
                    },

                    //TCF is using ‘Reload Reconciliation’ function on their program, they can pass 
                    //the subclient node ID of the location that is performing the unload transaction. 
                    //This will feed reporting for TCF to settle with their branches

                    UnloadLocationNodeId = credential.VisaLocationNodeId  // Need to figure out the value need to be passed for this.
                }
            };

            try
            {
                UpdateCardholderMessageResponseType response = prepaidServicesClient.UpdateCardholderMessage(ref claimListHeader, message);
                withdrawSuccess = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);

                throw new FundException(FundException.CARD_WITHDRAW_ERROR, ex);
            }

            return withdrawSuccess;
        }

        public List<TransactionHistory> GetTransactionHistory(TransactionHistoryRequest request, Credential credential, ZeoContext context)
        {
            List<TransactionHistory> transactionHistoryList = new List<TransactionHistory>();

            PrepaidServicesClient prepaidServicesClient = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            string beginDate = DateTime.Now.AddDays(-request.DateRange).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            string endDate = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");

            GetCardholderMessageRequestType message = new GetCardholderMessageRequestType()
            {
                TransactionHistoryRequest = new CardholderTransactionHistoryRequestType()
                {
                    CardholderAliasId = request.AliasId,
                    BeginDate = Convert.ToDateTime(beginDate),
                    EndDate = Convert.ToDateTime(endDate),
                    IncludeDeniedInResults = request.TransactionStatus == Helper.TransactionStatus.Denied,
                    IncludePendingInResults = request.TransactionStatus == Helper.TransactionStatus.Pending,
                    IncludePostedInResults = request.TransactionStatus == Helper.TransactionStatus.Posted,
                    Paging = new PagingParametersType()
                    {
                        MaxRows = 1000,
                        StartRow = 0
                    }
                }
            };

            try
            {
                GetCardholderMessageResponseType response = prepaidServicesClient.GetCardholderMessage(ref claimListHeader, message);

                if (response != null && response.TransactionHistoryResponse != null && response.TransactionHistoryResponse.TransactionList.Count > 0)
                {
                    foreach (var transaction in response.TransactionHistoryResponse.TransactionList)
                    {
                        string[] locations = new string[1];

                        if (transaction.MerchantAddress != null)
                        {
                            locations = new string[]
                            {
                                transaction.MerchantAddress.City,
                                transaction.MerchantAddress.Region.Display,
                                transaction.MerchantAddress.CountryCode.Display
                            };
                        }

                        transactionHistoryList.Add(new TransactionHistory()
                        {
                            PostedDateTime = transaction.PostedDateTime,
                            TransactionDateTime = transaction.TransactionDateTime,
                            MerchantName = transaction.MerchantName,
                            Location = string.Join(",", locations),
                            TransactionAmount = Math.Abs(transaction.TransactionAmount.Amount),
                            TransactionDescription = transaction.TransactionDescription,
                            AvailableBalance = Math.Abs(transaction.AfterAvailable),
                            ActualBalance = Math.Abs(transaction.AfterLedger),
                            DeclineReason = transaction.ActionCode != null ? transaction.ActionCode.Display : string.Empty
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                HandleException(ex);

                throw new FundException(FundException.CARD_HISTORY_RETRIVEL_ERROR, ex);
            }
            return transactionHistoryList;
        }

        public bool CloseAccount(long aliasId, Credential credential, ZeoContext context)
        {
            PrepaidServicesClient prepaidServicesClient = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            UpdateCardholderMessageRequestType message = new UpdateCardholderMessageRequestType()
            {
                CloseAccountRequest = new UpdateCloseAccountRequestType()
                {
                    AccountClosureFee = new FeeType()
                    {
                        IsDefined = false,
                        IsDiverted = false,
                        IsWaived = false,
                        Number = "1012",
                        Value = new MonetaryValueType()
                        {
                            Amount = 0,
                            CurrencyCode = CurrencyCode
                        }
                    },
                    CardholderIdentifier = new CardholderIdentifierType()
                    {
                        AliasId = aliasId,
                        CardProgramNodeId = credential.CardProgramNodeId,
                        ClientNodeId = credential.ClientNodeId,
                        SubClientNodeId = context.VisaLocationNodeId
                    },
                    PaperCheckFee = new FeeType()
                    {
                        IsDefined = false,
                        IsDiverted = false,
                        IsWaived = false,
                        Number = "1049",
                        Value = new MonetaryValueType()
                        {
                            Amount = 0,
                            CurrencyCode = CurrencyCode
                        }
                    },
                    RefundForm = "1",
                }
            };

            try
            {
                UpdateCardholderMessageResponseType response = prepaidServicesClient.UpdateCardholderMessage(ref claimListHeader, message);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new FundException(FundException.ACCOUNT_CLOSURE_ERROR, ex);
            }

            return true;
        }

        public bool UpdateCardStatus(long aliasId, string cardStatus, Credential credential, ZeoContext context)
        {
            PrepaidServicesClient prepaidServicesClient = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            UpdateCardholderMessageRequestType message = new UpdateCardholderMessageRequestType()
            {
                UpdateCardStatusRequest = new UpdateCardStatusRequestType()
                {
                    CardholderAliasId = aliasId,
                    NewStatus = cardStatus
                }
            };

            try
            {
                UpdateCardholderMessageResponseType response = prepaidServicesClient.UpdateCardholderMessage(ref claimListHeader, message);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new FundException(FundException.CARD_STATUS_UPDATE_ERROR, ex);
            }

            return true;
        }

        public CardPurchaseResponse CompanianCardOrder(long aliasId, Credential credential, CustomerInfo customer, CardAccount cardAccount, ZeoContext context)
        {
            DateTime expiryDate = DateTime.Now.AddMonths(context.CardExpiryPeriod);
            PrepaidServicesClient prepaidServicesClient = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);
            claimListHeader.ClaimStatementList.Find(x => x.Name == "SubClientNodeId").Value = cardAccount.SubClientNodeId.ToString();

            CardPurchaseResponse issuecardResponse = new CardPurchaseResponse();

            try
            {
                UpdateMailOrderMessageRequestType message = new UpdateMailOrderMessageRequestType()
                {
                    PayComMailOrderRequest = new PayComMailOrderRequestType()
                    {
                        BuyerDetails = null,
                        CardOptionsIdentifier = new CardOptionsIdentifierType()
                        {
                            CardProgramNodeId = credential.CardProgramNodeId,
                            ClientNodeId = credential.ClientNodeId,
                            SubClientNodeId = Convert.ToInt64(cardAccount.SubClientNodeId)
                        },
                        CardOrderList = new List<PayComMailOrderType>()
                    {

                        new PayComMailOrderType()
                        {

                            CardOrderDetails = new PayComMailOrderDetailsType()
                            {

                                CardClass = context.CardClass,
                                CustomImage = null,
                                DeliveryFee = new FeeType()
                                {
                                    IsDefined = false,
                                    IsDiverted = false,
                                    IsWaived = false,
                                    Number = CompanianCardDeliveryFeeNumber, // add constant - Done
									Value = new MonetaryValueType()
                                    {
                                        Amount = 0,
                                        CurrencyCode = CurrencyCode,
                                        ExchangeRate = null,
                                        OrginalAmount = 0,
                                        OriginalAlphaCurrencyCode = null,
                                        OriginalCurrencyCode =null
                                    }
                                },
                                EmbossCompanyName = false,
                                EmbossType = "0",
                                EmbossedMessage1 = null,
                                EmbossedMessage2 = null,
                                ExpirationDate= new CardExpirationType()
                                {
                                    Month = expiryDate.Month,
                                    Year = expiryDate.Year
                                },
                                InitialLoadFee = new FeeType()
                                {
                                    IsDefined = false,
                                    IsDiverted = false,
                                    IsWaived = false,
                                    Number = "1054", // add constant
									Value = new MonetaryValueType()
                                    {
                                        Amount = 0,
                                        CurrencyCode = CurrencyCode,
                                        ExchangeRate = null,
                                        OrginalAmount = 0,
                                        OriginalAlphaCurrencyCode = null,
                                        OriginalCurrencyCode =null
                                    }
                                },
                                InitialValueLoad = new MonetaryValueType()
                                {
                                    Amount = 0, //this should be set zero always for companion card
									CurrencyCode = CurrencyCode,
                                    ExchangeRate = null,
                                    OrginalAmount = 0,
                                    OriginalAlphaCurrencyCode = null,
                                    OriginalCurrencyCode = null
                                },
                                IsBuyerCard = false,
                                PromotionCode = null,
                                PurchaseFee = new FeeType()
                                {
                                    IsDefined = false,
                                    IsDiverted = false,
                                    IsWaived = false,
                                    Number = "1047", // add constant
									Value = new MonetaryValueType()
                                    {
                                        Amount = 0,
                                        CurrencyCode = CurrencyCode,
                                        ExchangeRate = null,
                                        OrginalAmount = 0,
                                        OriginalAlphaCurrencyCode = null,
                                        OriginalCurrencyCode = null
                                    }
                                },
                                ShipTo = "1",
                                ShippingMethod = "2",
                                StockId = "967CS002",
                                UseOverriddenExpirationDate = false
                            },
                            // This information is for Companian Code
                            CardholderDetails = new MailingAddressDemographicUpdateBaseType()
                            {
                                Address = new UpdatePostalAddressType()
                                {
                                    AddressLine1 = customer.Address1,
                                    AddressLine2 = customer.Address2,
                                    AddressLine3 = null,
                                    City = customer.City,
                                    CountryCode = CountryCode,
                                    Region = customer.State,
                                    ZipCode = customer.ZipCode
                                },
                                ChallengeList = new List<UpdateChallengeType>()
                                {
                                    new UpdateChallengeType()
                                    {
                                        SecurityAnswer = null,
                                        SecurityQuestion = "-999"
                                    }
                                },
                                DateOfBirth = customer.DateOfBirth,
                                CustomFields = new CustomFieldsType()
                                {
                                    CustomField01 = new CustomFieldType()
                                    {
                                        Value = customer.MothersMaidenName
                                    }
                                },
                                EmailAddress = null,
                                EmployeeId = null,
                                GovernmentId = new UpdateGovernmentIdType()
                                {
                                    Country = CountryCode,
                                    GovernmentIdIdentifier = customer.SSN,
                                    IdKind = MapIDKind(customer.IDCode),
                                    Region = null
                                },
                                LanguagePreference = null,
                                Name = new PersonNameType()
                                {
                                    FirstName = customer.FirstName,
                                    LastName = customer.LastName,
                                    MiddleInitial  = null,
                                    Suffix = null
                                },
                                PhoneNumber = new PhoneNumberType()
                                {
                                    Kind = CompanianCardPhoneNumberKind,
                                    Number = customer.Phone
                                },
                                StandardizedAddress = new UpdateStandardizedPostalAddressType()
                                {
                                        AddressLine1 = customer.Address1,
                                        AddressLine2 = customer.Address2,
                                        AddressLine3 = null,
                                        City = customer.City,
                                        CountryCode = CountryCode,
                                        Region = customer.State,
                                        ZipCode = customer.ZipCode,
                                        ContainsGenDelAddr = false,
                                        ContainsPoBox = false
                                },
                                 UseStandardizedAddress = CompanianCardUseStandardizedAddress,
                                UserName = null
                            },
                            Password = null
                        },
                    },
                        FundingSource = new CreateOrderFundingAccountType()
                        {
                            Item = new FundingAccountNoPaymentType(),
                            PaymentType = "-999"
                        },
                        MailOrderType = "0",
                        PrimaryCardholderAliasId = aliasId
                    }
                };

                UpdateMailOrderMessageResponseType response = prepaidServicesClient.UpdateMailOrderMessage(ref claimListHeader, message);
                if (response != null && response.PayComMailOrderResponse != null)
                {
                    issuecardResponse.AccountAliasId = response.PayComMailOrderResponse.BuyerAliasId;
                    issuecardResponse.ConfirmationNumber = response.PayComMailOrderResponse.OrderConfirmationNumber;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new FundException(FundException.COMPANION_CARD_ORDER_ERROR, ex);
            }
            return issuecardResponse;
        }

        public bool ReplaceCard(long aliasId, CardMaintenanceInfo cardMaintenanceInfo, Credential credential, ZeoContext context)
        {
            PrepaidServicesClient prepaidServicesClient = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            bool isInstantIssue = cardMaintenanceInfo.ShippingType == "-2" || cardMaintenanceInfo.ShippingType == "-3";

            UpdateCardholderMessageRequestType message = new UpdateCardholderMessageRequestType()
            {
                ReplaceCardRequest = isInstantIssue ? GetReplaceCardInstantIssueType(aliasId, cardMaintenanceInfo) : GetReplaceCardPanlessType(aliasId, cardMaintenanceInfo)
            };

            try
            {
                UpdateCardholderMessageResponseType response = prepaidServicesClient.UpdateCardholderMessage(ref claimListHeader, message);
            }
            catch (Exception ex)
            {
                HandleException(ex);

                throw new FundException(FundException.CARD_REPLACEMENT_ERROR, ex);
            }
            return true;
        }

        public CardInfo GetCardInfoByCardNumber(string cardNumber, Credential credential, ZeoContext context)
        {
            CardInfo cardInformation = new CardInfo();

            PrepaidServicesClient proxy = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            GetSearchMessageRequestType messageRequest = new GetSearchMessageRequestType()
            {
                CardholderSearchRequest = new CardholderSearchRequestType()
                {
                    Item = new SearchCardholderByPanType()
                    {
                        CardProgramIdentifier = new CardProgramIdentifierType()
                        {
                            CardProgramNodeId = credential.CardProgramNodeId,
                            ClientNodeId = credential.ClientNodeId
                        },
                        Paging = new PagingParametersType()
                        {
                            MaxRows = 1,
                            StartRow = 0
                        },
                        Pan = cardNumber
                    }
                }
            };

            try
            {
                GetSearchMessageResponseType response = proxy.GetSearchMessage(ref claimListHeader, messageRequest);
                if (response != null && response.CardholderSearchResponse != null && response.CardholderSearchResponse.CardholderSearchList.Any())
                {
                    CardholderSearchResultType cardholderSearch = response.CardholderSearchResponse.CardholderSearchList.FirstOrDefault();
                    if (cardholderSearch != null)
                    {
                        cardInformation.ProxyId = cardholderSearch.ProxyId;

                        if (cardholderSearch.CardholderIdentifier != null)
                        {
                            cardInformation.AliasId = cardholderSearch.CardholderIdentifier.AliasId;
                            cardInformation.SubClientNodeId = cardholderSearch.CardholderIdentifier.SubClientNodeId;
                        }
                        if (cardholderSearch.CardholderDemo != null)
                        {
                            if (cardholderSearch.CardholderDemo.GovernmentId != null)
                            {
                                cardInformation.SSN = cardholderSearch.CardholderDemo.GovernmentId.GovernmentIdIdentifier;
                            }
                            if (cardholderSearch.CardholderDemo != null && cardholderSearch.CardholderDemo.Name != null)
                            {
                                cardInformation.LastName = cardholderSearch.CardholderDemo.Name.LastName;
                            }
                        }
                        cardInformation.ProxyId = cardholderSearch.ProxyId;

                        if (cardInformation.AliasId > 0)
                        {
                            cardInformation.PsedoDDA = GetPsedoDDAFromAliasId(cardInformation.AliasId, credential, context);
                        }

                        if (cardholderSearch.CardSummary != null)
                        {
                            if (cardholderSearch.CardSummary.CardStatus != null)
                            {
                                cardInformation.Status = cardholderSearch.CardSummary.CardStatus.Value;
                            }

                            if (cardholderSearch.CardSummary.CardExpiryDate != null)
                            {
                                cardInformation.ExpirationMonth = cardholderSearch.CardSummary.CardExpiryDate.Month;
                                cardInformation.ExpirationYear = cardholderSearch.CardSummary.CardExpiryDate.Year;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new FundException(FundException.CARD_INFORMATION_RETRIEVAL_ERROR, ex);
            }
            return cardInformation;
        }

        public CardInfo GetCardHolderInfo(long aliasId, Credential credential, ZeoContext context)
        {
            CardInfo cardInformation = new CardInfo();
            bool isPrimaryCardHolder = false;
            List<string> joinHolders = new List<string>();
            PrepaidServicesClient prepaidServicesClient = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);


            GetCardholderMessageRequestType messageRequest = new GetCardholderMessageRequestType()
            {
                AccountHolderDetailsRequest = new CardholderIdentifierRequestType()
                {
                    AliasId = aliasId,
                },
                CardDetailsRequest = new CardholderDataWithFraudRequestType()
                {
                    AliasId = aliasId,
                },
                CardholderJointAccountListRequest = new CardholderAliasIdPagingRequestType()
                {
                    CardholderAliasId = aliasId
                }
            };

            try
            {
                GetCardholderMessageResponseType response = prepaidServicesClient.GetCardholderMessage(ref claimListHeader, messageRequest);

                if (response != null)
                {
                    // empty response if Primary card holder
                    if (response.CardDetailsResponse != null && response.CardDetailsResponse.CardholderIdentifier != null)
                    {
                        isPrimaryCardHolder = response.CardDetailsResponse.IsPrimaryAccount;
                        if (isPrimaryCardHolder)
                        {
                            cardInformation.AliasId = response.CardDetailsResponse.CardholderIdentifier.AliasId;
                        }
                        else if (response.AccountHolderDetailsResponse != null)
                        {
                            cardInformation.PrimaryAliasId = response.AccountHolderDetailsResponse.AccountHolderId;
                        }
                        if (response.CardDetailsResponse.CardDetails.CurrentPan != null)
                        {
                            cardInformation.CardNumber = response.CardDetailsResponse.CardDetails.CurrentPan.Pan;
                        }
                    }

                    // empty response if Companion card holder
                    if (response.CardholderJointAccountListResponse != null && response.CardholderJointAccountListResponse.JointCardholders.Count > 0)
                    {
                        cardInformation.AliasId = aliasId;

                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new FundException(FundException.CARD_INFORMATION_RETRIEVAL_ERROR, ex);
            }
            return cardInformation;
        }

        public AccountHistory GetAccountholderInfo(long aliasId, Credential credential, ZeoContext context)
        {
            PrepaidServicesClient prepaidServicesClient = SetupInternetProxy(credential);
            ClaimListHeader claimListHeader = GetClaimListHeader(credential);

            AccountHistory accountHistory = new AccountHistory();
            GetCardholderMessageRequestType messageRequest = new GetCardholderMessageRequestType()
            {
                AccountHistoryListRequest = new AliasIdPagingRequestType()
                {
                    AliasId = aliasId,
                    Paging = new PagingParametersType()
                    {
                        MaxRows = 1,
                        StartRow = 0
                    }
                }
            };
            try
            {
                GetCardholderMessageResponseType response = prepaidServicesClient.GetCardholderMessage(ref claimListHeader, messageRequest);

                if (response != null)
                {
                    if (response.AccountHistoryListResponse != null)
                    {
                        MaintenanceInformationType accountrepsonse = response.AccountHistoryListResponse.FirstOrDefault();
                        accountHistory.TransactionDate = accountrepsonse.Date;
                        accountHistory.TransactionType = accountrepsonse.Function.Display;
                        accountHistory.AdditionalDetails = accountrepsonse.Notes;
                    }
                }
                return accountHistory;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw new FundException(FundException.CARD_ACCOUNT_HISTORY_RETRIEVAL_ERROR, ex);
            }
        }

        #region Private Methods

        private static PrepaidServicesClient SetupInternetProxy(Credential credential)
        {
            //const string visaServiceUrl = "https://certservicesgateway.visaonline.com/websrv_prepaid/v14_10/prepaidservices";
            PrepaidServicesClient prepaidServicesClient = new PrepaidServicesClient(BindingName, credential.ServiceUrl);

            prepaidServicesClient.Endpoint.Behaviors.Add(new MustUnderstandBehavior(false));

            // Set the username and password for the service account
            prepaidServicesClient.ClientCredentials.UserName.UserName = credential.UserName;
            prepaidServicesClient.ClientCredentials.UserName.Password = credential.Password;

            prepaidServicesClient.ClientCredentials.ClientCertificate
                .SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, credential.CertificateName);

            // Create the base security binding element with UserName/Password
            TransportSecurityBindingElement securityElement = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            securityElement.EnableUnsecuredResponse = true;

            // Create supporting token parameters for the client X509 certificate.
            X509SecurityTokenParameters clientX509SupportingTokenParameters = new X509SecurityTokenParameters();
            // Specify that the supporting token is passed in message send by the client to the service
            clientX509SupportingTokenParameters.InclusionMode = SecurityTokenInclusionMode.AlwaysToRecipient;
            // Turn off derived keys
            clientX509SupportingTokenParameters.RequireDerivedKeys = false;
            // Add the client X509 cert
            securityElement.EndpointSupportingTokenParameters.Endorsing.Add(clientX509SupportingTokenParameters);

            TextMessageEncodingBindingElement quota = new TextMessageEncodingBindingElement(MessageVersion.Soap11, System.Text.Encoding.UTF8);
            quota.ReaderQuotas.MaxDepth = 32;
            quota.ReaderQuotas.MaxStringContentLength = 104857600;
            quota.ReaderQuotas.MaxArrayLength = 104857600;
            quota.ReaderQuotas.MaxBytesPerRead = 4096; // Maximum message size
            quota.ReaderQuotas.MaxNameTableCharCount = 104857600;

            prepaidServicesClient.Endpoint.Binding = new CustomBinding(securityElement, quota, new HttpsTransportBindingElement()
            {
                MaxReceivedMessageSize = 2147483647,
                MaxBufferSize = 2147483647
            });

            return prepaidServicesClient;
        }

        private static ClaimListHeader GetClaimListHeader(Credential credential)
        {
            ClaimListHeader header = new ClaimListHeader()
            {
                ClaimStatementList = new List<ClaimStatement>()
                {
                    new ClaimStatement() { Name = "ClientNodeId",       Value = credential.ClientNodeId.ToString() },
                    new ClaimStatement() { Name = "CardProgramNodeId",  Value = credential.CardProgramNodeId.ToString() },
                    new ClaimStatement() { Name = "SubClientNodeId",    Value = credential.SubClientNodeId.ToString() },
                    new ClaimStatement() { Name = "ServerIdentity",     Value = credential.UserName },
                }
            };
            return header;
        }

        private string MapIDKind(string idCode)
        {
            string idKind = string.Empty;
            switch (idCode)
            {
                case "S":
                    idKind = "1";
                    break;
                case "I":
                    idKind = "8";
                    break;
            }
            return idKind;
        }

        //AL-2999 :As VisaDPS, I need to truncate profile element values sent from Alloy
        public string GetTrimmedEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                if (email.Length > 50)
                    email = string.Empty;
            }
            return email;
        }

        private FeeType GetExpreessFee(string shippingFee, CardMaintenanceInfo cardMaintenanceInfo)
        {
            FeeType fee = new FeeType();
            if (cardMaintenanceInfo.ShippingType == "0") //Below block is executed only when shipping type is Express Delivery
            {
                fee.IsDefined = true;
                fee.IsDiverted = false;
                fee.IsWaived = false;
                fee.Number = Convert.ToString(cardMaintenanceInfo.ShippingFeeCode);
                fee.Value = new MonetaryValueType()
                {
                    Amount = cardMaintenanceInfo.ShippingFee,
                    CurrencyCode = CurrencyCode
                };
            }
            return fee;
        }


        private ReplaceCardRequestType GetReplaceCardInstantIssueType(long aliasId, CardMaintenanceInfo cardMaintenanceInfo)
        {
            return new ReplaceCardRequestType()
            {

                Card = new ReplaceCardChoiceType()
                {
                    Item = new ReplaceCardInstantIssueType()
                    {
                        CardStatus = cardMaintenanceInfo.CardStatus,
                        CardholderAliasId = aliasId,
                        CustomImage = null,
                        ReplacementFee = new FeeType()
                        {
                            IsDefined = true,
                            IsDiverted = false,
                            IsWaived = false,
                            Number = Convert.ToString(cardMaintenanceInfo.ShippingFeeCode),
                            Value = new MonetaryValueType()
                            {
                                Amount = cardMaintenanceInfo.ShippingFee,
                                CurrencyCode = CurrencyCode,
                                ExchangeRate = null,
                                OrginalAmount = InstantIssueOriginalAmount,
                                OriginalAlphaCurrencyCode = null,
                                OriginalCurrencyCode = null
                            }
                        },
                        CardIdentifier = new InstantIssueCardIdentifierType()
                        {
                            CardIdentifier = cardMaintenanceInfo.CardNumber,
                            CardIdentifierType = CardIdentifierTypePan,//TODO: pass const value with name has PAN

                        }
                    }
                }
            };
        }

        private ReplaceCardRequestType GetReplaceCardPanlessType(long aliasId, CardMaintenanceInfo cardMaintenanceInfo)
        {
            ReplaceCardRequestType request = new ReplaceCardRequestType();

            request.Card = new ReplaceCardChoiceType()
            {
                Item = new ReplaceCardPanlessType()
                {
                    CardholderAliasId = aliasId,
                    CardStatus = cardMaintenanceInfo.CardStatus,
                    ReplacementFee = new FeeType()
                    {
                        IsDefined = true,
                        IsDiverted = false,
                        IsWaived = false,
                        Number = Convert.ToString(cardMaintenanceInfo.ReplacementFeeCode),
                        Value = new MonetaryValueType()
                        {
                            Amount = cardMaintenanceInfo.ReplacementFee,
                            CurrencyCode = CurrencyCode
                        }
                    },
                    DeliveryMethod = cardMaintenanceInfo.ShippingType,
                    ExpirationDate = new CardExpirationType()
                    {
                        Month = cardMaintenanceInfo.ExpiryMonth,
                        Year = cardMaintenanceInfo.ExpiryYear
                    },
                    ExpressDeliveryFee = GetExpreessFee(cardMaintenanceInfo.ShippingType, cardMaintenanceInfo),
                    ShipTo = "1", // Mail to card recipient
                    StockId = cardMaintenanceInfo.StockId, // Replacement card stockId
                    UseRecalculatedExpirationDate = false,
                    CardClass = cardMaintenanceInfo.CardClass,
                }
            };

            return request;
        }

        private static void HandleException(Exception ex)
        {
            FaultException<VisaDetail> visaFaultException = ex as FaultException<VisaDetail>;
            if (visaFaultException != null)
            {
                VisaDetail visaDetail = visaFaultException.Detail;
                if (visaDetail != null && visaDetail.VisaErrors != null && visaDetail.VisaErrors.Any())
                {
                    var visaError = visaDetail.VisaErrors.FirstOrDefault();
                    string errorCode = Convert.ToString(visaError.RuleId);
                    throw new VisaProviderException(errorCode, visaError.Description, ex);
                }
            }

            Exception faultException = ex as FaultException;
            if (faultException != null)
            {
                throw new VisaProviderException(ProviderException.PROVIDER_FAULT_ERROR, string.Empty, faultException);
            }
            Exception endpointException = ex as EndpointNotFoundException;
            if (endpointException != null)
            {
                throw new VisaProviderException(ProviderException.PROVIDER_ENDPOINTNOTFOUND_ERROR, string.Empty, endpointException);
            }
            Exception commException = ex as CommunicationException;
            if (commException != null)
            {
                throw new VisaProviderException(ProviderException.PROVIDER_COMMUNICATION_ERROR, string.Empty, commException);
            }
            Exception timeOutException = ex as TimeoutException;
            if (timeOutException != null)
            {
                throw new VisaProviderException(ProviderException.PROVIDER_TIMEOUT_ERROR, string.Empty, timeOutException);
            }
        }
        #endregion

    }
}
