using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

using Spring.Transaction.Interceptor;

using MGI.Common.Util;
using MGI.Common.DataAccess.Data;
using MGI.Common.DataAccess.Contract;

using CXNData = MGI.Cxn.Check.Data;
using CXNContract = MGI.Cxn.Check.Contract;

using CXEData = MGI.Core.CXE.Data;
using CXEContract = MGI.Core.CXE.Contract;

using PTNRData = MGI.Core.Partner.Data;
using PTNRContract = MGI.Core.Partner.Contract;

using MGI.Biz.CPEngine.Contract;
using System.Configuration;
using NLog.Config;
using NLog;
using NLog.Targets;
using System.Text.RegularExpressions;
using MGI.Cxn.Check.Data;

namespace MGI.Biz.CPEngine.Monitor
{
	public class BizCPMonitor : ICPMonitor
	{
		#region Dependencies
		private CXEContract.ICheckService _cxeCheckSvc;
		public CXEContract.ICheckService CxeCheckSvc
		{
			set { _cxeCheckSvc = value; }
		}

		private PTNRContract.ITransactionService<PTNRData.Transactions.Check> _ptnrCheckSvc;
		public PTNRContract.ITransactionService<PTNRData.Transactions.Check> PtnrCheckSvc
		{
			set { _ptnrCheckSvc = value; }
		}

		private PTNRContract.IFeeService _feeSvc;
		public PTNRContract.IFeeService FeeSvc
		{
			set { _feeSvc = value; }
		}

		private PTNRContract.IChannelPartnerService _ptnrSvc;
		public PTNRContract.IChannelPartnerService PtnrSvc
		{
			set { _ptnrSvc = value; }
		}

		private CXNContract.ICheckProcessor _cxnCheckSvc;
		public CXNContract.ICheckProcessor CxnCheckSvc
		{
			set { _cxnCheckSvc = value; }
		}
		public PTNRContract.IMessageCenter MessageCenterService { private get; set; }
		public PTNRContract.IMessageStore MessageStore { private get; set; }
        // AL-4604: handled duplicate fee adjustments.
        public PTNRContract.IFeeAdjustmentService FeeAdjustmentService { private get; set; }         

		#endregion

		[Transaction()]
		public void Run(string[] args)
		{
			string logFilePath = System.Configuration.ConfigurationManager.AppSettings["LogPath"].ToString();
			string treeStruct = string.Format(DateTime.Now.ToString("yyyy/yyyyMM/yyyyMMdd"));
			string file = @"BizCPMonitor\CPEngineMonitor_" + DateTime.Today.ToString("MMddyyyy");
			string LogFileName = string.Format(@"{0}\{1}\{2}.log", logFilePath, treeStruct, file);

			if (!System.IO.File.Exists(LogFileName))
			{
				try
				{
					Logger.WriteLog(string.Format("===========****** CPMonitor Begin ******=========="));
					Logger.WriteLog(string.Format("12 AM clean up started"));

					MessageCenterService.DeleteAllMessages();

					Logger.WriteLog(string.Format("12 AM clean up completed"));
					Logger.WriteLog(string.Format("===========****** CPMonitor Ends ******=========="));

					return;
				}
				catch (Exception ex)
				{
					Logger.WriteLogError(string.Format("Deleting checks at 12 AM failed \n Reason : {0} \n InnerException : {1} \n Exception : {2}", ex.Message, ex.InnerException, ex.ToString()));
				}
			}

			Logger.WriteLog(string.Format("===========****** CPMonitor Begin ******=========="));

			// 0. Environment/variable setup
			List<CXNData.CheckTrx> openChecks = null;
			List<Core.Partner.Data.Transactions.Check> chkTrans = null;
			List<MGIContext> checkLogin = new List<MGIContext>();
			List<PTNRData.Transactions.Check> lstPtnrCheck = new List<PTNRData.Transactions.Check>();
			List<PTNRData.Transactions.Check> checkInfo = null;

			// 1. get list of pending checks
			try
			{
				openChecks = _cxnCheckSvc.PendingChecks();
				Logger.WriteLog(string.Format("{0} pending checks found", openChecks.Count));
				if (openChecks != null)
				{
					foreach (var item in openChecks)
					{
						try
						{
							PTNRData.Transactions.Check ptnrCheck = _ptnrCheckSvc.Lookup(item.Id);
							// the goal is to not include checks which are not found or they dont have location/terminal
							if (ptnrCheck != null && ptnrCheck.CustomerSession != null 
								&& ptnrCheck.CustomerSession.AgentSession != null 
								&& ptnrCheck.CustomerSession.AgentSession.Terminal != null 
								&& ptnrCheck.CustomerSession.AgentSession.Terminal.Location != null)
							{
								lstPtnrCheck.Add(ptnrCheck);
							}
							else
							{
								Logger.WriteLogError(string.Format("Check processing failed due to Check info, terminal or location not found for checkId: {0}", item.Id));
							}
						}
						catch (Exception ex)
						{
							Logger.WriteLogError(string.Format("Retrieving Check failed : {0} \n Reason : {1} \n Exception : {2}", item.Id, ex.Message, ex.ToString()));
						}
					}
					checkInfo = lstPtnrCheck.GroupBy(m =>
														new { m.CustomerSession.Customer.ChannelPartner.Id, m.CustomerSession.AgentSession.Terminal.Location.LocationIdentifier }
													).Select(grp => grp.First()).ToList();
					if (checkInfo != null)
					{
						foreach (var item in checkInfo)
						{
							try
							{
								if (openChecks != null && openChecks.Count > 0)
								{
									MGIContext contextCheck = new MGIContext();

									//PTNRData.Transactions.Check ptnrCheck = _ptnrCheckSvc.Lookup(openChecks.FirstOrDefault().Id);
									var INGOCredential = item.CustomerSession.AgentSession.Terminal.Location.LocationProcessorCredentials.Where(c => c.ProviderId == (int)PTNRContract.ProviderIds.Ingo).FirstOrDefault();

									if (INGOCredential != null)
									{
										contextCheck.CheckUserName = INGOCredential.UserName;
										contextCheck.CheckPassword = INGOCredential.Password;
									}
									contextCheck.ChannelPartnerId = item.CustomerSession.Customer.ChannelPartner.Id;
									contextCheck.TimeZone = item.CustomerSession.AgentSession.Terminal.Location.TimezoneID;
									Cxn.Check.Data.CheckLogin checkLoginInfo = _cxnCheckSvc.GetCheckSessions(contextCheck);
									if (checkLoginInfo != null)
									{
										contextCheck.URL = checkLoginInfo.URL;
										contextCheck.IngoBranchId = checkLoginInfo.BranchId;
										contextCheck.CompanyToken = checkLoginInfo.CompanyToken;
										contextCheck.EmployeeId = checkLoginInfo.EmployeeId;
										//AL-1759 Pricing cluster user story changes - found while testing AL-3032
										if (item.CustomerSession.Customer != null && item.CustomerSession.Customer.ChannelPartner != null)
										{
											contextCheck.ChannelPartnerRowGuid = item.CustomerSession.Customer.ChannelPartner.rowguid;
											if (item.CustomerSession.AgentSession != null && item.CustomerSession.AgentSession.Terminal != null && item.CustomerSession.AgentSession.Terminal.Location != null)
											{
												contextCheck.LocationRowGuid = item.CustomerSession.AgentSession.Terminal.Location.rowguid;
											}
										}
										checkLogin.Add(contextCheck);
									}
								}
							}
							catch (Exception ex)
							{
								Logger.WriteLogError(string.Format("Retrieving Check status failed for check : {0} \n Reason : {1} \n Exception : {2}", item.Id, ex.Message, ex.ToString()));
							}
						}
						// 2. for each check, poll status and take action
						if (openChecks != null)
						{
							foreach (CXNData.CheckTrx pendingCheck in openChecks)
							{
								try
								{
									MGIContext context = new MGIContext();

									Logger.WriteLog(string.Format("Processing check {0}", pendingCheck.Id));

									// get the ptnr check transaction - status needs to be updated
									PTNRData.Transactions.Check ptnrCheck = _ptnrCheckSvc.Lookup(pendingCheck.Id);

									context = (MGIContext)checkLogin.FirstOrDefault(m => m.ChannelPartnerId == ptnrCheck.CustomerSession.Customer.ChannelPartner.Id);

									CXNData.CheckStatus status = _cxnCheckSvc.Status(pendingCheck.Id, ptnrCheck.CustomerSession.AgentSession.Terminal.Location.TimezoneID, context);
									/****************************END AL-86 Changes************************************************/
									// get the modified transaction during get status.
									CXNData.CheckTrx updatedTrx = _cxnCheckSvc.Get(pendingCheck.Id);

									chkTrans = _ptnrCheckSvc.GetAllForCustomer(ptnrCheck.CustomerSession.Customer.Id);
									//AL-313 to remove the Current Check Transaction in the list of Customer Transactions
									chkTrans.Remove(ptnrCheck);
									Logger.WriteLog(string.Format("   Current Status: {0}", status));

									if (status != CXNData.CheckStatus.Pending)
									{

										// if status change to approved, retrieve check fee and store in CXE
										if (status == CXNData.CheckStatus.Approved)
										{
											PTNRData.ChannelPartner channelPartner = _ptnrSvc.ChannelPartnerConfig(ptnrCheck.CustomerSession.Customer.ChannelPartnerId);

											//US2030
											removeCancelParkedTransactions(ptnrCheck.CustomerSession, ref chkTrans);

											//US1799 To Consider ManualEntry Promotions adding PromotionCode in context
											context.PromotionCode = ptnrCheck.DiscountName;
											context.IsSystemApplied = ptnrCheck.IsSystemApplied;

											if (updatedTrx.Amount != updatedTrx.ReturnAmount)
											{
												ptnrCheck.Amount = updatedTrx.ReturnAmount;
											}
											PTNRData.TransactionFee tranFee = _feeSvc.GetCheckFee(ptnrCheck.CustomerSession, chkTrans, ptnrCheck.Amount, (int)updatedTrx.ReturnType, context);

											ptnrCheck.Fee = tranFee.NetFee;
											ptnrCheck.BaseFee = tranFee.BaseFee;
											ptnrCheck.DiscountApplied = tranFee.DiscountApplied;
                                        // AL-4604: handled duplicate fee adjustments.
                                        FeeAdjustmentService.DeleteFeeAdjustments(ptnrCheck.rowguid); 

											ptnrCheck.AddFeeAdjustments(tranFee.Adjustments);
											ptnrCheck.Description = updatedTrx.ReturnType.ToString();
											//US1799 - Added for manual promotions - found during testing AL-3032
											ptnrCheck.DiscountName = tranFee.DiscountName;
											ptnrCheck.DiscountDescription = tranFee.DiscountDescription;
											ptnrCheck.AdditionalFee = tranFee.AdditionalFee;
											ptnrCheck.IsSystemApplied = tranFee.IsSystemApplied;

											Logger.WriteLog("Update Partner DB:- Begin of Transactid: " + ptnrCheck.Id);
											_ptnrCheckSvc.Update(ptnrCheck);
											Logger.WriteLog("Update Partner DB:- END of Transactid: " + ptnrCheck.Id);
											_cxeCheckSvc.Update(updatedTrx.Id, (int)updatedTrx.ReturnType, tranFee.NetFee, ptnrCheck.CustomerSession.AgentSession.Terminal.Location.TimezoneID);

											ptnrCheck.CXEState = (int)CXEData.TransactionStates.Authorized;
										}
										else if (status == CXNData.CheckStatus.Declined)
										{
											ptnrCheck.CXEState = (int)CXEData.TransactionStates.Declined;
											MGI.Core.Partner.Data.Language lang = MGI.Core.Partner.Data.Language.EN;
											BizCPEngineException bizcpexception = new BizCPEngineException(pendingCheck.DeclineCode);
											Core.Partner.Data.Message _message = MessageStore.Lookup(ptnrCheck.CustomerSession.AgentSession.Terminal.ChannelPartner.Id, Convert.ToString(bizcpexception.MajorCode) + "." + Convert.ToString(bizcpexception.MinorCode), lang);
											if (_message != null)
											{
												pendingCheck.DeclineMessage = _message.Content;
												_cxnCheckSvc.Update(pendingCheck, context);
											}
										}

										// inefficient, but update again
										string timeZone = ptnrCheck.CustomerSession.AgentSession.Terminal.Location.TimezoneID;
										_cxeCheckSvc.Update(updatedTrx.Id, ptnrCheck.CXEState, timeZone);

										_ptnrCheckSvc.UpdateStates(updatedTrx.Id, ptnrCheck.CXEState, (int)status);
										PTNRData.AgentMessage agentMessage = MessageCenterService.Lookup(ptnrCheck);
										if (agentMessage != null)
										{
											MessageCenterService.UpdateStatus(agentMessage, timeZone);
										}
									}
								}
								catch (Exception ex)
								{
									Logger.WriteLogError(string.Format("Updating status failed for check : {0} \n Reason : {1} \n Exception : {2}", pendingCheck.Id, ex.Message, ex.ToString()));
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Logger.WriteLogError(string.Format("Get PendingChecks failed : \n Reason : {0} \n Exception : {1}", ex.Message, ex.ToString()));
			}

			Logger.WriteLog(string.Format("===========****** CPMonitor Ends ******=========="));
		}

		private void removeCancelParkedTransactions(PTNRData.CustomerSession session, ref List<PTNRData.Transactions.Check> transactions)
		{
			var ShoppingCart = session.ParkingShoppingCart;

			if (ShoppingCart != null)
			{
				foreach (var transaction in ShoppingCart.ShoppingCartTransactions)
				{
					transactions.RemoveAll(x => x.rowguid == transaction.Transaction.rowguid);
				}
			}
			transactions.RemoveAll(x => x.CXEState == (int)PTNRData.PTNRTransactionStates.Canceled || x.CXEState == (int)PTNRData.PTNRTransactionStates.Declined);
		}
		private string mapLogFilePath(string fileName)
		{
			fileName = string.Format(@"{0}{1}\{2}", ConfigurationManager.AppSettings["LogPath"], NexxoUtil.CreateYearMonthDayTree(), fileName);

			if (!Regex.IsMatch(fileName, @"\.log$"))
				fileName = string.Format("{0} {1}.log", fileName, DateTime.Now.ToString("s").Replace("-", string.Empty).Replace(":", string.Empty));

			return fileName;
		}
	}
}
