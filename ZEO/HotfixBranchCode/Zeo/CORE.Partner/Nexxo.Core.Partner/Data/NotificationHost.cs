using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
    public class NotificationHost
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string MailGateway { get; set; }
        public virtual System.Nullable<int> MaxMessageLen { get; set; }
        public virtual System.Nullable<System.DateTime> DTServerCreate { get; set; }
        public virtual System.Nullable<System.DateTime> DTServerLastModified { get; set; }
    }
}
