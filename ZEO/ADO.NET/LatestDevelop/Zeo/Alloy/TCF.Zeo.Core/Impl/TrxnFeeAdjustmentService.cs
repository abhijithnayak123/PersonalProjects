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
                
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("FeeAdjustmentId").WithValue(trxFeeAdjustment.FeeAdjustmentId));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("IsActive").WithValue(trxFeeAdjustment.IsActive));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("TransactionId").WithValue(trxFeeAdjustment.TransactionId));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(trxFeeAdjustment.DTTerminalCreate));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(trxFeeAdjustment.DTServerCreate));

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
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("feeAdjustmentId").WithValue(trxFeeAdjustment.FeeAdjustmentId));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("transactionType").WithValue((int)trxFeeAdjustment.TransactionType));
                trxnFeeAdjustmentProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));

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
