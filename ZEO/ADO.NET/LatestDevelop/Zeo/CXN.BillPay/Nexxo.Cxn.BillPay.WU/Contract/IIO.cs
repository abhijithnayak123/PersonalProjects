using MGI.Common.Util;
using MGI.Cxn.BillPay.Data;
using MGI.Cxn.BillPay.WU.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Cxn.BillPay.WU.Impl
{
	public interface IIO
	{	 
		string GetBillerMessage(string billerName, MGIContext mgiContext);
		
		List<Location> GetLocations(string billerName, string accountNumber, decimal amount, WesternUnionAccount account, BillPaymentRequest billPaymentRequest, MGIContext mgiContext);
		
		Fee GetDeliveryMethods(string billerName, string accountNumber, decimal amount, Location location, WesternUnionAccount account, BillPaymentRequest billPaymentRequest, MGIContext mgiContext);
		
		List<Field> GetBillerFields(string billerName, string locationName, MGIContext mgiContext);
		
		long ValidatePayment(BillPaymentRequest billPaymentRequest, WesternUnionTrx trx, MGIContext mgiContext);
		
		long MakePayment(WesternUnionTrx trx, MGIContext mgiContext);
	}
}
