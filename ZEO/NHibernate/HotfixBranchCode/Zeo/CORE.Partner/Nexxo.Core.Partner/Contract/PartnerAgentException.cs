using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.Sys;

namespace MGI.Core.Partner.Contract
{
    public partial class ChannelPartnerException : NexxoException
    {
        //Partner Exceptions From 3100 - 3199
        static public int AGENTSESSION_CREATE_FAILED = 3100;
        static public int AGENTSESSION_NOT_FOUND = 3101;
		static public int AGENTSESSION_UPDATE_FAILED = 3102;
	}
}
