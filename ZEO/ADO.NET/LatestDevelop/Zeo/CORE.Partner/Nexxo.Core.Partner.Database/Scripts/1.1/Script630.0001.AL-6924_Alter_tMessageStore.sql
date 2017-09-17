--- ================================================================================
-- Author:		<Ashok Kumar G>
-- Create date: <05/17/2016>
-- Description:	<As engineer, I want to store product code, provider and errorcode in separate columns in tMessageStore>
-- Jira ID:		<AL-6924>
-- ================================================================================
IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'Type' AND Object_ID = Object_ID(N'tMessageStore'))
	BEGIN 
		ALTER TABLE tMessageStore
		ADD Type INT NOT NULL DEFAULT(2);
	END
GO
IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'ProductKey' AND Object_ID = Object_ID(N'tMessageStore'))
	BEGIN 
		ALTER TABLE tMessageStore
		ADD ProductKey VARCHAR(4) NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'ProviderKey' AND Object_ID = Object_ID(N'tMessageStore'))
	BEGIN 
		ALTER TABLE tMessageStore
		ADD ProviderKey VARCHAR(3) NULL
	END
GO

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'ErrorKey' AND Object_ID = Object_ID(N'tMessageStore'))
	BEGIN 
		ALTER TABLE tMessageStore
		ADD ErrorKey VARCHAR(10) NULL
	END
GO
