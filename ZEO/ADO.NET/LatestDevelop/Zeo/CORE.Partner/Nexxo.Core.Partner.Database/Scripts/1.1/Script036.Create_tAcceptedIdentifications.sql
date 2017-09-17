
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tAcceptedIdentifications]') AND type in (N'U'))
DROP TABLE [dbo].[tAcceptedIdentifications]
GO

CREATE TABLE [dbo].[tAcceptedIdentifications](
	[Id] [int] NOT NULL,
	[CountryCode] [int] NOT NULL,
	[StateId] [uniqueidentifier] NULL,
	[Name] [nvarchar](75) NOT NULL,
	[IdType] [int] NULL,
	[WCSTypeId] [bigint] NOT NULL,
	[Mask] [nvarchar](25) NOT NULL,
	[HasExpirationDate] [bit] NOT NULL,
	[ChexarIdType] [int] NULL
) ON [PRIMARY]

GO

