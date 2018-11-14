using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using System;
using System.Collections.Generic;

namespace TCF.Zeo.Core.Contract
{
    public interface IMessageCenter
    {
        List<AgentMessage> GetMessagesByAgentId(long AgentId, ZeoContext context);

        AgentMessage GetMessageByTransactionId(long transactionId, ZeoContext context);

        void DeleteAllMessages(ZeoContext context);
    }
}
