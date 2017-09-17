namespace MGI.Cxn.BillPay.Data
{
	public enum ErrorCode
	{
		Success,
		Biller_Lookup_Failed,
		No_Matching_Product_Found,
		Invalid_Credentials,
		Could_Not_Retrive_Processor_Credentials,
		Could_Not_Validate_Biller,
		Error_While_Validating_Biller,
		Data_Prompts_Need_To_Be_Answered,
		BillerZip_Need_To_Be_Answered,
		Invalid_Customer_Details,
		Invalid_Customer_Account_Number,
		Processor_Error,
		Could_Not_Build_Transaction_Object,
		Could_Not_Post_Payment,
		No_Mapping_TransactionRef_Found,
		TransmissionFailure,
		DuplicateTransaction,
		AccountError,
		TelephoneNumberNotUnique
	}
}
