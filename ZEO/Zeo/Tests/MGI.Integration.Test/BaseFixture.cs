using System;
using System.Collections.Generic;
using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;

namespace MGI.Integration.Test
{
    public class BaseFixture
    {
        #region Public member
        public ZeoServiceClient Client { get; set; }
        public long AlloyId { get; set; }
        public string TerminalName { get; set; }
        public string ChannelPartnerId { get; set; }
        public string ChannelPartnerName { get; set; }
        public CustomerSession CustomerSession { get; set; }
        public AgentSession AgentSession { get; set; }
        public AgentSSO AgentSSO { get; set; }
        public string DeliveryService { set; get; }
        public ZeoContext zeoContext { get; set; }
        #endregion

        #region Public Methods
        public long GetCustomerAlloyId(long agentSessionId, string lastName, DateTime dOB)
        {
            CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria() { Lastname = lastName, DateOfBirth = dOB };

            Response customerResponse = Client.SearchCustomer(searchCriteria, zeoContext);
            //if (VerifyException(customerResponse)) throw new AlloyWebException(customerResponse.Error.Details); 
            List<CustomerProfile> customers = customerResponse.Result as List<CustomerProfile>;

            //if (customers.Length != 0)
            //{
            //	return long.Parse(customers[0].);
            //}
            //else
            //{
            CustomerProfile customer = GetCustomerProspect(agentSessionId, lastName, dOB);

            Response response = Client.InsertCustomer(customer, zeoContext);
            //if (VerifyException(response)) throw new AlloyWebException(response.Error.Details); 
            AlloyId = Convert.ToInt64(response.Result);

            Response saveResponse = Client.UpdateCustomer(customer, zeoContext);
            //if (VerifyException(response)) throw new AlloyWebException(response.Error.Details); 

            //MgiContext = new Channel.DMS.Server.Data.MGIContext() { EditMode = false };

            //Response activateResponse = Client.NexxoActivate(agentSessionId, AlloyId, MgiContext);
            //if (VerifyException(activateResponse)) throw new AlloyWebException(activateResponse.Error.Details);

            Response clientActivateResponse = Client.RegisterToClient(zeoContext);
            //if (VerifyException(clientActivateResponse)) throw new AlloyWebException(clientActivateResponse.Error.Details);

            return AlloyId;
            //}
        }

        public CustomerProfile GetCustomerProspect(long agentSessionId, string lastName, DateTime dOB)
        {
            Response response = Client.GetZeoContextForAgent(agentSessionId, zeoContext);
            CustomerProfile customer = new CustomerProfile()
            {
                Address = new Address
                {
                    Address1 = "4723 Apple St.AnyTown",
                    Address2 = "Street",
                    City = "FL",
                    State = "CA",
                    ZipCode = "33716",
                    Country = "UNITED STATES",

                },
                FirstName = "John",
                MiddleName = string.Empty,
                LastName = lastName,
                LastName2 = string.Empty,
                DateOfBirth = dOB,
                SSN = GetSSN(),
                Gender = HelperGender.MALE,// "Male",
                Phone1 = new Phone()
                {
                    Number = "3039637881",
                    Type = "Home",
                    Provider = "",
                },
                MailingAddressDifferent = false,

                PrimaryCountryCitizenShip = "US",
                ProfileStatus = HelperProfileStatus.Active,
                MothersMaidenName = "Eng",
                SecondaryCountryCitizenShip = "US",
                IdExpirationDate = new DateTime(2020, 10, 10),
                IdNumber = "345678912",
                IdType = "PASSPORT",
                IDTypeName = "PASSPORT",
                IdIssueDate = new DateTime(2005, 10, 10),
                Occupation = "STUDENT",
                PIN = "1111",
                ChannelPartnerId = (response.Result as ZeoContext).ChannelPartnerId,
                ReferralCode = string.Empty
            };

            return customer;
        }

        public AgentSession GetAgentSession()
        {
            AgentSSO = new AgentSSO();
            AgentSSO.UserName = "systemadmin@moneygram.com";
            //AgentSSO.role = new UserRoles ;
            AgentSSO.RoleId = 1;
            Response response = Client.AuthenticateSSO(AgentSSO, ChannelPartnerId, TerminalName, zeoContext);
            AgentSession = response.Result as AgentSession;
            return AgentSession;
        }

     

        public void GetChannelPartnerDataTCF()
        {
            TerminalName = "TCF";
            ChannelPartnerName = "TCF";
            ChannelPartnerId = "34";
            CustomerSession = new CustomerSession();
            zeoContext.Context = new Dictionary<string, object>();
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
            Client.AddReceiver( receiver, zeoContext);
        }
        public void UpdateCounterId()
        {
            Client.UpdateCounterId(zeoContext);
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

        public bool VerifyException(Response response)
        {
            if (response != null && response.Error != null)
                return true;
            return false;
        }
    }
}
