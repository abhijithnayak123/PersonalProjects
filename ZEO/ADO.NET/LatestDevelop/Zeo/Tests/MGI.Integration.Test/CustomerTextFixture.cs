using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using MGI.Integration.Test.Data;
using TCF.Channel.Zeo.Web.ServiceClient.ZeoService;

namespace MGI.Integration.Test
{
    [TestFixture]
    public partial class AlloyIntegrationTestFixture
    {

        [TestCase("TCF")]
        public void CustomerRegister(string channelPartnerName)
        {
            ZeoContext alloyContext = new ZeoContext();
            var registerCustomer = RegisterCustomer(channelPartnerName, alloyContext);
            Assert.That(registerCustomer, Is.Not.Null);
        }
        [TestCase("TCF")]
        public void InitiateCustomerSessionIT(string channelPartnerName)
        {
            var customerSession = InitiateCustomerSession(channelPartnerName);
            Assert.That(customerSession, Is.Not.Null);
        }


        private CustomerProfile RegisterCustomer(string channelPartnerName, ZeoContext zeoContext)
        {

            AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            CustomerProfile prospect = IntegrationTestData.GetCustomerProspect(channelPartner);

            if (channelPartner.Id == 34)
            {
                Dictionary<string, object> ssoAttributes = IntegrationTestData.GetSSOAttributes();
                //alloyContext.Context = new Dictionary<string, object>();
                if (ssoAttributes == null)
                {
                    ssoAttributes = new Dictionary<string, object>();
                }
                zeoContext.SSOAttributes = ssoAttributes;
            }


            Response response = client.GetZeoContextForAgent(Convert.ToInt64(agentSession.SessionId), zeoContext);
            zeoContext = response.Result as ZeoContext;
            Response custResponse = client.InsertCustomer(prospect, zeoContext);

            CustomerSearchCriteria criteria = new CustomerSearchCriteria();
            client.RegisterToClient(zeoContext);
            criteria.Lastname = channelPartnerName;
            criteria.DateOfBirth = new DateTime(1950, 10, 10);

            Response searchResponse = client.SearchCustomer(criteria, zeoContext);
            //if(VerifyException(searchResponse)) throw new Exception(searchResponse.Error.Details);
            List<CustomerProfile> searchResult = searchResponse.Result as List<CustomerProfile>;

            var customer = searchResult.Where(c => c.ProfileStatus == HelperProfileStatus.Active).FirstOrDefault();
            return customer;
        }


        [TestCase("TCF")]
        public void CustomerModify(string channelPartnerName)
        {
            var modifyCustomer = ModifyCustomer(channelPartnerName);
            Assert.AreEqual("D3210123", modifyCustomer.IdNumber);
        }

        private CustomerProfile ModifyCustomer(string channelPartnerName)
        {
            AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);
            Response response = client.GetZeoContextForAgent(Convert.ToInt64(agentSession.SessionId), zeoContext);
            zeoContext = response.Result as ZeoContext;
            CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria() { Lastname = channelPartnerName, DateOfBirth = new DateTime(1950, 10, 10) };

            Response searchResponse = client.SearchCustomer(searchCriteria, zeoContext);
            //if (VerifyException(searchResponse)) throw new AlloyWebException(searchResponse.Error.Details); 
            List<CustomerProfile> customers = searchResponse.Result as List<CustomerProfile>;
            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);
            CustomerProfile prospect = IntegrationTestData.GetCustomerProspect(channelPartner);

            prospect.IDTypeName = "D3210123";

            Response saveResponse = client.InsertCustomer(prospect, zeoContext);

            Response custSearchResponse = client.SearchCustomer(searchCriteria, zeoContext);
            customers = custSearchResponse.Result as List<CustomerProfile>;

            
            var customer = customers.Where(c => c.IdNumber == "D3210123").FirstOrDefault();
            return customer;
        }
        private CustomerSession InitiateCustomerSession(string channelPartnerName, bool IsGprCard = false)
        {
            CustomerSearchCriteria searchCriteria;

            ZeoContext zeoContext = new ZeoContext();

            AgentSession agentSession = CreateAgentAndAgentSession(channelPartnerName, zeoContext);

            ChannelPartner channelPartner = GetChannelPartner(channelPartnerName, zeoContext);

            Response response = client.GetZeoContextForAgent(Convert.ToInt64(agentSession.SessionId), zeoContext);

            zeoContext = response.Result as ZeoContext;

            if (!IsGprCard)
                searchCriteria = IntegrationTestData.GetSearchCriteria(channelPartner);
            else
                searchCriteria = IntegrationTestData.GetSearchCriteria(channelPartner, IsGprCard);

            Response searchResponse = client.SearchCustomer(searchCriteria, zeoContext);

            List<CustomerProfile> searchResult = searchResponse.Result as List<CustomerProfile>;

            CustomerProfile result = searchResult.FirstOrDefault();

            zeoContext.CustomerId = Convert.ToInt64(result.CustomerId);

            var customer = searchResult.Where(c => c.ProfileStatus == HelperProfileStatus.Active).FirstOrDefault();

            Response customerSessionResponse = client.InitiateCustomerSession(3, zeoContext);

            CustomerSession customerSession = (CustomerSession)customerSessionResponse.Result;

            return customerSession;
        }
    }
}
