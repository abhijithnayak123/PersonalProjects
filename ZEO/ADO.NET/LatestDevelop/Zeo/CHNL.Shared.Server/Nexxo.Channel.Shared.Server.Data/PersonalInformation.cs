using System;
using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
    public class PersonalInformation
    {
        public PersonalInformation() { }
        [DataMember]
        public string FName { get; set; }
        [DataMember]
        public string MName { get; set; }
        [DataMember]
        public string LName { get; set; }
        [DataMember]
        public string LName2 { get; set; }
        [DataMember]
        public string MothersMaidenName { get; set; }
        [DataMember]
		public DateTime DateOfBirth { get; set; }
        [DataMember]
        public string Gender { get; set; }

        override public string ToString()
        {
            string str = string.Empty;
            str = string.Concat(str, "FName = ", FName, "\r\n");
            str = string.Concat(str, "MName = ", MName, "\r\n");
            str = string.Concat(str, "LName = ", LName, "\r\n");
            str = string.Concat(str, "LName2 = ", LName2, "\r\n");
            str = string.Concat(str, "MothersMaidenName = ", MothersMaidenName, "\r\n");
            str = string.Concat(str, "DOB = ", DateOfBirth, "\r\n");
            str = string.Concat(str, "Gender = ", Gender, "\r\n");
            return str;
        }

    }
}
