using System.Collections.Generic;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;

namespace MGI.Channel.Shared.Server.Contract
{
    public interface IMoneyOrderService
    {
        MoneyOrder GetMoneyOrderStage(long customerSessionId, long moneyOrderId, MGIContext mgiContext);
    }
}
