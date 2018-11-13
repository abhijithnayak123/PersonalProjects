

CREATE TABLE [dbo].[tTerminals](
    [rowguid] [uniqueidentifier] NOT NULL,
	[Id] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[LocationPK] [uniqueidentifier] NOT NULL,
    [DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
CONSTRAINT [PK_tTerminals] PRIMARY KEY CLUSTERED 
(
    [rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 
ALTER TABLE [dbo].[tTerminals]  WITH CHECK ADD CONSTRAINT [FK_tTerminals_tLocations] FOREIGN KEY([LocationPK])
REFERENCES [dbo].[tLocations] ([rowguid])
GO
 
ALTER TABLE [dbo].[tTerminals] CHECK CONSTRAINT [FK_tTerminals_tLocations]
GO

