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
	public class FeeServiceImpl : IFeeService
	{

		private IRepository<CheckFee> _checkFeeRepo;
		public IRepository<CheckFee> CheckFeeRepo { set { _checkFeeRepo = value; } }

		private IRepository<FundsFee> _fundsFeeRepo;
		public IRepository<FundsFee> FundsFeeRepo { set { _fundsFeeRepo = value; } }

		private IRepository<MoneyOrderFee> _moneyOrderFeeRepo;
        public IRepository<MoneyOrderFee> MoneyOrderFeeRepo { get { return _moneyOrderFeeRepo; } set { _moneyOrderFeeRepo = value; } }

		private IFeeAdjustmentService _feeAdjustmentSvc;
		public IFeeAdjustmentService FeeAdjustmentService { set { _feeAdjustmentSvc = value; } }

		protected IChannelPartnerService _channelPartnerSvc;
		public IChannelPartnerService ChannelPartnerSvc{ set { _channelPartnerSvc = value; }}

		public IRepository<ChannelPartnerPricing> ChannelPartnerPricingRepo { private get; set; }

		private Dictionary<int, decimal> centrisCheckFeeRates;
		private Dictionary<int, decimal> centrisFundsFees;

		private Dictionary<int, decimal> synovusCheckFeeRates;
		private Dictionary<int, decimal> synovusFundsFees;
		private decimal synovusMoneyOrderFees;

		private Dictionary<int, decimal> carverCheckFeeRates;
		private Dictionary<int, decimal> carverFundsFees;
		private decimal carverMoneyOrderFees;

		private Dictionary<int, decimal> TCFCheckFeeRates;
		private Dictionary<int, decimal> TCFFundsFees;
		private decimal TCFMoneyOrderFees;

		private Dictionary<long, Dictionary<int, decimal>> channelPartnerCheckFees;
		private Dictionary<long, Dictionary<int, decimal>> channelPartnerFundsFees;
		private Dictionary<long, decimal> channelPartnerMoneyOrderFees;

		private Dictionary<long, decimal> channelPartnerCheckMinFees;
		private Dictionary<long, Dictionary<string, decimal>> billpayFees;
		private Dictionary<string, decimal> carverBillpayFees;
		public NLoggerCommon NLogger { get; set; }
		public TLoggerCommon MongoDBLogger { get; set; }

		// US1800 Referral & Referree Promotions
		public IRepository<CustomerFeeAdjustments> CustomerFeeAdjustmentsRepo { private get; set; }

		private IRepository<FeeAdjustment> _feeAdjustmentsRepo;
		public IRepository<FeeAdjustment> FeeAdjustmentsRepo { set { _feeAdjustmentsRepo = value; } }

		public FeeServiceImpl()
		{
			// hardcoded with Centris fees for now
			centrisCheckFeeRates = new Dictionary<int, decimal>();
			centrisCheckFeeRates.Add(1, 0.060m);
			centrisCheckFeeRates.Add(2, 0.020m);
			centrisCheckFeeRates.Add(3, 0.020m);
			centrisCheckFeeRates.Add(4, 0.015m);
			centrisCheckFeeRates.Add(5, 0.060m);
			centrisCheckFeeRates.Add(6, 0.040m);
			centrisCheckFeeRates.Add(7, 0.020m);
			centrisCheckFeeRates.Add(8, 0.020m);
			centrisCheckFeeRates.Add(9, 0.020m);
			centrisCheckFeeRates.Add(10, 0.060m);
			centrisCheckFeeRates.Add(11, 0.060m);
			centrisCheckFeeRates.Add(12, 0.060m);
			centrisCheckFeeRates.Add(13, 0.060m);
			centrisCheckFeeRates.Add(14, 0.034m);
			centrisCheckFeeRates.Add(15, 0.060m);
			centrisCheckFeeRates.Add(16, 0.060m);
			centrisCheckFeeRates.Add(17, 0.034m);

			synovusCheckFeeRates = new Dictionary<int, decimal>();
			synovusCheckFeeRates.Add(1, 0.029m);
			synovusCheckFeeRates.Add(2, 0.015m);
			synovusCheckFeeRates.Add(3, 0.015m);
			synovusCheckFeeRates.Add(4, 0.015m);
			synovusCheckFeeRates.Add(5, 0.049m);
			synovusCheckFeeRates.Add(6, 0.029m);
			synovusCheckFeeRates.Add(7, 0.015m);
			synovusCheckFeeRates.Add(8, 0.015m);
			synovusCheckFeeRates.Add(9, 0.015m);
			synovusCheckFeeRates.Add(10, 0.029m);
			synovusCheckFeeRates.Add(11, 0.029m);
			synovusCheckFeeRates.Add(12, 0.029m);
			synovusCheckFeeRates.Add(13, 0.029m);
			synovusCheckFeeRates.Add(14, 0.029m);
			synovusCheckFeeRates.Add(15, 0.029m);
			synovusCheckFeeRates.Add(16, 0.029m);
			synovusCheckFeeRates.Add(17, 0.029m);

			//Carver check fee rate
			carverCheckFeeRates = new Dictionary<int, decimal>();
			carverCheckFeeRates.Add(1, 0.0186m);
			carverCheckFeeRates.Add(2, 0.0186m);
			carverCheckFeeRates.Add(3, 0.0186m);
			carverCheckFeeRates.Add(4, 0.0186m);
			carverCheckFeeRates.Add(5, 0.0186m);
			carverCheckFeeRates.Add(6, 0.0186m);
			carverCheckFeeRates.Add(7, 0.0186m);
			carverCheckFeeRates.Add(8, 0.0186m);
			carverCheckFeeRates.Add(9, 0.0186m);
			carverCheckFeeRates.Add(10, 0.0186m);
			carverCheckFeeRates.Add(11, 0.0186m);
			carverCheckFeeRates.Add(12, 0.0186m);
			carverCheckFeeRates.Add(13, 0.0186m);
			carverCheckFeeRates.Add(14, 0.0186m);
			carverCheckFeeRates.Add(15, 0.0186m);
			carverCheckFeeRates.Add(16, 0.0186m);
			carverCheckFeeRates.Add(17, 0.0186m);

			//TCF check fee rate
			TCFCheckFeeRates = new Dictionary<int, decimal>();
			TCFCheckFeeRates.Add(1, 0.030m);
			TCFCheckFeeRates.Add(2, 0.010m);
			TCFCheckFeeRates.Add(3, 0.010m);
			TCFCheckFeeRates.Add(4, 0.010m);
			TCFCheckFeeRates.Add(5, 0.030m);
			TCFCheckFeeRates.Add(6, 0.030m);
			TCFCheckFeeRates.Add(7, 0.010m);
			TCFCheckFeeRates.Add(8, 0.010m);
			TCFCheckFeeRates.Add(9, 0.010m);
			TCFCheckFeeRates.Add(10, 0.030m);
			TCFCheckFeeRates.Add(11, 0.030m);
			TCFCheckFeeRates.Add(12, 0.030m);
			TCFCheckFeeRates.Add(13, 0.030m);
			TCFCheckFeeRates.Add(14, 0.030m);
			TCFCheckFeeRates.Add(15, 0.030m);
			TCFCheckFeeRates.Add(16, 0.030m);
			TCFCheckFeeRates.Add(17, 0.030m);

			channelPartnerCheckFees = new Dictionary<long, Dictionary<int, decimal>>();
			channelPartnerCheckFees.Add(27, centrisCheckFeeRates);
			channelPartnerCheckFees.Add(33, synovusCheckFeeRates);
			channelPartnerCheckFees.Add(28, carverCheckFeeRates);
			channelPartnerCheckFees.Add(34, TCFCheckFeeRates);

			centrisFundsFees = new Dictionary<int, decimal>();
			centrisFundsFees.Add(0, 2m);
			centrisFundsFees.Add(1, 2m);
			centrisFundsFees.Add(2, 0m);

			synovusFundsFees = new Dictionary<int, decimal>();
			synovusFundsFees.Add(0, 0m);
			synovusFundsFees.Add(1, 0m);
			synovusFundsFees.Add(2, 0m);

			//Carver fund fee 
			carverFundsFees = new Dictionary<int, decimal>();
			carverFundsFees.Add(0, 0m);
			carverFundsFees.Add(1, 0m);
			carverFundsFees.Add(2, 0m);

			//TCF fund fee  
			TCFFundsFees = new Dictionary<int, decimal>();
			TCFFundsFees.Add(0, 0m);
			TCFFundsFees.Add(1, 0m);
			TCFFundsFees.Add(2, 0m);

			channelPartnerFundsFees = new Dictionary<long, Dictionary<int, decimal>>();
			channelPartnerFundsFees.Add(27, centrisFundsFees);
			channelPartnerFundsFees.Add(33, synovusFundsFees);
			channelPartnerFundsFees.Add(28, carverFundsFees);
			channelPartnerFundsFees.Add(34, TCFFundsFees);

			//Synovus,Carver,TCF Money Order Fee 
			synovusMoneyOrderFees = 1m;
			carverMoneyOrderFees = 0.5m;
			TCFMoneyOrderFees = 5m;

			channelPartnerMoneyOrderFees = new Dictionary<long, decimal>();
			channelPartnerMoneyOrderFees.Add(33, synovusMoneyOrderFees);
			channelPartnerMoneyOrderFees.Add(28, carverMoneyOrderFees);
			channelPartnerMoneyOrderFees.Add(34, TCFMoneyOrderFees);

			channelPartnerCheckMinFees = new Dictionary<long, decimal>();
			channelPartnerCheckMinFees.Add(27, 2m);
			channelPartnerCheckMinFees.Add(33, 1m);
			channelPartnerCheckMinFees.Add(28, 3m);
			channelPartnerCheckMinFees.Add(34, 0m);

			carverBillpayFees = new Dictionary<string, decimal>();


			billpayFees = new Dictionary<long, Dictionary<string, decimal>>();
			billpayFees.Add(28, carverBillpayFees);

            NLogger = new NLoggerCommon();
		}

		#region IFeeService Members

		/// <summary>
		/// Author : Abhijith
		/// Description: Updated the existing method to add the pricing cluster logic.
		/// User Story: AL-1759
		/// </summary>
		/// <param name="session"></param>
		/// <param name="transactions"></param>
		/// <param name="amount"></param>
		/// <param name="checkType"></param>
		/// <param name="mgiContext"></param>
		/// <returns></returns>
		public TransactionFee GetCheckFee(CustomerSession session, List<Check> transactions, decimal amount, int checkType, MGIContext mgiContext)
		{
			// Author: Abhijith 
			// Description : Get the channel partner details to get the ProductPK from product name as there is no direct
			// methods to fetch the product details.
			// User Story: AL-1759
			//Starts Here

			decimal baseFee = 0;
			ChannelPartnerPricing cpPricing;

			ChannelPartner channelPartner = _channelPartnerSvc.ChannelPartnerConfig(mgiContext.ChannelPartnerRowGuid);
			var channelPartnerProductProviders = channelPartner.Providers.Where(c => c.ProductProcessor.Product.Name == MGI.Common.Util.Product.ProcessCheck.ToString()).FirstOrDefault();

			if (channelPartnerProductProviders != null && channelPartnerProductProviders.ProductProcessor != null
					&& channelPartnerProductProviders.ProductProcessor.Product != null)
			{
				var productPK = channelPartnerProductProviders.ProductProcessor.Product.rowguid;

				//Check the pricing cluster at the location level 
				cpPricing = ChannelPartnerPricingRepo.FindBy(f => f.ChannelPartner.rowguid == mgiContext.ChannelPartnerRowGuid
					&& f.Location.rowguid == mgiContext.LocationRowGuid && f.ProductType == checkType
					&& f.Product.rowguid == productPK);

				// If there are no location level pricing clusters then go for default cluster, which is 
				// at the channel partner level.
				if (cpPricing == null)
				{
					cpPricing = ChannelPartnerPricingRepo.FindBy(f => f.ChannelPartner.rowguid == mgiContext.ChannelPartnerRowGuid
						&& f.ProductType == checkType && f.Location.rowguid == null
						&& f.Product.rowguid == productPK);
				}
				
				if (cpPricing != null)
					baseFee = getFee(amount, cpPricing.PricingGroup);
				//User Story: AL-1759 Ends Here

				//US2030 - If Parked - Don't Apply any Promotions 
				//AL-3032 If Parked, remove only system promotions, persists manual promotions
				if (mgiContext.IsParked && mgiContext.IsSystemApplied)
					return new TransactionFee(baseFee, new List<FeeAdjustment>());

				//US1799 -Targeted promotions for check cashing and money order
				// Validating Promocode if SystemApplied is false
				if (!string.IsNullOrEmpty(mgiContext.PromotionCode) && !mgiContext.IsSystemApplied)
				{
					if (!ValidateCheckPromoCode(session, mgiContext)) // Has to be refactored more to use transaction type
						throw new ChannelPartnerException(ChannelPartnerException.PROMOCODE_INVALID, string.Format("Invalid PromoCode {0}", mgiContext.PromotionCode));
				}
				// US1799 changes - ends here
			}

			return new TransactionFee(baseFee, getCheckFeeAdjustments(session, transactions, amount, checkType, mgiContext));
		}

		/// <summary>
		/// Author : Abhijith
		/// Description: Updated the existing method to add the pricing cluster logic.
		/// User Story: AL-1759
		/// </summary>
		/// <param name="session"></param>
		/// <param name="transactions"></param>
		/// <param name="amount"></param>
		/// <param name="fundsType"></param>
		/// <param name="mgiContext"></param>
		/// <returns></returns>
		public TransactionFee GetFundsFee(CustomerSession session, List<Funds> transactions, decimal amount, int fundsType, MGIContext mgiContext)
		{
			// Author: Abhijith 
			// Description : Get the channel partner details to get the ProductPK from product name as there is no direct
			// methods to fetch the product details.
			// User Story: AL-1759
			//Starts Here
			decimal baseFee = 0;
			ChannelPartnerPricing fundPricing;

			ChannelPartner channelPartner = _channelPartnerSvc.ChannelPartnerConfig(mgiContext.ChannelPartnerRowGuid);
			var channelPartnerProductProviders = channelPartner.Providers.Where(c => c.ProductProcessor.Product.Name == MGI.Common.Util.Product.ProductCredential.ToString()).FirstOrDefault();

			if (channelPartnerProductProviders != null && channelPartnerProductProviders.ProductProcessor != null
					&& channelPartnerProductProviders.ProductProcessor.Product != null)
			{
				var productPK = channelPartnerProductProviders.ProductProcessor.Product.rowguid;

				//Check the pricing cluster at the location level 
				fundPricing = ChannelPartnerPricingRepo.FindBy(f => f.ChannelPartner.rowguid == mgiContext.ChannelPartnerRowGuid
					&& f.Location.rowguid == mgiContext.LocationRowGuid && f.Product.rowguid == productPK && f.ProductType == fundsType);

				// If there are no location level pricing clusters then go for default cluster, which is 
				// at the channel partner level.
				if (fundPricing == null)
				{
					fundPricing = ChannelPartnerPricingRepo.FindBy(f => f.ChannelPartner.rowguid == mgiContext.ChannelPartnerRowGuid && f.Location.rowguid == null
						 && f.Product.rowguid == productPK && f.ProductType == fundsType);
				}

				if (fundPricing != null)
					baseFee = getFee(amount, fundPricing.PricingGroup);
				//User Story: AL-1759 Ends Here
			}

			return new TransactionFee(baseFee, getFundsFeeAdjustments(session, transactions, amount, fundsType, mgiContext));
		}

		#endregion

		/// <summary>
		/// Author : Abhijith
		/// Description: Updated the existing method to add the pricing cluster logic.
		/// User Story: AL-1759
		/// </summary>
		/// <param name="session"></param>
		/// <param name="transactions"></param>
		/// <param name="amount"></param>
		/// <param name="mgiContext"></param>
		/// <returns></returns>
		public TransactionFee GetMoneyOrderFee(CustomerSession session, List<MoneyOrder> transactions, decimal amount, MGIContext mgiContext)
		{
			// Author: Abhijith 
			// Description : Get the channel partner details to get the ProductPK from product name as there is no direct
			// methods to fetch the product details.
			// User Story: AL-1759
			//Starts Here
			decimal baseFee = 0;
			ChannelPartnerPricing moPricing;

			ChannelPartner channelPartner = _channelPartnerSvc.ChannelPartnerConfig(mgiContext.ChannelPartnerRowGuid);
			var channelPartnerProductProviders = channelPartner.Providers.Where(c => c.ProductProcessor.Product.Name == MGI.Common.Util.Product.MoneyOrder.ToString()).FirstOrDefault();

			if (channelPartnerProductProviders != null && channelPartnerProductProviders.ProductProcessor != null
					&& channelPartnerProductProviders.ProductProcessor.Product != null)
			{
				var productPK = channelPartnerProductProviders.ProductProcessor.Product.rowguid;

				//Check the pricing cluster at the location level 
				moPricing = ChannelPartnerPricingRepo.FindBy(f => f.ChannelPartner.rowguid == mgiContext.ChannelPartnerRowGuid
					&& f.Location.rowguid == mgiContext.LocationRowGuid && f.Product.rowguid == productPK);

				// If there are no location level pricing clusters then go for default cluster, which is 
				// at the channel partner level.
				if (moPricing == null)
				{
					moPricing = ChannelPartnerPricingRepo.FindBy(f => f.ChannelPartner.rowguid == mgiContext.ChannelPartnerRowGuid
						 && f.Product.rowguid == productPK && f.Location.rowguid == null);
				}

				if (moPricing != null)
					baseFee = getFee(amount, moPricing.PricingGroup);
				//User Story: AL-1759 Ends Here

				////US2030 - If Parked - Don't Apply any Promotions
				//AL-3032 If Parked, remove only system promotions, persists manual promotions
				if (mgiContext.IsParked && mgiContext.IsSystemApplied)
					return new TransactionFee(baseFee, new List<FeeAdjustment>());

				//US1799 -Targeted promotions for check cashing and money order
				// Validating Promocode if SystemApplied is false
				if (!string.IsNullOrEmpty(mgiContext.PromotionCode) && !mgiContext.IsSystemApplied)
				{
					if (!ValidateMOPromoCode(session, mgiContext)) // Has to be refactored more to use transaction type
						throw new ChannelPartnerException(ChannelPartnerException.PROMOCODE_INVALID, string.Format("Invalid PromoCode {0}", mgiContext.PromotionCode));
				}

			}

			// just getting the base fees working for now
			return new TransactionFee(baseFee, getMOFeeAdjustments(session, transactions, amount, mgiContext));
		}

		public decimal GetBillPayFee(string providerName, long channelPartnerId, MGIContext mgiContext)
		{
			Dictionary<string, decimal> billpayFee;
			decimal fee;

			if (!billpayFees.TryGetValue(channelPartnerId, out billpayFee))
				throw new ChannelPartnerException(ChannelPartnerException.CHANNEL_PARTNER_BILLPAY_FEE_NOT_FOUND, string.Format("No Fees found for channel partner {0}", channelPartnerId));
			if (!billpayFee.TryGetValue(providerName.ToLower(), out fee))
				throw new ChannelPartnerException(ChannelPartnerException.CHANNEL_PARTNER_BILLPAY_FEE_NOT_FOUND, string.Format("No Fees found for provider  {0}", providerName));
			return fee;
		}


		/// <summary>
		// US1799 -Targeted promotions for check cashing and money order
		// Validating Promocode if SystemApplied is false for Check
		/// </summary>
		/// <param name="session"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		private bool ValidateCheckPromoCode(CustomerSession session, MGIContext mgiContext)
		{
			List<FeeAdjustment> feeAdjustments = _feeAdjustmentSvc.Lookup(session.AgentSession.Terminal.ChannelPartner);

			if (feeAdjustments.Exists(x => x.PromotionType != null && x.PromotionType.ToLower() == Core.Partner.Data.Fees.PromotionType.Code.ToString().ToLower() && x.SystemApplied == false && x.TransactionType == FeeAdjustmentTransactionType.Check && x.Name.ToLower() == mgiContext.PromotionCode.ToLower()))
				return true;
			else
				return false;
		}


		/// <summary>
		// US1799 -Targeted promotions for check cashing and money order
		// Validating Promocode if SystemApplied is false for MO
		/// </summary>
		/// <param name="session"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		private bool ValidateMOPromoCode(CustomerSession session, MGIContext mgiContext)
		{
			List<FeeAdjustment> feeAdjustments = _feeAdjustmentSvc.Lookup(session.AgentSession.Terminal.ChannelPartner);

			if (feeAdjustments.Exists(x => x.PromotionType != null && x.PromotionType.ToLower() == Core.Partner.Data.Fees.PromotionType.Code.ToString().ToLower() && x.SystemApplied == false && x.TransactionType == FeeAdjustmentTransactionType.MoneyOrder && x.Name.ToLower() == mgiContext.PromotionCode.ToLower()))
				return true;
			else
				return false;
		}

		private List<FeeAdjustment> getFundsFeeAdjustments(CustomerSession session, List<Funds> transactions, decimal amount, int fundsType, MGIContext mgiContext)
		{
			FeeAdjustmentTransactionType transType;

			if (fundsType == 0)
				transType = FeeAdjustmentTransactionType.FundsDebit;
			else if (fundsType == 1)
				transType = FeeAdjustmentTransactionType.FundsCredit;
			else
				transType = FeeAdjustmentTransactionType.FundsActivation;

			mgiContext.Amount = amount;

			return _feeAdjustmentSvc.GetApplicableAdjustments(transType, session, transactions.ToList<Transaction>(), mgiContext);
		}

		private List<FeeAdjustment> getMOFeeAdjustments(CustomerSession session, List<MoneyOrder> transactions, decimal amount, MGIContext mgiContext)
		{
			mgiContext.Amount = amount;
			if (string.IsNullOrEmpty(mgiContext.PromotionCode))
				mgiContext.PromotionCode = string.Empty;

			List<FeeAdjustment> feeAdjustments = _feeAdjustmentSvc.GetApplicableAdjustments(FeeAdjustmentTransactionType.MoneyOrder, session, transactions.ToList<Transaction>(), mgiContext);

			getCustomerMOFeeAdjustments(session, transactions, ref feeAdjustments, mgiContext);

			string adjustmentNames = string.Join(",", feeAdjustments.Select(a => a.Name).ToArray());

			// NLogger.Info(string.Format("{0} applicable adjustments: {1}", feeAdjustments.Count, adjustmentNames));

			return feeAdjustments;

		}

		private Guid getChannelPartnerId(CustomerSession session)
		{
			return session.AgentSession.Terminal.ChannelPartner.rowguid;
		}

		private TransactionFee defaultFee(decimal baseFee)
		{
			return new TransactionFee(baseFee, new List<FeeAdjustment>());
		}

		private List<FeeAdjustment> getCheckFeeAdjustments(CustomerSession session, List<Check> transactions, decimal amount, int checkType, MGIContext mgiContext)
		{
			mgiContext.CheckType = checkType;
			mgiContext.Amount = amount;
			if (string.IsNullOrEmpty(mgiContext.PromotionCode))
				mgiContext.PromotionCode = string.Empty;

			List<FeeAdjustment> feeAdjustments = _feeAdjustmentSvc.GetApplicableAdjustments(FeeAdjustmentTransactionType.Check, session, transactions.ToList<Transaction>(), mgiContext);

			getCustomerCheckFeeAdjustments(session, transactions, ref feeAdjustments, mgiContext);

			string adjustmentNames = string.Join(",", feeAdjustments.Select(a => a.Name).ToArray());

			NLogger.Info(string.Format("{0} applicable adjustments: {1}", feeAdjustments.Count, adjustmentNames));

			return feeAdjustments;
		}

		// US1800 Referral & Referree Promotions
		// Getting customer applicable promotions
		private void getCustomerCheckFeeAdjustments(CustomerSession session, List<Check> transactions, ref List<FeeAdjustment> feeAdjustments, MGIContext mgiContext)
		{
			var allPossibleAdjustments = _feeAdjustmentsRepo.FilterBy(a => a.TransactionType == FeeAdjustmentTransactionType.Check && a.channelPartner.rowguid == session.Customer.ChannelPartnerId
				&& DateTime.Today >= a.DTStart && (DateTime.Today <= a.DTEnd || a.DTEnd == null));

			foreach (FeeAdjustment f in allPossibleAdjustments)
			{
				var customerPosAdjustments = CustomerFeeAdjustmentsRepo.FilterBy(x => x.CustomerID == session.Customer.Id && x.IsAvailed == false && x.feeAdjustment == f).ToList();

				List<Check> addedAdjustments = transactions.Where(x => x.CXEState == (int)PTNRTransactionStates.Authorized && x.FeeAdjustments.Where(y => y.feeAdjustment == f).Any()).ToList();

				if (customerPosAdjustments != null && (customerPosAdjustments.Count() - addedAdjustments.Count()) > 0)
				{
					var customerfeeAdjustment = customerPosAdjustments.FirstOrDefault();
					if (customerfeeAdjustment.feeAdjustment != null)
						feeAdjustments.Add(customerfeeAdjustment.feeAdjustment);
				}
			}
		}


		// US1800 Referral & Referree Promotions
		// Getting customer applicable promotions
		private void getCustomerMOFeeAdjustments(CustomerSession session, List<MoneyOrder> transactions, ref List<FeeAdjustment> feeAdjustments, MGIContext mgiContext)
		{
			var allPossibleAdjustments = _feeAdjustmentsRepo.FilterBy(a => a.TransactionType == FeeAdjustmentTransactionType.MoneyOrder && a.channelPartner.rowguid == session.Customer.ChannelPartnerId
				&& DateTime.Today >= a.DTStart && (DateTime.Today <= a.DTEnd || a.DTEnd == null));

			foreach (FeeAdjustment f in allPossibleAdjustments)
			{
				var customerPosAdjustments = CustomerFeeAdjustmentsRepo.FilterBy(x => x.CustomerID == session.Customer.Id && x.IsAvailed == false && x.feeAdjustment == f).ToList();

				List<MoneyOrder> addedAdjustments = transactions.Where(x => x.CXEState == (int)PTNRTransactionStates.Authorized && x.FeeAdjustments.Where(y => y.feeAdjustment == f).Any()).ToList();

				if (customerPosAdjustments != null && (customerPosAdjustments.Count() - addedAdjustments.Count()) > 0)
				{
					var customerfeeAdjustment = customerPosAdjustments.FirstOrDefault();
					if (customerfeeAdjustment.feeAdjustment != null)
						feeAdjustments.Add(customerfeeAdjustment.feeAdjustment);
				}
			}
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Get the Fee for the products like - Check, MoneyOrder etc.
		/// User Story: AL-1759
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="group"></param>
		/// <returns></returns>
		private decimal getFee(decimal amount, PricingGroup group)
		{
			decimal baseFee = 0;
			decimal totalFee = 0;

			if (group != null)
			{
				foreach (var objPricing in group.Pricings)
				{
					if (meetCondition(amount, objPricing))
					{
						if (objPricing.IsPercentage) //For percentage  
						{
							totalFee = amount * (objPricing.Value / 100);  //percentage value will be in value column   

							if (totalFee > objPricing.MinimumFee)
								baseFee = totalFee;
							else
								baseFee = objPricing.MinimumFee;
						}
						else
						{
							baseFee = objPricing.Value;
						}

						return decimal.Round(baseFee,2);
					}
				}
			}
			
			return baseFee;
		}

		/// <summary>
		/// Author : Abhijith
		/// Description : Check the compare type in the pricing condition compare type. 
		/// Check the amount with the compare type. If it falls in the price range then return true.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="objPricing"></param>
		/// <returns></returns>
		private bool meetCondition(decimal amount, Pricing objPricing)
		{
			switch (objPricing.CompareType)
			{
				case 5:
					return amount > objPricing.MinimumAmount;
				case 6:
					return amount < objPricing.MaximumAmount;
				case 7:
					return amount >= objPricing.MinimumAmount;
				case 8:
					return amount <= objPricing.MaximumAmount;
				case 9:
					return amount >= objPricing.MinimumAmount && amount < objPricing.MaximumAmount;
				default:
					return true;  // For percentage and Fixed Fee

			}
		}

	}
}
