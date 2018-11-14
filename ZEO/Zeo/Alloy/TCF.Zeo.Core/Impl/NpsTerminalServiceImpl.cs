using TCF.Zeo.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Core.Data;
using P3Net.Data.Common;
using TCF.Zeo.Common.Data;
using System.Data;
using P3Net.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : INpsTerminal
    {
        public bool CreateNpsTerminal(NpsTerminal npsTerminal, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure npsTerminalCreateProcedure = new StoredProcedure("usp_CreateNpsTerminal");

                npsTerminalCreateProcedure.WithParameters(InputParameter.Named("name").WithValue(npsTerminal.Name));
                npsTerminalCreateProcedure.WithParameters(InputParameter.Named("Status").WithValue(npsTerminal.Status));
                npsTerminalCreateProcedure.WithParameters(InputParameter.Named("Description").WithValue(npsTerminal.Description));
                npsTerminalCreateProcedure.WithParameters(InputParameter.Named("ipAddress").WithValue(npsTerminal.IpAddress));
                npsTerminalCreateProcedure.WithParameters(InputParameter.Named("Port").WithValue(npsTerminal.Port));
                npsTerminalCreateProcedure.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(npsTerminal.ChannelPartnerId));
                npsTerminalCreateProcedure.WithParameters(InputParameter.Named("LocationId").WithValue(npsTerminal.LocationId));
                npsTerminalCreateProcedure.WithParameters(InputParameter.Named("PeripheralServiceUrl").WithValue(npsTerminal.PeripheralServiceUrl));
                npsTerminalCreateProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue((DateTime.Now)));
                npsTerminalCreateProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(Helper.GetTimeZoneTime(timeZone)));
                npsTerminalCreateProcedure.WithParameters(OutputParameter.Named("npsTerminalId").OfType<long>());

                int count = DataHelper.GetConnectionManager().ExecuteNonQuery(npsTerminalCreateProcedure);

                npsTerminal.NpsTerminalId = Convert.ToInt64(npsTerminalCreateProcedure.Parameters["npsTerminalId"].Value);
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new TerminalException(TerminalException.NPSTERMINAL_CREATE_FAILED, ex);
            }
        }
        public List<NpsTerminal> GetNpsTerminalBylocationId(string locationId, ZeoContext context)
        {
            try
            {
                List<NpsTerminal> npsTerminals = new List<NpsTerminal>();

                StoredProcedure npsterminalGetProcedure = new StoredProcedure("usp_GetNpsTerminalBylocationId");

                npsterminalGetProcedure.WithParameters(InputParameter.Named("locationId").WithValue(locationId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(npsterminalGetProcedure))
                {
                    while (datareader.Read())
                    {
                        NpsTerminal npsterminal = new NpsTerminal();
                        npsterminal.NpsTerminalId = datareader.GetInt64OrDefault("npsTerminalID");
                        npsterminal.Name = datareader.GetStringOrDefault("Name");
                        npsterminal.Status = datareader.GetStringOrDefault("Status");
                        npsterminal.Description = datareader.GetStringOrDefault("Description");
                        npsterminal.Port = datareader.GetStringOrDefault("Port");
                        npsterminal.PeripheralServiceUrl = datareader.GetStringOrDefault("PeripheralServiceUrl");
                        npsTerminals.Add(npsterminal);
                    }
                }

                return npsTerminals;
            }
            catch(Exception ex)
            {
                throw new TerminalException(TerminalException.NPSTERMINAL_GET_FAILED, ex);
            }
        }
        public NpsTerminal GetNpsTerminalByName(string name, long channelPartnerId, ZeoContext context)
        {
            try
            {
                NpsTerminal npsterminal = new NpsTerminal();

                StoredProcedure npsterminalGetProcedure = new StoredProcedure("usp_GetNpsTerminalByName");

                npsterminalGetProcedure.WithParameters(InputParameter.Named("name").WithValue(name));
                npsterminalGetProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(npsterminalGetProcedure))
                {
                    while (datareader.Read())
                    {
                        npsterminal.NpsTerminalId = datareader.GetInt64OrDefault("npsTerminalID");
                        npsterminal.Name = datareader.GetStringOrDefault("Name");
                        npsterminal.Status = datareader.GetStringOrDefault("Status");
                        npsterminal.Description = datareader.GetStringOrDefault("Description");
                        npsterminal.IpAddress = datareader.GetStringOrDefault("IpAddress");
                        npsterminal.Port = datareader.GetStringOrDefault("Port");
                        npsterminal.PeripheralServiceUrl = datareader.GetStringOrDefault("PeripheralServiceUrl");
                    }
                }

                return npsterminal;
            }
            catch (Exception ex)
            {

                throw new TerminalException(TerminalException.NPSTERMINAL_GET_FAILED, ex);
            }
        }
        public List<NpsTerminal> GetNpsterminalByTerminalId(long NpsTerminalId, ZeoContext context)
        {
            try
            {
                List<NpsTerminal> NpsTerminal = new List<NpsTerminal>();
                NpsTerminal npsTerminal;

                StoredProcedure getNpsTerminalProcedure = new StoredProcedure("usp_GetNpsTerminalById");

                getNpsTerminalProcedure.WithParameters(InputParameter.Named("npsTerminalId").WithValue(NpsTerminalId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(getNpsTerminalProcedure))
                {
                    while (datareader.Read())
                    {
                        npsTerminal = new NpsTerminal();
                        npsTerminal.NpsTerminalId = datareader.GetInt64OrDefault("NpsTerminalID");
                        npsTerminal.Name = datareader.GetStringOrDefault("Name");
                        npsTerminal.Status = datareader.GetStringOrDefault("Status");
                        npsTerminal.Description = datareader.GetStringOrDefault("Description");
                        npsTerminal.IpAddress = datareader.GetStringOrDefault("IpAddress");
                        npsTerminal.Port = datareader.GetStringOrDefault("Port");
                        npsTerminal.PeripheralServiceUrl = datareader.GetStringOrDefault("PeripheralServiceUrl");
                        npsTerminal.LocationId = datareader.GetInt64OrDefault("locationId");
                        NpsTerminal.Add(npsTerminal);
                    }
                }

                return NpsTerminal;
            }
            catch (Exception ex)
            {
                throw new TerminalException(TerminalException.NPSTERMINAL_GET_FAILED, ex);
            }
        }
        public bool UpdateNpsTerminal(NpsTerminal npsTerminal, string timeZone, ZeoContext context)
        {
            {
                try
                {
                    StoredProcedure updateNpsTerminal = new StoredProcedure("usp_updateNpsTerminal");
                    updateNpsTerminal.WithParameters(InputParameter.Named("npsTerminalId").WithValue(npsTerminal.NpsTerminalId));
                    updateNpsTerminal.WithParameters(InputParameter.Named("Description").WithValue(npsTerminal.Description));
                    updateNpsTerminal.WithParameters(InputParameter.Named("IpAddress").WithValue(npsTerminal.IpAddress));
                    updateNpsTerminal.WithParameters(InputParameter.Named("Name").WithValue(npsTerminal.Name));
                    updateNpsTerminal.WithParameters(InputParameter.Named("PeripheralServiceUrl").WithValue(npsTerminal.PeripheralServiceUrl));
                    updateNpsTerminal.WithParameters(InputParameter.Named("Port").WithValue(npsTerminal.Port));
                    updateNpsTerminal.WithParameters(InputParameter.Named("Status").WithValue(npsTerminal.Status));
                    updateNpsTerminal.WithParameters(InputParameter.Named("dTTerminatalLastModified").WithValue(Helper.GetTimeZoneTime(timeZone)));
                    updateNpsTerminal.WithParameters(InputParameter.Named("dTServerLastModified").WithValue(DateTime.Now));
                    updateNpsTerminal.WithParameters(InputParameter.Named("locationId").WithValue(npsTerminal.LocationId));

                    DataHelper.GetConnectionManager().ExecuteNonQuery(updateNpsTerminal);

                    return true;
                }
                catch (Exception ex)
                {
                    throw new TerminalException(TerminalException.NPSTERMINAL_UPDATE_FAILED, ex);
                }
            }
        }

    }
}
