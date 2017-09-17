using System;

using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Data.Transactions;
using System.Linq;
using System.Linq.Expressions;
using MGI.TimeStamp;

namespace MGI.Core.Partner.Impl
{
    public class CustomerSessionCounterIdServiceImpl : ICustomerSessionCounterIdService
    {

		public IRepository<CustomerSessionCounter> CustomerSessionCounterIdRepo { private get; set; }
		
	}
}
