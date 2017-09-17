using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    public class ReceiveMoneyRequest
    {
		[DataMember]
		public string ConfirmationNumber { get; set; }
    }
}
