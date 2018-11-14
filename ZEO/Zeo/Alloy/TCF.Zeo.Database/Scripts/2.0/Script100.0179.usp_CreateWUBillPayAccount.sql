--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <18-11-2016>
-- Description:	Create WU bill pay account
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('usp_CreateWUBillPayAccount') IS NOT NULL
	 BEGIN
		  DROP PROCEDURE usp_CreateWUBillPayAccount
	 END
GO

CREATE PROCEDURE usp_CreateWUBillPayAccount 
(
     @accountId         BIGINT OUTPUT,
     @customerId        BIGINT,															 
	 @customerSessionID BIGINT,	 																 
	 @cardNumber        VARCHAR(50),
	 @dtTerminalDate    DATETIME,																		 
	 @dtServerDate      DATETIME
)
AS
	 BEGIN TRY
                  		 
			--  Getting the custmerRevisionNo from tcustomers_Aud table

			DECLARE @customerRevisionNo INT =
			(
			   SELECT 
				    ISNULL(MAX(RevisionNo), 0)
			   FROM 
				   tCustomers_Aud WITH (NOLOCK)							
			   WHERE
				   CustomerId = @customerId					 
			);

			INSERT INTO dbo.tWUnion_BillPay_Account
			(
			 CustomerID,
			 CustomerSessionID,
			 CustomerRevisionNo,					  
			 CardNumber,
			 DTTerminalCreate,
			 DTServerCreate
			)
			VALUES
			(
			 @customerId,
			 @customerSessionID,
			 @customerRevisionNo,					  
			 @cardNumber,
			 @dtServerDate,
			 @dtTerminalDate
			);
			
		SELECT @accountId = SCOPE_IDENTITY()					

	 END TRY

	 BEGIN CATCH
		  EXECUTE dbo.usp_CreateErrorInfo;
	 END CATCH;