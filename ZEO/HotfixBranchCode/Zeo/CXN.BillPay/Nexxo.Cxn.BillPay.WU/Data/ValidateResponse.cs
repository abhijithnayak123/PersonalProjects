using System.Collections.Generic;
using MGI.Cxn.WU.Common.Data;
namespace MGI.Cxn.BillPay.WU.Data
{
	public class ValidateResponse
	{
		public PaymentTransaction PaymentTransaction { get; set; }
		public DfFields DfField { get; set; }
		public ForeignRemoteSystem ForeignRemoteSystem { get; set; }
		public string misc_bufferField { get; set; }
	}
}
