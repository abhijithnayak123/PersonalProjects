--Add this Query to enable SQL Injection Solution US#1789

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Id' AND OBJECT_ID = OBJECT_ID(N'tFISConnectsDb'))
BEGIN
	ALTER TABLE dbo.tFISConnectsDb 
	ADD 
		[Id] bigint IDENTITY(1000000000,1) NOT NULL, 
		[rowguid] uniqueidentifier NOT NULL DEFAULT (newid())
END
GO