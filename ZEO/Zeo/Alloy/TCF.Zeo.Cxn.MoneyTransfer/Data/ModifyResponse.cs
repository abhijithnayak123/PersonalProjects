﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCF.Zeo.Cxn.MoneyTransfer.Data
{
    public class ModifyResponse
    {
        public long CancelTransactionId { get; set; }
        public long ModifyTransactionId { get; set; }
    }
}
