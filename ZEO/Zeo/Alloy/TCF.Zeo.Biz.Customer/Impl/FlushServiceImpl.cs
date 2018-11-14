using TCF.Zeo.Biz.Customer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.Customer.Impl.TCF;
using CommonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Biz.Common.Data.Exceptions;
using TCF.Zeo.Common.Util;

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

        public void FinalCommit(CommonData.ZeoContext context)
        {
            try
            {
                IFlushService service = GetBizCustomerService(context.ChannelPartnerName);
                service.FinalCommit(context);
            }
            catch(Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FinalCommitException(FinalCommitException.FINAL_COMMIT_FAILED, ex);
            }
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
