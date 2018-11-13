using System;

namespace MGI.Biz.Partner.Data
{
    public class UserAuthentication
    {
        public int AgentId { get; set; }
        public string UserName { get; set; }
        public int AuthenticationFailures { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public bool TemporaryPassword { get; set; }
        public string LastPasswordUpdateBy { get; set; }
        public DateTime DTLastPasswordUpdate { get; set; }
    }
}
