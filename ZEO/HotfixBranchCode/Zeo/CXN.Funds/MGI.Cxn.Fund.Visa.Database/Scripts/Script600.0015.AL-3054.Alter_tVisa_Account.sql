-- ================================================================================
-- Author:		<Rogy Eapen>
-- Create date: <12/07/2015>
-- Description:	<As Synovus, send Mothers Maiden Name in custom field for card purchase>
-- Jira ID:		<AL-3054>
-- ================================================================================
IF NOT EXISTS
(
	SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
	WHERE 
	TABLE_NAME = 'tVisa_Account' AND  COLUMN_NAME = 'MothersMaidenName'
)
BEGIN
	ALTER TABLE tVISA_Account
	ADD MothersMaidenName VARCHAR(200)
END
GO