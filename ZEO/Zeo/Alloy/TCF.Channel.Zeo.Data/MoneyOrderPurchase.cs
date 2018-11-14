﻿using System;

namespace TCF.Channel.Zeo.Data
{
    public class MoneyOrderPurchase
    {
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
		public string PromotionCode { get; set; }
		public bool IsSystemApplied { get; set; }
    }
}
