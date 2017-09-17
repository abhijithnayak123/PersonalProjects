using System;
using System.Collections.Generic;
using System.Linq;
using PTNRContract = MGI.Core.Partner.Contract;
using PTNRData = MGI.Core.Partner.Data;
using Moq;
using CXEContract = MGI.Core.CXE.Contract;
using CXEData = MGI.Core.CXE.Data;
using MGI.Common.Util;
using MGI.Biz.Compliance.Contract;
using MGI.Biz.Events.Contract;
using MGI.Cxn.Customer.Contract;
using MGI.Cxn.Customer.Data;
using MGI.Biz.Common.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using MGI.Core.Partner.Contract;

namespace MGI.Unit.Test.MockClasses
{
	public class MockCommonClass : IntializMoqObject
	{
		public MockCommonClass() : base() { }

		#region Core Partner Customer Service
		public PTNRContract.ICustomerSessionService CreateCustomerSessionService()
		{
			Mock<PTNRContract.ICustomerSessionService> CustomerSessionService = _moqRepository.Create<PTNRContract.ICustomerSessionService>();

			CustomerSessionService.Setup(moq => moq.Create(It.IsAny<PTNRData.AgentSession>(), It.IsAny<PTNRData.Customer>(), It.IsAny<bool>(), It.IsAny<string>())).Returns(
				(PTNRData.AgentSession agentSession, PTNRData.Customer customer, bool cardPresent, string timeZone) =>
				{
					PTNRData.CustomerSession customerSession = new PTNRData.CustomerSession()
					{
						AgentSession = agentSession,
						Customer = customer,
						CardPresent = cardPresent,
						Id = 1000000000 + (customerSessions.Count() + 1),
						TimezoneID = timeZone,
						DTServerCreate = DateTime.Now,
						DTStart = DateTime.Now,
						DTTerminalCreate = DateTime.Now,
						rowguid = Guid.NewGuid(),
						ShoppingCarts = new List<ShoppingCart>()
						{
							new ShoppingCart()
							{
								IsParked = false, Active = true, 
								ShoppingCartTransactions = new List<ShoppingCartTransaction>()
								{
									new ShoppingCartTransaction(new ShoppingCartTransaction(){ Transaction = new BillPay(){CXEState = 1}})
								} 
							}
						}

					};
					customerSessions.Add(customerSession);
					return customerSession;
				});

			CustomerSessionService.Setup(moq => moq.GetParkingShoppingCart(It.IsAny<PTNRData.CustomerSession>())).Returns(
				(PTNRData.CustomerSession customerSession) =>
				{
					return shoppingCarts.Find(a => a.Customer == customerSession.Customer && a.IsParked == true);
				});

			CustomerSessionService.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(
				(long customerSessionId) =>
				{
					return customerSessions.Find(a => a.Id == customerSessionId);
				});

			CustomerSessionService.Setup(moq => moq.Save(It.IsAny<PTNRData.CustomerSession>())).Callback(
				(PTNRData.CustomerSession customerSession) =>
				{
					var existingSession = customerSessions.Find(a => a.Id == customerSession.Id);
					if (existingSession != null)
					{
						customerSessions.Remove(existingSession);
						existingSession = customerSession;
						customerSessions.Add(existingSession);
					}
				});

			return CustomerSessionService.Object;
		} 
		#endregion

		#region CXE Customer Service
		public CXEContract.ICustomerService CreateInstanceOfCXECustomer()
		{
			CXECustomerService = _moqRepository.Create<CXEContract.ICustomerService>();

			CXECustomerService.Setup(moq => moq.Get(It.IsAny<string>(), It.IsAny<string>())).Returns(
				(string phoneNumber, string pin) =>
				{
					var existingCustomer = coreCxeCustomers.Find(a => a.Phone1 == phoneNumber && a.ZipCode == pin);
					long id = 1000000000000000;
					if (existingCustomer != null)
					{
						id = existingCustomer.Id;
					}
					return id;
				});

			CXECustomerService.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(
				(long cxeId) =>
				{
					if(cxeId == 1000000000000107)
					{
						return null;
					}
					var existCustomer = coreCxeCustomers.Find(a => a.Id == cxeId);
					if (existCustomer == null)
					{
						existCustomer = coreCxeCustomers.FirstOrDefault();
					}
					return existCustomer;
				});

			CXECustomerService.Setup(moq => moq.Lookup(1000000000000125)).Returns(
				(long cxeId) =>
				{
					return null;
				});

			CXECustomerService.Setup(moq => moq.Lookup(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
				(long channelPartnerId, string firstName, string lastName) => 
				{
					return coreCxeCustomers.FirstOrDefault();
				});

			CXECustomerService.Setup(moq => moq.Lookup(It.IsAny<CXEData.CustomerSearchCriteria>())).Returns(
				(CXEData.CustomerSearchCriteria criteria) =>
				{
					return coreCxeCustomers.FindAll(
						c => (string.IsNullOrEmpty(criteria.FirstName) || c.FirstName == criteria.FirstName)
							&& (string.IsNullOrEmpty(criteria.LastName) || c.LastName == criteria.LastName)
							&& (criteria.DateOfBirth == null || c.DateOfBirth == criteria.DateOfBirth)
							&& (string.IsNullOrEmpty(criteria.PhoneNumber) || c.Phone1 == criteria.PhoneNumber)
							&& (string.IsNullOrEmpty(criteria.GovernmentId) || c.GovernmentId.Identification == criteria.GovernmentId)
							&& (string.IsNullOrEmpty(criteria.SSN) || c.SSN == criteria.SSN)
							&& (criteria.AlloyID <= 0 || c.Id == criteria.AlloyID));
				});

			CXECustomerService.Setup(moq => moq.Lookup(It.IsAny<string>())).Returns(
				(string referalCode) => 
				{
					return coreCxeCustomers.Find(x => x.Id == long.Parse(referalCode));
				});

			CXECustomerService.Setup(moq => moq.Register(It.IsAny<CXEData.Customer>())).Returns(
				(CXEData.Customer customer) =>
				{
					customer.Id = 1000000000000000 + (coreCxeCustomers.Count() + 1);
					customer.rowguid = Guid.NewGuid();
					coreCxeCustomers.Add(customer);
					return customer;
				});

			CXECustomerService.Setup(moq => moq.Save(It.IsAny<CXEData.Customer>())).Callback(
				(CXEData.Customer customer) =>
				{
					var existingCustomer = coreCxeCustomers.Find(a => a.Id == customer.Id);
					if (existingCustomer == null)
					{
						customer.Id = 1000000000000000 + (coreCxeCustomers.Count() + 1);
						customer.rowguid = Guid.NewGuid();
						coreCxeCustomers.Add(customer);
					}
				});

			return CXECustomerService.Object;
		} 
		#endregion

		#region CXE Account Service
		public CXEContract.IAccountService CreateInstanceOfAccountService()
		{
			Mock<CXEContract.IAccountService> CXEAccountService = _moqRepository.Create<CXEContract.IAccountService>();

			CXEAccountService.Setup(moq => moq.AddCustomerBillPayAccount(It.IsAny<CXEData.Customer>())).Returns(
				(CXEData.Customer customer) =>
				{
					CXEData.Account acct = new CXEData.Account()
					{
						Customer = customer,
						DTServerCreate = DateTime.Now,
						DTTerminalCreate = DateTime.Now,
						Id = 1000000000 + (cxeAccounts.Count() + 1),
						Type = (int)CXEData.AccountTypes.BillPay,
						rowguid = Guid.NewGuid()
					};
					cxeAccounts.Add(acct);
					return acct;
				});

			CXEAccountService.Setup(moq => moq.AddCustomerCashAccount(It.IsAny<CXEData.Customer>())).Returns(
				(CXEData.Customer customer) =>
				{
					CXEData.Account acct = new CXEData.Account()
					{
						Customer = customer,
						DTServerCreate = DateTime.Now,
						DTTerminalCreate = DateTime.Now,
						Id = 1000000000 + (cxeAccounts.Count() + 1),
						Type = (int)CXEData.AccountTypes.Cash,
						rowguid = Guid.NewGuid()
					};
					cxeAccounts.Add(acct);
					return acct;
				});

			CXEAccountService.Setup(moq => moq.AddCustomerCheckAccount(It.IsAny<CXEData.Customer>())).Returns(
				(CXEData.Customer customer) =>
				{
					CXEData.Account acct = new CXEData.Account()
					{
						Customer = customer,
						DTServerCreate = DateTime.Now,
						DTTerminalCreate = DateTime.Now,
						Id = 1000000000 + (cxeAccounts.Count() + 1),
						Type = (int)CXEData.AccountTypes.Check,
						rowguid = Guid.NewGuid()
					};
					cxeAccounts.Add(acct);
					return acct;
				});

			CXEAccountService.Setup(moq => moq.AddCustomerFundsAccount(It.IsAny<CXEData.Customer>())).Returns(
				(CXEData.Customer customer) =>
				{
					CXEData.Account acct = new CXEData.Account()
					{
						Customer = customer,
						DTServerCreate = DateTime.Now,
						DTTerminalCreate = DateTime.Now,
						Id = 1000000000 + (cxeAccounts.Count() + 1),
						Type = (int)CXEData.AccountTypes.Funds,
						rowguid = Guid.NewGuid()
					};
					cxeAccounts.Add(acct);
					return acct;
				});

			CXEAccountService.Setup(moq => moq.AddCustomerMoneyOrderAccount(It.IsAny<CXEData.Customer>())).Returns(
				(CXEData.Customer customer) =>
				{
					CXEData.Account acct = new CXEData.Account()
					{
						Customer = customer,
						DTServerCreate = DateTime.Now,
						DTTerminalCreate = DateTime.Now,
						Id = 1000000000 + (cxeAccounts.Count() + 1),
						Type = (int)CXEData.AccountTypes.MoneyOrder,
						rowguid = Guid.NewGuid()
					};
					cxeAccounts.Add(acct);
					return acct;
				});

			CXEAccountService.Setup(moq => moq.AddCustomerMoneyTransferAccount(It.IsAny<CXEData.Customer>())).Returns(
				(CXEData.Customer customer) =>
				{
					CXEData.Account acct = new CXEData.Account()
					{
						Customer = customer,
						DTServerCreate = DateTime.Now,
						DTTerminalCreate = DateTime.Now,
						Id = 1000000000 + (cxeAccounts.Count() + 1),
						Type = (int)CXEData.AccountTypes.MoneyTransfer,
						rowguid = Guid.NewGuid()
					};
					cxeAccounts.Add(acct);
					return acct;
				});

			return CXEAccountService.Object;
		} 
		#endregion

		#region Agent Session Service Core Partner
		public PTNRContract.IAgentSessionService CreateInstanceOfAgentSessionService()
		{
			AgentSessionService = _moqRepository.Create<PTNRContract.IAgentSessionService>();

			AgentSessionService.Setup(moq => moq.Create(It.IsAny<PTNRData.UserDetails>(), It.IsAny<PTNRData.Terminal>(), It.IsAny<MGIContext>())).Returns(
				(PTNRData.UserDetails userDetail, PTNRData.Terminal terminal, MGIContext mgiContext) =>
				{
					PTNRData.AgentSession agentSession = new PTNRData.AgentSession
					{
						rowguid = Guid.NewGuid(),
						DTServerCreate = DateTime.Now,
						Agent = userDetail,
						AgentId = userDetail.Id.ToString(),
						Terminal = terminal,
						BusinessDate = DateTime.Now,
						Id = 1000000000 + (agentSessions.Count() + 1)
					};
					agentSessions.Add(agentSession);
					return agentSession;
				});

			AgentSessionService.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(
				(long id) =>
				{
					return agentSessions.Find(a => a.Id == id);
				});

			AgentSessionService.Setup(moq => moq.Lookup(10000099)).Returns(
				(long id) =>
				{
					return null;
				});

			AgentSessionService.Setup(moq => moq.Update(It.IsAny<PTNRData.AgentSession>())).Returns(
				(PTNRData.AgentSession agentSession) =>
				{
					var existingAgentSession = agentSessions.Find(a => a.Id == agentSession.Id);
					if (existingAgentSession != null)
					{
						agentSessions.Remove(existingAgentSession);
						existingAgentSession.DTTerminalLastModified = DateTime.Now;
						existingAgentSession.DTServerLastModified = DateTime.Now;
						agentSessions.Add(agentSession);
					}
					return true;
				});

			return AgentSessionService.Object;
		} 
		#endregion

		#region Partner Customer Service
		public PTNRContract.ICustomerService CreateInstanceOfPTNRCustomer()
		{
			PTNRCustomerService = _moqRepository.Create<PTNRContract.ICustomerService>();

			PTNRCustomerService.Setup(moq => moq.ConfirmIdentity(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<bool>())).Returns(
				(long agentId, long customerSessionId, bool status) =>
				{
					PTNRData.IdentificationConfirmation identityConfirmed = new PTNRData.IdentificationConfirmation()
					{
						AgentID = agentId,
						CustomerSessionID = customerSessionId,
						ConfirmStatus = status,
						DateIdentified = DateTime.Now,
						DTServerCreate = DateTime.Now,
						rowguid = Guid.NewGuid(),
						Id = 1000000000 + (identityConfirms.Count() + 1)
					};
					identityConfirms.Add(identityConfirmed);
					return identityConfirmed.Id.ToString();
				});

			PTNRCustomerService.Setup(moq => moq.Create(It.IsAny<PTNRData.Customer>())).Returns(
				(PTNRData.Customer customer) =>
				{
					customer.Id = 1000000000000000 + (ptrnCustomers.Count() + 1);
					customer.CXEId = 1000000000000000 + (ptrnCustomers.Count() + 1);
					ptrnCustomers.Add(customer);
					return customer;
				});

			PTNRCustomerService.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(
				(long id) =>
				{
					return ptrnCustomers.Find(a => a.Id == id);
				});

			PTNRCustomerService.Setup(moq => moq.LookupByCxeId(It.IsAny<long>())).Returns(
				(long cxeId) =>
				{
					return ptrnCustomers.Find(a => a.CXEId == cxeId);
				});

			PTNRCustomerService.Setup(moq => moq.LookupByCXNAccountId(It.IsAny<long>(), It.IsAny<int>())).Returns(
				(long cxnAccountId, int providerId) =>
				{
					var ptnrAccount = ptnrAccounts.Find(a => a.CXNId == cxnAccountId && a.ProviderId == providerId);
					if (ptnrAccount != null)
					{
						return ptrnCustomers.Find(a => a.Id == ptnrAccount.Customer.Id);
					}
					return ptrnCustomers.FirstOrDefault();
				});

			PTNRCustomerService.Setup(moq => moq.LookupProspect(It.IsAny<long>())).Returns(
				(long alloyId) =>
				{
					var exitProspect = prospects.Find(a => a.AlloyID == alloyId);
					if (exitProspect == null)
					{
						exitProspect = prospects.FirstOrDefault();
					}
					return exitProspect;
				});

			PTNRCustomerService.Setup(moq => moq.SaveGroupSetting(It.IsAny<PTNRData.CustomerGroupSetting>()));

			PTNRCustomerService.Setup(moq => moq.SaveProspect(It.IsAny<PTNRData.Prospect>())).Callback(
				(PTNRData.Prospect prospect) =>
				{
					if (prospect.AlloyID == 0)
					{
						prospect.AlloyID = 1000000000000000 + (prospects.Count() + 1);
						prospects.Add(prospect);
					}
				});

			PTNRCustomerService.Setup(moq => moq.Update(It.IsAny<PTNRData.Customer>())).Callback(
				(PTNRData.Customer customer) =>
				{
					customer.DTServerLastModified = DateTime.Now;
					var existingCustomer = ptrnCustomers.Find(a => a.Id == customer.Id);
					if (existingCustomer != null)
					{
						ptrnCustomers.Remove(existingCustomer);
						existingCustomer = customer;
						ptrnCustomers.Add(existingCustomer);
					}
				});

			return PTNRCustomerService.Object;
		} 
		#endregion

		#region Core Partner IManageUsers
		public PTNRContract.IManageUsers CreateInstanceOfManageUsers()
		{
			ManageUserService = _moqRepository.Create<PTNRContract.IManageUsers>();

			ManageUserService.Setup(moq => moq.AddUser(It.IsAny<PTNRData.UserDetails>())).Returns(
				(PTNRData.UserDetails userDetail) =>
				{
					userDetail.Id = 500001 + (userDetails.Count() + 1);
					userDetails.Add(userDetail);
					return (int)userDetail.Id;
				});

			ManageUserService.Setup(moq => moq.GetUser(It.IsAny<int>())).Returns(
				(int userId) =>
				{
					return userDetails.Find(a => a.Id == userId);
				});

			ManageUserService.Setup(moq => moq.GetUsers(It.IsAny<long>())).Returns(
				(long locationId) =>
				{
					return userDetails.FindAll(a => a.LocationId == locationId);
				});

			ManageUserService.Setup(moq => moq.HasPermission(It.IsAny<int>(), It.IsAny<string>())).Returns(
				(int userId, string permission) =>
				{
					return true;
				});

			ManageUserService.Setup(moq => moq.UpdateUser(It.IsAny<PTNRData.UserDetails>())).Returns(
				(PTNRData.UserDetails userDetail) =>
				{
					var exitUserDetail = userDetails.Find(a => a.Id == userDetail.Id);
					if (exitUserDetail != null)
					{
						userDetails.Remove(exitUserDetail);
						exitUserDetail = userDetail;
						userDetails.Add(exitUserDetail);
					}
					return (int)exitUserDetail.Id;
				});

			ManageUserService.Setup(moq => moq.GetUser(It.IsAny<string>(), It.IsAny<long>())).Returns(
				(string userName, long channelPartnerId) => 
				{
					if (channelPartnerId == 36)
					{
						throw new PTNRData.PartnerAgentException(PTNRData.PartnerAgentException.USER_NOT_FOUND, null);
					}
					if (userName == "Nitish")
					{
						return null;
					}
					return userDetails.FirstOrDefault();
				});

			return ManageUserService.Object;
		} 
		#endregion

		#region Core Partner IChannelPartnerService Service
		public PTNRContract.IChannelPartnerService CreateInstanceOfChannelPartnerService()
		{
			Mock<PTNRContract.IChannelPartnerService> CorePartnerChannelPartnerService = _moqRepository.Create<PTNRContract.IChannelPartnerService>();

			CorePartnerChannelPartnerService.Setup(moq => moq.ChannelPartnerConfig(It.IsAny<Guid>())).Returns(
				(Guid rowguid) =>
				{
					return channelPartners.Find(a => a.rowguid == rowguid);
				});

			CorePartnerChannelPartnerService.Setup(moq => moq.ChannelPartnerConfig(It.IsAny<long>())).Returns(
				(long channelPartnerId) =>
				{
					return channelPartners.Find(a => a.Id == channelPartnerId);
				});

			CorePartnerChannelPartnerService.Setup(moq => moq.ChannelPartnerConfig(It.IsAny<string>())).Returns(
				(string channelPartnerName) =>
				{
					return channelPartners.Find(a => a.Name == channelPartnerName);
				});

			CorePartnerChannelPartnerService.Setup(moq => moq.DBGetChannelPartnerProductProcessors(It.IsAny<long>(), It.IsAny<int>())).Returns(new List<string>() { "WU", "MGI", "INGO", "VISA" });

			CorePartnerChannelPartnerService.Setup(moq => moq.GetChannelPartnerCertificateInfo(It.IsAny<long>(), It.IsAny<string>())).Returns(new PTNRData.ChannelPartnerCertificate());

			CorePartnerChannelPartnerService.Setup(moq => moq.GetCheckProcessor(It.IsAny<long>())).Returns("INGO");

			CorePartnerChannelPartnerService.Setup(moq => moq.GetCheckType(It.IsAny<string>())).Returns((string name) => { return checkTypes.Find(a => a.Name == name); });

			CorePartnerChannelPartnerService.Setup(moq => moq.GetCheckType(It.IsAny<int>())).Returns((int id) => { return checkTypes.FirstOrDefault(); });

			CorePartnerChannelPartnerService.Setup(moq => moq.GetCheckTypes()).Returns(new List<string>() { "Printed Pay Role", "Testing", "" });

			CorePartnerChannelPartnerService.Setup(moq => moq.GetTipsAndOffers(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(tipsAndOffers);

			CorePartnerChannelPartnerService.Setup(moq => moq.Locations(It.IsAny<string>())).Returns(new List<string>() { "Testing" });

			return CorePartnerChannelPartnerService.Object;
		} 
		#endregion

		#region Core Partner NexxoDataStructureService 
		public PTNRContract.INexxoDataStructuresService CreateInstanceOfNexxoIdType()
		{
			Mock<PTNRContract.INexxoDataStructuresService> PTNRDataStructureService = _moqRepository.Create<PTNRContract.INexxoDataStructuresService>();

			PTNRDataStructureService.Setup(moq => moq.Countries()).Returns(new List<string>() { "United State", "India", "China" });

			PTNRDataStructureService.Setup(moq => moq.Find(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
				(long channelPartnerId, string name, string country, string state) =>
				{
					return nexxoIdType.FirstOrDefault();
				});

			PTNRDataStructureService.Setup(moq => moq.Find(It.IsAny<long>(), It.IsAny<long>())).Returns(
				(long channelPartnerId, long idType) =>
				{
					return nexxoIdType.FirstOrDefault();
				});

			PTNRDataStructureService.Setup(moq => moq.GetCountry(It.IsAny<string>())).Returns(
				(string countryAbbr) =>
				{
					return masterCountrys.FirstOrDefault().Name;
				});

			PTNRDataStructureService.Setup(moq => moq.States(It.IsAny<string>())).Returns(
				(string countryCode) =>
				{
					return states;
				});

			PTNRDataStructureService.Setup(moq => moq.IdCountries(It.IsAny<long>())).Returns(
				(long channelPartnerId) =>
				{
					return idCountries;
				});

			PTNRDataStructureService.Setup(moq => moq.IdStates(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>())).Returns(
				(long channelPartnerId, string country, string idType) =>
				{
					return states;
				});


			PTNRDataStructureService.Setup(moq => moq.MasterCountries(It.IsAny<long>())).Returns(
				(long channelPartnerId) =>
				{
					return masterCountrys;
				});

			PTNRDataStructureService.Setup(moq => moq.GetMasterCountryByCode(It.IsAny<string>())).Returns(
				(string abbr) =>
				{
					return masterCountrys.FirstOrDefault();
				});


			PTNRDataStructureService.Setup(moq => moq.PhoneTypes()).Returns(phoneTypes);

			PTNRDataStructureService.Setup(moq => moq.MobileProviders()).Returns(mobileProviders);

			PTNRDataStructureService.Setup(moq => moq.GetLegalCodes()).Returns(legalCodes);

			PTNRDataStructureService.Setup(moq => moq.GetOccupations()).Returns(occupations);
				

			PTNRDataStructureService.Setup(moq => moq.IdTypes(It.IsAny<long>(), It.IsAny<string>())).Returns(
				(long channelPartnerId, string idType) =>
				{
					return idTypes;
				});

			PTNRDataStructureService.Setup(moq => moq.USStates()).Returns(states);
			

			PTNRDataStructureService.Setup(moq => moq.GetIDState(It.IsAny<string>(), It.IsAny<string>())).Returns(
				(string country, string stateAbbr) =>
				{
					return "United States";
				});

			PTNRDataStructureService.Setup(moq => moq.GetMasterCountryByCode(It.IsAny<string>())).Returns(
				(string countryAbbr2) =>
				{
					return masterCountrys.Find(a => a.Abbr2 == countryAbbr2);
				});

			PTNRDataStructureService.Setup(moq => moq.GetOccupations()).Returns(occupations);

			return PTNRDataStructureService.Object;
		} 
		#endregion

		#region Core Message Center Imp
		public PTNRContract.IMessageCenter CreateInstanceOfMessageCenter()
		{
			Mock<PTNRContract.IMessageCenter> MessageCenterService = _moqRepository.Create<PTNRContract.IMessageCenter>();

			MessageCenterService.Setup(moq => moq.Create(It.IsAny<PTNRData.AgentMessage>(), It.IsAny<string>())).Returns(
				(PTNRData.AgentMessage agentMessage, string timeZone) =>
				{
					agentMessage.Id = 100000000 + (agentMessages.Count() + 1);
					agentMessages.Add(agentMessage);
					return true;
				});

			MessageCenterService.Setup(moq => moq.DeleteAllMessages()).Returns(true);

			MessageCenterService.Setup(moq => moq.GetByAgentID(It.IsAny<long>())).Returns(agentMessages);

			MessageCenterService.Setup(moq => moq.Lookup(It.IsAny<PTNRData.Transactions.Transaction>())).Returns(agentMessages.FirstOrDefault());

			MessageCenterService.Setup(moq => moq.Delete(It.IsAny<PTNRData.Transactions.Transaction>())).Returns(true);

			MessageCenterService.Setup(moq => moq.Update(It.IsAny<PTNRData.AgentMessage>())).Returns(true);

			MessageCenterService.Setup(moq => moq.UpdateStatus(It.IsAny<PTNRData.AgentMessage>(), It.IsAny<string>())).Returns(true);

			return MessageCenterService.Object;
		} 
		#endregion

		#region Core Partner Customer Fee Adjustment Service
		public PTNRContract.ICustomerFeeAdjustmentService CreateInstanceOfCustomerFeeAdjustmentService()
		{
			var obj = _moqRepository.Create<PTNRContract.ICustomerFeeAdjustmentService>();

			obj.Setup(moq => moq.lookup(It.IsAny<long>(), It.IsAny<PTNRData.Fees.FeeAdjustment>())).Returns(
				(long customerId, PTNRData.Fees.FeeAdjustment feeAdjustment) =>
				{
					return customerFeeAdjustments.Find(a => a.Id == customerId && a.IsAvailed == false);
				});

			obj.Setup(moq => moq.Create(It.IsAny<PTNRData.Fees.CustomerFeeAdjustments>())).Returns(
				(PTNRData.Fees.CustomerFeeAdjustments customerFeeAdjustment) =>
				{
					customerFeeAdjustment.Id = 100000002;
					customerFeeAdjustment.rowguid = Guid.NewGuid();
					customerFeeAdjustments.Add(customerFeeAdjustment);
					return true;
				});

			obj.Setup(moq => moq.Update(It.IsAny<PTNRData.Fees.CustomerFeeAdjustments>())).Returns(
				(PTNRData.Fees.CustomerFeeAdjustments customerFeeAdjustment) =>
				{
					var existing = customerFeeAdjustments.Find(a => a.Id == customerFeeAdjustment.Id);
					if (existing != null)
					{
						customerFeeAdjustments.Remove(existing);
						existing = customerFeeAdjustment;
						customerFeeAdjustments.Add(existing);
					}
					return true;
				});

			return obj.Object;
		} 
		#endregion

		#region Fee Adjustment Service Fake Object
		public PTNRContract.IFeeAdjustmentService CreateInstanceOfFeeAdjustmentService()
		{
			var obj = _moqRepository.Create<PTNRContract.IFeeAdjustmentService>();

			obj.Setup(moq => moq.Lookup(It.IsAny<PTNRData.ChannelPartner>())).Returns(feeAdjustments);

			obj.Setup(moq => moq.DeleteFeeAdjustments(It.IsAny<Guid>()));

			obj.Setup(moq => moq.GetApplicableAdjustments(It.IsAny<PTNRData.Fees.FeeAdjustmentTransactionType>(), It.IsAny<PTNRData.CustomerSession>(), It.IsAny<List<PTNRData.Transactions.Transaction>>(), It.IsAny<MGIContext>())).Returns(feeAdjustments);

			return obj.Object;
		} 
		#endregion

		#region Limit Service Fake Object
		public ILimitService CreateInstanceOfLimitService()
		{
			var obj = _moqRepository.Create<ILimitService>();

			obj.Setup(moq => moq.GetProductMinimum(It.IsAny<string>(), It.IsAny<Biz.Compliance.Data.TransactionTypes>(), It.IsAny<MGIContext>())).Returns(
				(string complianceProgram, Biz.Compliance.Data.TransactionTypes type, MGIContext mgiContext) =>
				{
					return long.MinValue;
				});

			obj.Setup(moq => moq.CalculateTransactionMaximumLimit(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<Biz.Compliance.Data.TransactionTypes>(), It.IsAny<MGIContext>())).Returns(
				(long customerSessionId, string complianceProgram, Biz.Compliance.Data.TransactionTypes type, MGIContext mgiContext) =>
				{
					return long.MaxValue;
				});
			return obj.Object;
		} 
		#endregion

		#region Nexxo Biz Event Publisher Fake Object
		public INexxoBizEventPublisher CreateInstanceOfNexxoBizEventPublisher()
		{
			EventPublisher = _moqRepository.Create<INexxoBizEventPublisher>();

			EventPublisher.Setup(moq => moq.Publish(It.IsAny<string>(), It.IsAny<NexxoBizEvent>()));

			return EventPublisher.Object;
		} 
		#endregion

		#region Location Service For BillPay and MoneyTransfer
		public PTNRContract.ILocationCounterIdService LocationServiceInstance()
		{
			Mock<PTNRContract.ILocationCounterIdService> LocationCounterIdService = _moqRepository.Create<PTNRContract.ILocationCounterIdService>();

			LocationCounterIdService.Setup(m => m.Get(It.IsAny<Guid>(), It.IsAny<int>())).Returns("13139925");

			LocationCounterIdService.Setup(m => m.Get(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>())).Returns(new PTNRData.LocationCounterId());

			LocationCounterIdService.Setup(m => m.Get(It.IsAny<Guid>(), It.IsAny<string>())).Returns(new PTNRData.LocationCounterId());

			LocationCounterIdService.Setup(m => m.Update(It.IsAny<PTNRData.LocationCounterId>())).Returns(true);

			return LocationCounterIdService.Object;
		} 
		#endregion

		#region Customer Session Counter Id Service
		public PTNRContract.ICustomerSessionCounterIdService CustomerSessionCounterIdServiceInstance()
		{
			Mock<PTNRContract.ICustomerSessionCounterIdService> CustomerSessionCounterIdService = _moqRepository.Create<PTNRContract.ICustomerSessionCounterIdService>();

			return CustomerSessionCounterIdService.Object;
		} 
		#endregion

		#region Core Partner Manage Location Service
		public PTNRContract.IManageLocations CreateInstanceOfManageLocations()
		{
			Mock<PTNRContract.IManageLocations> CorePartnerLocationService = _moqRepository.Create<PTNRContract.IManageLocations>();

			CorePartnerLocationService.Setup(moq => moq.Create(It.IsAny<PTNRData.Location>())).Returns(
				(PTNRData.Location location) => 
				{
					location.rowguid = Guid.NewGuid();
					location.Id = 1000000003 + (locations.Count() + 1);
					locations.Add(location);
					return location.Id;
				});

			CorePartnerLocationService.Setup(moq => moq.GetAll()).Returns(locations);

			CorePartnerLocationService.Setup(moq => moq.GetByName(It.IsAny<string>())).Returns(
				(string locationName) => 
				{
					var location = locations.Find(a => a.LocationName == locationName);
					if (location == null)
					{
						location = locations.FirstOrDefault();
					}
					return location;
				});

			CorePartnerLocationService.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(
				(long locationId) => 
				{
					var location = locations.Find(a => a.Id == locationId);
					if (location == null)
					{
						location = locations.FirstOrDefault();
					}
					return location;
				});

			CorePartnerLocationService.Setup(moq=>moq.Update(It.IsAny<PTNRData.Location>())).Returns(
				(PTNRData.Location location) =>
				{
					var exitingLocation = locations.Find(a => a.Id == location.Id);
					if (exitingLocation != null)
					{
						locations.Remove(exitingLocation);
						exitingLocation = location;
						locations.Add(exitingLocation);
					}
					return true;
				});

			return CorePartnerLocationService.Object;
		} 
		#endregion

		#region Core Partner Terminal Service
		public PTNRContract.ITerminalService CreateInstanceOfTerminalService()
		{
			Mock<PTNRContract.ITerminalService> TerminalService = _moqRepository.Create<PTNRContract.ITerminalService>();

			TerminalService.Setup(moq=>moq.Create(It.IsAny<PTNRData.Terminal>())).Returns(
				(PTNRData.Terminal terminal) =>
				{
					terminal.rowguid = Guid.NewGuid();
					terminal.Id = 1003 + (terminals.Count() + 1);
					terminals.Add(terminal);
					return terminal.rowguid;
				});

			TerminalService.Setup(moq => moq.Lookup(It.IsAny<System.Guid>())).Returns(
				(System.Guid pk) => 
				{
					var terminal = terminals.Find(a => a.rowguid == pk);
					if (terminal == null)
					{
						terminal = terminals.FirstOrDefault();
					}
					return terminal;
				});

			TerminalService.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(
				(long id) => 
				{
					var terminal = terminals.Find(a => a.Id == id);
					if (terminal == null)
					{
						terminal = terminals.FirstOrDefault();
					}
					return terminal;
				});

			TerminalService.Setup(moq => moq.Lookup(It.IsAny<string>(), It.IsAny<PTNRData.ChannelPartner>(), It.IsAny<MGIContext>())).Returns(
				(string terminalName, PTNRData.ChannelPartner channelPartner, MGIContext mgiContext) => 
				{
					return terminals.FirstOrDefault();
				});

			TerminalService.Setup(moq => moq.Lookup("Testing", It.IsAny<PTNRData.ChannelPartner>(), It.IsAny<MGIContext>())).Returns(
			(string terminalName, PTNRData.ChannelPartner channelPartner, MGIContext mgiContext) =>
			{
				return terminals.Find(t => t.Id == 1004);
			});

			TerminalService.Setup(moq=>moq.Update(It.IsAny<PTNRData.Terminal>())).Returns(
				(PTNRData.Terminal terminal) =>
				{
					var existingTerminal = terminals.Find(a => a.Id == terminal.Id);
					if (existingTerminal != null)
					{
						terminals.Remove(existingTerminal);
						existingTerminal = terminal;
						terminals.Add(existingTerminal);
					}
					return true;
				});

			return TerminalService.Object;
		} 
		#endregion

		#region Core Partner ChannelPartner Group Service
		public PTNRContract.IChannelPartnerGroupService CreateInstanceOfChannelPartnerGroupService()
		{
			Mock<PTNRContract.IChannelPartnerGroupService> PartnerGroupService = _moqRepository.Create<PTNRContract.IChannelPartnerGroupService>();

			PartnerGroupService.Setup(moq => moq.Create(It.IsAny<PTNRData.ChannelPartnerGroup>()));

			PartnerGroupService.Setup(moq => moq.Get(It.IsAny<int>())).Returns(new PTNRData.ChannelPartnerGroup() { Id = 1000000000, Name = "Test" });

			PartnerGroupService.Setup(moq => moq.GetAll(It.IsAny<System.Guid>())).Returns(
				new List<PTNRData.ChannelPartnerGroup>() 
				{
					new PTNRData.ChannelPartnerGroup(){ Id = 1000000001, Name = "Test1"},
					new PTNRData.ChannelPartnerGroup(){ Id = 1000000002, Name = "Test2"},
					new PTNRData.ChannelPartnerGroup(){ Id = 1000000003, Name = "Test1"},
				});

			PartnerGroupService.Setup(moq => moq.Update(It.IsAny<PTNRData.ChannelPartnerGroup>()));

			PartnerGroupService.Setup(moq=>moq.GetAll(It.IsAny<string>())).Returns(
				new List<PTNRData.ChannelPartnerGroup>() 
				{
					new PTNRData.ChannelPartnerGroup(){ Id = 1000000001, Name = "Test1"},
					new PTNRData.ChannelPartnerGroup(){ Id = 1000000002, Name = "Test2"},
					new PTNRData.ChannelPartnerGroup(){ Id = 1000000003, Name = "Test1"},
				});

			return PartnerGroupService.Object;
		} 
		#endregion

		#region CXE Customer Service
		public IClientCustomerService CreateInstanceOfClientCustomerService()
		{
			CxnClientCustomerService = _moqRepository.Create<IClientCustomerService>();

			CxnClientCustomerService.Setup(moq => moq.Add(It.IsAny<CustomerProfile>(), It.IsAny<MGIContext>())).Returns(
				(CustomerProfile profile, MGIContext mgiContext) => 
				{
					if (mgiContext.Context != null && mgiContext.Context.ContainsKey("ProviderError"))
					{
						throw new CustomerException("1001");
					}
					if (mgiContext.Context != null && mgiContext.Context.ContainsKey("FISError"))
					{
						throw new Exception();
					}
					return long.MaxValue;
				});

			CxnClientCustomerService.Setup(moq => moq.FetchAll(It.IsAny<Dictionary<string, object>>(), It.IsAny<MGIContext>())).Returns(
				(Dictionary<string, object> customerLookUpCriteria, MGIContext mgiContext) =>
				{
					if (customerLookUpCriteria.ContainsKey("BrandNewCustomer"))
					{
						return null;
					}
					return new List<CustomerProfile>()
					{
						new CustomerProfile()
						{ 
							CustInd = true, ClientID = "1111111111111111" 
						} 
					};
				});

			CxnClientCustomerService.Setup(moq => moq.GetClientProfileStatus(It.IsAny<long>(), It.IsAny<MGIContext>())).Returns(
				(long id, MGIContext mgiContext) => 
				{
					if (mgiContext.Context != null && mgiContext.Context.ContainsKey("FISRegistrationFailed"))
					{
						return ProfileStatus.Inactive;
					}
					return ProfileStatus.Active;
 
				});

			CxnClientCustomerService.Setup(moq => moq.Update(It.IsAny<string>(), It.IsAny<CustomerProfile>(), It.IsAny<MGIContext>())).Callback(
				(string id, CustomerProfile customerFeeAdjustments, MGIContext mgiContext) => 
				{
					if (mgiContext.Context != null && mgiContext.Context.ContainsKey("ProviderError"))
					{
						throw new CustomerException("1001", null);
					}
				});

			CxnClientCustomerService.Setup(moq => moq.AddAccount(It.IsAny<CustomerProfile>(), It.IsAny<MGIContext>())).Returns(1000000000);

			CxnClientCustomerService.Setup(moq => moq.AddCXNAccount(It.IsAny<CustomerProfile>(), It.IsAny<MGIContext>())).Returns(1000000000);

			CxnClientCustomerService.Setup(moq => moq.GetClientCustID(It.IsAny<long>(), It.IsAny<MGIContext>())).Returns(
				(long id, MGIContext mgiContext) =>
				{
					if (mgiContext.ChannelPartnerName == "Synovus")
					{
						return "1111111111111111";
					}
					if (id == 1000000000)
					{
						return string.Empty;
					}
					return "1111111111111111";
				});

			CxnClientCustomerService.Setup(moq => moq.Fetch(It.IsAny<MGIContext>())).Returns(
				new CustomerProfile() 
				{ 
					FirstName ="Nitish", 
					LastName = "Biradar",
 					LastName2 = "Test",
					Address1 = "Testing",
					Address2 = "Testing",
					IDIssuingCountry = "USA",
					IDIssuingState = "CA",
					LegalCode = "12345",
					ZipCode = "12345",
					State = "CA",
					City = "Testing",
					CountryOfBirth = "United states",
					DateOfBirth = new DateTime(1990, 10, 10),
					Email = "testing@moneygram.com",
					Gender = "Male",
					GovernmentId = "Passport",
					GovernmentIDType = "Passport",
					MothersMaidenName = "Testing",
					Occupation = "Student",
					SSN = "123456789",
					Phone1 = "194875912",
					Phone1Provider = "test",
					Phone1Type = "Home",
					Phone2 = "194875912",
					Phone2Provider = "test",
					Phone2Type = "Home",
					OccupationDescription = "Testing",
					EmployerName = "Testing",
					EmployerPhone = "159487263",
				});

			return CxnClientCustomerService.Object;
		} 
		#endregion

		#region Print Template Instance
		public IPrintTemplate CreateInstanceOfPrintTemplate()
		{
			Mock<IPrintTemplate> PrintTemplateRepo = _moqRepository.Create<IPrintTemplate>();

			PrintTemplateRepo.Setup(moq => moq.GetPrintTemplate(It.IsAny<string>())).Returns("Test");

			return PrintTemplateRepo.Object;
		} 
		#endregion

		#region Core Message Store
		public IMessageStore CreateInstanceOfMessageStore()
		{
			Mock<IMessageStore> messageStore = _moqRepository.Create<IMessageStore>();
			messageStore.Setup(moq => moq.Lookup(1, "1006.100.8000", Language.EN)).Returns(new Message()
			{ 
			Processor = "MGiAlloy",
			MessageKey = "1006.100.8000",
			Content = "Error occured while doing transaction",
			AddlDetails = "Please contact technical support team for assistance"});
			return messageStore.Object;
		}
		#endregion
	}
}
