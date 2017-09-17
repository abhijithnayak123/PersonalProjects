--===========================================================================================
-- Author:		<Abhijith>
-- Create date: <Jan 21 2015>
-- Description:	<Creating a new table for storing the processor credentials>
-- Rally ID:	<US2321>
--===========================================================================================

IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].tLocationProcessorCredentials') AND type in (N'U'))
BEGIN
	DROP TABLE [dbo].tLocationProcessorCredentials
END
GO

CREATE TABLE tLocationProcessorCredentials
(
	rowguid UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	Id BIGINT IDENTITY(1000000000,1) NOT NULL,
	LocationId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES tLocations(rowguid),
	ProviderId INT NOT NULL,
	UserName NVARCHAR(255),
	[Password] NVARCHAR(50),
	Identifier NVARCHAR(50),
	DTCreate DATETIME NOT NULL,
	DTLastMod DATETIME NULL	
)