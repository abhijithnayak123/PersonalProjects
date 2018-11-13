using MGI.Channel.DMS.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Integration.Test.Data
{
	public partial class IntegrationTestData
	{
		public static AgentSSO GetSSOAgentData(string channelPartnerName)
		{
			var ssoAgent = new AgentSSO()
				{
					FirstName = channelPartnerName,
					LastName = "Integration",
					UserName = channelPartnerName + "agent",
					Role = new UserRole() { role = "Teller", Id = 4 },
					BusinessDate = DateTime.Now
				};
			return ssoAgent;
		}

		public static Dictionary<string, object> GetSSOAttributes()
		{			
			 Dictionary<string, object> ssoAttributes = new Dictionary<string, object>()
			{
				{"UserID", "ZeoMGI"},
				{"TellerNum", "98001"},
				{"BranchNum", "43"},
				{"BankNum", "99"},
				{"DPSBranchID", "12392"},
				{"LawsonID", "104"},
				{"LU", "23B7"},
				{"CashDrawer", "980"},
				{"AmPmInd", "A"},
				{"MachineName", "001-MGIw7"},
				{"BusinessDate", "2015-06-10"}
			};			 
				return ssoAttributes;						
		}
	}
}
