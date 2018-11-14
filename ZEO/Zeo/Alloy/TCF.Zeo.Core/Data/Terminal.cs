using TCF.Zeo.Common.Data;
using System;

namespace TCF.Zeo.Core.Data
{
    public class Terminal : ZeoModel
    {
        public virtual long TerminalId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string IpAddress { get; set; }
        public virtual string MacAddress { get; set; }
        public virtual long LocationId { get; set; }
        public virtual string PeripheralServerId { get; set; }
        public virtual long ChannelPartnerId { get; set; }
        public virtual string PeripheralServerUrl { get; set; }
        public virtual string LocationName { get; set; }
        public virtual string PeripheralServeName { get; set; }
        public virtual bool IsLocationActive { get; set; }
    }
}

