using TCF.Zeo.Cxn.Customer.Contract;
using System;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.Customer.TCF.Data;
using P3Net.Data.Common;
using System.Data;
using P3Net.Data;
using TCF.Zeo.Common.Util;
using System.Collections.Generic;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Cxn.Customer.TCF.Contract;
using System.Configuration;
using TCF.Zeo.Cxn.Customer.TCF.Data.Exceptions;
using System.Linq;
using TCF.Zeo.Common.DataProtection.Contract;
using TCF.Zeo.Common.DataProtection.Impl;

namespace TCF.Zeo.Cxn.Customer.TCF.Impl
{
    public class FlushProcessorImpl : IFlushProcessor
    {
        internal IIO IO { get; set; }

        public FlushProcessorImpl()
        {
            IO = GetTCFProcessor();
        }

        #region Public

        public void PostFlush(decimal cardBalance, ZeoContext context)
        {
            try
            {
                CustomerTransactionDetails custTrx = GetCustomerTransactionDetails(context, Helper.ShoppingCartStatus.Closed, true, cardBalance);
                if (!custTrx.Customer.CustInd)
                {
                    UpdateCustomerCustInd(true, context);
                }

                if (custTrx.Transactions.Where(X => X.TransferType != TransactionType.Cash.ToString()).Count() > 0)
                {
                    RCIFCredential credential = RCIFCommon.GetCredential(context.ChannelPartnerId);
                    IO.PostFlush(custTrx, credential, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FlushException(FlushException.POST_FLUSH_FAILED, ex);
            }
        }

        public void PreFlush(decimal cashToCustomer, ZeoContext context)
        {
            try
            {
                CustomerTransactionDetails custTrx = GetCustomerTransactionDetails(context, Helper.ShoppingCartStatus.Active);

                //This condition added, in case there are multiple trx in shopping cart and if any one of them are either in committed status or in processing status update the TcfCustInd flag to 1.
                if (!custTrx.Customer.CustInd &&
                        custTrx.Transactions.Where(c => (TransactionType)Enum.Parse(typeof(TransactionType), c.Type) != TransactionType.Cash && (c.Status == TransactionStates.Committed.ToString("D") || c.Status == TransactionStates.Processing.ToString("D"))).Any())
                {
                    //This line is added because when any transaction is in the committed status, then we have to pass TcfCustInd as true for PreFlush.
                    custTrx.Customer.CustInd = true;
                    UpdateCustomerCustInd(true, context);
                }

                custTrx.Transactions = custTrx.Transactions.Where(x => x.Status == TransactionStates.Authorized.ToString("D")).ToList();

                if (cashToCustomer > 0)
                {
                    Transaction trx = new Transaction()
                    {
                        ID = "20000000",
                        Type = TransactionType.Cash.ToString(),
                        CashType = CashType.CashOut.ToString(),
                        Amount = cashToCustomer,
                        Fee = 0,
                        GrossTotalAmount = cashToCustomer - 0
                    };
                    custTrx.Transactions.Add(trx);
                }

                if (custTrx.Transactions.Where(X => X.TransferType != TransactionType.Cash.ToString()).Count() > 0)
                {
                    RCIFCredential credential = RCIFCommon.GetCredential(context.ChannelPartnerId);
                    IO.PreFlush(custTrx, credential, context);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FlushException(FlushException.PRE_FLUSH_FAILED, ex);
            }
        }

        public void TellerFinalCommitCalls(ZeoContext context)
        {
            try
            {
                CustomerTransactionDetails custTrx = GetCustomerTransactionDetails(context, Helper.ShoppingCartStatus.Closed, true);

                bool isTellerWorkQueueWSCallFailed = false;

                if (CanCallFinalCommit(custTrx, context))
                {
                    RCIFCredential credential = RCIFCommon.GetCredential(context.ChannelPartnerId);

                    bool isTellerMainFrameCallFailed = false;

                    //Conditions - 
                    // 1. If the Teller Middle tier is failed, then we should not call Teller Main Frame but we need to call RCIF call if any risk score > 0.
                    // 2. If the Teller Main Frame is failed for any of the product trxn in the shopping cart, do not call the Teller Main Frame for the Cash In or Cash Out but make the call for the other product trxs.
                    // 3. If RCIF call is failed, do not do anything.
                    for (int count = 0; count < custTrx?.Transactions?.Count; count++)
                    {
                        string CIF7454TemplateType = null;

                        Transaction transaction = custTrx?.Transactions[count];

                        //riskScores.Item1 = Customer Risk score,riskScores.Item2 = GPR Risk score, riskScores.Item3 = ZeoSvc Risk score
                        Tuple<long, long, long, bool> riskScoresWithCallSuccess = IO.TellerMiddleTierCommit(custTrx, transaction, ref CIF7454TemplateType, credential, context);

                        if (!riskScoresWithCallSuccess.Item4)
                        {
                            Tuple<long, long, long> riskScores = new Tuple<long, long, long>(riskScoresWithCallSuccess.Item1, riskScoresWithCallSuccess.Item2, riskScoresWithCallSuccess.Item3);

                            if (!isTellerMainFrameCallFailed || (isTellerMainFrameCallFailed && transaction.Type != Convert.ToString(TransactionType.Cash)))
                                isTellerMainFrameCallFailed = IO.TellerMainFrameCommit(transaction, credential, context);

                            //Different fields are passed for GPR and for Non GPR trx.
                            if (transaction?.Type == Convert.ToString(TransactionType.Fund))
                                IO.RCIFFinalCommit(true, CIF7454TemplateType, riskScores, custTrx, transaction, credential, context);
                            else if (riskScores.Item1 > 0 || riskScores.Item2 > 0 || riskScores.Item3 > 0)
                                IO.RCIFFinalCommit(false, CIF7454TemplateType, riskScores, custTrx, transaction, credential, context);

                            //Updating the TCF Cust Ind to "true" for the first trx.
                            if (!custTrx.Customer.CustInd && count == 0)
                            {
                                UpdateCustomerCustInd(true, context);

                                custTrx.Customer.CustInd = true;
                            }
                        }
                        else
                        {
                            isTellerWorkQueueWSCallFailed = riskScoresWithCallSuccess.Item4; //True

                            // We should not make a Teller Main frame call for cash trxs if the teller work queue is failed for any of the transactions 
                            isTellerMainFrameCallFailed = true; 
                        }
                    }

                    if (isTellerWorkQueueWSCallFailed)
                    {
                        //throw new FlushException(FlushException.TELLER_FINAL_COMMIT_FAILED);
                    }
                }
            }

            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new FlushException(FlushException.TELLER_FINAL_COMMIT_FAILED, ex);
            }
        }

        #endregion

        #region Private
        private CustomerTransactionDetails GetCustomerTransactionDetails(ZeoContext context, Helper.ShoppingCartStatus cartState, bool isPostflush = false, decimal cardBalance = 0)
        {
            CustomerTransactionDetails customertrx = new CustomerTransactionDetails();
            CustomerDetails customerProfile = null;

            StoredProcedure flushProcedure = new StoredProcedure("usp_GetPreflushPostflushDetails");

            flushProcedure.WithParameters(InputParameter.Named("CustomerId").WithValue(context.CustomerId));
            flushProcedure.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(context.CustomerSessionId));
            flushProcedure.WithParameters(InputParameter.Named("State").WithValue((int)cartState));

            using (IDataReader datareader = DataHelper.GetConnectionManager().ExecuteReader(flushProcedure))
            {
                while (datareader.Read())
                {
                    customerProfile = new CustomerDetails();
                    customerProfile.AlloyID = context.CustomerId;
                    customerProfile.SSN = datareader.GetStringOrDefault("SSN");
                    customerProfile.FirstName = datareader.GetStringOrDefault("FirstName");
                    customerProfile.MiddleName = datareader.GetStringOrDefault("MiddleName");
                    customerProfile.LastName = datareader.GetStringOrDefault("LastName");
                    customerProfile.SecondLastName = datareader.GetStringOrDefault("LastName2");
                    customerProfile.DateOfBirth = Convert.ToString(datareader.GetDateTimeOrDefault("DOB"));
                    customerProfile.Address1 = datareader.GetStringOrDefault("Address1");
                    customerProfile.Address2 = datareader.GetStringOrDefault("Address2");
                    customerProfile.City = datareader.GetStringOrDefault("City");
                    customerProfile.State = datareader.GetStringOrDefault("State");
                    customerProfile.Zip = datareader.GetStringOrDefault("ZipCode");
                    customerProfile.Phone1 = datareader.GetStringOrDefault("Phone1");
                    customerProfile.Ph1Type1 = datareader.GetStringOrDefault("Phone1Type");
                    customerProfile.Phone2 = datareader.GetStringOrDefault("Phone2");
                    customerProfile.Ph2Type2 = datareader.GetStringOrDefault("Phone2Type");
                    customerProfile.Ph2Prov = datareader.GetStringOrDefault("Phone2Provider");
                    customerProfile.Gender = datareader.GetStringOrDefault("Gender");
                    customerProfile.Email = datareader.GetStringOrDefault("Email");
                    customerProfile.Maiden = datareader.GetStringOrDefault("MothersMaidenName");
                    customerProfile.ClientCustId = datareader.GetStringOrDefault("ClientID");
                    customerProfile.LegalCode = datareader.GetStringOrDefault("LegalCode");
                    customerProfile.PrimaryCountryCitizenship = datareader.GetStringOrDefault("PrimaryCountryCitizenShipName");
                    customerProfile.SecondaryCountryCitizenship = datareader.GetStringOrDefault("SecondaryCountryCitizenShipName");
                    customerProfile.Occupation = datareader.GetStringOrDefault("Occupation");
                    customerProfile.EmployerName = datareader.GetStringOrDefault("Employer");
                    customerProfile.EmployerPhoneNum = datareader.GetStringOrDefault("EmployerPhone");
                    customerProfile.OccupationDescription = datareader.GetStringOrDefault("OccupationDescription");
                    customerProfile.IdIssuer = datareader.GetStringOrDefault("IdStateAbbr");
                    customerProfile.IdIssuerCountry = datareader.GetStringOrDefault("IdCountry");
                    customerProfile.ExpirationDate = datareader.GetDateTimeOrDefault("GovtIDExpirationDate");
                    customerProfile.IssueDate = datareader.GetDateTimeOrDefault("GovtIdIssueDate");
                    customerProfile.Identification = datareader.GetStringOrDefault("GovtIdentification");
                    customerProfile.CustInd = datareader.GetBooleanOrDefault("CustInd");
                    customerProfile.CustomerSessionId = context.CustomerSessionId;
                    customerProfile.IdType = datareader.GetStringOrDefault("IdName");
                }

                datareader.NextResult();

                List<Transaction> transactions = new List<Transaction>();

                while (datareader.Read())
                {
                    Transaction trx = new Transaction();

                    trx.ID = Convert.ToString(datareader.GetInt64OrDefault("Id"));
                    trx.AccountNumber = datareader.GetStringOrDefault("AccountNumber");
                    trx.AliasId = datareader.GetStringOrDefault("AliasId");
                    trx.Amount = datareader.GetDecimalOrDefault("Amount");
                    trx.TransferSubType = datareader.GetInt32OrDefault("transferSubType");
                    trx.CashType = datareader.GetStringOrDefault("CashType");
                    trx.CheckNumber = datareader.GetStringOrDefault("CheckNumber");
                    trx.CheckRoutingNumber = datareader.GetStringOrDefault("CheckRoutingNumber");
                    trx.CheckAccountNumber = datareader.GetStringOrDefault("CheckAccountNumber");
                    trx.TransactionAccountId = datareader.GetStringOrDefault("TransactionAccountId");
                    trx.CheckType = datareader.GetInt32OrDefault("CheckType") == 0 ? string.Empty : ((Helper.CheckTypes)datareader.GetInt32OrDefault("CheckType")).ToString();
                    trx.CheckTypeName = datareader.GetStringOrDefault("CheckTypeName");
                    trx.ConfirmationNumber = datareader.GetStringOrDefault("ConfirmationNumber");
                    trx.Fee = datareader.GetDecimalOrDefault("Fee");
                    trx.GrossTotalAmount = datareader.GetDecimalOrDefault("GrossTotalAmount");
                    trx.MTCN = datareader.GetStringOrDefault("MTCN");
                    trx.Payee = datareader.GetStringOrDefault("Payee");
                    trx.Status = datareader.GetStringOrDefault("Status");
                    trx.ToAddress = datareader.GetStringOrDefault("ToAddress");
                    trx.ToCity = datareader.GetStringOrDefault("ToCity");
                    trx.ToCountry = datareader.GetStringOrDefault("ToCountry");
                    trx.ToCountryOfBirth = datareader.GetStringOrDefault("ToCountryOfBirth");
                    trx.ToDeliveryMethod = datareader.GetStringOrDefault("ToDeliveryMethod");
                    trx.ToDeliveryOption = datareader.GetStringOrDefault("ToDeliveryOption");
                    trx.ToFirstName = datareader.GetStringOrDefault("ToFirstName");
                    trx.ToLastName = datareader.GetStringOrDefault("ToLastName");
                    trx.ToPhoneNumber = datareader.GetStringOrDefault("ToPhoneNumber");
                    trx.ToPickUpCity = datareader.GetStringOrDefault("ToPickUpCity");
                    trx.ToPickUpCountry = datareader.GetStringOrDefault("ToPickUpCountry");
                    trx.ToPickUpState_Province = datareader.GetStringOrDefault("ToPickUpState_Province");
                    trx.ToSecondLastName = datareader.GetStringOrDefault("ToSecondLastName");
                    trx.ToState_Province = datareader.GetStringOrDefault("ToState_Province");
                    trx.ToZipCode = datareader.GetStringOrDefault("ToZipCode");
                    trx.TransferType = datareader.GetStringOrDefault("TransferType");
                    trx.Type = datareader.GetStringOrDefault("Type");
                    trx.InitialPurchase = datareader.GetBooleanOrDefault("isActivate") ? "Y" : "N";
                    trx.NewCardBalance = isPostflush ? cardBalance : datareader.GetDecimalOrDefault("CardBalance");
                    trx.CardNumber = (trx.Type == TransactionType.Fund.ToString()) ? DecryptCardNumber(datareader.GetStringOrDefault("CardNumber")) : string.Empty;
                    transactions.Add(trx);
                }
                customertrx.Customer = customerProfile;
                customertrx.Transactions = transactions;
            }

            return customertrx;
        }

        private void UpdateCustomerCustInd(bool tcfCustInd, ZeoContext context)
        {
            StoredProcedure flushProcedure = new StoredProcedure("usp_UpdateCustIndByCustomerId");

            flushProcedure.WithParameters(InputParameter.Named("CustomerId").WithValue(context.CustomerId));
            flushProcedure.WithParameters(InputParameter.Named("custInd").WithValue(tcfCustInd));
            DataHelper.GetConnectionManager().ExecuteNonQuery(flushProcedure);
        }

        private static IIO GetTCFProcessor()
        {
            bool isRealTimeTellerCall = Convert.ToBoolean(ConfigurationManager.AppSettings["RealTimeTellerCalls"].ToString());

            if (isRealTimeTellerCall)
                return new IO();
            else
                return new SimulatorIO();
        }

        private string DecryptCardNumber(string encryptedCardNumber)
        {
            IDataProtectionService dataProtectionService = null;

            string type = (ConfigurationManager.AppSettings["DataProtectionService"]);
            if (type == "Simulator")
            {
                dataProtectionService = new DataProtectionSimulator();
            }
            else
            {
                dataProtectionService = new DataProtectionService();
            }

            return dataProtectionService.Decrypt(encryptedCardNumber, 0);
        }

        private bool CanCallFinalCommit(CustomerTransactionDetails custTrx, ZeoContext context)
        {
            bool isTrxCount = custTrx.Transactions.Where(X => X.TransferType != TransactionType.Cash.ToString()).Count() > 0 ? true : false;

            return isTrxCount;
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
