using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MGI.Biz.Customer.Contract;
using IBizCustomerService = MGI.Biz.Customer.Contract.ICustomerService;
using MGI.Biz.Customer.Data;
using MGI.Biz.Customer.Impl;

using MGI.Core.CXE.Data;
using MGI.Core.CXE.Contract;
using ICXECustomerService = MGI.Core.CXE.Contract.ICustomerService;

using NUnit.Framework;

using Moq;

namespace MGI.Biz.Customer.Test
{
	[TestFixture]
	public class UpdateManagerTests
	{
		//private CustomerServiceManager _mgr = new CustomerServiceManager();
		//private Mock<ICXECustomerService> _mokCustSvc = new Mock<ICXECustomerService>();
		//private Mock<IIdTypeService> _mokIdTypeSvc = new Mock<IIdTypeService>();

		//[TestFixtureSetUp]
		//public void fixtureSetup()
		//{
		//    _mgr.CXECustomerService = _mokCustSvc.Object;
		//    _mgr.CXEIdTypeService = _mokIdTypeSvc.Object;
		//}


	}
}
