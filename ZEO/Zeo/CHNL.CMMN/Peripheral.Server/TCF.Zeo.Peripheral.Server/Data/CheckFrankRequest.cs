using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace TCF.Zeo.Peripheral.Server.Data
{
    public class CheckFrankRequest
    {
        [DataMember]
        public String FrankData { get; set; }

        [DataMember]
        public String Orientation { get; set; }

        [DataMember]
        public String FontFace { get; set; }

        [DataMember]
        public String FontType { get; set; }

        [DataMember]
        public int FontSize { get; set; }

        [DataMember]
        public int XPos { get; set; }

        [DataMember]
        public int YPos { get; set; }

        [DataMember]
        public int Width { get; set; }

        [DataMember]
        public int Height { get; set; }

        [DataMember]
        public Guid UniqueId { get; set; }
    }
}
