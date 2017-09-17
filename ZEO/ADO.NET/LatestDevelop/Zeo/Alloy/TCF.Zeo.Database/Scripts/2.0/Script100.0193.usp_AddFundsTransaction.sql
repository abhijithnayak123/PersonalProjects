-- =============================================
-- Author:		Shwetha		
-- Create date: 10/18/2016	
-- Description:	To add fund transaction 
-- =============================================
IF OBJECT_ID(N'usp_AddFundsTransaction', N'P') IS NOT NULL
DROP PROC usp_AddFundsTransaction 
GO

CREATE PROCEDURE usp_AddFundsTransaction
	(
		@transactionId BIGINT OUTPUT,
		@amount MONEY,
		@baseFee MONEY,
		@discountApplied MONEY,
		@additionalFee MONEY,
		@fee MONEY,
		@fundType INT,
		@customerSessionId BIGINT,
		@addOnCustomerId BIGINT,
		@dTTerminalCreate DATETIME,
		@dTServerCreate DATETIME,
		@state INT,
		@providerID INT,
		@confirmationNumber VARCHAR(50)
	)
AS
BEGIN
	BEGIN TRY
	

	DECLARE @customerRevisionNo INT,
			@providerAccountID BIGINT
	
	-- Get latest customer revision number

	 SELECT 
	   @customerRevisionNo = ISNULL(MAX(ca.RevisionNo),0)
	 FROM tCustomers_Aud AS ca
	 INNER JOIN tCustomerSessions AS cs ON ca.CustomerID = cs.CustomerID
	 WHERE cs.CustomerSessionID = @customerSessionId

	 SELECT @providerAccountID = va.VisaAccountID
	 FROM tCustomerSessions cs 
	 INNER JOIN tVisa_Account va on va.CustomerId = cs.CustomerID
	 WHERE CS.CustomerSessionID = @customerSessionId

	INSERT INTO
		tTxn_Funds
		(
			Amount,
			BaseFee,
			DiscountApplied,
			AdditionalFee,
			Fee,
			FundType,
			CustomerSessionId,
			AddOnCustomerId,
			State,
			ProviderId,
			ProviderAccountId,
			CustomerRevisionNo,
			ConfirmationNumber,
			DTTerminalCreate,
			DTServerCreate
		)
	VALUES
		(
			@amount ,
			@baseFee ,
			@discountApplied ,
			@additionalFee ,
			@fee ,
			@fundType ,
			@customerSessionId ,
			@addOnCustomerId ,
			@state,
			@providerID,
			@providerAccountID,
			@customerRevisionNo,
			@confirmationNumber,
			@dTTerminalCreate ,
			@dTServerCreate
		)	
		
		SET @transactionId = CAST(SCOPE_IDENTITY() AS bigint)
		
	END TRY
	BEGIN CATCH	
	
		EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError

	END CATCH
END
GO
