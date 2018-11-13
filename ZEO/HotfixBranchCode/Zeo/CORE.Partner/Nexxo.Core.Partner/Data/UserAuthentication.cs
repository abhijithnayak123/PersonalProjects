using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class UserAuthentication
    {
        public UserAuthentication(UserAuthentication userAuthentication)
        {
            AgentId = userAuthentication.AgentId;
            UserName = userAuthentication.UserName;
            AuthenticationFailures = userAuthentication.AuthenticationFailures;
            passwordHash = userAuthentication.passwordHash;
            Salt = userAuthentication.Salt;
            TemporaryPassword = userAuthentication.TemporaryPassword;
            LastPasswordUpdateBy = userAuthentication.LastPasswordUpdateBy;
            DTLastPasswordUpdate = userAuthentication.DTLastPasswordUpdate;
        }

        public UserAuthentication()
        {

        }
		public virtual Guid UserAuthenticationPK { get; set; }
        public virtual int AgentId { get; set; }
        public virtual string UserName { get; set; }
        public virtual int AuthenticationFailures { get; set; }
        public virtual string passwordHash { get; set; }
        public virtual string Salt { get; set; }
        public virtual bool TemporaryPassword { get; set; }
        public virtual string LastPasswordUpdateBy { get; set; }
        public virtual DateTime DTLastPasswordUpdate { get; set; }
    }
}
