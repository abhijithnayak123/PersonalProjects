using TCF.Zeo.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
    public class MoneyTransferTransaction : ZeoModel
    {
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string Description { get; set; }
        public string ConfirmationNumber { get; set; }
        public int RecipientId { get; set; }
        public decimal ExchangeRate { get; set; }
        public int TransferType { get; set; }
        public string TransactionSubType { get; set; }
        public long OriginalTransactionID { get; set; }
        public long CustomerSessionId { get; set; }
        public long ProviderId { get; set; }
        public long ProviderAccountId { get; set; }
        public string Destination { get; set; }
        public int State { get; set; }
        public Dictionary<string, object> MetaData { get; set; }
    }
}
