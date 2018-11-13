using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ChexarIO.Communication.Contract;
using ChexarIO.Communication.Data;
using ChexarIO.Communication.Impl;
using Spring.Testing.NUnit;

namespace ChexarIO.Communication.Test
{
    [TestFixture]
    public class ChexarMessengerFixture //: AbstractTransactionalSpringContextTests
    {
        ChexarMessenger ChexarMessenger = new ChexarMessenger();
        private string url = "http://beta.chexar.net/webservice/callcenterservice.asmx";
        ChexarLogin login = new ChexarLogin();
        int TicketNo;

        [SetUp]
        public void SetUp()
        {
            login.URL = url;
            login.CompanyToken = "F59DD48B";
            TicketNo = 38796;
            login.EmployeeId = 897;
        }

        [Test]
        public void CanCheckForMessage()
        {
            ChexarResult chexarResult = ChexarMessenger.CheckForMessage(login, TicketNo);

            Assert.AreEqual(chexarResult.ErrorStatus, false, chexarResult.ErrorDescription);
        }

        [Test]
        public void CanComposeMessage()
        {
            ChexarResult chexarResult = ChexarMessenger.ComposeMessage(login, TicketNo, "HI BIJO JAMESES");

            Assert.AreEqual(chexarResult.ErrorStatus, false, chexarResult.ErrorDescription);
        }

        [Test]
        public void CanConfirmMessage()
        {
            ChexarResult chexarResult = ChexarMessenger.ConfirmAllMessages(login, TicketNo, 12345);

            Assert.AreEqual(chexarResult.ErrorStatus, false, chexarResult.ErrorDescription);
        }
    }
}
