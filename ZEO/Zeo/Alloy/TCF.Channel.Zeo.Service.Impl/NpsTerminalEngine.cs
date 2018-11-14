using TCF.Zeo.Biz.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Channel.Zeo.Data;
using TCF.Channel.Zeo.Service.Contract;
using TCF.Zeo.Biz.Impl;
using System.ServiceModel;
using commonData = TCF.Zeo.Common.Data;


namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : INpsTerminalService
    {
        public IManageNpsTerminal ManageNpsTerminal;

        public Response CreateNpsTerminal(NpsTerminal npsTerminal, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            ManageNpsTerminal = new ManageNpsTerminal();
            response.Result = ManageNpsTerminal.CreateNpsTerminal(npsTerminal, commonContext);

            return response;
        }

        public Response GetNpsTerminalBylocationId(string locationId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            ManageNpsTerminal = new ManageNpsTerminal();
            response.Result = ManageNpsTerminal.GetNpsTerminalBylocationId(locationId, commonContext);

            return response;
        }


        public Response GetNpsTerminalByName(string name, long channelPartnerId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            ManageNpsTerminal = new ManageNpsTerminal();
            response.Result = ManageNpsTerminal.GetNpsTerminalByName(name, channelPartnerId, commonContext);

            return response;
        }

        public Response GetNpsterminalByTerminalId(long npsTerminalId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            ManageNpsTerminal = new ManageNpsTerminal();
            response.Result = ManageNpsTerminal.GetNpsterminalByTerminalId(npsTerminalId, commonContext);

            return response;
        }

        public Response UpdateNpsTerminal(NpsTerminal npsTerminal, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            ManageNpsTerminal = new ManageNpsTerminal();
            response.Result = ManageNpsTerminal.UpdateNpsTerminal(npsTerminal, commonContext);

            return response;
        }
    }
}
