using OPT.TalentRadar.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.Authentication.Contract
{
    public interface IAuthenticationService
    {
        ADError ValidateUser(string userName, string password);

        ADError ValidatepGroupUser(string userName);

        List<ADUser> GetADUserDetails(string userName);
    }
}
