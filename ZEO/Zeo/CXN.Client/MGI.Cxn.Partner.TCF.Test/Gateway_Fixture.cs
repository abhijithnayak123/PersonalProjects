using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MGI.Cxn.Partner.TCF;
using Spring.Testing.NUnit;
using MGI.Cxn.Partner.TCF.Data;
using MGI.Cxn.Partner.TCF.Impl;
using MGI.Common.Util;


namespace MGI.Cxn.Partner.TCF.Test
{
	[TestFixture]
	public class Gateway_Fixture : AbstractTransactionalSpringContextTests
	{
		protected override string[] ConfigLocations
		{
			get { return new string[] { "assembly://MGI.Cxn.Partner.TCF.Test/MGI.Cxn.Partner.TCF.Test/MGI.Cxn.Partner.TCF.Test.Spring.xml" }; }
		}

		public IO IO { private get; set; }

		public MGIContext MgiContext { get; set; }

		#region Negative Pre Flush and Post Flush test case

		[TestFixtureSetUp]
		public void SetUpAttribute()
		{
			MgiContext = new MGIContext()
			{
				TimeZone = TimeZone.CurrentTimeZone.StandardName,
				ChannelPartnerId = 34,
				BankId = "099"
			};
		}


		[Test]
		public void PreFlush()
		{
			Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>();
			customerLookUpCriteria.Add("SSN", "718111072");
			customerLookUpCriteria.Add("DateOfBirth", "01/03/1975");

			CustomerTransactionDetails cart = new CustomerTransactionDetails()
			{
				Customer = new Customer()
				{
					FirstName = "FNAME",
					LastName = "LNAME",
					Address1 = "TEST",
					City = "ACTON",
					State = "CA",
					Zip = "93510",
					Phone1 = "7373839393",
					Ph1Type1 = "Home",
					SSN = "758959595",
					AlloyID = 1000000000000030,
					Gender = "M",
				},

				Transactions = new List<Transaction>()
				{
					new Transaction()
					{
						ID = "1000000266",
						TransferType = "SendMoney",
						Amount = Convert.ToDecimal("0.00"),
						Fee = Convert.ToDecimal("12.60"),
						GrossTotalAmount = Convert.ToDecimal("134.60"),
						ToFirstName = "RFNAME",
						ToLastName = "RLNAME",
						ToCountry = "US",
						ToAddress  = "TEST",
						ToCity = "ACTON",
						ToZipCode = "93510",
						ToPickUpCountry = "US",
						ToPickUpState_Province = "CA",
						ToDeliveryMethod = "000",
						ToDeliveryOption = "002",
					},
					new Transaction()
					{
						ID = "1000000218",
						Type = "Cash",
						CashType = "CashIn",
						Amount = Convert.ToDecimal("135.0000"),
						Fee = Convert.ToDecimal("0.0000"),
						GrossTotalAmount = Convert.ToDecimal("135.0000")
					}
				}
			};

			bool res = IO.PreFlush(cart, MgiContext);

			Assert.IsTrue(res);
		}


		[Test]
		public void PostFlush()
		{
			Dictionary<string, object> customerLookUpCriteria = new Dictionary<string, object>();

			customerLookUpCriteria.Add("DateOfBirth", "01/03/1975");
			customerLookUpCriteria.Add("CardNumber", "57222720001572648");
			customerLookUpCriteria.Add("AccountType", "SAV");

			CustomerTransactionDetails cart = new CustomerTransactionDetails()
			{
				Customer = new Customer()
				{
					FirstName = "FNAME",
					LastName = "LNAME",
					Address1 = "TEST",
					City = "ACTON",
					State = "CA",
					Zip = "93510",
					Phone1 = "7373839393",
					Ph1Type1 = "Home",
					SSN = "758959595",
					AlloyID = 1000000000000030,
					Gender = "M",
				},

				Transactions = new List<Transaction>()
				{
					new Transaction()
					{
						ID = "1000000266",
						TransferType = "SendMoney",
						Type= MGI.Core.Partner.Data.Transactions.TransactionType.MoneyTransfer.ToString(),
						Amount = Convert.ToDecimal("0.00"),
						Fee = Convert.ToDecimal("12.60"),
						GrossTotalAmount = Convert.ToDecimal("134.60"),
						ToFirstName = "RFNAME",
						ToLastName = "RLNAME",
						ToCountry = "US",
						ToAddress  = "TEST",
						ToCity = "ACTON",
						ToZipCode = "93510",
						ToPickUpCountry = "US",
						ToPickUpState_Province = "CA",
						ToDeliveryMethod = "000",
						ToDeliveryOption = "002",
					},
					new Transaction()
					{
						ID = "1000000218",
						Type = "Cash",
						CashType = "CashIn",
						Amount = Convert.ToDecimal("135.0000"),
						Fee = Convert.ToDecimal("0.0000"),
						GrossTotalAmount = Convert.ToDecimal("135.0000")
					}
				}
			};

			IO.PostFlush(cart, MgiContext);
		}


		#endregion
	}
}
