using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OPT.TalentRadar.Common.Data
{
    public class ADUser
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ADName { get; set; }

        public string ADStatus { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string FullName { get; set; }

        public string Manager { get; set; }

        public string Mobile { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }
    }
}
