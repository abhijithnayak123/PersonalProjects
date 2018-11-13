using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace ChexarIO
{
	public class ChexarCustomerIO
	{
		//Chexar default values:  string - "", int - 0, DateTime - 1/1/1900
		public int Badge = 0;
		public string FName = string.Empty;
		public string LName = string.Empty;
		public string ITIN = string.Empty;
		public string SSN = string.Empty;
		public DateTime? DateOfBirth = new DateTime(1900, 1, 1);
		public string Address1 = string.Empty;
		public string Address2 = string.Empty;
		public string City = string.Empty;
		public string State = string.Empty;
		public string Zip = string.Empty;
		public string Phone = string.Empty;
		public string Occupation = string.Empty;
		public string Employer = string.Empty;
		public string EmployerPhone = string.Empty;
		public ChexarIDTypes IDType = ChexarIDTypes.Unknown;
		public string IDCountry = string.Empty;
		public DateTime IDExpDate = new DateTime(1900, 1, 1);
		public string GovernmentId = string.Empty;
		public byte[] IDImage = null;
		public long CardNumber = long.MinValue;
		public int CustomerScore = 100;
		public string IDCode = string.Empty;

		public ChexarCustomerIO() { }

		public ChexarCustomerIO(XElement customerInfo)
		{
			Badge = ChexarXMLHelper.GetIntToken(customerInfo, "badgeno");
			FName = ChexarXMLHelper.GetXMLValue(customerInfo, "fname");
			LName = ChexarXMLHelper.GetXMLValue(customerInfo, "lname");
			ITIN = ChexarXMLHelper.GetXMLValue(customerInfo, "itin");
			SSN = ChexarXMLHelper.GetXMLValue(customerInfo, "ssn");
			DateOfBirth = ChexarXMLHelper.GetDateTimeToken(customerInfo, "bday");
			Address1 = ChexarXMLHelper.GetXMLValue(customerInfo, "address");
			Address2 = ChexarXMLHelper.GetXMLValue(customerInfo, "address2");
			City = ChexarXMLHelper.GetXMLValue(customerInfo, "city");
			State = ChexarXMLHelper.GetXMLValue(customerInfo, "state");
			Zip = ChexarXMLHelper.GetXMLValue(customerInfo, "zip");
			Phone = ChexarXMLHelper.GetXMLValue(customerInfo, "hphone");
			Occupation = ChexarXMLHelper.GetXMLValue(customerInfo, "occupation");
			Employer = ChexarXMLHelper.GetXMLValue(customerInfo, "employer");
			EmployerPhone = ChexarXMLHelper.GetXMLValue(customerInfo, "custworkempno");
			IDType = (ChexarIDTypes)ChexarXMLHelper.GetIntToken(customerInfo, "doc1type");
			IDCountry = ChexarXMLHelper.GetXMLValue(customerInfo, "doc1issuedfor");
			IDExpDate = ChexarXMLHelper.GetDateTimeToken(customerInfo, "doc1date");
			GovernmentId = ChexarXMLHelper.GetXMLValue(customerInfo, "doc1no");

			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			IDImage = encoding.GetBytes(ChexarXMLHelper.GetXMLValue(customerInfo, "idcardimage"));
		}
	}
}
