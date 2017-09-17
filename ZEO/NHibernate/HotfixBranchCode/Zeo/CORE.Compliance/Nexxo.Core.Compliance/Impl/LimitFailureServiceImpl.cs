using System;

using MGI.Core.Compliance.Contract;
using MGI.Core.Compliance.Data;

using MGI.Common.DataAccess.Contract;

namespace MGI.Core.Compliance.Impl
{
	public class LimitFailureServiceImpl : ILimitFailureService
	{
		private IRepository<LimitFailure> _failRepo;
		public IRepository<LimitFailure> FailureRepo { set { _failRepo = value; } }

		public void Add( LimitFailure limitFailure )
		{
			limitFailure.DTServerCreate = DateTime.Now;
			_failRepo.Add( limitFailure );
		}
	}
}
