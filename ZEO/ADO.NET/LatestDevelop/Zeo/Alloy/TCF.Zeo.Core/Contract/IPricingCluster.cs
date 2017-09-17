using System;
using System.Collections.Generic;
using TCF.Zeo.Core.Data;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Contract
{
    public interface IPricingCluster : IDisposable 
    {
        List<Pricing> GetBaseFee(TransactionType transactionType, long channelPartnerId, long locationId, string productType, ZeoContext context);
    }
}
