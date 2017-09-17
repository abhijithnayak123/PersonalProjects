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

		//[Test]
		//[ExpectedException(typeof(InvalidOperationException))]
		//public void Should_Not_Get_User_for_NonExistingID_Test()
		//{
			//using (session)
			//{
			//	CallSessionContext.Bind(session);
			//	using (ITransaction txn = session.BeginTransaction())
			//	{
			//		userInfo = UserManagementRepo.GetUser(9999);
			//		txn.Commit();
			//	}
			//userInfo = UserManagementRepo.GetUser(9999);
			//Assert.AreSame("Could not find User 9999", userInfo);
			//Assert.Throws("Could not find User 9999", userInfo);
				//Assert.IsNullOrEmpty(userInfo.FirstName);
				//Assert.IsNullOrEmpty(userInfo.UserName);
               
            
       // }

        [Test]
        public void Can_Get_User_Status_Test()
        {
            //userStatusRepo.SessionFactory = NHibernateHelper.SessionFactory;
            //manageUser.UserStatusRepo = userStatusRepo;

							//using (session)//(ISession session = NHibernateHelper.OpenSession())
							//{
							//	List<UserStatus> userStatusList = new List<UserStatus>();
							//	CallSessionContext.Bind(session);
							//	using (ITransaction txn = session.BeginTransaction())
							//	{
							//		userStatusList = UserManagementRepo.GetUserStatuses();
							//		txn.Commit();
							//	}
                //Guid savedID = userRepo.FindBy(newUserID).Rowguid;
				List<UserStatus> userStatusList = new List<UserStatus>();
				userStatusList = UserManagementRepo.GetUserStatuses();
                Assert.AreEqual(userStatusList.Count, 3);
            }
        
        [Test]
        public void Can_Get_User_Roles_Test()
        {
            //userRoleRepo.SessionFactory = NHibernateHelper.SessionFactory;
            //manageUser.UserRoleRepo = userRoleRepo;

			//using (ISession session = NHibernateHelper.OpenSession())
			//{
			//	List<UserRole> userRoleList = new List<UserRole>();
			//	CallSessionContext.Bind(session);
			//	using (ITransaction txn = session.BeginTransaction())
			//	{
			//		userRoleList = UserManagementRepo.GetUserRoles();
			//		txn.Commit();
			//	}
			//	//Guid savedID = userRepo.FindBy(newUserID).Rowguid;
                
			//}
			List<UserRole> userRoleList = new List<UserRole>();
			userRoleList = UserManagementRepo.GetUserRoles();
			Assert.AreEqual(userRoleList.Count, 4);
        }

        [Test]
        public void Can_Search_Users_FirstName_LastName_Test()
        {
            //userRepo.SessionFactory = NHibernateHelper.SessionFactory;
            //manageUser.SearchUserRepo = userRepo;

            UserSearchCriteria criteria = new UserSearchCriteria();
			criteria.FirstName = "System";
			criteria.LastName = "Administrator";
			criteria.ChannelPartnerId = 28;
            using (ISession session = NHibernateHelper.OpenSession())
            {
                List<UserDetails> userSearchList = new List<UserDetails>();
                CallSessionContext.Bind(session);
                using (ITransaction txn = session.BeginTransaction())
                {
					userSearchList = UserManagementRepo.SearchUsers(criteria);
                    txn.Commit();
                }
				Assert.AreEqual(userSearchList.ElementAt(0).FirstName, "System");
				Assert.AreEqual(userSearchList.ElementAt(0).LastName, "Administrator");
            }
        }

		//[Test]
		//[ExpectedException(typeof(ArgumentOutOfRangeException))]
		//public void Should_not_return_nonExisting_User_Test()
		//{

		//	UserSearchCriteria criteria = new UserSearchCriteria();
		//	criteria.FirstName = "";
		//	criteria.LastName = "Administrator";
		//	criteria.ChannelPartnerId = 28;
		//	using (ISession session = NHibernateHelper.OpenSession())
		//	{
		//		List<UserDetails> userSearchList = new List<UserDetails>();
		//		CallSessionContext.Bind(session);
		//		using (ITransaction txn = session.BeginTransaction())
		//		{
		//			userSearchList = UserManagementRepo.SearchUsers(criteria);
		//			txn.Commit();
		//		}
		//		Assert.IsNotNullOrEmpty(userSearchList.ElementAt(0).FirstName);
		//		//Assert.IsTrue(userSearchList.ElementAt(0).FirstName);
		//	}
		//}

        [Test]
        public void Can_Search_Users_FirstName_Test()
        {
            //userRepo.SessionFactory = NHibernateHelper.SessionFactory;
            //manageUser.SearchUserRepo = userRepo;

            UserSearchCriteria criteria = new UserSearchCriteria();
			criteria.FirstName = "System";
			//criteria.LastName = "Administrator";
			criteria.ChannelPartnerId = 28;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                List<UserDetails> userSearchList = new List<UserDetails>();
                CallSessionContext.Bind(session);
                using (ITransaction txn = session.BeginTransaction())
                {
					userSearchList = UserManagementRepo.SearchUsers(criteria);
                    txn.Commit();
                }
				Assert.AreEqual(userSearchList.ElementAt(0).FirstName, "System");
            }
        }

        [Test]
        public void Can_Search_Users_No_Criteria_Test()
        {
            //userRepo.SessionFactory = NHibernateHelper.SessionFactory;
            //manageUser.SearchUserRepo = userRepo;

            UserSearchCriteria criteria = new UserSearchCriteria();
            //criteria.FirstName = "Arthur";
            //criteria.LastName = "Morris";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                List<UserDetails> userSearchList = new List<UserDetails>();
                CallSessionContext.Bind(session);
                using (ITransaction txn = session.BeginTransaction())
                {
					userSearchList = UserManagementRepo.SearchUsers(criteria);
                    txn.Commit();
                }
                Assert.IsTrue(userSearchList.Count == 0);
            }
        }

        [Test]
        public void Can_Get_Managers_Test()
        {
            //userRepo.SessionFactory = NHibernateHelper.SessionFactory;
            //manageUser.UserManagementRepo = userRepo;

            //UserSearchCriteria criteria = new UserSearchCriteria();
            //criteria.FirstName = "Arthur";
            //criteria.LastName = "Morris";

            using (ISession session = NHibernateHelper.OpenSession())
            {
                List<UserDetails> managersList = new List<UserDetails>();
                CallSessionContext.Bind(session);
                using (ITransaction txn = session.BeginTransaction())
                {
					managersList = UserManagementRepo.GetManagers();
                    txn.Commit();
                }
                
                Assert.IsNotNull(managersList.ElementAt(0).Id);
                
            }
        }

        [Test]
        public void Can_Save_UserLocation_Test()
        {

            UserLocation userLocation = new UserLocation();
            userLocation.AgentId = 2390;
            userLocation.LocationId = 3;
            int AgentId;

            using (ISession session = NHibernateHelper.OpenSession())
            {
                CallSessionContext.Bind(session);
                using (ITransaction txn = session.BeginTransaction())
                {

					AgentId = UserManagementRepo.SaveUserLocation(userLocation);
                    txn.Commit();
                }
                Assert.IsNotNull(AgentId);
                Assert.AreEqual(userLocation.AgentId, 2390);
            }
        }


    }
 
}
