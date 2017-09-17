using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Channel.Shared.Server.Data
{
    public class Account
    {       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string LoyaltyCardNumber { get; set; }
        public string LevelCode { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public string MobilePhone { get; set; }
        public string SmsNotificationFlag { get; set; }

        override public string ToString()
        {
            string str = string.Empty;

			str = string.Concat(str, "FirstName = ", FirstName, "\r\n");
			str = string.Concat(str, "LastName = ", LastName, "\r\n");
			str = string.Concat(str, "Address = ", Address, "\r\n");
			str = string.Concat(str, "City = ", City, "\r\n");
			str = string.Concat(str, "State = ", State, "\r\n");
			str = string.Concat(str, "PostalCode = ", PostalCode, "\r\n");
			str = string.Concat(str, "LoyaltyCardNumber = ", LoyaltyCardNumber, "\r\n");
			str = string.Concat(str, "LevelCode = ", LevelCode, "\r\n");
			str = string.Concat(str, "Email = ", Email, "\r\n");
			str = string.Concat(str, "ContactPhone = ", ContactPhone, "\r\n");
			str = string.Concat(str, "MobilePhone = ", MobilePhone, "\r\n");
			str = string.Concat(str, "Email = ", Email, "\r\n");
			str = string.Concat(str, "SmsNotificationFlag = ", SmsNotificationFlag, "\r\n");

            return str;
        }
    }
}
