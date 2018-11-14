using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.Check.TCF.Contract;
using TCF.Zeo.Cxn.Check.TCF.Data;

namespace TCF.Zeo.Cxn.Check.TCF.Impl
{
    public class SimulatorIO : IIO
    {
        public TellerMainFrameResponse TellerInquiry(TCFOnusTransaction tcfonusTrx, RCIFCredential credential, ZeoContext context)
        {
            int endsWith = (int)(tcfonusTrx.Amount * 100) % 10;

            string status = endsWith == 1 ? "1" : "0";

            return new TellerMainFrameResponse()
            {
                TEL7770OperationResponse = new TEL7770OperationResponse()
                {
                    telnexxc_tran_return = new TELNexxcTranReturn()
                    {
                        telnexxc_reject = status
                    }
                }
            };
        }
    }
}
