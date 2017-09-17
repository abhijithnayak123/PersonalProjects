using System;
using System.Collections.Generic;
using System.Linq;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.Check.Contract;
using TCF.Zeo.Cxn.Check.Data;
using TCF.Zeo.Cxn.Check.Chexar.Data;
using ChxrIO = TCF.Zeo.Cxn.Check.Chexar.Data;
using TCF.Zeo.Common.Data;
using System.Data;
using System.Configuration;
using AutoMapper;
using P3Net.Data.Common;
using P3Net.Data;
using TCF.Zeo.Cxn.Check.Chexar.Contract;
using TCF.Zeo.Cxn.Common;
using TCF.Zeo.Cxn.Check.Data.Exceptions;

namespace TCF.Zeo.Cxn.Check.Chexar.Impl
{
    public class ChexarGateway : ICheckProcessor
    {

        #region Dependencies

        IMapper mapper;
        public IIO _chexar { get; } = GetChexarIO();

        #region set Chexar Login Prefix

        private bool getPendingStatusFromProcessor { get; } = Convert.ToBoolean(ConfigurationManager.AppSettings["GetPendingStatusFromProcessor"]);
        private string BranchUserNamePrefix { get; } = ConfigurationManager.AppSettings["BranchUserNamePrefix"];
        private string BranchPasswordPrefix { get; } = ConfigurationManager.AppSettings["BranchPasswordPrefix"];
        private string EmployeeUserNamePrefix { get; } = ConfigurationManager.AppSettings["EmployeeUserNamePrefix"];
        private string EmployeePasswordPrefix { get; } = ConfigurationManager.AppSettings["EmployeePasswordPrefix"];
        private string ChexarSessionSeperator { get; } = ConfigurationManager.AppSettings["ChexarSessionSeperator"];

        #endregion

        #endregion

        public ChexarGateway()
        {
            #region Mapping
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ChexarTransaction, CheckTransaction>()
                .ForMember(x => x.ReturnAmount, s => s.MapFrom(c => c.ChexarAmount))
                .ForMember(x => x.ReturnFee, s => s.MapFrom(c => c.ChexarFee))
                .ForMember(x => x.DeclineMessage, s => s.MapFrom(c => c.Message))
                .ForMember(x => x.ConfirmationNumber, s => s.MapFrom(c => c.InvoiceId.ToString()))
                .ForMember(x => x.ReturnType, s => s.MapFrom(c => c.DmsReturnType));
                cfg.CreateMap<ChexarSession, CheckLogin>()
                  .ForMember(x => x.ChannelPartnerId, s => s.MapFrom(c => c.ChexarPartnerId));

            });
            mapper = config.CreateMapper();
            #endregion
        }

        #region ICheckProcessor methods

        public CheckLogin GetCheckSessions(ZeoContext context)
        {
            try
            {
                long channelPartnerId = context.ChannelPartnerId;
                string timezone = context.TimeZone;
                string userName = context.CheckUserName;
                string password = context.CheckPassword;

                string chexarLocation = GenerateChexarLocationId(channelPartnerId, context.CheckUserName);
                ChexarPartner chexarPartner = GetChexarPartnerByChannelPartnerId(context.ChannelPartnerId);

                if (chexarPartner == null)
                    throw new CheckException(CheckException.CHECK_CREDENTIALS_NOT_FOUND);



                string branchUsername = string.Format("{0}{1}{2}{3}", BranchUserNamePrefix, channelPartnerId, ChexarSessionSeperator, userName);
                string branchPassword = string.Format("{0}{1}{2}{3}", BranchPasswordPrefix, channelPartnerId, ChexarSessionSeperator, password);
                string employeeUsername = string.Format("{0}{1}", EmployeeUserNamePrefix, channelPartnerId);
                string employeePassword = string.Format("{0}{1}", EmployeePasswordPrefix, channelPartnerId.ToString().PadLeft(4, '0'));

                ChexarLogin login = _chexar.GetChexarLogin(chexarPartner.URL, chexarPartner.Name, branchUsername, branchPassword, employeeUsername, employeePassword);

                ChexarSession ChexarSession = new ChexarSession()
                {
                    Location = chexarLocation,
                    BranchId = login.IngoBranchId,
                    CompanyToken = login.CompanyToken,
                    EmployeeId = login.EmployeeId,
                    ChexarPartnerId = chexarPartner.Id,
                    URL = chexarPartner.URL,
                    DTTerminalCreate = Helper.GetTimeZoneTime(timezone)
                };

                CreateOrUpdateChexarSession(ChexarSession);

                return mapper.Map<CheckLogin>(ChexarSession);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_GET_SESSION_FAILED, ex);
            }
        }

        public CheckProcessorInfo GetCheckProcessorInfo(long agentSessionId, string location)
        {
            try
            {
                CheckProcessorInfo checkProcessorInfo = null;

                StoredProcedure coreChexarProcedure = new StoredProcedure("usp_GetCheckProcessorInfo");

                coreChexarProcedure.WithParameters(InputParameter.Named("agentSessionId").WithValue(agentSessionId));
                coreChexarProcedure.WithParameters(InputParameter.Named("location").WithValue(location));

                using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreChexarProcedure))
                {
                    while (datareader.Read())
                    {
                        checkProcessorInfo = new CheckProcessorInfo();
                        string url = datareader.GetStringOrDefault("URL");
                        checkProcessorInfo.EmployeeId = datareader.GetInt32("EmployeeId");
                        checkProcessorInfo.Tocken = datareader.GetStringOrDefault("CompanyToken");
                        checkProcessorInfo.Url = string.IsNullOrWhiteSpace(url) ? string.Empty : string.Format("{0}{1}", url, "callcenterservice.asmx");
                    }
                }

                return checkProcessorInfo;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_GET_CHECK_PROCESSOR_INFO_FAILED, ex);
            }
        }

        public bool Cancel(long transactionId, ZeoContext context)
        {
            try
            {
                ChexarTransaction chexarTrx = GetChexrTransactionById(transactionId);

                // cannot access check while pending with Chexar, so do nothing
                if (chexarTrx.Status == CheckStatus.Pending)
                    return true;

                ChexarLogin login = GetChexarLoginDetails(context);

                // Cancel transactions other than failed. Failed transactions have invoice Id value of 0
                if (chexarTrx.InvoiceId > 0)
                {
                    cancelWithChexar(login, chexarTrx);
                    chexarTrx.Status = CheckStatus.Canceled;
                }

                chexarTrx.DTServerLastModified = DateTime.Now;
                chexarTrx.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);

                UpdateChexarTransactionStatus(chexarTrx);

                return true;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_CANCEL_TRANSACTION_FAILED, ex);
            }
        }

        public void Commit(long transactionId, ZeoContext context)
        {
            try
            {
                ChexarTransaction chxrTrx = GetChexrTransactionById(transactionId);

                ChexarLogin login = GetChexarLoginDetails(context);

                string error;
                bool completed = _chexar.CloseTransaction(login, chxrTrx.InvoiceId, out error);

                chxrTrx.Status = CheckStatus.Cashed;

                if (completed)
                    chxrTrx.ChexarStatus = "Completed";

                UpdateChexarTransactionStatus(chxrTrx);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_COMMIT_FAILED, ex);
            }
        }

        public CheckTransaction Get(long transactionId)
        {
            try
            {
                return mapper.Map<CheckTransaction>(GetChexrTransactionById(transactionId));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_GET_TRANSACTION_FAILED, ex);
            }
        }

        public CheckTransaction Status(long transactionId, ZeoContext context)
        {
            try
            {
                ChexarTransaction chxrTrx = GetChexrTransactionById(transactionId);

                if (chxrTrx.Status != CheckStatus.Pending || !getPendingStatusFromProcessor)
                    return mapper.Map<CheckTransaction>(chxrTrx);

                ChexarLogin login = GetChexarLoginDetails(context);

                string error;
                ChexarInvoiceCheck invoiceDetails = _chexar.GetTransactionStatus(login, chxrTrx.InvoiceId, out error);

                if (invoiceDetails.Badge == int.MinValue)
                {
                    ChxrIO.ChexarTicketStatus ticketStatus = _chexar.GetWaitTime(login, chxrTrx.TicketId);

                    if (ticketStatus != null)
                    {
                        chxrTrx.WaitTime = ticketStatus.WaitTime;
                        chxrTrx.Status = CheckStatus.Pending;
                    }
                }
                else if (invoiceDetails.Approved)
                {
                    chxrTrx.Status = CheckStatus.Approved;
                    chxrTrx.ChexarStatus = "Approved";
                    ChexarMICRDetails checkDetails = _chexar.GetMICRDetails(login, chxrTrx.InvoiceId, out error);
                    addChexarMicrDetails(chxrTrx, checkDetails);
                }
                else
                {
                    chxrTrx.Status = CheckStatus.Declined;
                    chxrTrx.ChexarStatus = "Declined";
                    addChexarDeclineDetails(chxrTrx, invoiceDetails);
                    cancelWithChexar(login, chxrTrx);
                    chxrTrx.DeclineMessageKey = ((int)Helper.ProductCode.CheckProcessing).ToString() + "." + ((int)Helper.ProviderId.Ingo).ToString() + "." + chxrTrx.DeclineCode;
                }

                chxrTrx.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);

                return UpdateChexarTransaction(chxrTrx, context.ChannelPartnerId);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_GET_STATUS_FAILED, ex);
            }
        }

        public CheckTransaction Submit(long accountId, CheckInfo check, ZeoContext context)
        {
            try
            {
                string chexarLocation = GenerateChexarLocationId(context.ChannelPartnerId, context.CheckUserName);

                ChexarTransaction chxrTrx = new ChexarTransaction
                {
                    Amount = check.Amount,
                    ChexarAmount = check.Amount,
                    Micr = check.Micr,
                    Latitude = check.Latitude,
                    Longitude = check.Longitude,
                    CheckDate = check.IssueDate,
                    ChexarAccountId = accountId,
                    ChexarStatus = "not submitted yet",
                    Location = chexarLocation,
                    DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone),
                    DTServerCreate = DateTime.Now
                };

                int badgeId = 0;
                int ChexarTypeId = 0;

                getChexarTypeAndBatchId(ref badgeId, ref ChexarTypeId, check.Type, accountId);

                chxrTrx.SubmitType = chxrTrx.ReturnType = ChexarTypeId;

                chxrTrx.Id = CreateChexarTransaction(chxrTrx);

                ChexarLogin login = GetChexarLoginDetails(context);

                int errorCode;
                string errorReason;

                ChexarNewInvoiceResult invoice = _chexar.CreateTransaction
                            (login, badgeId, check.Amount,
                            check.IssueDate, ChexarTypeId, "", "", "", check.Micr, check.FrontImage,
                            check.BackImage, check.ImageFormat, check.FrontImageTIF,
                            check.BackImageTIF, null,
                            out errorCode, out errorReason);


                if (errorCode != 0)  // If the error code is not Zero, either the check is declined or failed. As per the story no - AL-2013
                {
                    chxrTrx.DeclineCode = errorCode;
                    chxrTrx.Message = errorReason;

                    if (errorCode == -3 || errorCode == -2 || errorCode == -1)
                    {
                        chxrTrx.Status = CheckStatus.Declined;
                        chxrTrx.ChexarStatus = "Declined";
                        chxrTrx.InvoiceId = invoice.InvoiceNo;
                    }
                    else
                    {
                        chxrTrx.Status = CheckStatus.Failed;
                        chxrTrx.ChexarStatus = "Failed";
                    }
                }
                else           //   if the error code is Zero, the check could be approved, pending, declined or failed 
                {
                    chxrTrx.InvoiceId = invoice.InvoiceNo;
                    chxrTrx.TicketId = invoice.TicketNo;
                    chxrTrx.ChexarStatus = invoice.Status;
                    chxrTrx.Status = ConvertToFacade(invoice.Status, invoice.OnHold);
                }

                switch (chxrTrx.Status)
                {
                    case CheckStatus.Approved:
                        ChexarMICRDetails checkDetails = _chexar.GetMICRDetails(login, chxrTrx.InvoiceId, out errorReason);
                        addChexarMicrDetails(chxrTrx, checkDetails);
                        break;

                    case CheckStatus.Pending:
                        ChexarTicketStatus ticketStatus = _chexar.GetWaitTime(login, chxrTrx.TicketId);
                        if (ticketStatus != null)
                            chxrTrx.WaitTime = ticketStatus.WaitTime;
                        break;

                    case CheckStatus.Declined:
                        ChexarInvoiceCheck invoiceDetails = _chexar.GetTransactionStatus(login, chxrTrx.InvoiceId, out errorReason);
                        addChexarDeclineDetails(chxrTrx, invoiceDetails);
                        cancelWithChexar(login, chxrTrx);
                        chxrTrx.DeclineMessageKey = ((int)Helper.ProductCode.CheckProcessing).ToString() + "." + ((int)Helper.ProviderId.Ingo).ToString() + "." + chxrTrx.DeclineCode;
                        break;

                    default:
                        break;
                }

                return UpdateChexarTransaction(chxrTrx, context.ChannelPartnerId);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_SUBMIT_FAILED, ex);
            }
        }

        public void UpdateTransactionFranked(long transactionId, bool IsCheckFranked)
        {
            try
            {
                StoredProcedure coreChexarProcedure = new StoredProcedure("usp_UpdateTransactionFranked");
                coreChexarProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));
                coreChexarProcedure.WithParameters(InputParameter.Named("IsCheckFranked").WithValue(IsCheckFranked));
                DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreChexarProcedure);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_UPDATE_TRANSACTION_FAILED, ex);
            }
        }

        public long GetAccount(ZeoContext context)
        {
            try
            {
                ChexarAccount chxrAccount = GetCheckAccount(context.CustomerSessionId);

                if (chxrAccount != null && chxrAccount.Badge == 0)
                {
                    int batchId = Register(chxrAccount, context);
                    chxrAccount.Badge = batchId;
                    chxrAccount.DTServerCreate = DateTime.Now;
                    chxrAccount.DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone);

                    chxrAccount.Id = CreateChexarAccount(chxrAccount);
                }

                return chxrAccount.Id;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_GET_ACCOUNT_FAILED, ex);
            }
        }

        #endregion

        #region Private Method

        private int Register(ChexarAccount chxrAccount, ZeoContext context)
        {
            ChexarCustomerIO chexarCustomer = ConvertToChexar(chxrAccount);

            ChexarLogin login = GetChexarLoginDetails(context);

            string error;

            int badge = _chexar.RegisterNewCustomer(login, chexarCustomer, out error);

            return badge;
        }

        private string GenerateChexarLocationId(long channelPartnerId, string checkUserName)
        {
            return string.Format("{0}{1}{2}", channelPartnerId, ChexarSessionSeperator, checkUserName);
        }

        private void cancelWithChexar(ChexarLogin login, ChexarTransaction chexarTrx)
        {
            try
            {
                string chexarStatus = chexarTrx.ChexarStatus.ToLower();

                if (chexarStatus != "canceled" && chexarStatus != "pending")
                {
                    string error;
                    bool canceled = _chexar.CancelTransaction(login, chexarTrx.InvoiceId, out error);

                    if (canceled)
                        chexarTrx.ChexarStatus = "Canceled";
                    else
                        chexarTrx.Message = error;
                }
            }
            catch (Exception ex)
            {
                chexarTrx.Message = ex.Message;
            }
        }

        private ChexarCustomerIO ConvertToChexar(ChexarAccount chxrAccount)
        {
            var chexarCustomer = new ChexarCustomerIO
            {
                FName = chxrAccount.FirstName ?? string.Empty,
                LName = chxrAccount.LastName ?? string.Empty,
                Address1 = chxrAccount.Address1 ?? string.Empty,
                Address2 = chxrAccount.Address2 ?? string.Empty,
                City = chxrAccount.City ?? string.Empty,
                State = chxrAccount.State ?? string.Empty,
                Zip = chxrAccount.Zip ?? string.Empty,
                Phone = chxrAccount.Phone ?? string.Empty,
                Occupation = chxrAccount.Occupation ?? string.Empty,
                Employer = chxrAccount.Employer ?? string.Empty,
                EmployerPhone = chxrAccount.EmployerPhone ?? string.Empty,
                GovernmentId = chxrAccount.GovernmentId ?? string.Empty,
                IDType = ChxrIO.ChexarIDTypes.Unknown, // fix later
                IDCountry = chxrAccount.IDCardIssuedCountry ?? string.Empty,
                CardNumber = chxrAccount.CardNumber,
                CustomerScore = chxrAccount.CustomerScore == 0 ? 100 : chxrAccount.CustomerScore,
                DateOfBirth = chxrAccount.DateOfBirth,
                SSN = chxrAccount.SSN ?? string.Empty,
                IDImage = chxrAccount.IDCardImage,
                IDCode = chxrAccount.IDCode,
                CustomerId = chxrAccount.CustomerId,
                CustomerSessionId = chxrAccount.CustomerSessionId,
                ITIN = chxrAccount.ITIN ?? string.Empty
            };

            if (chxrAccount.IDCardExpireDate != null)
                chexarCustomer.IDExpDate = (DateTime)chxrAccount.IDCardExpireDate;
            if (chexarCustomer.IDCode == "I" && string.IsNullOrWhiteSpace(chexarCustomer.ITIN))
            {
                chexarCustomer.ITIN = chxrAccount.SSN;
                chexarCustomer.SSN = string.Empty;
            }
            return chexarCustomer;
        }

        public static CheckStatus ConvertToFacade(string status, bool onhold)
        {
            if (onhold) { return CheckStatus.Pending; }

            switch (status.ToLower())
            {
                case "approved":
                    return CheckStatus.Approved;

                case "failed":
                    return CheckStatus.Failed;

                default:
                    return CheckStatus.Declined;
            }
        }

        private void addChexarMicrDetails(ChexarTransaction chexarTrx, ChxrIO.ChexarMICRDetails checkDetails)
        {
            chexarTrx.CheckNumber = checkDetails.CheckNumber;
            chexarTrx.RoutingNumber = checkDetails.ABARoutingNumber;
            chexarTrx.AccountNumber = checkDetails.AccountNumber;
            chexarTrx.ChexarAmount = checkDetails.CheckAmount;
            chexarTrx.ChexarFee = checkDetails.FeeAmount;
            chexarTrx.ReturnType = checkDetails.CheckTypeId;
        }

        private void addChexarDeclineDetails(ChexarTransaction chexarTrx, ChxrIO.ChexarInvoiceCheck invoiceDetails)
        {
            if (chexarTrx.DeclineCode != -3 && chexarTrx.DeclineCode != -2 && chexarTrx.DeclineCode != -1)//AL-2013
            {
                if (invoiceDetails.CheckTypeId != 0)
                    chexarTrx.ReturnType = invoiceDetails.CheckTypeId;
                chexarTrx.DeclineCode = invoiceDetails.DeclineId;
                chexarTrx.Message = invoiceDetails.DeclineReason;
            }
        }

        private ChexarLogin GetChexarLoginDetails(ZeoContext context)
        {
            return new ChexarLogin
            {
                URL = context.URL,
                IngoBranchId = Convert.ToInt32(context.IngoBranchId),
                CompanyToken = context.CompanyToken,
                EmployeeId = context.EmployeeId
            };
        }


        private static IIO GetChexarIO()
        {
            string checkProcessor = ConfigurationManager.AppSettings["CheckProcessor"].ToString();

            if (checkProcessor.ToUpper() == "IO")
                return new IO();
            else
                return new SimulatorIO();

        }

        #endregion

        #region using P3 dot net code changes

        private long CreateChexarTransaction(ChexarTransaction chxrTrx)
        {
            StoredProcedure coreChexarProcedure = new StoredProcedure("usp_CreateChexarTransaction");
            coreChexarProcedure.WithParameters(OutputParameter.Named("ChxrTransactionId").OfType<long>());
            coreChexarProcedure.WithParameters(InputParameter.Named("Location").WithValue(chxrTrx.Location));
            coreChexarProcedure.WithParameters(InputParameter.Named("Amount").WithValue(chxrTrx.Amount));
            coreChexarProcedure.WithParameters(InputParameter.Named("CheckDate").WithValue(chxrTrx.CheckDate));
            coreChexarProcedure.WithParameters(InputParameter.Named("ChexarAccountId").WithValue(chxrTrx.ChexarAccountId));
            coreChexarProcedure.WithParameters(InputParameter.Named("ChexarStatus").WithValue(chxrTrx.ChexarStatus));
            coreChexarProcedure.WithParameters(InputParameter.Named("Status").WithValue((int)chxrTrx.Status));
            coreChexarProcedure.WithParameters(InputParameter.Named("Latitude").WithValue(chxrTrx.Latitude));
            coreChexarProcedure.WithParameters(InputParameter.Named("Longitude").WithValue(chxrTrx.Longitude));
            coreChexarProcedure.WithParameters(InputParameter.Named("Micr").WithValue(chxrTrx.Micr));
            coreChexarProcedure.WithParameters(InputParameter.Named("SubmitType").WithValue(chxrTrx.SubmitType));
            coreChexarProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(chxrTrx.DTServerCreate));
            coreChexarProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(chxrTrx.DTTerminalCreate));

            int trxCount = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreChexarProcedure);

            return Convert.ToInt64(coreChexarProcedure.Parameters["ChxrTransactionId"].Value);

        }

        private CheckTransaction UpdateChexarTransaction(ChexarTransaction chxrTrx, long channelPartnerId)
        {
            StoredProcedure coreChexarProcedure = new StoredProcedure("usp_UpdateChexarTransactionById");

            coreChexarProcedure.WithParameters(InputParameter.Named("chxrTransactionId").WithValue(chxrTrx.Id));
            coreChexarProcedure.WithParameters(InputParameter.Named("AccountNumber").WithValue(chxrTrx.AccountNumber));
            coreChexarProcedure.WithParameters(InputParameter.Named("Amount").WithValue(chxrTrx.Amount));
            coreChexarProcedure.WithParameters(InputParameter.Named("CheckNumber").WithValue(chxrTrx.CheckNumber));
            coreChexarProcedure.WithParameters(InputParameter.Named("ChexarAmount").WithValue(chxrTrx.ChexarAmount));
            coreChexarProcedure.WithParameters(InputParameter.Named("ChexarFee").WithValue(chxrTrx.ChexarFee));
            coreChexarProcedure.WithParameters(InputParameter.Named("ChexarStatus").WithValue(chxrTrx.ChexarStatus));
            coreChexarProcedure.WithParameters(InputParameter.Named("DeclineCode").WithValue(chxrTrx.DeclineCode));
            coreChexarProcedure.WithParameters(InputParameter.Named("Latitude").WithValue(chxrTrx.Latitude));
            coreChexarProcedure.WithParameters(InputParameter.Named("Longitude").WithValue(chxrTrx.Longitude));
            coreChexarProcedure.WithParameters(InputParameter.Named("ReturnType").WithValue(chxrTrx.ReturnType));
            coreChexarProcedure.WithParameters(InputParameter.Named("RoutingNumber").WithValue(chxrTrx.RoutingNumber));
            coreChexarProcedure.WithParameters(InputParameter.Named("Status").WithValue((int)chxrTrx.Status));
            coreChexarProcedure.WithParameters(InputParameter.Named("SubmitType").WithValue(chxrTrx.SubmitType));
            coreChexarProcedure.WithParameters(InputParameter.Named("TicketId").WithValue(chxrTrx.TicketId));
            coreChexarProcedure.WithParameters(InputParameter.Named("InvoiceId").WithValue(chxrTrx.InvoiceId));
            coreChexarProcedure.WithParameters(InputParameter.Named("WaitTime").WithValue(chxrTrx.WaitTime));
            coreChexarProcedure.WithParameters(InputParameter.Named("dTServerLastModified").WithValue(DateTime.Now));
            coreChexarProcedure.WithParameters(InputParameter.Named("dTTerminalLastModified").WithValue(chxrTrx.DTTerminalLastModified));
            coreChexarProcedure.WithParameters(InputParameter.Named("DeclineMessageKey").WithValue(chxrTrx.DeclineMessageKey));
            coreChexarProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));
            coreChexarProcedure.WithParameters(InputParameter.Named("lang").WithValue((int)Helper.Language.English));

            CheckTransaction checkTrx = mapper.Map<CheckTransaction>(chxrTrx);

            using (IDataReader dataReader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreChexarProcedure))
            {
                while (dataReader.Read())
                {
                    checkTrx.DeclineMessage = dataReader.GetStringOrDefault("declineMessage");
                    checkTrx.SubmitType = (CheckType)dataReader.GetInt32OrDefault("SubmitType");   // DMS check Submit type
                    checkTrx.ReturnType = (CheckType)dataReader.GetInt32OrDefault("ReturnType");  // DMS check Return type
                }
            }

            return checkTrx;
        }

        private void UpdateChexarTransactionStatus(ChexarTransaction chxrTrx)
        {
            StoredProcedure coreChexarProcedure = new StoredProcedure("usp_UpdateChexarTransactionStatus");

            coreChexarProcedure.WithParameters(InputParameter.Named("TransactionId").WithValue(chxrTrx.Id));
            coreChexarProcedure.WithParameters(InputParameter.Named("ChexarStatus").WithValue(chxrTrx.ChexarStatus));
            coreChexarProcedure.WithParameters(InputParameter.Named("Status").WithValue((int)chxrTrx.Status));
            coreChexarProcedure.WithParameters(InputParameter.Named("DTServerLastModified").WithValue(chxrTrx.DTServerLastModified));
            coreChexarProcedure.WithParameters(InputParameter.Named("DTTerminalLastModified").WithValue(chxrTrx.DTTerminalLastModified));

            DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreChexarProcedure);
        }

        private ChexarTransaction GetChexrTransactionById(long transactionId)
        {
            StoredProcedure coreChexarProcedure = new StoredProcedure("usp_GetChexarTransaction");

            coreChexarProcedure.WithParameters(InputParameter.Named("transactionId").WithValue(transactionId));

            ChexarTransaction chxrtrx = DataConnectionHelper.GetConnectionManager().ExecuteQueryWithResult<ChexarTransaction>(coreChexarProcedure, dr => new ChexarTransaction
            {
                AccountNumber = dr.GetStringOrDefault("AccountNumber"),
                Amount = dr.GetDecimalOrDefault("Amount"),
                CheckDate = dr.GetDateTimeOrDefault("CheckDate"),
                CheckNumber = dr.GetStringOrDefault("CheckNumber"),
                ChexarAccountId = dr.GetInt64OrDefault("ChxrAccountId"),
                ChexarAmount = dr.GetDecimalOrDefault("ChexarAmount"),
                ChexarFee = dr.GetDecimalOrDefault("ChexarFee"),
                ChexarStatus = dr.GetStringOrDefault("ChexarStatus"),
                DeclineCode = dr.GetInt32OrDefault("DeclineCode"),
                Message = dr.GetStringOrDefault("Message"),
                InvoiceId = dr.GetInt32OrDefault("InvoiceId"),
                IsCheckFranked = dr.GetBooleanOrDefault("IsCheckFranked"),
                Latitude = dr.GetDoubleOrDefault("Latitude"),
                Longitude = dr.GetDoubleOrDefault("Longitude"),
                Location = dr.GetStringOrDefault("Location"),
                Micr = dr.GetStringOrDefault("Micr"),
                ReturnType = dr.GetInt32OrDefault("ReturnType"),
                RoutingNumber = dr.GetStringOrDefault("RoutingNumber"),
                Status = (CheckStatus)Enum.Parse(typeof(CheckStatus), dr.GetStringOrDefault("Status")),
                SubmitType = dr.GetInt32OrDefault("SubmitType"),
                TicketId = dr.GetInt32OrDefault("TicketId"),
                WaitTime = dr.GetStringOrDefault("WaitTime"),
                DiscountDescription = dr.GetStringOrDefault("DiscountDescription"),
                DiscountApplied = dr.GetDecimalOrDefault("DiscountApplied"),
                DiscountName = dr.GetStringOrDefault("DiscountName"),
                DmsReturnType = (CheckType)dr.GetInt32OrDefault("DmsReturnType"),
                BaseFee = dr.GetDecimalOrDefault("BaseFee"),
                ImageFront = (dr["FrontImage"] != DBNull.Value) ? (byte[])dr["FrontImage"] : null,
                Fee = dr.GetDecimalOrDefault("Fee"),
                Id = transactionId,
            });

            return chxrtrx;

        }

        private ChexarAccount GetCheckAccount(long customerSessionId)
        {
            StoredProcedure coreChexarProcedure = new StoredProcedure("usp_GetChexarAccount");

            coreChexarProcedure.WithParameters(InputParameter.Named("customerSessionId").WithValue(customerSessionId));

            ChexarAccount chxrAccount = DataConnectionHelper.GetConnectionManager().ExecuteQueryWithResult<ChexarAccount>(coreChexarProcedure, dr => new ChexarAccount
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
                Badge = dr.GetInt32OrDefault("Badge"),
                Id = dr.GetInt64OrDefault("ChxrAccountId"),
                CustomerId = dr.GetInt64OrDefault("CustomerId"),
                CustomerSessionId = customerSessionId
            });

            if (chxrAccount.IDCode == "I")
            {
                chxrAccount.ITIN = chxrAccount.SSN;
                chxrAccount.SSN = string.Empty;
            }

            return chxrAccount;
        }

        private long CreateChexarAccount(ChexarAccount chxrAccount)
        {
            StoredProcedure coreChexarProcedure = new StoredProcedure("usp_CreateChexarAccount");

            coreChexarProcedure.WithParameters(OutputParameter.Named("ChxrAccountId").OfType<long>());
            coreChexarProcedure.WithParameters(InputParameter.Named("BadgeId").WithValue(chxrAccount.Badge));
            coreChexarProcedure.WithParameters(InputParameter.Named("CustomerId").WithValue(chxrAccount.CustomerId));
            coreChexarProcedure.WithParameters(InputParameter.Named("CustomerSessionId").WithValue(chxrAccount.CustomerSessionId));
            coreChexarProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(chxrAccount.DTServerCreate));
            coreChexarProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(chxrAccount.DTTerminalCreate));

            DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreChexarProcedure);

            return Convert.ToInt64(coreChexarProcedure.Parameters["ChxrAccountId"].Value);
        }

        private ChexarPartner GetChexarPartnerByChannelPartnerId(long channelPartnerId)
        {
            StoredProcedure coreChexarProcedure = new StoredProcedure("usp_GetChxrPartnerByChannelPartnerId");

            coreChexarProcedure.WithParameters(InputParameter.Named("channelPartnerId").WithValue(channelPartnerId));

            ChexarPartner chexarPartner = DataConnectionHelper.GetConnectionManager().ExecuteQueryWithResult<ChexarPartner>(coreChexarProcedure, dr => new ChexarPartner
            {
                Id = dr.GetInt64("ChxrPartnerID"),
                Name = dr.GetStringOrDefault("Name"),
                URL = dr.GetStringOrDefault("URL")
            });

            return chexarPartner;
        }

        private bool CreateOrUpdateChexarSession(ChexarSession chexarSession)
        {
            StoredProcedure coreChexarProcedure = new StoredProcedure("usp_CreateOrUpdateChexarSession");

            coreChexarProcedure.WithParameters(InputParameter.Named("Location").WithValue(chexarSession.Location));
            coreChexarProcedure.WithParameters(InputParameter.Named("EmployeeId").WithValue(chexarSession.EmployeeId));
            coreChexarProcedure.WithParameters(InputParameter.Named("CompanyToken").WithValue(chexarSession.CompanyToken));
            coreChexarProcedure.WithParameters(InputParameter.Named("BranchId").WithValue(chexarSession.BranchId));
            coreChexarProcedure.WithParameters(InputParameter.Named("ChexarPartnerId").WithValue(chexarSession.ChexarPartnerId));
            coreChexarProcedure.WithParameters(InputParameter.Named("DTServerCreate").WithValue(DateTime.Now));
            coreChexarProcedure.WithParameters(InputParameter.Named("DTTerminalCreate").WithValue(chexarSession.DTTerminalCreate));

            int isExecute = DataConnectionHelper.GetConnectionManager().ExecuteNonQuery(coreChexarProcedure);
            return (isExecute == 0) ? false : true;
        }

        private void getChexarTypeAndBatchId(ref int batchId, ref int chexarTypeId, CheckType type, long chxrAccountId)
        {
            StoredProcedure coreChexarProcedure = new StoredProcedure("usp_GetChexarTypeAndBadgeId");

            coreChexarProcedure.WithParameters(InputParameter.Named("CheckType").WithValue((int)type));
            coreChexarProcedure.WithParameters(InputParameter.Named("chxrAccountId").WithValue(chxrAccountId));

            using (IDataReader datareader = DataConnectionHelper.GetConnectionManager().ExecuteReader(coreChexarProcedure))
            {
                while (datareader.Read())
                {
                    batchId = datareader.GetInt32OrDefault("BadgeId");
                    chexarTypeId = datareader.GetInt32OrDefault("ChexarTypeId");
                }
            }
        }

        public List<PendingCheck> GetPendingChecks()
        {
            try
            {
                StoredProcedure coreChexarProcedure = new StoredProcedure("usp_GetAllChexarPendingChecks");

                List<PendingCheck> pendingChecks = DataConnectionHelper.GetConnectionManager().ExecuteQueryWithResults<PendingCheck>(coreChexarProcedure, dr => new PendingCheck
                {
                    TransactionId = dr.GetInt64OrDefault("TransactionId"),
                    CxnTrasactionId = dr.GetInt64OrDefault("CxnTrasactionId"),
                    CustomerSessionId = dr.GetInt64OrDefault("CustomerSessionId"),
                    CheckUserName = dr.GetStringOrDefault("UserName"),
                    CheckPassword = dr.GetStringOrDefault("Password"),
                    DiscountName = dr.GetStringOrDefault("DiscountName"),
                    IsSystemApplied = dr.GetBooleanOrDefault("IsSystemApplied"),
                    ChannelPartnerId = dr.GetInt32OrDefault("ChannelPartnerId"),
                    ChannelPartnerName = dr.GetStringOrDefault("ChannelPartnerName"),
                    LocationTimeZone = dr.GetStringOrDefault("LocationTimeZone"),
                    CustomerId = dr.GetInt64OrDefault("CustomerID"),
                    LocationIdentifier = dr.GetStringOrDefault("LocationIdentifier")
                }).ToList();

                return pendingChecks;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_PENDING_CHECK_FAILED, ex);
            }
        }

        #endregion

    }
}
