using TCF.Zeo.Common.Util;
using TCF.Zeo.Core.Contract;
using TCF.Zeo.Core.Data;
using TCF.Zeo.Core.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCF.Zeo.Common.Data;
namespace TCF.Zeo.Core.Test
{
    [TestFixture]
    public class FundService_Fixture
    {
        IFundService fundService = new ZeoCoreImpl();

        [Test]
        public void Can_CommitTransaction()
        {
            ZeoContext alloycontext = new ZeoContext();
            long transactionId = 10000000;
            long customerSessionId = 1000000;
            int status = 2;
            string timeZone = DateTime.Now.ToString();
            fundService.CommitFundTransaction(transactionId, status, customerSessionId, alloycontext);
        }

        [Test]
        public void can_CreateFundTransaction()
        {
            ZeoContext alloycontext = new ZeoContext();
            Funds funds = new Funds();
            funds.Amount = 12;
            funds.BaseFee = 2;
            funds.DiscountApplied = 0;
            funds.AdditionalFee = 1;
            funds.Fee = 2;
            funds.FundsType = Helper.FundType.Credit;
            funds.CustomerSessionId = 10000000;
            funds.AddOnCustomerId = 100000;
            funds.DTServerCreate = DateTime.Now;
            funds.DTTerminalCreate = DateTime.Now;
            funds.State = 2;
            funds.ProviderAccountId = 100000;
            funds.ProviderId = 34;
            funds.ConfirmationNumber = "10000000";
            long id = fundService.CreateFundTransaction(funds, alloycontext);
            Assert.IsNotNullOrEmpty(id.ToString());
        }

        [Test]
        public void can_GetFundTransaction()
        {
            ZeoContext alloycontext = new ZeoContext();

            long transactionId = 10000000;
            Funds fund = fundService.GetFundTransaction(transactionId, alloycontext);
            Assert.IsNotNull(fund);
        }

        [Test]
        public void can_UpdateFundTransaction()
        {
            ZeoContext alloycontext = new ZeoContext();
            Funds funds = new Funds();
            funds.Id = 1000000;
            funds.Amount = 12;
            funds.Fee = 2;
            funds.Description = "test";
            funds.State = 2;
            funds.ConfirmationNumber = "100000";
            funds.BaseFee = 2;
            funds.AdditionalFee = 2;
            funds.DiscountName = "";
            funds.IsSystemApplied = false;
            funds.DiscountApplied = 0;
            funds.FundsType = Helper.FundType.Credit;
            funds.DTTerminalLastModified = DateTime.Now;
            funds.DTServerLastModified = DateTime.Now;
            bool states = fundService.UpdateFundTransaction(funds, alloycontext);
            Assert.IsNotNullOrEmpty(states.ToString());


        }
        [Test]
        public void can_UpdateFundTransactionState()
        {
            ZeoContext alloycontext = new ZeoContext();
            long transactionId = 1000000;
            Helper.TransactionStates transactionStates = Helper.TransactionStates.Pending;
            string timeZone = DateTime.Now.ToString();
            fundService.UpdateFundTransactionState(transactionId, transactionStates, alloycontext);
        }
    }
}
