--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <18-11-2016>
-- Description:	Create/Update Favourite biller 
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('usp_CreateOrUpdateFavouriteBiller') IS NOT NULL
BEGIN
	DROP PROCEDURE dbo.usp_CreateOrUpdateFavouriteBiller
END
GO

CREATE PROCEDURE usp_CreateOrUpdateFavouriteBiller
(
	 @transactionId  BIGINT,																	
	 @state          INT,																	
	 @dtTerminalDate DATETIME,																	
	 @dtServerDate   DATETIME
)
AS
BEGIN

	BEGIN TRY
		
		DECLARE @customerId BIGINT
		DECLARE @productId BIGINT
		DECLARE @accountNumber NVARCHAR(30)
		DECLARE @phoneNumber BIGINT
		DECLARE @accountDOB DATETIME

		SELECT 
			@customerId = tc.CustomerID,
			@phoneNumber = tc.Phone1,
			@accountDOB = tc.DOB,
			@productId = tb.ProductId,
			@accountNumber = tb.AccountNumber
		FROM 
			dbo.tCustomers tc WITH (NOLOCK)
			INNER JOIN dbo.tCustomerSessions tcs WITH (NOLOCK) ON tcs.CustomerID = tc.CustomerID
			INNER JOIN dbo.tTxn_BillPay tb WITH (NOLOCK) ON tb.CustomerSessionID = tcs.CustomerSessionID
			INNER JOIN dbo.tMasterCatalog tmc WITH (NOLOCK) ON tmc.MasterCatalogID = tb.ProductId
		WHERE 
			tb.TransactionID = @transactionId

		IF NOT EXISTS(SELECT 1 FROM dbo.tCustomerPreferedProducts WHERE ProductId = @productId AND CustomerID = @customerId)
		BEGIN
			INSERT INTO dbo.tCustomerPreferedProducts
			(
				CustomerID,
				ProductId,
				AccountNumber,
				PhoneNumber,
				AccountDOB,
				Enabled,
				DTTerminalCreate,
				DTServerCreate
			)
			VALUES
			(
				@customerId,
				@productId,
				@accountNumber,
				@phoneNumber,
				@accountDOB,
				1, --making the favourite biller active
				@dtTerminalDate,
				@dtServerDate
			)
		END

		ELSE
		BEGIN
			   -- Update Favourite biller details.   
				UPDATE 
					dbo.tCustomerPreferedProducts
				SET
					Enabled = 1,   --making the favourite biller active,
					AccountNumber = @accountNumber,
					DTTerminalLastModified = @dtTerminalDate,
					DTServerLastModified = @dtServerDate
				WHERE 
					CustomerID = @customerId
					AND ProductId = @productId
		END

		--updating the transaction state 
		EXECUTE usp_UpdateBillPayTransactionState
				@state,
				@transactionId,
				@dtTerminalDate,
				@dtServerDate
		  

	END TRY

	BEGIN CATCH
       EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
	END CATCH

END
GO

