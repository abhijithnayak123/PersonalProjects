--- ===================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <11/04/2015>
-- Description:	<As Alloy, I need a minimum age for customers to transact in the system>
-- Jira ID:		<AL-1628>
-- =====================================================================================

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerProductProcessorsMapping'
		  AND COLUMN_NAME = 'MinimumTransactAge'
		)
BEGIN
	ALTER TABLE tChannelPartnerProductProcessorsMapping
	ADD MinimumTransactAge INT DEFAULT (18)
END
GO

