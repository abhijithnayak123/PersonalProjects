--- ===================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <11/04/2015>
-- Description:	<As Alloy, I need a minimum age for customers to transact in the system>
-- Jira ID:		<AL-1628>
-- =====================================================================================

-- Setting the default value 

IF EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tChannelPartnerProductProcessorsMapping'
		  AND COLUMN_NAME = 'MinimumTransactAge'
		)
BEGIN
	UPDATE tChannelPartnerProductProcessorsMapping
	SET MinimumTransactAge = 18
	WHERE MinimumTransactAge IS NULL
END
GO