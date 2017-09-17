-- =============================================
-- Author:		Shwetha		
-- Create date: 10/18/2016	
-- Description:	To get card class by state code
-- =============================================

IF OBJECT_ID(N'usp_GetVisaAccountBycustomerIdandStatus', N'P') IS NOT NULL
DROP PROC usp_GetVisaAccountBycustomerIdandStatus
GO

CREATE PROCEDURE usp_GetVisaAccountBycustomerIdandStatus 
		@customerID  BIGINT,
		@activated BIT = 0

AS
BEGIN
BEGIN TRY
	SELECT
		VisaAccountID,
		ProxyId,
		PseudoDDA,
		CardNumber,
		CardAliasId,
		ExpirationMonth,
		ExpirationYear,
		SubClientNodeId,
		PrimaryCardAliasId,
		Activated,
		FraudScore,
		DTAccountClosed
	From
		tVisa_Account 
	WHERE 
		CustomerID = @customerID AND Activated = @activated
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH

END
GO
