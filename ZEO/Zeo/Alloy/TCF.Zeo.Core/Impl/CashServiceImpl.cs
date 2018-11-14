using TCF.Zeo.Common.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Contract;
using P3Net.Data.Common;
using System;
using TCF.Zeo.Core.Data;
using System.Data;
using P3Net.Data;
using TCF.Zeo.Core.Data.Exceptions;

namespace TCF.Zeo.Core.Impl
{
    public class CashServiceImpl : ICashService
    {
        public bool CashIn(long customerSessionId, decimal amount, string timeZone,ZeoContext context)
        {
            try
            {
                StoredProcedure spAddCashTxn = new StoredProcedure("usp_CashIn");
                spAddCashTxn.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));
                spAddCashTxn.WithParameters(InputParameter.Named("Amount").WithValue(amount));
                spAddCashTxn.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(Helper.GetTimeZoneTime(timeZone)));
                spAddCashTxn.WithParameters(InputParameter.Named("DTServerCreate").WithValue(DateTime.Now));

                int trxCount = DataHelper.GetConnectionManager().ExecuteNonQuery(spAddCashTxn);

                return trxCount > 0;
            }
            catch (Exception ex)
            {
                throw new CashServiceException(CashServiceException.CASHIN_FAILED, ex);
            }
        }

        public CashTransaction GetCashTransaction(long transactionId, ZeoContext context)
        {
            try
            {
                StoredProcedure spCashTxn = new StoredProcedure("usp_GetCashTransactionById");

                spCashTxn.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));

                CashTransaction cashTrx = null;

                using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(spCashTxn))
                {
                    while (datareader.Read())
                    {
                        cashTrx = new CashTransaction();
                        cashTrx.Amount = datareader.GetDecimalOrDefault("Amount");
                        cashTrx.TransactionStatus = datareader.GetStringOrDefault("TransactionStatus");
                        cashTrx.TransactionType = datareader.GetStringOrDefault("TransactionType");
                    }
                }

                return cashTrx;
            }
            catch (Exception ex)
            {
                throw new CashServiceException(CashServiceException.GETCASHTRANSACTION_FAILED, ex);
            }
        }

        public bool RemoveCashIn(long customerSessionId, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure removeCashIn = new StoredProcedure("usp_CancelCashIn");

                removeCashIn.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));
                removeCashIn.WithParameters(InputParameter.Named("DTTerminalModified").WithValue(Helper.GetTimeZoneTime(timeZone)));
                removeCashIn.WithParameters(InputParameter.Named("DTServerModified").WithValue(DateTime.Now));

                int trxCount = DataHelper.GetConnectionManager().ExecuteNonQuery(removeCashIn);

                return trxCount > 0;
            }
            catch (Exception ex)
            {
                throw new CashServiceException(CashServiceException.REMOVECASHIN_FAILED, ex);
            }
        }

        public bool UpdateOrCancelCash(long customerSessionId, decimal amount, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure spAddCashTxn = new StoredProcedure("usp_UpdateOrCancelCash");

                spAddCashTxn.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(customerSessionId));
                spAddCashTxn.WithParameters(InputParameter.Named("CashCollected").WithValue(amount));
                spAddCashTxn.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(Helper.GetTimeZoneTime(timeZone)));
                spAddCashTxn.WithParameters(InputParameter.Named("DTServerCreate").WithValue(DateTime.Now));

                int trxCount = DataHelper.GetConnectionManager().ExecuteNonQuery(spAddCashTxn);

                return trxCount > 0;
            }
            catch (Exception ex)
            {
                throw new CashServiceException(CashServiceException.UPDATEORCANCELCASH_FAILED, ex);
            }
        }
    }
}
