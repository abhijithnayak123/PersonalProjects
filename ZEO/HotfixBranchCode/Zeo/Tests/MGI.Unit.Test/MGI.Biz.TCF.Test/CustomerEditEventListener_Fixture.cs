using MGI.Biz.Customer.Data;
using MGI.Biz.Events.Contract;
using MGI.Common.Util;
using CXNData = MGI.Cxn.Customer.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using PTNRData = MGI.Core.Partner.Data;
using Moq;
using MGI.Biz.Partner.Data;
using CXNPartnerTCF = MGI.Cxn.Partner.TCF.Data;

namespace MGI.Unit.Test.MGI.Biz.TCF.Test
{
	[TestFixture]
	public class CustomerEditEventListener_Fixture : BaseClass_Fixture
	{
		public Dictionary<string, INexxoBizEventListener> TCFEventListenerDictionary { private get; set; }

		[Test]
		public void Can_Notify()
		{
			PTNRData.Customer ptnrCustomer = new PTNRData.Customer() 
			{ 
				Accounts = new List<PTNRData.Account>() { new PTNRData.Account(){ CXNId = 1000000000000000, ProviderId = 602} }, 
				rowguid = Guid.NewGuid(),
				Id = 1000000001,
			};
			CustomerEditEvent customerEditEvent = new CustomerEditEvent();
			customerEditEvent.mgiContext = new Common.Util.MGIContext();
			customerEditEvent.mgiContext.Context = new Dictionary<string, object>();
			customerEditEvent.mgiContext.Context.Add("PTNRCustomer", ptnrCustomer);
			customerEditEvent.mgiContext.ProviderId = 602;
			customerEditEvent.mgiContext.CXECustomerId = 1000000000;
			customerEditEvent.profile = new CustomerProfile() 
			{
				GovernmentIDType = "PASSPORT",
				FirstName = "Nitish",
				MiddleName = "Biradar",
				LastName = "Biradar",
				LastName2 = "Test"
			};

			INexxoBizEventListener nexxoEvent = TCFEventListenerDictionary["Customer-Edit"];

			nexxoEvent.Notify(customerEditEvent);

			CxnClientCustomerService.Verify(moq => moq.Update(It.IsAny<string>(), It.IsAny<CXNData.CustomerProfile>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
			//CxnClientCustomerService.Verify(moq => moq.Add(It.IsAny<CXNData.CustomerProfile>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Notify_Registration()
		{
			PTNRData.Customer ptnrCustomer = new PTNRData.Customer()
			{
				Accounts = new List<PTNRData.Account>() { new PTNRData.Account() { CXNId = 1000000000000000, ProviderId = 602 } },
				rowguid = Guid.NewGuid(),
				Id = 1000000001,
			};
			CustomerRegistrationEvent customerRegistrationEvent = new CustomerRegistrationEvent();
			customerRegistrationEvent.mgiContext = new Common.Util.MGIContext();
			customerRegistrationEvent.mgiContext.Context = new Dictionary<string, object>();
			customerRegistrationEvent.mgiContext.Context.Add("PTNRCustomer", ptnrCustomer);
			customerRegistrationEvent.mgiContext.ProviderId = 602;
			customerRegistrationEvent.mgiContext.CXECustomerId = 1000000000;
			customerRegistrationEvent.profile = new CustomerProfile()
			{
				GovernmentIDType = "PASSPORT",
				FirstName = "Nitish",
				MiddleName = "Biradar",
				LastName = "Biradar"
			};

			INexxoBizEventListener nexxoEvent = TCFEventListenerDictionary["Customer-Registration"];

			nexxoEvent.Notify(customerRegistrationEvent);

			CxnClientCustomerService.Verify(moq => moq.Add(It.IsAny<CXNData.CustomerProfile>(), It.IsAny<MGIContext>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Notify_Customer_SyncIn()
		{
			PTNRData.Customer ptnrCustomer = new PTNRData.Customer()
			{
				Accounts = new List<PTNRData.Account>() { new PTNRData.Account() { CXNId = 1000000000000000, ProviderId = 602 } },
				rowguid = Guid.NewGuid(),
				Id = 1000000001,
			};

			CustomerSyncInEvent customerSyncInEvent = new CustomerSyncInEvent();
			customerSyncInEvent.mgiContext = new Common.Util.MGIContext();
			customerSyncInEvent.mgiContext.Context = new Dictionary<string, object>();
			customerSyncInEvent.mgiContext.Context.Add("PTNRCustomer", ptnrCustomer);
			customerSyncInEvent.mgiContext.ProviderId = 602;
			customerSyncInEvent.mgiContext.CXECustomerId = 1000000000;
			customerSyncInEvent.cxnCustomerId = 1000000000;

			INexxoBizEventListener nexxoEvent = TCFEventListenerDictionary["Customer-SyncIn"];

			nexxoEvent.Notify(customerSyncInEvent);

			CxnClientCustomerService.Verify(moq => moq.Fetch(It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Notify_PreFlush()
		{
			CXNPartnerTCF.Customer ptnrCustomer = new CXNPartnerTCF.Customer() { IdType = "PASSPORT", FirstName = "Nitish", LastName= "Biradar", MiddleName ="", CustInd = true };
			PreFlushEvent preFlushEvent = new PreFlushEvent();
			preFlushEvent.CustomerTransactionDetails = new Cxn.Partner.TCF.Data.CustomerTransactionDetails() { Customer = ptnrCustomer };
			preFlushEvent.mgiContext = new Common.Util.MGIContext();
			preFlushEvent.mgiContext.Context = new Dictionary<string, object>();
			preFlushEvent.mgiContext.ProviderId = 602;
			preFlushEvent.mgiContext.CXECustomerId = 1000000000;

			INexxoBizEventListener nexxoEvent = TCFEventListenerDictionary["PreFlush-ShoppingCart"];

			nexxoEvent.Notify(preFlushEvent);

			GatewayService.Verify(moq => moq.PreFlush(It.IsAny<Cxn.Partner.TCF.Data.CustomerTransactionDetails>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Notify_PostFlush()
		{
			CXNPartnerTCF.Customer ptnrCustomer = new CXNPartnerTCF.Customer() { IdType = "PASSPORT", FirstName = "Nitish", LastName = "Biradar", MiddleName = "", CustInd = true };
			PostFlushEvent postFlushEvent = new PostFlushEvent();
			postFlushEvent.CustomerTransactionDetails = new Cxn.Partner.TCF.Data.CustomerTransactionDetails() { Customer = ptnrCustomer };
			postFlushEvent.mgiContext = new Common.Util.MGIContext();
			postFlushEvent.mgiContext.Context = new Dictionary<string, object>();
			postFlushEvent.mgiContext.ProviderId = 602;
			postFlushEvent.mgiContext.CXECustomerId = 1000000000;

			INexxoBizEventListener nexxoEvent = TCFEventListenerDictionary["PostFlush-ShoppingCart"];

			nexxoEvent.Notify(postFlushEvent);

			GatewayService.Verify(moq => moq.PostFlush(It.IsAny<Cxn.Partner.TCF.Data.CustomerTransactionDetails>(), It.IsAny<MGIContext>()), Times.Exactly(1));
		}
	}
}
