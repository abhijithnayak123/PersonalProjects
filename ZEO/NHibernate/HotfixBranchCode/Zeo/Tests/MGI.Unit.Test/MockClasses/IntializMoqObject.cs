using MGI.Common.Util;
using CXNBillpayData = MGI.Cxn.BillPay.Data;
using MGI.Cxn.Check.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using CXEData = MGI.Core.CXE.Data;
using PTNRData = MGI.Core.Partner.Data;
using MGI.Core.Catalog.Data;
using CXNFundData = MGI.Cxn.Fund.Data;
using MoneyTransferCommit = MGI.Core.CXE.Data.Transactions.Commit.MoneyTransfer;
using MoneyTransferStage = MGI.Core.CXE.Data.Transactions.Stage.MoneyTransfer;
using MGI.Cxn.MoneyTransfer.Data;
using CXNAccount = MGI.Cxn.MoneyTransfer.Data.Account;
using MGI.Cxn.Customer.FIS.Data;
using MGI.Cxn.Customer.CCIS.Data;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;

using CoreTrans = MGI.Core.Partner.Data.Transactions;

using System.Collections.ObjectModel;


namespace MGI.Unit.Test.MockClasses
{
	public class IntializMoqObject : BaseClass_Fixture
	{
		#region ChannelPartnerDetails
		internal static List<PTNRData.ChannelPartner> channelPartners = new List<PTNRData.ChannelPartner>() {
			new PTNRData.ChannelPartner()
			{ 
				rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"), 
				Id = 33, 
				Name = "Synovus", 
				FeesFollowCustomer = false, 
				CashFeeDescriptionEN = string.Empty, 
				CashFeeDescriptionES = string.Empty, 
				DebitFeeDescriptionEN = string.Empty, 
				DebitFeeDescriptionES = string.Empty, 
				ConvenienceFeeCash = 0, 
				ConvenienceFeeDebit = 0, 
				ConvenienceFeeDescriptionEN = string.Empty, 
				ConvenienceFeeDescriptionES = string.Empty, 
				CanCashCheckWOGovtId = false, 
				LogoFileName = "synovusLogo.png", 
				IsEFSPartner = false, 
				EFSClientId = 3, 
				UsePINForNonGPR = true, 
				IsCUPartner = true, 
				HasNonGPRCard = true, 
				ManagesCash = false, 
				AllowPhoneNumberAuthentication = false, 
				ComplianceProgramName = "SynovusTest",
				ChannelPartnerConfig = new PTNRData.ChannelPartnerConfig(){ MasterSSN = "9999999", IsReferralSectionEnable = true}
			},
			new PTNRData.ChannelPartner()
			{
				rowguid = Guid.Parse("6D7E785F-7BDD-42C8-BC49-44536A1885FC"),
				Id = 34, 
				Name = "TCF", 
				FeesFollowCustomer = false,
 				CashFeeDescriptionEN = string.Empty, 
				CashFeeDescriptionES = string.Empty, 
				DebitFeeDescriptionEN = string.Empty, 
				DebitFeeDescriptionES = string.Empty, 
				ConvenienceFeeCash = 0, 
				ConvenienceFeeDebit = 0, 
				ConvenienceFeeDescriptionEN = string.Empty, 
				ConvenienceFeeDescriptionES = string.Empty, 
				CanCashCheckWOGovtId = false, 
				LogoFileName = "TCF.png",
				IsEFSPartner = false, 
				EFSClientId = 3, 
				UsePINForNonGPR = true,
				IsCUPartner = true, 
				HasNonGPRCard = true, 
				ManagesCash = false, 
				TIM = 3,
				AllowPhoneNumberAuthentication = false, 
				ComplianceProgramName = "TCFCompliance", 
				CardPresenceVerificationConfig = 0,
				ChannelPartnerConfig = new PTNRData.ChannelPartnerConfig(){ MasterSSN = "888888888"},
				Providers=new List<ChannelPartnerProductProvider>()
				{
				    new ChannelPartnerProductProvider
					{
					   ProductProcessor =new ProductProcessor()
					   {
					     Code = (long)ProviderIds.Visa
					   },
					   CardExpiryPeriod = 48
					}				
				}
			},
			new PTNRData.ChannelPartner(){
				Id = 28,
				Name = "Carver",
				rowguid = Guid.Parse("578AC8FB-F69C-4DBD-A502-57B1EECD41D6"),
				ComplianceProgramName = "CarverCompliance",
				ChannelPartnerConfig = new PTNRData.ChannelPartnerConfig(){ MasterSSN = "888888888", CustomerMinimumAge = 24}
			},
			new PTNRData.ChannelPartner(){
				Id = 1,
				Name = "MGI",
				rowguid = Guid.Parse("10F2865B-DBC5-4A0B-983C-62E0A0574354"),
				ComplianceProgramName = "MGICompliance"
			},
			new PTNRData.ChannelPartner(){
				Id = 35,
				Name = "Redstone",
				rowguid = Guid.Parse("F8365E3B-FDD5-439A-BE9E-9B4444E17ED6"),
				ComplianceProgramName = "RedstoneCompliance"
			}
		};
		#endregion

		#region Location and Terminals Dateails
		internal static List<Core.Partner.Data.Location> locations = new List<Core.Partner.Data.Location>() {
			new Core.Partner.Data.Location(){ 
				Address1 = "test", 
				Address2 = "test",
				City = "test",
				IsActive = true,
				State = "CA",
				ZipCode = "12345",
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				LocationName = "Synovus",
				rowguid = Guid.Parse("BC46F466-16D3-47B9-97CC-A9F95E2A2CCB"),
				ChannelPartnerId = 33,
				Id = 1000000001
			},
			new Core.Partner.Data.Location(){
				Address1 = "801 Marquette Avenue",
				Address2 = string.Empty,
				City = "Minneapolis",
				ChannelPartnerId = 34,
				ZipCode = "55402",
				IsActive = true,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				rowguid  = Guid.Parse("CB0AFFF8-9404-4C22-B282-F2160D901C93"),
				LocationName = "TCF Service Desk",
				State = "MN",
				PhoneNumber = "6126616600",
				Id = 1000000003
			}
		};

		internal static List<PTNRData.Terminal> terminals = new List<PTNRData.Terminal>() {
			new PTNRData.Terminal(){ 
				Name = "TCF", 
				MacAddress = "10.111.109.16", 
				ChannelPartner = channelPartners.Find(a=>a.Name == "TCF"), 
				DTServerCreate = DateTime.Now, 
				DTTerminalCreate = DateTime.Now, 
				Location = locations.Find(a=>a.ChannelPartnerId == 34), 
				Id = 1003 },
			new PTNRData.Terminal(){
				Name = "Synovus",
				MacAddress = "10.111.109.16",
				ChannelPartner = channelPartners.Find(a=>a.Name == "Synovus"),
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Location = locations.Find(a=>a.ChannelPartnerId == 33),
				Id = 1001},
			new PTNRData.Terminal(){
				Name = "TCF",
				MacAddress = "10.111.109.16",
				ChannelPartner = channelPartners.Find(a=>a.Name == "TCF"),
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				PeripheralServer = new PTNRData.NpsTerminal() { Location = new PTNRData.Location()} ,
				Id = 1004}
		};
		#endregion

		#region User and Agents Dateails
		internal static List<PTNRData.UserDetails> userDetails = new List<PTNRData.UserDetails>() {
			new PTNRData.UserDetails(){
 				Rowguid = Guid.Parse("BCD46D04-9453-4AE8-BBA9-4A2C6172F301"),
				ChannelPartnerId = 34, 
				UserName = "ZeoMGI", 
				LastName = "System Admin", 
				FirstName = "Alloy", 
				FullName = "Alloy System Admin",
				IsEnabled = true,
				ManagerId = null,
				UserRoleId = 4,
				UserStatusId = 1,
				ClientAgentIdentifier = "98001",
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 500001,
				LocationId = 1000000000
			},
			new PTNRData.UserDetails(){
				Rowguid = Guid.Parse("7FC77C4B-1814-42BF-B961-E7FA3C80497D"),
				ChannelPartnerId = 33,
				UserName = "SysAdmin",
				LastName = "Administrator",
				FirstName = "System",
				FullName = "System Administrator",
				IsEnabled = true,
				ManagerId = 200000,
				UserRoleId = 4,
				UserStatusId = 1,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 200000,
				LocationId = 1000000000
			}
		};

		internal static List<PTNRData.AgentSession> agentSessions = new List<PTNRData.AgentSession>() {
			new PTNRData.AgentSession(){ 
				AgentId = "500001",
				Agent = userDetails.Find(a=>a.Rowguid == Guid.Parse("BCD46D04-9453-4AE8-BBA9-4A2C6172F301")),
				BusinessDate = DateTime.Now,
				rowguid = Guid.Parse("45BA3151-5A89-4424-BF73-4629CE1C14DC"),
				Terminal = terminals.Find(a=>a.Name == "TCF"),
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000
			},
			new PTNRData.AgentSession(){
				AgentId = "200000",
				Agent = userDetails.Find(a=>a.Rowguid == Guid.Parse("7FC77C4B-1814-42BF-B961-E7FA3C80497D")),
				BusinessDate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				DTServerCreate = DateTime.Now,
				Terminal = terminals.Find(a=>a.Name == "Synovus"),
				Id = 1000000001
			}
		};
		#endregion

		#region Core Partner Customer Account and Shopping cart details
		internal static List<PTNRData.Account> CoreAccounts = new List<PTNRData.Account>(){
			new PTNRData.Account(){
				CXNId = 1000000000000001,
				CXEId = 1000000001,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000,
				ProviderId = 401,
				rowguid = Guid.NewGuid()
			},
			new PTNRData.Account(){
				CXNId = 1000000000000000,
				CXEId = 1000000000,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000,
				ProviderId = 602,
				rowguid = Guid.NewGuid()
			},
			new PTNRData.Account(){
				CXNId = 1000000000000000,
				CXEId = 1000000000000000,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000,
				ProviderId = 602,
				rowguid = Guid.NewGuid()
			},
			new PTNRData.Account(){
				CXNId = 1000000000000000,
				CXEId = 1000000000,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000,
				ProviderId = 103,
				rowguid = Guid.NewGuid()
			},
		};

		internal static List<PTNRData.Customer> ptrnCustomers = new List<PTNRData.Customer>() { 
			new PTNRData.Customer(){ 
				AgentSessionId = Guid.Parse("45BA3151-5A89-4424-BF73-4629CE1C14DC"), 
				ChannelPartner = channelPartners.Find(a=>a.Name == "TCF"),
				ChannelPartnerId = Guid.Parse("6D7E785F-7BDD-42C8-BC49-44536A1885FC"),
				CustomerProfileStatus = ProfileStatus.Active,
				CXEId = 1000000000000000,
				ReferralCode = string.Empty,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000000000,
				Accounts = CoreAccounts,
			},
			new PTNRData.Customer(){ 
				AgentSessionId = Guid.Parse("45BA3151-5A89-4424-BF73-4629CE1C14DC"), 
				ChannelPartner = channelPartners.Find(a=>a.Name == "TCF"),
				ChannelPartnerId = Guid.Parse("6D7E785F-7BDD-42C8-BC49-44536A1885FC"),
				CustomerProfileStatus = ProfileStatus.Active,
				CXEId = 1000000000000001,
				ReferralCode = "1234",
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000000001,
				Accounts = CoreAccounts
			},
			new PTNRData.Customer(){ 
				AgentSessionId = Guid.Parse("45BA3151-5A89-4424-BF73-4629CE1C14DC"), 
				ChannelPartner = channelPartners.Find(a=>a.Name == "TCF"),
				ChannelPartnerId = Guid.Parse("6D7E785F-7BDD-42C8-BC49-44536A1885FC"),
				CustomerProfileStatus = ProfileStatus.Active,
				CXEId = 1000000000000002,
				ReferralCode = "1234",
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000000002,
				Accounts = CoreAccounts
			},
			new PTNRData.Customer(){ 
				AgentSessionId = Guid.Parse("45BA3151-5A89-4424-BF73-4629CE1C14DC"), 
				ChannelPartner = channelPartners.Find(a=>a.Name == "TCF"),
				ChannelPartnerId = Guid.Parse("6D7E785F-7BDD-42C8-BC49-44536A1885FC"),
				CustomerProfileStatus = ProfileStatus.Active,
				CXEId = 1000000000000003,
				ReferralCode = "1234",
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000000003,
			},
		};

		internal static List<PTNRData.Account> ptnrAccounts = new List<PTNRData.Account>() {
			new PTNRData.Account(){ 
				Customer = ptrnCustomers.Find(a=>a.CXEId == 1000000000000000),
				CXEId = 1000000000000000,
				CXNId = 1000000000,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000,
				ProviderId = 401,
				rowguid = Guid.NewGuid()
			}
		};

		internal List<PTNRData.CustomerSession> customerSessions = new List<PTNRData.CustomerSession>() {
			new PTNRData.CustomerSession(){ 
				AgentSession = agentSessions.Find(a=>a.Id == 1000000000),
				Customer = ptrnCustomers.Find(a=>a.CXEId == 1000000000000000),
				CardPresent = true,
				Id = 1000000000,
				TimezoneID = "Central Standard Time",
				DTServerCreate = DateTime.Now,
				DTStart = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				rowguid = Guid.NewGuid(),
				ShoppingCarts = new List<PTNRData.ShoppingCart>(){ new PTNRData.ShoppingCart(){ }},
			},
			// For Fund Transaction
			new PTNRData.CustomerSession()
			{
				CardPresent = true,
				Id = 1000000001,
				TimezoneID = "Central Standard Time",
				DTStart = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				rowguid = Guid.NewGuid(),
				AgentSession = agentSessions.Find(a=>a.Id == 1000000000),
				Customer = ptrnCustomers.Find(a=>a.CXEId == 1000000000000000),
			},
			new PTNRData.CustomerSession()
			{ 
				CardPresent = true, 
				rowguid = Guid.NewGuid(), 
				DTServerCreate = DateTime.Now, 
				DTTerminalCreate = DateTime.Now, 
				TimezoneID = "Central Standard Time",
				CustomerSessionCounter = new PTNRData.CustomerSessionCounter(){ CounterId = "100"},
				AgentSession = agentSessions.Find(a=>a.Id == 1000000001),
				Customer = ptrnCustomers.Find(a=>a.CXEId == 1000000000000001),
				Id = 1000000002,
			},
			new PTNRData.CustomerSession() // This Customer Session for CPENGINE
			{ 
				CardPresent = true, 
				rowguid = Guid.NewGuid(), 
				DTServerCreate = DateTime.Now, 
				DTTerminalCreate = DateTime.Now, 
				TimezoneID = "Central Standard Time",
				AgentSession = agentSessions.Find(a=>a.Id == 1000000001),
				Customer = ptrnCustomers.Find(a=>a.CXEId == 1000000000000001),
				Id = 1000000003,
			},
			new PTNRData.CustomerSession() // This Customer Session for MoneyTransfer
			{ 
				CardPresent = true, 
				rowguid = Guid.NewGuid(), 
				DTServerCreate = DateTime.Now, 
				DTTerminalCreate = DateTime.Now, 
				TimezoneID = "Central Standard Time",
				AgentSession = agentSessions.Find(a=>a.Id == 1000000001),
				Customer = ptrnCustomers.Find(a=>a.CXEId == 1000000000000001),
				Id = 1000000004,
			},
			new PTNRData.CustomerSession() // This Customer Session for Shopping Cart
			{ 
				CardPresent = true, 
				rowguid = Guid.NewGuid(), 
				DTServerCreate = DateTime.Now, 
				DTTerminalCreate = DateTime.Now, 
				TimezoneID = "Central Standard Time",
				AgentSession = agentSessions.Find(a=>a.Id == 1000000001),
				Customer = ptrnCustomers.Find(a=>a.CXEId == 1000000000000001),
				Id = 1000000005,
			},
			new PTNRData.CustomerSession() // This Customer Session for Billpay
			{ 
				CardPresent = true, 
				rowguid = Guid.NewGuid(), 
				DTServerCreate = DateTime.Now, 
				DTTerminalCreate = DateTime.Now, 
				TimezoneID = "Central Standard Time",
				AgentSession = agentSessions.Find(a=>a.Id == 1000000001),
				Customer = ptrnCustomers.Find(a=>a.CXEId == 1000000000000001),
				Id = 1000000006,
			},
			new PTNRData.CustomerSession() 
			{ 
				CardPresent = true, 
				rowguid = Guid.NewGuid(), 
				DTServerCreate = DateTime.Now, 
				DTTerminalCreate = DateTime.Now, 
				TimezoneID = "Central Standard Time",
				AgentSession = agentSessions.Find(a=>a.Id == 1000000001),
				Customer = ptrnCustomers.Find(a=>a.CXEId == 1000000000000001),
				Id = 1000000007,
				ShoppingCarts = new Collection<ShoppingCart>
				(
					new List<PTNRData.ShoppingCart>()
					{
							new PTNRData.ShoppingCart(){ 
								Customer = ptrnCustomers.Find(a=>a.Id == 1000000000000000 ),
								Active = true,
								IsParked = false,
								IsReferral = false,
								DTServerCreate = DateTime.Now,
								DTTerminalCreate = DateTime.Now,
								Id = 1000000000,
								rowguid = Guid.NewGuid(),
								ShoppingCartTransactions = new Collection<ShoppingCartTransaction>
								(
									new List<PTNRData.ShoppingCartTransaction>()
									{
											new PTNRData.ShoppingCartTransaction(){ 
												CartItemStatus = ShoppingCartItemStatus.Added,
												Transaction = new CoreTrans.MoneyTransfer() { CXEState = 2 }
											}
									}
								) 
							}
					}
				)  
			},
			new PTNRData.CustomerSession() 
			{ 
				CardPresent = true, 
				rowguid = Guid.NewGuid(), 
				DTServerCreate = DateTime.Now, 
				DTTerminalCreate = DateTime.Now, 
				TimezoneID = "Central Standard Time",
				AgentSession = agentSessions.Find(a=>a.Id == 1000000001),
				Customer = ptrnCustomers.Find(a=>a.CXEId == 1000000000000001),
				Id = 1000000010,
				ShoppingCarts = new Collection<ShoppingCart>
				(
					new List<PTNRData.ShoppingCart>()
					{
							new PTNRData.ShoppingCart(){ 
								Customer = ptrnCustomers.Find(a=>a.Id == 1000000000000000 ),
								Active = true,
								IsParked = false,
								IsReferral = false,
								DTServerCreate = DateTime.Now,
								DTTerminalCreate = DateTime.Now,
								Id = 1000000000,
								rowguid = Guid.NewGuid(),
								ShoppingCartTransactions = new Collection<ShoppingCartTransaction>
								(
									new List<PTNRData.ShoppingCartTransaction>()
									{
											new PTNRData.ShoppingCartTransaction(){ 
												CartItemStatus = ShoppingCartItemStatus.Added,
												Transaction = new CoreTrans.Check() { CXEState = 2 }
											}
									}
								) 
							}
					}
				)  
			},
			new PTNRData.CustomerSession() 
			{ 
				CardPresent = true, 
				rowguid = Guid.NewGuid(), 
				DTServerCreate = DateTime.Now, 
				DTTerminalCreate = DateTime.Now, 
				TimezoneID = "Central Standard Time",
				AgentSession = agentSessions.Find(a=>a.Id == 1000000001),
				Customer = ptrnCustomers.Find(a=>a.CXEId == 1000000000000001),
				Id = 1000000011,
				ShoppingCarts = new Collection<ShoppingCart>
				(
					new List<PTNRData.ShoppingCart>()
					{
							new PTNRData.ShoppingCart(){ 
								Customer = ptrnCustomers.Find(a=>a.Id == 1000000000000000 ),
								Active = true,
								IsParked = false,
								IsReferral = false,
								DTServerCreate = DateTime.Now,
								DTTerminalCreate = DateTime.Now,
								Id = 1000000010,
								rowguid = Guid.NewGuid(),
								ShoppingCartTransactions = new Collection<ShoppingCartTransaction>
								(
									new List<PTNRData.ShoppingCartTransaction>()
									{
											new PTNRData.ShoppingCartTransaction(){ 
												CartItemStatus = ShoppingCartItemStatus.Added,
												Transaction = new CoreTrans.MoneyTransfer() { CXEState = 4, CXNState = 4 }
											}
									}
								) 
							}
					}
				)  
			},
			new PTNRData.CustomerSession() 
			{ 
				CardPresent = true, 
				rowguid = Guid.NewGuid(), 
				DTServerCreate = DateTime.Now, 
				DTTerminalCreate = DateTime.Now, 
				TimezoneID = "Central Standard Time",
				AgentSession = agentSessions.Find(a=>a.Id == 1000000001),
				Customer = ptrnCustomers.Find(a=>a.CXEId == 1000000000000001),
				Id = 1000000012,
				ShoppingCarts = new Collection<ShoppingCart>
				(
					new List<PTNRData.ShoppingCart>()
					{
							new PTNRData.ShoppingCart(){ 
								Customer = ptrnCustomers.Find(a=>a.Id == 1000000000000000 ),
								Active = true,
								IsParked = false,
								IsReferral = false,
								DTServerCreate = DateTime.Now,
								DTTerminalCreate = DateTime.Now,
								Id = 1000000000,
								rowguid = Guid.NewGuid(),
								ShoppingCartTransactions = new Collection<ShoppingCartTransaction>
								(
									new List<PTNRData.ShoppingCartTransaction>()
									{
											new PTNRData.ShoppingCartTransaction(){ 
												CartItemStatus = ShoppingCartItemStatus.Added,
												Transaction = new CoreTrans.Cash() { CXEState = 2 }
											}
									}
								) 
							}
					}
				)  
			},
		};

		internal static List<PTNRData.ShoppingCart> shoppingCarts = new List<PTNRData.ShoppingCart>() {
			new PTNRData.ShoppingCart(){ 
				Customer = ptrnCustomers.Find(a=>a.Id == 1000000000000000 ),
				Active = true,
				IsParked = true,
				IsReferral = false,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000,
				rowguid = Guid.NewGuid(),
                ShoppingCartTransactions = new List<PTNRData.ShoppingCartTransaction>(){ new PTNRData.ShoppingCartTransaction(){ CartItemStatus = PTNRData.ShoppingCartItemStatus.Added, Transaction = new PTNRData.Transactions.BillPay(){ CustomerSession = new PTNRData.CustomerSession()}}}
			},
			new PTNRData.ShoppingCart(){
				Customer = ptrnCustomers.Find(a=>a.Id == 1000000000000000 ),
				Active = true,
				IsParked = true,
				IsReferral = false,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000001,
				rowguid = Guid.NewGuid()
			},
			new PTNRData.ShoppingCart(){
				Customer = ptrnCustomers.Find(a=>a.Id == 1000000000000000 ),
				Active = true,
				IsParked = true,
				IsReferral = false,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000005,
				rowguid = Guid.NewGuid(),
			},
			new PTNRData.ShoppingCart(){
				Customer = ptrnCustomers.Find(a=>a.Id == 1000000000000002),
				Active = true,
				IsParked = true,
				IsReferral = false,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000002,
				rowguid = Guid.NewGuid(),
				ShoppingCartTransactions = new Collection<ShoppingCartTransaction>
				(
					new List<PTNRData.ShoppingCartTransaction>()
					{
							new PTNRData.ShoppingCartTransaction(){ 
								CartItemStatus = ShoppingCartItemStatus.Added,
								Transaction = new CoreTrans.MoneyTransfer() { CustomerSession = new CustomerSession(){ Id=100000000}, CXEState = 4, CXNState = 4 }
							}
					}
				) 
			}
		};
		#endregion

		#region CXE Customer account details
		internal static List<CXEData.Account> cxeAccountsForCashIn = new List<CXEData.Account>() {
			new CXEData.Account(){ 
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000,
				Type = 1,
			},
			new CXEData.Account(){
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000,
				Type = (int)CXEData.AccountTypes.BillPay
			},
			new CXEData.Account(){
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000,
				Type = (int)CXEData.AccountTypes.Funds
			},
			new CXEData.Account(){
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000,
				Type = (int)CXEData.AccountTypes.MoneyOrder
			},
			new CXEData.Account(){
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000,
				Type = (int)CXEData.AccountTypes.MoneyTransfer
			},
			
		};

		internal static List<CXEData.Customer> coreCxeCustomers = new List<CXEData.Customer>() { 
			new CXEData.Customer(){ 
				Address1 = "BAY",
				Address2 = "Street",
				FirstName = "Nitish",
				LastName = "Biradar",
				City = "Bangalore",
				CountryOfBirth ="US",
				DateOfBirth = new DateTime(1990, 05, 01),
				SSN = "123456789",
				IDCode = "S",
				State = "KA",
				Phone1 = "125896312",
				Phone1Type = "Home",
				SecondaryCountryCitizenShip = "US",
				GovernmentId = new CXEData.CustomerGovernmentId(){ 
					IdTypeId = 2, 
					Identification = "Passport", 
					IssueDate = new DateTime(2005, 10, 10), 
					ExpirationDate = new DateTime(2020, 10, 10), 
					DTServerCreate = DateTime.Now, 
					DTTerminalCreate = DateTime.Now },
				ProfileStatus = Common.Util.ProfileStatus.Active,
				EmploymentDetails = new CXEData.CustomerEmploymentDetails(){
					Occupation = "Student",
					OccupationDescription = "Student",
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					Employer = "Testing",
					EmployerPhone = "9874563215",
					Id = Guid.NewGuid()
				},
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000000000,
				Accounts = cxeAccountsForCashIn
			},
			new CXEData.Customer(){ // For Money Order
				Address1 = "BAY",
				Address2 = "Street",
				FirstName = "Nitish",
				LastName = "Biradar",
				City = "Bangalore",
				CountryOfBirth ="US",
				DateOfBirth = new DateTime(1990, 05, 01),
				SSN = "123456789",
				IDCode = "S",
				State = "KA",
				Phone1 = "125896312",
				Phone1Type = "Home",
				SecondaryCountryCitizenShip = "US",
				GovernmentId = new CXEData.CustomerGovernmentId(){ 
					IdTypeId = 2, 
					Identification = "Passport", 
					IssueDate = new DateTime(2005, 10, 10), 
					ExpirationDate = new DateTime(2020, 10, 10), 
					DTServerCreate = DateTime.Now, 
					DTTerminalCreate = DateTime.Now },
				ProfileStatus = Common.Util.ProfileStatus.Active,
				EmploymentDetails = new CXEData.CustomerEmploymentDetails(){
					Occupation = "Student",
					OccupationDescription = "Student",
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					Employer = "Testing",
					EmployerPhone = "9874563215",
					Id = Guid.NewGuid()
				},
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000000001,
				Accounts = cxeAccountsForCashIn,
				ChannelPartnerId = 34
			}
		};

		internal static List<CXEData.Account> cxeAccounts = new List<CXEData.Account>() {
			new CXEData.Account(){ 
				Customer = coreCxeCustomers.Find(a=>a.Id == 1000000000000000),
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 1000000000,
				Type = 1
			}
		};
		#endregion

		#region Prospect Details
		internal List<PTNRData.Prospect> prospects = new List<PTNRData.Prospect>() 
		{
  			new PTNRData.Prospect()
			{ 
				Address1 = "test", 
				Address2 = "test2", 
				ChannelPartner = channelPartners.Find(a=>a.Name == "TCF"), 
				ChannelPartnerId = Guid.Parse("6D7E785F-7BDD-42C8-BC49-44536A1885FC"), 
				City = "Testing",
				ZipCode = "12345",
				State = "CA",
				DateOfBirth = new DateTime(1990,10,10),
				Phone1 = "6985478569",
				Phone1Type = "Home",
				CountryOfBirth = "US",
				FirstName = "Nitish",
				SecondaryCountryCitizenShip = "US",
				LastName = "Biradar",
				AlloyID = 1000000000000000,
				ClientID = "1000000000011110",
				GovernmentId = new PTNRData.ProspectGovernmentId()
				{
					Id = Guid.NewGuid(),
					IdType = new PTNRData.NexxoIdType(){ Country = "US", CountryId = new PTNRData.MasterCountry(){ Name = "United State"}},
					ExpirationDate = new DateTime(2020,10,10),
				}
			},
			new PTNRData.Prospect()
			{ 
				Address1 = "test", 
				Address2 = "test2", 
				ChannelPartner = channelPartners.Find(a=>a.Name == "TCF"), 
				ChannelPartnerId = Guid.Parse("6D7E785F-7BDD-42C8-BC49-44536A1885FC"), 
				City = "Testing",
				ZipCode = "12345",
				State = "CA",
				DateOfBirth = new DateTime(1990,10,10),
				Phone1 = "6985478569",
				Phone1Type = "Home",
				CountryOfBirth = "US",
				FirstName = "Nitish",
				SecondaryCountryCitizenShip = "US",
				LastName = "Biradar",
				AlloyID = 1000000000000001,
				ClientID = "1000000000011110",
				GovernmentId = new PTNRData.ProspectGovernmentId()
				{
					Id = Guid.NewGuid(),
					IdType = new PTNRData.NexxoIdType(){ Country = "US", CountryId = new PTNRData.MasterCountry(){ Name = "United State"}},
					ExpirationDate = new DateTime(2020,10,10),
				}
			}
		};
		#endregion

		#region Collection Of Check Type
		internal List<PTNRData.CheckType> checkTypes = new List<PTNRData.CheckType>() 
		{
			new PTNRData.CheckType(){
				Name = "Goverment Type",
				Id = 1000000000,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				rowguid = Guid.NewGuid()
			},
			new PTNRData.CheckType(){
				Name = "check",
				Id = 1000000001,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				rowguid = Guid.NewGuid()
			}
		};
		#endregion

		#region Core Partner  Demmy collections
		internal List<PTNRData.TipsAndOffers> tipsAndOffers = new List<PTNRData.TipsAndOffers>() { new PTNRData.TipsAndOffers() { Id = 1 } };

		internal List<PTNRData.UserRole> userRole = new List<PTNRData.UserRole>() { };

		internal List<PTNRData.IdentificationConfirmation> identityConfirms = new List<PTNRData.IdentificationConfirmation>() { };

		internal List<PTNRData.UserStatus> userStatus = new List<PTNRData.UserStatus>() { };

		internal List<PTNRData.UserLocation> userLocations = new List<PTNRData.UserLocation>() { };
		#endregion

		#region Transaction Fee For Funds, Check, and MoneyOrder
		internal List<PTNRData.TransactionFee> transactionFee = new List<PTNRData.TransactionFee>()
		{
			new PTNRData.TransactionFee(0 , new List<PTNRData.Fees.FeeAdjustment>()){ 
				IsSystemApplied = true,
				NetFee = 5M,
			}
		};
		#endregion

		#region Nexxo Id Types
		internal List<PTNRData.NexxoIdType> nexxoIdType = new List<PTNRData.NexxoIdType>() 
		{
 			new PTNRData.NexxoIdType(){ 
				Country = "US",
				IsActive = true,
				Name = "Test",
				rowguid = Guid.NewGuid(),
				HasExpirationDate = true,
				Id = 10000000000,
				State = "AZ",
				StateId = new PTNRData.State(){ Abbr = "CA", CountryCode = "USA", DTServerCreate = DateTime.Now, DTTerminalCreate = DateTime.Now, Id = Guid.NewGuid(), Name = "Califoria"},
				CountryId = new PTNRData.MasterCountry(){ Abbr2 ="US", Abbr3 = "USA", DTServerCreate = DateTime.Now, Id = 100, Name = "United States", Rowguid = Guid.NewGuid()}
			},
			new PTNRData.NexxoIdType()
				{
					rowguid = Guid.Parse("5128F4BB-4578-4790-AC22-1098F1291184"),
					Id = 142,
					Name = "DRIVER'S LICENSE",
					Mask = "^[\\w-*]{4,20}$",
					Country = "UNITED STATES",
					State = "CALIFORNIA",
					CountryId = new PTNRData.MasterCountry()
					{
						Name = "UNITED STATES",
						Abbr2 = "US",
						Abbr3 = "USA"
					},
					StateId = new PTNRData.State()
					{
						Name = "CALIFORNIA",
						Abbr = "CA",
						CountryCode = "840"
					}
				}
		};
		#endregion

		#region AgentMessage Details
		internal List<PTNRData.AgentMessage> agentMessages = new List<PTNRData.AgentMessage>() { 
			new PTNRData.AgentMessage(){ 
				Agent = userDetails.FirstOrDefault(), 
				DTServerCreate = DateTime.Now, 
				DTTerminalCreate = DateTime.Now, 
				Id = 1000000000, 
				Transaction = new PTNRData.Transactions.Check(){
					 Account = ptnrAccounts.FirstOrDefault(),
					 CXEState = 1,
					 CXNState = 1,
					 CustomerSession = new PTNRData.CustomerSession(){ Customer = ptrnCustomers.FirstOrDefault()}
				}
			}
		};
		#endregion

		#region Check Transaction
		internal List<CXEData.Transactions.Stage.Check> stageChecks = new List<CXEData.Transactions.Stage.Check>() 
		{
			new CXEData.Transactions.Stage.Check(){ 
				Id = 1000000001,
				Amount = 100,
				CheckType = 1,
				Fee = 1,
				Status = 1,
				rowguid = Guid.NewGuid(),
				IssueDate = DateTime.Now,
				MICR = "o001003ot075911603t182380188280o",
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Images = new CXEData.CheckImages()
				{ 
					id = Guid.NewGuid(), 
					DTServerCreate = DateTime.Now,
					Back = Convert.FromBase64String("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IjraJEELdCz2R8jxHS92rFBszY2R8IZnMV2N7KHXiR09iPBEdPRQtiI/R4TFBhA3aGmE5Q6tKwsWI6tlDI6PiqHEIEM9ozWzjgiPo46XEIEIaCCFtHiDCZQ+d4vGxyh0WVIQyHHO5Q53CBtC3cIMEGLhCDiLEGF4RFK4QVacfgmDCPDZxwhFNnhCIiKO7FUGmeCJJWvOOCI6OEPDUJhqIcodhAn/BgiOhDVTvKN5o/PgTnhFxEZQ/DFMovEOho8BBDrE7qCp3/89ydiI15ZCkxDhnmlMbOhA72ynnkY+2CLpilOCr/ufMzRHyPsocp0+juxIIME20kDCbcUPFiJMYiHap44p04jER/czrYRHxGUPlOEECFp0iyq0R8j4QZ2nRdMIQYLaz45x+fv9MiQpmxc+i+cKIRH+wg6NDGItAzzVMIjoDUGEwRHWlCI+UaIu47FI8WccI8ggRHRxKELnHxxDQIvhESJ4WccER0/FoNe0CBIRiKuccER/2FV08RaGyoQtUjwmqDKHK88UvzUyPs0yhh1U44Ij844Ij9uQQcSQg489gv/qR+LVRlDhMpDEatSOkLtM06+L7Cx+1o44Ij63nhkf6ioj9DaYyfdBgpwU2aEacXXWCPawzDlUsVF8R5U4ooxFr8shaoIjoMIEkHY1gynRnMocEXRgYiNjxrBEdRTu7ygyRwVfdwwgpT0eGemVClDhEfTF4xZQ4RZUg6o2mCLwURZbqEpBBT4xTCBj5ZCjCphX1wknHxQTODTYXdpi4UkhF0Ij6R8YZQ+z+R72KlPRyA4pipUPHhEdVKeERWUmCLoNl8NtHkCBUqFnMuyOi+cbsodM49RDo0ULD4xu4NP61GaKNlDlMo14h2IQiLOIuiOi6DKco7jsKog2LbOO/5x75ZCptSf2V7jhEfCbKHhPEQQKMMpwg7OOccRESMFCnexCwRT+kCI6Fr4Ij/qKqbQT6YpAiPjCUGEZnGIiIhkHoVZQsRed9OCFHxYpT0hBx76Z4iLpHHCI/EJHhi006RdSoYIK0ceIaEbYrBsjpar/ivrImN4oNRniG1I6CGnxcQiOiyk06LcLyeERDQiynKdBlOgWnKNGH+GDtS7iMjaizPjw8EIiIiIiK7BcIRoOynBF1KtMIMp1spwWGCLoQ49iIiIiIiIiIiIi/h/D+P////+H+v8swQGUXP0L/X17/+v//v/6/7ss9X9D//9vVP39fr6///////+v//v/6+/v////6+tX9f///f1///3//v71///r66/+/////////f/+/9b/+v/r/////////r//7/vf1v/qv/r/v+v+v7+v/////v77Xi9ev4X//////7//en3113wv//////3/+HoP++uFwv///////99Pb1/+uv5z/////4kGBzOU5kyTxESBmBOOYcw5hzuSc7mHO53PBTncrzuVZUHHO5xQpUKaZBQwkXK48HHOOQadcRERERERERERERERERERgAgAgAAOAAABAwABAAAASAYAAAEBAwABAAAAtAIAAAIBAwABAAAAAQAAAAMBAwABAAAABAAAAAYBAwABAAAAAAAAAAoBAwABAAAAAQAAABEBBAABAAAACAAAABUBAwABAAAAAQAAABYBAwABAAAAtAIAABcBBAABAAAADzIAABoBBQABAAAAxjIAABsBBQABAAAAzjIAABwBAwABAAAAAQAAACgBAwABAAAAAgAAAAAAAAAAAAAZAAAgAAAAABkAACAA"),
					Front = Convert.FromBase64String("SUkqACAuAAAmoFtyB0BfiGQBuOQOByMcw5A8Y53JOQVxzDmHIaY5xzOccw53JOdzuYc9lDngpyoPxSgoczlBMhkDHMOQo5nJDmHMOYcw5GOYcw5DLHMOZyQ53O53MORjmHMOeCoKc7nc7nc7mHO53O54KgqCoK48HoeDoKHKoUgo5Wh3KmVqU505/KgriigqCtDnK8qZXlQVBSCtSvKgrUqCvK48FOVB4Kc8FalalIKEygsoc8iwoGVBTLWMHkNtzjkY5hyNzudMKmREysa/luguvW/v5bdSJoiIzqiMvzsaRvIjIxGpG86ZuIebzpmM6ZvOmYiHGI+jkXZtF0ci7Noj5Hy+YZtEfI6LsuzaI+R0RzI7Noui6LxiNoj5HRjMM2iPkdEdmGeRdF4uzaI+R0YzDNojouiOzEbRdHMwzaI+R0XjDNoujkXZ5F0XjDPI5GI+jkXZ5HswzyLovF2bRdF0R2XZjI+R0Xi7LojsjjLsjouiPkdEcyOZeI+R0R2XDBHZdmw5czAcuy5EdEcyOMjojmRzI4cjjLsjoj5HyOiOZdkdEcyOiOy5mGR0RzLsxkdkdlzMIj5HRHMuyOMjojmRw2EcNBcNBdkcQjmXZHRHDlzI5EdEcyOGguG0Rw0Fw0EcNhHDQXDQRw2Fw0BENg5DQOIi/yKRxHDLx9F0R4wGSRHA8NyOGwjg5HIjg4IURw2iODkciOG0Rw0EcNhHDYRw2EcHI5EcNhHDQRwcjkRw2EcNBHBwQoIREIIRERBAhCBCIiJ3EQiODkci4IRyBCIggQmHEREw4iEEIiIgiOQQiIiERwQjkRw0EcNgQiIIEJhwQITDidwRHIIRBEcghEQQIREECEw4IjhsI5EcHBCYcERyLggITDiYcERxkcEBCYcERyI4OCEw5BBxOOCBCYch3JOQ45GOQQcjHIcf+hEERyI4aC4aZHIjg5HIjghHGRwQjjI4IRxkcORxlw5HGRw5HGyHHIo5DjkY7I4IRxlwqZHDqyHHI3IUdkcOmRw5HGRw5cZHDkcZcKRxS4UjikcEI4yOHTI4cjjZDjsjh0yOHI42Q47I4dMjhoI4dMjhyONkOOy4aGkyOHTI4cjjZCjsjh0yOHI42kyOHI4y4UjikcOrSZHDkcbSZHDpkcORxkcOmRw8jdJkcOmRw0NJkcORx0rSZHDpkcOmRw0EcOmRwqfv7/0mRw0SGwdJkcOmRw9kcPv9/38chx9Kv3//+v+vS/8f9x//Ghqv8f/x/r9/H/x/Gv8hR6/+v/yFH/8RHFfyo///xER8RHHHHZHDYRw0RcRHEREX2Rw0cREXZHC8RIUcREREWRw2EcNhHDIBUI4RCkLiIiIiyOETI4RCLI4NSERFkcNhHCIRZHCIUmRwWP4iIiIiyOBmDIRwyAVSOBNDK/iI1+QOgMo5A8NscgeG2OQ0xyMcgg5DYOSHIbByGkORjkEHIxyHHIxyCDknr8geDUOQQcjchpOSHJOTckOQ0DknJuTckOSHJOTHJOSHJOTckORjkxyTkhyTkhyQ5JybkO5Ick5NyQ5JyY5JyQ5Ick5NyQ5McjHJDknJuSHJDkhyTk3JDknJuScm5Mck5NyTk3JuSHJDknJuSHJDknJDk3JDkhyTk3JDkhyQ5IckOScm5IckOScm5IckOScm5Ick5IckOScm5IckOSHJOSHJDkhyQ5JyQ5IckOSHJOTckOSHJDkY5NybkhyTk3JuSHJDknJvknOOdyQ5IckOSHIxyQ5IckORjkhybkh8k5NyQ5IckOSHJOTckOScm5IckOSHyTnHO5IckPeSHyTkh7Ek5IfJDknJDkh2Ryk3tSbsu2THv/yI5Ick5NyHckOSHJOSHJDknsmOyOSJOScmOSHb61JuSfr1vtvS9K3X18m7f31brfqyOe6zjp/br6WyOduu/vX1BEeevSp/df967r/iv96Vit9tuleruv6ft6Q+tileq/elj2NP1ekyOfHxbFLdcWRzrj41Q1QiLf/r0SHcREVxFkcDwbxERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERETjoIocw4ikJxxSEsdQQJkeBBl5FDiUOYcsdI45hxKHBBMvBMvI8JHcqDudyosEEy6RUBBnc7hSoTK3dP+kPiJAhzjmHO6R3SO5UFQVBTaJRX8SkwD/5kYBgvnaAV//i1kx7/kqRhGF8odmP/QiD/YMjp/ncIjoiQGF4jf6CWCB/S/EZJhq6W/6Dr190Gvf+E//1T0i3QpThEdf4T8t/F0CS75Q4RHSDrRcM7FCIaMIujqjCLooRjI+Xy+eiPl0SeR4jjPIwjCPoujCMZdGEfCEdE8R4j5HGfIxnyMIg0XiOj+R0ZovkgjaOzJmEYyNxT5myKRXF+Iv+y56foJwgYIRJeIk/ERENCJGnEQ0IsELQiIaEGhEYQIjjQIjlEQYIXEMECI4oIIjmqputf8RqvQQJsInYYZHAYZKAwyPIZKGGS6GEwQZKGGStgyXUEHCCDYYTYYSBB0EHd22nS+PT6S3SDbTeG3D4Ye9tYb28PT0007pB0E/346wRH9PXL66V/t7/u6T29/vv3Sur/f1/3Uhlucc0Hcp04f3379/dtL/3V0370uvMInXOPyDWXRQidSnr+q0IiI4fW+96Obr5v2//zi7/Vv3xwgwQZwsxd5uMMxIGRziDBBmJP+uCI+mUtZECVYrX/3j/jpL/qNjDF+11iU7R4aPb62/ey5l6W9FW7I+V7KN/jUjYh0BM6AQU4bFbSX0u9f9in/+Dh8p3WPX9egtv6pp6bXaEyFD7f+zgZwgwnggfnY0MgozWGCBhgkweK2/+t0tX661Yberf9aFqOlv/17x52WsqyBA1p/1TUEHQQfIEgZJyEHJQQz8QfUgRwZQDcEQbjCGwgL9L49KP0sJR2w921/edyux7D+//nPIKRJkoQNP/9EGt+EkHhBxaEahNMJBBhBhAx+IX0hW3FRCww4ftt7/j3v9f8eCDBB4TTWTHa/wjQGhDCeE7XCD1CDCBhB/XyTupJ7wuSi4f7r9b/j7f+d+mEwmmiXPlDiU9f0CekKTVLT1CaaD+THf07Jj8OnJ6kx3u2G/Y+/wgVginBEe36X/wRT0GmqJY4Tcz9t/53BF1T6p2lFxhNV0tv91b9ffwcP77mi9tLS293v/q0tEuolzWEHSdViP+gqwn9PSvTSX9+rrvziRnf359Bsug/vsNL42Pb3sL/qyO6CbSDYVJvb3p/iOn1TtJNJNUn+/84tvdUr99wQK3t/d2RLe00nYTeJQ9UGmxVBOm6eSH71pe/6TqqppenXdbetCrvilT+3ivXm73s1wMEIgwhERERERER3p4T1r/+L/pJxVqmkk/v1rt6q39/96f7lGmIpfcLV90u/Vevrj6dLvSvW9UrVW9b+r7bbCS+ljXf/r/6og3x13VWkm3hLC76r3bqC2la6atNhL39P/rlOER1/UcQnpOQWPyGjMhoyG0kG2EIQIUEHDCGEGEGEGEHEVBpQwQWEGEG7FP//9r8Eq8ER/jdLp6DSThiCXknMOE4Z3CJ+UOYczndBneDKgEDKgEGVBRz8dzD53EKU4p53PClQE4YKv9/6pYj7j661tOrDQiIiIiIiIiIiIiIiIiIiIhoRERER///V//jrrsPbS/6///XyCn5DNvS/DGGNf//aX/6CBhMVIF8yBc42Gw0v9/6+v9aZA8FnUofTFcHBoL//9LlP/tKqFqE1sOGP///6/BEf69hf23//91x/HYRDMHXqtbbff/9Lf/DXXX92///vr/YaSuiCqDKyvWG36/99f/gy4aX4sgsDteQ2FDDwiOvf/2JF1Jjv+I7WL1oO2WeEZo0R/J0T8d6//yhxKdfsgfOkJAuPJxrrZGmHOQOWgDFLkSFYQMEHRHFkcB9O/617DI6fhEdatWiH9w17D25kJIwiNkUIwihF0YiPl8vl0YRhGEfRfIMiHl4jooRdGEYRQjCL5fL55F8wpUZ3ahUeM71PU8JHd+v++GVBUFOWMEfodfYRDQO4rJxph0GHCEMoEBA0LCFoREcRDQsIER2CESfiIg4iIsIRJLjEk4Vd0m+qFJ/YRHXv2oiNV+1Wx32iCeHpuHhIvG2GEGS5hhBhBAmGGRwGyOAwyWQyNw18Gweh+kv/jXuvv92l/WGEQ0OHoNXSSbbrDvpNtNtBvDtBuvrRHH9Ecf7/XS/+v7kDcexEcd6t39dK3+3v6Ta/BEe9Upndq2vXew1q1ra2mk62l9K7fdtVvfvffjLv6Df1i8etrulaX/tfXSyCqO6tHHr35xW3VP/5xfpAwRH2r2v1SwynsNJtLYYVirlOER1+9O1a00hzj695x6+Okt/t6j+cxxrv/znhBD2Em1KA2Qwgmq0EvfS7UgeC8Oul7X72/Vv///XdL9JtKvW0mKYqRHhigwmoj/YW7STdbVb117frpeqX69cfscfsVGGmph1sEDCH/DXTW9U1Ia469dfb42K91/C6VBhCLBBhCLCDCaYUcmOiIx/2qDtQmKVppqr/+//iF8cUIiIiDCBmHCERH/a9gqkGUdbCaDW9d+xyUWvr5J9CI1+wlDIaIgwqdrDQNA1dr/vv+iQ7/eTHcp/4YLaw0yGqOmq2EGE1vW/v7+7f77/+DCZxzOccw5xMISXBBFwwqDBA0DCqP/ff/f/24v8MEIiIkXIXDCEGCBkF1AW/1v5vXrvrc391+QMGBcGCESNO63XfHd129cUt/8lSxH/r9dut+v27/F9rbS30ldJb0te8H9o0ZJowipxWcp0XyJ66tpW+C7dW2mEsKm8ocm8LzvdNNMJqvYQbhA2GkEG4QwgbDCQQbEQghhBsPYZHT9BAi6dPpPVeUPGd0DEIMqLhnc7ncER8GVAIGII6pncqCi5hzDmHO6lOdwpUBRGvEbzPI6NQ8wzzOMwczzDPETowzDy5eXM8RwdCIiIiIiIiIiIiIiIiIiI1/WNm4LhBoNNPEINBkfTQh6foNBp/+8NdV06Thot6LH8Sx6S9It2ix6LetrW7fU1+aBMOIU1thxEINc44U1uLqIigxTX97hulSikokx0lhj1Xr1Sq+ER/9wRUD/0vwRUEEHr+vX9YPXyh9J384/mHNddJOSxf9Pzjl35hzWU7Rh60gl+JQ4IuniYerDC/2vYUuohhNj0lhhLcL1YTY2GqhD+Ib1FR9bHsUrI3FUx7FcVGxuxX/bTThpraapraaDtC1QalUCRQ6YIjof9xEREREVEQRoiIiIoSnYj/tB4piv6EXalP+xGP4Iun+Nf2g/3+ZIRGZAmdGcRfL5fLooRfOCFxSYI5nIxnCM4nMzI15jIupMf+dpDOGcGfjDQYIGbjkXgmEGhFxJQIaoNNdU7CaaqaiNmEDMDPNFOy/87001+k/0Hb3+n+mqafpprrwyOvoIIjq5h3t+i7f6LijXBgpsy5loBIu/6LvaLtou38z5h2i3c57H8Ro1qnmD30/9N71/1X/T9PT+kKT0OuF/G+D9L8F1eGPa5H5F+dHjH4ow/H/kceR2Ssw9L+RYFXH/H+yL2OIOHp//ebPv7+062/702CBD/2tvr9V/V9f+/uYev/zRmHYZUa/mz3hvuXMtHebPx//zR+aIhuMyBxzPGcCGeez0ejPMDOpnM9EcIXEIZHo5mAh8KR45HCORyMDM49HMwKURyNDNQzMj2aZIjwh4i3EZ/KfMRHyT4RH//Scfr/9T+D8uNe+cv847wfr//++wg7VV04YQdr2g1Vfwn6a9w/CDhoNVuGg9dLJf0U4VfKHBEdf5Ht+1/6t+nv6v/ultbX//yPfenV972vXad718t38t3d1/M+0THva6LfdDQ4ikv2RyI61vtdzc2Ev2OP+Qo5CZBpj/Y5ssbFGLkJMbaTHpH6+bJ6fXSmvNb+ahsovPzJwXoJv0nW+a3rQbmvCbWrmzQd85HEYVl0ezzsujkcReQ/iNimNiuuK/pr/vFr9rt2v6jpj/79L77Sdv/rvX/fVxvT39Me1f/7HUIOMj6dhBpp2g0L/2mmmvDX7TCd9pp2E7tMJpoMuid2mg0GmFV9Pt8ER79/0pEhr3BEe5CEY/+DmH3/uwe9d6MPtYO6nhwtrRno8NrR4sztfxEREREREREREREREREcREREYIj+v9Jf//3Fb/v6/waXOPa/4f91fcbBvpPXWk3T17dO/6Xf/r3/+3X67GvLHv+G+9j/UMP9+t6D6UML/0IkcvS/38dj/fj6Vfjmoa//LT6B/3M3Grf+Q3rpehzWzRt8f/hf9cH/9teNe3q/9vrf/11I86v/56ddv7vcvPHPT/ginPf73zvUL8ER/e0lq/1dX/bC7/9pXV97Wl1eYTpa3r/+c/9tD/f9/j9jd0XmRB8VFVIR/FeyER9jY2ObPxxmJik4pipCDkIObPxTTDX/tGHgt8V//uwuq96DT/hp3/wwmmku24Qa2umt9vsNbEL/DFBi29r/9oQwRHQiIiIgwQiIiIiIiIMEIiIiGUyqd/ERERZN/6KHDI6Wp3BF0V1COwRRF90FzstCFWzkU8YZHiE04dfEe6/r//JOunSHoEU87GuveEJxnMj95szER/TI8YI2SDswRsZuNMxFyMRcjEXjMjZBDMGbIuZiMRcUwRczZGxhBmyMCkdmYpHZiLu/8INDfTjX00/TvtbWCI8unqg9BtpXp3qnFxd1fqyLBn+kjO/a5Y+YewtFu0THf87//RbtBTPRowkW+NFvhIztGho0OZ2i3ctzPljkx7FAwRH/1SbhN+8FtdPX9O04+vf9U7ST317W9aKHw4UOEk9PX01uCKHCr5Q4Iuv66Q/XoWP/reOIp4//j42mviNi2Lx//+ooECX4lDjl/+P/1X/XjZMfE4///0/9tv///iP4/92b/9mNGj998mOv6/+a1zUp/N5ohGjmL//dP/q0usu/fddfdXy9XVVmJhrscNPsjtAiPWvIg77y+2rYX8veqf/bH8V68ccPiq999/ivYqmKW2K2opik4qK//5n/e7W7XvtO1hr/rfacMKmWPaw1T1TQdppoNPvvT9yn/iIiIiIiIiIiIiIiIiIiIiIotrCvfgi6avf9Y/jH/+vX5J6+jjhkdfO9iL+ggiOq7iP/+n+1v8yNUXRhHYblOCI/+hEnukUFXwRHwy4ZF8pE2EgX7I63SXj+PtV//0zNJ0YzT//LjNkXoYIGg4sIPf9JGhtNQkaGwrRnzj1/S71fvlvSTr+CI/Fe3WvVe97/y0f/3vQ4+o8O316/JuFrXhFj4//6H79Lr/+//S3vrrc2gy3/ra1hFP2wRTvsKEUOGR187gi6Ti+NtK1tioi+6C9suE7CYhNNQRdBhV8RiI0IiIiP+ZJJf/r/u/2gwRHX4oJflDgi6SC/Ecf+//+/mQKi6IWcp/WhoER714Ij7rx/H/rf4/XkXX95knI2ZHZgQ4RgICBoP/RnbGix6M7dGhsKUOW/9JuTHpOvLHr/DI6+VAIj/0P/j3xH+ENb/i//+P/618tIVC1//8ud/DNgf7X///sO/V6//8iqMIwjCMIwjCMIny6MIwjCMIwjCMIvl0YRhGEYRhGEYRhGEXRhGEYR9E6MIwjCPovnkYRhFOjCMIg0QaMIoRBo+jCPowiTRhHRGEfROjCMIvmEeRjL5Hy+Xy8R8vl8vkfI4y+R4j5jLovl8vl0YRhGEYRfL5fLoxl8vl8j5hF4jxHyOyPEfI8R8jxcCCOGUXIjsjojsjhCPEcZHCke2zr39//Zhb1nQCWC+6yhwRHXyh7aWxbBq6Sb9mDQZtjCX9giOmISDZbhNWxWO6PAbCCO4YSCvxFiIiIiP79PH/+tL//ivH/+vObFXqtII8b5T+r9JfPr1uCI/pfVrf8VpL3aUfehCYhw2K/tAmZwmgwv8RER/oMt/zjwihwyOvqEXUR/Ef9V/ft/q39MER0/KHwl/DCI+EgvxFR//X/6+D/BEfBFP+OPv/////5Q5cfneMP+kER0EP0I/+UOveL//2ER/7RQ4QWvlDgiOnd6su8gw6H8R5apAX/IEwzQ9fSw3/hL/9BeVXlP7wgvMGbMxd/wRdJmFqjw2mkHH8cQ+lvUED/2WOCI6in3wn/iIzabq//Cuq/+EZ96JOd2GXH53CI6IML1sV7oP+gv6/XofoRidqf6tf/+d01YvvGUOn3TSLKTUIj7PC44v8gV9iIjT3+QemmmmsGVwtBgiP/6BEez5kdmGfM2ZHMwZHaDztCJajUgSYZXCDOGERHIXYmFXSOP6eUO2IsIER3tsXWqqrYYTSNCDNYar7ZcrS15Ny4dO7y4bSBEeUl0UJVYQYe1Qbww0P4tj6evW//rNIjmYM8M5G2YZHNAyhhMEDQNBnBiD+r763TWt///6toSnBl5Ht7xKHsFZeR7bBMII1ucd+37f+Iw////rYTTatvsJva0m/q8P9j/9xf//+t/03/+3/q5XrP/Ye3Kf9I/f///f9df/1b///Yf/8EXRBh6v///j/////9v/2/v+PX/etfyOk//f///+/8N/H+QJrd+2kxarHKn/v////q9L30v8JQ01TTYqTHCvaVv9//6//WI/Np//0EIiIiIiI//1f/e2l97X76X+klf7a6//17002tr8oct79hBJ3rtpRaqwaq2kwcG2kZKGQeGFpX4QXDI6XncER0iFZlVUHaDYhMtwu2rsMVCYQYoEGFBgmKikKiN9IJdiIiIiIiIiIiKNwYdoMIhw/sRik34jv3//0k3j/km/LxHPMGYr/CD+PaalOCI+vo1vtmh+vCr5Q4RHSCfqvfigX4lDj2//uo/j/791//LNML6+6/21LNnpkNGozQzQzUZo20v2P/tIJggYKgRfR4aPDR4YQIvf/yn/sUHRnKeqTaTaVpWk22lftf8IjoMKq3D1a2rq9OwwS9Pj/xv/V9+0r8GIJBEfdAi6r+PtR++6/WxEREf/IRGaLMszjLs+zNmZnxI/61/9f88jRLaOO9oI8BhAiO0cdozp/1/78yFzkx/8ML6QbekuE6QbSdvS/73+pQ7L/7aXpba7q2l1bf/7/9cMj/zuE2l8cUkMcauw///jxiK9IKR0xTaUtwRH2tFwCkxwRH9J+/+uYf78RhSPwYWEIt9DBCO8p3v/r1/7obFa+v/+////gyoBAwrgiPXv+/1dP6//YiOfS3hFDsEU7BHH5DS/+t771a9Wkk7aTraVp+ufnV96Od1aynBEf/IFMi4m44qKbThul7dXVpK6vQoJL5Q4RHR4Kc02gmmwmg0xTEQgg7imKYrYpigh/EfoER8REGEDBCDKCBCHBghBhCGCDC/yG+mxEehER/wlpYr/oJj0px/4Sqhr/QQj/wRdJl+P44v/DK+eCf4in/jJj386Uodl/7C7DI+vO4IjoEj1EU+hWEpG20r7Gi4gyK21itX4N/7H/dA2CCv8GwQKU4Ijr7B4LQS/BEfFg4oL8UDBx/n2R2DD+9C1/udzPPizj/rpwiTbS1/12ykwa/qER0hGMfcbX/KTFd6x/ve+UOXHqcdbFr0sfxZGqlrv+h/zyPV93pzj+9Ud54WoIj/yh7q2g9BJfFXWQ+KCf3Htx/tV/0I9f5agsiNsicRWnH/lRlzX1r+ER0eECHbu/v/DNEmVax4v4zrjr9fto49f/vId2fZhoHkcz5HsvEdmwhwz4v1WC07htgolDtHsMIRz2gjw0eP+oQS3/CdLDM5Ua6bSeUOCLr53CLo4FCVP/6t0OKS6+xa9BegicV/9/9Y/jeohj8X/r/pTn9d4Y1//766/7Buf3/v9a9/7gw9///6sIof2tpB0r/1br0gih6XynCI6/Hdtf/vVKGFv6X5Q4RH1Ti2D7bSW4YVimIeggu/Zd5Bo2002W4VJqwaaxsJk3CiI/iKWxERERERERH/0CL/99Z2IEJWdftpcHyh/1HsnAb7/tU7+r8Ij4j3xZKkvH5Bo/5jPxHR/NM8jnX19Ecy5lzCYQcWhYQMINA08fyXCau7Fozu0Z3PDRnbHf7fXyY6S5cVdNpW5Mf9ZA5CF9NL+62P0Mpy3/O6eTP8cfa2HHllNUTYXXH3QQW//73BAuWRbQQZ/K5YjsSETj+IP//wix4bghyyEquVDQRdJo9+v1/79Lf710mU5S4tV2v6IQJ//+Xw9ginlkUr/wgh7Ow1Tjfbpeu2X/19jwggW7I6rhqYSrb/piEHF2x2sRFUnVsQgr21Syh50RJEvlaU44RHX+yY4SYYS2y3LHKHQYU46BFQOCBRFj6NpWwQM8R4jbMGYYQaBwxNx4ZHy7I5niNwTqEvyhwRHQiIiIiIiIilBBBYjqvo8Qjw0ew2w8Sh2GCbVGgdl5FW0PSC/Zd5A2kECfGKCpNpNq6W1tPsN6hNNrss2ExH8RhLCViQIv6p1b+vbdf1uv7IQYIJpBSnl6rUVH1/UN7v8b/0gRH3wkLSCI+h/7f9t9/zH/6C4YQrV7/6/ab9f3p/YwRTxYQJ6PP//+dE9sV/yn/CI+HiEXQsf4RQ7//7eoX59emSpdVQMM7A40QRToYRjSu0nBFQ6v/0tHI7a8f8YbCISA0m1T3bCwYX9fu9YP4aS494o18gi3FAi6W2KYptKLW4cMJXISKsVdLV76oMUtBhNoNMGy3BWrTBAhCTu7aTv6IRK7Sg3WIiIiIiIiM/xERlDlj+7RLY1cVDKWEk+xBEf+dwRdAyLtIDEuZsG5sVBkcy5lwhghHSiP6C5MHDdGgm9GjCd3Rncf8R7etS43b/Jj12FX5UC3/3/9D2DKN6fcIlsTf+rS/8YnYUitIg0YR9EyR9EXROjiMIjq/0g+/uH/5IdFploZRHyOyPEcG5HDIEI4ahHZHAxEf6sM7AkH7S8Isf/whkxxSHJD5BBzDkMtyDcciDkNJxIV0gQITOJQ4Ij8pwiOvoGDt9ivS//ERERERERERER0Evyhwi6Bv9GP//84ooL9lxhb9fVcEceuwwlH8YIFaVyFesXFRdsf+ECil/DLdNBmcEnCIGBoGv/sEItiIiIiIrgin+KN1kDwbj5T/quhH/+yOljH7DEQvrH7yzrAzsf/4IurLr74LiyzgRmxyhyb/nH2spnFMHsf0wRHTFDpx/hC2r/4xI6OxyWSfX5Z1kqdD/cWoq/zskCEqMsus6BX6sNwRxwiPtwpTgiOv5OA+tyZYyx1oJflDgiOv/Wi65MfEfxG78dKpH/9++z6Po8j/l80jPLo9HkYmh6oQwoX+tEc5NxYIOIYIMIYTCFibTGoj9Vq7tot3c8NGijO3LITGECS/3S+THVcuKfuvLeGmxlP64hfQr/8Me8WmHevCLpEz/x8NLDj6NokiQIuo/0v/+NtE50k4x+4//9Fjw2R3iEER8P/b//pYIqPq7f3/9/+Hsx8ECDf+l12y//3TpYiR8gqGUOTf847Ti4bFJpxccUmkR1cQyOvpgi6sMzgkmF7LdSnQaZUUmUUndx/hVEREREREREUgUjpBAhCI//EdNIMJNPX0IwRfXG/bTEEeGGUzBv+V0sDQGxBEfN5EJBEdON+l4qXvZUOU4Ijr5Q+Tc45nKHLHM5GOU5Y5rPBTnc7lQVBhyvKc0FYVBUFOUOVh3OOccqCnO5UFYcclBNzuYc7ncw5UFQU5hzjkbmHJDncw53JDkKOdzjmc45hzDmc7lOeChzOccw5hzDmcqCoKHMOZzucc8FQU5hzudzucc7nc45nO53KgqCoKdMNBAsIvl4ECv30Ev7CLoRERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERERDoe6CLq42UzCkR/EZka5CsZT48WIIj//R2LB0yJEgx0d0GbLj9bJwHkQm2h8IqLj9W/q20Paj/7YaXUQ1Kf9eK3jr8IjryOyOyOZcyOaDlDvNjNmRzI5nij7i9KLI7YutvRobEWjO/v6kx/Jva2+7lwW7X/qK2uv2P/f/+TpH/9pf4v/qv/cP/zjlv+cd//7hFj/5bYS1DI6+mCI/v/7pf/Ef4QrT/9///4//y63/+zHX2n93GnxaqrFX7Saaaq0m7Lfd21vqIiIiIiIiI2vzjgiOvlDukEr1hgiOoj+Iv+te/63+UPXzskIqM6maHH3CI+jDvd9+KSdb/++///Wv+4qycU9Zhl42ZwjbI6/qEeWoQ4tAiPIztAghynLf878Jo49Fvlj3SbSdXiP6QSdVaWk8t0vfuWspL+INGHdK//i3wxhiP9JP1e/6G3DJMMkGojhkjpf4pD1/l0bcd+6bV//thv3090fv/tgirgiojKHCI/9WwyOtvv/CCt8Nb0tShwiOmI/bW14/h4pK/ZdsrYURHhNNi2IabUVFIfxGGCYQYQMEzHMECM+VCXqhERERERfvX/7uU/6j+CLr+P/////nHLf87hEdcMjr6C8R/Ef/X9+9+sER/rnHVfKHCI/Ef2XdfcR/////8p/wiOo//+P//3/X+U5b/nfof0EE/xBqEF1U46/2L/fH1RQ4RH+8WEl8oeEP7Lsjr+I17///LXCIk0fR9FOjCPowidGEYRhGEYRhGEYRdGEYRhFCJ0QiMIoRhGEeRhGEYRfL5fLowjCL5HiPFwcjgqFx/ZE0YR0zCLowj6Loxl0YzmbMjkXIjkRyLkRw5ci5EczBEcEI4ZyORgOXGRwpgyODcjhpEcZcyORciOHLhoLkRwcjjI4UuMjhy5FwpcNBcZHDSLhsI5FwpHFI4cuMjg1lyI4yORciOHLghHMjikcMsjkRwcjkRxkcNIuMjghHDQRwcjkRxkcORwcjkXBtI4yOGkRwcjkXIuGwjhyORciODkcORwcjkRxkcORxlwqJODBAyORcKXJEO4MIRaEREGERjojkCBkcOXJEhwmGxaERoMIGhCCFsk+Scm5Ick5J0vyOCGwcm6CIIO0IgwhoMJhB/XVSQ96VKTdSTk3ty31JvTI5lzLlJuTck5J06TU46kh7JuW62XAIEqlxqXC5N1JOW+pNybknLvJDk3CqTe1JxqTcm5IcnHapKTdScBbUm5NyTqFJOqqWOU5bknJvqTdSQ5N1JOqqTeyQ+pxzuTckOScmP5NybknJD2SH+iQ5J0iQ/qSHUkPZIfVSbsjkiQ7Va0SHeuiQ9k319SQ/S+yORHK4Ij++mRz/pV+Gybkh18m65Ie6+rd9r32Ry1WlTi9VTe41v0HFpofcQyOWmh6acWnSoN0GRwRVpNOLtVTYtDVatBkc0L1TiIjT04MjmhehpxF63FwyOaF63ERGnxFkcNGtxER3ERFxEXERHxFkcNOIiIuIiOO4iI4iIiIiIiIiP9OLiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiI+oiP5DJDXHIHhuOQ2xyEHIbQ5A3HIQcgeG45A8KOQg5CDkDw2xyEHIHhRyGwchByEHIaByGwcgeG45DaHIbY5CDkIOQPDcchByDSOQg5A3HIbByGwchtDkIOQahyEHIQchtDkNg5DYP+QyAYHIHhuOJBpHIUdkcNhHDYRw2iOG0Rw2EcNBHEI4UjgeiOKRwpHDYRw2EcUjhSOGwjhsI4aC4aCOG0Rw2yOGwjhsI4aCOGwjhsI4bCOGgjikcORw0EcNhHFI4cjipkcUjhoI4cjikcORxUQo5FHZHFRDjkUchxyKOyOKRw5HFI4cjhoI4pHDkcVEOORR2RxUyOGguMjhyOKRw5HDYRxSOFL/yGqORRyHHIaByKOQ45FHIceyMch3IxyHHIo7I4yOHI4pHDkceyMfIo+RR8ijkOORj2RjkOOyOMjh5FHZHGrIxyHHIo5DjkY/2Rj5FH9kcZHDyKPkUdyMfIo5DjsjjI4cjikcPIo+RR9kcZHDyKP5GPjkY+RR//Ehx/8b/x0vxUjH///9fIx16CI/87kOP+Rj/j///j0+yOPfaX/kY/3/+v6T9+l9+P2lkY/sjjj2lZIf9LIx/v2RwzEyOSskP7I5LIx3SZHJWSHvZHDYRyI4IxZHKyOSZHJMjkrdIMjkmRw2EcwhEfxskOyOSZHOyOSZHIjg5HDQ0m0mRyTI5pkciODkcNDStJ0nFkc02kyOGwjmnSZHNNpMjmmRzTI4aGk6TI5WRzTpMjhsI4aIsjhojiQQcXFxEWRw2yOGiI4iLiIiLI4FwQiLnHETjiInHETjiCI4bAQiCBCIIELnHEECE44QQncTuTsTubj4IIISg19zjiIiCBCIIEJEHCCE7idyhz4J3Lw+AghK4rjQJ8PYncT2fBKgSuK4zs+ghO5uPhoEriuNDRoZ9I1lBiVZ+K40M+ghKg3lYJUCVxXGhn0EJUCVx8NDPoIQQR/KwSoEryuNB+K40CVx8NDPpGdn0EJUCVxXGhn0jQfQ8H0NBwxKs4ZoP5XGg+hoPoaytDjDWVocQfCtD2VoaytDjDWVocuUIOIPZWpWFaHEBEukES+RCBIl0RCJdEuiXQiRiEREREREfZxUorCEQ0IiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIj4iI///3///////9dBBeP/33////////0WYoR2BhCGIdA50CFOZ1BCGHJkDhBdFvqiOIV7FJwhrDkgOUBg6mRBnUISsIERSgnqQSt2oQd6lOITgiZGDHwhDKAQpzOgOSATCBra350BP/7T/T0SHMO/yuZCpoNU0ygyYLW6+5Meg2/yriMpvSMi835vvNZQ+CE5f8ulEnKHuiEHaap8ujm9TDC6aqE6kRFFvN/99sP/lQ8X8/GP/QTwQnrmIzIfkbPQT/VYcPXMhunv/61/+6/6/ZXU8xe34f0nzXmL/b/9v33td//f/////u//+v+//r//x///9f//r/////v//2u//3/9hf/v//7////9ev//j0P/X/WP9en//6/+v//fv///YW//9P/KeC8V+hwyOl4a7+//////6+FsupFH/+SAnxoWSAt/oNCMpwkevj/+v6X//+lEjoNdfh+nUMIP1TQahB//1sodb1/v/tP8Fvabf3phBzOUnyQ5V/CITyXUSt1f/94j99+v+PrRJsjCCITUhDkMfyMLCITyPOJUFQ/3vg8zlIJf////CIT///+73f/9P///+P//1/X/+cOv////PFf1////X++/X//1/OC//WjH1+n/+///1/69e1//x//+t/+//36/1//7T//f//3//9f+r/6/39//fiP9a39/vW19e9//j/v/pev98bhCN1/9qU5U0Pa/1t9//yEDxsfHFcU12OwS426cR2PNYJ+hwwkQwT4/2vuQn2u0x7Uftf9Ox/X9MIWmvHtMLaa2E09NBnHTmsbTQi0IiIi47ERIaI+GFiIiIiIgyhzjghEeIiGEIjsf////+v/Cf//h///////Wv9/f//3ymQgvIksujCI8R4uCUypDZTSLSnM5xzDmHIxzDmc45hzOeCnPBTnc45Q5TngpzudzudyoO5RBQ53O53Px3O5hzuYc7nc7ncw5hzDmHMOdzDncw53MOdzDncg45hyGccjHM5xzOccw5nIUcw53MOQwOQwOQo4iIjiQPDLHIHgpDkDUhxERERERERERERERERERERERERERERERERERERERERERERERERgAgAgADgAAAQMAAQAAAEQGAAABAQMAAQAAALUCAAACAQMAAQAAAAEAAAADAQMAAQAAAAQAAAAGAQMAAQAAAAAAAAAKAQMAAQAAAAEAAAARAQQAAQAAAAgAAAAVAQMAAQAAAAEAAAAWAQMAAQAAALUCAAAXAQQAAQAAABcuAAAaAQUAAQAAAM4uAAAbAQUAAQAAANYuAAAcAQMAAQAAAAEAAAAoAQMAAQAAAAIAAAAAAAAAAAAAGQAAIAAAAAAZAAAgAA=="),
					Format = "TIFF"
				}
			},
			new CXEData.Transactions.Stage.Check(){ 
				Id = 1000000003,
				Amount = 100,
				CheckType = 1,
				Fee = 1,
				Status = 1,
				rowguid = Guid.NewGuid(),
				IssueDate = DateTime.Now,
				MICR = "o001003ot075911603t182380188280o",
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Images = new CXEData.CheckImages()
				{ 
					id = Guid.NewGuid(), 
					DTServerCreate = DateTime.Now,
					Back = new byte[100],
					Front = new byte[100],
					Format = "TIFF"
				}
			}
		};

		internal List<CXEData.Transactions.Commit.Check> commitChecks = new List<CXEData.Transactions.Commit.Check>() 
		{
			new CXEData.Transactions.Commit.Check(){ 
				Id = 1000000001,
				Amount = 100,
				CheckType = 1,
				Fee = 1,
				Status = 1,
				rowguid = Guid.NewGuid(),
				IssueDate = DateTime.Now,
				MICR = "o001003ot075911603t182380188280o",
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now
			}
		};

		internal List<CheckTrx> checkTrxns = new List<CheckTrx>() 
		{
			new CheckTrx(){ Id = 1000000000, Amount = 100, CheckNumber = "123456", ConfirmationNumber = "1594815926", Status = CheckStatus.Approved },
			new CheckTrx(){ Id = 2000000000, Amount = 100, CheckNumber = "123456", ConfirmationNumber = "1594815926", Status = CheckStatus.Approved },
			new CheckTrx(){ Id = 1000000002, Amount = 100, CheckNumber = "123456", ConfirmationNumber = "1594815926", Status = CheckStatus.Pending },
			new CheckTrx(){ Id = 1000000001, Amount = 100, CheckNumber = "123456", ConfirmationNumber = "1594815926", Status = CheckStatus.Approved },
			new CheckTrx(){ Id = 1000000003, Amount = 100, CheckNumber = "123456", ConfirmationNumber = "1594815926", Status = CheckStatus.Declined },
			new CheckTrx(){ Id = 1000000000000001, Amount = 100, CheckNumber = "123456", ConfirmationNumber = "1594815926", Status = CheckStatus.Declined },
			new CheckTrx(){ Id = 1000000004, Amount = 100, CheckNumber = "123456", ConfirmationNumber = "1594815926", Status = CheckStatus.Declined },
		};

		internal List<CheckAccount> checkAccounts = new List<CheckAccount>() 
		{
 			new CheckAccount()
			{
				Address1 = "test", 
				Address2 = "test2", 
				City = "Testing",
				State = "CA",
				DateOfBirth = new DateTime(1990,10,10),
				Phone = "6985478569",
				Zip = "12345",
				FirstName = "Nitish",
				LastName = "Biradar",
				Id = 1000000000,
				IDCountry = "US",
				IDExpireDate = new DateTime(2020, 10,10),
				IDIssueDate = new DateTime(2005, 10, 10),
				IDState = "CA",
				IDType = "Passport",
				Occupation = "Student",
				SecondLastName = string.Empty,
				SSN = "124578963",
				IDCode = "S",
				GovernmentId =  "784596321"
			}
		};

		internal List<CheckLogin> checkLogins = new List<CheckLogin>() 
		{
			new CheckLogin(){ BranchId = 100, CompanyToken = "", EmployeeId = 100, URL = "" }
		};
		#endregion

		#region MasterCountry Collection

		internal List<PTNRData.MasterCountry> masterCountrys = new List<PTNRData.MasterCountry>()
		{
			new PTNRData.MasterCountry(){ Abbr2 = "US", Abbr3 = "USA", DTServerCreate = DateTime.Now, Id = 1000000228, Name = "United States", Rowguid = Guid.NewGuid()}
		};

		internal List<string> idCountries = new List<string>()
		{
			"US", "MX"
		};

		internal List<string> states = new List<string>()
		{
			"CA", "WU", "MG"
		};

		internal List<string> phoneTypes = new List<string>()
		{
			"PhoneType1", "PhoneType2"
		};

		internal List<string> mobileProviders = new List<string>()
		{
			"Prov1", "Prov2"
		};

		internal List<string> idTypes = new List<string>()
		{
			"DL", "PP"
		};

		internal List<LegalCode> legalCodes = new List<LegalCode>()
		{
			new LegalCode() { Code = "1", Name = "Code1"},
			new LegalCode() { Code = "2", Name = "Code2"}
		};

		internal static List<MasterCatalog> masterCatalogs = new List<MasterCatalog>() 
		{
			new MasterCatalog(){ 
				rowguid = Guid.Parse("81082EF8-52AB-4DE2-820A-A26DDFF4D3ED"),
				Id = 100015530,
				ProviderCatalogId = 1000005931,
				BillerName = "REGIONAL ACCEPTANCE",
				ProviderId = 401,
				BillerCode = "14563",
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				IsActive = true,
				ChannelPartnerId = 34,
			},
			new MasterCatalog(){ 
				rowguid = Guid.Parse("81082EF8-52AB-4DE2-820A-A26DDFF4D3ED"),
				Id = 14563,
				ProviderCatalogId = 1000005931,
				BillerName = "REGIONAL ACCEPTANCE",
				ProviderId = 401,
				BillerCode = "14563",
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				IsActive = true,
				ChannelPartnerId = 33,
			}
		};

		internal List<PartnerCatalog> partnerCatalogs = new List<PartnerCatalog>() 
		{
			new PartnerCatalog(){
				rowguid = Guid.NewGuid(),
				PartnerCatalogPK = Guid.NewGuid(),
				Id = 100015530,
				BillerName = "REGIONAL ACCEPTANCE",
				ProviderId = 401,
				BillerCode = "14563",
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				ChannelPartnerId = 34,
				MasterCatalog = masterCatalogs.FirstOrDefault()
			}
		};

		#endregion

		#region Occupations
		internal List<PTNRData.Occupation> occupations = new List<PTNRData.Occupation>() 
		{
 			new PTNRData.Occupation(){ Code = "00001", DTServerCreate = DateTime.Now, DTTerminalCreate = DateTime.Now, Id = 100000000, IsActive = true, Name = "Student", rowguid = Guid.NewGuid()},
			new PTNRData.Occupation(){ Code = "00002", DTServerCreate = DateTime.Now, DTTerminalCreate = DateTime.Now, Id = 100000001, IsActive = true, Name = "Student2", rowguid = Guid.NewGuid()},
			new PTNRData.Occupation(){ Code = "00003", DTServerCreate = DateTime.Now, DTTerminalCreate = DateTime.Now, Id = 100000002, IsActive = true, Name = "Student3", rowguid = Guid.NewGuid()},
			new PTNRData.Occupation(){ Code = "00004", DTServerCreate = DateTime.Now, DTTerminalCreate = DateTime.Now, Id = 100000003, IsActive = true, Name = "Student4", rowguid = Guid.NewGuid()},
		};
		#endregion

		#region BillPayAccounts
		internal List<CXNBillpayData.BillPayAccount> billPayAccounts = new List<CXNBillpayData.BillPayAccount>() {
			new CXNBillpayData.BillPayAccount(){ 
				Address1 = "Test",
				Address2 = "Test2",
				City = "City",
				ContactPhone = "9874563215",
				DateOfBirth = new DateTime(1990,01,01),
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				FirstName = "Nitish",
				LastName = "Biradar",
				Id = 1000000000000000,
				rowguid = Guid.NewGuid(),
				State = "CA",
				PostalCode = "900005",
				Street = "Testing Street",
				CardNumber = "1458545664"
			},
			new CXNBillpayData.BillPayAccount(){ 
				Address1 = "Test",
				Address2 = "Test2",
				City = "City",
				ContactPhone = "9874563215",
				DateOfBirth = new DateTime(1990,01,01),
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				FirstName = "Nitish",
				LastName = "Biradar",
				Id = 1000000000000001,
				rowguid = Guid.NewGuid(),
				State = "CA",
				PostalCode = "900005",
				Street = "Testing Street",
				CardNumber = "1458545554",
			}
		};
		#endregion

		#region CustomerFeeAdjustment

		internal List<PTNRData.Fees.CustomerFeeAdjustments> customerFeeAdjustments = new List<PTNRData.Fees.CustomerFeeAdjustments>(){
			new PTNRData.Fees.CustomerFeeAdjustments(){
				CustomerID = 100000002,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 100000000,
				rowguid = Guid.NewGuid(),
				IsAvailed = false,
				feeAdjustment = new PTNRData.Fees.FeeAdjustment(){
					Id = 100000000,
					rowguid = Guid.NewGuid(),
					TransactionType = PTNRData.Fees.FeeAdjustmentTransactionType.Check,
					Name = "Check"
				}
			},
			new PTNRData.Fees.CustomerFeeAdjustments(){
				CustomerID = 100000002,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 100000001,
				rowguid = Guid.NewGuid(),
				IsAvailed = false,
				feeAdjustment = new PTNRData.Fees.FeeAdjustment(){
					Id = 100000001,
					rowguid = Guid.NewGuid(),
					TransactionType = PTNRData.Fees.FeeAdjustmentTransactionType.FundsCredit,
					Name = "Fund Credit"
				}
			},
			new PTNRData.Fees.CustomerFeeAdjustments(){
				CustomerID = 100000002,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Id = 100000002,
				rowguid = Guid.NewGuid(),
				IsAvailed = true,
				feeAdjustment = new PTNRData.Fees.FeeAdjustment(){
					Id = 100000002,
					rowguid = Guid.NewGuid(),
					TransactionType = PTNRData.Fees.FeeAdjustmentTransactionType.MoneyOrder,
					Name = "Money Order"
				}
			}
		};

		internal List<PTNRData.Fees.FeeAdjustment> feeAdjustments = new List<PTNRData.Fees.FeeAdjustment>() 
		{
			new PTNRData.Fees.FeeAdjustment()
			{
				channelPartner = channelPartners.Find(a=>a.Id == 33),
				Id = 100000003,
				TransactionType = PTNRData.Fees.FeeAdjustmentTransactionType.Check,
				rowguid = Guid.NewGuid()
			},
			new PTNRData.Fees.FeeAdjustment()
			{
				channelPartner = channelPartners.Find(a=>a.Id == 33),
				Id = 100000010,
				TransactionType = PTNRData.Fees.FeeAdjustmentTransactionType.Check,
				PromotionType = "Referral",
				rowguid = Guid.NewGuid()
			}
		};
		#endregion

		#region Field and Customer Prefered Product
		internal List<CXNBillpayData.Field> fields = new List<CXNBillpayData.Field>()
			{
				new CXNBillpayData.Field() { DataType = "string", IsMandatory = false, MaxLength = 100, Label = "Field1"},
				new CXNBillpayData.Field() { DataType = "string", IsMandatory = true, MaxLength = 70, Label = "Field2"}

			};

		internal List<CXEData.CustomerPreferedProduct> customerPreferdProducts = new List<CXEData.CustomerPreferedProduct>() {
			new CXEData.CustomerPreferedProduct(){ }
		};
		#endregion

		#region Fund Engine Data Collections
		internal List<CXEData.Transactions.Stage.Funds> stageFunds = new List<CXEData.Transactions.Stage.Funds>() 
		{
 			new CXEData.Transactions.Stage.Funds()
			{
				rowguid = Guid.NewGuid(),
				Id = 1000000000,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Status = 1,
				Fee = 1M,
				Type = 1,
				Amount = 100,
				Account = new CXEData.Account()
				{
					rowguid = Guid.NewGuid(),
					Id = 1000000000,
					Type = 1,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					Customer = coreCxeCustomers.FirstOrDefault()
				}
			}
		};

		internal List<CXEData.Transactions.Commit.Funds> commitFunds = new List<CXEData.Transactions.Commit.Funds>() 
		{
			new CXEData.Transactions.Commit.Funds()
 			{
				rowguid = Guid.NewGuid(),
				Id = 1000000000,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				Status = 1,
				Fee = 1M,
				Type = 1,
				Amount = 100,
				Account = new CXEData.Account()
				{
					rowguid = Guid.NewGuid(),
					Id = 1000000000,
					Type = 1,
					DTServerCreate = DateTime.Now,
					DTTerminalCreate = DateTime.Now,
					Customer = coreCxeCustomers.FirstOrDefault()
				}
			}
		};

		internal static List<CXNFundData.CardAccount> cardAccounts = new List<CXNFundData.CardAccount>() 
		{
			new CXNFundData.CardAccount()
			{
				AccountNumber = "5915915915",
				Address1 = "Test",
				Address2 = "Test2",
				City = "City",
				Phone = "9874563215",
				DateOfBirth = new DateTime(1990,01,01),
				FirstName = "Nitish",
				LastName = "Biradar",
				Id = 1000000000000001,
				State = "CA",
				ZipCode = "900005",
				CardNumber = "1458545554",
			}
		};

		internal List<CXNFundData.FundTrx> fundTrxns = new List<CXNFundData.FundTrx>() 
		{
			new CXNFundData.FundTrx()
			{
				Account = cardAccounts.FirstOrDefault(),
				CardBalance = 100,
				Fee = 5,
				TransactionID = "1000000000",
				TransactionAmount = 100,
				TransactionType = "Activation",
				PreviousCardBalance = 200,
			}
		};
		internal List<Cxn.Fund.Data.ShippingTypes> lstshippingTypes = new List<Cxn.Fund.Data.ShippingTypes>() 
		{
 			new Cxn.Fund.Data.ShippingTypes()
			{
				Code = "0",
				Name = "Express Service"
			}
		};


		#endregion

		#region Stage BillPay Collection
		internal List<Core.CXE.Data.Transactions.Stage.BillPay> stageBillPays = new List<CXEData.Transactions.Stage.BillPay>() 
		{
			new Core.CXE.Data.Transactions.Stage.BillPay(){ 
				Id = 1000000000,
				rowguid = Guid.NewGuid(),
				Fee = 5,
				Amount = 100,
				AccountNumber = "1234561830",
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				ProductName = "REGIONAL ACCEPTANCE",
				ProductId = 101,
				ConfirmationNumber = "2656665"
			}
		};
		#endregion

		#region Money Order Service
		internal List<CXEData.Transactions.Commit.MoneyOrder> commitMoneyOrders = new List<CXEData.Transactions.Commit.MoneyOrder>() { };
		internal List<CXEData.Transactions.Stage.MoneyOrder> stageMoneyOrders = new List<CXEData.Transactions.Stage.MoneyOrder>() 
		{
			new CXEData.Transactions.Stage.MoneyOrder()
			{
				Id = 1000000002,
				rowguid = Guid.NewGuid(),
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
			}
		};
		internal List<PTNRData.MoneyOrderImage> moneyOrderImages = new List<PTNRData.MoneyOrderImage>() 
		{
			new PTNRData.MoneyOrderImage(){ 
				CheckBackImage = new byte[10], 
				CheckFrontImage = new byte[10], 
				DTServerCreate = DateTime.Now, 
				DTTerminalCreate = DateTime.Now, 
				Id = 1000000000, 
				rowguid = Guid.Parse("EC6AAAE3-2BA7-4E0B-898E-D296DB432C17"), 
				TrxId = Guid.NewGuid()
			}
		};
		#endregion

		#region Money Transfer
		internal List<Cxn.MoneyTransfer.Data.Account> accounts = new List<Cxn.MoneyTransfer.Data.Account>() 
		{
			new Cxn.MoneyTransfer.Data.Account()
			{
				Address = "Test",
				City ="Test",
				Id = 1000000001,
				DTServerCreate = DateTime.Now,
				DTTerminalCreate = DateTime.Now,
				FirstName = "Nitish",
				LastName = "Biradar",
				MiddleName = string.Empty,
				MobilePhone = "1594872635",
				LoyaltyCardNumber = "159487623",
			}
		};

		internal List<MoneyTransferStage> stagedmoneyTransfers = new List<MoneyTransferStage>()
		{
			new MoneyTransferStage()
			{
			
			 Id=1000000001,
			 rowguid=new Guid(),
			 Amount=80,
			 Fee=12.0m,
			 Account=new CXEData.Account()
			 {
				 Customer=new CXEData.Customer()
				 {
					 FirstName="John",
					 LastName="Smith",
					 DateOfBirth=new DateTime(1985,01,01)
				 },
				 Type=1,
				 DTServerCreate = DateTime.Now 
			 },
			 Status=1,
			 ReceiverName="John",
			 Destination="MXN",
			 DestinationAmount=92.0m,
			 ConfirmationNumber="987676756",
			 DTServerCreate=DateTime.Now,
			 DTServerLastModified=DateTime.Now,
			 DTTerminalCreate=DateTime.Now,
			 DTTerminalLastModified=DateTime.Now

			}
		};

		internal List<MoneyTransferCommit> committedMoneyTransfers = new List<MoneyTransferCommit>()
		{
			new MoneyTransferCommit()
			{
			
			 Id=1000000001,
			 rowguid=new Guid(),
			 Amount=80,
			 Fee=12.0m,
			 Account=new CXEData.Account()
			 {
				 Customer=new CXEData.Customer()
				 {
					 FirstName="John",
					 LastName="Smith",
					 DateOfBirth=new DateTime(1985,01,01)
				 },
				 Type=1,
				 DTServerCreate = DateTime.Now 
			 },
			 Status=1,
			 ReceiverName="John",
			 Destination="MXN",
			 DestinationAmount=92.0m,
			 ConfirmationNumber="987676756",
			 DTServerCreate=DateTime.Now,
			 DTServerLastModified=DateTime.Now,
			 DTTerminalCreate=DateTime.Now,
			 DTTerminalLastModified=DateTime.Now

			}
		};

		internal List<DeliveryService> deliveryServices(string countryCode)
		{
			List<DeliveryService> serviceName = new List<DeliveryService>();

			switch (countryCode.ToUpper())
			{
				case "IN":
					serviceName = new List<DeliveryService>()
					{
						new DeliveryService(){ Code="000",Name="MONEY IN MINUTES"}
					};
					break;
				case "MX":
					serviceName = new List<DeliveryService>()
					{
						new DeliveryService(){Code ="400",Name="DINERO  EN MINUTOS/IN MINUTES"},
						new DeliveryService(){Code ="402",Name="DINERO DIA SIGUIENTE/NEXT DAY"},
						new DeliveryService(){Code ="102",Name="GIRO  PAISANO/IN MINUTES"},
						new DeliveryService(){Code ="101",Name="GIRO WITH NOTIFICATION"},
						new DeliveryService(){Code ="100",Name="GIRO WITHOUT NOTIFICATION"}
					};
					break;
				case "US":
					serviceName = new List<DeliveryService>()
					{
						new DeliveryService(){Code ="069",Name="MONEY IN MINUTES"},
						new DeliveryService(){Code ="070",Name="NEXT DAY DELIVERY SERVICE"}
					};
					break;
				default:
					serviceName = new List<DeliveryService>();
					break;
			}
			return serviceName;
		}

		internal FeeResponse feeResponse = new FeeResponse()
		{
			TransactionId = 1000000001,
			FeeInformations = new List<FeeInformation>()
			{
			   new FeeInformation()
			   {
				  Amount =100,
				  Fee =20,
				  TotalAmount =120
			   }
			}
		};

		internal ValidateResponse validateResponse = new ValidateResponse()
		{
			TransactionId = 1000000001,
			HasLPMTError = false
		};

		internal SearchResponse searchResponse = new SearchResponse()
		{
			FirstName = "John",
			LastName = "smith",
			ConfirmationNumber = "98765432",
			RefundTransactionId = 1000000032,
			TestQuestion = "Test Question",
			TestAnswer = "Test answer",
			TransactionId = 1000000001,
			TransactionStatus = "",
			FeeRefund = "0",
			CancelTransactionId = 100000034
		};


		internal List<Reason> refundReasons = new List<Reason>()
		{
			new Reason()
			{
				Code = "W9200",
				Name = "RCM - Customer Changed Mind"
			},

			new Reason()
			{
				Code = "W9201",
				Name = "RCF - Consumer Fraud Suspected"
			},

			new Reason()
			{
				Code = "W9202",
				Name = "RAE - Input Error"
			},
			new Reason()
			{
				Code = "W9203",
				Name = "RFD - Wrong Information"
			},
			new Reason()
			{
				Code = "W9204",
				Name = "RSE - System Error"
			},
			new Reason()
			{
				Code = "W9199",
				Name = "RNA - Money not available by date on receipt"
			},
			new Reason()
			{
				Code = "W9198",
				Name = "RWR - Wrong amount available to recipient"
			}
		};

		internal List<Transaction> transactions = new List<Transaction>()
		{
		   new Transaction()
		   {
			 Account=new CXNAccount(),
			 AmountToReceiver=100,
			 ConfirmationNumber="9876543",
			 DestinationCountryCode="MX",
			 DeliveryServiceName="10 minute service",
			 Fee=12m,
			 ReceiverFirstName="john",
			 ReceiverLastName="smith",
			 SenderName="Synovus Fname",
			 TaxAmount=0m,
			 StateTax=0m,
			 TransactionID="1000000001",
			 TransactionType="1",
			 TransactionAmount=80,
			 Receiver= new Receiver()
			 {

			 },
			 MetaData = new Dictionary<string,object>(),
		   }		
		};

		internal List<Receiver> receivers = new List<Receiver>()
		{
		  new Receiver()
		  {			
            FirstName = "TestFirst",
            LastName = "TestLast",
			Id = 1000000000,
            PickupCity = "Michigan",
            PickupCountry = "United States",
           						
		  },
		  new Receiver()
		  {
			  FirstName="John",
			  LastName="smith",
			  Occupation="Engineer",
			  Id = 1000000001,
              PickupCity = "California",
              PickupCountry = "United States",
			  Status = "Active"
		  }
		};


		internal List<PTNRData.LocationCounterId> locationCounterIds = new List<PTNRData.LocationCounterId>()
		{
			new PTNRData.LocationCounterId()
			{
			 CounterId="13139925",
			 Id=1,
			 rowguid=new Guid(),
			 //Location=new Location(),
			 ProviderId=401,
			 LocationId=new Guid(),
			 IsAvailable=true,
			 DTServerCreate=DateTime.Now,
			 DTServerLastModified=DateTime.Now,
			 DTTerminalCreate=DateTime.Now,
			 DTTerminalLastModified=DateTime.Now

			}
		};
		#endregion

		#region Core partner transaction history
		internal List<PTNRData.TransactionHistory> transactionHistory = new List<PTNRData.TransactionHistory>()
		{
			new PTNRData.TransactionHistory()
			{
				CustomerId = 1000000000,
				rowguid = Guid.NewGuid(),
				CustomerName = "Nitish",
				Location = "Test",
				SessionId = 1000000000,
				Teller = "System Admin",
				TellerId = 500001,
				TransactionId = 1000000001,
				TotalAmount =100,
				TransactionDate = DateTime.Now,
				TransactionStatus = "Completed",
				TransactionType = "Check"
			}
		};
		#endregion


		#region Core partner nps terminal
		internal PTNRData.NpsTerminal terminal = new PTNRData.NpsTerminal()
		{
			rowguid = Guid.NewGuid(),
			Location = new PTNRData.Location() { Address1 = "TestAddr" },
			Description = "Testing"
		};


		internal List<PTNRData.NpsTerminal> npsTerminals = new List<PTNRData.NpsTerminal>()
		{
			new PTNRData.NpsTerminal()
			{
				rowguid = Guid.NewGuid(),
				Location = new PTNRData.Location() { Address1 = "TestAddr1"},
				Description = "Testing1"
			},

			new PTNRData.NpsTerminal()
			{
				rowguid = Guid.NewGuid(),
				Location = new PTNRData.Location() { Address1 = "TestAddr2"},
				Description = "Testing2"
			}
		};
		#endregion

		#region Core partner User
		internal PTNRData.UserDetails userInfo = new PTNRData.UserDetails()
		{
			rowguid = Guid.NewGuid(),
			ChannelPartnerId = 34
		};

		#endregion

		#region FISAccount
		internal List<FISAccount> fisAccounts = new List<FISAccount>() {
				new FISAccount()
				{
					FirstName = "John",
					MiddleName = "Smith",
					LastName = "Mariate",
					LastName2 = "",
					MothersMaidenName = "MotherSmith",
					DateOfBirth = new DateTime(1990, 05, 01),
					Address1 = "267 SCRUB OAK ",
					Address2 = "CIR",
					City = "BOULDER",
					State = "CO",
					ZipCode = "80305 ",
					Phone1 = "719-637-5029",
					SSN = "123456789",
					Gender = "Female",
					PartnerAccountNumber = "1503436995000",
					RelationshipAccountNumber = "RelationshipAccountNumber",
					ProfileStatus = ProfileStatus.Active,
					BankId = "0004",
					BranchId = "9004",
					Phone1Type = "",
					GovernmentIDType = "Driver'sLicense",
					GovernmentId = "D3210123",
					IDIssuingCountry = "united State",
					IDIssuingState = "CA",
					IDIssueDate = DateTime.MinValue,
					IDExpirationDate = DateTime.MaxValue,
					IDCode = "S"
				}			
			};
		internal List<FISAccount> fisAccountsMoq = new List<FISAccount>() {
				new FISAccount()
				{
					FirstName = "John",
					MiddleName = "Smith",
					LastName = "Mariate",
					LastName2 = "",
					MothersMaidenName = "MotherSmith",
					DateOfBirth = new DateTime(1990, 05, 01),
					Address1 = "267 SCRUB OAK ",
					Address2 = "CIR",
					City = "BOULDER",
					State = "CO",
					ZipCode = "80305 ",
					Phone1 = "719-637-5029",
					SSN = "123456789",
					Gender = "Female",
					PartnerAccountNumber = "1503436995000",
					RelationshipAccountNumber = "RelationshipAccountNumber",
					ProfileStatus = ProfileStatus.Active,
					BankId = "0004",
					BranchId = "9004",
					Phone1Type = "",
					GovernmentIDType = "Driver'sLicense",
					GovernmentId = "D3210123",
					IDIssuingCountry = "united State",
					IDIssuingState = "CA",
					IDIssueDate = DateTime.MinValue,
					IDExpirationDate = DateTime.MaxValue
				}			
			};

		#endregion

		internal List<CCISAccount> ccisAccount = new List<CCISAccount>() 
		{
			new CCISAccount(){ Address1 = "Testing", Address2 = string.Empty, BankId = "1234", BranchId= "12346", City = "Oakland", DateOfBirth = new DateTime(1990,10,10), }
		};
	}
}
