using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using MGI.Channel.DMS.Server.Contract;
using MGI.Channel.DMS.Server.Data;
using MGI.Channel.DMS.Server.Impl;

namespace MGI.Channel.DMS.Server.Svc
{
	public partial class DesktopWSImpl : ICashService
	{
		public long CashIn(long customerSessionId, decimal amount, MGIContext mgiContext)
		{
			return DesktopEngine.CashIn(customerSessionId, amount, mgiContext);
		}		
		
		//AL-2729 user story for updating the cash-in transaction
        public long UpdateCash(long customerSessionId, long trxId, decimal amount, MGIContext mgiContext)
		{
            return DesktopEngine.UpdateCash(customerSessionId, trxId, amount,  mgiContext);
		}
    }
}
