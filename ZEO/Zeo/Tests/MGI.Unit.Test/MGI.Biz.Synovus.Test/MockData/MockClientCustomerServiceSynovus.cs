using System;
using System.Collections.Generic;
using MGI.Cxn.Customer.FIS.Data;
using MGI.Unit.Test.MockClasses;
using MGI.Cxn.Customer.FIS.Contract;
using MGI.Common.DataAccess.Contract;
using Moq;
using System.Linq.Expressions;
using System.Linq;

namespace MGI.Unit.Test.MockData
{
	public class MockClientCustomerServiceSynovus : IntializMoqObject
	{
		#region Creating Fake Instance For FISConnet
		public IFISConnect CreateFISCustomer()
		{
			Mock<IFISConnect> CxnFISCustomer = _moqRepository.Create<IFISConnect>();

			CxnFISCustomer.Setup(m => m.GetSSNForCustomer(It.IsAny<string>()));

			CxnFISCustomer.Setup(m => m.FISConnectCustomerLookUp(It.IsAny<Dictionary<string, object>>())).Returns(
				(Dictionary<string, object> customerLookUpCriteria) => 
				{
					if (customerLookUpCriteria.ContainsKey("Exception"))
					{
						throw new Exception();
					}
					if (customerLookUpCriteria.ContainsKey("NoneFISCustomer"))
						return new List<FISConnect>() { new FISConnect() { } };

					return new List<FISConnect>();
				});

			return CxnFISCustomer.Object;
		} 
		#endregion

		#region Creating Fake Repository for FISAccount
		public IRepository<FISAccount> CreateIntsanceOfFISAccount()
		{
			int count = 1;
			var FISAccountRepo = _moqRepository.Create<IRepository<FISAccount>>();

			FISAccountRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<FISAccount, bool>>>())).Returns((Expression<Func<FISAccount, bool>> predicate)=>
			{
				var fisAccount = fisAccounts.FirstOrDefault();
				return fisAccount;
			}).Callback(
				(Expression<Func<FISAccount, bool>> predicate) => 
				{
					if (count == 2 || count == 4)
					{
						fisAccounts = new List<FISAccount>(); 
					}
					if (count == 3)
					{
						fisAccounts = new List<FISAccount>() { new FISAccount() { ProfileStatus = Common.Util.ProfileStatus.Active } };
					}
					count++;
				});
				

			return FISAccountRepo.Object;
		} 
		#endregion
	}
}
