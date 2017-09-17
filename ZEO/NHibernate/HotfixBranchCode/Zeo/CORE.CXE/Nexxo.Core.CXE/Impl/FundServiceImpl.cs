using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using AutoMapper;
using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;


using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Core.CXE.Impl
{
    public class FundServiceImpl : IFundsService
    {
        private IRepository<Data.Transactions.Stage.Funds> _fundsStageRepo;
        private IRepository<Data.Transactions.Commit.Funds> _fundsCommitRepo;

        public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }

        public IRepository<Data.Transactions.Stage.Funds> FundsStageRepo
        {
            set { _fundsStageRepo = value; }
        }

        public IRepository<Data.Transactions.Commit.Funds> FundsCommitRepo
        {
            set { _fundsCommitRepo = value; }
        }

        public FundServiceImpl()
        {
            Mapper.CreateMap<Data.Transactions.Stage.Funds, Data.Transactions.Commit.Funds>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funds"></param>
        /// <returns></returns>
        public long Create(Data.Transactions.Stage.Funds funds)
        {
            try
            {
                _fundsStageRepo.AddWithFlush(funds);
                return funds.Id;
            }
            catch (Exception ex)
            {
                NLogger.Error("Error in saving fund stage transaction data. " + ex.Message);

				//AL-3372 transaction information for GPR cards.
				MongoDBLogger.Error<Data.Transactions.Stage.Funds>(funds, "Create", AlloyLayerName.CXE, ModuleName.Funds,
					"Error in Create - MGI.Core.CXE.Impl.FundServiceImpl", ex.Message, ex.StackTrace);		


                throw new CXEFundException(CXEFundException.FUND_CREATE_FAILED, "Error in saving fund stage transaction data", ex);
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
                var funds = _fundsStageRepo.FindBy(x => x.Id == Id);
                funds.Status = (int)state;
                funds.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
                funds.DTServerLastModified = DateTime.Now;
                _fundsStageRepo.UpdateWithFlush(funds);

            }
            catch (Exception ex)
            {
                NLogger.Error("Error in updating fund stage transaction data. " + ex.Message);

				//AL-3372 transaction information for GPR cards.
				List<string> details = new List<string>();
				details.Add("Id : " + Convert.ToString(Id));
				details.Add("Transaction State : " + Convert.ToString(state));
				details.Add("Timezone : " + timezone);
				MongoDBLogger.ListError<string>(details, "Update", AlloyLayerName.CXE, ModuleName.Funds,
					"Error in Update - MGI.Core.CXE.Impl.FundServiceImpl", ex.Message, ex.StackTrace);
			
                throw new CXEFundException(CXEFundException.FUND_UPDATE_FAILED, "Error in updating fund stage transaction data", ex);
            }
        }

        public void UpdateAmount(long Id, decimal amount, string timezone)
        {
            try
            {
                var funds = _fundsStageRepo.FindBy(x => x.Id == Id);
                funds.Amount = amount;
                funds.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
                funds.DTServerLastModified = DateTime.Now;
                _fundsStageRepo.UpdateWithFlush(funds);
            }
            catch (Exception ex)
            {
                NLogger.Error("Error in updating fund stage transaction data. " + ex.Message);

				//AL-3372 transaction information for GPR cards.
				List<string> details = new List<string>();
				details.Add("Id : " + Convert.ToString(Id));
				details.Add("Amount : " + Convert.ToString(amount));
				details.Add("Timezone : " + timezone);
				MongoDBLogger.ListError<string>(details, "UpdateAmount", AlloyLayerName.CXE, ModuleName.Funds,
					"Error in UpdateAmount - MGI.Core.CXE.Impl.FundServiceImpl", ex.Message, ex.StackTrace);				

                throw new CXEFundException(CXEFundException.FUND_UPDATE_FAILED, "Error in updating fund stage transaction data", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        public void Commit(long Id, string timezone)
        {
            try
            {
                var fundsStage = _fundsStageRepo.FindBy(x => x.Id == Id);
                var fundCommit = Mapper.Map<Data.Transactions.Stage.Funds, Data.Transactions.Commit.Funds>(fundsStage);
                fundCommit.Id = Id; //Id field will be used to relate stage and commit table.
                fundCommit.Status = (int)TransactionStates.Committed;
                //Changes for timestamp
                fundCommit.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
                fundCommit.DTServerLastModified = DateTime.Now;
                _fundsCommitRepo.SaveOrUpdate(fundCommit);
            }
            catch (Exception ex)
            {
                NLogger.Error("Error in committing fund stage transaction data. " + ex.Message);

				//AL-3372 transaction information for GPR cards.
				MongoDBLogger.Error<string>(Convert.ToString(Id), "Commit", AlloyLayerName.CXE, ModuleName.Funds,
					"Error in Commit - MGI.Core.CXE.Impl.FundServiceImpl", ex.Message, ex.StackTrace);
				
                throw new CXEFundException(CXEFundException.FUND_COMMIT_FAILED, "Error in committing fund stage transaction data", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Data.Transactions.Commit.Funds Get(long Id)
        {
            try
            {
                return _fundsCommitRepo.FindBy(x => x.Id == Id);
            }
            catch (Exception ex)
            {
                NLogger.Error("Error in Getting fund transaction data. " + ex.Message);

				//AL-3372 transaction information for GPR cards.
				MongoDBLogger.Error<string>(Convert.ToString(Id), "Get", AlloyLayerName.CXE, ModuleName.Funds,
					"Error in Get - MGI.Core.CXE.Impl.FundServiceImpl", ex.Message, ex.StackTrace);
				
                throw new CXEFundException(CXEFundException.FUND_NOT_FOUND, "Error in Getting fund transaction data", ex);
            }
        }
    }
}
