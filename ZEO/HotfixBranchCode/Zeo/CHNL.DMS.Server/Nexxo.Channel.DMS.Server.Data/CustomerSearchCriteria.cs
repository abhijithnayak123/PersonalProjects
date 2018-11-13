using System;
using System.Runtime.Serialization;
using System.Text;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class CustomerSearchCriteria
	{
		public CustomerSearchCriteria() { }

		[DataMember]
		public string FirstName { get; set; }
		[DataMember]
		public string LastName { get; set; }
		[DataMember]
		public string PhoneNumber { get; set; }
		[DataMember]
		public DateTime DateOfBirth { get; set; }
		[DataMember]
		public string CardNumber { get; set; }
		[DataMember]
		public string GovernmentId { get; set; }
        [DataMember]
		public string SSN { get; set; }
		[DataMember]
		public bool IsIncludeClosed { get; set; }

		
		/// <summary>
		/// Added this new method to implement the SQL Injection. US#1788
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
        //private string safeSQLString(string s)
        //{
        //    if (s != null)
        //        return s.Replace("\\", "").Replace("'", "''");
        //    else
        //        return null;

        //}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine( "Customer Search Request:" );
			sb.AppendLine( string.Format( "	FirstName: {0}", NexxoUtil.safeSQLString(FirstName) ) );
            sb.AppendLine(string.Format("	LastName: {0}", NexxoUtil.safeSQLString(LastName)));
            sb.AppendLine(string.Format("	PhoneNumber: {0}", NexxoUtil.safeSQLString(PhoneNumber)));
			sb.AppendLine(string.Format("	DateOfBirth: {0}", DateOfBirth.ToShortDateString()));
			//sb.AppendLine( string.Format( "	Cardnumber: {0}", Cardnumber ) );
			sb.AppendLine(string.Format("	CardNumber: {0}", NexxoUtil.safeSQLString(NexxoUtil.cardLastFour(CardNumber))));
			sb.AppendLine(string.Format(" GovernmentId: {0}{1}", "****", NexxoUtil.getLastFour((GovernmentId))));

			//sb.AppendLine(string.Format(" SSN Number : {0}", safeSQLString(SSN)));
           //sb.AppendLine(string.Format(" SSN Number After Masking : {0}", "XXX-XX-XXXX"));
            sb.AppendLine(string.Format("SSN After Masking : {0}{1}", "XXX-XX-", NexxoUtil.getLastFour(SSN)));
            return sb.ToString();
		}

       
	}
}
