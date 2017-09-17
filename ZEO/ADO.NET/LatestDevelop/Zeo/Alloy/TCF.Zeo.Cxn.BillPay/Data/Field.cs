using System.Collections.Generic;

namespace TCF.Zeo.Cxn.BillPay.Data
{
    public class Field
    {
        public string Label { get; set; }
        public string DataType { get; set; }
        public int MaxLength { get; set; }
        public string ValidationMessage { get; set; }
        public string Mask { get; set; }
        public bool IsMandatory { get; set; }
        public string TagName { get; set; }
        public Dictionary<string, object> Values { get; set; }
    }
}
