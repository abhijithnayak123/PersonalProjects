﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Core.Data
{
    public class TransactionSearchCriteria
    {
        public DateTime DatePeriod { get; set; }

        public string TransactionType { get; set; }

        public long AgentId { get; set; }

        public long CustomerId { get; set; }

        public string LocationName { get; set; }

        public bool ShowAll { get; set; }

        public long TransactionId { get; set; }
    }
}
