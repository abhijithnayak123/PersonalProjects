using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using System.Net.Mail;

namespace MGI.Channel.DMS.Server.Svc
{
    public partial class DesktopWSImpl : IUserService
    {
        public int SaveUser(MGI.Channel.DMS.Server.Data.UserDetails userInfo, MGI.Channel.DMS.Server.Data.SaveMode mode, MGIContext mgiContext)
        {
            return DesktopEngine.SaveUser(userInfo, mode, mgiContext);
        }

        public MGI.Channel.DMS.Server.Data.UserDetails GetUser(long agentSessionId, int UserId, MGIContext mgiContext)
        {
            return DesktopEngine.GetUser(agentSessionId, UserId, mgiContext);
        }

        public List<MGI.Channel.DMS.Server.Data.UserDetails> GetUsers(long agentSessionId, long locationId, MGIContext mgiContext)
		{
			return DesktopEngine.GetUsers(agentSessionId, locationId, mgiContext);
		}
    }
}