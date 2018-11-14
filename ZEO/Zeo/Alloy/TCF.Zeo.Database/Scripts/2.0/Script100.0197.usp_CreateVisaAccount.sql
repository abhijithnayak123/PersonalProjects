-- =============================================
-- Author:		Shwetha		
-- Create date: 10/18/2016	
-- Description:	To create visa Account 
-- =============================================

IF OBJECT_ID(N'usp_CreateVisaAccount', N'P') IS NOT NULL
DROP PROC usp_CreateVisaAccount
GO

CREATE PROCEDURE usp_CreateVisaAccount
	(
		@cardAliasId VARCHAR(50),
		@primaryAliasId VARCHAR(50),
		@proxyId VARCHAR(25),
		@pseudoDDA VARCHAR(50),
		@cardNumber VARCHAR(50),
		@expmonth INT,
		@expyear INT,
		@subClientNodeId BIGINT,
		@activationNodeId BIGINT,
		@dtServerCreate DATETIME,
		@dtTerminalCreate DATETIME,
		@activated BIT,
		@customerId BIGINT,
		@customerSessionId BIGINT
	)
AS
BEGIN
BEGIN TRY
DECLARE @revisionNumber INT

	SELECT @revisionNumber = MAX (RevisionNo) FROM tCustomers_Aud 
	WHERE CustomerID = @customerId

	INSERT INTO tVisa_Account
		(
			 CardAliasId,
			 PrimaryCardAliasId,
			 ProxyId,
			 PseudoDDA,
			 CardNumber,
			 ExpirationMonth,
			 ExpirationYear,
			 SubClientNodeId,
			 ActivatedLocationNodeId,
			 Activated,
			 CustomerID,
			 CustomerSessionID,
			 CustomerRevisionNo,
			 DTServerCreate,
			 DTTerminalCreate
		 )
	 VALUES
		(
			@cardAliasId ,
			@primaryAliasId ,
			@proxyId ,
			@pseudoDDA ,
			@cardNumber ,
			@expmonth ,
			@expyear ,
			@subClientNodeId ,
			@activationNodeId ,
			@activated ,
			@customerId ,
			@customerSessionId ,
			@revisionNumber,			
			@dtServerCreate ,
			@dtTerminalCreate
		)
		SELECT CAST(SCOPE_IDENTITY() AS BIGINT) AS VisaAccountID
END TRY

BEGIN CATCH
		EXECUTE usp_CreateErrorInfo	
END CATCH

END
GO