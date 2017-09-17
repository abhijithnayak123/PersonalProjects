using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class BillPayLocation
    {
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public List<BillerLocation> BillerLocation { set; get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("BillPay Location:");
            sb.AppendLine("	TransactionId:" + TransactionId);
            if (BillerLocation != null)
            {
                foreach (BillerLocation Location in BillerLocation)
                {
                    sb.AppendLine("Id :" + Location.Id);
                    sb.AppendLine("Name :" + Location.Name);
                    sb.AppendLine("Type :" + Location.Type);
                }
            }
            return sb.ToString();
        }
    }
}
