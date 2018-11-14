﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Cxn.Common;

namespace TCF.Zeo.Cxn.Check.TCF.Data
{
    public class RCIFCredential : BaseRequest
    {
        public string ServiceUrl { get; set; }
        public string TellerInquiryUrl { get; set; }
        public string CertificateName { get; set; }
        public string ThumbPrint { get; set; }
        public string RCIFFinalCommitURL { get; set; }
    }
}
