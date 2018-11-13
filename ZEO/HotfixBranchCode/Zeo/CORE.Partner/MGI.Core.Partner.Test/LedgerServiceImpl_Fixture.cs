using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Data;
using MGI.Core.Partner.Impl;

using MGI.Common.DataAccess.Contract;
using MGI.Common.DataAccess.Impl;

using NUnit.Framework;
//using Moq;

namespace MGI.Core.Partner.Test
{
	[TestFixture]
	public class LedgerServiceImpl_Fixture : AbstractPartnerTest
	{
		private ICustomerService _custSvc;
		public ICustomerService CustomerService { set { _custSvc = value; } }

		public void CreateCustomerAndAccount()
		{
			Guid customerRowguid = Guid.NewGuid();
			AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tPartnerCustomers(rowguid,id,cxeid,dtcreate) values('{0}',1000000000000020,1000000000000020,getdate())", customerRowguid));
			AdoTemplate.ExecuteNonQuery(CommandType.Text, string.Format("insert tAccounts(rowguid,id,cxeid,cxnid,customerpk,dtcreate,ProviderId) values(newid(),555,555,555,'{0}',getdate(),301)", customerRowguid));
		}
		

		//[TestFixtureTearDown]
		//public void teardown()
		//{
		//	AdoTemplate.ExecuteNonQuery(CommandType.Text, "delete from tAccounts where Id=1000000000000010");
		//	AdoTemplate.ExecuteNonQuery(CommandType.Text, "delete from tPartnerCustomers where Id=1000000000000010");
		//}
	}
}
