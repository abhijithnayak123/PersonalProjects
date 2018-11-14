using System;
using System.Xml.Linq;

namespace TCF.Zeo.Cxn.Check.Chexar.Data
{
    public class ChexarTransactionIO
    {
        public int InvoiceNo = int.MinValue;
        public int Status = int.MinValue;
        public DateTime DTAdded = DateTime.MinValue;
        public bool Complete = false;
        public int BranchId = int.MinValue;
        public int EmployeeId = int.MinValue;

        public ChexarTransactionIO(XElement x)
        {
            InvoiceNo = ChexarXMLHelper.GetIntToken(x, "invoiceno");
            Status = ChexarXMLHelper.GetIntToken(x, "status");
            BranchId = ChexarXMLHelper.GetIntToken(x, "branchid");
            EmployeeId = ChexarXMLHelper.GetIntToken(x, "employeeid");
            Complete = ChexarXMLHelper.GetBoolToken(x, "printed");
            DTAdded = ChexarXMLHelper.GetDateTimeToken(x, "adddate");
        }
    }
}
