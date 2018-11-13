using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MGI.Biz.Customer.Contract;
using MGI.Biz.Customer.Data;
using MGI.Biz.Customer.Impl;

using IdType = MGI.Core.CXE.Data.IdType;

using NUnit.Framework;

namespace MGI.Biz.Customer.Test
{
	[TestFixture]
	public class FieldValidatorTests
	{
		IProspectFieldValidator fieldValidator;

		[SetUp]
		public void setup()
		{
			fieldValidator = new ProspectFieldValidator();
		}

		/// <summary>
		/// Make sure the NexxoException minor code matches
		/// </summary>
		/// <typeparam name="T">NexxoException type</typeparam>
		/// <param name="code">Code that's being checked</param>
		/// <param name="minorCode">Minor code to match</param>
		private void MinorCodeMatch<T>( TestDelegate code, int minorCode ) where T : MGI.Common.Sys.NexxoException
		{
			try
			{
				code();
				Assert.IsTrue( false );
			}
			catch ( T ex )
			{
				Assert.IsTrue( ex.MinorCode == minorCode );
			}
		}

		[Test]
		public void NamesOK()
		{
			Prospect prospect = new Prospect();
			Assert.DoesNotThrow( () => fieldValidator.ValidateNames( prospect ) );

			prospect.FName = "Red123";

			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateNames( prospect ), BizCustomerException.INVALID_CUSTOMER_DATA_FNAME );

			prospect.FName = "Red O'Green-Hasselbeck";

			Assert.DoesNotThrow( () => fieldValidator.ValidateNames( prospect ) );

			prospect.MName = "BILL2";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateNames( prospect ), BizCustomerException.INVALID_CUSTOMER_DATA_MNAME );

			prospect.MName = "BILL";
			Assert.DoesNotThrow( () => fieldValidator.ValidateNames( prospect ) );

			prospect.LName = "ROOT@#";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateNames( prospect ), BizCustomerException.INVALID_CUSTOMER_DATA_LNAME );

			prospect.LName = "GREEN";
			Assert.DoesNotThrow( () => fieldValidator.ValidateNames( prospect ) );

			prospect.LName2 = "ROOT@#";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateNames( prospect ), BizCustomerException.INVALID_CUSTOMER_DATA_LNAME2 );

			prospect.LName2 = "GREEN";
			Assert.DoesNotThrow( () => fieldValidator.ValidateNames( prospect ) );

			prospect.MoMaName = "ROOT@#";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateNames( prospect ), BizCustomerException.INVALID_CUSTOMER_DATA_MOMANAME );

			prospect.MoMaName = "GREEN";
			Assert.DoesNotThrow( () => fieldValidator.ValidateNames( prospect ) );
		}

		[Test]
		public void AddressOK()
		{
			Prospect prospect = new Prospect();
			Assert.DoesNotThrow( () => fieldValidator.ValidateAddress( prospect ) );

			//prospect.Address1 = "P.O. Box 123";
			//MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateAddress( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_ADDRESS1 );

			//prospect.Address1 = "PO Box 123";
			//MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateAddress( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_ADDRESS1 );

			prospect.Address1 = "123 open rd";
			fieldValidator.ValidateAddress( prospect );
			Assert.DoesNotThrow( () => fieldValidator.ValidateAddress( prospect ) );

			prospect.City = "Club 51";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateAddress( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_CITY );

			prospect.City = "SAN JUAN";
			Assert.DoesNotThrow( () => fieldValidator.ValidateAddress( prospect ) );

			prospect.PostalCode = "A2345";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateAddress( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_POSTAL_CODE );

			prospect.PostalCode = "98334";
			Assert.DoesNotThrow( () => fieldValidator.ValidateAddress( prospect ) );
		}

		[Test]
		public void DOBOver18()
		{
			Prospect prospect = new Prospect();
			Assert.DoesNotThrow( () => fieldValidator.ValidateDOB( prospect ) );

			prospect.DOB = DateTime.MinValue;
			Assert.DoesNotThrow( () => fieldValidator.ValidateDOB( prospect ) );

			prospect.DOB = DateTime.Today.AddYears( -17 );
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateDOB( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_DOB );

			prospect.DOB = DateTime.Today.AddYears( -18 );
			Assert.DoesNotThrow( () => fieldValidator.ValidateDOB( prospect ) );
		}

		[Test]
		public void PhoneOK()
		{
			Prospect prospect = new Prospect();
			Assert.DoesNotThrow( () => fieldValidator.ValidateDOB( prospect ) );

			prospect.Phone1 = "8888888888";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidatePhone( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_PHONE1 );

			prospect.Phone1 = "1234567890";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidatePhone( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_PHONE1 );

			prospect.Phone1 = "6509898899";
			Assert.DoesNotThrow( () => fieldValidator.ValidateDOB( prospect ) );

			prospect.Phone2 = "7777777777";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidatePhone( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_PHONE2 );

			prospect.Phone2 = "0123456789";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidatePhone( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_PHONE2 );

			prospect.Phone2 = "4126812593";
			Assert.DoesNotThrow( () => fieldValidator.ValidateDOB( prospect ) );
		}

		[Test]
		public void EmailOK()
		{
			Prospect prospect = new Prospect();
			Assert.DoesNotThrow( () => fieldValidator.ValidateEmail( prospect ) );

			prospect.Email = "mdowd@help@gmail.com";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateEmail( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_EMAIL );

			prospect.Email = "mdowd@gmail.com";
			Assert.DoesNotThrow( () => fieldValidator.ValidateEmail( prospect ) );
		}

		[Test]
		public void FilledInProspect()
		{
			Prospect prospect = new Prospect();

			prospect.FName = "jill";
			prospect.LName = "doe";
			prospect.MoMaName = "rigg";
			prospect.Address1 = "123 open st";
			prospect.City = "san francisco";
			prospect.State = "CA";
			prospect.Gender = "female";
			prospect.DOB = new DateTime( 1988, 10, 1 );
			prospect.Phone1 = "6509998833";
			prospect.Phone1Type = "home";
			prospect.PIN = "8899";
			prospect.SSN = "234884576";
			prospect.Occupation = "HIKER";
			prospect.Employer = "REI";
			prospect.EmployerPhone = "4152233344";

			Assert.DoesNotThrow( () => fieldValidator.ValidateNames( prospect ) );
			Assert.DoesNotThrow( () => fieldValidator.ValidateAddress( prospect ) );
			Assert.DoesNotThrow( () => fieldValidator.ValidateDOB( prospect ) );
			Assert.DoesNotThrow( () => fieldValidator.ValidatePhone( prospect ) );
			Assert.DoesNotThrow( () => fieldValidator.ValidateSSN( prospect ) );
			Assert.DoesNotThrow( () => fieldValidator.ValidateEmployment( prospect ) );
		}

		[Test]
		public void SSNOK()
		{
			Prospect prospect = new Prospect();
			Assert.DoesNotThrow( () => fieldValidator.ValidateSSN( prospect ) );

			prospect.SSN = "01234";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateSSN( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_SSN );

			prospect.SSN = "000223333";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateSSN( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_SSN );

			prospect.SSN = "111002222";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateSSN( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_SSN );

			prospect.SSN = "111220000";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateSSN( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_SSN );

			prospect.SSN = "666112222";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateSSN( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_SSN );

			prospect.SSN = "123456789";
			Assert.DoesNotThrow( () => fieldValidator.ValidateSSN( prospect ) );
		}

		[Test]
		public void EmploymentOK()
		{
			Prospect prospect = new Prospect();
			Assert.DoesNotThrow( () => fieldValidator.ValidateEmployment( prospect ) );

			prospect.Occupation = "TYPIST 3";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateEmployment( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_OCCUPATION );

			prospect.Occupation = "WALKER";
			Assert.DoesNotThrow( () => fieldValidator.ValidateEmployment( prospect ) );

			prospect.Employer = "FANCY! #34";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateEmployment( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_EMPLOYER_NAME );

			prospect.Employer = "FANCY 54";
			Assert.DoesNotThrow( () => fieldValidator.ValidateEmployment( prospect ) );

			prospect.EmployerPhone = "0123456789";
			MinorCodeMatch<Customer.Contract.BizCustomerException>( () => fieldValidator.ValidateEmployment( prospect ), Customer.Contract.BizCustomerException.INVALID_CUSTOMER_DATA_EMPLOYER_PHONE );

			prospect.EmployerPhone = "6509897788";
			Assert.DoesNotThrow( () => fieldValidator.ValidateEmployment( prospect ) );
		}

		[Test]
		public void IDTest()
		{
			Prospect prospect = new Prospect();
			

			Assert.DoesNotThrow( () => fieldValidator.ValidateID( prospect.ID, null ) );
			
			prospect.ID = new Identification
			{
				 Country="UNITED STATES"
			};
		}

		[Test]
		public void SearchCriteriaOK()
		{
			CustomerSearchCriteria cr = null;
			MinorCodeMatch<BizCustomerException>( () => fieldValidator.ValidateCriteria( cr ), BizCustomerException.INVALID_CUSTOMER_SEARCH_NO_CRITERIA_PROVIDED );

			cr = new CustomerSearchCriteria();
			MinorCodeMatch<BizCustomerException>( () => fieldValidator.ValidateCriteria( cr ), BizCustomerException.INVALID_CUSTOMER_SEARCH_NOT_ENOUGH_CRITERIA_PROVIDED );

			cr.FirstName = "bill";
			MinorCodeMatch<BizCustomerException>( () => fieldValidator.ValidateCriteria( cr ), BizCustomerException.INVALID_CUSTOMER_SEARCH_NOT_ENOUGH_CRITERIA_PROVIDED );

			cr.LastName = "barns";
			Assert.DoesNotThrow( () => fieldValidator.ValidateCriteria( cr ) );

			cr.FirstName = "";
			cr.LastName = "";
			cr.PhoneNumber = "1234567";
			cr.GovernmentIDNumber = "D3456";
			Assert.DoesNotThrow( () => fieldValidator.ValidateCriteria( cr ) );

			cr.PhoneNumber = "";
			cr.GovernmentIDNumber = "";
			cr.Cardnumber = "1029384756";
			Assert.DoesNotThrow( () => fieldValidator.ValidateCriteria( cr ) );
		}
	}
}
