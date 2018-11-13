using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Data;

namespace MGI.Biz.Partner.Test
{
    [TestFixture]
    public class TransactionHistoryTest : AbstractPartnerTest
    {
        public ITransactionHistoryService BIZPartnerTransactionHistoryService { private get; set; }

        //private TransactionHistoryServiceImpl svc = new TransactionHistoryServiceImpl();
        
        //private Mock<ITransactionHistoryService> moqCSSvc = new Mock<ITransactionHistoryService>();
        //private Mock<MGI.Core.Partner.Contract.ITransactionHistoryService> moqSCSvc = new Mock<Core.Partner.Contract.ITransactionHistoryService>();

        
        [Test]
        public void GetTransactionHistoryTest()
        {
          // List<TransactionHistory> transactionHistory =  BIZPartnerTransactionHistoryService.GetTransactionHistory(200000,"Check Processing","TestTimeStamp",false);
            
          //  Assert.IsTrue(transactionHistory.Count>0);

            
        }
    }
}
