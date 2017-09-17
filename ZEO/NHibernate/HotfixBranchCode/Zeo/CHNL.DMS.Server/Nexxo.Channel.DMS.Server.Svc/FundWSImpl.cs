using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : IFundsProcessorService
	{
		public long Add(long customerSessionId, FundsProcessorAccount fundsAccount, MGIContext mgiContext)
		{
			return DesktopEngine.Add(customerSessionId, fundsAccount, mgiContext);
		}

		public bool AuthenticateCard(long customerSessionId, string cardNumber, string pin, MGIContext mgiContext)
		{
			return DesktopEngine.AuthenticateCard(customerSessionId, cardNumber, pin, mgiContext);
		}

		public long Load(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			return DesktopEngine.Load(customerSessionId, funds, mgiContext);
		}

		public long ActivateGPRCard(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			return DesktopEngine.ActivateGPRCard(customerSessionId, funds, mgiContext);
		}

		public long Withdraw(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			return DesktopEngine.Withdraw(customerSessionId, funds, mgiContext);
		}

		public Data.CardInfo GetBalance(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetBalance(customerSessionId, mgiContext);
		}

		public FundsProcessorAccount Lookup(string accountIdentifier, string agentSessionId, string customerSessionId, MGIContext mgiContext)
		{
			throw new NotImplementedException();
		}

		public FundsProcessorAccount LookupForPAN(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.LookupForPAN(customerSessionId, mgiContext);
		}

		public TransactionFee GetFee(long customerSessionId, decimal amount, FundType fundsType, MGIContext mgiContext)
		{
			return DesktopEngine.GetFee(customerSessionId, amount, fundsType, mgiContext);
		}

		// just changing this so it will build
		public long UpdateFundAmount(long customerSessionId, long cxeFundTrxId, decimal amount, FundType fundType, MGIContext mgiContext)
		{
			return DesktopEngine.UpdateFundAmount(customerSessionId, cxeFundTrxId, amount, fundType, mgiContext);
			//return DesktopEngine.UpdateFundAmount( customerSessionId, cxeFundTrxId, amount );
		}

		public decimal GetMinimumLoadAmount(long customerSessionId, bool initialLoad, MGIContext mgiContext)
		{
			return DesktopEngine.GetMinimumLoadAmount(customerSessionId, initialLoad, mgiContext);
		}


		public List<Data.CardTransactionHistory> GetCardTransactionHistory(long customerSessionId, Data.TransactionHistoryRequest request, MGIContext mgiContext)
		{
			return DesktopEngine.GetCardTransactionHistory(customerSessionId, request, mgiContext);
		}

		public bool CloseAccount(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.CloseAccount(customerSessionId, mgiContext);
		}

		public bool UpdateCardStatus(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			return DesktopEngine.UpdateCardStatus(customerSessionId, cardMaintenanceInfo, mgiContext);
		}

		public bool ReplaceCard(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			return DesktopEngine.ReplaceCard(customerSessionId, cardMaintenanceInfo, mgiContext);
		}

		public List<ShippingTypes> GetShippingTypes(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetShippingTypes(customerSessionId, mgiContext);

		}

		public double GetShippingFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			return DesktopEngine.GetShippingFee(customerSessionId, cardMaintenanceInfo, mgiContext);
		}

		public long AssociateCard(long customerSessionId, FundsProcessorAccount fundsAccount, MGIContext mgiContext)
		{
			return DesktopEngine.AssociateCard(customerSessionId, fundsAccount, mgiContext);
		}

		public double GetFundFee(long customerSessionId, CardMaintenanceInfo cardMaintenanceInfo, MGIContext mgiContext)
		{
			return DesktopEngine.GetFundFee(customerSessionId, cardMaintenanceInfo, mgiContext);
		}

		public long IssueAddOnCard(long customerSessionId, Funds funds, MGIContext mgiContext)
		{
			return DesktopEngine.IssueAddOnCard(customerSessionId, funds, mgiContext);
		}
	}
}
