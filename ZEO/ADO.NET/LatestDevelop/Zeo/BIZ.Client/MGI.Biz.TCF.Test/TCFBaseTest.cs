using System;
using MGI.Biz.TCF.Impl;
using System.Collections.Generic;
using NUnit.Framework;

namespace MGI.Biz.TCF.Test
{
	[TestFixture]
	public class TCFBaseTest
	{
		[Test]
		public void TruncateFirstName()
		{
			Dictionary<string, string> customerName = new Dictionary<string, string>();

			customerName = BaseClass.TruncateFullName("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "", "ABCDEFGHIJKLMN", "");

			Assert.AreEqual(customerName["FirstName"].Length, 20);
			Assert.AreEqual(customerName["FirstName"], "ABCDEFGHIJKLMNOPQRST");
		}

		[Test]
		public void TruncateLastName()
		{
			Dictionary<string, string> customerName = new Dictionary<string, string>();

			customerName = BaseClass.TruncateFullName("John", "Joseph", "ABCDEFGHIJKLMN", "OPQRSTUVWXYZ");

			Assert.AreEqual((customerName["LastName"] + " " + customerName["SecondLastName"]).Length, 20);
			Assert.AreEqual(customerName["LastName"], "ABCDEFGHIJKLMN");
			Assert.AreEqual(customerName["SecondLastName"], "OPQRS");
		}

		[Test]
		public void TruncateLastName20()
		{
			Dictionary<string, string> customerName = new Dictionary<string, string>();

			customerName = BaseClass.TruncateFullName("John", "Joseph", "ABCDEFGHIJKLMNOPQRST", "OPQRSTUVWXYZ");

			Assert.AreEqual(customerName["LastName"].Length, 20);
			Assert.AreEqual(customerName["LastName"], "ABCDEFGHIJKLMNOPQRST");
			Assert.AreEqual(customerName["SecondLastName"], "");
		}

		[Test]
		public void TruncateFistNameLastName()
		{
			Dictionary<string, string> customerName = new Dictionary<string, string>();

			customerName = BaseClass.TruncateFullName("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "JOSEPH", "ABCDEFGHIJKLMN", "OPQRSTUVWXYZ");

			string fullName = customerName["FirstName"] + " " + customerName["LastName"] + " " + customerName["SecondLastName"];

			Assert.IsTrue((customerName["LastName"] + customerName["SecondLastName"]).Length <= 40);

			Assert.AreEqual(fullName, "ABCDEFGHIJKLMNOPQRST ABCDEFGHIJKLMN OPQR");
		}

		[Test]
		public void TruncateFullName()
		{
			Dictionary<string, string> customerName = new Dictionary<string, string>();

			customerName = BaseClass.TruncateFullName("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "JOSEPH", "ABCDEFGHIJKL", "");

			string fullName = customerName["FirstName"] + " " + "JOSEPH" + " " + customerName["LastName"];

			Assert.IsTrue(customerName["LastName"].Length <= 40);

			Assert.AreEqual(fullName, "ABCDEFGHIJKLMNOPQRST JOSEPH ABCDEFGHIJKL");
		}

		[Test]
		public void TruncateMiddleNameToFirstChar()
		{
			Dictionary<string, string> customerName = new Dictionary<string, string>();

			customerName = BaseClass.TruncateFullName("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "JOSEPH", "ABCDEFGHI", "KLMNOP");

			string fullName = customerName["FirstName"] + " " + "J" + " " + customerName["LastName"] + " " + customerName["SecondLastName"];

			Assert.IsTrue((customerName["LastName"] + customerName["SecondLastName"]).Length <= 40);

			Assert.AreEqual(fullName, "ABCDEFGHIJKLMNOPQRST J ABCDEFGHI KLMNOP");
		}
	
		[Test]
		public void Can_Validate_When_ZipCode_Contains_White_Spaces()
		{
			bool isValid = BaseClass.IsValidZipCode("     ");
			Assert.IsFalse(isValid);
		}

		[Test]
		public void Can_Validate_When_ZipCode_Contains_Five_Zeros()
		{
			bool Zipcode1 = BaseClass.IsValidZipCode("00000");
			Assert.IsFalse(Zipcode1);

		}


		[Test]
		public void Can_Validate_When_ZipCode_Contains_Four_Zeros()
		{
			bool isValid = BaseClass.IsValidZipCode("0000");
			Assert.IsFalse(isValid);
		}

		
		[Test]
		public void Can_Validate_When_ZipCode_Contains_Three_Zeros()
		{
			bool Zipcode1 = BaseClass.IsValidZipCode("000");
			Assert.IsFalse(Zipcode1);

		}

		[Test]
		public void Can_Validate_When_ZipCode_Contains_Two_Zeros()
		{
			bool Zipcode1 = BaseClass.IsValidZipCode("00");
			Assert.IsFalse(Zipcode1);

		}

		[Test]
		public void Can_Validate_When_ZipCode_Contains_One_Zero()
		{
			bool Zipcode1 = BaseClass.IsValidZipCode("0");
			Assert.IsFalse(Zipcode1);

		}

		[Test]
		public void Can_Validate_When_ZipCode_Does_Not_Contains_Zeros()
		{
			bool Zipcode1 = BaseClass.IsValidZipCode("9005");
			Assert.IsTrue(Zipcode1);

		}

		[Test]
		public void Can_Validate_When_ZipCode_Contains_Null()
		{
			bool Zipcode1 = BaseClass.IsValidZipCode(null);
			Assert.IsTrue(Zipcode1);

		}
	}
}
