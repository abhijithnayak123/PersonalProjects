using System.Runtime.Serialization;
using System.Text;
using System.Collections.Generic;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class ModifySendMoneyResponse
    {
      
		[DataMember]
		public long CancelTransactionId { get; set; }
		[DataMember]
		public long ModifyTransactionId { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
           
            return sb.ToString();
        }
    }
}
