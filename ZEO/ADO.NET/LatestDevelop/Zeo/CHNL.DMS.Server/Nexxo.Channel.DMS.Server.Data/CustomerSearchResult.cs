using System;
using System.Runtime.Serialization;
using MGI.Common.Util;

namespace MGI.Channel.DMS.Server.Data
{
	[DataContract]
	public class CustomerSearchResult
	{
		public CustomerSearchResult() { }

		[DataMember]
		public string AlloyID { get; set; }
		[DataMember]
		public string FullName{get; set;}
		[DataMember]
		public string PhoneNumber{get; set;}
		[DataMember]
		public string Address{get; set;}
		[DataMember]
		public DateTime? DateOfBirth { get; set; }
		[DataMember]
		public string MothersMaidenName{get; set;}
		[DataMember]
		public string GovernmentId { get; set; }
		[DataMember]
		public string CardNumber { get; set; }
		[DataMember]
		public string SSN{get; set; }
        [DataMember]
        public string ProfileStatus { get; set; }

       

		override public string ToString()
		{
			string str = string.Empty;
			str = string.Concat(str, "AlloyID = ", NexxoUtil.MaskSensitiveData(AlloyID, "AlloyID"), "\r\n");
			str = string.Concat(str, "FullName = ", FullName, "\r\n");
			str = string.Concat(str, "PhoneNumber = ", PhoneNumber, "\r\n");
			str = string.Concat(str, "Address = ", Address, "\r\n");
			str = string.Concat(str, "DateOfBirth = ", DateOfBirth, "\r\n");
			str = string.Concat(str, "MothersMaidenName = ", MothersMaidenName, "\r\n");
			str = string.Concat(str, "GovernmentId = ", NexxoUtil.MaskSensitiveData(GovernmentId, "GovernmentId"), "\r\n");
			str = string.Concat(str, "CardNumber = ", NexxoUtil.cardLastFour(CardNumber), "\r\n");
            str = string.Concat(str, "SSN = ", NexxoUtil.MaskSensitiveData(SSN, "SSN"), "\r\n");
		    str = string.Concat(str, "ProfileStatus =", ProfileStatus, "\r\n");
			return str;
		}
	}
}
