using System.Collections.Generic;
using System.Linq;
using Moq;
using MGI.Core.CXE.Data.Transactions.Stage;
using MGI.Unit.Test.MockClasses;

namespace MGI.Biz.CashEngine.Test
{
	public class MockCashEngine : IntializMoqObject
	{	
		#region Public Methods
		private List<Cash> cashCollections = new List<Cash>();

		public Core.CXE.Contract.ICashService CoreCXECashServiceInstanceIIO() 
		{

			CashService = _moqRepository.Create<Core.CXE.Contract.ICashService>();
			 
			
			CashService.Setup(moq => moq.Create(It.IsAny<Core.CXE.Data.Transactions.Stage.Cash>())).Returns(
				(Cash cash) =>
				{
					cash.Id = 1000000000 + (cashCollections.Count() + 1);
					cashCollections.Add(cash);
					return cash.Id;

				});

			CashService.Setup(moq => moq.Commit(It.IsAny<long>(), It.IsAny<string>()));

			CashService.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<Core.CXE.Data.TransactionStates>(), It.IsAny<string>()));

			CashService.Setup(moq => moq.UpdateAmount(It.IsAny<long>(), It.IsAny<decimal>(), It.IsAny<string>())); //AL-2729 for update cash-in transaction

			return CashService.Object;
			
		}
		#endregion
	}
}
