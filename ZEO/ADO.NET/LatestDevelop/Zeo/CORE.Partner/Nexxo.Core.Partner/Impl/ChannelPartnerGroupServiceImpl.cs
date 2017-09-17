using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Common.DataAccess.Contract;

using MGI.Core.Partner.Data;
using MGI.Core.Partner.Contract;

namespace MGI.Core.Partner.Impl
{
	public class ChannelPartnerGroupServiceImpl : IChannelPartnerGroupService
	{
		private IRepository<ChannelPartnerGroup> _groupsRepo;
		public IRepository<ChannelPartnerGroup> GroupsRepo { set { _groupsRepo = value; } }

		public void Create(ChannelPartnerGroup group)
		{
			try
			{
				_groupsRepo.AddWithFlush(group);
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_GROUP_CREATE_FAILED, ex);
			}
		}

		public void Update(ChannelPartnerGroup group)
		{
			try
			{
				_groupsRepo.UpdateWithFlush(group);
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_GROUP_UPDATE_FAILED, ex);
			}
		}

		public ChannelPartnerGroup Get(int id)
		{
			try
			{
				ChannelPartnerGroup cpGroup = _groupsRepo.FindBy(g => g.Id == id);

				if (cpGroup == null)
					throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_GROUP_NOT_FOUND, null);

				return cpGroup;
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_GROUP_GET_FAILED, ex);
			}
		}

		public List<ChannelPartnerGroup> GetAll(Guid channelPartnerPK)
		{
			try
			{
				return _groupsRepo.FilterBy(g => g.channelPartner.rowguid == channelPartnerPK).ToList();
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_GROUP_GET_FAILED, ex);
			}
		}

		public List<ChannelPartnerGroup> GetAll(string channelPartnerName)
		{
			try
			{
				return _groupsRepo.FilterBy(g => g.channelPartner.Name == channelPartnerName).ToList();
			}
			catch (Exception ex)
			{
				throw new ChannelPartnerExceptions(ChannelPartnerExceptions.CHANNEL_PARTNER_GROUP_GET_FAILED, ex);
			}
		}
	}
}
