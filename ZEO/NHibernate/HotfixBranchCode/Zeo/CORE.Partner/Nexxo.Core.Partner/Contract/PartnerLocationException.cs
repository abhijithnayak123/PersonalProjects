using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Core.Partner.Contract
{
    public partial class ChannelPartnerException : NexxoException
    {
        //Location  Exceptions  From 3200 - 3299
        static public int LOCATION_CREATE_FAILED = 3200;
        static public int LOCATION_UPDATE_FAILED = 3201;
        static public int LOCATION_NOT_FOUND = 3202;
        static public int LOCATION_ALREADY_EXISTS = 3203;
		static public int LOCATION_IDENTIFIER_ALREADY_EXISTS = 3204;
		static public int LOCATION_BANKID_BRANCHID_ALREADY_EXISTS = 3205;
		static public int LOCATION_COUNTERID_NOT_FOUND = 3206;
		static public int LOCATION_COUNTERID_STATUS_UPDATE_FAILED = 3207;
    }
}
