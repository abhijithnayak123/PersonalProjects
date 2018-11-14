using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using commonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Contract
{
    public interface ILocationService
    {
        List<Location> GetLocationsByChannelPartnerId(long channelpartnerId, commonData.ZeoContext context);
        List<Location> GetLocationById(long locationId, commonData.ZeoContext context);
        long CreateLocation( Location location, commonData.ZeoContext context);
        bool UpdateLocation( Location location, commonData.ZeoContext context);
        bool ValidateLocation( Location Location, commonData.ZeoContext context);

        List<MasterData> GetStateNamesAndIdByChannelPartnerId(commonData.ZeoContext context);

    }
}
