-- ============================================================
-- Author:		<Bineesh Raghavan>
-- Create date: <11/11/2014>
-- Description:	<DDL script to create tVisa_Trx table>
-- Rally ID:	<US2154>
-- ============================================================

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_tVisa_Trx_tVisa_Account]') AND parent_object_id = OBJECT_ID(N'[dbo].[tVisa_Trx]'))
ALTER TABLE [dbo].[tVisa_Trx] DROP CONSTRAINT [FK_tVisa_Trx_tVisa_Account]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tVisa_Trx]') AND type in (N'U'))
DROP TABLE [dbo].[tVisa_Trx]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tVisa_Trx](
	[rowguid] [uniqueidentifier] NOT NULL,
	[Id] [bigint] IDENTITY(1000000000,1) NOT NULL,
	[AccountPK] [uniqueidentifier] NOT NULL,
	[TransactionType] [int] NOT NULL,
	[Amount] [money] NOT NULL,
	[Fee] [money] NULL,
	[Description] [nvarchar](200) NULL,
	[Status] [int] NOT NULL,
	[ConfirmationId] [nvarchar](50) NULL,
	[ChannelPartnerID] [bigint] NOT NULL,
	[Balance] [money] NOT NULL,	
	[ErrorCode] [nvarchar](50) NULL,
	[ErrorMsg] [nvarchar](100) NULL,
	[DTTransmission] [datetime] NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastMod] [datetime] NULL,
	[DTServerCreate] [datetime] NULL,
	[DTServerLastMod] [datetime] NULL,
 CONSTRAINT [PK_tVisa_Trx] PRIMARY KEY CLUSTERED 
(
	[rowguid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tVisa_Trx]  WITH CHECK ADD  CONSTRAINT [FK_tVisa_Trx_tVisa_Account] FOREIGN KEY([AccountPK])
REFERENCES [dbo].[tVisa_Account] ([rowguid])
GO

ALTER TABLE [dbo].[tVisa_Trx] CHECK CONSTRAINT [FK_tVisa_Trx_tVisa_Account]
GO
