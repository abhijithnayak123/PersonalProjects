using System;
using System.Data;
using System.Collections.Generic;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Contract;
using P3Net.Data;
using P3Net.Data.Common;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data.Exceptions;
using System.Linq;
using static TCF.Zeo.Common.Util.Helper;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : IFeeService
    {
        public List<FeeAdjustment> GetChannelPartnerFeeAdj(Helper.TransactionType transactionType, long channelPartnerId, ZeoContext context)
        {
            try
            {
                List<FeeAdjustment> feeAdjustments = new List<FeeAdjustment>();
                List<Data.FeeAdjustmentConditions> conditionList = new List<Data.FeeAdjustmentConditions>();
                FeeAdjustment feeAdjustment;
                Data.FeeAdjustmentConditions feeAdjustmentCondition;
                StoredProcedure coreChannelPartnerFeeAdjProcedure = new StoredProcedure("usp_GetFeeAdjustments");
                DateTime today = DateTime.Today;
                coreChannelPartnerFeeAdjProcedure.WithParameters(InputParameter.Named("transactionType").WithValue((int)transactionType));
                coreChannelPartnerFeeAdjProcedure.WithParameters(InputParameter.Named("dTStart").WithValue(today));
                coreChannelPartnerFeeAdjProcedure.WithParameters(InputParameter.Named("dTEnd").WithValue(today));
                coreChannelPartnerFeeAdjProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreChannelPartnerFeeAdjProcedure))
                {
                    while(datareader.Read())
                    {
                        feeAdjustmentCondition = new Data.FeeAdjustmentConditions();
                        feeAdjustmentCondition.FeeAdjustmentId = datareader.GetInt64OrDefault("FeeAdjustmentId");
                        feeAdjustmentCondition.CompareType = datareader.GetInt32OrDefault("CompareTypeId");
                        feeAdjustmentCondition.ConditionType = datareader.GetInt32OrDefault("ConditionTypeId");
                        feeAdjustmentCondition.ConditionValue = datareader.GetStringOrDefault("ConditionValue");
                        conditionList.Add(feeAdjustmentCondition);
                    }

                    datareader.NextResult();

                    while (datareader.Read())
                    {
                        feeAdjustment = new FeeAdjustment();
                        feeAdjustment.DTStart = datareader.GetDateTimeOrDefault("DTStart");
                        feeAdjustment.Id = datareader.GetInt64OrDefault("FeeAdjustmentId");
                        feeAdjustment.AdjustmentAmount = datareader.GetDecimalOrDefault("AdjustmentAmount");
                        feeAdjustment.Name = datareader.GetStringOrDefault("Name");
                        feeAdjustment.Description = datareader.GetStringOrDefault("Description");
                        feeAdjustment.AdjustmentRate = datareader.GetDecimalOrDefault("AdjustmentRate");
                        feeAdjustment.SystemApplied = datareader.GetBooleanOrDefault("SystemApplied");
                        feeAdjustment.PromotionType = datareader.GetStringOrDefault("PromotionType");
                        feeAdjustment.Conditions = conditionList.Where(X => X.FeeAdjustmentId == feeAdjustment.Id).ToList();
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

        public bool ValidatePromoCode(Helper.TransactionType transactionType, long channelPartnerId, string promotionCode, ZeoContext context)
        {
            try
            {
                bool isValid = false;

                StoredProcedure coreChannelPartnerFeeAdjProcedure = new StoredProcedure("usp_ValidatePromoCode");
                DateTime today = DateTime.Today;
                coreChannelPartnerFeeAdjProcedure.WithParameters(InputParameter.Named("transactionType").WithValue((int)transactionType));
                coreChannelPartnerFeeAdjProcedure.WithParameters(InputParameter.Named("dTStart").WithValue(today));
                coreChannelPartnerFeeAdjProcedure.WithParameters(InputParameter.Named("dTEnd").WithValue(today));
                coreChannelPartnerFeeAdjProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));
                coreChannelPartnerFeeAdjProcedure.WithParameters(InputParameter.Named("promoCode").WithValue(promotionCode));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreChannelPartnerFeeAdjProcedure))
                {
                    while (datareader.Read())
                    {
                        isValid = datareader.GetBooleanOrDefault("IsValid");
                    }
                }

                return isValid;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TransactionFeeAdjustment> GetAuthorizedTransaction(Helper.TransactionType transactionType, long customerId, ZeoContext context)
        {
            try
            {
                List<TransactionFeeAdjustment> trxnFeeAdjustments = new List<TransactionFeeAdjustment>();
                TransactionFeeAdjustment feeAdjustment;

                string usp_Name = string.Empty;
                if (transactionType == Helper.TransactionType.ProcessCheck)
                {
                    usp_Name = "usp_GetAuthorizedCPTransaction";
                }
                else
                {
                    usp_Name = "usp_GetAuthorizedMOTransaction";
                }
                StoredProcedure coreTrxProcedure = new StoredProcedure(usp_Name);

                coreTrxProcedure.WithParameters(InputParameter.Named("customerId").WithValue(customerId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (datareader.Read())
                    {
                        feeAdjustment = new TransactionFeeAdjustment();
                        feeAdjustment.FeeAdjustmentId = datareader.GetInt64OrDefault("FeeAdjustmentId");
                        feeAdjustment.TransactionId = datareader.GetInt64OrDefault("TransactionId");
                        trxnFeeAdjustments.Add(feeAdjustment);
                    }
                }

                return trxnFeeAdjustments;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int GetTransactionCount(Helper.TransactionType transactionType, long transactionId, long customerId, ZeoContext context)
        {
            try
            {
                int transactionCount = 0;
                string usp_Name = string.Empty;

                if (transactionType == Helper.TransactionType.ProcessCheck)
                {
                    usp_Name = "usp_GetCPTransactionCount";
                }
                else
                {
                    usp_Name = "usp_GetMOTransactionCount";
                }
                StoredProcedure coreTrxProcedure = new StoredProcedure(usp_Name);

                coreTrxProcedure.WithParameters(InputParameter.Named("customerId").WithValue(customerId));
                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (datareader.Read())
                    {
                        transactionCount = datareader.GetInt32OrDefault("TransactionCount");
                    }
                }

                return transactionCount;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public int GetTransactionsCountByPromoCode(ref string promoCode, TransactionType transactionType, long transactionId, bool systemApplied, DateTime dTStart, ZeoContext context)
        {
            try
            {
                int count = 0;

                string usp_Name = "usp_GetCheckTrxCountByPromoCode";

                if (transactionType == TransactionType.MoneyOrder)
                    usp_Name = "usp_GetMOTrxCountByPromoCode";

                StoredProcedure coreTrxProcedure = new StoredProcedure(usp_Name);

                coreTrxProcedure.WithParameters(InputParameter.Named("customerId").WithValue(context.CustomerId));
                coreTrxProcedure.WithParameters(InputParameter.Named("promoCode").WithValue(promoCode));
                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("isSystemApplied").WithValue(systemApplied));
                coreTrxProcedure.WithParameters(InputParameter.Named("productId").WithValue((int)transactionType));
                coreTrxProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(context.CustomerSessionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("dTStart").WithValue(dTStart));

                using (IDataReader dr = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (dr.Read())
                    {
                        count = dr.GetInt32OrDefault("count");
                        promoCode = dr.GetStringOrDefault("PromoDescrition");
                    }
                }

                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
