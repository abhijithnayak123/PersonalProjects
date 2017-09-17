using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace TCF.Zeo.Common.Util
{
    public class TransactionHandler
    {
        public static TransactionScope CreateTransactionScope(Helper.TransactionScopeOptions option = Helper.TransactionScopeOptions.Required)
        {
            var scopeOption = (TransactionScopeOption)option;
            var transactionOptions = new TransactionOptions();
            transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = TransactionManager.MaximumTimeout;
            return new TransactionScope(scopeOption, transactionOptions);
        }
    }
}
