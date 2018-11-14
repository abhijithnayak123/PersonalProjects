using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Contract;
using P3Net.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using P3Net.Data;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{

    public partial class ZeoCoreImpl : ILocationProcessorCredentialService
    {
        StoredProcedure coreLocationProcessorCredential;
        public bool SaveLocationProcessorCredentials(LocationProcessorCredentials locProcessorCredential, string timeZone, ZeoContext context)
        {
            try
            {

                coreLocationProcessorCredential = new StoredProcedure("usp_SaveLocationProcessorCredentials");

                coreLocationProcessorCredential.WithParameters(InputParameter.Named("UserName").WithValue(locProcessorCredential.UserName));
                coreLocationProcessorCredential.WithParameters(InputParameter.Named("Password").WithValue(locProcessorCredential.Password));
                coreLocationProcessorCredential.WithParameters(InputParameter.Named("Identifier").WithValue(locProcessorCredential.Identifier));
                coreLocationProcessorCredential.WithParameters(InputParameter.Named("ProviderId").WithValue(locProcessorCredential.ProviderId));
                coreLocationProcessorCredential.WithParameters(InputParameter.Named("locationId").WithValue(locProcessorCredential.LocationId));
                coreLocationProcessorCredential.WithParameters(InputParameter.Named("Identifier2").WithValue(locProcessorCredential.Identifier2));
                coreLocationProcessorCredential.WithParameters(InputParameter.Named("dTServerCreate").WithValue(DateTime.Now));
                coreLocationProcessorCredential.WithParameters(InputParameter.Named("dTTerminalCreate").WithValue(Helper.GetTimeZoneTime(timeZone)));
                coreLocationProcessorCredential.WithParameters(InputParameter.Named("dtTerminalLastModified").WithValue(Helper.GetTimeZoneTime(timeZone)));
                coreLocationProcessorCredential.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(DateTime.Now));

                int isExecute = DataHelper.GetConnectionManager().ExecuteNonQuery(coreLocationProcessorCredential);
                return (isExecute == 0) ? false : true;
            }
            catch (Exception ex)
            {
                throw new LocationException(LocationException.LOCATION_PROCESSOR_CREDENTIALS_CREATE_FAILED, ex);
            }
        }
        public List<LocationProcessorCredentials> GetLocationProcessorCredentials(long locationId, ZeoContext context)
        {
            try
            {
                List<LocationProcessorCredentials> credentials = new List<LocationProcessorCredentials>();
                LocationProcessorCredentials credential = new LocationProcessorCredentials();

                coreLocationProcessorCredential = new StoredProcedure("usp_GetLocationProcessorCredentials");
                coreLocationProcessorCredential.WithParameters(InputParameter.Named("locationId").WithValue(locationId));
                //coreLocationProcessorCredential.WithParameters(InputParameter.Named("locationId").WithValue(locationId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreLocationProcessorCredential))
                {
                    while (datareader.Read())
                    {
                        credential = new LocationProcessorCredentials();
                        credential.ProviderId = datareader.GetInt64OrDefault("ProviderId");
                        credential.UserName = datareader.GetStringOrDefault("UserName");
                        credential.Password = datareader.GetStringOrDefault("PassWord");
                        credential.Identifier = datareader.GetStringOrDefault("Identifier");
                        credential.Identifier2 = datareader.GetStringOrDefault("Identifier2");
                        credential.LocationId = locationId;
                        credentials.Add(credential);
                    }
                }

                return credentials;
            }
            catch (Exception ex)
            {
                throw new LocationException(LocationException.LOCATION_PROCESSOR_CREDENTIALS_GET_FAILED, ex);
            }
        }

    }
}
