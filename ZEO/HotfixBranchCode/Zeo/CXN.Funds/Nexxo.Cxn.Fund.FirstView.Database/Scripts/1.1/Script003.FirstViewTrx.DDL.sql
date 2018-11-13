/****** Object:  Table [dbo].[tFView_Trx]    Script Date: 05/14/2013 17:32:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tFView_Trx_DTCreate]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tFView_Trx] DROP CONSTRAINT [DF_tFView_Trx_DTCreate]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_tFView_Trx_DTLastMod]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tFView_Trx] DROP CONSTRAINT [DF_tFView_Trx_DTLastMod]
END
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[PK_tFView_Trx]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[tFView_Trx] DROP CONSTRAINT [PK_tFView_Trx]
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tFView_Trx]') AND type in (N'U'))
DROP TABLE [dbo].[tFView_Trx]
GO

CREATE TABLE [dbo].[tFView_Trx](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[ProcessorId] [int] NULL,
	[PrimaryAccountNumber] [bigint] NULL,
	[TransactionType] [varchar](20) NULL,
	[TransactionAmount] [money] NOT NULL,
	[CardAcceptorIdCode] [varchar](20) NULL,
	[CardAcceptorTerminalID] [uniqueidentifier] NULL,
	[CardAcceptorBusinessCode] [int] NULL,
	[TransactionDescription] [varchar](50) NULL,
	[MessageTypeIdentifier] [varchar](10) NULL,
	[TransactionCurrencyCode] [varchar](10) NULL,
	[DTLocalTransaction] [datetime] NULL,
	[DTTransmission] [datetime] NULL,
	[CreditPlanMaster] [int] NULL,
	[AccountNumber] [varchar](20) NULL,
	[TransactionID] [varchar](20) NULL,
	[CardBalance] [money] NOT NULL,
	[ErrorCode] [varchar](10) NULL,
	[ErrorMsg] [varchar](200) NULL,
	[CardStatus] [varchar](10) NULL,
	[ActivationRequired] [varchar](5) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tFView_Trx] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tFView_Trx] ADD  CONSTRAINT [DF_tFView_Trx_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO

ALTER TABLE [dbo].[tFView_Trx] ADD  CONSTRAINT [DF_tFView_Trx_DTLastMod]  DEFAULT (getdate()) FOR [DTLastMod]
GO

ALTER TABLE [dbo].[tFView_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tFView_Trx_tFView_Card] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tFView_Card] ([rowguid])
GO