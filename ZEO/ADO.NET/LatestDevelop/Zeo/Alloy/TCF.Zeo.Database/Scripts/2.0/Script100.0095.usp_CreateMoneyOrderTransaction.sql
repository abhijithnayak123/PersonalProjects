-- ================================================================================
-- Author:		Nishad Varghese
-- Create date: 01/Oct/2016
-- Description:	Update MO status
-- Jira ID:		AL-7706
-- ================================================================================

IF OBJECT_ID(N'usp_CreateMoneyOrderTransaction', N'P') IS NOT NULL
DROP PROCEDURE usp_CreateMoneyOrderTransaction   -- Drop SP
GO

CREATE PROCEDURE [dbo].[usp_CreateMoneyOrderTransaction]
(
	 @customerSessionId   BIGINT,                                                       
	 @amount              MONEY,                                                       
	 @fee                 MONEY,                                                       
	 @state               INT,                                                       
	 @baseFee             MONEY,                                                       
	 @discountApplied     MONEY,                                                       
	 @additionalFee       MONEY,                                                       
	 @discountName        VARCHAR(50),                                                       
	 @discountDescription VARCHAR(100),                                                       
	 @isSystemApplied     BIT,                                                       
	 @purchaseDate        DATETIME,                                                      
	 @dTTerminalCreate    DATETIME,                                                       
	 @dTServerCreate      DATETIME                                                      
)
 AS
BEGIN
	 BEGIN TRY
		  DECLARE @transactionId BIGINT
		  DECLARE @customerRevisionNo INT

		 

          --Getting the customer Revision No from tCustomers_Aud table
		  SELECT @customerRevisionNo = ISNULL(MAX(ca.RevisionNo), 0)
		  FROM tCustomers_Aud AS ca WITH (NOLOCK)
				 INNER JOIN tCustomerSessions AS cs WITH (NOLOCK) ON ca.CustomerID = cs.CustomerID
		  WHERE cs.CustomerSessionID = @customerSessionId

		  INSERT INTO [dbo].[tTxn_MoneyOrder]
		  ( 
			[Amount],
			[Fee],
			[State],
			[BaseFee],
			[DiscountApplied],
			[AdditionalFee],
			[DiscountName],
			[DiscountDescription],
			[IsSystemApplied],
			[PurchaseDate],
			[CustomerSessionId],
			[CustomerRevisionNo],
			[DTTerminalCreate],
			[DTServerCreate]
		  )
		  VALUES
		  ( 
			@amount,
			@fee,
			@state,
			@baseFee,
			@discountApplied,
			@additionalFee,
			@discountName,
			@discountDescription,
			@isSystemApplied,
			@purchaseDate,
			@customerSessionId,
			@customerRevisionNo,
			@dTTerminalCreate,
			@dTServerCreate
		  )

          SELECT CAST(SCOPE_IDENTITY() AS BIGINT) as TransactionId
	 END TRY

	 BEGIN CATCH
		  EXECUTE usp_CreateErrorInfo;
	 END CATCH
END
GO