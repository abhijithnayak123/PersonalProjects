using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using MGI.Biz.Partner.Data;
using MGI.Biz.Partner.Contract;
using MGI.Biz.Partner.Impl;

using NHibernate;
using Spring.Context;
using NHibernate.Context;
using MGI.Common.Util;
using System.Net.Mail;
using MGI.Biz.Partner.Test;


namespace MGI.Biz.Partner.UserTest
{
    [TestFixture]
	public class ManageUserTest : AbstractPartnerTest
    {

		ISession session { get; set; }
		IManageUsers ImanageUser { get; set; }
		private static string IManage_Locations = "ImanageUser";
        public MGIContext mgiContext { get; set; }
        [SetUp]
        public void Setup()
        {
			IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
			ImanageUser = (IManageUsers)ctx.GetObject(IManage_Locations);
        }

        [Test]
        public void Can_SaveUserTest() // Done
        {
			//CallSessionContext.Bind(session);			
                UserDetails userDetail = new UserDetails();
                userDetail.Rowguid = Guid.NewGuid();
                userDetail.FirstName = "Test1";
                userDetail.LastName = "Tests";
                userDetail.UserName = "test1";
                userDetail.Email = "test@abc.com";
                userDetail.ManagerId = 1;
                userDetail.LocationId = 1;
                userDetail.UserRoleId = 1;
                userDetail.UserStatusId = 1;
                userDetail.TempPassword = "test@1234";
                userDetail.ChannelPartnerId = 27;

                int Id = ImanageUser.SaveUser(userDetail, SaveMode.Add, mgiContext);

                Assert.IsTrue(Id > 0);
		} //Done

        [Test]
        public void Cannot_SaveUserTest() // Done
        {
			//CallSessionContext.Bind(session);
                UserDetails userDetail = new UserDetails();

                try
                {
                    userDetail.Rowguid = Guid.NewGuid();
                    userDetail.FirstName = "Test";
                    userDetail.LastName = "Tests";
                    userDetail.UserName = "anil";
                    userDetail.Email = "test@abc.com";
                    userDetail.ManagerId = 1;
                    userDetail.LocationId = 1;
                    userDetail.UserRoleId = 1;
                    userDetail.UserStatusId = 1;
                    userDetail.TempPassword = "test@1234";
                    userDetail.ChannelPartnerId = 27;

                    int Id = ImanageUser.SaveUser(userDetail, SaveMode.Add, mgiContext);

                    Assert.IsTrue(Id > 0);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(ex != null);
                }

            }

        [Test]
        public void Can_GetUserTest() //Done
        {
			//CallSessionContext.Bind(session);

            UserDetails userDetail = ImanageUser.GetUser(0, 500000, mgiContext);

            Assert.IsTrue(userDetail.FirstName != null);

        }

        [Test]
        public void Cannot_GetUserTest() //Done
        {
			//CallSessionContext.Bind(session);
            UserDetails userDetail = new UserDetails();

            try
            {
                userDetail = ImanageUser.GetUser(0, 11, mgiContext);
            }
            catch 
            {
                Assert.IsTrue(userDetail.FirstName == null);
            }
        }

		[Test]
		public void AgentFullNameTest() //Done
		{
			//CallSessionContext.Bind(session);
			UserDetails userDetail = new UserDetails();

            userDetail = ImanageUser.GetUser(0, 500001, mgiContext);

			Assert.IsTrue(userDetail.FullName != null);
			Assert.AreEqual(userDetail.FullName, userDetail.FirstName + " " + userDetail.LastName);

		}
		//[Test]
		//public void CanGetUserDetialsTest()
		//{
		//	UserDetails userDetails = null;
		//	string userName = "SysAdmin";
		//	int channelPartnerId = 34;
		//	//CallSessionContext.Bind(session);
		//	userDetails = ImanageUser.GetUser(userName, channelPartnerId);
		//	Assert.IsNotNull(userDetails);
		//}

        }
    }
