using AutoMapper;
using P3Net.Data.Common;
using P3Net.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.Check.Contract;
using TCF.Zeo.Cxn.Check.Data;
using TCF.Zeo.Cxn.Check.TCF.Contract;
using TCF.Zeo.Cxn.Check.TCF.Data;
using TCF.Zeo.Cxn.Common;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.Check.Data.Exceptions;
using System.Security.Cryptography.X509Certificates;
using System.Data;

namespace TCF.Zeo.Cxn.Check.TCF.Impl
{
    public class TCFGateway : ICheckProcessor
    {
        #region Dependencies

        IMapper mapper;
        public IIO _tcfOnUS { get; } = GetTCFIO();

        #endregion

        public TCFGateway()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TCFOnusTransaction, CheckTransaction>()
                .ForMember(x => x.DeclineMessage, s => s.MapFrom(c => c.Message));
            });
            mapper = config.CreateMapper();
        }

        public bool Cancel(long trxId, ZeoContext context)
        {
            return true;
        }

        public void Commit(long trxId, ZeoContext context)
        {
        }

        public CheckTransaction Get(long trxId)
        {
            return new CheckTransaction();
        }

        public long GetAccount(ZeoContext context)
        {
            try
            {
                TCFOnusAccount account = GetCheckAccount(context.CustomerSessionId);

                if (account != null && account.Id == 0)
                {
                    account.DTServerCreate = DateTime.Now;
                    account.DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone);

                    account.Id = CreateTCFOnusAccount(account);
                }

                return account.Id;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_GET_ACCOUNT_FAILED, ex);
            }
        }

        public CheckProcessorInfo GetCheckProcessorInfo(long agentSessionId, string locationId)
        {
            return new CheckProcessorInfo();
        }

        public CheckLogin GetCheckSessions(ZeoContext context)
        {
            return new CheckLogin();
        }

        public List<PendingCheck> GetPendingChecks()
        {
            return new List<PendingCheck>();
        }

        public CheckTransaction Status(long transactionId, ZeoContext context)
        {
            return new CheckTransaction();
        }

        public CheckTransaction Submit(long accountId, CheckInfo check, ZeoContext context)
        {
            try
            {
                TCFOnusTransaction tcfonusTrx = new TCFOnusTransaction
                {
                    Amount = check.Amount,
                    Micr = check.Micr,
                    Latitude = check.Latitude,
                    Longitude = check.Longitude,
                    CheckDate = check.IssueDate,
                    TcfOnusAccountId = accountId,
                    TcfOnusStatus = TCFOnusStatus.None,
                    Location = Convert.ToString(context.LocationID),
                    Fee = check.Fee,
                    AccountNumber = check.AccountNumber,
                    CheckNumber = check.CheckNumber,
                    RoutingNumber = check.RoutingNumber,
                    DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone),
                    DTServerCreate = DateTime.Now
                };

                RCIFCredential credential = GetCredential(context.ChannelPartnerId);

                tcfonusTrx.Id = CreateTCFOnusTransaction(tcfonusTrx);

                TellerMainFrameResponse inquiryResponse = _tcfOnUS.TellerInquiry(tcfonusTrx, credential, context);
                tcfonusTrx.Message = inquiryResponse.TEL7770OperationResponse?.telnexxc_tran_return?.telnexxc_message;
                tcfonusTrx.TcfOnusStatus = inquiryResponse.TEL7770OperationResponse?.telnexxc_tran_return?.telnexxc_reject == "0" ? TCFOnusStatus.Approved : TCFOnusStatus.Declined;
                tcfonusTrx.TellerTraceCode = inquiryResponse.TEL7770OperationResponse?.telnexxc_tran_return?.telnexxc_trace;

                //if (tcfonusTrx.TcfOnusStatus == TCFOnusStatus.Declined)
                //    tcfonusTrx.DeclineMessageKey = ((int)Helper.ProductCode.CheckProcessing).ToString() + "." + ((int)Helper.ProviderId.TCFCheck).ToString() + "." + 0;

                CheckTransaction checkTrx = UpdateTCFOnusTransaction(tcfonusTrx, context.ChannelPartnerId);
                checkTrx.Status = tcfonusTrx.TcfOnusStatus == TCFOnusStatus.Approved ? CheckStatus.Approved : CheckStatus.Declined;
                checkTrx.ReturnAmount = tcfonusTrx.Amount;
                checkTrx.SubmitType = checkTrx.ReturnType = check.Type;

                HandleTellerInquiryExceptions(inquiryResponse);

                return checkTrx;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_SUBMIT_FAILED, ex);
            }
        }

        public void UpdateTransactionFranked(long trxId, bool isCheckFranked)
        {
            try
            {
                StoredProcedure coreChexarProcedure = new StoredProcedure("usp_UpdateTransactionFrankedForOnUS");
                coreChexarProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(trxId));
                coreChexarProcedure.WithParameters(InputParameter.Named("IsCheckFranked").WithValue(isCheckFranked));
                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreChexarProcedure);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_UPDATE_TRANSACTION_FAILED, ex);
            }
        }

        #region Private Method

        private static IIO GetTCFIO()
        {
            bool isRealTimeTellerCall = Convert.ToBoolean(ConfigurationManager.AppSettings["RealTimeTellerCalls"].ToString());

            if (isRealTimeTellerCall)
                return new IO();
            else
                return new SimulatorIO();

        }

        private TCFOnusAccount GetCheckAccount(long customerSessionId)
        {
            StoredProcedure coreChexarProcedure = new StoredProcedure("usp_GetTCFOnusAccount");

            coreChexarProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));

            TCFOnusAccount tcfonusAccount = DataConnectionHelper.GetConnectionManager().ExecuteQueryWithResult<TCFOnusAccount>(coreChexarProcedure, dr => new TCFOnusAccount
            {
                FirstName = dr.GetStringOrDefault("FirstName"),
                LastName = dr.GetStringOrDefault("LastName"),
                Address1 = dr.GetStringOrDefault("Address1"),
                Address2 = dr.GetStringOrDefault("Address2"),
                Phone = dr.GetStringOrDefault("Phone"),
                City = dr.GetStringOrDefault("City"),
                State = dr.GetStringOrDefault("State"),
                Zip = dr.GetStringOrDefault("Zip"),
                SSN = dr.GetStringOrDefault("SSN"),
                IDCode = dr.GetStringOrDefault("IDcode"),
                GovernmentId = dr.GetStringOrDefault("GovernmentId"),
                DateOfBirth = dr.GetDateTimeOrDefault("DOB"),
                Employer = dr.GetStringOrDefault("Employer"),
                EmployerPhone = dr.GetStringOrDefault("EmployerPhone"),
                Occupation = dr.GetStringOrDefault("Occupation"),
                IDCardType = dr.GetStringOrDefault("IDCardType"),
                IDCardIssuedCountry = dr.GetStringOrDefault("IDCardIssuedCountry"),
                IDCardIssuedDate = dr.GetDateTimeOrDefault("IDCardIssuedDate"),
                IDCardExpireDate = dr.GetDateTimeOrDefault("IDCardExpireDate"),
                Id = dr.GetInt64OrDefault("TCFOnusAccountID"),
                CustomerId = dr.GetInt64OrDefault("CustomerId"),
                CustomerSessionId = customerSessionId
            });

            if (tcfonusAccount.IDCode == "I")
            {
                tcfonusAccount.ITIN = tcfonusAccount.SSN;
                tcfonusAccount.SSN = string.Empty;
            }

            return tcfonusAccount;
        }

        private long CreateTCFOnusAccount(TCFOnusAccount tcfonusAccount)
        {
            StoredProcedure coreChexarProcedure = new StoredProcedure("usp_CreateTCFOnusAccount");

            coreChexarProcedure.WithParameters(OutputParameter.Named("TCFOnusAccountId").OfType<long>());
            coreChexarProcedure.WithParameters(InputParameter.Named("CustomerId").WithValue(tcfonusAccount.CustomerId));
            coreChexarProcedure.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(tcfonusAccount.CustomerSessionId));
            coreChexarProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(tcfonusAccount.DTServerCreate));
            coreChexarProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(tcfonusAccount.DTTerminalCreate));

            DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreChexarProcedure);

            return Convert.ToInt64(coreChexarProcedure.Parameters["TCFOnusAccountId"].Value);
        }

        private RCIFCredential GetCredential(long channelPartnerId)
        {
            RCIFCredential credentials = new RCIFCredential();

            StoredProcedure coreCustomerProcedure = new StoredProcedure("usp_GetRCIFCredentials");

            coreCustomerProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreCustomerProcedure))
            {
                while (datareader.Read())
                {
                    credentials.ServiceUrl = datareader.GetStringOrDefault("ServiceUrl");
                    credentials.ChannelPartnerId = datareader.GetInt32OrDefault("ChannelPartnerId");
                    credentials.CertificateName = datareader.GetStringOrDefault("CertificateName");
                    credentials.ThumbPrint = datareader.GetStringOrDefault("ThumbPrint");
                    credentials.TellerInquiryUrl = datareader.GetStringOrDefault("TellerInquiryURL");
                    credentials.RCIFFinalCommitURL = datareader.GetStringOrDefault("RCIFFinalCommitURL");
                }
            }

            return credentials;
        }

        private long CreateTCFOnusTransaction(TCFOnusTransaction tcfonusTrx)
        {
            StoredProcedure tcfOnusProcedure = new StoredProcedure("usp_CreateTCFOnusTransaction");
            tcfOnusProcedure.WithParameters(OutputParameter.Named("TcfonusTransactionId").OfType<long>());
            tcfOnusProcedure.WithParameters(InputParameter.Named("Location").WithValue(tcfonusTrx.Location));
            tcfOnusProcedure.WithParameters(InputParameter.Named("Amount").WithValue(tcfonusTrx.Amount));
            tcfOnusProcedure.WithParameters(InputParameter.Named("CheckDate").WithValue(tcfonusTrx.CheckDate));
            tcfOnusProcedure.WithParameters(InputParameter.Named("TcfonusAccountId").WithValue(tcfonusTrx.TcfOnusAccountId));
            tcfOnusProcedure.WithParameters(InputParameter.Named("TcfonusStatus").WithValue(tcfonusTrx.TcfOnusStatus.ToString()));
            //coreChexarProcedure.WithParameters(InputParameter.Named("Status").WithValue((int)tcfonusTrx.Status));
            tcfOnusProcedure.WithParameters(InputParameter.Named("Latitude").WithValue(tcfonusTrx.Latitude));
            tcfOnusProcedure.WithParameters(InputParameter.Named("Longitude").WithValue(tcfonusTrx.Longitude));
            tcfOnusProcedure.WithParameters(InputParameter.Named("Micr").WithValue(tcfonusTrx.Micr));
            tcfOnusProcedure.WithParameters(InputParameter.Named("SubmitType").WithValue(tcfonusTrx.SubmitType));
            tcfOnusProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(tcfonusTrx.DTServerCreate));
            tcfOnusProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(tcfonusTrx.DTTerminalCreate));

            int trxCount = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(tcfOnusProcedure);

            return Convert.ToInt64(tcfOnusProcedure.Parameters["TcfonusTransactionId"].Value);
        }

        private CheckTransaction UpdateTCFOnusTransaction(TCFOnusTransaction tcfonusTrx, long channelPartnerId)
        {
            StoredProcedure coreOnusProcedure = new StoredProcedure("usp_UpdateTCFOnusTransactionById");

            coreOnusProcedure.WithParameters(InputParameter.Named("TcfonusTransactionId").WithValue(tcfonusTrx.Id));
            coreOnusProcedure.WithParameters(InputParameter.Named("AccountNumber").WithValue(tcfonusTrx.AccountNumber));
            coreOnusProcedure.WithParameters(InputParameter.Named("Amount").WithValue(tcfonusTrx.Amount));
            coreOnusProcedure.WithParameters(InputParameter.Named("CheckNumber").WithValue(tcfonusTrx.CheckNumber));
            coreOnusProcedure.WithParameters(InputParameter.Named("TcfOnusStatus").WithValue(tcfonusTrx.TcfOnusStatus.ToString()));
            coreOnusProcedure.WithParameters(InputParameter.Named("Status").WithValue(Convert.ToInt32(tcfonusTrx.TcfOnusStatus)));
            coreOnusProcedure.WithParameters(InputParameter.Named("Latitude").WithValue(tcfonusTrx.Latitude));
            coreOnusProcedure.WithParameters(InputParameter.Named("Longitude").WithValue(tcfonusTrx.Longitude));
            coreOnusProcedure.WithParameters(InputParameter.Named("RoutingNumber").WithValue(tcfonusTrx.RoutingNumber));
            coreOnusProcedure.WithParameters(InputParameter.Named("dTServerLastModified").WithValue(DateTime.Now));
            coreOnusProcedure.WithParameters(InputParameter.Named("dTTerminalLastModified").WithValue(tcfonusTrx.DTTerminalLastModified));
            coreOnusProcedure.WithParameters(InputParameter.Named("ChannelPartnerId").WithValue(channelPartnerId));
            coreOnusProcedure.WithParameters(InputParameter.Named("Message").WithValue(tcfonusTrx.Message));
            coreOnusProcedure.WithParameters(InputParameter.Named("TellerTraceCode").WithValue(tcfonusTrx.TellerTraceCode));
            coreOnusProcedure.WithParameters(InputParameter.Named("CurrentBalance").WithValue(tcfonusTrx.CurrentBalance));
            coreOnusProcedure.WithParameters(InputParameter.Named("AvailableBalance").WithValue(tcfonusTrx.AvailabletBalance));
            coreOnusProcedure.WithParameters(InputParameter.Named("lang").WithValue((int)Helper.Language.English));

            DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreOnusProcedure);

            CheckTransaction checkTrx = mapper.Map<CheckTransaction>(tcfonusTrx);

            return checkTrx;
        }

        private void HandleTellerInquiryExceptions(TellerMainFrameResponse inquiryResponse)
        {
            if(inquiryResponse != null && inquiryResponse.TEL7770OperationResponse != null 
                    && inquiryResponse.TEL7770OperationResponse.telnexxc_tran_return != null)
            {
                var response = inquiryResponse.TEL7770OperationResponse.telnexxc_tran_return;

                if (!string.IsNullOrWhiteSpace(response.telnexxc_nsf) && response.telnexxc_nsf == "1")
                {
                    throw new CheckException(CheckException.TELLERINQUIRYNONSUFFICIENT_FUNDS);
                }

                if (!string.IsNullOrWhiteSpace(response.telnexxc_stops) && response.telnexxc_stops == "1")
                {
                    throw new CheckException(CheckException.TELLERINQUIRYACCOUNT_STOPS);
                }

                if (!string.IsNullOrWhiteSpace(response.telnexxc_caution) && response.telnexxc_caution == "1")
                {
                    throw new CheckException(CheckException.TELLERINQUIRYACCOUNT_CAUTION);
                }

                if (!string.IsNullOrWhiteSpace(response.telnexxc_nopost) && response.telnexxc_nopost == "1")
                {
                    throw new CheckException(CheckException.TELLERINQUIRYFLAGGEDFORNO_POSTING);
                }

                if (!string.IsNullOrWhiteSpace(response.telnexxc_nodebits) && response.telnexxc_nodebits == "1")
                {
                    throw new CheckException(CheckException.TELLERINQUIRYFLAGGEDFORNO_DEBITS);
                }

                if (!string.IsNullOrWhiteSpace(response.telnexxc_noacct) && response.telnexxc_noacct == "1")
                {
                    throw new CheckException(CheckException.TELLERINQUIRYNO_ACCOUNT);
                }

                if (!string.IsNullOrWhiteSpace(response.telnexxc_closed) && response.telnexxc_closed == "1")
                {
                    throw new CheckException(CheckException.TELLERINQUIRYACCOUNT_CLOSED);
                }

                if (!string.IsNullOrWhiteSpace(response.telnexxc_dormant) && response.telnexxc_dormant == "1")
                {
                    throw new CheckException(CheckException.TELLERINQUIRYACCOUNT_DORMANT);
                }

            }
        }
        #endregion
    }
}
