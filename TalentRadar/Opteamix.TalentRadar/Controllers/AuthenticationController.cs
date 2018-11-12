using OPT.TalentRadar.BIZ.Contract;
using OPT.TalentRadar.BIZ.Implementation;
using OPT.TalentRadar.Common.Data;
using Opteamix.TalentRadar.Models;
using System.Web;
using System.Web.Http;

namespace Opteamix.TalentRadar.Controllers
{
    [RoutePrefix("api/Authentication")]
    public class AuthenticationController : ApiController
    {
        [Route("UserInfo")]
        public ADUser GetUserInfo()
        {
            string userName = HttpContext.Current.Request.LogonUserIdentity.Name;

            if (userName.Contains("\\"))
            {
                userName = userName.Split('\\')[1];
            }

            IAuthService auth = new AuthServiceImpl();
            ADUser ret = auth.GetADUserDetails(userName);

            return ret;
        }

        [Route("ValidateUser")]
        [HttpGet]
        public bool ValidateUser()
        {
            bool isUserValid = false;
            string userName = HttpContext.Current.Request.LogonUserIdentity.Name;

            if (userName.Contains("\\"))
            {
                userName = userName.Split('\\')[1];
            }
            IAuthService auth = new AuthServiceImpl();

            ADError result = auth.ValidateUserGroup(userName);

            if (result == ADError.AD_Success)
            {
                isUserValid = true;
            }
            return isUserValid;
        }

        [HttpPost]
        [Route("Authenticate")]
        public string AuthenticateUser(UserInfo user)
        {
            IAuthService auth = new AuthServiceImpl();
            ADError err = auth.ValidateUser(user.UserName, user.Password);

            if (err == ADError.AD_Success)
            {
                ADError result = auth.ValidateUserGroup(user.UserName);
                return GetErrorMessage(result);
            }
            else
            {
                return GetErrorMessage(err);
            }
        }

        private string GetErrorMessage(ADError err)
        {
            string errorMessage = "Success";
            switch (err)
            {
                case ADError.AD_Success:
                    errorMessage = "Success";
                    break;
                case ADError.AD_InvalidParams:
                case ADError.AD_InvalidCredentials:
                    errorMessage = "Please Enter a Valid UserName / Password";
                    break;
                case ADError.AD_NotOperational:
                case ADError.AD_InvalidDomainPath:
                    errorMessage = "Please use valid User details to login.";
                    break;
                case ADError.AD_InvalidMemberInGroup:
                    errorMessage = "Access denied. User does not have the right level of permissions to perform this action.";
                    break;
            }

            return errorMessage;
        }

    }
}
