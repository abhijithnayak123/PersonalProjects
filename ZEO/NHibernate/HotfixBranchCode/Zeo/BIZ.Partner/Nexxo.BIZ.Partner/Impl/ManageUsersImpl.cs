using System;
using System.Collections.Generic;
using System.Linq;

using MGI.Biz.Partner.Data;

using UserDetailsDTO = MGI.Biz.Partner.Data.UserDetails;
using UserRoleDTO = MGI.Biz.Partner.Data.UserRole;
using UserStatusDTO = MGI.Biz.Partner.Data.UserStatus;
using UserSearchCriteriaDTO = MGI.Biz.Partner.Data.UserSearchCriteria;
using ManageLocationDTO = MGI.Biz.Partner.Data.Location;
using SessionContextDTO = MGI.Biz.Partner.Data.SessionContext;

using MGI.Core.Partner.Data;

using IManageUserService = MGI.Core.Partner.Contract.IManageUsers;
using IManageLocationService = MGI.Core.Partner.Contract.IManageLocations;

using UserDetails = MGI.Core.Partner.Data.UserDetails;
using UserRole = MGI.Core.Partner.Data.UserRole;
using UserStatus = MGI.Core.Partner.Data.UserStatus;
using UserSearchCriteria = MGI.Core.Partner.Data.UserSearchCriteria;
using ManageLocation = MGI.Core.Partner.Data.Location;
//using UserAuthentication = MGI.Core.Partner.Data.UserAuthentication;
using UserLocation = MGI.Core.Partner.Data.UserLocation;

using AutoMapper;
using MGI.Common.Util;
using System.Net.Mail;
using System.Net;
using MGI.Biz.Partner.Contract;

namespace MGI.Biz.Partner.Impl
{
    public class ManageUsersImpl : MGI.Biz.Partner.Contract.IManageUsers
    {
        private IManageUserService _manageUserService;
        public IManageUserService ManageUserService
        {
            set { _manageUserService = value; }
        }

        private IManageLocationService _manageLocationService;
        public IManageLocationService ManageLocationService
        {
            set { _manageLocationService = value; }
        }

        public ManageUsersImpl()
        {
            Mapper.CreateMap<UserDetailsDTO, UserDetails>();
            Mapper.CreateMap<UserDetails, UserDetailsDTO>();
            Mapper.CreateMap<UserRole, UserRoleDTO>();
            Mapper.CreateMap<UserStatus, UserStatusDTO>();
            Mapper.CreateMap<UserSearchCriteriaDTO, UserSearchCriteria>();
            Mapper.CreateMap<ManageLocation, ManageLocationDTO>();
            Mapper.CreateMap<ChannelPartnerSMTPDTO, ChannelPartnerSMTPDetails>();
            Mapper.CreateMap<ChannelPartnerSMTPDetails, ChannelPartnerSMTPDTO>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
		public int SaveUser(UserDetailsDTO userInfo, SaveMode mode, MGIContext mgiContext)
        {
            UserDetails userDetails = Mapper.Map<UserDetailsDTO, UserDetails>(userInfo);
            int UserId = (int)userInfo.Id;

            try
            {
                if (mode == SaveMode.Add)
                {
                    if (ValidateUser(userDetails.UserName, userDetails.ChannelPartnerId))
                    {
                        UserId = _manageUserService.AddUser(userDetails);
                        if (UserId > 0)
                        {
                            UserLocation userLocation = new UserLocation();
                            userLocation.AgentId = UserId;
                            userLocation.LocationId = userDetails.LocationId;
                            _manageUserService.SaveUserLocation(userLocation);
                        }
                    }
                    else
                    {
                        throw new BizUserException(BizUserException.USER_USERNAME_ALREADY_EXISTS,"User Name Already Exists");
                    }
                }
                else
                {

					_manageUserService.UpdateUser(userDetails);
					
                    UserLocation userLocation = new UserLocation();
                    userLocation.AgentId = (int)userDetails.Id;
                    userLocation.LocationId = userDetails.LocationId;
                   _manageUserService.SaveUserLocation(userLocation);
                }
            }
            catch (BizUserException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new BizUserException(BizUserException.USER_CREATE_FAILED, ex);
            }

            return UserId;
        }

		public UserDetailsDTO GetUser(long agentSessionId, int UserId, MGIContext mgiContext)
        {
            UserDetails userDetails = _manageUserService.GetUser(UserId);

            UserDetailsDTO userDetailsDTO = Mapper.Map<UserDetailsDTO>(userDetails);

            return userDetailsDTO;
        }

		public List<UserDetailsDTO> GetUsers(long agentSessionId, long locationId, MGIContext mgiContext)
        {
            List<UserDetails> userDetails = _manageUserService.GetUsers(locationId);

            List<UserDetailsDTO> userDetailsDTO = Mapper.Map<List<UserDetailsDTO>>(userDetails);

            return userDetailsDTO;
        }

        private bool ValidateUser(string userName, long channelPartnerId)
        {
            UserDetails userDetailsList = _manageUserService.GetUser(userName, channelPartnerId);
            if (userDetailsList != null)
            {
                return false;
            }
            return true;
        }
    }
}
