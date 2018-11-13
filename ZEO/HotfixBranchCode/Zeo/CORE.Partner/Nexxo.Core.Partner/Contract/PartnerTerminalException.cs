using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Core.Partner.Contract
{
    public partial class ChannelPartnerException : NexxoException
    {
        //Terminal   Exceptions     From 3300 - 3399

        public static int TERMINAL_CREATE_FAILED = 3300;
        public static int TERMINAL_UPDATE_FAILED = 3301;
        public static int TERMINAL_NOT_FOUND = 3302;

        public static int NPSTERMINAL_CREATE_FAILED = 3303;
        public static int NPSTERMINAL_UPDATE_FAILED = 3304;
        public static int NPSTERMINAL_NOT_FOUND = 3305;
    }
}
