using System;
using System.Collections.Generic;
using System.Linq;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;

using MGI.Common.DataAccess.Contract;

namespace MGI.Core.Partner.Impl
{
	public class ComplianceTransactionServiceImpl : IComplianceTransactionService
	{
		private IRepository<ComplianceTransaction> _complianceTransactionRepo;
		public IRepository<ComplianceTransaction> ComplianceTransactionRepo { set { _complianceTransactionRepo = value; } }

		public List<ComplianceTransaction> Get( long customerId )
		{
			return _complianceTransactionRepo.FilterBy( x=>x.CustomerId==customerId ).ToList<ComplianceTransaction>();
		}

		public List<ComplianceTransaction> Get( long customerId, long xRecipientId )
		{
			return _complianceTransactionRepo.FilterBy( t => t.CustomerId == customerId
														|| t.xRecipientId == xRecipientId ).ToList<ComplianceTransaction>();
		}

		public List<ComplianceTransaction> Get( long customerId, long ProductId, string AccountNumber )
		{
			return _complianceTransactionRepo.FilterBy( t => t.CustomerId == customerId
													   || ( t.bpProductId == ProductId && t.bpAccountNumber == AccountNumber ) )
													   .ToList<ComplianceTransaction>();
		}
	}
}
