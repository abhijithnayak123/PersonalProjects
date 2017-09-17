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
				throw new ChannelPartnerException(ChannelPartnerException.GROUP_INSERT_FAILED, ex);
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
                throw new ChannelPartnerException(ChannelPartnerException.GROUP_UPDATE_FAILED, ex);
            }
		}

		public ChannelPartnerGroup Get(int id)
		{
			ChannelPartnerGroup cpGroup = _groupsRepo.FindBy(g => g.Id == id);

			if (cpGroup == null)
				throw new ChannelPartnerException(ChannelPartnerException.GROUP_NOT_FOUND, string.Format("Group Id {0} not found", id));

				return cpGroup;
		}

		public List<ChannelPartnerGroup> GetAll(Guid channelPartnerPK)
		{
			return _groupsRepo.FilterBy(g => g.channelPartner.rowguid == channelPartnerPK).ToList();
		}

		public List<ChannelPartnerGroup> GetAll(string channelPartnerName)
		{
			return _groupsRepo.FilterBy(g => g.channelPartner.Name == channelPartnerName).ToList();
		}
	}
}
