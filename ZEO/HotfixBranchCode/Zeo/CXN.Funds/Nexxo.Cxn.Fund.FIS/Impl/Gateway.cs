using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

// CXN Dependencies
using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Fund.Data;
using MGI.Cxn.Fund.FIS.ISO8583;
using MGI.Cxn.Fund.FIS.Data;

// Common Dependencies
using MGI.Common.DataAccess.Contract;
using MGI.Common.DataProtection.Contract;
using MGI.TimeStamp;
using MGI.Common.DataProtection.Impl;

namespace MGI.Cxn.Fund.FIS.Impl
{
    public class Gateway : IFundProcessor
    {
        private IO IO { get; set; }

        private IDataProtectionService DataProtectionSvc { get; set; }

        private IRepository<FISAccount> AccountRepo { get; set; }

        private IRepository<FISTrx> TransactionRepo { get; set; }

        #region IFundProcessor Members
        public long Register(CardAccount cardAccount, Dictionary<string, object> context, out ProcessorResult processorResult)
        {
            FISAccount account =  Mapper.ToFISGPRAccount(cardAccount);

            AccountRepo.AddWithFlush(account);
            processorResult = new ProcessorResult();
            return account.Id;
        }

        public long Authenticate(string cardNumber, Dictionary<string, object> context, out ProcessorResult processorResult)
        {
            long result = IO.Authenticate(cardNumber, context, out processorResult);
            return result;
        }

        public decimal GetBalance(long accountId, Dictionary<string, object> context, out ProcessorResult processorResult)
        {
            decimal result = IO.GetBalance(accountId, context, out processorResult);
            return result;
        }

        public long Load(long accountId, FundRequest fundRequest, Dictionary<string, object> context, out ProcessorResult processorResult)
        {
            long result = IO.Load(accountId, fundRequest, context, out processorResult);
            return result;
        }

        public long Withdraw(long accountId, FundRequest fundRequest, Dictionary<string, object> context, out ProcessorResult processorResult)
        {
            long result = IO.Withdraw(accountId, fundRequest, context, out processorResult);
            return result;
        }

        public void Commit(long transactionId, Dictionary<string, object> context, out ProcessorResult processorResult, string cardNumber = "")
        {
            FISTrx stagedTrx = getFISTrx(transactionId);

            if (stagedTrx.TransactionType == FISTransactionType.ATM)// .Activation)
                stagedTrx.Description = "ATM";// "Account activation";
            else
                stagedTrx.Description = "POS";

            try
            {
                string confirmationId = string.Empty;
                /*
                if (stagedTrx.TransactionType == TSysTransactionType.Activation)
                    activateAccount(stagedTrx.Account, getContextTimezone(context));
                else if (stagedTrx.TransactionType == TSysTransactionType.Credit)
                {
                    confirmationId = _TSysIO.Load(decryptCardNumber(stagedTrx.Account.CardNumber), stagedTrx.Amount, stagedTrx.Description);

                    // special code for Synovus - call applyFee silently for the activation fee if this is the first credit
                    if (firstLoadWithActivation(stagedTrx.Account.Id))
                    {
                        Trace.WriteLine(string.Format("First credit: calling applyFee silently for TSys account Id {0}", stagedTrx.Account.AccountId));
                        string feeId = _TSysIO.ApplyFee(stagedTrx.Account.AccountId, 4m, "Account Opening Fee");
                        Trace.WriteLine(string.Format("success. Fee confirmationId: {0}", feeId));
                    }
                }
                else  // Debit
                    confirmationId = _TSysIO.Withdraw(stagedTrx.Account.AccountId, stagedTrx.Amount, stagedTrx.Description);
                */

                activateAccount(stagedTrx.Account, getContextTimezone(context));

                stagedTrx.ConfirmationId = confirmationId;
                stagedTrx.DTTransmission = DateTime.Now;
                stagedTrx.Status = FISTransactionStatus.Committed;

                processorResult = new ProcessorResult(true);
                processorResult.ConfirmationNumber = stagedTrx.ConfirmationId;
            }
            catch (Exception tex)
            {
                stagedTrx.ErrorMsg = tex.Message;

                processorResult = new ProcessorResult(FundException.PROVIDER_ERROR, tex.Message, false, tex);
            }

            stagedTrx.DTLastMod = MGI.TimeStamp.Clock.DateTimeWithTimeZone(getContextTimezone(context));
            stagedTrx.DTServerLastMod = DateTime.Now;

            TransactionRepo.UpdateWithFlush(stagedTrx);
        }

        public long Activate(long accountId, FundRequest fundRequest, Dictionary<string, object> context, out ProcessorResult processorResult)
        {
            FISTrx newTransaction = stageTransaction(accountId, FISTransactionType.POS, fundRequest.Amount, context);

            processorResult = new ProcessorResult(true);

            return newTransaction.Account.Id;
        }
        
        public CardAccount Lookup(long accountId)
        {
            FISAccount account = getAccount(accountId);

            CardAccount cardAccount = null;
            long cardNumber = 0L;

            if (account.Activated == false)
                return null;

            try
            {
                cardNumber = 8907220000576574;// IO.GetActiveCard(account.UserId, account.AccountId);
            }
            catch (Exception ex)
            {
                
            }

            Random rand = new Random(1235487);
            string randomnumercstring = (Math.Abs(rand.Next(54687) * 10000)).ToString();
            Dictionary<string, object> context = new Dictionary<string, object>();
            ProcessorResult result;

            cardAccount = Mapper.ToCardAccount(account);
            cardAccount.CardNumber = cardNumber.ToString();
            cardAccount.CardBalance = IO.GetBalance(account.AccountId,context, out result);

            return cardAccount;
        }

        public CardAccount LookupCardAccount(long accountId)
        {
            FISAccount account = getAccount(accountId);

            if (account.Activated == false)
                return null;

            account.CardNumber = decryptCardNumber(account.CardNumber);
            CardAccount cardAccount = Mapper.ToCardAccount(account);

            return cardAccount;
        }

        public long GetPanForCardNumber(string cardNumber, Dictionary<string, object> context)
        {
            ProcessorResult processorResult;
            return Authenticate(cardNumber, context, out processorResult);
        }

        public long UpdateAmount(long cxnFundTrxId, decimal amount, string timezone)
        {
            FISTrx fisTrx = getFISTrx(cxnFundTrxId);

            fisTrx.Amount = amount;
            fisTrx.DTLastMod = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            fisTrx.DTServerLastMod = DateTime.Now;

            TransactionRepo.UpdateWithFlush(fisTrx);

            return fisTrx.Id;
        }

        public FundTrx Get(long cxnFundTrxId)
        {
            FISTrx fisTrx = getFISTrx(cxnFundTrxId);

            Dictionary<string, object> context = new Dictionary<string, object>();
            ProcessorResult result;

            FundTrx fundTrx = Mapper.ToFundTrx(fisTrx);
            fundTrx.CardBalance = IO.GetBalance(fisTrx.Account.AccountId,context, out result);
            fundTrx.Account.CardNumber = decryptCardNumber(fisTrx.Account.CardNumber);
            fundTrx.Account.CardNumber = fundTrx.Account.CardNumber.Length > 4 ? fundTrx.Account.CardNumber.Substring(fundTrx.Account.CardNumber.Length - 4) : fundTrx.Account.CardNumber;
            return fundTrx;
        }

        public void UpdateRegistrationDetails(long cxnAccountId, string cardNumber, string timezone)
        {
            FISAccount account = getAccount(cxnAccountId);

            account.CardNumber = encryptCardNumber(cardNumber);
            account.DTLastMod = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            account.DTServerLastMod = DateTime.Now;

            AccountRepo.UpdateWithFlush(account);
        }

        #endregion

		#region private methods

        private FISAccount getAccount(long accountId)
        {
            FISAccount Acct = AccountRepo.FindBy(a => a.Id == accountId);

            if (Acct == null)
                throw new FundException(FundException.ACCOUNT_NOT_FOUND, string.Format("Could not find funds account {0}", accountId));

            return Acct;
        }

        private FISTrx stageTransaction(long accountId, FISTransactionType transactionType, decimal amount, Dictionary<string, object> context)
        {
            FISAccount Acct = getAccount(accountId);
            ProcessorResult result;

            // stage transaction
            FISTrx newTransaction = new FISTrx
            {
				Account = Acct,
				Amount = amount,
				DTCreate = DateTime.Now,// MGI.TimeStamp.Clock.DateTimeWithTimeZone(getContextTimezone(context)),
				DTServerCreate = DateTime.Now,
				TransactionType = transactionType,
				Status = FISTransactionStatus.Staged,
				Description = string.Empty,
				Balance = IO.GetBalance(Acct.AccountId, context, out result),
				ChannelPartnerID = getContextChannelPartnerId(context)
			};

            TransactionRepo.AddWithFlush(newTransaction);

            return newTransaction;
        }
        private long getContextChannelPartnerId(Dictionary<string, object> context)
        {
            return Convert.ToInt64(getContextItem(context, "ChannelPartnerId"));
        }
        private object getContextItem(Dictionary<string, object> context, string key)
        {
            if (!context.ContainsKey(key))
                throw new FundException(FundException.CONTEXT_NOT_FOUND, string.Format("{0} not set in the context", key));

            return context[key];
        }
        private FISTrx getFISTrx(long trxId)
        {
            FISTrx fisTrx = TransactionRepo.FindBy(x => x.Id == trxId);

            if (fisTrx == null)
                throw new FundException(FundException.TRANSACTION_NOT_FOUND, string.Format("Could not find transaction Id {0}", trxId));

            return fisTrx;
        }
        private string encryptCardNumber(string cardNumber)
        {
            return DataProtectionSvc.Encrypt(cardNumber, 0);
        }
        private string decryptCardNumber(string encryptedCard)
        {
            return DataProtectionSvc.Decrypt(encryptedCard, 0);
        }
        
        private void activateAccount(FISAccount fisAccount, string timezone)
        {
            //TSysIOProfile tSysProfile = Mapper.ToTSysIOProfile(fisAccount);
            FISIOProfile fisProfile = Mapper.ToFISIOProfile(fisAccount);

            // decrypt card number to activate with TSys
            //fisProfile.CardNumber = long.Parse(decryptCardNumber(fisAccount.CardNumber));

            // update account details and activate
            //IO.UpdateCardAccount(fisProfile);
            //IO.ActivateCardAccount(fisProfile);

            fisAccount.Activated = true;
            fisAccount.DTLastMod = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            fisAccount.DTServerLastMod = DateTime.Now;

            AccountRepo.UpdateWithFlush(fisAccount);
        }

        private string getContextTimezone(Dictionary<string, object> context)
        {
            return Convert.ToString(getContextItem(context, "TimeZone"));
        }
        #endregion
    }
}
