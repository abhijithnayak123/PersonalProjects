-- =========================================================================================
-- Author:		<Kaushik Sakal>
-- Create date: <12/12/2016>
-- Description:	<As an engineer, I want to implement ADO.Net for Transaction History module>
-- Jira ID:		<AL-8869>
-- ==========================================================================================


IF EXISTS(
	SELECT 1
	FROM SYS.objects
	WHERE NAME = 'usp_GetCustomerTransactionLocations'
)

BEGIN
	DROP PROCEDURE usp_GetCustomerTransactionLocations
END
GO

CREATE PROCEDURE usp_GetCustomerTransactionLocations
	@customerId BIGINT,
	@dateRange DATETIME,
	@channelPartnerId BIGINT
AS
BEGIN
	BEGIN TRY		
		SELECT 
			DISTINCT(location)
		FROM 
			vTransactionHistory t
			INNER JOIN tLocations l ON upper(l.LocationName) = upper(t.Location)
		WHERE 
			CustomerId = @customerId 
			AND (TransactionDate >= @dateRange)
			AND l.ChannelPartnerId = @channelPartnerId
	END TRY
	BEGIN CATCH 
		EXECUTE usp_CreateErrorInfo;  
	END CATCH
END
GO