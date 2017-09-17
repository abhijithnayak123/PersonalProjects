IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tGPRCards_DTCreate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tGPRCards] DROP CONSTRAINT [DF_tGPRCards_DTCreate]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tGPRCards_DTLastMod]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tGPRCards] DROP CONSTRAINT [DF_tGPRCards_DTLastMod]
END

GO

/****** Object:  Table [dbo].[tGPRCards]    Script Date: 05/24/2013 20:40:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tGPRCards]') AND type in (N'U'))
DROP TABLE [dbo].[tGPRCards]
GO

/****** Object:  Table [dbo].[tGPRCards]    Script Date: 05/24/2013 20:40:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tGPRCards](
	[Id] [uniqueidentifier] NOT NULL,
	[PAN] [bigint] NULL,
	[CardNumber] [nvarchar](30) NULL,
	[NameAsOnCard] [nvarchar](50) NULL,
	[ExpiryDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[DTActivated] [datetime] NULL,
	[ActivatedBy] [int] NULL,
	[DTDeactivated] [datetime] NULL,
	[DeactivatedBy] [int] NULL,
	[DeactivatedReason] [nvarchar](100) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tGPRCards] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tGPRCards] ADD  CONSTRAINT [DF_tGPRCards_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO

ALTER TABLE [dbo].[tGPRCards] ADD  CONSTRAINT [DF_tGPRCards_DTLastMod]  DEFAULT (getdate()) FOR [DTLastMod]
GO


