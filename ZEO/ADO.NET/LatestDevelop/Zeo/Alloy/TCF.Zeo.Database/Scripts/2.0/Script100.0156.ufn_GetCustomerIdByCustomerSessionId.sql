--- ===============================================================================
-- Author:		<Purna Pushkal>
-- Create date: <18-11-2016>
-- Description:	 GetCustomerId by Customer Session Id
-- Jira ID:		<AL-8320>
-- ================================================================================

IF OBJECT_ID('ufn_GetCustomerIdByCustomerSessionId') IS NOT NULL
	 BEGIN
		  DROP FUNCTION dbo.ufn_GetCustomerIdByCustomerSessionId
	 END
GO

CREATE FUNCTION ufn_GetCustomerIdByCustomerSessionId(@customerSessionId BIGINT)
RETURNS BIGINT
AS
BEGIN
	 DECLARE @customerId BIGINT;

	 SELECT @customerId = tc.CustomerID
	 FROM tCustomers tc WITH (NOLOCK)
			INNER JOIN tCustomerSessions tcs WITH (NOLOCK) ON tcs.CustomerID = tc.CustomerID
	 WHERE tcs.CustomerSessionID = @customerSessionId

	 RETURN @customerId
END