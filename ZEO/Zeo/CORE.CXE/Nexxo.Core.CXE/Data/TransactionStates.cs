using System;

namespace MGI.Core.CXE.Data
{
	// just putting some values here to work with
	// these states are subject to change
	public enum TransactionStates : int
	{
		Pending = 1,
		Authorized = 2,
		AuthorizationFailed = 3,
		Committed = 4,
		Failed = 5,
		Canceled = 6,
		Expired = 7,
		Declined = 8,
		Initiated = 9,
		Hold = 10,
		Priced = 11,
		Processing = 12, 
		CommittedReversed = 14
	}
}
