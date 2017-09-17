using System;
using System.Data;
using System.Collections.Generic;

#region Project Reference
using TCF.Zeo.Cxn.Fund.Contract;
using TCF.Zeo.Cxn.Fund.Data;
using TCF.Zeo.Common.Data;
using P3Net.Data.Common;
using P3Net.Data;
using TCF.Zeo.Common.DataProtection.Contract;
using TCF.Zeo.Common.DataProtection.Impl;
using TCF.Zeo.Common.Util;
using System.Configuration;
using TCF.Zeo.Cxn.Common;
using TCF.Zeo.Cxn.Fund.Visa.Contract;
using TCF.Zeo.Cxn.Fund.Data.Exceptions;
#endregion

namespace TCF.Zeo.Cxn.Fund.Visa.Impl
{
    public class Gateway : IFundProcessor
    {
        private IIO IO;
        private Credential _credential = null;
        public Gateway()
        {
            IO = GetVisaIO();
        }

        #region Public Methods
        public long Register(CardAccount cardAccount, ZeoContext context)
        {
            try
            {
                cardAccount = GetAccountInformation(cardAccount, context);
                return AddVisaAccount(cardAccount);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.ACCOUNT_CREATE_ERROR, ex);
            }
        }

        public void UpdateRegistrationDetails(CardAccount cardAccount, ZeoContext context)
        {
            try
            {
                cardAccount = GetAccountInformation(cardAccount, context);
                UpdateVisaAccount(cardAccount);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.ACCOUNT_CREATE_ERROR, ex);
            }
        }

        public long Activate(FundRequest fundRequest, ZeoContext context)
        {
            try
            {
                Helper.FundType transactionType = (Helper.FundType)Enum.Parse(typeof(Helper.FundType), fundRequest.RequestType);

                return StageTransaction(transactionType, fundRequest, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.STAGE_TRANSACTION_FAILED, ex);
            }
        }

        public long Load(FundRequest fundRequest, ZeoContext context)
        {
            try
            {
                Helper.FundType transactionType = (Helper.FundType)Enum.Parse(typeof(Helper.FundType), fundRequest.RequestType);

                return StageTransaction(transactionType, fundRequest, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.CARD_LOAD_ERROR, ex);
            }
        }

        public long Withdraw(FundRequest fundRequest, ZeoContext context)
        {
            try
            {
                Helper.FundType transactionType = (Helper.FundType)Enum.Parse(typeof(Helper.FundType), fundRequest.RequestType);

                return StageTransaction(transactionType, fundRequest, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.CARD_WITHDRAW_ERROR, ex);
            }
        }

        public CustomerCard AssociateCard(CardAccount cardAccount, CustomerInfo customer, ZeoContext context, bool isNewCard = false)
        {
            GetCredential(context.ChannelPartnerId);
            CardInfo cardInformation = new CardInfo();
            CardInfo latestCardInformation = new CardInfo();
            CustomerCard card = null;
            try
            {
                cardInformation = IO.GetCardInfoByCardNumber(cardAccount.CardNumber, _credential, context);
                //AL-5592 Description : QA-RC-6.1-Getting inappropriate error message when a User searches for a card customer with invalid card number
                //Added if condition to avoid getting "Access Not Authorized" error from visa for sending wrong aliasid to visa.
                if (cardInformation.AliasId <= 0)
                {
                    return card;
                }

                latestCardInformation = IO.GetCardHolderInfo(cardInformation.AliasId, _credential, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.CARD_INFORMATION_RETRIEVAL_ERROR, ex);
            }
            string latestCardNumber = (latestCardInformation != null && !string.IsNullOrWhiteSpace(latestCardInformation.CardNumber)) ? latestCardInformation.CardNumber : cardAccount.CardNumber;
            //AL-1955 : As Synovus, Handle VISA cards update for existing customers in Alloy
            // isNewcard is used for enrolling new Visa DPS Card for exsisting Card Customer, if its not used then it will use associate card functionality
            if (isNewCard)
            {
                latestCardNumber = EncryptCardNumber(latestCardNumber);
                card = new CustomerCard();
                card.CardNumber = latestCardNumber;
                card.CustomerId = UpdateCardExsistingAccount(cardInformation.AliasId, cardInformation.SSN, cardInformation.LastName, latestCardNumber, context);
                return card;
            }
            if (!string.IsNullOrWhiteSpace(cardInformation.SSN) && !string.IsNullOrWhiteSpace(customer.SSN)
                        && !string.IsNullOrWhiteSpace(customer.LastName) && !string.IsNullOrWhiteSpace(cardInformation.LastName))
            {
                if (((cardInformation.SSN.Substring(cardInformation.SSN.Length - 4, 4) == customer.SSN.Substring(customer.SSN.Length - 4, 4)))
                                        && (string.Compare(cardInformation.LastName, customer.LastName, true) == 0))
                {
                    card = new CustomerCard();

                    cardAccount.CardAliasId = Convert.ToString(cardInformation.AliasId);
                    cardAccount.ProxyId = cardInformation.ProxyId;
                    cardAccount.PseudoDDA = cardInformation.PsedoDDA;
                    cardAccount.CardNumber = EncryptCardNumber(latestCardNumber);
                    cardAccount.ExpirationMonth = cardInformation.ExpirationMonth;
                    cardAccount.ExpirationYear = cardInformation.ExpirationYear;
                    cardAccount.SubClientNodeId = cardInformation.SubClientNodeId;
                    cardAccount.IsCardActive = true;
                    card.CardNumber = cardAccount.CardNumber;

                    if (cardAccount.Id != 0)
                    {
                        cardAccount.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);
                        cardAccount.DTServerLastModified = DateTime.Now;
                        card.CustomerId = UpdateVisaAccount(cardAccount);
                    }
                    else
                    {
                        cardAccount.DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone);
                        cardAccount.DTServerCreate = DateTime.Now;
                        card.CustomerId = AddVisaAccount(cardAccount);
                    }
                }
                else
                {
                    throw new FundException(FundException.CARD_ASSOCIATION_ERROR);
                }
            }
            else
            {
                throw new FundException(FundException.INVALID_CUSTOMER_DETAILS);
            }
            return card;
        }

        public void Cancel(ZeoContext context)
        {
            try
            {
                CardAccount account = new CardAccount();

                account.CustomerID = context.CustomerId;
                account.CardAliasId = null;
                account.PrimaryCardAliasId = null;
                account.CardNumber = null;
                account.ProxyId = null;
                account.PseudoDDA = null;
                account.ExpirationMonth = 0;
                account.ExpirationYear = 0;
                account.SubClientNodeId = 0;
                account.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);
                account.DTServerLastModified = DateTime.Now;
                account.IsCardActive = false;
                account.CustomerSessionId = context.CustomerSessionId;

                UpdateVisaAccount(account);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.ACCOUNT_CLOSE_FAILED, ex);
            }
        }

        public bool CloseAccount(ZeoContext context)
        {
            long aliasId = 0L;
            bool couldCloseAccount = false;

            try
            {
                if (AccountExists(context.CustomerId, true, context))
                {
                    aliasId = GetAliasId(context.CustomerId, true);
                    GetCredential(context.ChannelPartnerId);

                    couldCloseAccount = IO.CloseAccount(aliasId, _credential, context);

                    StoredProcedure fundProcedure = new StoredProcedure("usp_UpdateClosureDateByCustomerID");

                    fundProcedure.WithParameters(InputParameter.Named("customerId").WithValue(context.CustomerId));
                    fundProcedure.WithParameters(InputParameter.Named("dTAccountClosed").WithValue(DateTime.Now));
                    fundProcedure.WithParameters(InputParameter.Named("dTServerLastModified").WithValue(DateTime.Now));
                    fundProcedure.WithParameters(InputParameter.Named("dTTerminalLastModified")
                        .WithValue(Helper.GetTimeZoneTime(context.TimeZone)));
                    int rowCount = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(fundProcedure);
                }
                return couldCloseAccount;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.ACCOUNT_CLOSURE_ERROR, ex);
            }

        }

        public void Commit(long transactionId, CustomerInfo customer, ZeoContext context, string cardNumber = "")
        {
            try
            {
                FundTrx fundTrx = GetTransaction(transactionId);

                long cardAliasId = GetAliasId(context.CustomerId, true);

                double transactionAmount = Convert.ToDouble(fundTrx.TransactionAmount);

                if (fundTrx.TransactionType == Helper.FundType.Activation.ToString())
                    fundTrx.Description = "Account activation";
                else
                    fundTrx.Description = string.Format("Teller {0} at {1}", (fundTrx.TransactionType == Helper.FundType.Credit.ToString()) ? "load"
                        : (fundTrx.TransactionType == Helper.FundType.Debit.ToString()) ? "withdraw" : "Order Companion Card", context.LocationName);

                string confirmationId = string.Empty;

                GetCredential(context.ChannelPartnerId);
                _credential.VisaLocationNodeId = context.VisaLocationNodeId;

                if (fundTrx.TransactionType == Helper.FundType.AddOnCard.ToString())
                {
                    CardAccount account = GetExistingAccount(context.CustomerId, true);
                    CardAccount addAccount = (CardAccount)context.Context["FundsAccount"];
                    CardPurchaseResponse issueCardResponse = new CardPurchaseResponse();

                    addAccount.SubClientNodeId = account.SubClientNodeId;

                    GetLocationStateCodeAndCardExpPeriod(context);

                    context.CardClass = GetCardClass(context);

                    DateTime today = DateTime.Now;
                    if (today.Date.Day > 15)
                    {
                        context.CardExpiryPeriod += 1;
                    }

                    issueCardResponse = IO.CompanianCardOrder(cardAliasId, _credential, customer, addAccount, context);
                    if (issueCardResponse != null && !string.IsNullOrWhiteSpace(issueCardResponse.ConfirmationNumber))
                    {
                        confirmationId = issueCardResponse.ConfirmationNumber;
                    }
                }
                else if (fundTrx.TransactionType == Helper.FundType.Activation.ToString())
                {
                    CardAccount account = GetExistingAccount(context.CustomerId);
                    account.CustomerSessionId = context.CustomerSessionId;
                    account.CustomerID = context.CustomerId;
                    CardPurchaseResponse issueCardResponse = ActivateAccount(fundTrx, customer, account, context.TimeZone, _credential, context);

                    if (issueCardResponse != null && !string.IsNullOrWhiteSpace(issueCardResponse.ConfirmationNumber))
                    {
                        confirmationId = issueCardResponse.ConfirmationNumber;
                    }
                }
                else if (fundTrx.TransactionType == Helper.FundType.Credit.ToString())
                {
                    LoadResponse response = IO.Load(cardAliasId, transactionAmount, _credential, context);
                    confirmationId = Convert.ToString(response.TransactionKey);
                }
                else if (fundTrx.TransactionType == Helper.FundType.Debit.ToString())  // Debit
                {
                    IO.Withdraw(cardAliasId, transactionAmount, _credential, context);
                }

                fundTrx.ConfirmationId = confirmationId;
                fundTrx.DTTransmission = DateTime.Now;
                fundTrx.Status = Helper.TransactionStates.Committed;
                fundTrx.LocationNodeId = context.VisaLocationNodeId;
                fundTrx.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);
                fundTrx.DTServerLastModified = DateTime.Now;
                updateTransaction(fundTrx);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.COMMIT_FAILED, ex);
            }
        }

        public FundTrx Get(long cxnFundTrxId, bool isEditTransaction, ZeoContext context)
        {
            try
            {
                FundTrx fundTrx = GetTransaction(cxnFundTrxId);
                bool? isAccountActive = null;
                
                CardAccount account = GetExistingAccount(fundTrx.CustomerID, isAccountActive);

                string cardAliasId = account.CardAliasId;

                if (!string.IsNullOrWhiteSpace(cardAliasId))
                {
                    long aliasId = Convert.ToInt64(cardAliasId);

                    string cardNumber = string.Empty;
                    if (!string.IsNullOrWhiteSpace(account.CardNumber))
                    {
                        fundTrx.CardNumber = account.CardNumber;
                        fundTrx.FullCardNumber = DecryptCardNumber(fundTrx.CardNumber);
                        fundTrx.ProxyId = account.ProxyId;
                        fundTrx.PseudoDDA = account.PseudoDDA;
                        fundTrx.ExpirationDate = string.Concat(account.ExpirationMonth, "/", account.ExpirationYear);
                    }
                }
                return fundTrx;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.TRANSACTION_NOT_FOUND, ex);
            }
        }

        public CardBalanceInfo GetBalance(ZeoContext context)
        {
            CardBalanceInfo cardInfo = new CardBalanceInfo();
            try
            {
                CardAccount account = GetExistingAccount(context.CustomerId, true);

                if (account != null)
                {
                    string aliasId = account.CardAliasId;
                    if (!string.IsNullOrWhiteSpace(aliasId))
                    {
                        long cardAliasId = Convert.ToInt64(aliasId);

                        GetCredential(context.ChannelPartnerId);

                        cardInfo = IO.GetBalance(cardAliasId, _credential, context);
                        cardInfo.ClosureDate = account.DTAccountClosed;
                        cardInfo.IsFraud = account.IsFraud;

                        account.DTServerLastModified = DateTime.Now;
                        account.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);

                        if (string.IsNullOrWhiteSpace(account.PrimaryCardAliasId))
                        {
                            UpdatePrimaryAliasID(account, context);
                        }
                        if (!string.IsNullOrWhiteSpace(cardInfo.NewCardNumber) && DecryptCardNumber(account.CardNumber) != cardInfo.NewCardNumber)
                        {
                            account.CardNumber = EncryptCardNumber(cardInfo.NewCardNumber);
                            UpdateCarNumber(account);
                        }

                        if (Convert.ToInt32(cardInfo.CardStatus) == (int)CardStatus.Closed && account.DTAccountClosed == null)
                        {
                            AccountHistory accountHistory = IO.GetAccountholderInfo(Convert.ToInt64(account.CardAliasId), _credential, context);

                            updateCardClosureDate(accountHistory.TransactionDate, context);
                            cardInfo.ClosureDate = accountHistory.TransactionDate;
                        }

                        if (Convert.ToInt32(cardInfo.CardStatus) == (int)CardStatus.ClosedForFraud && !account.IsFraud)
                        {
                            AccountHistory accountHistory = IO.GetAccountholderInfo(Convert.ToInt64(account.CardAliasId), _credential, context);
                            cardInfo.IsFraud = account.IsFraud = true;
                            updateFraudStatus(true, accountHistory.TransactionDate, context);
                        }

                        cardInfo.MetaData = new Dictionary<string, object>();
                        if (!string.IsNullOrEmpty(account.PrimaryCardAliasId) && !string.IsNullOrEmpty(account.CardAliasId)
                                                                        && account.PrimaryCardAliasId == account.CardAliasId)
                        {
                            cardInfo.MetaData.AddOrUpdate("IsPrimaryCardHolder", true);
                        }
                        else
                        {
                            cardInfo.MetaData.AddOrUpdate("IsPrimaryCardHolder", false);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.CARD_BALANCE_RETRIEVAL_ERROR, ex);
            }

            return cardInfo;
        }

        public double GetFundFee(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            try
            {
                int feeType = 0;
                int cardStatus = Convert.ToInt32(cardMaintenanceInfo.CardStatus);
                if (cardStatus == (int)CardStatus.Lost || cardStatus == (int)CardStatus.Stolen)
                    feeType = (int)FundFeeType.CardReplacementFee;
                else if (cardStatus == (int)CardStatus.ReplaceCard)
                    feeType = (int)FundFeeType.MailOrderFee;
                VisaFee visaFee = GetVisaFee(context.ChannelPartnerId, feeType);

                double fee = 0.0;
                if (visaFee != null)
                {
                    fee = visaFee.Fee;
                }
                return fee;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.FEE_RETRIVEL_FAILED, ex);
            }
        }
        public bool AccountExists(long customerId, bool? isActive, ZeoContext context)
        {
            try
            {
                bool isAccountExists = false;

                StoredProcedure accountProcedure = new StoredProcedure("usp_CheckAccountExists");
                accountProcedure.WithParameters(InputParameter.Named("IsActive").WithValue(isActive));
                accountProcedure.WithParameters(InputParameter.Named("customerId").WithValue(customerId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(accountProcedure))
                {
                    while (datareader.Read())
                    {
                        isAccountExists = datareader.GetBooleanOrDefault("AccountExists");
                    }
                }

                return isAccountExists;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.ACCOUNT_RETRIVEL_FAILED, ex);
            }
        }

        public double GetShippingFee(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            try
            {
                VisaShippingFee shippingFee = GetVisaShippingFee(cardMaintenanceInfo, context);
                return shippingFee.Fee;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.SHIPPING_RETRIVEL_FAILED, ex);
            }
        }

        public List<ShippingTypes> GetShippingTypes(long channelPartnerId, ZeoContext context)
        {
            try
            {
                List<ShippingTypes> shippingTypes = new List<ShippingTypes>();

                StoredProcedure customerProcedure = new StoredProcedure("usp_GetShippingTypesByChannelPartnerId");

                customerProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
                {
                    while (datareader.Read())
                    {
                        ShippingTypes shippingType = new ShippingTypes();
                        shippingType.Name = datareader.GetStringOrDefault("Name");
                        shippingType.Code = datareader.GetStringOrDefault("code");

                        shippingTypes.Add(shippingType);
                    }
                }

                return shippingTypes;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.SHIPPING_RETRIVEL_FAILED, ex);
            }
        }

        public List<TransactionHistory> GetTransactionHistory(TransactionHistoryRequest request, ZeoContext context)
        {
            try
            {
                List<TransactionHistory> transactionHistoryList = new List<TransactionHistory>();

                GetCredential(context.ChannelPartnerId);
                request.AliasId = GetAliasId(context.CustomerId, false);

                transactionHistoryList = IO.GetTransactionHistory(request, _credential, context);

                return transactionHistoryList;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.CARD_HISTORY_RETRIVEL_ERROR, ex);
            }
        }

        public CardAccount Lookup(ZeoContext context, bool? isCardAccountActivated = null)
        {
            try
            {
                return GetExistingAccount(context.CustomerId, isCardAccountActivated);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.ACCOUNT_RETRIVEL_FAILED, ex);
            }
        }

        public bool ReplaceCard(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            long aliasId = 0L;
            string proxyId = string.Empty;
            bool couldReplaceCard = false;
            CardAccount account = null;
            try
            {
                account = GetExistingAccount(context.CustomerId, true);
                account.CustomerID = context.CustomerId;

                if (account != null)
                {
                    GetCredential(context.ChannelPartnerId);
                    context = GetLocationStateCodeAndCardExpPeriod(context);

                    cardMaintenanceInfo.CardClass = GetCardClass(context);
                    int cardStatus = Convert.ToInt32(cardMaintenanceInfo.SelectedCardStatus);
                    int shippingType = Convert.ToInt32(cardMaintenanceInfo.ShippingType);

                    if ((cardStatus == (int)CardStatus.Lost || cardStatus == (int)CardStatus.Stolen) && shippingType == (int)ShippingFeeType.ReplaceInstantIssue)
                        cardMaintenanceInfo.ShippingType = Convert.ToString((int)ShippingFeeType.InstantIssueReplaceLostOrStolen);
                    VisaShippingFee visashippingfee = GetVisaShippingFee(cardMaintenanceInfo, context);
                    cardMaintenanceInfo.ShippingFee = visashippingfee.Fee;
                    cardMaintenanceInfo.ShippingFeeCode = visashippingfee.FeeCode;

                    int visaFeeType = (cardStatus == (int)CardStatus.Lost || cardStatus == (int)CardStatus.Stolen) ? Convert.ToInt32(FundFeeType.CardReplacementFee) : Convert.ToInt32(FundFeeType.MailOrderFee);
                    VisaFee visaFee = GetVisaFee(context.ChannelPartnerId, visaFeeType);
                    cardMaintenanceInfo.ReplacementFee = visaFee.Fee;
                    cardMaintenanceInfo.ReplacementFeeCode = visaFee.FeeCode;
                    cardMaintenanceInfo.StockId = visaFee.StockId;
                    string cardAliasId = account.CardAliasId;
                    if (account != null && !string.IsNullOrWhiteSpace(cardAliasId))
                    {
                        aliasId = Convert.ToInt64(cardAliasId);
                        proxyId = account.ProxyId;
                    }

                    DateTime today = DateTime.Now;
                    cardMaintenanceInfo.ExpiryYear = today.AddMonths(context.CardExpiryPeriod).Year;
                    cardMaintenanceInfo.ExpiryMonth = today.AddMonths(context.CardExpiryPeriod).Month;
                }

                couldReplaceCard = IO.ReplaceCard(aliasId, cardMaintenanceInfo, _credential, context);

                if (couldReplaceCard && !string.IsNullOrWhiteSpace(cardMaintenanceInfo.CardNumber))
                {
                    account.CardNumber = EncryptCardNumber(cardMaintenanceInfo.CardNumber);
                    //TODO : change the new sp
                    UpdateCarNumber(account);
                }
                return couldReplaceCard;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.CARD_REPLACEMENT_ERROR, ex);
            }

        }

        public long UpdateAccount(CardAccount cardAccount, ZeoContext context)
        {
            try
            {
                cardAccount.CustomerID = context.CustomerId;
                cardAccount.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);
                cardAccount.DTServerLastModified = DateTime.Now;
                cardAccount.CustomerSessionId = context.CustomerSessionId;
                return UpdateVisaAccount(cardAccount);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.GET_CARDNUMBER_FAILED, ex);
            }
        }

        public long UpdateAmount(long cxnFundTrxId, FundRequest fundRequest, string timezone, ZeoContext context)
        {
            try
            {
                FundTrx fundTrx = new FundTrx();
                if (fundRequest.RequestType == "Activation")
                {
                    fundTrx.PromoCode = fundRequest.PromoCode;
                }
                fundTrx.Id = cxnFundTrxId;
                fundTrx.TransactionAmount = fundRequest.Amount;
                fundTrx.DTTerminalLastModified = Helper.GetTimeZoneTime(timezone);
                fundTrx.DTServerLastModified = DateTime.Now;
                updateTransactionAmount(fundTrx);

                return fundTrx.Id;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.CAN_NOT_UPDATE_TRANSACTION, ex);
            }
        }

        public bool UpdateCardStatus(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            try
            {
                long aliasId = GetAliasId(context.CustomerId, false);
                bool couldUpdateStatus = false;
                GetCredential(context.ChannelPartnerId);
                couldUpdateStatus = IO.UpdateCardStatus(aliasId, cardMaintenanceInfo.CardStatus, _credential, context);
                return couldUpdateStatus;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.GET_CARDNUMBER_FAILED, ex);
            }
        }

        public string GetPrepaidCardNumber(ZeoContext context)
        {
            string cardNumber = string.Empty;
            try
            {
                cardNumber = GetPrepaidCardNumber(context.CustomerId);
                cardNumber = DecryptCardNumber(cardNumber);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.GET_CARDNUMBER_FAILED, ex);
            }

            return cardNumber;
        }

        #endregion

        #region Private Methods
        private void GetCredential(long channelPartnerId)
        {
            Credential credential = new Credential();

            StoredProcedure customerProcedure = new StoredProcedure("usp_GetVisaCredentialsByChannelPartnerId");

            customerProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
            {
                while (datareader.Read())
                {
                    credential.CertificateName = datareader.GetStringOrDefault("CertificateName");
                    credential.ClientNodeId = datareader.GetInt64OrDefault("ClientNodeId");
                    credential.Password = datareader.GetStringOrDefault("Password");
                    credential.ServiceUrl = datareader.GetStringOrDefault("ServiceUrl");
                    credential.StockId = datareader.GetStringOrDefault("StockId");
                    credential.SubClientNodeId = datareader.GetInt64OrDefault("SubClientNodeId");
                    credential.UserName = datareader.GetStringOrDefault("UserName");
                    credential.CardProgramNodeId = datareader.GetInt64OrDefault("CardProgramNodeId");
                }

                _credential = credential;
            }
        }

        private string DecryptCardNumber(string encryptedCardNumber)
        {
            IDataProtectionService dataProtectionService = null;

            string type = (ConfigurationManager.AppSettings["DataProtectionService"]);
            if (type == "Simulator")
            {
                dataProtectionService = new DataProtectionSimulator();
            }
            else
            {
                dataProtectionService = new DataProtectionService();
            }

            return dataProtectionService.Decrypt(encryptedCardNumber, 0);
        }

        private bool ValidateCard(string cardNumber, long customerId)
        {
            bool isCardValid = false;

            StoredProcedure customerProcedure = new StoredProcedure("usp_ValidateCard");

            customerProcedure.WithParameters(InputParameter.Named("cardNumber").WithValue(cardNumber));
            customerProcedure.WithParameters(InputParameter.Named("customerId").WithValue(customerId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
            {
                while (datareader.Read())
                {
                    isCardValid = datareader.GetBooleanOrDefault("ValidCard");
                }
            }

            return isCardValid;
        }

        private string EncryptCardNumber(string cardNumber)
        {
            IDataProtectionService dataProtectionService = null;

            string type = (ConfigurationManager.AppSettings["DataProtectionService"]);
            if (type == "Simulator")
            {
                dataProtectionService = new DataProtectionSimulator();
            }
            else
            {
                dataProtectionService = new DataProtectionService();
            }
            return dataProtectionService.Encrypt(cardNumber, 0);
        }

        private long AddVisaAccount(CardAccount cardAccount)
        {
            long visaAccountId = 0;

            StoredProcedure customerProcedure = new StoredProcedure("usp_CreateVisaAccount");
            customerProcedure.WithParameters(InputParameter.Named("cardAliasId").WithValue(cardAccount.CardAliasId));
            customerProcedure.WithParameters(InputParameter.Named("primaryAliasId").WithValue(cardAccount.PrimaryCardAliasId));
            customerProcedure.WithParameters(InputParameter.Named("ProxyId").WithValue(cardAccount.ProxyId));
            customerProcedure.WithParameters(InputParameter.Named("pseudoDDA").WithValue(cardAccount.PseudoDDA));
            customerProcedure.WithParameters(InputParameter.Named("cardNumber").WithValue(cardAccount.CardNumber));
            customerProcedure.WithParameters(InputParameter.Named("expmonth").WithValue(cardAccount.ExpirationMonth));
            customerProcedure.WithParameters(InputParameter.Named("expyear").WithValue(cardAccount.ExpirationYear));
            customerProcedure.WithParameters(InputParameter.Named("subClientNodeId").WithValue(cardAccount.SubClientNodeId));
            customerProcedure.WithParameters(InputParameter.Named("activationNodeId").WithValue(cardAccount.ActivatedLocationNodeId));
            customerProcedure.WithParameters(InputParameter.Named("activated").WithValue(cardAccount.IsCardActive));
            customerProcedure.WithParameters(InputParameter.Named("customerId").WithValue(cardAccount.CustomerID));
            customerProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(cardAccount.CustomerSessionId));
            customerProcedure.WithParameters(InputParameter.Named("dtServerCreate").WithValue(cardAccount.DTServerCreate));
            customerProcedure.WithParameters(InputParameter.Named("dtTerminalCreate").WithValue(cardAccount.DTTerminalCreate));
            customerProcedure.WithParameters(InputParameter.Named("dtAccountClosure").WithValue((DateTime?)null));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(customerProcedure))
            {
                while (datareader.Read())
                {
                    visaAccountId = datareader.GetInt64OrDefault("VisaAccountID");
                }
            }

            return visaAccountId;
        }

        private long UpdateVisaAccount(CardAccount cardAccount)
        {
            StoredProcedure fundProcedure = new StoredProcedure("usp_UpdateVisaAccountByCustomerID");

            fundProcedure.WithParameters(InputParameter.Named("cardAliasId").WithValue(cardAccount.CardAliasId));
            fundProcedure.WithParameters(InputParameter.Named("primaryAliasId").WithValue(cardAccount.PrimaryCardAliasId));
            fundProcedure.WithParameters(InputParameter.Named("ProxyId").WithValue(cardAccount.ProxyId));
            fundProcedure.WithParameters(InputParameter.Named("pseudoDDA").WithValue(cardAccount.PseudoDDA));
            fundProcedure.WithParameters(InputParameter.Named("cardNumber").WithValue(cardAccount.CardNumber));
            fundProcedure.WithParameters(InputParameter.Named("expyear").WithValue(cardAccount.ExpirationYear));
            fundProcedure.WithParameters(InputParameter.Named("expmonth").WithValue(cardAccount.ExpirationMonth));
            fundProcedure.WithParameters(InputParameter.Named("subClientNodeId").WithValue(cardAccount.SubClientNodeId));
            fundProcedure.WithParameters(InputParameter.Named("ActivatedLocationNodeId").WithValue(cardAccount.ActivatedLocationNodeId));
            fundProcedure.WithParameters(InputParameter.Named("activated").WithValue(cardAccount.IsCardActive));
            fundProcedure.WithParameters(InputParameter.Named("customerId").WithValue(cardAccount.CustomerID));
            fundProcedure.WithParameters(InputParameter.Named("isFraud").WithValue(cardAccount.IsFraud));
            fundProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(cardAccount.CustomerSessionId));
            fundProcedure.WithParameters(InputParameter.Named("dTServerLastModified").WithValue(cardAccount.DTServerLastModified));
            fundProcedure.WithParameters(InputParameter.Named("dTTerminalLastModified").WithValue(cardAccount.DTTerminalLastModified));
            fundProcedure.WithParameters(InputParameter.Named("dtAccountClosure").WithValue(cardAccount.DTAccountClosed));

            int rowCount = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(fundProcedure);

            return cardAccount.Id;
        }

        private CardAccount GetExistingAccount(long customerId, bool? isActive = null)
        {
            CardAccount account = null;
            StoredProcedure accountProcedure = new StoredProcedure("usp_GetVisaAccountBycustomerIdandStatus");

            accountProcedure.WithParameters(InputParameter.Named("customerID").WithValue(customerId));
            accountProcedure.WithParameters(InputParameter.Named("activated").WithValue(isActive));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(accountProcedure))
            {
                while (datareader.Read())
                {
                    account = new CardAccount();
                    account.ProxyId = datareader.GetStringOrDefault("ProxyId");
                    account.PseudoDDA = datareader.GetStringOrDefault("PseudoDDA");
                    account.CardNumber = datareader.GetStringOrDefault("CardNumber");
                    account.CardAliasId = datareader.GetStringOrDefault("CardAliasId");
                    account.SubClientNodeId = datareader.GetInt64OrDefault("SubClientNodeId");
                    account.PrimaryCardAliasId = datareader.GetStringOrDefault("PrimaryCardAliasId");
                    account.Id = datareader.GetInt64OrDefault("VisaAccountID");
                    account.ExpirationMonth = datareader.GetInt32OrDefault("ExpirationMonth");
                    account.ExpirationYear = datareader.GetInt32OrDefault("ExpirationYear");
                    account.IsCardActive = datareader.GetBooleanOrDefault("Activated");
                    account.IsFraud = Convert.ToBoolean(datareader.GetInt32OrDefault("FraudScore"));
                    account.DTAccountClosed = datareader.GetDateTimeOrDefault("DTAccountClosed");
                    account.DTAccountClosed = account.DTAccountClosed == DateTime.MinValue ? null : account.DTAccountClosed;
                    account.ActivatedLocationNodeId = datareader.GetInt64OrDefault("ActivatedLocationNodeId");
                }
            }
            return account;
        }

        private long UpdateCardExsistingAccount(long aliasId, string SSN, string lastName, string latestCardNumber, ZeoContext context)
        {
            long customerId = 0;

            string cardNumber = string.IsNullOrWhiteSpace(latestCardNumber) ? "" : latestCardNumber;

            StoredProcedure accountProcedure = new StoredProcedure("usp_UpdateAccountByAliasId");

            accountProcedure.WithParameters(InputParameter.Named("AliasId").WithValue(aliasId));
            accountProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(Helper.GetTimeZoneTime(context.TimeZone)));
            accountProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now));
            accountProcedure.WithParameters(InputParameter.Named("CardNumber").WithValue(cardNumber));
            accountProcedure.WithParameters(OutputParameter.Named("CustomerId").OfType<long>());

            int rowCount = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(accountProcedure);

            if (rowCount > 0)
            {
                customerId = Convert.ToInt64(accountProcedure.Parameters["CustomerId"].Value);
            }

            return customerId;
        }

        private CardAccount GetAccountByAlias(long aliasId)
        {
            CardAccount account = null;

            StoredProcedure accountProcedure = new StoredProcedure("usp_GetAccountByAlias");

            accountProcedure.WithParameters(InputParameter.Named("cardAliasId").WithValue(aliasId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(accountProcedure))
            {
                while (datareader.Read())
                {
                    account = new CardAccount();
                    account.ProxyId = datareader.GetStringOrDefault("ProxyId");
                    account.PseudoDDA = datareader.GetStringOrDefault("PseudoDDA");
                    account.CardNumber = datareader.GetStringOrDefault("CardNumber");
                    account.SubClientNodeId = datareader.GetInt64OrDefault("SubClientNodeId");
                    account.PrimaryCardAliasId = datareader.GetStringOrDefault("PrimaryCardAliasId");
                    account.Id = datareader.GetInt64OrDefault("VisaAccountID");
                    account.CardAliasId = aliasId.ToString();
                }
            }

            return account;
        }

        private long StageTransaction(Helper.FundType transactionType, FundRequest fundRequest, ZeoContext context)
        {
            bool isActive = true;
            long trxAccountId = 0;

            if (transactionType == Helper.FundType.Activation)
            {
                isActive = false;
            }

            CardAccount account = GetExistingAccount(context.CustomerId, isActive);
            CardBalanceInfo balance = GetBalance(context);

            StoredProcedure accountProcedure = new StoredProcedure("usp_AddVisaTransaction");

            accountProcedure.WithParameters(InputParameter.Named("visaaccountid").WithValue(account.Id));
            accountProcedure.WithParameters(InputParameter.Named("amount").WithValue(fundRequest.Amount));
            accountProcedure.WithParameters(InputParameter.Named("transactiontype").WithValue((int)transactionType));
            accountProcedure.WithParameters(InputParameter.Named("status").WithValue((int)Helper.TransactionStates.Staged));
            accountProcedure.WithParameters(InputParameter.Named("balance").WithValue(balance.Balance));
            accountProcedure.WithParameters(InputParameter.Named("promoCode").WithValue(fundRequest.PromoCode));
            accountProcedure.WithParameters(InputParameter.Named("locationNodeId").WithValue(context.VisaLocationNodeId));
            accountProcedure.WithParameters(InputParameter.Named("dTServerCreate").WithValue(DateTime.Now));
            accountProcedure.WithParameters(InputParameter.Named("dTTerminalCreate").WithValue(Helper.GetTimeZoneTime(context.TimeZone)));
            accountProcedure.WithParameters(InputParameter.Named("dtTransmission").WithValue(DateTime.Now));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(accountProcedure))
            {
                while (datareader.Read())
                {
                    trxAccountId = datareader.GetInt64OrDefault("VisaTrxID");
                }
            }

            return trxAccountId;
        }

        private int GetCardClass(ZeoContext context)
        {
            string locationStateCode = context.LocationStateCode;
            int cardClass = 7;

            if (!string.IsNullOrWhiteSpace(locationStateCode))
            {
                StoredProcedure fundProcedure = new StoredProcedure("usp_GetCardClassbyStateCode");

                fundProcedure.WithParameters(InputParameter.Named("stateCode").WithValue(locationStateCode));
                fundProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(fundProcedure))
                {
                    while (datareader.Read())
                    {
                        cardClass = datareader.GetInt32OrDefault("CardClass");
                    }
                }
            }

            return cardClass;
        }

        private VisaFee GetVisaFee(long channelPartnerId, int feeType)
        {
            VisaFee fee = null;

            StoredProcedure accountProcedure = new StoredProcedure("USP_GetVisaFeeByChannelPartnerIdandVisaFeeType");
            accountProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));
            accountProcedure.WithParameters(InputParameter.Named("code").WithValue(feeType));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(accountProcedure))
            {
                while (datareader.Read())
                {
                    fee = new VisaFee();
                    fee.Fee = Convert.ToDouble(datareader.GetDecimalOrDefault("Fee"));
                    fee.FeeCode = Convert.ToInt32(datareader.GetInt64OrDefault("Feecode"));
                    fee.StockId = datareader.GetStringOrDefault("StockId");
                }
            }

            return fee;
        }

        private FundTrx GetTransaction(long transactionId)
        {
            FundTrx fundTrx = null;
            StoredProcedure accountProcedure = new StoredProcedure("usp_GetVisaTransactionByTransactionId");
            accountProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(accountProcedure))
            {
                while (datareader.Read())
                {
                    fundTrx = new FundTrx();
                    fundTrx.TransactionAmount = datareader.GetDecimalOrDefault("Amount");
                    fundTrx.Fee = datareader.GetDecimalOrDefault("Fee");
                    fundTrx.PreviousCardBalance = datareader.GetDecimalOrDefault("Balance");
                    fundTrx.TransactionID = transactionId;
                    int value = datareader.GetInt32OrDefault("TransactionType");
                    Helper.FundType type = (Helper.FundType)value;
                    fundTrx.TransactionType = type.ToString();
                    fundTrx.PromoCode = datareader.GetStringOrDefault("PromoCode");
                    fundTrx.Id = datareader.GetInt64OrDefault("VisaTrxID");
                    fundTrx.CustomerID = datareader.GetInt64OrDefault("CustomerId");
                    fundTrx.Status = (Helper.TransactionStates)datareader.GetInt32OrDefault("Status");
                }
            }

            return fundTrx;
        }

        private long GetAliasId(long customerId, bool isPrimary)
        {
            long aliasId = 0;
            StoredProcedure accountProcedure = new StoredProcedure("usp_GetAliasIdByCustomerId");
            accountProcedure.WithParameters(InputParameter.Named("customerId").WithValue(customerId));
            accountProcedure.WithParameters(InputParameter.Named("isPrimary").WithValue(isPrimary));

            using (IDataReader reader = DataConnectionHelper.GetConnectionManager().ExecuteReader(accountProcedure))
            {
                while (reader.Read())
                {
                    aliasId = Convert.ToInt64(reader.GetStringOrDefault("AliasId"));
                }
            }

            return aliasId;
        }

        private CardPurchaseResponse ActivateAccount(FundTrx fundTrx, CustomerInfo customer, CardAccount account, string timeZone, Credential _credential, ZeoContext context)
        {
            account.ActivatedLocationNodeId = _credential.VisaLocationNodeId;

            CardInfo cardInformation = new CardInfo()
            {
                //This line is specific to Synovus Visa, we need to send LocationNodeId to Visa. 
                //For TCF it's always -1. Hence the changes
                SubClientNodeId = _credential.VisaLocationNodeId == -1 ? account.SubClientNodeId : _credential.VisaLocationNodeId,
                ExpirationMonth = account.ExpirationMonth,
                ExpirationYear = account.ExpirationYear,
                ProxyId = account.ProxyId,
                PromotionCode = fundTrx.PromoCode
            };
            CardPurchaseResponse activateResponse = new CardPurchaseResponse();

            try
            {
                activateResponse = IO.IssueCard(account, customer, Convert.ToDouble(fundTrx.TransactionAmount), cardInformation, _credential, context);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            account.IsCardActive = true;
            account.DTTerminalLastModified = Helper.GetTimeZoneTime(timeZone);
            account.DTServerLastModified = DateTime.Now;
            UpdateVisaAccount(account);

            return activateResponse;
        }

        private void UpdateCarNumber(CardAccount account)
        {
            StoredProcedure fundProcedure = new StoredProcedure("usp_UpdateCardByAccountID");

            fundProcedure.WithParameters(InputParameter.Named("cardNumber").WithValue(account.CardNumber));
            fundProcedure.WithParameters(InputParameter.Named("accountId").WithValue(account.Id));
            fundProcedure.WithParameters(InputParameter.Named("dTServerLastModified").WithValue(account.DTServerLastModified));
            fundProcedure.WithParameters(InputParameter.Named("dTTerminalLastModified").WithValue(account.DTTerminalLastModified));

            DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(fundProcedure);
        }

        private VisaShippingFee GetVisaShippingFee(CardMaintenanceInfo cardMaintenanceInfo, ZeoContext context)
        {
            VisaShippingFee shippingFee = null;

            StoredProcedure accountProcedure = new StoredProcedure("usp_GetVisaShippingFeeByShippingType");
            accountProcedure.WithParameters(InputParameter.Named("code").WithValue(cardMaintenanceInfo.ShippingType));
            accountProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(accountProcedure))
            {
                while (datareader.Read())
                {
                    shippingFee = new VisaShippingFee();
                    decimal fee = datareader.GetDecimalOrDefault("Fee");
                    shippingFee.Fee = Convert.ToDouble(fee);
                    shippingFee.FeeCode = Convert.ToDouble(datareader.GetInt64OrDefault("FeeCode"));
                }
            }

            return shippingFee;
        }

        private void UpdatePrimaryAliasID(CardAccount account, ZeoContext context)
        {
            GetCredential(context.ChannelPartnerId);
            CardInfo cardInformation = IO.GetCardHolderInfo(Convert.ToInt64(account.CardAliasId), _credential, context);
            if (cardInformation.PrimaryAliasId > 0)
            {
                account.PrimaryCardAliasId = Convert.ToString(cardInformation.PrimaryAliasId);
            }
            else if (cardInformation.PrimaryAliasId == 0)
            {
                account.PrimaryCardAliasId = account.CardAliasId;
            }

            StoredProcedure fundProcedure = new StoredProcedure("usp_UpdateAliasIdByAccountID");

            fundProcedure.WithParameters(InputParameter.Named("aliasId").WithValue(account.PrimaryCardAliasId));
            fundProcedure.WithParameters(InputParameter.Named("accountId").WithValue(account.Id));
            fundProcedure.WithParameters(InputParameter.Named("dTServerLastModified").WithValue(account.DTServerLastModified));
            fundProcedure.WithParameters(InputParameter.Named("dTTerminalLastModified").WithValue(account.DTTerminalLastModified));

            DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(fundProcedure);
        }

        private ZeoContext GetLocationStateCodeAndCardExpPeriod(ZeoContext context)
        {
            StoredProcedure accountProcedure = new StoredProcedure("usp_GetLocationStateCodeAndCardExpPeriod");
            accountProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));
            accountProcedure.WithParameters(InputParameter.Named("locationId").WithValue(context.LocationID));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(accountProcedure))
            {
                while (datareader.Read())
                {
                    context.CardExpiryPeriod = datareader.GetInt32OrDefault("CardExpiryPeriod");
                    context.LocationStateCode = datareader.GetStringOrDefault("State");
                }
            }

            return context;
        }

        private void updateTransactionAmount(FundTrx fundTrx)
        {
            StoredProcedure fundProcedure = new StoredProcedure("usp_UpdateTransactionAmountByTransactionId");

            fundProcedure.WithParameters(InputParameter.Named("TrxId").WithValue(fundTrx.Id));
            fundProcedure.WithParameters(InputParameter.Named("PromoCode").WithValue(fundTrx.PromoCode));
            fundProcedure.WithParameters(InputParameter.Named("Amount").WithValue(fundTrx.TransactionAmount));
            fundProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(fundTrx.DTServerLastModified));
            fundProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(fundTrx.DTTerminalLastModified));

            DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(fundProcedure);
        }

        private void updateTransaction(FundTrx fundTrx)
        {
            StoredProcedure fundProcedure = new StoredProcedure("usp_UpdateVisaTransaction");

            fundProcedure.WithParameters(InputParameter.Named("transactionID").WithValue(fundTrx.TransactionID));
            fundProcedure.WithParameters(InputParameter.Named("confirmationId").WithValue(fundTrx.ConfirmationId));
            fundProcedure.WithParameters(InputParameter.Named("status").WithValue((int)fundTrx.Status));
            fundProcedure.WithParameters(InputParameter.Named("dtTransmission").WithValue(fundTrx.DTTransmission));
            fundProcedure.WithParameters(InputParameter.Named("locationNodeId").WithValue(Convert.ToString(fundTrx.LocationNodeId)));
            fundProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(fundTrx.DTServerLastModified));
            fundProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(fundTrx.DTTerminalLastModified));

            DataHelper.GetConnectionManager().ExecuteNonQuery(fundProcedure);
        }

        private string GetPrepaidCardNumber(long customerId)
        {
            string cardNumber = string.Empty;
            StoredProcedure procedure = new StoredProcedure("usp_GetVisaCardNumber");
            procedure.WithParameters(InputParameter.Named("customerId").WithValue(customerId));

            using (IDataReader reader = DataHelper.GetConnectionManager().ExecuteReader(procedure))
            {
                while (reader.Read())
                {
                    cardNumber = reader.GetStringOrDefault("CardNumber");
                }
            }

            return cardNumber;
        }

        private CardAccount GetAccountInformation(CardAccount cardAccount, ZeoContext context)
        {
            CardInfo cardInformation = new CardInfo();
            string pseudoDDA = string.Empty;
            string cardNumber = string.Empty;

            try
            {
                GetCredential(context.ChannelPartnerId);

                cardInformation = IO.GetCardInfoByProxyId(cardAccount.ProxyId, _credential, context);

                pseudoDDA = IO.GetPsedoDDAFromAliasId(cardInformation.AliasId, _credential, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FundException(FundException.CARD_INFORMATION_RETRIEVAL_ERROR, ex);
            }

            string expiryDate = string.Format("{0}/{1}", cardInformation.ExpirationMonth, cardInformation.ExpirationYear);
            cardAccount.ExpirationDate = cardAccount.ExpirationDate.TrimStart('0');

            if (expiryDate != cardAccount.ExpirationDate)
            {
                throw new FundException(FundException.INVALID_EXPIRATION_DATE);
            }

            if (!string.IsNullOrEmpty(cardAccount.PseudoDDA))
            {
                if (cardAccount.PseudoDDA != pseudoDDA)
                {
                    throw new FundException(FundException.PSEUDO_DDA_MISMATCH);
                }
            }
            else if (!string.IsNullOrEmpty(cardAccount.CardNumber) && cardAccount.CardNumber != cardInformation.CardNumber)
            {
                throw new FundException(FundException.PAN_NUMBER_MISMATCH);
            }

            if (cardInformation != null && !string.IsNullOrWhiteSpace(cardInformation.CardNumber))
            {
                if (cardInformation.Status != "1")
                {
                    throw new FundException(FundException.CARD_ALREADY_ISSUED);
                }
                cardNumber = EncryptCardNumber(cardInformation.CardNumber);
                bool isCardValid = ValidateCard(cardNumber, context.CustomerId);

                if (!isCardValid)
                {
                    throw new FundException(FundException.CARD_ALREADY_REGISTERED);
                }
            }

            try
            {
                cardAccount.CardAliasId = Convert.ToString(cardInformation.AliasId);
                cardAccount.PrimaryCardAliasId = Convert.ToString(cardInformation.AliasId);
                cardAccount.ProxyId = cardInformation.ProxyId;
                cardAccount.PseudoDDA = pseudoDDA;
                cardAccount.CardNumber = cardNumber;
                cardAccount.ExpirationMonth = cardInformation.ExpirationMonth;
                cardAccount.ExpirationYear = cardInformation.ExpirationYear;
                cardAccount.SubClientNodeId = cardInformation.SubClientNodeId;
                cardAccount.ActivatedLocationNodeId = context.VisaLocationNodeId;
                cardAccount.DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone);
                cardAccount.DTServerCreate = DateTime.Now;
                cardAccount.DTServerLastModified = DateTime.Now;
                cardAccount.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);
                cardAccount.CustomerSessionId = context.CustomerSessionId;
                cardAccount.CustomerID = context.CustomerId;
                cardAccount.DTAccountClosed = null;
                return cardAccount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void updateCardClosureDate(DateTime transactionDate, ZeoContext context)
        {
            StoredProcedure procedure = new StoredProcedure("usp_UpdateCardClosureByCustomerId");
            procedure.WithParameters(InputParameter.Named("customerId").WithValue(context.CustomerId));
            procedure.WithParameters(InputParameter.Named("dTAccountClosure").WithValue(transactionDate));
            procedure.WithParameters(InputParameter.Named("dTServerLastModified").WithValue(DateTime.Now));
            procedure.WithParameters(InputParameter.Named("dTTerminalLastModified").WithValue(Helper.GetTimeZoneTime(context.TimeZone)));

            DataHelper.GetConnectionManager().ExecuteNonQuery(procedure);
        }

        private void updateFraudStatus(bool isFraud, DateTime transactionDate, ZeoContext context)
        {
            StoredProcedure procedure = new StoredProcedure("usp_UpdateFraudStatusByCustomerId");
            procedure.WithParameters(InputParameter.Named("customerId").WithValue(context.CustomerId));
            procedure.WithParameters(InputParameter.Named("IsFraud").WithValue(isFraud));
            procedure.WithParameters(InputParameter.Named("dTAccountClosure").WithValue(transactionDate));
            procedure.WithParameters(InputParameter.Named("dTServerLastModified").WithValue(DateTime.Now));
            procedure.WithParameters(InputParameter.Named("dTTerminalLastModified").WithValue(Helper.GetTimeZoneTime(context.TimeZone)));

            DataHelper.GetConnectionManager().ExecuteNonQuery(procedure);
        }

        private static IIO GetVisaIO()
        {
            string fundProcessor = ConfigurationManager.AppSettings["FundsProcessor"].ToString();

            if (fundProcessor.ToUpper() == "IO")
                return new IO();
            else
                return new SimulatorIO();
        }

        #endregion
    }
}
