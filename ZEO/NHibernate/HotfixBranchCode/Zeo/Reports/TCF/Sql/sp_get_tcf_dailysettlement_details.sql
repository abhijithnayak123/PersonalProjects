
IF OBJECT_ID('sp_get_tcf_dailysettlement_details', 'P') IS NOT NULL
DROP PROC sp_get_tcf_dailysettlement_details
GO


CREATE PROCEDURE sp_get_tcf_dailysettlement_details
@tran_start_date DATETIME = NULL,
@tran_end_date DATETIME = NULL
AS   
BEGIN --{   
/*==============================================================================  
 *  
 *  NAME:  sp_get_tcf_dailysettlement_details  
 *  
 *  DESC:  1.	The below stored procedure retreives all the daily settlement transaction details for TCF from ODS DB
 *		   2.	By default the data is extracted for the current business day covering previous day transactions 	
 *		   3.	The SP can also be passed with any custome date as reqired	
 *  
 * ==============================================================================  
 *  
 *  RETS:  0 on success, -1 on failure  
 *  
 * ==============================================================================  
 *  
 *  HIST:  
 *  
 *    Date		Programmer             Description  
 *  --------	------------           -------------  ----------------------------------------------------  
 *  08/30/2016  MGI-Onsite 			  * Initial Version 
 *  10/11/2016  MGI-Onsite			  * Discount greater than Zero to be multiplied by (-1)	
 *  10/18/2016  MGI-Onsite			  * Distinct records to be selected to avoid picking duplicate [failed check processing] transactions
*==============================================================================*/  
 
-- Declare Variables
DECLARE 
	@ErrorMessage    NVARCHAR(4000),
	@ErrorNumber     INT,
	@ErrorSeverity   INT,
	@ErrorState      INT,
	@ErrorLine       INT,
	@ErrorProcedure  NVARCHAR(200);

-- Set Tran date for daily default run unless the dates have been passed by the user for a different date range
IF (@tran_start_date is null or @tran_end_date is null)
BEGIN

	SET @tran_start_date = cast(dateadd(dd,-1,getdate()) as date)
	SET @tran_end_date = cast(getdate() as Date) 

END

 BEGIN TRY

 BEGIN TRANSACTION

 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.
  SET NOCOUNT ON; 
 
 
 -- Begin data extract SQL query
 -- If the transaction date is null tran_date should be set to previous business date

 SELECT DISTINCT
 tt.ClientId as ClientID,
 str(tt.[TransactionId],10) as TransactionID,
 tt.SessionId as SessionID,
 tt.NexxoPan as MGIPAN,
 tc.ClientCustomerId as CustomerID,
 tc.FirstName as FirstName,
 tc.LastName as lastName,
 tt.TellerUserName as TelleruserName,
 tt.TellerId as TellerID,
 tt.ProviderId as ProviderID,
 tt.ProviderTxnId as ProviderTxnID,
 tt.ProviderResponseCode as ProviderResponseCode,
 tt.TransactionType as TransactionType,
 tt.TransactionDescription as transactionDescription,
 tt.TransactionAmount as TransactionAmount,
 CustomerFees = CASE WHEN (tt.Promotion = '' or tt.Promotion is null)
 THEN(
 CASE WHEN 
	(tt.[TransactionType] in ('BillPay', 'Check Processing', 'SendMoney', 'ReceiveMoney'))
	THEN  
	isnull(ROUND(tt.[CustomerFees],2),0)
	else
	isnull(ROUND(tt.[BaseFee],2),0)
	end)
 else isnull(ROUND(tt.[BaseFee],2),0) END,
 Discount = case when tt.DiscountApplied>0 then (tt.DiscountApplied*-1)
			else tt.DiscountApplied end,
 tt.NetTransactionAmount as NetTransactionAmount,
 ISNULL(tt.Promotion,'') as PromoCode,
 ISNULL(tt.PromotionDescription,'') as PromoName,
CAST(ISNULL(CASE WHEN tt.[FinalStatus] = 4 then 'Committed' 
	WHEN tt.[FinalStatus] = 5 then 'Failed' 
	WHEN tt.[FinalStatus] = 6 then 'Cancelled' 
	WHEN tt.[FinalStatus] = 8 then 'Declined' 
	ELSE cast(tt.[FinalStatus] as nvarchar(2)) 
	END,'') as nvarchar(10))
 as TransactionStatus,
 tt.LocationID as LocationID,
 tt.Channel as Channel,
 convert(varchar(10),tt.[DTServer], 101) + right(convert(varchar(32),tt.[DTServer],100),8) as Create_DateTime,
 convert(varchar(10),tt.[DTTerminalCreate], 101) + right(convert(varchar(32),tt.[DTTerminalCreate],100),8) as DTLocal,
 CASE
	WHEN (tl.BankID = '' or tl.BankID is null ) then ('000')
	ELSE tl.BankID
 END AS BankID,
 CASE
	WHEN (tl.BranchID = '' or tl.BranchID is null) then ('00000')
	ELSE tl.BranchID
 END as BranchID,
 convert(varchar(10),tt.BusinessDate, 101) as BusinessDate
 FROM 
 tTransaction tt 
 left join tCustomer tc on tt.NexxoPan = tc.NexxoPAN
 and tc.Revision = (select max(Revision) from tCustomer tc1 where tc1.CustomerId = tc.CustomerId)
 left join tLocation tl on tt.LocationID = tl.LocationID
 and tl.Revision = (select max(Revision) from tLocation tl1 where tl1.LocationID = tl.LocationID)
 WHERE tt.ClientId = 34
 and cast(tt.DTServer  as varchar(12))>= @tran_start_date
 and cast(tt.DTServer  as varchar(12))< @tran_end_date
 and tt.TransactionSubType not in (1,3)
 ORDER by TransactionID
 
 COMMIT TRANSACTION

 END TRY


 BEGIN CATCH

 IF @@ERROR > 0  
    BEGIN  

    -- Assign variables to error-handling functions that 
    -- capture information for RAISERROR.
    SELECT 
        @ErrorNumber = ERROR_NUMBER(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE(),
        @ErrorLine = ERROR_LINE(),
        @ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-'),
		@ErrorMessage = ERROR_MESSAGE();

    RAISERROR 
        (
        @ErrorMessage, 
        @ErrorSeverity, 
        1,               
        @ErrorNumber,    -- parameter: original error number.
        @ErrorSeverity,  -- parameter: original error severity.
        @ErrorState,     -- parameter: original error state.
        @ErrorProcedure, -- parameter: original error procedure name.
        @ErrorLine       -- parameter: original error line number.
        );

        ROLLBACK TRANSACTION
			
    END  
	
 END CATCH 

 END
GO
