using TCF.Channel.Zeo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CommonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface IShoppingCartService
    {


        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetShoppingCart(long customerSessionId, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response ShoppingCartCheckout(decimal cashToCustomer, Helper.ShoppingCartCheckoutStatus status, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response ParkShoppingCartTransaction(long customerSessionId, long transactionId, int productId, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response RemoveCheck(long transactionId, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response RemoveBillPay(long transactionId, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response RemoveMoneyTransfer(long transactionId, int productType, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response RemoveFund(long transactionId, bool hasFundsAccount, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response RemoveMoneyOrder(long transactionId, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response RemoveCashIn(ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response PostFlush(decimal cardBalance, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response FinalCommit(ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetPrepaidCardNumber(ZeoContext context);

        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response IsShoppingCartEmpty(Data.ZeoContext context);

        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response CanCloseCustomerSession(Data.ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetAllParkedTransaction(ZeoContext context);

        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response GenerateReceiptsForShoppingCart(long customerSessionId, long shoppingCartId, ZeoContext context);

    }
}
