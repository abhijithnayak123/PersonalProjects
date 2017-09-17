--- ================================================================================
-- Author:		<Abhijith>
-- Create date: <11/04/2015>
-- Description:	<As Alloy, I need a minimum age for customers to register in the system to be configurable.>
-- Jira ID:		<AL-1626>
-- ================================================================================

IF NOT EXISTS
(
	SELECT 1
    FROM   INFORMATION_SCHEMA.COLUMNS
    WHERE  TABLE_NAME = 'tChannelPartnerConfig'
               AND COLUMN_NAME = 'CustomerMinimumAge'
) 
BEGIN
	
	ALTER TABLE tChannelPartnerConfig
	ADD CustomerMinimumAge INT DEFAULT 18
	
END	


