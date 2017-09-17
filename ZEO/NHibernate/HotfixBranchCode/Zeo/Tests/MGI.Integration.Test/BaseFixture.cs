using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Web.ServiceClient;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Integration.Test
{
	public class BaseFixture
	{
		#region Public member
		public Desktop Client { get; set; }
		public long AlloyId { get; set; }
		public string TerminalName { get; set; }
		public string ChannelPartnerId { get; set; }
		public string ChannelPartnerName { get; set; }
		public CustomerSession CustomerSession { get; set; }
		public AgentSession AgentSession { get; set; }
		public AgentSSO AgentSSO { get; set; }
		public string DeliveryService { set; get; }
		public MGI.Channel.DMS.Server.Data.MGIContext MgiContext { get; set; }
		#endregion

		#region Public Methods
		public long GetCustomerAlloyId(string agentSessionId, string lastName, DateTime dOB)
		{
			CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria() { LastName = lastName, DateOfBirth = dOB };

			CustomerSearchResult[] customers = Client.SearchCustomers(agentSessionId, searchCriteria, MgiContext);

			if (customers.Length != 0)
			{
				return long.Parse(customers[0].AlloyID);
			}
			else
			{
				Prospect customer = GetCustomerProspect(agentSessionId, lastName, dOB);

				AlloyId = long.Parse(Client.GeneratePAN(agentSessionId, customer, MgiContext));

				Client.SaveCustomerProfile(agentSessionId, AlloyId, customer, MgiContext, false);

				MgiContext = new Channel.DMS.Server.Data.MGIContext() { EditMode = false };

				Client.NexxoActivate(agentSessionId, AlloyId, MgiContext);

				Client.ClientActivate(agentSessionId, AlloyId, MgiContext);

				return AlloyId;
			}
		}

		public Prospect GetCustomerProspect(string agentSessionId, string lastName, DateTime dOB)
		{
			Prospect customer = new Prospect()
			{
				Address1 = "4723 Apple St.AnyTown",
				Address2 = "Street",
				City = "FL",
				FName = "John",
				MName = string.Empty,
				LName = lastName,
				LName2 = string.Empty,
				DateOfBirth = dOB,
				SSN = GetSSN(),
				Gender = "Male",
				Phone1 = "6874565654",
				Phone1Type = "Home",
				MailingAddressDifferent = false,
				PostalCode = "33716",
				PrimaryCountryCitizenShip = "US",
				ProfileStatus = ProfileStatus.Active,
				State = "CA",
				MoMaName="Eng",
				SecondaryCountryCitizenShip = "US",
				CustomerScreen = Common.Util.CustomerScreen.Identification,
				ID = new Identification()
				{
					CountryOfBirth = "US",
					Country = "UNITED STATES",
					ExpirationDate = new DateTime(2020, 10, 10),
					GovernmentId = "345678912",
					IDType = "PASSPORT",
					IDTypeName = "PASSPORT",
					IssueDate = new DateTime(2005, 10, 10),
					State = string.Empty
				},
				Occupation = "STUDENT",
				PIN = "1111",
				ChannelPartnerId = Client.GetChannelPartner(ChannelPartnerName, MgiContext).rowguid,
				ReferralCode = string.Empty
			};

			return customer;
		}

		public AgentSession GetAgentSession()
		{
			AgentSSO = new AgentSSO();
			AgentSSO.UserName = "systemadmin@moneygram.com";
			AgentSSO.Role = new UserRole();
			AgentSSO.Role.Id = 1;
			AgentSession = Client.AuthenticateSSO(AgentSSO, ChannelPartnerId, TerminalName, MgiContext);
			return AgentSession;
		}

		public void GetChannelPartnerDataCarver()
		{
			TerminalName = "Carver";
			ChannelPartnerName = "Carver";
			ChannelPartnerId = "28";
			CustomerSession = new CustomerSession();
			AgentSession = new AgentSession();
			MgiContext = new Channel.DMS.Server.Data.MGIContext();
			MgiContext.Context = new Dictionary<string, object>();
			AlloyId = 0;
			DeliveryService = "000";
		}

		public void GetChannelPartnerDataSynovus()
		{
			TerminalName = "Synovus";
			ChannelPartnerName = "Synovus";
			ChannelPartnerId = "33";
			CustomerSession = new CustomerSession();
			AgentSession = new AgentSession();
			MgiContext = new Channel.DMS.Server.Data.MGIContext();
			MgiContext.Context = new Dictionary<string, object>();
			AlloyId = 0;
			DeliveryService = "000";
		}
	
		public void GetChannelPartnerDataMGI()
		{
			TerminalName = "MGI";
			ChannelPartnerName = "MGI";
			ChannelPartnerId = "1";
			CustomerSession = new CustomerSession();
			MgiContext = new Channel.DMS.Server.Data.MGIContext();
			MgiContext.Context = new Dictionary<string, object>();
			AgentSession = new AgentSession();
			AlloyId = 0;
		}

		public void GetChannelPartnerDataRedstone()
		{
			TerminalName = "Redstone";
			ChannelPartnerName = "Redstone";
			ChannelPartnerId = "35";
			CustomerSession = new CustomerSession();
			MgiContext = new Channel.DMS.Server.Data.MGIContext();
			MgiContext.Context = new Dictionary<string, object>();
			AgentSession = new AgentSession();
			AlloyId = 0;
		}

		public void GetChannelPartnerDataTCF()
		{
			TerminalName = "TCF";
			ChannelPartnerName = "TCF";
			ChannelPartnerId = "34";
			CustomerSession = new CustomerSession();
			MgiContext = new Channel.DMS.Server.Data.MGIContext();
			MgiContext.Context = new Dictionary<string, object>();
			AgentSession = new AgentSession();
			AlloyId = 0;
		}

		public void CreateReceiver_WU(long customerSessionId)
		{
			Receiver receiver = new Receiver()
			{
				DeliveryMethod = "000",
				FirstName = "NEXXO",
				LastName = "WESTERN",
				//PickupCity = "COLIMA",
				PickupCountry = "US",
				PickupState_Province = "CA",
				Status = "Active"
			};
			Client.SaveReceiver(customerSessionId, receiver, MgiContext);
		}

		public void CreateReceiver_MGI(long customerSessionId)
		{
			Receiver receiver = new Receiver()
			{
				DeliveryMethod = "000",
				FirstName = "RECEIVERFNAME",
				LastName = "LNAME",
				//PickupCity = "COLIMA",
				PickupCountry = "USA",
				PickupState_Province = "AZ",
				SecurityQuestion="TEST QUESTION",
				SecurityAnswer="TEST ANSWER",
				Status = "Active"
			};
			Client.SaveReceiver(customerSessionId, receiver, MgiContext);
		}

		public void UpdateCounterId()
		{
			MgiContext.IsAvailable = true;
			Client.UpdateCounterId(long.Parse(CustomerSession.CustomerSessionId), MgiContext);
		}

		public decimal GetRandomAmount()
		{
			decimal amount = 20.00M;
			Random rnd = new Random();
			amount = (decimal)rnd.Next(20, 140);
			return amount;
		}

		#endregion

		#region GetRandomSSN

		protected virtual string GetSSN()
		{
			if (ChannelPartnerName == "Redstone")
			{
				return "226589994";
			}
			string ssn = string.Empty;
			Random random = new Random();
			for (int i = 0; i < 8; i++)
			{
				ssn += Convert.ToString(random.Next(0, 9));
			}
			return ssn;
		}

		#endregion
	}
}
