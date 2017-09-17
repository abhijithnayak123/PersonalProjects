using System;

namespace MGI.Biz.Partner.Data
{
    public class ChannelPartnerSMTPDTO
    {
        public long Id { get; set; }
        
        public int ChannelPartnerId { get; set; }
               
        public string SmtpHost { get; set; }
               
        public int SmtpPort { get; set; }

        public string SenderEmail { get; set; }

        public string SenderPassword { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        //public DateTime DTCreate { get; set; }

        //public Nullable<DateTime> DTLastMod { get; set; }
    }
}
