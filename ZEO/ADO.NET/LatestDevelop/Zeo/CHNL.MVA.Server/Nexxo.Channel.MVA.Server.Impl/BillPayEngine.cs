using System;
using System.Collections.Generic;
using MGI.Channel.MVA.Server.Contract;
using MGI.Channel.Shared.Server.Data;
using MGI.Core.Partner.Data;
using MGI.Common.Util;

namespace MGI.Channel.MVA.Server.Impl
{
    public partial class MVAEngine : IBillPayService
    {
        #region IBillPayService Impl

        #region Biller Related Methods

        public List<MGI.Channel.Shared.Server.Data.Product> GetBillers(string channelPartnerName, string searchTerm)
        {
            MGIContext context = Self.GetPartnerContext(channelPartnerName);
            long customerSessionId = 0L;
			return ConsumerEngine.GetBillers(customerSessionId, context.ChannelPartnerId, searchTerm, context);
        }

        public List<FavoriteBiller> GetFrequentBillers(long customerSessionId)
        {
            MGIContext context = Self.GetCustomerContext(customerSessionId);

			List<FavoriteBiller> favoriteBillers = ConsumerEngine.GetFrequentBillers(customerSessionId, context.AlloyId, context);
            
            foreach (var biller in favoriteBillers)
            {
                biller.AccountNumber = ConsumerEngine.GetFavoriteBiller(customerSessionId, biller.BillerCode, context).AccountNumber;

                BillPayTransaction billPayTransaction = ConsumerEngine.GetBillerLastTransaction(customerSessionId, biller.BillerCode, context.AlloyId, context);

                if (billPayTransaction != null)
                {
                    biller.LastTransactionDate = (DateTime?)billPayTransaction.MetaData["LastTransactionDate"];

                    biller.LastTransactionAmount = billPayTransaction.Amount;
                }


            }
            return favoriteBillers;
        }

        public void AddFavoriteBiller(long customerSessionId, FavoriteBiller favoriteBiller)
        {
            ConsumerEngine.AddFavoriteBiller(customerSessionId,favoriteBiller,Self.GetCustomerContext(customerSessionId));
        }

        public bool UpdateFavoriteBillerAccountNumber(long customerSessionId, long billerId, string accountNumber)
        {
            return ConsumerEngine.UpdateFavoriteBillerAccountNumber(customerSessionId, billerId, accountNumber, Self.GetCustomerContext(customerSessionId));
        }

        public bool UpdateFavoriteBillerStatus(long customerSessionId, long billerId, bool status)
        {
            return ConsumerEngine.UpdateFavoriteBillerStatus(customerSessionId, billerId, status, Self.GetCustomerContext(customerSessionId));
        }

        public BillerInfo GetBillerInfo(long customerSessionId, string billerNameOrCode)
        {
            return ConsumerEngine.GetBillerInfo(customerSessionId, billerNameOrCode, Self.GetCustomerContext(customerSessionId));
        }

        #endregion

        #region BillPay Trx Methods

        public BillFee GetBillPaymentFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, long transactionId = 0)
        {
            MGIContext context = Self.GetCustomerContext(customerSessionId);
            
            if ( transactionId != 0 )
				context.TrxId = transactionId;

            return ConsumerEngine.GetBillPaymentFee(customerSessionId, billerNameOrCode, accountNumber, amount, null, context);
        }

        public List<Field> GetBillPaymentProviderAttributes(long customerSessionId, string billerNameOrCode, long transactionId)
        {
            MGIContext context = Self.GetCustomerContext( customerSessionId );
			context.TrxId = transactionId;

            return ConsumerEngine.GetBillPaymentProviderAttributes(customerSessionId, billerNameOrCode, string.Empty, context);
        }

        public long ValidateBillPayment(long customerSessionId, BillPayment billPayment, long transactionId, int accountNumberRetryCount)
        {
            MGIContext context = Self.GetCustomerContext(customerSessionId);
            context.TrxId = transactionId;
            context.AccountNumberRetryCount = accountNumberRetryCount > 1 ? "2" : accountNumberRetryCount.ToString();

            return ConsumerEngine.ValidateBillPayment(customerSessionId, billPayment, context);
        }

        public BillPayTransaction GetBillerLastTransaction(string billerCode, long customerSessionId)
        {
            MGIContext context = Self.GetCustomerContext(customerSessionId);

            return ConsumerEngine.GetBillerLastTransaction(customerSessionId, billerCode, context.AlloyId, context);
        }

        #endregion

        #endregion


    }
}
