using TCF.Channel.Zeo.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using System.ServiceModel;
using commonData = TCF.Zeo.Common.Data;
using bizMessageService = TCF.Zeo.Biz.Contract;
using TCF.Zeo.Biz.Impl;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : IMessageCenterService
    {
        public bizMessageService.IMessageCenterService messageService;

        public Response GetAgentMessages(Data.ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            messageService = new MessageCenterServiceImpl();
            response.Result = messageService.GetMessagesByAgentID(commonContext);
            return response;
        }

        public Response GetMessageDetails(long transactionId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();
            messageService = new MessageCenterServiceImpl();
            response.Result = messageService.GetMessageDetails(transactionId, commonContext);

            return response;
        }

    }
}
