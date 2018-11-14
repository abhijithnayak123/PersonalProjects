using System.Runtime.Serialization;

namespace TCF.Zeo.Peripheral.Server.Data
{
    [DataContract]
    public class PendingCheck
    {
        public long ChannelPartnerId { get; set; }
        public string TokenNo { get; set; }
        public int TicketNo { get; set; }
        public int EmployeeId { get; set; }
    }
}
