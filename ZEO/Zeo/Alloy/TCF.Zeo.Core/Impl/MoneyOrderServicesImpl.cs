using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Data;
using P3Net.Data;
using P3Net.Data.Common;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : IMoneyOrderService
    {
        public long CreateMoneyOrderTransaction(MoneyOrder moneyOrderTransaction, ZeoContext context)
        {
            long transactionId = 0;
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_CreateMoneyOrderTransaction");

                coreTrxProcedure.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(moneyOrderTransaction.CustomerSessionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("Amount").WithValue(moneyOrderTransaction.Amount));
                coreTrxProcedure.WithParameters(InputParameter.Named("Fee").WithValue(moneyOrderTransaction.Fee));
                coreTrxProcedure.WithParameters(InputParameter.Named("State").WithValue(moneyOrderTransaction.State));
                coreTrxProcedure.WithParameters(InputParameter.Named("BaseFee").WithValue(moneyOrderTransaction.BaseFee));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountApplied").WithValue(moneyOrderTransaction.DiscountApplied));
                coreTrxProcedure.WithParameters(InputParameter.Named("AdditionalFee").WithValue(moneyOrderTransaction.AdditionalFee));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountName").WithValue(moneyOrderTransaction.DiscountName));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountDescription").WithValue(moneyOrderTransaction.DiscountDescription));
                coreTrxProcedure.WithParameters(InputParameter.Named("IsSystemApplied").WithValue(moneyOrderTransaction.IsSystemApplied));
                coreTrxProcedure.WithParameters(InputParameter.Named("PurchaseDate").WithValue(moneyOrderTransaction.PurchaseDate));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(moneyOrderTransaction.DTTerminalCreate));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(DateTime.Now));

                using (IDataReader reader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (reader.Read())
                    {
                        transactionId = reader.GetInt64OrDefault("transactionId");
                    }
                }

                return transactionId;
            }
            catch(Exception ex)
            {
                throw new MoneyorderException(MoneyorderException.MONEYORDER_CREATE_FAILED, ex);
            }
        }

        public MoneyOrder GetMoneyOrderTransactionById(long transactionId, ZeoContext context)
        {
            try
            {
                MoneyOrder moneyOrder = new MoneyOrder();

                StoredProcedure moneyOrderTransaction = new StoredProcedure("usp_GetMoneyOrderTransactionById");

                moneyOrderTransaction.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyOrderTransaction))
                {
                    while (datareader.Read())
                    {
                        moneyOrder.TransactionId = moneyOrder.Id = transactionId;
                        moneyOrder.Amount = datareader.GetDecimalOrDefault("Amount");
                        moneyOrder.Fee = datareader.GetDecimalOrDefault("Fee");
                        moneyOrder.Description = datareader.GetStringOrDefault("Description");
                        moneyOrder.State = datareader.GetInt32OrDefault("State");
                        moneyOrder.BaseFee = datareader.GetDecimalOrDefault("BaseFee");
                        moneyOrder.DiscountApplied = datareader.GetDecimalOrDefault("DiscountApplied");
                        moneyOrder.AdditionalFee = datareader.GetDecimalOrDefault("AdditionalFee");
                        moneyOrder.DiscountName = datareader.GetStringOrDefault("DiscountName");
                        moneyOrder.DiscountDescription = datareader.GetStringOrDefault("DiscountDescription");
                        moneyOrder.IsSystemApplied = datareader.GetBooleanOrDefault("IsSystemApplied");
                        moneyOrder.CheckNumber = datareader.GetStringOrDefault("CheckNumber");
                        moneyOrder.AccountNumber = datareader.GetStringOrDefault("AccountNumber");
                        moneyOrder.RoutingNumber = datareader.GetStringOrDefault("RoutingNumber");
                        moneyOrder.MICR = datareader.GetStringOrDefault("MICR");
                        moneyOrder.CustomerSessionId = datareader.GetInt64OrDefault("CustomerSessionId");
                        moneyOrder.PurchaseDate = datareader.GetDateTimeOrDefault("PurchaseDate");
                        moneyOrder.DTTerminalCreate = datareader.GetDateTimeOrDefault("DTTerminalCreate");
                        moneyOrder.DTTerminalLastModified = datareader.GetDateTimeOrDefault("DTTerminalLastModified");
                    }
                }

                return moneyOrder;
            }
            catch(Exception ex)
            {
                throw new MoneyorderException(MoneyorderException.MONEYORDER_GET_FAILED, ex);
            }
        }

        public bool UpdateMoneyOrderState(long transactionId,long customerId, int state, DateTime dtTerminalModified, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_UpdateMoneyOrderStatus");

                coreTrxProcedure.WithParameters(InputParameter.Named("CustomerId").WithValue(customerId));
                coreTrxProcedure.WithParameters(InputParameter.Named("TransactionId").WithValue(transactionId));
                coreTrxProcedure.WithParameters(InputParameter.Named("State").WithValue(state));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(dtTerminalModified));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(DateTime.Now));
                coreTrxProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(context.ChannelPartnerId));

                int val = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);
                return (val == 0) ? false : true;
            }
            catch(Exception ex)
            {
                throw new MoneyorderException(MoneyorderException.MONEYORDER_STATUS_UPDATE_FAILED, ex);
            }
        }

        public bool UpdateMoneyOrderTransaction(MoneyOrder moneyOrderTransaction, MoneyOrderImage moneyOrderImage,bool AllowDuplicateMoneyOrder, string timeZone, ZeoContext context)
        {
            bool isUpdated = false;
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_UpdateMoneyOrderTransaction");
                
                coreTrxProcedure.WithParameters(InputParameter.Named("TransactionId").WithValue(moneyOrderTransaction.Id));
                coreTrxProcedure.WithParameters(InputParameter.Named("MICR").WithValue(moneyOrderTransaction.MICR));
                coreTrxProcedure.WithParameters(InputParameter.Named("CheckNumber").WithValue(moneyOrderTransaction.CheckNumber));
                coreTrxProcedure.WithParameters(InputParameter.Named("AccountNumber").WithValue(moneyOrderTransaction.AccountNumber));
                coreTrxProcedure.WithParameters(InputParameter.Named("RoutingNumber").WithValue(moneyOrderTransaction.RoutingNumber));
                coreTrxProcedure.WithParameters(InputParameter.Named("CheckFrontImage").WithValue(moneyOrderImage.CheckFrontImage));
                coreTrxProcedure.WithParameters(InputParameter.Named("CheckBackImage").WithValue(moneyOrderImage.CheckBackImage));
                coreTrxProcedure.WithParameters(InputParameter.Named("AllowDuplicateMoneyOrder").WithValue(AllowDuplicateMoneyOrder));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalDate").WithValue(Helper.GetTimeZoneTime(timeZone)));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerDate").WithValue(DateTime.Now));

                using (IDataReader reader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (reader.Read())
                    {
                        isUpdated = reader.GetBooleanOrDefault("isUpdated");
                    }
                }

                return isUpdated;
            }
            catch(Exception ex)
            {
                throw new MoneyorderException(MoneyorderException.MONEYORDER_UPDATE_FAILED, ex);
            }
        }

        public bool UpdateMoneyOrderFee(Core.Data.MoneyOrder moneyOrder, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_UpdateMoneyOrderFee");

                coreTrxProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(moneyOrder.Id));
                coreTrxProcedure.WithParameters(InputParameter.Named("Amount").WithValue(moneyOrder.Amount));
                coreTrxProcedure.WithParameters(InputParameter.Named("Fee").WithValue(moneyOrder.Fee));
                coreTrxProcedure.WithParameters(InputParameter.Named("BaseFee").WithValue(moneyOrder.BaseFee));
                coreTrxProcedure.WithParameters(InputParameter.Named("AdditionalFee").WithValue(moneyOrder.AdditionalFee));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountName").WithValue(moneyOrder.DiscountName));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountDescription").WithValue(moneyOrder.DiscountDescription));
                coreTrxProcedure.WithParameters(InputParameter.Named("IsSystemApplied").WithValue(moneyOrder.IsSystemApplied));
                coreTrxProcedure.WithParameters(InputParameter.Named("DiscountApplied").WithValue(moneyOrder.DiscountApplied));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(moneyOrder.DTTerminalLastModified));
                coreTrxProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(moneyOrder.DTServerLastModified));

                int val = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreTrxProcedure);
                return (val == 0) ? false : true;
            }
            catch(Exception ex)
            {
                throw new MoneyorderException(MoneyorderException.MONEYORDER_FEE_UPDATE_FAILED, ex);
            }
        }
    }
}
