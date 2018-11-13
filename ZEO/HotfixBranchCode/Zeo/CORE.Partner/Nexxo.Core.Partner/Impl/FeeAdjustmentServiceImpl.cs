using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using MGI.Common.DataAccess.Data;
using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Core.Partner.Data.Fees;
using MGI.Core.Partner.Contract;
using MGI.Common.Util;

namespace MGI.Core.Partner.Impl
{
	public class FeeAdjustmentServiceImpl : IFeeAdjustmentService
	{

		private IRepository<FeeAdjustment> _feeAdjustmentsRepo;
		public IRepository<FeeAdjustment> FeeAdjustmentsRepo { set { _feeAdjustmentsRepo = value; } }

		// AL-591: This is introduced to update IsActive Status in tTxn_FeeAdjustments, this issue occured as in ODS Report we found duplicate transactions and the reason was tTxn_FeeAdjustments table having duplicate records
		// Developed by: Sunil Shetty || 03/07/2015
        private IRepository<TransactionFeeAdjustment> _transactionFeeAdjustmentRepo;
        public IRepository<TransactionFeeAdjustment> TransactionFeeAdjustmentRepo { set { _transactionFeeAdjustmentRepo = value; } }

		private IFeeAdjustmentConditionFactory _feeConditionFactory;
		public IFeeAdjustmentConditionFactory FeeConditionFactory { set { _feeConditionFactory = value; } }
		public NLoggerCommon NLogger { get; set; }
		//// US1800 Referral & Referree Promotions
		//public IRepository<CustomerFeeAdjustments> CustomerFeeAdjustmentsRepo { private get; set; }        

		public List<FeeAdjustment> GetApplicableAdjustments(FeeAdjustmentTransactionType transactionType, CustomerSession session, List<Transaction> transactions, MGIContext mgiContext)
		{
			var allPossibleAdjustments = _feeAdjustmentsRepo.FilterBy(a => a.TransactionType == transactionType && a.channelPartner.rowguid == session.Customer.ChannelPartnerId
				&& DateTime.Today >= a.DTStart && (DateTime.Today <= a.DTEnd || a.DTEnd == null));

			// NLogger.Info(string.Format("{0} possible adjustments", allPossibleAdjustments.Count()));

			List<FeeAdjustment> applicableAdjustments = new List<FeeAdjustment>();
			// US1800 Referral & Referree Promotions
			List<FeeAdjustment> customerApplicableAdjustments = new List<FeeAdjustment>();
			foreach (FeeAdjustment f in allPossibleAdjustments)
			{
				List<IFeeCondition> conditions = getConditions(f);
				if (conditions.TrueForAll(c => c.MeetsCondition(session, transactions, mgiContext)))
					applicableAdjustments.Add(f);

				//// US1800 Referral & Referree Promotions
				//// US1800 Referral & Referree Promotions
				//var customerPosAdjustments = CustomerFeeAdjustmentsRepo.FilterBy(x => x.CustomerID == session.Customer.Id && x.IsAvailed == false && x.feeAdjustment == f).FirstOrDefault();

				//if (customerPosAdjustments != null && customerPosAdjustments.feeAdjustment != null)
				//	applicableAdjustments.Add(customerPosAdjustments.feeAdjustment);
				//// US1800 Referral & Referree Promotions - Ends here
			}

			//string adjustmentNames = string.Join(",", applicableAdjustments.Select(a => a.Name).ToArray());
			return applicableAdjustments;
		}


		/// <summary>
		/// US1800 Referral & Referree Promotions
		/// Lookup for FeeAdjustments for a ChannelPartner
		/// </summary>
		/// <param name="channelPartner"></param>
		/// <returns></returns>
		public List<FeeAdjustment> Lookup(ChannelPartner channelPartner)
		{
			return _feeAdjustmentsRepo.All().Where(x => x.channelPartner == channelPartner && DateTime.Today >= x.DTStart && (DateTime.Today <= x.DTEnd || x.DTEnd == null)).ToList();
		}

		private List<IFeeCondition> getConditions(FeeAdjustment f)
		{
			List<IFeeCondition> conditions = new List<IFeeCondition>();
			f.Conditions.ToList().ForEach(a => conditions.Add(_feeConditionFactory.GetFeeCondition(a)));
			return conditions;
		}
		// AL-591: The below methos is introduced to Soft delete feeadjustment ny using IsActive Status in tTxn_FeeAdjustments, this issue occured as in ODS Report we found duplicate transactions and the reason was tTxn_FeeAdjustments table having duplicate records
		// Developed by: Sunil Shetty || 03/07/2015
		public void DeleteFeeAdjustments(Guid checkTransactionId)
		{
			List<TransactionFeeAdjustment> transactionFeeAdjustment = _transactionFeeAdjustmentRepo.FilterBy(m => m.transaction.rowguid == checkTransactionId).ToList();
			if (transactionFeeAdjustment != null)
			{
				foreach (TransactionFeeAdjustment feeAdjustment in transactionFeeAdjustment)
				{
					feeAdjustment.IsActive = false;
					_transactionFeeAdjustmentRepo.UpdateWithFlush(feeAdjustment);
				}
			}

		}
	}
}
