using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGI.Cxn.Partner.TCF.Data
{
	public class Customer 
	{
		public string ChannelPartnerName { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string SecondLastName { get; set; }
		public string DateOfBirth { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string CountryofBirth { get; set; }
		public string Zip { get; set; }
		public string Phone1 { get; set; }
		public string Ph1Type1 { get; set; }
		public string Phone2 { get; set; }
		public string Ph2Type2 { get; set; }
		public string Ph2Prov { get; set; }
		public string Email { get; set; }
		public string Occupation { get; set; }
		public string CustType { get; set; }
		public string SSN { get; set; }
		public string TaxCd { get; set; }
		public long AlloyID { get; set; }
		public string ClientCustId { get; set; }
		public string Gender { get; set; }
		public bool CustInd { get; set; }
		public string Identification { get; set; }
		public Nullable<DateTime> IssueDate { get; set; }
		public Nullable<DateTime> ExpirationDate { get; set; }
		public string IdType { get; set; }
		public string Notes { get; set; }
		public string ClientID { get; set; }
		public string LegalCode { get; set; }
		public string PrimaryCountryCitizenship { get; set; }
		public string SecondaryCountryCitizenship { get; set; }
		public long CustomerSessionId { get; set; }
		public string Maiden { get; set; }
		public string IdIssuer { get; set; }
		public string IdIssuerCountry { get; set; }
		public string IdIssuerCountryCode { get; set; }
		public string OccupationDescription { get; set; }
		public string EmployerName { get; set; }
		public string EmployerPhoneNum { get; set; }
		public string IDCode { get; set; }
	}
}
