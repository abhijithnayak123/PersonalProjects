using System;
using System.Collections.Generic;
using MGI.Biz.Customer.Data;
using MGI.Biz.FundsEngine.Data;
using MGI.Biz.Events.Contract;
using CxnCustomer = MGI.Cxn.Customer.Data.CustomerProfile;
using PtnrCustomer = MGI.Core.Partner.Data.Customer;
using PTNRData = MGI.Core.Partner.Data;
using MGI.Common.Util;
using NUnit.Framework;
using MGI.Unit.Test;
using Moq;


namespace MGI.Biz.Synovus.Test
{
    [TestFixture]
    class SynovusCustomerEditEventListenerTest : BaseClass_Fixture
    {
		public Dictionary<string, INexxoBizEventListener> SynovusEventListenerDictionary { private get; set; }

        [Test]
        public void NotifyTest()
		{
			PtnrCustomer pt = new PtnrCustomer() { Id = 33, CustomerProfileStatus = ProfileStatus.Active };
			var cxncontext = new Dictionary<string, object>();
			cxncontext.Add("PTNRCustomer", pt);
			cxncontext.Add("ProviderId", 402);
			CustomerEditEvent EventData = new CustomerEditEvent()
			{
				Name = "Customer-Edit-Synovus",
				profile = new CustomerProfile() { FirstName = "ANNA", MiddleName ="Test", LastName = "LName", ProfileStatus = ProfileStatus.Active, GovernmentIDType = "DRIVER'S LICENCE", PrimaryCountryCitizenship = "US" },
				mgiContext = new MGI.Common.Util.MGIContext() { Context = cxncontext, ProviderId = 402, ChannelPartnerId = 33 },
			};

			INexxoBizEventListener nexxoEvent = SynovusEventListenerDictionary[EventData.Name];
			nexxoEvent.Notify(EventData);

			CxnClientCustomerService.Verify(moq => moq.Update(It.IsAny<string>(), It.IsAny<CxnCustomer>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
        }

		[Test]
		public void Can_Notify_Edit_Customer()
		{
			PtnrCustomer pt = new PtnrCustomer() { Id = 33, CustomerProfileStatus = ProfileStatus.Active };
			var cxncontext = new Dictionary<string, object>();
			cxncontext.Add("PTNRCustomer", pt);
			cxncontext.Add("ProviderId", 402);
			CustomerEditEvent EventData = new CustomerEditEvent()
			{
				Name = "Customer-Edit-Synovus",
				profile = new CustomerProfile() { FirstName = "ANNA", MiddleName = "Test", LastName = "LName", ProfileStatus = ProfileStatus.Active, GovernmentIDType = "DRIVER'S LICENCE", PrimaryCountryCitizenship = "US" },
				mgiContext = new MGI.Common.Util.MGIContext() { Context = cxncontext, ProviderId = 402, ChannelPartnerId = 33 },
			};

			INexxoBizEventListener nexxoEvent = SynovusEventListenerDictionary[EventData.Name];
			nexxoEvent.Notify(EventData);

			CxnClientCustomerService.Verify(moq => moq.Update(It.IsAny<string>(), It.IsAny<CxnCustomer>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Notify_Customer_Registration()
		{
			PtnrCustomer ptnrCustomer = new PtnrCustomer()
			{
				Accounts = new List<PTNRData.Account>() { new PTNRData.Account() { CXNId = 1000000000000000, ProviderId = 602 } },
				rowguid = Guid.NewGuid(),
				Id = 1000000000,
			};
			MGIContext mgiContext = new MGIContext() { };
			mgiContext.Context = new Dictionary<string, object>();
			mgiContext.Context.Add("PTNRCustomer", ptnrCustomer);
			mgiContext.Context.Add("FetchedFromCustomerLookUp", true);
			mgiContext.Context.Add("CustomerLookUpRelationshipAccountNumber", "1234567891");
			mgiContext.Context.Add("CustomerLookUpProgramId", "ProgramId");
			mgiContext.Context.Add("CustomerLookUpPartnerAccountNumber", "12345678");
			mgiContext.Context.Add("CustomerLookUpBankId", "1001");
			mgiContext.Context.Add("CustomerLookUpBranchId", "15948");
			mgiContext.ChannelPartnerName = "Synovus";

			CustomerRegistrationEvent customerRegistrationEvent = new CustomerRegistrationEvent()
			{
				Name = "Customer-Registration-Synovus",
				mgiContext = mgiContext,
				profile = new CustomerProfile()
				{
					GovernmentIDType = "PASSPORT",
					FirstName = "Nitish",
					MiddleName = "Biradar",
					LastName = "Biradar"
				},
			};

			INexxoBizEventListener nexxoEvent = SynovusEventListenerDictionary[customerRegistrationEvent.Name];
			nexxoEvent.Notify(customerRegistrationEvent);

			CxnClientCustomerService.Verify(moq => moq.Add(It.IsAny<CxnCustomer>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Notify_For_Add_GPR_Account()
		{
			MGIContext mgiContext = new MGIContext() { };
			mgiContext.Context = new Dictionary<string, object>();
			GPRAddEvent gprAddEvent = new GPRAddEvent() { Name = "GPRAdd-Synovus", CXNId = 1000000000, mgiContext = mgiContext };

			INexxoBizEventListener nexxoEvent = SynovusEventListenerDictionary[gprAddEvent.Name];
			nexxoEvent.Notify(gprAddEvent);

			CxnClientCustomerService.Verify(moq => moq.AddAccount(It.IsAny<CxnCustomer>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}
    }
}
