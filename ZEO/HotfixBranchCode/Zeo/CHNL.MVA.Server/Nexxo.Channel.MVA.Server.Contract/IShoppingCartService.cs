using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Sys;

namespace MGI.Channel.MVA.Server.Contract
{
    /// <summary>
    /// ShoppingCart related all services
    /// </summary>
    [ServiceContract]
    public interface IShoppingCartService
    {
        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        Receipts Checkout(long customerSessionId);


        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        void RemoveMoneyTransfer(long customerSessionId, long moneyTransferId);

        [OperationContract]
        [FaultContract(typeof(NexxoSOAPFault))]
        void RemoveBillPay(long customerSessionId, long billPayId);


    }
}
