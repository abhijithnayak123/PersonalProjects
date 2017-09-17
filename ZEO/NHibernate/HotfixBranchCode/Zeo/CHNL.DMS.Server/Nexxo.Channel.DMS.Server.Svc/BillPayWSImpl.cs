using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;
using MGI.Channel.Shared.Server.Data;

namespace MGI.Channel.DMS.Server.Svc
{

	public partial class DesktopWSImpl : IBillPayService
	{
		public List<string> GetBillers(long customerSessionId, long channelPartnerID, string searchTerm, MGIContext mgiContext)
		{
			return DesktopEngine.GetBillers(customerSessionId, channelPartnerID, searchTerm, mgiContext);
		}

		public Product GetBiller(long customerSessionId, long billerId, MGIContext mgiContext)
		{
			return DesktopEngine.GetBiller(customerSessionId, billerId, mgiContext);
		}

		public Product GetBillerByName(long customerSessionId, long channelPartnerID, string billerNameOrCode, MGIContext mgiContext)
		{
			return DesktopEngine.GetBillerByName(customerSessionId, channelPartnerID, billerNameOrCode, mgiContext);
		}

		public List<Product> GetFrequentBillers(long customerSessionId, long alloyId, MGIContext mgiContext)
		{
			return DesktopEngine.GetFrequentBillers(customerSessionId, alloyId, mgiContext);
		}

		public List<Product> GetAllBillers(long customerSessionId, long ChannelPartnerID, Guid LocationRegionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetAllBillers(customerSessionId, ChannelPartnerID, LocationRegionId, mgiContext);
		}

		public BillPayLocation GetLocations(long customerSessionId, string billerName, string accountNumber, decimal amount, MGIContext mgiContext)
		{
			return DesktopEngine.GetLocations(customerSessionId, billerName, accountNumber, amount, mgiContext);
		}

		public BillFee GetFee(long customerSessionId, string billerNameOrCode, string accountNumber, decimal amount, BillerLocation location, MGIContext mgiContext)
		{
			return DesktopEngine.GetFee(customerSessionId, billerNameOrCode, accountNumber, amount, location, mgiContext);
		}

		public BillerInfo GetBillerInfo(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
		{
			return DesktopEngine.GetBillerInfo(customerSessionId, billerNameOrCode, mgiContext);
		}

		public List<Field> GetProviderAttributes(long customerSessionId, string billerNameOrCode, string location, MGIContext mgiContext)
		{
			return DesktopEngine.GetProviderAttributes(customerSessionId, billerNameOrCode, location, mgiContext);
		}

		public FavoriteBiller GetFavoriteBiller(long customerSessionId, string billerNameOrCode, MGIContext mgiContext)
		{
			return DesktopEngine.GetFavoriteBiller(customerSessionId, billerNameOrCode, mgiContext);
		}
		public decimal GetBillPayFee(long customerSessionId, string providerName, MGIContext mgiContext)
		{
			return DesktopEngine.GetBillPayFee(customerSessionId, providerName, mgiContext);
		}

		public void StageBillPayment(long customerSessionId, long transactionID, MGIContext mgiContext)
		{
			DesktopEngine.StageBillPayment(customerSessionId, transactionID, mgiContext);
		}


		public long ValidateBillPayment(long customerSessionId, BillPayment billPayment, MGIContext mgiContext)
		{
			return DesktopEngine.ValidateBillPayment(customerSessionId, billPayment, mgiContext);
		}

		public CashierDetails GetAgent(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetAgent(customerSessionId, mgiContext);
		}

		public MGI.Channel.Shared.Server.Data.CardInfo GetCardInfo(long customerSessionId, MGIContext mgiContext)
		{
			return DesktopEngine.GetCardInfo(customerSessionId, mgiContext);
		}

		/// <summary>
		/// This method will publish the methods in the DMS Service Operation for Add Past Billers - User Story # US1646.
		/// </summary>
		/// <param name="customerSessionId">Session ID</param>
		/// <param name="cardNumber">Card Number</param>
		/// <param name="mgiContext">mgiContext</param>
		public void AddPastBillers(long customerSessionId, string cardNumber, MGIContext mgiContext)
		{
			DesktopEngine.AddPastBillers(customerSessionId, cardNumber, mgiContext);
		}

		public void CancelBillPayment(long customerSessionId, long transactionId, MGIContext mgiContext)
		{
			DesktopEngine.CancelBillPayment(customerSessionId, transactionId, mgiContext);
		}
		//Begin TA-191 Changes
		//       User Story Number: TA-191 | Server |   Developed by: Sunil Shetty     Date: 21.04.2015
		//       Purpose: The user / teller will have the ability to delete a specific biller from the Favorite Billers list. we are sending Customer session id and biller id.
		// the customer session id will get AlloyID/Pan ID and biller ID will help us to disable biller from tCustomerPreferedProducts table
		public void DeleteFavoriteBiller(long customerSessionId, long billerID, MGIContext mgiContext)
		{
			DesktopEngine.DeleteFavoriteBiller(customerSessionId, billerID, mgiContext);
		}
		//End TA-191 Changes
	}
}
