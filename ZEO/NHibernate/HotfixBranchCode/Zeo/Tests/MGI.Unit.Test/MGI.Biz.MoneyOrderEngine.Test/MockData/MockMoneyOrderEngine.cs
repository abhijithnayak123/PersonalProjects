using System.Linq;
using Moq;
using AutoMapper;
using CXEData = MGI.Core.CXE.Data;
using CorePartnerContract = MGI.Core.Partner.Contract;
using CorePartnerData = MGI.Core.Partner.Data;
using MGI.Unit.Test.MockClasses;
using CoreCXEContract = MGI.Core.CXE.Contract;

namespace MGI.Unit.Test
{
	public class MockMoneyOrderEngine : IntializMoqObject
    {
        public MockMoneyOrderEngine()
        {
            Mapper.CreateMap<CXEData.Transactions.Stage.MoneyOrder, CXEData.Transactions.Commit.MoneyOrder>();
        }

		#region Core CXE Contract
		public CoreCXEContract.IMoneyOrderService CoreCXEMOServiceInstance()
		{
			CxeMoneyOrderSvc = _moqRepository.Create<CoreCXEContract.IMoneyOrderService>();

			CxeMoneyOrderSvc.Setup(moq => moq.Create(It.IsAny<CXEData.Transactions.Stage.MoneyOrder>(), It.IsAny<string>())).Returns(
				(CXEData.Transactions.Stage.MoneyOrder mo, string timeZone) =>
				{
					mo.Id = 1000000002 + (stageMoneyOrders.Count() + 1);
					stageMoneyOrders.Add(mo);
					return mo.Id;

				});

			CxeMoneyOrderSvc.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<decimal>(), It.IsAny<string>())).Callback((long id, decimal fee, string timeZone) => { });

			CxeMoneyOrderSvc.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<string>())).Callback((long id, int newStatus, string timeZone) => { });

			CxeMoneyOrderSvc.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Callback(
			(long id, string checknum, string accountnum, string routingnum, string micr, string timeZone) =>
			{
				var existingTrxn = stageMoneyOrders.Find(a => a.Id == id);
				if (existingTrxn != null)
				{
					stageMoneyOrders.Remove(existingTrxn);
					existingTrxn.MICR = micr;
					existingTrxn.AccountNumber = accountnum;
					existingTrxn.RoutingNumber = routingnum;
					existingTrxn.MoneyOrderCheckNumber = checknum;
					stageMoneyOrders.Add(existingTrxn);
				}
			});

			CxeMoneyOrderSvc.Setup(moq => moq.Get(It.IsAny<long>())).Returns(
				(long id) => 
				{
					return commitMoneyOrders.Find(a => a.Id == id);
				});

			CxeMoneyOrderSvc.Setup(moq => moq.GetStage(It.IsAny<long>())).Returns(
				(long id) =>
				{
					return stageMoneyOrders.Find(a => a.Id == id);
				});

			CxeMoneyOrderSvc.Setup(moq => moq.GetMOByMICR(It.IsAny<string>())).Returns(
				(string micr) =>
				{
					return commitMoneyOrders.Where(a => a.MICR == micr).ToList();
				});

			CxeMoneyOrderSvc.Setup(moq => moq.Commit(It.IsAny<long>())).Callback(
				 (long id) =>
				 {
					 var existingTrxn = stageMoneyOrders.Find(a => a.Id == id);
					 var moCommit = Mapper.Map<CXEData.Transactions.Stage.MoneyOrder, CXEData.Transactions.Commit.MoneyOrder>(existingTrxn);
					 moCommit.Id = 1000000001 + (commitMoneyOrders.Count() + 1);
					 moCommit.Status = (int)CXEData.TransactionStates.Committed;
					 commitMoneyOrders.Add(moCommit);
				 });

			return CxeMoneyOrderSvc.Object;

		} 
		#endregion

		#region Core Partner Money Order Image
		public CorePartnerContract.IMoneyOrderImage CreateInstanceOfMoneyOrderImage()
		{
			Mock<CorePartnerContract.IMoneyOrderImage> MoneyOrderImageSvc = _moqRepository.Create<CorePartnerContract.IMoneyOrderImage>();

			MoneyOrderImageSvc.Setup(moq => moq.Create(It.IsAny<CorePartnerData.MoneyOrderImage>(), It.IsAny<string>())).Returns(
				(CorePartnerData.MoneyOrderImage moImage, string timeZone) => 
				{
					return long.MaxValue;
				});

			MoneyOrderImageSvc.Setup(moq => moq.FindMoneyOrderByTxnId(It.IsAny<System.Guid>())).Returns(
				(System.Guid rowguid) => 
				{
					return moneyOrderImages.Find(x => x.rowguid == rowguid);
				});

			MoneyOrderImageSvc.Setup(moq => moq.Update(It.IsAny<CorePartnerData.MoneyOrderImage>(), It.IsAny<string>())).Callback((CorePartnerData.MoneyOrderImage moImage, string timeZone) => { });

			return MoneyOrderImageSvc.Object;
		} 
		#endregion
    }
}
