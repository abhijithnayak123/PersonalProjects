-- ================================================================================
-- Author:		Manikandan Govindraj
-- Create date: 12/07/2016
-- Description: Create cash transaction
-- Jira ID:		AL-8047
-- ================================================================================


IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'usp_AddCash')
BEGIN
	DROP PROCEDURE usp_AddCash
END
GO


CREATE PROCEDURE usp_AddCash
(
    @transactionId     BIGINT OUTPUT,
	@customerSessionId BIGINT,
	@amount            MONEY,
	@state             INT,
	@cashType          INT,
	@dTTerminalCreate  DATETIME,
	@dTServerCreate    DATETIME	
)
AS
BEGIN
	
	BEGIN TRY

     INSERT INTO tTxn_Cash
            (
			  CustomerSessionId,
			  Amount,
			  [State],
			  CashType,
			  DTTerminalCreate,
			  DTServerCreate
			)
	  VALUES
	        (
			  @customerSessionId,
			  @amount,
			  @state,
			  @cashType,    
			  @dTTerminalCreate,
			  @dTServerCreate	  
	         )

	 SELECT @transactionId = CAST(SCOPE_IDENTITY() AS BIGINT)

END TRY

BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH

END