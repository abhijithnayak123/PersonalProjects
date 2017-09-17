using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ServiceModel;
using MGI.Common.Sys;
using MGI.Common.Util;
using MGI.Cxn.Fund.Contract;
using MGI.Cxn.Fund.TSys.Data;
using MGI.Cxn.Fund.TSys.Contract;

namespace MGI.Cxn.Fund.TSys.Impl
{
    public class SimulatorIO : IIO
    {
        public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { private get; set; }

        private string _apiUsername;
        public string APIUserName { set { _apiUsername = value; } }

        private string _apiPassword;
        public string APIPassword { set { _apiPassword = value; } }

        private bool _cardNumberLogging;
        public bool CardNumberLogging { set { _cardNumberLogging = value; } }

        #region ITSysIO implementation
        public TSysIONewUser ValidateNewCardAccount(long programId, string kitId, long cardNumber)
        {
            return new TSysIONewUser
            {
                UserId = NexxoUtil.GetLongRandomNumber(6),
                CardId = cardNumber,
                AccountId = Convert.ToInt64(kitId),
                Balance = 300
            };
        }

        public TSysIONewUser ValidateExistingCardAccount(long programId, string kitId)
        {
            return new TSysIONewUser
             {
                 UserId = NexxoUtil.GetLongRandomNumber(6),
                 CardId = NexxoUtil.GetLongRandomNumber(16),
                 AccountId = Convert.ToInt64(kitId),
                 Balance = 300
             };
        }

        public void UpdateCardAccount(TSysIOProfile account)
        {
            NLogger.Info("basic user profile updated");
            NLogger.Info("Addresses updated");
            NLogger.Info("phone updated");
            NLogger.Info("SSN updated");
        }

        public void ActivateCardAccount(TSysIOProfile tSysAccount)
        {
            long CardNo = NexxoUtil.GetLongRandomNumber(16);
            NLogger.Info(string.Format("Activating card ID: {0}, Number: {1}", CardNo, ISOCard.EncodeCardNumber(CardNo)));
            NLogger.Info(string.Format("New card created. ID: {0}, Number: {1}", CardNo, cardNumberString(CardNo)));
        }

        public void ValidateCard(long userId, long accountId, long cardNumber)
        {
            NLogger.Info(string.Format("card: {0}, status: {1}", cardNumberString(cardNumber), "Active"));
        }

        public long GetActiveCard(long userId, long accountId)
        {
            //validateAccount(accountId);
            return getActiveCard(userId, false);
        }

        public string Load(string cardNumber, decimal amount, string description)
        {
            string confirmationId = string.Empty;
            try
            {
                confirmationId = Convert.ToString(NexxoUtil.GetLongRandomNumber(10));
            }
            catch (Exception ex)
            {
                NLogger.Error(string.Format("Load failed: {0}", ex));
            }
            return confirmationId;
        }

        public string Withdraw(long accountId, decimal amount, string description)
        {
            long confirmationId = 0;
            try
            {
                confirmationId = NexxoUtil.GetLongRandomNumber(10);
            }
            catch (Exception ex)
            {
                NLogger.Error(string.Format("Adjust failed: {0}", ex));
            }
            return confirmationId.ToString();
        }

        public decimal GetBalance(long accountId)
        {
            return validateAccount(accountId);
        }

        public string ApplyFee(long accountId, decimal fee, string description)
        {
            string transactionId = string.Empty;
            try
            {
                transactionId = Convert.ToString(NexxoUtil.GetLongRandomNumber(10));
            }
            catch (Exception ex)
            {
                NLogger.Error(string.Format("ApplyFee failed for account {0}: {1}", accountId, ex));
            }

            return transactionId;
        }

        #endregion

        #region private methods

        private decimal validateAccount(long accountId)
        {
            return 23;
        }

        private long getActiveCard(long userId, bool acceptAny)
        {
            return 4756755000044500;
        }

        private long convertDollarsToCents(decimal dollarAmount)
        {
            return (long)(dollarAmount * 100);
        }

        private decimal convertCentsToDollars(long centsAmount)
        {
            return (decimal)(centsAmount / 100m);
        }

        private string cardNumberString(long cardNumber)
        {
            return cardNumber.ToString();
        }
        #endregion
    }
}
