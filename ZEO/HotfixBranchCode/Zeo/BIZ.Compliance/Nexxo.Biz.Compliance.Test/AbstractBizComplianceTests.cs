using Spring.Testing.NUnit;

namespace MGI.Biz.Compliance.Test
{
	public abstract class AbstractBizComplianceTests : AbstractTransactionalSpringContextTests
	{
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Biz.Compliance.Test/MGI.Biz.Compliance.Test/BizCompliance.xml" }; }
		}
	}
}
