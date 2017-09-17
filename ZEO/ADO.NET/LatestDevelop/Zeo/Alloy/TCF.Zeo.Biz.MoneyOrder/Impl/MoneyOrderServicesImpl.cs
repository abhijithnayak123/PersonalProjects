using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using CommonUtil = TCF.Zeo.Common.Util;
using BizContract = TCF.Zeo.Biz.MoneyOrder.Contract;
using ZeoData = TCF.Channel.Zeo.Data;
using CoreContract = TCF.Zeo.Core.Contract;
using CoreImpl = TCF.Zeo.Core.Impl;
using CoreData = TCF.Zeo.Core.Data;
using BizCore = TCF.Zeo.Biz.Common;
using TCF.Zeo.Common.Util;
using TCF.Zeo.Biz.Common.Data;
using TCF.Channel.Zeo.Data;
#region External References
using commonData = TCF.Zeo.Common.Data;
using AutoMapper;
using System.Configuration;
using static TCF.Zeo.Common.Util.Helper;
#endregion

namespace TCF.Zeo.Biz.MoneyOrder.Impl
{
    public class MoneyOrderServicesImpl : BizContract.IMoneyOrderServices
    {

        #region Dependencies
        IMapper mapper;
        CoreContract.IMoneyOrderService CoreMoneyOrder;
        public bool AllowDuplicateMoneyOrder
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["AllowDuplicateMoneyOrder"].ToString()); }
        } 
        MoneyOrderCheckPrintTemplateRepo CheckPrintRepo;
        BizCore.Contract.IFeeService feeService;
        BizCore.Contract.IComplianceService complianceService;
        CoreContract.ITrxnFeeAdjustmentService trxFeeAdjustment;

        #endregion

        public MoneyOrderServicesImpl()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CoreData.MoneyOrderImage, MoneyOrderImage>();

                cfg.CreateMap<CoreData.MoneyOrder, ZeoData.MoneyOrder>();
            });
            mapper = config.CreateMapper();
        }

        public ZeoData.MoneyOrder Add(MoneyOrderPurchase moneyOrderPurchase, commonData.ZeoContext context)
        {
            complianceService = new BizCore.Impl.ComplianceServiceImpl();
            feeService = new BizCore.Impl.FeeServiceImpl();
            trxFeeAdjustment = new CoreImpl.TrxnFeeAdjustmentService();

            try
            {
                Limit limit = complianceService.GetTransactionLimit(Helper.TransactionType.MoneyOrder, context);

                if (moneyOrderPurchase.Amount < limit.PerTransactionMinimum)
                {
                    throw new BizCore.Data.Exceptions.BizComplianceLimitException(BizCore.MoneyOrderException.MOProductCode, BizCore.Data.Exceptions.BizComplianceLimitException.MINIMUM_LIMIT_FAILED, limit.PerTransactionMinimum.ToString(), Convert.ToDecimal(limit.PerTransactionMinimum));
                }
                if (moneyOrderPurchase.Amount > limit.PerTransactionMaximum)
                {
                    throw new BizCore.Data.Exceptions.BizComplianceLimitException(BizCore.MoneyOrderException.MOProductCode, BizCore.Data.Exceptions.BizComplianceLimitException.MAXIMUM_LIMIT_FAILED, limit.PerTransactionMaximum.ToString(), Convert.ToDecimal(limit.PerTransactionMaximum));
                }

                TransactionFee fee = feeService.GetFee(Helper.TransactionType.MoneyOrder, moneyOrderPurchase.Amount, 0, context);

                CoreData.MoneyOrder moneyOrderTransaction = new CoreData.MoneyOrder();
                moneyOrderTransaction.CustomerSessionId = context.CustomerSessionId;
                moneyOrderTransaction.Amount = moneyOrderPurchase.Amount;
                moneyOrderTransaction.Fee = fee.NetFee;
                moneyOrderTransaction.State = (int)Helper.TransactionStates.Authorized;
                moneyOrderTransaction.DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone);
                moneyOrderTransaction.BaseFee = fee.BaseFee;
                moneyOrderTransaction.DiscountApplied = fee.DiscountApplied;
                moneyOrderTransaction.AdditionalFee = fee.AdditionalFee;
                moneyOrderTransaction.DiscountName = fee.DiscountName;
                moneyOrderTransaction.DiscountDescription = fee.DiscountDescription;
                moneyOrderTransaction.IsSystemApplied = fee.IsSystemApplied;
                moneyOrderTransaction.PurchaseDate = DateTime.Now;

                long transactionID = 0;
                using (CoreMoneyOrder = new CoreImpl.ZeoCoreImpl())
                {
                    transactionID = CoreMoneyOrder.CreateMoneyOrderTransaction(moneyOrderTransaction, context);
                }

                if (fee.FeeAdjustmentId != 0)
                {
                    CoreData.TransactionFeeAdjustment trxFeeAdj = new CoreData.TransactionFeeAdjustment();
                    trxFeeAdj.FeeAdjustmentId = fee.FeeAdjustmentId;
                    trxFeeAdj.TransactionId = transactionID;
                    trxFeeAdj.IsActive = true;
                    trxFeeAdj.DTTerminalCreate = Helper.GetTimeZoneTime(context.TimeZone);
                    trxFeeAdj.DTServerCreate = DateTime.Now;
                    trxFeeAdjustment.Create(trxFeeAdj, context);
                }

                moneyOrderTransaction.Id = transactionID;

                ZeoData.MoneyOrder moneyOrder = mapper.Map<ZeoData.MoneyOrder>(moneyOrderTransaction);
                return moneyOrder;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCore.MoneyOrderException(BizCore.MoneyOrderException.MONEYOREDER_ADD_FAILED, ex);
            }
        }

        public bool Commit(long transactionId, commonData.ZeoContext context)
        {
            using (var scope = TransactionHandler.CreateTransactionScope(TransactionScopeOptions.RequiresNew))
            {
                try
                {
                    bool retValue = false;
                    using (CoreMoneyOrder = new CoreImpl.ZeoCoreImpl())
                    {
                        CoreMoneyOrder.UpdateMoneyOrderState(transactionId, context.CustomerId, (int)Helper.TransactionStates.Committed, Helper.GetTimeZoneTime(context.TimeZone), context);
                    }

                    scope.Complete();

                    return retValue;
                }
                catch (Exception ex)
                {
                    scope.Dispose();

                    using (CoreMoneyOrder = new CoreImpl.ZeoCoreImpl())
                    {
                        CoreMoneyOrder.UpdateMoneyOrderState(transactionId, context.CustomerId, (int)Helper.TransactionStates.Failed, Helper.GetTimeZoneTime(context.TimeZone), context);
                    }

                    if (ExceptionHelper.IsExceptionHandled(ex)) throw;

                    throw new BizCore.MoneyOrderException(BizCore.MoneyOrderException.COMMIT_MONEYORDER_FAILED, ex);
                }
            }
        }

        public bool UpdateMoneyOrder(ZeoData.MoneyOrder moneyOrder, commonData.ZeoContext context)
        {
            bool isUpdated = false;
            try
            {
                CoreData.MoneyOrder coreMoneyOrder = new CoreData.MoneyOrder();
                coreMoneyOrder.Id = moneyOrder.Id;
                coreMoneyOrder.MICR = moneyOrder.MICR;
                coreMoneyOrder.CheckNumber = moneyOrder.CheckNumber;
                coreMoneyOrder.AccountNumber = moneyOrder.AccountNumber;
                coreMoneyOrder.RoutingNumber = moneyOrder.RoutingNumber;

                CoreData.MoneyOrderImage image = new CoreData.MoneyOrderImage();
                image.CheckFrontImage = moneyOrder.FrontImage;
                image.CheckBackImage = moneyOrder.BackImage;
                using (CoreMoneyOrder = new CoreImpl.ZeoCoreImpl())
                {
                    isUpdated = CoreMoneyOrder.UpdateMoneyOrderTransaction(coreMoneyOrder, image, AllowDuplicateMoneyOrder, context.TimeZone, context);
                }
                if (!AllowDuplicateMoneyOrder && !isUpdated)
                    throw new BizCore.MoneyOrderException(BizCore.MoneyOrderException.MONEYORDER_COMMIT_ALREADY_EXIST);

                return isUpdated;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCore.MoneyOrderException(BizCore.MoneyOrderException.UPDATE_MONEYORDER_FAILED, ex);
            }
        }

        public bool UpdateMoneyOrderStatus(long transactionId, int state, commonData.ZeoContext context)
        {
            bool isUpdated = false;
            try
            {
                Helper.TransactionStates txnState = (Helper.TransactionStates)state;
                using (CoreMoneyOrder = new CoreImpl.ZeoCoreImpl())
                {
                    isUpdated = CoreMoneyOrder.UpdateMoneyOrderState(transactionId, context.CustomerId, (int)txnState, Helper.GetTimeZoneTime(context.TimeZone), context);
                }
                return isUpdated;
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCore.MoneyOrderException(BizCore.MoneyOrderException.UPDATE_MONEYORDER_STATUS_EXCEPTION, ex);
            }
        }

        public MoneyOrderCheckPrint GetMoneyOrderCheck(long transactionId, commonData.ZeoContext context)
        {
            CheckPrintRepo = new MoneyOrderCheckPrintTemplateRepo();
            try
            {
                CoreData.MoneyOrder moneyOrder = null;
                using (CoreMoneyOrder = new CoreImpl.ZeoCoreImpl())
                {
                    moneyOrder = CoreMoneyOrder.GetMoneyOrderTransactionById(transactionId, context);
                }
                string checkPrintContents = CheckPrintRepo.GetCheckPrintTemplate(context.ChannelPartnerName, Helper.TransactionTypes.MoneyOrder, Helper.ProviderId.Continental, string.Empty);
                checkPrintContents = ReplaceTags(GetTrxMoneyOrderTags(moneyOrder, context), checkPrintContents);
                return GetcheckPrint(checkPrintContents);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCore.MoneyOrderException(BizCore.MoneyOrderException.MONEYORDER_CHECK_PRINT_FAILED, ex);
            }
        }

        public bool Resubmit(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                using (CoreMoneyOrder = new CoreImpl.ZeoCoreImpl())
                {
                    CoreData.MoneyOrder coreMO = CoreMoneyOrder.GetMoneyOrderTransactionById(transactionId, context);
                    updatePartnerNextMOFee(ref coreMO, context);
                    CoreMoneyOrder.UpdateMoneyOrderFee(coreMO, context);
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCore.MoneyOrderException(BizCore.MoneyOrderException.RESUBMIT_MONEYORDER_FAILED, ex);
            }
        }

        public MoneyOrderCheckPrint GetMoneyOrderDiagnostics(commonData.ZeoContext context)
        {
            CheckPrintRepo = new MoneyOrderCheckPrintTemplateRepo();
            try
            {
                string moneyOrderPrintContents = string.Empty;
                moneyOrderPrintContents = CheckPrintRepo.GetMoneyOrderDiagnosticsTemplate();

                Dictionary<string, string> tags = new Dictionary<string, string>()
                {
                    {"{USDollars}", ("***USDOLLARS***")},
                    {"{TransactionDate}", DateTime.Now.ToString("MM/dd/yyyy")},
                    {"{TransactionAmount}", "$ 0.00" },
                    {"{TransactionAmountInWords}", "THIS IS A PRINT TEST VOID CHECK"}
                };

                moneyOrderPrintContents = ReplaceTags(tags, moneyOrderPrintContents);

                return GetcheckPrint(moneyOrderPrintContents);
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCore.MoneyOrderException(BizCore.MoneyOrderException.MONEYORDER_DIAGONOSTIC_FAILED, ex);
            }
        }

        public TransactionFee GetMoneyOrderFee(decimal amount, commonData.ZeoContext context)
        {
            try
            { 
                feeService = new BizCore.Impl.FeeServiceImpl();
                return feeService.GetFee(Helper.TransactionType.MoneyOrder, amount, 0, mapper.Map<commonData.ZeoContext>(context));
            }
            catch(Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCore.MoneyOrderException(BizCore.MoneyOrderException.MONEYORDER_GETFEE_FAILED, ex);
            }
        }
        
        public ZeoData.MoneyOrder GetMoneyOrderTransaction(long transactionId, commonData.ZeoContext context)
        {
            try
            {
                using (CoreMoneyOrder = new CoreImpl.ZeoCoreImpl())
                {
                    CoreData.MoneyOrder mo = CoreMoneyOrder.GetMoneyOrderTransactionById(transactionId, context);
                    return mapper.Map<ZeoData.MoneyOrder>(mo);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionHelper.IsExceptionHandled(ex)) throw;
                throw new BizCore.MoneyOrderException(BizCore.MoneyOrderException.GET_MONEYORDER_FAILED, ex);
            }
        }


        #region Private Methods

        private string ReplaceTags(Dictionary<string, string> tags, string checkPrintContents)
        {
            foreach (KeyValuePair<string, string> tag in tags)
            {
                checkPrintContents = checkPrintContents.Replace(tag.Key, tag.Value);
            }
            return checkPrintContents;
        }

        private Dictionary<string, string> GetTrxMoneyOrderTags(CoreData.MoneyOrder moneyOrder, commonData.ZeoContext context)
        {
            string customerId = Convert.ToString(context.CustomerId);
            customerId = (customerId.Length > 4 ? customerId.Substring(customerId.Length - 4) : customerId);
            string referenceNumber = string.Format("******{0} {1}", customerId, context.ClientAgentIdentifier); //To check

            DateTime tranDate = moneyOrder.DTTerminalCreate;

            if (moneyOrder != null && moneyOrder.DTTerminalLastModified != null)
                tranDate = moneyOrder.DTTerminalLastModified.Value;

            Dictionary<string, string> tags = new Dictionary<string, string>()
            {
                {"{Amount}",GetFormattedAmount(moneyOrder.Amount) },
                {"{AmountInWords}", GetAmountInWord(moneyOrder.Amount)},
                {"{Date}", tranDate.ToString("MM/dd/yyyy") },
                {"{TransactionId}",Convert.ToString(moneyOrder.Id)},
                {"{MoneyOrderCheckNumber}",Convert.ToString(moneyOrder.CheckNumber)},
                {"{MoneyOrderAccountNumber}", Convert.ToString(moneyOrder.AccountNumber)},
                {"{MoneyOrderRoutingNumber}", Convert.ToString(moneyOrder.RoutingNumber)},

                {"{USDollars}", ("***USDOLLARS***")},
                {"{TransactionReferenceNo}", referenceNumber},
                {"{TransactionDate}", tranDate.ToString("MMMM dd yyyy") },
                {"{TransactionAmount}", GetFormattedAmountForTCF(moneyOrder.Amount) },
                {"{TransactionAmountInWords}", GetAmountInWordForTCF(moneyOrder.Amount)}
            };
            return tags;
        }

        private string GetFormattedAmountForTCF(decimal amount)
        {
            return (amount.ToString("N2").PadLeft(13, '$')) + "******";
        }

        private string GetAmountInWordForTCF(decimal amount)
        {
            return AlloyUtil.AmountToStringForTCF((double)amount);
        }

        private string GetAmountInWord(decimal Amount)
        {
            return AlloyUtil.AmountToString((double)Amount, "dollar");
        }

        private string GetFormattedAmount(decimal Amount)
        {
            string[] amountParts = Amount.ToString("0.00").Split('.');

            string formattedAmount = "$" + amountParts[0] + "." + amountParts[1];

            return formattedAmount;
        }

        private MoneyOrderCheckPrint GetcheckPrint(string checkPrintContent)
        {
            List<string> checkPrintContents = new List<string>(checkPrintContent.Split(new char[] { '\n' }));
            checkPrintContents = checkPrintContents.Where(x => (x.Length > 0)).ToList<string>();
            return new MoneyOrderCheckPrint
            {
                Lines = checkPrintContents
            };
        }

        private void updatePartnerNextMOFee(ref CoreData.MoneyOrder moneyOrder, commonData.ZeoContext context)
        {
            feeService = new BizCore.Impl.FeeServiceImpl();
            context.IsSystemApplied = moneyOrder.IsSystemApplied;
            context.PromotionCode = moneyOrder.DiscountName != null ? moneyOrder.DiscountName.Trim() : string.Empty;

            TransactionFee fee = feeService.GetFee(Helper.TransactionType.MoneyOrder, moneyOrder.Amount, 0, context);

            moneyOrder.Fee = fee.NetFee;
            moneyOrder.BaseFee = fee.BaseFee;
            moneyOrder.AdditionalFee = fee.AdditionalFee;
            moneyOrder.DiscountApplied = fee.DiscountApplied;
            moneyOrder.DiscountDescription = fee.DiscountDescription;
            moneyOrder.DiscountName = fee.DiscountName;
            moneyOrder.Description = string.Empty;
            moneyOrder.IsSystemApplied = fee.IsSystemApplied;
            moneyOrder.DiscountApplied = fee.DiscountApplied;
            moneyOrder.DTServerLastModified = DateTime.Now;
            moneyOrder.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);
            if (fee.FeeAdjustmentId != 0)
            {
                CoreData.TransactionFeeAdjustment trxFeeAdj = new CoreData.TransactionFeeAdjustment();
                trxFeeAdj.FeeAdjustmentId = fee.FeeAdjustmentId;
                trxFeeAdj.TransactionId = moneyOrder.Id;
                trxFeeAdj.IsActive = true;
                trxFeeAdj.TransactionType = Helper.TransactionType.MoneyOrder;
                trxFeeAdj.DTTerminalLastModified = Helper.GetTimeZoneTime(context.TimeZone);
                trxFeeAdj.DTServerLastModified = DateTime.Now;
                trxFeeAdjustment.Update(trxFeeAdj, context); 
            }


        }

        #endregion
    }
}
