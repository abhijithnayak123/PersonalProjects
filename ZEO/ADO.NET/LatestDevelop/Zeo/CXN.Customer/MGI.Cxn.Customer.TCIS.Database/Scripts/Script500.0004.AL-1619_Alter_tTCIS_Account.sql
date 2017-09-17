-- ================================================================================
-- Author:		<Anisha Abraham>
-- Create date: <11/05/2015>
-- Description:	<As Alloy, I need a flag to differentiate between SSN and ITIN>
-- Jira ID:		<AL-1619>
-- ================================================================================
IF NOT EXISTS
(
	  SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
WHERE 
	  TABLE_NAME = 'tTCIS_Account' AND  COLUMN_NAME = 'IDCode')
BEGIN
	ALTER TABLE [dbo].[tTCIS_Account]
	ADD IDCode varchar(2)
END 
GO

BEGIN
UPDATE [dbo].[tTCIS_Account]
set IDCode = (
			CASE WHEN LEFT(SSN,1) = 9  AND LEN(SSN) = 9 AND (SUBSTRING(SSN,4,2) BETWEEN 70 AND 88 
			OR
			SUBSTRING(SSN,4,2) BETWEEN 90 AND 92 
			OR
			SUBSTRING(SSN,4,2) BETWEEN 94 AND 99) THEN 'I'
			WHEN LEN(SSN) = 0 THEN ''
			ELSE 'S'
			END
			) 
	WHERE  LEFT(SSN,1) != '*' 
END
GO