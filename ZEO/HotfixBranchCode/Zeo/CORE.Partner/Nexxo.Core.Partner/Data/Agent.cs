using System;

using MGI.Common.DataAccess.Data;

namespace MGI.Core.Partner.Data
{
	public class Agent : NexxoModel
	{
		public virtual int ChannelPartnerId { get; set; }
	}
}
