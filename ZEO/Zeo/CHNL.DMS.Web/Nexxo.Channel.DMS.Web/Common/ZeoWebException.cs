using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCF.Channel.Zeo.Web.Common
{
	public class ZeoWebException : Exception
	{
		public ZeoWebException(string message)
			: base(message)
		{
		}
	}

    public class ShoppingCartException
    {
        public const string GPR_TRANSACTION_ALREADY_EXIST   = "1000.100.8080";//Cart already having GPR Transaction, cannot do another GPR Transaction for this customer session
        public const string DECLINED_CHECK_EXIST            = "1000.100.8081";//Cannot Complete Shopping Cart with a Declined Check. Remove Check to Continue
        public const string FAILED_MONEYORDER               = "1000.100.8082";//Cannot Complete Shopping Cart with a Failed Money Order. Remove Money Order to Continue
        public const string AVAILABLE_BALANCE_LESS          = "1000.100.8083";//Withdraw Amount and related fee should be less than available balance
        public const string CARD_NOT_ACTIVATED              = "1000.100.8084";//Card not activated, hence cannot withdraw amount
        public const string INSUFFICIENT_FUNDS              = "1000.100.8085";//Insufficient Funds ! Add Cash
        public const string RELOAD_EXCEPTION                = "1000.100.8086";//ReloadException
    }
}