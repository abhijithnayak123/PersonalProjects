// -----------------------------------------------------------------------
// <copyright file="MessageStoreTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using NUnit.Framework;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using Spring.Testing.NUnit;
using System;
using System.Collections.Generic;

namespace MGI.Core.Partner.Test
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestFixture]
    public class MessageStoreImpl_Fixture : AbstractPartnerTest
    {
        private IMessageStore _MessageStore;
		Message msg = new Message() { Content = "Please ask for more..." };

        string key = "1000.100";

        public IMessageStore MessageStore
        {
            get { return _MessageStore; }
            set { _MessageStore = value; }
        }

        private ChannelPartner Partner = new ChannelPartner { Id = 223};

        [TestFixtureSetUp]
        public void Setup() { 
           // create the channel partner 
            Partner = new ChannelPartner { Id = 123};
        }

        [Test]
        public void TestAddMessage()
        {
            long msgId = _MessageStore.Add(Partner.Id,key,Language.EN, msg,"");
            _CheckMessage(msg, msgId);
            
           // SetComplete();
           // EndTransaction();
        }

        [Test]
        [ExpectedException("MGI.Core.Partner.Data.MessageStoreException")]
        [TestCaseSource("MsgStoreData")]
        public void TestAdd_Same_Messages_Same_Partner_Same_Lang(long partnerId, string key,
            Language lang, Message msg) 
        {
            long msgId1 = _MessageStore.Add(partnerId, key, lang, msg,"");
            long msgId2 = _MessageStore.Add(partnerId, key, lang, msg,"");
        }

        [Test]
        [TestCaseSource("MsgStoreData")]
        public void TestAdd_Same_Message_Same_Partner_Diff_Lang(long partnerId, string key, 
                Language lang, Message msg)
        {
            long msgId = _MessageStore.Add(partnerId, key, lang, msg,"");
            _CheckMessage(msg, msgId);
        }
       
		//[Test]
		//public void TestLookup_PiggyBackOnDefault() 
		//{
		//	long msgId1 = _MessageStore.Add(1, key, Language.EN, msg);

		//	string message = _MessageStore.Lookup(Partner.Id, key, Language.EN).ToString();
		//	Assert.AreEqual(msg, message);
			
		//}

        [Test]
        public void TestLookup_NonExistentMessage() 
        {
			//string message = _MessageStore.Lookup(Partner.Id, key, Language.EN).ToString();
			//Assert.Null(message);
        }

        #region helper mthds
        public IEnumerable<TestCaseData> MsgStoreData 
        {
            get {
                yield return new TestCaseData(Partner.Id, key, Language.EN, msg);
                yield return new TestCaseData(Partner.Id, key, Language.ES, msg + "es");
            }  
        }
 
        private void _CheckMessage(Message expectedMsg, long msgId)
        {
            Message message = _MessageStore.Lookup(msgId);

            Assert.AreEqual(expectedMsg.Content, message.Content);
        }
        
        #endregion
    }
}
