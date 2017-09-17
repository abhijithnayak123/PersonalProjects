-- =============================================
-- Author:		Shwetha		
-- Create date: 10/18/2016	
-- Description:	To Update Visa Account
-- =============================================


IF OBJECT_ID(N'usp_UpdateVisaAccountByCustomerID', N'P') IS NOT NULL
DROP PROC usp_UpdateVisaAccountByCustomerID
GO

CREATE PROCEDURE usp_UpdateVisaAccountByCustomerID
	@cardAliasId VARCHAR(50),
	@primaryAliasId VARCHAR(50),
	@proxyId VARCHAR(25),
	@pseudoDDA VARCHAR(50),
	@cardNumber VARCHAR(50),
	@expmonth INT,
	@expyear INT,
	@subClientNodeId BIGINT,
	@dTTerminalLastModified DATETIME,
	@dTServerLastModified DATETIME,
	@customerSessionId BIGINT,
	@activated bit,
	@ActivatedLocationNodeId BIGINT,
	@CustomerId BIGINT,
	@isFraud INT

AS
BEGIN
BEGIN TRY

	DECLARE @revisionNumber INT
	SELECT @revisionNumber = MAX (RevisionNo) FROM tCustomers_Aud 
	WHERE CustomerID = @customerId

	UPDATE
		tVisa_Account
	SET
	 PrimaryCardAliasId = @primaryAliasID,
	 DTServerLastModified = @dTServerLastModified,
	 DTTerminalLastModified = @dTTerminalLastModified,
	 CardNumber = @cardNumber,
	 Activated = @activated,
	 CardAliasId = @cardAliasId,
	 ProxyId = @proxyId,
	 PseudoDDA = @pseudoDDA,
	 SubClientNodeId = @subClientNodeId,
	 ActivatedLocationNodeId = CASE WHEN @activated = 0 THEN @ActivatedLocationNodeId ELSE ActivatedLocationNodeId END,
	 CustomerSessionId = CASE WHEN @activated = 0 THEN @customerSessionId ELSE CustomerSessionId END,
	 ExpirationMonth = @expmonth,
	 ExpirationYear = @expyear,
	 FraudScore = @isFraud,
	 CustomerRevisionNo =  CASE WHEN @activated = 0 THEN @revisionNumber ELSE CustomerRevisionNo END
	WHERE
		CustomerId = @CustomerId
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO