﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;
using MGI.Common.DataAccess.Contract;
using MGI.TimeStamp;


namespace MGI.Core.Partner.Impl
{
    public class LocationServiceImpl : IManageLocations
    {
        public IRepository<Location> LocationRepo { private get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manageLocation"></param>
        /// <returns></returns>
        public long Create(Location manageLocation)
        {
			long Id = 0;

            try
            {
                var location = LocationRepo.FindBy(x => x.LocationName.ToLower() == manageLocation.LocationName.ToLower());
                if (location != null)
                    throw new ChannelPartnerException(ChannelPartnerException.LOCATION_ALREADY_EXISTS, string.Format("Location Name {0} already exists.", location.LocationName));

				location = null;
				location = LocationRepo.FilterBy(x => x.BankID == manageLocation.BankID && x.BranchID == manageLocation.BranchID && x.ChannelPartnerId == manageLocation.ChannelPartnerId)
                                       .Where(x => x.BranchID.Trim() != "" && x.BankID.Trim() != "")                                   
                                       .ToList().FirstOrDefault();
				if (location != null)
				{
					throw new ChannelPartnerException(ChannelPartnerException.LOCATION_BANKID_BRANCHID_ALREADY_EXISTS, string.Format("Bank Id {0} and Branch Id {1}  is already exists.", location.BankID, location.BranchID));
				}

				CheckDuplicateLocation(manageLocation);

				manageLocation.rowguid = Guid.NewGuid();
                manageLocation.DTServerCreate = DateTime.Now;
                manageLocation.DTServerLastModified = DateTime.Now;
				manageLocation.DTTerminalCreate = Clock.DateTimeWithTimeZone(manageLocation.TimezoneID);
				LocationRepo.AddWithFlush(manageLocation);

				Id = manageLocation.Id;

				return Id;
            }
            catch (ChannelPartnerException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerException(ChannelPartnerException.LOCATION_CREATE_FAILED, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manageLocation"></param>
        public bool Update(Location manageLocation)
        {
            try
            {
                var location = LocationRepo.FindBy(x => x.LocationName.ToLower() == manageLocation.LocationName.ToLower());
				if (location != null && location.Id != manageLocation.Id)
                    throw new ChannelPartnerException(ChannelPartnerException.LOCATION_ALREADY_EXISTS, string.Format("Location Name {0} already exists.", location.LocationName));

				CheckDuplicateLocation(manageLocation);

                manageLocation.DTServerCreate = DateTime.Now;
                manageLocation.DTServerLastModified = DateTime.Now;
				manageLocation.DTTerminalLastModified = Clock.DateTimeWithTimeZone(manageLocation.TimezoneID);
                LocationRepo.Merge(manageLocation);
                return true;
            }
            catch (ChannelPartnerException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerException(ChannelPartnerException.LOCATION_UPDATE_FAILED, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationName"></param>
        /// <returns></returns>
        public Location GetByName(string locationName)
        {           
            try
            {
                return LocationRepo.FindBy(x => x.LocationName.ToLower() == locationName.ToLower());
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerException(ChannelPartnerException.LOCATION_NOT_FOUND, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Location> GetAll()
        {
            return LocationRepo.All().ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public Location Lookup(long locationId)
        {
            try
            {
                return LocationRepo.FindBy(a => a.Id == locationId);
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerException(ChannelPartnerException.LOCATION_NOT_FOUND, ex);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="locationId"></param>
		/// <returns></returns>
		public Location Lookup(Guid locationId)
		{
			try
			{
				return LocationRepo.FindBy(a => a.rowguid == locationId);
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerException(ChannelPartnerException.LOCATION_NOT_FOUND, ex);
			}
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Added this code to check whether location identifier already exists in database.
		/// If not update the location identifier with the location identifier user entered in location screen.
		/// Starts Here
		/// </summary>
		/// <param name="location"></param>
		private void CheckDuplicateLocation(Location location)
		{
			var locationIdentifier = LocationRepo.FilterBy(a => a.LocationIdentifier == location.LocationIdentifier 
				&& a.Id != location.Id 
				&& a.ChannelPartnerId == location.ChannelPartnerId).FirstOrDefault();

			if (locationIdentifier != null)
				throw new ChannelPartnerException(ChannelPartnerException.DUPLICATE_LOCATIONID, "Duplicate location ID. Verify the Location ID entered");
		}
		//Ends Here

	}
}
