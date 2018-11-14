--- ===============================================================================
-- Author:		<Manikandan Govindraj>
-- Create date: <23-10-2017>
-- Description: Add new column as 'DisplayMessage' in tMessageStore table.
-- ================================================================================

IF NOT EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tMessageStore' AND COLUMN_NAME = 'DisplayMessage')
BEGIN
    ALTER TABLE tMessageStore
	ADD DisplayMessage NVARCHAR(4000)
END
GO

