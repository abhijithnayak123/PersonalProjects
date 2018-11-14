using System;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using P3Net.Data.Common;
using System.Data;
using System.Collections.Generic;
using TCF.Zeo.Common.Data;
using P3Net.Data;

namespace TCF.Zeo.Core.Impl
{
    public class ComplianceServiceImpl : IComplianceService
    {
        public decimal GetComplianceTransactionTotal(long customerSessionId, Helper.TransactionType transactionType, bool shouldIncludeShoppingCartItems, int period, ZeoContext context)
        {
            try
            {
                decimal xDayTrxsTotalAmount = decimal.Zero;
                StoredProcedure coreCompliance = new StoredProcedure("usp_GetComplianceTransactions");

                coreCompliance.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));
                coreCompliance.WithParameters(InputParameter.Named("transactionType").WithValue(Convert.ToInt32(transactionType)));
                coreCompliance.WithParameters(InputParameter.Named("shouldIncludeShoppingCartItems").WithValue(shouldIncludeShoppingCartItems));
                coreCompliance.WithParameters(InputParameter.Named("period").WithValue(Convert.ToInt32(period)));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCompliance))
                {
                    while (datareader.Read())
                    {
                        xDayTrxsTotalAmount = datareader.GetDecimalOrDefault("xDayTrxsTotalAmount");
                    }
                }

                return xDayTrxsTotalAmount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Limit GetTransactionLimit(Helper.TransactionType transactionType, long channelPartnerId, ZeoContext context)
        {
            try
            {
                Limit limit = new Limit();

                StoredProcedure coreCompliance = new StoredProcedure("usp_GetTransactionLimit");

                coreCompliance.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));
                coreCompliance.WithParameters(InputParameter.Named("transactionType").WithValue(transactionType.ToString()));

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(coreCompliance))
                {
                    while (datareader.Read())
                    {
                        limit.PerTransactionMaximum = datareader.GetDecimalOrDefault("PerTransactionMaximum");
                        limit.PerTransactionMinimum = datareader.GetDecimalOrDefault("PerTransactionMinimum");
                        limit.RollingLimits = datareader.GetStringOrDefault("RollingLimits");
                    }
                }

                return limit;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
