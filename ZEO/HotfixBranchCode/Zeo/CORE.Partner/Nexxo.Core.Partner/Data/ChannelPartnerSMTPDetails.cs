using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data
{
    public class ChannelPartnerSMTPDetails : NexxoModel
    {
		public virtual Guid  ChannelPartnerSMTPPK { get; set; }

        public virtual long ChannelPartnerId { get; set; }
               
        public virtual string SmtpHost { get; set; }
               
        public virtual int SmtpPort { get; set; }

        public virtual string SenderEmail { get; set; }

        public virtual string SenderPassword { get; set; }

        public virtual string Subject { get; set; }

        public virtual string Body { get; set; }
    }
}
