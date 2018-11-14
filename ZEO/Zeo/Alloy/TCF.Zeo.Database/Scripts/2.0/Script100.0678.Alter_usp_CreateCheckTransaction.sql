--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Modify By : Nitish Biradar
-- Create date: <08-04-2016>
-- Modify Date : 06-07-2018
-- Description:	 Create check transaction and add check image
-- Jira ID:		<AL-7582>
-- ================================================================================

IF OBJECT_ID(N'usp_CreateCheckTransaction', N'P') IS NOT NULL
DROP PROC usp_CreateCheckTransaction
GO


CREATE PROCEDURE usp_CreateCheckTransaction
(
    @transactionId BIGINT OUTPUT,
	@providerAccountId BIGINT,
	@providerId INT,
	@customerSessionId BIGINT,
	@amount MONEY,
	@fee MONEY,
	@description NVARCHAR(250),
	@state INT ,
	@confirmationNumber VARCHAR(50),
	@baseFee MONEY,
	@discountApplied MONEY,
	@additionalfee MONEY,
	@discountName VARCHAR(50),
	@discountDescription NVARCHAR(100),
	@isSystemApplied BIT,
	@MICR NVARCHAR(100),
	@checkType NVARCHAR(100),	
	@frontImage VARBINARY(MAX),
    @backImage VARBINARY(MAX),
    @format varchar(10),
	@initiatedProviderId INT,
	@dTTerminalCreate DATETIME,
	@dTServerCreate DATETIME
	
)
AS
BEGIN
	
	BEGIN TRY

	--DECLARE @checkTypeId INT
	DECLARE @customerRevisionNo INT
	
	-- Get latest customer revision number

	 SELECT 
	   @customerRevisionNo = ISNULL(MAX(ca.RevisionNo),0) 
	 FROM
	   tCustomers_Aud AS ca
	 INNER JOIN 
	   tCustomerSessions AS cs
	 ON
	   ca.CustomerID = cs.CustomerID
	 WHERE 
	   cs.CustomerSessionID = @customerSessionId
	
	-- Insert the transaction with authorized state.

    INSERT INTO tTxn_Check
	(	   
		Amount,
		Fee,
		Description,
		State,
		ConfirmationNumber,
		BaseFee,
		DiscountApplied,
		Additionalfee,
		DiscountName,
		DiscountDescription,
		IsSystemApplied,
		CheckType,
		CustomerSessionId,
		ProviderAccountId,
		ProviderId,
		CustomerRevisionNo,
		MICR,
		InitiatedProviderId,
		DTTerminalCreate,
		DTServerCreate 
	)	
	VALUES
	(	   
		@amount,
		@fee,
		@description,
		@state,
		@confirmationNumber,
		@baseFee,
		@discountApplied,
		@additionalfee,
		@discountName,
		@discountDescription,
		@isSystemApplied,
		@checkType,
		@customerSessionId,
		@providerAccountId,
		@providerId,
		@customerRevisionNo,
		@MICR,
		@initiatedProviderId,
		@dTTerminalCreate,
		@dTServerCreate 
	)

	-- Inser check image details 
	SELECT @transactionId = CAST(SCOPE_IDENTITY() AS BIGINT)

	INSERT INTO tCheckImages
     (
        Front,
        Back,
        Format,
        DTServerCreate,
        TransactionId
     )
     VALUES
     (
		@frontImage,
		@backImage,
		@format,
		@dTServerCreate,
        @transactionId
	)

END TRY

BEGIN CATCH

    EXEC usp_CreateErrorInfo  -- Create the error in 'tErrorLog' table and RaiseError
		
END CATCH

END
GO


