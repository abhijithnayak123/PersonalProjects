using System;
using System.Collections.Generic;
using System.Linq;
using MGI.Core.CXE.Contract;
using MGI.Cxn.BillPay.Contract;
using CXNBillPayData = MGI.Cxn.BillPay.Data;
using MGI.Common.Util;
using Moq;
using CoreCXE = MGI.Core.CXE;
using CatalogData = MGI.Core.Catalog;
using MGI.Core.CXE.Data;
using MGI.Common.DataProtection.Contract;
using MGI.Unit.Test.MockClasses;
using MGI.Biz.BillPay.Contract;
using MGI.Biz.BillPay.Data;

namespace MGI.Unit.Test
{
	public class BillPayMockFactory : IntializMoqObject
	{
		#region Product Catalog Service
		public CatalogData.Contract.IProductService ProductServiceInstance()
		{
			Mock<CatalogData.Contract.IProductService> ProductService = _moqRepository.Create<CatalogData.Contract.IProductService>();

			ProductService.Setup(m => m.Get(It.IsAny<long>())).Returns(
				(long productId) =>
				{
					var trxn = masterCatalogs.Find(a => a.BillerCode == Convert.ToString(productId));
					if (trxn == null)
					{
						trxn = masterCatalogs.FirstOrDefault();
					}
					return trxn;
				});

			ProductService.Setup(moq => moq.Get(It.IsAny<string>())).Returns(
				(string productName) =>
				{
					return masterCatalogs.Find(a => a.BillerName == productName);
				});

			ProductService.Setup(moq => moq.Get(It.IsAny<long>(), It.IsAny<string>())).Returns(
				(long channelPartnerId, string productName) =>
				{
					return masterCatalogs.Find(a => a.ChannelPartnerId == channelPartnerId && a.BillerName == productName);
				});

			ProductService.Setup(moq => moq.Get(It.IsAny<string>(), It.IsAny<long>())).Returns(
				(string productCode, long channelPartnerId) =>
				{
					return masterCatalogs.Find(a => a.BillerCode == productCode && a.ChannelPartnerId == channelPartnerId);
				});

			ProductService.Setup(moq => moq.GetAll(It.IsAny<int>(), It.IsAny<int>())).Returns(
				(int channelPartnerId, int providerId) =>
				{
					return masterCatalogs;
				});

			ProductService.Setup(moq => moq.GetProducts(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(
				(string term, int channelPartnerId, int providerId) =>
				{
					return partnerCatalogs;
				});

			ProductService.Setup(moq => moq.GetProductsByIDs(It.IsAny<long[]>())).Returns(masterCatalogs);

			return ProductService.Object;
		} 
		#endregion

		#region BILLPAY PROCESSOR
		public IBillPayProcessor BillPayProcessorInstance()
		{
			BillPayService = _moqRepository.Create<IBillPayProcessor>();

			BillPayService.Setup(m => m.AddBillPayAccount(It.IsAny<CXNBillPayData.BillPayRequest>(), It.IsAny<string>())).Returns(
			(CXNBillPayData.BillPayRequest billPayRequest, string timeZone) =>
			{
				CXNBillPayData.BillPayAccount account = new CXNBillPayData.BillPayAccount()
				{
					Id = 1000000000000001 + (billPayAccounts.Count() + 1),
					CardNumber = billPayRequest.CardNumber,
					FirstName = billPayRequest.CustomerFirstName,
					LastName = billPayRequest.CustomerLastName,
					Address1 = billPayRequest.CustomerAddress1,
					Address2 = billPayRequest.CustomerAddress2,
					City = billPayRequest.CustomerCity,
					State = billPayRequest.CustomerState,
					PostalCode = billPayRequest.CustomerZip,
					Street = billPayRequest.CustomerStreet,
					DateOfBirth = billPayRequest.CustomerDateOfBirth,
					Email = billPayRequest.CustomerEmail,
					ContactPhone = billPayRequest.CustomerPhoneNumber,
					DTServerCreate = DateTime.Now,
					MobilePhone = billPayRequest.CustomerMobileNumber
				};
				billPayAccounts.Add(account);
				return account.Id;
			});

			BillPayService.Setup(m => m.GetBillPayAccount(It.IsAny<long>())).Returns(
				(long Id) =>
				{
					return billPayAccounts.Find(a => a.Id == Id);
				});
			BillPayService.Setup(m => m.Validate(It.IsAny<long>(), It.IsAny<CXNBillPayData.BillPayRequest>(), It.IsAny<MGIContext>())).Returns(long.MaxValue);

			BillPayService.Setup(m => m.GetTransaction(It.IsAny<long>())).Returns(new CXNBillPayData.BillPayTransaction()
			{
				Fee = 5,
				ConfirmationNumber = "4580627029",
				AccountNumber = "1234561830",
				MetaData = new Dictionary<string, object>() { { "MTCN", "1546546456" } }
			});

			BillPayService.Setup(moq => moq.Commit(It.IsAny<long>(), It.IsAny<MGIContext>())).Returns(
				(long transactionId, MGIContext mgiContext) =>
				{
					return transactionId;
				});

			BillPayService.Setup(moq => moq.GetFee(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<CXNBillPayData.Location>(), It.IsAny<MGIContext>())).Returns(new CXNBillPayData.Fee()
			{
				AccountHolderName = "test",
				AvailableBalance = "150",
				CityCode = "CA",
				TransactionId = 1000000002
			});

			BillPayService.Setup(moq => moq.GetLocations(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<MGIContext>())).Returns(
				new List<CXNBillPayData.Location>() {
						new CXNBillPayData.Location() { Id = "1", Name = "KINGFISH FL", Type = "03" },
						new CXNBillPayData.Location() { Id = "2", Name = "RACCHES VA", Type = "03" },
						new CXNBillPayData.Location() { Id = "3", Name = "RACWEST NC", Type = "03" },
						new CXNBillPayData.Location() { Id = "4", Name = "RECRCY NC", Type = "03" },
						new CXNBillPayData.Location() { Id = "5", Name = "REGA NC", Type = "03" },
						new CXNBillPayData.Location() { Id = "6", Name = "REGI NC", Type = "03" },
						new CXNBillPayData.Location() { Id = "7", Name = "REGIONALCREDIT TX", Type = "03" }
			});

			BillPayService.Setup(moq => moq.GetBillerInfo(It.IsAny<string>(), It.IsAny<MGIContext>())).Returns(new CXNBillPayData.BillerInfo()
			{
				BillerState = "California",
				Message = "test"
			});

			BillPayService.Setup(moq => moq.GetBillerLastTransaction(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<MGIContext>())).Returns(new CXNBillPayData.BillPayTransaction());

			BillPayService.Setup(moq => moq.GetProviderAttributes(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MGIContext>())).Returns(fields);

			BillPayService.Setup(moq => moq.GetCardInfo(It.IsAny<string>(), It.IsAny<MGIContext>())).Returns(
				(string cardNumber, MGIContext mgiContext) =>
				{
					return new CXNBillPayData.CardInfo() { PromoCode = "1000", TotalPointsEarned = "10" };
				});

			BillPayService.Setup(moq => moq.GetPastBillers(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<MGIContext>())).Returns(
				(long customerSessionId, string cardNumber, MGIContext mgiContext) => 
				{
					return new List<CXNBillPayData.Biller>() { new CXNBillPayData.Biller() { AccountNumber = "123456789", IndexNumber = "2", Name = "REGIONAL ACCEPTANCE" } };
				});

			return BillPayService.Object;
		}
		#endregion

		#region Core CXE BillPay Service
		public Core.CXE.Contract.IBillPayService CoreBillPayServiceInstance()
		{
			CXEBillPayService = _moqRepository.Create<Core.CXE.Contract.IBillPayService>();

			CXEBillPayService.Setup(moq => moq.Commit(It.IsAny<long>())).
				Callback((long id) => { });

			CXEBillPayService.Setup(moq => moq.Create(It.IsAny<CoreCXE.Data.Transactions.Stage.BillPay>())).Returns(
				(CoreCXE.Data.Transactions.Stage.BillPay billPay) =>
				{
					billPay.Id = 1000000000 + (stageBillPays.Count() + 1);
					billPay.rowguid = Guid.NewGuid();
					stageBillPays.Add(billPay);
					return billPay.Id;
				});
						
			CXEBillPayService.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<TransactionStates>(), It.IsAny<string>())).Callback(
				(long transactionId, TransactionStates state, string conformationNumber) =>
				{
					var existing = stageBillPays.Find(a => a.Id == transactionId);
					if (existing != null)
					{
						stageBillPays.Remove(existing);
						existing.Status = (int)state;
						existing.ConfirmationNumber = conformationNumber;
						stageBillPays.Add(existing);
					}
				});

			CXEBillPayService.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<TransactionStates>(), It.IsAny<string>(), It.IsAny<decimal>())).Callback(
				(long transactionId, TransactionStates state, string conformationNumber, decimal fee) =>
				{
					var existing = stageBillPays.Find(a => a.Id == transactionId);
					if (existing != null)
					{
						stageBillPays.Remove(existing);
						existing.Status = (int)state;
						existing.ConfirmationNumber = conformationNumber;
						existing.Fee = fee;
						stageBillPays.Add(existing);
					}
				});

			CXEBillPayService.Setup(moq => moq.Update(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>())).Callback(
				(long transactionId, string billerName, string accountNumber, decimal amount) =>
				{
					var existing = stageBillPays.Find(a => a.Id == transactionId);
					if (existing != null)
					{
						stageBillPays.Remove(existing);
						existing.ProductName = billerName;
						existing.AccountNumber = accountNumber;
						existing.Amount = amount;
						stageBillPays.Add(existing);
					}
				});

			CXEBillPayService.Setup(moq => moq.Get(It.IsAny<long>())).Returns(
				(long transactionId) =>
				{
					return new CoreCXE.Data.Transactions.Commit.BillPay() { rowguid = Guid.NewGuid(), Amount = 100 };
				});

			return CXEBillPayService.Object;
		}
		#endregion

		#region BillPay Setup Srvice
		public IBillPaySetupService CXEBillpayServiceInstance()
		{
			CXEBillPaySetup = _moqRepository.Create<IBillPaySetupService>();

			CXEBillPaySetup.Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>())).Returns(
				(long billerId, long alloyId) => 
				{
					var preferdProduct = customerPreferdProducts.Find(x => x.ProductId == billerId);
					return preferdProduct;
				});

			CXEBillPaySetup.Setup(m => m.Create(It.IsAny<CustomerPreferedProduct>())).Returns(
				(CustomerPreferedProduct product) =>
				{
					product.Id = 1000000000 + (customerPreferdProducts.Count() + 1);
					customerPreferdProducts.Add(product);
					return product.Id;
				});

			CXEBillPaySetup.Setup(m => m.Update(It.IsAny<CustomerPreferedProduct>(), It.IsAny<string>())).Returns(true);

			CXEBillPaySetup.Setup(moq => moq.GetBillerReceiverIndex(It.IsAny<long>(), It.IsAny<long>())).Returns(
				(long productId, long customerId) =>
				{
					if(productId == 111)
					{
						return null;
					}
					return customerPreferdProducts.FirstOrDefault();
				});

			CXEBillPaySetup.Setup(moq => moq.GetPrefered(It.IsAny<long>())).Returns(new long[1]);

			return CXEBillPaySetup.Object;
		} 
		#endregion

		#region Data Protection Service
		public object CreateInstanceOfDataProtection()
		{
			Mock<IDataProtectionService> obj = _moqRepository.Create<IDataProtectionService>();

			obj.Setup(moq => moq.Encrypt(It.IsAny<string>(), It.IsAny<int>())).Returns(
				(string cardNumber, int count) =>
				{
					return cardNumber;
				});

			obj.Setup(moq => moq.Decrypt(It.IsAny<string>(), It.IsAny<int>())).Returns(
				(string cardNumber, int count) =>
				{
					return cardNumber;
				});
			obj.Setup(moq => moq.Decrypt(It.IsAny<string>(), It.IsAny<int>())).Returns(
				(string cardNumber, int count) =>
				{
					throw new BizBillPayException(BizBillPayException.ACCOUNT_UPDATE_FAILED, null);
				});
			return obj.Object;
		} 
		#endregion
	}
}
