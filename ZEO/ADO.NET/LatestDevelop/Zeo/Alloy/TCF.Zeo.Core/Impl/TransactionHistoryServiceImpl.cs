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
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public class TransactionHistoryServiceImpl : ITransactionHistoryService
    {
        public List<TransactionHistory> GetAgentTransactionHistory(TransactionSearchCriteria searchCriteria, ZeoContext context)
        {
            try
            {
                List<TransactionHistory> txnHistory = new List<TransactionHistory>();
                TransactionHistory txn;

                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_GetAgentTransactionHistoryBySearchCriteria");

                coreTrxProcedure.WithParameters(InputParameter.Named("agentId").WithValue(searchCriteria.AgentId));
                coreTrxProcedure.WithParameters(InputParameter.Named("dateRange").WithValue(searchCriteria.DatePeriod));
                coreTrxProcedure.WithParameters(InputParameter.Named("location").WithValue(searchCriteria.LocationName));
                coreTrxProcedure.WithParameters(InputParameter.Named("showAll").WithValue(searchCriteria.ShowAll));
                coreTrxProcedure.WithParameters(InputParameter.Named("transactionType").WithValue(searchCriteria.TransactionType));
                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(searchCriteria.TransactionId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (datareader.Read())
                    {
                        txn = new TransactionHistory();
                        txn.TransactionId = datareader.GetInt64OrDefault("TransactionId");
                        txn.TotalAmount = datareader.GetDecimalOrDefault("TotalAmount");
                        txn.TransactionDetail = datareader.GetStringOrDefault("TransactionDetail");
                        txn.CustomerId = datareader.GetInt64OrDefault("CustomerId");
                        txn.CustomerName = datareader.GetStringOrDefault("CustomerName");
                        txn.Location = datareader.GetStringOrDefault("Location");
                        txn.SessionID = datareader.GetInt64OrDefault("SessionID");
                        txn.Teller = datareader.GetStringOrDefault("Teller");
                        txn.TellerId = datareader.GetInt64OrDefault("TellerId");
                        txn.TransactionDate = datareader.GetDateTimeOrDefault("TransactionDate");
                        txn.TransactionStatus = datareader.GetStringOrDefault("TransactionStatus");
                        txn.TransactionType = datareader.GetStringOrDefault("TransactionType");
                        txnHistory.Add(txn);
                    }
                }

                return txnHistory;
            }
            catch (Exception ex)
            {
                throw new TransactionHistoryException(TransactionHistoryException.GET_AGENT_TRANSACTION_FAILED, ex);
            }
        }

        public List<TransactionHistory> GetCustomerTransactionHistory(TransactionSearchCriteria searchCriteria, ZeoContext context)
        {
            try
            {
                List<TransactionHistory> txnHistory = new List<TransactionHistory>();
                TransactionHistory txn;

                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_GetCustomerTransactionHistoryBySearchCriteria");

                coreTrxProcedure.WithParameters(InputParameter.Named("customerId").WithValue(searchCriteria.CustomerId));
                coreTrxProcedure.WithParameters(InputParameter.Named("dateRange").WithValue(searchCriteria.DatePeriod));
                coreTrxProcedure.WithParameters(InputParameter.Named("location").WithValue(searchCriteria.LocationName));
                coreTrxProcedure.WithParameters(InputParameter.Named("transactionType").WithValue(searchCriteria.TransactionType));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (datareader.Read())
                    {
                        txn = new TransactionHistory();
                        txn.TransactionId = datareader.GetInt64OrDefault("TransactionId");
                        txn.TotalAmount = datareader.GetDecimalOrDefault("TotalAmount");
                        txn.TransactionDetail = datareader.GetStringOrDefault("TransactionDetail");
                        txn.CustomerId = datareader.GetInt64OrDefault("CustomerId");
                        txn.CustomerName = datareader.GetStringOrDefault("CustomerName");
                        txn.Location = datareader.GetStringOrDefault("Location");
                        txn.SessionID = datareader.GetInt64OrDefault("SessionID");
                        txn.Teller = datareader.GetStringOrDefault("Teller");
                        txn.TellerId = datareader.GetInt64OrDefault("TellerId");
                        txn.TransactionDate = datareader.GetDateTimeOrDefault("TransactionDate");
                        txn.TransactionStatus = datareader.GetStringOrDefault("TransactionStatus");
                        txn.TransactionType = datareader.GetStringOrDefault("TransactionType");
                        txnHistory.Add(txn);
                    }
                }

                return txnHistory;
            }
            catch (Exception ex)
            {
                throw new TransactionHistoryException(TransactionHistoryException.GET_CUSTOMER_TRANSACTION_FAILED, ex);
            }
        }

        public List<string> GetCustomerTransactionLocation(DateTime dateRange, ZeoContext context)
        {
            try
            {
                List<string> locations = new List<string>();

                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_GetCustomerTransactionLocations");

                coreTrxProcedure.WithParameters(InputParameter.Named("customerId").WithValue(context.CustomerId));
                coreTrxProcedure.WithParameters(InputParameter.Named("dateRange").WithValue(dateRange));
                coreTrxProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (datareader.Read())
                    {
                        locations.Add(datareader.GetStringOrDefault("location"));
                    }
                }

                return locations;
            }
            catch (Exception ex)
            {
                throw new TransactionHistoryException(TransactionHistoryException.GET_CUSTOMER_TRANSACTION_LOCATIONS_FAILED, ex);
            }
        }

        public long GetCustomerSessionId(long transactionId, int transactionType, ZeoContext context)
        {
            long customerSessionId = 0;
            try
            {
                StoredProcedure spGetCustomerSessionId = new StoredProcedure("usp_GetCustomerSessionIdByTransactionId");
                spGetCustomerSessionId.WithParameters(InputParameter.Named("TransactionId").WithValue(transactionId));
                spGetCustomerSessionId.WithParameters(InputParameter.Named("TransactionType").WithValue(transactionType));
                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(spGetCustomerSessionId))
                {
                    while (datareader.Read())
                    {
                        customerSessionId = datareader.GetInt64OrDefault("CustomerSessionId");
                    }
                }

                return customerSessionId;
            }

            catch (Exception ex)
            {
                throw new TransactionHistoryException(TransactionHistoryException.GET_CUSTOMER_TRANSACTION_FAILED, ex);
            }
        }
    }
}
