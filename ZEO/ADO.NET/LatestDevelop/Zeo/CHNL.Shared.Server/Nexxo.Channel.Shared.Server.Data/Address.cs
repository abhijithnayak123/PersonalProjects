using System.Runtime.Serialization;
using System.Text;
using MGI.Common.Util;

namespace MGI.Channel.Shared.Server.Data
{
    public class Address
    {
        public Address() { }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string PostalCode { get; set; }

       
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("	Address1: {0}", NexxoUtil.safeSQLString(Address1)));
            sb.AppendLine(string.Format("	Address2: {0}", NexxoUtil.safeSQLString(Address2)));
            sb.AppendLine(string.Format("	City: {0}", NexxoUtil.safeSQLString(City)));
            sb.AppendLine(string.Format("	State: {0}", NexxoUtil.safeSQLString(State)));
            sb.AppendLine(string.Format("	PostalCode: {0}", NexxoUtil.safeSQLString(PostalCode)));
            return sb.ToString();
        }
    }
}
