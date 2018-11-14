using TCF.Zeo.Core.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using P3Net.Data.Common;
using P3Net.Data;
using System.Data;

namespace TCF.Zeo.Core.Impl
{
    public class ZeoContextServiceImpl : IZeoContext
    {
        /// <summary>
        /// This method is used to return the AlloyContext when the agent session is initiated.
        /// </summary>
        /// <param name="agentSessionId"></param>
        /// <returns></returns>
        public ZeoContext GetZeoContextForAgent(long agentSessionId, ZeoContext context)
        {
            StoredProcedure customerProcedure = new StoredProcedure("usp_GetAlloyContextByAgentSessionId");

            customerProcedure.WithParameters(InputParameter.Named("agentSessionId").WithValue(agentSessionId));

            using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(customerProcedure))
            {
                while (datareader.Read())
                {
                    context = new ZeoContext();
                    context.ChannelPartnerId = datareader.GetInt64OrDefault("ChannelPartnerID");
                    context.ChannelPartnerName = datareader.GetStringOrDefault("ChannelPartnerName");
                    context.LocationID = datareader.GetInt64OrDefault("LocationID");
                    context.LocationName = datareader.GetStringOrDefault("LocationName");
                    context.BranchId = datareader.GetStringOrDefault("BranchID");
                    context.BankId = datareader.GetStringOrDefault("BankID");
                    context.TimeZone = datareader.GetStringOrDefault("TimezoneID");
                    context.AgentSessionId = datareader.GetInt64OrDefault("AgentSessionID");
                    context.TerminalName = datareader.GetStringOrDefault("TerminalName");
                    context.AgentId = datareader.GetInt64OrDefault("AgentID");
                    context.AgentFirstName = datareader.GetStringOrDefault("AgentFirstName");
                    context.AgentLastName = datareader.GetStringOrDefault("AgentLastName");
                    context.AgentName = datareader.GetStringOrDefault("AgentName");
                    context.CheckUserName = datareader.GetStringOrDefault("CheckUserName");
                    context.CheckPassword = datareader.GetStringOrDefault("CheckPassword");
                    long nodeId = 0;
                    long.TryParse(datareader.GetStringOrDefault("VisaLocationNodeId"), out nodeId);
                    context.VisaLocationNodeId = nodeId;
                    context.StateCode = datareader.GetStringOrDefault("StateCode");
                    //context.LocationUserName = datareader.GetStringOrDefault("LocationUserName");
                    //context.LocationPassword = datareader.GetStringOrDefault("LocationPassword");
                    //context.ProviderId = datareader.GetInt32OrDefault("ProviderID");
                }
            }

            return context;
        }

        /// <summary>
        /// This method is used to return the AlloyContext when the customer session is initiated.
        /// </summary>
        /// <param name="customerSessionId"></param>
        /// <returns></returns>
        public ZeoContext GetZeoContextForCustomer(long customerSessionId, ZeoContext context)
        {
            StoredProcedure customerProcedure = new StoredProcedure("usp_GetAlloyContextByCustomerSessionId");

            customerProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));

            using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(customerProcedure))
            {
                while (datareader.Read())
                {
                    context = new ZeoContext();
                    context.CustomerSessionId = datareader.GetInt64OrDefault("CustomerSessionID");
                    context.ChannelPartnerId = datareader.GetInt32OrDefault("ChannelPartnerID");
                    context.ChannelPartnerName = datareader.GetStringOrDefault("ChannelPartnerName");
                    context.LocationID = datareader.GetInt64OrDefault("LocationID");
                    context.LocationName = datareader.GetStringOrDefault("LocationName");
                    context.BranchId = datareader.GetStringOrDefault("BranchID");
                    context.BankId = datareader.GetStringOrDefault("BankID");
                    context.TimeZone = datareader.GetStringOrDefault("TimezoneID");
                    context.AgentSessionId = datareader.GetInt64OrDefault("AgentSessionID");
                    context.TerminalName = datareader.GetStringOrDefault("TerminalName");
                    context.AgentId = datareader.GetInt64OrDefault("AgentID");
                    context.AgentFirstName = datareader.GetStringOrDefault("AgentFirstName");
                    context.AgentLastName = datareader.GetStringOrDefault("AgentLastName");
                    context.AgentName = datareader.GetStringOrDefault("AgentName");
                    context.CheckUserName = datareader.GetStringOrDefault("CheckUserName");
                    context.CheckPassword = datareader.GetStringOrDefault("CheckPassword");
                    context.LocationZipCode = datareader.GetStringOrDefault("LocationZipCode");
                    long nodeId = 0;
                    long.TryParse(datareader.GetStringOrDefault("VisaLocationNodeId"), out nodeId);
                    context.VisaLocationNodeId = nodeId;
                    //context.LocationUserName = datareader.GetStringOrDefault("LocationUserName");
                    //context.LocationPassword = datareader.GetStringOrDefault("LocationPassword");
                    //context.ProviderId = datareader.GetInt32OrDefault("ProviderID");
                    context.CustomerId = datareader.GetInt64OrDefault("CustomerID");
                    context.WUCardNumber = datareader.GetStringOrDefault("WUCardNumber");
                    context.ClientAgentIdentifier = datareader.GetStringOrDefault("ClientAgentIdentifier");
                    context.StateCode = datareader.GetStringOrDefault("StateCode");
                }

            }

            return context;
        }
    }
}
