using AutoMapper;
using System;

using MGI.Biz.FundsEngine.Data;
using MGI.Biz.FundsEngine.Contract;
using MGI.Cxn.Customer.Data;
using MGI.Cxn.Customer.Contract;
using MGI.Cxn.Customer.FIS.Data;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Data;

using MGI.Biz.Events.Contract;
using System.Collections.Generic;
using MGI.Common.Util;

namespace MGI.Biz.Synovus.Impl
{
    public class SynovusGprAddEventListener : INexxoBizEventListener
    {
        public IClientCustomerService CXNClientCustomerSvc
            { private get; set; }
        public IRepository<FISAccount> CXNClientFISRepo
            { private get; set; }
        public NLoggerCommon NLogger { get; set; }

        public void Notify(NexxoBizEvent BizEvent)
        {
            GPRAddEvent EventData = (GPRAddEvent)BizEvent;
            Mapper.CreateMap<FundsAccount, CustomerProfile>()
                .ForMember(x => x.RelationshipAccountNumber, y => y.MapFrom(z => z.AccountNumber));
            AddGpr(EventData);
                        
        }

        private void AddGpr(GPRAddEvent EventData)
        {
            NLogger.Info("Add gpr event being called");
            Dictionary<string, object> context = EventData.mgiContext.Context;

            //TODO: Karun - I don't even remember why he had this code here.

            //string bankId = location.BankID;
            //string branchId = location.BranchID;

            //if (string.IsNullOrEmpty(bankId))
            //    throw new Exception("Location does not contain the required bank and branch information");
            //if (string.IsNullOrEmpty(branchId))
            //    throw new Exception("Location does not contain the required bank and branch information");

			bool isExistingAccount = EventData.mgiContext.IsExistingAccount;
            if (!isExistingAccount)
            {
                context.Add("AccountType", MGI.Cxn.Customer.FIS.Data.CxnFISEnum.ConnectionsType.PREPD.ToString());
                

                CustomerProfile account = Mapper.Map<FundsAccount, CustomerProfile>(EventData.Gpr);
                //uncomment this, after we fix misc account creation.
                NLogger.Info("Add account being called in customer.fis.");
                CXNClientCustomerSvc.AddAccount(account, EventData.mgiContext);
            }
        }
    }
}
