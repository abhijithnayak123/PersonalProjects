using System.ServiceModel;
#region Zeo References
using TCF.Channel.Zeo.Data;
using CommonData = TCF.Zeo.Common.Data;
#endregion


namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface ICheckService
    {

		[FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response SubmitCheck(CheckSubmission check, ZeoContext context);

       
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response CancelCheck(long transactionId, ZeoContext context);

       
		[FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response GetCheckStatus(long transactionId, ZeoContext context);

        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response GetCheckTypes( ZeoContext context);

      
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response GetCheckFee(CheckSubmission checkSubmit, ZeoContext context);

       
		[FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response GetCheckTranasactionDetails(long transactionId, ZeoContext context);

      
		[OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetCheckFrankingData(long transactionId, ZeoContext context);

      
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response GetCheckProcessorInfo(ZeoContext context);

      
		[FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response UpdateCheckTransactionFranked(long transactionId, ZeoContext context);

        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        [OperationContract]
        Response GetCheckSession(ZeoContext context);
    }
}
