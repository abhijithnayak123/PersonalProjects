
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx] DROP CONSTRAINT [FK_tWUnion_Trx_tWUnion_Account]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_Recipient_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx] DROP CONSTRAINT [FK_tWUnion_Trx_tWUnion_Recipient_Account]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx] DROP CONSTRAINT [FK_tWUnion_Trx_tWUnion_Account]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_Recipient_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx] DROP CONSTRAINT [FK_tWUnion_Trx_tWUnion_Recipient_Account]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_Trx]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Account]') AND type in (N'U'))
DROP TABLE [dbo].[tWUnion_Account]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Account]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_Account](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_Account] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[tWUnion_Trx](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[WUnionAccountPK] [uniqueidentifier] NOT NULL,
	[WUnionRecipientAccountPK] [uniqueidentifier] NOT NULL,
	[OriginatorsPrincipalAmount] [bigint] NULL,
	[OriginatingCountryCode] [varchar](20) NULL,
	[OriginatingCurrencyCode] [varchar](20) NULL,
	[TranascationType] [varchar](20) NULL,
	[PromotionsCode] [varchar](50) NULL,
	[ExchangeRate] [float] NULL,
	[DestinationPrincipalAmount] [bigint] NULL,
	[GrossTotalAmount] [bigint] NULL,
	[Charges] [bigint] NULL,
	[TaxAmount] [bigint] NULL,
	[Mtcn] [varchar](50) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
 CONSTRAINT [PK_tWUnion_Trx] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_Trx_tWUnion_Account] FOREIGN KEY([WUnionAccountPK])
REFERENCES [dbo].[tWUnion_Account] ([rowguid])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx] CHECK CONSTRAINT [FK_tWUnion_Trx_tWUnion_Account]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_Recipient_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tWUnion_Trx_tWUnion_Recipient_Account] FOREIGN KEY([WUnionRecipientAccountPK])
REFERENCES [dbo].[tWUnion_Recipient_Account] ([rowguid])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tWUnion_Trx_tWUnion_Recipient_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tWUnion_Trx]'))
ALTER TABLE [dbo].[tWUnion_Trx] CHECK CONSTRAINT [FK_tWUnion_Trx_tWUnion_Recipient_Account]
GO
