using System;
using System.Collections.Generic;
using NUnit.Framework;
using MGI.Biz.Customer.Data;
using MGI.Biz.Events.Contract;
using CxnCustomer = MGI.Cxn.Customer.Data.CustomerProfile;
using PtnrCustomer = MGI.Core.Partner.Data.Customer;
using MGI.Common.Util;
using MGI.Unit.Test;
using PTNRData = MGI.Core.Partner.Data;
using Moq;

namespace MGI.Biz.Carver.Test
{
    [TestFixture]
    class CarverCustomerEditEventListenerTest : BaseClass_Fixture
    {
		public Dictionary<string, INexxoBizEventListener> CarverEventListenerDictionary { private get; set; }

		[Test]
		public void NotifyTest()
		{
			PtnrCustomer pt = new PtnrCustomer() 
            { 
                Id = 33, 
                CustomerProfileStatus = ProfileStatus.Active,
                Accounts = new List<PTNRData.Account>() { new PTNRData.Account() { CXNId = 1000000000000000, ProviderId = 602 } },
            };
			MGIContext mgiContext = new MGIContext() { };
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.Context.Add("FetchedFromCustomerLookUp", new Customer.Data.CustomerSearchCriteria());
			mgiContext.Context.Add("CustomerLookUpPartnerAccountNumber", "12345678");
			mgiContext.Context.Add("CustomerLookUpRelationshipAccountNumber", "12345678");
			mgiContext.Context.Add("CustomerLookUpBankId", "1001");
			mgiContext.Context.Add("CustomerLookUpBranchId", "1245");
			mgiContext.Context.Add("PTNRCustomer", pt);
			mgiContext.Context.Add("ProviderId", 402);
			mgiContext.ProviderId = 402;

			CustomerEditEvent EventData = new CustomerEditEvent()
			{
				Name = "Customer-Edit-Carver",
				profile = new CustomerProfile() { FirstName = "ANNA", MiddleName = "Test", LastName = "LName", ProfileStatus = ProfileStatus.Active, GovernmentIDType = "DRIVER'S LICENCE", PrimaryCountryCitizenship = "US" },
				mgiContext = mgiContext,
			};


			INexxoBizEventListener nexxoEvent = CarverEventListenerDictionary[EventData.Name];
			nexxoEvent.Notify(EventData);

			CxnClientCustomerService.Verify(moq => moq.Update(It.IsAny<string>(), It.IsAny<CxnCustomer>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Notify_Customer_Registration()
		{
			PtnrCustomer ptnrCustomer = new PtnrCustomer()
			{
				Accounts = new List<PTNRData.Account>() { new PTNRData.Account() { CXNId = 1000000000000000, ProviderId = 602 } },
				rowguid = Guid.NewGuid(),
				Id = 1000000001,
			};
			MGIContext mgiContext = new MGIContext() { };
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.Context.Add("FetchedFromCustomerLookUp", new Customer.Data.CustomerSearchCriteria());
			mgiContext.Context.Add("CustomerLookUpPartnerAccountNumber", "12345678");
			mgiContext.Context.Add("CustomerLookUpRelationshipAccountNumber", "12345678");
			mgiContext.Context.Add("CustomerLookUpBankId", "1001");
			mgiContext.Context.Add("CustomerLookUpBranchId", "1245");
			mgiContext.Context.Add("PTNRCustomer", ptnrCustomer);

			CustomerRegistrationEvent customerRegistrationEvent = new CustomerRegistrationEvent()
			{
				Name = "Customer-Registration-Carver",
				mgiContext = mgiContext,
				profile = new CustomerProfile()
				{
					GovernmentIDType = "PASSPORT",
					FirstName = "Nitish",
					MiddleName = "Biradar",
					LastName = "Biradar"
				},
			};

			INexxoBizEventListener nexxoEvent = CarverEventListenerDictionary[customerRegistrationEvent.Name];
			nexxoEvent.Notify(customerRegistrationEvent);

			CxnClientCustomerService.Verify(moq => moq.AddCXNAccount(It.IsAny<CxnCustomer>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}
    }
}
