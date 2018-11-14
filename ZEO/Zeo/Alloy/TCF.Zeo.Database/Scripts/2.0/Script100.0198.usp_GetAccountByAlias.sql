-- =============================================
-- Author:		Shwetha		
-- Create date: 10/18/2016	
-- Description:	To get account by card alias id
-- =============================================

IF OBJECT_ID(N'usp_GetAccountByAlias', N'P') IS NOT NULL
DROP PROC usp_GetAccountByAlias
GO

CREATE PROCEDURE usp_GetAccountByAlias
	(
		@cardAliasId VARCHAR(50)
	)
AS
BEGIN
BEGIN TRY
	SELECT 
		CustomerSessionID,
		CustomerRevisionNo,
		ProxyId,
		PseudoDDA,
		CardNumber,
		Activated,
		SubClientNodeId,
		DTAccountClosed,
		PrimaryCardAliasId,
		ActivatedLocationNodeId,
		VisaAccountID

	FROM
		tVisa_Account
	WHERE
		CardAliasId = @cardAliasId
END TRY

BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
END CATCH

END
GO
