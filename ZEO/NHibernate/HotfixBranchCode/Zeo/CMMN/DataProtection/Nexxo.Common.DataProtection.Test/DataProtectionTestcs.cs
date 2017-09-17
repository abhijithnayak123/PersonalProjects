using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;

using NUnit.Framework;

using MGI.Common.DataProtection.Contract;
using MGI.Common.DataProtection.Impl;

namespace MGI.Common.DataProtection.Test
{
	[TestFixture]
	public class DataProtectionTestcs
	{
		DataProtectionService _DataProtection = new DataProtectionService();

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestEncrypt()
		{
			_DataProtection.Encrypt("123-456-7890", 0);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestDecrypt()
		{
			_DataProtection.Decrypt("123-456-7890", 0);
		}
	}
}
