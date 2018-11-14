-- =============================================
-- Author:		Kaushik Sakala		
-- Create date: 10/11/2016	
-- Description:	To get card class by state code
-- =============================================

IF OBJECT_ID(N'usp_GetAliasIdByCustomerId', N'P') IS NOT NULL
DROP PROC usp_GetAliasIdByCustomerId
GO

CREATE PROCEDURE usp_GetAliasIdByCustomerId 
	@customerId BIGINT,
	@isPrimary BIT,
	@true BIT = 1
AS
BEGIN
BEGIN TRY
	SELECT 
		 CASE WHEN @isPrimary =  @true 
			THEN PrimaryCardAliasId
		ELSE 
			CardAliasId
		END AS AliasId
	FROM
		tVisa_Account
	WHERE 
		CustomerID = @customerId
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH
END
GO

