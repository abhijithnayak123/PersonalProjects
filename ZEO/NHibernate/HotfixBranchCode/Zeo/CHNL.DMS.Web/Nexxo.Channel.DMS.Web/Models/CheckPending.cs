﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
    public class CheckPending : CheckDetails
    {
        public string CheckInProcessMessage { get; set; }
        public string CheckWaitTimeMessage { get; set; }

        public string CheckInProcessMessageDetails { get; set; }

    }
}