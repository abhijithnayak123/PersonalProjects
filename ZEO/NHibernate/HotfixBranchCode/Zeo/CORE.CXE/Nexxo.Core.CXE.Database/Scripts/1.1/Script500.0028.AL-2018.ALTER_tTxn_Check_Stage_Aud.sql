-- ============================================================
-- Author:		Manikandan Govindraj
-- Create date: <09/24/2015>
-- Description:	<Script for Altering tTxn_Check_Stage_Aud table>
-- Jira ID:		<AL-2018>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tTxn_Check_Stage_Aud' AND COLUMN_NAME = 'DTServerCreate')
BEGIN
	ALTER TABLE dbo.tTxn_Check_Stage_Aud
	ADD DTServerCreate DATETIME NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tTxn_Check_Stage_Aud' AND COLUMN_NAME = 'DTServerLastModified')
BEGIN
	ALTER TABLE dbo.tTxn_Check_Stage_Aud
	ADD DTServerLastModified DATETIME NULL 
END
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tTxn_Check_Stage_Aud' AND COLUMN_NAME = 'RevisionNo')
BEGIN
	ALTER TABLE dbo.tTxn_Check_Stage_Aud
	ADD RevisionNo BIGINT NULL 
END
GO