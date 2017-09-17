using System;

using MGI.Core.Compliance.Contract;
using MGI.Core.Compliance.Data;

using MGI.Common.DataAccess.Contract;
using MGI.Common.Sys;
using MGI.Common.Util;

namespace MGI.Core.Compliance.Impl
{
	public class ComplianceProgramServiceImpl : IComplianceProgramService
	{
		private IRepository<ComplianceProgram> _complianceProgramRepo;
		public IRepository<ComplianceProgram> ComplianceProgramRepo { set { _complianceProgramRepo = value; } }

		public NLoggerCommon NLogger { get; set; }

        public ComplianceProgramServiceImpl()
        {
            NLogger = new NLoggerCommon();
        }

		public ComplianceProgram Get( string name )
		{
			try
			{
				ComplianceProgram complianceProgram = _complianceProgramRepo.FindBy( p => p.Name == name );
				NLogger.Info(string.Format("{0} Getting Compliance Limits", DateTime.Now.ToString()));
				NLogger.Info(string.Format(complianceProgram.ToString()));
				return complianceProgram;
			}
			catch ( Exception ex )
			{
				throw new CoreComplianceException(CoreComplianceException.COMPLIANCE_PROGRAM_NOT_FOUND, ex );
			}
		}
	}
}
