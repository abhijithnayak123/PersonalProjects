--- ===============================================================================
-- Author:		<Abhijith>
-- Create date: <11-02-2016>
-- Description:	 Get WU card account details by customer session Id. 
-- Jira ID:		<AL-8324>

-- EXEC usp_GetWUCardAccountInfo 100000001
-- ================================================================================

IF OBJECT_ID(N'usp_GetWUCardAccountInfo', N'P') IS NOT NULL
DROP PROC usp_GetWUCardAccountInfo
GO


CREATE PROCEDURE usp_GetWUCardAccountInfo
(
    @customerSessionId BIGINT
)
AS
BEGIN
	
BEGIN TRY
	
	SELECT 
		wa.WUAccountID AS WUAccountId
		, wa.PreferredCustomerAccountNumber AS PreferredCustomerAccountNumber
		, wa.NameType AS NameType
		, wa.PreferredCustomerLevelCode AS PreferredCustomerLevelCode
		, wa.SmsNotificationFlag AS SmsNotificationFlag
		, c.FirstName AS FirstName
		, c.MiddleName AS MiddleName
		, c.LastName AS LastName
		, c.LastName2 AS SecondLastName
	FROM tCustomerSessions cs
		INNER JOIN tCustomers c ON cs.CustomerID = c.CustomerID
		INNER JOIN tWUnion_Account wa ON wa.CustomerID = c.CustomerID
	WHERE cs.CustomerSessionID = @customerSessionId
	
    
END TRY
BEGIN CATCH

	EXECUTE usp_CreateErrorInfo
		
END CATCH
END
GO
