using System;
using System.Text;

namespace MGI.Biz.Customer.Data
{
	public class CustomerSearchCriteria
	{
		public string FirstName;
		public string LastName;
		public string PhoneNumber;
		public DateTime DateOfBirth;
		public string CardNumber;
		public string GovernmentId;
        public string SSN;
		public bool IsIncludeClosed { get; set; }

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine( "Customer Search Request:" );
			sb.AppendLine( string.Format( "	FirstName: {0}", FirstName ) );
			sb.AppendLine( string.Format( "	LastName: {0}", LastName ) );
			sb.AppendLine( string.Format( "	PhoneNumber: {0}", PhoneNumber ) );
			sb.AppendLine(string.Format("	DateOfBirth: {0}", DateOfBirth.ToShortDateString()));
			sb.AppendLine( string.Format( "	Cardnumber: {0}", CardNumber ) );
			sb.AppendLine(string.Format(" GovernmentId: {0}", GovernmentId));
            sb.AppendLine(string.Format(" SSN Number : {0}", SSN));
			sb.AppendLine(string.Format(" Is Include Closed : {0}", IsIncludeClosed));
			return sb.ToString();
		}
	}
}
