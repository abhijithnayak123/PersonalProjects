using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Data;

namespace MGI.Core.Partner.Contract
{
	public interface IManageUsers
	{
		/// <summary>
		/// This method is to add the user details
		/// </summary>
		/// <param name="userInfo">This is user details</param>
		/// <returns>Unique identifier of user details</returns>
		int AddUser(UserDetails userInfo);

		/// <summary>
		/// This method is get the user details by user id
		/// </summary>
		/// <param name="UserId">This is unique identifier of user details</param>
		/// <returns>This is user details</returns>
		UserDetails GetUser(int UserId);

		/// <summary>
		/// This method is to update the user details
		/// </summary>
		/// <param name="userInfo">This is user details</param>
		/// <returns>Unique identifier of user details</returns>
		int UpdateUser(UserDetails userInfo);

		/// <summary>
		/// This method is to get the collection of user details by search criteria
		/// </summary>
		/// <param name="searchCriteria">This is user search criteria</param>
		/// <returns>Collection of user details</returns>
		List<UserDetails> SearchUsers(UserSearchCriteria searchCriteria);

		/// <summary>
		/// This method is to get the collection of user details by location id
		/// </summary>
		/// <param name="locationId">This is location id</param>
		/// <returns>Collection of user details</returns>
		List<UserDetails> GetUsers(long locationId);

		/// <summary>
		/// This method is to get the collection of all user details
		/// </summary>
		/// <returns>Collection of user details</returns>
		List<UserDetails> GetManagers();

		/// <summary>
		/// This method is to get the collection of all user roles
		/// </summary>
		/// <returns>Collection of user roles</returns>
		List<UserRole> GetUserRoles();

		/// <summary>
		/// This method is to get the collection of all user status
		/// </summary>
		/// <returns>Collection of user status</returns>
		List<UserStatus> GetUserStatuses();

		/// <summary>
		/// This method is to save the user location
		/// </summary>
		/// <returns>Unique identifier of user location</returns>
		int SaveUserLocation(UserLocation userLocation);

		/// <summary>
		/// This method is to check whether user is having permission or not
		/// </summary>
		/// <param name="UserId">This is user id</param>
		/// <param name="permission">This is permission</param>
		/// <returns>Status of user permission</returns>
		bool HasPermission(int UserId, string permission);

		#region SendMail
		/// <summary>
		/// This method is to get the user details
		/// </summary>
		/// <param name="UserName">This is user name</param>
		/// <param name="channelPartnerId">This is channel partner id</param>
		/// <returns>This is user details</returns>
		UserDetails GetUser(string UserName, long channelPartnerId);
        
		#endregion
	}
}
