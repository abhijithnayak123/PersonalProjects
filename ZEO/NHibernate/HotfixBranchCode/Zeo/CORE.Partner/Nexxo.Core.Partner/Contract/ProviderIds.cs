namespace MGI.Core.Partner.Contract
{
	public enum ProviderIds
	{
		// Cash/Other 1 - 99
		Cash = 1,

		// Funds 100 - 199
		NexxoPurse = 100,
		FirstView = 101,
		TSys = 102,
		Visa = 103,

		// Checks 200 - 299
		Ingo = 200,
		Certegy = 201,

		// Money Transfer 300 - 399
		NexxoMoneyTransfer = 300,
		WesternUnion = 301,
		MoneyGram = 302, //Changes for MGI 

		// Bill Pay AND TopUp 400 - 499
		CheckFree = 400,
		WesternUnionBillPay = 401,
		TIO = 402,
		Movilix = 403,
        MoneyGramBillPay=405,

		// Money Order 500 - 599
		OrderExpress = 500,
		WoodForest = 501,
		Nexxo = 502,
        Continental =503,
        TCF = 504,
        MGIMoneyOrder=505,

        //Customer 600 - 699
        FIS = 600,
        CCISCustomer=601,
        TCISCustomer=602
	}
}
