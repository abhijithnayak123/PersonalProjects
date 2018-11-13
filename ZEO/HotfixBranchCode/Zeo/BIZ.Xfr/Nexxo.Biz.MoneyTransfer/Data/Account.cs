﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
    public class Account
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string SecondLastName { get; set; }
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
    }
}
