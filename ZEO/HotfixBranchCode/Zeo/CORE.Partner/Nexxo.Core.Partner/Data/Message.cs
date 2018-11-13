// -----------------------------------------------------------------------
// <copyright file="Message.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace MGI.Core.Partner.Data
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Message
    {
        public virtual Guid rowguid { get; set; }
		public virtual long Id { get; set; }
	    public virtual string MessageKey { get; set; }
        public virtual Language Language { get; set; }
        public virtual string Content { get; set; }
        public virtual string AddlDetails { get; set; }
        public virtual string Processor { get; set; }
        public virtual long Partner { get; set; } // the simulated partner. Use below when ready.
        //public virtual ChannelPartner Partner { get; set; }

        public virtual DateTime DTServerCreate { get; set; }
        public virtual Nullable<DateTime> DTServerLastModified { get; set; }
	}
}
