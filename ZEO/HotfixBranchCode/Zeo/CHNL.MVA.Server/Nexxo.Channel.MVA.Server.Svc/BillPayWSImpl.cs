using System;
using System.Collections.Generic;
using MGI.Channel.MVA.Server.Contract;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.MVA.Server.Svc
{
    public partial class MVAWSImpl : IBillPayService
    {
        #region IBillPayService Impl

        #region Biller Related Methods

        public List<Product> GetBillers(string channelPartnerName, string searchTerm)
        {
            return MVAEngine.GetBillers(channelPartnerName, searchTerm);
        }

        public List<FavoriteBiller> GetFrequentBillers(long customerSessionId)
        {
            return MVAEngine.GetFrequentBillers( customerSessionId );
        }

        public void AddFavoriteBiller(long customerSessionId, FavoriteBiller favoriteBiller)
        {
            MVAEngine.AddFavoriteBiller(customerSessionId,favoriteBiller);
        }

        public bool UpdateFavoriteBillerAccountNumber(long customerSessionId, long billerId, string accountNumber)
        {
            return MVAEngine.UpdateFavoriteBillerAccountNumber(customerSessionId, billerId, accountNumber);
        }

        public bool UpdateFavoriteBillerStatus(long customerSessionId, long billerId, bool status)
        {
            return MVAEngine.UpdateFavoriteBillerStatus(customerSessionId, billerId, status);
        }

        public BillerInfo GetBillerInfo(long customerSessionId, string billerNameOrCode)
        {
            return MVAEngine.GetBillerInfo(customerSessionId, billerNameOrCode);
        }

        #endregion

        #region BillPay Trx Methods

        public BillFee GetBillPaymentFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, long transactionId = 0)
        {
            return MVAEngine.GetBillPaymentFee(customerSessionId, billerNameOrCode, accountNumber, amount, transactionId);
        }

        public List<Field> GetBillPaymentProviderAttributes(long customerSessionId, string billerNameOrCode, long transactionId)
        {
            return MVAEngine.GetBillPaymentProviderAttributes(customerSessionId, billerNameOrCode, transactionId);
        }

        public long ValidateBillPayment(long customerSessionId, BillPayment billPayment, long transactionId, int AccountNumberRetryCount = 1)
        {
            return MVAEngine.ValidateBillPayment(customerSessionId, billPayment, transactionId, AccountNumberRetryCount);
        }

        public BillPayTransaction GetBillerLastTransaction(string billerCode, long customerSessionId)
        {
            return MVAEngine.GetBillerLastTransaction(billerCode, customerSessionId);
        }

        #endregion

        #endregion
    }
}
