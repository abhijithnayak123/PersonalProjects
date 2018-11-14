using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MGI.Common.Sys;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.MVA.Server.Contract
{
    [ServiceContract]
    public interface IBillPayService
    {

        #region Biller Related Methods

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        List<Product> GetBillers( string channelPartnerName, string searchTerm );

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        List<FavoriteBiller> GetFrequentBillers(long customerSessionId);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        void AddFavoriteBiller(long customerSessionId, FavoriteBiller favoriteBiller );

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        bool UpdateFavoriteBillerAccountNumber(long customerSessionId, long billerId, string accountNumber );

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        bool UpdateFavoriteBillerStatus(long customerSessionId, long billerId, bool status );

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        BillerInfo GetBillerInfo(long customerSessionId, string billerNameOrCode);

        #endregion

        #region BillPay Trx Methods

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        BillFee GetBillPaymentFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, long transactionId = 0);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        List<Field> GetBillPaymentProviderAttributes(long customerSessionId, string billerNameOrCode, long transactionId);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        long ValidateBillPayment(long customerSessionId, BillPayment billPayment, long transactionId, int accountNumberRetryCount = 1);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        BillPayTransaction GetBillerLastTransaction(string billerCode, long customerSessionId);

        #endregion
    }
}
