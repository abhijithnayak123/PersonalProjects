
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tStates_tCountries]') AND parent_object_id = OBJECT_ID(N'[dbo].[tStates]'))
ALTER TABLE [dbo].[tStates] DROP CONSTRAINT [FK_tStates_tCountries]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tStates_id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tStates] DROP CONSTRAINT [DF_tStates_id]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tStates_DTCreate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tStates] DROP CONSTRAINT [DF_tStates_DTCreate]
END

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tStates]') AND type in (N'U'))
DROP TABLE [dbo].[tStates]
GO

CREATE TABLE [dbo].[tStates](
	[id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Abbr] [nvarchar](10) NULL,
	[CountryCode] [int] NOT NULL,
	[HASC1] [nvarchar](10) NULL,
	[Region] [nvarchar](50) NULL,
	[Label] [nvarchar](50) NULL,
	[FIPS] [nvarchar](10) NULL,
	[DTCreate] [datetime] NULL,
	[DTLastMod] [datetime] NULL,
	[XLicenseId] [int] NULL,
	[TimeZone] [nvarchar](50) NULL,
 CONSTRAINT [PK_tStates] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tStates]  WITH CHECK ADD  CONSTRAINT [FK_tStates_tCountries] FOREIGN KEY([CountryCode])
REFERENCES [dbo].[tCountries] ([Code])
GO

ALTER TABLE [dbo].[tStates] CHECK CONSTRAINT [FK_tStates_tCountries]
GO

ALTER TABLE [dbo].[tStates] ADD  CONSTRAINT [DF_tStates_id]  DEFAULT (newid()) FOR [id]
GO

ALTER TABLE [dbo].[tStates] ADD  CONSTRAINT [DF_tStates_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO

