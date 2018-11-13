﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.Data
{
    public class SearchResponse
    {
        public long TransactionId { get; set; }
        public string ConfirmationNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string TestQuestion { get; set; }
        public string TestAnswer { get; set; }
        public string TestQuestionAvailable { get; set; }
		public long CancelTransactionId { get; set; }
		public long RefundTransactionId { get; set; }
		public string RefundStatus { get; set; }
		public string PayStatus { get; set; }
        public string MiddleName { get; set; }
        public string TransactionStatus { get; set; }
        public string FeeRefund { get; set; }

    }
}
