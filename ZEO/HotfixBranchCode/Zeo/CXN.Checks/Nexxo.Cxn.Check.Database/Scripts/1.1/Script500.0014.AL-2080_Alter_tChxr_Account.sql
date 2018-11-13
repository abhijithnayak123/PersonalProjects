-- ================================================================================
-- Author:		<Sunil Shetty>
-- Create date: <10/08/2015>
-- Description:	<Alter DOB to nullable As Carver, I'd like to reduce the number of required registration fields>
-- Jira ID:		<AL-2080>
-- ================================================================================

IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DateOfBirth' 
AND Object_ID = Object_ID(N'tChxr_Account'))
BEGIN
ALTER TABLE tChxr_Account ALTER COLUMN DateOfBirth Datetime NULL
END

IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DateOfBirth' 
AND Object_ID = Object_ID(N'tChxr_Account_Aud'))
BEGIN
ALTER TABLE tChxr_Account_Aud ALTER COLUMN DateOfBirth Datetime NULL
END

