using MGI.Biz.Events.Contract;
using IPtnrDataStructureService = MGI.Core.Partner.Contract.INexxoDataStructuresService;
using CxnCustomerTransactionDetails = MGI.Cxn.Partner.TCF.Data.CustomerTransactionDetails;
using CxnCustomerProfile = MGI.Cxn.Customer.Data.CustomerProfile;
using CXNCustomer = MGI.Cxn.Partner.TCF.Data.Customer;
using MGI.Biz.Partner.Data;
using MGI.Cxn.Partner.TCF.Impl;
using System.Collections.Generic;
using MGI.Core.Partner.Data.Transactions;
using System;
using MGI.Core.Partner.Data;
using MGI.Common.Util;
using AutoMapper;
using MGI.Cxn.Partner.TCF.Contract;

namespace MGI.Biz.TCF.Impl
{
	public class PostFlushEventListener : INexxoBizEventListener
	{
		public IFlushProcessor GatewayService { private get; set; }

		public IPtnrDataStructureService PTNRDataStructureService { private get; set; }

		public MGI.Cxn.Customer.Contract.IClientCustomerService CxnClientCustomerService { private get; set; }
		
		public void Notify(NexxoBizEvent bizEvent)
		{
			Mapper.CreateMap<CXNCustomer, CxnCustomerProfile>()
				.ForMember(c => c.CustInd, o => o.MapFrom(s => s.CustInd));

			PostFlushEvent EventData = (PostFlushEvent)bizEvent;

			PostFlush(EventData.CustomerTransactionDetails, EventData.mgiContext);
		}

		private void PostFlush(CxnCustomerTransactionDetails customertranDetails, MGIContext mgiContext)
		{
			CxnCustomerProfile customerProfile = Mapper.Map<CXNCustomer, CxnCustomerProfile>(customertranDetails.Customer);

			bool cxnTcfCustInd = CxnClientCustomerService.GetCustInd(mgiContext.CxnAccountId, mgiContext);

			if (!cxnTcfCustInd)
			{
				customerProfile.CustInd = true;
				CxnClientCustomerService.Update(Convert.ToString(mgiContext.CxnAccountId), customerProfile, mgiContext);
			}

			if (customertranDetails.Customer.IdType.ToUpper() == "PASSPORT")
			{
				customertranDetails.Customer.IdIssuer = customertranDetails.Customer.IdIssuerCountryCode;
			}

			customertranDetails.Customer.PrimaryCountryCitizenship = BaseClass.GetMasterCountryName(customertranDetails.Customer.PrimaryCountryCitizenship);
			customertranDetails.Customer.SecondaryCountryCitizenship = BaseClass.GetMasterCountryName(customertranDetails.Customer.SecondaryCountryCitizenship);

			for (var i = 0; i <= customertranDetails.Transactions.Count - 1; i++)
			{
				if (customertranDetails.Transactions[i].Type == Convert.ToString(TransactionType.MoneyTransfer))
				{
					customertranDetails.Transactions[i].ToCountry = customertranDetails.Transactions[i].ToCountry != null ? BaseClass.GetMasterCountryName(customertranDetails.Transactions[i].ToCountry) : null;
					customertranDetails.Transactions[i].ToCountryOfBirth = customertranDetails.Transactions[i].ToCountryOfBirth != null ? BaseClass.GetMasterCountryName(customertranDetails.Transactions[i].ToCountryOfBirth) : null;
					customertranDetails.Transactions[i].ToPickUpCountry = customertranDetails.Transactions[i].ToPickUpCountry != null ? BaseClass.GetMasterCountryName(customertranDetails.Transactions[i].ToPickUpCountry) : null;
				}
			}

			Dictionary<string, string> fullName = BaseClass.TruncateFullName(customertranDetails.Customer.FirstName, customertranDetails.Customer.MiddleName, customertranDetails.Customer.LastName, customertranDetails.Customer.SecondLastName);

			customertranDetails.Customer.FirstName = fullName["FirstName"];
			customertranDetails.Customer.MiddleName = fullName["MiddleName"];
			customertranDetails.Customer.LastName = fullName["LastName"];
			customertranDetails.Customer.SecondLastName = fullName["SecondLastName"];

			GatewayService.PostFlush(customertranDetails, mgiContext);
		}
	}
}
