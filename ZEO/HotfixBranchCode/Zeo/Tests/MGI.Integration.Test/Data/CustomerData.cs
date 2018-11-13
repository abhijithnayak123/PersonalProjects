using MGI.Channel.DMS.Server.Data;
using MGI.Channel.Shared.Server.Data;
using MGI.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Integration.Test.Data
{
	public partial class IntegrationTestData
	{
		public static Prospect GetCustomerProspect(ChannelPartner channelPartner )
		{
			Prospect prospect = new Prospect()
			{
				Address1 = "FORT COLLINS,#121",
				Address2 = "COLORADO TOURISM OFFICE",
				City = "DENVER",
				FName = "JAMES",
				MName = "CHRIST",
				Gender = "MALE",
				LName = channelPartner.Name,
				LName2 = "HENRY",
				MoMaName = "JACALBERT",
				Phone1 = "3039637881",
				Phone1Type = "Home",
				PostalCode = "80205",
				SSN = RandomNumber(8),
				State = "CO",
				ProfileStatus = ProfileStatus.Active,
				ChannelPartnerId = channelPartner.rowguid,
				DateOfBirth = new DateTime(1950, 10, 10),
				ID = new Identification()
				{
					Country = "UNITED STATES",
					CountryOfBirth = "US",
					ExpirationDate = new DateTime(2019, 10, 10),
					IssueDate = new DateTime(2011, 10, 10),
					GovernmentId = "340007237",
					IDType = "PASSPORT",
					IDTypeName = "",
					State = ""
				},
				Occupation = "Software Engineer",
				Employer = "Opteamix",
				EmployerPhone = "6509878765"
			};

			Prospect prospectTCF = new Prospect()
			{
				Address1 = "FORT COLLINS,#121",
				Address2 = "COLORADO TOURISM OFFICE",
				City = "DENVER",
				FName = GetName("JAMES"),
				MName = GetName("CHRIST"),
				Gender = "MALE",
				LName = channelPartner.Name,
				LName2 = "HENRY",
				MoMaName = "JACALBERT",
				Phone1 = "3039637881",
				Phone1Type = "Home",
				PostalCode = "80205",
				SSN = RandomNumber(8),
				State = "CO",
				Email = "JAMES@AABC.COM",
				ProfileStatus = ProfileStatus.Active,
				ChannelPartnerId = channelPartner.rowguid,
				DateOfBirth = new DateTime(1950, 10, 10),
				ID = new Identification()
				{
					Country = "UNITED STATES",
					CountryOfBirth = "US",
					ExpirationDate = new DateTime(2019, 10, 10),
					IssueDate = new DateTime(2011, 10, 10),
					GovernmentId = "ASDFGHJKIU",
					IDType = "PASSPORT",
					IDTypeName = "",
					State = ""
				},
				Occupation = "0014",
				Employer = "Opteamix",
				EmployerPhone = "6509878765"
			};

			switch(channelPartner.Id)
			{
				case 34:
					return prospectTCF;
				default:
					return prospect;
			}
		}

		private static string RandomNumber(int length)
		{
			string ssn = "6";
			Random random = new Random();
			for (int i = 0; i < length; i++)
			{
				ssn += Convert.ToString(random.Next(0, 9));
			}
			return ssn;
		}

		internal static string GetName(string name)
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			Random random = new Random();
			StringBuilder str = new StringBuilder();
			str = str.Append(name);
			for (int i = 0; i < 5; i++ )
			{
				str.Append(chars[random.Next(chars.Length)]);
			}			
			return str.ToString();
		}
	}
}
