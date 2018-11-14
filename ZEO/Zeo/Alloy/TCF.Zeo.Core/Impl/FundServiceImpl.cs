using System;
using System.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Contract;
using P3Net.Data.Common;
using P3Net.Data;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : IFundService
    {
        public void CommitFundTransaction(long transactionId, int status, long customerSessionId, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_CommitFundTransaction");

                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("status").WithValue(status));
                coreTrxProcedure.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(Helper.GetTimeZoneTime(context.TimeZone)));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);
            }
            catch (Exception ex)
            {
                throw new FundsException(FundsException.TRANSACTION_COMMIT_FAILED, ex);
            }

        }

        public long CreateFundTransaction(Funds funds, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_AddFundsTransaction");

                coreTrxProcedure.WithParameters(OutputParameter.Named("transactionId").OfType<long>());
                coreTrxProcedure.WithParameters(InputParameter.Named("Amount").WithValue(funds.Amount));
                coreTrxProcedure.WithParameters(InputParameter.Named("BaseFee").WithValue(funds.BaseFee));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountApplied").WithValue(funds.DiscountApplied));
                coreTrxProcedure.WithParameters(InputParameter.Named("AdditionalFee").WithValue(funds.AdditionalFee));
                coreTrxProcedure.WithParameters(InputParameter.Named("Fee").WithValue(funds.Fee));
                coreTrxProcedure.WithParameters(InputParameter.Named("fundType").WithValue((int)funds.FundsType));
                coreTrxProcedure.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(funds.CustomerSessionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("addOnCustomerId").WithValue(funds.AddOnCustomerId));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(funds.DTServerCreate));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(funds.DTTerminalCreate));
                coreTrxProcedure.WithParameters(InputParameter.Named("State").WithValue(funds.State));
                coreTrxProcedure.WithParameters(InputParameter.Named("ProviderId").WithValue(funds.ProviderId));
                coreTrxProcedure.WithParameters(InputParameter.Named("ConfirmationNumber").WithValue(funds.ConfirmationNumber));
                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);

                return Convert.ToInt64(coreTrxProcedure.Parameters["transactionId"].Value);
            }
            catch (Exception ex)
            {
                throw new FundsException(FundsException.TRANSACTION_CREATE_FAILED, ex);
            }
        }

        public Funds GetFundTransaction(long transactionId, ZeoContext context)
        {
            try
            {
                Funds fundTrx = new Funds();

                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_GetFundTransactionById");

                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (datareader.Read())
                    {
                        fundTrx.ProviderId = datareader.GetInt32OrDefault("ProviderId");
                        fundTrx.ProviderAccountId = datareader.GetInt64OrDefault("ProviderAccountId");
                        fundTrx.Amount = datareader.GetDecimalOrDefault("Amount");
                        fundTrx.Fee = datareader.GetDecimalOrDefault("Fee");
                        fundTrx.Description = datareader.GetStringOrDefault("Description");
                        fundTrx.State = datareader.GetInt32OrDefault("State");
                        fundTrx.ConfirmationNumber = datareader.GetStringOrDefault("ConfirmationNumber");
                        fundTrx.FundsType = (Helper.FundType)datareader.GetInt32OrDefault("FundType");
                        fundTrx.BaseFee = datareader.GetDecimalOrDefault("BaseFee");
                        fundTrx.AdditionalFee = datareader.GetDecimalOrDefault("AdditionalFee");
                        fundTrx.DiscountName = datareader.GetStringOrDefault("PromoCode");
                        fundTrx.IsSystemApplied = datareader.GetBooleanOrDefault("IsSystemApplied");
                        fundTrx.AddOnCustomerId = datareader.GetInt64OrDefault("AddOnCustomerId");
                        fundTrx.Id = transactionId;
                    }
                }

                return fundTrx;
            }
            catch (Exception ex)
            {
                throw new FundsException(FundsException.TRANSACTION_GET_FAILED, ex);
            }
        }

        public bool UpdateFundTransaction(Funds funds, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_UpdateFundTransactionById");

                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(funds.Id));
                coreTrxProcedure.WithParameters(InputParameter.Named("Amount").WithValue(funds.Amount));
                coreTrxProcedure.WithParameters(InputParameter.Named("Fee").WithValue(funds.Fee));
                coreTrxProcedure.WithParameters(InputParameter.Named("Description").WithValue(funds.Description));
                coreTrxProcedure.WithParameters(InputParameter.Named("State").WithValue(funds.State));
                coreTrxProcedure.WithParameters(InputParameter.Named("ConfirmationNumber").WithValue(funds.ConfirmationNumber));
                coreTrxProcedure.WithParameters(InputParameter.Named("BaseFee").WithValue(funds.BaseFee));
                coreTrxProcedure.WithParameters(InputParameter.Named("AdditionalFee").WithValue(funds.AdditionalFee));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountName").WithValue(funds.DiscountName));
                coreTrxProcedure.WithParameters(InputParameter.Named("IsSystemApplied").WithValue(funds.IsSystemApplied));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountApplied").WithValue(funds.DiscountApplied));
                coreTrxProcedure.WithParameters(InputParameter.Named("fundsTypeId").WithValue(Convert.ToInt32(funds.FundsType)));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(funds.DTTerminalLastModified));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(funds.DTServerLastModified));

                int trxCount = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);

                return trxCount > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw new FundsException(FundsException.TRANSACTION_UPDATE_FAILED, ex);
            }
        }

        public void UpdateFundTransactionAmount(long transactionId, decimal amount, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_UpdateFundTransactionAmount");
                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("amount").WithValue(amount));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(Helper.GetTimeZoneTime(context.TimeZone)));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);
            }
            catch (Exception ex)
            {
                throw new FundsException(FundsException.TRANSACTION_UPDATE_AMOUNT_FAILED, ex);
            }
        }

        public void UpdateFundTransactionState(long transactionId, Helper.TransactionStates transactionStates, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_UpdateFundTransactionState");
                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("State").WithValue((int)transactionStates));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(Helper.GetTimeZoneTime(context.TimeZone)));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);
            }
            catch (Exception ex)
            {
                throw new FundsException(FundsException.TRANSACTION_UPDATE_FAILED, ex);
            }
        }

        public long GetCXNTransactionId(long transactionId, ZeoContext context)
        {
            try
            {
                long cxnTransactionId = 0;
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_GetFundsCXNTransactionId");
                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (datareader.Read())
                    {
                        cxnTransactionId = datareader.GetInt64OrDefault("CXNId");
                    }
                }
                return cxnTransactionId;
            }
            catch (Exception ex)
            {
                throw new FundsException(FundsException.TRANSACTION_GET_FAILED, ex);
            }
        }


        public void UpdateCXNTransctionId(long transactionId, long cxnTransactionId, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_UpdateFundsCXNTransactionId");
                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("cxnId").WithValue(cxnTransactionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(Helper.GetTimeZoneTime(context.TimeZone)));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);
            }
            catch (Exception ex)
            {
                throw new FundsException(FundsException.TRANSACTION_GET_FAILED, ex);
            }
        }
    }
}