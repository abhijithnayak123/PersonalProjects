
-- ================================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <11/20/2015>
-- Description:	<Alter Table to Add CardExpiryPeriod column in tProductProcessorsMapping table>
-- Jira ID:		<AL-16377>
-- ================================================================================

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE TABLE_NAME = 'tProductProcessorsMapping'
			AND COLUMN_NAME = 'CardExpiryPeriod'
		)
BEGIN
	ALTER TABLE [dbo].[tProductProcessorsMapping]
	ADD CardExpiryPeriod INT DEFAULT(0) NOT NULL
END
GO





