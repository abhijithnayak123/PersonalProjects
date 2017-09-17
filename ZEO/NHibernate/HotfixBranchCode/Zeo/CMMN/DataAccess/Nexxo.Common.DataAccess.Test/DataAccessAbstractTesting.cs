using Spring.Testing.NUnit;

namespace MGI.Common.DataAccess.Test
{
	public class DataAccessAbstractTesting : AbstractTransactionalDbProviderSpringContextTests
	{
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Common.DataAccess.Test/MGI.Common.DataAccess.Test/DataAccessTestSpring.xml" }; }
		}
	}
}
