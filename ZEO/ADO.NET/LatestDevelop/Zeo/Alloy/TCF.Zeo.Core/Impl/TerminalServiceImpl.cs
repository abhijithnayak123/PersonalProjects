using TCF.Zeo.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Core.Data;
using P3Net.Data.Common;
using System.Data;
using TCF.Zeo.Common.Data;
using P3Net.Data;
using coredata = TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : ITerminalService
    {

        public Terminal GetTerminalById(long terminalId, ZeoContext context)
        {
            try
            {
                Terminal terminal = new Terminal();

                StoredProcedure terminalGetProcedure = new StoredProcedure("usp_GetTerminalByTerminalId");

                terminalGetProcedure.WithParameters(InputParameter.Named("terminalId").WithValue(terminalId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(terminalGetProcedure))
                {
                    while (datareader.Read())
                    {
                        terminal.Id = datareader.GetInt64OrDefault("TerminalID");
                        terminal.Name = datareader.GetStringOrDefault("Name");
                        terminal.LocationId = datareader.GetInt64OrDefault("LocationId");
                        terminal.IpAddress = datareader.GetStringOrDefault("IpAddress");
                        terminal.MacAddress = datareader.GetStringOrDefault("MacAddress");
                        terminal.PeripheralServerId = datareader.GetStringOrDefault("NpsTerminalId");
                    }
                }

                return terminal;
            }
            catch (Exception ex)
            {
                throw new TerminalException(TerminalException.TERMINAL_GET_FAILED, ex);
            }
        }

        public Terminal GetTerminalByName( string terminalName, ZeoContext context)
        {
            try
            {
                Terminal terminal = new Terminal();

                StoredProcedure terminalGetProcedure = new StoredProcedure("usp_GetTerminalByName");

                terminalGetProcedure.WithParameters(InputParameter.Named("terminalName").WithValue(terminalName));
                terminalGetProcedure.WithParameters(InputParameter.Named("channelPartnerId ").WithValue(context.ChannelPartnerId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(terminalGetProcedure))
                {
                    while (datareader.Read())
                    {
                        terminal.TerminalId = datareader.GetInt64OrDefault("terminalId");
                        terminal.Name = datareader.GetStringOrDefault("Name");
                        terminal.MacAddress = datareader.GetStringOrDefault("MacAddress");
                        terminal.IpAddress = datareader.GetStringOrDefault("IpAddress");
                        terminal.LocationId = datareader.GetInt64OrDefault("LocationId");
                        terminal.PeripheralServerId = Convert.ToString(datareader.GetInt64OrDefault("NpsTerminalId"));
                        terminal.PeripheralServerUrl = datareader.GetStringOrDefault("PeripheralServiceUrl");
                        terminal.ChannelPartnerId = context.ChannelPartnerId;
                    }
                }

                return terminal;
            }
            catch (Exception ex)
            {
                throw new TerminalException(TerminalException.TERMINAL_GET_FAILED, ex);
            }
        }

        public bool CreateTerminal(Terminal terminal, ZeoContext context)
        {
            try
            {

                StoredProcedure CreateTerminal = new StoredProcedure("usp_CreateTerminal");

                CreateTerminal.WithParameters(InputParameter.Named("AgentId").WithValue(context.AgentId));
                CreateTerminal.WithParameters(InputParameter.Named("AgentFirstName").WithValue(context.AgentFirstName));
                CreateTerminal.WithParameters(InputParameter.Named("AgentLastName").WithValue(context.AgentLastName));

                CreateTerminal.WithParameters(InputParameter.Named("name").WithValue(terminal.Name));
                CreateTerminal.WithParameters(InputParameter.Named("IpAddress").WithValue(terminal.IpAddress));
                CreateTerminal.WithParameters(InputParameter.Named("MacAddress").WithValue(terminal.MacAddress));
                CreateTerminal.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(context.ChannelPartnerId));
                CreateTerminal.WithParameters(InputParameter.Named("locationId").WithValue(terminal.LocationId));
                CreateTerminal.WithParameters(InputParameter.Named("npsTerminalId").WithValue(terminal.PeripheralServerId));
                CreateTerminal.WithParameters(InputParameter.Named("agentSessionId").WithValue(context.AgentSessionId));
                CreateTerminal.WithParameters(InputParameter.Named("terminalDate").WithValue(DateTime.Now));
                CreateTerminal.WithParameters(InputParameter.Named("serverDate").WithValue(DateTime.Now));
                CreateTerminal.WithParameters(InputParameter.Named("dTServerCreate").WithValue(DateTime.Now));
                CreateTerminal.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(DateTime.Now));
                CreateTerminal.WithParameters(OutputParameter.Named("terminalId").OfType<long>());

                int count = DataHelper.GetConnectionManager().ExecuteNonQuery(CreateTerminal);
                terminal.TerminalId = Convert.ToInt64(CreateTerminal.Parameters["terminalId"].Value);
                return count > 0 ? false : true;
            }
            catch (Exception ex)
            {
                throw new TerminalException(TerminalException.TERMINAL_CREATE_FAILED, ex);
            }
        }

        public bool UpdateTerminal(Terminal terminal, ZeoContext context)
        {
            try
            {
                StoredProcedure terminalCreateProcedure = new StoredProcedure("usp_UpdateTerminal");

                terminalCreateProcedure.WithParameters(InputParameter.Named("terminalId").WithValue(terminal.TerminalId));
                terminalCreateProcedure.WithParameters(InputParameter.Named("name").WithValue(terminal.Name));
                terminalCreateProcedure.WithParameters(InputParameter.Named("macAddress").WithValue(terminal.MacAddress));
                terminalCreateProcedure.WithParameters(InputParameter.Named("ipAddress").WithValue(terminal.IpAddress));
                terminalCreateProcedure.WithParameters(InputParameter.Named("agentSessionId").WithValue(context.AgentSessionId));
                terminalCreateProcedure.WithParameters(InputParameter.Named("locationId").WithValue(terminal.LocationId));
                terminalCreateProcedure.WithParameters(InputParameter.Named("dTServerCreate").WithValue(DateTime.Now));
                terminalCreateProcedure.WithParameters(InputParameter.Named("serverDate").WithValue(DateTime.Now));
                terminalCreateProcedure.WithParameters(InputParameter.Named("terminalDate").WithValue(Helper.GetTimeZoneTime(context.TimeZone)));

                int count = DataHelper.GetConnectionManager().ExecuteNonQuery(terminalCreateProcedure);
                return count > 0;

            }
            catch (Exception ex)
            {
                throw new TerminalException(TerminalException.TERMINAL_UPDATE_FAILED, ex);
            }
        }


        public ChannelPartner GetChannelPartnerTim(long locationId, ZeoContext context)
        {
            try
            {
                ChannelPartner channelpartner = new ChannelPartner();

                StoredProcedure channelPartnerTim = new StoredProcedure("usp_GetChannelPartnerTim");

                channelPartnerTim.WithParameters(InputParameter.Named("locationId").WithValue(locationId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(channelPartnerTim))
                {
                    while (datareader.Read())
                    {
                        channelpartner = new ChannelPartner();
                        channelpartner.TIM = datareader.GetSByte("tim");
                    }
                }

                return channelpartner;
            }
            catch (Exception ex)
            {
                throw new TerminalException(TerminalException.TERMINAL_GET_FAILED, ex);
            }
        }

        public Terminal GetNpsdiagnosticInfo(long terminalId, ZeoContext context)
        {
            try
            {
                Terminal terminal = new Terminal();
                StoredProcedure npsterminalGetProcedure = new StoredProcedure("usp_getNpsdiagnosticInfo");

                npsterminalGetProcedure.WithParameters(InputParameter.Named("terminalId").WithValue(terminalId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(npsterminalGetProcedure))
                {
                    while (datareader.Read())
                    {
                        terminal.Name = datareader.GetStringOrDefault("Name");
                        terminal.LocationName = datareader.GetStringOrDefault("LocationName");
                        terminal.PeripheralServeName = datareader.GetStringOrDefault("PeripheralServeName");
                    }
                }

                return terminal;
            }
            catch (Exception ex)
            {
                throw new TerminalException(TerminalException.TERMINAL_GET_FAILED, ex);
            }
        }
    }
}
