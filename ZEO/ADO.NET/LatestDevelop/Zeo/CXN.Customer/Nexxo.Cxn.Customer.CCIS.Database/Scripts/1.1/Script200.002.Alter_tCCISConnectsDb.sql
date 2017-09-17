--===========================================================================================
-- Author:		Saleem 
-- Create date: <07/05/2014>
-- Description:	<Script for Adding Gender and DriversLicenseNumber  columns to tCCISConnectsDb Table >
-- Rally ID:	<US1983>
--===========================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tCCISConnectsDb]') AND type in (N'U'))
BEGIN

    IF EXISTS(SELECT * FROM sys.columns
    WHERE Name = N'IdNumber' AND OBJECT_ID = OBJECT_ID(N'tCCISConnectsDb'))
	BEGIN
		EXEC sp_rename '[tCCISConnectsDb].[IdNumber]', 'DriversLicenseNumber', 'COLUMN'
	END
 
	IF EXISTS( SELECT * FROM sys.columns
	WHERE Name = N'SecondaryPhoneNumber' AND OBJECT_ID = OBJECT_ID(N'tCCISConnectsDb'))
	BEGIN
		EXEC sp_rename '[tCCISConnectsDb].[SecondaryPhoneNumber]', 'SecondaryPhone', 'COLUMN'
	END
      
	ALTER TABLE dbo.[tCCISConnectsDb] 
	ADD [Gender] [varchar](6) NULL
END
GO



