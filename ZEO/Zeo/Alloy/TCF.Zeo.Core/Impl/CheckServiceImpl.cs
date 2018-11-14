using System;
using System.Collections.Generic;
using System.Linq;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Contract;
using P3Net.Data;
using P3Net.Data.Common;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data.Exceptions;
using System.Data;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : ICheckService
    {
        public long CreateCheckTransaction(Data.Check check, Data.CheckImages checkImage, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_CreateCheckTransaction");

                coreTrxProcedure.WithParameters(OutputParameter.Named("transactionId").OfType<long>());
                coreTrxProcedure.WithParameters(InputParameter.Named("Amount").WithValue(check.Amount));
                coreTrxProcedure.WithParameters(InputParameter.Named("Fee").WithValue(check.Fee));
                coreTrxProcedure.WithParameters(InputParameter.Named("Description").WithValue(check.Description));
                coreTrxProcedure.WithParameters(InputParameter.Named("State").WithValue(check.State));
                coreTrxProcedure.WithParameters(InputParameter.Named("ConfirmationNumber").WithValue(check.ConfirmationNumber));
                coreTrxProcedure.WithParameters(InputParameter.Named("BaseFee").WithValue(check.BaseFee));
                coreTrxProcedure.WithParameters(InputParameter.Named("AdditionalFee").WithValue(check.AdditionalFee));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountName").WithValue(check.DiscountName));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountDescription").WithValue(check.DiscountDescription));
                coreTrxProcedure.WithParameters(InputParameter.Named("IsSystemApplied").WithValue(check.IsSystemApplied));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountApplied").WithValue(check.DiscountApplied));
                coreTrxProcedure.WithParameters(InputParameter.Named("CheckType").WithValue(check.CheckType));
                coreTrxProcedure.WithParameters(InputParameter.Named("BackImage").WithValue(checkImage.Back));
                coreTrxProcedure.WithParameters(InputParameter.Named("FrontImage").WithValue(checkImage.Front));
                coreTrxProcedure.WithParameters(InputParameter.Named("Format").WithValue(checkImage.Format));
                coreTrxProcedure.WithParameters(InputParameter.Named("MICR").WithValue(check.MICR));
                coreTrxProcedure.WithParameters(InputParameter.Named("ProviderAccountId").WithValue(check.ProviderAccountId));
                coreTrxProcedure.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(check.CustomerSessionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("ProviderId").WithValue((int)check.ProductProviderCode));
                coreTrxProcedure.WithParameters(InputParameter.Named("initiatedProviderId").WithValue((int)check.ProductProviderCode));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(check.DTServerCreate));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(check.DTTerminalCreate));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);

                return Convert.ToInt64(coreTrxProcedure.Parameters["transactionId"].Value);
            }
            catch (Exception ex)
            {
                throw new CheckException(CheckException.CREATE_CHECK_TRANSACTION_FAILED, ex);
            }
        }

        public bool UpdateCheckTransaction(Data.Check check, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_UpdateCheckTransactionById");

                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(check.Id));
                coreTrxProcedure.WithParameters(InputParameter.Named("cxnTransactionId").WithValue(check.CxnTransactionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("Amount").WithValue(check.Amount));
                coreTrxProcedure.WithParameters(InputParameter.Named("Fee").WithValue(check.Fee));
                coreTrxProcedure.WithParameters(InputParameter.Named("Description").WithValue(check.Description));
                coreTrxProcedure.WithParameters(InputParameter.Named("ShoppingCartdescription").WithValue(check.ShoppingCartDescription));
                coreTrxProcedure.WithParameters(InputParameter.Named("State").WithValue(check.State));
                coreTrxProcedure.WithParameters(InputParameter.Named("ConfirmationNumber").WithValue(check.ConfirmationNumber));
                coreTrxProcedure.WithParameters(InputParameter.Named("BaseFee").WithValue(check.BaseFee));
                coreTrxProcedure.WithParameters(InputParameter.Named("AdditionalFee").WithValue(check.AdditionalFee));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountName").WithValue(check.DiscountName));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountDescription").WithValue(check.DiscountDescription));
                coreTrxProcedure.WithParameters(InputParameter.Named("IsSystemApplied").WithValue(check.IsSystemApplied));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountApplied").WithValue(check.DiscountApplied));
                coreTrxProcedure.WithParameters(InputParameter.Named("IsPendingCheckApprovedOrDeclined").WithValue(check.IsPendingCheckApprovedOrDeclined));
                coreTrxProcedure.WithParameters(InputParameter.Named("CheckTypeId").WithValue(Convert.ToInt32(check.CheckType)));
                coreTrxProcedure.WithParameters(InputParameter.Named("ProviderId").WithValue((int)check.ProductProviderCode));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(check.DTTerminalLastModified));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(check.DTServerLastModified));

                int trxCount = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);

                return trxCount > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw new CheckException(CheckException.UPDATE_CHECK_TRANSACTION_FAILED, ex);
            }
        }

        public Data.Check GetCheckTransaction(long transactionId, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_GetCheckTransactionById");

                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));

                Data.Check checkTrx = DataConnectionHelper.GetConnectionManager().ExecuteQueryWithResult<Data.Check>(coreTrxProcedure, r => new Data.Check
                {
                    AdditionalFee = r.GetDecimalOrDefault("AdditionalFee"),
                    Amount = r.GetDecimalOrDefault("Amount"),
                    BaseFee = r.GetDecimalOrDefault("BaseFee"),
                    CheckType = r.GetStringOrDefault("CheckType"),
                    ConfirmationNumber = r.GetStringOrDefault("ConfirmationNumber"),
                    Description = r.GetStringOrDefault("Description"),
                    ShoppingCartDescription = r.GetStringOrDefault("ShoppingCartDescription"),
                    DiscountApplied = r.GetDecimalOrDefault("DiscountApplied"),
                    DiscountDescription = r.GetStringOrDefault("DiscountDescription"),
                    DiscountName = r.GetStringOrDefault("DiscountName"),
                    Fee = r.GetDecimalOrDefault("Fee"),
                    IsSystemApplied = r.GetBooleanOrDefault("IsSystemApplied"),
                    MICR = r.GetStringOrDefault("MICR"),
                    ProviderAccountId = r.GetInt64OrDefault("ProviderAccountId"),
                    ProductProviderCode = (Helper.ProviderId)r.GetInt32OrDefault("ProviderId"),
                    State = r.GetInt32OrDefault("State"),
                    FrankData = r.GetStringOrDefault("FrankData"),
                    Id = transactionId,
                    CxnTransactionId = r.GetInt64OrDefault("CxnTransactionId")
                });

                return checkTrx;
            }
            catch (Exception ex)
            {
                throw new CheckException(CheckException.GET_CHECK_TRANSACTION_FAILED, ex);
            }
        }

        public void CommitTransaction(long transactionId, int status, long customerId, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_CommitCheckTransaction");

                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));
                coreTrxProcedure.WithParameters(InputParameter.Named("state").WithValue(status));
                coreTrxProcedure.WithParameters(InputParameter.Named("CustomerId").WithValue(customerId));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(Helper.GetTimeZoneTime(timeZone)));

                // Update the check transaction status
                // delete message center data if the check is pending
                // call Add or update FeeAdjustment for the customer

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);
            }
            catch (Exception ex)
            {
                throw new CheckException(CheckException.SUBMIT_CHECK_TRANSACTION_FAILED, ex);
            }
        }

        public List<CheckType> GetCheckTypes(ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_GetCheckTypes");
                var checkTypes = DataConnectionHelper.GetConnectionManager().ExecuteQueryWithResults<CheckType>(coreTrxProcedure, r => new CheckType
                {
                    Name = r.GetStringOrDefault("Name"),
                    Id = r.GetInt32OrDefault("Id"),
                    ProductProviderCode = (Helper.ProviderId)r.GetInt64OrDefault("ProductProviderCode")
                });

                return checkTypes.ToList();
            }
            catch (Exception ex)
            {
                throw new CheckException(CheckException.GET_CHECK_TYPE_FAILED, ex);
            }
        }

        public void UpdateCheckTransactionState(long transactionId, Helper.TransactionStates transactionStates, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_UpdateCheckTransactionState");

                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("State").WithValue((int)transactionStates));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(Helper.GetTimeZoneTime(timeZone)));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);
            }
            catch (Exception ex)
            {
                throw new CheckException(CheckException.UPDATE_CHECK_TRANSACTION_FAILED, ex);
            }

        }

        public bool CancelCheckTransaction(long transactionId, Helper.TransactionStates transactionStates, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_CancelCheckTransactionById");

                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("State").WithValue((int)transactionStates));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(Helper.GetTimeZoneTime(timeZone)));

                int trxCount = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);
                return trxCount > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw new CheckException(CheckException.CANCEL_CHECK_TRANSACTION_FAILED, ex);
            }

        }

        public long GetCheckCxnTransactionId(long transactionId)
        {
            try
            {
                long cxnTransactionId = 0;
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_GetCheckCxnTransactionId");

                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));

                using (IDataReader dr = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (dr.Read())
                    {
                        cxnTransactionId = dr.GetInt64OrDefault("CxnTransactionId");
                    }
                };

                return cxnTransactionId;
            }
            catch (Exception ex)
            {
                throw new CheckException(CheckException.GET_CHECK_TRANSACTION_FAILED, ex);
            }
        }

        public CheckProviderDetails GetCheckProvider(MICRDetails micrDetails, ZeoContext context)
        {
            try
            {
                CheckProviderDetails checkProviderDetails = new CheckProviderDetails();

                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_CheckIsOnusProvider");

                coreTrxProcedure.WithParameters(InputParameter.Named("AccountNumber").WithValue(micrDetails?.AccountNumber));
                coreTrxProcedure.WithParameters(InputParameter.Named("RoutingNumber").WithValue(micrDetails?.RoutingNumber));

                using (IDataReader dr = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (dr.Read())
                    {
                        checkProviderDetails.CheckTypeId = dr.GetInt32OrDefault("CheckTypeId");
                        checkProviderDetails.ProductProviderCode = (Helper.ProviderId)dr.GetInt64OrDefault("ProductProviderCode");
                    }
                };

                return checkProviderDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
