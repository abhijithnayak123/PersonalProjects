using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using AutoMapper;

using MGI.Common.Sys;
using MGI.Common.DataAccess.Contract;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MoneyOrderStage = MGI.Core.CXE.Data.Transactions.Stage.MoneyOrder;
using MoneyOrderCommit = MGI.Core.CXE.Data.Transactions.Commit.MoneyOrder;
using MGI.Common.Util;
using MGI.Common.TransactionalLogging.Data;

namespace MGI.Core.CXE.Impl
{
	public class MoneyOrderServiceImpl : IMoneyOrderService
	{
		private IRepository<MoneyOrderStage> _moneyOrderStageRepo;
		public IRepository<MoneyOrderStage> MoneyOrderStageRepo
		{
			set { _moneyOrderStageRepo = value; }
		}

		private IRepository<MoneyOrderCommit> _moneyOrderCommitRepo;
		public IRepository<MoneyOrderCommit> MoneyOrderCommitRepo
		{
			set { _moneyOrderCommitRepo = value; }
		}
        public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }

		public MoneyOrderServiceImpl()
		{
			Mapper.CreateMap<MoneyOrderStage, MoneyOrderCommit>();
            NLogger = new NLoggerCommon();
		}

		public long Create(MoneyOrderStage moneyOrder, string timezone)
		{
			try
			{
				_moneyOrderStageRepo.AddWithFlush(moneyOrder);

				return moneyOrder.Id;
			}
			catch (Exception ex)
			{
                NLogger.Error("Error in saving moneyOrder transaction data. " + ex.Message);

				MongoDBLogger.Error<MoneyOrderStage>(moneyOrder, "Create", AlloyLayerName.CXE, ModuleName.MoneyOrder,
							"Error in Create - MGI.Core.CXE.Impl.MoneyOrderServiceImpl", ex.Message, ex.StackTrace);

				throw new CXEMoneyOrderException(CXEMoneyOrderException.MONEYORDER_CREATE_FAILED, "Error in saving Money Order transaction data", ex);
			}
		}

		public void Update(long Id, int newStatus, string timezone)
		{
			var moneyOrder = GetStage(Id);
			moneyOrder.Status = newStatus;
			moneyOrder.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			moneyOrder.DTServerLastModified = DateTime.Now;
			try
			{
				_moneyOrderStageRepo.Merge(moneyOrder);
			}
			catch (Exception ex)
			{
                NLogger.Error("Error in updating moneyOrder transaction data. " + ex.Message);

				List<string> details = new List<string>();
				details.Add("Id : " + Convert.ToString(Id));
				details.Add("New Status : " + Convert.ToString(newStatus));
				details.Add("Timezone : " + timezone);

				MongoDBLogger.ListError<string>(details, "Update", AlloyLayerName.CXE, ModuleName.MoneyOrder,
								"Error in Update - MGI.Core.CXE.Impl.MoneyOrderServiceImpl", ex.Message, ex.StackTrace);

				throw new CXEMoneyOrderException(CXEMoneyOrderException.MONEYORDER_UPDATE_FAILED, string.Format("Could not update Money Order {0}", Id), ex);
			}
		}

		public void Update(long Id, string checkNumber, string accountNumber, string routingNumber, string micr, string timezone)
		{
			var moneyOrder = GetStage(Id);
			moneyOrder.MoneyOrderCheckNumber = checkNumber;
			moneyOrder.AccountNumber = accountNumber;
			moneyOrder.RoutingNumber = routingNumber;
			moneyOrder.MICR = micr;
			moneyOrder.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			moneyOrder.DTServerLastModified = DateTime.Now;
			try
			{
				_moneyOrderStageRepo.Merge(moneyOrder);
			}
			catch (Exception ex)
			{
               NLogger.Error ("Error in updating moneyOrder transaction data. " + ex.Message);

			   List<string> details = new List<string>();
			   details.Add("Id : " + Convert.ToString(Id));
			   details.Add("Check Number : " + checkNumber);
			   details.Add("Account Number : " + accountNumber);
			   details.Add("Routing Number : " + routingNumber);
			   details.Add("MICR : " + micr);
			   details.Add("Timezone : " + timezone);

			   MongoDBLogger.ListError<string>(details, "Update", AlloyLayerName.CXE, ModuleName.MoneyOrder,
							   "Error in Update - MGI.Core.CXE.Impl.MoneyOrderServiceImpl", ex.Message, ex.StackTrace);

				throw new CXEMoneyOrderException(CXEMoneyOrderException.MONEYORDER_UPDATE_FAILED, string.Format("Could not update Money Order {0}", Id), ex);
			}
		}

		public void Update(long Id, decimal fee, string timezone)
		{
			var moneyOrder = GetStage(Id);
			moneyOrder.Fee = fee;
			moneyOrder.DTTerminalLastModified = MGI.TimeStamp.Clock.DateTimeWithTimeZone(timezone);
			moneyOrder.DTServerLastModified = DateTime.Now;
			try
			{
				_moneyOrderStageRepo.Merge(moneyOrder);
			}
			catch (Exception ex)
			{
                NLogger.Error("Error in updating moneyOrder transaction data. " + ex.Message);

				List<string> details = new List<string>();
				details.Add("Id : " + Convert.ToString(Id));
				details.Add("Fee : " + Convert.ToString(fee));
				details.Add("Timezone : " + timezone);

				MongoDBLogger.ListError<string>(details, "Update", AlloyLayerName.CXE, ModuleName.MoneyOrder,
								"Error in Update - MGI.Core.CXE.Impl.MoneyOrderServiceImpl", ex.Message, ex.StackTrace);

				throw new CXEMoneyOrderException(CXEMoneyOrderException.MONEYORDER_UPDATE_FAILED, string.Format("Could not update Money Order {0}", Id), ex);
			}
		}

		public void Commit(long Id)
		{

			var moneyOrderStage = GetStage(Id);
			var moneyOrderCommit = Mapper.Map<Data.Transactions.Stage.MoneyOrder, Data.Transactions.Commit.MoneyOrder>(moneyOrderStage);
			moneyOrderCommit.Id = Id; //Id field will be used to relate stage and commit table.
			moneyOrderCommit.Status = (int)TransactionStates.Committed;
			try
			{
				_moneyOrderCommitRepo.SaveOrUpdate(moneyOrderCommit);
			}
			catch (Exception ex)
			{
                NLogger.Error("Error in commiting moneyOrder transaction data. " + ex.Message);

				MongoDBLogger.Error<string>(Convert.ToString(Id), "Commit", AlloyLayerName.CXE, ModuleName.MoneyOrder,
								"Error in Commit - MGI.Core.CXE.Impl.MoneyOrderServiceImpl", ex.Message, ex.StackTrace);

				throw new CXEMoneyOrderException(CXEMoneyOrderException.MONEYORDER_COMMIT_FAILED, string.Format("Could not commit Money Order {0}", Id), ex);
			}
		}

		public MoneyOrderCommit Get(long Id)
		{
			try
			{
				return _moneyOrderCommitRepo.FindBy(x => x.Id == Id);
			}
			catch (Exception ex)
			{
                NLogger.Error("Error in retrieving MoneyOrder transaction data. " + ex.Message);

				MongoDBLogger.Error<string>(Convert.ToString(Id), "Get", AlloyLayerName.CXE, ModuleName.MoneyOrder,
								"Error in Get - MGI.Core.CXE.Impl.MoneyOrderServiceImpl", ex.Message, ex.StackTrace);

				throw new CXEMoneyOrderException(CXEMoneyOrderException.MONEYORDER_NOT_FOUND, string.Format("Could not find committed MoneyOrder {0}", Id), ex);
			}
		}

        
		public MoneyOrderStage GetStage(long Id)
		{
			try
			{
				return _moneyOrderStageRepo.FindBy(x => x.Id == Id);
			}
			catch (Exception ex)
			{
                NLogger.Error("Error in retrieving MoneyOrder transaction data. " + ex.Message);

				MongoDBLogger.Error<string>(Convert.ToString(Id), "GetStage", AlloyLayerName.CXE, ModuleName.MoneyOrder,
								"Error in GetStage - MGI.Core.CXE.Impl.MoneyOrderServiceImpl", ex.Message, ex.StackTrace);

				throw new CXEMoneyOrderException(CXEMoneyOrderException.MONEYORDER_NOT_FOUND, string.Format("Could not find committed MoneyOrder {0}", Id), ex);
			}
		}

        public IList<MoneyOrderCommit> GetMOByMICR(string micr)
        {
            try
            {
                return _moneyOrderCommitRepo.FilterBy(x => x.MICR == micr).ToList();
            }
            catch (Exception ex)
            {
                NLogger.Error("Error in retrieving MoneyOrder transaction data. " + ex.Message);

				MongoDBLogger.Error<string>(micr, "GetMOByMICR", AlloyLayerName.CXE, ModuleName.MoneyOrder,
								"Error in GetMOByMICR - MGI.Core.CXE.Impl.MoneyOrderServiceImpl", ex.Message, ex.StackTrace);

                throw new CXEMoneyOrderException(CXEMoneyOrderException.MONEYORDER_NOT_FOUND, string.Format("could not find committed MoneyOrder {0}", micr), ex);
            }
        }


	}
}
