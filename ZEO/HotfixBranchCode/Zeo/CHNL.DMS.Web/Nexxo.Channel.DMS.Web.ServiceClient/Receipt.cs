using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Channel.DMS.Web.ServiceClient
{
    public class Receipts
    {
        public List<string []> ReceiptData; // self reference. This is a tree of receipts.
    }
}
