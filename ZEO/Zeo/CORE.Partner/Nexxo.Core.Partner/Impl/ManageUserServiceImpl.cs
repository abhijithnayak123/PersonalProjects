using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;

using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;

namespace MGI.Core.Partner.Impl
{
	public class ManageUserServiceImpl : IManageUsers
	{

		private IRepository<UserDetails> _userManagementRepo;
		public IRepository<UserDetails> UserManagementRepo
		{
			set { _userManagementRepo = value; }
		}

		private IReadOnlyRepository<UserStatus> _userStatusRepo;
		public IReadOnlyRepository<UserStatus> UserStatusRepo
		{
			set { _userStatusRepo = value; }
		}

		private IReadOnlyRepository<UserRole> _userRoleRepo;
		public IReadOnlyRepository<UserRole> UserRoleRepo
		{
			set { _userRoleRepo = value; }
		}

		private IReadOnlyRepository<UserDetails> _searchUserRepo;
		public IReadOnlyRepository<UserDetails> SearchUserRepo
		{
			set { _searchUserRepo = value; }
		}

		private IRepository<UserLocation> _userLocationRepo;
		public IRepository<UserLocation> UserLocationRepo
		{
			set { _userLocationRepo = value; }
		}

		private IRepository<ChannelPartnerSMTPDetails> _userSMTPRepo;
		public IRepository<ChannelPartnerSMTPDetails> UserSMTPRepo
		{
			set { _userSMTPRepo = value; }
		}

		private IRepository<Location> _LocationRepo;
		public IRepository<Location> LocationRepo
		{
			set { _LocationRepo = value; }
		}

		public int AddUser(UserDetails userInfo)
		{
			try
			{
				DateTime modDate = DateTime.Now;
				userInfo.DTServerCreate = modDate;
				userInfo.DTServerLastModified = modDate;
				userInfo.DtLastlogin = null;

				_userManagementRepo.AddWithFlush(userInfo);
				return (int)userInfo.Id;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new PartnerAgentException(PartnerAgentException.USER_CREATE_FAILED, ex);
			}
		}

		public UserDetails GetUser(int UserId)
		{
			try
			{
				UserDetails user = _userManagementRepo.FindBy(m => m.Id == UserId);

				if (user == null)
					throw new PartnerAgentException(PartnerAgentException.USER_NOT_FOUND, null);

				return user;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new PartnerAgentException(PartnerAgentException.USER_GET_FAILED, ex);
			}
		}

		public int UpdateUser(UserDetails userInfo)
		{
			try
			{
				DateTime modDate = DateTime.Now;
				userInfo.DTServerLastModified = modDate;
				userInfo.DtLastlogin = null;

				_userManagementRepo.SaveOrUpdate(userInfo);
				return (int)userInfo.Id;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new PartnerAgentException(PartnerAgentException.USER_UPDATE_FAILED, ex);
			}
		}

		public List<UserDetails> GetUsers(long locationId)
		{
			try
			{
				List<UserDetails> UserList = new List<UserDetails>();

				UserList = _userManagementRepo.FilterBy(c => c.LocationId == locationId).Distinct().ToList();

				return UserList;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new PartnerAgentException(PartnerAgentException.USER_GET_FAILED, ex);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the user has permission for an activity
		/// </summary>
		/// <param name="userId">Id of an user</param>
		/// <param name="permission">Permission for an activity</param>
		/// <returns><c>true</c> if the user has access; otherwise, <c>false</c>.</returns>
		public bool HasPermission(int userId, string permission)
		{
			try
			{
				// Get user details from the database by passing Id of the user
				UserDetails user = GetUser(userId);

				int userRoleId = user.UserRoleId;
				long channelPartnerId = user.ChannelPartnerId;

				// Get role of the user
				UserRole userRole = _userRoleRepo.FindBy(u => u.Id == userRoleId);
				// Filter the role
				return userRole.RolesPermissions
					   .Where(x => x.ChannelPartner.Id == channelPartnerId
							 && x.IsEnabled == true
							 && String.Equals(x.Permission.PermissionName, permission, StringComparison.CurrentCultureIgnoreCase))
					   .Any();
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new PartnerAgentException(PartnerAgentException.USER_AUTHENTICATION_FAILED, ex);
			}

		}

		#region SendMail

		public UserDetails GetUser(string UserName, long channelPartnerId)
		{
			try
			{
				IQueryable<UserDetails> userList = _userManagementRepo.All();
				UserDetails userDetails = userList.Where(q => q.UserName.ToLower() == UserName.ToLower() && q.ChannelPartnerId == channelPartnerId).FirstOrDefault();

				if (userDetails == null)
					throw new PartnerAgentException(PartnerAgentException.USER_NOT_FOUND, null);

				return userDetails;
			}
			catch (Exception ex)
			{
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new PartnerAgentException(PartnerAgentException.USER_GET_FAILED, ex);
			}
		}
		#endregion



	}
}
