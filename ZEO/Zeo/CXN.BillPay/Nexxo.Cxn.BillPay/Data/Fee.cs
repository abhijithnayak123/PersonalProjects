using System.Collections.Generic;

namespace MGI.Cxn.BillPay.Data
{
    public class Fee
    {
		public long TransactionId { get; set; }
        public string SessionCookie { get; set; }
		public string AccountHolderName { get; set; }
		public string AvailableBalance { get; set; }
        public List<DeliveryMethod> DeliveryMethods { get; set; }
		public string CityCode { get; set; }
    }
}
