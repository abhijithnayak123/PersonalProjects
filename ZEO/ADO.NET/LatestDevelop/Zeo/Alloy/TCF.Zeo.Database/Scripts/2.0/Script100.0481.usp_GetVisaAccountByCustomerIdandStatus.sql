-- =============================================
-- Author:		KARUN
-- Create date: 04/10/2017	
-- Description:	Get Visa Account By CustomerId and / or Activated Status
-- =============================================

IF OBJECT_ID(N'usp_GetVisaAccountBycustomerIdandStatus', N'P') IS NOT NULL
DROP PROC usp_GetVisaAccountBycustomerIdandStatus
GO

CREATE PROCEDURE usp_GetVisaAccountBycustomerIdandStatus 
		@customerID  BIGINT,
		@activated BIT = NULL
AS
BEGIN
BEGIN TRY

	SELECT
		VisaAccountID,
		ProxyId,
		PseudoDDA,
		CardNumber,
		CardAliasId,
		PrimaryCardAliasId,
		ExpirationMonth,
		ExpirationYear,
		SubClientNodeId,
		ActivatedLocationNodeId,
		Activated,
		FraudScore,
		DTAccountClosed,
		ActivatedLocationNodeId
	From
		tVisa_Account 
	WHERE 
		CustomerID = @customerID AND 
		Activated = COALESCE(@activated,Activated) 
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH

END
GO
