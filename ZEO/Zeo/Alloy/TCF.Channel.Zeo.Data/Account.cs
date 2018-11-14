using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Channel.Zeo.Data
{
    [DataContract]
    public class Account
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string SecondLastName { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string ContactPhone { get; set; }
        [DataMember]
        public string MobilePhone { get; set; }
        [DataMember]
        public string LoyaltyCardNumber { get; set; }
        [DataMember]
        public string LevelCode { get; set; }
        [DataMember]
        public string SmsNotificationFlag { get; set; }
    }
}
