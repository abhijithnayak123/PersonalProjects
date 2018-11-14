using System;
using System.Linq;
using System.Collections.Generic;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Contract;
using P3Net.Data.Common;
using System.Data;
using TCF.Zeo.Common.Data;
using P3Net.Data;
using TCF.Zeo.Common.Util;
using static TCF.Zeo.Common.Util.Helper;
using System.IO;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public class MessageCenterImpl : IMessageCenter
    {

        public AgentMessage GetMessageByTransactionId(long transactionId, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_GetMessagesByTransactionId");

                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));

                AgentMessage agentMessage = DataHelper.GetConnectionManager().ExecuteQueryWithResult<AgentMessage>(coreTrxProcedure, k => new AgentMessage
                {
                    TransactionId = transactionId,
                    CustomerFirstName = k.GetStringOrDefault("FirstName"),
                    CustomerLastName = k.GetStringOrDefault("LastName"),
                    TicketNumber = k.GetStringOrDefault("TicketId"),
                    TransactionState = Enum.GetName(typeof(TransactionStates), k.GetInt32OrDefault("State")),
                    Amount = k.GetDecimalOrDefault("Amount").ToString("0.00")
                });

                return agentMessage;
            }
            catch (Exception ex)
            {
                throw new MessageCenterException(MessageCenterException.AGENT_MESSAGE_GET_FAILED, ex);
            }
        }

        public List<AgentMessage> GetMessagesByAgentId(long AgentId, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_GetMessagesByAgentId");

                coreTrxProcedure.WithParameters(InputParameter.Named("AgentId").WithValue(AgentId));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now.AddMinutes(-30)));

                var agentMessage = DataHelper.GetConnectionManager().ExecuteQueryWithResults<AgentMessage>(coreTrxProcedure, k => new AgentMessage
                {
                    TransactionId = k.GetInt64OrDefault("TransactionId"),
                    CustomerFirstName = k.GetStringOrDefault("FirstName"),
                    CustomerLastName = k.GetStringOrDefault("LastName"),
                    TicketNumber = k.GetStringOrDefault("TicketId"),
                    TransactionState = Enum.GetName(typeof(CheckStatus), k.GetInt32OrDefault("State")),
                    Amount = k.GetDecimalOrDefault("Amount").ToString("0.00"),
                    DeclineMessage = k.GetStringOrDefault("Message")
                });

                return agentMessage.ToList();
            }
            catch (Exception ex)
            {
                throw new MessageCenterException(MessageCenterException.AGENT_MESSAGE_GET_FAILED, ex);
            }
        }

        public void DeleteAllMessages(ZeoContext context)
        {
            try
            {
                DataTable table = GetAllActiveMessages();
                if (table != null && table.HasRows())
                {
                    StoredProcedure coreTrxProcedure = new StoredProcedure("usp_DeleteAllActiveMessages");
                    StringWriter writer = new StringWriter();

                    table.TableName = "ActiveMessages";
                    table.WriteXml(writer);

                    DataParameter[] dataParameters = new DataParameter[]
                    {
                    new DataParameter("ActiveMessages", DbType.Xml)
                    {
                        Value =  writer.ToString()
                    }
                    };

                    coreTrxProcedure.WithParameters(dataParameters);
                    DataHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);
                }
            }
            catch (Exception ex)
            {
                throw new MessageCenterException(MessageCenterException.AGENT_MESSAGE_DELETE_FAILED, ex);
            }
        }

        private DataTable GetAllActiveMessages()
        {
            StoredProcedure coreTrxProcedure = new StoredProcedure("usp_GetAllActiveMessages");

            DataTable table = new DataTable();
            table.Columns.Add("TransactionId", typeof(long));
            table.Columns.Add("DTTerminalLastModified", typeof(DateTime));
            table.Columns.Add("DTServerLastModified", typeof(DateTime));

            DataRow dr;
            using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
            {
                while (datareader.Read())
                {
                    dr = table.NewRow();
                    dr["TransactionId"] = datareader.GetInt64OrDefault("TransactionId");
                    dr["DTTerminalLastModified"] = Helper.GetTimeZoneTime(datareader.GetStringOrDefault("LocationTimeZone"));
                    dr["DTServerLastModified"] = DateTime.Now;
                    table.Rows.Add(dr);
                }
            }

            return table;
        }
    }
}
