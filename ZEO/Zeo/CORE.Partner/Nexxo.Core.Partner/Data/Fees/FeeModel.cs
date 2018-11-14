using MGI.Common.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data.Fees
{
	public abstract class FeeModel : NexxoModel
	{
		public virtual ChannelPartner channelPartner { get; set; }
	}
}
