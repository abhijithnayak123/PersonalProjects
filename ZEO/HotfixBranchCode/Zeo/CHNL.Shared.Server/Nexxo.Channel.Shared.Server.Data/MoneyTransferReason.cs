﻿using System.Runtime.Serialization;

namespace MGI.Channel.Shared.Server.Data
{
    [DataContract]
    public class MoneyTransferReason
    {	
       
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Name { get; set; }

		override public string ToString()
		{
			string str = string.Empty;			
			str = string.Concat(str, "Code = ", Code, "\r\n");
			str = string.Concat(str, "Name = ", Name, "\r\n");
			return str;
		}
	}
}
