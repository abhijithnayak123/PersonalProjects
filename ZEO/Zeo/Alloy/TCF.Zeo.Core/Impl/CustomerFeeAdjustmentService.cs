using System;
using System.Data;
using System.Collections.Generic;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Contract;
using static TCF.Zeo.Common.Util.Helper;
using P3Net.Data;
using P3Net.Data.Common;
using TCF.Zeo.Common.Data;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : ICustomerFeeAdjustmentService
    {
        public List<FeeAdjustment> LookupCustomerFeeAdjustments(TransactionType transactionType, long customerId, ZeoContext context)
        {
            try
            {
                List<FeeAdjustment> feeAdjustments = new List<FeeAdjustment>();
                FeeAdjustment feeAdjustment;

                StoredProcedure coreCustomerFeeAdjProcedure = new StoredProcedure("usp_GetCutomerFeeAdjustments");

                coreCustomerFeeAdjProcedure.WithParameters(InputParameter.Named("CustomerId").WithValue(customerId));
                coreCustomerFeeAdjProcedure.WithParameters(InputParameter.Named("TransactionType").WithValue((int)transactionType));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreCustomerFeeAdjProcedure))
                {
                    while (datareader.Read())
                    {
                        feeAdjustment = new FeeAdjustment();
                        feeAdjustment.Id = datareader.GetInt64OrDefault("FeeAdjustmentId");
                        feeAdjustment.AdjustmentAmount = datareader.GetDecimalOrDefault("AdjustmentAmount");
                        feeAdjustment.Name = datareader.GetStringOrDefault("Name");
                        feeAdjustment.Description = datareader.GetStringOrDefault("Description");
                        feeAdjustment.AdjustmentRate = datareader.GetDecimalOrDefault("AdjustmentRate");
                        //feeAdjustment.CompareType = datareader.GetInt32OrDefault("CompareTypePK");
                        //feeAdjustment.ConditionType = datareader.GetInt32OrDefault("ConditionTypePK");
                        //feeAdjustment.ConditionValue = datareader.GetStringOrDefault("ConditionValue");
                        feeAdjustment.SystemApplied = datareader.GetBooleanOrDefault("SystemApplied");
                        feeAdjustment.PromotionType = datareader.GetStringOrDefault("PromotionType");
                        feeAdjustments.Add(feeAdjustment);
                    }
                }

                return feeAdjustments;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
