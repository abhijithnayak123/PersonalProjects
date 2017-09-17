using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using AutoMapper;

using MGI.Common.Sys;
using MGI.Common.Util;
using MGI.Common.DataAccess.Contract;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using CheckStage = MGI.Core.CXE.Data.Transactions.Stage.Check;
using CheckCommit = MGI.Core.CXE.Data.Transactions.Commit.Check;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Core.CXE.Impl
{
	public class CheckServiceImpl : ICheckService
	{
		private IRepository<CheckStage> _checkStageRepo;
		public IRepository<CheckStage> CheckStageRepo
		{
			set { _checkStageRepo = value; }
		}

		private IRepository<CheckCommit> _checkCommitRepo;
		public IRepository<CheckCommit> CheckCommitRepo
		{
			set { _checkCommitRepo = value; }
		}
        public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }

		public CheckServiceImpl()
		{
			Mapper.CreateMap<CheckStage, CheckCommit>();
		}

		public CheckCommit Get(long Id)
		{
			try
			{
				return _checkCommitRepo.FindBy(x => x.Id == Id);
			}
			catch (Exception ex)
			{			
                NLogger.Error("Error in retrieving check transaction data. " + ex.Message);

				MongoDBLogger.Error<long>(Id, "Get", AlloyLayerName.CXE, ModuleName.ProcessCheck,
								"Error in Get - MGI.Core.CXE.Impl.CheckServiceImpl", ex.Message, ex.StackTrace);

				throw new CXECheckException(CXECheckException.CHECK_NOT_FOUND, string.Format("Could not find committed check {0}", Id), ex);
			}
		}

		public int GetStatus(long Id)
		{
			var check = getCheckStage(Id);
			return check.Status;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Id"></param>
		/// <param name="newStatus"></param>
		public void Update(long Id, int newStatus,string timezone)
        {
			var check = getCheckStage(Id);
			check.Status = newStatus;

            //changes for timestamp
            check.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            check.DTServerLastModified = DateTime.Now;
            try
            {
                _checkStageRepo.Merge(check);
            }
            catch (Exception ex)
            {              
                NLogger.Error("Error while updating check transaction. " + ex.Message);

				List<string> details = new List<string>();
				details.Add("Id : " + Convert.ToString(Id));
				details.Add("New Status : " + Convert.ToString(newStatus));
				details.Add("Timezone : " + timezone);

				MongoDBLogger.ListError<string>(details, "Update", AlloyLayerName.CXE, ModuleName.ProcessCheck,
								"Error in Update - MGI.Core.CXE.Impl.CheckServiceImpl", ex.Message, ex.StackTrace);

                throw new CXECheckException(CXECheckException.CHECK_UPDATE_FAILED, "Error while updating check transaction", ex);
            }
        }

		public void Update(long Id, int checkType, decimal fee,string timezone)
		{
			var check = getCheckStage(Id);
			check.CheckType = checkType;
			check.Fee = fee;

           //changes for timestamp
            check.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            check.DTServerLastModified = DateTime.Now;
			try
			{
				_checkStageRepo.Merge(check);
			}
			catch (Exception ex)
			{
                NLogger.Error("Error while updating check transaction. " + ex.Message);

				List<string> details = new List<string>();
				details.Add("Id : " + Convert.ToString(Id));
				details.Add("Check Type : " + Convert.ToString(checkType));
				details.Add("Fee : " + Convert.ToString(fee));
				details.Add("Timezone : " + timezone);

				MongoDBLogger.ListError<string>(details, "Update", AlloyLayerName.CXE, ModuleName.ProcessCheck,
								"Error in Update - MGI.Core.CXE.Impl.CheckServiceImpl", ex.Message, ex.StackTrace);

				throw new CXECheckException(CXECheckException.CHECK_UPDATE_FAILED, "Error while updating check transaction", ex);
			}
		}

		public long Create(CheckStage check,string timezone)
		{
			try
			{
                if (check.DTTerminalCreate == DateTime.MinValue)
                {
                    //changes for timestamp
                    check.DTTerminalCreate = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
                    check.DTServerCreate = DateTime.Now;
                }
				_checkStageRepo.AddWithFlush(check);

				return check.Id;
			}
			catch (Exception ex)
			{
				
                NLogger.Error("Error in saving check transaction data. " + ex.Message);

				MongoDBLogger.Error<CheckStage>(check, "Create", AlloyLayerName.CXE, ModuleName.ProcessCheck,
								"Error in Create - MGI.Core.CXE.Impl.CheckServiceImpl", ex.Message, ex.StackTrace);

				throw new CXECheckException(CXECheckException.CHECK_CREATE_FAILED, "Error in saving check transaction data", ex);
			}
		}

		public void Commit(long Id)
        {
			var checkStage = getCheckStage(Id);

            try
            {
                var checkCommit = Mapper.Map<Data.Transactions.Stage.Check, Data.Transactions.Commit.Check>(checkStage);
                _checkCommitRepo.Add(checkCommit);
            }
            catch (Exception ex)
            {
              
                NLogger.Error("Error while commiting check transaction. " + ex.Message);

				MongoDBLogger.Error<string>(Convert.ToString(Id), "Commit", AlloyLayerName.CXE, ModuleName.ProcessCheck,
								"Error in Commit - MGI.Core.CXE.Impl.CheckServiceImpl", ex.Message, ex.StackTrace);

                throw new CXECheckException(CXECheckException.CHECK_COMMIT_FAILED, "Error while commiting check transaction", ex);
            }
        }

		public CheckImages GetImages(long Id)
		{
			CheckStage checkStage = getCheckStage(Id);

			return checkStage.Images;
		}

		private CheckStage getCheckStage(long Id)
		{
			CheckStage check;
			try
			{
				check = _checkStageRepo.FindBy(x => x.Id == Id);
			}
			catch (Exception ex)
			{
				throw new CXECheckException(CXECheckException.CHECK_NOT_FOUND, string.Format("Could not find staged check {0}", Id), ex);
			}

			return check;
		}

        //AL-101 changes for updating reclassified check amount,checktype and associated fee
        public void Update(long Id, int checkType,decimal amount, decimal fee, string timezone)
        {
            var check = getCheckStage(Id);
            check.Amount = amount;
            check.CheckType = checkType;
            check.Fee = fee;

            //changes for timestamp
            check.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
            check.DTServerLastModified = DateTime.Now;
            try
            {
                _checkStageRepo.Merge(check);
            }
            catch (Exception ex)
            {

                NLogger.Error("Error while updating check transaction. " + ex.Message);

				List<string> details = new List<string>();
				details.Add("Id : " + Convert.ToString(Id));
				details.Add("Check Type : " + Convert.ToString(checkType));
				details.Add("Amount : " + Convert.ToString(amount));
				details.Add("Fee : " + Convert.ToString(fee));
				details.Add("Timezone : " + timezone);

				MongoDBLogger.ListError<string>(details, "Update", AlloyLayerName.CXE, ModuleName.ProcessCheck,
								"Error in Update - MGI.Core.CXE.Impl.CheckServiceImpl", ex.Message, ex.StackTrace);

                throw new CXECheckException(CXECheckException.CHECK_UPDATE_FAILED, "Error while updating check transaction", ex);
            }
        }
	}
}
