using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MGI.Common.DataAccess.Data;

namespace MGI.Cxn.WU.Common.Data
{
    public class WUBaseRequestResponse : NexxoModel
    {
        public Channel Channel { get; set; }
        public ForeignRemoteSystem ForeignRemoteSystem  { get; set; }
        public X509Certificate2 ClientCertificate       { get; set; }
        public string ServiceUrl { get; set; }
        public string ReferenceNo { get; set; }
        public string pay_or_do_not_pay_indicator { get; set; }
    }
}
