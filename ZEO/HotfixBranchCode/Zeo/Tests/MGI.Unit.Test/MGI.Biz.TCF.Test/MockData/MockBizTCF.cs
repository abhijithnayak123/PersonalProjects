using MGI.Common.DataAccess.Contract;
using MGI.Common.Util;
using MGI.Cxn.Customer.TCIS.Data;
using MGI.Cxn.Partner.TCF.Contract;
using MGI.Cxn.Partner.TCF.Data;
using MGI.Unit.Test.MockClasses;
using Moq;
using System;
using System.Linq.Expressions;

namespace MGI.Unit.Test.MGI.Biz.TCF.Test.MockData
{
	public class MockBizTCF : IntializMoqObject
	{
		#region Mocking RCIF Service
		public IFlushProcessor CreateInstanceOfFlushProcessor()
		{
			GatewayService = _moqRepository.Create<IFlushProcessor>();

			GatewayService.Setup(moq => moq.PreFlush(It.IsAny<CustomerTransactionDetails>(), It.IsAny<MGIContext>()));

			GatewayService.Setup(moq => moq.PostFlush(It.IsAny<CustomerTransactionDetails>(), It.IsAny<MGIContext>()));

			return GatewayService.Object;
		} 
		#endregion

		#region TCIS Account Fake Repository
		public IRepository<Account> CreateInstanceOf()
		{
			var tcisAccountRepo = _moqRepository.Create<IRepository<Account>>();

			tcisAccountRepo.Setup(moq => moq.FindBy(It.IsAny<Expression<Func<Account, bool>>>())).Returns(new Account() { });

			return tcisAccountRepo.Object;
		} 
		#endregion
	}
}
