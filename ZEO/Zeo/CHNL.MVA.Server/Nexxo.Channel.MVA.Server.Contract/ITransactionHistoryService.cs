using System.Collections.Generic;
using System.ServiceModel;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;

namespace MGI.Channel.MVA.Server.Contract
{
    [ServiceContract]
    public interface ITransactionHistoryService
    {
         [FaultContract(typeof(NexxoSOAPFault))]
         [OperationContract]
         List<PastTransaction> GetPastTransactions(long customerSessionId,string transactionType);

         [FaultContract(typeof(NexxoSOAPFault))]
         [OperationContract]
         BillPayTransaction GetBillPayTransaction(long customerSessionId, long transactionId);
    }
}
