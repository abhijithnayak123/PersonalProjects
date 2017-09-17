using TCF.Channel.Zeo.Data;
using System.ServiceModel;
using CommonData = TCF.Zeo.Common.Data;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface ICashService
    {
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response CashIn(decimal amount, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response UpdateOrCancelCashIn(decimal amount, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetCashTransaction(long transactionId,ZeoContext context);
    }
}
