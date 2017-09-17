using AutoMapper;
using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Data.Transactions;
using MGI.Common.Util;
using MGI.Core.Catalog.Contract;
using MGI.Cxn.Common.Processor.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BIZFundData = MGI.Biz.FundsEngine.Data;
using CXEContract = MGI.Core.CXE.Contract;
using CXEData = MGI.Core.CXE.Data;
using CXNBillPayData = MGI.Cxn.BillPay.Data;
using CXNCheckContract = MGI.Cxn.Check.Contract;
using CXNCheckData = MGI.Cxn.Check.Data;
using CXNFundData = MGI.Cxn.Fund.Data;
using CXNMoneyTransferContract = MGI.Cxn.MoneyTransfer.Contract;
using CXNMoneyTransferData = MGI.Cxn.MoneyTransfer.Data;
using PTNRContract = MGI.Core.Partner.Contract;
using PTNRData = MGI.Core.Partner.Data;
using bizMasterData = MGI.Biz.MoneyTransfer.Data.MasterData;
using cxnMasterData = MGI.Cxn.MoneyTransfer.Data.MasterData;
using MGI.Cxn.Check.Contract;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Biz.Partner.Impl
{
	public class TransactionHistoryServiceImpl : ITransactionHistoryService
	{
		public TransactionHistoryServiceImpl()
		{
			AutoMapper.Mapper.CreateMap<PTNRData.Transactions.PastTransaction, PastTransaction>();
			AutoMapper.Mapper.CreateMap<MGI.Cxn.MoneyTransfer.Data.Account, Account>();


		}

		#region Dependencies

		public PTNRContract.ITransactionHistoryService PartnerTransactionHistoryService { private get; set; }
		public PTNRContract.ITransactionService<PTNRData.Transactions.Check> PartnerCheckService { private get; set; }
		public PTNRContract.ITransactionService<PTNRData.Transactions.Funds> PartnerFundsService { private get; set; }
		public PTNRContract.ITransactionService<PTNRData.Transactions.MoneyTransfer> PartnerMoneyTransferService { private get; set; }
		public PTNRContract.ITransactionService<PTNRData.Transactions.MoneyOrder> PartnerMoneyOrderService { private get; set; }
		public PTNRContract.ITransactionService<PTNRData.Transactions.BillPay> PartnerBillPayService { private get; set; }
		public PTNRContract.ITransactionService<PTNRData.Transactions.Cash> PartnerCashService { private get; set; }

		public PTNRContract.IChannelPartnerService ChannelPartnerSvc { private get; set; }
		public CXEContract.IMoneyOrderService CxeMoneyOrderSvc { private get; set; }
		public IProcessorRouter ProcessorSvc { private get; set; }
		public IProcessorRouter CheckProcessorRouter { private get; set; }
		public IProcessorRouter MoneyTransferProcessorSvc { private get; set; }
		public IProcessorRouter BillPayProcessorRouter { private get; set; }
		public IProductService ProductService { private get; set; }
		public TLoggerCommon MongoDBLogger { private get; set; }
		#endregion

		/// <summary>
		/// Transaction History
		/// </summary>
		/// <param name="customerId"></param>
		/// <param name="transactionType"></param>
		/// <param name="location"></param>
		/// <param name="dateRange"></param>
		/// <returns></returns>
		public List<TransactionHistory> GetTransactionHistory(long customerSessionId, long customerId, string transactionType, string location, DateTime dateRange, MGIContext mgiContext)
		{
			try
			{
				string committedState = CXEData.TransactionStates.Committed.ToString();
				Expression<System.Func<PTNRData.TransactionHistory, bool>> expression = txn => txn.CustomerId == customerId && txn.TransactionDate >= dateRange && txn.TransactionStatus == committedState;

				if (!string.IsNullOrWhiteSpace(location) && !string.IsNullOrWhiteSpace(transactionType))
				{
					expression = txn => txn.CustomerId == customerId && txn.TransactionDate >= dateRange && txn.TransactionStatus == committedState
						&& txn.Location == location && txn.TransactionType == transactionType;
				}
				else if (!string.IsNullOrWhiteSpace(location) && string.IsNullOrWhiteSpace(transactionType))
				{
					expression = txn => txn.CustomerId == customerId && txn.TransactionDate >= dateRange && txn.TransactionStatus == committedState
						&& txn.Location == location;
				}
				else if (!string.IsNullOrWhiteSpace(transactionType) && string.IsNullOrWhiteSpace(location))
				{
					expression = txn => txn.CustomerId == customerId && txn.TransactionDate >= dateRange && txn.TransactionStatus == committedState
						&& txn.TransactionType == transactionType;
				}
				List<PTNRData.TransactionHistory> transactionHistory = PartnerTransactionHistoryService.Get(expression);

				return Mapper(transactionHistory);
			}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetTransactionHistory", AlloyLayerName.BIZ,
					ModuleName.Transaction, "Error in GetTransactionHistory - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", ex.Message, ex.StackTrace);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizTransactionHistoryException(BizTransactionHistoryException.GET_CUSTOMER_TRANSACTION_HISTORY_FAILED, ex);
			}

			//return Mapper.Map<List<PTNRData.TransactionHistory>, List<TransactionHistory>>(transactionHistory);
		}

		/// <summary>
		/// Transaction Viewer
		/// </summary>
		/// <param name="agentId"></param>
		/// <param name="transactionType"></param>
		/// <param name="location"></param>
		/// <returns></returns>
		public List<TransactionHistory> GetTransactionHistory(long agentSessionId, long? agentId, string transactionType, string location, bool showAll, long transactionId, int duration, MGIContext mgiContext)
		{
			try
			{
				DateTime startDate = DateTime.Now.AddDays(-duration);
				Expression<System.Func<PTNRData.TransactionHistory, bool>> expression = null;

				expression = txn => txn.TransactionDate.Date >= startDate && txn.Location == location;

				if (transactionId > 0)
				{
					expression = txn => txn.TransactionId == transactionId;
				}
				else if (!string.IsNullOrWhiteSpace(transactionType) && agentId != null)
				{
					expression = txn => txn.TransactionDate.Date >= startDate && txn.Location == location
						&& txn.TransactionType == transactionType && txn.TellerId == agentId;
				}
				else if (!string.IsNullOrWhiteSpace(transactionType) && agentId == null)
				{
					expression = txn => txn.TransactionDate.Date >= startDate && txn.Location == location
						&& txn.TransactionType == transactionType;
				}
				else if (string.IsNullOrWhiteSpace(transactionType) && agentId != null)
				{
					expression = txn => txn.TransactionDate.Date >= startDate && txn.Location == location
						&& txn.TellerId == agentId;
				}

				List<PTNRData.TransactionHistory> transactionHistory = PartnerTransactionHistoryService.Get(expression);
				if (showAll == false)
				{
					transactionHistory = transactionHistory.Where(c => c.TransactionStatus == CXEData.TransactionStates.Committed.ToString()).ToList<PTNRData.TransactionHistory>();
				}
				return Mapper(transactionHistory);
			}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(agentSessionId), "GetTransactionHistory", AlloyLayerName.BIZ,
					ModuleName.Transaction, "Error in GetTransactionHistory - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", ex.Message, ex.StackTrace);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizTransactionHistoryException(BizTransactionHistoryException.GET_AGENT_TRANSACTION_HISTORY_FAILED, ex);
			}
		}

		public FundTransaction GetFundTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			try
			{
				PTNRData.Transactions.Funds fundTrx = PartnerFundsService.Lookup(transactionId);

				CXNFundData.CardAccount cardAccount = _GetProcessor(mgiContext.ChannelPartnerName).Lookup(fundTrx.CustomerSession.Customer.FindAccountByCXEId(fundTrx.Account.CXEId).CXNId);
				CXNFundData.FundTrx cxnFundTrx = _GetProcessor(mgiContext.ChannelPartnerName).Get(fundTrx.CXNId, mgiContext);

				decimal cardBalance = (decimal)cxnFundTrx.PreviousCardBalance;
				if (fundTrx.FundType == (int)BIZFundData.FundType.Debit && fundTrx.CXNState == (int)PTNRData.PTNRTransactionStates.Committed)
					cardBalance = (decimal)(cxnFundTrx.PreviousCardBalance - cxnFundTrx.TransactionAmount);
				else if (fundTrx.FundType == (int)BIZFundData.FundType.Credit && fundTrx.CXNState == (int)PTNRData.PTNRTransactionStates.Committed)
					cardBalance = (decimal)(cxnFundTrx.PreviousCardBalance + cxnFundTrx.TransactionAmount);

				return new FundTransaction()
				{
					FundType = fundTrx.FundType,
					CardNumber = (cardAccount != null) ? cardAccount.CardNumber : string.Empty,
					Amount = fundTrx.Amount,
					Fee = fundTrx.Fee,
					CardBalance = cardBalance,
					ConfirmationNumber = fundTrx.ConfirmationNumber,
					ProviderId = fundTrx.Account.ProviderId,
					PromoCode = cxnFundTrx.PromoCode,
					MetaData = new Dictionary<string, object>()
                  {
                     {"ProxyId",  cxnFundTrx.Account.ProxyId},
                     {"PAN",  cxnFundTrx.Account.FullCardNumber},
                     {"ExpirationDate", cxnFundTrx.Account.ExpirationDate},
                     {"PseudoDDA",  cxnFundTrx.Account.PseudoDDA}
                  }
				};
			}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetFundTransaction", AlloyLayerName.BIZ,
					ModuleName.Funds, "Error in GetFundTransaction - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", ex.Message, ex.StackTrace);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizTransactionHistoryException(BizTransactionHistoryException.GET_FUND_TRANSACTION_FAILED, ex);
			}
		}


		public CheckTransaction GetCheckTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			try
			{
				PTNRData.Transactions.Check check = PartnerCheckService.Lookup(transactionId);

				CXNCheckData.CheckTrx cxnCheck = _GetCheckProcessor(mgiContext.ChannelPartnerName).Get(check.Id);

				return new CheckTransaction()
				{
					CheckType = cxnCheck.ReturnType.ToString(),
					Amount = check.Amount,
					Fee = check.Fee,
					ConfirmationNumber = cxnCheck.ConfirmationNumber,
				};
			}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetCheckTransaction", AlloyLayerName.BIZ,
					ModuleName.ProcessCheck, "Error in GetCheckTransaction - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", ex.Message, ex.StackTrace);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizTransactionHistoryException(BizTransactionHistoryException.GET_CHECK_TRANSACTION_FAILED, ex);
			}
		}

		public MoneyOrderTransaction GetMoneyOrderTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			try
			{
				PTNRData.Transactions.MoneyOrder moneyOrder = PartnerMoneyOrderService.Lookup(transactionId);
				CXEData.Transactions.Commit.MoneyOrder cxemoneyOrder = CxeMoneyOrderSvc.Get(moneyOrder.Id);
				return new MoneyOrderTransaction()
				{
					Amount = moneyOrder.Amount,
					Fee = moneyOrder.Fee,
					CheckNumber = cxemoneyOrder == null ? string.Empty : cxemoneyOrder.MoneyOrderCheckNumber,
					ProviderId = moneyOrder.Account.ProviderId,
					BaseFee = moneyOrder.BaseFee,
					DiscountApplied = moneyOrder.DiscountApplied,
					DiscountName = moneyOrder.DiscountName,
					DiscountDescription = moneyOrder.DiscountDescription
				};
			}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetMoneyOrderTransaction", AlloyLayerName.BIZ,
					ModuleName.MoneyOrder, "Error in GetMoneyOrderTransaction - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", ex.Message, ex.StackTrace);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizTransactionHistoryException(BizTransactionHistoryException.GET_MONEYORDER_TRANSACTION_FAILED, ex);
			}

		}

		public MoneyTransferTransaction GetMoneyTransferTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			try
			{
				PTNRData.Transactions.MoneyTransfer moneyTransfer = PartnerMoneyTransferService.Lookup(transactionId);

				List<PTNRData.Transactions.MoneyTransfer> customerTransactions = PartnerMoneyTransferService.GetAll(moneyTransfer.CustomerSession.Customer.CXEId);

				bool isModifiedOrRefund = false;
				if (customerTransactions != null)
				{
					isModifiedOrRefund = customerTransactions.Exists(x => x.OriginalTransactionId == transactionId);
				}
				CXNMoneyTransferData.TransactionRequest request = new CXNMoneyTransferData.TransactionRequest()
				{
					TransactionId = moneyTransfer.CXNId
				};

				CXNMoneyTransferContract.IMoneyTransfer moneyTransferProcessor = _GetMoneyTransferProcessor(mgiContext.ChannelPartnerName);

				CXNMoneyTransferData.Transaction cxnMonetyTransferTrx = moneyTransferProcessor.GetTransaction(request, mgiContext);

				if (moneyTransfer.TransactionSubType == Convert.ToString((int)MGI.Biz.MoneyTransfer.Data.TransactionSubType.Refund))
				{
					cxnMonetyTransferTrx.TransactionSubType = Convert.ToString((int)MGI.Biz.MoneyTransfer.Data.TransactionSubType.Refund);
				}

				string receiverCountryName = string.Empty;
				if (cxnMonetyTransferTrx.Receiver != null && (!string.IsNullOrEmpty(cxnMonetyTransferTrx.Receiver.Country)))
				{
					List<bizMasterData> countries = AutoMapper.Mapper.Map<List<cxnMasterData>, List<bizMasterData>>(moneyTransferProcessor.GetCountries()).Where(
							x => x.Code == cxnMonetyTransferTrx.Receiver.Country.ToNullSafeString()).ToList();

					if (countries != null && countries.Count > 0)
						receiverCountryName = countries.FirstOrDefault().Name;
				}

				//Author : Abhijith
				//Description : Dont show the Fee component for Receive Money (Transfer type = 2) as fee should not be 
				// included for receive money in Transaction History.
				//Starts Here
				decimal fee = 0;

				if (moneyTransfer.TransferType != Convert.ToInt32(MGI.Biz.MoneyTransfer.Data.TransferType.ReceiveMoney))
					fee = cxnMonetyTransferTrx.Fee + NexxoUtil.GetDecimalDictionaryValueIfExists(cxnMonetyTransferTrx.MetaData, "MessageCharge")
						+ NexxoUtil.GetDecimalDictionaryValueIfExists(cxnMonetyTransferTrx.MetaData, "PlusChargesAmount")
						+ NexxoUtil.GetDecimalDictionaryValueIfExists(cxnMonetyTransferTrx.MetaData, "TransferTax");
				//Ends Here


				return new MoneyTransferTransaction()
				{
					Account = AutoMapper.Mapper.Map<Account>(cxnMonetyTransferTrx.Account),
					AccountNumber = NexxoUtil.GetDictionaryValueIfExists(cxnMonetyTransferTrx.MetaData, "GCNumber"),
					Amount = cxnMonetyTransferTrx.TransactionAmount,
					ConfirmationNumber = cxnMonetyTransferTrx.ConfirmationNumber,
					Fee = fee,
					TransferType = int.Parse(cxnMonetyTransferTrx.TransactionType),
					TestQuestion = cxnMonetyTransferTrx.TestQuestion.ToNullSafeString(),
					TestAnswer = cxnMonetyTransferTrx.TestAnswer.ToNullSafeString(),
					ReceiverFirstName = cxnMonetyTransferTrx.ReceiverFirstName,
					ReceiverLastName = cxnMonetyTransferTrx.ReceiverLastName.ToNullSafeString(),
					ReceiverSecondLastName = cxnMonetyTransferTrx.ReceiverSecondLastName.ToNullSafeString(),
					ReceiverAddress = (cxnMonetyTransferTrx.Receiver != null) ? cxnMonetyTransferTrx.Receiver.Address.ToNullSafeString() : "",
					ProviderId = moneyTransfer.Account.ProviderId,
					SenderName = cxnMonetyTransferTrx.SenderName,
					TransactionSubType = cxnMonetyTransferTrx.TransactionSubType,
					IsModifiedOrRefunded = isModifiedOrRefund,
					PromotionDiscount = cxnMonetyTransferTrx.PromotionDiscount,
					PromotionsCode = cxnMonetyTransferTrx.PromotionsCode,
					GrossTotalAmount = cxnMonetyTransferTrx.GrossTotalAmount,
					OriginalTransactionID = moneyTransfer.OriginalTransactionId,
					ExpectedPayoutCityName = NexxoUtil.GetDictionaryValueIfExists(cxnMonetyTransferTrx.MetaData, "ExpectedPayoutCityName").ToNullSafeString(),
					ReceiverCity = (cxnMonetyTransferTrx.Receiver != null) ? cxnMonetyTransferTrx.Receiver.City.ToNullSafeString() : "",
					ReceiverState = (cxnMonetyTransferTrx.Receiver != null) ? cxnMonetyTransferTrx.Receiver.State_Province.ToNullSafeString() : "",
					ReceiverCountry = receiverCountryName,
					ReceiverZipCode = (cxnMonetyTransferTrx.Receiver != null) ? cxnMonetyTransferTrx.Receiver.ZipCode.ToNullSafeString() : "",
					IsReceiverHasPhotoId = (cxnMonetyTransferTrx.Receiver != null) ? cxnMonetyTransferTrx.Receiver.IsReceiverHasPhotoId : false,
					ReceiverSecurityQuestion = (cxnMonetyTransferTrx.Receiver != null) ? cxnMonetyTransferTrx.Receiver.SecurityQuestion : "",
					ReceiverSecurityAnswer = (cxnMonetyTransferTrx.Receiver != null) ? cxnMonetyTransferTrx.Receiver.SecurityAnswer : "",
					ReceiverId = (cxnMonetyTransferTrx.Receiver != null) ? cxnMonetyTransferTrx.Receiver.Id : 0,
					ReceiverPhone = (cxnMonetyTransferTrx.Receiver != null) ? cxnMonetyTransferTrx.Receiver.PhoneNumber : "",
					ReceiverNickName = (cxnMonetyTransferTrx.Receiver != null) ? cxnMonetyTransferTrx.Receiver.NickName : "",
					PersonalMessage = cxnMonetyTransferTrx.PersonalMessage,
					IsDomesticTransfer = cxnMonetyTransferTrx.IsDomesticTransfer,
					DeliveryServiceName = cxnMonetyTransferTrx.DeliveryServiceName,
					DeliveryServiceDesc = cxnMonetyTransferTrx.DeliveryServiceDesc,
					DestinationCountryCode = cxnMonetyTransferTrx.DestinationCountryCode,
					DestinationCurrencyCode = cxnMonetyTransferTrx.DestinationCurrencyCode,
					DestinationState = cxnMonetyTransferTrx.DestinationState,
					LoyaltyCardNumber = cxnMonetyTransferTrx.LoyaltyCardNumber,
					LoyaltyCardPoints = cxnMonetyTransferTrx.LoyaltyCardPoints,
					MetaData = cxnMonetyTransferTrx.MetaData,
					AmountToReceiver = cxnMonetyTransferTrx.AmountToReceiver
				};
			}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetMoneyTransferTransaction", AlloyLayerName.BIZ,
					ModuleName.MoneyTransfer, "Error in GetMoneyTransferTransaction - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", ex.Message, ex.StackTrace);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizTransactionHistoryException(BizTransactionHistoryException.GET_MONEYTRANSFER_TRANSACTION_FAILED, ex);
			}
		}

		public BillPayTransaction GetBillPayTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
        {
			try
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<string>(mgiContext.CustomerSessionId, Convert.ToString(transactionId), "GetBillPayTransaction", AlloyLayerName.BIZ,
					ModuleName.BillPayment, "Begin GetBillPayTransaction - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", mgiContext);
				#endregion

				PTNRData.Transactions.BillPay billPay = PartnerBillPayService.Lookup(transactionId);

				int providerId = billPay.Account.ProviderId;

				CXNBillPayData.BillPayTransaction trx = GetBillPayProcessor(mgiContext.ChannelPartnerName, (PTNRContract.ProviderIds)providerId).GetTransaction(billPay.CXNId);

				BillPayTransaction billPayTransaction = new BillPayTransaction()
				{
					BillerName = trx.BillerName,
					AccountNumber = trx.AccountNumber,
					Amount = trx.Amount,
					Fee = trx.Fee,
					MetaData = trx.MetaData,
					ProviderId = providerId
				};

				if (billPayTransaction.MetaData == null)
				{
					billPayTransaction.MetaData = new Dictionary<string, object>();
				}
				billPayTransaction.MetaData.Add("TransactionTimeZone", billPay.CustomerSession.TimezoneID);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<BillPayTransaction>(mgiContext.CustomerSessionId, billPayTransaction, "GetBillPayTransaction", AlloyLayerName.BIZ,
					ModuleName.BillPayment, "End GetBillPayTransaction - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", mgiContext);
				#endregion

				return billPayTransaction;
			}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetBillPayTransaction", AlloyLayerName.BIZ,
					ModuleName.BillPayment, "Error in GetBillPayTransaction - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", ex.Message, ex.StackTrace);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizTransactionHistoryException(BizTransactionHistoryException.GET_BILLPAY_TRANSACTION_FAILED, ex);
			}
		}

		public List<PastTransaction> GetPastTransactions(long agentSessionId, long customerSessionId, long customerId, string transactionType, MGIContext mgiContext)
		{
			try
			{
				var corePastTranssction = PartnerTransactionHistoryService.Get(customerId, transactionType, mgiContext);
				return AutoMapper.Mapper.Map<List<PastTransaction>>(corePastTranssction);
			}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetPastTransactions", AlloyLayerName.BIZ,
					ModuleName.Transaction, "Error in GetPastTransactions - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", ex.Message, ex.StackTrace);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizTransactionHistoryException(BizTransactionHistoryException.GET_PAST_TRANSACTION_FAILED, ex);
			}
		}

		public CashTransaction GetCashTransaction(long agentSessionId, long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			try
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<string>(mgiContext.CustomerSessionId, Convert.ToString(transactionId), "GetCashTransaction", AlloyLayerName.BIZ,
					ModuleName.Transaction, "Begin GetCashTransaction - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", mgiContext);
				#endregion

				PTNRData.Transactions.Cash cash = PartnerCashService.Lookup(transactionId);

				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Info<PTNRData.Transactions.Cash>(mgiContext.CustomerSessionId, cash, "GetCashTransaction", AlloyLayerName.BIZ,
					ModuleName.Transaction, "End GetCashTransaction - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", mgiContext);
				#endregion

				return new CashTransaction()
				{
					Amount = cash.Amount,
					TransactionStatus = ((CXEData.TransactionStates)cash.CXEState).ToString(),
					TransactionType = ((MGI.Biz.CashEngine.Data.CashTransactionType)cash.CashType).ToString()
				};
			}
			catch (Exception ex)
			{
				#region AL-1014 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(customerSessionId), "GetCashTransaction", AlloyLayerName.BIZ,
					ModuleName.Transaction, "Error in GetCashTransaction - MGI.Biz.Partner.Impl.TransactionHistoryServiceImpl", ex.Message, ex.StackTrace);
				#endregion
				if (ExceptionHelper.IsExceptionHandled(ex)) throw;
				throw new BizTransactionHistoryException(BizTransactionHistoryException.GET_CASH_TRANSACTION_FAILED, ex);
			}
		}

		#region Private Methods

		private ICheckProcessor _GetCheckProcessor(string channelPartner)
		{
			// get the fund processor for the channel partner.
			return (ICheckProcessor)CheckProcessorRouter.GetProcessor(channelPartner);
		}

		private MGI.Cxn.Fund.Contract.IFundProcessor _GetProcessor(string channelPartner)
		{
			// get the fund processor for the channel partner.
			return (MGI.Cxn.Fund.Contract.IFundProcessor)ProcessorSvc.GetProcessor(channelPartner);
		}

		private CXNMoneyTransferContract.IMoneyTransfer _GetMoneyTransferProcessor(string channelPartner)
		{
			// get the moneytransfer processor for the channel partner.
			return (CXNMoneyTransferContract.IMoneyTransfer)MoneyTransferProcessorSvc.GetProcessor(channelPartner);
		}

		private List<TransactionHistory> Mapper(List<PTNRData.TransactionHistory> transactionHistory)
		{
			List<TransactionHistory> transHistoryList = new List<TransactionHistory>();
			foreach (var item in transactionHistory)
			{
				TransactionHistory transaction = new TransactionHistory();
				transaction.CustomerId = item.CustomerId;
				transaction.CustomerName = item.CustomerName;
				transaction.Location = item.Location;
				transaction.SessionId = item.SessionId;
				transaction.Teller = item.Teller;
				transaction.TellerId = item.TellerId;
				transaction.TotalAmount = item.TotalAmount;
				transaction.TransactionDate = item.TransactionDate;
				transaction.TransactionDetail = item.TransactionDetail;
				transaction.TransactionId = item.TransactionId;
				transaction.TransactionStatus = item.TransactionStatus;
				transaction.TransactionType = item.TransactionType;

				transHistoryList.Add(transaction);
			}

			return transHistoryList;
		}

		private MGI.Cxn.BillPay.Contract.IBillPayProcessor GetBillPayProcessor(string channelPartnerName, PTNRContract.ProviderIds providerId)
		{
			return (MGI.Cxn.BillPay.Contract.IBillPayProcessor)BillPayProcessorRouter.GetProcessor(channelPartnerName, providerId.ToString());
		}


		#endregion Private Methods
	}
}
