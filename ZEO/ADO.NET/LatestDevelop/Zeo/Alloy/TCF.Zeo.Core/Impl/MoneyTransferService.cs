using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TCF.Zeo.Core.Data;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Contract;
using P3Net.Data;
using P3Net.Data.Common;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : IMoneyTransferService
    {
        public long CreateTransaction(MoneyTransfer moneyTransfer, ZeoContext context)
        {
            try
            {
                long transactionId = 0;
                bool isExist = false;
                int state = 0;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_CreateMoneyTransferTransaction");

                moneyTransferProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(moneyTransfer.CustomerSessionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("providerAccountId").WithValue(moneyTransfer.ProviderAccountId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("amount").WithValue(moneyTransfer.Amount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("fee").WithValue(moneyTransfer.Fee));
                moneyTransferProcedure.WithParameters(InputParameter.Named("description").WithValue(moneyTransfer.Description));
                moneyTransferProcedure.WithParameters(InputParameter.Named("confirmationNumber").WithValue(moneyTransfer.ConfirmationNumber));
                moneyTransferProcedure.WithParameters(InputParameter.Named("recipientId").WithValue(moneyTransfer.RecipientId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("exchangeRate").WithValue(moneyTransfer.ExchangeRate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("transferType").WithValue((int)moneyTransfer.MoneyTransferType));
                moneyTransferProcedure.WithParameters(InputParameter.Named("transactionSubType").WithValue(Convert.ToString(moneyTransfer.TransactionSubType)));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originalTransactionId").WithValue(moneyTransfer.OriginalTransactionID));
                moneyTransferProcedure.WithParameters(InputParameter.Named("providerId").WithValue(moneyTransfer.ProviderId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("destination").WithValue(moneyTransfer.Destination));
                moneyTransferProcedure.WithParameters(InputParameter.Named("state").WithValue((int)moneyTransfer.State));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerCreate").WithValue(moneyTransfer.DTServerCreate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalCreate").WithValue(moneyTransfer.DTTerminalCreate));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        transactionId = datareader.GetInt64OrDefault("TransactionId");
                        isExist = datareader.GetBooleanOrDefault("IsExist");
                        state = datareader.GetInt32OrDefault("State");
                    }
                }

                if (isExist && state == (int)TransactionStates.Authorized)
                {
                    throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_TRANSACTION_ALREADY_ADDED_SHOPPINGCART);
                }

                return transactionId;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_CREATE_FAILED, ex);
            }
        }

        public void UpdateTransaction(MoneyTransfer moneyTransfer, ZeoContext context)
        {
            try
            {
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_UpdateMoneyTransferTransaction");

                moneyTransferProcedure.WithParameters(InputParameter.Named("TransactionId").WithValue(moneyTransfer.Id));
                moneyTransferProcedure.WithParameters(InputParameter.Named("Fee").WithValue(moneyTransfer.Fee));
                moneyTransferProcedure.WithParameters(InputParameter.Named("Amount").WithValue(moneyTransfer.Amount));
                moneyTransferProcedure.WithParameters(InputParameter.Named("CXNId").WithValue((long)moneyTransfer.WUTrxId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(moneyTransfer.DTServerLastModified));
                moneyTransferProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(moneyTransfer.DTTerminalLastModified));

                IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure);
            }
            catch (Exception ex)
            {
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_UPDATE_FAILED, ex);
            }
        }

        public MoneyTransfer GetTransaction(long transactionId, ZeoContext context)
        {
            try
            {
                MoneyTransfer moneyTransfer = null;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_GetMoneyTransferTransactionById");

                moneyTransferProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        moneyTransfer = new MoneyTransfer();
                        moneyTransfer.Id = transactionId;
                        moneyTransfer.Amount = datareader.GetDecimalOrDefault("Amount");
                        moneyTransfer.WUTrxId = datareader.GetInt64OrDefault("cxnId");
                        moneyTransfer.Fee = datareader.GetDecimalOrDefault("Fee");
                        moneyTransfer.Description = datareader.GetStringOrDefault("Description");
                        moneyTransfer.ConfirmationNumber = datareader.GetStringOrDefault("ConfirmationNumber");
                        moneyTransfer.RecipientId = datareader.GetInt64OrDefault("RecipientId");
                        moneyTransfer.MoneyTransferType = (Common.Util.Helper.MoneyTransferType)datareader.GetInt32OrDefault("TransferType");
                        moneyTransfer.TransactionSubType = (Common.Util.Helper.TransactionSubType)datareader.GetInt32OrDefault("TransactionSubType");
                        moneyTransfer.ExchangeRate = datareader.GetDecimalOrDefault("ExchangeRate");
                        moneyTransfer.OriginalTransactionID = datareader.GetInt64OrDefault("OriginalTransactionID");
                        moneyTransfer.CustomerSessionId = datareader.GetInt64OrDefault("CustomerSessionId");
                        // moneyTransfer.ProviderId = datareader.GetInt64OrDefault("ProviderId");
                        moneyTransfer.ProviderAccountId = datareader.GetInt64OrDefault("ProviderAccountId");
                        moneyTransfer.Destination = datareader.GetStringOrDefault("Destination");
                        moneyTransfer.State = (Common.Util.Helper.TransactionStates)datareader.GetInt32OrDefault("State");
                    }
                }

                return moneyTransfer;
            }
            catch (Exception ex)
            {
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_GET_FAILED, ex);
            }
        }

        public void UpdateTransactionState(long transactionId, int state, DateTime dtTerminalDate, DateTime dtServerDate, ZeoContext context)
        {
            try
            {
                StoredProcedure updateMTState = new StoredProcedure("usp_UpdateStateByTransactionId");
                updateMTState.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                updateMTState.WithParameters(InputParameter.Named("state").WithValue(state));
                updateMTState.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(dtServerDate));
                updateMTState.WithParameters(InputParameter.Named("dtTerminalLastModified").WithValue(dtTerminalDate));
                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(updateMTState);
            }
            catch (Exception ex)
            {
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_UPDATE_STATUS_FAILED, ex);
            }
        }

        public void UpdateTransactionStates(long modifiedorRefundTranId, int state, DateTime dtTerminalDate, DateTime dtServerDate, ZeoContext context)
        {
            try
            {
                StoredProcedure updateMTState = new StoredProcedure("usp_UpdateStatesForTransactions");
                updateMTState.WithParameters(InputParameter.Named("modifiedorRefundTransactionId").WithValue(modifiedorRefundTranId));
                updateMTState.WithParameters(InputParameter.Named("state").WithValue(state));
                updateMTState.WithParameters(InputParameter.Named("dtTerminalDate").WithValue(dtTerminalDate));
                updateMTState.WithParameters(InputParameter.Named("dtServerDate").WithValue(dtServerDate));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(updateMTState);
            }
            catch (Exception ex)
            {
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_UPDATE_STATUS_FAILED, ex);
            }
        }

        public ModifyResponse AddModifyandRefundTransactions(MoneyTransfer moneyTransfer, ZeoContext context)
        {
            try
            {
                ModifyResponse modifyResp = null;

                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_AddSendMoneyModifyorRefundTransactions");

                moneyTransferProcedure.WithParameters(InputParameter.Named("mtTransactionId").WithValue(moneyTransfer.Id));
                moneyTransferProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(moneyTransfer.CustomerSessionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("state").WithValue((int)moneyTransfer.State));
                moneyTransferProcedure.WithParameters(InputParameter.Named("transactionSubType").WithValue(Convert.ToString((int)moneyTransfer.TransactionSubType)));
                moneyTransferProcedure.WithParameters(InputParameter.Named("recipientId").WithValue(moneyTransfer.RecipientId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("originalTransactionId").WithValue(moneyTransfer.OriginalTransactionID));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalDate").WithValue(moneyTransfer.DTTerminalCreate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerDate").WithValue(moneyTransfer.DTServerCreate));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(moneyTransferProcedure))
                {
                    while (datareader.Read())
                    {
                        modifyResp = new ModifyResponse();
                        modifyResp.CancelTransactionId = datareader.GetInt64OrDefault("CancelTransactionId");
                        modifyResp.ModifyTransactionId = datareader.GetInt64OrDefault("ModifyorRefundTransactionId");
                    }
                }

                return modifyResp;
            }
            catch (Exception ex)
            {
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_CREATEMSMRSM_FAILED, ex);
            }
        }

        public bool UpdatePTNRTransactionStates(long transactionId, int state, DateTime dtTerminalDate, DateTime dtServerDate, ZeoContext context)
        {
            try
            {
                bool isSendMoneyOrReceiveMoney = false;

                StoredProcedure updateMTState = new StoredProcedure("usp_UpdateStateTransaction");

                updateMTState.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                updateMTState.WithParameters(InputParameter.Named("state").WithValue(state));
                updateMTState.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(dtServerDate));
                updateMTState.WithParameters(InputParameter.Named("dtTerminalLastModified").WithValue(dtTerminalDate));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(updateMTState))
                {
                    while (datareader.Read())
                    {
                        isSendMoneyOrReceiveMoney = datareader.GetBooleanOrDefault("isSendMoneyOrReceiveMoney");
                    }
                }

                return isSendMoneyOrReceiveMoney;
            }
            catch (Exception ex)
            {
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_UPDATE_STATUS_FAILED, ex);
            }
        }

        public void UpdateModifyorRefundTransactions(ModifyResponse modifyResp, long wuCancelTrxId, long wuModifyTrxId, DateTime dtTerminalDate, DateTime dtServerDate, ZeoContext context)
        {
            try
            {
                StoredProcedure moneyTransferProcedure = new StoredProcedure("usp_UpdateModifyTransactions");

                moneyTransferProcedure.WithParameters(InputParameter.Named("cancelTransactionId").WithValue(modifyResp.CancelTransactionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("modifyTransactionId").WithValue(modifyResp.ModifyTransactionId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("cancelWUTransactionId").WithValue(wuCancelTrxId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("modifyWUTransactionId").WithValue(wuModifyTrxId));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtTerminalDate").WithValue(dtTerminalDate));
                moneyTransferProcedure.WithParameters(InputParameter.Named("dtServerDate").WithValue(dtServerDate));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(moneyTransferProcedure);
            }
            catch (Exception ex)
            {
                throw new MoneyTransferException(MoneyTransferException.MONEYTRANSFER_CREATEMSMRSM_FAILED, ex);
            }
        }
    }
}
