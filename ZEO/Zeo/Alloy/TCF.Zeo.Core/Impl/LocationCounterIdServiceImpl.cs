using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Core.Contract;
using P3Net.Data.Common;
using P3Net.Data;
using System.Data;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : ILocationCounterIdService
    {
        StoredProcedure coreCounterIdProcedure;

        public string CreateCustomerSessionCounterId(long productProviderId, long locationId, long customerSessionId, string timeZone, ZeoContext context)
        {
            string counterId = string.Empty;

            try
            {
                StoredProcedure getLocationCounterId = new StoredProcedure("usp_CreateCustomerSessionCounterId");
                getLocationCounterId.WithParameters(OutputParameter.Named("counterId").OfType<string>());
                getLocationCounterId.WithParameters(InputParameter.Named("providerId").WithValue(productProviderId));
                getLocationCounterId.WithParameters(InputParameter.Named("locationId").WithValue(locationId));
                getLocationCounterId.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));
                getLocationCounterId.WithParameters(InputParameter.Named("dtTerminalDate").WithValue(Helper.GetTimeZoneTime(timeZone)));
                getLocationCounterId.WithParameters(InputParameter.Named("dtServerDate").WithValue(DateTime.Now));

                DataHelper.GetConnectionManager().ExecuteNonQuery(getLocationCounterId);

                counterId = Convert.ToString(getLocationCounterId.Parameters["counterId"].Value);
            }
            catch (Exception ex)
            {
                 throw new LocationException(LocationException.LOCATION_COUNTERID_CUSTOMER_SESSION_FAILED, ex);
            }
            return counterId;
        }

        public string GetLocationCounterID(long locationId,int providerId, ZeoContext context)
        {
            string counterId = string.Empty;
            try
            {
                StoredProcedure getCounterId = new StoredProcedure("usp_GetLocationCounterIdByLocation");

                getCounterId.WithParameters(InputParameter.Named("locationId").WithValue(locationId));
                getCounterId.WithParameters(InputParameter.Named("providerId").WithValue(providerId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(getCounterId))
                {
                    while (datareader.Read())
                    {
                        counterId = datareader.GetStringOrDefault("CounterId");
                    }
                }

                return counterId;
            }
            catch (Exception ex)
            {
                throw new LocationException(LocationException.LOCATION_COUNTERID_GET_FAILED, ex);
            }
        }


        public bool UpdateLocationCounterID(ZeoContext context)
        {
            try
            {
                coreCounterIdProcedure = new StoredProcedure("usp_UpdateLocationCounterId");

                coreCounterIdProcedure.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(context.CustomerSessionId));
                coreCounterIdProcedure.WithParameters(InputParameter.Named("LocationId").WithValue(context.LocationId));
                coreCounterIdProcedure.WithParameters(InputParameter.Named("IsAvailable").WithValue(true));
                coreCounterIdProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(Helper.GetTimeZoneTime(context.TimeZone)));
                coreCounterIdProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now));

                DataHelper.GetConnectionManager().ExecuteNonQuery(coreCounterIdProcedure);
            }
            catch (Exception ex)
            {
                throw new LocationException(LocationException.LOCATION_COUNTERID_STATUS_UPDATE_FAILED, ex);
            }
            return true;
        }
    }
}
