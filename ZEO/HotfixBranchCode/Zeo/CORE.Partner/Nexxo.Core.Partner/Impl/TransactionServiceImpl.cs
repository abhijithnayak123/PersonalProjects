using System;
using System.Linq;
using System.Collections.Generic;

using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Common.TransactionalLogging.Data;
using MGI.Common.Util;

using MGI.TimeStamp;

namespace MGI.Core.Partner.Impl
{
	public class TransactionServiceImpl<T> : ITransactionService<T> where T:Transaction
	{
		private IRepository<T> _txnRepo;
        public IRepository<T> TransactionRepo { get { return _txnRepo; } set { _txnRepo = value; } }
		public TLoggerCommon MongoDBLogger { get; set; }
		public void Create( T transaction )
		{
            try
            {
                //Changes for timezone
                string timezone = transaction.CustomerSession.TimezoneID;
                transaction.DTTerminalCreate = Clock.DateTimeWithTimeZone(timezone);
                transaction.DTServerCreate = DateTime.Now;
                _txnRepo.AddWithFlush(transaction);
            }
            catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<T>(transaction, "Create", AlloyLayerName.CORE, ModuleName.Transaction,
							"Error in Create - MGI.Core.Partner.Impl.TransactionServiceImpl", ex.Message, ex.StackTrace);

                throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_CREATE_FAILED, ex);
            }
		}

		public T Lookup( long Id )
		{
			T transaction = _txnRepo.FindBy(x => x.Id == Id);

			if (transaction == null)
            { 
                //AL-3370 Transactional Log User Story
                MongoDBLogger.Error<T>(transaction, "Lookup", AlloyLayerName.CORE, ModuleName.Transaction,
                            "Error in Lookup - MGI.Core.Partner.Impl.TransactionServiceImpl", string.Format("Could not find Transaction Id {0}", Id),string.Empty);

				throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_NOT_FOUND, string.Format("Could not find Transaction Id {0}", Id));
            }

			return transaction;
		}

        public List<T> GetAll(long CustomerId) // US1686,US1685 For MoneyTransfer OriginalTransaction - Ismodifiedorrefund 
        {
            return _txnRepo.FilterBy(x => x.Account.Customer.CXEId == CustomerId && x.CXEState == (int) Core.Partner.Data.PTNRTransactionStates.Committed && x.CXNState == (int) Core.Partner.Data.PTNRTransactionStates.Committed).ToList<T>();           
        }
		
		public void UpdateStates( long Id, int CXEState, int CXNState, string description = "" )
		{
			T txn = Lookup(Id);

            try
            {
                string timezone = txn.CustomerSession.TimezoneID;

                txn.CXEState = CXEState;
                txn.CXNState = CXNState;
                if (description.Length > 0)
                {
                    txn.Description = description;
                }
                //Changes for timestamp
                txn.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
                txn.DTServerLastModified = DateTime.Now;
                _txnRepo.UpdateWithFlush(txn);
            }
            catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(Id));
				details.Add("CXE State:" + Convert.ToString(CXEState));
				details.Add("CXN State:" + Convert.ToString(CXNState));
				details.Add("Description:" + Convert.ToString(description));
				MongoDBLogger.ListError<string>(details, "UpdateStates", AlloyLayerName.CORE, ModuleName.Transaction,
				 "Error in UpdateStates -MGI.Core.Partner.Impl.TransactionServiceImpl", ex.Message, ex.StackTrace);
				
                throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_UPDATE_STATES_FAILED, ex);
            }
		}

		public void UpdateCXEStatus(long Id, int CXEStatus)
		{
			T txn = Lookup(Id);

            try
            {
                string timezone = txn.CustomerSession.TimezoneID;
   
                txn.CXEState = CXEStatus;
                txn.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
                txn.DTServerLastModified = DateTime.Now;

                _txnRepo.UpdateWithFlush(txn);
            }
            catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(Id));
				details.Add("CXE Status:" + Convert.ToString(CXEStatus));
				MongoDBLogger.ListError<string>(details, "UpdateCXEStatus", AlloyLayerName.CORE, ModuleName.Transaction,
				 "Error in UpdateCXEStatus -MGI.Core.Partner.Impl.TransactionServiceImpl", ex.Message, ex.StackTrace);
				
                throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_UPDATE_CXE_STATE_FAILED, ex);
            }
		}

		public void UpdateCXNStatus(long Id, int CXNStatus)
		{
			T txn = Lookup(Id);

            try
            {
                //Changes for timestamp
                string timezone = txn.CustomerSession.TimezoneID;

                txn.CXNState = CXNStatus;
                txn.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
                txn.DTServerLastModified = DateTime.Now;

                _txnRepo.UpdateWithFlush(txn);
            }
            catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(Id));
				details.Add("CXN Status:" + Convert.ToString(CXNStatus));
				MongoDBLogger.ListError<string>(details, "UpdateCXNStatus", AlloyLayerName.CORE, ModuleName.Transaction,
				"Error in UpdateCXNStatus -MGI.Core.Partner.Impl.TransactionServiceImpl", ex.Message, ex.StackTrace);
				
                throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_UPDATE_CXN_STATE_FAILED, ex);
            }
		}
		//Open # US 1706 02.03.2014: Updating CXN values in partner Database//
		public void UpdateCXNStatus(long Id, long cxnId, int CXNStatus)
		{
			try
			{
				T txn = Lookup(Id);
				//Changes for timestamp
				string timezone = txn.CustomerSession.TimezoneID;
				if (txn == null)
					throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_NOT_FOUND, "Transaction Not Found");

				txn.CXNState = CXNStatus;
				txn.CXNId = cxnId;
				txn.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
				txn.DTServerLastModified = DateTime.Now;

				_txnRepo.UpdateWithFlush(txn);
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(Id));
				details.Add("CXN Transaction Id:" + Convert.ToString(cxnId));
				details.Add("CXN Status:" + Convert.ToString(CXNStatus));
				MongoDBLogger.ListError<string>(details, "UpdateCXNStatus", AlloyLayerName.CORE, ModuleName.Transaction,
				 "Error in UpdateCXNStatus -MGI.Core.Partner.Impl.TransactionServiceImpl", ex.Message, ex.StackTrace);
				
				throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_UPDATE_CXN_STATE_FAILED, ex);
			}
		}
		//Close # US 1706 02.03.2014//
		public void UpdateCXNStatus(long Id, long cxnId, int CXNStatus, decimal amount, decimal fee, string confirmationNumber)
		{
			try
			{
				T txn = Lookup(Id);
				//Changes for timestamp
				string timezone = txn.CustomerSession.TimezoneID;
				if (txn == null)
					throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_NOT_FOUND, "Transaction Not Found");

				txn.CXNState = CXNStatus;
				txn.CXNId = cxnId;
				txn.Amount = amount;
				txn.Fee = fee;
				txn.ConfirmationNumber = confirmationNumber;
				txn.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
				txn.DTServerLastModified = DateTime.Now;

				_txnRepo.UpdateWithFlush(txn);
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(Id));
				details.Add("CXN Transaction Id:" + Convert.ToString(cxnId));
				details.Add("CXN Status:" + Convert.ToString(CXNStatus));
				details.Add("Amount:" + Convert.ToString(amount));
				details.Add("Fee:" + Convert.ToString(fee));
				details.Add("Confirmation Number:" + confirmationNumber);
				MongoDBLogger.ListError<string>(details, "UpdateCXNStatus", AlloyLayerName.CORE, ModuleName.Transaction,
				"Error in UpdateCXNStatus -MGI.Core.Partner.Impl.TransactionServiceImpl", ex.Message, ex.StackTrace);
				
				throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_UPDATE_CXN_STATE_FAILED, ex);
			}
		}
		
		public void UpdateFee(long Id, decimal Fee)
		{
			T txn = Lookup(Id);

            try
            {
                txn.Fee = Fee;
                _txnRepo.UpdateWithFlush(txn);
            }
            catch (Exception ex)
            {
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(Id));
				details.Add("Fee:" + Convert.ToString(Fee));
				MongoDBLogger.ListError<string>(details, "UpdateFee", AlloyLayerName.CORE, ModuleName.Transaction,
				"Error in UpdateFee -MGI.Core.Partner.Impl.TransactionServiceImpl", ex.Message, ex.StackTrace);
				
                throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_UPDATE_FEE_FAILED, ex);
            }
		}
		
        public void UpdateTransactionDetails(long Id, int CXEState, int CXNState, string ConfirmationNumber, int Type)
        {
			Transaction txn = Lookup(Id);

            try
            {
                string timezone = txn.CustomerSession.TimezoneID;

                txn.CXEState = CXEState;
                txn.CXNState = CXNState;
                txn.ConfirmationNumber = ConfirmationNumber;
                txn.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
                txn.DTServerLastModified = DateTime.Now;//Changes for timestamp

                if (txn.Type == (int)TransactionType.Funds)
                    ((Funds)txn).FundType = Type;
                else if (txn.Type == (int)TransactionType.Cash)
                    ((Cash)txn).CashType = Type;

                T txnT = (T)txn;
                _txnRepo.UpdateWithFlush(txnT);
            }
            catch (Exception ex)
            {
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(Id));
				details.Add("CXE State:" + Convert.ToString(CXEState));
				details.Add("CXN State:" + Convert.ToString(CXNState));
				details.Add("Confirmation Number:" + ConfirmationNumber);
				details.Add("Type:" + Convert.ToString(Type));
				MongoDBLogger.ListError<string>(details, "UpdateTransactionDetails", AlloyLayerName.CORE, ModuleName.Transaction,
				 "Error in UpdateTransactionDetails -MGI.Core.Partner.Impl.TransactionServiceImpl", ex.Message, ex.StackTrace);
				
                throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_UPDATE_TRANSACTIONDETAILS_FAILED, ex);
            }
        }

        public void UpdateAmount(long Id, decimal amount)
        {
			T txn = Lookup(Id);

            try
            {
                string timezone = txn.CustomerSession.TimezoneID;

                if (txn.Type == (int)TransactionType.Funds || txn.Type == (int)TransactionType.MoneyTransfer || txn.Type == (int) TransactionType.Cash)
                {
                    txn.Amount = amount;
                    txn.DTTerminalLastModified = Clock.DateTimeWithTimeZone(timezone);
                    txn.DTServerLastModified = DateTime.Now;
                    _txnRepo.UpdateWithFlush(txn);
                }
            }
            catch (Exception ex)
            {
				//AL-1014 Transactional Log User Story
				List<string> details = new List<string>();
				details.Add("Transaction Id:" + Convert.ToString(Id));
				details.Add("Amount:" + Convert.ToString(amount));
				MongoDBLogger.ListError<string>(details, "UpdateAmount", AlloyLayerName.CORE, ModuleName.Transaction,
				 "Error in UpdateAmount -MGI.Core.Partner.Impl.TransactionServiceImpl", ex.Message, ex.StackTrace);
				
				throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_UPDATE_AMOUNT_FAILED, ex);
            }
        }

		public void Update(T txn)
		{
			try
			{
				txn.DTTerminalLastModified = Clock.DateTimeWithTimeZone(txn.CustomerSession.TimezoneID);
				txn.DTServerLastModified = DateTime.Now;

				_txnRepo.UpdateWithFlush(txn);
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<T>(txn, "Update", AlloyLayerName.CORE, ModuleName.Transaction,
				"Error in Update -MGI.Core.Partner.Impl.TransactionServiceImpl", ex.Message, ex.StackTrace);
				
				throw new ChannelPartnerException(ChannelPartnerException.TERMINAL_UPDATE_FAILED, ex);
			}
		}

		public List<T> GetAllForCustomer(long customerId)
		{
			return _txnRepo.FilterBy(t => t.Account.Customer.Id == customerId).ToList();
		}
		public void UpdateTransactionDetails(T transaction)
		{
			try
			{
				string timezone = transaction.CustomerSession.TimezoneID;
				
				transaction.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
				transaction.DTServerLastModified = DateTime.Now;//Changes for timestamp

				T txnT = (T)transaction;
				_txnRepo.UpdateWithFlush(txnT);
			}
			catch (Exception ex)
			{
				//AL-1014 Transactional Log User Story
				MongoDBLogger.Error<T>(transaction, "UpdateTransactionDetails", AlloyLayerName.CORE, ModuleName.Transaction,
				"Error in UpdateTransactionDetails -MGI.Core.Partner.Impl.TransactionServiceImpl", ex.Message, ex.StackTrace);
				
				throw new ChannelPartnerException(ChannelPartnerException.TRANSACTION_UPDATE_TRANSACTIONDETAILS_FAILED, ex);
			}
		}

    }
}
