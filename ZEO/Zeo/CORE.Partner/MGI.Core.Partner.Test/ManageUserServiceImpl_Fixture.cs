using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;

using NUnit.Framework;
using Spring.Context;
using Spring.Context.Support;


namespace MGI.Core.Partner.Test
{
    [TestFixture]
    public class ManageUserServiceImpl_Fixture : AbstractPartnerTest
    {
        Guid newUserID = Guid.NewGuid();
		public IManageUsers UserManagementRepo { get; set; }
      	 
		//[SetUp]
		//public void Setup()
		//{
		//	IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
		//	UserManagement = (IManageUsers)ctx.GetObject("UserManagement");
		//	session = (ISession)ctx.GetObject("session");
		//}

        UserDetails userInfo = new UserDetails();

		[Test]
		public void Can_Add_New_User_Test()
		{
			//userRepo.SessionFactory = NHibernateHelper.SessionFactory;
			//manageUser.UserManagementRepo = userRepo;

			userInfo.Rowguid = newUserID;
			userInfo.LastName = "Doyle";
			userInfo.FirstName = "Conan";
			userInfo.ManagerId = 36;
			userInfo.PhoneNumber = "9886533567";
			userInfo.UserRoleId = 1;
			userInfo.UserStatusId = 2;
			userInfo.LocationId = 10;
			userInfo.IsEnabled = false;

			int userId;
			using (ISession session = NHibernateHelper.OpenSession())
			{
				CallSessionContext.Bind(session);
				using (ITransaction txn = session.BeginTransaction())
				{
					userId = UserManagementRepo.AddUser(userInfo);
					txn.Commit();
				}
				Assert.IsTrue(userId > 0);
				Assert.IsNotNull(userId);
			}

		}

		//[Test]
		//[ExpectedException(typeof(NHibernate.Exceptions.GenericADOException))]
		//public void Should_Not_Insert_NewUser_With_Mandatory_Fields_As_Null()
		//{
		//	userInfo.LastName = "Rajkumar";
		//	userInfo.FirstName = "Ranjith";
		//	userInfo.ManagerId = 1;
		//	userInfo.PhoneNumber = "9865476";
		//	//userInfo.UserRoleId = 1;
		//	//userInfo.UserStatusId = 2;
		////	userInfo.LocationId = 10;
		////	userInfo.IsEnabled = false;

		//	 bool userInsertion;
		//	using (ISession session = NHibernateHelper.OpenSession())
		//	{
		//		CallSessionContext.Bind(session);
		//		using (ITransaction txn = session.BeginTransaction())
		//		{
		//			userInsertion = Convert.ToBoolean(UserManagementRepo.AddUser(userInfo));
		//			txn.Commit();
		//		}

		//		Assert.IsFalse(false, "NHibernate.Exceptions.GenericADOException was expected");
				
		//		Assert.IsNotNull(userInsertion!= null);
				
		//	}
		//}

	   [Test]
        public void Can_Update_User_Test()
        {

           //     CallSessionContext.Bind(session);
			userInfo = UserManagementRepo.GetUser(500001);
                userInfo.Email = "arthur.Morris@opcm.com";
                userInfo.DTServerLastModified = DateTime.Now;

                int userId;
				//using (ITransaction txn = session.BeginTransaction())
				//{
				//	userId = UserManagementRepo.UpdateUser(userInfo);
				//	txn.Commit();
				//}
				userId = UserManagementRepo.UpdateUser(userInfo);
                Assert.IsNotNull(userId);

        }

		//[Test]

		//[ExpectedException(typeof(ArgumentNullException))]
		//public void Should_Not_Save_User_With_Mandatory_Fields_As_Null()
		//{
		//	userInfo = null;

		//	int userId;

		//	//CallSessionContext.Bind(session);
		//	//using (ITransaction txn = session.BeginTransaction())
		//	//{
		//	//	userId = UserManagementRepo.UpdateUser(userInfo);
		//	//	txn.Commit();
		//	//}
		//	userId = UserManagementRepo.UpdateUser(userInfo);
		//	Assert.IsNotNull(userId);
		//}

        [Test]
        public void Can_Get_User_Test()
        {
				//CallSessionContext.Bind(session);
				//using (ITransaction txn = session.BeginTransaction())
				//{
				//	userInfo = UserManagementRepo.GetUser(2);
				//	txn.Commit();
				//}
			userInfo = UserManagementRepo.GetUser(200000);
			Assert.AreEqual(userInfo.FirstName, "System");
                Assert.IsNotNull(userInfo.Rowguid);
        }
    } 
}
