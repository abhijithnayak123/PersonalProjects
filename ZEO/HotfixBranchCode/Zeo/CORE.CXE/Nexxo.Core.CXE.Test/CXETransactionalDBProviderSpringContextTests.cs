using Spring.Testing.NUnit;

namespace MGI.Core.CXE.Test
{
	public class CXETransactionalDBProviderSpringContextTests : AbstractTransactionalDbProviderSpringContextTests
	{
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Core.CXE.Test/MGI.Core.CXE.Test/springCustTest.xml" }; }
		}
	}
}
