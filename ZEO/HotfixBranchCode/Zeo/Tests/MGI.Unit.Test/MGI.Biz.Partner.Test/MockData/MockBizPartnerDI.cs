using MGI.Biz.Partner.Impl;
using MGI.Unit.Test.MockClasses;
using BizPartnerData = MGI.Biz.Partner.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using System.Linq.Expressions;
using MGI.Common.Util;
using MGI.Core.Partner.Data.Transactions;

namespace MGI.Unit.Test.MGI.Biz.Partner.Test.MockData
{
	public class MockBizPartnerDI : IntializMoqObject
	{
		private bool CheckAge(BizPartnerData.Prospect prospect, int age)
		{
			if (age < 18)
				return false;
			return true;
		}

		#region Created Mock Object For Test Cases.
		public IProspectFieldValidator CreateInstanceOfProspectFieldValidator()
		{
			ProspectFieldValidator = _moqRepository.Create<IProspectFieldValidator>();

			ProspectFieldValidator.Setup(moq => moq.ValidateDOB(It.IsAny<BizPartnerData.Prospect>(), It.IsAny<int>()))
					.Callback<BizPartnerData.Prospect, int>((prospect, age) => CheckAge(prospect, age));

			return ProspectFieldValidator.Object;

		} 
		#endregion

		#region Core Partner Location Processor Credential Service
		public ILocationProcessorCredentialService CreateInstanceOfLocationProcessorCredential()
		{
			Mock<ILocationProcessorCredentialService> LocationProcessorCredentialsService = _moqRepository.Create<ILocationProcessorCredentialService>();

			LocationProcessorCredentialsService.Setup(moq => moq.Get(It.IsAny<long>())).Returns(new List<LocationProcessorCredentials>() { new LocationProcessorCredentials(){ Id = 10000} });

			LocationProcessorCredentialsService.Setup(moq => moq.Save(It.IsAny<LocationProcessorCredentials>())).Returns(true);

			return LocationProcessorCredentialsService.Object;
		} 
		#endregion

		#region Core Partner Shopping Cart Service
		public IShoppingCartService CreateInstanceOfShoppingCartService()
		{
			Mock<IShoppingCartService> ShoppingCartSvc = _moqRepository.Create<IShoppingCartService>();

			ShoppingCartSvc.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(
				(long id) =>
				{
					var existShop = shoppingCarts.Find(a => a.Id == id);
					if (existShop == null)
					{
						existShop = shoppingCarts.FirstOrDefault();
					}
					return existShop;
				});

			ShoppingCartSvc.Setup(moq => moq.LookupForCustomer(It.IsAny<long>())).Returns(
				(long alloyId) =>
				{
					var exsitingShoppingCart = shoppingCarts.FindAll(a => a.Customer.CXEId == alloyId);
					if (exsitingShoppingCart == null)
					{
						exsitingShoppingCart = shoppingCarts;
					}
					return exsitingShoppingCart;
				});

			ShoppingCartSvc.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<bool>()));

			ShoppingCartSvc.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<ShoppingCartStatus>())).Callback(
				(long id, ShoppingCartStatus status) =>
				{
					var existShop = shoppingCarts.Find(a => a.Id == id);
					if (existShop != null)
					{
						shoppingCarts.Remove(existShop);
						existShop.Status = status;
						shoppingCarts.Add(existShop);
					}
				});

            ShoppingCartSvc.Setup(moq => moq.GetAllParkedShoppingCarts()).Returns(shoppingCarts);

			return ShoppingCartSvc.Object;
		} 
		#endregion

		#region Core partner transaction history service
		public ITransactionHistoryService CreateInstanceOfTransactionHistoryService()
		{
			Mock<ITransactionHistoryService> PartnerTransactionHistoryService = _moqRepository.Create<ITransactionHistoryService>();

			PartnerTransactionHistoryService.Setup(moq => moq.Get(It.IsAny<Expression<System.Func<TransactionHistory, bool>>>())).Returns(transactionHistory);

			PartnerTransactionHistoryService.Setup(moq => moq.Get(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>())).Returns(
				new List<PastTransaction>() 
				{
 					new PastTransaction(){ AccountNumber = "1245789632", Id = 1000000000, rowguid = Guid.NewGuid()}
				});

			return PartnerTransactionHistoryService.Object;
		} 
		#endregion

		private NpsTerminal ReturnVal(string p, ChannelPartner cp)
		{
			if (p == "test")
				return null;

			return new NpsTerminal();
		}

		public INpsTerminal CreateInstanceOfNpsTerminalService()
		{
			Mock<INpsTerminal> NPSTerminalService = _moqRepository.Create<INpsTerminal>();

			NPSTerminalService.Setup(moq => moq.Create(It.IsAny<NpsTerminal>())).Returns(true);

			NPSTerminalService.Setup(moq => moq.Update(It.IsAny<NpsTerminal>())).Returns(true);

			NPSTerminalService.Setup(moq => moq.Lookup(It.IsAny<long>())).Returns(terminal);

			NPSTerminalService.Setup(moq => moq.Lookup(It.IsAny<Guid>())).Returns(terminal);

			NPSTerminalService.Setup(moq => moq.Lookup(It.IsAny<string>())).Returns(terminal);

			NPSTerminalService.Setup(moq => moq.Lookup(It.IsAny<string>(), It.IsAny<ChannelPartner>())).Returns(terminal);

			NPSTerminalService.Setup(moq => moq.Lookup("test", It.IsAny<ChannelPartner>())).Callback<string, ChannelPartner>((p, cp) => ReturnVal(p, cp));

			NPSTerminalService.Setup(moq => moq.GetByLocationID(It.IsAny<long>())).Returns(npsTerminals);

			return NPSTerminalService.Object;
		}
	}
}
