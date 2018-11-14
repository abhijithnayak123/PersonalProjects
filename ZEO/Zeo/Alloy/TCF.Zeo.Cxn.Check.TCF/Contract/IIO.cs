using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.Check.TCF.Data;

namespace TCF.Zeo.Cxn.Check.TCF.Contract
{
    public interface IIO
    {
        TellerMainFrameResponse TellerInquiry(TCFOnusTransaction tcfonusTrx, RCIFCredential credential, ZeoContext context);
    }
}
