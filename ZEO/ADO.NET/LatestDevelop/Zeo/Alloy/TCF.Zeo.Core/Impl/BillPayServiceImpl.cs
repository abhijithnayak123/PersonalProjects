using System;
using System.Collections.Generic;
using System.Data;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Common.DataProtection.Contract;
using TCF.Zeo.Common.DataProtection.Impl;
using P3Net.Data;
using P3Net.Data.Common;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Core.Data.Exceptions;
using System.Configuration;

namespace TCF.Zeo.Core.Impl
{
    public partial class ZeoCoreImpl : IBillPayService
    {

        #region IBillPayService Methods

        #region Transaction's Methods

        public long CreateBillPayTransaction(BillPay billPay, ZeoContext context)
        {
            try
            {
                StoredProcedure createBPTrx = new StoredProcedure("usp_CreateBillPayTransaction");

                createBPTrx.WithParameters(InputParameter.Named("customerSessionId").WithValue(billPay.CustomerSessionId));
                createBPTrx.WithParameters(InputParameter.Named("providerAccountId").WithValue(billPay.ProviderAccountId));
                createBPTrx.WithParameters(InputParameter.Named("providerId").WithValue(billPay.ProviderId));
                createBPTrx.WithParameters(InputParameter.Named("amount").WithValue(billPay.Amount));
                createBPTrx.WithParameters(InputParameter.Named("fee").WithValue(billPay.Fee));
                createBPTrx.WithParameters(InputParameter.Named("description").WithValue(billPay.Description));
                createBPTrx.WithParameters(InputParameter.Named("state").WithValue(billPay.State));
                createBPTrx.WithParameters(InputParameter.Named("confirmationNumber").WithValue(billPay.ConfirmationNumber));
                createBPTrx.WithParameters(InputParameter.Named("dtTerminalCreate").WithValue(billPay.DTTerminalCreate));
                createBPTrx.WithParameters(InputParameter.Named("dtServerCreate").WithValue(billPay.DTServerCreate));
                createBPTrx.WithParameters(InputParameter.Named("billerNameOrCode").WithValue(billPay.BillerNameOrCode));
                createBPTrx.WithParameters(InputParameter.Named("accountNumber").WithValue(billPay.AccountNumber));
                createBPTrx.WithParameters(OutputParameter.Named("transactionId").OfType<string>());

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(createBPTrx);

                return Convert.ToInt64(createBPTrx.Parameters["transactionId"].Value);
            }
            catch(Exception ex)
            {
                throw new BillpayException(BillpayException.BILLPAY_CREATE_FAILED, ex);
            }

        }

        public void UpdateBillPayTransaction(BillPay billPay, long channelPartnerId, ZeoContext context)
        {
            try
            {
                StoredProcedure updateBPTrx = new StoredProcedure("usp_UpdateBillPayTransaction");
                updateBPTrx.WithParameters(InputParameter.Named("transactionId").WithValue(billPay.Id));
                updateBPTrx.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));
                updateBPTrx.WithParameters(InputParameter.Named("amount").WithValue(billPay.Amount));
                updateBPTrx.WithParameters(InputParameter.Named("billerNameOrCode").WithValue(billPay.BillerNameOrCode));
                updateBPTrx.WithParameters(InputParameter.Named("accountNumber").WithValue(billPay.AccountNumber));
                updateBPTrx.WithParameters(InputParameter.Named("dtTerminalLastModified").WithValue(billPay.DTTerminalLastModified));
                updateBPTrx.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(billPay.DTServerLastModified));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(updateBPTrx);
            }
            catch(Exception ex)
            {
                throw new BillpayException(BillpayException.BILLPAY_UPDATE_FAILED, ex);
            }
        }

        public void UpdateBillPayTransactionState(long transactionId, int newState, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure updateBPTrxState = new StoredProcedure("usp_UpdateBillPayTransactionState");
                updateBPTrxState.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                updateBPTrxState.WithParameters(InputParameter.Named("newState").WithValue(newState));
                updateBPTrxState.WithParameters(InputParameter.Named("dtTerminalLastModified").WithValue(Helper.GetTimeZoneTime(timeZone)));
                updateBPTrxState.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(DateTime.Now));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(updateBPTrxState);
            }
            catch (Exception ex)
            {
                throw new BillpayException(BillpayException.BILLPAY_UPDATE_FAILED, ex);
            }
        }

        public void UpdateBillPayTransactionFee(long transactionId, int state, decimal fee, decimal amount, string confirmationNumber, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure updateBPTrxState = new StoredProcedure("usp_UpdateBillPayTransactionFee");
                updateBPTrxState.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                updateBPTrxState.WithParameters(InputParameter.Named("state").WithValue(state));
                updateBPTrxState.WithParameters(InputParameter.Named("fee").WithValue(fee));
                updateBPTrxState.WithParameters(InputParameter.Named("amount").WithValue(amount));
                updateBPTrxState.WithParameters(InputParameter.Named("confirmationNumber").WithValue(confirmationNumber));
                updateBPTrxState.WithParameters(InputParameter.Named("dtTerminalLastModified").WithValue(Helper.GetTimeZoneTime(timeZone)));
                updateBPTrxState.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(DateTime.Now));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(updateBPTrxState);
            }
            catch (Exception ex)
            {
                throw new BillpayException(BillpayException.BILLPAY_UPDATE_FAILED, ex);
            }
        }

        public void UpdatePreferredProductsAndState(long transactionId, int state, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure updatePreferredProductsState = new StoredProcedure("usp_CreateOrUpdateFavouriteBiller");
                updatePreferredProductsState.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                updatePreferredProductsState.WithParameters(InputParameter.Named("state").WithValue(state));
                updatePreferredProductsState.WithParameters(InputParameter.Named("dtTerminalDate").WithValue(Helper.GetTimeZoneTime(timeZone)));
                updatePreferredProductsState.WithParameters(InputParameter.Named("dtServerDate").WithValue(DateTime.Now));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(updatePreferredProductsState);
            }
            catch (Exception ex)
            {
                throw new BillpayException(BillpayException.FAVOURITEBILLER_CREATEORUPDATE_FAILED, ex);
            }
        }

        public long GetBillPayCxnTransactionId(long transactionId)
        {
            try
            {
                long cxnId = 0;

                StoredProcedure billPayCxnTransaction = new StoredProcedure("usp_GetBillPayCxnTransactionId");

                billPayCxnTransaction.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(billPayCxnTransaction))
                {
                    while (datareader.Read())
                    {
                        cxnId = datareader.GetInt64OrDefault("CxnTransactionId");
                    }
                }

                return cxnId;
            }
            catch (Exception ex)
            {
                throw new BillpayException(BillpayException.BILLPAY_FETCH_CXN_ID_FAILED, ex);
            }
        }

        public void UpdateTransactionwithCXNID(long wuTrxId, BillPay billPay, ZeoContext context)
        {
            try
            {
                StoredProcedure updateBPTrx = new StoredProcedure("usp_UpdateCXNIDInBillPayTransaction");
                updateBPTrx.WithParameters(InputParameter.Named("TransactionId").WithValue(billPay.Id));
                updateBPTrx.WithParameters(InputParameter.Named("WUBillPayTrxID").WithValue(wuTrxId));
                updateBPTrx.WithParameters(InputParameter.Named("dtTerminalLastModified").WithValue(billPay.DTTerminalLastModified));
                updateBPTrx.WithParameters(InputParameter.Named("dtServerLastModified").WithValue(billPay.DTServerLastModified));

                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(updateBPTrx);
            }
            catch (Exception ex)
            {
                throw new BillpayException(BillpayException.BILLPAY_UPDATE_FAILED, ex);
            }
        }

        #endregion

        #region Biller's methods

        public List<string> GetBillers(string term, int channelPartnerId, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_GetBillers");
                coreTrxProcedure.WithParameters(InputParameter.Named("channelPartnerID").WithValue(channelPartnerId));
                coreTrxProcedure.WithParameters(InputParameter.Named("term").WithValue(term));
                List<string> products = new List<string>();
                FavouriteBiller biller;

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (datareader.Read())
                    {
                        biller = new FavouriteBiller();
                        biller.BillerName = datareader.GetStringOrDefault("BillerName");
                        biller.BillerCode = datareader.GetStringOrDefault("BillerCode");
                        if (!string.IsNullOrWhiteSpace(biller.BillerCode))
                        {
                            biller.BillerName = biller.BillerName + "/" + biller.BillerCode;
                        }
                        products.Add(biller.BillerName);
                    }
                }

                return products;
            }
            catch (Exception ex)
            {
                throw new BillpayException(BillpayException.BILLERS_GET_FAILED, ex);
            }
        }

        public List<FavouriteBiller> GetFrequentBillers(long customerId, ZeoContext context)
        {
            try
            {
                StoredProcedure frequentBillersProcedure = new StoredProcedure("usp_GetFavouriteBillersByCustomerId");
                frequentBillersProcedure.WithParameters(InputParameter.Named("customerId").WithValue(customerId));
                List<FavouriteBiller> billers = new List<FavouriteBiller>();
                FavouriteBiller biller;

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(frequentBillersProcedure))
                {
                    while (datareader.Read())
                    {
                        biller = new FavouriteBiller();
                        biller.BillerName = datareader.GetStringOrDefault("BillerName");
                        biller.BillerCode = datareader.GetStringOrDefault("BillerCode");
                        biller.ProductId = datareader.GetStringOrDefault("ProductId");
                        billers.Add(biller);
                    }
                }

                return billers;
            }
            catch (Exception ex)
            {
                throw new BillpayException(BillpayException.FAVOURITEBILLER_GET_FAILED, ex);
            }
        }

        public List<FavouriteBiller> DeleteFavouriteBiller(long productId, long customerId, string timeZone, ZeoContext context)
        {
            try
            {
                StoredProcedure coreTrxProcedure = new StoredProcedure("usp_DeleteFavoriteBillerByBillerId");

                coreTrxProcedure.WithParameters(InputParameter.Named("customerId").WithValue(customerId));
                coreTrxProcedure.WithParameters(InputParameter.Named("productId").WithValue(productId));
                coreTrxProcedure.WithParameters(InputParameter.Named("dtServerModified").WithValue(DateTime.Now));
                coreTrxProcedure.WithParameters(InputParameter.Named("dtTerminalModified").WithValue(Helper.GetTimeZoneTime(timeZone)));
                List<FavouriteBiller> billers = new List<FavouriteBiller>();
                FavouriteBiller biller;

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreTrxProcedure))
                {
                    while (datareader.Read())
                    {
                        biller = new FavouriteBiller();
                        biller.BillerName = datareader.GetStringOrDefault("BillerName");
                        biller.BillerCode = datareader.GetStringOrDefault("BillerCode");
                        biller.ProductId = datareader.GetStringOrDefault("ProductId");
                        billers.Add(biller);
                    }
                }

                return billers;
            }
            catch (Exception ex)
            {
                throw new BillpayException(BillpayException.FAVOURITEBILLER_DELETE_FAILED, ex);
            }
        }

        public FavouriteBiller GetBillerDetails(string billerName, long customerId, int channelPartnerId, ZeoContext context)
        {
            try
            {
                FavouriteBiller favouriteBiller = new FavouriteBiller();
                StoredProcedure getBillerDetails = new StoredProcedure("usp_GetBillerInfo");
                getBillerDetails.WithParameters(InputParameter.Named("billerNameOrCode").WithValue(billerName));
                getBillerDetails.WithParameters(InputParameter.Named("customerId").WithValue(customerId));
                getBillerDetails.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));

                using (IDataReader reader = DataConnectionHelper.GetConnectionManager().ExecuteReader(getBillerDetails))
                {
                    while (reader.Read())
                    {
                        favouriteBiller.AccountNumber = Decrypt(reader.GetStringOrDefault("AccountNumber"));
                        favouriteBiller.BillerName = reader.GetStringOrDefault("BillerName");
                        favouriteBiller.ProviderId = reader.GetInt32OrDefault("ProviderId");
                        favouriteBiller.ProductId = reader.GetStringOrDefault("MasterCatalogId");
                    }
                }

                return favouriteBiller;
            }
            catch(Exception ex)
            {
                throw new BillpayException(BillpayException.BILLERS_GET_FAILED, ex);
            }
        }

        private string Decrypt(string encryptedString)
        {
            IDataProtectionService BPDataProtectionSvc = new DataProtectionService();
            string type = (ConfigurationManager.AppSettings["DataProtectionService"]);
            if (type == "Simulator")
            {
                BPDataProtectionSvc = new DataProtectionSimulator();
            }
            else
            {
                BPDataProtectionSvc = new DataProtectionService();
            }

            string decryptString = encryptedString;
            if (!string.IsNullOrWhiteSpace(encryptedString))
            {
                try
                {
                    decryptString = BPDataProtectionSvc.Decrypt(encryptedString, 0);
                }
                catch
                {
                    decryptString = encryptedString;
                }
            }
            return decryptString;
        }

        #endregion

        #endregion
    }
}
