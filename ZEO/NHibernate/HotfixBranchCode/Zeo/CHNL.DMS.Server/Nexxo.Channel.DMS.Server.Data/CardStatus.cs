namespace MGI.Channel.DMS.Server.Data
{
	public enum CardStatus
	{
		NotApplicable = -1,
		Active = 0,
		PendingCardIssuance = 1,
		CardIssued = 2,
		Suspended = 3,
		LostCard = 5,
		StolenCard = 6,
		ExpiredCard = 7,
		PendingAccountClosure = 8,
		Closed = 9,
		ClosedForFraud = 10,
		ReturnedUndeliverable = 11,
		ResearchRequired = 12,
		Voided = 13,
		Damaged = 14,
		Stale = 15,
		PendingDestruction = 16,
		Destroyed = 17,
		ClosedDueToUpgrade = 18,
		ClosedForDeceased = 19,
		NA = 100,
	}
}
