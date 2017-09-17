using System.Collections.Generic;
using MGI.Biz.Events.Contract;
using MGI.Common.Util;

namespace MGI.Biz.FundsEngine.Data
{
    public class GPRAddEvent : NexxoBizEvent
    {
        private static readonly string NAME = "GPRAdd";

        public GPRAddEvent() : base(NAME) {} 

        public FundsAccount Gpr { get; set; }
        public MGIContext mgiContext;

        public long CXNId { get; set; }
    }
}
