-- =============================================
-- Author:		Kaushik Sakala		
-- Create date: 10/18/2016	
-- Description:	To get account by card number
-- =============================================

IF OBJECT_ID(N'usp_CheckAccountExists', N'P') IS NOT NULL
DROP PROC usp_CheckAccountExists
GO

CREATE PROCEDURE usp_CheckAccountExists
		@isActive   BIT,
		@customerId BIGINT
AS
BEGIN
BEGIN TRY
	SELECT 
		CAST(COUNT(1) AS BIT)  AS AccountExists 
	FROM
		tVisa_Account
	WHERE 
	 CustomerID = @customerId AND Activated = @isActive
END TRY

BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
