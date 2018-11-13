using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using NUnit.Framework;
using Spring.Testing.NUnit;

using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;
using MGI.Core.Partner.Impl;

namespace MGI.Core.Partner.Test
{
	[TestFixture]
	public class ChannelPartnerGroupServiceImpl_Fixture : AbstractPartnerTest
	{
		private IChannelPartnerGroupService _groupSvc;
		public IChannelPartnerGroupService GroupSvc { set { _groupSvc = value; } }


		private IRepository<ChannelPartner> _channelPartnerRepo;
		public IRepository<ChannelPartner> ChannelPartnerRepo { set { _channelPartnerRepo = value; } }

		private ChannelPartner _ChannelPartner;

		[TestFixtureSetUp]
		public void Init()
		{
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
		}

		[SetUp]
		public void TestSetup()
		{
			_ChannelPartner = _channelPartnerRepo.FindBy(c => c.Name == "Synovus");
			Trace.WriteLine("ChannelPartner: " + _ChannelPartner.Name);
		}

		[Test]
		public void AddGroup()
		{
			ChannelPartnerGroup g = new ChannelPartnerGroup
			{
				channelPartner = _ChannelPartner,
				Name = "Group 1",
				DTServerCreate = DateTime.Now
			};

			_groupSvc.Create(g);

			Assert.IsTrue(g.Id > 0);
		}

		[Test]
		public void EditGroup()
		{
			ChannelPartnerGroup g = new ChannelPartnerGroup
			{
				channelPartner = _ChannelPartner,
				Name = "Group 1",
				DTServerCreate = DateTime.Now
			};

			_groupSvc.Create(g);

			Assert.IsTrue(g.Id > 0);

			g.Name = "Group 0";
			g.DTServerLastModified = DateTime.Now;

			_groupSvc.Update(g);

			ChannelPartnerGroup g2 = _groupSvc.Get(g.Id);

			Assert.AreEqual(g.Name, g2.Name);
		}

		[Test]
		public void GetGroupsByGuid()
		{
			List<ChannelPartnerGroup> cpGroups = _groupSvc.GetAll(_ChannelPartner.rowguid);

			Assert.AreEqual(1, cpGroups.Count);

			ChannelPartnerGroup g = new ChannelPartnerGroup
			{
				channelPartner = _ChannelPartner,
				Name = "Group 1",
				DTServerCreate = DateTime.Now
			};

			_groupSvc.Create(g);

			cpGroups = _groupSvc.GetAll(_ChannelPartner.rowguid);

			Assert.AreEqual(2, cpGroups.Count);

			g = new ChannelPartnerGroup
			{
				channelPartner = _ChannelPartner,
				Name = "Group 2",
				DTServerCreate = DateTime.Now
			};

			_groupSvc.Create(g);

			cpGroups = _groupSvc.GetAll(_ChannelPartner.rowguid);

			Assert.AreEqual(3, cpGroups.Count);
		}

		[Test]
		public void GetGroupsByName()
		{
			List<ChannelPartnerGroup> cpGroups = _groupSvc.GetAll(_ChannelPartner.rowguid);

			Assert.AreEqual(1, cpGroups.Count);

			ChannelPartnerGroup g = new ChannelPartnerGroup
			{
				channelPartner = _ChannelPartner,
				Name = "Group 1",
				DTServerCreate = DateTime.Now
			};

			_groupSvc.Create(g);

			cpGroups = _groupSvc.GetAll(_ChannelPartner.rowguid);

			Assert.AreEqual(2, cpGroups.Count);

			g = new ChannelPartnerGroup
			{
				channelPartner = _ChannelPartner,
				Name = "Group 2",
				DTServerCreate = DateTime.Now
			};

			_groupSvc.Create(g);

			cpGroups = _groupSvc.GetAll("Synovus");

			Assert.AreEqual(3, cpGroups.Count);
		}

		[Test]
		public void GetEmptyList()
		{
			List<ChannelPartnerGroup> cpGroups = _groupSvc.GetAll("Synovus");

			Assert.AreEqual(1, cpGroups.Count);
		}
	}
}
