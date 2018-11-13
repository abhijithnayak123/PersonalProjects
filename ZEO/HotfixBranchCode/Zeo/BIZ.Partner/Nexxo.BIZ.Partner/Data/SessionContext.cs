using System;

namespace MGI.Biz.Partner.Data
{
    public class SessionContext
    {
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public int LocationAgentId { get; set; }
        public Guid LocationId { get; set; }
        public int ChannelPartnerId { get; set; }
        public string AppName { get; set; }
        public string CustomerSessionId { get; set; }
        public DateTime DTKiosk { get; set; }
        public int SelectedLanguage { get; set; }
    }
}
