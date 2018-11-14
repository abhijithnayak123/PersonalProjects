using TCF.Zeo.Biz.Contract;
using TCF.Zeo.Biz.Impl;
using NUnit.Framework;
using System.Collections.Generic;
using TCF.Zeo.Common.Data;
using ZeoData = TCF.Channel.Zeo.Data;

namespace TCF.Zeo.Biz.Test
{
    public class MessageCenter_Fixture
    {

        [Test]
        public void Can_GetMessageDetails()
        {
           
            IMessageCenterService _messageCenterDetail = new MessageCenterServiceImpl();
            var context = GetContext();
            long transactionid = 1000000044;
            ZeoData.AgentMessage message = _messageCenterDetail.GetMessageDetails(transactionid,context);
            Assert.IsNotNullOrEmpty(message.ToString());


        }
        [Test]
        public void Can_GetMessagesByAgentId()
        {

            IMessageCenterService _messageCenterDetail = new MessageCenterServiceImpl();
            var context = GetContext();
            List<ZeoData.AgentMessage> message = _messageCenterDetail.GetMessagesByAgentID(context); 
            Assert.IsNotNullOrEmpty(message.ToString());


        }
        private ZeoContext GetContext()
        {
            ZeoContext context = new ZeoContext()
            {
                ChannelPartnerId = 34,
                CheckUserName = "INGO",
                URL = "https://proxy.ic.local/ingo/webservice/",
                IngoBranchId = 12345,
                CompanyToken = "simulator",
                EmployeeId = 12345,
                ChannelPartnerName = "Synovus",
                CustomerSessionId = 1234567890,
                AgentId = 1234567890,
                ProviderId = 34,
            };
            return context;
        }

    }
}
