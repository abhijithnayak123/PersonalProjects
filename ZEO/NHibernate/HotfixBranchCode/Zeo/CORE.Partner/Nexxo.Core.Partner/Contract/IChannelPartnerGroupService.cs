using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MGI.Core.Partner.Data;

namespace MGI.Core.Partner.Contract
{
	public interface IChannelPartnerGroupService
	{

		/// <summary>
		/// This method is to create the channel partner group
		/// </summary>
		/// <param name="group">This is channel partner group details.</param>
		/// <returns></returns>
		void Create(ChannelPartnerGroup group);

		/// <summary>
		/// This method is to update the channel partner group
		/// </summary>
		/// <param name="group">This is channel partner group details.</param>
		/// <returns></returns>
		void Update(ChannelPartnerGroup group);

		/// <summary>
		/// This method is to get a channel partner group details by id
		/// </summary>
		/// <param name="Id">Unique identifier of channel partner group</param>
		/// <returns>channel partner group details</returns>
		ChannelPartnerGroup Get(int Id);

		/// <summary>
		/// This method is to get the collection of channel partner group details by channel partner.
		/// </summary>
		/// <param name="channelPartnerPK">channel partner primary key.</param>
		/// <returns>Collection of channel partner group details</returns>
		List<ChannelPartnerGroup> GetAll(Guid channelPartnerPK);

		/// <summary>
		/// This method is to get the collection of channel partner group details by channel partner name.
		/// </summary>
		/// <param name="channelPartnerName">This is channel  partner name.</param>
		/// <returns>Collection of channel partner group details</returns>
		List<ChannelPartnerGroup> GetAll(string channelPartnerName);
	}
}
