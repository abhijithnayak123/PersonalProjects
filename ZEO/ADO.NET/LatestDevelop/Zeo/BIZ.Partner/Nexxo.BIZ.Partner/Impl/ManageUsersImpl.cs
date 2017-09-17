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

		public UserDetailsDTO GetUser(long agentSessionId, int UserId, MGIContext mgiContext)
		{
			try
			{
				UserDetails userDetails = _manageUserService.GetUser(UserId);

				UserDetailsDTO userDetailsDTO = Mapper.Map<UserDetailsDTO>(userDetails);

				return userDetailsDTO;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizAgentException(BizAgentException.USER_GET_FAILED, ex);
			}
		}

		public List<UserDetailsDTO> GetUsers(long agentSessionId, long locationId, MGIContext mgiContext)
		{
			try
			{
				List<UserDetails> userDetails = _manageUserService.GetUsers(locationId);

				List<UserDetailsDTO> userDetailsDTO = Mapper.Map<List<UserDetailsDTO>>(userDetails);

				return userDetailsDTO;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizAgentException(BizAgentException.USER_GET_FAILED, ex);
			}

		}
	}
}
