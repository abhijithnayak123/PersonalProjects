using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Data.Exceptions;
using P3Net.Data;
using P3Net.Data.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : IAgentService
    {
        #region IAgentService Methods

        /// <summary>
        /// Creating/updating the Agent Details.
        /// </summary>
        /// <param name="UserName"> Agent User name</param>
        /// <param name="FirstName">Agent First Name</param>
        /// <param name="LastName">Agent Last Name</param>
        /// <param name="RoleId">Agent Role</param>
        /// <param name="ClientAgentIdentifier">Agent ClientAgentIdentifier</param>
        /// <param name="terminalName">Terminal Name</param>
        /// <param name="channelPartnerId">Agent Chanel partner id</param>
        /// <returns>Agent Details after Create/Update</returns>
        public UserDetails AuthenticateSSOAgent(string userName, string firstName,string lastName, string fullName, int roleId,string clientAgentIdentifier,string terminalName, int channelPartnerId,ZeoContext context)
        {
            UserDetails userDetail = new UserDetails();

            string name = string.IsNullOrWhiteSpace(fullName) ? string.Format("{0} {1}", firstName, lastName) : fullName;
            try
            {
                StoredProcedure getUserDetails = new StoredProcedure("usp_CreateUpdateSSOAgent");
                getUserDetails.WithParameters(InputParameter.Named("UserName").WithValue(userName));
                getUserDetails.WithParameters(InputParameter.Named("FirstName").WithValue(firstName));
                getUserDetails.WithParameters(InputParameter.Named("LastName").WithValue(lastName));
                getUserDetails.WithParameters(InputParameter.Named("FullName").WithValue(name));
                getUserDetails.WithParameters(InputParameter.Named("RoleId").WithValue(roleId));
                getUserDetails.WithParameters(InputParameter.Named("ClientAgentIdentifier").WithValue(clientAgentIdentifier));
                getUserDetails.WithParameters(InputParameter.Named("TerminalName").WithValue(terminalName));
                getUserDetails.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(channelPartnerId));
                getUserDetails.WithParameters(InputParameter.Named("DtServerLastModified").WithValue(DateTime.Now));

                using (IDataReader reader = DataHelper.GetConnectionManager().ExecuteReader(getUserDetails))
                {
                    while (reader.Read())
                    {
                        userDetail.AgentID = reader.GetInt64OrDefault("AgentID");
                        userDetail.FirstName = reader.GetStringOrDefault("FirstName");
                        userDetail.LastName = reader.GetStringOrDefault("LastName");
                        userDetail.FullName = reader.GetStringOrDefault("FullName");
                        userDetail.UserName = reader.GetStringOrDefault("UserName");
                    }
                }

                return userDetail;
            }
            catch (Exception ex)
            {
                throw new AgentException(AgentException.AGENTSESSION_CREATE_FAILED, ex);
            }
                
        }

        /// <summary>
        /// Creating the Agent Session
        /// </summary>
        /// <param name="AgentID">agent id which is unique for agent</param>
        /// <param name="ClientAgentIdentifier">Agent ClientAgentIdentifier</param>
        /// <param name="channelPartnerId">Agent Chanel partner id</param>
        /// <param name="terminalName">Terminal Name</param>
        /// <param name="businessDate">Business Date</param>
        /// <returns>returns the agent session</returns>
        public AgentSession CreateSession(long agentID, string clientAgentIdentifier, int channelPartnerId, string terminalName,string businessDate, ZeoContext context)
        {
            try
            {
                AgentSession agentSession = new AgentSession();

                DateTime? BusinessDate = (!string.IsNullOrEmpty(businessDate)) ? Convert.ToDateTime(businessDate) : (DateTime?)null;
                StoredProcedure sesstionDetails = new StoredProcedure("usp_CreateAgentSession");

                sesstionDetails.WithParameters(OutputParameter.Named("AgentSessionID").OfType<string>());
                sesstionDetails.WithParameters(OutputParameter.Named("LocationName").OfType<string>());
                sesstionDetails.WithParameters(OutputParameter.Named("BankID").OfType<string>());
                sesstionDetails.WithParameters(OutputParameter.Named("BranchID").OfType<string>());
                sesstionDetails.WithParameters(OutputParameter.Named("ChannelPartnerName").OfType<string>());
                sesstionDetails.WithParameters(OutputParameter.Named("Description").OfType<string>());
                sesstionDetails.WithParameters(OutputParameter.Named("PeripheralServiceUrl").OfType<string>());
                sesstionDetails.WithParameters(OutputParameter.Named("TerminalId").OfType<long>());
                sesstionDetails.WithParameters(InputParameter.Named("DtServerCreate").WithValue(DateTime.Now));
                sesstionDetails.WithParameters(InputParameter.Named("AgentID").WithValue(agentID));
                sesstionDetails.WithParameters(InputParameter.Named("TerminalName").WithValue(terminalName));
                sesstionDetails.WithParameters(InputParameter.Named("BusinessDate").WithValue(businessDate));
                sesstionDetails.WithParameters(InputParameter.Named("ClientAgentIdentifier").WithValue(clientAgentIdentifier));
                sesstionDetails.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(channelPartnerId));

                DataHelper.GetConnectionManager().ExecuteNonQuery(sesstionDetails);

                agentSession.SessionId = Convert.ToString(sesstionDetails.Parameters["AgentSessionID"].Value);
                agentSession.LocationName = Convert.ToString(sesstionDetails.Parameters["LocationName"].Value);
                agentSession.BankID = Convert.ToString(sesstionDetails.Parameters["BankID"].Value);
                agentSession.BranchID = Convert.ToString(sesstionDetails.Parameters["BranchID"].Value);
                agentSession.ChannelPartnerName = Convert.ToString(sesstionDetails.Parameters["ChannelPartnerName"].Value);
                agentSession.PeripheralServiceUrl = Convert.ToString(sesstionDetails.Parameters["PeripheralServiceUrl"].Value);
                agentSession.TerminalId = Convert.ToInt64(!string.IsNullOrEmpty(sesstionDetails.Parameters["TerminalId"].Value.ToString()) ? sesstionDetails.Parameters["TerminalId"].Value : "0");
                agentSession.TerminalName = terminalName;
                return agentSession;
            }

            catch (Exception ex)
            {
                throw new AgentException(AgentException.AGENTSESSION_CREATE_FAILED, ex);
            }
        }

        /// <summary>
        /// This method is get the agent RoleId details by agent Id
        /// </summary>
        /// <param name="agentId">This is unique identifier of user details</param>
        /// <returns>This is user details</returns>
        public int GetAgentRoleId(long agentId, ZeoContext context)
        {
            int roleId=0;
            try
            {
                StoredProcedure getUserDetails = new StoredProcedure("usp_GetSSOAgentRole");
                getUserDetails.WithParameters(InputParameter.Named("AgentId").WithValue(agentId));

                using (IDataReader reader = DataHelper.GetConnectionManager().ExecuteReader(getUserDetails))
                {
                    while (reader.Read())
                    {
                        roleId = reader.GetInt32OrDefault("UserRoleId");
                    }
                }
                 
                return roleId;
            }

            catch (Exception ex)
            {
                throw new AgentException(AgentException.AGENTSESSION_GET_FAILED, ex);
            }
        }

        /// <summary>
        ///  this method will get the agent Details by passing sessionId
        /// </summary>
        /// <param name="agentsessionId"></param>
        /// <returns></returns>
        public UserDetails GetAgentDetails(long agentsessionId, ZeoContext context)
        {
            UserDetails userDetail = new UserDetails();
            try
            {
                StoredProcedure getUserDetails = new StoredProcedure("usp_GetAgentBySessionId");
                getUserDetails.WithParameters(InputParameter.Named("AgentsessionId").WithValue(agentsessionId));

                using (IDataReader reader = DataHelper.GetConnectionManager().ExecuteReader(getUserDetails))
                {
                    while (reader.Read())
                    {
                        userDetail.AgentID = reader.GetInt64OrDefault("AgentID");
                        userDetail.FirstName = reader.GetStringOrDefault("FirstName");
                        userDetail.LastName = reader.GetStringOrDefault("LastName");
                        userDetail.UserRoleId = reader.GetInt32OrDefault("UserRoleId");
                        userDetail.LocationId = reader.GetInt64OrDefault("PrimaryLocationId");
                        userDetail.FullName = string.Format("{0} {1}", userDetail.FirstName, userDetail.LastName);
                        userDetail.AuthStatus = reader.GetInt32OrDefault("UserStatusId");
                    }
                }
              
                return userDetail;
            }

            catch (Exception ex)
            {
                throw new AgentException(AgentException.AGENTSESSION_GET_FAILED, ex);
            }
        }
        public List<UserDetails> GetAgents(long locationId, ZeoContext context)
        {
            List<UserDetails> users = new List<UserDetails>();
            try
            {
                StoredProcedure getUsers = new StoredProcedure("usp_GetAgentsByLocationId");
                getUsers.WithParameters(InputParameter.Named("LocationId").WithValue(locationId));

                using (IDataReader reader = DataHelper.GetConnectionManager().ExecuteReader(getUsers))
                {
                    while (reader.Read())
                    {
                        UserDetails details = new UserDetails();
                        details.AgentID = reader.GetInt64OrDefault("AgentID");
                        details.FirstName = reader.GetStringOrDefault("FirstName");
                        details.LastName = reader.GetStringOrDefault("LastName");
                        details.UserRoleId = reader.GetInt32OrDefault("UserRoleId");
                        details.LocationId = reader.GetInt64OrDefault("PrimaryLocationId");
                        details.FullName = reader.GetStringOrDefault("FullName");
                        details.AuthStatus = reader.GetInt32OrDefault("UserStatusId");
                        users.Add(details);
                    }
                }
                   
                return users;
            }
            catch (Exception ex)
            {
                throw new AgentException(AgentException.AGENTSESSION_GET_FAILED, ex);
            }
        }
        #endregion
    }
}


  