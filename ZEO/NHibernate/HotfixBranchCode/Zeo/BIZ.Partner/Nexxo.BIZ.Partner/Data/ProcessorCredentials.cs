using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.Partner.Data
{
    public class ProcessorCredentials
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Identifier { get; set; }
        public long ProviderId { get; set; }
    }
}
