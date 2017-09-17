using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Core.Partner.Contract
{
    public partial class ChannelPartnerException : NexxoException
    {
        //Shopping Cart Exceptions  From 3600 - 3699

        static public int TRANSACTION_CREATE_FAILED = 3600;
        static public int TRANSACTION_NOT_FOUND = 3601;
        static public int TRANSACTION_UPDATE_STATES_FAILED = 3602;
        static public int TRANSACTION_UPDATE_CXE_STATE_FAILED = 3603;
        static public int TRANSACTION_UPDATE_CXN_STATE_FAILED = 3604;
        static public int TRANSACTION_UPDATE_FEE_FAILED = 3605;
        static public int TRANSACTION_LINK_TO_LEDGER_ENTRY_FAILED = 3606;
        static public int TRANSACTION_UPDATE_AMOUNT_FAILED = 3607;
        static public int TRANSACTION_UPDATE_TRANSACTIONDETAILS_FAILED = 3608;
		static public int TRANSACTION_UPDATE_FAILED = 3609;
		static public int TRANSACTION_UPDATE_MONEYORDERIMAGE_FAILED = 3610;
    }
}
