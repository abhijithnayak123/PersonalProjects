using TCF.Zeo.Common.Data;
using TCF.Zeo.Cxn.MoneyTransfer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.Contract
{
    public interface IReceiver
    {
        long AddReceiver(Receiver receiver);

        long UpdateReceiver(Receiver receiver);

        //bool DeleteReceiver(long receiverId, string status);

        Receiver GetReceiver(long receiverId);
        
        List<Receiver> GetFrequentReceivers(long customerId);

        bool DeleteFavoriteReceiver(Receiver receiver);
    }
}
