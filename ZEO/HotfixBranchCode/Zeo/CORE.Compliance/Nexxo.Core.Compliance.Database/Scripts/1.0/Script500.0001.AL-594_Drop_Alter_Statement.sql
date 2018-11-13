-- ================================================================
-- Author:		Kaushik Sakala
-- Create date: <24/06/2015>
-- Description:	<As a product owner, I should have the ability to 
--				configure multiple limits for each client.>
-- JIRA ID:	<AL-594>
-- =================================================================

--Renaming the Columns for CompliancePrograms
IF EXISTS(SELECT 1 FROM sys.columns WHERE NAME =N'DTCreate' and Object_ID = Object_ID(N'tCompliancePrograms'))
	BEGIN
		EXEC sp_rename 'tCompliancePrograms.DTCreate', 'DTServerCreate', 'COLUMN'		
	END
GO

IF EXISTS(SELECT 1 FROM sys.columns WHERE NAME =N'DTLastMod' and Object_ID = Object_ID(N'tCompliancePrograms'))
	BEGIN		
		EXEC sp_rename 'tCompliancePrograms.DTLastMod', 'DTServerLastModified', 'COLUMN'
	END
GO

--Renaming the Columns for LimitFailures
IF EXISTS(SELECT 1 FROM sys.columns WHERE NAME =N'DTCreate' and Object_ID = Object_ID(N'tLimitFailures'))
	BEGIN
		EXEC sp_rename 'tLimitFailures.DTCreate', 'DTServerCreate', 'COLUMN'		
	END
GO

IF EXISTS(SELECT 1 FROM sys.columns WHERE NAME =N'DTLastMod' and Object_ID = Object_ID(N'tLimitFailures'))
	BEGIN		
		EXEC sp_rename 'tLimitFailures.DTLastMod', 'DTServerLastModified', 'COLUMN'
	END
GO

-- Remove foriegn key references for tTransactionMinimums
WHILE EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' and TABLE_NAME= 'tTransactionMinimums')
	BEGIN
		DECLARE @Constarint NVARCHAR(2000)
		SELECT TOP 1 @Constarint =('ALTER TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME
		+ '] DROP CONSTRAINT [' + CONSTRAINT_NAME + ']')
		FROM information_schema.table_constraints
		WHERE CONSTRAINT_TYPE = 'FOREIGN KEY' and TABLE_NAME= 'tTransactionMinimums'
		EXEC (@Constarint)
	END

GO

--Checking if table exists
IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tTransactionMinimums'))
	BEGIN
		--Delete table 
		DROP TABLE tTransactionMinimums
	END

GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tBridgerFailures'))
	BEGIN
		--Delete table 
		DROP TABLE tBridgerFailures
	END

GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tLimitConditions'))
	BEGIN
		--Delete table 
		DROP TABLE tLimitConditions
	END

GO
--Removing the foregin key constraint on tlimits to drop the  tLimitTypes data
WHILE EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' and TABLE_NAME= 'tLimits')
	BEGIN
		DECLARE @Constarint NVARCHAR(2000)
		SELECT TOP 1 @Constarint =('ALTER TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME
		+ '] DROP CONSTRAINT [' + CONSTRAINT_NAME + ']')
		FROM information_schema.table_constraints
		WHERE CONSTRAINT_TYPE = 'FOREIGN KEY' and TABLE_NAME= 'tLimits'
		EXEC (@Constarint)
	END

GO

--Removing the foregin key constraint on tlimits to drop the tLimitTypes data
WHILE EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE='FOREIGN KEY' and TABLE_NAME= 'tLimitTypes')
	BEGIN
		DECLARE @Constarint NVARCHAR(2000)
		SELECT TOP 1 @Constarint =('ALTER TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME
		+ '] DROP CONSTRAINT [' + CONSTRAINT_NAME + ']')
		FROM information_schema.table_constraints
		WHERE CONSTRAINT_TYPE = 'FOREIGN KEY' and TABLE_NAME= 'tLimitTypes'
		EXEC (@Constarint)
	END

GO


IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tLimitTypes'))
	BEGIN
		--Delete table 
		DROP TABLE tLimitTypes
	END

GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tLimits'))
	BEGIN
		--Delete table 
		DROP TABLE tLimits
	END

GO