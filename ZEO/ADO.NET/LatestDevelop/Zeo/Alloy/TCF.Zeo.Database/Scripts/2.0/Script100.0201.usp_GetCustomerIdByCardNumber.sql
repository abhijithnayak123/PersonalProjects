-- =============================================
-- Author:		Kaushik Sakala
-- Create date: 10/18/2016	
-- Description:	To get CustomerId from CardNumer
-- =============================================

IF OBJECT_ID(N'usp_GetCustomerIdByCardNumber', N'P') IS NOT NULL
DROP PROC usp_GetCustomerIdByCardNumber
GO

CREATE PROCEDURE usp_GetCustomerIdByCardNumber
	@cardNumber VARCHAR(50)
	
AS
BEGIN
BEGIN TRY
	SELECT 
		CustomerId,
		CardNumber
		FROM
			tVisa_Account
		WHERE
			CardNumber = @cardNumber
END TRY

BEGIN CATCH
	EXECUTE usp_CreateErrorInfo
END CATCH

END
GO
