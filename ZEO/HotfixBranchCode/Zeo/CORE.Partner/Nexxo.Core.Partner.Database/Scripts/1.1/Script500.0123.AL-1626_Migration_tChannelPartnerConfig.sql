--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <11/04/2015>
-- Description:	<As Alloy, I need a minimum age for customers to register in the system to be configurable.>
-- Jira ID:		<AL-1626>
-- ================================================================================

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerConfig'
		  AND COLUMN_NAME = 'CustomerMinimumAge'
		)
BEGIN
	UPDATE tChannelPartnerConfig
	SET CustomerMinimumAge = 18
	WHERE CustomerMinimumAge IS NULL
END
GO 
