using System;
using System.Text.RegularExpressions;

using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Contract;

using IdType = MGI.Core.Partner.Data.NexxoIdType;
using MGI.Common.Util;

namespace MGI.Biz.Partner.Impl
{
	public class ProspectFieldValidator : IProspectFieldValidator
	{
		public void ValidateNames( Prospect prospect )
		{
			Regex nameRgx = new Regex( "[^A-Za-z\\-' ]" );

			if (!string.IsNullOrEmpty( prospect.FName ) && nameRgx.IsMatch( prospect.FName ?? "" ) )
				throw new BizCustomerException(BizCustomerException.INVALID_CUSTOMER_DATA_FNAME, InvalidDataMessage(prospect.FName));
			if ( !string.IsNullOrEmpty( prospect.MName ) && nameRgx.IsMatch( prospect.MName ?? "" ) )
				throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_MNAME, InvalidDataMessage( prospect.MName ) );
			if ( !string.IsNullOrEmpty( prospect.LName ) && nameRgx.IsMatch( prospect.LName ?? "" ) )
				throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_LNAME, InvalidDataMessage( prospect.LName ) );
			if ( !string.IsNullOrEmpty( prospect.LName2 ) && nameRgx.IsMatch( prospect.LName2 ?? "" ) )
				throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_LNAME2, InvalidDataMessage( prospect.LName2 ) );
			if ( !string.IsNullOrEmpty( prospect.MoMaName ) && nameRgx.IsMatch( prospect.MoMaName ?? "" ) )
				throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_MOMANAME, InvalidDataMessage( prospect.MoMaName ) );
		}

		public void ValidateAddress( Prospect prospect )
		{
			//Regex addressRgx = new Regex( "^P\\.*O\\.*.*BOX.*[\\d-]{1,9}" );
			Regex cityRgx = new Regex( "[^A-Za-z .-]" );
			Regex zipRgx = new Regex("\\d{5}");

			// remove no PO Box validation
			//if ( !string.IsNullOrEmpty( prospect.Address1 ) && addressRgx.IsMatch( prospect.Address1.ToUpper() ?? "" ) )
			//    throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_ADDRESS1, InvalidDataMessage( prospect.Address1 ) );
			//if ( !string.IsNullOrEmpty( prospect.Address2 ) && addressRgx.IsMatch( prospect.Address2.ToUpper() ?? "" ) )
			//    throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_ADDRESS2, InvalidDataMessage( prospect.Address2 ) );
			if ( !string.IsNullOrEmpty( prospect.City ) && cityRgx.IsMatch( prospect.City.ToUpper() ?? "" ) )
				throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_CITY, InvalidDataMessage( prospect.City ) );
			if (!string.IsNullOrEmpty(prospect.PostalCode) && !zipRgx.IsMatch(prospect.PostalCode??""))
				throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_POSTAL_CODE, InvalidDataMessage( prospect.PostalCode ) );
		}

		public void ValidatePhone( Prospect prospect )
		{
			if ( _validatePhone(prospect.Phone1))
				throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_PHONE1, InvalidDataMessage( prospect.Phone1 ) );
			if ( _validatePhone(prospect.Phone2))
				throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_PHONE2, InvalidDataMessage( prospect.Phone2 ) );
		}

		private bool _validatePhone( string phone )
		{
			Regex phoneRgx1 = new Regex( "^[2-9]\\d{9}$" );
			Regex phoneRgx2 = new Regex( "2{9}|3{9}|4{9}|5{9}|6{9}|7{9}|8{9}|9{9}" );

			return ( !string.IsNullOrEmpty( phone ) && ( !phoneRgx1.IsMatch( phone ?? "" ) || phoneRgx2.IsMatch( phone ?? "" ) ) );
		}

		public void ValidateEmail( Prospect prospect )
		{
			Regex emailRgx = new Regex(@"^(([^<>()[\]\.,;:\s@""]+(\.[^<>()[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$");

			if ( !string.IsNullOrEmpty( prospect.Email ) && ( !emailRgx.IsMatch( prospect.Email ?? "" ) ) )
				throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_EMAIL, InvalidDataMessage( prospect.Email ) );
		}

		public void ValidateDOB(Prospect prospect, int minimumAge)
		{
			//AL-1626
			//Starts Here
			int customerAge = 0;

			if (prospect.DateOfBirth != null)
				customerAge = NexxoUtil.GetCustomerAgeByDateOfBirth(Convert.ToDateTime(prospect.DateOfBirth));

			if (prospect.DateOfBirth != null && prospect.DateOfBirth != DateTime.MinValue && customerAge < minimumAge)
			{
				throw new BizCustomerException(BizCustomerException.INVALID_CUSTOMER_DATA_DOB, InvalidDataMessage(prospect.DateOfBirth));
			}
			//Ends Here
		}

		public void ValidateSSN( Prospect prospect )
		{
			Regex ssnRgx = new Regex( "^(?!000)(?!666)(?!9)[0-9]{3}[ -]?(?!00)[0-9]{2}[ -]?(?!0000)[0-9]{4}$" );

			if ( !string.IsNullOrEmpty( prospect.SSN ) && !ssnRgx.IsMatch( prospect.SSN ?? "" ) )
				throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_SSN, InvalidDataMessage( prospect.SSN ) );
		}

		public void ValidateEmployment( Prospect prospect )
		{
            //Regex occRgx = new Regex( "[^A-Za-z ]" );
            //Regex empRgx = new Regex( "[^A-Za-z0-9\\-' ]" );

            Regex Rgx = new Regex("[^A-Za-z0-9\\-_' ]");

            if (!string.IsNullOrEmpty(prospect.Occupation) && Rgx.IsMatch(prospect.Occupation ?? ""))
				throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_OCCUPATION, InvalidDataMessage(prospect.Occupation) );
            if (!string.IsNullOrEmpty(prospect.Employer) && Rgx.IsMatch(prospect.Employer ?? ""))
				throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_EMPLOYER_NAME, InvalidDataMessage(prospect.Employer) );
			if ( _validatePhone( prospect.EmployerPhone ) )
				throw new BizCustomerException(BizCustomerException.INVALID_CUSTOMER_DATA_EMPLOYER_PHONE, InvalidDataMessage(prospect.EmployerPhone));
		}

		public void ValidateID( Identification id, IdType idType )
		{
           
            if ( idType != null)
                if(idType.HasExpirationDate)
                if (!Regex.IsMatch( id.GovernmentId, idType.Mask ) ||
                    ((idType.HasExpirationDate && (id.IssueDate != DateTime.MinValue && (id.ExpirationDate == DateTime.MinValue || id.ExpirationDate < DateTime.Today )))  //namit
                    ||
                    ( !idType.HasExpirationDate && id.ExpirationDate != DateTime.MinValue ) ) )
                    
                    throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_DATA_INVALID_ID,
                        string.Format( "ID# {0} (Exp:{1}) does not match type requirements: Format {2} (Exp required: {3})",
                            id.GovernmentId,
                            id.ExpirationDate,
                            idType.Mask,
                            idType.HasExpirationDate ) );
		}

        //public void ValidateCriteria( CustomerSearchCriteria criteria )
        //{
        //    if ( criteria != null )
        //    {
        //        if ( string.IsNullOrEmpty( criteria.Cardnumber )
        //            && ( BoolToInt( criteria.DOB > DateTime.MinValue ) + BoolToInt( !string.IsNullOrEmpty( criteria.FirstName ) )
        //                 + BoolToInt( !string.IsNullOrEmpty( criteria.LastName ) ) + BoolToInt( !string.IsNullOrEmpty( criteria.PhoneNumber ) )
        //                 + BoolToInt( !string.IsNullOrEmpty( criteria.GovernmentIDNumber ) ) ) < 2 )
        //            throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_SEARCH_NOT_ENOUGH_CRITERIA_PROVIDED );
        //    }
        //    else
        //        throw new BizCustomerException( BizCustomerException.INVALID_CUSTOMER_SEARCH_NO_CRITERIA_PROVIDED );
        //}

		private int BoolToInt( bool flag )
		{
			return flag ? 1 : 0;
		}

		private string InvalidDataMessage( DateTime? data )
		{
			if (data != null)
				return InvalidDataMessage(data.Value.ToString("yyyy-MM-dd"));
			else
				return string.Empty;
		}
		private string InvalidDataMessage( string data )
		{
			return string.Format( "INVALID DATA: {0}", data );
		}
	}
}
