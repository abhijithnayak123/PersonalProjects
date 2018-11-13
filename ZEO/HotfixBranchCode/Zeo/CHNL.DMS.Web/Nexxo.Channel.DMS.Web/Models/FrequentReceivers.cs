using MGI.Channel.Shared.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MGI.Channel.DMS.Web.Models
{
	public class FrequentReceivers
	{
		public long SelectedReceiverId { get; set; }
		public List<Receiver> Receivers { get; set; }
	}
}