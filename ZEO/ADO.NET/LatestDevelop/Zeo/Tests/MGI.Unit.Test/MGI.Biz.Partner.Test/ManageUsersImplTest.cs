using NUnit.Framework;
using MGI.Biz.Partner.Contract;
using MGI.Unit.Test;
using MGI.Biz.Partner.Data;
using MGI.Common.Util;
using System.Collections.Generic;


namespace MGI.Biz.Customer.Test
{
    [TestFixture]
    public class ManageUsersImplTest : BaseClass_Fixture
    {
		public IManageUsers BIZPartnerUserService { get; set; }

		[Test]
		public void Can_Get_User_By_UserId()
		{
			long agentSessionId = 1000000000;
			int userId = 500001;
			MGIContext mgiContext = new MGIContext() { };

			UserDetails userDetails = BIZPartnerUserService.GetUser(agentSessionId, userId, mgiContext);

			Assert.IsNotNull(userDetails);
		}

		[Test]
		public void Can_Get_All_User()
		{
			long agentSessionId = 1000000000;
			long locationId = 1000000000;
			MGIContext mgiContext = new MGIContext() { };

			List<UserDetails> userDetails = BIZPartnerUserService.GetUsers(agentSessionId, locationId, mgiContext);

			Assert.AreNotEqual(userDetails.Count, 0);
		}
    }
}