using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Core.Partner.Contract
{
    public partial class ChannelPartnerException : NexxoException
    {
        //User   Exceptions  From 3400 - 3499
        static public int USER_CREATE_FAILED = 3400;
        static public int USER_UPDATE_FAILED = 3401;
        static public int USER_NOT_FOUND = 3402;
        static public int USER_AUTHENTICATION_CREATE_FAILED = 3403;
        static public int USER_LOCATION_MAPPING_CREATE_FAILED = 3404;
        static public int USER_AUTHENTICATION_NOT_FOUND = 3405;
       
       
    }
}
