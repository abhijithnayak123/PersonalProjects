using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MGI.Common.TransactionalLogging.Data;
namespace MGI.Common.TransactionalLogging.Contract
{
     public interface ITLogger
    {
       // void Log();
       // void SetContext();
        void Savelog(TransactionLogEntry obj);
       // Task<IList<TLogEntry>> Readlog();
        TransactionLogEntry ReadSinglelog();
        List<TransactionLogEntry> Readlog();
    }
}
