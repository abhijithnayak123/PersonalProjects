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
	  TABLE_NAME = 'tChxr_Account' AND  COLUMN_NAME = 'IDCode')
BEGIN
	ALTER TABLE [dbo].[tChxr_Account]
	ADD IDCode varchar(2)
END 
GO

IF NOT EXISTS
(
	  SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
WHERE 
	  TABLE_NAME = 'tChxrSim_Account' AND  COLUMN_NAME = 'IDCode')
BEGIN
	ALTER TABLE [dbo].[tChxrSim_Account]
	ADD IDCode varchar(2)
END 
GO

IF NOT EXISTS
(
	  SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
WHERE 
	  TABLE_NAME = 'tChxr_Account_Aud' AND  COLUMN_NAME = 'IDCode')
BEGIN
	ALTER TABLE [dbo].[tChxr_Account_Aud]
	ADD IDCode varchar(2)
END 
GO

