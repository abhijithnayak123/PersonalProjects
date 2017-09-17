using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.WU.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TCF.Zeo.Cxn.MoneyTransfer.WU.Data
{
    public class ImportReceiver : ZeoModel
    {
        public string NameType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public string ReceiverIndexNumber { get; set; }
      //  public string Address { get; set; }
        public string CountryCode { get; set; }
        public string Status { get; set; }
        public string PickupCountry { get; set; }
        public long CustomerId { get; set; }
        public string GoldCardNumber { get; set; }
        public DateTime DTTerminalDate { get; set; }
        public DateTime DTServerDate { get; set; }
    }
}
