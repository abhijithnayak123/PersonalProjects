using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Util;

namespace TCF.Zeo.Core.Data
{
  public class UserDetails: ZeoModel
    {
        /// <summary>
        /// Agent User Name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Agent First Name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Agent Last Name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Agent Full Name
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// AgentID which is unique
        /// </summary>
        public long AgentID { get; set; }

        /// <summary>
        /// AgentID Role Id
        /// </summary>
        public int UserRoleId { get; set; }

        public string TerminalName { get; set; }
        public long TerminalId { get; set; }

        public long LocationId { get; set; }

        public int AuthStatus { get; set; }
    }
}
