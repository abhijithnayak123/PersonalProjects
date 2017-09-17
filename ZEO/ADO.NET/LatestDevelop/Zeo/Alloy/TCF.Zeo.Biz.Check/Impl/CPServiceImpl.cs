#region External References
using System;
using System.Collections.Generic;
using AutoMapper;
#endregion
#region Zeo References
using TCF.Zeo.Biz.Check.Contract;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Cxn.Check.Data;
using ZeoData = TCF.Channel.Zeo.Data;
using TCF.Zeo.Processor;
using TCF.Zeo.Cxn.Check.Contract;
using CXNData = TCF.Zeo.Cxn.Check.Data;
using CoreData = TCF.Zeo.Core.Data;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Impl;
using TCF.Zeo.Common.Data;
using BizCommon = TCF.Zeo.Biz.Common;
using TCF.Zeo.Biz.Common.Data.Exceptions;
#endregion

namespace TCF.Zeo.Biz.Check.Impl
{
    public class CPServiceImpl : ICPService
    {
        #region Dependencies

        CheckFrankTemplateRepo CheckFrankRepo = new CheckFrankTemplateRepo();
        ProcessorRouter processorRouter = new ProcessorRouter();
        ICheckProcessor checkProcessor;
        ICheckService coreCheckService;
        BizCommon.Contract.IFeeService feeService = new BizCommon.Impl.FeeServiceImpl();
        BizCommon.Contract.IComplianceService complianceService = new BizCommon.Impl.ComplianceServiceImpl();
        ITrxnFeeAdjustmentService trxFeeAdjustment = new TrxnFeeAdjustmentService();

        IMapper mapper;

        #endregion

        public CPServiceImpl()
        {
            #region Mapping

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CXNData.CheckLogin, ZeoData.CheckLogin>();
                cfg.CreateMap<CXNData.CheckProcessorInfo, ZeoData.CheckProcessorInfo>();
                cfg.CreateMap<CoreData.CheckType, ZeoData.CheckType>();
                cfg.CreateMap<CXNData.CheckInfo, ZeoData.CheckSubmission>().ReverseMap()
                .ForMember(x => x.FrontImageTIF, s => s.MapFrom(c => c.FrontImageTIFF))
                .ForMember(x => x.BackImageTIF, s => s.MapFrom(c => c.BackImageTIFF))
                .AfterMap((s, d) =>
                {
                    d.Type = (CheckType)Convert.ToInt32(s.CheckType);
                });

                cfg.CreateMap<CXNData.CheckTransaction, ZeoData.CheckTransactionDetails>()
                .ForMember(x => x.Amount, s => s.MapFrom(c => c.ReturnAmount))
                .ForMember(x => x.DeclineErrorCode, s => s.MapFrom(c => c.DeclineCode))
                .ForMember(x => x.ImageFront, s => s.MapFrom(c => c.ImageFront));
            });
            mapper = config.CreateMapper();

            #endregion
        }

        #region ICPService methods

        public ZeoData.CheckLogin GetChexarSessions(ZeoContext context)
        {
            try
            {
                return mapper.Map<ZeoData.CheckLogin>(GetProcessor(context.ChannelPartnerName).GetCheckSessions(context));
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.GET_CHECK_LOGIN_FAILED, ex);
            }
        }

        public ZeoData.CheckProcessorInfo GetCheckProcessorInfo(ZeoContext context)
        {
            try
            {
                string locationId = string.Format("{0}-{1}", context.ChannelPartnerId, context.CheckUserName);
                checkProcessor = GetProcessor(context.ChannelPartnerName);
                if (checkProcessor != null)
                {
                    CXNData.CheckProcessorInfo checkProcessorInfo = checkProcessor.GetCheckProcessorInfo(context.AgentSessionId, locationId);
                    return mapper.Map<ZeoData.CheckProcessorInfo>(checkProcessorInfo);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.GET_CHECK_PROCESSOR_INFO_FAILED, ex);
            }
        }

        public List<ZeoData.CheckType> GetCheckTypes(ZeoContext context)
        {
            try
            {
                using (coreCheckService = new ZeoCoreImpl())
                {
                    return mapper.Map<List<ZeoData.CheckType>>(coreCheckService.GetCheckTypes(context));
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.GET_CHECK_TYPE_FAILED, ex);

            }
        }

        public ZeoData.TransactionFee GetFee(ZeoData.CheckSubmission check, ZeoContext context)
        {
            try
            {
                BizCommon.Data.Limit limit = complianceService.GetTransactionLimit(Helper.TransactionType.ProcessCheck, context);

                if (check.Amount < limit.PerTransactionMinimum)
                {
                    throw new BizComplianceLimitException(CheckException.ProductCode, BizComplianceLimitException.MINIMUM_LIMIT_FAILED, limit.PerTransactionMinimum.ToString(), Convert.ToDecimal(limit.PerTransactionMinimum));
                }
                if (check.Amount > limit.PerTransactionMaximum)
                {
                    throw new BizComplianceLimitException(CheckException.ProductCode, BizComplianceLimitException.MAXIMUM_LIMIT_FAILED, limit.PerTransactionMaximum.ToString(), Convert.ToDecimal(limit.PerTransactionMaximum));
                }

                context.PromotionCode = check.PromoCode;
                context.IsSystemApplied = check.IsSystemApplied;

                return feeService.GetFee(TransactionType.ProcessCheck, check.Amount, Convert.ToInt32(check.CheckType), context);

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.GET_FEE_FAILED, ex);

            }
        }

        public ZeoData.Check Submit(ZeoData.CheckSubmission checkSubmission, ZeoContext context)
        {
            try
            {
                checkProcessor = GetProcessor(context.ChannelPartnerName);
                long chxrAccountId = checkProcessor.GetAccount(context);

                CoreData.Check coreCheck = new CoreData.Check()
                {
                    ProviderAccountId = chxrAccountId,
                    Amount = checkSubmission.Amount,
                    Fee = checkSubmission.Fee,
                    DiscountName = checkSubmission.PromoCode,
                    IsSystemApplied = checkSubmission.IsSystemApplied,
                    CheckType = checkSubmission.CheckType,
                    CustomerSessionId = context.CustomerSessionId,
                    State = (int)TransactionStates.Initiated,
                    MICR = checkSubmission.MICR,
                    ProviderId = (int)processorRouter.GetCheckProvider(context.ChannelPartnerName),
                    AdditionalFee = checkSubmission.AdditionalFee,
                    DiscountApplied = checkSubmission.DiscountApplied,
                    DiscountDescription = checkSubmission.DiscountDescription,
                    BaseFee = checkSubmission.BaseFee
                };
                context.PromotionCode = checkSubmission.PromoCode;
                context.IsSystemApplied = checkSubmission.IsSystemApplied;
                coreCheck.DTServerLastModified = coreCheck.DTServerCreate = DateTime.Now;
                coreCheck.DTTerminalLastModified = coreCheck.DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone);

                CoreData.CheckImages checkImage = new CoreData.CheckImages()
                {
                    Front = checkSubmission.FrontImage,
                    Back = checkSubmission.BackImage,
                    Format = checkSubmission.ImageFormat
                };

                long Id = 0;
                using (coreCheckService = new ZeoCoreImpl())
                {
                    Id = coreCheckService.CreateCheckTransaction(coreCheck, checkImage, context);
                }
                coreCheck.Id = Id;
                CXNData.CheckInfo cxnCheckSubmission = mapper.Map<CXNData.CheckInfo>(checkSubmission);

                CXNData.CheckTransaction cxnCheck = checkProcessor.Submit(chxrAccountId, cxnCheckSubmission, context);

                if (checkSubmission.FeeAdjustmentId != 0 && (cxnCheck.Status == CXNData.CheckStatus.Approved || cxnCheck.Status == CXNData.CheckStatus.Pending))
                {
                    CoreData.TransactionFeeAdjustment trxFeeAdj = new CoreData.TransactionFeeAdjustment();
                    trxFeeAdj.FeeAdjustmentId = checkSubmission.FeeAdjustmentId;
                    trxFeeAdj.TransactionId = coreCheck.Id;
                    trxFeeAdj.IsActive = true;
                    trxFeeAdj.DTTerminalCreate = GetTimeZoneTime(context.TimeZone);
                    trxFeeAdj.DTServerCreate = DateTime.Now;
                    trxFeeAdjustment.Create(trxFeeAdj, context);
                }

                return UpdateCheckTransaction(coreCheck, cxnCheck, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_SUBMIT_FAILED, ex);
            }
        }

        public bool Cancel(long transactionId, ZeoContext context)
        {
            try
            {

                checkProcessor = GetProcessor(context.ChannelPartnerName);

                long cxnTransactionId = GetCheckCxnTransactionId(transactionId);

                CXNData.CheckTransaction cxnCheck = checkProcessor.Status(cxnTransactionId, context);

                TransactionStates transactionState = (cxnCheck.Status == CXNData.CheckStatus.Declined) ? TransactionStates.Declined : TransactionStates.Canceled;

                bool isCheckCancelled = checkProcessor.Cancel(cxnTransactionId, context);

                if (isCheckCancelled)
                {
                    using (coreCheckService = new ZeoCoreImpl())
                    {
                        isCheckCancelled = coreCheckService.CancelCheckTransaction(transactionId, transactionState, context.TimeZone, context);
                    }
                }

                return isCheckCancelled;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_CANCEL_FAILED, ex);
            }
        }

        public void Commit(long transactionId, ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope(TransactionScopeOptions.RequiresNew))
            {
                try
                {
                    checkProcessor = GetProcessor(context.ChannelPartnerName);

                    long cxnTransactionId = GetCheckCxnTransactionId(transactionId);

                    checkProcessor.Commit(cxnTransactionId, context);

                    CXNData.CheckTransaction cxnCheck = checkProcessor.Status(cxnTransactionId, context);

                    // Update the check transaction status
                    // delete message center data if the check is pending
                    // call Add or update FeeAdjustment for the customer

                    using (coreCheckService = new ZeoCoreImpl())
                    {
                        coreCheckService.CommitTransaction(transactionId, (int)TransactionStates.Committed, context.CustomerSessionId, context.TimeZone, context);
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                    throw new CheckException(CheckException.CHECK_COMMIT_FAILED, ex);
                }
            }
        }

        public ZeoData.CheckFrankingDetails GetCheckFrankingData(long transactionId, ZeoContext context)
        {
            try
            {
                CoreData.Check check = new CoreData.Check();

                using (coreCheckService = new ZeoCoreImpl())
                {
                    check = coreCheckService.GetCheckTransaction(transactionId, context);
                }

                Helper.ProviderId provider = (Helper.ProviderId)Enum.Parse(typeof(Helper.ProviderId), _GetCheckProvider(context.ChannelPartnerName));

                string checkPrintContents = CheckFrankRepo.GetCheckFrankingTemplate(context.ChannelPartnerName, Helper.TransactionTypes.Check, provider, string.Empty);

                ZeoData.CheckFrankingDetails checkFrankDetails = new ZeoData.CheckFrankingDetails
                {
                    FrankData = checkPrintContents.Replace("{frankdata}", GetFrankText(check, context)),
                    Amount = check.Amount,
                    MICR = check.MICR
                };

                return checkFrankDetails;

            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.GET_CHECK_FRANK_FAILED, ex);
            }
        }

        public ZeoData.Check GetStatus(long transactionId, bool includeImage, ZeoContext context)
        {
            try
            {
                CoreData.Check check = new CoreData.Check();

                using (coreCheckService = new ZeoCoreImpl())
                {
                    check = coreCheckService.GetCheckTransaction(transactionId, context);
                }

                ICheckProcessor checkProcessor = GetProcessor(context.ChannelPartnerName);

                long cxnTransactionId = GetCheckCxnTransactionId(transactionId);

                CXNData.CheckTransaction cxnCheck = checkProcessor.Status(cxnTransactionId, context);

                context.IsSystemApplied = check.IsSystemApplied;
                context.PromotionCode = check.DiscountName != null ? check.DiscountName.Trim() : string.Empty;

                return UpdateCheckTransaction(check, cxnCheck, context);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.GET_STATUS_FAILED, ex);
            }
        }

        public ZeoData.CheckTransactionDetails GetTransaction(long transactionId, ZeoContext context)
        {
            try
            {
                checkProcessor = GetProcessor(context.ChannelPartnerName);

                long cxnTransactionId = GetCheckCxnTransactionId(transactionId);

                CXNData.CheckTransaction cxnCheck = checkProcessor.Get(cxnTransactionId);

                return mapper.Map<ZeoData.CheckTransactionDetails>(cxnCheck);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.GET_TRANSACTION_FAILED, ex);
            }
        }

        public void UpdateCheckTransactionFranked(long transactionId, ZeoContext context)
        {
            try
            {
                long cxnTransactionId = GetCheckCxnTransactionId(transactionId);

                bool isCheckFranked = true;

                processorRouter.GetCXNCheckProcessor(context.ChannelPartnerName).UpdateTransactionFranked(cxnTransactionId, isCheckFranked);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_FRANK_UPDATE_FAILED, ex);
            }

        }

        public bool Resubmit(long transactionId, ZeoContext context)
        {
            try
            {
                CoreData.Check coreCheck = null;
                using (coreCheckService = new ZeoCoreImpl())
                {
                    coreCheck = coreCheckService.GetCheckTransaction(transactionId, context);
                }
                CheckType checktype = (CheckType)Convert.ToInt32(coreCheck.CheckType);
                updatePartnerNextCheckFee(checktype, ref coreCheck, context);

                using (coreCheckService = new ZeoCoreImpl())
                {
                    coreCheck.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);
                    coreCheck.DTServerLastModified = DateTime.Now;
                    coreCheckService.UpdateCheckTransaction(coreCheck, context);
                }
                return true;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new CheckException(CheckException.CHECK_RESUBMIT_FAILED, ex);
            }
        }

        #endregion

        #region Private Methods
        private ZeoData.Check UpdateCheckTransaction(CoreData.Check coreCheck, CheckTransaction cxnCheck, ZeoContext context)
        {

            if (coreCheck.State == (int)Helper.TransactionStates.Pending && cxnCheck.Status != CXNData.CheckStatus.Pending)
                coreCheck.IsPendingCheckApprovedOrDeclined = true;

            switch (cxnCheck.Status)
            {
                case CXNData.CheckStatus.Approved:
                    coreCheck.Amount = cxnCheck.ReturnAmount;
                    updatePartnerCheckFee(ref coreCheck, cxnCheck.ReturnType, context);
                    coreCheck.State = (int)Helper.TransactionStates.Authorized;
                    break;

                case CXNData.CheckStatus.Pending:
                    if (cxnCheck.SubmitType != cxnCheck.ReturnType || cxnCheck.Amount != cxnCheck.ReturnAmount)
                    {
                        coreCheck.Amount = cxnCheck.ReturnAmount;
                        updatePartnerCheckFee(ref coreCheck, cxnCheck.ReturnType, context);
                    }
                    coreCheck.State = (int)Helper.TransactionStates.Pending;
                    break;

                case CXNData.CheckStatus.Declined:
                    coreCheck.State = (int)Helper.TransactionStates.Declined;
                    break;

                case CXNData.CheckStatus.Failed:
                    coreCheck.State = (int)Helper.TransactionStates.Failed;
                    break;
                default:
                    break;
            }

            coreCheck.Description = cxnCheck.ReturnType.ToString();
            coreCheck.ConfirmationNumber = cxnCheck.ConfirmationNumber;
            coreCheck.CheckType = ((int)cxnCheck.ReturnType).ToString();
            coreCheck.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);
            coreCheck.DTServerLastModified = DateTime.Now;
            coreCheck.CxnTransactionId = cxnCheck.Id;

            using (coreCheckService = new ZeoCoreImpl())
            {
                coreCheckService.UpdateCheckTransaction(coreCheck, context);
            }

            ZeoData.Check check = new ZeoData.Check()
            {
                Id = coreCheck.Id,
                Amount = coreCheck.Amount,
                SelectedType = ((int)cxnCheck.SubmitType).ToString(),
                SelectedFee = coreCheck.Fee,
                Status = cxnCheck.Status.ToString(),
                SubmissionDate = coreCheck.DTTerminalCreate,
                StatusMessage = cxnCheck.DeclineMessage,
                StatusDescription = cxnCheck.WaitTime,
                DmsStatusMessage = cxnCheck.DeclineMessage,
                ValidatedFee = coreCheck.Fee,
                BaseFee = coreCheck.BaseFee,
                DiscountApplied = coreCheck.DiscountApplied,
                DiscountName = coreCheck.DiscountName,
                ValidatedType = ((int)cxnCheck.ReturnType).ToString(),
                DiscountDescription = coreCheck.DiscountDescription,
                Fee = coreCheck.Fee
            };

            return check;
        }

        private ICheckProcessor GetProcessor(string channelPartnerName)
        {
            return processorRouter.GetCXNCheckProcessor(channelPartnerName);
        }

        private void updatePartnerCheckFee(ref CoreData.Check check, CXNData.CheckType validatedType, ZeoContext context)
        {
            ZeoData.TransactionFee fee = feeService.GetFee(TransactionType.ProcessCheck, check.Amount, Convert.ToInt32(validatedType), context, check.Id);
            check.Fee = fee.NetFee;
            check.BaseFee = fee.BaseFee;
            check.AdditionalFee = fee.AdditionalFee;
            check.DiscountApplied = fee.DiscountApplied;
            check.DiscountDescription = fee.DiscountDescription;
            check.DiscountName = fee.DiscountName;
            check.IsSystemApplied = fee.IsSystemApplied;

            //Commented this in the code and moved in the update SP. Issue  - "After reclassification if promotion not applicable to the checktype. It should update IsActive = 0.  
            //if (fee.FeeAdjustmentId != 0)
            //{
                CoreData.TransactionFeeAdjustment trxFeeAdj = new CoreData.TransactionFeeAdjustment();
                trxFeeAdj.FeeAdjustmentId = fee.FeeAdjustmentId;
                trxFeeAdj.TransactionId = check.Id;
                trxFeeAdj.IsActive = true;
                trxFeeAdj.TransactionType = TransactionType.ProcessCheck;
                trxFeeAdj.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);
                trxFeeAdj.DTServerLastModified = DateTime.Now;
                trxFeeAdjustment.Update(trxFeeAdj, context);
            //}
        }


        private void updatePartnerNextCheckFee(CheckType returnType, ref CoreData.Check coreCheck, ZeoContext context)
        {

            context.IsSystemApplied = coreCheck.IsSystemApplied;
            context.PromotionCode = coreCheck.DiscountName != null ? coreCheck.DiscountName.Trim() : string.Empty;

            ZeoData.TransactionFee fee = feeService.GetFee(TransactionType.ProcessCheck, coreCheck.Amount, Convert.ToInt32(returnType), context, coreCheck.Id);

            //TODO : Add & Delete Fee Adjustment, this can be done once Fee adjustments integrated with MO and CP

            coreCheck.Fee = fee.NetFee;
            coreCheck.BaseFee = fee.BaseFee;
            coreCheck.AdditionalFee = fee.AdditionalFee;
            coreCheck.DiscountApplied = fee.DiscountApplied;
            coreCheck.DiscountDescription = fee.DiscountDescription;
            coreCheck.DiscountName = fee.DiscountName;
            coreCheck.Description = returnType.ToString();
            coreCheck.IsSystemApplied = fee.IsSystemApplied;
            coreCheck.DiscountApplied = fee.DiscountApplied;

            //Commented this in the code and moved in the update SP. Issue  - "After reclassification if promotion not applicable to the checktype. It should update IsActive = 0.
            //if (fee.FeeAdjustmentId != 0)
            //{
                CoreData.TransactionFeeAdjustment trxFeeAdj = new CoreData.TransactionFeeAdjustment();
                    trxFeeAdj.FeeAdjustmentId = fee.FeeAdjustmentId;
                    trxFeeAdj.TransactionId = coreCheck.Id;
                    trxFeeAdj.IsActive = true;
                    trxFeeAdj.TransactionType = TransactionType.ProcessCheck;
                    trxFeeAdj.DTTerminalLastModified = GetTimeZoneTime(context.TimeZone);
                    trxFeeAdj.DTServerLastModified = DateTime.Now;
                    trxFeeAdjustment.Update(trxFeeAdj, context);
            //}

        }

        private string _GetCheckProvider(string channelPartner)
        {
            return processorRouter.GetCheckProvider(channelPartner).ToString();
        }

        private string GetFrankText(CoreData.Check transaction, ZeoContext context)
        {
            string frankText = transaction.FrankData;

            if (!string.IsNullOrWhiteSpace(frankText))
            {
                frankText = frankText.Replace("BankID", context.BankId);
                frankText = frankText.Replace("BranchID", context.BranchId);
                frankText = frankText.Replace("LocationIdentifier", context.LocationId);
                frankText = frankText.Replace("LocationName", context.LocationName);
                frankText = frankText.Replace("TerminalName", context.TerminalName);
                frankText = frankText.Replace("TellerName", context.AgentName);
                frankText = frankText.Replace("TellerID", Convert.ToString(context.AgentId));


                if (transaction != null)
                {
                    string TransactionID = transaction.Id == 0 ? "" : transaction.Id.ToString();
                    string CheckNumber = String.IsNullOrEmpty(transaction.ConfirmationNumber) ? "" : transaction.ConfirmationNumber;
                    string TransactionDate = transaction.DTTerminalCreate == null ? "" : transaction.DTTerminalCreate.ToShortDateString();
                    string TransactionTime = transaction.DTTerminalCreate == null ? "" : transaction.DTTerminalCreate.ToShortDateString();
                    string CheckAmount = transaction.Amount == 0 ? "" : transaction.Amount.ToString("0.00");
                    string FormatedTransactionDate = transaction.DTTerminalCreate == null ? "" : transaction.DTTerminalCreate.ToString("yyyyMMdd");
                    frankText = frankText.Replace("FormatedTransactionDate", FormatedTransactionDate);
                    frankText = frankText.Replace("TransactionDate", TransactionDate);
                    frankText = frankText.Replace("TransactionTime", TransactionTime);
                    frankText = frankText.Replace("CheckAmount", CheckAmount);
                    frankText = frankText.Replace("CheckNumber", CheckNumber);
                    frankText = frankText.Replace("TransactionID", TransactionID);
                    frankText = frankText.Replace("SequenceNo", Convert.ToString(context.CustomerId));
                }

                frankText = frankText.Replace("|", "");
            }
            return frankText;
        }

        private long GetCheckCxnTransactionId(long transactionId)
        {
            using (coreCheckService = new ZeoCoreImpl())
            {
                return coreCheckService.GetCheckCxnTransactionId(transactionId);
            }
        }

        #endregion
    }
}
