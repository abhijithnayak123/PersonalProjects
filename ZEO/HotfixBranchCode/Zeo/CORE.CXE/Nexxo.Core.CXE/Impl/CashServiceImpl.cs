using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AutoMapper;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Common.Util;

using MGI.Common.DataAccess.Contract;
using System.Diagnostics;
using MGI.TimeStamp;
using MGI.Common.TransactionalLogging.Data;



namespace MGI.Core.CXE.Impl
{
    public class CashServiceImpl : ICashService
    {
        private IRepository<Data.Transactions.Commit.Cash> _cashCommitRepo;
        public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }
        public IRepository<Data.Transactions.Commit.Cash> CashCommitRepo
        {
            set { _cashCommitRepo = value; }
        }

        private IRepository<Data.Transactions.Stage.Cash> _cashStageRepo;
        public IRepository<Data.Transactions.Stage.Cash> CashStageRepo
        {
            set { _cashStageRepo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public CashServiceImpl()
        {
            Mapper.CreateMap<Data.Transactions.Stage.Cash, Data.Transactions.Commit.Cash>();
            NLogger = new NLoggerCommon();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        public long Create(Data.Transactions.Stage.Cash cash)
        {
            try
            {
                _cashStageRepo.AddWithFlush(cash);
                return cash.Id;
            }
            catch (Exception ex)
            {
                NLogger.Error("Error in commiting cash transaction data. " + ex.Message);

				MongoDBLogger.Error<Data.Transactions.Stage.Cash>(cash, "Create", AlloyLayerName.CXE, ModuleName.CashIn,
							"Error in Create - MGI.Core.CXE.Impl.CashServiceImpl", ex.Message, ex.StackTrace);

                throw new CXECashException(CXECashException.CASH_CREATE_FAILED, string.Format("Could not create cash"), ex);
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="state"></param>
		public void Update(long Id, TransactionStates state, string timezone)
		{
			try
			{
				var cash = _cashStageRepo.FindBy(x => x.Id == Id);
				cash.Status = (int)state;
				cash.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
				cash.DTServerLastModified = DateTime.Now;
				_cashStageRepo.UpdateWithFlush(cash);

			}
			catch (Exception ex)
			{
				NLogger.Error("Error in updating cash stage transaction data. " + ex.Message);
				throw new CXECashException(CXECashException.CASH_UPDATE_FAILED, "Error in updating cash stage transaction data", ex);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cash"></param>
        /// <returns></returns>
        public void Commit(long Id, string timezone)
        {
            try
            {
                //changes for timestamp
                var cashStage = _cashStageRepo.FindBy(x => x.Id == Id);
                var cashCommit = Mapper.Map<Data.Transactions.Stage.Cash, Data.Transactions.Commit.Cash>(cashStage);
                cashCommit.Id = Id; //Id field will be used to relate stage and commit table.
                cashCommit.Status = (int)TransactionStates.Committed;
                cashCommit.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
                cashCommit.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
                cashCommit.DTServerCreate = DateTime.Now;
                cashCommit.DTServerLastModified = DateTime.Now; //Added for Timestamp
                _cashCommitRepo.SaveOrUpdate(cashCommit);
            }
            catch (Exception ex)
            {
                NLogger.Error("Error in commiting cash transaction data. " + ex.Message);

				List<string> details = new List<string>();
				details.Add("Id : " + Convert.ToString(Id));
				details.Add("Timezone : " + timezone);

				MongoDBLogger.ListError<string>(details, "Commit", AlloyLayerName.CXE, ModuleName.CashIn,
							"Error in Commit - MGI.Core.CXE.Impl.CashServiceImpl", ex.Message, ex.StackTrace);

                throw new CXECashException(CXECashException.CASH_COMMIT_FAILED, string.Format("Could not commit cash {0}", Id), ex);
            }
        }

		//AL-2729 user story for updating the cash-in transaction
		public void UpdateAmount(long Id, decimal amount, string timezone)
		{
			try
			{ 
			var cashStage = _cashStageRepo.FindBy(x => x.Id == Id);
			cashStage.Amount = amount;
			cashStage.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			cashStage.DTServerLastModified = DateTime.Now;
			_cashStageRepo.UpdateWithFlush(cashStage);
			}
			catch (Exception ex)
            {
				NLogger.Error("Error in updating fund stage transaction data. " + ex.Message);
				throw new CXECashException(CXECashException.CASH_UPDATE_FAILED, string.Format("Could not update cash {0}", Id), ex);
			}
		}
    }
}
