using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Biz.MoneyTransfer.Data
{
	public class ModifyRequest
    {
        public long TransactionId { get; set; }
        public string ConfirmationNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string TestQuestion { get; set; }
        public string TestAnswer { get; set; }
        public string TestQuestionAvailable { get; set; }

		public long ModifyTransactionId { get; set; }
		public long CancelTransactionId { get; set; }

		//Only for Biz Layer - to Write in CXE and PTNR
		public decimal Amount { get; set; }
		public decimal Fee { get; set; }
		public string TransactionSubType { get; set; }
		public long OriginalTransactionId { get; set; }
	}
}
