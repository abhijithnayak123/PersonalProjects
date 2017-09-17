-- ============================================================
-- Author:		Manikandan Govindraj
-- Create date: <09/24/2015>
-- Description:	<Script for Altering tCustomerEmploymentDetails_Aud table>
-- Jira ID:		<AL-2018>
-- ============================================================

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'tCustomerEmploymentDetails_Aud' AND COLUMN_NAME = 'OccupationDescription')
BEGIN
	ALTER TABLE dbo.tCustomerEmploymentDetails_Aud
	ADD OccupationDescription NVARCHAR(255) NULL 
END
GO