using System;
using System.Collections.Generic;
using CXEData = MGI.Core.CXE.Data;
using NUnit.Framework;
using MGI.Biz.Customer.Impl;
using MGI.Biz.Customer.Data;
using MGI.Biz.Customer.Contract;
using MGI.Common.Util;
using MGI.Unit.Test;
using Moq;
using PTNRData = MGI.Core.Partner.Data;
using MGI.Biz.Events.Contract;

namespace MGI.Biz.Customer.Test
{
    [TestFixture]
    public class CustomerServiceCoreWrapper_Fixture : BaseClass_Fixture
    {
		public CustomerServiceValidatingWrapper InterceptedBizCustomerEngine { get; set; }

		[Test]
		public void Can_Register_Customer()
		{
			long agentSessionId = 1000000001;
			SessionContext sessionContext = new SessionContext() { AgentSessionId = Guid.NewGuid(), LocationId = Guid.Parse("BC46F466-16D3-47B9-97CC-A9F95E2A2CCB"), TimezoneId = "Standard Time Zone" };
			long alloyId = 1000000000000001;
			MGIContext mgiContext = new MGIContext() { };

			InterceptedBizCustomerEngine.Register(agentSessionId, sessionContext, alloyId, mgiContext);

			CXECustomerService.Verify(moq => moq.Register(It.IsAny<CXEData.Customer>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Search_Customer()
		{
			long agentSessionId = 1000000001;
			CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria() { LastName = "Biradar", SSN = "123456789" };
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34 };

			List<CustomerSearchResult> customerSearchResult = InterceptedBizCustomerEngine.Search(agentSessionId, customerSearchCriteria, mgiContext);

			Assert.AreNotEqual(customerSearchResult.Count, 0);
		}

		[Test]
		public void Can_Initiate_CustomerSession()
		{
			long agentSessionId = 1000000001;
			long alloyId = 1000000000000000;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34 };

			MGI.Biz.Customer.Data.CustomerSession customerSession = InterceptedBizCustomerEngine.InitiateCustomerSession(agentSessionId, alloyId, mgiContext);

			Assert.IsNotNull(customerSession);
		}

		[Test]
		public void Can_Get_Customer()
		{
			long agentSessionId = 1000000001;
			long alloyId = 1000000000000000;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34 };

			MGI.Biz.Customer.Data.Customer customer = InterceptedBizCustomerEngine.GetCustomer(agentSessionId, alloyId, mgiContext);

			Assert.IsNotNull(customer);
		}

		[Test]
		public void Can_Save_Customer()
		{
			long agentSessionId = 1000000001;
			long alloyId = 1000000000000000;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34 };
			MGI.Biz.Customer.Data.Customer customer = new Data.Customer() 
			{ 
				Profile = new CustomerProfile() 
				{
 					FirstName = "Nitish", 
					LastName = "Biradar", 
					Address1 = "Test", 
					City = "Derven", 
					DateOfBirth = new DateTime(1990, 05, 01), 
					Gender = "Male", 
					Phone1 ="1234567891", 
					Phone1Type = "Home", 
					ZipCode = "12345", 
					MothersMaidenName = "Test", 
					MailingAddressDifferent = false,
					PrimaryCountryCitizenship = "US",
					LegalCode = "12345",
					PIN = "1111"
				},
				Employment = new EmploymentDetails() 
				{
					Occupation = "Student"
				},
				ID = new Identification()
				{
					CountryOfBirth = "US",
					Country = "US",
					IDType = "DRIVER'S LICENSE",
					State = "Test",
					ExpirationDate = new DateTime(2020, 10, 10),
					IssueDate = new DateTime(2000, 10, 10),
					GovernmentId = "Passport"
				},
				Groups = new List<string>() { "Test1" } 
			};

			InterceptedBizCustomerEngine.SaveCustomer(agentSessionId, alloyId, customer, mgiContext);

			PTNRCustomerService.Verify(moq => moq.Update(It.IsAny<PTNRData.Customer>()), Times.AtLeastOnce());
		}

		[Test]
		[ExpectedException(typeof(BizCustomerException))]
		public void Can_Save_Customer_With_Referal_Code()
		{
			long agentSessionId = 1000000001;
			long alloyId = 1000000000000000;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34 };
			MGI.Biz.Customer.Data.Customer customer = new Data.Customer() { Profile = new CustomerProfile() { ReferralCode = "1000000000000000" } };

			InterceptedBizCustomerEngine.SaveCustomer(agentSessionId, alloyId, customer, mgiContext);
		}

		[Test]
		public void Can_Get_Customer_By_Phone()
		{
			long customerSessionId = 1000000000;
			string pin = "";
			string phone = "";
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34 };

			MGI.Biz.Customer.Data.Customer customer = InterceptedBizCustomerEngine.Get(customerSessionId, phone, pin, mgiContext);

			Assert.IsNotNull(customer);
		}

		[Test]
		public void Can_Get_Customer_By_CardNumber()
		{
			long agentSessionId = 1000000001;
			string cardNumber = "";
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF"};

			MGI.Biz.Customer.Data.Customer customer = InterceptedBizCustomerEngine.GetCustomerForCardNumber(agentSessionId, cardNumber, mgiContext);

			Assert.IsNotNull(customer);
		}

		[Test]
		public void Can_Validate_SSN()
		{
			long agentSessionId = 1000000001;
			string ssn = "888888888";
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF", AlloyId = 1000000000000000 };

			bool status = InterceptedBizCustomerEngine.ValidateSSN(agentSessionId, ssn, mgiContext);

			Assert.IsTrue(status);
		}

		[Test]
		public void Can_Validate_Customer_Last4DigitOFSSN()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF" };
			string last4DigitsOfSSN = "6789";

			bool status = InterceptedBizCustomerEngine.IsValidSSN(customerSessionId, last4DigitsOfSSN, mgiContext);

			Assert.IsTrue(status);
		}

		[Test]
		public void Can_Validate_Customer_Last4DigitOfSSN()
		{
			long customerSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF" };
			string last4DigitsOfSSN = "1234";

			bool status = InterceptedBizCustomerEngine.IsValidSSN(customerSessionId, last4DigitsOfSSN, mgiContext);

			Assert.IsFalse(status);
		}

		[Test]
		public void Can_LookUp_Customer()
		{
			long agentSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF" };
			Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>();

			List<Customer.Data.Customer> customers = InterceptedBizCustomerEngine.CustomerLookUp(agentSessionId, customerLookUpCriteria, mgiContext);

			Assert.AreNotEqual(customers.Count, 0);
		}

		[Test]
		public void Can_Get_AnonymousUser_PAN()
		{
			long agentSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF" };
			long channelPartnerId = 34;
			string firstName = "Nitish";
			string lastName = "Biradar";

			long pan = InterceptedBizCustomerEngine.GetAnonymousUserPAN(agentSessionId, channelPartnerId, firstName, lastName, mgiContext);

			Assert.AreNotEqual(pan, 0);
		}

		[Test]
		public void Can_Change_Profile_Status()
		{
			long agentSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF" };
			long userId = 500001;
			string profileStatus = "Active";

			bool status = InterceptedBizCustomerEngine.CanChangeProfileStatus(agentSessionId, userId, profileStatus, mgiContext);

			Assert.IsTrue(status);
		}

		[Test]
		public void Can_Validate_Customer_Status()
		{
			long agentSessionId = 1000000001;
			long alloyId = 1000000000000000;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF" };

			InterceptedBizCustomerEngine.ValidateCustomerStatus(agentSessionId, alloyId, mgiContext);

			CXECustomerService.Verify(moq => moq.ValidateStatus(It.IsAny<long>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Register_Customer_To_Client()
		{
			long agentSessionId = 1000000001;
			long alloyId = 1000000000000000;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF", Context = new Dictionary<string, object>(), LocationRowGuid = Guid.Parse("CB0AFFF8-9404-4C22-B282-F2160D901C93"), };

			InterceptedBizCustomerEngine.RegisterToClient(agentSessionId, alloyId, mgiContext);

			EventPublisher.Verify(moq => moq.Publish(It.IsAny<string>(), It.IsAny<NexxoBizEvent>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_Update_Customer_To_Client()
		{
			long agentSessionId = 1000000001;
			long alloyId = 1000000000000000;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF", Context = new Dictionary<string, object>(), LocationRowGuid = Guid.Parse("CB0AFFF8-9404-4C22-B282-F2160D901C93"), };

			InterceptedBizCustomerEngine.UpdateCustomerToClient(agentSessionId, alloyId, mgiContext);

			EventPublisher.Verify(moq => moq.Publish(It.IsAny<string>(), It.IsAny<NexxoBizEvent>()), Times.AtLeastOnce());
		}

		[Test]
		public void Can_CustomerSyncInFromClient()
		{
			long agentSessionId = 1000000001;
			long cxeCustomerId = 1000000000000000;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF", Context = new Dictionary<string, object>(), LocationRowGuid = Guid.Parse("CB0AFFF8-9404-4C22-B282-F2160D901C93"), };

			InterceptedBizCustomerEngine.CustomerSyncInFromClient(agentSessionId, cxeCustomerId, mgiContext);

			EventPublisher.Verify(moq => moq.Publish(It.IsAny<string>(), It.IsAny<NexxoBizEvent>()), Times.Exactly(1));
		}

		[Test]
		public void Can_Get_Client_Customer_Status()
		{
			long agentSessionId = 1000000001;
			long alloyId = 1000000000000000;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF", Context = new Dictionary<string, object>(), LocationRowGuid = Guid.Parse("CB0AFFF8-9404-4C22-B282-F2160D901C93"), };

			ProfileStatus status = InterceptedBizCustomerEngine.GetClientProfileStatus(agentSessionId, alloyId, mgiContext);

			Assert.AreEqual(status, ProfileStatus.Active);
		}

		[Test]
		public void Can_Validate_Customer()
		{
			long agentSessionId = 1000000001;
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 34, ChannelPartnerName = "TCF" };
			Data.Customer customer = new Data.Customer() 
			{ 
				Profile = new CustomerProfile() 
				{
 					FirstName = "Nitish", 
					LastName = "Biradar", 
					Address1 = "Test", 
					City = "Derven", 
					DateOfBirth = new DateTime(1990, 05, 01), 
					Gender = "Male", 
					Phone1 ="1234567891", 
					Phone1Type = "Home", 
					ZipCode = "12345", 
					MothersMaidenName = "Test", 
					MailingAddressDifferent = false,
					PrimaryCountryCitizenship = "US",
					LegalCode = "12345",
					PIN = "1111"
				},
				Employment = new EmploymentDetails() 
				{
					Occupation = "Student"
				},
				ID = new Identification()
				{
					CountryOfBirth = "US",
					Country = "US",
					IDType = "DRIVER'S LICENSE",
					State = "Test",
					ExpirationDate = new DateTime(2020, 10, 10),
					IssueDate = new DateTime(2000, 10, 10),
					GovernmentId = "Passport"
				}
			};

			bool status = InterceptedBizCustomerEngine.ValidateCustomer(agentSessionId, customer, mgiContext);

			Assert.IsTrue(status);
		}

		[Test]
		public void Can_Search_Customer_By_CardNumber()
		{
			long agentSessionId = 1000000001;
			
			//CXECustomer
			CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria() { CardNumber = "4756756000186663" };
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 33, ChannelPartnerName = "Synovus" };				
			List<CustomerSearchResult> customerSearchResult = InterceptedBizCustomerEngine.Search(agentSessionId, customerSearchCriteria, mgiContext);

			Assert.AreEqual(customerSearchResult.Count, 0);

			ProcessorRouter.Verify(moq => moq.AssociateCard(It.IsAny<MGI.Cxn.Fund.Data.CardAccount>(), It.IsAny<MGIContext>(), It.IsAny<bool>()), Times.AtLeastOnce());
		}


		[Test]
		[ExpectedException(typeof(MGI.Cxn.Fund.Contract.FundException))]
		public void Can_Search_Customer_By_CardNumber_Expection()
		{
			long agentSessionId = 1000000001;

			//CXECustomer
			CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria() { CardNumber = "4756756000186664" };
			MGIContext mgiContext = new MGIContext() { ChannelPartnerId = 33, ChannelPartnerName = "Synovus" };
			List<CustomerSearchResult> customerSearchResult = InterceptedBizCustomerEngine.Search(agentSessionId, customerSearchCriteria, mgiContext);

		}
    }
}
