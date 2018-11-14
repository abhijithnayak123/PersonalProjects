using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TCF.Zeo.Common.Util.Helper;
using TCF.Zeo.Common.Data;
namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class Compliance_Fixture
    {
        ZeoContext context = new ZeoContext();
        IComplianceService compliance = new ComplianceServiceImpl();

        //[TestCase(TransactionType.BillPayment)]
        //[TestCase(TransactionType.DebitGPR)]
        //[TestCase(TransactionType.LoadToGPR)]
        [TestCase(TransactionType.MoneyOrder)]
        [TestCase(TransactionType.MoneyTransfer)]
        //[TestCase(TransactionType.ProcessCheck)]
        public void Can_Get_Transaction_Limit(TransactionType transactionType)
        {
            Limit limit = compliance.GetTransactionLimit(transactionType, 34, context);

            Assert.IsNotNull(limit);
        }
    }
}
