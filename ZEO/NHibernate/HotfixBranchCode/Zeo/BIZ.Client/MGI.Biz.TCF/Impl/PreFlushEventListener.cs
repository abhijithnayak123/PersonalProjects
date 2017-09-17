using AutoMapper;
using MGI.Biz.Events.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IPtnrDataStructureService = MGI.Core.Partner.Contract.INexxoDataStructuresService;
using CxnCustomerTransactionDetails = MGI.Cxn.Partner.TCF.Data.CustomerTransactionDetails;
using CxnCustomerProfile = MGI.Cxn.Customer.Data.CustomerProfile;
using CXNCustomer = MGI.Cxn.Partner.TCF.Data.Customer;
using MGI.Cxn.Customer.Contract;
using MGI.Biz.Partner.Data;
using MGI.Biz.Customer.Data;
using MGI.Cxn.Partner.TCF.Impl;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Common.Util;
using MGI.Cxn.Partner.TCF.Contract;

namespace MGI.Biz.TCF.Impl
{
	public class PreFlushEventListener : INexxoBizEventListener
	{
		public IFlushProcessor GatewayService { private get; set; }

		public IPtnrDataStructureService PTNRDataStructureService { private get; set; }
		public MGI.Cxn.Customer.Contract.IClientCustomerService CxnClientCustomerService { private get; set; }

		public void Notify(NexxoBizEvent bizEvent)
		{
			Mapper.CreateMap<CXNCustomer, CxnCustomerProfile>()
				.ForMember(c => c.CustInd, o => o.MapFrom(s => s.CustInd));
				

			PreFlushEvent EventData = (PreFlushEvent)bizEvent;

			PreFlush(EventData.CustomerTransactionDetails, EventData.mgiContext);
		}

		private void PreFlush(CxnCustomerTransactionDetails cartDetails, MGIContext mgiContext)
		{
			CxnCustomerProfile customerProfile = Mapper.Map<CXNCustomer, CxnCustomerProfile>(cartDetails.Customer);

			bool cxnTcfCustInd = CxnClientCustomerService.GetCustInd(mgiContext.CxnAccountId, mgiContext);

			if (cartDetails.Customer.CustInd && !cxnTcfCustInd)
			{
				CxnClientCustomerService.Update(Convert.ToString(mgiContext.CxnAccountId), customerProfile, mgiContext);
			}

			if (cartDetails.Customer.IdType.ToUpper() == "PASSPORT")
			{
				cartDetails.Customer.IdIssuer = cartDetails.Customer.IdIssuerCountryCode;
			}

			cartDetails.Customer.PrimaryCountryCitizenship = BaseClass.GetMasterCountryName(cartDetails.Customer.PrimaryCountryCitizenship);
			cartDetails.Customer.SecondaryCountryCitizenship = BaseClass.GetMasterCountryName(cartDetails.Customer.SecondaryCountryCitizenship);

			for (var i = 0; i <= cartDetails.Transactions.Count - 1; i++)
			{
				if (cartDetails.Transactions[i].Type == Convert.ToString(TransactionType.MoneyTransfer))
				{
					cartDetails.Transactions[i].ToCountry = cartDetails.Transactions[i].ToCountry != null ? BaseClass.GetMasterCountryName(cartDetails.Transactions[i].ToCountry) : null;
					cartDetails.Transactions[i].ToCountryOfBirth = cartDetails.Transactions[i].ToCountryOfBirth != null ? BaseClass.GetMasterCountryName(cartDetails.Transactions[i].ToCountryOfBirth) : null;
					cartDetails.Transactions[i].ToPickUpCountry = cartDetails.Transactions[i].ToPickUpCountry != null ? BaseClass.GetMasterCountryName(cartDetails.Transactions[i].ToPickUpCountry) : null;
				}
			}

			Dictionary<string, string> fullName = BaseClass.TruncateFullName(cartDetails.Customer.FirstName, cartDetails.Customer.MiddleName, cartDetails.Customer.LastName, cartDetails.Customer.SecondLastName);

			cartDetails.Customer.FirstName = fullName["FirstName"];
			cartDetails.Customer.MiddleName = fullName["MiddleName"];
			cartDetails.Customer.LastName = fullName["LastName"];
			cartDetails.Customer.SecondLastName = fullName["SecondLastName"];

			GatewayService.PreFlush(cartDetails, mgiContext);
		}
	}
}
