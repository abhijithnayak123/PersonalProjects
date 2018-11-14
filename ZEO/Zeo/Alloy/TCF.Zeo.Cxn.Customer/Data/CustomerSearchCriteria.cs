using System;
using System.Text;

#region Zeo References
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.Common;
#endregion
namespace TCF.Zeo.Cxn.Customer.Data
{
    public class CustomerSearchCriteria : BaseRequest
    {
        public string Lastname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Cardnumber { get; set; }
        public string SSN { get; set; }
        public string Accountnumber { get; set; }
        public string ClientCustId { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Customer Search Request:");
            sb.AppendLine(string.Format("	LastName: {0}", Lastname));
            sb.AppendLine(string.Format("	DateOfBirth: {0}", DateOfBirth.HasValue ? DateOfBirth.Value.ToShortDateString() : string.Empty));
            sb.AppendLine(string.Format("	CardNumber: {0}", Cardnumber.MaskLeft(4)));
            sb.AppendLine(string.Format("SSN After Masking : {0}", SSN.MaskLeft(4)));
            sb.AppendLine(string.Format("	Accountnumber: {0}", Accountnumber.MaskLeft(4)));
            return sb.ToString();
        }
    }
}