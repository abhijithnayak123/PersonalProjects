using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using MGI.Common.DataProtection.Contract;
using MGI.Common.DataProtection.Impl;

namespace MGI.Common.DataProtection.Test
{
	[TestFixture]
	public class DataProtectionSimulatorTest
	{
		private IDataProtectionService _DataProtection = new DataProtectionSimulator();

		[Test]
		public void TestEncrypt()
		{
			string testString = "1234567890";

			Assert.AreEqual(_DataProtection.Encrypt(testString, 0), "0987654321");
		}

		[Test]
		public void TestDecrypt()
		{
			string testString = "321 dlrow olleh";

			Assert.AreEqual(_DataProtection.Encrypt(testString, 0), "hello world 123");
		}
	}
}
