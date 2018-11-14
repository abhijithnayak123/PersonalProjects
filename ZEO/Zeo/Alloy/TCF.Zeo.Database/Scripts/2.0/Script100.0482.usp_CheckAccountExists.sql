-- =============================================
-- Author:		Karun		
-- Create date: 04/11/2017	
-- Description:	Check whether account exists in tvisa accounts table.
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
	 CustomerID = @customerId AND Activated = COALESCE(@isActive,Activated) 
END TRY

BEGIN CATCH
		EXECUTE usp_CreateErrorInfo
END CATCH
END
GO
