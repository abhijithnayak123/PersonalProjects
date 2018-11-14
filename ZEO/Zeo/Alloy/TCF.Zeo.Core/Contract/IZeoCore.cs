using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Contract
{
    public interface IZeoCore : ICustomerService, IMoneyOrderService, ICheckService, IBillPayService, IMoneyTransferService, IFundService, IFeeService, IPricingCluster, ICustomerFeeAdjustmentService,
                                  ILocationCounterIdService, ILocationProcessorCredentialService, ILocationService, INpsTerminal,ITerminalService
    {
    }
}
