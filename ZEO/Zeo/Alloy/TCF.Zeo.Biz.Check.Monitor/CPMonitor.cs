using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

#region Zeo reference

using TCF.Zeo.Cxn.Check.Chexar.Impl;
using TCF.Zeo.Cxn.Check.Contract;
using TCF.Zeo.Cxn.Check.Data;
using TCF.Zeo.Common.Data;
using BizCommon = TCF.Zeo.Biz.Common;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Impl;
using System.Reflection;

#endregion

namespace TCF.Zeo.Biz.Check.Monitor
{
    public class CPMonitor : ICPMonitor
    {
        #region Dependencies

        IMessageCenter messageCenter = new MessageCenterImpl();
        ICheckProcessor cxnProcessor = new ChexarGateway();
        Contract.ICPService bizCheckService = new Impl.CPServiceImpl();
        BizCommon.Contract.IFeeService feeService = new BizCommon.Impl.FeeServiceImpl();
        ICheckService checkService = new ZeoCoreImpl();
        ITrxnFeeAdjustmentService feeAdjustmentService = new TrxnFeeAdjustmentService();

        #endregion

        #region ICPMonitor methods

        public void Run(string[] args)
        {
            CleanUpMessageCenter();
            UpdatePendingChecks();
        }

        #endregion

        #region Private Methods

        private void CleanUpMessageCenter()
        {

            string logFilePath = ConfigurationManager.AppSettings["LogPath"].ToString();
            string treeStruct = string.Format(DateTime.Now.ToString("yyyy/yyyyMM/yyyyMMdd"));
            string file = Assembly.GetExecutingAssembly().GetName().Name + @"\CPEngineMonitor_" + DateTime.Today.ToString("MMddyyyy");
            string LogFileName = string.Format(@"{0}\{1}\{2}.log", logFilePath, treeStruct, file);

            if (!System.IO.File.Exists(LogFileName))
            {
                try
                {

                    Logger.WriteLog(string.Format("===========****** CPMonitor Begin ******=========="));
                    Logger.WriteLog(string.Format("12 AM clean up started"));

                    messageCenter.DeleteAllMessages(new ZeoContext());

                    Logger.WriteLog(string.Format("12 AM clean up completed"));
                    Logger.WriteLog(string.Format("===========****** CPMonitor Ends ******=========="));
                }
                catch (Exception ex)
                {
                    Logger.WriteLogError(string.Format("Deleting checks at 12 AM failed \n Reason : {0} \n InnerException : {1} \n Exception : {2}", ex.Message, ex.InnerException, ex.ToString()));

                }
            }
        }

        private void UpdatePendingChecks()
        {
            try
            {
                Logger.WriteLog(string.Format("===========****** CPMonitor Begin ******=========="));


                // Get all the Pending Checks 
                List<PendingCheck> pendingChecks = cxnProcessor.GetPendingChecks();

                Logger.WriteLog(string.Format("{0} pending checks found", pendingChecks.Count));

                if (pendingChecks.Count > 0)
                {
                    ZeoContext zeoContext = new ZeoContext();

                    // Get Chexar login details for the channel partners
                    List<CheckLogin> chxrSessions = GetChexarSessionForChannelPartners(pendingChecks);

                    foreach (PendingCheck pendingCheck in pendingChecks)
                    {
                        Logger.WriteLog("\n ---------------Processing of Pending Check of PTNR trx Id : " + pendingCheck.TransactionId  + " Starts Here ---------------------------------------------- ");

                        //Get alloy context with the login details for the specific channelpartner id
                        zeoContext = GetProviderZeoContext(chxrSessions, pendingCheck);
                        CheckTransaction cxnCheck = new CheckTransaction();

                        try
                        {
                            //Check is these properties need to be set ? Now it is set because in Approve status we were passing these in the monitor.
                            zeoContext.IsSystemApplied = pendingCheck.IsSystemApplied;
                            zeoContext.PromotionCode = pendingCheck.DiscountName;

                            Channel.Zeo.Data.Check checkTrx = bizCheckService.GetStatus(pendingCheck.TransactionId, false, zeoContext);

                            Logger.WriteLog("\n PTNR TransactionId : " + checkTrx.Id + "\n Check Amount : " + checkTrx.Amount);

                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLogError(string.Format("Retrieving Check status failed for check : {0} \n Reason : {1} \n Exception : {2}", pendingCheck.TransactionId, ex.Message, ex.ToString()));
                        }

                        Logger.WriteLog("\n ---------------Processing of Pending Check of PTNR trx Id : " + pendingCheck.TransactionId + " Ends Here ---------------------------------------------- ");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLogError(string.Format("Get PendingChecks failed : \n Reason : {0} \n Exception : {1}", ex.Message, ex.ToString()));
            }

            Logger.WriteLog(string.Format("===========****** CPMonitor Ends ******=========="));
        }

        private List<CheckLogin> GetChexarSessionForChannelPartners(List<PendingCheck> pendingChecks)
        {
            // Get All the channel partners from the pending checks
            List<PendingCheck> channelPartnerCredintials = pendingChecks.GroupBy(i => new { i.ChannelPartnerId, i.LocationIdentifier }).Select(grp => grp.First()).ToList();

            List<CheckLogin> chxrSessions = new List<CheckLogin>();
            ZeoContext context = new ZeoContext();

            foreach (PendingCheck item in channelPartnerCredintials)
            {
                context.ChannelPartnerId = item.ChannelPartnerId;
                context.CheckUserName = item.CheckUserName;
                context.CheckPassword = item.CheckPassword;
                context.TimeZone = item.LocationTimeZone;
                CheckLogin checkLogin = cxnProcessor.GetCheckSessions(context);
                checkLogin.LocationIdentifier = item.LocationIdentifier;
                chxrSessions.Add(checkLogin);
            }

            return chxrSessions;
        }

        private ZeoContext GetProviderZeoContext(List<CheckLogin> chxrSessions, PendingCheck pendingCheck)
        {
            CheckLogin chxrLogin = chxrSessions.Find(i => i.ChannelPartnerId == pendingCheck.ChannelPartnerId && i.LocationIdentifier == pendingCheck.LocationIdentifier);
            return new ZeoContext
            {
                URL = chxrLogin.URL,
                CompanyToken = chxrLogin.CompanyToken,
                IngoBranchId = chxrLogin.BranchId,
                EmployeeId = chxrLogin.EmployeeId,
                ChannelPartnerId = chxrLogin.ChannelPartnerId,
                ChannelPartnerName = pendingCheck.ChannelPartnerName,
                CustomerSessionId = pendingCheck.CustomerSessionId,
                TimeZone = pendingCheck.LocationTimeZone,
                CustomerId = pendingCheck.CustomerId,
                ProviderId = 200  //Run this CP Monitor only for INGO.
            };
        }

        #endregion 


    }
}
