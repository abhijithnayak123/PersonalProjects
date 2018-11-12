using OPT.TalentRadar.BIZ.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPT.TalentRadar.Common.Data;
using OPT.TalentRadar.Authentication.Contract;
using OPT.TalentRadar.Authentication.Implementation;

namespace OPT.TalentRadar.BIZ.Implementation
{
    public class AuthServiceImpl : IAuthService
    {
        IAuthenticationService authService;

        public AuthServiceImpl()
        {
            authService = new AuthenticationServiceImpl();
        }

        public ADUser GetADUserDetails(string userName)
        {
            List<ADUser> users = authService.GetADUserDetails(userName);
            return users.FirstOrDefault();
        }

        public ADError ValidateUser(string userName, string password)
        {
            return authService.ValidateUser(userName, password);
        }

        public ADError ValidateUserGroup(string userName)
        {
            return authService.ValidatepGroupUser(userName);
        }
    }
}
