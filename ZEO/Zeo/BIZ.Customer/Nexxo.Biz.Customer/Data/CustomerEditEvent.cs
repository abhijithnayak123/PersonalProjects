using System.Collections.Generic;
using MGI.Biz.Events.Contract;
using MGI.Common.Util;

namespace MGI.Biz.Customer.Data
{
    public class CustomerEditEvent : NexxoBizEvent
    {
        private static readonly string NAME = "Customer-Edit";

        public CustomerEditEvent() : base(NAME) { }
        public CustomerProfile profile{get;set;}
        public MGIContext mgiContext;
    }
}
