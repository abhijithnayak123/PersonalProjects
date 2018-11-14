-- ============================================================
-- Author:        <Chinar Kulkarni>
-- Create date:   <06/25/2015>
-- Description:   <Added New datetime columns to the tables tCustomer, tTransaction and tLocation in ODS> 
-- Rally ID:      <AL-603>
-- ============================================================

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTServerCreate' AND Object_ID = Object_ID(N'tTransaction'))
	BEGIN 
		ALTER TABLE tTransaction
		ADD DTServerCreate DATETIME NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTServerLastModified' AND Object_ID = Object_ID(N'tTransaction'))
	BEGIN 
		ALTER TABLE tTransaction
		ADD DTServerLastModified DATETIME NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTTerminalCreate' AND Object_ID = Object_ID(N'tTransaction'))
	BEGIN 
		ALTER TABLE tTransaction
		ADD DTTerminalCreate DATETIME NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTTerminalLastModified' AND Object_ID = Object_ID(N'tTransaction'))
	BEGIN 
		ALTER TABLE tTransaction
		ADD DTTerminalLastModified DATETIME NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTServerCreate' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD DTServerCreate DATETIME NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTServerLastModified' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD DTServerLastModified DATETIME NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTTerminalCreate' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD DTTerminalCreate DATETIME NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTTerminalLastModified' AND Object_ID = Object_ID(N'tCustomer'))
	BEGIN 
		ALTER TABLE tCustomer
		ADD DTTerminalLastModified DATETIME NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTServerCreate' AND Object_ID = Object_ID(N'tLocation'))
	BEGIN 
		ALTER TABLE tLocation
		ADD DTServerCreate DATETIME NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTServerLastModified' AND Object_ID = Object_ID(N'tLocation'))
	BEGIN 
		ALTER TABLE tLocation
		ADD DTServerLastModified DATETIME NULL
	END
GO

--IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTTerminalCreate' AND Object_ID = Object_ID(N'tLocation'))
--	BEGIN 
--		ALTER TABLE tLocation
--		ADD DTTerminalCreate DATETIME NULL
--	END
--GO

--IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'DTTerminalLastModified' AND Object_ID = Object_ID(N'tLocation'))
--	BEGIN 
--		ALTER TABLE tLocation
--		ADD DTTerminalLastModified DATETIME NULL
--	END
--GO

