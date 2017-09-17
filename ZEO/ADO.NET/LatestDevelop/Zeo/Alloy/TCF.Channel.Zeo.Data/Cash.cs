using System.Runtime.Serialization;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class Cash
    {
        public Cash() { }

        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public int CashType { get; set; }
        [DataMember]
        public string Status { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "Id = ", Id, "\r\n");
            str = string.Concat(str, "Amount = ", Amount, "\r\n");
            str = string.Concat(str, "CashType = ", CashType, "\r\n");
            str = string.Concat(str, "Status = ", Status, "\r\n");
            return str;
        }
    }
}
