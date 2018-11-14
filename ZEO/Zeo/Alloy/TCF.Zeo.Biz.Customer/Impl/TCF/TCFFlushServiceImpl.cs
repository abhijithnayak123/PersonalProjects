using TCF.Zeo.Biz.Customer.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Cxn.Customer.Contract;
using AutoMapper;
using TCF.Zeo.Cxn.Customer.TCF.Impl;
using CommonData = TCF.Zeo.Common.Data;
namespace TCF.Zeo.Biz.Customer.Impl.TCF
{
    class TCFFlushServiceImpl : IFlushService
    {
        IMapper mapper;
        private IFlushProcessor processor { get; set; }

        public TCFFlushServiceImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ZeoContext, CommonData.ZeoContext>().ReverseMap();
            });
            mapper = config.CreateMapper();
        }

        public void PostFlush(decimal cardBalance, CommonData.ZeoContext context)
        {
            if (context.Context == null)
            {
                context.Context = new Dictionary<string, object>();
            }
            context.Context.Add("SSOAttributes", context.SSOAttributes);

            using (processor = new FlushProcessorImpl())
            {
                processor.PostFlush(cardBalance, context);
            }
        }

        public void PreFlush(decimal cashToCustomer, CommonData.ZeoContext context)
        {
            if (context.Context == null)
            {
                context.Context = new Dictionary<string, object>();
            }
            context.Context.Add("SSOAttributes", context.SSOAttributes);

            using (processor = new FlushProcessorImpl())
            {
                processor.PreFlush(cashToCustomer, context);
            }
        }

        public void FinalCommit(CommonData.ZeoContext context)
        {
            if (context.Context == null)
            {
                context.Context = new Dictionary<string, object>();
            }
            context.Context.Add("SSOAttributes", context.SSOAttributes);

            using (processor = new FlushProcessorImpl())
            {
                processor.TellerFinalCommitCalls(context);
            }
        }
    }
}
