-- Author: SwarnaLakshmi S
-- Date Created: Jan 08 2015
-- Description: Adding new column IsReferral to tShoppingCart Table to know has customer referred someone
-- User Story ID: US1800 Task ID: 

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'IsReferral' and Object_ID = Object_ID(N'tShoppingCarts'))
BEGIN 
	ALTER TABLE tShoppingCarts
	ADD IsReferral BIT NOT NULL DEFAULT (0)
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'IsReferral' and Object_ID = Object_ID(N'tShoppingCarts_Aud'))
BEGIN 
	ALTER TABLE tShoppingCarts_Aud
	ADD IsReferral BIT NOT NULL DEFAULT (0)
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Status' and Object_ID = Object_ID(N'tShoppingCarts_Aud'))
BEGIN 
	ALTER TABLE tShoppingCarts_Aud
	ADD [Status] VARCHAR(50) NULL
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'IsParked' and Object_ID = Object_ID(N'tShoppingCarts_Aud'))
BEGIN 
	ALTER TABLE tShoppingCarts_Aud
	ADD IsParked BIT NOT NULL DEFAULT (0)
END
GO