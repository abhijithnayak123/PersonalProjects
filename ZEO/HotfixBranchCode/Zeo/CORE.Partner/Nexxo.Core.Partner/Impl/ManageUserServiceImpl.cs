using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Sql;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;

using MGI.Common.DataAccess.Contract;

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

		private IRepository<UserAuthentication> _userAuthenticationRepo;
		public IRepository<UserAuthentication> UserAuthenticationRepo
		{
			set { _userAuthenticationRepo = value; }
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
                throw new ChannelPartnerException(ChannelPartnerException.USER_CREATE_FAILED, ex);
            }
        }

		public UserDetails GetUser(int UserId)
		{
			UserDetails user = _userManagementRepo.FindBy(m => m.Id == UserId);
			
			if (user == null)
				throw new ChannelPartnerException(ChannelPartnerException.USER_NOT_FOUND, string.Format("Could not find User {0}", UserId));

			return user;
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
                throw new ChannelPartnerException(ChannelPartnerException.USER_UPDATE_FAILED, ex);
            }
        }

        public List<UserStatus> GetUserStatuses()
        {
            return _userStatusRepo.All().AsQueryable().ToList();
        }

        public List<UserRole> GetUserRoles()
        {
            return _userRoleRepo.All().AsQueryable().ToList();
        }

        public List<UserDetails> SearchUsers(UserSearchCriteria searchCriteria)
        {
            var UserList = (from x in _userManagementRepo.FilterBy(u=> u.ChannelPartnerId == searchCriteria.ChannelPartnerId)
                            join a in _userStatusRepo.All() on x.UserStatusId equals a.Id
                            join b in _LocationRepo.All() on x.LocationId equals b.Id
                            select new UserDetails()
                            {
                                ChannelPartnerId = x.ChannelPartnerId,
                                DTServerCreate = x.DTServerCreate,
                                DtLastlogin = x.DtLastlogin,
                                DTServerLastModified = x.DTServerLastModified,
                                Email = x.Email,
                                FirstName = x.FirstName,
                                FullName = x.FullName,
                                Id = x.Id,
                                IsEnabled = x.IsEnabled,
                                LastName = x.LastName,
                                LocationId = x.LocationId,
                                LocationName = b.LocationName,
                                ManagerId = x.ManagerId,
                                Notes = x.Notes,
                                PhoneNumber = x.PhoneNumber,
                                Rowguid = x.Rowguid,
                                UserName = x.UserName,
                                UserRoleId = x.UserRoleId,
                                UserStatusId = x.UserStatusId,
                                UserStatus = a.Status
                            });           

            if (!string.IsNullOrEmpty(searchCriteria.FirstName) || !string.IsNullOrEmpty(searchCriteria.LastName) || !string.IsNullOrEmpty(searchCriteria.UserName) || !string.IsNullOrEmpty(searchCriteria.LocationName))                
            {
                IQueryable<UserDetails> users =
                    UserList.Where(c => (string.IsNullOrEmpty(searchCriteria.FirstName) || c.FirstName == searchCriteria.FirstName)
                        && (string.IsNullOrEmpty(searchCriteria.LastName) || c.LastName == searchCriteria.LastName)
                        && (string.IsNullOrEmpty(searchCriteria.UserName) || c.UserName == searchCriteria.UserName)
                        && (string.IsNullOrEmpty(searchCriteria.LocationName) || c.LocationName == searchCriteria.LocationName)
						);

                return users.ToList<UserDetails>();
            }
            else
                return new List<UserDetails>();


            //List<UserDetails> users = new List<UserDetails>();
            //if ((searchCriteria.FirstName != "") & (searchCriteria.LastName != "") & (searchCriteria.LocationName != ""))
            //{
            //    users.AddRange(
            //        UserList.Where(a => a.FirstName.ToLower() == searchCriteria.FirstName.ToLower() && a.LastName.ToLower() == searchCriteria.LastName.ToLower()
            //         && a.LocationName.ToLower() == searchCriteria.LocationName.ToLower())
            //        );
            //}
            //else if ((searchCriteria.FirstName != "") & (searchCriteria.LastName != ""))
            //{
            //    users.AddRange(
            //        UserList.Where(a => a.FirstName.ToLower() == searchCriteria.FirstName.ToLower() && a.LastName.ToLower() == searchCriteria.LastName.ToLower())
            //        );
            //}
            //else if ((searchCriteria.LastName != "") & (searchCriteria.LocationName != ""))
            //{
            //    users.AddRange(
            //        UserList.Where(a => a.LastName.ToLower() == searchCriteria.LastName.ToLower() && a.LocationName.ToLower() == searchCriteria.LocationName.ToLower())
            //        );
            //}
            //else if ((searchCriteria.FirstName != "") & (searchCriteria.LocationName != ""))
            //{
            //    users.AddRange(
            //        UserList.Where(a => a.FirstName.ToLower() == searchCriteria.FirstName.ToLower() && a.LocationName.ToLower() == searchCriteria.LocationName.ToLower())
            //        );
            //}
            //else if ((searchCriteria.FirstName != ""))
            //{
            //    users.AddRange( UserList.Where(a => a.FirstName.ToLower() == searchCriteria.FirstName.ToLower()));
            //}
            //else if ((searchCriteria.LocationName != ""))
            //{
            //    users.AddRange(UserList.Where(a => a.LocationName.ToLower() == searchCriteria.LocationName.ToLower()));
            //}
            //else if ((searchCriteria.LastName != ""))
            //{
            //    users.AddRange(UserList.Where(a => a.LastName.ToLower() == searchCriteria.LastName.ToLower()));
            //}
            //return users;
        }

        public List<UserDetails> GetManagers()
        {
            List<UserDetails> UserList = new List<UserDetails>();

            UserList = _userManagementRepo.All().Distinct().ToList();

            return UserList;
        }

		public List<UserDetails> GetUsers(long locationId)
		{
			List<UserDetails> UserList = new List<UserDetails>();

			//UserList = _userManagementRepo.FilterBy(c => c.LocationId == locationId && c.UserRoleId == 1).ToList();
           
               UserList = _userManagementRepo.FilterBy(c => c.LocationId == locationId).Distinct().ToList();
           

			return UserList;
		}
		/// <summary>
		/// Gets a value indicating whether the user has permission for an activity
		/// </summary>
		/// <param name="userId">Id of an user</param>
		/// <param name="permission">Permission for an activity</param>
		/// <returns><c>true</c> if the user has access; otherwise, <c>false</c>.</returns>
		public bool HasPermission(int userId, string permission)
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

        public int SaveUserLocation(UserLocation userLocation)
        {
            try
            {
                UserLocation userLoc = _userLocationRepo.FindBy(x => x.AgentId == userLocation.AgentId && x.LocationId == userLocation.LocationId);

                if (userLoc == null)
                    _userLocationRepo.AddWithFlush(userLocation);

                return userLocation.AgentId;
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerException(ChannelPartnerException.USER_LOCATION_MAPPING_CREATE_FAILED, ex);
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
					throw new ChannelPartnerException(ChannelPartnerException.USER_NOT_FOUND, "User not found");

				return userDetails;
            }
            catch (Exception ex)
            {
                throw new ChannelPartnerException(ChannelPartnerException.USER_NOT_FOUND, ex);
            }
        }
       #endregion



    }
}
