-- =============================================
-- Author:		Kaushik Sakala
-- Create date: 10/18/2016	
-- Description:	To check if card is valid 
-- =============================================

IF OBJECT_ID(N'usp_ValidateCard', N'P') IS NOT NULL
DROP PROC usp_ValidateCard
GO

CREATE PROCEDURE usp_ValidateCard
	@cardNumber VARCHAR(50),
	@customerId BIGINT
AS
BEGIN
BEGIN TRY

DECLARE @true BIT = 1,	@false BIT = 0

SELECT CASE WHEN (
	SELECT COUNT(1)
		FROM
			tVisa_Account
		WHERE
			CardNumber = @cardNumber 
		AND CustomerId ! = @customerId
) = 0 THEN @true ELSE @false END AS ValidCard
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH

END
GO
