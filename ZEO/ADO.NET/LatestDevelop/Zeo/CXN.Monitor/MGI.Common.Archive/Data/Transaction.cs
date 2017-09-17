using System;
using System.Collections.Generic;

namespace MGI.Common.Archive.Data
{
    public class Transaction
    {
        public long Id { get; set; }

        public Dictionary<string, byte[]> Images { get; set; }

        public string FrontImagePath { get; set; }

        public string BackImagePath { get; set; }

		public string ChannelPartnerName { get; set; }


    }
}
