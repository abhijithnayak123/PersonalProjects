using System.ServiceModel;
using TCF.Channel.Zeo.Data;
using CommonData = TCF.Zeo.Common.Data;

namespace TCF.Channel.Zeo.Service.Contract
{
    [ServiceContract]
    public interface INpsTerminalService
    {
        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response CreateNpsTerminal(NpsTerminal npsTerminal, ZeoContext context);

        [OperationContract]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response UpdateNpsTerminal(NpsTerminal npsTerminal, ZeoContext context);

        [OperationContract(Name = "GetNpsterminalByTerminalId")]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetNpsterminalByTerminalId(long Id, ZeoContext context);

        [OperationContract(Name = "GetNpsTerminalBylocationId")]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetNpsTerminalBylocationId(string locationId, ZeoContext context);

        [OperationContract(Name = "GetNpsTerminalByName")]
        [FaultContract(typeof(CommonData.ZeoSoapFault))]
        Response GetNpsTerminalByName(string name, long channelPartnerId, ZeoContext context);

    }
}
