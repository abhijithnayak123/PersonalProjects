-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <08/19/2015>
-- Description:	<Adding column to store the date of visa card 
--				 closure in tVisa_Account table>
-- Jira Id:		<AL-823>
-- ============================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tVisa_Account' AND COLUMN_NAME = 'DTAccountClosed')
BEGIN
	ALTER TABLE tVisa_Account
	ADD DTAccountClosed DATETIME NULL
END
GO
