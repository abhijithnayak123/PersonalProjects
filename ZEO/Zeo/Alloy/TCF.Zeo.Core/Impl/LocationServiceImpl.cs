using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Core.Contract;
using P3Net.Data.Common;
using System.Data;
using TCF.Zeo.Common.Data;
using P3Net.Data;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl  : ILocationService
    {

        public List<Location> GetLocationsByChannelPartnerId(long channelpartnerId, ZeoContext context)
        {
            try
            {
                List<Location> Locations = new List<Location>();
                Location location;

                StoredProcedure getLocationsProcedure = new StoredProcedure("usp_GetLocationsByChannelPartnerId");

                getLocationsProcedure.WithParameters(InputParameter.Named("channelpartnerId").WithValue(channelpartnerId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(getLocationsProcedure))
                {
                    while (datareader.Read())
                    {
                        location = new Location();
                        location.LocationIdentifier = datareader.GetStringOrDefault("LocationID");
                        location.LocationName = datareader.GetStringOrDefault("LocationName");
                        location.IsActive = datareader.GetBooleanOrDefault("IsActive");
                        Locations.Add(location);
                    }
                }

                return Locations;
            }
            catch (Exception ex)
            {
                throw new LocationException(LocationException.LOCATION_GET_FAILED, ex);
            }
        }

        public List<Location> GetLocationById(long locationId, ZeoContext context)
        {
            try
            {
                List<Location> Locations = new List<Location>();
                Location location;

                StoredProcedure getLocationProcedure = new StoredProcedure("usp_GetLocationById");

                getLocationProcedure.WithParameters(InputParameter.Named("locationid").WithValue(locationId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(getLocationProcedure))
                {
                    while (datareader.Read())
                    {
                        location = new Location();
                        location.LocationId = datareader.GetInt64OrDefault("locationid");
                        location.LocationName = datareader.GetStringOrDefault("LocationName");
                        location.IsActive = datareader.GetBooleanOrDefault("IsActive");
                        location.Address1 = datareader.GetStringOrDefault("Address1");
                        location.Address2 = datareader.GetStringOrDefault("Address2");
                        location.City = datareader.GetStringOrDefault("City");
                        location.State = datareader.GetStringOrDefault("state");
                        location.ZipCode = datareader.GetStringOrDefault("ZipCode");
                        location.PhoneNumber = datareader.GetStringOrDefault("PhoneNumber");
                        location.BankID = datareader.GetStringOrDefault("BankId");
                        location.BranchID = datareader.GetStringOrDefault("BranchId");
                        location.TimezoneID = datareader.GetStringOrDefault("TimezoneID");
                        location.LocationIdentifier = datareader.GetStringOrDefault("LocationIdentifier");
                        Locations.Add(location);
                    }
                }

                return Locations;
            }
            catch(Exception ex)
            {
                throw new LocationException(LocationException.LOCATION_GET_FAILED, ex);
            }
        }

        public long CreateLocation(Location createlocation, ZeoContext context)
        {

            try
            {

                StoredProcedure createLocation = new StoredProcedure("usp_CreateLocation");

                createLocation.WithParameters(InputParameter.Named("isActive").WithValue(createlocation.IsActive));
                createLocation.WithParameters(InputParameter.Named("locationName").WithValue(createlocation.LocationName));
                createLocation.WithParameters(InputParameter.Named("address1").WithValue(createlocation.Address1));
                createLocation.WithParameters(InputParameter.Named("address2").WithValue(createlocation.Address2));
                createLocation.WithParameters(InputParameter.Named("city").WithValue(createlocation.City));
                createLocation.WithParameters(InputParameter.Named("state").WithValue(createlocation.State));
                createLocation.WithParameters(InputParameter.Named("zipCode").WithValue(createlocation.ZipCode));
                createLocation.WithParameters(InputParameter.Named("channelPartnerId").WithValue(createlocation.ChannelPartnerId));
                createLocation.WithParameters(InputParameter.Named("phoneNumber").WithValue(createlocation.PhoneNumber));
                createLocation.WithParameters(InputParameter.Named("bankId").WithValue(createlocation.BankID));
                createLocation.WithParameters(InputParameter.Named("branchId").WithValue(createlocation.BranchID));
                createLocation.WithParameters(InputParameter.Named("NoOfCounterId").WithValue(createlocation.NoOfCounterIDs));
                createLocation.WithParameters(InputParameter.Named("locationIdentifier").WithValue(createlocation.LocationIdentifier));
                createLocation.WithParameters(InputParameter.Named("timezone").WithValue(createlocation.TimezoneID));
                createLocation.WithParameters(InputParameter.Named("dTTerminatalCreate").WithValue(Helper.GetTimeZoneTime(createlocation.TimezoneID)));
                createLocation.WithParameters(InputParameter.Named("dTserverCreate").WithValue(DateTime.Now));
                createLocation.WithParameters(OutputParameter.Named("locationId").OfType<long>());

                DataHelper.GetConnectionManager().ExecuteNonQuery(createLocation);

                createlocation.LocationId = Convert.ToInt64(createLocation.Parameters["locationId"].Value);
                return createlocation.LocationId;
            }
            catch (Exception ex)
            {
                throw new LocationException(LocationException.LOCATION_CREATE_FAILED, ex);
            }
        }

        public bool UpdateLocation(Location updateloc, ZeoContext context)
        {
            try
            {

                StoredProcedure updateLocation = new StoredProcedure("usp_UpdateLocation");
                updateLocation.WithParameters(InputParameter.Named("isActive").WithValue(updateloc.IsActive));
                updateLocation.WithParameters(InputParameter.Named("locationName").WithValue(updateloc.LocationName));
                updateLocation.WithParameters(InputParameter.Named("address1").WithValue(updateloc.Address1));
                updateLocation.WithParameters(InputParameter.Named("address2").WithValue(updateloc.Address2));
                updateLocation.WithParameters(InputParameter.Named("city").WithValue(updateloc.City));
                updateLocation.WithParameters(InputParameter.Named("State").WithValue(updateloc.State));
                updateLocation.WithParameters(InputParameter.Named("zipCode").WithValue(updateloc.ZipCode));
                updateLocation.WithParameters(InputParameter.Named("channelPartnerId").WithValue(updateloc.ChannelPartnerId));
                updateLocation.WithParameters(InputParameter.Named("phoneNumber").WithValue(updateloc.PhoneNumber));
                updateLocation.WithParameters(InputParameter.Named("bankId").WithValue(updateloc.BankID));
                updateLocation.WithParameters(InputParameter.Named("branchID").WithValue(updateloc.BranchID));
                updateLocation.WithParameters(InputParameter.Named("locationId").WithValue(updateloc.LocationId));
                updateLocation.WithParameters(InputParameter.Named("timeZoneId").WithValue(updateloc.TimezoneID));
                updateLocation.WithParameters(InputParameter.Named("numberOfCounterId").WithValue(updateloc.NoOfCounterIDs));
                updateLocation.WithParameters(InputParameter.Named("locationIdentifier").WithValue(updateloc.LocationIdentifier));
                updateLocation.WithParameters(InputParameter.Named("dTTerminatalLastModified").WithValue(Helper.GetTimeZoneTime(updateloc.TimezoneID)));
                updateLocation.WithParameters(InputParameter.Named("dTServerLastModified").WithValue(DateTime.Now));

                DataHelper.GetConnectionManager().ExecuteNonQuery(updateLocation);

                return true;
            }
            catch (Exception ex)
            {
                throw new LocationException(LocationException.LOCATION_UPDATE_FAILED, ex);
            }

        }

        public int ValidateLocation(Location validateLocation, ZeoContext context)
        {
            try
            {
                int errorCode = 0;

                StoredProcedure validatelocation = new StoredProcedure("usp_ValidateLocation");
                validatelocation.WithParameters(InputParameter.Named("bankId").WithValue(validateLocation.BankID));
                validatelocation.WithParameters(InputParameter.Named("branchId").WithValue(validateLocation.BranchID));
                validatelocation.WithParameters(InputParameter.Named("locationIdentifier").WithValue(validateLocation.LocationIdentifier));
                validatelocation.WithParameters(InputParameter.Named("locationName").WithValue(validateLocation.LocationName));
                validatelocation.WithParameters(InputParameter.Named("channelPartnerId").WithValue(validateLocation.ChannelPartnerId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(validatelocation))
                {
                    while (datareader.Read())
                    {
                        errorCode = datareader.GetInt32("errorCode");
                    }
                }

                return errorCode;
            }
            catch (Exception ex)
            {
                throw new LocationException(LocationException.DUPLICATE_LOCATIONID, ex);
            }

        }

        public List<MasterData> GetStateNameAndIdByChannelPartnerId(ZeoContext context)
        {
            try
            {
                List<MasterData> stateNames = new List<MasterData>();
                StoredProcedure getLocationStateNames = new StoredProcedure("usp_GetStateNameAndIdByChannelPartnerId");
                getLocationStateNames.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));

                MasterData locationDetails = null;

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(getLocationStateNames))
                {
                    while (datareader.Read())
                    {
                        locationDetails = new MasterData()
                        {
                            Code = datareader.GetStringOrDefault("LocationName"),
                            Name = datareader.GetStringOrDefault("stateName"),
                            Id = datareader.GetInt64OrDefault("LocationID")
                        };
                        stateNames.Add(locationDetails);
                    }
                }

                return stateNames;
            }
            catch (Exception ex)
            {
                throw new LocationException(LocationException.GET_LACATION_STATE_NAMES, ex);
            }
        }
    }
}
