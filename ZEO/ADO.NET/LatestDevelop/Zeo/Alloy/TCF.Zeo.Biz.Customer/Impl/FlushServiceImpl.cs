using TCF.Zeo.Biz.Customer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.Customer.Impl.TCF;
using CommonData = TCF.Zeo.Common.Data;

namespace TCF.Zeo.Biz.Customer.Impl
{
    public class FlushServiceImpl : IFlushService
    {
        public void PostFlush( decimal cardBalance, CommonData.ZeoContext context)
        {
            IFlushService service = GetBizCustomerService(context.ChannelPartnerName);
            service.PostFlush(cardBalance, context);
        }

        public void PreFlush(decimal cashToCustomer, CommonData.ZeoContext context)
        {
            GetBizCustomerService(context.ChannelPartnerName).PreFlush(cashToCustomer, context);
        }

        private IFlushService GetBizCustomerService(string channelpartnerName)
        {
            switch (channelpartnerName)
            {
                case "TCF":
                    return new TCFFlushServiceImpl();
                default:
                    break;
            }
            return null;
        }
    }
}
