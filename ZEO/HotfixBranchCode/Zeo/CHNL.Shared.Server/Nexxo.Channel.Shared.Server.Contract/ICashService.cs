using MGI.Common.Util;
using System.Collections.Generic;

namespace MGI.Channel.Shared.Server.Contract
{
	public interface ICashService
	{

        long CashOut(long customerSessionId, decimal amount, MGIContext mgiContext);
	
    }
}
