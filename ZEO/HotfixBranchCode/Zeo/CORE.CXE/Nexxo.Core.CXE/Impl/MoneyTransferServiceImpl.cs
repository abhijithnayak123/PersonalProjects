using AutoMapper;
using MGI.Common.DataAccess.Contract;
using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using System;
using MGI.Common.Util;
using System.Diagnostics;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Core.CXE.Impl
{
	public class MoneyTransferServiceImpl : IMoneyTransferService
	{
		private IRepository<Data.Transactions.Stage.MoneyTransfer> _moneyTransferStageRepo;
		private IRepository<Data.Transactions.Commit.MoneyTransfer> _moneyTransferCommitRepo;

		public IRepository<Data.Transactions.Stage.MoneyTransfer> MoneyTransferStageRepo
		{
			set { _moneyTransferStageRepo = value; }
		}

		public IRepository<Data.Transactions.Commit.MoneyTransfer> MoneyTransferCommitRepo
		{
			set { _moneyTransferCommitRepo = value; }
		}

        public NLoggerCommon NLogger { get; set; }
        public TLoggerCommon MongoDBLogger { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public MoneyTransferServiceImpl()
		{
			Mapper.CreateMap<Data.Transactions.Stage.MoneyTransfer, Data.Transactions.Commit.MoneyTransfer>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="funds"></param>
		/// <returns></returns>
		public long Create(Data.Transactions.Stage.MoneyTransfer moneytransfer)
		{
			try
			{
				_moneyTransferStageRepo.AddWithFlush(moneytransfer);
				return moneytransfer.Id;
			}
			catch (Exception ex)
			{
                //AL-3370 Transactional Log User Story
                MongoDBLogger.Error<Data.Transactions.Stage.MoneyTransfer>(moneytransfer, "Create", AlloyLayerName.CORE, ModuleName.Transaction,
                            "Error in Create - MGI.Core.CXE.Impl.MoneyTransferServiceImpl", ex.Message, ex.StackTrace);

                NLogger.Error("Error while staging money transfer. " + ex.Message);
				throw new CXEMoneyTransferException(CXEMoneyTransferException.MONEYTRANSFER_CREATE_FAILED, "Error in saving SendMoney stage transaction data", ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="state"></param>
		public void Update(long Id, TransactionStates state, string timeZone)
		{
			try
			{
				var moneytransfer = _moneyTransferStageRepo.FindBy(x => x.Id == Id);
				moneytransfer.Status = (int)state;
				moneytransfer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timeZone);
				moneytransfer.DTServerLastModified = DateTime.Now;
				_moneyTransferStageRepo.Merge(moneytransfer);

			}
			catch (Exception ex)
			{

                //AL-1014 Transactional Log User Story
                MongoDBLogger.Error<TransactionStates>(state, "Update", AlloyLayerName.CORE, ModuleName.Transaction,
                            "Error in Update - MGI.Core.CXE.Impl.MoneyTransferServiceImpl", ex.Message, ex.StackTrace);
                NLogger.Error("Error while updating money transfer. " + ex.Message);
				throw new CXEMoneyTransferException(CXEMoneyTransferException.MONEYTRANSFER_UPDATE_FAILED, "Error while updating money transfer",ex);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="state"></param>
        /// <param name="timeZone"></param>
        /// <param name="confirmationNumber"></param>
        public void Update(long Id, TransactionStates state, string timeZone, string confirmationNumber)
        {
            try
            {
                var moneytransfer = _moneyTransferStageRepo.FindBy(x => x.Id == Id);
                moneytransfer.Status = (int)state;
                moneytransfer.DTServerLastModified = DateTime.Now;
                moneytransfer.ConfirmationNumber = confirmationNumber;
                _moneyTransferStageRepo.Merge(moneytransfer);

            }
            catch (Exception ex)
            {
 				//AL-3370 Transactional Log User Story
                MongoDBLogger.Error<TransactionStates>(state, "Update", AlloyLayerName.CXE, ModuleName.Transaction,
                "Error in Update -MGI.Core.CXE.Impl.MoneyTransferServiceImpl", ex.Message, ex.StackTrace);
                NLogger.Error("Error while updating money transfer. " + ex.Message);
                throw new CXEMoneyTransferException(CXEMoneyTransferException.MONEYTRANSFER_UPDATE_FAILED, "Error while updating money transfer",ex);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Id"></param>
		public void Commit(long Id)
		{
			try
			{
				var moneytransferStage = _moneyTransferStageRepo.FindBy(x => x.Id == Id);
				var moneytransferCommit = Mapper.Map<Data.Transactions.Stage.MoneyTransfer, Data.Transactions.Commit.MoneyTransfer>(moneytransferStage);
				moneytransferCommit.Id = Id; //Id field will be used to relate stage and commit table.
				moneytransferCommit.Status = (int)TransactionStates.Committed;
				_moneyTransferCommitRepo.SaveOrUpdate(moneytransferCommit);
			}
			catch (Exception ex)
			{
 				//AL-3370 Transactional Log User Story
                MongoDBLogger.Error<string>(Convert.ToString(Id), "Commit", AlloyLayerName.CXE, ModuleName.Transaction,
                "Error in Commit -MGI.Core.CXE.Impl.MoneyTransferServiceImpl", ex.Message, ex.StackTrace);
                NLogger.Error("Error while commiting money transfer. " + ex.Message);
				throw new CXEMoneyTransferException(CXEMoneyTransferException.MONEYTRANSFER_COMMIT_FAILED, "Error while commiting money transfer",ex);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		public Data.Transactions.Commit.MoneyTransfer Get(long Id)
		{
			try
			{
				return _moneyTransferCommitRepo.FindBy(x => x.Id == Id);
			}
			catch (Exception ex)
			{
				//AL-3370 Transactional Log User Story
				MongoDBLogger.Error<string>(Convert.ToString(Id), "Get", AlloyLayerName.CXE, ModuleName.Transaction,
				"Error in Get -MGI.Core.CXE.Impl.MoneyTransferServiceImpl", ex.Message, ex.StackTrace);

                NLogger.Error("Error in retrieving money transfer. " + ex.Message);
				throw new CXEMoneyTransferException(CXEMoneyTransferException.MONEYTRANSFER_NOT_FOUND, "Error in retrieving money transfer",ex);
			}
		}


		public Data.Transactions.Stage.MoneyTransfer GetStage(long Id)
		{
			try
			{
				return _moneyTransferStageRepo.FindBy(c => c.Id == Id);
			}
			catch (Exception ex)
			{
                //AL-3370 Transactional Log User Story
                MongoDBLogger.Error<string>(Convert.ToString(Id), "GetStage", AlloyLayerName.CORE, ModuleName.Transaction,
                            "Error in GetStage - MGI.Core.CXE.Impl.MoneyTransferServiceImpl", ex.Message, ex.StackTrace);

                NLogger.Error("Error in retrieving money transfer. " + ex.Message);
				throw new CXEMoneyTransferException(CXEMoneyTransferException.MONEYTRANSFER_NOT_FOUND, "Error in retrieving money transfer stage",ex);
			}

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="state"></param>
		public void Update(Data.Transactions.Stage.MoneyTransfer moneytransfer, string timezone)
		{
			try
			{
				//changes for timestamp
				moneytransfer.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
				moneytransfer.DTServerLastModified = DateTime.Now; // Added for TimeStamp
				_moneyTransferStageRepo.Update(moneytransfer);
			}
			catch (Exception ex)
			{
                //AL-3370 Transactional Log User Story
                MongoDBLogger.Error<Data.Transactions.Stage.MoneyTransfer>(moneytransfer, "Update", AlloyLayerName.CORE, ModuleName.Transaction,
                            "Error in Update - MGI.Core.CXE.Impl.MoneyTransferServiceImpl", ex.Message, ex.StackTrace);

                NLogger.Error("Error while updating money transfer. " + ex.Message);
				throw new CXEMoneyTransferException(CXEMoneyTransferException.MONEYTRANSFER_UPDATE_FAILED, "Error while updating money transfer",ex);
			}
		}
	}
}
