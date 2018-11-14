using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using commonData = TCF.Zeo.Common.Data;
namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IMessageStoreService
    {
        public Response GetMessage(string messageKey, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            TCF.Zeo.Biz.Contract.IMessageStoreService messageService = new TCF.Zeo.Biz.Impl.MessageStoreServiceImpl();
            Response response = new Response();
            response.Result = messageService.GetMessage(messageKey, commonContext);

            return response;
        }

        public Response LookUp(MessageStoreSearch search, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            TCF.Zeo.Biz.Contract.IMessageStoreService messageService = new TCF.Zeo.Biz.Impl.MessageStoreServiceImpl();
            Response response = new Response();
            response.Result = messageService.Lookup(search, commonContext);

            return response;
        }
    }
}
