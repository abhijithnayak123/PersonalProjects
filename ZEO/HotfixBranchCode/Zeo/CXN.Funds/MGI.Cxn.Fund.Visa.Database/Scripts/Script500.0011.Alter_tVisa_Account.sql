-- ================================================================================
-- Author:		<Kaushik Sakala>
-- Create date: <11/20/2015>
-- Description:	<As VisaDPS, I need to truncate profile element values sent from Alloy>
-- Jira ID:		<AL-2999>
-- ================================================================================
IF NOT EXISTS
(
	  SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
WHERE 
	  TABLE_NAME = 'tVisa_Account' AND  COLUMN_NAME = 'Email'
)
BEGIN
ALTER TABLE tVISA_Account
ADD Email VARCHAR(200)
END
GO