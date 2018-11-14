using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MGI.Core.CXE.Contract;
using MGI.Core.CXE.Data;
using MGI.Core.CXE.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;
using Moq;

using Spring.Testing.NUnit;

namespace MGI.Core.CXE.Test
{
	[TestFixture]
	public class IdTypeServiceDBIntegrationTests : AbstractTransactionalDbProviderSpringContextTests
	{
		private IIdTypeService _idTypeSvc;
		public IIdTypeService IdTypeService { set { _idTypeSvc = value; } }

		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Core.CXE.Test/MGI.Core.CXE.Test/springCustTest.xml" }; }
		}

		[Test]
		public void FindIdSuccess()
		{
			IdType idType = _idTypeSvc.Find( "DRIVER'S LICENSE", "UNITED STATES", "CALIFORNIA" );
			Assert.IsTrue( idType.Mask == @"^[a-zA-Z]\d{7}$" );
		}

		[Test]
		public void FindIdTooMany()
		{
			IdType idType = _idTypeSvc.Find( "DRIVER'S LICENSE", "UNITED STATES", "" );
			Assert.IsNull( idType );
		}
	}
}
