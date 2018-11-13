using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Biz.Partner.Data;
using UserDetailsDTO = MGI.Biz.Partner.Data.UserDetails;
using System.Net.Mail;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Contract
{
    public interface IManageUsers
    {
        /// <summary>
        /// To Save user detail in PNTR Database.
        /// </summary>
        /// <param name="userInfo">A transient instance of UserDetails[Class] </param>
        /// <param name="mode">A transient instance of SaveMode[Class]</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>Unique Identifier For User</returns>
		int SaveUser(UserDetails userInfo, SaveMode mode, MGIContext mgiContext);

        /// <summary>
        /// This method is fetch the user details by user id
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="UserId">This is the unique identifier for user</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>User Details</returns>
		UserDetails GetUser(long agentSessionId, int UserId, MGIContext mgiContext);
        
        /// <summary>
        /// To fetch the collection user based on location id.
        /// </summary>
        /// <param name="agentSessionId">This is unique identifier for Agent Session</param>
        /// <param name="locationId">this is unique identifier location</param>
		/// <param name="mgiContext">This is the common class parameter used to pass supplimental information</param>
        /// <returns>List Of User Details</returns>
		List<UserDetails> GetUsers(long agentSessionId, long locationId, MGIContext mgiContext);
    }
}
