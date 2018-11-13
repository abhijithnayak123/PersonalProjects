using System;
using System.Runtime.Serialization;

namespace MGI.Peripheral.Server.Data
{
    public class ScanCheckRequest
    {
        [DataMember]
        public string ScanFileType { get; set; }

        [DataMember]
        public string SaveRequired { get; set; }

        [DataMember]
		public Guid UniqueId { get; set; }
    }
}