using OPT.TalentRadar.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.BIZ.Contract
{
    public interface IAuthService
    {
        ADError ValidateUser(string userName, string password);

        ADError ValidateUserGroup(string userName);

        ADUser GetADUserDetails(string userName);
    }
}
