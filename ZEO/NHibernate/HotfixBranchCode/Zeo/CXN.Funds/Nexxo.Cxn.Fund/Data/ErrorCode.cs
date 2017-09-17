namespace MGI.Cxn.Fund.Data
{
	public enum ErrorCode
	{
		Success,
		Transaction_Failed,
		Could_Not_Authenticate_Card,
		Could_Not_Retrieve_Credentials,
		Could_Not_Retrieve_Card_Info,
		Could_Not_Build_RequestURL,
		Could_Not_Retrieve_Card_Balance,
		Could_Not_Lookup_Card_Info,
		Could_Not_Update_Card,
		Could_Not_Post_Credit,
		Could_Not_Post_Debit
	}
}
