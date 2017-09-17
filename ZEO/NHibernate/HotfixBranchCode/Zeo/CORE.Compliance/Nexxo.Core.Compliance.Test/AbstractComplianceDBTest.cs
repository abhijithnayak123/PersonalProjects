using System.Data;

using Spring.Testing.NUnit;

namespace MGI.Core.Compliance.Test
{
	public abstract class AbstractComplianceDBTest : AbstractTransactionalDbProviderSpringContextTests
	{
		protected override string[] ConfigLocations
		{
			get
			{
				return new string[] { "assembly://MGI.Core.Compliance.Test/MGI.Core.Compliance.Test/Core.Compliance.Test.Spring.xml" };
			}
		}

		protected void AdoExecNonQuery( string sql )
		{
			AdoTemplate.ExecuteNonQuery( CommandType.Text, sql );
		}

		protected object AdoExecScalar( string sql )
		{
			return AdoTemplate.ExecuteScalar( CommandType.Text, sql );
		}
	}
}
