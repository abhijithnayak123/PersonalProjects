﻿using System;
using MGI.Common.DataAccess.Contract;
using MGI.Unit.Test.MockClasses;
using MGI.Cxn.Customer.CCIS.Data;
using System.Linq.Expressions;
using System.Linq;
using Moq;

namespace MGI.Unit.Test.MockData
{
	public class MockClientCustomerServiceCarver : IntializMoqObject
	{
		#region Creating CCISAccount Fake Repository
		public IRepository<CCISAccount> CreateInstanceOfCCISAccount()
		{
			var CCISAccountRepo = _moqRepository.Create<IRepository<CCISAccount>>();

			CCISAccountRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<CCISAccount, bool>>>())).Returns(new CCISAccount());

			return CCISAccountRepo.Object;
		} 
		#endregion
	}
}
