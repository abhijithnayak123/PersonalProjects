using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using TCF.Channel.Zeo.Service.Contract;
using TCF.Channel.Zeo.Data;
using TCF.Zeo.Biz.Contract;
using TCF.Zeo.Biz.Impl;
using System.ServiceModel;
using commonData = TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;

namespace TCF.Channel.Zeo.Service.Impl
{
    public partial class ZeoServiceImpl : ITerminalService
    {
        public IManageTerminal ManageTerminalService;
        public Data.Response GetTerminalById(long terminalId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            ManageTerminalService = new ManageTerminal();
            response.Result = ManageTerminalService.GetTerminalById(terminalId, commonContext);

            return response;
        }

        public Data.Response GetTerminalByName(string terminalName, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            ManageTerminalService = new ManageTerminal();
            response.Result = ManageTerminalService.GetTerminalByName(terminalName, commonContext);

            return response;
        }

        public Data.Response CreateTerminal(Data.Terminal terminal, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            ManageTerminalService = new ManageTerminal();
            response.Result = ManageTerminalService.CreateTerminal(terminal, commonContext);

            return response;
        }

        public Data.Response UpdateTerminal(Data.Terminal terminal, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope())
            {
                commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

                Response response = new Response();

                ManageTerminalService = new ManageTerminal();
                response.Result = ManageTerminalService.UpdateTerminal(terminal, commonContext);

                scope.Complete();

                return response;
            }
        }

        public Response GetNpsdiagnosticInfo(long terminalId, ZeoContext context)
        {
            commonData.ZeoContext commonContext = mapper.Map<commonData.ZeoContext>(context);

            Response response = new Response();

            ManageTerminalService = new ManageTerminal();
            response.Result = ManageTerminalService.GetNpsdiagnosticInfo(terminalId, commonContext);

            return response;
        }
    }
}
