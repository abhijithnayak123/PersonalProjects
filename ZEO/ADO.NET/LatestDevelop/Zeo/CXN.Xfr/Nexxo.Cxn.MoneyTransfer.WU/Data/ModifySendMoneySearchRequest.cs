using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGI.Cxn.WU.Common.Data;    

namespace MGI.Cxn.MoneyTransfer.WU.Data
{
	public class ModifySendMoneySearchRequest : WUBaseRequestResponse 
	{
		public PaymentTransaction paymentTransaction; 
	}
}
