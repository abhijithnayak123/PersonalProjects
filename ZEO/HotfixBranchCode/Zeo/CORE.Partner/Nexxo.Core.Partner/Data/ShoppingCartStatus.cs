﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MGI.Core.Partner.Data
{
	public enum ShoppingCartStatus
	{
		InitialCheckout = 1,
		CashOverCounter = 2,
		CashCollected = 3,
		FinalCheckout = 4,
		Completed = 5
	}
}
