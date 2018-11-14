using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using P3Net.Data.Common;
using TCF.Zeo.Common.Data;
using System;

namespace TCF.Zeo.Core.Impl
{
    public class TrxnFeeAdjustmentService : ITrxnFeeAdjustmentService
    {

        public bool Create(TransactionFeeAdjustment trxFeeAdjustment, ZeoContext context)
        {
            try
            {
                StoredProcedure trxnFeeAdjustmentProcedure = new StoredProcedure("usp_CreateTrxFeeAdjustment");
                
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("PromotionId").WithValue(trxFeeAdjustment.PromotionId));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("IsActive").WithValue(trxFeeAdjustment.IsActive));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("TransactionId").WithValue(trxFeeAdjustment.TransactionId));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(trxFeeAdjustment.DTTerminalCreate));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(trxFeeAdjustment.DTServerCreate));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("CustomerId").WithValue(context.CustomerId));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("TransactionType").WithValue((int)trxFeeAdjustment.TransactionType));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("provisionId").WithValue(trxFeeAdjustment.ProvisionId));

                int count = DataHelper.GetConnectionManager().ExecuteNonQuery(trxnFeeAdjustmentProcedure);

                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(TransactionFeeAdjustment trxFeeAdjustment, ZeoContext context)
        {
            try
            {
                StoredProcedure trxnFeeAdjustmentProcedure = new StoredProcedure("usp_UpdateTrxnFeeAdjustment");

                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("isActive").WithValue(trxFeeAdjustment.IsActive));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(trxFeeAdjustment.TransactionId));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("dTTerminalLastModified").WithValue(trxFeeAdjustment.DTTerminalLastModified));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("dTServerLastModified").WithValue(trxFeeAdjustment.DTServerLastModified));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("promotionId").WithValue(trxFeeAdjustment.PromotionId));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("transactionType").WithValue((int)trxFeeAdjustment.TransactionType));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("customerId").WithValue(context.CustomerId));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("provisionId").WithValue(trxFeeAdjustment.ProvisionId));


                int count = DataHelper.GetConnectionManager().ExecuteNonQuery(trxnFeeAdjustmentProcedure);

                return count > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
