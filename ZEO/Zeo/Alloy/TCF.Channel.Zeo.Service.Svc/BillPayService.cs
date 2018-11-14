using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCF.Channel.Zeo.Data;

namespace TCF.Channel.Zeo.Service.Svc
{
    public partial class ZeoService : IBillPayService
    {
        #region IBillPayService methods

        #region Bill pay transaction's Methods

        public Response GetLocations(long transactionId, string billerName, string accountNumber, decimal amount, ZeoContext context)
        {
            return serviceEngine.GetLocations(transactionId, billerName, accountNumber, amount, context);
        }

        public Response GetBillPayFee(long transactionId, string billerNameOrCode, string accountNumber, decimal amount, BillerLocation billerlocation, ZeoContext context)
        {
            return serviceEngine.GetBillPayFee(transactionId, billerNameOrCode, accountNumber, amount, billerlocation, context);
        }

        public Response GetProviderAttributes(string billerNameOrCode, string location, ZeoContext context)
        {
            return serviceEngine.GetProviderAttributes(billerNameOrCode, location, context);
        }

        public Response StageBillPayment(long transactionId, ZeoContext context)
        {
            return serviceEngine.StageBillPayment(transactionId, context);
        }

        public Response ValidateBillPayment(long transactionId, BillPayment billPayment, ZeoContext context)
        {
            return serviceEngine.ValidateBillPayment(transactionId, billPayment, context);
        }

        public Response AddPastBillers(string cardNumber, ZeoContext context)
        {
            return serviceEngine.AddPastBillers(cardNumber, context);
        }

        public Response CancelBillPayment(long transactionId, ZeoContext context)
        {
            return serviceEngine.CancelBillPayment(transactionId, context);
        }

        #endregion

        #region Biller's Methods

        public Response GetBillerInfo(string billerNameOrCode, ZeoContext context)
        {
            return serviceEngine.GetBillerInfo(billerNameOrCode, context);
        }

        public Response GetBillers(string searchTerm, ZeoContext context)
        {
            return serviceEngine.GetBillers(searchTerm, context);
        }

        public Response GetBillerDetails(string billerNameOrCode, ZeoContext context)
        {
            return serviceEngine.GetBillerDetails(billerNameOrCode, context);
        }

        public Response GetFrequentBillers(ZeoContext context)
        {
            return serviceEngine.GetFrequentBillers(context);
        }

        public Response DeleteFavoriteBiller(long billerId, ZeoContext context)
        {
            return serviceEngine.DeleteFavoriteBiller(billerId, context);
        }
        public Response GetCardInfo(ZeoContext context)
        {
             return serviceEngine.GetCardInfo(context);
        }

        public Response GetBillPayTransaction(long transactionId, ZeoContext context)
        {
            return serviceEngine.GetBillPayTransaction(transactionId, context);
        }
        #endregion

        #endregion

    }
}