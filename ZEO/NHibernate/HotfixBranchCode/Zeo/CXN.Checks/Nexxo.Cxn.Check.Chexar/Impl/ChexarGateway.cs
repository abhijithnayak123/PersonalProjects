// -----------------------------------------------------------------------
// <copyright file="ChexarChequeProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using MGI.Common.DataAccess.Contract;
using MGI.Common.Sys;

using MGI.Cxn.Check.Contract;
using CxnCheckData = MGI.Cxn.Check.Data;
using MGI.Cxn.Check.Chexar.Data;
using MGI.Cxn.Check.Chexar.Contract;

using ChexarIO;
using MGI.TimeStamp;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;
namespace MGI.Cxn.Check.Chexar.Impl
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChexarGateway : ICheckProcessor, IChexarPartnerConfigurator
    {
        #region Dependencies

        #region Repositories
        private IRepository<ChexarPartner> _partnerRepo;

        public IRepository<ChexarPartner> PartnerRepo
        {
            get { return _partnerRepo; }
            set { _partnerRepo = value; }
        }

        private IRepository<ChexarAccount> _accountRepo;

        public IRepository<ChexarAccount> AccountRepo
        {
            get { return _accountRepo; }
            set { _accountRepo = value; }
        }
        private IRepository<ChexarTrx> _chxrTrxRepo;

        public IRepository<ChexarTrx> ChxrTrxRepo
        {
            get { return _chxrTrxRepo; }
            set { _chxrTrxRepo = value; }
        }


        private IRepository<CheckImage> _imgRepo;

        public IRepository<CheckImage> ImgRepo
        {
            get { return _imgRepo; }
            set { _imgRepo = value; }
        }

        private IRepository<ChexarSession> _chxrSessionRepo;

        public IRepository<ChexarSession> ChxrSessionRepo
        {
            get { return _chxrSessionRepo; }
            set { _chxrSessionRepo = value; }
        }

        private IReadOnlyRepository<ChexarCheckTypeMapping> _checkTypeMappingRepo;
        public IReadOnlyRepository<ChexarCheckTypeMapping> CheckTypeMappingRepo
        {
            get { return _checkTypeMappingRepo; }
            set { _checkTypeMappingRepo = value; }
        }

        public string BranchUserNamePrefix { private get; set; }
        public string BranchPasswordPrefix { private get; set; }
        public string EmployeeUserNamePrefix { private get; set; }
        public string EmployeePasswordPrefix { private get; set; }
        public string ChexarSessionSeperator { private get; set; }

        #endregion

        private IChexarWebSvc _chexar;

        public IChexarWebSvc Chexar
        {
            get { return _chexar; }
            set { _chexar = value; }
        }

        private bool getPendingStatusFromProcessor;

        public bool GetPendingStatusFromProcessor { set { getPendingStatusFromProcessor = value; } }
        public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }

        #endregion

        public ChexarGateway()
        {
            NLogger = new NLoggerCommon();
        }

        #region ChexarProcessorImplementation

        #region Trx'al
        public CxnCheckData.CheckStatus Submit(long trxId, long accountId, CxnCheckData.CheckInfo check, MGIContext mgiContext)
        {
            string chexarLocation = generateChexarLocationId(PartnerId(mgiContext), IngoUserName(mgiContext));
            string timezone = mgiContext.TimeZone != null ? mgiContext.TimeZone : string.Empty;

            if (string.IsNullOrEmpty(timezone))
                throw new Exception("Time zone not provided in the context");

            int frontImageTIFFLength = check.FrontImageTIF == null ? 0 : check.FrontImageTIF.Length;
            int backImageTIFFLength = check.BackImageTIF == null ? 0 : check.BackImageTIF.Length;

            NLogger.Info(string.Format("Front Image Size of TIFF is {0}", frontImageTIFFLength.ToString()));
            NLogger.Info(string.Format("Back Image Size of TIFF is {0}", backImageTIFFLength.ToString()));

            NLogger.Info(LogImageDetails(check.FrontImageTIF, "TIFF Front Image"));

            NLogger.Info(LogImageDetails(check.BackImageTIF, "TIFF Back Image"));
            NLogger.Info(LogImageDetails(check.FrontImage, "JPEG Front Image"));
            NLogger.Info(LogImageDetails(check.BackImage, "JPEG Back Image"));

            ChexarTrx chxrTrx = ChexarMapper.ConvertToCxn(check);
            chxrTrx.SubmitType = getChexarType(check.Type);

            //update the return type irresepective of the check status. If Approved then Return Type will be updated below.
            chxrTrx.ReturnType = getChexarType(check.Type);

            chxrTrx.Id = trxId;
            chxrTrx.Location = chexarLocation;
            //timestamp changes
            chxrTrx.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            chxrTrx.DTServerCreate = DateTime.Now;
            chxrTrx.ChexarStatus = "not submitted yet";
            // Karun@10/14/2013: added PartnerId to ChexarTrx
            chxrTrx.ChannelPartnerID = PartnerId(mgiContext);


            ChexarAccount account = _accountRepo.FindBy(x => x.Id == accountId);

            if (account == null)
                throw new CheckException(CheckException.ACCOUNT_NOT_FOUND, string.Format("could not find account {0}", accountId));

            chxrTrx.Account = account;
            _chxrTrxRepo.AddWithFlush(chxrTrx); // add to the local repo.

            CheckImage chxrImages = ChexarMapper.ConvertToCheckImage(check);
            //timestamp changes
            chxrImages.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            chxrImages.DTServerCreate = DateTime.Now;

            chxrImages.Trx = chxrTrx;
            _imgRepo.AddWithFlush(chxrImages);

            string userName = mgiContext.CheckUserName;
            string password = mgiContext.CheckPassword;

            //AL-681: The GetChexarSession method we are now calling from Web and passing in context 
            ChexarIO.ChexarLogin login = new ChexarIO.ChexarLogin
            {
                URL = mgiContext.URL,
                IngoBranchId = Convert.ToInt32(mgiContext.IngoBranchId),
                CompanyToken = mgiContext.CompanyToken,
                EmployeeId = mgiContext.EmployeeId
            };
            //ChexarIO.ChexarLogin login = GetChexarSession(PartnerId(context), chexarLocation, userName, password, timezone);

            int errorCode;
            string errorReason;

            NLogger.Info(string.Format("login.BranchId {0}", login.IngoBranchId.ToString()));
            NLogger.Info(string.Format("login.CompanyToken {0}", login.CompanyToken.ToString()));
            NLogger.Info(string.Format("login.EmployeeId {0}", login.EmployeeId.ToString()));
            NLogger.Info(string.Format("login.URL {0}", login.URL.ToString()));
            NLogger.Info(string.Format("account.Badge {0}", account.Badge.ToString()));
            NLogger.Info(string.Format("account.Badge {0}", account.Badge.ToString()));


            ChexarNewInvoiceResult invoice = _chexar.CreateTransaction
                    (login, account.Badge, check.Amount,
                    check.IssueDate, getChexarType(check.Type), "", "", "", check.Micr, check.FrontImage,
                    check.BackImage, check.ImageFormat, check.FrontImageTIF,
                    check.BackImageTIF, null,
                    out errorCode, out errorReason);

            if (errorCode != 0)
            {
                if (errorCode == -3 || errorCode == -2 || errorCode == -1)//AL-2013
                {
                    chxrTrx.Status = CxnCheckData.CheckStatus.Declined;
                    chxrTrx.ChexarStatus = "Declined";
                    chxrTrx.DeclineCode = errorCode;
                    chxrTrx.Message = errorReason;
                    //chxrTrx.InvoiceId = invoice.InvoiceNo;//(AL-4125)commented this line since Ingo don't send any invoice number for instant declines, so by default chxrTrx.InvoiceId will be considered as 0

					//AL-3371 Transactional Log User Story(Process check)
					MongoDBLogger.Error<ChexarTrx>(chxrTrx, "Submit", AlloyLayerName.CXN, ModuleName.ProcessCheck,
										"Error in Submit - MGI.Cxn.Check.Certegy.Impl.Gateway", "Error - Declined", string.Empty);
				
				}
                else
                {
                    //throw new CheckException(CheckException.PROVIDER_ERROR, new ChexarProviderException(errorCode, errorReason));
                    chxrTrx.Status = CxnCheckData.CheckStatus.Failed;
                    chxrTrx.ChexarStatus = "Failed";
                    chxrTrx.DeclineCode = errorCode;
                    chxrTrx.Message = errorReason;

					//AL-3371 Transactional Log User Story(Process check)
					MongoDBLogger.Error<ChexarTrx>(chxrTrx, "Submit", AlloyLayerName.CXN, ModuleName.ProcessCheck,
										"Error in Submit - MGI.Cxn.Check.Certegy.Impl.Gateway", "Error - Failed", string.Empty);
					
				}
            }
            else
            {
                chxrTrx.InvoiceId = invoice.InvoiceNo;
                chxrTrx.TicketId = invoice.TicketNo;
                chxrTrx.ChexarStatus = invoice.Status;
                chxrTrx.Status = ChexarMapper.ConvertToFacade(invoice.Status, invoice.OnHold); // convert invoice.Status
            }

            if (chxrTrx.Status == CxnCheckData.CheckStatus.Approved)
            {
                ChexarMICRDetails checkDetails = _chexar.GetMICRDetails(login, chxrTrx.InvoiceId, out errorReason);

                if (checkDetails == null)
                    throw new CheckException(CheckException.PROVIDER_ERROR, new ChexarProviderException(ChexarProviderException.PROVIDER_SUB_ERROR + errorCode, errorReason));

                addChexarMicrDetails(chxrTrx, checkDetails);
            }
            else if (chxrTrx.Status == CxnCheckData.CheckStatus.Declined)
            {
                ChexarInvoiceCheck invoiceDetails = _chexar.GetTransactionStatus(login, chxrTrx.InvoiceId, out errorReason);
                addChexarDeclineDetails(chxrTrx, invoiceDetails);
                cancelWithChexar(login, chxrTrx);
            }

            chxrTrx.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            chxrTrx.DTServerLastModified = DateTime.Now; //added for TimeStamp

            _chxrTrxRepo.UpdateWithFlush(chxrTrx); // update the attributes we got back from Chexar.

            return chxrTrx.Status;
        }

        public void Commit(long trxId, string timezone, MGIContext mgiContext)
        {
            ChexarTrx chexarTrx = getChexarTrx(trxId);

            string userName = mgiContext.CheckUserName;
            string password = mgiContext.CheckPassword;

            ChexarIO.ChexarLogin login = new ChexarIO.ChexarLogin
            {
                URL = mgiContext.URL,
                IngoBranchId = Convert.ToInt32(mgiContext.IngoBranchId),
                CompanyToken = mgiContext.CompanyToken,
                EmployeeId = mgiContext.EmployeeId
            };

            string error;
            bool completed = _chexar.CloseTransaction(login, chexarTrx.InvoiceId, out error);

            chexarTrx.Status = CxnCheckData.CheckStatus.Cashed;

            if (completed)
                chexarTrx.ChexarStatus = "Completed";
            else
                chexarTrx.Message = error;

            // before you update it, keep a history of the old record.
            /// would like to do it through an interceptor or enverse or something similar 
            /// so that the code doesn't have to do it.

            _chxrTrxRepo.UpdateWithFlush(chexarTrx);
        }

        public CxnCheckData.CheckTrx Get(long trxId)
        {
            ChexarTrx chexarTrx = getChexarTrx(trxId);

            CxnCheckData.CheckTrx checkTrx = ChexarMapper.ConvertToCheckTrx(chexarTrx);

            checkTrx.SubmitType = getCheckType(chexarTrx.SubmitType);
            checkTrx.ReturnType = getCheckType(chexarTrx.ReturnType);

            return checkTrx;
        }

        public CxnCheckData.CheckStatus Status(long trxId, string timezone, MGIContext mgiContext)
        {
            ChexarTrx chexarTrx = getChexarTrx(trxId);

            // if not pending, return currently stored status
            if (chexarTrx.Status != CxnCheckData.CheckStatus.Pending || !getPendingStatusFromProcessor)
                return chexarTrx.Status;

            string userName = mgiContext.CheckUserName;
            string password = mgiContext.CheckPassword;

            //ChexarLogin login = GetChexarSession(chexarTrx.ChannelPartnerID ?? long.MinValue, chexarTrx.Location, userName, password, timezone);

            ChexarIO.ChexarLogin login = new ChexarIO.ChexarLogin
            {
                URL = mgiContext.URL,
                IngoBranchId = Convert.ToInt32(mgiContext.IngoBranchId),
                CompanyToken = mgiContext.CompanyToken,
                EmployeeId = mgiContext.EmployeeId
            };
            string error;
            ChexarIO.ChexarInvoiceCheck invoiceDetails = _chexar.GetTransactionStatus(login, chexarTrx.InvoiceId, out error);

            if (invoiceDetails.Badge == int.MinValue)
            {
                ChexarTicketStatus ticketStatus = _chexar.GetWaitTime(login, chexarTrx.TicketId);
                //        User Story Number: - | Biz |   Fixed by: Sunil Shetty      Date: 13.05.2015
                //        Purpose: On not having null check, object refrence error was reported by Chandra while running CPMonitor
                if (ticketStatus != null)
                {
                    chexarTrx.WaitTime = ticketStatus.WaitTime;
                    chexarTrx.Status = CxnCheckData.CheckStatus.Pending;
                }
            }
            else if (invoiceDetails.Approved)
            {
                chexarTrx.Status = CxnCheckData.CheckStatus.Approved;
                chexarTrx.ChexarStatus = "Approved";

                ChexarMICRDetails checkDetails = _chexar.GetMICRDetails(login, chexarTrx.InvoiceId, out error);
                addChexarMicrDetails(chexarTrx, checkDetails);
            }
            else
            {
                chexarTrx.Status = CxnCheckData.CheckStatus.Declined;
                chexarTrx.ChexarStatus = "Declined";
                addChexarDeclineDetails(chexarTrx, invoiceDetails);
                cancelWithChexar(login, chexarTrx);
            }

            Console.WriteLine("status: {0}", chexarTrx.Status);
            Console.WriteLine("wait time: {0}", chexarTrx.WaitTime);

            _chxrTrxRepo.UpdateWithFlush(chexarTrx);

            return chexarTrx.Status;
        }

        public bool Cancel(long trxId, string timezone, MGIContext mgiContext)
        {
            ChexarTrx chexarTrx = getChexarTrx(trxId);

            if (chexarTrx.Status == CxnCheckData.CheckStatus.Pending)
            {
                // cannot access check while pending with Chexar, so do nothing
                return true;
            }

            string userName = mgiContext.CheckUserName;
            string password = mgiContext.CheckPassword;

            ChexarIO.ChexarLogin login = new ChexarIO.ChexarLogin
            {
                URL = mgiContext.URL,
                IngoBranchId = Convert.ToInt32(mgiContext.IngoBranchId),
                CompanyToken = mgiContext.CompanyToken,
                EmployeeId = mgiContext.EmployeeId
            };

            // Cancel transactions other than failed. Failed transactions have invoice Id value of 0
            if (chexarTrx.InvoiceId > 0)
            {
                cancelWithChexar(login, chexarTrx);
                chexarTrx.Status = CxnCheckData.CheckStatus.Canceled;
            }

            _chxrTrxRepo.UpdateWithFlush(chexarTrx);
            return true;

        }

        public void Close(long trxId)
        {
            throw new NotImplementedException();
        }


        public CxnCheckData.CheckProcessorInfo GetCheckProcessorInfo(string locationId)
        {
            ChexarSession session = _chxrSessionRepo.FindBy(x => x.Location == locationId);
            if (session != null)
            {
                return new CxnCheckData.CheckProcessorInfo()
                {
                    EmployeeId = session.EmployeeId,
                    Tocken = session.CompanyToken,
                    Url = session.Partner != null ? string.Format("{0}{1}", session.Partner.URL, "callcenterservice.asmx") : string.Empty
                };
            }
            return null;
        }

        public void UpdateTransactionFranked(long trxId)
        {
            ChexarTrx chexarTrx = getChexarTrx(trxId);

            chexarTrx.IsCheckFranked = true;

            _chxrTrxRepo.Update(chexarTrx);
        }

        #endregion


        #region Monitor
        public List<CxnCheckData.CheckTrx> PendingChecks()
        {
            var pendingChexarChecks = _chxrTrxRepo.FilterBy(c => c.Status == CxnCheckData.CheckStatus.Pending).ToList();
            var pendingChecks = new List<CxnCheckData.CheckTrx>();
            pendingChexarChecks.ForEach(c => pendingChecks.Add(ChexarMapper.ConvertToCheckTrx(c)));
            return pendingChecks;
        }
        #endregion


        #region Chexar Account mthds
        public long Register(MGI.Cxn.Check.Data.CheckAccount account, MGIContext mgiContext, string timezone)
        {
            // covert the CheckAccount to ChexarAccount 
            ChexarAccount chxrAccount = ChexarMapper.ConvertToCxn(account);
            // add the account to cxn Db.
            chxrAccount.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            chxrAccount.DTServerCreate = DateTime.Now;
            _accountRepo.AddWithFlush(chxrAccount); // is this persisted, if the chexar-io failed?? TBD - find out...

            string userName = mgiContext.CheckUserName;
            string password = mgiContext.CheckPassword;

            ChexarCustomerIO chexarCustomer = ChexarMapper.ConvertToChexar(account);

            ChexarIO.ChexarLogin login = new ChexarIO.ChexarLogin
            {
                URL = mgiContext.URL,
                IngoBranchId = Convert.ToInt32(mgiContext.IngoBranchId),
                CompanyToken = mgiContext.CompanyToken,
                EmployeeId = mgiContext.EmployeeId
            };
            //ChexarIO.ChexarLogin login = GetChexarSession(PartnerId(context), generateChexarLocationId(PartnerId(context), IngoUserName(context)), userName, password, timezone);

            string error;
            int badge = _chexar.RegisterNewCustomer(login, chexarCustomer, out error);

            if (badge == 0)
                throw new CheckException(CheckException.PROVIDER_ERROR, new ChexarProviderException(CheckException.CHEXAR_BADGE_NOT_CREATED, error));

            chxrAccount.Badge = badge;
            //Changes for timestamp
            chxrAccount.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            chxrAccount.DTServerLastModified = DateTime.Now;

            _accountRepo.AddWithFlush(chxrAccount);

            return chxrAccount.Id;
        }

        public void Update(MGI.Cxn.Check.Data.CheckAccount account, MGIContext mgiContext)
        {
            string timezone = GetTimeZone(mgiContext);

            ChexarAccount chxrAccount = _accountRepo.FindBy(x => x.Id == account.Id);

            chxrAccount.FirstName = account.FirstName;
            chxrAccount.LastName = account.LastName;
            chxrAccount.Address1 = account.Address1;
            chxrAccount.City = account.City;
            chxrAccount.State = account.State;
            chxrAccount.Zip = account.Zip;
            chxrAccount.DateOfBirth = account.DateOfBirth;
            chxrAccount.Phone = account.Phone;
            chxrAccount.SSN = account.SSN;
            chxrAccount.Occupation = account.Occupation;
            chxrAccount.Employer = account.Employer;
            chxrAccount.EmployerPhone = account.EmployerPhone;
            chxrAccount.GovernmentId = account.GovernmentId;
            chxrAccount.IDCardIssuedCountry = account.IDCountry;
            chxrAccount.IDCardExpireDate = account.IDExpireDate;
            chxrAccount.IDCardType = account.IDType;
            chxrAccount.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            chxrAccount.DTServerLastModified = DateTime.Now;
            chxrAccount.IDCardIssuedDate = account.IDIssueDate;
			chxrAccount.IDCode = account.IDCode;
			if(chxrAccount.IDCode == "I")
			{
				chxrAccount.ITIN = account.SSN;
				chxrAccount.SSN = string.Empty;
			}
			else if(chxrAccount.IDCode == "S")
			{
				chxrAccount.ITIN = string.Empty;
			}
            _accountRepo.Update(chxrAccount);
        }

        public Cxn.Check.Data.CheckAccount GetAccount(long accountId)
        {
            ChexarAccount chxrAccount = _accountRepo.FindBy(x => x.Id == accountId);

            return ChexarMapper.ConvertToCheck(chxrAccount);
        }

        public void Update(Cxn.Check.Data.CheckTrx checkTrx, MGIContext mgiContext)
        {
            ChexarTrx chexarTrx = getChexarTrx(checkTrx.Id);
            chexarTrx.Message = checkTrx.DeclineMessage;
            _chxrTrxRepo.UpdateWithFlush(chexarTrx);
        }

        #endregion
        #endregion

        #region Partner Setup
        public void SetupPartner(ChexarPartner partner)
        {
            _partnerRepo.AddWithFlush(partner);
        }
        #endregion

        #region Private Mthds

        private string GetTimeZone(MGIContext mgiContext)
        {
            if (string.IsNullOrEmpty(mgiContext.TimeZone))
            {
                throw new CheckException(CheckException.INVALID_DICTIONARY_KEY, "TimeZone not found in the context");
            }
            return mgiContext.TimeZone;
        }

        private ChexarTrx getChexarTrx(long trxId)
        {
            ChexarTrx chexarTrx = _chxrTrxRepo.FindBy(x => x.Id == trxId);

            if (chexarTrx == null)
                throw new CheckException(CheckException.TRANSACTION_NOT_FOUND, string.Format("Could not find Id {0}", trxId));

            return chexarTrx;
        }

        private string IngoUserName(MGIContext mgiContext)
        {
            if (string.IsNullOrEmpty(mgiContext.CheckUserName))
                throw new CheckException(CheckException.LOCATION_NOT_SET, "Location Ingo username not set in the context");

            return mgiContext.CheckUserName;
        }

        private long PartnerId(MGIContext context)
        {
            if (context.ChannelPartnerId == 0)
                throw new CheckException(CheckException.PARTNER_NOT_SET, "PartnerId not set in the context");

            return context.ChannelPartnerId;
        }

        // Karun@10/14/2013: added private method to get the ChannelPartnerPK from the context
        private Guid getChannelPartnerPK(Dictionary<string, object> context)
        {
            if (!context.ContainsKey("ChannelPartnerRowGuid") && context["ChannelPartnerRowGuid"] == null)
                throw new CheckException(CheckException.PARTNER_NOT_SET, "PartnerPK not set in the context");

            return new Guid(context["ChannelPartnerRowGuid"].ToString());
        }

        private CxnCheckData.CheckType getCheckType(int chexarType)
        {
            CxnCheckData.CheckType checkType = CxnCheckData.CheckType.TwoParty;

            ChexarCheckTypeMapping chexarCheckTypeMap = _checkTypeMappingRepo.FindBy(t => t.ChexarType == chexarType);

            if (chexarCheckTypeMap != null)
                checkType = chexarCheckTypeMap.CheckType;

            return checkType;
        }

        private int getChexarType(CxnCheckData.CheckType checkType)
        {
            List<ChexarCheckTypeMapping> chexarTypes = _checkTypeMappingRepo.FilterBy(t => t.CheckType == checkType).ToList();
            NLogger.Info(string.Format("chexarTypes.Count {0}", chexarTypes.Count.ToString()));

            if (chexarTypes.Count == 0)
                throw new CheckException(CheckException.CHEXAR_CHECK_TYPE_NOT_FOUND, string.Format("No Chexar type found for Nexxo type {0}", checkType));

            return chexarTypes.OrderBy(t => t.CheckType).First().ChexarType;
        }

        private void addChexarMicrDetails(ChexarTrx chexarTrx, ChexarMICRDetails checkDetails)
        {
            chexarTrx.CheckNumber = checkDetails.CheckNumber;
            chexarTrx.RoutingNumber = checkDetails.ABARoutingNumber;
            chexarTrx.AccountNumber = checkDetails.AccountNumber;
            chexarTrx.ChexarAmount = checkDetails.CheckAmount;
            chexarTrx.ChexarFee = checkDetails.FeeAmount;
            chexarTrx.ReturnType = checkDetails.CheckTypeId;
        }

        private void addChexarDeclineDetails(ChexarTrx chexarTrx, ChexarInvoiceCheck invoiceDetails)
        {
            //if (invoiceDetails.CheckTypeId != 0)
            //    chexarTrx.ReturnType = invoiceDetails.CheckTypeId;
            if (chexarTrx.DeclineCode != -3 && chexarTrx.DeclineCode != -2 && chexarTrx.DeclineCode != -1)//AL-2013
            {
                if (invoiceDetails.CheckTypeId != 0)//AL-4306 
                    chexarTrx.ReturnType = invoiceDetails.CheckTypeId;
                chexarTrx.DeclineCode = invoiceDetails.DeclineId;
                chexarTrx.Message = invoiceDetails.DeclineReason;
            }
        }

        private void cancelWithChexar(ChexarIO.ChexarLogin login, ChexarTrx chexarTrx)
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

        private string generateChexarLocationId(long partnerId, string location)
        {
            return string.Format("{0}{1}{2}", partnerId, ChexarSessionSeperator, location);
        }

        private string LogImageDetails(byte[] imgByte, string imageType)
        {
            String logStr = "Image Properties : " + imageType + "\n ";
            // Byte[] imgByte = scanner.GetCheckFrontImage("tiff");
            using (MemoryStream ms = new MemoryStream(imgByte))
            {
                Image chkImage = Image.FromStream(ms);
                int compressionTagIndex = Array.IndexOf(chkImage.PropertyIdList, 0x103);
                if (compressionTagIndex >= 0)
                {
                    PropertyItem compressionTag = chkImage.PropertyItems[compressionTagIndex];
                    if (BitConverter.ToInt16(compressionTag.Value, 0) == 1)
                        logStr += "Compression:Uncompressed TIFF 6.0 ";
                    else if (BitConverter.ToInt16(compressionTag.Value, 0) == 2)
                        logStr += "Compression:CCITT Group 3(CCITT 1D) TIFF 6.0 ";
                    else if (BitConverter.ToInt16(compressionTag.Value, 0) == 3)
                        logStr += "Compression:CCITT T.4 bi-level encoding(CCITT Group 3 2D)  TIFF 6.0 ";
                    else if (BitConverter.ToInt16(compressionTag.Value, 0) == 4)
                        logStr += "Compression:CCITT T.6 bi-level encoding (CCITT Group 4 fax encoding) TIFF 6.0  ";
                    else if (BitConverter.ToInt16(compressionTag.Value, 0) == 5)
                        logStr += "LZW  TIFF 6.0 ";
                    else if (BitConverter.ToInt16(compressionTag.Value, 0) > 5)
                        logStr += "Compression:Other ";
                }
                else
                    logStr += "Compression:Other ";

                logStr += "Image Length: " + imgByte.Length + " ";
                logStr += "Pixel Format: " + chkImage.PixelFormat + " ";
                logStr += "Image Width: " + chkImage.PhysicalDimension.Width + " ";
                logStr += "Image Height: " + chkImage.PhysicalDimension.Height + " ";
                logStr += "Image Resolution: " + chkImage.HorizontalResolution + " X " + chkImage.VerticalResolution;
            }
            return logStr;
        }

        public CxnCheckData.CheckLogin GetCheckSessions(MGIContext mgiContext)
        {
            long channelPartnerId = PartnerId(mgiContext);
            string timezone = mgiContext.TimeZone != null ? mgiContext.TimeZone : string.Empty;
            string userName = mgiContext.CheckUserName;
            string password = mgiContext.CheckPassword;
            NLogger.Info(string.Format("Partner Id -1 {0}", channelPartnerId.ToString()));

            string chexarLocation = generateChexarLocationId(PartnerId(mgiContext), IngoUserName(mgiContext));
            NLogger.Info(string.Format("Location {0}", chexarLocation));


            // establish local session if it isn't present already.

            //Developed by: Sunil Shetty || Date: 09/06/2015 
            //Comments: If() condition was removed so that company token gets updated on every transaction as requested in AL-573, changes over here as imapct on CPMonitor
            //AL - 573: As a product owner, I need InGo integrations to update tokens for each customer session
            ChexarPartner channelPartner = _partnerRepo.FindBy(x => x.Id == channelPartnerId);

            if (channelPartner == null)
                throw new CheckException(CheckException.CHEXAR_CREDENTIALS_NOT_FOUND, string.Format("No Chexar partner set up for partnerId {0}", channelPartnerId));

            ChexarIO.ChexarLogin login;
            string branchUsername = string.Format("{0}{1}{2}{3}", BranchUserNamePrefix, channelPartnerId, ChexarSessionSeperator, userName);
            string branchPassword = string.Format("{0}{1}{2}{3}", BranchPasswordPrefix, channelPartnerId, ChexarSessionSeperator, password);
            string employeeUsername = string.Format("{0}{1}", EmployeeUserNamePrefix, channelPartnerId);
            string employeePassword = string.Format("{0}{1}", EmployeePasswordPrefix, channelPartnerId.ToString().PadLeft(4, '0'));

            NLogger.Info(string.Format("branchUsername {0}", branchUsername));
            NLogger.Info(string.Format("branchPassword {0}", branchPassword));
            NLogger.Info(string.Format("employeeUsername {0}", employeeUsername));
            NLogger.Info(string.Format("employeePassword {0}", employeePassword));

            try
            {
                login = _chexar.GetChexarLogin(channelPartner.URL, channelPartner.Name, branchUsername, branchPassword, employeeUsername, employeePassword);
            }
            catch
            {
                throw new CheckException(CheckException.PROVIDER_ERROR, new ChexarProviderException(CheckException.CHEXAR_LOGIN_FAILED, "Chexar Login Failed"));
            }


            ChexarSession session = _chxrSessionRepo.FindBy(x => x.Location == chexarLocation);//context["Location"]);

            if (session != null)
            {
                channelPartnerId = session.Partner.Id;
                NLogger.Info(string.Format("Partner Id -2 {0}", channelPartnerId.ToString()));
            }

            if (session == null)
            {
                session = new ChexarSession
                {
                    Location = chexarLocation,
                    BranchId = login.IngoBranchId,
                    CompanyToken = login.CompanyToken,
                    EmployeeId = login.EmployeeId,
                    Partner = channelPartner,
                    //timestamp changes
                    DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),
                    DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone),

                };

                _chxrSessionRepo.AddWithFlush(session);
            }

            //Developed by: Sunil Shetty || Date: 09/18/2015 
            //Comments: The below condition was introduced for couple of issues 1) Performance issue was solved: Earlier we used to update the 
            //tChxr_Session table used to get updated every day once or on session not found but a new user story was brought(AL-573) in according to which we were supposed 
            //to update company token every time but after doing changes(removing condition AL-573, mentioned above comment) we got into performance issues to solve it we added below condition.
            //2) tChxr_Session value doesnt get updated if companytoken is simulator: To avoid this we added login.CompanyToken == "simulator"
            //NOTE: changes over here as imapct on CPMonitor
            // && session.BranchId == login.BranchId && session.EmployeeId == login.EmployeeId || (session.CompanyToken != login.CompanyToken && session.CompanyToken == "simulator")
            else if (session.CompanyToken != login.CompanyToken)
            {
                session.BranchId = login.IngoBranchId;
                session.CompanyToken = login.CompanyToken;
                session.EmployeeId = login.EmployeeId;
                session.DTTerminalLastModified = DateTime.Now;
                _chxrSessionRepo.UpdateWithFlush(session);
            }

            return new CxnCheckData.CheckLogin
            {
                URL = session.Partner.URL,
                CompanyToken = session.CompanyToken,
                EmployeeId = session.EmployeeId,
                BranchId = session.BranchId
            };
        }
        #endregion
    }
}
