using MGI.Cxn.Partner.TCF.Contract;
using System;
using MGI.Common.Util;
using MGI.Cxn.Partner.TCF.Data;

namespace MGI.Cxn.Partner.TCF.Impl
{
    public class FlushProcessorImpl : IFlushProcessor
    {
        public void PostFlush(CustomerTransactionDetails cart, MGIContext mgiContext)
        {
            throw new NotImplementedException();
        }

        public void PreFlush(CustomerTransactionDetails cart, MGIContext mgiContext)
        {
            throw new NotImplementedException();
        }
    }
}
