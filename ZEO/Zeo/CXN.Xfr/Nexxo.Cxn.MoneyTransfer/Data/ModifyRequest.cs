using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.MoneyTransfer.Data
{
    public class ModifyRequest
    {
        public long TransactionId { get; set; }
        public string ConfirmationNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string TestQuestion { get; set; }
        public string TestAnswer { get; set; }
        public string TestQuestionAvailable { get; set; }
        public string MiddleName { get; set; }
    }
}
